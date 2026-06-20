using Mcc;
using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R
{
    public partial class OutRobot : CProcessAbstract, IUnitNode
    {
        public OutRobotPicker[] Pickers { get; private set; }
        public OutRobotTurnCylinder[] TurnCylinders { get; private set; }
        public OutRobotNachi Nachi { get; private set; }
        public IReadOnlyList<CellDataHandler> CellContainer { get; private set; }
        public Utils.SyncToken SyncTact { get; private set; } = new Utils.SyncToken();
        public IReadOnlyDictionary<CDefine.ECycleTactOutRobot, CTimeElement> CycleTactTime => mCycleTactTime;
        public bool IsIdle => (
            Pickers.All(item => item.GetCommand() == 0) // == Idle
            && TurnCylinders.All(item => item.GetCommand() == 0)
            && Nachi.GetCommand() == 0
            && IsRunningSequence == false
            );
        public bool IsEmpty => CellContainer.GetExistCellCount() == 0;
        private Thread mThreadBatch;
        private ManualResetEvent mOneCycleUnlock;
        private ManualResetEvent mOneCycleEnd;
        private Dictionary<CDefine.ECycleTactOutRobot, CTimeElement> mCycleTactTime;
        private string[] mLastInnerID;
        private string[] mLastCellID;
        private Stopwatch mUserStopTimeStopwatch = Stopwatch.StartNew();
        private static readonly string REGISTRY_PATH = Path.Combine("ENC", Program.ID, "SEQUENCE", nameof(OutRobot));

        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;

            do
            {
                // 도큐먼트 객체
                m_objDocument = document;
                m_objConfig = m_objDocument.m_objConfig;
                mDefine = new Define(position);
                // IO 객체 연결
                m_objIO = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objIO;
                CellContainer = CellDataManager.ProcessCells[mDefine.ProcessIndex];
                foreach (var cellDataHandler in CellContainer)
                {
                    cellDataHandler.OwnerManager = this;
                }

                mCycleTactTime = m_objDocument.m_objCycleTactTime.m_tactOutRobot.dicTactTime;
                mLastInnerID = new string[CellContainer.Count];
                mLastCellID = new string[CellContainer.Count];

                // 피커 객체 생성
                Pickers = new OutRobotPicker[mDefine.PickerPositions.Length];
                for (int i = 0; i < mDefine.PickerPositions.Length; ++i)
                {
                    Pickers[i] = new OutRobotPicker();
                    if (Pickers[i].Initialize(m_objDocument, mDefine.PickerPositions[i]) == false)
                    {
                        break;
                    }
                }

                // 실린더 객체 생성
                TurnCylinders = new OutRobotTurnCylinder[mDefine.TurnCylinderPositions.Length];
                for (int i = 0; i < Pickers.Length; i++)
                {
                    TurnCylinders[i] = new OutRobotTurnCylinder();
                    if (TurnCylinders[i].Initialize(m_objDocument, mDefine.TurnCylinderPositions[i]) == false)
                    {
                        break;
                    }
                }

                // 나치 객체 생성
                Nachi = new OutRobotNachi();
                if (Nachi.Initialize(m_objDocument, mDefine.NachiPosition) == false)
                {
                    break;
                }

                mOneCycleUnlock = new ManualResetEvent(true);
                mOneCycleEnd = new ManualResetEvent(true);

                mThreadProcess = new Thread(ThreadProcess);
                mThreadProcess.Start();

                mThreadInitialize = new Thread(ThreadInitializeProcess);
                mThreadInitialize.Start();

                mThreadBatch = new Thread(ThreadManualProcess);
                mThreadBatch.Start();

                mThreadRestart = new Thread(ThreadRestartProcess);
                mThreadRestart.Start();

                mThreadPendantRemoteControl = new Thread(ThreadPendantRemoteControlProcess);
                mThreadPendantRemoteControl.Start();

                mbThreadExit = false;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override void DeInitialize()
        {
            mbThreadExit = true;
            mThreadPendantRemoteControl.Join();
            mThreadInitialize.Join();
            mThreadBatch.Join();
            mThreadRestart.Join();
            mThreadProcess.Join();

            mOneCycleUnlock.Dispose();
            mOneCycleEnd.Dispose();

            Nachi.DeInitialize();
            TurnCylinders.ForEach(i => i.DeInitialize());
            Pickers.ForEach(i => i.DeInitialize());
        }

        public void LockOneCycle()
        {
            mOneCycleUnlock.Reset();
            mOneCycleEnd.WaitOne();
        }

        public void UnlockOneCycle()
        {
            mOneCycleUnlock.Set();
        }

        private bool DoProcessLoadGetWait()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.CheckOverlap())).ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // 동작전 Cylinder Return            
            using (TurnCylinders.All(cylinder => cylinder.IsInposition(mDefine.LoadPositionCylinder)) ? null
                : MccLogManager.OutRobot.CYL_T_RETURN.CreateMccLogProvider()
                )
            {
                TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.LoadPositionCylinder));
                if (TurnCylinders.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            Nachi.SetMccLogItem(MccLogManager.OutRobot.MOVE_LD_WAIT_POS);
            Nachi.SetCommand(OutRobotNachi.ECommand.LoadProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            MccLogManager.OutRobot.IN_WAIT_TIME.BatchWriteStart();
            return true;
        }

        private bool DoProcessLoadGet()
        {
            var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.CheckOverlap()))
                .Concat(inspStage.Pickers.Select(i => Task.Run(() => i.Check())))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // 작업 순서 생성
            var cellPlacement = CellDataManager.GetCellPlacementFromProcess(EProcess.InspStage);
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, EMethod.Get, EProcess.InspStage, ECellPlacement.Empty, cellPlacement) == false)
            {
                // ! 나오면 안되는 상황
                Debug.Assert(false);
                return false;
            }

            MccLogProvider[] mccComponentInOutItems = null;
            try
            {
                inspStage.SyncTact.Lock();
                inspStage.LockOneCycle();
                MccLogManager.OutRobot.IN_WAIT_TIME.BatchWriteEnd();
                MccLogManager.InspStage.OUT_WAIT_TIME.BatchWriteEndExist();
                for (int index = 0; index < workOrders.Count; ++index)
                {
                    var workOrder = workOrders[index];
                    // + MCC: Write COMPONENT_IN/OUT Start
                    mccComponentInOutItems = workOrder.RobotPickers
                        .SelectMany(picker =>
                        {
                            var list = new List<MccLogProvider>(3)
                            {
                                MccLogManager.OutRobot.COMPONENT_IN[picker.CellPort.PositionIndex].CreateMccLogProvider(),
                                MccLogManager.InspStage.COMPONENT_OUT[picker.CellPort.PositionIndex].CreateMccLogProvider()
                            };
                            if (workOrders.Count > 1)
                            {
                                list.Add(MccLogManager.OutRobot.TR_ACT_WAIT_TIME_COMPONENT_IN[picker.CellPort.PositionIndex].CreateMccLogProvider());
                                list.Add(MccLogManager.InspStage.TR_ACT_WAIT_TIME_COMPONENT_OUT[picker.CellPort.PositionIndex].CreateMccLogProvider());
                            }
                            return list;
                        })
                        .ToArray();
                    SubSequenceSetPreOrder(workOrders, index);
                    try
                    {
                        if (SubSequenceLoad(workOrder.MethodIndex, workOrder.ToolIndex, workOrder.StageIndex) == false)
                        {
                            return false;
                        }
                        if (SubSequenceCycleEnd() == false)
                        {
                            return false;
                        }
                        // + MCC: Write COMPONENT_IN/OUT End
                        Array.ForEach(mccComponentInOutItems, item => item.Dispose());
                        mccComponentInOutItems = null;
                    }
                    finally
                    {
                        int[] positionIndexes = workOrder.RobotPickers.Select(i => i.CellPort.PositionIndex).ToArray();
                        // ! 하나라도 파기를 진행한 경우 셀 데이터를 이동함
                        if (positionIndexes.Any(i => inspStage.Pickers[i].Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_OFF) == true)
                        {
                            workOrder.CellDataMove();
                        }
                    }
                }
            }
            finally
            {
                inspStage.CycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOADING_END_TIME].SetTime();
                inspStage.CycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOAD_START_TIME].SetTime();
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_END_TIME].SetTime();
                inspStage.UnlockOneCycle();
                inspStage.SyncTact.Unlock();
                // + MCC: Abnormal case. Write COMPONENT_IN/OUT End
                if (mccComponentInOutItems != null) Array.ForEach(mccComponentInOutItems, item => item.Dispose());
            }

            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_EXIT_START_TIME].SetTime();
            Nachi.SetMccLogItem(MccLogManager.OutRobot.MOVE_LD_SAFETY_POS);
            Nachi.SetCommand(OutRobotNachi.ECommand.LoadProcessExit);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }

            return true;
        }


        private bool DoProcessUnloadPutWait()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check())).ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // 동작전 Cylinder Return
            TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.UnloadPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetMccLogItem(MccLogManager.OutRobot.MOVE_ULD_WAIT_POS);
            Nachi.SetCommand(OutRobotNachi.ECommand.UnloadProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private bool DoProcessWaitInspResult()
        {
            bool bError = false;
            using (MccLogManager.OutRobot.INSP_RESULT_WAIT_TIME.CreateMccLogProvider(bShouldWriteExistCell: true))
            {
                Parallel.ForEach(CellContainer.GetExistCellList(), cellDataHandler =>
                {
                    var dataCapture = cellDataHandler.Data;
                    if (m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage.Inspects.First(x => x.IsPrimary()).SetWaitInspectResult(ref dataCapture) == false)
                    {
                        bError = true;
                        return;
                    }
                    m_objDocument.m_objProcessMain.m_objProcessMotion.TrackOut.CheckInspectResultCombination(cellDataHandler.Data);
                    // 검사결과에따른 Out Robot 실린더 상태만 업데이트
                    TurnCylinders.ForEach(cylinder =>
                    {
                        if (cylinder.CellPort.Data.Cell.MachineResult == CDefine.DEFECT_MACHINE_RESULT_OK)
                        {
                            cylinder.CellPort.Data.Cell.CylinderStatus = m_objDocument.m_objConfig.GetOptionParameter().bUseOutRobotCylinderTurnOk == true ? ECylinderStatus.Turn : ECylinderStatus.Return;
                        }
                        else
                        {
                            cylinder.CellPort.Data.Cell.CylinderStatus = m_objDocument.m_objConfig.GetOptionParameter().bUseOutRobotCylinderTurnNg == true ? ECylinderStatus.Turn : ECylinderStatus.Return;
                        }
                    });
                    cellDataHandler.IsChanged = true;
                    m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetUpdateHistoryProcessData(cellDataHandler.Data.DeepClone());
                });
            }
            if (bError == true)
            {
                return false;
            }
            return true;
        }

        private bool DoProcessUnloadPut()
        {
            var outFlip = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip;
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .Concat(outFlip.Pickers.Select(i => Task.Run(() => i.CheckOverlap())))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // OutRobot 실린더 상태에 따른 실린더 회전            
            using (TurnCylinders.All(cylinder => cylinder.IsInposition(cylinder.CellPort.Data.Cell.CylinderStatus == ECylinderStatus.Return ? OutRobotTurnCylinder.ECommand.Return : OutRobotTurnCylinder.ECommand.Turn)) ? null
                : MccLogManager.OutRobot.CYL_T_TURN.CreateMccLogProvider()
                )
            {
                TurnCylinders.ForEach(cylinder => cylinder.SetCommand(cylinder.CellPort.Data.Cell.CylinderStatus == ECylinderStatus.Return ? OutRobotTurnCylinder.ECommand.Return : OutRobotTurnCylinder.ECommand.Turn));
                if (TurnCylinders.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            // Update TactTime Log Information
            mLastInnerID = CellContainer.Select(i => i.GetInnerID()).ToArray();
            mLastCellID = CellContainer.Select(i => i.GetCellID()).ToArray();

            // 작업 순서 생성
            var cellPlacement = CellDataManager.GetCellPlacementFromProcess(EProcess.OutRobot);
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, EMethod.Put, EProcess.OutFlip, cellPlacement, ECellPlacement.Empty) == false)
            {
                // ! 나오면 안되는 상황
                Debug.Assert(false);
                return false;
            }

            MccLogProvider[] mccComponentInOutItems = null;
            try
            {
                outFlip.SyncTact.Lock();
                outFlip.LockOneCycle();
                MccLogManager.OutRobot.OUT_WAIT_TIME.BatchWriteEndExist();
                MccLogManager.OutFlip.IN_WAIT_TIME.BatchWriteEnd();
                for (int index = 0; index < workOrders.Count; ++index)
                {
                    var workOrder = workOrders[index];
                    // + MCC: Write COMPONENT_IN/OUT Start
                    mccComponentInOutItems = workOrder.RobotPickers
                        .SelectMany(picker =>
                        {
                            var list = new List<MccLogProvider>(3)
                            {
                                MccLogManager.OutRobot.COMPONENT_OUT[picker.CellPort.PositionIndex].CreateMccLogProvider(),
                                MccLogManager.OutFlip.COMPONENT_IN[picker.CellPort.PositionIndex].CreateMccLogProvider(),
                            };
                            if (workOrders.Count > 1)
                            {
                                list.Add(MccLogManager.OutRobot.TR_ACT_WAIT_TIME_COMPONENT_OUT[picker.CellPort.PositionIndex].CreateMccLogProvider());
                                list.Add(MccLogManager.OutFlip.TR_ACT_WAIT_TIME_COMPONENT_IN[picker.CellPort.PositionIndex].CreateMccLogProvider());
                            }
                            return list;
                        })
                        .ToArray();
                    SubSequenceSetPreOrder(workOrders, index);
                    try
                    {
                        var cylinderIndex = workOrder.RobotPickers[0].CellPort.Data.Cell.CylinderStatus == ECylinderStatus.Return ? ERobotCylinder.Return : ERobotCylinder.Turn;
                        if (SubSequenceUnload(workOrder.MethodIndex, workOrder.ToolIndex, workOrder.StageIndex, cylinderIndex) == false)
                        {
                            return false;
                        }
                        SubPreOrderToProcessExit(OutRobotNachi.ECommand.UnloadProcessExit, workOrders, index);
                        if (SubSequenceCycleEnd() == false)
                        {
                            return false;
                        }
                        // + MCC: Write COMPONENT_IN/OUT End
                        Array.ForEach(mccComponentInOutItems, item => item.Dispose());
                        mccComponentInOutItems = null;
                    }
                    finally
                    {
                        // ! 하나라도 파기를 진행한 경우 셀 데이터를 이동함
                        if (workOrder.RobotPickers.Any(i => i.Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_OFF) == true)
                        {
                            workOrder.CellDataMove();
                        }
                    }
                }
            }
            finally
            {
                outFlip.CycleTactTime[CDefine.ECycleTactOutFlip.MOVE_LOADING_END_TIME].SetTime();
                outFlip.CycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOAD_READY_START_TIME].SetTime();
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_END_TIME].SetTime();
                outFlip.UnlockOneCycle();
                outFlip.SyncTact.Unlock();
                // + MCC: Abnormal case. Write COMPONENT_IN/OUT End
                if (mccComponentInOutItems != null) Array.ForEach(mccComponentInOutItems, item => item.Dispose());
            }

            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_EXIT_START_TIME].SetTime();
            Nachi.SetMccLogItem(MccLogManager.OutRobot.MOVE_ULD_SAFETY_POS);
            Nachi.SetCommand(OutRobotNachi.ECommand.UnloadProcessExit);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }

            return true;
        }

        private bool DoProcessInitialize()
        {
            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)OutRobotNachi.ECommand.Initialize) == false)
            {
                return false;
            }
            Nachi.SetMccLogItem(MccLogManager.OutRobot.INIT_UNIT);
            Nachi.SetCommand(OutRobotNachi.ECommand.Initialize);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private bool DoProcessRestart()
        {
            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)OutRobotNachi.ECommand.Initialize) == false)
            {
                return false;
            }
            Nachi.SetMccLogItem(MccLogManager.OutRobot.INIT_UNIT);
            Nachi.SetCommand(OutRobotNachi.ECommand.Initialize);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private void ThreadProcess()
        {
            int sequenceIndex = 0;
            EAutoSequence[] sequenceItems = Enum.GetValues(typeof(EAutoSequence)).Cast<EAutoSequence>().ToArray();

            while (mbThreadExit == false)
            {
                // END ONE CYCLE
                mOneCycleEnd.Set();

                // BEGIN ONE CYCLE
                mOneCycleUnlock.WaitOne();
                mOneCycleEnd.Reset();

                Thread.Sleep(WAIT_FOR_END_PROCESS_PERIOD_TIME);

                // 설비 상태 확인
                if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Start
                    && m_objDocument.GetRunStatus() != CDefine.ERunStatus.LoadingStop
                    )
                {
                    if (mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_START_TIME].IsEmpty() == false
                        && mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_END_TIME].IsEmpty() == true
                        )
                    {
                    }
                    else
                    {
                        mUserStopTimeStopwatch.Start();
                    }
                    sequenceIndex = 0;
                    continue;
                }

                mUserStopTimeStopwatch.Stop();

                EAutoSequence sequencePointer = sequenceItems[sequenceIndex];

                // 다음 시퀀스 인덱스 선택
                sequenceIndex = (sequenceItems.Length != sequenceIndex + 1) ? sequenceIndex + 1 : 0;

                // 시퀀스 조건 확인
                if (IsRobot(sequencePointer) == false)
                {
                    continue;
                }

                // 시퀀스 동작
                var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
                var outFlip = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip;
                IsRunningSequence = true;
                IsCompleteWriteCycleLog = false;
                SetProcessLog($"AutoSequence(Begin) : {sequencePointer}");
                switch (sequencePointer)
                {
                    case EAutoSequence.LoadGetWait:
                        mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOAD_APPROACH_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOAD_APPROACH_START_TIME].SetTime();
                        if (DoProcessLoadGetWait() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOAD_APPROACH_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.CYCLE_END_TIME].SetTime();
                            SetWriteCycleTactTimeLog();
                            IsCompleteWriteCycleLog = true;
                            mCycleTactTime[CDefine.ECycleTactOutRobot.CYCLE_START_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOADING_START_TIME].SetTime();

                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOAD_APPROACH_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOAD_APPROACH_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.LoadGet:
                        inspStage.SyncTact.WaitingUnlock();
                        inspStage.SyncTact.Lock();
                        inspStage.CycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_END_TIME].SetTime();
                        inspStage.CycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOADING_START_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_START_TIME].SetTime();
                        inspStage.SyncTact.Unlock();
                        if (DoProcessLoadGet() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_EXIT_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOAD_APPROACH_START_TIME].SetTime();
                        }
                        else
                        {
                            inspStage.CycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_END_TIME].Clear();
                            inspStage.CycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOADING_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_START_TIME].Clear();
                            inspStage.CycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOADING_END_TIME].Clear();
                            inspStage.CycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOAD_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_EXIT_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        InterferenceRegion.InspectionStageArea.Exit();
                        break;

                    case EAutoSequence.UnloadPutWait:
                        mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOAD_APPROACH_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOAD_APPROACH_START_TIME].SetTime();
                        if (DoProcessUnloadPutWait() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOAD_APPROACH_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_INSP_RESULT_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOAD_APPROACH_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOAD_APPROACH_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.WaitInspResult:
                        mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_INSP_RESULT_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_INSP_RESULT_START_TIME].SetTime();
                        if (DoProcessWaitInspResult() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_INSP_RESULT_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_START_TIME].SetTime();
                            MccLogManager.OutRobot.OUT_WAIT_TIME.BatchWriteStartExist();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_INSP_RESULT_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_INSP_RESULT_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.UnloadPut:
                        outFlip.SyncTact.WaitingUnlock();
                        outFlip.SyncTact.Lock();
                        outFlip.CycleTactTime[CDefine.ECycleTactOutFlip.WAIT_LOADING_END_TIME].SetTime();
                        outFlip.CycleTactTime[CDefine.ECycleTactOutFlip.MOVE_LOADING_START_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_START_TIME].SetTime();
                        if (DoProcessUnloadPut() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_EXIT_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOAD_APPROACH_START_TIME].SetTime();
                        }
                        else
                        {
                            outFlip.CycleTactTime[CDefine.ECycleTactOutFlip.WAIT_LOADING_END_TIME].Clear();
                            outFlip.CycleTactTime[CDefine.ECycleTactOutFlip.MOVE_LOADING_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_START_TIME].Clear();
                            outFlip.CycleTactTime[CDefine.ECycleTactOutFlip.MOVE_LOADING_END_TIME].Clear();
                            outFlip.CycleTactTime[CDefine.ECycleTactOutFlip.WAIT_RETURN_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_EXIT_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
                IsRunningSequence = false;
                SetProcessLog($"AutoSequence(End) : {sequencePointer}");
            }
        }

        private void ThreadInitializeProcess()
        {
            while (mbThreadExit == false)
            {
                if (EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_START == GetInitializeStatus())
                {
                    if (DoProcessInitialize() == true)
                    {
                        SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE);
                    }
                    else
                    {
                        SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_ERROR);
                    }
                }

                Thread.Sleep(10);
            }
        }

        private void ThreadRestartProcess()
        {
            while (mbThreadExit == false)
            {
                if (EProcessManagerRestart.PROCESS_MANAGER_RESTART_START == GetRestartStatus())
                {
                    if (DoProcessRestart() == true)
                    {
                        SetRestartCommand(EProcessManagerRestart.PROCESS_MANAGER_RESTART_DONE);
                    }
                    else
                    {
                        SetRestartCommand(EProcessManagerRestart.PROCESS_MANAGER_RESTART_ERROR);
                    }
                }

                Thread.Sleep(10);
            }
        }

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                CellDataManager.GetLogTypeFromProcess(mDefine.ProcessIndex),
                string.Format("{0} -> {1}", callerMemberName, logMessage)
                );
        }

        private void SetWriteCycleTactTimeLog()
        {
            var unitTact = new TactTime.UnitTact()
            {
                UnitName = EProcess.OutRobot.ToString()
            };
            double cycleTactTime, loadingWaitTime, unloadingWaitTime, userStopTime;

            // Calculate Wait Time
            {
                DateTime startTime, endTime;
                double waitingTime, tactTime;
                loadingWaitTime = unloadingWaitTime = 0;
                unitTact.InputDelayTime = unitTact.OutputDelayTime = TimeSpan.Zero;

                // INNER ID
                mCycleTactTime[CDefine.ECycleTactOutRobot.INNER_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[0]) ? " " : mLastInnerID[0];
                mCycleTactTime[CDefine.ECycleTactOutRobot.INNER_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[1]) ? " " : mLastInnerID[1];

                // CELL ID
                mCycleTactTime[CDefine.ECycleTactOutRobot.CELL_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[0]) ? " " : mLastCellID[0];
                mCycleTactTime[CDefine.ECycleTactOutRobot.CELL_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[1]) ? " " : mLastCellID[1];

                // USER_STOP_TIME [WAIT]
                unitTact.UserStopTime = mUserStopTimeStopwatch.Elapsed;
                userStopTime = unitTact.UserStopTime.TotalSeconds;
                mUserStopTimeStopwatch.Reset();
                mCycleTactTime[CDefine.ECycleTactOutRobot.USER_STOP_TIME].m_strTime = userStopTime.ToString();

                // PURE CYCLE TACT [TACT] - 유닛 사이클 택타임에서 인터페이스 대기시간을 뺀 시간
                tactTime = m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutRobot.CYCLE_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutRobot.CYCLE_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOADING_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_END_TIME].GetTime());
                tactTime -= userStopTime;
                mCycleTactTime[CDefine.ECycleTactOutRobot.PURE_CYCLE_TACT].m_strTime = tactTime.ToString();

                // BEFORE_CYCLE_START ~ CYCLE_START [TACT] 
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.CYCLE_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.CYCLE_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.CYCLE_TACT_TIME].m_strTime = tactTime.ToString();
                cycleTactTime = tactTime;
                unitTact.UnitTactTime = TimeSpan.FromSeconds(tactTime);

                // NACHI LOADING FROM INSP-STAGE [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOADING].m_strTime = waitingTime.ToString();
                loadingWaitTime += waitingTime;
                unitTact.InputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // NACHI LOADING FROM INSP-STAGE [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING].m_strTime = tactTime.ToString();

                // NACHI LOADING EXIT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_EXIT_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_EXIT_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOADING_EXIT].m_strTime = tactTime.ToString();

                // NACHI UNLOAD TO OUT FLIP APPROACH [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOAD_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOAD_APPROACH_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOAD_APPROACH].m_strTime = waitingTime.ToString();

                // NACHI UNLOAD TO OUT FLIP APPROACH [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOAD_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOAD_APPROACH_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOAD_APPROACH].m_strTime = tactTime.ToString();

                // NACHI WAIT INSP RESULT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_INSP_RESULT_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_INSP_RESULT_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_INSP_RESULT].m_strTime = waitingTime.ToString();

                // NACHI MOVE INSP RESULT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_INSP_RESULT_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_INSP_RESULT_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_INSP_RESULT].m_strTime = tactTime.ToString();

                // NACHI UNLOADING TO OUT FLIP [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_UNLOADING].m_strTime = waitingTime.ToString();
                unloadingWaitTime += waitingTime;
                unitTact.OutputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // NACHI UNLOADING TO OUT FLIP [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING].m_strTime = tactTime.ToString();

                // NACHI UNLOADING EXIT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_EXIT_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_EXIT_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_UNLOADING_EXIT].m_strTime = tactTime.ToString();

                // NACHI LOAD FROM INSP-STAGE APPROACH [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOAD_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOAD_APPROACH_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.WAIT_LOAD_APPROACH].m_strTime = waitingTime.ToString();

                // NACHI LOAD FROM INSP-STAGE APPROACH [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOAD_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOAD_APPROACH_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutRobot.MOVE_LOAD_APPROACH].m_strTime = tactTime.ToString();

                mCycleTactTime[CDefine.ECycleTactOutRobot.RAWDATA].m_strTime = " ";
            }

            // Write Log
            {
                Dictionary<string, string> tactTimeDetail = new Dictionary<string, string>();
                var sbLog = new StringBuilder();
                bool bIncomplete = false;
                ETactTime tactTimeIndex = ETactTime.OUT_ROBOT;
                foreach (CDefine.ECycleTactOutRobot column in Enum.GetValues(typeof(CDefine.ECycleTactOutRobot)))
                {
                    // 불완전한 자료는 찍지 않음
                    if (true == mCycleTactTime[column].IsEmpty())
                    {
                        bIncomplete = true;
                        continue;
                    }
                    tactTimeDetail[column.ToString()] = mCycleTactTime[column].m_strTime;
                    sbLog.Append(mCycleTactTime[column].m_strTime.Trim());
                    sbLog.Append(",");
                    mCycleTactTime[column].Clear();
                }
                if (true == bIncomplete)
                {
                    Debug.WriteLine("SKIP_TACT_LOG_" + tactTimeIndex.ToString());
                    return;
                }
                m_objDocument.m_objTactTime.CycleTactTime[tactTimeIndex] = cycleTactTime;
                m_objDocument.m_objTactTime.LoadingWaitTime[tactTimeIndex] = loadingWaitTime;
                m_objDocument.m_objTactTime.UnloadingWaitTime[tactTimeIndex] = unloadingWaitTime;
                m_objDocument.m_objTactTime.UserStopTime[tactTimeIndex] = userStopTime;
                m_objDocument.m_objTactTime.UnitTactTimeDetailInformation[tactTimeIndex] = tactTimeDetail;
                unitTact.Rawdata = tactTimeDetail;
                m_objDocument.SetUpdateLog(m_objDocument.m_objCycleTactTime.m_tactOutRobot.LogType, sbLog.ToString());
            }

            string[] innerIds = mLastInnerID;
            string[] cellIds = mLastCellID;
            var tact = TactTime.Manager.GetTactOrNull(innerIds);
            if (null != tact)
            {
                tact.InnerID = innerIds;
                tact.CellID = cellIds;
                tact.UnitTacts[mDefine.ProcessIndex] = unitTact;
                m_objDocument.m_objTactTime.SetOutputTactTime(tact);
            }
        }
    }
}
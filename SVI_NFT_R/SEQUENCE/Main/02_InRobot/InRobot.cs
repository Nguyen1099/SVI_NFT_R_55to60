using Mcc;
using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R
{
    public partial class InRobot : CProcessAbstract, IUnitNode
    {
        public IReadOnlyList<CellDataHandler> CellContainer { get; private set; }
        public InRobotPicker[] Pickers { get; private set; }
        public InRobotTurnCylinder[] TurnCylinders { get; private set; }
        public InRobotMcr[] Mcrs { get; private set; }
        public InRobotNachi Nachi { get; private set; }
        public InRobotSendAlign AlignSend { get; private set; }
        public Utils.SyncToken SyncTact { get; private set; } = new Utils.SyncToken();
        public IReadOnlyDictionary<CDefine.ECycleTactInRobot, CTimeElement> CycleTactTime => mCycleTactTime;
        public bool IsIdle => (
            Pickers.All(item => item.GetCommand() == 0) // == Idle
            && TurnCylinders.All(item => item.GetCommand() == 0)
            && Mcrs.All(item => item.GetCommand() == 0)
            && Nachi.GetCommand() == 0
            && AlignSend.GetCommand() == 0
            && IsRunningSequence == false
            );
        public bool IsEmpty => CellContainer.GetExistCellCount() == 0;
        private Thread mThreadBatch;
        private ManualResetEvent mOneCycleUnlock;
        private ManualResetEvent mOneCycleEnd;
        private Dictionary<CDefine.ECycleTactInRobot, CTimeElement> mCycleTactTime;
        private string[] mLastInnerID;
        private string[] mLastCellID;
        private readonly Stopwatch mUserStopTimeStopwatch = Stopwatch.StartNew();
        private InShuttleAlignVision mPreAlign;
        private int[] mRepeatCounts;
        private double[] mAlignOffsetCounts;

        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;

            do
            {
                // 도큐먼트 객체
                m_objDocument = document;
                // IO 객체 연결
                m_objIO = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objIO;
                m_objConfig = m_objDocument.m_objConfig;
                mDefine = new Define(position);
                CellContainer = CellDataManager.ProcessCells[mDefine.ProcessIndex];
                foreach (var cellDataHandler in CellContainer)
                {
                    cellDataHandler.OwnerManager = this;
                }

                mCycleTactTime = m_objDocument.m_objCycleTactTime.m_tactInRobot.dicTactTime;
                mLastInnerID = new string[CellContainer.Count];
                mLastCellID = new string[CellContainer.Count];

                // 피커 객체 생성
                Pickers = new InRobotPicker[mDefine.PickerPositions.Length];
                for (int i = 0; i < Pickers.Length; ++i)
                {
                    Pickers[i] = new InRobotPicker();
                    if (Pickers[i].Initialize(m_objDocument, mDefine.PickerPositions[i]) == false)
                    {
                        break;
                    }
                }

                // 실린더 객체 생성
                TurnCylinders = new InRobotTurnCylinder[mDefine.TurnCylinderPositions.Length];
                for (int i = 0; i < Pickers.Length; i++)
                {
                    TurnCylinders[i] = new InRobotTurnCylinder();
                    if (TurnCylinders[i].Initialize(m_objDocument, mDefine.TurnCylinderPositions[i]) == false)
                    {
                        break;
                    }
                }

                // 나치 객체 생성
                Nachi = new InRobotNachi();
                if (Nachi.Initialize(m_objDocument, mDefine.NachiPosition) == false)
                {
                    break;
                }

                AlignSend = new InRobotSendAlign();
                if (AlignSend.Initialize(m_objDocument, mDefine.AlignSendPosition) == false)
                {
                    break;
                }

                mPreAlign = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle.AlignVision;
                mPreAlign.ReceiveCalibrationStart += PreAlign_ReceiveCalibrationStart;
                mPreAlign.ReceiveCalibrationMove += PreAlign_ReceiveCalibrationMove;
                mPreAlign.ReceiveCalibrationFinish += PreAlign_ReceiveCalibrationFinish;
                mPreAlign.ReceiveAlignError += PreAlign_ReceiveAlignError;

                Mcrs = new InRobotMcr[mDefine.McrPositions.Length];
                for (int i = 0; i < Mcrs.Length; ++i)
                {
                    Mcrs[i] = new InRobotMcr();
                    if (Mcrs[i].Initialize(m_objDocument, mDefine.McrPositions[i]) == false)
                    {
                        break;
                    }
                }

                mRepeatCounts = Enumerable.Repeat(10, 2).ToArray();
                mAlignOffsetCounts = Enumerable.Repeat(5.0, 3).ToArray();

                // 프로세스 동기화 객체 초기화
                mOneCycleUnlock = new ManualResetEvent(true);
                mOneCycleEnd = new ManualResetEvent(true);

                // 프로세스 쓰레드 생성
                mThreadProcess = new Thread(ThreadProcess);
                mThreadProcess.Start();

                // 초기화 쓰레드 생성
                mThreadInitialize = new Thread(ThreadInitializeProcess);
                mThreadInitialize.Start();

                // 배치 동작 쓰레드 생성
                mThreadBatch = new Thread(ThreadManualProcess);
                mThreadBatch.Start();

                // 재시작 동작 쓰레드 생성
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

            Pickers.ForEach(i => i.DeInitialize());
            TurnCylinders.ForEach(i => i.DeInitialize());
            Mcrs.ForEach(i => i.DeInitialize());
            mPreAlign.ReceiveCalibrationStart -= PreAlign_ReceiveCalibrationStart;
            mPreAlign.ReceiveCalibrationMove -= PreAlign_ReceiveCalibrationMove;
            mPreAlign.ReceiveCalibrationFinish -= PreAlign_ReceiveCalibrationFinish;
            mPreAlign.ReceiveAlignError -= PreAlign_ReceiveAlignError;
            AlignSend.DeInitialize();
            Nachi.DeInitialize();
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

            using (TurnCylinders.All(cylinder => cylinder.IsInposition(mDefine.LoadPositionCylinder)) ? null
                : MccLogManager.InRobot.CYL_T_RETURN.CreateMccLogProvider()
                )
            {
                TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.LoadPositionCylinder));
                if (TurnCylinders.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            Nachi.SetMccLogItem(MccLogManager.InRobot.MOVE_LD_WAIT_POS);
            Nachi.SetCommand(InRobotNachi.ECommand.LoadProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            MccLogManager.InRobot.IN_WAIT_TIME.BatchWriteStart();
            return true;
        }

        private bool DoProcessLoadGet()
        {
            var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.CheckOverlap()))
                .Concat(inShuttle.Pickers.Select(i => Task.Run(() => i.Check())))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // 작업 순서 생성
            var cellPlacement = CellDataManager.GetCellPlacementFromProcess(EProcess.InShuttle);
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, EMethod.Get, EProcess.InShuttle, ECellPlacement.Empty, cellPlacement) == false)
            {
                // ! 나오면 안되는 상황
                Debug.Assert(false);
                return false;
            }

            AlignSend.SetAlignDataFromCellData(inShuttle.CellContainer);
            AlignSend.SetCommand(InRobotSendAlign.ECommand.AlignDataSend);
            if (AlignSend.WaitForEndProcess() == false)
            {
                return false;
            }

            MccLogProvider[] mccComponentInOutItems = null;
            try
            {
                inShuttle.SyncTact.Lock();
                inShuttle.LockOneCycle();
                MccLogManager.InShuttle.OUT_WAIT_TIME.BatchWriteEndExist();
                MccLogManager.InRobot.IN_WAIT_TIME.BatchWriteEnd();
                for (int index = 0; index < workOrders.Count; index++)
                {
                    var workOrder = workOrders[index];
                    // + MCC: Write COMPONENT_IN/OUT Start
                    mccComponentInOutItems = workOrder.RobotPickers
                        .SelectMany(picker =>
                        {
                            var list = new List<MccLogProvider>(3)
                            {
                                MccLogManager.InRobot.COMPONENT_IN[picker.CellPort.PositionIndex].CreateMccLogProvider(),
                                MccLogManager.InShuttle.COMPONENT_OUT[picker.CellPort.PositionIndex].CreateMccLogProvider()
                            };
                            if (workOrders.Count > 1)
                            {
                                list.Add(MccLogManager.InRobot.TR_ACT_WAIT_TIME_COMPONENT_IN[picker.CellPort.PositionIndex].CreateMccLogProvider());
                                list.Add(MccLogManager.InShuttle.TR_ACT_WAIT_TIME_COMPONENT_OUT[picker.CellPort.PositionIndex].CreateMccLogProvider());
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
                        if (positionIndexes.Any(i => inShuttle.Pickers[i].Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_OFF) == true)
                        {
                            workOrder.CellDataMove();
                        }
                    }
                }
            }
            finally
            {
                inShuttle.CycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOADING_END_TIME].SetTime();
                inShuttle.CycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOAD_START_TIME].SetTime();
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_END_TIME].SetTime();
                inShuttle.UnlockOneCycle();
                inShuttle.SyncTact.Unlock();
                // + 얼라인 데이터 리셋
                AlignSend.SetCommand(InRobotSendAlign.ECommand.AlignZeroDataSend);
                AlignSend.WaitForEndProcess();
                // + MCC: Abnormal case. Write COMPONENT_IN/OUT End
                if (mccComponentInOutItems != null) Array.ForEach(mccComponentInOutItems, item => item.Dispose());
            }

            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_EXIT_START_TIME].SetTime();
            Nachi.SetMccLogItem(MccLogManager.InRobot.MOVE_LD_SAFETY_POS);
            Nachi.SetCommand(InRobotNachi.ECommand.LoadProcessExit);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }

            return true;
        }

        private bool DoProcessMcr()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            var cellPlacement = CellDataManager.GetCellPlacementFromProcess(EProcess.InRobot);
            if (TryGetAtOnceWorkOrders(out List<RobotSequenceItemSet> workOrders, EMethod.Put, EProcess.InRobot, cellPlacement, ECellPlacement.Empty) == false)
            {
                // ! 나오면 안되는 상황
                Debug.Assert(false);
                return false;
            }

            SetPreOrderToProcessBegin(InRobotNachi.ECommand.McrProcessBegin, workOrders);
            TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.McrPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetMccLogItem(MccLogManager.InRobot.MOVE_MCR_WAIT_POS);
            Nachi.SetCommand(InRobotNachi.ECommand.McrProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_APPROACH_END_TIME].SetTime();
            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_START_TIME].SetTime();

            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_END_TIME].SetTime();
            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_START_TIME].SetTime();
            try
            {
                for (int index = 0; index < workOrders.Count; ++index)
                {
                    SubSequenceSetPreOrder(workOrders, index);
                    var workOrder = workOrders[index];
                    if (SubSequenceMcr(workOrder.MethodIndex, workOrder.ToolIndex, workOrder.StageIndex) == false)
                    {
                        return false;
                    }
                    SubPreOrderToProcessExit(InRobotNachi.ECommand.McrProcessExit, workOrders, index);
                    if (SubSequenceCycleEnd() == false)
                    {
                        return false;
                    }
                }
            }
            finally
            {
            }

            Nachi.SetMccLogItem(MccLogManager.InRobot.MOVE_MCR_SAFETY_POS);
            Nachi.SetCommand(InRobotNachi.ECommand.McrProcessExit);
            foreach (var cellDataHandler in CellContainer.GetExistCellList())
            {
                if (cellDataHandler.Data.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK)
                {
                    TrackInManager.TryAdd(cellDataHandler);
                    Thread.Sleep(10);
                }
                cellDataHandler.Data.Cell.McrStatus = EStatus.Done;
                cellDataHandler.IsChanged = true;
                m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetUpdateHistoryProcessData(cellDataHandler.Data.DeepClone());
                m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryMCRStatus(cellDataHandler.Data.Cell.CellID, cellDataHandler.Data.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK ? CDatabaseDefine.EMcrResult.OK : CDatabaseDefine.EMcrResult.NG);
            }
            ProductCounter.TodayTrackIn.AddAsync(CellContainer.GetExistCellCount());
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }

            return true;
        }

        private bool DoProcessUnloadPutWait()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            using (TurnCylinders.All(cylinder => cylinder.IsInposition(mDefine.UnloadPositionCylinder)) ? null
                : MccLogManager.InRobot.CYL_T_TURN.CreateMccLogProvider()
                )
            {
                TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.UnloadPositionCylinder));
                if (TurnCylinders.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            Nachi.SetMccLogItem(MccLogManager.InRobot.MOVE_ULD_WAIT_POS);
            Nachi.SetCommand(InRobotNachi.ECommand.UnloadProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private bool DoProcessUnloadPut()
        {
            var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .Concat(inspStage.Pickers.Select(i => Task.Run(() => i.CheckOverlap())))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // Update TactTime Log Information
            mLastInnerID = CellContainer.Select(i => i.GetInnerID()).ToArray();
            mLastCellID = CellContainer.Select(i => i.GetCellID()).ToArray();

            // 작업 순서 생성
            var cellPlacement = CellDataManager.GetCellPlacementFromProcess(EProcess.InRobot);
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, EMethod.Put, EProcess.InspStage, cellPlacement, ECellPlacement.Empty) == false)
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
                MccLogManager.InRobot.OUT_WAIT_TIME.BatchWriteEndExist();
                MccLogManager.InspStage.IN_WAIT_TIME.BatchWriteEnd();
                for (int index = 0; index < workOrders.Count; ++index)
                {
                    var workOrder = workOrders[index];
                    // + MCC: Write COMPONENT_IN/OUT Start
                    mccComponentInOutItems = workOrder.RobotPickers
                        .SelectMany(picker =>
                        {
                            var list = new List<MccLogProvider>(3)
                            {
                                MccLogManager.InRobot.COMPONENT_OUT[picker.CellPort.PositionIndex].CreateMccLogProvider(),
                                MccLogManager.InspStage.COMPONENT_IN[picker.CellPort.PositionIndex].CreateMccLogProvider()
                            };
                            if (workOrders.Count > 1)
                            {
                                list.Add(MccLogManager.InRobot.TR_ACT_WAIT_TIME_COMPONENT_OUT[picker.CellPort.PositionIndex].CreateMccLogProvider());
                                list.Add(MccLogManager.InspStage.TR_ACT_WAIT_TIME_COMPONENT_IN[picker.CellPort.PositionIndex].CreateMccLogProvider());
                            }
                            return list;
                        })
                        .ToArray();
                    SubSequenceSetPreOrder(workOrders, index);
                    try
                    {
                        if (SubSequenceUnload(workOrder.MethodIndex, workOrder.ToolIndex, workOrder.StageIndex) == false)
                        {
                            return false;
                        }
                        SubPreOrderToProcessExit(InRobotNachi.ECommand.UnloadProcessExit, workOrders, index);
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
                inspStage.CycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOADING_END_TIME].SetTime();
                inspStage.CycleTactTime[CDefine.ECycleTactInspStage.WAIT_GRAB_START_TIME].SetTime();
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_END_TIME].SetTime();
                inspStage.UnlockOneCycle();
                inspStage.SyncTact.Unlock();
                // + MCC: Abnormal case. Write COMPONENT_IN/OUT End
                if (mccComponentInOutItems != null) Array.ForEach(mccComponentInOutItems, item => item.Dispose());
            }

            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_EXIT_START_TIME].SetTime();
            Nachi.SetMccLogItem(MccLogManager.InRobot.MOVE_ULD_SAFETY_POS);
            Nachi.SetCommand(InRobotNachi.ECommand.UnloadProcessExit);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }

            return true;
        }

        private bool DoProcessInitialize()
        {
            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)InRobotNachi.ECommand.Initialize) == false)
            {
                return false;
            }
            Nachi.SetMccLogItem(MccLogManager.InRobot.INIT_UNIT);
            Nachi.SetCommand(InRobotNachi.ECommand.Initialize);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private bool DoProcessRestart()
        {
            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)InRobotNachi.ECommand.Initialize) == false)
            {
                return false;
            }
            Nachi.SetMccLogItem(MccLogManager.InRobot.INIT_UNIT);
            Nachi.SetCommand(InRobotNachi.ECommand.Initialize);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            if (IsEmpty == true)
            {
                InterferenceRegion.InShuttleAlignArea.Exit();
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
                    if (mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_START_TIME].IsEmpty() == false
                        && mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_END_TIME].IsEmpty() == true
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
                var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
                var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
                IsRunningSequence = true;
                IsCompleteWriteCycleLog = false;
                SetProcessLog($"AutoSequence(Begin) : {sequencePointer}");
                switch (sequencePointer)
                {
                    case EAutoSequence.LoadGetWait:
                        mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOAD_APPROACH_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOAD_APPROACH_START_TIME].SetTime();
                        if (DoProcessLoadGetWait() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOAD_APPROACH_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInRobot.CYCLE_END_TIME].SetTime();
                            SetWriteCycleTactTimeLog();
                            IsCompleteWriteCycleLog = true;
                            mCycleTactTime[CDefine.ECycleTactInRobot.CYCLE_START_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOADING_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOAD_APPROACH_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOAD_APPROACH_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.LoadGet:
                        inShuttle.SyncTact.WaitingUnlock();
                        inShuttle.SyncTact.Lock();
                        inShuttle.CycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_END_TIME].SetTime();
                        inShuttle.CycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOADING_START_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_START_TIME].SetTime();
                        inShuttle.SyncTact.Unlock();
                        if (DoProcessLoadGet() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_EXIT_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_APPROACH_START_TIME].SetTime();
                        }
                        else
                        {
                            inShuttle.CycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_END_TIME].Clear();
                            inShuttle.CycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOADING_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_START_TIME].Clear();
                            inShuttle.CycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOADING_END_TIME].Clear();
                            inShuttle.CycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOAD_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_EXIT_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.Mcr:
                        mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_APPROACH_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_APPROACH_START_TIME].SetTime();
                        if (DoProcessMcr() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOAD_APPROACH_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_APPROACH_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_APPROACH_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_APPROACH_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.UnloadPutWait:
                        mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOAD_APPROACH_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOAD_APPROACH_START_TIME].SetTime();
                        if (DoProcessUnloadPutWait() == true)
                        {
                            InterferenceRegion.InShuttleAlignArea.Exit();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOAD_APPROACH_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_START_TIME].SetTime();
                            MccLogManager.InRobot.OUT_WAIT_TIME.BatchWriteStartExist();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOAD_APPROACH_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOAD_APPROACH_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.UnloadPut:
                        inspStage.SyncTact.WaitingUnlock();
                        inspStage.SyncTact.Lock();
                        inspStage.CycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOADING_END_TIME].SetTime();
                        inspStage.CycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOADING_START_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_START_TIME].SetTime();
                        inspStage.SyncTact.Unlock();
                        if (DoProcessUnloadPut() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_EXIT_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOAD_APPROACH_START_TIME].SetTime();
                        }
                        else
                        {
                            inspStage.CycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOADING_END_TIME].Clear();
                            inspStage.CycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOADING_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_START_TIME].Clear();
                            inspStage.CycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOADING_END_TIME].Clear();
                            inspStage.CycleTactTime[CDefine.ECycleTactInspStage.WAIT_GRAB_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_EXIT_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        InterferenceRegion.InspectionStageArea.Exit();
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
                UnitName = EProcess.InRobot.ToString()
            };
            double cycleTactTime, loadingWaitTime, unloadingWaitTime, userStopTime;

            // Calculate Wait Time
            {
                DateTime startTime, endTime;
                double waitingTime, tactTime;
                loadingWaitTime = unloadingWaitTime = 0;
                unitTact.InputDelayTime = unitTact.OutputDelayTime = TimeSpan.Zero;

                // INNER ID
                mCycleTactTime[CDefine.ECycleTactInRobot.INNER_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[0]) ? " " : mLastInnerID[0];
                mCycleTactTime[CDefine.ECycleTactInRobot.INNER_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[1]) ? " " : mLastInnerID[1];

                // CELL ID
                mCycleTactTime[CDefine.ECycleTactInRobot.CELL_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[0]) ? " " : mLastCellID[0];
                mCycleTactTime[CDefine.ECycleTactInRobot.CELL_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[1]) ? " " : mLastCellID[1];

                // USER_STOP_TIME [WAIT]
                unitTact.UserStopTime = mUserStopTimeStopwatch.Elapsed;
                userStopTime = unitTact.UserStopTime.TotalSeconds;
                mUserStopTimeStopwatch.Reset();
                mCycleTactTime[CDefine.ECycleTactInRobot.USER_STOP_TIME].m_strTime = userStopTime.ToString();

                // PURE CYCLE TACT [TACT] - 유닛 사이클 택타임에서 인터페이스 대기시간을 뺀 시간
                tactTime = m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInRobot.CYCLE_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInRobot.CYCLE_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOADING_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_END_TIME].GetTime());
                tactTime -= userStopTime;
                mCycleTactTime[CDefine.ECycleTactInRobot.PURE_CYCLE_TACT].m_strTime = tactTime.ToString();

                // BEFORE_CYCLE_START ~ CYCLE_START [TACT] 
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.CYCLE_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.CYCLE_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.CYCLE_TACT_TIME].m_strTime = tactTime.ToString();
                cycleTactTime = tactTime;
                unitTact.UnitTactTime = TimeSpan.FromSeconds(tactTime);

                // NACHI LOADING FROM IN-SHUTTLE [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOADING].m_strTime = waitingTime.ToString();
                loadingWaitTime += waitingTime;
                unitTact.InputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // NACHI LOADING FROM IN-SHUTTLE [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING].m_strTime = tactTime.ToString();

                // NACHI LOADING EXIT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_EXIT_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_EXIT_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOADING_EXIT].m_strTime = tactTime.ToString();

                // NACHI MCR APPROACH [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_APPROACH_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_APPROACH].m_strTime = waitingTime.ToString();

                // NACHI MCR APPROACH [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_APPROACH_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_APPROACH].m_strTime = tactTime.ToString();

                // NACHI MCR [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_MCR].m_strTime = waitingTime.ToString();

                // NACHI MCR [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_MCR].m_strTime = tactTime.ToString();

                // NACHI UNLOAD TO INSP-STAGE APPROACH [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOAD_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOAD_APPROACH_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOAD_APPROACH].m_strTime = waitingTime.ToString();

                // NACHI UNLOAD TO INSP-STAGE APPROACH [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOAD_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOAD_APPROACH_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOAD_APPROACH].m_strTime = tactTime.ToString();

                // NACHI UNLOADING TO INSP-STAGE [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_UNLOADING].m_strTime = waitingTime.ToString();
                unloadingWaitTime += waitingTime;
                unitTact.OutputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // NACHI UNLOADING TO INSP-STAGE [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING].m_strTime = tactTime.ToString();

                // NACHI UNLOADING EXIT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_EXIT_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_EXIT_START_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_UNLOADING_EXIT].m_strTime = tactTime.ToString();

                // NACHI LOAD FROM INSP-STAGE APPROACH [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOAD_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOAD_APPROACH_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.WAIT_LOAD_APPROACH].m_strTime = waitingTime.ToString();

                // NACHI LOAD FROM INSP-STAGE APPROACH [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOAD_APPROACH_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOAD_APPROACH_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInRobot.MOVE_LOAD_APPROACH].m_strTime = tactTime.ToString();

                mCycleTactTime[CDefine.ECycleTactInRobot.RAWDATA].m_strTime = " ";
            }

            // Write Log
            {
                Dictionary<string, string> tactTimeDetail = new Dictionary<string, string>();
                var sbLog = new StringBuilder();
                bool bIncomplete = false;
                ETactTime tactTimeIndex = ETactTime.IN_ROBOT;
                foreach (CDefine.ECycleTactInRobot column in Enum.GetValues(typeof(CDefine.ECycleTactInRobot)))
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
                m_objDocument.SetUpdateLog(m_objDocument.m_objCycleTactTime.m_tactInRobot.LogType, sbLog.ToString());
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
using Mcc;
using SVI_NFT_R.CellData;
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
    public partial class InspStage : CProcessAbstract, IUnitNode
    {
        public IReadOnlyList<CellDataHandler> CellContainer { get; private set; }
        public InspStagePicker[] Pickers { get; private set; }
        public InspStageInspect[] Inspects { get; private set; }
        public InspStageMotorY MotorStageY { get; private set; }
        public Utils.SyncToken SyncTact { get; private set; } = new Utils.SyncToken();
        public bool CanInput { get; private set; } = true;
        public bool IsIdle => (
            Pickers.All(item => item.GetCommand() == 0) // == Idle
            && Inspects.All(item => item.GetCommand() == 0)
            && MotorStageY.GetCommand() == 0
            && IsRunningSequence == false
            );
        public bool IsEmpty => CellContainer.GetExistCellCount() == 0;
        public IReadOnlyDictionary<CDefine.ECycleTactInspStage, CTimeElement> CycleTactTime => mCycleTactTime;
        private readonly InternalAlgorithmData[][] mMccInspectAlgorithmItems = new InternalAlgorithmData[][]
        {
            new InternalAlgorithmData[0],
            new InternalAlgorithmData[0]
        };
        private int mMccInspectAlgorithmCurrentIndex = int.MaxValue - 10;
        private Thread mThreadBatch;
        private ManualResetEvent mOneCycleUnlock;
        private ManualResetEvent mOneCycleEnd;
        private Dictionary<CDefine.ECycleTactInspStage, CTimeElement> mCycleTactTime;
        private string[] mLastInnerID;
        private string[] mLastCellID;
        private readonly Stopwatch mUserStopTimeStopwatch = Stopwatch.StartNew();
        private static readonly string REGISTRY_PATH = Path.Combine("ENC", Program.ID, "SEQUENCE", nameof(InspStage));
        private readonly Settings.RegistryComponent mBackupData = new Settings.RegistryComponent(REGISTRY_PATH);

        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;
            do
            {
                // 도큐먼트 객체
                m_objDocument = document;
                // IO 객체 연결
                m_objIO = m_objDocument.m_objProcessMain.m_objIO;
                m_objConfig = m_objDocument.m_objConfig;
                mDefine = new Define(position);
                CellContainer = CellDataManager.ProcessCells[mDefine.ProcessIndex];
                foreach (var cellDataHandler in CellContainer)
                {
                    cellDataHandler.OwnerManager = this;
                }

                mCycleTactTime = m_objDocument.m_objCycleTactTime.m_tactInspStage.dicTactTime;

                mLastInnerID = new string[CellContainer.Count];
                mLastCellID = new string[CellContainer.Count];

                // 모터 객체 생성
                MotorStageY = new InspStageMotorY();
                if (MotorStageY.Initialize(m_objDocument, mDefine.StageMotorPosition) == false)
                {
                    break;
                }

                // 피커 객체 생성
                Pickers = new InspStagePicker[mDefine.PickerPositions.Length];
                for (int i = 0; i < Pickers.Length; ++i)
                {
                    Pickers[i] = new InspStagePicker();
                    if (Pickers[i].Initialize(m_objDocument, mDefine.PickerPositions[i]) == false)
                    {
                        break;
                    }
                }

                // 검사 인터페이스 객체 생성
                Inspects = new InspStageInspect[mDefine.InspectPositions.Length];
                for (int i = 0; i < Inspects.Length; ++i)
                {
                    Inspects[i] = new InspStageInspect();
                    if (Inspects[i].Initialize(m_objDocument, mDefine.InspectPositions[i]) == false)
                    {
                        break;
                    }
                }
                Inspects.First(x => x.IsPrimary()).OnReceiveTotalResult += InspectOnReceiveTotalResult;

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

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override void DeInitialize()
        {
            mbThreadExit = true;
            mThreadBatch.Join();
            mThreadRestart.Join();
            mThreadInitialize.Join();
            mThreadProcess.Join();

            mOneCycleUnlock.Dispose();
            mOneCycleEnd.Dispose();

            foreach (var item in Inspects)
            {
                item.DeInitialize();
            }
            Inspects.First(x => x.IsPrimary())
                .OnReceiveTotalResult -= InspectOnReceiveTotalResult;
            foreach (var item in Pickers)
            {
                item.DeInitialize();
            }
            MotorStageY.DeInitialize();
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

        public void StopInput()
        {
            CanInput = false;
        }

        public void ResumeInput()
        {
            CanInput = true;
        }

        public bool TryGetCellPlacementFromVacuum(out ECellPlacement cellPlacement)
        {
            cellPlacement = 0;
            int cellCount = 0;
            foreach (var picker in Pickers)
            {
                if (picker.Vacuum.Status != CVacuumAbstract.EVacuumStatus.STS_ON)
                {
                    continue;
                }
                cellPlacement |= picker.CellPort.CellPlacement;
                ++cellCount;
            }
            if (cellCount == 0)
            {
                cellPlacement = ECellPlacement.Empty;
                return false;
            }
            if (cellCount == Pickers.Length)
            {
                cellPlacement |= ECellPlacement.Full;
            }
            return true;
        }

        private void SetMccAlgorithmStart(CDefine.EInspectType inspectType)
        {
#if !SCT_AMO
            if (
                m_objDocument.m_objConfig.GetInspOptionParameter(inspectType).eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE
                || m_objDocument.GetRunMode() != CDefine.ERunMode.RealRun
                )
            {
                return;
            }
#endif
            InternalAlgorithmData[] createItems = InternalAlgorithmData.CreateArrayFromCellDataHandler(CellContainer);
            if (createItems == null)
            {
                return;
            }

            var mccAlgorithemItems = mMccInspectAlgorithmItems;
            IncreaseAlgorithmIndex(ref mMccInspectAlgorithmCurrentIndex, mccAlgorithemItems.Length);
            int algorithmCurrentIndex = mMccInspectAlgorithmCurrentIndex;
            mccAlgorithemItems[algorithmCurrentIndex] = createItems;

            //MccLogManager.Inspect.INSP_ALGORITHM[0][algorithmCurrentIndex].WriteStart();
            foreach (var item in createItems)
            {
                //MccLogManager.Inspect.INSP_CELL_SIGNALING_POINT[0][item.PositionIndex].WriteStart(item.Data);
                //MccLogManager.Inspect.INSP_CELL_SIGNALING_POINT[0][item.PositionIndex].WriteEnd();
            }
        }

        private void IncreaseAlgorithmIndex(ref int index, int maxCount)
        {
            index++;
            if (index >= maxCount)
            {
                index = 0;
            }
        }

        private void InspectOnReceiveTotalResult(object sender, DEVICE.Svi.ReceiveData.TotalResultDataForRear resultData)
        {
            InternalAlgorithmData[] captureItems = null;
            int algorithmIndex = -1;
            for (int i = 0; i < mMccInspectAlgorithmItems.Length; i++)
            {
                captureItems = mMccInspectAlgorithmItems[i];
                foreach (var item in captureItems)
                {
                    if (item.TrySetAlgorithmFinished(resultData.CellID) == false)
                    {
                        continue;
                    }

                    //MccLogManager.Inspect.INSP_CELL_SIGNALING_POINT[0][item.PositionIndex].WriteStart(item.Data);
                    //MccLogManager.Inspect.INSP_CELL_SIGNALING_POINT[0][item.PositionIndex].WriteEnd();
                    algorithmIndex = i;
                    break;
                }

                if (algorithmIndex != -1)
                {
                    break;
                }
            }

            if (algorithmIndex == -1)
            {
                return;
            }

            foreach (var item in captureItems)
            {
                if (item.IsAlgorithmFinished == false)
                {
                    return;
                }
            }

            //MccLogManager.Inspect.INSP_ALGORITHM[0][algorithmIndex].WriteEnd();
        }

        private bool DoProcessLoad()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.CheckOverlap()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            using (var mccMoveLD = MccLogManager.InspStage.MOVE_X_LD_POS.CreateMccLogProvider())
            {
                if (SetSubCommandStageMove(InspStageMotorY.ECommand.LoadPosition) == false)
                {
                    return false;
                }
            }
            MccLogManager.InspStage.IN_WAIT_TIME.BatchWriteStart();
            return true;
        }

        private bool DoProcessScan()
        {
            Task<bool>[] cellCheckTasks = Pickers
                .Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            Parallel.ForEach(CellContainer.GetExistCellList(), cellDataHandler =>
            {
                // ! 이미 트랙인이 완료된 경우 MCC로그를 남기지 않음
                using (var logScope = TrackInManager.IsTrackInFinished(cellDataHandler) == true ? null
                    : MccLogManager.InspStage.CIM_TRACKING_PROCESS[cellDataHandler.PositionIndex].CreateMccLogProvider()
                    )
                {
                    TrackInManager.WaitForEndProcess(cellDataHandler);
                    CellDataManager.UpdateID(cellDataHandler);
                }
            });

            try
            {
                using (var mccInpsection = MccLogManager.InspStage.VISION_INSP_PROCESS.CreateMccLogProvider())
                using (var mccInpsect = MccLogManager.Inspect.INSP_PROCESS_2.CreateMccLogProvider())
                {
                    Inspects.ForEach(x => x.SetGrabInformation(CellContainer));

                    var cellDataSyncTask = Task.Run(() =>
                    {
                        foreach (var cellDataHandler in CellContainer.GetExistCellList())
                        {
                            if (cellDataHandler.Data.Cell.InspCellDataSyncStatus == EStatus.Done)
                            {
                                continue;
                            }
                            try
                            {
                                Inspects.ForEach(x => x.SetCellDataSyncInformation(cellDataHandler));
                                Inspects.ForEach(x => x.SetCommand(InspStageInspect.ECommand.CellDataSync));
                                if (Inspects.WaitForEndProcess() == false)
                                {
                                    return false;
                                }
                                cellDataHandler.Data.Cell.InspCellDataSyncStatus = EStatus.Done;
                                cellDataHandler.IsChanged = true;
                            }
                            finally
                            {
                                Inspects.ForEach(x => x.ResetCellDataSyncInformation());
                            }
                        }
                        return true;
                    });
                    if (CellContainer[0].IsCellExist() == true)
                    {
                        MotorStageY.SetMccLogItem(MccLogManager.InspStage.MOVE_Y_LD_TO_IS21_INSP_POS);
                        MotorStageY.SetCommand(InspStageMotorY.ECommand.GrabP1Position);
                        if (MotorStageY.WaitForEndProcess() == false)
                        {
                            return false;
                        }
                        Task.WaitAll(cellDataSyncTask);
                        if (cellDataSyncTask.Result == false)
                        {
                            return false;
                        }
                        using (var mccInspectionGrab = MccLogManager.InspStage.VISION_OPTIC_INSP_PROCESS_1.CreateMccLogProvider())
                        using (var mccInspectGrab = MccLogManager.Inspect.INSP_WAIT_TIME.CreateMccLogProvider())
                        {
                            var taskInspectScanBeforeWait = SetSubCommandInspectScanBeforeWait(0);
                            Task.WaitAll(taskInspectScanBeforeWait);
                            if (taskInspectScanBeforeWait.Result == false)
                            {
                                return false;
                            }
                        }
                    }
                    mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P1_END_TIME].SetTime();
                    mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P2_START_TIME].SetTime();
                    if (CellContainer[1].IsCellExist() == true)
                    {
                        // Grab P2 시작위치 분기
                        MotorStageY.SetMccLogItem(MotorStageY.IsInposition(InspStageMotorY.ECommand.GrabP1Position) == true ? MccLogManager.InspStage.MOVE_Y_IS21_INSP_TO_IS22_INSP_POS : MccLogManager.InspStage.MOVE_Y_LD_TO_IS22_INSP_POS);
                        MotorStageY.SetCommand(InspStageMotorY.ECommand.GrabP2Position);
                        if (MotorStageY.WaitForEndProcess() == false)
                        {
                            return false;
                        }
                        using (var mccInspectionGrab = MccLogManager.InspStage.VISION_OPTIC_INSP_PROCESS_2.CreateMccLogProvider())
                        using (var mccInspectGrab = MccLogManager.Inspect.INSP_WAIT_TIME.CreateMccLogProvider())
                        {
                            var taskInspectScanBeforeWait = SetSubCommandInspectScanBeforeWait(1);
                            Task.WaitAll(taskInspectScanBeforeWait);
                            if (taskInspectScanBeforeWait.Result == false)
                            {
                                return false;
                            }
                        }
                    }
                }

                foreach (var cellDataHandler in CellContainer)
                {
                    cellDataHandler.Data.Cell.InspStatus = EStatus.Done;
                    cellDataHandler.IsChanged = true;
                    m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetUpdateHistoryProcessData(cellDataHandler.Data.DeepClone());
                }
                return true;
            }
            finally
            {
                Inspects.ForEach(x => x.ResetGrabInformation());
            }
        }

        private bool DoProcessUnload()
        {
            Task<bool>[] cellCheckTasks = Pickers
                .Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // Update TactTime Log Information
            mLastInnerID = CellContainer.Select(i => i.GetInnerID()).ToArray();
            mLastCellID = CellContainer.Select(i => i.GetCellID()).ToArray();

            // Unload 시작위치 분기
            using (var mccMoveUnload = 
                MotorStageY.IsInposition(InspStageMotorY.ECommand.GrabP1Position) == true ? MccLogManager.InspStage.MOVE_Y_IS21_INSP_TO_ULD_POS.CreateMccLogProvider()
                : MotorStageY.IsInposition(InspStageMotorY.ECommand.GrabP2Position) == true ? MccLogManager.InspStage.MOVE_Y_IS22_INSP_TO_ULD_POS.CreateMccLogProvider()
                : null
                )
            {
                if (SetSubCommandStageMove(InspStageMotorY.ECommand.UnloadPosition) == false)
                {
                    return false;
                }
            }
            return true;
        }

        private bool DoProcessInitialize()
        {
            try
            {
                using (var mccInitVision = MccLogManager.Inspect.INIT_UNIT.CreateMccLogProvider())
                using (var mccInitMotor = MccLogManager.InspStage.INIT_UNIT.CreateMccLogProvider())
                {
                    if (MotorStageY.m_objInterlock.CheckMotionClassInterlock(MotorStageY.MotorIndex.ToString(), CDefine.ORIGIN_POSITION_NO) == false)
                    {
                        return false;
                    }
                    MotorStageY.SetCommand(InspStageMotorY.ECommand.Home);
                    if (MotorStageY.WaitForEndProcess() == false)
                    {
                        return false;
                    }
                    // Home 완료 후 바로 이동하면 커맨드 포지션과 액추얼 포지션이 틀어지는 현상이 있어서 추가함.
                    Thread.Sleep(100);
                    return true;
                }
            }
            finally
            {
                // 모터 정지
                MotorStageY.Axis.HLMoveStop();
            }
        }

        private bool DoProcessRestart()
        {
            if (MotorStageY.IsNeedOrigin == true
                && MotorStageY.m_objInterlock.CheckMotionClassInterlock(MotorStageY.MotorIndex.ToString(), CDefine.ORIGIN_POSITION_NO) == false
                )
            {
                return false;
            }
            MotorStageY.SetCommand(MotorStageY.IsNeedOrigin ? InspStageMotorY.ECommand.Home : InspStageMotorY.ECommand.WaitInterlock);
            if (MotorStageY.WaitForEndProcess() == false)
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
                    if (mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_START_TIME].IsEmpty() == false
                        && mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_END_TIME].IsEmpty() == true
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
                if (IsInspStage(sequencePointer) == false)
                {
                    continue;
                }

                // 시퀀스 동작
                IsRunningSequence = true;
                IsCompleteWriteCycleLog = false;
                SetProcessLog($"AutoSequence(Begin) : {sequencePointer}");
                switch (sequencePointer)
                {
                    case EAutoSequence.Load:
                        SyncTact.WaitingUnlock();
                        SyncTact.Lock();
                        mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOAD_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOAD_START_TIME].SetTime();
                        if (DoProcessLoad() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOAD_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInspStage.CYCLE_END_TIME].SetTime();
                            SetWriteCycleTactTimeLog();
                            IsCompleteWriteCycleLog = true;
                            mCycleTactTime[CDefine.ECycleTactInspStage.CYCLE_START_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOADING_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOAD_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOAD_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        SyncTact.Unlock();
                        break;

                    case EAutoSequence.Scan:
                        mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_GRAB_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P1_START_TIME].SetTime();
                        if (DoProcessScan() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P2_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOAD_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_GRAB_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P1_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P1_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P2_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.Unload:
                        SyncTact.WaitingUnlock();
                        SyncTact.Lock();
                        mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOAD_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOAD_START_TIME].SetTime();
                        if (DoProcessUnload() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOAD_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_START_TIME].SetTime();
                            MccLogManager.InspStage.OUT_WAIT_TIME.BatchWriteStartExist();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOAD_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOAD_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        SyncTact.Unlock();
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
                UnitName = mDefine.ProcessIndex.ToString()
            };
            double cycleTactTime, loadingWaitTime, unloadingWaitTime, userStopTime, grabTime;

            // Calculate Wait Time
            {
                DateTime startTime, endTime;
                double waitingTime, tactTime;
                loadingWaitTime = unloadingWaitTime = grabTime = 0;
                unitTact.InputDelayTime = unitTact.OutputDelayTime = TimeSpan.Zero;

                // INNER ID
                mCycleTactTime[CDefine.ECycleTactInspStage.INNER_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[0]) ? " " : mLastInnerID[0];
                mCycleTactTime[CDefine.ECycleTactInspStage.INNER_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[1]) ? " " : mLastInnerID[1];

                // CELL ID
                mCycleTactTime[CDefine.ECycleTactInspStage.CELL_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[0]) ? " " : mLastCellID[0];
                mCycleTactTime[CDefine.ECycleTactInspStage.CELL_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[1]) ? " " : mLastCellID[1];

                // USER_STOP_TIME [WAIT]
                unitTact.UserStopTime = mUserStopTimeStopwatch.Elapsed;
                userStopTime = unitTact.UserStopTime.TotalSeconds;
                mUserStopTimeStopwatch.Reset();
                mCycleTactTime[CDefine.ECycleTactInspStage.USER_STOP_TIME].m_strTime = userStopTime.ToString();

                // PURE CYCLE TACT [TACT] - 유닛 사이클 택타임에서 인터페이스 대기시간을 뺀 시간
                tactTime = m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInspStage.CYCLE_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInspStage.CYCLE_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOADING_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_END_TIME].GetTime());
                tactTime -= userStopTime;
                mCycleTactTime[CDefine.ECycleTactInspStage.PURE_CYCLE_TACT].m_strTime = tactTime.ToString();

                // BEFORE_CYCLE_START ~ CYCLE_START [MOVE] 
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.CYCLE_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.CYCLE_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.CYCLE_TACT_TIME].m_strTime = tactTime.ToString();
                unitTact.UnitTactTime = TimeSpan.FromSeconds(tactTime);
                cycleTactTime = tactTime;

                // INSP-STAGE LOADING FROM IN-ROBOT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOADING].m_strTime = waitingTime.ToString();
                loadingWaitTime += waitingTime;
                unitTact.InputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // INSP-STAGE LOADING FROM IN-ROBOT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOADING].m_strTime = tactTime.ToString();

                // INSP-STAGE STANDBY GRAB [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_GRAB_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_GRAB_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_GRAB].m_strTime = waitingTime.ToString();

                // INSP-STAGE GRAB P1 [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P1_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P1_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                grabTime += tactTime;
                mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P1].m_strTime = tactTime.ToString();

                // INSP-STAGE GRAB P2 [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P2_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P2_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                grabTime += tactTime;
                mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_GRAB_P2].m_strTime = tactTime.ToString();

                // INSP-STAGE UNLOAD FROM OUT-ROBOT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOAD_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOAD].m_strTime = waitingTime.ToString();

                // INSP-STAGE UNLOAD FROM OUT-ROBOT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOAD_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOAD].m_strTime = tactTime.ToString();

                // INSP-STAGE UNLOADING TO OUT-ROBOT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_UNLOADING].m_strTime = waitingTime.ToString();
                unloadingWaitTime += waitingTime;
                unitTact.OutputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // INSP-STAGE UNLOADING TO OUT-ROBOT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_UNLOADING].m_strTime = tactTime.ToString();

                // INSP-STAGE LOAD FROM IN-ROBOT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOAD_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.WAIT_LOAD].m_strTime = waitingTime.ToString();

                // INSP-STAGE LOAD FROM IN-ROBOT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOAD_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInspStage.MOVE_LOAD].m_strTime = tactTime.ToString();

                mCycleTactTime[CDefine.ECycleTactInspStage.RAWDATA].m_strTime = " ";
            }

            // Write Log
            {
                Dictionary<string, string> tactTimeDetail = new Dictionary<string, string>();
                var sbLog = new StringBuilder();
                bool bIncomplete = false;
                ETactTime tactTimeIndex = ETactTime.INSP_STAGE;
                CDefine.ELogType eLogType = CDefine.ELogType.LOG_CYCLE_TACT_03_INSP_STAGE;
                foreach (CDefine.ECycleTactInspStage column in Enum.GetValues(typeof(CDefine.ECycleTactInspStage)))
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

                m_objDocument.SetUpdateLog(eLogType, sbLog.ToString());
            }

            string[] innerIds = mLastInnerID;
            string[] cellIds = mLastCellID;
            var tact = TactTime.Manager.AddTactOrNull(innerIds);
            if (null != tact)
            {
                tact.InnerID = innerIds;
                tact.CellID = cellIds;
                tact.InputDelayTime = TimeSpan.FromSeconds(loadingWaitTime);
                tact.OutputDelayTime = TimeSpan.FromSeconds(unloadingWaitTime);
                tact.InspectionTact = TimeSpan.FromSeconds(grabTime);
                tact.UnitTacts[mDefine.ProcessIndex] = unitTact;
                m_objDocument.m_objTactTime.SetOutputTactTime(tact);
            }
        }
    }
}
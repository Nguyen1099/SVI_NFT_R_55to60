using EqToEq;
using Mcc;
using SVI_NFT_R.CellData;
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
    public partial class InShuttle : CProcessAbstract, IInputUnitNode
    {
        public IReadOnlyList<CellDataHandler> CellContainer { get; private set; }
        public InShuttlePicker[] Pickers { get; private set; }
        public InShuttleInspect[] Inspects { get; private set; }
        public InShuttleAlignVision AlignVision { get; private set; }
        public InShuttleMotorX MotorStageX { get; private set; }
        public Utils.SyncToken SyncTact { get; private set; } = new Utils.SyncToken();
        public bool CanInput { get; private set; } = true;
        public bool IsIdle => (
            Pickers.All(item => item.GetCommand() == 0) // == Idle
            && Inspects.All(item => item.GetCommand() == 0)
            && AlignVision.GetCommand() == 0
            && MotorStageX.GetCommand() == 0
            && IsRunningSequence == false
            );
        public bool IsEmpty => CellContainer.GetExistCellCount() == 0;
        public IReadOnlyDictionary<CDefine.ECycleTactInShuttle, CTimeElement> CycleTactTime => mCycleTactTime;
        public bool IsStartLoadInterface
        {
            get
            {
                return mbStartLoadInterface;
            }
            set
            {
                mbStartLoadInterface = value;
            }
        }
        private bool mbStartLoadInterface;
        private Thread mThreadBatch;
        private Thread mThreadMfq;
        private ManualResetEvent mOneCycleUnlock;
        private ManualResetEvent mOneCycleEnd;
        private Dictionary<CDefine.ECycleTactInShuttle, CTimeElement> mCycleTactTime;
        private string[] mLastInnerID;
        private string[] mLastCellID;
        private readonly Stopwatch mUserStopTimeStopwatch = Stopwatch.StartNew();

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

                mCycleTactTime = m_objDocument.m_objCycleTactTime.m_tactInShuttle.dicTactTime;

                mLastInnerID = new string[CellContainer.Count];
                mLastCellID = new string[CellContainer.Count];

                // 모터 객체 생성
                MotorStageX = new InShuttleMotorX();
                if (MotorStageX.Initialize(m_objDocument, mDefine.StageMotorPosition) == false)
                {
                    break;
                }

                // 얼라인 객체 생성
                AlignVision = new InShuttleAlignVision();
                if (AlignVision.Initialize(m_objDocument, mDefine.AlignPosition) == false)
                {
                    break;
                }

                // 피커 객체 생성
                Pickers = new InShuttlePicker[mDefine.PickerPositions.Length];
                for (int i = 0; i < Pickers.Length; ++i)
                {
                    Pickers[i] = new InShuttlePicker();
                    if (Pickers[i].Initialize(m_objDocument, mDefine.PickerPositions[i]) == false)
                    {
                        break;
                    }
                }

                // 검사 인터페이스 객체 생성
                Inspects = new InShuttleInspect[mDefine.InspectPositions.Length];
                for (int i = 0; i < Inspects.Length; ++i)
                {
                    Inspects[i] = new InShuttleInspect();
                    if (Inspects[i].Initialize(m_objDocument, mDefine.InspectPositions[i]) == false)
                    {
                        break;
                    }
                }

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

                // MFQ 동작 쓰레드 생성
                mThreadMfq = new Thread(ThreadMfqProcess);
                mThreadMfq.Start();

                // 재시작 동작 쓰레드 생성
                mThreadRestart = new Thread(ThreadRestartProcess);
                mThreadRestart.Start();

                Task.Run(() =>
                {
                    SpinWait.SpinUntil(() => m_objDocument.IsInitialized);

                    var loadInterface = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface as IPassiveVacuumAction;
                    Debug.Assert(loadInterface != null, $"{nameof(IPassiveVacuumAction)} 기능 미구현!!!");
                    loadInterface.ReceiveActiveVacuumRequest += LoadInterface_ReceiveActiveVacuumRequest;
                });

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
            mThreadMfq.Join();

            mOneCycleUnlock.Dispose();
            mOneCycleEnd.Dispose();
            foreach (var item in Inspects)
            {
                item.DeInitialize();
            }
            foreach (var item in Pickers)
            {
                item.DeInitialize();
            }
            AlignVision.DeInitialize();
            MotorStageX.DeInitialize();
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

        public void LoadInterface_ReceiveActiveVacuumRequest(object sender, ReceiveVacuumEventArgs e)
        {
            if (e.Placement.HasFlag(EPosition.P1))
            {
                var picker = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle.Pickers[0];
                MccLogManager.InShuttle.COMPONENT_IN[0].WriteStart();
                picker.SetMccLogItem(MccLogManager.InShuttle.GET_VAC_ON[0]);
                picker.SetCommand(InShuttlePicker.ECommand.Pick);
            }
            if (e.Placement.HasFlag(EPosition.P2))
            {
                var picker = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle.Pickers[1];
                MccLogManager.InShuttle.COMPONENT_IN[1].WriteStart();
                picker.SetMccLogItem(MccLogManager.InShuttle.GET_VAC_ON[1]);
                picker.SetCommand(InShuttlePicker.ECommand.Pick);
            }
        }

        public void CreateCellData(int position)
        {
            CellContainer[position].Data.IsUse = true;
            CellContainer[position].Data.Cell.ChannelName = string.Format("CN{0:00}", position + 1);
            CellContainer[position].Data.Cell.PPID = m_objDocument.m_objConfig.GetSystemParameter().strPPID;
            CellContainer[position].Data.Cell.InputTime = DateTime.Now;
            CellContainer[position].CreateInnerID();

            // 복구용 데이터 백업
            CellContainer[position].IsChanged = true;

            m_objDocument.m_objProcessDatabase?.m_objDatabaseSendMessage.SetInsertHistoryCellInput(
                CellContainer[position].GetInnerID(),
                CellContainer[position].GetCellID()
                );
            m_objDocument.m_objProcessDatabase?.m_objDatabaseSendMessage.SetInsertHistoryProcessData(CellContainer[position].Data.DeepClone());
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

            using (var mccMoveLD = MccLogManager.InShuttle.MOVE_X_LD_POS.CreateMccLogProvider())
            {
                if (SetSubCommandStageMove(InShuttleMotorX.ECommand.LoadPosition) == false)
                {
                    return false;
                }
            }
            MccLogManager.InShuttle.IN_WAIT_TIME.BatchWriteStart();
            return true;
        }

        private bool DoProcessScan()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            try
            {
                using (var mccInpsection = MccLogManager.InShuttle.VISION_INSP_PROCESS.CreateMccLogProvider())
                using (var mccInpsect = MccLogManager.Inspect.INSP_PROCESS_1.CreateMccLogProvider())
                {
                    Inspects.ForEach(x => x.SetGrabInformation(CellContainer));
                    MccLogManager.Inspect.VISION_SCAN_BEFORE_WAIT_TIME.BatchWriteStart();
                    var taskInspectScanBeforeWait = SetSubCommandInspectScanBeforeWait();
                    MotorStageX.SetMccLogItem(MccLogManager.InShuttle.MOVE_X_LD_TO_INSP_START_POS);
                    MotorStageX.SetCommand(InShuttleMotorX.ECommand.ScanStartWaitPosition);
                    Task.WaitAll(taskInspectScanBeforeWait);
                    MccLogManager.Inspect.VISION_SCAN_BEFORE_WAIT_TIME.BatchWriteEnd();
                    if (MotorStageX.WaitForEndProcess() == false)
                    {
                        return false;
                    }
                    if (taskInspectScanBeforeWait.Result == false)
                    {
                        return false;
                    }
                    mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_STANDBY_SCAN_END_TIME].SetTime();

                    mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_SCAN_START_TIME].SetTime();
                    MotorStageX.SetMccLogItem(MccLogManager.InShuttle.MOVE_X_INSP_START_TO_ALIGN_POS);
                    MccLogManager.Inspect.VISION_SCAN.BatchWriteStart();
                    MotorStageX.SetCommand(InShuttleMotorX.ECommand.Scan);
                    if (MotorStageX.WaitForCommandSync(Constants.PASS_BY_SCAN_TRIGGER_END_POSITION) == false)
                    {
                        return false;
                    }
                    MccLogManager.Inspect.VISION_SCAN.BatchWriteEnd();
                    Inspects.ForEach(x =>
                    {
                        if (x.IsPrimary() == true)
                        {
                            x.SetMccLogItem(MccLogManager.Inspect.VISION_SCAN_AFTER_WAIT_TIME);
                        }
                        x.SetCommand(InShuttleInspect.ECommand.GrabEnd);
                    });
                    if (Inspects.WaitForEndProcess() == false
                        || MotorStageX.WaitForEndProcess() == false
                        )
                    {
                        return false;
                    }
                }
                foreach (var cellDataHandler in CellContainer)
                {
                    cellDataHandler.Data.Cell.SubOpticsInspStatus = EStatus.Done;
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

        private bool DoProcessAlignWait()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            if (SetSubCommandStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
            {
                return false;
            }
            return true;
        }

        private bool DoProcessAlign()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            using (var mccAlignVisionProcess = MccLogManager.InShuttle.VISION_ALIGN_PROCESS.CreateMccLogProvider())
            {
                // ! 얼라인 실패 후 재시도시 문제가 발생하기 때문에 TotalRevision 값을 초기화 해야함
                foreach (var cellDataHandler in CellContainer.GetExistCellList())
                {
                    var alignResult = AlignVision.GetAlignResultDataFromCellData(cellDataHandler);
                    alignResult.TotalRevisionX = alignResult.TotalRevisionY = alignResult.TotalRevisionT = 0d;
                }
                AlignVision.SetAlignInformation(CellContainer);
                AlignVision.SetCommand(InShuttleAlignVision.ECommand.Align);
                if (AlignVision.WaitForEndProcess() == false)
                {
                    return false;
                }
                AlignVision.ApplyAlignResultDataToCellData(CellContainer);

                foreach (var cellDataHandler in CellContainer.GetExistCellList())
                {
                    var alignResult = AlignVision.GetAlignResultDataFromCellData(cellDataHandler);
                    // ! 이 설비는 H/W 적으로 Align Retry가 불가능한 구조임으로 기능 구현 안함
                    alignResult.RetryCount = 0;
                    cellDataHandler.Data.Cell.PreAlignStatus = EStatus.Done;
                    cellDataHandler.IsChanged = true;
                    m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetUpdateHistoryProcessData(cellDataHandler.Data.DeepClone());
                }
            }

            return true;
        }

        private bool DoProcessUnload()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // Update TactTime Log Information
            mLastInnerID = CellContainer.Select(i => i.GetInnerID()).ToArray();
            mLastCellID = CellContainer.Select(i => i.GetCellID()).ToArray();

            using (var mccAlignMoveULD = MccLogManager.InShuttle.MOVE_X_ALIGN_TO_ULD_POS.CreateMccLogProvider())
            {
                if (SetSubCommandStageMove(InShuttleMotorX.ECommand.UnloadPosition) == false)
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
                using (var mccInitMotor = MccLogManager.InShuttle.INIT_UNIT.CreateMccLogProvider())
                {
                    if (MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), CDefine.ORIGIN_POSITION_NO) == false)
                    {
                        return false;
                    }
                    MotorStageX.SetCommand(InShuttleMotorX.ECommand.Home);
                    if (MotorStageX.WaitForEndProcess() == false)
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
                MotorStageX.Axis.HLMoveStop();
            }
        }

        private bool DoProcessRestart()
        {
            AlignVision.SetCommand(InShuttleAlignVision.ECommand.CheckExistModel);
            if (AlignVision.WaitForEndProcess() == false)
            {
                return false;
            }

            AlignVision.TaskLightTemperatureRequest();
            AlignVision.TaskLightLevelRequest();

            if (MotorStageX.IsNeedOrigin == true
                && MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), CDefine.ORIGIN_POSITION_NO) == false
                )
            {
                return false;
            }
            MotorStageX.SetCommand(MotorStageX.IsNeedOrigin ? InShuttleMotorX.ECommand.Home : InShuttleMotorX.ECommand.WaitInterlock);
            if (MotorStageX.WaitForEndProcess() == false)
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
                    && mbStartLoadInterface == false
                    )
                {
                    if (mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_START_TIME].IsEmpty() == false
                        && mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_END_TIME].IsEmpty() == true
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
                if (IsInShuttle(sequencePointer) == false)
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
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOAD_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOAD_START_TIME].SetTime();
                        if (DoProcessLoad() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOAD_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.CYCLE_END_TIME].SetTime();
                            SetWriteCycleTactTimeLog();
                            IsCompleteWriteCycleLog = true;
                            mCycleTactTime[CDefine.ECycleTactInShuttle.CYCLE_START_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOADING_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOAD_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOAD_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        SyncTact.Unlock();
                        break;

                    case EAutoSequence.LoadInterface:
                        MccLogManager.InShuttle.IN_WAIT_TIME.BatchWriteEnd();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOADING_START_TIME].SetTime();
                        mbStartLoadInterface = true;
                        break;

                    case EAutoSequence.LoadInterfaceFinish:
                        mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_SCAN_START_TIME].SetTime();
                        mbStartLoadInterface = false;
                        break;

                    case EAutoSequence.Scan:
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_SCAN_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_STANDBY_SCAN_START_TIME].SetTime();
                        if (DoProcessScan() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_SCAN_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_ALIGN_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_SCAN_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_STANDBY_SCAN_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_STANDBY_SCAN_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_SCAN_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.AlignWait:
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_ALIGN_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_ALIGN_START_TIME].SetTime();
                        if (DoProcessAlignWait() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_ALIGN_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_GRAB_ALIGN_START_TIME].SetTime();
                        }
                        else
                        {
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.Align:
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_GRAB_ALIGN_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.GRAB_ALIGN_START_TIME].SetTime();
                        if (DoProcessAlign() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.GRAB_ALIGN_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOAD_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_ALIGN_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_ALIGN_START_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_ALIGN_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.GRAB_ALIGN_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.Unload:
                        SyncTact.WaitingUnlock();
                        SyncTact.Lock();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOAD_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOAD_START_TIME].SetTime();
                        if (DoProcessUnload() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOAD_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_START_TIME].SetTime();
                            MccLogManager.InShuttle.OUT_WAIT_TIME.BatchWriteStartExist();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOAD_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOAD_START_TIME].Clear();
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

        private void ThreadMfqProcess()
        {
            // MFQ 모드가 아니면 스레드를 바로 종료함
            while (
                mbThreadExit == false
                && m_objDocument.IsManualInputMode == true
                )
            {
                Thread.Sleep(10);
                if (
                    m_objDocument.GetRunStatus() == CDefine.ERunStatus.Stopping
                    && m_objDocument.GetRunMode() == CDefine.ERunMode.RealRun
                    && CellContainer.IsCellExistFromList() == false
                    && MotorStageX.GetStatus() == InShuttleMotorX.EStatus.LoadPosition
                    )
                {
                    Array.ForEach(Pickers, picker => picker.Vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE));
                }

                if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                {
                    // In Shuttle에 셀 데이터가 하나라도 있다
                    if (true == CellContainer.IsCellExistFromList())
                    {
                        continue;
                    }
                    // In Shuttle이 LOAD_POSITION이 아니다
                    if (InShuttleMotorX.EStatus.LoadPosition != MotorStageX.GetStatus())
                    {
                        continue;
                    }

                    // MFQ 투입 버튼 누름 감시
                    bool bMfqInsertButtonPressed = default(bool);
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_X054_RESERVED, ref bMfqInsertButtonPressed);
                    if (true == bMfqInsertButtonPressed)
                    {
                        if (Pickers.All(i => i.IsCellDetectSensor != true))
                        {
                            continue;
                        }

                        Array.ForEach(Pickers, picker => picker.Vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_ON, CVacuumAbstract.ESensorCheck.IGNORE));
                        Thread.Sleep(400);

                        // 베큠 체크 (실물이 감지되어 있는지 체크한다)
                        if (Pickers.All(i => i.Vacuum.Status != CVacuumAbstract.EVacuumStatus.STS_ON))
                        {
                            continue;
                        }

                        //Tact Time 처리
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOADING_START_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_ALIGN_START_TIME].SetTime();
                        MccLogManager.InShuttle.IN_WAIT_TIME.BatchWriteEnd();
                        // 셀 정보 생성
                        LockOneCycle();
                        CreateCellData(0);
                        CreateCellData(1);
                        Interlocked.Increment(ref m_objDocument.MfqInputCount);
                        Interlocked.Increment(ref m_objDocument.MfqInputCount);
                        UnlockOneCycle();
                    }
                }
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
                mCycleTactTime[CDefine.ECycleTactInShuttle.INNER_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[0]) ? " " : mLastInnerID[0];
                mCycleTactTime[CDefine.ECycleTactInShuttle.INNER_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[1]) ? " " : mLastInnerID[1];

                // CELL ID
                mCycleTactTime[CDefine.ECycleTactInShuttle.CELL_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[0]) ? " " : mLastCellID[0];
                mCycleTactTime[CDefine.ECycleTactInShuttle.CELL_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[1]) ? " " : mLastCellID[1];

                // USER_STOP_TIME [WAIT]
                unitTact.UserStopTime = mUserStopTimeStopwatch.Elapsed;
                userStopTime = unitTact.UserStopTime.TotalSeconds;
                mUserStopTimeStopwatch.Reset();
                mCycleTactTime[CDefine.ECycleTactInShuttle.USER_STOP_TIME].m_strTime = userStopTime.ToString();

                // PURE CYCLE TACT [TACT] - 유닛 사이클 택타임에서 인터페이스 대기시간을 뺀 시간
                tactTime = m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInShuttle.CYCLE_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInShuttle.CYCLE_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOADING_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_END_TIME].GetTime());
                tactTime -= userStopTime;
                mCycleTactTime[CDefine.ECycleTactInShuttle.PURE_CYCLE_TACT].m_strTime = tactTime.ToString();

                // BEFORE_CYCLE_START ~ CYCLE_START [MOVE] 
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.CYCLE_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.CYCLE_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.CYCLE_TACT_TIME].m_strTime = tactTime.ToString();
                unitTact.UnitTactTime = TimeSpan.FromSeconds(tactTime);
                cycleTactTime = tactTime;

                // IN-SHUTTLE LOADING FROM UPPER-EQ [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOADING].m_strTime = waitingTime.ToString();
                loadingWaitTime += waitingTime;
                unitTact.InputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // IN-SHUTTLE LOADING FROM UPPER-EQ [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOADING].m_strTime = tactTime.ToString();

                // INSP-STAGE STANDBY SCAN [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_SCAN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_SCAN_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_SCAN].m_strTime = waitingTime.ToString();

                // INSP-STAGE STANDBY SCAN [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_STANDBY_SCAN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_STANDBY_SCAN_END_TIME].GetTime();
                grabTime += m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_STANDBY_SCAN].m_strTime = grabTime.ToString();

                // INSP-STAGE SCAN [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_SCAN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_SCAN_END_TIME].GetTime();
                grabTime += m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_SCAN].m_strTime = grabTime.ToString();

                // IN-SHUTTLE ALIGN [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_ALIGN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_ALIGN_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_ALIGN].m_strTime = waitingTime.ToString();

                // IN-SHUTTLE ALIGN [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_ALIGN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_ALIGN_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_ALIGN].m_strTime = tactTime.ToString();

                // IN-SHUTTLE ALIGN [GRAB WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_GRAB_ALIGN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_GRAB_ALIGN_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_GRAB_ALIGN].m_strTime = waitingTime.ToString();

                // IN-SHUTTLE ALIGN [GRAB]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.GRAB_ALIGN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.GRAB_ALIGN_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.GRAB_ALIGN].m_strTime = tactTime.ToString();

                // IN-SHUTTLE UNLOAD TO IN-ROBOT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOAD_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOAD].m_strTime = waitingTime.ToString();

                // IN-SHUTTLE UNLOAD TO IN-ROBOT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOAD_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOAD].m_strTime = tactTime.ToString();

                // IN-SHUTTLE UNLOADING TO IN-ROBOT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_UNLOADING].m_strTime = waitingTime.ToString();
                unloadingWaitTime += waitingTime;
                unitTact.OutputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // IN-SHUTTLE UNLOADING TO IN-ROBOT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_UNLOADING].m_strTime = tactTime.ToString();

                // IN-SHUTTLE LOAD FROM UPPER-EQ [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOAD_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.WAIT_LOAD].m_strTime = waitingTime.ToString();

                // IN-SHUTTLE LOAD FROM ZONE-TRANSFER [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOAD_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactInShuttle.MOVE_LOAD].m_strTime = tactTime.ToString();

                mCycleTactTime[CDefine.ECycleTactInShuttle.RAWDATA].m_strTime = " ";
            }

            // Write Log
            {
                Dictionary<string, string> tactTimeDetail = new Dictionary<string, string>();
                var sbLog = new StringBuilder();
                bool bIncomplete = false;
                ETactTime tactTimeIndex = ETactTime.IN_SHUTTLE;
                CDefine.ELogType eLogType = CDefine.ELogType.LOG_CYCLE_TACT_01_IN_SHUTTLE;
                foreach (CDefine.ECycleTactInShuttle column in Enum.GetValues(typeof(CDefine.ECycleTactInShuttle)))
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
                tact.UnitTacts[mDefine.ProcessIndex] = unitTact;
                m_objDocument.m_objTactTime.SetOutputTactTime(tact);
            }
        }
    }
}
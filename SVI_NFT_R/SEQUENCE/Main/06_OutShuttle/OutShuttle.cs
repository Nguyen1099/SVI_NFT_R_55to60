using EqToEq;
using Mcc;
using SVI_NFT_R.CellData;
using SVI_NFT_R.UI.Dialog;
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
    public partial class OutShuttle : CProcessAbstract, IInputUnitNode
    {
        public IReadOnlyList<CellDataHandler> CellContainer { get; private set; }
        public OutShuttlePicker[] Pickers { get; private set; }
        public OutShuttleMotorX MotorStageX { get; private set; }
        public Utils.SyncToken SyncTact { get; private set; } = new Utils.SyncToken();
        public bool CanInput { get; private set; } = true;
        public bool IsIdle => (
            Pickers.All(item => item.GetCommand() == 0) // == Idle
            && MotorStageX.GetCommand() == 0
            && IsRunningSequence == false
            && mDialogNotificationOrNull == null
            );
        public bool IsEmpty => CellContainer.GetExistCellCount() == 0;
        public IReadOnlyDictionary<CDefine.ECycleTactOutShuttle, CTimeElement> CycleTactTime => mCycleTactTime;
        public bool IsStartUnloadInterface
        {
            get
            {
                return mbStartUnloadInterface;
            }
            set
            {
                mbStartUnloadInterface = value;
            }
        }
        private bool mbStartUnloadInterface;
        private Thread mThreadBatch;
        private Thread mThreadMfq;
        private ManualResetEvent mOneCycleUnlock;
        private ManualResetEvent mOneCycleEnd;
        private Dictionary<CDefine.ECycleTactOutShuttle, CTimeElement> mCycleTactTime;
        private string[] mLastInnerID;
        private string[] mLastCellID;
        private readonly Stopwatch mUserStopTimeStopwatch = Stopwatch.StartNew();
        private CDialogNotification mDialogNotificationOrNull;

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

                mCycleTactTime = m_objDocument.m_objCycleTactTime.m_tactOutShuttle.dicTactTime;

                mLastInnerID = new string[CellContainer.Count];
                mLastCellID = new string[CellContainer.Count];

                // 모터 객체 생성
                MotorStageX = new OutShuttleMotorX();
                if (MotorStageX.Initialize(m_objDocument, mDefine.StageMotorPosition) == false)
                {
                    break;
                }

                // 피커 객체 생성
                Pickers = new OutShuttlePicker[mDefine.PickerPositions.Length];
                for (int i = 0; i < Pickers.Length; ++i)
                {
                    Pickers[i] = new OutShuttlePicker();
                    if (Pickers[i].Initialize(m_objDocument, mDefine.PickerPositions[i]) == false)
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

                // kiểm tra lại logic sau
                //Task.Run(() =>
                //{
                //    SpinWait.SpinUntil(() => m_objDocument.IsInitialized);

                //    var unloadInterface = m_objDocument.m_objProcessMain.m_objProcessMotion.UnLoadInterface as IPassiveVacuumAction;
                //    Debug.Assert(unloadInterface != null, $"{nameof(IPassiveVacuumAction)} 기능 미구현!!!");
                //    unloadInterface.ReceiveActiveVacuumRequest += UnLoadInterface_ReceiveActiveVacuumRequest;
                //});

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
            foreach (var item in Pickers)
            {
                item.DeInitialize();
            }
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

        //public void UnLoadInterface_ReceiveActiveVacuumRequest(object sender, ReceiveVacuumEventArgs e)
        //{
        //    if (e.Placement.HasFlag(EPosition.P1))
        //    {
        //        var picker = m_objDocument.m_objProcessMain.m_objProcessMotion.OutShuttle.Pickers[0];
        //        MccLogManager.OutShuttle.COMPONENT_IN[0].WriteStart();
        //        picker.SetMccLogItem(MccLogManager.OutShuttle.GET_VAC_ON[0]);
        //        picker.SetCommand(OutShuttlePicker.ECommand.Pick);
        //    }
        //    if (e.Placement.HasFlag(EPosition.P2))
        //    {
        //        var picker = m_objDocument.m_objProcessMain.m_objProcessMotion.OutShuttle.Pickers[1];
        //        MccLogManager.OutShuttle.COMPONENT_IN[1].WriteStart();
        //        picker.SetMccLogItem(MccLogManager.OutShuttle.GET_VAC_ON[1]);
        //        picker.SetCommand(OutShuttlePicker.ECommand.Pick);
        //    }
        //}

        public void UnLoadInterface_ReceiveBlowVacuumRequest(object sender, ReceiveVacuumEventArgs e)
        {
            if (e.Placement.HasFlag(EPosition.P1))
            {
                var picker = m_objDocument.m_objProcessMain.m_objProcessMotion.OutShuttle.Pickers[0];
                MccLogManager.OutShuttle.COMPONENT_IN[0].WriteStart();
                picker.SetMccLogItem(MccLogManager.OutShuttle.GET_VAC_ON[0]);
                picker.SetCommand(OutShuttlePicker.ECommand.Pick);
            }
            if (e.Placement.HasFlag(EPosition.P2))
            {
                var picker = m_objDocument.m_objProcessMain.m_objProcessMotion.OutShuttle.Pickers[1];
                MccLogManager.OutShuttle.COMPONENT_IN[1].WriteStart();
                picker.SetMccLogItem(MccLogManager.OutShuttle.GET_VAC_ON[1]);
                picker.SetCommand(OutShuttlePicker.ECommand.Pick);
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

            using (var mccMoveLD = MccLogManager.OutShuttle.MOVE_X_LD_POS.CreateMccLogProvider())
            {
                if (SetSubCommandStageMove(OutShuttleMotorX.ECommand.LoadPosition) == false)
                {
                    return false;
                }
            }
            MccLogManager.OutShuttle.IN_WAIT_TIME.BatchWriteStart();
            return true;
        }

        private bool DoProcessUnloadWait()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.Check()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // ! Không ghi nhật ký MCC nếu đã đến vị trí UNLOAD 
            using (var mccMoveULD = MotorStageX.IsInposition(OutShuttleMotorX.ECommand.UnloadPosition) == true ? null
                : MccLogManager.OutShuttle.MOVE_X_ULD_POS.CreateMccLogProvider())
            {
                if (SetSubCommandStageMove(OutShuttleMotorX.ECommand.UnloadPosition) == false)
                {
                    return false;
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

            using (var mccMoveULD = MccLogManager.OutShuttle.MOVE_X_ULD_POS.CreateMccLogProvider())
            {
                if (SetSubCommandStageMove(OutShuttleMotorX.ECommand.UnloadPosition) == false)
                {
                    return false;
                }
            }
            MccLogManager.OutShuttle.OUT_WAIT_TIME.BatchWriteStart();
            return true;
        }

        private bool DoProcessInitialize()
        {
            try
            {
                using (var mccInitMotor = MccLogManager.OutShuttle.INIT_UNIT.CreateMccLogProvider())
                {
                    if (MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), CDefine.ORIGIN_POSITION_NO) == false)
                    {
                        return false;
                    }
                    MotorStageX.SetCommand(OutShuttleMotorX.ECommand.Home);
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
            if (MotorStageX.IsNeedOrigin == true
                && MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), CDefine.ORIGIN_POSITION_NO) == false
                )
            {
                return false;
            }
            MotorStageX.SetCommand(MotorStageX.IsNeedOrigin ? OutShuttleMotorX.ECommand.Home : OutShuttleMotorX.ECommand.WaitInterlock);
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
                    && m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface.IsHandShaking == false
                    && mDialogNotificationOrNull == null
                    )
                {
                    if (mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_START_TIME].IsEmpty() == false
                        && mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_END_TIME].IsEmpty() == true
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
                if (IsOutShuttle(sequencePointer) == false)
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
                        //
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOAD_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOAD_START_TIME].SetTime();

                        if (DoProcessLoad() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOAD_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.CYCLE_END_TIME].SetTime();
                            SetWriteCycleTactTimeLog();
                            IsCompleteWriteCycleLog = true;
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.CYCLE_START_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOADING_START_TIME].SetTime();
                        }
                        else
                        {
                            // xoá dữ liệu tact time khi load thất bại
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOAD_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOAD_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        SyncTact.Unlock();
                        break;

                    case EAutoSequence.UnloadWait:
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOAD_READY_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOAD_READY_START_TIME].SetTime();

                        if (DoProcessUnloadWait() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOAD_READY_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOAD_READY_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOAD_READY_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.UnloadInterface:
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOADING_START_TIME].SetTime();

                        mbStartUnloadInterface = true;
                        if (DoProcessUnload() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOADING_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOAD_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOADING_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.UnloadInterfaceFinish:
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOAD_END_TIME].SetTime();
                        mbStartUnloadInterface = false;
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
                    && MotorStageX.GetStatus() == OutShuttleMotorX.EStatus.LoadPosition
                    )
                {
                    Array.ForEach(Pickers, picker => picker.Vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE));
                }

                if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                {
                    // Out Shuttle에 셀 데이터가 하나라도 있다
                    if (true == CellContainer.IsCellExistFromList())
                    {
                        continue;
                    }
                    // Out Shuttle이 LOAD_POSITION이 아니다
                    if (OutShuttleMotorX.EStatus.LoadPosition != MotorStageX.GetStatus())
                    {
                        continue;
                    }

                    // MFQ 투입 버튼 누름 감시
                    bool bMfqInsertButtonPressed = default(bool);
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_X055_RESERVED, ref bMfqInsertButtonPressed);
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
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOADING_START_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOADING_END_TIME].SetTime();
                        MccLogManager.OutShuttle.OUT_WAIT_TIME.BatchWriteEnd();
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
            double cycleTactTime, loadingWaitTime, unloadingWaitTime, userStopTime;

            // Calculate Wait Time
            {
                DateTime startTime, endTime;
                double waitingTime, tactTime;
                loadingWaitTime = unloadingWaitTime = 0;
                unitTact.InputDelayTime = unitTact.OutputDelayTime = TimeSpan.Zero;

                // INNER ID
                mCycleTactTime[CDefine.ECycleTactOutShuttle.INNER_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[0]) ? " " : mLastInnerID[0];
                mCycleTactTime[CDefine.ECycleTactOutShuttle.INNER_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[1]) ? " " : mLastInnerID[1];

                // CELL ID
                mCycleTactTime[CDefine.ECycleTactOutShuttle.CELL_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[0]) ? " " : mLastCellID[0];
                mCycleTactTime[CDefine.ECycleTactOutShuttle.CELL_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[1]) ? " " : mLastCellID[1];

                // USER_STOP_TIME [WAIT]
                unitTact.UserStopTime = mUserStopTimeStopwatch.Elapsed;
                userStopTime = unitTact.UserStopTime.TotalSeconds;
                mUserStopTimeStopwatch.Reset();
                mCycleTactTime[CDefine.ECycleTactOutShuttle.USER_STOP_TIME].m_strTime = userStopTime.ToString();

                // PURE CYCLE TACT [TACT] - 유닛 사이클 택타임에서 인터페이스 대기시간을 뺀 시간
                tactTime = m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutShuttle.CYCLE_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutShuttle.CYCLE_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOADING_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_END_TIME].GetTime());
                tactTime -= userStopTime;
                mCycleTactTime[CDefine.ECycleTactOutShuttle.PURE_CYCLE_TACT].m_strTime = tactTime.ToString();

                // BEFORE_CYCLE_START ~ CYCLE_START [MOVE] 
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.CYCLE_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.CYCLE_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.CYCLE_TACT_TIME].m_strTime = tactTime.ToString();
                unitTact.UnitTactTime = TimeSpan.FromSeconds(tactTime);
                cycleTactTime = tactTime;

                // OUT-SHUTTLE LOADING FROM OUTFLIP [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOADING].m_strTime = waitingTime.ToString();
                loadingWaitTime += waitingTime;
                unitTact.InputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // OUT-SHUTTLE LOADING FROM OUTFLIP [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOADING].m_strTime = tactTime.ToString();

                // OUT-SHUTTLE UNLOAD FROM LOWER-EQ [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOAD_READY_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOAD_READY_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOAD_READY].m_strTime = waitingTime.ToString();

                // OUT-SHUTTLE UNLOAD FROM LOWER-EQ [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOAD_READY_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOAD_READY_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOAD_READY].m_strTime = tactTime.ToString();

                // OUT-SHUTTLE UNLOADING FROM LOWER-EQ [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_UNLOADING].m_strTime = waitingTime.ToString();

                // OUT-SHUTTLE LOAD FROM ZONE-TRANSFER [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_UNLOADING].m_strTime = tactTime.ToString();

                // OUT-SHUTTLE LOAD TO OUT-FLIP [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOAD_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.WAIT_LOAD].m_strTime = waitingTime.ToString();

                // OUT-SHUTTLE LOAD TO OUT-FLIP [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOAD_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOAD_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutShuttle.MOVE_LOAD].m_strTime = tactTime.ToString();

                mCycleTactTime[CDefine.ECycleTactOutShuttle.RAWDATA].m_strTime = " ";
            }

            // Write Log
            {
                Dictionary<string, string> tactTimeDetail = new Dictionary<string, string>();
                var sbLog = new StringBuilder();
                bool bIncomplete = false;
                ETactTime tactTimeIndex = ETactTime.OUT_SHUTTLE;
                CDefine.ELogType eLogType = CDefine.ELogType.LOG_CYCLE_TACT_06_OUT_SHUTTLE;
                foreach (CDefine.ECycleTactOutShuttle column in Enum.GetValues(typeof(CDefine.ECycleTactOutShuttle)))
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
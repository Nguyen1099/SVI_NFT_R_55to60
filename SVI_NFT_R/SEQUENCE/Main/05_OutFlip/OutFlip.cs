using Mcc;
using SVI_NFT_R.CellData;
using SVI_NFT_R.EHS;
using SVI_NFT_R.UI.Dialog;
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
    public partial class OutFlip : CProcessAbstract, IUnitNode
    {
        public IReadOnlyList<CellDataHandler> CellContainer { get; private set; }
        //private Thread mThreadOutConveyor;
        public OutFlipPicker[] Pickers { get; private set; }
        public OutFlipSensor Sensor { get; private set; }
        public OutFlipMotorR[] MotorRs { get; private set; }
        public OutFlipMotorZ MotorZ { get; private set; }
        //public OutFlipMotorX MotorConveyorX { get; private set; }
        public Utils.SyncToken SyncTact { get; private set; } = new Utils.SyncToken();
        public bool CanInput { get; private set; } = true;
        public bool IsUnloadOutFlipToOutConveyor { get { return mbUnloadOutFlipToOutConveyor; } }
        private bool mbUnloadOutFlipToOutConveyor = false;
        public bool IsOutConveyorAutoMode { get; set; }
        public bool IsIdle => (
            Pickers.All(item => item.GetCommand() == 0) // == Idle
            && MotorRs.All(item => item.GetCommand() == 0)
            && MotorZ.GetCommand() == 0
            && Sensor.GetCommand() == 0
            //&& MotorConveyorX.GetCommand() == 0
            && IsRunningSequence == false
            && mDialogNotificationOrNull == null
            );
        public bool IsEmpty => CellContainer.GetExistCellCount() == 0;
        public bool IsUnloadPending => (
            IsEmpty == false
            && IsWaitUnloadPosition == true
            && m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot.Nachi.IsUnloadInterlock() == false
            //&& MotorConveyorX.IsArrivalUnloadPosition() == false
            );
        public bool IsWaitUnloadPosition => MotorZ.GetStatus() == OutFlipMotorZ.EStatus.UpPosition
                && MotorRs.All(motor => motor.GetStatus() == OutFlipMotorR.EStatus.UnloadPosition);
        public IReadOnlyDictionary<CDefine.ECycleTactOutFlip, CTimeElement> CycleTactTime => mCycleTactTime;
        private Thread mThreadBatch;
        private ManualResetEvent mOneCycleUnlock;
        private ManualResetEvent mOneCycleEnd;
        private Dictionary<CDefine.ECycleTactOutFlip, CTimeElement> mCycleTactTime;
        private string[] mLastInnerID;
        private string[] mLastCellID;
        private bool[] mConveyorCellExist;
        private bool mConveyorRun = true;
        private readonly Stopwatch mUserStopTimeStopwatch = Stopwatch.StartNew();
        private CDialogNotification mDialogNotificationOrNull;
        private CAlarmDefine.EMessageList mNotificationMessage = 0;
        public TimeSpan CellOutputDelayTimeForFdcData { get; private set; }
        private DateTime mCellOutputDelayStartTimeForFdcData = DateTime.MinValue;
        private DateTime mLastPendingDateTime = DateTime.Now;
        private static readonly string REGISTRY_PATH = Path.Combine("ENC", Program.ID, "SEQUENCE", nameof(OutFlip));
        private readonly Settings.RegistryComponent mBackupFdcData = new Settings.RegistryComponent(REGISTRY_PATH);
        private ECellPlacement mFullCellPlacement = 0;

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
                    mFullCellPlacement |= cellDataHandler.CellPlacement;
                }

                mCycleTactTime = m_objDocument.m_objCycleTactTime.m_TactOutFlip.dicTactTime;

                mLastInnerID = new string[CellContainer.Count];
                mLastCellID = new string[CellContainer.Count];
                mConveyorCellExist = new bool[CellContainer.Count];

                // Restore FDC Data 
                {
                    CellOutputDelayTimeForFdcData = TimeSpan.FromTicks(Convert.ToInt64(mBackupFdcData.GetValue($"{mDefine.ProcessIndex}.{nameof(CellOutputDelayTimeForFdcData)}", 0L)));
                    mCellOutputDelayStartTimeForFdcData = DateTime.FromBinary(Convert.ToInt64(mBackupFdcData.GetValue($"{mDefine.ProcessIndex}.{nameof(mCellOutputDelayStartTimeForFdcData)}", DateTime.MinValue.ToBinary())));
                }

                // 모터 객체 생성
                //MotorConveyorX = new OutFlipMotorX();
                //if (MotorConveyorX.Initialize(m_objDocument, mDefine.MotorPosition) == false)
                //{
                //    break;
                //}

                // 피커 객체 생성
                Pickers = new OutFlipPicker[mDefine.PickerPositions.Length];
                for (int i = 0; i < Pickers.Length; ++i)
                {
                    Pickers[i] = new OutFlipPicker();
                    if (Pickers[i].Initialize(m_objDocument, mDefine.PickerPositions[i]) == false)
                    {
                        break;
                    }
                }

                // 모터 객체 생성
                MotorRs = new OutFlipMotorR[mDefine.TurnMotorPositions.Length];
                for (int i = 0; i < MotorRs.Length; ++i)
                {
                    MotorRs[i] = new OutFlipMotorR();
                    if (MotorRs[i].Initialize(m_objDocument, mDefine.TurnMotorPositions[i]) == false)
                    {
                        break;
                    }
                }
                MotorZ = new OutFlipMotorZ();
                if (MotorZ.Initialize(m_objDocument, mDefine.DownMotorPosition) == false)
                {
                    break;
                }

                // 센서 객체 생성
                Sensor = new OutFlipSensor();
                if (Sensor.Initialize(m_objDocument, mDefine.SensorPositon) == false)
                {
                    break;
                }

                // 프로세스 동기화 객체 초기화
                mOneCycleUnlock = new ManualResetEvent(true);
                mOneCycleEnd = new ManualResetEvent(true);

                // 프로세스 쓰레드 생성
                mThreadProcess = new Thread(ThreadProcess);
                mThreadProcess.Start();

                // 아웃 컨베이어 쓰레드 생성
                //mThreadOutConveyor = new Thread(ThreadOutConveyorProcess);
                //mThreadOutConveyor.Start();

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
          //  mThreadOutConveyor.Join();
            mThreadProcess.Join();

            mOneCycleUnlock.Dispose();
            mOneCycleEnd.Dispose();

            foreach (var item in MotorRs)
            {
                item.DeInitialize();
            }
            MotorZ.DeInitialize();
            Sensor.DeInitialize();
            foreach (var item in Pickers)
            {
                item.DeInitialize();
            }
            //MotorConveyorX.DeInitialize();
        }

        public void SetCellOutputDelayStartForFdc()
        {
            mCellOutputDelayStartTimeForFdcData = DateTime.UtcNow;
            // Backup FDC Data
            mBackupFdcData.SetValue($"{mDefine.ProcessIndex}.{nameof(mCellOutputDelayStartTimeForFdcData)}", mCellOutputDelayStartTimeForFdcData.ToBinary());
        }

        public void SetCellOutputDelayEndForFdc()
        {
            CellOutputDelayTimeForFdcData = DateTime.UtcNow - mCellOutputDelayStartTimeForFdcData;
            // Backup FDC Data
            mBackupFdcData.SetValue($"{mDefine.ProcessIndex}.{nameof(CellOutputDelayTimeForFdcData)}", CellOutputDelayTimeForFdcData.Ticks);
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
            // ! 이 설비는 2Ch이지만 Picker가 4개로 구성된 특수한 경우로 cellCount와 Picker 수를 비교해서 체크하지 않음
            if (cellPlacement == mFullCellPlacement)
            {
                cellPlacement |= ECellPlacement.Full;
            }
            return true;
        }

        private bool DoProcessLoadWait()
        {
            Task<bool>[] cellCheckTasks = Pickers.Select(i => Task.Run(() => i.CheckOverlap()))
                .ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // 투입 대기 위치 이동
            using (MccLogManager.OutFlip.MOVE_LD_POS.CreateMccLogProvider())
            {
                MotorZ.SetCommand(OutFlipMotorZ.ECommand.UpPosition);
                if (MotorZ.WaitForEndProcess() == false)
                {
                    return false;
                }
                MotorRs.ForEach(motor => motor.SetCommand(OutFlipMotorR.ECommand.LoadPosition));
                if (MotorRs.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            MccLogManager.OutFlip.IN_WAIT_TIME.BatchWriteStart();
            return true;
        }

        private bool DoProcessUnloadWait()
        {
            var cellCheckTasks = Pickers.Where(picker => picker.CellPort.IsCellExist() == true)
                .Select(picker =>
                {
                    var position = picker.CellPort.PositionIndex;
                    var status = picker.CellPort.Data.Cell.CylinderStatus;
                    OutFlipPicker target = null;
                    switch (status)
                    {
                        case ECylinderStatus.Return:
                            target = (position == 0) ? Pickers[1] : (position == 1) ? Pickers[3] : null;
                            break;
                        case ECylinderStatus.Turn:
                            target = (position == 0) ? Pickers[0] : (position == 1) ? Pickers[2] : null;
                            break;
                        default:
                            Debug.Assert(false);
                            target = null;
                            break;
                    }
                    return target;
                }).Where(t => t != null).Distinct().Select(i => Task.Run(() => i.Check())).ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // 배출 대기 위치 이동
            using (MccLogManager.OutFlip.MOVE_ULD_WAIT_POS.CreateMccLogProvider())
            {
                MotorZ.SetCommand(OutFlipMotorZ.ECommand.UpPosition);
                if (MotorZ.WaitForEndProcess() == false)
                {
                    return false;
                }
                MotorRs.ForEach(motor => motor.SetCommand(OutFlipMotorR.ECommand.UnloadPosition));
                if (MotorRs.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            return true;
        }

        private bool DoProcessShowNotification()
        {
            mDialogNotificationOrNull = m_objDocument.GetMainFrame().ShowNotificationDialog(mNotificationMessage, "FVI CONVEYOR");
            mDialogNotificationOrNull.IsVisibleTimeShow = true;
            return true;
        }

        private bool DoProcessCloseNotification()
        {
            m_objDocument.GetMainFrame().CloseNotificationDialog(mDialogNotificationOrNull);
            mDialogNotificationOrNull = null;
            return true;
        }

        private bool DoProcessUnload()
        {
            var cellExistPicker = Pickers.Where(picker => picker.CellPort.IsCellExist() == true).ToArray();

            var cellCheckTasks = cellExistPicker.Select(picker =>
                {
                    var position = picker.CellPort.PositionIndex;
                    var status = picker.CellPort.Data.Cell.CylinderStatus;
                    OutFlipPicker target = null;
                    switch (status)
                    {
                        case ECylinderStatus.Return:
                            target = (position == 0) ? Pickers[1] : (position == 1) ? Pickers[3] : null;
                            break;
                        case ECylinderStatus.Turn:
                            target = (position == 0) ? Pickers[0] : (position == 1) ? Pickers[2] : null;
                            break;
                        default:
                            Debug.Assert(false);
                            target = null;
                            break;
                    }
                    return target;
                }).Where(t => t != null).Distinct().Select(i => Task.Run(() => i.Check())).ToArray();
            Task.WaitAll(cellCheckTasks);
            if (cellCheckTasks.All(i => i.Result == true) == false)
            {
                return false;
            }

            // 배출 컨베이어 센서 감지 상태 확인
            Sensor.SetCommand(OutFlipSensor.ECommand.CheckInput);
            if (Sensor.WaitForEndProcess() == false)
            {
                return false;
            }
            Sensor.SetCommand(OutFlipSensor.ECommand.CheckBlocked);
            if (Sensor.WaitForEndProcess() == false)
            {
                return false;
            }
            Sensor.SetCellPlacement(CellDataManager.GetCellPlacementFromProcess(EProcess.OutFlip));

            // Update TactTime Log Information
            mLastInnerID = CellContainer.Select(i => i.GetInnerID()).ToArray();
            mLastCellID = CellContainer.Select(i => i.GetCellID()).ToArray();

            mbUnloadOutFlipToOutConveyor = true;

            try
            {
                MccLogManager.OutFlip.OUT_WAIT_TIME.BatchWriteEndExist();
                MccLogManager.OutFlip.COMPONENT_OUT.BatchWriteStartExist();
                //MccLogManager.OutConveyor.IN_WAIT_TIME.BatchWriteEnd();
                //for (int i = 0; i < CellContainer.Count; i++)
                //{
                //    if (CellContainer[i].IsCellExist() == true)
                //    {
                //        MccLogManager.OutConveyor.COMPONENT_IN[i].WriteStart();
                //    }
                //}
                // 컨베이어 정지 및 ZERO POSITION SETTING
                //MotorConveyorX.SetCommand(OutFlipMotorX.ECommand.PositionZeroSet);
                //if (MotorConveyorX.WaitForEndProcess() == false)
                //{
                //    return false;
                //}
                // 모터 다운 위치 이동
                using (MccLogManager.OutFlip.PUT_MOVE_ULD_POS.CreateMccLogProvider(bShouldWriteExistCell: true))
                {
                    MotorZ.SetCommand(OutFlipMotorZ.ECommand.DownPosition);
                    if (MotorZ.WaitForEndProcess() == false)
                    {
                        return false;
                    }
                }
                using (MccLogManager.OutFlip.PUT_VAC_OFF.CreateMccLogProvider(bShouldWriteExistCell: true))
                {
                    Array.ForEach(cellExistPicker, picker => picker.SetCommand(OutFlipPicker.ECommand.Place));
                    if (cellExistPicker.WaitForEndProcess() == false)
                    {
                        return false;
                    }
                }
                // 모터 업 위치 이동
                using (MccLogManager.OutFlip.PUT_MOVE_ULD_UP_POS.CreateMccLogProvider(bShouldWriteExistCell: true))
                {
                    MotorZ.SetCommand(OutFlipMotorZ.ECommand.UpPosition);
                    if (MotorZ.WaitForEndProcess() == false)
                    {
                        return false;
                    }
                }
                Sensor.SetCommand(OutFlipSensor.ECommand.CheckOutput);
                if (Sensor.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            finally
            {
                // ! 하나라도 파기를 진행한 경우 셀 데이터를 이동함
                if (Pickers.Any(i => i.Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_OFF) == true)
                {
                    // 컨베이어 자재유무 Set
                    for (int i = 0; i < CellContainer.Count; i++)
                    {
                        mConveyorCellExist[i] = CellContainer[i].IsCellExist();
                    }
                    foreach (var cellDataHandler in CellContainer.GetExistCellList())
                    {
                        m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryCellOutput(cellDataHandler.GetInnerID(), cellDataHandler.GetCellID());
                        m_objDocument.m_objProcessMain.m_objProcessMotion.TrackOut.TrackOutRequestPushInQueue(cellDataHandler.Data.DeepClone());
                        m_objDocument.m_objProcessMain.m_objProcessMotion.TrackOut.WaitForEndProcess(1000);
                    }
                    MccLogManager.OutFlip.COMPONENT_OUT.BatchWriteEndExist();
                    //for (int i = 0; i < CellContainer.Count; i++)
                    //{
                    //    if (CellContainer[i].IsCellExist() == true)
                    //    {
                    //        MccLogManager.OutConveyor.COMPONENT_IN[i].WriteEnd();
                    //    }
                    //}
                    foreach (var cellDataHandler in CellContainer.GetExistCellList())
                    {
                        // 셀 데이터 삭제 (MCC 정합성을 위해 내부 정보는 삭제하지 않고 플래그만 변경함. 다음 셀이 들어오면 덮어쓰기됨.)
                        cellDataHandler.Data.IsUse = false;
                        cellDataHandler.IsChanged = true;
                    }
                }
                //if (mConveyorCellExist.Any(x => x) == true)
                //{
                //    MccLogManager.OutConveyor.OUT_WAIT_TIME.BatchWriteStart();
                //}
                //else
                //{
                //    MccLogManager.OutConveyor.IN_WAIT_TIME.BatchWriteStart();
                //}
                // 모터 업 위치 이동
                mbUnloadOutFlipToOutConveyor = false;
            }

            return true;
        }

        //private void DoProcessOutConveyorStop()
        //{
        //    MotorConveyorX.SetConveyorStop();
        //}

        //private void DoProcessOutConveyorRun()
        //{
        //    MotorConveyorX.SetConveyorUnloadMoveStart();
        //    // ! 시뮬레이션 모드에서 MCC 로그가 꼬이는 문제가 있어서 딜레이 추가
        //    if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
        //    {
        //        Thread.Sleep(500);
        //    }
        //}

        private bool DoProcessInitialize()
        {
            // 모터 업 위치 이동
            MotorZ.SetCommand(OutFlipMotorZ.ECommand.Home);
            if (MotorZ.WaitForEndProcess() == false)
            {
                return false;
            }
            MotorZ.SetCommand(OutFlipMotorZ.ECommand.UpPosition);
            if (MotorZ.WaitForEndProcess() == false)
            {
                return false;
            }
            // 모터 리턴 위치 이동
            MotorRs.ForEach(motor => motor.SetCommand(OutFlipMotorR.ECommand.Home));
            if (MotorRs.WaitForEndProcess() == false)
            {
                return false;
            }
            MotorRs.ForEach(motor => motor.SetCommand(OutFlipMotorR.ECommand.LoadPosition));
            if (MotorRs.WaitForEndProcess() == false)
            {
                return false;
            }

            //MotorConveyorX.SetCommand(OutFlipMotorX.ECommand.Home);
            //if (false == MotorConveyorX.WaitForEndProcess())
            //{
            //    return false;
            //}

            IsOutConveyorAutoMode = true;

            return true;
        }

        private bool DoProcessRestart()
        {
            // 모터 업 위치 이동
            MotorZ.SetCommand(MotorZ.IsNeedOrigin == true ? OutFlipMotorZ.ECommand.Home : OutFlipMotorZ.ECommand.WaitInterlock);
            if (MotorZ.WaitForEndProcess() == false)
            {
                return false;
            }
            MotorZ.SetCommand(OutFlipMotorZ.ECommand.UpPosition);
            if (MotorZ.WaitForEndProcess() == false)
            {
                return false;
            }
            // 모터 리턴 위치 이동
            MotorRs.ForEach(motor => motor.SetCommand(motor.IsNeedOrigin == true ? OutFlipMotorR.ECommand.Home : OutFlipMotorR.ECommand.WaitInterlock));
            if (MotorRs.WaitForEndProcess() == false)
            {
                return false;
            }
            MotorRs.ForEach(motor => motor.SetCommand(OutFlipMotorR.ECommand.LoadPosition));
            if (MotorRs.WaitForEndProcess() == false)
            {
                return false;
            }

            //if (MotorConveyorX.IsNeedOrigin == true)
            //{
            //    MotorConveyorX.SetCommand(OutFlipMotorX.ECommand.Home);
            //    if (false == MotorConveyorX.WaitForEndProcess())
            //    {
            //        return false;
            //    }
            //}

            // 모터 인터락 대기 상태 변경
            MotorZ.SetCommand(OutFlipMotorZ.ECommand.WaitInterlock);
            MotorRs.ForEach(motor => motor.SetCommand(OutFlipMotorR.ECommand.WaitInterlock));
            MotorZ.WaitForEndProcess();
            MotorRs.WaitForEndProcess();

            IsOutConveyorAutoMode = true;

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
                    && mDialogNotificationOrNull == null
                    )
                {
                    if (mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_START_TIME].IsEmpty() == false
                        && mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_END_TIME].IsEmpty() == true
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
                if (IsOutFlip(sequencePointer) == false)
                {
                    continue;
                }

                // 시퀀스 동작
                var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
                IsRunningSequence = true;
                if (
                    sequencePointer != EAutoSequence.ShowNotification
                    && sequencePointer != EAutoSequence.CloseNotification
                    )
                {
                    IsCompleteWriteCycleLog = false;
                }
                SetProcessLog($"AutoSequence(Begin) : {sequencePointer}");
                switch (sequencePointer)
                {
                    case EAutoSequence.LoadWait:
                        SyncTact.WaitingUnlock();
                        SyncTact.Lock();
                        mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_RETURN_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_RETURN_START_TIME].SetTime();
                        if (DoProcessLoadWait() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_RETURN_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutFlip.CYCLE_END_TIME].SetTime();
                            SetWriteCycleTactTimeLog();
                            IsCompleteWriteCycleLog = true;
                            mCycleTactTime[CDefine.ECycleTactOutFlip.CYCLE_START_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_LOADING_START_TIME].SetTime();

                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_RETURN_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_RETURN_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        SyncTact.Unlock();
                        break;

                    case EAutoSequence.UnloadWait:
                        mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOAD_READY_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOAD_READY_START_TIME].SetTime();
                        if (DoProcessUnloadWait() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOAD_READY_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_START_TIME].SetTime();
                            MccLogManager.OutFlip.OUT_WAIT_TIME.BatchWriteStartExist();
                            SetCellOutputDelayStartForFdc();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOAD_READY_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOAD_READY_START_TIME].Clear();
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                        break;

                    case EAutoSequence.ShowNotification:
                        DoProcessShowNotification();
                        break;

                    case EAutoSequence.CloseNotification:
                        DoProcessCloseNotification();
                        break;

                    case EAutoSequence.Unload:
                        SyncTact.WaitingUnlock();
                        SyncTact.Lock();
                        mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_END_TIME].SetTime();
                        mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOADING_START_TIME].SetTime();
                        SetCellOutputDelayEndForFdc();
                        if (DoProcessUnload() == true)
                        {
                            mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOADING_END_TIME].SetTime();
                            mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_RETURN_START_TIME].SetTime();
                        }
                        else
                        {
                            mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_END_TIME].Clear();
                            mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOADING_START_TIME].Clear();
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

        //private void ThreadOutConveyorProcess()
        //{
        //    /*
        //     * 내부 배출 컨베이어 제어
        //     */
        //    while (false == mbThreadExit)
        //    {
        //        // 컨베이어 정지
        //        if (true == IsOutConveyor(EOutConveyorSequence.Stop))
        //        {
        //            if (mConveyorRun == true)
        //            {
        //                mConveyorRun = false;
        //                MccLogManager.OutConveyor.CV_RUN.BatchWriteEnd();
        //                if (mConveyorCellExist[1] == true
        //                   // && MotorConveyorX.IsArrivalUnloadPosition() == true
        //                    )
        //                {
        //                    mConveyorCellExist[1] = false;
        //                    Task.Run(async () =>
        //                    {
        //                        using (var logScope = MccLogManager.OutConveyor.COMPONENT_OUT[1].CreateMccLogProvider())
        //                        {
        //                            await Task.Delay(100);
        //                        }
        //                    });
        //                }
        //                if (mConveyorCellExist.Any(x => x) == true)
        //                {
        //                    MccLogManager.OutConveyor.OUT_WAIT_TIME.BatchWriteStart();
        //                }
        //                else
        //                {
        //                    MccLogManager.OutConveyor.IN_WAIT_TIME.BatchWriteStart();
        //                }
        //            }
        //            //DoProcessOutConveyorStop();
        //        }
        //        // 컨베이어 언로드 위치 절대 이동 시작
        //        if (true == IsOutConveyor(EOutConveyorSequence.Run))
        //        {
        //            if (mConveyorRun == false)
        //            {
        //                mConveyorRun = true;
        //                MccLogManager.OutConveyor.CV_RUN.BatchWriteStart();
        //                if (mConveyorCellExist.Any(x => x) == true)
        //                {
        //                    MccLogManager.OutConveyor.OUT_WAIT_TIME.BatchWriteEnd();
        //                }
        //                else
        //                {
        //                    MccLogManager.OutConveyor.IN_WAIT_TIME.BatchWriteEnd();
        //                }
        //            }
        //            //DoProcessOutConveyorRun();
        //        }
        //        // MCC Component In/Out
        //        if (true == IsOutConveyor(EOutConveyorSequence.Mcc))
        //        {
        //            if (mConveyorCellExist[0] == true
        //                //&& MotorConveyorX.IsHalfwayToUnloadPosition() == true
        //                )
        //            {
        //                mConveyorCellExist[0] = false;
        //                Task.Run(async () =>
        //                {
        //                    using (var logScope = MccLogManager.OutConveyor.COMPONENT_OUT[0].CreateMccLogProvider())
        //                    {
        //                        await Task.Delay(100);
        //                    }
        //                });
        //            }
        //        }
        //        Thread.Sleep(WAIT_FOR_END_PROCESS_PERIOD_TIME);
        //    }
        //}

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
                UnitName = EProcess.OutFlip.ToString()
            };
            double cycleTactTime, loadingWaitTime, unloadingWaitTime, userStopTime;

            // Calculate Wait Time
            {
                DateTime startTime, endTime;
                double waitingTime, tactTime;
                loadingWaitTime = unloadingWaitTime = 0;
                unitTact.InputDelayTime = unitTact.OutputDelayTime = TimeSpan.Zero;

                // INNER ID
                mCycleTactTime[CDefine.ECycleTactOutFlip.INNER_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[0]) ? " " : mLastInnerID[0];
                mCycleTactTime[CDefine.ECycleTactOutFlip.INNER_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastInnerID[1]) ? " " : mLastInnerID[1];

                // CELL ID
                mCycleTactTime[CDefine.ECycleTactOutFlip.CELL_ID_P1].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[0]) ? " " : mLastCellID[0];
                mCycleTactTime[CDefine.ECycleTactOutFlip.CELL_ID_P2].m_strTime = string.IsNullOrWhiteSpace(mLastCellID[1]) ? " " : mLastCellID[1];

                // USER_STOP_TIME [WAIT]
                unitTact.UserStopTime = mUserStopTimeStopwatch.Elapsed;
                userStopTime = unitTact.UserStopTime.TotalSeconds;
                mUserStopTimeStopwatch.Reset();
                mCycleTactTime[CDefine.ECycleTactOutFlip.USER_STOP_TIME].m_strTime = userStopTime.ToString();

                // PURE CYCLE TACT [TACT] - 유닛 사이클 택타임에서 인터페이스 대기시간을 뺀 시간
                tactTime = m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutFlip.CYCLE_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutFlip.CYCLE_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_LOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_LOADING_END_TIME].GetTime());
                tactTime -= m_objDocument.CountTactTime(mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_START_TIME].GetTime(), mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_END_TIME].GetTime());
                tactTime -= userStopTime;
                mCycleTactTime[CDefine.ECycleTactOutFlip.PURE_CYCLE_TACT].m_strTime = tactTime.ToString();

                // BEFORE_CYCLE_START ~ CYCLE_START [TACT] 
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.CYCLE_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.CYCLE_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.CYCLE_TACT_TIME].m_strTime = tactTime.ToString();
                cycleTactTime = tactTime;
                unitTact.UnitTactTime = TimeSpan.FromSeconds(tactTime);

                // NACHI LOADING FROM OUT-ROBOT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_LOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_LOADING].m_strTime = waitingTime.ToString();
                loadingWaitTime += waitingTime;
                unitTact.InputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // NACHI LOADING FROM OUT-ROBOT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_LOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_LOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_LOADING].m_strTime = tactTime.ToString();

                // NACHI UNLOAD TO OUT CONVEYOR [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOAD_READY_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOAD_READY_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOAD_READY].m_strTime = waitingTime.ToString();

                // NACHI UNLOAD TO OUT CONVEYOR [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOAD_READY_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOAD_READY_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOAD_READY].m_strTime = tactTime.ToString();

                // NACHI UNLOADING TO OUT CONVEYOR [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_UNLOADING].m_strTime = waitingTime.ToString();
                unloadingWaitTime += waitingTime;
                unitTact.OutputDelayTime += TimeSpan.FromSeconds(waitingTime);

                // NACHI UNLOADING TO OUT CONVEYOR [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOADING_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOADING_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_UNLOADING].m_strTime = tactTime.ToString();

                // NACHI LOAD FROM OUT-ROBOT [WAIT]
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_RETURN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_RETURN_END_TIME].GetTime();
                waitingTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.WAIT_RETURN].m_strTime = waitingTime.ToString();

                // NACHI LOAD FROM OUT-ROBOT [MOVE]
                startTime = mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_RETURN_START_TIME].GetTime();
                endTime = mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_RETURN_END_TIME].GetTime();
                tactTime = m_objDocument.CountTactTime(startTime, endTime);
                mCycleTactTime[CDefine.ECycleTactOutFlip.MOVE_RETURN].m_strTime = tactTime.ToString();

                mCycleTactTime[CDefine.ECycleTactOutFlip.RAWDATA].m_strTime = " ";
            }

            // Write Log
            {
                Dictionary<string, string> tactTimeDetail = new Dictionary<string, string>();
                var sbLog = new StringBuilder();
                bool bIncomplete = false;
                ETactTime tactTimeIndex = ETactTime.OUT_FLIP;
                foreach (CDefine.ECycleTactOutFlip column in Enum.GetValues(typeof(CDefine.ECycleTactOutFlip)))
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
                m_objDocument.SetUpdateLog(m_objDocument.m_objCycleTactTime.m_TactOutFlip.LogType, sbLog.ToString());
            }

            string[] innerIds = mLastInnerID;
            string[] cellIds = mLastCellID;
            var tact = TactTime.Manager.GetTactOrNull(innerIds);
            if (null != tact)
            {
                tact.InnerID = innerIds;
                tact.CellID = cellIds;
                tact.UnitTacts[mDefine.ProcessIndex] = unitTact;
                tact.OutTact = GetOutTactTimeElapsed();
                m_objDocument.m_objTactTime.SetOutputTactTime(tact);
            }

            // 배출 택타임 시작
            SetOutTactTimeStartNew();
        }
    }
}

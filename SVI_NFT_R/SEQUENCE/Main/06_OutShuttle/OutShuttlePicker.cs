using SVI_NFT_R.CellData;
using SVI_NFT_R.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class OutShuttlePicker : CProcessAbstract
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            Pick,
            Place
        };

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            Pick,
            Place
        };

        private enum EOption : int
        {
            /// <summary>
            /// 모든 베큠 사용
            /// </summary>
            All = -1,
        }

        public CVacuum Vacuum;
        public CProcessMotion.EVacuum VacuumIndex => mDefine.VacuumIndex;
        public CellDataHandler CellPort => mDefine.CellPort;
        public bool IsCellDetectSensor => m_objDocument.GetIOBit(mDefine.CellDetectSensor);
        private const string ID = nameof(OutShuttlePicker);
        private ECommand mCommand;
        private EStatus mStatus;
        private Stopwatch mTimerCommandSync = new Stopwatch();
        private static readonly string REGISTRY_PATH = Path.Combine("ENC", Program.ID, "SEQUENCE", ID);
        private readonly Settings.RegistryComponent mBackupData = new Settings.RegistryComponent(REGISTRY_PATH);
        private CDefine.ELogType mLogType;

        public override string ToString() => $"{ID},{VacuumIndex},{mCommand},{mStatus}";

        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmObject = ID;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                m_objDocument = document;
                m_objConfig = m_objDocument.m_objConfig;
                mDefine = new Define(position);
                Vacuum = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[mDefine.VacuumIndex];
                mLogType = CellDataManager.GetLogTypeFromProcess(CellPort.ProcessIndex);
                // 쓰레드 생성 및 시작
                mThreadProcess = new Thread(ThreadProcess);
                mThreadProcess.Start();

                m_objDocument.m_objProcessMain.m_objProcessMotion.AllMotions.Add(this);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override void DeInitialize()
        {
            mbThreadExit = true;

            mThreadProcess.Join();
        }

        public bool Check()
        {
            CDefine.ELogType logTarget = CellDataManager.GetLogTypeFromProcess(CellPort.ProcessIndex);
            SetProcessLog("[START]", logTarget);
            try
            {
                if (m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun
                    || m_objDocument.IsManualInputMode == true
                    )
                {
                    return true;
                }
                if (CellPort.IsCellExist() == false)
                {
                    return true;
                }
                // !!! 센서 안정화 시간 대기
                SpinWait.SpinUntil(() =>
                {
                    Thread.Sleep(10);
                    if (Vacuum.Status != CVacuumAbstract.EVacuumStatus.STS_ON)
                    {
                        return false;
                    }
                    if (IsCellDetectSensor == false)
                    {
                        return false;
                    }
                    return true;
                }, WaitTime.Etc.CellDetectSensorSensingTimeout.ToTimeSpan());
                bool bResult = true;
                if (Vacuum.Status != CVacuumAbstract.EVacuumStatus.STS_ON)
                {
                    m_objDocument.SetForcedAlarm(mDefine.AlarmCheckFail);
                    SetProcessLog($"[FAILED] if (Vacuum.Status != CVacuumAbstract.EVacuumStatus.STS_ON)", logTarget);
                    bResult = false;
                }
                if (IsCellDetectSensor == false)
                {
                    m_objDocument.SetForcedAlarm(mDefine.AlarmCheckSensorFail);
                    SetProcessLog($"[FAILED] if (IsCellDetectSensor == false)", logTarget);
                    bResult = false;
                }
                return bResult;
            }
            finally
            {
                SetProcessLog($"[END]", logTarget);
            }
        }

        public bool CheckOverlap()
        {
            CDefine.ELogType logTarget = CellDataManager.GetLogTypeFromProcess(CellPort.ProcessIndex);
            SetProcessLog("[START]", logTarget);
            try
            {
                if (m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun
                    || m_objDocument.IsManualInputMode == true
                    )
                {
                    return true;
                }
                if (CellPort.IsCellExist() == true)
                {
                    return true;
                }
                // !!! 센서 안정화 시간 대기
                SpinWait.SpinUntil(() =>
                {
                    Thread.Sleep(10);
                    if (Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_ON)
                    {
                        return false;
                    }
                    if (IsCellDetectSensor == true)
                    {
                        return false;
                    }
                    return true;
                }, WaitTime.Etc.CellDetectSensorSensingTimeout.ToTimeSpan());
                bool bResult = true;
                if (Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_ON)
                {
                    m_objDocument.SetForcedAlarm(mDefine.AlarmOverlapFail);
                    SetProcessLog($"[FAILED] if (Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_ON)", logTarget);
                    bResult = false;
                }
                if (IsCellDetectSensor == true)
                {
                    m_objDocument.SetForcedAlarm(mDefine.AlarmOverlapSensorFail);
                    SetProcessLog($"[FAILED] if (IsCellDetectSensor == true)", logTarget);
                    bResult = false;
                }
                return bResult;
            }
            finally
            {
                SetProcessLog($"[END]", logTarget);
            }
        }

        protected override void OnBootUpAction()
        {
            // Restore Data
            Vacuum.OptionIndex = (int)mBackupData.GetValue($"{VacuumIndex}.{nameof(Vacuum.OptionIndex)}", (int)EOption.All);

            // 제품이 잡혀있는 상태로 PC를 껏다 켯을 때 출력신호가 초기화되고 제품이 잔압으로 잡혀있는 상태에대한 예외처리
            if (Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_ON)
            {
                // !!! 'Vacuum.SetVacuumCommand()'는 반드시 'Vacuum.OptionIndex'를 한 번이라도 설정한 뒤에 호출해야함
                Vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_ON, CVacuumAbstract.ESensorCheck.IGNORE);
            }
        }

        protected override bool SetPick()
        {
            Vacuum.OptionIndex = (int)EOption.All;

            if (
                Vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_ON
                && Vacuum.Command == CVacuumAbstract.EVacuumCommand.CMD_ON
               )
            {
                return true;
            }

            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]", mLogType);
            do
            {
                SetProcessLog(string.Format("SetVacuum -> Cmd:{0}, SensorIgnore:{1}", CVacuumAbstract.EVacuumCommand.CMD_ON, CVacuumAbstract.ESensorCheck.CHECK), mLogType);
                SetProcessLog("SetVacuum -> WaitForSignal", mLogType);
                if (false == Vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_ON))
                {
                    SetProcessLog(string.Format("SetVacuum -> WaitForSignal -> Error"), mLogType);
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmVacuumFail;
                    break;
                }
                bReturn = true;
            } while (false);
            SetProcessLog("[END] " + bReturn, mLogType);
            return bReturn;
        }

        protected override bool SetPlace()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]", mLogType);
            do
            {
                SetProcessLog(string.Format("SetVacuum -> Cmd:{0}, SensorIgnore:{1}", CVacuumAbstract.EVacuumCommand.CMD_BLOW, CVacuumAbstract.ESensorCheck.CHECK), mLogType);
                SetProcessLog("SetVacuum -> WaitForSignal", mLogType);
                if (false == Vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_BLOW))
                {
                    SetProcessLog(string.Format("SetVacuum -> WaitForSignal -> Error"), mLogType);
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmBlowFail;
                    break;
                }
                bReturn = true;
            } while (false);

            // 진공 , 파기 OFF
            Vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
            SetProcessLog("[END] " + bReturn, mLogType);
            return bReturn;
        }

        public bool SetCommand(ECommand eCommand)
        {
            mTimerCommandSync = Stopwatch.StartNew();
            mCommand = eCommand;
            return true;
        }

        public ECommand GetCommand()
        {
            return mCommand;
        }

        public EStatus GetStatus()
        {
            return mStatus;
        }

        public override bool WaitForEndProcess()
        {
            bool bResult = false;

            do
            {
                while (
                    ECommand.Idle != GetCommand()
                    || CDefine.ERunStatus.Pause == m_objDocument.GetRunStatus()
                    )
                {
                    Thread.Sleep(WAIT_FOR_END_PROCESS_PERIOD_TIME);
                }
                if (EStatus.Stop == GetStatus())
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        private void SetProcessLog(string logMessage, CDefine.ELogType logType, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                logType,
                string.Format("{0}({1}) -> {2}", callerMemberName, VacuumIndex, logMessage)
                );
        }

        private void ThreadProcess()
        {
            while (false == mbThreadExit)
            {
                if (ECommand.Idle != mCommand)
                {
                    RaiseMccLogWhenJobsBegined();
                    mStatus = EStatus.Unknown;
                    if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                    {
                        Thread.Sleep(CDefine.DEF_SIMULATION_SLEEP_TIME);
                    }
                    switch (mCommand)
                    {
                        case ECommand.Pick:
                            if (true == SetPick())
                            {
                                mStatus = EStatus.Pick;
                            }
                            break;

                        case ECommand.Place:
                            if (true == SetPlace())
                            {
                                mStatus = EStatus.Place;
                            }
                            break;
                    }

                    if (ECommand.Stop == mCommand)
                    {
                        // 사용자 정지 체크
                        mStatus = EStatus.Stop;
                    }
                    else
                    {
                        // 날라온 Command에 대한 알람이 있었는지 체크. STS_UNKNOWN 그대로이면 알람.
                        if (EStatus.Unknown == mStatus)
                        {
                            mStatus = EStatus.Error;
                            m_objDocument.SetAlarmEvent(m_objAlarmStructure);
                            int iReturn = (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP;
                            // 알람에 대해 작업자 입력에 따른 처리
                            switch (iReturn)
                            {
                                // 해당 커맨드에 대한 재시도
                                case (int)CDefine.EErrorProcess.ERROR_PROCESS_RETRY:
                                    continue;
                                // 작업자가 알람 처리를 정상적으로 했다는 가정 하에 커맨드에 맞는 상태로 변경
                                case (int)CDefine.EErrorProcess.ERROR_PROCESS_CONTINUE:
                                    if (ECommand.Pick == mCommand)
                                    {
                                        mStatus = EStatus.Pick;
                                    }
                                    else if (ECommand.Place == mCommand)
                                    {
                                        mStatus = EStatus.Place;
                                    }
                                    break;
                                // 에러에 대한 설비 정지
                                case (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP:
                                    mCommand = ECommand.Stop;
                                    continue;
                            }
                        }
                    }
                    RaiseMccLogWhenJobsFinished();
                    mCommand = ECommand.Idle;
                }
                Thread.Sleep(10);
            }
        }
    }
}
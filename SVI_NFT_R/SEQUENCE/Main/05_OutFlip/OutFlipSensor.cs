using SVI_NFT_R.CellData;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class OutFlipSensor : CProcessAbstract
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            CheckInput,
            CheckBlocked,
            CheckOutput,
        };

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            CheckInput,
            CheckBlocked,
            CheckOutput,
        };

        public bool IsCellDetectSensor1 => m_objDocument.GetIOBit(mDefine.CellDetectSensor1);
        public bool IsCellDetectSensor2 => m_objDocument.GetIOBit(mDefine.CellDetectSensor2);
        public bool IsConveyorBlockedSensor => m_objDocument.GetIOBit(mDefine.ConveyorBlockSensor);

        private const string ID = nameof(OutFlipSensor);
        private ECommand mCommand;
        private EStatus mStatus;
        private ECellPlacement mCellPlacement;

        public override string ToString() => $"{ID},,{mCommand},{mStatus}";

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

        private bool SetCheckInput()
        {
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            try
            {
                if (m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun
                    || m_objConfig.GetOptionParameter().bUseOutConveyorInputCheck == false
                    )
                {
                    return true;
                }

                if (
                    IsCellDetectSensor1 == true
                    || IsCellDetectSensor2 == true
                    )
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCheckInputFail;
                    return false;
                }

                return true;
            }
            finally
            {
                SetProcessLog($"[END]");
            }
        }

        private bool SetCheckBlocked()
        {
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            try
            {
                if (m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun
                    || m_objConfig.GetOptionParameter().bUseOutConveyorBlockedCheck == false
                    )
                {
                    return true;
                }

                if (IsConveyorBlockedSensor == true)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCheckBlockedFail;
                    return false;
                }

                return true;
            }
            finally
            {
                SetProcessLog($"[END]");
            }
        }

        private bool SetCheckCellOutput()
        {
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            try
            {
                if (m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun
                    || m_objConfig.GetOptionParameter().bUseOutConveyorOutputCheck == false
                    )
                {
                    return true;
                }

                if (mCellPlacement.HasFlag(ECellPlacement.Full) == true)
                {
                    int failCount = 0;
                    if (IsCellDetectSensor1 == false)
                    {
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCheckOutputCellP1;
                        failCount++;
                    }
                    if (IsCellDetectSensor2 == false)
                    {
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCheckOutputCellP2;
                        failCount++;
                    }
                    if (failCount > 0)
                    {
                        m_objAlarmStructure.iAlarmCode = failCount == 2 ? (int)mDefine.AlarmCheckOutputCellAll : m_objAlarmStructure.iAlarmCode;
                        return false;
                    }
                }
                else
                {
                    if (mCellPlacement.HasFlag(ECellPlacement.P1) == true)
                    {
                        if (IsCellDetectSensor1 == false)
                        {
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCheckOutputCellP1;
                            return false;
                        }
                    }
                    else if (mCellPlacement.HasFlag(ECellPlacement.P2) == true)
                    {
                        if (IsCellDetectSensor2 == false)
                        {
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCheckOutputCellP2;
                            return false;
                        }
                    }
                }

                return true;
            }
            finally
            {
                SetProcessLog($"[END]");
            }
        }

        public bool SetCommand(ECommand eCommand)
        {
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

        public bool SetCellPlacement(ECellPlacement eCellPlacement)
        {
            mCellPlacement = eCellPlacement;
            return true;
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

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                mDefine.LogType,
                string.Format("{0} -> {1}", callerMemberName, logMessage)
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
                        case ECommand.CheckInput:
                            if (true == SetCheckInput())
                            {
                                mStatus = EStatus.CheckInput;
                            }
                            break;

                        case ECommand.CheckBlocked:
                            if (true == SetCheckBlocked())
                            {
                                mStatus = EStatus.CheckBlocked;
                            }
                            break;

                        case ECommand.CheckOutput:
                            if (true == SetCheckCellOutput())
                            {
                                mStatus = EStatus.CheckOutput;
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
                                    if (ECommand.CheckInput == mCommand)
                                    {
                                        mStatus = EStatus.CheckInput;
                                    }
                                    else if (ECommand.CheckBlocked == mCommand)
                                    {
                                        mStatus = EStatus.CheckBlocked;
                                    }
                                    else if (ECommand.CheckOutput == mCommand)
                                    {
                                        mStatus = EStatus.CheckOutput;
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
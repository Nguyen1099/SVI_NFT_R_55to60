using SVI_NFT_R.CellData;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class OutRobotTurnCylinder : CProcessAbstract
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            Turn,
            Return,
        };

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            Turn,
            Return,
        };

        public CCylinder Cylinder;
        public CProcessMotion.ECylinder CylinderIndex => mDefine.CylinderIndex;
        public CellDataHandler CellPort => mDefine.CellPort;
        private const string ID = nameof(OutRobotTurnCylinder);
        private ECommand mCommand;
        private EStatus mStatus;

        public override string ToString() => $"{ID},{CylinderIndex},{mCommand},{mStatus}";

        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmObject = ID;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                m_objDocument = document;
                mDefine = new Define(position);
                Cylinder = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objCylinder[mDefine.CylinderIndex];

                // 인터락 객체 생성
                m_objInterlock = new CProcessInterlock(new OutRobotInterlock(m_objDocument));
                if (false == m_objInterlock.Initialize())
                {
                    break;
                }
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
            m_objInterlock.DeInitialize();
        }

        public bool IsInposition(ECommand command)
        {
            switch (command)
            {
                case ECommand.Turn:
                    return Cylinder.Status == CCylinderAbstract.ECylinderStatus.STS_TURN;
                case ECommand.Return:
                    return Cylinder.Status == CCylinderAbstract.ECylinderStatus.STS_RETURN;
                default:
                    break;
            }
            return false;
        }

        private bool SetTurn()
        {
            if (Cylinder.Status == CCylinderAbstract.ECylinderStatus.STS_TURN)
            {
                return true;
            }

            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");
            do
            {
                SetProcessLog(string.Format("SetCylinder -> Cmd:{0}, SensorIgnore:{1}", CCylinderAbstract.ECylinderCommand.CMD_TURN, CCylinderAbstract.ESensorCheck.CHECK));
                SetProcessLog(string.Format("SetCylinder -> WaitForSignal"));
                if (false == Cylinder.SetCylinderCommand(CCylinderAbstract.ECylinderCommand.CMD_TURN))
                {
                    SetProcessLog(string.Format("SetCylinder -> WaitForSignal -> Error"));
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmTurnFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog("[END] " + bReturn);
            return bReturn;
        }

        private bool SetReturn()
        {
            if (Cylinder.Status == CCylinderAbstract.ECylinderStatus.STS_RETURN)
            {
                return true;
            }

            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");
            do
            {
                SetProcessLog(string.Format("SetCylinder -> Cmd:{0}, SensorIgnore:{1}", CCylinderAbstract.ECylinderCommand.CMD_RETURN, CCylinderAbstract.ESensorCheck.CHECK));
                SetProcessLog(string.Format("SetCylinder -> WaitForSignal"));
                if (false == Cylinder.SetCylinderCommand(CCylinderAbstract.ECylinderCommand.CMD_RETURN))
                {
                    SetProcessLog(string.Format("SetCylinder -> WaitForSignal -> Error"));
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmReturnFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog("[END] " + bReturn);
            return bReturn;
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

                    switch (mCommand)
                    {
                        case ECommand.Turn:
                            if (true == SetTurn())
                            {
                                mStatus = EStatus.Turn;
                            }
                            break;
                        case ECommand.Return:
                            if (true == SetReturn())
                            {
                                mStatus = EStatus.Return;
                            }
                            break;
                        case ECommand.Idle:
                        case ECommand.Stop:
                            break;
                        default:
                            Debug.Assert(false);
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
                                    if (ECommand.Turn == mCommand)
                                    {
                                        mStatus = EStatus.Turn;
                                    }
                                    else if (ECommand.Return == mCommand)
                                    {
                                        mStatus = EStatus.Return;
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

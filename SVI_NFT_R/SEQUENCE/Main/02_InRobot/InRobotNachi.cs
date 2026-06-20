using SVI_NFT_R.DEVICE.Nachi;
using SVI_NFT_R.DEVICE.Nachi.SignalNames;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class InRobotNachi : CProcessAbstract, IEmsHandle
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            Initialize,

            LoadProcessBegin,
            LoadPermit,
            LoadCycleEnd,
            LoadProcessExit,

            McrProcessBegin,
            McrPermit,
            McrCycleEnd,
            McrProcessExit,

            UnloadProcessBegin,
            UnloadPermit,
            UnloadCycleEnd,
            UnloadProcessExit,

            JcmModeEntry,
            JcmBeginProcess,
            JcmCheckTeachingMove,
            JcmCheckTeachingSave,
            JcmCheckTeachingNoSave,
            JcmEndProcess,
            JcmModeExit
        };

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            Initialize,

            LoadProcessBegin,
            LoadPermit,
            LoadCycleEnd,
            LoadProcessExit,

            McrProcessBegin,
            McrPermit,
            McrCycleEnd,
            McrProcessExit,

            UnloadProcessBegin,
            UnloadPermit,
            UnloadCycleEnd,
            UnloadProcessExit,

            JcmModeEntry,
            JcmBeginProcess,
            JcmCheckTeachingMove,
            JcmCheckTeachingSave,
            JcmCheckTeachingNoSave,
            JcmEndProcess,
            JcmModeExit
        };

        public CProcessMotion.ERobot RobotIndex => mDefine.RobotIndex;
        public EMethod ProcessMethodIndex { get; set; } = EMethod.None;
        public ETool ProcessToolIndex { get; set; } = ETool.None;
        public EStage ProcessStageIndex { get; set; } = EStage.None;
        public ProcessPreOrder ProcessPreOrder { get; set; } = ProcessPreOrder.Empty;
        public CDeviceNachi Robot { get; private set; }
        public IReadOnlyDictionary<ECommand, Action> SetProcessPreOrder => mSetProcessPreOrder;
        public IReadOnlyDictionary<ECommand, Action> SetProcessExitPreOrder => mSetProcessExitPreOrder;
        public ERobotProcess JcmRobotProcessIndex { get; set; } = ERobotProcess.None;
        public EJcmCommand JcmCommandIndex { get; set; } = EJcmCommand.None;
        public EEmsGroupFlags EmergencyStopGroup => EEmsGroupFlags.Loader;
        private const string ID = nameof(InRobotNachi);
        private readonly Dictionary<ECommand, Action> mSetProcessPreOrder = new Dictionary<ECommand, Action>();
        private readonly Dictionary<ECommand, Action> mSetProcessExitPreOrder = new Dictionary<ECommand, Action>();
        private ECommand mCommand;
        private EStatus mStatus;
        private EStatus mBeforeStatus;
        private RobotHandshakeHandler mRobotHandshakeHandler;
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

                mSetProcessPreOrder[ECommand.LoadProcessBegin] = () => Robot.ProcessPreOrder(ERobotProcess.P1, ProcessPreOrder);
                mSetProcessPreOrder[ECommand.McrProcessBegin] = () => Robot.ProcessPreOrder(ERobotProcess.P3, ProcessPreOrder);
                mSetProcessPreOrder[ECommand.UnloadProcessBegin] = () => Robot.ProcessPreOrder(ERobotProcess.P4, ProcessPreOrder);

                mSetProcessExitPreOrder[ECommand.LoadProcessExit] = () => Robot.ProcessExitPreOrder(ERobotProcess.P1);
                mSetProcessExitPreOrder[ECommand.McrProcessExit] = () => Robot.ProcessExitPreOrder(ERobotProcess.P3);
                mSetProcessExitPreOrder[ECommand.UnloadProcessExit] = () => Robot.ProcessExitPreOrder(ERobotProcess.P4);

                // 나찌 로봇 객체
                Robot = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot[mDefine.RobotIndex];

                // 나찌 인터락 객체
                m_objInterlock = new CProcessInterlock(new InRobotInterlock(m_objDocument));
                if (false == m_objInterlock.Initialize())
                {
                    break;
                }

                // 쓰레드 생성
                mThreadProcess = new Thread(ThreadProcess);
                // 쓰레드 시작
                mThreadProcess.Start();

                m_objDocument.m_objProcessMain.m_objProcessMotion.AllMotions.Add(this);

                Robot.RmsDataChanged += Robot_RmsDataChanged;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override void DeInitialize()
        {
            Robot.RmsDataChanged -= Robot_RmsDataChanged;
            mbThreadExit = true;
            mThreadProcess.Join();

            m_objInterlock.DeInitialize();
        }

        public bool IsLoadInterlock()
        {
            // 자동 모드일 경우
            if (Robot.Status.IsPlayBackModeEntry == true && Robot.Status.IsRunningU1 == true)
            {
                if (Robot.Signals.X.BB[Bit.EInput.P1_INTERLOCK].Value == false)
                {
                    return true;
                }
            }
            // 자동 모드가 아닐 경우 인터락
            else
            {
                return true;
            }

            return false;
        }

        public bool IsMcrInterlock()
        {
            // 자동 모드일 경우
            if (Robot.Status.IsPlayBackModeEntry == true && Robot.Status.IsRunningU1 == true)
            {
                if (Robot.Signals.X.BB[Bit.EInput.P3_INTERLOCK].Value == false)
                {
                    return true;
                }
            }
            // 자동 모드가 아닐 경우 인터락
            else
            {
                return true;
            }

            return false;
        }

        public bool IsUnloadInterlock()
        {
            // 자동 모드일 경우
            if (Robot.Status.IsPlayBackModeEntry == true && Robot.Status.IsRunningU1 == true)
            {
                if (Robot.Signals.X.BB[Bit.EInput.P4_INTERLOCK].Value == false)
                {
                    return true;
                }
            }
            // 자동 모드가 아닐 경우 인터락
            else
            {
                return true;
            }

            return false;
        }

        public void EmergencyStop()
        {
            if (Robot.Status.IsFailure == true
                || Robot.Status.IsBusy == false
                || Robot.Status.IsMotorEnergized == false
                )
            {
                return;
            }
            Robot.SetPause();
        }

        protected override void OnBootUpAction()
        {
            // PC 최초 부팅시 신호가 모두 꺼져있어서 팬던트 쪽에서 초기화를 바로 진행 할 수 없음으로 프로그램 구동시 신호 켜줌
            Robot.Signals.Y.BB[Bit.EOutput.PLAY_STOP_U1].Value = CDefine.ON;

            // MC가 꺼져 있으면 스킵
            if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON) == false)
            {
                return;
            }

            // 알람 리셋
            Robot.SetAlarmReset();
        }

        private bool SetInitialize()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog("[START]");

            do
            {
                var offsetParameter = m_objConfig.GetNachiModelParameter(mDefine.RobotIndex);
                if (
                    // 알람 리셋
                    false == Robot.SetAlarmReset()
                    // 속도 설정
                    || false == Robot.SetRobotOverrideSpeed(offsetParameter.OverrideSpeed)
                    // 초기화 진행
                    || false == Robot.SetRobotInitial(offsetParameter, Robot.IsNeedSendOffsetData, Robot.IsNeedReceiveRmsData)
                    )
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmInitializeFail;
                    break;
                }
                Robot.IsNeedSendOffsetData = false;
                Robot.IsNeedReceiveRmsData = false;

                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn.ToString());
            return bReturn;
        }

        private bool SetProcessBegin(ERobotProcess robotProcessIndex)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START] {robotProcessIndex}");

            do
            {
                if (false == Robot.SetOtherProcessSkipIfRunning(robotProcessIndex))
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmRunProcessSkipFail;
                    break;
                }

                if (false == Robot.ProcessBegin(robotProcessIndex))
                {
                    m_objAlarmStructure.iAlarmCode = mDefine.GetAlarmProcessBeginFail(robotProcessIndex);
                    break;
                }

                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn.ToString());
            return bReturn;
        }

        private bool SetProcessPermit(ERobotProcess robotProcessIndex, EMethod methodIndex, ETool toolIndex, EStage stageIndex, bool bAlignUse)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START] {robotProcessIndex}/{methodIndex}/{toolIndex}/{stageIndex}");
            mRobotHandshakeHandler = Robot.Status.GetRobotHandshakeHandler(robotProcessIndex, methodIndex, toolIndex, stageIndex);

            do
            {
                if (false == Robot.ProcessPermit(robotProcessIndex, methodIndex, toolIndex, stageIndex, bAlignUse))
                {
                    m_objAlarmStructure.iAlarmCode = mDefine.GetAlarmProcessPermitFail(robotProcessIndex);
                    break;
                }

                if (mRobotHandshakeHandler.Begin_Down() == false)
                {
                    m_objAlarmStructure.iAlarmCode = mDefine.GetAlarmProcessPermitFail(robotProcessIndex);
                    break;
                }

                if (Robot.ProcessPreOrder(robotProcessIndex, ProcessPreOrder, bUsePreOrderPermit: true) == false)
                {
                    m_objAlarmStructure.iAlarmCode = mDefine.GetAlarmProcessPermitFail(robotProcessIndex);
                    break;
                }

                bReturn = true;
            } while (false);

            ProcessPreOrder = ProcessPreOrder.Empty;
            SetProcessLog("[END] " + bReturn.ToString());
            return bReturn;
        }

        private bool SetProcessCycleEnd(ERobotProcess robotProcessIndex)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START] {robotProcessIndex}");

            do
            {
                if (mRobotHandshakeHandler.End_Up() == false)
                {
                    m_objAlarmStructure.iAlarmCode = mDefine.GetAlarmProcessPermitFail(robotProcessIndex);
                    break;
                }

                if (false == Robot.ProcessCycleEnd(robotProcessIndex))
                {
                    m_objAlarmStructure.iAlarmCode = mDefine.GetAlarmProcessPermitFail(robotProcessIndex);
                    break;
                }

                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn.ToString());
            return bReturn;
        }

        private bool SetProcessExit(ERobotProcess robotProcessIndex)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START] {robotProcessIndex}");

            do
            {
                if (false == Robot.ProcessExit(robotProcessIndex))
                {
                    m_objAlarmStructure.iAlarmCode = mDefine.GetAlarmProcessExitFail(robotProcessIndex);
                    break;
                }

                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn.ToString());
            return bReturn;
        }

        private bool SetProcessJcmModeEntry()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START]");
            do
            {
                if (Robot.JcmEnterMode() == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmJcmModeEntryFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog($"[END] {bReturn}");
            return bReturn;
        }

        private bool SetProcessJcmBeginProcess()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START]");
            do
            {
                if (Robot.JcmSelectProcess(JcmRobotProcessIndex) == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmJcmModeMoveFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog($"[END] {bReturn}");
            return bReturn;
        }

        private bool SetProcessJcmCheckTeachingMove()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START]");
            do
            {
                if (Robot.JcmCheckTeaching(JcmCommandIndex) == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmJcmModeMoveFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog($"[END] {bReturn}");
            return bReturn;
        }

        private bool SetProcessJcmCheckTeachingSave()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START]");
            do
            {
                if (Robot.JcmCheckTeachingSave() == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmJcmModeMoveFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog($"[END] {bReturn}");
            return bReturn;
        }

        private bool SetProcessJcmCheckTeachingNoSave()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START]");
            do
            {
                if (Robot.JcmCheckTeachingNoSave() == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmJcmModeMoveFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog($"[END] {bReturn}");
            return bReturn;
        }

        private bool SetProcessJcmEndProcess()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START]");
            do
            {
                if (Robot.JcmEndProcess() == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmJcmModeMoveFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog($"[END] {bReturn}");
            return bReturn;
        }

        private bool SetProcessJcmModeExit()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.iAlarmCode = int.MaxValue;
            SetProcessLog($"[START]");
            do
            {
                if (Robot.JcmExitMode() == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmJcmModeExitFail;
                    break;
                }

                bReturn = true;
            } while (false);
            SetProcessLog($"[END] {bReturn}");
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
                        case ECommand.Initialize:
                            if (true == SetInitialize())
                                mStatus = EStatus.Initialize;
                            break;

                        case ECommand.LoadProcessBegin:
                            if (true == SetProcessBegin(ERobotProcess.P1))
                                mStatus = EStatus.LoadProcessBegin;
                            break;

                        case ECommand.LoadPermit:
                            if (true == SetProcessPermit(ERobotProcess.P1, ProcessMethodIndex, ProcessToolIndex, ProcessStageIndex, true))
                                mStatus = EStatus.LoadPermit;
                            break;

                        case ECommand.LoadCycleEnd:
                            if (true == SetProcessCycleEnd(ERobotProcess.P1))
                                mStatus = EStatus.LoadCycleEnd;
                            break;

                        case ECommand.LoadProcessExit:
                            if (true == SetProcessExit(ERobotProcess.P1))
                                mStatus = EStatus.LoadProcessExit;
                            break;

                        case ECommand.McrProcessBegin:
                            if (true == SetProcessBegin(ERobotProcess.P3))
                                mStatus = EStatus.McrProcessBegin;
                            break;

                        case ECommand.McrPermit:
                            if (true == SetProcessPermit(ERobotProcess.P3, ProcessMethodIndex, ProcessToolIndex, ProcessStageIndex, false))
                                mStatus = EStatus.McrPermit;
                            break;

                        case ECommand.McrCycleEnd:
                            if (true == SetProcessCycleEnd(ERobotProcess.P3))
                                mStatus = EStatus.McrCycleEnd;
                            break;

                        case ECommand.McrProcessExit:
                            if (true == SetProcessExit(ERobotProcess.P3))
                                mStatus = EStatus.McrProcessExit;
                            break;

                        case ECommand.UnloadProcessBegin:
                            if (true == SetProcessBegin(ERobotProcess.P4))
                                mStatus = EStatus.UnloadProcessBegin;
                            break;

                        case ECommand.UnloadPermit:
                            if (true == SetProcessPermit(ERobotProcess.P4, ProcessMethodIndex, ProcessToolIndex, ProcessStageIndex, false))
                                mStatus = EStatus.UnloadPermit;
                            break;

                        case ECommand.UnloadCycleEnd:
                            if (true == SetProcessCycleEnd(ERobotProcess.P4))
                                mStatus = EStatus.UnloadCycleEnd;
                            break;

                        case ECommand.UnloadProcessExit:
                            if (true == SetProcessExit(ERobotProcess.P4))
                                mStatus = EStatus.UnloadProcessExit;
                            break;

                        case ECommand.JcmModeEntry:
                            if (SetProcessJcmModeEntry() == true)
                                mStatus = EStatus.JcmModeEntry;
                            break;

                        case ECommand.JcmBeginProcess:
                            if (SetProcessJcmBeginProcess() == true)
                                mStatus = EStatus.JcmBeginProcess;
                            break;

                        case ECommand.JcmCheckTeachingMove:
                            if (SetProcessJcmCheckTeachingMove() == true)
                                mStatus = EStatus.JcmCheckTeachingMove;
                            break;

                        case ECommand.JcmCheckTeachingSave:
                            if (SetProcessJcmCheckTeachingSave() == true)
                                mStatus = EStatus.JcmCheckTeachingSave;
                            break;

                        case ECommand.JcmCheckTeachingNoSave:
                            if (SetProcessJcmCheckTeachingNoSave() == true)
                                mStatus = EStatus.JcmCheckTeachingNoSave;
                            break;

                        case ECommand.JcmEndProcess:
                            if (SetProcessJcmEndProcess() == true)
                                mStatus = EStatus.JcmEndProcess;
                            break;

                        case ECommand.JcmModeExit:
                            if (SetProcessJcmModeExit() == true)
                                mStatus = EStatus.JcmModeExit;
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
                            if (m_objAlarmStructure.iAlarmCode != int.MaxValue)
                            {
                                m_objDocument.SetAlarmEvent(m_objAlarmStructure);
                            }
                            int iReturn = (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP;
                            // 알람에 대해 작업자 입력에 따른 처리
                            switch (iReturn)
                            {
                                // 해당 커맨드에 대한 재시도
                                case (int)CDefine.EErrorProcess.ERROR_PROCESS_RETRY:
                                    continue;
                                // 작업자가 알람 처리를 정상적으로 했다는 가정 하에 커맨드에 맞는 상태로 변경
                                case (int)CDefine.EErrorProcess.ERROR_PROCESS_CONTINUE:
                                    if (ECommand.Initialize == mCommand)
                                        mStatus = EStatus.Initialize;
                                    else if (ECommand.LoadProcessBegin == mCommand)
                                        mStatus = EStatus.LoadProcessBegin;
                                    else if (ECommand.LoadPermit == mCommand)
                                        mStatus = EStatus.LoadPermit;
                                    else if (ECommand.LoadCycleEnd == mCommand)
                                        mStatus = EStatus.LoadCycleEnd;
                                    else if (ECommand.LoadProcessExit == mCommand)
                                        mStatus = EStatus.LoadProcessExit;
                                    else if (ECommand.McrProcessBegin == mCommand)
                                        mStatus = EStatus.McrProcessBegin;
                                    else if (ECommand.McrPermit == mCommand)
                                        mStatus = EStatus.McrPermit;
                                    else if (ECommand.McrCycleEnd == mCommand)
                                        mStatus = EStatus.McrCycleEnd;
                                    else if (ECommand.McrProcessExit == mCommand)
                                        mStatus = EStatus.McrProcessExit;
                                    else if (ECommand.UnloadPermit == mCommand)
                                        mStatus = EStatus.UnloadPermit;
                                    else if (ECommand.UnloadProcessBegin == mCommand)
                                        mStatus = EStatus.UnloadProcessBegin;
                                    else if (ECommand.UnloadCycleEnd == mCommand)
                                        mStatus = EStatus.UnloadCycleEnd;
                                    else if (ECommand.UnloadProcessExit == mCommand)
                                        mStatus = EStatus.UnloadProcessExit;
                                    else if (ECommand.JcmModeEntry == mCommand)
                                        mStatus = EStatus.JcmModeEntry;
                                    else if (ECommand.JcmBeginProcess == mCommand)
                                        mStatus = EStatus.JcmBeginProcess;
                                    else if (ECommand.JcmCheckTeachingMove == mCommand)
                                        mStatus = EStatus.JcmCheckTeachingMove;
                                    else if (ECommand.JcmCheckTeachingSave == mCommand)
                                        mStatus = EStatus.JcmCheckTeachingSave;
                                    else if (ECommand.JcmCheckTeachingNoSave == mCommand)
                                        mStatus = EStatus.JcmCheckTeachingNoSave;
                                    else if (ECommand.JcmEndProcess == mCommand)
                                        mStatus = EStatus.JcmEndProcess;
                                    else if (ECommand.JcmModeExit == mCommand)
                                        mStatus = EStatus.JcmModeExit;
                                    break;
                                // 에러에 대한 설비 정지
                                case (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP:
                                    // 인 나찌 초기화 상태 초기화
                                    mCommand = ECommand.Stop;
                                    continue;
                            }
                        }
                    }
                    RaiseMccLogWhenJobsFinished();
                    mCommand = ECommand.Idle;
                    mBeforeStatus = mStatus;
                }

                if (Robot.Status.IsTeachModeEntry == true)
                {
                    // !!! 로봇 디바이스 쪽으로 플래그 위치 이동함: UI에서 처리하기 편하도록 하기 위함
                    Robot.IsNeedSendOffsetData = true;
                    Robot.IsNeedReceiveRmsData = true;
                }
                Thread.Sleep(10);
            }
        }

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                mDefine.LogType,
                string.Format("{0} -> {1}", callerMemberName, logMessage)
                );
        }

        private void Robot_RmsDataChanged(object sender, RmsDataChangedEventArgs e)
        {
            // EC 변경사항있는경우 보고됨
            m_objDocument.SetECList();
        }
    }
}
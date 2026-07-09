using HLDevice;
using HLDevice.Abstract;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    //public partial class OutFlipMotorX : CProcessAbstract, IEmsHandle
    //{
    //    public enum ECommand
    //    {
    //        Idle = 0,
    //        Stop,

    //        Home,
    //        PositionZeroSet,
    //        WaitInterlock
    //    };

    //    public enum EStatus
    //    {
    //        Unknown = 0,
    //        Error,
    //        Stop,

    //        Home,
    //        PositionZeroSet,
    //        WaitInterlock
    //    };

    //    public enum EPosition
    //    {
    //        Unload,
    //    };

    //    public CDeviceMotor Axis { get; private set; }
    //    public CProcessMotion.EMotor MotorIndex => mDefine.MotorIndex;
    //    public EEmsGroupFlags EmergencyStopGroup => EEmsGroupFlags.Unloader;
    //    public bool IsNeedOrigin { get; private set; } = true;
    //    private const string ID = nameof(OutFlipMotorX);
    //    private ECommand mCommand;
    //    private EStatus mStatus;
    //    private const int SERVO_RESET_DELAY_TIME = 200;
    //    private const int SERVO_CHANGE_DELAY_TIME = 3000;
    //    public override string ToString() => $"{ID},{MotorIndex},{mCommand},{mStatus}";

    //    public override bool Initialize(CDocument document, int position = 0, params object[] args)
    //    {
    //        bool bReturn = false;
    //        m_objAlarmStructure.strAlarmObject = ID;
    //        m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

    //        do
    //        {
    //            m_objDocument = document;
    //            m_objConfig = m_objDocument.m_objConfig;
    //            mDefine = new Define(position);

    //            Axis = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[mDefine.MotorIndex];

    //            // 인터락 객체 생성
    //            m_objInterlock = new CProcessInterlock(new OutFlipInterlock(m_objDocument));
    //            if (false == m_objInterlock.Initialize())
    //            {
    //                break;
    //            }

    //            // 쓰레드 생성 및 시작
    //            mThreadProcess = new Thread(ThreadProcess);
    //            mThreadProcess.Start();

    //            m_objDocument.m_objProcessMain.m_objProcessMotion.AllMotions.Add(this);
    //            m_objDocument.m_objProcessMain.m_objProcessMotion.MotorInterlocks[mDefine.MotorIndex] = m_objInterlock;

    //            bReturn = true;
    //        } while (false);

    //        return bReturn;
    //    }

    //    public override void DeInitialize()
    //    {
    //        mbThreadExit = true;
    //        mThreadProcess.Join();

    //        m_objInterlock.DeInitialize();
    //    }

    //    public bool IsRunning()
    //    {
    //        return (Axis.HLGetMotorStatus().bInposition == false);
    //    }

    //    public bool IsArrivalUnloadPosition()
    //    {
    //        bool bResult = false;

    //        do
    //        {
    //            int targetPositionIndex = (int)EPosition.Unload;
    //            // 현재 위치가 목표 위치보다 작은가?
    //            double unloadPosition = Axis.HLGetMotorPosition().dPosition[targetPositionIndex];
    //            var motorState = Axis.HLGetMotorStatus();
    //            double currentPosition = motorState.dEncoderPosition;
    //            double deltaPosition = Math.Abs(unloadPosition - currentPosition);
    //            double tolerence = Math.Abs(Axis.HLGetMotorOperationParameter().dStandardTolerence);
    //            if (deltaPosition > tolerence)
    //            {
    //                break;
    //            }

    //            // 모터가 인포지션 상태가 아닌가?
    //            bool bInposition = motorState.bInposition;
    //            if (false == bInposition)
    //            {
    //                break;
    //            }

    //            bResult = true;
    //        } while (false);

    //        return bResult;
    //    }

    //    public bool IsHalfwayToUnloadPosition()
    //    {
    //        bool bResult = false;
    //        do
    //        {
    //            int targetPositionIndex = (int)EPosition.Unload;
    //            // 현재 위치가 목표 위치보다 작은가?
    //            double unloadPosition = Axis.HLGetMotorPosition().dPosition[targetPositionIndex];
    //            var motorState = Axis.HLGetMotorStatus();
    //            double currentPosition = motorState.dEncoderPosition;
    //            double deltaPosition = Math.Abs(unloadPosition - currentPosition);
    //            double halfWayPosition = Math.Abs(unloadPosition) / 2.0;
    //            if (deltaPosition > halfWayPosition)
    //            {
    //                break;
    //            }
    //            bResult = true;
    //        } while (false);
    //        return bResult;
    //    }

    //    public void SetConveyorStop()
    //    {
    //        Axis.HLMoveEStop();

    //        // 정지 후 슬립이 필요함
    //        // !여기에 슬립이 없으면 이상 동작함
    //        WaitForMotorInposition();
    //    }

    //    public void SetConveyorUnloadMoveStart()
    //    {
    //        int targetPositionIndex = (int)EPosition.Unload;
    //        // 언로드 포지션으로 이동을 시작한다 (설비 모드에 상관없이 비동기로 돌기 때문에 자동이동 속도로만 동작한다)
    //        Axis.HLMoveAbsoluteIndex(targetPositionIndex, CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_AUTO_RUN);
    //    }

    //    public bool SetCommand(ECommand eCommand)
    //    {
    //        mCommand = eCommand;
    //        return true;
    //    }

    //    public ECommand GetCommand()
    //    {
    //        return mCommand;
    //    }

    //    public EStatus GetStatus()
    //    {
    //        return mStatus;
    //    }

    //    public override bool WaitForEndProcess()
    //    {
    //        bool bResult = false;

    //        do
    //        {
    //            while (
    //                ECommand.Idle != GetCommand()
    //                || CDefine.ERunStatus.Pause == m_objDocument.GetRunStatus()
    //                )
    //            {
    //                Thread.Sleep(WAIT_FOR_END_PROCESS_PERIOD_TIME);
    //            }
    //            if (EStatus.Stop == GetStatus())
    //            {
    //                break;
    //            }

    //            bResult = true;
    //        } while (false);

    //        return bResult;
    //    }

    //    public void EmergencyStop()
    //    {
    //        // 컨베이어는 정지없음
    //    }

    //    protected override void OnBootUpAction()
    //    {
    //        // MC가 꺼져 있으면 스킵
    //        if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON) == false)
    //        {
    //            return;
    //        }

    //        var motor = Axis;
    //        // 서보 OFF
    //        motor.SetServoAndWaitChanged(CDefine.OFF, SERVO_CHANGE_DELAY_TIME);
    //        // 알람 리셋
    //        motor.HLAlarmReset();
    //        // 서보 ON
    //        motor.SetServoAndWaitChanged(CDefine.ON, SERVO_CHANGE_DELAY_TIME);
    //    }

    //    protected override bool SetHome()
    //    {
    //        bool bReturn = false;
    //        m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
    //        SetProcessLog("[START]");

    //        do
    //        {
    //            int alarmResetRetryCount = 0;
    //            var motor = Axis;
    //            while (
    //                motor.HLGetMotorStatus().bAlarm == true
    //                && alarmResetRetryCount < 5
    //                )
    //            {
    //                // 서보 Off
    //                SetProcessLog("SetServoDisable");
    //                motor.SetServoAndWaitChanged(CDefine.OFF, SERVO_CHANGE_DELAY_TIME);
    //                // 알람 리셋
    //                SetProcessLog("SetAlarmReset");
    //                motor.HLAlarmReset();
    //                Thread.Sleep(SERVO_RESET_DELAY_TIME);
    //                // 알람 클리어 확인
    //                if (motor.HLGetMotorStatus().bAlarm == false)
    //                {
    //                    break;
    //                }
    //                // 서보 On
    //                SetProcessLog("SetServoEnable");
    //                motor.SetServoAndWaitChanged(CDefine.ON, SERVO_CHANGE_DELAY_TIME);
    //                // 알람 리셋
    //                SetProcessLog("SetAlarmReset");
    //                motor.HLAlarmReset();

    //                alarmResetRetryCount++;
    //            }

    //            motor.HLSetSoftwareLimit(CDeviceMotorAbstract.EUse.ENABLE);
    //            if (motor.HLGetMotorStatus().bServo == CDefine.OFF)
    //            {
    //                // 서보 온
    //                SetProcessLog("SetServoEnable");
    //                if (false == motor.SetServoAndWaitChanged(CDefine.ON, SERVO_CHANGE_DELAY_TIME))
    //                {
    //                    SetProcessLog("SetServoEnable -> Error " + mDefine.AlarmHomeMotionFail.ToString());
    //                    m_objDocument.SetInitializeErrorLog($"{mDefine.MotorIndex} Axis Servo On Operation Failed.");
    //                    // AlarmMsg: 인스펙션 스테이지 X축 서보 온 동작 하지 못했습니다.
    //                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmHomeMotionFail;
    //                    break;
    //                }
    //                Thread.Sleep(SERVO_RESET_DELAY_TIME);
    //            }

    //            bool bNeedHomeProcess = (
    //                motor.HLGetMotorStatus().bHome == false
    //                || motor.HLGetMotorStatus().dEncoderPosition != motor.HLGetMotorStatus().dCommandPosition
    //                || IsNeedOrigin == true
    //                );
    //            if (
    //                true == motor.HLGetMotorOperationParameter().bUseHome
    //                && true == bNeedHomeProcess
    //                )
    //            {
    //                // 홈 동작
    //                SetProcessLog("HomeProcess");
    //                if (false == motor.HLSetHomeProcess())
    //                {
    //                    SetProcessLog("HomeProcess -> Error " + mDefine.AlarmHomeMotionFail.ToString());
    //                    m_objDocument.SetInitializeErrorLog($"{mDefine.MotorIndex} Axis Home Motion Failed.");
    //                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmHomeMotionFail;
    //                    break;
    //                }

    //                // 홈 동작 완료
    //                SetProcessLog("WaitForHomeEnd");
    //                if (false == WaitHomeComplete(motor, motor.HLGetMotorOperationParameter().iStandardLimitTimeOut))
    //                {
    //                    SetProcessLog("WaitForHomeEnd -> TimeOut " + mDefine.AlarmHomeMotionFail.ToString());
    //                    m_objDocument.SetInitializeErrorLog($"{mDefine.MotorIndex} Axis Home Motion Timeout.");
    //                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmHomeMotionFail;
    //                    break;
    //                }
    //                IsNeedOrigin = false;
    //            }

    //            //// 0 위치로 이동 (컨베이어 미사용)
    //            //SetProcessLog("MoveProcess -> Zero Position");
    //            //motor.HLMoveAbsolutePosition(0d);
    //            //SetProcessLog("WaitForMoveEnd");
    //            //if (false == motor.HLWaitMotorPositionStatus(motor, CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL, 0d, motor.HLGetMotorOperationParameter().iStandardLimitTimeOut))
    //            //{
    //            //    SetProcessLog("WaitForMoveEnd -> Error");
    //            //    m_objDocument.SetInitializeErrorLog($"{mDefine.MotorIndex} Axis Move Zero Position Failed.");
    //            //    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmHomeMotionFail;
    //            //    break;
    //            //}

    //            bReturn = true;
    //        } while (false);

    //        SetProcessLog("[END] " + bReturn);
    //        return bReturn;
    //    }

    //    private bool SetZeroPosition()
    //    {
    //        bool bReturn = false;
    //        m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
    //        SetProcessLog("[START]");

    //        do
    //        {
    //            // 속도 모드 설정 (컨베이어는 항상 오토 모드 속도 적용)
    //            m_eMoveVelocityType = CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_AUTO_RUN;

    //            // 정지
    //            if (false == Axis.HLMoveEStop())
    //            {
    //                SetProcessLog("HLMoveEStop -> Error {0}", mDefine.AlarmPositionResetFail.ToString());
    //                m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmPositionResetFail;
    //                break;
    //            }

    //            // 정지 후 슬립이 필요함
    //            // !여기에 슬립이 없으면 이상 동작함
    //            WaitForMotorInposition();

    //            // Position Zero Set
    //            if (false == Axis.HLSetPositionZeroSet())
    //            {
    //                SetProcessLog("HLSetPositionZeroSet -> Error {0}", mDefine.AlarmPositionResetFail.ToString());
    //                m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmPositionResetFail;
    //                break;
    //            }

    //            bReturn = true;
    //        } while (false);
    //        SetProcessLog("[END] " + bReturn);
    //        return bReturn;
    //    }

    //    private void WaitForMotorInposition(int timeoutSec = 2, int sleepTime = 10)
    //    {
    //        Thread.Sleep(20);
    //        if (
    //            Axis.HLGetMotorStatus().dEncoderPosition == 0d
    //            && Axis.HLGetMotorStatus().bInposition == true
    //            )
    //        {
    //            return;
    //        }

    //        var timeoutSw = Stopwatch.StartNew();
    //        TimeSpan timeout = TimeSpan.FromSeconds(timeoutSec);
    //        int targetCount = Axis.HLGetMotorOperationParameter().iDelayAfterMoving / sleepTime + 1;
    //        int inpositionCount = 0;
    //        while (targetCount > inpositionCount)
    //        {
    //            Thread.Sleep(sleepTime);
    //            if (timeoutSw.Elapsed > timeout)
    //            {
    //                break;
    //            }
    //            if (true == Axis.HLGetMotorStatus().bInposition)
    //            {
    //                inpositionCount++;
    //            }
    //            else
    //            {
    //                inpositionCount = 0;
    //            }
    //        }
    //        timeoutSw.Stop();
    //    }

    //    private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
    //    {
    //        m_objDocument.SetUpdateLog(
    //            mDefine.LogType, string.Format($"{callerMemberName}({mDefine.MotorIndex}) -> {logMessage}")
    //            );
    //    }

    //    private void ThreadProcess()
    //    {
    //        while (false == mbThreadExit)
    //        {
    //            if (ECommand.Idle != mCommand)
    //            {
    //                RaiseMccLogWhenJobsBegined();
    //                mStatus = EStatus.Unknown;
    //                if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
    //                {
    //                    Thread.Sleep(CDefine.DEF_SIMULATION_SLEEP_TIME);
    //                }
    //                switch (mCommand)
    //                {
    //                    case ECommand.Home:
    //                        if (true == SetHome())
    //                        {
    //                            mStatus = EStatus.Home;
    //                        }
    //                        break;

    //                    case ECommand.PositionZeroSet:
    //                        if (true == SetZeroPosition())
    //                        {
    //                            mStatus = EStatus.PositionZeroSet;
    //                        }
    //                        break;

    //                    case ECommand.WaitInterlock:
    //                        mStatus = EStatus.WaitInterlock;
    //                        break;
    //                }

    //                if (ECommand.Stop == mCommand)
    //                {
    //                    // 사용자 정지 체크
    //                    mStatus = EStatus.Stop;
    //                }
    //                else
    //                {
    //                    // 날라온 Command에 대한 알람이 있었는지 체크. STS_UNKNOWN 그대로이면 알람.
    //                    if (EStatus.Unknown == mStatus)
    //                    {
    //                        mStatus = EStatus.Error;
    //                        m_objDocument.SetAlarmEvent(m_objAlarmStructure);
    //                        int iReturn = (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP;
    //                        // 알람에 대해 작업자 입력에 따른 처리
    //                        switch (iReturn)
    //                        {
    //                            // 해당 커맨드에 대한 재시도
    //                            case (int)CDefine.EErrorProcess.ERROR_PROCESS_RETRY:
    //                                continue;
    //                            // 작업자가 알람 처리를 정상적으로 했다는 가정 하에 커맨드에 맞는 상태로 변경
    //                            case (int)CDefine.EErrorProcess.ERROR_PROCESS_CONTINUE:
    //                                if (ECommand.Home == mCommand)
    //                                {
    //                                    mStatus = EStatus.Home;
    //                                }
    //                                else if (ECommand.PositionZeroSet == mCommand)
    //                                {
    //                                    mStatus = EStatus.PositionZeroSet;
    //                                }
    //                                else if (ECommand.WaitInterlock == mCommand)
    //                                {
    //                                    mStatus = EStatus.WaitInterlock;
    //                                }
    //                                break;
    //                            // 에러에 대한 설비 정지
    //                            case (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP:
    //                                mCommand = ECommand.Stop;
    //                                continue;
    //                        }
    //                    }
    //                }
    //                RaiseMccLogWhenJobsFinished();
    //                mCommand = ECommand.Idle;
    //            }
    //            if (IsNeedOrigin == false
    //                && (Axis.HLGetMotorStatus().bServo == CDefine.OFF || Axis.HLGetMotorStatus().bAlarm == CDefine.ON)
    //                )
    //            {
    //                IsNeedOrigin = true;
    //            }
    //            Thread.Sleep(10);
    //        }
    //    }
    //}
}
using HLDevice;
using HLDevice.Abstract;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class OutFlipMotorZ : CProcessAbstract, IEmsHandle
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            Home,
            UpPosition,
            DownPosition,
            WaitInterlock
        };

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            Home,
            UpPosition,
            DownPosition,
            WaitInterlock
        };

        public enum EPosition
        {
            Up,
            Down,
        };

        public CDeviceMotor Axis { get; private set; }
        public CProcessMotion.EMotor MotorIndex => mDefine.MotorIndex;
        public bool IsNeedOrigin { get; private set; } = true;
        public EEmsGroupFlags EmergencyStopGroup => EEmsGroupFlags.Unloader;
        private const string ID = nameof(OutFlipMotorZ);
        private Dictionary<ECommand, MotorPositionSet<ECommand, EPosition>> mCommandMapping;
        private ECommand mCommand;
        private EStatus mStatus;
        private const int SERVO_RESET_DELAY_TIME = 200;
        private const int SERVO_CHANGE_DELAY_TIME = 3000;
        public override string ToString() => $"{ID},{MotorIndex},{mCommand},{mStatus}";

        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmObject = ID;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                mCommandMapping = new Dictionary<ECommand, MotorPositionSet<ECommand, EPosition>>()
                {
                    [ECommand.UpPosition] = new MotorPositionSet<ECommand, EPosition>(ECommand.UpPosition, EPosition.Up),
                    [ECommand.DownPosition] = new MotorPositionSet<ECommand, EPosition>(ECommand.DownPosition, EPosition.Down),
                };

                m_objDocument = document;
                m_objConfig = m_objDocument.m_objConfig;
                mDefine = new Define(position);

                Axis = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[mDefine.MotorIndex];

                // 인터락 객체 생성
                m_objInterlock = new CProcessInterlock(new InShuttleInterlock(m_objDocument));
                if (false == m_objInterlock.Initialize())
                {
                    break;
                }

                // 쓰레드 생성 및 시작
                mThreadProcess = new Thread(ThreadProcess);
                mThreadProcess.Start();

                m_objDocument.m_objProcessMain.m_objProcessMotion.AllMotions.Add(this);
                m_objDocument.m_objProcessMain.m_objProcessMotion.MotorInterlocks[mDefine.MotorIndex] = m_objInterlock;

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

        /// <summary>
        /// 실제 모터가 해당 포지션 위치에 도착해 있는지 체크함
        /// </summary>
        /// <param name="command">포지션</param>
        /// <returns>true=포지션 위치, false=포지션 위치가 아님 or 알 수 없음</returns>
        /// <remarks>인터락에서 해당 포지션 위치에 실제로 도착해 있는지 체크할 때 사용하기 위해 구현함</remarks>
        public bool IsInposition(ECommand command)
        {
            var motor = Axis;
            var motorOpParam = motor.HLGetMotorOperationParameter();
            var motorStatus = motor.HLGetMotorStatus();
            if (motorOpParam.bUseHome == true
                && motorStatus.bHome == false
                )
            {
                return false;
            }
            MotorPositionSet<ECommand, EPosition> positionInfo = mCommandMapping[command];
            double targetPosition = motor.HLGetMotorPosition().dPosition[(int)positionInfo.Position] + positionInfo.Offset;
            return Math.Abs(motor.HLGetMotorStatus().dEncoderPosition - targetPosition) <= motor.HLGetMotorOperationParameter().dStandardTolerence;
        }

        /// <summary>
        /// 커맨드의 목표위치를 구함
        /// </summary>
        /// <param name="command">포지션</param>
        /// <returns>커맨드의 목표위치</returns>
        /// <remarks>인터락에서 해당 포지션 위치를 가져오기위해 사용함</remarks>
        public double GetCommandTargetPosition(ECommand command)
        {
            var motor = Axis;
            MotorPositionSet<ECommand, EPosition> positionInfo = mCommandMapping[command];
            return motor.HLGetMotorPosition().dPosition[(int)positionInfo.Position] + positionInfo.Offset;
        }

        public EPosition GetPositionIndexFromCommand(ECommand command)
        {
            MotorPositionSet<ECommand, EPosition> positionInfo = mCommandMapping[command];
            return positionInfo.Position;
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

        public void EmergencyStop()
        {
            var motor = Axis;
            if (motor.HLGetMotorStatus().bInposition == true)
            {
                return;
            }
            motor.HLMoveEStop();
        }

        protected override void OnBootUpAction()
        {
            // MC가 꺼져 있으면 스킵
            if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON) == false)
            {
                return;
            }

            var motor = Axis;
            // 서보 OFF
            motor.SetServoAndWaitChanged(CDefine.OFF, SERVO_CHANGE_DELAY_TIME);
            // 알람 리셋
            motor.HLAlarmReset();
            // 서보 ON
            motor.SetServoAndWaitChanged(CDefine.ON, SERVO_CHANGE_DELAY_TIME);
        }

        protected override bool SetHome()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            do
            {
                int alarmResetRetryCount = 0;
                var motor = Axis;
                while (
                    motor.HLGetMotorStatus().bAlarm == true
                    && alarmResetRetryCount < 5
                    )
                {
                    // 서보 Off
                    SetProcessLog("SetServoDisable");
                    motor.SetServoAndWaitChanged(CDefine.OFF, SERVO_CHANGE_DELAY_TIME);
                    // 알람 리셋
                    SetProcessLog("SetAlarmReset");
                    motor.HLAlarmReset();
                    Thread.Sleep(SERVO_RESET_DELAY_TIME);
                    // 알람 클리어 확인
                    if (motor.HLGetMotorStatus().bAlarm == false)
                    {
                        break;
                    }
                    // 서보 On
                    SetProcessLog("SetServoEnable");
                    motor.SetServoAndWaitChanged(CDefine.ON, SERVO_CHANGE_DELAY_TIME);
                    // 알람 리셋
                    SetProcessLog("SetAlarmReset");
                    motor.HLAlarmReset();

                    alarmResetRetryCount++;
                }

                motor.HLSetSoftwareLimit(CDeviceMotorAbstract.EUse.ENABLE);
                if (motor.HLGetMotorStatus().bServo == CDefine.OFF)
                {
                    // 서보 온
                    SetProcessLog("SetServoEnable");
                    if (false == motor.SetServoAndWaitChanged(CDefine.ON, SERVO_CHANGE_DELAY_TIME))
                    {
                        SetProcessLog("SetServoEnable -> Error " + mDefine.AlarmHomeMotionFail.ToString());
                        m_objDocument.SetInitializeErrorLog($"{mDefine.MotorIndex} Axis Servo On Operation Failed.");
                        // AlarmMsg: 인스펙션 스테이지 X축 서보 온 동작 하지 못했습니다.
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmHomeMotionFail;
                        break;
                    }
                    Thread.Sleep(SERVO_RESET_DELAY_TIME);
                }

                bool bNeedHomeProcess = (
                    motor.HLGetMotorStatus().bHome == false
                    || motor.HLGetMotorStatus().dEncoderPosition != motor.HLGetMotorStatus().dCommandPosition
                    || IsNeedOrigin == true
                    );
                if (
                    true == motor.HLGetMotorOperationParameter().bUseHome
                    && true == bNeedHomeProcess
                    )
                {
                    // 홈 동작
                    SetProcessLog("HomeProcess");
                    if (false == motor.HLSetHomeProcess())
                    {
                        SetProcessLog("HomeProcess -> Error " + mDefine.AlarmHomeMotionFail.ToString());
                        m_objDocument.SetInitializeErrorLog($"{mDefine.MotorIndex} Axis Home Motion Failed.");
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmHomeMotionFail;
                        break;
                    }

                    // 홈 동작 완료
                    SetProcessLog("WaitForHomeEnd");
                    if (false == WaitHomeComplete(motor, motor.HLGetMotorOperationParameter().iStandardLimitTimeOut))
                    {
                        SetProcessLog("WaitForHomeEnd -> TimeOut " + mDefine.AlarmHomeMotionFail.ToString());
                        m_objDocument.SetInitializeErrorLog($"{mDefine.MotorIndex} Axis Home Motion Timeout.");
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmHomeMotionFail;
                        break;
                    }
                    IsNeedOrigin = false;
                }

                // 0 위치로 이동
                SetProcessLog("MoveProcess -> Zero Position");
                motor.HLMoveAbsolutePosition(0d);
                SetProcessLog("WaitForMoveEnd");
                if (false == motor.HLWaitMotorPositionStatus(motor, CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL, 0d, motor.HLGetMotorOperationParameter().iStandardLimitTimeOut))
                {
                    SetProcessLog("WaitForMoveEnd -> Error");
                    m_objDocument.SetInitializeErrorLog($"{mDefine.MotorIndex} Axis Move Zero Position Failed.");
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmHomeMotionFail;
                    break;
                }

                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn);
            return bReturn;
        }

        private bool SetMotorRelativePosition(ECommand command, double relativePosition, double velocity, double acceleration, double deceleration, CAlarmDefine.EAlarmList alarmCode, bool bShouldDriveAutoRunSpeed = false)
        {
            var axis = Axis;
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog($"[START] {command}");

            do
            {
                if (bShouldDriveAutoRunSpeed == true)
                {
                    // 속도 모드 설정 (실제 런과 검사 컨디션을 맞추기 위해 검사는 항상 오토 모드 속도 적용)
                    m_eMoveVelocityType = CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_AUTO_RUN;
                }
                else
                {
                    // 속도 모드 설정
                    SetVelocityMode();
                }

                double targetPosition = axis.HLGetMotorStatus().dEncoderPosition + relativePosition;
                SetProcessLog($"MoveRelativeProcess -> {targetPosition:0.000} ({relativePosition:0.000})");
                axis.HLMoveAbsolutePosition(targetPosition, velocity, acceleration, deceleration);
                SetProcessLog("WaitForMoveEnd");
                if (false == axis.HLWaitMotorPositionStatus(axis, CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL, targetPosition, axis.HLGetMotorOperationParameter().iStandardLimitTimeOut))
                {
                    SetProcessLog("WaitForMoveEnd -> Error");
                    m_objAlarmStructure.iAlarmCode = (int)alarmCode;
                    break;
                }

                Thread.Sleep(axis.HLGetMotorOperationParameter().iDelayAfterMoving);

                bReturn = true;
            } while (false);

            SetProcessLog($"[END] {command} {bReturn}");
            return bReturn;
        }

        private bool SetMotorPosition(MotorPositionSet<ECommand, EPosition> positionInfo, CAlarmDefine.EAlarmList alarmCode, bool bShouldDriveAutoRunSpeed = false)
        {
            var axis = Axis;
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog($"[START] {positionInfo.Command}");

            do
            {
                if (bShouldDriveAutoRunSpeed == true)
                {
                    // 속도 모드 설정 (실제 런과 검사 컨디션을 맞추기 위해 검사는 항상 오토 모드 속도 적용)
                    m_eMoveVelocityType = CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_AUTO_RUN;
                }
                else
                {
                    // 속도 모드 설정
                    SetVelocityMode();
                }

                double targetPosition = axis.HLGetMotorPosition().dPosition[(int)positionInfo.Position] + positionInfo.Offset;
                SetProcessLog($"MoveProcess -> {positionInfo.Position} -> {targetPosition:0.000} ({positionInfo.Offset:0.000})");
                axis.HLMoveAbsolutePosition((int)positionInfo.Position, targetPosition, m_eMoveVelocityType);
                SetProcessLog("WaitForMoveEnd");
                if (false == axis.HLWaitMotorPositionStatus(axis, CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL, targetPosition, axis.HLGetMotorOperationParameter().iStandardLimitTimeOut))
                {
                    SetProcessLog("WaitForMoveEnd -> Error");
                    m_objAlarmStructure.iAlarmCode = (int)alarmCode;
                    break;
                }

                Thread.Sleep(axis.HLGetMotorOperationParameter().iDelayAfterMoving);

                bReturn = true;
            } while (false);

            SetProcessLog($"[END] {positionInfo.Command} {bReturn}");
            return bReturn;
        }

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                mDefine.LogType, string.Format($"{callerMemberName}({mDefine.MotorIndex}) -> {logMessage}")
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
                        case ECommand.Home:
                            if (true == SetHome())
                            {
                                mStatus = EStatus.Home;
                            }
                            break;

                        case ECommand.UpPosition:
                            if (SetMotorPosition(mCommandMapping[mCommand], mDefine.AlarmUpPositionMoveFail) == true)
                            {
                                mStatus = EStatus.UpPosition;
                            }
                            break;

                        case ECommand.DownPosition:
                            if (SetMotorPosition(mCommandMapping[mCommand], mDefine.AlarmDownPositionMoveFail) == true)
                            {
                                mStatus = EStatus.DownPosition;
                            }
                            break;

                        case ECommand.WaitInterlock:
                            mStatus = EStatus.WaitInterlock;
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
                                    if (ECommand.Home == mCommand)
                                    {
                                        mStatus = EStatus.Home;
                                    }
                                    else if (ECommand.UpPosition == mCommand)
                                    {
                                        mStatus = EStatus.UpPosition;
                                    }
                                    else if (ECommand.DownPosition == mCommand)
                                    {
                                        mStatus = EStatus.DownPosition;
                                    }
                                    else if (ECommand.WaitInterlock == mCommand)
                                    {
                                        mStatus = EStatus.WaitInterlock;
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
                if (IsNeedOrigin == false
                    && (Axis.HLGetMotorStatus().bServo == CDefine.OFF || Axis.HLGetMotorStatus().bAlarm == CDefine.ON)
                    )
                {
                    IsNeedOrigin = true;
                }
                Thread.Sleep(10);
            }
        }
    }
}
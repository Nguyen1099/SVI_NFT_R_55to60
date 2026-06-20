using HLDevice;
using HLDevice.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class InShuttleMotorX : CProcessAbstract, IEmsHandle
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            Home,
            LoadPosition,
            ScanStartWaitPosition,
            ScanTriggerStartPosition,
            ScanTriggerEndPosition,
            Scan,
            AlignPosition,
            UnloadPosition,
            WaitInterlock
        };

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            Home,
            LoadPosition,
            ScanStartWaitPosition,
            ScanTriggerStartPosition,
            ScanTriggerEndPosition,
            Scan,
            AlignPosition,
            UnloadPosition,
            WaitInterlock
        };

        public enum EPosition
        {
            Load,
            ScanStartWait,
            ScanTriggerStart,
            ScanTriggerEnd,
            Align,
            Unload
        };

        public CDeviceMotor Axis { get; private set; }
        public CDeviceMotor TriggerAxis { get; private set; }
        public CProcessMotion.EMotor MotorIndex => mDefine.MotorIndex;
        public CProcessMotion.EMotor TriggerModuleIndex => mDefine.TriggerModuleIndex;
        public bool IsNeedOrigin { get; private set; } = true;
        public EEmsGroupFlags EmergencyStopGroup => EEmsGroupFlags.Loader;
        private const string ID = nameof(InShuttleMotorX);
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
                    [ECommand.LoadPosition] = new MotorPositionSet<ECommand, EPosition>(ECommand.LoadPosition, EPosition.Load),
                    [ECommand.ScanStartWaitPosition] = new MotorPositionSet<ECommand, EPosition>(ECommand.ScanStartWaitPosition, EPosition.ScanStartWait),
                    [ECommand.ScanTriggerStartPosition] = new MotorPositionSet<ECommand, EPosition>(ECommand.ScanTriggerStartPosition, EPosition.ScanTriggerStart),
                    [ECommand.ScanTriggerEndPosition] = new MotorPositionSet<ECommand, EPosition>(ECommand.ScanTriggerEndPosition, EPosition.ScanTriggerEnd),
                    [ECommand.Scan] = new MotorPositionSet<ECommand, EPosition>(ECommand.Scan, EPosition.Align),
                    [ECommand.AlignPosition] = new MotorPositionSet<ECommand, EPosition>(ECommand.AlignPosition, EPosition.Align),
                    [ECommand.UnloadPosition] = new MotorPositionSet<ECommand, EPosition>(ECommand.UnloadPosition, EPosition.Unload),
                };

                m_objDocument = document;
                m_objConfig = m_objDocument.m_objConfig;
                mDefine = new Define(position);

                Axis = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[mDefine.MotorIndex];
                TriggerAxis = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[mDefine.TriggerModuleIndex];

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

        public override bool WaitForCommandSync(object syncTypeOrNull = null)
        {
            if (syncTypeOrNull == null)
            {
                return true;
            }

            var axis = Axis;
            long startTime = Utils.GetTimestamp();
            long timeout = TimeSpan.FromMilliseconds(axis.HLGetMotorOperationParameter().iStandardLimitTimeOut).Ticks;
            if (syncTypeOrNull.ToString() == Constants.PASS_BY_SCAN_TRIGGER_END_POSITION)
            {
                // 가상 모드 처리
                if (m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
                {
                    Thread.Sleep(100);
                    return true;
                }

                // 트리거 종료 위치 통과 대기
                double triggerStartPosition = axis.HLGetMotorPosition().dPosition[(int)EPosition.ScanTriggerStart];
                double triggerEndPosition = axis.HLGetMotorPosition().dPosition[(int)EPosition.ScanTriggerEnd];
                bool bIsMovePositiveDirection = (triggerStartPosition - triggerEndPosition) < 0;
                var spinDelay = new SpinWait();
                while (true)
                {
                    spinDelay.SpinOnce();

                    if (axis.HLGetMotorStatus().bAlarm == true
                        || axis.HLGetMotorStatus().bServo == false
                        )
                    {
                        return false;
                    }
                    if (Utils.GetTimestamp() - startTime > timeout)
                    {
                        return false;
                    }
                    double positionDelta = axis.HLGetMotorStatus().dEncoderPosition - triggerEndPosition;
                    bool bIsPassByTriggerEndPosition = bIsMovePositiveDirection ? positionDelta >= 0 : positionDelta <= 0;
                    if (bIsPassByTriggerEndPosition == true)
                    {
                        break;
                    }
                }
            }

            return true;
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

                // + 트리거 모듈에 현재 위치를 0으로 설정함
                CDeviceMotor triggerAxis;
                if (HasTriggerAxis(out triggerAxis) == true)
                {
                    SetProcessLog("TriggerModuleZeroSet");
                    triggerAxis.HLSetPositionZeroSet();
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

        private bool SetMotorOverrideMove(MotorPositionSet<ECommand, EPosition> arrivalPositionInfo, CAlarmDefine.EAlarmList alarmCode, bool bShouldDriveAutoRunSpeed, params EPosition[] overridePositionIndexes)
        {
            var axis = Axis;
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog($"[START] {arrivalPositionInfo.Command}");

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

                var teachingData = axis.HLGetMotorPosition();
                int[] alignedPositionIndexes = overridePositionIndexes.Concat(new EPosition[] { arrivalPositionInfo.Position })
                    .Cast<int>()
                    .ToArray();

                // + 오버라이드 프로파일 설정
                int startPositionIndex = alignedPositionIndexes.First();
                int arrivalPositionIndex = alignedPositionIndexes.Last();
                double maxVelocity = alignedPositionIndexes.Max(i => teachingData.dVelocity[i]);
                double startVelocity = teachingData.dVelocity[startPositionIndex];
                double acceleration = alignedPositionIndexes.Max(i => teachingData.dAcceleration[i]);
                double deceleration = alignedPositionIndexes.Max(i => teachingData.dDeceleration[i]);
                double[] overrideVelocities = alignedPositionIndexes.Skip(1)
                    .Select(i => teachingData.dVelocity[i])
                    .ToArray();
                double[] overridePositions = overrideVelocities.Length != 0 ? alignedPositionIndexes.Take(alignedPositionIndexes.Length - 1)
                    .Select(i => teachingData.dPosition[i])
                    .ToArray()
                    // ! 변속 구간이 없을 때 예외처리
                    : new double[0];
                double arrivalPosition = teachingData.dPosition[arrivalPositionIndex] + arrivalPositionInfo.Offset;

                // + 오버라이드 구동 시작
                SetProcessLog($"OverrideMoveProcess -> {(EPosition)arrivalPositionIndex} -> {arrivalPosition:0.000} ({arrivalPositionInfo.Offset:0.000})");
                bool bOverrideCommandResult = axis.HLMoveMultiOverrideAtPosition(
                    dPosition: arrivalPosition,
                    eModeType: m_eMoveVelocityType,
                    dOverridePosition: overridePositions,
                    dOverrideVelocity: overrideVelocities,
                    dMaxOverrideVelocity: maxVelocity,
                    dVelocity: startVelocity,
                    dAcceleration: acceleration,
                    dDeceleration: deceleration
                    );
                if (bOverrideCommandResult == false)
                {
                    SetProcessLog("OverrideMoveProcess -> Error");
                    m_objAlarmStructure.iAlarmCode = (int)alarmCode;
                    break;
                }

                // + 이동 완료 대기
                SetProcessLog("WaitForMoveEnd");
                if (axis.HLWaitMotorPositionStatus(axis, CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL, arrivalPosition, axis.HLGetMotorOperationParameter().iStandardLimitTimeOut) == false)
                {
                    SetProcessLog("WaitForMoveEnd -> Error");
                    m_objAlarmStructure.iAlarmCode = (int)alarmCode;
                    break;
                }

                Thread.Sleep(axis.HLGetMotorOperationParameter().iDelayAfterMoving);

                bReturn = true;
            } while (false);

            SetProcessLog($"[END] {arrivalPositionInfo.Command} {bReturn}");
            return bReturn;
        }

        private bool HasTriggerAxis(out CDeviceMotor motorOrNull)
        {
            motorOrNull = null;
            if (mDefine.IsUsingTrigger == false)
            {
                return false;
            }

            motorOrNull = TriggerAxis;
            return true;
        }

        private void SetTriggerBlock(EPosition startPosition, EPosition endPosition, double offset = 0d)
        {
            CDeviceMotor triggerAxis;
            if (HasTriggerAxis(out triggerAxis) == false)
            {
                return;
            }

            var positionInfo = Axis.HLGetMotorPosition();
            // 트리거 현재위치 맞춤
            triggerAxis.HLSetActualPosition(Axis.HLGetMotorStatus().dEncoderPosition);
            double triggerStart = positionInfo.dPosition[(int)startPosition] + offset;
            double triggerEnd = positionInfo.dPosition[(int)endPosition] + offset;
            double pulseWidthTime = m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).dPulseWidth;
            // ㎛ -> ㎜ 변환
            double pulsePeriodLength = m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).dPeriod / 1000d;

            // 시작전 트리거 설정
            SetProcessLog(string.Format("SetTriggerTime -> Time:{0}us", pulseWidthTime));
            triggerAxis.HLSetTriggerTime(pulseWidthTime);
            SetProcessLog(string.Format("SetTriggerBlock -> Start:{0}mm,End:{1}mm,Period:{2}mm", triggerStart, triggerEnd, pulsePeriodLength));
            triggerAxis.HLSetTriggerBlock(dStartPosition: triggerStart, dEndPosition: triggerEnd, dPeriodPosition: pulsePeriodLength);
        }

        private void ResetTriggerBlock()
        {
            CDeviceMotor triggerAxis;
            if (HasTriggerAxis(out triggerAxis) == false)
            {
                return;
            }

            SetProcessLog(string.Format("ResetTriggerBlock"));
            triggerAxis.HLSetTriggerBlockReset();
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

                        case ECommand.LoadPosition:
                            if (SetMotorPosition(mCommandMapping[mCommand], mDefine.AlarmLoadPositionMoveFail) == true)
                            {
                                mStatus = EStatus.LoadPosition;
                            }
                            break;

                        case ECommand.ScanStartWaitPosition:
                            if (SetMotorPosition(mCommandMapping[mCommand], mDefine.AlarmScanStartWaitPositionMoveFail) == true)
                            {
                                mStatus = EStatus.ScanStartWaitPosition;
                            }
                            break;

                        case ECommand.ScanTriggerStartPosition:
                            if (SetMotorPosition(mCommandMapping[mCommand], mDefine.AlarmScanTriggerStartPositionMoveFail) == true)
                            {
                                mStatus = EStatus.ScanTriggerStartPosition;
                            }
                            break;

                        case ECommand.ScanTriggerEndPosition:
                            if (SetMotorPosition(mCommandMapping[mCommand], mDefine.AlarmScanTriggerEndPositionMoveFail) == true)
                            {
                                mStatus = EStatus.ScanTriggerEndPosition;
                            }
                            break;

                        case ECommand.Scan:
                            // + 트리거 블록 설정
                            SetTriggerBlock(EPosition.ScanTriggerStart, EPosition.ScanTriggerEnd);
                            // + 시작점 ~ OutRobot 구간 오버라이드 이동
                            //  1. 시작점 ~ ScanTriggerEnd 구간은 ScanTriggerEnd 이동 속도 적용
                            //  2. ScanTriggerEnd ~ OutRobot 구간은 OutRobot 이동 속도 적용
                            if (SetMotorOverrideMove(mCommandMapping[mCommand], mDefine.AlarmScanTriggerEndPositionMoveFail, /*bShouldDriveAutoRunSpeed:*/ true, EPosition.ScanTriggerEnd) == true)
                            {
                                mStatus = EStatus.Scan;
                            }
                            // + 트리거 블록 해제
                            ResetTriggerBlock();
                            break;

                        case ECommand.AlignPosition:
                            if (SetMotorPosition(mCommandMapping[mCommand], mDefine.AlarmAlignPositionMoveFail) == true)
                            {
                                mStatus = EStatus.AlignPosition;
                            }
                            break;

                        case ECommand.UnloadPosition:
                            if (SetMotorPosition(mCommandMapping[mCommand], mDefine.AlarmUnloadPositionMoveFail) == true)
                            {
                                mStatus = EStatus.UnloadPosition;
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
                                    else if (ECommand.LoadPosition == mCommand)
                                    {
                                        mStatus = EStatus.LoadPosition;
                                    }
                                    else if (ECommand.ScanStartWaitPosition == mCommand)
                                    {
                                        mStatus = EStatus.ScanStartWaitPosition;
                                    }
                                    else if (ECommand.ScanTriggerStartPosition == mCommand)
                                    {
                                        mStatus = EStatus.ScanTriggerStartPosition;
                                    }
                                    else if (ECommand.ScanTriggerEndPosition == mCommand)
                                    {
                                        mStatus = EStatus.ScanTriggerEndPosition;
                                    }
                                    else if (ECommand.Scan == mCommand)
                                    {
                                        mStatus = EStatus.Scan;
                                    }
                                    else if (ECommand.AlignPosition == mCommand)
                                    {
                                        mStatus = EStatus.AlignPosition;
                                    }
                                    else if (ECommand.UnloadPosition == mCommand)
                                    {
                                        mStatus = EStatus.UnloadPosition;
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
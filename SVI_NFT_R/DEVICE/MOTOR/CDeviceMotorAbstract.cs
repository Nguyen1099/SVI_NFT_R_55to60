using System;

namespace HLDevice.Abstract
{
    public abstract class CDeviceMotorAbstract
    {
        /// <summary>
        /// 최대 포지션 숫자
        /// </summary>
        public const int DEF_MAX_POSITION_COUNT = 50;
        public enum EVelocityMode
        {
            VELOCITY_MODE_AUTO_RUN = 0,
            VELOCITY_MODE_MANUAL_RUN
        };
        /// <summary>
        /// 위치 비교
        /// </summary>
        public enum EPositionCompare
        {
            POSITION_COMPARE_UNEQUAL = 0,
            POSITION_COMPARE_EQUAL,
            POSITION_COMPARE_UNKNOWN,
            POSITION_COMPARE_FINAL
        };
        /// <summary>
        /// 사용 유무
        /// </summary>
        public enum EUse
        {
            DISABLE = 0,
            ENABLE
        };
        /// <summary>
        /// 방향
        /// </summary>
        public enum EMoveDirection
        {
            DIRECTION_CCW = 0,
            DIRECTION_CW
        };
        /// <summary>
        /// 모션 레벨 설정
        /// </summary>
        public enum EMotionLevel
        {
            LEVEL_LOW = 0,
            LEVEL_HIGH,
            LEVEL_UNUSED,
            LEVEL_USED
        };
        /// <summary>
        /// 모션 방향
        /// </summary>
        public enum EMotionDirection
        {
            DIR_CCW = 0,
            DIR_CW
        };
        /// <summary>
        /// 홈 센서 감지
        /// </summary>
        public enum EHomeDetect
        {
            POS_END_LIMIT = 0,
            NEG_END_LIMIT = 1,
            HOME_SENSOR = 4,
            ENCODER_Z_PHASE = 5
        };

        /// <summary>
        /// 모터 알람 발생 확인 클래스
        /// </summary>
        public class CMotorError : ICloneable
        {
            /// <summary>
            /// 이벤트 발생 시간
            /// </summary>
            public string strEventTime;
            /// <summary>
            /// 수행된 함수 이름
            /// </summary>
            public string strFunctionName;
            /// <summary>
            /// 알람 리턴 결과
            /// </summary>
            public int iReturnCode;
            /// <summary>
            /// 메세지
            /// </summary>
            public string strMessage;

            public object Clone()
            {
                CMotorError objIOError = new CMotorError();
                objIOError.strEventTime = strEventTime;
                objIOError.strFunctionName = strFunctionName;
                objIOError.iReturnCode = iReturnCode;
                objIOError.strMessage = strMessage;

                return objIOError;
            }
        }

        /// <summary>
        /// 모터 포지션 데이터 클래스
        /// </summary>
        [Serializable]
        public class CMotorPosition : ICloneable
        {
            public string[] strPositionName;
            public double[] dPosition;
            public double[] dPrevPosition;
            public double[] dVelocity;
            public double[] dAcceleration;
            public double[] dDeceleration;
            public double[] dTolerence;

            // 생성자
            public CMotorPosition()
            {
                // 모터 포지션 정보 구조체 생성 ( 최대 포지션 설정 된만큼 생성 )
                strPositionName = new string[DEF_MAX_POSITION_COUNT];
                dPosition = new double[DEF_MAX_POSITION_COUNT];
                dPrevPosition = new double[DEF_MAX_POSITION_COUNT];
                dVelocity = new double[DEF_MAX_POSITION_COUNT];
                dAcceleration = new double[DEF_MAX_POSITION_COUNT];
                dDeceleration = new double[DEF_MAX_POSITION_COUNT];
                dTolerence = new double[DEF_MAX_POSITION_COUNT];
            }

            public object Clone()
            {
                CMotorPosition objMotorPosition = new CMotorPosition();
                for (int iLoopCount = 0; iLoopCount < DEF_MAX_POSITION_COUNT; iLoopCount++)
                {
                    objMotorPosition.strPositionName[iLoopCount] = strPositionName[iLoopCount];
                    objMotorPosition.dPosition[iLoopCount] = dPosition[iLoopCount];
                    objMotorPosition.dPrevPosition[iLoopCount] = dPrevPosition[iLoopCount];
                    objMotorPosition.dVelocity[iLoopCount] = dVelocity[iLoopCount];
                    objMotorPosition.dAcceleration[iLoopCount] = dAcceleration[iLoopCount];
                    objMotorPosition.dDeceleration[iLoopCount] = dDeceleration[iLoopCount];
                    objMotorPosition.dTolerence[iLoopCount] = dTolerence[iLoopCount];
                }
                return objMotorPosition;
            }
        }

        /// <summary>
        /// 모터 구동에 필요한 클래스 정보 ( 초기 셋팅 시 필요 )
        /// </summary>
        public class CMotorOperationParameter : ICloneable
        {
            public int iHomePriority;
            public EMotionLevel eMotionLevel;
            public EMotionDirection eHomeDirection;
            public EHomeDetect eHomeDetect;
            public double dHomeOffset;
            public int iUseZPhase;
            public int iHomeClearTime;
            public double dVelocityHomeFirst;
            public double dVelocityHomeSecond;
            public double dVelocityHomeThird;
            public double dVelocityHomeLast;
            public double dAccelerationHomeFirst;
            public double dAccelerationHomeSecond;

            public double dVelocityJogFast;
            public double dVelocityJogSlow;
            public double dStandardAutoVelocity;
            public double dStandardManualVelocity;
            public double dStandardAcceleration;
            public double dStandardDeceleration;
            public int iStandardLimitTimeOut;
            public double dStandardTolerence;
            public double dLimitSoftwarePlus;
            public double dLimitSoftwareMinus;
            public bool bUseHome;
            public int iDelayAfterMoving;

            public object Clone()
            {
                CMotorOperationParameter objMotorOperationParameter = new CMotorOperationParameter();
                objMotorOperationParameter.iHomePriority = iHomePriority;
                objMotorOperationParameter.eMotionLevel = eMotionLevel;
                objMotorOperationParameter.eHomeDirection = eHomeDirection;
                objMotorOperationParameter.eHomeDetect = eHomeDetect;
                objMotorOperationParameter.dHomeOffset = dHomeOffset;
                objMotorOperationParameter.iUseZPhase = iUseZPhase;
                objMotorOperationParameter.iHomeClearTime = iHomeClearTime;
                objMotorOperationParameter.dVelocityHomeFirst = dVelocityHomeFirst;
                objMotorOperationParameter.dVelocityHomeSecond = dVelocityHomeSecond;
                objMotorOperationParameter.dVelocityHomeThird = dVelocityHomeThird;
                objMotorOperationParameter.dVelocityHomeLast = dVelocityHomeLast;
                objMotorOperationParameter.dAccelerationHomeFirst = dAccelerationHomeFirst;
                objMotorOperationParameter.dAccelerationHomeSecond = dAccelerationHomeSecond;

                objMotorOperationParameter.dVelocityJogFast = dVelocityJogFast;
                objMotorOperationParameter.dVelocityJogSlow = dVelocityJogSlow;
                objMotorOperationParameter.dStandardAutoVelocity = dStandardAutoVelocity;
                objMotorOperationParameter.dStandardManualVelocity = dStandardManualVelocity;
                objMotorOperationParameter.dStandardAcceleration = dStandardAcceleration;
                objMotorOperationParameter.dStandardDeceleration = dStandardDeceleration;
                objMotorOperationParameter.iStandardLimitTimeOut = iStandardLimitTimeOut;
                objMotorOperationParameter.dStandardTolerence = dStandardTolerence;
                objMotorOperationParameter.dLimitSoftwareMinus = dLimitSoftwareMinus;
                objMotorOperationParameter.dLimitSoftwarePlus = dLimitSoftwarePlus;
                objMotorOperationParameter.bUseHome = bUseHome;
                objMotorOperationParameter.iDelayAfterMoving = iDelayAfterMoving;
                return objMotorOperationParameter;
            }
        }

        /// <summary>
        /// 초기화 파라미터
        /// </summary>
        public class CMotorInitializeParameter //: ICloneable
        {
            public int iAxisNo;
            public int iBoardNo;
            public int iPort;
            public string strIPAddress;
            public string strFilePath;
            public string strModelName;
            public string strMotorName;
            //public CDefine.enumMoveModeType eMoveModeType;

            public object Clone()
            {
                CMotorInitializeParameter objinitializeParameter = new CMotorInitializeParameter();
                // objinitializeParameter.eMoveModeType = this.eMoveModeType;
                objinitializeParameter.iAxisNo = iAxisNo;
                objinitializeParameter.iBoardNo = iBoardNo;
                objinitializeParameter.iPort = iPort;
                objinitializeParameter.strIPAddress = strIPAddress;
                objinitializeParameter.strFilePath = strFilePath;
                objinitializeParameter.strModelName = strModelName;
                objinitializeParameter.strMotorName = strMotorName;

                return objinitializeParameter;
            }
        }

        /// <summary>
        /// 모터 상태 정보
        /// </summary>
        public class CMotorStatus
        {
            /// <summary>
            /// 서보 온오프 상태
            /// </summary>
            public virtual bool bServo { get; } = false;
            /// <summary>
            /// 모터 홈 완료 상태
            /// </summary>
            public virtual bool bHome { get; } = false;
            /// <summary>
            /// 모터 인포지션 상태
            /// </summary>
            public virtual bool bInposition { get; } = false;
            /// <summary>
            /// 모터 알람 상태
            /// </summary>
            public virtual bool bAlarm { get; } = false;
            /// <summary>
            /// 모터 동작중 상태
            /// </summary>
            public virtual bool bBusy { get; } = false;
            /// <summary>
            /// 홈 센서 감지 상태
            /// </summary>
            public virtual bool bHomeSensor { get; } = false;
            /// <summary>
            /// - 리미트 센서 감지 상태
            /// </summary>
            public virtual bool bMinusLimitSensor { get; } = false;
            /// <summary>
            /// + 리미트 센서 감지 상태
            /// </summary>
            public virtual bool bPlusLimitSensor { get; } = false;
            /// <summary>
            /// 지령 위치
            /// </summary>
            public virtual double dCommandPosition { get; } = 0d;
            /// <summary>
            /// 엔코더 위치
            /// </summary>
            public virtual double dEncoderPosition { get; } = 0d;
            /// <summary>
            /// 지정 축의 부하율을 반환
            /// </summary>
            public virtual double dServoLoadRatio { get; } = 0d;
            /// <summary>
            /// 지정 축의 부하율을 반환
            /// </summary>
            public virtual double dCurrentVelocity { get; } = 0d;

            public CMotorStatus()
            {
            }

            public CMotorStatus(bool bServo, bool bHome, bool bInposition, bool bAlarm, bool bBusy, bool bHomeSensor, bool bMinusLimitSensor, bool bPlusLimitSensor, double dCommandPosition, double dEncoderPosition, double dServoLoadRatio, double dCurrentVelocity)
            {
                this.bServo = bServo;
                this.bHome = bHome;
                this.bInposition = bInposition;
                this.bAlarm = bAlarm;
                this.bBusy = bBusy;
                this.bHomeSensor = bHomeSensor;
                this.bMinusLimitSensor = bMinusLimitSensor;
                this.bPlusLimitSensor = bPlusLimitSensor;
                this.dCommandPosition = dCommandPosition;
                this.dEncoderPosition = dEncoderPosition;
                this.dServoLoadRatio = dServoLoadRatio;
                this.dCurrentVelocity = dCurrentVelocity;
            }
        }

        public class CMotorStatusFromFunction : CMotorStatus
        {
            public override bool bServo => mGetServo.Invoke();
            public override bool bHome => mGetHome.Invoke();
            public override bool bInposition => mGetInposition.Invoke();
            public override bool bAlarm => mGetAlarm.Invoke();
            public override bool bBusy => mGetBusy.Invoke();
            public override bool bHomeSensor => mGetHomeSensor.Invoke();
            public override bool bMinusLimitSensor => mGetMinusLimit.Invoke();
            public override bool bPlusLimitSensor => mGetPlusLimit.Invoke();
            public override double dCommandPosition => mGetCommandPosition.Invoke();
            public override double dEncoderPosition => mGetEncoderPosition.Invoke();
            public override double dServoLoadRatio => mGetServoLoadRatio.Invoke();
            public override double dCurrentVelocity => mCurrentVelocity.Invoke();
            private readonly Func<bool> mGetServo;
            private readonly Func<bool> mGetHome;
            private readonly Func<bool> mGetInposition;
            private readonly Func<bool> mGetAlarm;
            private readonly Func<bool> mGetBusy;
            private readonly Func<bool> mGetHomeSensor;
            private readonly Func<bool> mGetMinusLimit;
            private readonly Func<bool> mGetPlusLimit;
            private readonly Func<double> mGetCommandPosition;
            private readonly Func<double> mGetEncoderPosition;
            private readonly Func<double> mGetServoLoadRatio;
            private readonly Func<double> mCurrentVelocity;

            public CMotorStatusFromFunction(Func<bool> getServo, Func<bool> getHome, Func<bool> getInposition, Func<bool> getAlarm, Func<bool> getBusy, Func<bool> getHomeSensor, Func<bool> getMinusLimit, Func<bool> getPlusLimit, Func<double> getCommandPosition, Func<double> getEncoderPosition, Func<double> getServoLoadRatio, Func<double> getCurrentVelocity)
            {
                mGetServo = getServo;
                mGetHome = getHome;
                mGetInposition = getInposition;
                mGetAlarm = getAlarm;
                mGetBusy = getBusy;
                mGetHomeSensor = getHomeSensor;
                mGetMinusLimit = getMinusLimit;
                mGetPlusLimit = getPlusLimit;
                mGetCommandPosition = getCommandPosition;
                mGetEncoderPosition = getEncoderPosition;
                mGetServoLoadRatio = getServoLoadRatio;
                mCurrentVelocity = getCurrentVelocity;
            }
        }

        private string m_strAlarmFunction = "";

        /// <summary>
        /// 다축 운동 시 입력해야할 파라미터
        /// </summary>
        public class CMultiMoveParameter
        {
            public object objMotor;
            public double dAcceleration;
            public double dDeceleration;
            public double dPosition;
            public double dVelocity;
            public int iPositionIndex;
        }

        /// <summary>
        /// 티칭된 속도에 비율로 동작함 (Range: 10% ~ 120%)
        /// 예> 1000 mm/s, OverrideSpeedRate 50% =>500 mm/s
        /// </summary>
        public abstract int OverrideSpeedRate { get; set; }

        /// <summary>
        /// 초기화 추상 함수
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public abstract bool HLInitialize(CMotorInitializeParameter objInitializeParameter);

        /// <summary>
        /// 해제 추상 함수
        /// </summary>
        public abstract void HLDeInitialize();

        /// <summary>
        /// 버전 정보 추상 함수
        /// </summary>
        /// <returns></returns>
        public abstract string HLGetVersion();

        /// <summary>
        /// 모터 알람 리셋 추상 함수
        /// </summary>
        /// <returns></returns>
        public abstract bool HLAlarmReset();

        /// <summary>
        /// 모터 디바이스 종료 추상 함수
        /// </summary>
        /// <returns></returns>
        public abstract bool HLClose();

        /// <summary>
        /// 모터 포지션 비교 추상 함수 ( 포지션 값 )
        /// </summary>
        /// <param name="dPosition"></param>
        /// <returns></returns>
        public abstract EPositionCompare HLGetCompareTargetPosition(double dPosition);

        /// <summary>
        /// 모터 포지션 비교 추상 함수 ( 포지션 값 )
        /// </summary>
        /// <param name="dStartPosition"></param>
        /// <param name="dPosition"></param>
        /// <returns></returns>
        public abstract EPositionCompare HLGetCompareTargetPosition(double dStartPosition, double dPosition);

        /// <summary>
        /// 모터 포지션 비교 추상 함수 ( 인덱스 포지션 값 )
        /// </summary>
        /// <param name="iPositionIndex"></param>
        /// <returns></returns>
        public abstract EPositionCompare HLGetCompareTargetPosition(int iPositionIndex);

        /// <summary>
        /// 홈 동작 추상 함수
        /// </summary>
        /// <returns></returns>
        public abstract bool HLSetHomeProcess();

        /// <summary>
        /// 홈 동작 진행 률 가상 함수
        /// </summary>
        /// <param name="iHomeMainStepNo"></param>
        /// <param name="iHomeStepNo"></param>
        /// <returns></returns>
        public virtual bool HLGetHomeProcessRate(ref int iHomeMainStepNo, ref int iHomeStepNo)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLGetHomeProcessRate";

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 서보 ON/OFF 추상 함수
        /// </summary>
        /// <param name="eUse"></param>
        /// <returns></returns>
        public abstract bool HLServoOn(EUse eUse);
        /// <summary>
        /// 서보 On/Off 상태 확인
        /// </summary>
        /// <param name="SetValue"></param>
        /// <param name="iTimeOut"></param>
        /// <returns></returns>
        public abstract bool SetServoAndWaitChanged(bool setValue, int timeout);
        /// <summary>
        /// 포지션 ZeroSet
        /// </summary>
        /// <returns></returns>
        public abstract bool HLSetPositionZeroSet();
        /// <summary>
        /// Actual 포지션 {dPosition}Set
        /// </summary>
        /// <returns></returns>
        public abstract bool HLSetActualPosition(double dPosition);

        /// <summary>
        /// 모터 unit/pulse Write
        /// </summary>
        /// <param name="dUnit"></param>
        /// <param name="iPulse"></param>
        /// <returns></returns>
        public abstract bool HLSetUnitPerPulse(double dUnit, int iPulse);

        /// <summary>
        /// 모터 unit/pulse Read
        /// </summary>
        /// <param name="dUnit"></param>
        /// <param name="iPulse"></param>
        /// <returns></returns>
        public abstract bool HLGetUnitperPulse(ref double dUnit, ref int iPulse);

        /// <summary>
        /// 홈 1차 검출 속도를 설정 한다. ( 검출 후 속도, 나머지 속도 는 초기 설정되어 있는 설정값 그대로 적용 )
        /// </summary>
        /// <param name="dVelocity"></param>
        /// <returns></returns>
        public abstract bool HLSetHomeVelocity(double dVelocity);

        /// <summary>
        /// 홈 1차 검출 속도를 읽어 온다. ( 검출 후 속도, 나머지 속도 는 초기 설정되어 있는 설정값 그대로 적용 )
        /// </summary>
        /// <param name="dVelocity"></param>
        /// <returns></returns>
        public abstract bool HLGetHomeVelocity(ref double dVelocity);

        /// <summary>
        /// 모터 소프트웨어 리미트 사용 유무,
        /// </summary>
        /// <param name="eUse"></param>
        /// <returns></returns>
        public abstract bool HLSetSoftwareLimit(EUse eUse);

        /// <summary>
        /// 빠른 조그 이동 추상 함수
        /// </summary>
        /// <param name="eDirection"></param>
        /// <returns></returns>
        public abstract bool HLJogFastMove(EMoveDirection eDirection);

        /// <summary>
        /// 느린 조그 이동 추상 함수
        /// </summary>
        /// <param name="eDirection"></param>
        /// <returns></returns>
        public abstract bool HLJogSlowMove(EMoveDirection eDirection);

        /// <summary>
        /// 연속 무브
        /// </summary>
        /// <param name="eDirection"></param>
        /// <param name="eModeType"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public abstract bool HLMoveContinue(EMoveDirection eDirection, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 단축 절대 위치 이동 추상 함수 ( 인덱스 포지션 )
        /// </summary>
        public abstract bool HLMoveAbsoluteIndex(int iPositionIndex, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 단축 절대 위치 이동 추상 함수 ( 포지션 값 )
        /// </summary>
        public abstract bool HLMoveAbsolutePosition(int index, double dPosition, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 단축 절대 위치 이동 추상 함수 ( 포지션 값 )
        /// </summary>
        public abstract bool HLMoveAbsolutePosition(double dPosition, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 단축 상대 위치 이동 추상 함수 ( 포지션 값 )
        /// </summary>
        /// <param name="dPosition"></param>
        /// <param name="eModeType"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public abstract bool HLMoveRelativePosition(double dPosition, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 단축 감속 정지 추상 함수
        /// </summary>
        /// <returns></returns>
        public abstract bool HLMoveStop();

        /// <summary>
        /// 단축 긴급 정지 추상 함수
        /// </summary>
        /// <returns></returns>
        public abstract bool HLMoveEStop();

        /// <summary>
        /// 다축 절대 위치 이동 추상 함수 ( 인덱스 포지션 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public abstract bool HLMoveMultiAbsoluteIndex(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter);

        /// <summary>
        /// 다축 절대 위치 이동 추상 함수 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public abstract bool HLMoveMultiAbsolutePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter);

        /// <summary>
        /// 다축 상대 위치 이동 추상 함수 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public abstract bool HLMoveMultiRelativePositioin(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter);

        /// <summary>
        /// 직선 보간 절대 위치 이동 추상 함수 ( 인덱스 포지션 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMulitMoveParameter"></param>
        /// <returns></returns>
        public abstract bool HLMoveLineAbsoluteIndex(EVelocityMode eModeType, CMultiMoveParameter[] objMulitMoveParameter);

        /// <summary>
        /// 직선 보간 절대 위치 이동 추상 함수 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMulitMoveParameter"></param>
        /// <returns></returns>
        public abstract bool HLMoveLineAbsolutePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMulitMoveParameter);

        /// <summary>
        /// 직선 보간 상대 위치 이동 추상 함수 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMulitMoveParameter"></param>
        /// <returns></returns>
        public abstract bool HLMoveLineRelativePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMulitMoveParameter);

        /// <summary>
        /// 다축 감속 정지 추상 함수
        /// </summary>
        /// <param name="objMotorAjin"></param>
        /// <returns></returns>
        public abstract bool HLMoveMultiStop(object[] objMotorAjin);

        /// <summary>
        /// 다축 긴급 정지 추상 함수
        /// </summary>
        /// <param name="objMotorAjin"></param>
        /// <returns></returns>
        public abstract bool HLMoveMultiEStop(object[] objMotorAjin);

        /// <summary>
        /// 위치 오버라이드 추상 함수
        /// </summary>
        /// <param name="dOverridePosition"></param>
        /// <returns></returns>
        public abstract bool HLOverridePosition(double dOverridePosition);

        /// <summary>
        /// 속도 오버라이드 추상 함수
        /// </summary>
        /// <param name="dMaxOverrideVelocity"></param>
        /// <returns></returns>
        public abstract bool HLOverrideMaxVelocity(double dMaxOverrideVelocity);

        /// <summary>
        /// 속도 오버라이드 추상 함수
        /// </summary>
        /// <param name="dOverrideVelocity"></param>
        /// <returns></returns>
        public abstract bool HLOverrideVelocity(double dOverrideVelocity);

        /// <summary>
        /// 단일 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동 추상 함수 ( 포지션 인덱스 값 절대 위치 )
        /// </summary>
        /// <param name="iPositionIndex"></param>
        /// <param name="eModeType"></param>
        /// <param name="dOverridePosition"></param>
        /// <param name="dOverrideVelocity"></param>
        /// <param name="dMaxOverrideVelocity"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public abstract bool HLMoveOverrideAtIndex(int iPositionIndex, EVelocityMode eModeType, double dOverridePosition, double dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 단일 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동 추상 함수 ( 포지션 값 절대 위치 )
        /// </summary>
        /// <param name="dPosition"></param>
        /// <param name="eModeType"></param>
        /// <param name="dOverridePosition"></param>
        /// <param name="dOverrideVelocity"></param>
        /// <param name="dMaxOverrideVelocity"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public abstract bool HLMoveOverrideAtPosition(double dPosition, EVelocityMode eModeType, double dOverridePosition, double dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 다중 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동 추상 함수 ( 포지션 인덱스 값 절대 위치 )
        /// </summary>
        /// <param name="iPositionIndex"></param>
        /// <param name="eModeType"></param>
        /// <param name="dOverridePosition"></param>
        /// <param name="dOverrideVelocity"></param>
        /// <param name="dMaxOverrideVelocity"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public abstract bool HLMoveMultiOverrideAtIndex(int iPositionIndex, EVelocityMode eModeType, double[] dOverridePosition, double[] dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 다중 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동 추상 함수 ( 포지션 값 절대 위치 )
        /// </summary>
        /// <param name="dPosition"></param>
        /// <param name="eModeType"></param>
        /// <param name="dOverridePosition"></param>
        /// <param name="dOverrideVelocity"></param>
        /// <param name="dMaxOverrideVelocity"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public abstract bool HLMoveMultiOverrideAtPosition(double dPosition, EVelocityMode eModeType, double[] dOverridePosition, double[] dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0);

        /// <summary>
        /// 알람 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public abstract CMotorError HLGetMotorError();

        /// <summary>
        /// 포지션 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public abstract CMotorPosition HLGetMotorPosition();

        /// <summary>
        /// 모터 운용 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public abstract CMotorOperationParameter HLGetMotorOperationParameter();

        /// <summary>
        /// 초기화 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public abstract CMotorInitializeParameter HLGetMotorInitializeParameter();

        /// <summary>
        /// 초기화 정보 객체를 설정한다.
        /// </summary>
        /// <param name="setParameter"></param>
        public abstract void HLSetMotorInitializeParameter(CMotorInitializeParameter setParameter);

        /// <summary>
        /// 모터 상태 정보 객체를 얻어 온다
        /// </summary>
        /// <returns></returns>
        public abstract CMotorStatus HLGetMotorStatus();

        /// <summary>
        /// 모터 이동 후 인포지션 확인.
        /// </summary>
        /// <param name="objMotor"></param>
        /// <param name="eCompareResult"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        public abstract bool HLWaitMotorStatus(object objMotor, EPositionCompare eCompareResult, int iTimeOut, int iSleepPeriod = 10);

        /// <summary>
        /// 모터 이동 후 현재 위치 타겟 위치 비교 ( 인덱스 포지션 값 )
        /// </summary>
        /// <param name="objMotor"></param>
        /// <param name="eCompareResult"></param>
        /// <param name="iPositionIndex"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        public abstract bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, int iPositionIndex, int iTimeOut, int iSleepPeriod = 10);

        /// <summary>
        /// 모터 이동 후 현재 위치 타겟 위치 비교 ( 포지션 값 ).
        /// </summary>
        /// <param name="objMotor"></param>
        /// <param name="eCompareResult"></param>
        /// <param name="dStartPosition"></param>
        /// <param name="dPosition"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        public abstract bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, double dStartPosition, double dPosition, int iTimeOut, int iSleepPeriod = 10);

        /// <summary>
        /// 모터 이동 후 현재 위치 타겟 위치 비교 ( 포지션 값 ).
        /// </summary>
        /// <param name="objMotor"></param>
        /// <param name="eCompareResult"></param>
        /// <param name="dPosition"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        public abstract bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, double dPosition, int iTimeOut, int iSleepPeriod = 10);

        /// <summary>
        /// 지정 축의 트리거 출력 시간, 트리거 레벨, 트리거 출력 방법, 트리거 출력시 인터럽트 설정여부
        /// </summary>
        /// <param name="dTriggerTimeWidth"></param>
        /// <returns></returns>
        public abstract bool HLSetTriggerTime(double dTriggerTimeWidth);

        public abstract bool HLSetTriggerTime(int iAxis, double dTriggerTimeWidth);

        /// <summary>
        /// 설정된 트리거 블록을 초기화함
        /// </summary>
        /// <param name="iAxis">축 번호</param>
        /// <returns>성공 여부</returns>
        public abstract bool HLSetTriggerBlockReset();

        /// <summary>
        /// 지정한 시작 위치부터 종료 위치까지 일정 구간 마다 트리거를 출력한다.
        /// </summary>
        /// <param name="dStartPosition"></param>
        /// <param name="dEndPosition"></param>
        /// <param name="dPeriodPosition"></param>
        /// <returns></returns>
        public abstract bool HLSetTriggerBlock(double dStartPosition, double dEndPosition, double dPeriodPosition);

        public abstract bool HLSetTriggerBlock(int iAxis, double dStartPosition, double dEndPosition, double dPeriodPosition);

        /// <summary>
        /// 해당 축에 서보 드라이버에 경고 코드를 요청한다.
        /// </summary>
        /// <returns>True=요청 성공, False=읽기 진행중</returns>
        public abstract bool HLSetStatusRequestReadA5nWarningData();

        /// <summary>
        /// 해당 축에서 경고 코드 요청 결과를 읽어온다.
        /// 이 함수는 HLSetStatusRequestReadA5nWarningData() 함수 호출 후에 호출해야한다.
        /// </summary>
        /// <param name="warningCode">경고 코드</param>
        /// <returns>True=성공, False=읽기 진행중</returns>
        public abstract bool HLGetStatusA5nWarningCode(ref uint warningCode);

        /// <summary>
        /// 모터 운용 파라미터 로딩 가상 함수
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public virtual bool HLLoadMotorOperationParameter(string strFilePath, string strModelName)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLLoadMotorOperationParameter";

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 운용 파라미터 저장 가상 함수
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <param name="objMotorOperationParameter"></param>
        /// <returns></returns>
        public virtual bool HLSaveMotorOperationParameter(string strFilePath, string strModelName, CMotorOperationParameter objMotorOperationParameter)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLSaveMotorOperationParameter";

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 포지션 파라미터 로딩 가상 함수
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public virtual bool HLLoadMotorPositionParameter(string strFilePath, string strModelName)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLLoadMotorPositionParameter";

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 포지션 파라미터 저장 가상 함수
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <param name="objMotorPosition"></param>
        /// <returns></returns>
        public virtual bool HLSaveMotorPositionParameter(string strFilePath, string strModelName, CMotorPosition objMotorPosition)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLSaveMotorPositionParameter";

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

    }
}

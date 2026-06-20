using HLDevice.Abstract;

namespace HLDevice
{
    public class CDeviceMotor
    {
        public int OverrideSpeedRate
        {
            get
            {
                return m_objMotor.OverrideSpeedRate;
            }
            set
            {
                m_objMotor.OverrideSpeedRate = value;
            }
        }
        /// <summary>
        /// 모터 상속 객채
        /// </summary>
        private readonly CDeviceMotorAbstract m_objMotor;

        private CDeviceMotorAbstract.CMotorInitializeParameter m_objInitializeParameter;

        private readonly CDeviceMotorAbstract.CMotorPosition m_objMotorPosition = new CDeviceMotorAbstract.CMotorPosition();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objMotor"></param>
        /// <returns></returns>
        public CDeviceMotor(CDeviceMotorAbstract objMotor)
        {
            m_objMotor = objMotor;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDeviceMotorAbstract.CMotorInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;
            do
            {
                m_objInitializeParameter = objInitializeParameter;
                // 모터 객체 초기화
                if (false == m_objMotor.HLInitialize(objInitializeParameter))
                    break;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objMotor.HLDeInitialize();
        }

        /// <summary>
        /// 버전 정보
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objMotor.HLGetVersion();
        }

        /// <summary>
        /// 모터 알람 리셋
        /// </summary>
        /// <returns></returns>
        public bool HLAlarmReset()
        {
            return m_objMotor.HLAlarmReset();
        }

        /// <summary>
        /// 모터 디바이스 종료
        /// </summary>
        /// <returns></returns>
        public bool HLClose()
        {
            return m_objMotor.HLClose();
        }

        /// <summary>
        /// 모터 포지션 비교 ( 포지션 값 )
        /// </summary>
        /// <param name="dPosition"></param>
        /// <returns></returns>
        public CDeviceMotorAbstract.EPositionCompare HLGetCompareTargetPosition(double dPosition)
        {
            return m_objMotor.HLGetCompareTargetPosition(dPosition);
        }

        /// <summary>
        /// 모터 포지션 비교 ( 포지션 값 )
        /// </summary>
        /// <param name="dStartPosition"></param>
        /// <param name="dPosition"></param>
        /// <returns></returns>
        public CDeviceMotorAbstract.EPositionCompare HLGetCompareTargetPosition(double dStartPosition, double dPosition)
        {
            return m_objMotor.HLGetCompareTargetPosition(dStartPosition, dPosition);
        }

        /// <summary>
        /// 모터 포지션 비교 ( 인덱스 포지션 값 )
        /// </summary>
        /// <param name="iPositionIndex"></param>
        /// <returns></returns>
        public CDeviceMotorAbstract.EPositionCompare HLGetCompareTargetPosition(int iPositionIndex)
        {
            return m_objMotor.HLGetCompareTargetPosition(iPositionIndex);
        }

        /// <summary>
        /// 홈 동작
        /// </summary>
        /// <returns></returns>
        public bool HLSetHomeProcess()
        {
            return m_objMotor.HLSetHomeProcess();
        }

        /// <summary>
        /// 홈 동작 진행 률
        /// </summary>
        /// <param name="iHomeMainStepNo"></param>
        /// <param name="iHomeStepNo"></param>
        /// <returns></returns>
        public bool HLGetHomeProcessRate(ref int iHomeMainStepNo, ref int iHomeStepNo)
        {
            return m_objMotor.HLGetHomeProcessRate(ref iHomeMainStepNo, ref iHomeStepNo);
        }

        /// <summary>
        /// 서보 ON/OFF 추상 함수
        /// </summary>
        /// <param name="eUse"></param>
        /// <returns></returns>
        public bool HLServoOn(CDeviceMotorAbstract.EUse eUse)
        {
            return m_objMotor.HLServoOn(eUse);
        }

        /// <summary>
        /// 서보 ON/OFF 대기 함수
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool SetServoAndWaitChanged(bool setValue, int timeout)
        {
            return m_objMotor.SetServoAndWaitChanged(setValue, timeout);
        }

        /// <summary>
        /// 모터 소프트웨어 리미트 사용 유무,
        /// </summary>
        /// <param name="eUse"></param>
        /// <returns></returns>
        public bool HLSetSoftwareLimit(CDeviceMotorAbstract.EUse eUse)
        {
            return m_objMotor.HLSetSoftwareLimit(eUse);
        }

        /// <summary>
        /// 포지션 ZeroSet
        /// </summary>
        /// <returns></returns>
        public bool HLSetPositionZeroSet()
        {
            return m_objMotor.HLSetPositionZeroSet();
        }

        /// <summary>
        /// Actual 포지션 {dPosition}Set
        /// </summary>
        /// <returns></returns>
        public bool HLSetActualPosition(double dPosition)
        {
            return m_objMotor.HLSetActualPosition(dPosition);
        }

        /// <summary>
        /// 모터 unit/pulse Write
        /// </summary>
        /// <param name="dUnit">모터가 1회전 할 때 변화량</param>
        /// <param name="iPulse">모터가 1회전 하기위한 펄스 수</param>
        /// <returns>함수 실행 결과</returns>
        /// <![CDATA[
        /// 예1> 리드가 20mm인 볼스크류에 모터 분해능이 10000 일 때 (유닛을 mm로)
        ///     dUnit = 20d; // 모터 1바퀴 회전하면 20mm 만큼 이동함
        ///     iPulse = 10000;
        /// 예2> 감속비가 10:1인 감속기에 모터 분해능이 3600 일 때 (유닛을 deg로)
        ///     dUnit = 36; // 모터가 1바퀴 회전하면 360deg * 1/10 만큼 이동함
        ///     iPulse = 3600;
        /// ]]>
        public bool HLSetUnitPerPulse(double dUnit, int iPulse)
        {
            return m_objMotor.HLSetUnitPerPulse(dUnit, iPulse);
        }

        /// <summary>
        /// 모터 unit/pulse Read
        /// </summary>
        /// <param name="dUnit"></param>
        /// <param name="iPulse"></param>
        /// <returns></returns>
        public bool HLGetUnitperPulse(ref double dUnit, ref int iPulse)
        {
            return m_objMotor.HLGetUnitperPulse(ref dUnit, ref iPulse);
        }

        /// <summary>
        /// 홈 1차 검출 속도를 설정 한다. ( 검출 후 속도, 나머지 속도 는 초기 설정되어 있는 설정값 그대로 적용 )
        /// </summary>
        /// <param name="dVelocity"></param>
        /// <returns></returns>
        public bool HLSetHomeVelocity(double dVelocity)
        {
            return m_objMotor.HLSetHomeVelocity(dVelocity);
        }

        /// <summary>
        /// 홈 1차 검출 속도를 읽어 온다. ( 검출 후 속도, 나머지 속도 는 초기 설정되어 있는 설정값 그대로 적용 )
        /// </summary>
        /// <param name="dVelocity"></param>
        /// <returns></returns>
        public bool HLGetHomeVelocity(ref double dVelocity)
        {
            return m_objMotor.HLGetHomeVelocity(ref dVelocity);
        }

        /// <summary>
        /// 빠른 조그 이동
        /// </summary>
        /// <param name="eDirection"></param>
        /// <returns></returns>
        public bool HLJogFastMove(CDeviceMotorAbstract.EMoveDirection eDirection)
        {
            return m_objMotor.HLJogFastMove(eDirection);
        }

        /// <summary>
        /// 느린 조그 이동
        /// </summary>
        /// <param name="eDirection"></param>
        /// <returns></returns>
        public bool HLJogSlowMove(CDeviceMotorAbstract.EMoveDirection eDirection)
        {
            return m_objMotor.HLJogSlowMove(eDirection);
        }

        /// <summary>
        /// 연속 무브
        /// </summary>
        /// <param name="eDirection"></param>
        /// <param name="eModeType"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public bool HLMoveContinue(CDeviceMotorAbstract.EMoveDirection eDirection, CDeviceMotorAbstract.EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveContinue(eDirection, eModeType, dVelocity, dAcceleration, dDeceleration);
        }

        /// <summary>
        /// 단축 절대 위치 이동 ( 인덱스 포지션 )
        /// </summary>
        /// <param name="iPositionIndex"></param>
        /// <param name="eModeType"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public bool HLMoveAbsoluteIndex(int iPositionIndex, CDeviceMotorAbstract.EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveAbsoluteIndex(iPositionIndex, eModeType, dVelocity, dAcceleration, dDeceleration);
        }

        /// <summary>
        /// 단축 절대 위치 이동 ( 포지션 값 )
        /// </summary>
        public bool HLMoveAbsolutePosition(int index, double dPosition, CDeviceMotorAbstract.EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveAbsolutePosition(index, dPosition, eModeType, dVelocity, dAcceleration, dDeceleration);
        }
        public bool HLMoveAbsolutePosition(double dPosition, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveAbsolutePosition(dPosition, dVelocity, dAcceleration, dDeceleration);
        }

        /// <summary>
        /// 단축 상대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="dPosition"></param>
        /// <param name="eModeType"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public bool HLMoveRelativePosition(double dPosition, CDeviceMotorAbstract.EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveRelativePosition(dPosition, eModeType, dVelocity, dAcceleration, dDeceleration);
        }

        /// <summary>
        /// 단축 감속 정지
        /// </summary>
        /// <returns></returns>
        public bool HLMoveStop()
        {
            return m_objMotor.HLMoveStop();
        }

        /// <summary>
        /// 단축 긴급 정지
        /// </summary>
        /// <returns></returns>
        public bool HLMoveEStop()
        {
            return m_objMotor.HLMoveEStop();
        }

        /// <summary>
        /// 다축 절대 위치 이동 ( 인덱스 포지션 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public bool HLMoveMultiAbsoluteIndex(CDeviceMotorAbstract.EVelocityMode eModeType, CDeviceMotorAbstract.CMultiMoveParameter[] objMultiMoveParameter)
        {
            return m_objMotor.HLMoveMultiAbsoluteIndex(eModeType, objMultiMoveParameter);
        }

        /// <summary>
        /// 다축 절대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public bool HLMoveMultiAbsolutePosition(CDeviceMotorAbstract.EVelocityMode eModeType, CDeviceMotorAbstract.CMultiMoveParameter[] objMultiMoveParameter)
        {
            return m_objMotor.HLMoveMultiAbsolutePosition(eModeType, objMultiMoveParameter);
        }

        /// <summary>
        /// 다축 상대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public bool HLMoveMultiRelativePositioin(CDeviceMotorAbstract.EVelocityMode eModeType, CDeviceMotorAbstract.CMultiMoveParameter[] objMultiMoveParameter)
        {
            return m_objMotor.HLMoveMultiRelativePositioin(eModeType, objMultiMoveParameter);
        }

        /// <summary>
        /// 직선 보간 절대 위치 이동 ( 인덱스 포지션 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public bool HLMoveLineAbsoluteIndex(CDeviceMotorAbstract.EVelocityMode eModeType, CDeviceMotorAbstract.CMultiMoveParameter[] objMultiMoveParameter)
        {
            return m_objMotor.HLMoveLineAbsoluteIndex(eModeType, objMultiMoveParameter);
        }

        /// <summary>
        /// 직선 보간 절대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public bool HLMoveLineAbsolutePosition(CDeviceMotorAbstract.EVelocityMode eModeType, CDeviceMotorAbstract.CMultiMoveParameter[] objMultiMoveParameter)
        {
            return m_objMotor.HLMoveLineAbsolutePosition(eModeType, objMultiMoveParameter);
        }

        /// <summary>
        /// 직선 보간 상대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public bool HLMoveLineRelativePosition(CDeviceMotorAbstract.EVelocityMode eModeType, CDeviceMotorAbstract.CMultiMoveParameter[] objMultiMoveParameter)
        {
            return m_objMotor.HLMoveLineRelativePosition(eModeType, objMultiMoveParameter);
        }

        /// <summary>
        /// 다축 감속 정지
        /// </summary>
        /// <param name="objMotorAjin"></param>
        /// <returns></returns>
        public bool HLMoveMultiStop(object[] objMotorAjin)
        {
            return m_objMotor.HLMoveMultiStop(objMotorAjin);
        }

        /// <summary>
        /// 다축 긴급 정지
        /// </summary>
        /// <param name="objMotorAjin"></param>
        /// <returns></returns>
        public bool HLMoveMultiEStop(object[] objMotorAjin)
        {
            return m_objMotor.HLMoveMultiEStop(objMotorAjin);
        }

        /// <summary>
        /// 위치 오버라이드
        /// </summary>
        /// <param name="dOverridePosition"></param>
        /// <returns></returns>
        public bool HLOverridePosition(double dOverridePosition)
        {
            return m_objMotor.HLOverridePosition(dOverridePosition);
        }

        /// <summary>
        /// 속도 오버라이드
        /// </summary>
        /// <param name="dMaxOverrideVelocity"></param>
        /// <returns></returns>
        public bool HLOverrideMaxVelocity(double dMaxOverrideVelocity)
        {
            return m_objMotor.HLOverrideMaxVelocity(dMaxOverrideVelocity);
        }

        /// <summary>
        /// 속도 오버라이드
        /// </summary>
        /// <param name="dOverrideVelocity"></param>
        /// <returns></returns>
        public bool HLOverrideVelocity(double dOverrideVelocity)
        {
            return m_objMotor.HLOverrideVelocity(dOverrideVelocity);
        }

        /// <summary>
        /// 단일 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동 ( 포지션 인덱스 값 절대 위치 )
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
        public bool HLMoveOverrideAtIndex(int iPositionIndex, CDeviceMotorAbstract.EVelocityMode eModeType, double dOverridePosition, double dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveOverrideAtIndex(iPositionIndex, eModeType, dOverridePosition, dOverrideVelocity, dMaxOverrideVelocity, dVelocity, dAcceleration, dDeceleration);
        }

        /// <summary>
        /// 단일 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동 ( 포지션 값 절대 위치 )
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
        public bool HLMoveOverrideAtPosition(double dPosition, CDeviceMotorAbstract.EVelocityMode eModeType, double dOverridePosition, double dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveOverrideAtPosition(dPosition, eModeType, dOverridePosition, dOverrideVelocity, dMaxOverrideVelocity, dVelocity, dAcceleration, dDeceleration);
        }

        /// <summary>
        /// 다중 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동 ( 포지션 인덱스 값 절대 위치 )
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
        public bool HLMoveMultiOverrideAtIndex(int iPositionIndex, CDeviceMotorAbstract.EVelocityMode eModeType, double[] dOverridePosition, double[] dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveMultiOverrideAtIndex(iPositionIndex, eModeType, dOverridePosition, dOverrideVelocity, dMaxOverrideVelocity, dVelocity, dAcceleration, dDeceleration);
        }

        /// <summary>
        /// 다중 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동 ( 포지션 값 절대 위치 )
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
        public bool HLMoveMultiOverrideAtPosition(double dPosition, CDeviceMotorAbstract.EVelocityMode eModeType, double[] dOverridePosition, double[] dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            return m_objMotor.HLMoveMultiOverrideAtPosition(dPosition, eModeType, dOverridePosition, dOverrideVelocity, dMaxOverrideVelocity, dVelocity, dAcceleration, dDeceleration);
        }

        /// <summary>
        /// 지정 축의 트리거 출력 시간, 트리거 레벨, 트리거 출력 방법, 트리거 출력시 인터럽트 설정여부
        /// </summary>
        /// <param name="dTriggerTimeWidth">us 단위. ( 1000 입력시 1ms )</param>
        /// <returns></returns>
        public bool HLSetTriggerTime(double dTriggerTimeWidth)
        {
            return m_objMotor.HLSetTriggerTime(dTriggerTimeWidth);
        }

        public bool HLSetTriggerTime(int iAxis, double dTriggerTimeWidth)
        {
            return m_objMotor.HLSetTriggerTime(iAxis, dTriggerTimeWidth);
        }

        /// <summary>
        /// 지정한 시작 위치부터 종료 위치까지 일정 구간 마다 트리거를 출력한다.
        /// </summary>
        /// <param name="dStartPosition"></param>
        /// <param name="dEndPosition"></param>
        /// <param name="dPeriodPosition"></param>
        /// <returns></returns>
        public bool HLSetTriggerBlock(double dStartPosition, double dEndPosition, double dPeriodPosition)
        {
            return m_objMotor.HLSetTriggerBlock(dStartPosition, dEndPosition, dPeriodPosition);
        }

        public bool HLSetTriggerBlock(int iAxis, double dStartPosition, double dEndPosition, double dPeriodPosition)
        {
            return m_objMotor.HLSetTriggerBlock(iAxis, dStartPosition, dEndPosition, dPeriodPosition);
        }

        /// <summary>
        /// 트리거 블록 설정을 초기화함
        /// </summary>
        /// <param name="iAxis">축 번호</param>
        /// <returns>성공 여부</returns>
        public bool HLSetTriggerBlockReset()
        {
            return m_objMotor.HLSetTriggerBlockReset();
        }

        /// <summary>
        /// 해당 축에 서보 드라이버에 경고 코드를 요청한다.
        /// </summary>
        /// <returns>True=요청 성공, False=읽기 진행중</returns>
        public bool HLSetStatusRequestReadA5nWarningData()
        {
            return m_objMotor.HLSetStatusRequestReadA5nWarningData();
        }

        /// <summary>
        /// 해당 축에서 경고 코드 요청 결과를 읽어온다.
        /// 이 함수는 HLSetStatusRequestReadA5nWarningData() 함수 호출 후에 호출해야한다.
        /// </summary>
        /// <param name="warningCode">경고 코드</param>
        /// <returns>True=성공, False=읽기 진행중</returns>
        public bool HLGetStatusA5nWarningCode(ref uint warningCode)
        {
            return m_objMotor.HLGetStatusA5nWarningCode(ref warningCode);
        }

        /// <summary>
        /// 알람 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public CDeviceMotorAbstract.CMotorError HLGetMotorError()
        {
            return m_objMotor.HLGetMotorError();
        }

        /// <summary>
        /// 포지션 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public CDeviceMotorAbstract.CMotorPosition HLGetMotorPosition()
        {
            return m_objMotor.HLGetMotorPosition();
        }

        /// <summary>
        /// 모터 운용 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public CDeviceMotorAbstract.CMotorOperationParameter HLGetMotorOperationParameter()
        {
            return m_objMotor.HLGetMotorOperationParameter();
        }

        /// <summary>
        /// 초기화 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public CDeviceMotorAbstract.CMotorInitializeParameter HLGetMotorInitializeParameter()
        {
            return m_objMotor.HLGetMotorInitializeParameter();
        }

        /// <summary>
        /// 초기화 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public void HLSetMotorInitializeParameter(CDeviceMotorAbstract.CMotorInitializeParameter setParameter)
        {
            m_objMotor.HLSetMotorInitializeParameter(setParameter);
        }

        /// <summary>
        /// 모터 상태 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public CDeviceMotorAbstract.CMotorStatus HLGetMotorStatus()
        {
            return m_objMotor.HLGetMotorStatus();
        }

        /// <summary>
        /// 모터 이동 후 인포지션 확인.
        /// </summary>
        /// <param name="objMotor"></param>
        /// <param name="eCompareResult"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        public bool HLWaitMotorStatus(object objMotor, CDeviceMotorAbstract.EPositionCompare eCompareResult, int iTimeOut, int iSleepPeriod = 10)
        {
            return m_objMotor.HLWaitMotorStatus(objMotor, eCompareResult, iTimeOut, iSleepPeriod);
        }

        /// <summary>
        /// 모터 이동 후 현재 위치 타겟 위치 비교 ( 인덱스 포지션 값 )
        /// </summary>
        /// <param name="objMotor"></param>
        /// <param name="eCompareResult"></param>
        /// <param name="iPositionIndex"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        public bool HLWaitMotorPositionStatus(object objMotor, CDeviceMotorAbstract.EPositionCompare eCompareResult, int iPositionIndex, int iTimeOut, int iSleepPeriod = 10)
        {
            return m_objMotor.HLWaitMotorPositionStatus(objMotor, eCompareResult, iPositionIndex, iTimeOut, iSleepPeriod);
        }

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
        public bool HLWaitMotorPositionStatus(object objMotor, CDeviceMotorAbstract.EPositionCompare eCompareResult, double dStartPosition, double dPosition, int iTimeOut, int iSleepPeriod = 10)
        {
            return m_objMotor.HLWaitMotorPositionStatus(objMotor, eCompareResult, dStartPosition, dPosition, iTimeOut, iSleepPeriod);
        }

        /// <summary>
        /// 모터 이동 후 현재 위치 타겟 위치 비교 ( 포지션 값 ).
        /// </summary>
        /// <param name="objMotor"></param>
        /// <param name="eCompareResult"></param>
        /// <param name="dPosition"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        public bool HLWaitMotorPositionStatus(object objMotor, CDeviceMotorAbstract.EPositionCompare eCompareResult, double dPosition, int iTimeOut, int iSleepPeriod = 10)
        {
            return m_objMotor.HLWaitMotorPositionStatus(objMotor, eCompareResult, dPosition, iTimeOut, iSleepPeriod);
        }

        /// <summary>
        /// 모터 운용 파라미터 로딩
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public bool HLLoadMotorOperationParameter(string strFilePath, string strModelName)
        {
            return m_objMotor.HLLoadMotorOperationParameter(strFilePath, strModelName);
        }

        /// <summary>
        /// 모터 운용 파라미터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <param name="objMotorOperationParameter"></param>
        /// <returns></returns>
        public bool HLSaveMotorOperationParameter(string strFilePath, string strModelName, CDeviceMotorAbstract.CMotorOperationParameter objMotorOperationParameter)
        {
            return m_objMotor.HLSaveMotorOperationParameter(strFilePath, strModelName, objMotorOperationParameter);
        }

        /// <summary>
        /// 모터 포지션 파라미터 로딩
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public bool HLLoadMotorPositionParameter(string strFilePath, string strModelName)
        {
            return m_objMotor.HLLoadMotorPositionParameter(strFilePath, strModelName);
        }

        /// <summary>
        /// 모터 포지션 파라미터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <param name="objMotorPosition"></param>
        /// <returns></returns>
        public bool HLSaveMotorPositionParameter(string strFilePath, string strModelName, CDeviceMotorAbstract.CMotorPosition objMotorPosition)
        {
            return m_objMotor.HLSaveMotorPositionParameter(strFilePath, strModelName, objMotorPosition);
        }

        /// <summary>
        /// 추상화 객체
        /// </summary>
        /// <returns></returns>
        public CDeviceMotorAbstract HLGetAbstractReference()
        {
            return m_objMotor;
        }
    }
}

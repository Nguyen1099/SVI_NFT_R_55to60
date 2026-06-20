using HLDevice.Abstract;
using System.Threading;

namespace HLDevice.Motor
{
    public class CDeviceMotorVirtual : CDeviceMotorAbstract
    {
        /// <summary>
        /// 모터 포지션 객체 생성
        /// </summary>
        private readonly CMotorPosition m_objMotorPosition = new CMotorPosition();
        /// <summary>
        /// 모터 초기화 객체 생성
        /// </summary>
        private readonly CMotorInitializeParameter m_objInitializeParameter = new CMotorInitializeParameter();
        /// <summary>
        /// 모터 운용 파라미터 객체 생성
        /// </summary>
        private readonly CMotorOperationParameter m_objMotorOperationParameter = new CMotorOperationParameter();
        /// <summary>
        /// 모터 상태 정보 생성
        /// </summary>
        private CMotorStatus m_objMotorStatus = new CMotorStatus();
        /// <summary>
        /// 알람 객체 생성
        /// </summary>
        private readonly CMotorError m_objMotorError = new CMotorError();

        /// <summary>
        /// 모터 객체 선언
        /// </summary>
        public HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtual m_objMotorVirtual;

        public override int OverrideSpeedRate { get; set; } = 100;

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(CDeviceMotorAbstract.CMotorInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;
            do
            {
                // 모터 객체 초기화
                m_objMotorVirtual = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtual();
                // 모터 초기화 객체 생성
                HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorInitializeParameter objInitializeParameterDLL = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorInitializeParameter();
                objInitializeParameterDLL.iBoardNo = objInitializeParameter.iBoardNo;
                objInitializeParameterDLL.iAxisNo = objInitializeParameter.iAxisNo;
                objInitializeParameterDLL.strFilePath = objInitializeParameter.strFilePath;
                objInitializeParameterDLL.strModelName = objInitializeParameter.strModelName;
                objInitializeParameterDLL.strMotorName = objInitializeParameter.strMotorName;
                if (false == m_objMotorVirtual.HLInitialize(objInitializeParameterDLL))
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제 함수
        /// </summary>
        public override void HLDeInitialize()
        {
            m_objMotorVirtual.HLDeInitialize();
        }

        /// <summary>
        /// 버전 정보
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objMotorVirtual.GetVersion();
        }

        /// <summary>
        /// 모터 알람 리셋
        /// </summary>
        /// <returns></returns>
        public override bool HLAlarmReset()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLAlarmReset())
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 알람 리셋
        /// </summary>
        /// <returns></returns>
        public override bool HLClose()
        {
            bool bReturn = false;
            do
            {
                m_objMotorVirtual.HLClose();
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 포지션 비교
        /// </summary>
        /// <param name="dPosition"></param>
        /// <returns></returns>
        public override CDeviceMotorAbstract.EPositionCompare HLGetCompareTargetPosition(double dPosition)
        {
            bool bReturn = false;
            CDeviceMotorAbstract.EPositionCompare ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_UNKNOWN;
            do
            {
                bReturn = m_objMotorVirtual.HLGetCompareTargetPosition(dPosition);
                if (false == bReturn)
                    ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_UNEQUAL;
                else if (true == bReturn)
                    ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL;
                bReturn = true;
            } while (false);

            return ePositionCompare;
        }

        /// <summary>
        /// 모터 포지션 비교
        /// </summary>
        /// <param name="dStartPosition"></param>
        /// <param name="dPosition"></param>
        /// <returns></returns>
        public override CDeviceMotorAbstract.EPositionCompare HLGetCompareTargetPosition(double dStartPosition, double dPosition)
        {
            bool bReturn = false;
            CDeviceMotorAbstract.EPositionCompare ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_UNKNOWN;
            do
            {
                bReturn = m_objMotorVirtual.HLGetCompareTargetPosition(dStartPosition, dPosition);
                if (false == bReturn)
                    ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_UNEQUAL;
                else if (true == bReturn)
                    ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL;
                bReturn = true;
            } while (false);

            return ePositionCompare;
        }

        /// <summary>
        /// 모터 포지션 비교
        /// </summary>
        /// <param name="iPositionIndex"></param>
        /// <returns></returns>
        public override CDeviceMotorAbstract.EPositionCompare HLGetCompareTargetPosition(int iPositionIndex)
        {
            bool bReturn = false;
            CDeviceMotorAbstract.EPositionCompare ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_UNKNOWN;
            do
            {
                bReturn = m_objMotorVirtual.HLGetCompareTargetPosition(iPositionIndex);
                if (false == bReturn)
                    ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_UNEQUAL;
                else if (true == bReturn)
                    ePositionCompare = CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL;
                bReturn = true;
            } while (false);

            return ePositionCompare;
        }

        /// <summary>
        /// 홈 동작 진행
        /// </summary>
        /// <returns></returns>
        public override bool HLSetHomeProcess()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLSetHomeProcess())
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 홈 동작 진행률 확인
        /// </summary>
        /// <param name="iHomeMainStepNo"></param>
        /// <param name="iHomeStepNo"></param>
        /// <returns></returns>
        public override bool HLGetHomeProcessRate(ref int iHomeMainStepNo, ref int iHomeStepNo)
        {
            bool bReturn = false;
            int iHomeMainStepNumber = 0;
            int iHomeStepNumber = 0;
            do
            {
                if (false == m_objMotorVirtual.HLGetHomeProcessRate(ref iHomeMainStepNumber, ref iHomeStepNumber))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }

                // 겐트리일 경우 마스터, 슬레이브가 동작하는지 확인 할때 사용한다. 마스터 축 동작일때는 0, 슬레이브 동작 일 때는 10을 리턴한다.
                iHomeMainStepNo = iHomeMainStepNumber;
                // 현재 진행 률을 리턴한다 0  ~100 100이 되면 원점 검색이 종료 되었음을 의미.
                iHomeStepNo = iHomeStepNumber;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 서보 온
        /// </summary>
        /// <param name="eUse"></param>
        /// <returns></returns>
        public override bool HLServoOn(CDeviceMotorAbstract.EUse eUse)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLServoOn((HLDeviceDLL.Motor.Virtual.AXT_USE)eUse))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 서보 On/Off 상태 대기
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override bool SetServoAndWaitChanged(bool setValue, int timeout)
        {
            if (HLServoOn(setValue ? CDeviceMotorAbstract.EUse.ENABLE : CDeviceMotorAbstract.EUse.DISABLE) == false)
            {
                return false;
            }
            if (SpinWait.SpinUntil(() => HLGetMotorStatus().bServo == setValue, timeout) == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 소프트웨어 리미트 설정
        /// </summary>
        /// <param name="eUse"></param>
        /// <returns></returns>
        public override bool HLSetSoftwareLimit(CDeviceMotorAbstract.EUse eUse)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLSetSoftwareLimit((HLDeviceDLL.Motor.Virtual.AXT_USE)eUse))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 포지션 ZeroSet
        /// </summary>
        /// <returns></returns>
        public override bool HLSetPositionZeroSet()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLSetPositionZeroSet())
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// Actual 포지션 {dPosition}Set
        /// </summary>
        /// <returns></returns>
        public override bool HLSetActualPosition(double dPosition)
        {
            bool bReturn = false;
            do
            {
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 unit/pulse Write
        /// </summary>
        /// <param name="dUnit"></param>
        /// <param name="iPulse"></param>
        /// <returns></returns>
        public override bool HLSetUnitPerPulse(double dUnit, int iPulse)
        {
            bool bReturn = false;
            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 unit/pulse Read
        /// </summary>
        /// <param name="dUnit"></param>
        /// <param name="iPulse"></param>
        /// <returns></returns>
        public override bool HLGetUnitperPulse(ref double dUnit, ref int iPulse)
        {
            bool bReturn = false;
            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 홈 1차 검출 속도를 설정 한다. ( 검출 후 속도, 나머지 속도 는 초기 설정되어 있는 설정값 그대로 적용 )
        /// </summary>
        /// <param name="dVelocity"></param>
        /// <returns></returns>
        public override bool HLSetHomeVelocity(double dVelocity)
        {
            bool bReturn = false;
            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 홈 1차 검출 속도를 읽어 온다. ( 검출 후 속도, 나머지 속도 는 초기 설정되어 있는 설정값 그대로 적용 )
        /// </summary>
        /// <param name="dVelocity"></param>
        /// <returns></returns>
        public override bool HLGetHomeVelocity(ref double dVelocity)
        {
            bool bReturn = false;
            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 빠른 조그 동작
        /// </summary>
        /// <param name="eDirection"></param>
        /// <returns></returns>
        public override bool HLJogFastMove(CDeviceMotorAbstract.EMoveDirection eDirection)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLJogFastMove((HLDeviceDLL.Motor.Virtual.AXT_MOTION_MOVE_DIR)eDirection))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 느린 조그 동작
        /// </summary>
        /// <param name="eDirection"></param>
        /// <returns></returns>
        public override bool HLJogSlowMove(CDeviceMotorAbstract.EMoveDirection eDirection)
        {
            bool bReturn = false;
            do
            {

                if (false == m_objMotorVirtual.HLJogSlowMove((HLDeviceDLL.Motor.Virtual.AXT_MOTION_MOVE_DIR)eDirection))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
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
        public override bool HLMoveContinue(EMoveDirection eDirection, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {

                if (false == m_objMotorVirtual.HLMoveContinue((HLDeviceDLL.Motor.Virtual.AXT_MOTION_MOVE_DIR)eDirection, (HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 단축 절대 위치 이동 함수 ( 인덱스 포지션 )
        /// </summary>
        /// <param name="iPositionIndex"></param>
        /// <param name="eModeType"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public override bool HLMoveAbsoluteIndex(int iPositionIndex, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLMoveAbsoluteIndex(iPositionIndex, (HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 단축 절대 위치 이동 함수 ( 포지션 값 )
        /// </summary>
        public override bool HLMoveAbsolutePosition(int index, double dPosition, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            if (false == m_objMotorVirtual.HLMoveAbsolutePosition(index, dPosition, (HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, dVelocity, dAcceleration, dDeceleration))
            {
                m_objMotorVirtual.HLGetMotorError();
                return false;
            }
            return true;
        }
        public override bool HLMoveAbsolutePosition(double dPosition, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            if (false == m_objMotorVirtual.HLMoveAbsolutePosition(dPosition, dVelocity, dAcceleration, dDeceleration))
            {
                m_objMotorVirtual.HLGetMotorError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 단축 상대 위치 이동 함수 ( 포지션 값 )
        /// </summary>
        /// <param name="dPosition"></param>
        /// <param name="eModeType"></param>
        /// <param name="dVelocity"></param>
        /// <param name="dAcceleration"></param>
        /// <param name="dDeceleration"></param>
        /// <returns></returns>
        public override bool HLMoveRelativePosition(double dPosition, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLMoveRelativePosition(dPosition, (HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 단축 감속 정지 함수
        /// </summary>
        /// <returns></returns>
        public override bool HLMoveStop()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLMoveStop())
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 단축 긴급 정지 함수
        /// </summary>
        /// <returns></returns>
        public override bool HLMoveEStop()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLMoveEStop())
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 다축 절대 위치 이동 ( 인덱스 포지션 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public override bool HLMoveMultiAbsoluteIndex(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter)
        {
            bool bReturn = false;
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[] objMotorMoveParameter = null;
            do
            {
                if (null != objMultiMoveParameter)
                {
                    objMotorMoveParameter = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[objMultiMoveParameter.Length];
                    for (int iLoopCount = 0; iLoopCount < objMultiMoveParameter.Length; iLoopCount++)
                    {
                        objMotorMoveParameter[iLoopCount] = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter();
                        objMotorMoveParameter[iLoopCount].iPositionIndex = objMultiMoveParameter[iLoopCount].iPositionIndex;
                        objMotorMoveParameter[iLoopCount].dPosition = objMultiMoveParameter[iLoopCount].dPosition;
                        objMotorMoveParameter[iLoopCount].dVelocity = objMultiMoveParameter[iLoopCount].dVelocity;
                        objMotorMoveParameter[iLoopCount].dAcceleration = objMultiMoveParameter[iLoopCount].dAcceleration;
                        objMotorMoveParameter[iLoopCount].dDeceleration = objMultiMoveParameter[iLoopCount].dDeceleration;

                        objMotorMoveParameter[iLoopCount].objMotorVirtual = ((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMultiMoveParameter[iLoopCount].objMotor).HLGetAbstractReference()).m_objMotorVirtual;
                    }
                }

                if (false == m_objMotorVirtual.HLMoveMultiAbsoluteIndex((HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, objMotorMoveParameter))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 다축 절대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public override bool HLMoveMultiAbsolutePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter)
        {
            bool bReturn = false;
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[] objMotorMoveParameter = null;
            do
            {
                if (null != objMultiMoveParameter)
                {
                    objMotorMoveParameter = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[objMultiMoveParameter.Length];
                    for (int iLoopCount = 0; iLoopCount < objMultiMoveParameter.Length; iLoopCount++)
                    {
                        objMotorMoveParameter[iLoopCount] = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter();
                        objMotorMoveParameter[iLoopCount].iPositionIndex = objMultiMoveParameter[iLoopCount].iPositionIndex;
                        objMotorMoveParameter[iLoopCount].dPosition = objMultiMoveParameter[iLoopCount].dPosition;
                        objMotorMoveParameter[iLoopCount].dVelocity = objMultiMoveParameter[iLoopCount].dVelocity;
                        objMotorMoveParameter[iLoopCount].dAcceleration = objMultiMoveParameter[iLoopCount].dAcceleration;
                        objMotorMoveParameter[iLoopCount].dDeceleration = objMultiMoveParameter[iLoopCount].dDeceleration;

                        objMotorMoveParameter[iLoopCount].objMotorVirtual = ((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMultiMoveParameter[iLoopCount].objMotor).HLGetAbstractReference()).m_objMotorVirtual;
                    }
                }

                if (false == m_objMotorVirtual.HLMoveMultiAbsolutePosition((HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, objMotorMoveParameter))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 다축 상대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public override bool HLMoveMultiRelativePositioin(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter)
        {
            bool bReturn = false;
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[] objMotorMoveParameter = null;
            do
            {
                if (null != objMultiMoveParameter)
                {
                    objMotorMoveParameter = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[objMultiMoveParameter.Length];
                    for (int iLoopCount = 0; iLoopCount < objMultiMoveParameter.Length; iLoopCount++)
                    {
                        objMotorMoveParameter[iLoopCount] = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter();
                        objMotorMoveParameter[iLoopCount].iPositionIndex = objMultiMoveParameter[iLoopCount].iPositionIndex;
                        objMotorMoveParameter[iLoopCount].dPosition = objMultiMoveParameter[iLoopCount].dPosition;
                        objMotorMoveParameter[iLoopCount].dVelocity = objMultiMoveParameter[iLoopCount].dVelocity;
                        objMotorMoveParameter[iLoopCount].dAcceleration = objMultiMoveParameter[iLoopCount].dAcceleration;
                        objMotorMoveParameter[iLoopCount].dDeceleration = objMultiMoveParameter[iLoopCount].dDeceleration;

                        objMotorMoveParameter[iLoopCount].objMotorVirtual = ((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMultiMoveParameter[iLoopCount].objMotor).HLGetAbstractReference()).m_objMotorVirtual;
                    }
                }

                if (false == m_objMotorVirtual.HLMoveMultiRelativePositioin((HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, objMotorMoveParameter))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 직선 보간 절대 위치 이동 ( 인덱스 포지션 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public override bool HLMoveLineAbsoluteIndex(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter)
        {
            bool bReturn = false;
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[] objMotorMoveParameter = null;
            do
            {
                if (null != objMultiMoveParameter)
                {
                    objMotorMoveParameter = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[objMultiMoveParameter.Length];
                    for (int iLoopCount = 0; iLoopCount < objMultiMoveParameter.Length; iLoopCount++)
                    {
                        objMotorMoveParameter[iLoopCount] = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter();
                        objMotorMoveParameter[iLoopCount].iPositionIndex = objMultiMoveParameter[iLoopCount].iPositionIndex;
                        objMotorMoveParameter[iLoopCount].dPosition = objMultiMoveParameter[iLoopCount].dPosition;
                        objMotorMoveParameter[iLoopCount].dVelocity = objMultiMoveParameter[iLoopCount].dVelocity;
                        objMotorMoveParameter[iLoopCount].dAcceleration = objMultiMoveParameter[iLoopCount].dAcceleration;
                        objMotorMoveParameter[iLoopCount].dDeceleration = objMultiMoveParameter[iLoopCount].dDeceleration;

                        objMotorMoveParameter[iLoopCount].objMotorVirtual = ((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMultiMoveParameter[iLoopCount].objMotor).HLGetAbstractReference()).m_objMotorVirtual;
                    }
                }

                if (false == m_objMotorVirtual.HLMoveLineAbsoluteIndex((HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, objMotorMoveParameter))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 직선 보간 절대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public override bool HLMoveLineAbsolutePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter)
        {
            bool bReturn = false;
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[] objMotorMoveParameter = null;
            do
            {
                if (null != objMultiMoveParameter)
                {
                    objMotorMoveParameter = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[objMultiMoveParameter.Length];
                    for (int iLoopCount = 0; iLoopCount < objMultiMoveParameter.Length; iLoopCount++)
                    {
                        objMotorMoveParameter[iLoopCount] = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter();
                        objMotorMoveParameter[iLoopCount].iPositionIndex = objMultiMoveParameter[iLoopCount].iPositionIndex;
                        objMotorMoveParameter[iLoopCount].dPosition = objMultiMoveParameter[iLoopCount].dPosition;
                        objMotorMoveParameter[iLoopCount].dVelocity = objMultiMoveParameter[iLoopCount].dVelocity;
                        objMotorMoveParameter[iLoopCount].dAcceleration = objMultiMoveParameter[iLoopCount].dAcceleration;
                        objMotorMoveParameter[iLoopCount].dDeceleration = objMultiMoveParameter[iLoopCount].dDeceleration;

                        objMotorMoveParameter[iLoopCount].objMotorVirtual = ((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMultiMoveParameter[iLoopCount].objMotor).HLGetAbstractReference()).m_objMotorVirtual;
                    }
                }

                if (false == m_objMotorVirtual.HLMoveLineAbsolutePosition((HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, objMotorMoveParameter))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 직선 보간 상대 위치 이동 ( 포지션 값 )
        /// </summary>
        /// <param name="eModeType"></param>
        /// <param name="objMultiMoveParameter"></param>
        /// <returns></returns>
        public override bool HLMoveLineRelativePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter)
        {
            bool bReturn = false;
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[] objMotorMoveParameter = null;
            do
            {
                if (null != objMultiMoveParameter)
                {
                    objMotorMoveParameter = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter[objMultiMoveParameter.Length];
                    for (int iLoopCount = 0; iLoopCount < objMultiMoveParameter.Length; iLoopCount++)
                    {
                        objMotorMoveParameter[iLoopCount] = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMultiMoveParameter();
                        objMotorMoveParameter[iLoopCount].iPositionIndex = objMultiMoveParameter[iLoopCount].iPositionIndex;
                        objMotorMoveParameter[iLoopCount].dPosition = objMultiMoveParameter[iLoopCount].dPosition;
                        objMotorMoveParameter[iLoopCount].dVelocity = objMultiMoveParameter[iLoopCount].dVelocity;
                        objMotorMoveParameter[iLoopCount].dAcceleration = objMultiMoveParameter[iLoopCount].dAcceleration;
                        objMotorMoveParameter[iLoopCount].dDeceleration = objMultiMoveParameter[iLoopCount].dDeceleration;

                        objMotorMoveParameter[iLoopCount].objMotorVirtual = ((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMultiMoveParameter[iLoopCount].objMotor).HLGetAbstractReference()).m_objMotorVirtual;
                    }
                }

                if (false == m_objMotorVirtual.HLMoveLineRelativePosition((HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, objMotorMoveParameter))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 다축 감속 정지
        /// </summary>
        /// <param name="objMotorVirtual"></param>
        /// <returns></returns>
        public override bool HLMoveMultiStop(object[] objMotorVirtual)
        {
            bool bReturn = false;
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtual[] objMotor = null;
            do
            {
                if (null != objMotorVirtual)
                {
                    objMotor = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtual[objMotorVirtual.Length];
                    for (int iLoopCount = 0; iLoopCount < objMotorVirtual.Length; iLoopCount++)
                    {
                        objMotor[iLoopCount] = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtual();
                        objMotor[iLoopCount] = ((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMotorVirtual[iLoopCount]).HLGetAbstractReference()).m_objMotorVirtual;
                    }
                }

                if (false == m_objMotorVirtual.HLMoveMultiStop(objMotor))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 다축 긴급 정지
        /// </summary>
        /// <param name="objMotorVirtual"></param>
        /// <returns></returns>
        public override bool HLMoveMultiEStop(object[] objMotorVirtual)
        {
            bool bReturn = false;
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtual[] objMotor = null;
            do
            {
                if (null != objMotorVirtual)
                {
                    objMotor = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtual[objMotorVirtual.Length];
                    for (int iLoopCount = 0; iLoopCount < objMotorVirtual.Length; iLoopCount++)
                    {
                        objMotor[iLoopCount] = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtual();
                        objMotor[iLoopCount] = ((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMotorVirtual[iLoopCount]).HLGetAbstractReference()).m_objMotorVirtual;
                    }
                }

                if (false == m_objMotorVirtual.HLMoveMultiEStop(objMotor))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 위치 오버라이드
        /// </summary>
        /// <param name="dOverridePosition"></param>
        /// <returns></returns>
        public override bool HLOverridePosition(double dOverridePosition)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLOverridePosition(dOverridePosition))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 속도 오버라이드
        /// </summary>
        /// <param name="dMaxOverrideVelocity"></param>
        /// <returns></returns>
        public override bool HLOverrideMaxVelocity(double dMaxOverrideVelocity)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLOverrideMaxVelocity(dMaxOverrideVelocity))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 속도 오버라이드
        /// </summary>
        /// <param name="dOverrideVelocity"></param>
        /// <returns></returns>
        public override bool HLOverrideVelocity(double dOverrideVelocity)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLOverrideVelocity(dOverrideVelocity))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 단일 지정 위치 도달 했을 경우 오버라이드 된 속도로 구동( 포지션 인덱스 값 절대 위치 )
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
        public override bool HLMoveOverrideAtIndex(int iPositionIndex, EVelocityMode eModeType, double dOverridePosition, double dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLMoveOverrideAtIndex(iPositionIndex, (HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, dOverridePosition, dOverrideVelocity, dMaxOverrideVelocity, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
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
        public override bool HLMoveOverrideAtPosition(double dPosition, EVelocityMode eModeType, double dOverridePosition, double dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLMoveOverrideAtPosition(dPosition, (HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, dOverridePosition, dOverrideVelocity, dMaxOverrideVelocity, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
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
        public override bool HLMoveMultiOverrideAtIndex(int iPositionIndex, EVelocityMode eModeType, double[] dOverridePosition, double[] dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLMoveMultiOverrideAtIndex(iPositionIndex, (HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, dOverridePosition, dOverrideVelocity, dMaxOverrideVelocity, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
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
        public override bool HLMoveMultiOverrideAtPosition(double dPosition, EVelocityMode eModeType, double[] dOverridePosition, double[] dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLMoveMultiOverrideAtPosition(dPosition, (HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.enumVelocityMode)eModeType, dOverridePosition, dOverrideVelocity, dMaxOverrideVelocity, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 지정 축의 트리거 출력 시간, 트리거 레벨, 트리거 출력 방법, 트리거 출력시 인터럽트 설정여부
        /// </summary>
        /// <param name="dTriggerTimeWidth"></param>
        /// <returns></returns>
        public override bool HLSetTriggerTime(double dTriggerTimeWidth)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLSetTriggerTime(dTriggerTimeWidth))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLSetTriggerTime(int iAxis, double dTriggerTimeWidth)
        {
            bool bReturn = false;
            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 지정한 시작 위치부터 종료 위치까지 일정 구간 마다 트리거를 출력한다.
        /// </summary>
        /// <param name="dStartPosition"></param>
        /// <param name="dEndPosition"></param>
        /// <param name="dPeriodPosition"></param>
        /// <returns></returns>
        public override bool HLSetTriggerBlock(double dStartPosition, double dEndPosition, double dPeriodPosition)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLSetTriggerBlock(dStartPosition, dEndPosition, dPeriodPosition))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLSetTriggerBlock(int iAxis, double dStartPosition, double dEndPosition, double dPeriodPosition)
        {
            bool bReturn = false;
            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 트리거 블록 설정을 초기화함
        /// </summary>
        /// <param name="iAxis">축 번호</param>
        /// <returns>성공 여부</returns>
        public override bool HLSetTriggerBlockReset()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLSetTriggerBlockReset())
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 알람 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public override CMotorError HLGetMotorError()
        {
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorError objMotorError = m_objMotorVirtual.HLGetMotorError();
            m_objMotorError.strEventTime = objMotorError.strEventTime;
            m_objMotorError.strFunctionName = objMotorError.strFunctionName;
            m_objMotorError.iReturnCode = objMotorError.iReturnCode;
            m_objMotorError.strMessage = objMotorError.strMessage;
            return (CMotorError)m_objMotorError.Clone();
        }

        /// <summary>
        /// 포지션 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public override CMotorPosition HLGetMotorPosition()
        {
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorPosition objMotorPosition = m_objMotorVirtual.HLGetMotorPosition();
            for (int iLoopCount = 0; iLoopCount < DEF_MAX_POSITION_COUNT; iLoopCount++)
            {
                m_objMotorPosition.strPositionName[iLoopCount] = objMotorPosition.strPositionName[iLoopCount];
                m_objMotorPosition.dPosition[iLoopCount] = objMotorPosition.dPosition[iLoopCount];
                m_objMotorPosition.dPrevPosition[iLoopCount] = objMotorPosition.dPrevPosition[iLoopCount];
                m_objMotorPosition.dVelocity[iLoopCount] = objMotorPosition.dVelocity[iLoopCount];
                m_objMotorPosition.dAcceleration[iLoopCount] = objMotorPosition.dAcceleration[iLoopCount];
                m_objMotorPosition.dDeceleration[iLoopCount] = objMotorPosition.dDeceleration[iLoopCount];
            }
            return (CMotorPosition)m_objMotorPosition.Clone();
        }

        /// <summary>
        /// 모터 운용 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public override CMotorOperationParameter HLGetMotorOperationParameter()
        {
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorOperationParameter objMotorOperationParameter = m_objMotorVirtual.HLGetMotorOperationParameter();
            m_objMotorOperationParameter.dAccelerationHomeFirst = objMotorOperationParameter.dAccelerationHomeFirst;
            m_objMotorOperationParameter.dAccelerationHomeSecond = objMotorOperationParameter.dAccelerationHomeSecond;
            m_objMotorOperationParameter.dHomeOffset = objMotorOperationParameter.dHomeOffset;
            m_objMotorOperationParameter.dLimitSoftwareMinus = objMotorOperationParameter.dLimitSoftwareMinus;
            m_objMotorOperationParameter.dLimitSoftwarePlus = objMotorOperationParameter.dLimitSoftwarePlus;
            m_objMotorOperationParameter.dStandardAcceleration = objMotorOperationParameter.dStandardAcceleration;
            m_objMotorOperationParameter.dStandardAutoVelocity = objMotorOperationParameter.dStandardAutoVelocity;
            m_objMotorOperationParameter.dStandardDeceleration = objMotorOperationParameter.dStandardDeceleration;
            m_objMotorOperationParameter.dStandardManualVelocity = objMotorOperationParameter.dStandardManualVelocity;
            m_objMotorOperationParameter.dStandardTolerence = objMotorOperationParameter.dStandardTolerence;
            m_objMotorOperationParameter.dVelocityHomeFirst = objMotorOperationParameter.dVelocityHomeFirst;
            m_objMotorOperationParameter.dVelocityHomeLast = objMotorOperationParameter.dVelocityHomeLast;
            m_objMotorOperationParameter.dVelocityHomeSecond = objMotorOperationParameter.dVelocityHomeSecond;
            m_objMotorOperationParameter.dVelocityHomeThird = objMotorOperationParameter.dVelocityHomeThird;
            m_objMotorOperationParameter.dVelocityJogFast = objMotorOperationParameter.dVelocityJogFast;
            m_objMotorOperationParameter.dVelocityJogSlow = objMotorOperationParameter.dVelocityJogSlow;
            m_objMotorOperationParameter.bUseHome = objMotorOperationParameter.bUseHome;
            m_objMotorOperationParameter.iDelayAfterMoving = objMotorOperationParameter.iDelayAfterMoving;

            if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_MOVE_DIR.DIR_CCW == objMotorOperationParameter.eHomeDirection)
            {
                m_objMotorOperationParameter.eHomeDirection = EMotionDirection.DIR_CCW;
            }
            else
            {
                m_objMotorOperationParameter.eHomeDirection = EMotionDirection.DIR_CW;
            }

            if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_HOME_DETECT.PosEndLimit == objMotorOperationParameter.eMotionHomeDetect)
            {
                m_objMotorOperationParameter.eHomeDetect = EHomeDetect.POS_END_LIMIT;
            }
            else if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_HOME_DETECT.NegEndLimit == objMotorOperationParameter.eMotionHomeDetect)
            {
                m_objMotorOperationParameter.eHomeDetect = EHomeDetect.NEG_END_LIMIT;
            }
            else if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_HOME_DETECT.HomeSensor == objMotorOperationParameter.eMotionHomeDetect)
            {
                m_objMotorOperationParameter.eHomeDetect = EHomeDetect.HOME_SENSOR;
            }
            else if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_HOME_DETECT.EncodZPhase == objMotorOperationParameter.eMotionHomeDetect)
            {
                m_objMotorOperationParameter.eHomeDetect = EHomeDetect.ENCODER_Z_PHASE;
            }

            if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_LEVEL_MODE.LOW == objMotorOperationParameter.eMotionLevel)
            {
                m_objMotorOperationParameter.eMotionLevel = EMotionLevel.LEVEL_LOW;
            }
            else if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_LEVEL_MODE.LOW == objMotorOperationParameter.eMotionLevel)
            {
                m_objMotorOperationParameter.eMotionLevel = EMotionLevel.LEVEL_HIGH;
            }
            else if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_LEVEL_MODE.LOW == objMotorOperationParameter.eMotionLevel)
            {
                m_objMotorOperationParameter.eMotionLevel = EMotionLevel.LEVEL_UNUSED;
            }
            else if (HLDeviceDLL.Motor.Virtual.AXT_MOTION_LEVEL_MODE.LOW == objMotorOperationParameter.eMotionLevel)
            {
                m_objMotorOperationParameter.eMotionLevel = EMotionLevel.LEVEL_USED;
            }

            m_objMotorOperationParameter.iHomeClearTime = objMotorOperationParameter.iHomeClearTime;
            m_objMotorOperationParameter.iHomePriority = objMotorOperationParameter.iHomePriority;
            m_objMotorOperationParameter.iStandardLimitTimeOut = objMotorOperationParameter.iStandardLimitTimeOut;
            m_objMotorOperationParameter.iUseZPhase = objMotorOperationParameter.iUseZPhase;
            return (CMotorOperationParameter)m_objMotorOperationParameter.Clone();
        }

        /// <summary>
        /// 초기화 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public override CMotorInitializeParameter HLGetMotorInitializeParameter()
        {
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorInitializeParameter objMotorInitializeParameter = m_objMotorVirtual.HLGetMotorInitializeParameter();
            m_objInitializeParameter.iAxisNo = objMotorInitializeParameter.iAxisNo;
            m_objInitializeParameter.iBoardNo = objMotorInitializeParameter.iBoardNo;
            m_objInitializeParameter.strFilePath = objMotorInitializeParameter.strFilePath;
            m_objInitializeParameter.strModelName = objMotorInitializeParameter.strModelName;
            m_objInitializeParameter.strMotorName = objMotorInitializeParameter.strMotorName;
            return (CMotorInitializeParameter)m_objInitializeParameter.Clone();
        }

        /// <summary>
        /// 초기화 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public override void HLSetMotorInitializeParameter(CMotorInitializeParameter setParameter)
        {
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorInitializeParameter objMotorInitializeParameter = new HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorInitializeParameter();
            objMotorInitializeParameter.iAxisNo = setParameter.iAxisNo;
            objMotorInitializeParameter.iBoardNo = setParameter.iBoardNo;
            objMotorInitializeParameter.strFilePath = setParameter.strFilePath;
            objMotorInitializeParameter.strModelName = setParameter.strModelName;
            objMotorInitializeParameter.strMotorName = setParameter.strMotorName;
            m_objMotorVirtual.HLSetMotorInitializeParameter(objMotorInitializeParameter);
        }

        /// <summary>
        /// 모터 상태 정보 객체를 얻어 온다.
        /// </summary>
        /// <returns></returns>
        public override CMotorStatus HLGetMotorStatus()
        {
            HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorStatus objMotorStatus = m_objMotorVirtual.HLGetMotorStatus();
            m_objMotorStatus = new CMotorStatus(
                objMotorStatus.bServo,
                objMotorStatus.bHome,
                objMotorStatus.bInposition,
                objMotorStatus.bAlarm,
                objMotorStatus.bBusy,
                objMotorStatus.bHomeSensor,
                objMotorStatus.bMinusLimitSensor,
                objMotorStatus.bPlusLimitSensor,
                objMotorStatus.dCommandPosition,
                objMotorStatus.dEncoderPosition,
                objMotorStatus.dServoLoadRatio,
                objMotorStatus.dCurrentVelocity
                );
            return m_objMotorStatus;
        }

        /// <summary>
        /// 모터 이동 후 인포지션 확인.
        /// </summary>
        /// <param name="objMotor"></param>
        /// <param name="eCompareResult"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        public override bool HLWaitMotorStatus(object objMotor, EPositionCompare eCompareResult, int iTimeOut, int iSleepPeriod = 10)
        {
            bool bResult = false;
            if (EPositionCompare.POSITION_COMPARE_EQUAL == eCompareResult)
                bResult = true;
            else if (EPositionCompare.POSITION_COMPARE_UNEQUAL == eCompareResult)
                bResult = false;
            else
                bResult = false;
            return m_objMotorVirtual.HLWaitMotorStatus(((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMotor).HLGetAbstractReference()).m_objMotorVirtual, bResult, iTimeOut, iSleepPeriod);
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
        public override bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, int iPositionIndex, int iTimeOut, int iSleepPeriod = 10)
        {
            bool bResult = false;
            if (EPositionCompare.POSITION_COMPARE_EQUAL == eCompareResult)
                bResult = true;
            else if (EPositionCompare.POSITION_COMPARE_UNEQUAL == eCompareResult)
                bResult = false;
            else
                bResult = false;
            return m_objMotorVirtual.HLWaitMotorPositionStatus(((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMotor).HLGetAbstractReference()).m_objMotorVirtual, bResult, iPositionIndex, iTimeOut, iSleepPeriod);
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
        public override bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, double dStartPosition, double dPosition, int iTimeOut, int iSleepPeriod = 10)
        {
            bool bResult = false;
            if (EPositionCompare.POSITION_COMPARE_EQUAL == eCompareResult)
                bResult = true;
            else if (EPositionCompare.POSITION_COMPARE_UNEQUAL == eCompareResult)
                bResult = false;
            else
                bResult = false;
            return m_objMotorVirtual.HLWaitMotorPositionStatus(((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMotor).HLGetAbstractReference()).m_objMotorVirtual, bResult, dStartPosition, dPosition, iTimeOut, iSleepPeriod);
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
        public override bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, double dPosition, int iTimeOut, int iSleepPeriod = 10)
        {
            bool bResult = false;
            if (EPositionCompare.POSITION_COMPARE_EQUAL == eCompareResult)
                bResult = true;
            else if (EPositionCompare.POSITION_COMPARE_UNEQUAL == eCompareResult)
                bResult = false;
            else
                bResult = false;
            return m_objMotorVirtual.HLWaitMotorPositionStatus(((HLDevice.Motor.CDeviceMotorVirtual)((HLDevice.CDeviceMotor)objMotor).HLGetAbstractReference()).m_objMotorVirtual, bResult, dPosition, iTimeOut, iSleepPeriod);
        }

        /// <summary>
        /// 모터 운용 파라미터 로딩
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public override bool HLLoadMotorOperationParameter(string strFilePath, string strModelName)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLLoadMotorOperationParameter(strFilePath, strModelName))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 운용 파라미터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <param name="objMotorOperationParameter"></param>
        /// <returns></returns>
        public override bool HLSaveMotorOperationParameter(string strFilePath, string strModelName, CMotorOperationParameter objMotorOperationParameter)
        {
            bool bReturn = false;
            do
            {
                HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorOperationParameter objMotorOperationParameterDll = m_objMotorVirtual.HLGetMotorOperationParameter();
                objMotorOperationParameterDll.dAccelerationHomeFirst = objMotorOperationParameter.dAccelerationHomeFirst;
                objMotorOperationParameterDll.dAccelerationHomeSecond = objMotorOperationParameter.dAccelerationHomeSecond;
                objMotorOperationParameterDll.dHomeOffset = objMotorOperationParameter.dHomeOffset;
                objMotorOperationParameterDll.dLimitSoftwareMinus = objMotorOperationParameter.dLimitSoftwareMinus;
                objMotorOperationParameterDll.dLimitSoftwarePlus = objMotorOperationParameter.dLimitSoftwarePlus;
                objMotorOperationParameterDll.dStandardAcceleration = objMotorOperationParameter.dStandardAcceleration;
                objMotorOperationParameterDll.dStandardAutoVelocity = objMotorOperationParameter.dStandardAutoVelocity;
                objMotorOperationParameterDll.dStandardDeceleration = objMotorOperationParameter.dStandardDeceleration;
                objMotorOperationParameterDll.dStandardManualVelocity = objMotorOperationParameter.dStandardManualVelocity;
                objMotorOperationParameterDll.dStandardTolerence = objMotorOperationParameter.dStandardTolerence;
                objMotorOperationParameterDll.dVelocityHomeFirst = objMotorOperationParameter.dVelocityHomeFirst;
                objMotorOperationParameterDll.dVelocityHomeLast = objMotorOperationParameter.dVelocityHomeLast;
                objMotorOperationParameterDll.dVelocityHomeSecond = objMotorOperationParameter.dVelocityHomeSecond;
                objMotorOperationParameterDll.dVelocityHomeThird = objMotorOperationParameter.dVelocityHomeThird;
                objMotorOperationParameterDll.dVelocityJogFast = objMotorOperationParameter.dVelocityJogFast;
                objMotorOperationParameterDll.dVelocityJogSlow = objMotorOperationParameter.dVelocityJogSlow;
                objMotorOperationParameterDll.bUseHome = objMotorOperationParameter.bUseHome;
                objMotorOperationParameterDll.iDelayAfterMoving = objMotorOperationParameter.iDelayAfterMoving;

                if (EMotionDirection.DIR_CCW == objMotorOperationParameter.eHomeDirection)
                {
                    objMotorOperationParameterDll.eHomeDirection = HLDeviceDLL.Motor.Virtual.AXT_MOTION_MOVE_DIR.DIR_CCW;
                }
                else
                {
                    objMotorOperationParameterDll.eHomeDirection = HLDeviceDLL.Motor.Virtual.AXT_MOTION_MOVE_DIR.DIR_CW;
                }

                if (EHomeDetect.POS_END_LIMIT == objMotorOperationParameter.eHomeDetect)
                {
                    objMotorOperationParameterDll.eMotionHomeDetect = HLDeviceDLL.Motor.Virtual.AXT_MOTION_HOME_DETECT.PosEndLimit;
                }
                else if (EHomeDetect.NEG_END_LIMIT == objMotorOperationParameter.eHomeDetect)
                {
                    objMotorOperationParameterDll.eMotionHomeDetect = HLDeviceDLL.Motor.Virtual.AXT_MOTION_HOME_DETECT.NegEndLimit;
                }
                else if (EHomeDetect.HOME_SENSOR == objMotorOperationParameter.eHomeDetect)
                {
                    objMotorOperationParameterDll.eMotionHomeDetect = HLDeviceDLL.Motor.Virtual.AXT_MOTION_HOME_DETECT.HomeSensor;
                }
                else if (EHomeDetect.ENCODER_Z_PHASE == objMotorOperationParameter.eHomeDetect)
                {
                    objMotorOperationParameterDll.eMotionHomeDetect = HLDeviceDLL.Motor.Virtual.AXT_MOTION_HOME_DETECT.EncodZPhase;
                }

                if (EMotionLevel.LEVEL_LOW == objMotorOperationParameter.eMotionLevel)
                {
                    objMotorOperationParameterDll.eMotionLevel = HLDeviceDLL.Motor.Virtual.AXT_MOTION_LEVEL_MODE.LOW;
                }
                else if (EMotionLevel.LEVEL_HIGH == objMotorOperationParameter.eMotionLevel)
                {
                    objMotorOperationParameterDll.eMotionLevel = HLDeviceDLL.Motor.Virtual.AXT_MOTION_LEVEL_MODE.LOW;
                }
                else if (EMotionLevel.LEVEL_UNUSED == objMotorOperationParameter.eMotionLevel)
                {
                    objMotorOperationParameterDll.eMotionLevel = HLDeviceDLL.Motor.Virtual.AXT_MOTION_LEVEL_MODE.LOW;
                }
                else if (EMotionLevel.LEVEL_USED == objMotorOperationParameter.eMotionLevel)
                {
                    objMotorOperationParameterDll.eMotionLevel = HLDeviceDLL.Motor.Virtual.AXT_MOTION_LEVEL_MODE.LOW;
                }

                objMotorOperationParameterDll.iHomeClearTime = objMotorOperationParameter.iHomeClearTime;
                objMotorOperationParameterDll.iHomePriority = objMotorOperationParameter.iHomePriority;
                objMotorOperationParameterDll.iStandardLimitTimeOut = objMotorOperationParameter.iStandardLimitTimeOut;
                objMotorOperationParameterDll.iUseZPhase = objMotorOperationParameter.iUseZPhase;

                if (false == m_objMotorVirtual.HLSaveMotorOperationParameter(strFilePath, strModelName, objMotorOperationParameterDll))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 포지션 파라미터 로딩
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public override bool HLLoadMotorPositionParameter(string strFilePath, string strModelName)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotorVirtual.HLLoadMotorPositionParameter(strFilePath, strModelName))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 포지션 파라미터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strModelName"></param>
        /// <param name="objMotorPosition"></param>
        /// <returns></returns>
        public override bool HLSaveMotorPositionParameter(string strFilePath, string strModelName, CMotorPosition objMotorPosition)
        {
            bool bReturn = false;
            do
            {
                HLDeviceDLL.Motor.Virtual.CDeviceMotorVirtualDefine.CMotorPosition objMotorPositionDll = m_objMotorVirtual.HLGetMotorPosition();
                for (int iLoopCount = 0; iLoopCount < DEF_MAX_POSITION_COUNT; iLoopCount++)
                {
                    objMotorPositionDll.strPositionName[iLoopCount] = objMotorPosition.strPositionName[iLoopCount];
                    objMotorPositionDll.dPosition[iLoopCount] = objMotorPosition.dPosition[iLoopCount];
                    objMotorPositionDll.dPrevPosition[iLoopCount] = objMotorPosition.dPrevPosition[iLoopCount];
                    objMotorPositionDll.dVelocity[iLoopCount] = objMotorPosition.dVelocity[iLoopCount];
                    objMotorPositionDll.dAcceleration[iLoopCount] = objMotorPosition.dAcceleration[iLoopCount];
                    objMotorPositionDll.dDeceleration[iLoopCount] = objMotorPosition.dDeceleration[iLoopCount];
                }

                if (false == m_objMotorVirtual.HLSaveMotorPositionParameter(strFilePath, strModelName, objMotorPositionDll))
                {
                    m_objMotorVirtual.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해당 축에 서보 드라이버에 경고 코드를 요청한다.
        /// </summary>
        /// <returns>True=요청 성공, False=읽기 진행중</returns>
        public override bool HLSetStatusRequestReadA5nWarningData()
        {
            return m_objMotorVirtual.HLSetStatusRequestReadA5nWarningData();
        }

        /// <summary>
        /// 해당 축에서 경고 코드 요청 결과를 읽어온다.
        /// 이 함수는 HLSetStatusRequestReadA5nWarningData() 함수 호출 후에 호출해야한다.
        /// </summary>
        /// <param name="warningCode">경고 코드</param>
        /// <returns>True=성공, False=읽기 진행중</returns>
        public override bool HLGetStatusA5nWarningCode(ref uint warningCode)
        {
            return m_objMotorVirtual.HLGetStatusA5nWarningCode(ref warningCode);
        }
    }
}

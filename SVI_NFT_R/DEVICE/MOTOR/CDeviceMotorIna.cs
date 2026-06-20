using ENCDeviceDLL.Motor.Ina;
using HLDevice.Abstract;
using System.Threading;

namespace HLDevice.Motor
{
    public class CDeviceMotorIna : CDeviceMotorAbstract
    {
        public readonly ENCDeviceDLL.Motor.Ina.CDeviceMotorIna m_objMotor = new ENCDeviceDLL.Motor.Ina.CDeviceMotorIna();
        public override int OverrideSpeedRate
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
        private CMotorPosition m_objMotorPosition = new CMotorPosition();
        private CMotorInitializeParameter m_objInitializeParameter = new CMotorInitializeParameter();
        private CMotorOperationParameter m_objMotorOperationParameter = new CMotorOperationParameter();
        private CMotorStatus m_objMotorStatus = new CMotorStatus();
        private CMotorError m_objMotorError = new CMotorError();

        public override bool HLInitialize(CMotorInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;
            do
            {
                // 모터 초기화 객체 생성
                ENCDeviceDLL.Motor.Ina.CDeviceMotorInaDefine.CMotorInitializeParameter objInitializeParameterDLL = new ENCDeviceDLL.Motor.Ina.CDeviceMotorInaDefine.CMotorInitializeParameter();
                objInitializeParameterDLL.LibraryType = (ENCDeviceDLL.Motor.Ina.CDeviceMotorInaDefine.ELibrary)objInitializeParameter.iBoardNo;
                objInitializeParameterDLL.SlaveIndex = objInitializeParameter.iAxisNo;
                objInitializeParameterDLL.PortName = objInitializeParameter.strIPAddress;
                objInitializeParameterDLL.BaudRate = objInitializeParameter.iPort;
                objInitializeParameterDLL.strFilePath = objInitializeParameter.strFilePath;
                objInitializeParameterDLL.strModelName = objInitializeParameter.strModelName;
                objInitializeParameterDLL.strMotorName = objInitializeParameter.strMotorName;
                if (false == m_objMotor.HLInitialize(objInitializeParameterDLL))
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override void HLDeInitialize()
        {
            m_objMotor.HLDeInitialize();
        }

        public override string HLGetVersion()
        {
            return m_objMotor.GetVersion();
        }

        public override bool HLAlarmReset()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLAlarmReset())
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override EPositionCompare HLGetCompareTargetPosition(double dPosition)
        {
            bool bReturn = false;
            EPositionCompare ePositionCompare = EPositionCompare.POSITION_COMPARE_UNKNOWN;
            do
            {
                bReturn = m_objMotor.HLGetCompareTargetPosition(dPosition);
                if (false == bReturn)
                    ePositionCompare = EPositionCompare.POSITION_COMPARE_UNEQUAL;
                else if (true == bReturn)
                    ePositionCompare = EPositionCompare.POSITION_COMPARE_EQUAL;
                bReturn = true;
            } while (false);

            return ePositionCompare;
        }

        public override EPositionCompare HLGetCompareTargetPosition(double dStartPosition, double dPosition)
        {
            bool bReturn = false;
            EPositionCompare ePositionCompare = EPositionCompare.POSITION_COMPARE_UNKNOWN;
            do
            {
                bReturn = m_objMotor.HLGetCompareTargetPosition(dStartPosition, dPosition);
                if (false == bReturn)
                    ePositionCompare = EPositionCompare.POSITION_COMPARE_UNEQUAL;
                else if (true == bReturn)
                    ePositionCompare = EPositionCompare.POSITION_COMPARE_EQUAL;
                bReturn = true;
            } while (false);

            return ePositionCompare;
        }

        public override EPositionCompare HLGetCompareTargetPosition(int iPositionIndex)
        {
            bool bReturn = false;
            EPositionCompare ePositionCompare = EPositionCompare.POSITION_COMPARE_UNKNOWN;
            do
            {
                bReturn = m_objMotor.HLGetCompareTargetPosition(iPositionIndex);
                if (false == bReturn)
                    ePositionCompare = EPositionCompare.POSITION_COMPARE_UNEQUAL;
                else if (true == bReturn)
                    ePositionCompare = EPositionCompare.POSITION_COMPARE_EQUAL;
                bReturn = true;
            } while (false);

            return ePositionCompare;
        }

        public override bool HLSetHomeProcess()
        {
            bool bReturn = false;
            do
            {

                if (false == m_objMotor.HLSetHomeProcess())
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLGetHomeProcessRate(ref int iHomeMainStepNo, ref int iHomeStepNo)
        {
            bool bReturn = false;
            int iHomeMainStepNumber = 0;
            int iHomeStepNumber = 0;
            do
            {
                if (false == m_objMotor.HLGetHomeProcessRate(ref iHomeMainStepNumber, ref iHomeStepNumber))
                {
                    m_objMotor.HLGetMotorError();
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

        public override bool HLServoOn(EUse eUse)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLServoOn(eUse == EUse.ENABLE))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLSetSoftwareLimit(EUse eUse)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLSetSoftwareLimit(eUse == EUse.ENABLE))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLSetPositionZeroSet()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLSetPositionZeroSet())
                {
                    m_objMotor.HLGetMotorError();
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

        public override bool HLSetUnitPerPulse(double dUnit, int iPulse)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLSetUnitPerPulse(dUnit, iPulse))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLGetUnitperPulse(ref double dUnit, ref int iPulse)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLGetUnitPerPulse(ref dUnit, ref iPulse))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLJogFastMove(EMoveDirection eDirection)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLJogFastMove(eDirection == EMoveDirection.DIRECTION_CW ? CDeviceMotorInaDefine.EMoveDirection.CW : CDeviceMotorInaDefine.EMoveDirection.CCW))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLJogSlowMove(EMoveDirection eDirection)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLJogSlowMove(eDirection == EMoveDirection.DIRECTION_CW ? CDeviceMotorInaDefine.EMoveDirection.CW : CDeviceMotorInaDefine.EMoveDirection.CCW))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLMoveContinue(EMoveDirection eDirection, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0) => true;

        public override bool HLMoveAbsoluteIndex(int iPositionIndex, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLMoveAbsoluteIndex(iPositionIndex, (CDeviceMotorInaDefine.ERunSpeed)eModeType, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLMoveAbsolutePosition(int index, double dPosition, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLMoveAbsolutePosition(index, dPosition, (CDeviceMotorInaDefine.ERunSpeed)eModeType, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLMoveAbsolutePosition(double dPosition, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLMoveAbsolutePosition(dPosition, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLMoveRelativePosition(double dPosition, EVelocityMode eModeType, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLMoveRelativePosition(dPosition, (CDeviceMotorInaDefine.ERunSpeed)eModeType, dVelocity, dAcceleration, dDeceleration))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLMoveStop()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLMoveStop())
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLMoveEStop()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLMoveEStop())
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLMoveMultiAbsoluteIndex(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter) => true;

        public override bool HLMoveMultiAbsolutePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter) => true;

        public override bool HLMoveMultiRelativePositioin(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter) => true;

        public override bool HLMoveLineAbsoluteIndex(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter) => true;

        public override bool HLMoveLineAbsolutePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter) => true;

        public override bool HLMoveLineRelativePosition(EVelocityMode eModeType, CMultiMoveParameter[] objMultiMoveParameter) => true;

        public override bool HLMoveMultiStop(object[] objMotorFastech)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.Controller.Stop(0))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        public override bool HLMoveMultiEStop(object[] objMotorFastech)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.Controller.Stop(0))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        public override bool HLOverridePosition(double dOverridePosition) => true;

        public override bool HLOverrideMaxVelocity(double dMaxOverrideVelocity) => true;

        public override bool HLOverrideVelocity(double dOverrideVelocity) => true;

        public override bool HLMoveOverrideAtIndex(int iPositionIndex, EVelocityMode eModeType, double dOverridePosition, double dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0) => true;

        public override bool HLMoveOverrideAtPosition(double dPosition, EVelocityMode eModeType, double dOverridePosition, double dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0) => true;

        public override bool HLMoveMultiOverrideAtIndex(int iPositionIndex, EVelocityMode eModeType, double[] dOverridePosition, double[] dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0) => true;

        public override bool HLMoveMultiOverrideAtPosition(double dPosition, EVelocityMode eModeType, double[] dOverridePosition, double[] dOverrideVelocity, double dMaxOverrideVelocity, double dVelocity = 0, double dAcceleration = 0, double dDeceleration = 0) => true;

        public override bool HLSetTriggerTime(double dTriggerTimeWidth) => true;

        public override bool HLSetTriggerTime(int iAxis, double dTriggerTimeWidth) => true;

        public override bool HLSetTriggerBlock(double dStartPosition, double dEndPosition, double dPeriodPosition) => true;

        public override bool HLSetTriggerBlock(int iAxis, double dStartPosition, double dEndPosition, double dPeriodPosition) => true;

        public override bool HLSetTriggerBlockReset() => true;

        public override CMotorError HLGetMotorError()
        {
            var objMotorError = m_objMotor.HLGetMotorError();
            m_objMotorError.strEventTime = objMotorError.strEventTime;
            m_objMotorError.strFunctionName = objMotorError.strFunctionName;
            m_objMotorError.iReturnCode = objMotorError.iReturnCode;
            m_objMotorError.strMessage = objMotorError.strMessage;
            return (CMotorError)m_objMotorError.Clone();
        }

        public override CMotorPosition HLGetMotorPosition()
        {
            var objMotorPosition = m_objMotor.HLGetMotorPosition();
            for (int iLoopCount = 0; iLoopCount < DEF_MAX_POSITION_COUNT; iLoopCount++)
            {
                m_objMotorPosition.strPositionName[iLoopCount] = objMotorPosition.strPositionName[iLoopCount];
                m_objMotorPosition.dPosition[iLoopCount] = objMotorPosition.dPosition[iLoopCount];
                m_objMotorPosition.dPrevPosition[iLoopCount] = objMotorPosition.dPrevPosition[iLoopCount];
                m_objMotorPosition.dVelocity[iLoopCount] = objMotorPosition.dVelocity[iLoopCount];
                m_objMotorPosition.dAcceleration[iLoopCount] = objMotorPosition.dAcceleration[iLoopCount];
                m_objMotorPosition.dDeceleration[iLoopCount] = objMotorPosition.dDeceleration[iLoopCount];
            }
            return m_objMotorPosition;
        }

        public override CMotorOperationParameter HLGetMotorOperationParameter()
        {
            var objMotorOperationParameter = m_objMotor.HLGetMotorOperationParameter();
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

            if (CDeviceMotorInaDefine.EMoveDirection.CCW == objMotorOperationParameter.eHomeDirection)
            {
                m_objMotorOperationParameter.eHomeDirection = EMotionDirection.DIR_CCW;
            }
            else
            {
                m_objMotorOperationParameter.eHomeDirection = EMotionDirection.DIR_CW;
            }

            if (CDeviceMotorInaDefine.EHomeMethod.ORIGIN == objMotorOperationParameter.eMotionHomeDetect)
            {
                m_objMotorOperationParameter.eHomeDetect = EHomeDetect.HOME_SENSOR;
            }

            if (CDeviceMotorInaDefine.ELevel.None == objMotorOperationParameter.eMotionLevel)
            {
                m_objMotorOperationParameter.eMotionLevel = EMotionLevel.LEVEL_LOW;
            }

            m_objMotorOperationParameter.iHomeClearTime = objMotorOperationParameter.iHomeClearTime;
            m_objMotorOperationParameter.iHomePriority = objMotorOperationParameter.iHomePriority;
            m_objMotorOperationParameter.iStandardLimitTimeOut = objMotorOperationParameter.iStandardLimitTimeOut;
            m_objMotorOperationParameter.iUseZPhase = objMotorOperationParameter.iUseZPhase;
            return (CMotorOperationParameter)m_objMotorOperationParameter.Clone();
        }

        public override CMotorInitializeParameter HLGetMotorInitializeParameter()
        {
            var objMotorInitializeParameter = m_objMotor.HLGetMotorInitializeParameter();
            m_objInitializeParameter.iAxisNo = objMotorInitializeParameter.SlaveIndex;
            m_objInitializeParameter.iBoardNo = (int)objMotorInitializeParameter.LibraryType;
            m_objInitializeParameter.iPort = objMotorInitializeParameter.BaudRate;
            m_objInitializeParameter.strIPAddress = objMotorInitializeParameter.PortName;
            m_objInitializeParameter.strFilePath = objMotorInitializeParameter.strFilePath;
            m_objInitializeParameter.strModelName = objMotorInitializeParameter.strModelName;
            m_objInitializeParameter.strMotorName = objMotorInitializeParameter.strMotorName;
            return m_objInitializeParameter;
        }

        public override void HLSetMotorInitializeParameter(CMotorInitializeParameter setParameter)
        {
            var objMotorInitializeParameter = new CDeviceMotorInaDefine.CMotorInitializeParameter()
            {
                SlaveIndex = setParameter.iAxisNo,
                LibraryType = (CDeviceMotorInaDefine.ELibrary)setParameter.iBoardNo,
                PortName = setParameter.strIPAddress,
                BaudRate = setParameter.iPort,
                strFilePath = setParameter.strFilePath,
                strModelName = setParameter.strModelName,
                strMotorName = setParameter.strMotorName
            };
            m_objMotor.HLSetMotorInitializeParameter(objMotorInitializeParameter);
        }

        public override CMotorStatus HLGetMotorStatus()
        {
            var objMotorStatus = m_objMotor.HLGetMotorStatus();
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

        public override bool HLWaitMotorStatus(object objMotor, EPositionCompare eCompareResult, int iTimeOut, int iSleepPeriod = 10)
        {
            bool bResult = false;
            if (EPositionCompare.POSITION_COMPARE_EQUAL == eCompareResult)
                bResult = true;
            else if (EPositionCompare.POSITION_COMPARE_UNEQUAL == eCompareResult)
                bResult = false;
            else
                bResult = false;
            var motor = objMotor as CDeviceMotorIna;
            if (motor == null)
            {
                return false;
            }
            return m_objMotor.HLWaitMotorPositionStatus(motor.m_objMotor, bResult, iTimeOut, iSleepPeriod);
        }

        public override bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, int iPositionIndex, int iTimeOut, int iSleepPeriod = 10)
        {
            bool bResult = false;
            if (EPositionCompare.POSITION_COMPARE_EQUAL == eCompareResult)
                bResult = true;
            else if (EPositionCompare.POSITION_COMPARE_UNEQUAL == eCompareResult)
                bResult = false;
            else
                bResult = false;
            var motor = objMotor as CDeviceMotorIna;
            if (motor == null)
            {
                return false;
            }
            return m_objMotor.HLWaitMotorPositionStatus(motor.m_objMotor, bResult, iPositionIndex, iTimeOut, iSleepPeriod);
        }

        public override bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, double dStartPosition, double dPosition, int iTimeOut, int iSleepPeriod = 10)
        {
            bool bResult = false;
            if (EPositionCompare.POSITION_COMPARE_EQUAL == eCompareResult)
                bResult = true;
            else if (EPositionCompare.POSITION_COMPARE_UNEQUAL == eCompareResult)
                bResult = false;
            else
                bResult = false;
            var motor = objMotor as CDeviceMotorIna;
            if (motor == null)
            {
                return false;
            }
            return m_objMotor.HLWaitMotorPositionStatus(motor.m_objMotor, bResult, dStartPosition, dPosition, iTimeOut, iSleepPeriod);
        }

        public override bool HLWaitMotorPositionStatus(object objMotor, EPositionCompare eCompareResult, double dPosition, int iTimeOut, int iSleepPeriod = 10)
        {
            bool bResult = false;
            if (EPositionCompare.POSITION_COMPARE_EQUAL == eCompareResult)
                bResult = true;
            else if (EPositionCompare.POSITION_COMPARE_UNEQUAL == eCompareResult)
                bResult = false;
            else
                bResult = false;
            var motor = objMotor as CDeviceMotorIna;
            if (motor == null)
            {
                return false;
            }
            return m_objMotor.HLWaitMotorPositionStatus(motor.m_objMotor, bResult, dPosition, iTimeOut, iSleepPeriod);
        }

        public override bool HLLoadMotorOperationParameter(string strFilePath, string strModelName)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLLoadMotorOperationParameter(strFilePath, strModelName))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLSaveMotorOperationParameter(string strFilePath, string strModelName, CMotorOperationParameter objMotorOperationParameter)
        {
            bool bReturn = false;
            do
            {
                var objMotorOperationParameterDll = m_objMotor.HLGetMotorOperationParameter();
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

                if (EMotionDirection.DIR_CCW == objMotorOperationParameter.eHomeDirection)
                {
                    objMotorOperationParameterDll.eHomeDirection = CDeviceMotorInaDefine.EMoveDirection.CCW;
                }
                else
                {
                    objMotorOperationParameterDll.eHomeDirection = CDeviceMotorInaDefine.EMoveDirection.CW;
                }

                if (EHomeDetect.HOME_SENSOR == objMotorOperationParameter.eHomeDetect)
                {
                    objMotorOperationParameterDll.eMotionHomeDetect = CDeviceMotorInaDefine.EHomeMethod.ORIGIN;
                }

                if (EMotionLevel.LEVEL_LOW == objMotorOperationParameter.eMotionLevel)
                {
                    objMotorOperationParameterDll.eMotionLevel = CDeviceMotorInaDefine.ELevel.None;
                }

                objMotorOperationParameterDll.iHomeClearTime = objMotorOperationParameter.iHomeClearTime;
                objMotorOperationParameterDll.iHomePriority = objMotorOperationParameter.iHomePriority;
                objMotorOperationParameterDll.iStandardLimitTimeOut = objMotorOperationParameter.iStandardLimitTimeOut;
                objMotorOperationParameterDll.iUseZPhase = objMotorOperationParameter.iUseZPhase;

                if (false == m_objMotor.HLSaveMotorOperationParameter(strFilePath, strModelName, objMotorOperationParameterDll))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLLoadMotorPositionParameter(string strFilePath, string strModelName)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objMotor.HLLoadMotorPositionParameter(strFilePath, strModelName))
                {
                    m_objMotor.HLGetMotorError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLSaveMotorPositionParameter(string strFilePath, string strModelName, CMotorPosition objMotorPosition)
        {
            bool bReturn = false;
            do
            {
                var objMotorPositionDll = m_objMotor.HLGetMotorPosition();
                for (int iLoopCount = 0; iLoopCount < DEF_MAX_POSITION_COUNT; iLoopCount++)
                {
                    objMotorPositionDll.strPositionName[iLoopCount] = objMotorPosition.strPositionName[iLoopCount];
                    objMotorPositionDll.dPosition[iLoopCount] = objMotorPosition.dPosition[iLoopCount];
                    objMotorPositionDll.dPrevPosition[iLoopCount] = objMotorPosition.dPrevPosition[iLoopCount];
                    objMotorPositionDll.dVelocity[iLoopCount] = objMotorPosition.dVelocity[iLoopCount];
                    objMotorPositionDll.dAcceleration[iLoopCount] = objMotorPosition.dAcceleration[iLoopCount];
                    objMotorPositionDll.dDeceleration[iLoopCount] = objMotorPosition.dDeceleration[iLoopCount];
                }
                if (false == m_objMotor.HLSaveMotorPositionParameter(strFilePath, strModelName, objMotorPositionDll))
                {
                    m_objMotor.HLGetMotorError();
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

        public override bool HLSetStatusRequestReadA5nWarningData() => true;

        public override bool HLGetStatusA5nWarningCode(ref uint warningCode) => true;

        public override bool HLClose() => true;

        public override bool HLSetHomeVelocity(double dVelocity) => true;

        public override bool HLGetHomeVelocity(ref double dVelocity) => true;
    }
}

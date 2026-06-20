using EqToEq;
using SVI_NFT_R.DEVICE.Nachi;
using SVI_NFT_R.DEVICE.Nachi.SignalNames;
using SVI_NFT_R.EHS;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SVI_NFT_R
{
    public abstract class CProcessInterlockAbstract
    {
        /// <summary>
        /// 알람 구조체
        /// </summary>
        protected CDefine.structureAlarmInformation m_objAlarmStructure;
        protected HLDevice.CDeviceMotor m_objMotor;
        protected Thread m_objThreadJog;
        protected bool m_bThreadExit;
        public CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CProcessInterlockAbstract()
        {
            m_objDocument = null; m_objMotor = null; m_objThreadJog = null;

            m_bThreadExit = false;

            m_objAlarmStructure = new CDefine.structureAlarmInformation();
            m_objAlarmStructure.strAlarmObject = typeof(CProcessInterlockAbstract).Name;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.strAlarmDescription = "";
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public abstract bool Initialize();

        /// <summary>
        /// 해제
        /// </summary>
        public abstract void DeInitialize();

        /// <summary>
        /// 모션 Class 인터락 ( 상대 위치 이동 )
        /// </summary>
        /// <param name="strMotorName"></param>
        /// <param name="dRelativePosition"></param>
        /// <returns></returns>
        public virtual bool CheckMotionClassInterlock(string strMotorName, double dRelativePosition)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모션 Class 인터락 ( Teaching Position 기준 )
        /// </summary>
        /// <param name="strMotorName"></param>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        public virtual bool CheckMotionClassInterlock(string strMotorName, int iPosition)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 인터락 ( Jog 중속 이상 및 움직일 Motor에 대한 모든 Interlock를 체크 )
        /// </summary>
        /// <param name="strMotorName"></param>
        /// <param name="bPositiveDirection"></param>
        /// <param name="callbackInterlockPreActionOrNull">UI Timer에서 인터락을 체크 할 때 메시지 창이 중복해서 뜨는 현상이 있으서 메시지를 띄우기 전에 플래그를 미리 바꾸기 위해 사용됨</param>
        /// <returns></returns>
        public virtual bool CheckForcedInterlock(string strMotorName, bool bPositiveDirection, Action callbackInterlockPreActionOrNull)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 실린더 인터락
        /// </summary>
        /// <param name="eCylinder"></param>
        /// <param name="eCommand"></param>
        /// <returns></returns>
        public virtual bool CheckCylinderInterlock(CProcessMotion.ECylinder eCylinder, CCylinder.ECylinderCommand eCommand)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 진공 인터락
        /// </summary>
        /// <param name="eVacuum"></param>
        /// <param name="eCommand"></param>
        /// <returns></returns>
        public virtual bool CheckVacuumInterlock(CProcessMotion.EVacuum eVacuum, CVacuum.EVacuumCommand eCommand)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모터 포지션별 설정 범위 인터락 (옵션)
        /// </summary>
        /// <param name="motorName">모터 이름</param>
        /// <param name="positionIndex">포지션 인덱스</param>
        /// <param name="setValue">포지션 설정값</param>
        /// <returns>true=정상, false=인터락</returns>
        public virtual bool CheckMotorSettingPositionInterlock(string motorName, int positionIndex, double setValue)
        {
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            /* 모든 인터락 함수에서 필수로 구현해야 할 항목은 아니다
             * 특수하게 소스 코드에서 포지션 입력값 범위를 관리해야되는 경우에만 해당 매니져의 인터락 클래스에 정의해서 사용한다
             */

            // 구현 예제
            /// <![CDATA[
            /// public override bool CheckMotorSettingPositionInterlock(string motorName, int positionIndex, double setValue)
            /// {
            ///     // 모터 이름 정합성 검사
            ///     CProcessMotion.EMotor motorIndex;
            ///     if (Enum.TryParse(motorName, out motorIndex) == false)
            ///     {
            ///         return false;
            ///     }
            ///     HLDevice.CDeviceMotor motor = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[motorIndex];
            /// 
            ///     // 모터별 셋팅 인터락 처리
            ///     switch (motorIndex)
            ///     {
            ///         case CProcessMotion.EMotor.OUT_TRANSFER_X:
            ///             {
            ///                 OutTransferMotorX.EPosition pos = (OutTransferMotorX.EPosition)positionIndex;
            ///                 switch (pos)
            ///                 {
            ///                     case OutTransferMotorX.EPosition.AvoidPosition:
            ///                         return InternalCheckMotorSettingPositionInterlock(motorName, motor.HLGetMotorPosition().strPositionName[positionIndex], setValue, 0, 50);
            /// 
            ///                     case OutTransferMotorX.EPosition.BypassAvoidPosition:
            ///                         return InternalCheckMotorSettingPositionInterlock(motorName, motor.HLGetMotorPosition().strPositionName[positionIndex], setValue, 50, 100);
            ///                 }
            ///             }
            ///             break;
            ///     }
            /// 
            ///     return true;
            /// }
            /// ]]>
            return true;
        }

        /// <summary>
        /// 포지션 설정값 범위를 체크하고 범위 초과시 메시지 처리하는 함수
        /// </summary>
        /// <param name="motorName">모터 이름(메시지 처리에 사용됨)</param>
        /// <param name="positionName">포지션 이름(메시지 처리에 사용됨)</param>
        /// <param name="setValue">포지션 설정값</param>
        /// <param name="min">최소값</param>
        /// <param name="max">최대값</param>
        /// <returns>true=정상, false=인터락</returns>
        protected bool InternalCheckMotorSettingPositionInterlock(string motorName, string positionName, double setValue, double min, double max)
        {
            double tempSetValue = setValue;
            bool bResult = Utils.InRange(ref setValue, min, max);
            if (bResult == false)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.MOTOR_SETTING_POSITION_OVER_RANGE_PARAM5,
                    motorName,
                    positionName,
                    min.ToString(),
                    max.ToString(),
                    tempSetValue.ToString()
                    );
                return false;
            }
            return true;
        }

        protected bool InternalCheckUpperEqInterfaceDoorOpen(IProcessManagerLoadInterface loadInterface)
        {
            if (loadInterface.CheckUpperEqInterfaceDoorOpened() == true)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_EQUIPMENT_DOOROPEN_SIGNAL_PARAM1, "UPPER");
                return false;
            }
            return true;
        }

        protected bool InternalCheckUpperEqInterfaceInterlock(IProcessManagerLoadInterface loadInterface)
        {
            if (loadInterface.CheckUpperEqInterfaceInterlocked() == true)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_EQUIPMENT_INTERLOCK_SIGNAL_PARAM1, "UPPER");
                return false;
            }
            return true;
        }

        protected bool InternalCheckUpperEqInterfaceEmergency(IProcessManagerLoadInterface loadInterface)
        {
            if (loadInterface.CheckUpperEqInterfaceOnEMS() == true)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_EQUIPMENT_EMERGENCY_SIGNAL_PARAM1, "UPPER");
                return false;
            }
            return true;
        }

        protected bool InternalCheckMcOn(string motorName, Action callbackInterlockPreActionOrNull = null)
        {
            // MC 확인
            if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON) == false)
            {
                callbackInterlockPreActionOrNull?.Invoke();
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.CAN_NOT_OPERATE_THE_UNIT_BECAUSE_THE_GPS_MC_IS_OFF,
                    motorName);
                return false;
            }
            return true;
        }

        protected bool InternalCheckEnableSwitch(Action callbackInterlockPreActionOrNull = null)
        {
            if (m_objDocument.IsMasterLogin == true)
            {
                return true;
            }

            if (m_objDocument.m_objProcessMain.GetSafetyIO().IsAnyEnableSwitchGripped() == true)
            {
                return true;
            }

            if (m_objDocument.m_objProcessMain.GetSafetyIO().IsSafetyAllDoorClosed() == true)
            {
                return true;
            }

            bool bDoorClose = DoorManager.Doors.Values.All(item => item.IsOpened == false);
            if (bDoorClose == true)
            {
                return true;
            }

            callbackInterlockPreActionOrNull?.Invoke();
            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ENABLE_SWITCH_IS_NOT_DETECT);
            return false;
        }

        protected bool InternalCheckRobotEMS(Action callbackInterlockPreActionOrNull = null)
        {
            if (m_objDocument.IsMasterLogin == true)
            {
                return true;
            }

            if (m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot.Values.All(i => i.Status.IsPendantEmsOn == false && i.Status.IsControllerEmsOn == false) == true)
            {
                return true;
            }

            callbackInterlockPreActionOrNull?.Invoke();
            foreach (var item in m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot)
            {
                if (item.Value.Status.IsPendantEmsOn == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_PENDANT_EMS_BUTTON_IS_PRESSED_CHECK_EMS_BUTTON, item.Key);
                }
                if (item.Value.Status.IsControllerEmsOn == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_CONTROLLER_EMS_BUTTON_IS_PRESSED_CHECK_EMS_BUTTON, item.Key);
                }
            }
            return false;
        }

        protected bool InternalCheckRobotMultipleOperation(Action callbackInterlockPreActionOrNull = null)
        {
            if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
            {
                return true;
            }

            if (m_objDocument.IsMasterLogin == true)
            {
                return true;
            }

            if (m_objDocument.m_objProcessMain.GetSafetyIO().IsSafetyAllDoorClosed() == true)
            {
                return true;
            }

            bool bDoorClose = DoorManager.Doors.Values.All(item => item.IsOpened == false);
            if (bDoorClose == true)
            {
                return true;
            }

            if (m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot.Values.All(i => i.Status.IsTeachModeEntry == false && i.Status.IsTeachModeEntry == false && i.Status.IsRunningU1 == true) == true)
            {
                return true;
            }

            callbackInterlockPreActionOrNull?.Invoke();
            foreach (var item in m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot)
            {
                if (item.Value.Status.IsTeachModeEntry == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_NOT_IN_AUTOMATIC_MODE_PARAM1,
                        item.Key);
                }
                if (item.Value.Status.IsPlayBackModeEntry == false
                    || item.Value.Status.IsRunningU1 == false
                    )
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_NOT_ENTRY_PLAYBACK_MODE_PARAM1,
                        item.Key);
                }
            }
            return false;
        }

        protected bool InternalCheckRobotUsable(CProcessMotion.ERobot robotIndex, bool bIsInitializeCommand)
        {
            // 시뮬레이션 모드인 경우 스킵
            if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
            {
                return true;
            }

            var robot = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot[robotIndex];
            if (robot.Status.IsTeachModeEntry == true)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_NOT_IN_AUTOMATIC_MODE_PARAM1,
                    robotIndex);
                return false;
            }
            if (robot.Status.IsBatteryWarningM1 == true)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_SERVO_BATTERY_WARNING_OCCURRED_STATUS_PARAM1,
                    robotIndex);
                return false;
            }
            if (robot.Status.IsEmergencyStop == true)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_EMERGENCY_STOP_STATUS_PARAM1,
                    robotIndex);
                return false;
            }
            if (robot.Status.ShouldHomingAfterTeaching == true)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.CHECK_ROBOT_TEACHING_AFTER_HOMMING_PARAM1,
                    robotIndex);
                return false;
            }

            // 초기화 커맨드인 경우 스킵
            if (bIsInitializeCommand == true)
            {
                return true;
            }

            if (robot.Status.IsFailure == true)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_ALARM_OCCURRED_STATUS_PARAM1,
                    robotIndex);
                return false;
            }
            if (robot.Status.IsPlayBackModeEntry == false
                || robot.Status.IsRunningU1 == false
                )
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_NOT_ENTRY_PLAYBACK_MODE_PARAM1,
                    robotIndex);
                return false;
            }
            if (robot.Status.IsMotorEnergized == false)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_NOT_SERVO_ON_STATUS_PARAM1,
                    robotIndex);
                return false;
            }
            if (robot.Signals.Y.BB[Bit.EOutput.PAUSE].Value == false)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_PAUSE_STOP_STATUS_PARAM1,
                    robotIndex);
                return false;
            }
            //Process1Error = 10,
            //Process1Tool1StartError = 11,
            //Process1Tool2StartError = 12,
            //Process1Tool3StartError = 13,
            //Process2Error = 20,
            //...
            EErrorCode currentErrorCode = robot.Status.CurrentErrorCode;
            string errorCode = currentErrorCode.ToString();
            if (errorCode.StartsWith("Process") == true
                && errorCode.EndsWith("Error") == true
                )
            {
                int processNumber = (int)currentErrorCode / 10;
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_IS_INTERFACE_ALARM_STATUS_PARAM2,
                    robotIndex,
                    $"P{processNumber}");
                return false;
            }
            return true;
        }

        protected bool InternalCheckMotorInitialized(CProcessMotion.EMotor motorIndex, object command)
        {
            // 초기화 동작인 경우 스킵
            if (Convert.ToInt32(command) == CDefine.ORIGIN_POSITION_NO
                || Convert.ToInt32(command) == CDefine.MANUAL_ORIGIN_POSITION_NO
                )
            {
                return true;
            }

            var motor = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[motorIndex];
            var motorStatus = motor.HLGetMotorStatus();
            var motorOperationParameter = motor.HLGetMotorOperationParameter();
            // 원점 완료 상태 체크
            if (motorOperationParameter.bUseHome == true
                && motorStatus.bHome == false
                )
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_NOT_INITIALIZED_PARAM1,
                    motorIndex);
                return false;
            }
            return true;
        }

        protected bool InternalCheckMotorUsable(CProcessMotion.EMotor motorIndex, object command, Action callbackInterlockPreActionOrNull = null)
        {
            // 초기화 동작인 경우 스킵
            if (Convert.ToInt32(command) == CDefine.ORIGIN_POSITION_NO)
            {
                return true;
            }

            var motor = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[motorIndex];
            var motorStatus = motor.HLGetMotorStatus();
            var motorOperationParameter = motor.HLGetMotorOperationParameter();
            // 모터 알람 상태 체크
            if (motorStatus.bAlarm == true)
            {
                callbackInterlockPreActionOrNull?.Invoke();
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_SERVO_ALARM_PARAM1,
                    motorIndex);
                return false;
            }
            // 모터 서보온 상태 체크
            if (motorStatus.bServo == false)
            {
                callbackInterlockPreActionOrNull?.Invoke();
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_SERVO_OFF_PARAM1,
                    motorIndex);
                return false;
            }
            return true;
        }

        protected bool InternalShouldCheckMotorCommandInterlock(object command)
        {
            switch (Convert.ToInt32(command))
            {
                case CDefine.ORIGIN_POSITION_NO:
                case CDefine.WAIT_INTERLOCK_POSITION_NO:
                case CDefine.MANUAL_ORIGIN_POSITION_NO:
                    return false;

                default:
                    return true;
            }
        }

        protected bool InternalCheckMotorJogAble(CProcessMotion.EMotor motorIndex, bool bPositiveDirection, Action callbackInterlockPreActionOrNull = null)
        {
            var motor = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[motorIndex];

            if (bPositiveDirection == true)
            {
                double swLimitMax = new double[] { motor.HLGetMotorOperationParameter().dLimitSoftwareMinus, motor.HLGetMotorOperationParameter().dLimitSoftwarePlus }.Max();
                if (motor.HLGetMotorStatus().dEncoderPosition >= swLimitMax)
                {
                    callbackInterlockPreActionOrNull?.Invoke();
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.CAN_NOT_DO_THE_ACTION_PARAM2,
                        Resource.Get("JOG"),
                        Resource.Get("SW LIMIT")
                        );
                    return false;
                }
            }
            else
            {
                double swLimitMin = new double[] { motor.HLGetMotorOperationParameter().dLimitSoftwareMinus, motor.HLGetMotorOperationParameter().dLimitSoftwarePlus }.Min();
                if (motor.HLGetMotorStatus().dEncoderPosition <= swLimitMin)
                {
                    callbackInterlockPreActionOrNull?.Invoke();
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.CAN_NOT_DO_THE_ACTION_PARAM2,
                        Resource.Get("JOG"),
                        Resource.Get("SW LIMIT")
                        );
                    return false;
                }
            }

            return true;
        }
    }
}
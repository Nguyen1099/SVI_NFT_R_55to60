using SVI_NFT_R.CellData;
using System;
using System.Diagnostics;
using System.Reflection;

namespace SVI_NFT_R
{
    public class OutFlipInterlock : CProcessInterlockAbstract
    {
        /// <summary>
        /// 생성자 함수
        /// </summary>
        /// <param name="objDocument"></param>
        public OutFlipInterlock(CDocument objDocument)
        {
            m_objDocument = objDocument;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public override bool Initialize()
        {
            bool bReturn = false;

            do
            {
                if (null == m_objDocument)
                    break;

                m_objAlarmStructure.strAlarmObject = typeof(OutFlipInterlock).Name;
                m_objAlarmStructure.eAlarmLevel = CDefine.EAlarmType.ALARM_INTERLOCK;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void DeInitialize()
        {
        }

        /// <summary>
        /// 모션 Class 인터락 ( 목표 위치 이동 )
        /// </summary>
        /// <param name="strMotorName">이동 할 모터 이름</param>
        /// <param name="dTargetPosition"></param>
        /// <returns>인터락 여부 (false = 인터락)</returns>
        public override bool CheckMotionClassInterlock(string strMotorName, double dTargetPosition)
        {
            bool bSafetyAutoMode = m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto();
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            // MC ON 확인
            if (InternalCheckMcOn(strMotorName, null) == false)
            {
                return false;
            }

            CProcessMotion.EMotor motorIndex;
            // 모터 초기화 상태 확인
            if (Enum.TryParse(strMotorName, out motorIndex) == true
                && InternalCheckMotorInitialized(motorIndex, dTargetPosition) == false
                )
            {
                return false;
            }
            // 모터 사용가능 상태 확인
            if (Enum.TryParse(strMotorName, out motorIndex) == true
                && InternalCheckMotorUsable(motorIndex, dTargetPosition, null) == false
                )
            {
                return false;
            }

            // 인터페이스 인터락
            if (InternalCheckUpperEqInterfaceDoorOpen(m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface) == false
                || InternalCheckUpperEqInterfaceEmergency(m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface) == false
                )
            {
                return false;
            }

            var outFlip = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip;
            bool bIsZSafetyPosition = outFlip.MotorZ.IsInposition(OutFlipMotorZ.ECommand.UpPosition);
            switch (strMotorName)
            {
                case nameof(CProcessMotion.EMotor.OUT_FLIP_R1):
                case nameof(CProcessMotion.EMotor.OUT_FLIP_R2):
                    {
                        if (bIsZSafetyPosition == false)
                        {
                            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                EProcess.OutFlip,
                                strMotorName);
                            return false;
                        }
                        // OutRobot Interlock
                        {
                            var outRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot;
                            // [Out Robot] Robot 사용불가
                            if (InternalCheckRobotUsable(outRobot.Nachi.RobotIndex, false) == false)
                            {
                                return false;
                            }
                            // [Out Robot] Robot 인터락 상태
                            if (outRobot.Nachi.IsUnloadInterlock() == true)
                            {
                                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                    motorIndex,
                                    outRobot.Nachi.RobotIndex);
                                return false;
                            }
                        }
                    }
                    break;

                case nameof(CProcessMotion.EMotor.OUT_FLIP_Z):
                    {
                        // OutRobot Interlock
                        {
                            var outRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot;
                            // [Out Robot] Robot 사용불가
                            if (InternalCheckRobotUsable(outRobot.Nachi.RobotIndex, false) == false)
                            {
                                return false;
                            }
                            // [Out Robot] Robot 인터락 상태
                            if (outRobot.Nachi.IsUnloadInterlock() == true)
                            {
                                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                    motorIndex,
                                    outRobot.Nachi.RobotIndex);
                                return false;
                            }
                        }
                    }
                    break;

                case nameof(CProcessMotion.EMotor.OUT_SHUTTLE_X2):
                    {
                        if (bIsZSafetyPosition == false)
                        {
                            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                EProcess.OutFlip,
                                strMotorName);
                            return false;
                        }
                    }
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            // ↑↑↑ 초기화에 영향이 있는 인터락 항목들 ↑↑↑
            // 커맨드 인터락 사용 여부
            if (InternalShouldCheckMotorCommandInterlock(dTargetPosition) == false)
            {
                return true;
            }
            // ↓↓↓ 초기화에 영향이 없는 인터락 항목들 ↓↓↓

            return true;
        }

        /// <summary>
        /// 메뉴얼 포지션 이동 인터락 확인
        /// </summary>
        /// <param name="strMotorName">이동 할 모터 이름</param>
        /// <param name="iPosition">이동 할 포지션 번호</param>
        /// <returns>인터락 여부 (false = 인터락)</returns>
        public override bool CheckMotionClassInterlock(string strMotorName, int iCommand)
        {
            bool bSafetyAutoMode = m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto();
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            // MC ON 확인
            if (InternalCheckMcOn(strMotorName, null) == false)
            {
                return false;
            }

            CProcessMotion.EMotor motorIndex;
            // 모터 초기화 상태 확인
            if (Enum.TryParse(strMotorName, out motorIndex) == true
                && InternalCheckMotorInitialized(motorIndex, iCommand) == false
                )
            {
                return false;
            }
            // 모터 사용가능 상태 확인
            if (Enum.TryParse(strMotorName, out motorIndex) == true
                && InternalCheckMotorUsable(motorIndex, iCommand, null) == false
                )
            {
                return false;
            }

            // 인터페이스 인터락
            if (InternalCheckUpperEqInterfaceDoorOpen(m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface) == false
                || InternalCheckUpperEqInterfaceEmergency(m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface) == false
                )
            {
                return false;
            }

            var outFlip = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip;
            bool bIsZSafetyPosition = outFlip.MotorZ.IsInposition(OutFlipMotorZ.ECommand.UpPosition);
            switch (strMotorName)
            {
                case nameof(CProcessMotion.EMotor.OUT_FLIP_R1):
                case nameof(CProcessMotion.EMotor.OUT_FLIP_R2):
                    {
                        if (bIsZSafetyPosition == false)
                        {
                            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                EProcess.OutFlip,
                                strMotorName);
                            return false;
                        }

                        // OutRobot Interlock
                        {
                            var outRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot;
                            // [Out Robot] Robot 사용불가
                            if (InternalCheckRobotUsable(outRobot.Nachi.RobotIndex, false) == false)
                            {
                                return false;
                            }
                            // [Out Robot] Robot 인터락 상태
                            if (outRobot.Nachi.IsUnloadInterlock() == true)
                            {
                                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                    motorIndex,
                                    outRobot.Nachi.RobotIndex);
                                return false;
                            }
                        }
                    }
                    break;

                case nameof(CProcessMotion.EMotor.OUT_FLIP_Z):
                    {
                        // OutRobot Interlock
                        {
                            var outRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot;
                            // [Out Robot] Robot 사용불가
                            if (InternalCheckRobotUsable(outRobot.Nachi.RobotIndex, false) == false)
                            {
                                return false;
                            }
                            // [Out Robot] Robot 인터락 상태
                            if (outRobot.Nachi.IsUnloadInterlock() == true)
                            {
                                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                    motorIndex,
                                    outRobot.Nachi.RobotIndex);
                                return false;
                            }
                        }
                    }
                    break;

                case nameof(CProcessMotion.EMotor.OUT_SHUTTLE_X2):
                    {
                        if (bIsZSafetyPosition == false)
                        {
                            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                EProcess.OutFlip,
                                strMotorName);
                            return false;
                        }
                    }
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            // ↑↑↑ 초기화에 영향이 있는 인터락 항목들 ↑↑↑
            // 커맨드 인터락 사용 여부
            if (InternalShouldCheckMotorCommandInterlock(iCommand) == false)
            {
                return true;
            }
            // ↓↓↓ 초기화에 영향이 없는 인터락 항목들 ↓↓↓

            return true;
        }

        /// <summary>
        /// 모터 인터락 ( Jog 중속 이상 및 움직일 Motor에 대한 모든 Interlock를 체크 )
        /// </summary>
        /// <param name="strMotorName"></param>
        /// <returns></returns>
        public override bool CheckForcedInterlock(string strMotorName, bool bPositveDirection, Action callbackInterlockPreActionOrNull)
        {
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            // 일반 모터와 컨베이어 모터의 인터락 구분
            switch (strMotorName)
            {
                case nameof(CProcessMotion.EMotor.OUT_FLIP_R1):
                case nameof(CProcessMotion.EMotor.OUT_FLIP_R2):
                case nameof(CProcessMotion.EMotor.OUT_FLIP_Z):
                    // TEACH 모드 확인
                    if (m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto() == true
                        && m_objDocument.IsMasterLogin == false
                        )
                    {
                        callbackInterlockPreActionOrNull?.Invoke();
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.SAFETY_KEY_MODE_IS_NOT_TEACH);
                        return false;
                    }
                    // Enable Switch 없음
                    // Enable Switch 상태 확인
                    //if (InternalCheckEnableSwitch(callbackInterlockPreActionOrNull) == false)
                    //{
                    //    return false;
                    //}
                    // Robot EMS 상태 확인
                    if (InternalCheckRobotEMS(callbackInterlockPreActionOrNull) == false)
                    {
                        return false;
                    }
                    // Robot 동시작업 확인
                    if (InternalCheckRobotMultipleOperation(callbackInterlockPreActionOrNull) == false)
                    {
                        return false;
                    }
                    break;

                case nameof(CProcessMotion.EMotor.OUT_SHUTTLE_X2):
                default:
                    break;
            }
            // MC ON 확인
            if (InternalCheckMcOn(strMotorName, callbackInterlockPreActionOrNull) == false)
            {
                return false;
            }

            CProcessMotion.EMotor motorIndex;
            // 모터 사용가능 상태 확인
            if (Enum.TryParse(strMotorName, out motorIndex) == true
                && InternalCheckMotorUsable(motorIndex, 0, callbackInterlockPreActionOrNull) == false
                )
            {
                return false;
            }

            // JOG 가능 상태 확인
            if (Enum.TryParse(strMotorName, out motorIndex) == true
                && InternalCheckMotorJogAble(motorIndex, bPositveDirection, callbackInterlockPreActionOrNull) == false
                )
            {
                return false;
            }

            return true;
        }
    }
}

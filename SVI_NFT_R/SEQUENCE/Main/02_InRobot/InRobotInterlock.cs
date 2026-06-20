using System.Reflection;

namespace SVI_NFT_R
{
    public class InRobotInterlock : CProcessInterlockAbstract
    {
        /// <summary>
        /// 생성자 함수
        /// </summary>
        /// <param name="objDocument"></param>
        public InRobotInterlock(CDocument objDocument)
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

                m_objAlarmStructure.strAlarmObject = typeof(InRobotInterlock).Name;
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
        /// 모션 Class 인터락
        /// </summary>
        /// <param name="strMotorName"></param>
        /// <param name="iCommand"></param>
        /// <returns></returns>
        public override bool CheckMotionClassInterlock(string strMotorName, int iCommand)
        {
            bool bSafetyAutoMode = m_objDocument.m_objProcessMain.CheckSaftyKeyModeIsAuto();
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            // 로봇 사용가능 상태 확인
            if (InternalCheckRobotUsable(CProcessMotion.ERobot.IN_ROBOT, bIsInitializeCommand: (int)InRobotNachi.ECommand.Initialize == iCommand) == false)
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

            // 100 이하 이면 커맨드 동작
            {
                // 로드 위치로 이동 할 경우
                if ((int)InRobotNachi.ECommand.LoadPermit == iCommand)
                {
                    var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
                    if (inShuttle.MotorStageX.IsInposition(InShuttleMotorX.ECommand.UnloadPosition) == false)
                    {
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_NOT_TARGET_POSITION_PARAM2,
                            inShuttle.MotorStageX.MotorIndex,
                            InShuttleMotorX.ECommand.UnloadPosition);
                        return false;
                    }
                }

                // MCR 위치로 이동 할 경우
                if ((int)InRobotNachi.ECommand.McrPermit == iCommand)
                {
                }

                // 언로드 위치로 이동 할 경우
                if ((int)InRobotNachi.ECommand.UnloadPermit == iCommand)
                {
                    var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
                    if (inspStage.MotorStageY.IsInposition(InspStageMotorY.ECommand.LoadPosition) == false)
                    {
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_NOT_TARGET_POSITION_PARAM2,
                            inspStage.MotorStageY.MotorIndex,
                            InspStageMotorY.ECommand.LoadPosition);
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
                        if (outRobot.Nachi.IsLoadInterlock() == true)
                        {
                            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2,
                                strMotorName,
                                outRobot.Nachi.RobotIndex);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public override bool CheckCylinderInterlock(CProcessMotion.ECylinder eCylinder, CCylinderAbstract.ECylinderCommand eCommand)
        {
            return true;
        }
    }
}
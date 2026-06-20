using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InspStageMotorY
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CProcessMotion.EMotor MotorIndex { get; private set; }
            public CDefine.EInspectType InspectType { get; private set; }
            public bool IsUsingTrigger { get; private set; }
            public CAlarmDefine.EAlarmList AlarmHomeMotionFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmLoadPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmGrabP1PositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmGrabP2PositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmUnloadPositionMoveFail { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_03_INSP_STAGE;
                        MotorIndex = CProcessMotion.EMotor.INSP_STAGE_Y;
                        InspectType = CDefine.EInspectType.Main;
                        IsUsingTrigger = false;
                        AlarmHomeMotionFail = CAlarmDefine.EAlarmList.MO_INSP_STAGE_Y_HOME_MOTION;
                        AlarmLoadPositionMoveFail = CAlarmDefine.EAlarmList.MO_INSP_STAGE_Y_LOAD_MOVE;
                        AlarmGrabP1PositionMoveFail = CAlarmDefine.EAlarmList.MO_INSP_STAGE_Y_GRAB_P1_MOVE;
                        AlarmGrabP2PositionMoveFail = CAlarmDefine.EAlarmList.MO_INSP_STAGE_Y_GRAB_P2_MOVE;
                        AlarmUnloadPositionMoveFail = CAlarmDefine.EAlarmList.MO_INSP_STAGE_Y_UNLOAD_MOVE;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        }

        private Define mDefine;
    }
}
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutFlipMotorX
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CProcessMotion.EMotor MotorIndex { get; private set; }
            public CAlarmDefine.EAlarmList AlarmHomeMotionFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmPositionResetFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmUnloadPositionMoveFail { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_05_OUT_FLIP;
                        MotorIndex = CProcessMotion.EMotor.OUT_CONVEYOR_X2;
                        AlarmHomeMotionFail = CAlarmDefine.EAlarmList.MO_UNLOADER_CONVEYOR_X2_HOME_MOTION;
                        AlarmPositionResetFail = CAlarmDefine.EAlarmList.MO_UNLOADER_CONVEYOR_X2_POSITION_RESET;
                        AlarmUnloadPositionMoveFail = CAlarmDefine.EAlarmList.MO_UNLOADER_CONVEYOR_X2_UNLOAD_MOVE;
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
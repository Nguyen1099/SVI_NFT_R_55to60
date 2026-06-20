using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutFlipMotorZ
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CProcessMotion.EMotor MotorIndex { get; private set; }
            public CAlarmDefine.EAlarmList AlarmHomeMotionFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmUpPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmDownPositionMoveFail { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_05_OUT_FLIP;
                        MotorIndex = CProcessMotion.EMotor.OUT_FLIP_Z;
                        AlarmHomeMotionFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_Z_HOME_MOTION;
                        AlarmUpPositionMoveFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_Z_UP_MOVE;
                        AlarmDownPositionMoveFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_Z_DOWN_MOVE;
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
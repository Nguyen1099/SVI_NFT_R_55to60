using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutShuttleMotorX
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CProcessMotion.EMotor MotorIndex { get; private set; }
            public CAlarmDefine.EAlarmList AlarmHomeMotionFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmLoadPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAlignP1PositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAlignP2PositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmUnloadPositionMoveFail { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_06_OUT_SHUTTLE;
                        MotorIndex = CProcessMotion.EMotor.OUT_SHUTTLE_X2;
                        AlarmHomeMotionFail = CAlarmDefine.EAlarmList.MO_OUT_SHUTTLE_X1_HOME_MOTION;
                        AlarmLoadPositionMoveFail = CAlarmDefine.EAlarmList.MO_OUT_SHUTTLE_X1_LOAD_MOVE;
                        AlarmUnloadPositionMoveFail = CAlarmDefine.EAlarmList.MO_OUT_SHUTTLE_X1_UNLOAD_MOVE;
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
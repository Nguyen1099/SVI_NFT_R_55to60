using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InRobotSendAlign
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAlignDataSendFail { get; internal set; }
            public CAlarmDefine.EAlarmList AlarmAlignZeroDataSendFail { get; internal set; }
            public CAlarmDefine.EAlarmList AlarmCalibrationDataSendFail { get; internal set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_02_IN_ROBOT;
                        AlarmAlignDataSendFail = CAlarmDefine.EAlarmList.CO_TRANSFER_ROBOT_IN_ALIGN_DATA;
                        AlarmAlignZeroDataSendFail = CAlarmDefine.EAlarmList.CO_TRANSFER_ROBOT_IN_RESET_DATA;
                        AlarmCalibrationDataSendFail = CAlarmDefine.EAlarmList.CO_TRANSFER_ROBOT_IN_CALIBRATION_DATA;
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
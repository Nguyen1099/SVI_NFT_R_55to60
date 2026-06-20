using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InShuttleInspect
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CDefine.EInspInterface InspIndex { get; private set; }
            public CDefine.EInspectType InspectType { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAmoGrabStartSendFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAmoGrabStartValidationFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAmoGrabEndSendFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAmoGrabEndValidationFail { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_01_IN_SHUTTLE;
                        InspIndex = CDefine.EInspInterface.PC1;
                        InspectType = CDefine.EInspectType.Main;
                        AlarmAmoGrabStartSendFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_AMO_GRAB_START_SEND;
                        AlarmAmoGrabStartValidationFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_AMO_GRAB_START_VALIDATION;
                        AlarmAmoGrabEndSendFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_AMO_GRAB_END_SEND;
                        AlarmAmoGrabEndValidationFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_AMO_GRAB_END_VALIDATION;
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
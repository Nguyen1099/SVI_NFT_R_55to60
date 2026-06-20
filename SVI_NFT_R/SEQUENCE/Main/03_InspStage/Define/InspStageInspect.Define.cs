using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InspStageInspect
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CDefine.EInspInterface InspIndex { get; private set; }
            public CDefine.EInspectType InspectType { get; private set; }
            public CAlarmDefine.EAlarmList AlarmDisconnected { get; private set; }
            public CAlarmDefine.EAlarmList AlarmLightIntencitySendFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmLightTemperatureSendFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCheckSendFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmNotReady { get; private set; }
            public CAlarmDefine.EAlarmList AlarmGrabStartSendFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmGrabStartValidationFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmGrabEndTimeout { get; private set; }
            public CAlarmDefine.EAlarmList AlarmGrabEndValidationFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmTotalResultTimeout { get; private set; }
            public CAlarmDefine.EAlarmList AlarmTotalResultSuccessivelyTimeout { get; private set; }
            public CAlarmDefine.EAlarmList AlarmJavasError { get; private set; }
            public CAlarmDefine.EAlarmList AlarmJavasSamePositionDefect { get; private set; }
            public CAlarmDefine.EAlarmList AlarmJavasCameraCheck { get; private set; }
            public CAlarmDefine.EAlarmList AlarmJavasLightControllerCheck { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCellDataSyncSendFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCellDataSyncValidationFail { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_03_INSP_STAGE;
                        InspIndex = CDefine.EInspInterface.PC1;
                        InspectType = CDefine.EInspectType.Main;
                        AlarmDisconnected = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_N_DISCONNECTED;
                        AlarmLightIntencitySendFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_LIGHT_INTENCITY_REQUEST;
                        AlarmLightTemperatureSendFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_LIGHT_TEMPERATURE_REQUEST;
                        AlarmCheckSendFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_CHECK_STATUS_SEND;
                        AlarmNotReady = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_NOT_READY;
                        AlarmGrabStartSendFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_GRAB_START_SEND;
                        AlarmGrabStartValidationFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_GRAB_START_VALIDATION;
                        AlarmGrabEndTimeout = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_GRAB_END_TIMEOUT;
                        AlarmGrabEndValidationFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_GRAB_END_VALIDATION;
                        AlarmTotalResultTimeout = CAlarmDefine.EAlarmList.IN_INSP_PC_MAIN_TOTAL_RESULT_TIMEOUT;
                        AlarmTotalResultSuccessivelyTimeout = CAlarmDefine.EAlarmList.IN_INSP_PC_MAIN_TOTAL_RESULT_NORESPONSE;
                        AlarmJavasError = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_ERROR;
                        AlarmJavasSamePositionDefect = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_ERROR_SAME_POSITION_DEFECT;
                        AlarmJavasCameraCheck = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_ERROR_CAMERA;
                        AlarmJavasLightControllerCheck = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_ERROR_LIGHT_CONTROLLER;
                        AlarmCellDataSyncSendFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_CELL_DATA_SYNC_SEND;
                        AlarmCellDataSyncValidationFail = CAlarmDefine.EAlarmList.CO_INSP_PC_MAIN_CELL_DATA_SYNC_VALIDATION;
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
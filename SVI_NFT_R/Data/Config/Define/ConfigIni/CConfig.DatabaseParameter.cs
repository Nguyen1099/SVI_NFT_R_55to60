using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        [Serializable]
        public class CDatabaseParameter
        {
            /// <summary>
            /// Database History
            /// </summary>
            public string strDatabaseHistory;
            /// <summary>
            /// Database Information
            /// </summary>
            public string strDatabaseInformation;
            /// <summary> 
            /// Table Information Alarm
            /// </summary>
            public string strTableInformationAlarm;
            /// <summary>
            /// Table Information SVID
            /// </summary>
            public string strTableInformationSVID;
            /// <summary>
            /// Table Information UI Text
            /// </summary>
            public string strTableInformationUIText;
            /// <summary>
            /// Table Information User Message
            /// </summary>
            public string strTableInformationUserMessage;
            /// <summary>
            /// Table Information User
            /// </summary>
            public string strTableInformationUser;
            /// <summary>
            /// Table Information EC
            /// </summary>
            public string strTableInformationEC;
            /// <summary>
            /// Table History Alarm
            /// </summary>
            public string strTableHistoryAlarm;
            /// <summary>
            /// Table History OP Call
            /// </summary>
            public string strTableHistoryOPCall;
            /// <summary>
            /// Table History Interlock Call
            /// </summary>
            public string strTableHistoryInterlockCall;
            /// <summary>
            /// Table History Terminal Call
            /// </summary>
            public string strTableHistoryTerminalCall;
            /// <summary>
            /// Table History Process Data Cell
            /// </summary>
            public string strTableHistoryProcessDataCell;
            /// <summary>
            /// Table History Process Data Judgement
            /// </summary>
            public string strTableHistoryProcessDataJudgement;
            /// <summary>
            /// Table History Process Data Reader
            /// </summary>
            public string strTableHistoryProcessDataReader;
            /// <summary>
            /// Table History Process Data Vision Result
            /// </summary>
            public string strTableHistoryProcessDataVisionResult;
            /// <summary>
            /// Table History Process Data Work Order
            /// </summary>
            public string strTableHistoryProcessDataWorkOrder;
            /// <summary>
            /// Table History Cell Track In
            /// </summary>
            public string strTableHistoryCellTrackIn;
            /// <summary>
            /// Table History Cell Track Out
            /// </summary>
            public string strTableHistoryCellTrackOut;
            /// <summary>
            /// Table History Cell Input
            /// </summary>
            public string strTableHistoryCellInput;
            /// <summary>
            /// Table History Cell Output
            /// </summary>
            public string strTableHistoryCellOutput;
            /// <summary>
            /// Table History MCR Status
            /// </summary>
            public string strTableHistoryMCRStatus;
            /// <summary>
            /// Table History Machine Result
            /// </summary>
            public string strTableHistoryMachineResult;
            /// <summary>
            /// Table History Alarm Event
            /// </summary>
            public string strTableHistoryAlarmEvent;
            /// <summary>
            /// Table History Equipment Stop Event
            /// </summary>
            public string strTableHistoryEquipmentStopEvent;
            /// <summary>
            /// Table History Equipment TP Code Event
            /// </summary>
            public string strTableHistoryEquipmentTPCodeEvent;
            /// <summary>
            /// Table History Equipment Upper Loss Time Event
            /// </summary>
            public string strTableHistoryEquipmentUpperLossTimeEvent;
            /// <summary>
            /// Table History Equipment Lower Loss Time Event
            /// </summary>
            public string strTableHistoryEquipmentLowerLossTimeEvent;
            /// <summary>
            /// Table Safety PLC Modified Time Event
            /// </summary>
            public string strTableHistorySafetyPlcModifiedEvent;
            /// <summary>
            /// Record Information Alarm
            /// </summary>
            public string strRecordInformationAlarm;
            /// <summary>
            /// Record Information EC
            /// </summary>
            public string strRecordInformationEC;
            /// <summary>
            /// Record Information SVID
            /// </summary>
            public string strRecordInformationSVID;
            /// <summary>
            /// Record Information UI Text
            /// </summary>
            public string strRecordInformationUIText;
            /// <summary>
            /// Record Information User Message
            /// </summary>
            public string strRecordInformationUserMessage;
            /// <summary>
            ///  Record Information User
            /// </summary>
            public string strRecordInformationUser;
            /// <summary>
            /// Delete Period Alarm
            /// </summary>
            public int iDeletePeriodAlarm;
            /// <summary>
            /// Delete Period OP Call
            /// </summary>
            public int iDeletePeriodOPCall;
            /// <summary>
            /// Delete Period Interlock Call
            /// </summary>
            public int iDeletePeriodInterlockCall;
            /// <summary>
            /// Delete Period Terminal Call
            /// </summary>
            public int iDeletePeriodTerminalCall;
            /// <summary>
            /// Delete Period Process Data Cell
            /// </summary>
            public int iDeletePeriodProcessDataCell;
            /// <summary>
            /// Delete Period Process Data Judgement
            /// </summary>
            public int iDeletePeriodProcessDataJudgement;
            /// <summary>
            /// Delete Period Process Data Reader
            /// </summary>
            public int iDeletePeriodProcessDataReader;
            /// <summary>
            /// Delete Period Process Data Vision Pixel Position
            /// </summary>
            public int iDeletePeriodProcessDataVisionPixelPosition;
            /// <summary>
            /// Delete Period Process Data Vision Result
            /// </summary>
            public int iDeletePeriodProcessDataVisionResult;
            /// <summary>
            /// Delete Period Process Data Work Order
            /// </summary>
            public int iDeletePeriodProcessDataWorkOrder;
            /// <summary>
            /// Delete Period Cell Track In
            /// </summary>
            public int iDeletePeriodCellTrackIn;
            /// <summary>
            /// Delete Period Cell Track Out
            /// </summary>
            public int iDeletePeriodCellTrackOut;
            /// <summary>
            /// Delete Period Cell Input
            /// </summary>
            public int iDeletePeriodCellInput;
            /// <summary>
            /// Delete Period Cell Output
            /// </summary>
            public int iDeletePeriodCellOutput;
            /// <summary>
            /// Delete Period MCR Status
            /// </summary>
            public int iDeletePeriodMCRStatus;
            /// <summary>
            /// Delete Period Machine Result
            /// </summary>
            public int iDeletePeriodMachineResult;
            /// <summary>
            /// Delete Period Alarm Event
            /// </summary>
            public int iDeletePeriodAlarmEvent;
            /// <summary>
            /// Delete Period Equipment Stop Event
            /// </summary>
            public int iDeletePeriodEquipmentStopEvent;
            /// <summary>
            /// Delete Period Equipment TP Code Event
            /// </summary>
            public int iDeletePeriodEquipmentTPCodeEvent;
            /// <summary>
            /// Delete Period Equipment Upper Loss Time Event
            /// </summary>
            public int iDeletePeriodEquipmentUpperLossTimeEvent;
            /// <summary>
            /// Delete Period Equipment Lower Loss Time Event
            /// </summary>
            public int iDeletePeriodEquipmentLowerLossTimeEvent;
        }

        /// <summary>
        /// 데이터베이스 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CDatabaseParameter GetDatabaseParameter() => m_objDatabaseParameter;

        /// <summary>
        /// 데이터베이스 파라미터 선언
        /// </summary>
        private readonly CDatabaseParameter m_objDatabaseParameter = new CDatabaseParameter();

        /// <summary>
        /// 데이터베이스 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadDatabaseParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string strSectionName = "DATABASE";
                // Database
                m_objDatabaseParameter.strDatabaseHistory = objINI.GetString(strSectionName, "strDatabaseHistory", "DATABASE_HISTORY");
                m_objDatabaseParameter.strDatabaseInformation = objINI.GetString(strSectionName, "strDatabaseInformation", "DATABASE_INFORMATION");
                // Table
                m_objDatabaseParameter.strTableInformationAlarm = objINI.GetString(strSectionName, "strTableInformationAlarm", "TABLE_INFORMATION_ALARM");
                m_objDatabaseParameter.strTableInformationSVID = objINI.GetString(strSectionName, "strTableInformationSVID", "TABLE_INFORMATION_SVID");
                m_objDatabaseParameter.strTableInformationUIText = objINI.GetString(strSectionName, "strTableInformationUIText", "TABLE_INFORMATION_UI_TEXT");
                m_objDatabaseParameter.strTableInformationUserMessage = objINI.GetString(strSectionName, "strTableInformationUserMessage", "TABLE_INFORMATION_USER_MESSAGE");
                m_objDatabaseParameter.strTableInformationUser = objINI.GetString(strSectionName, "strTableInformationUser", "TABLE_INFORMATION_USER");
                m_objDatabaseParameter.strTableInformationEC = objINI.GetString(strSectionName, "strTableInformationEC", "TABLE_INFORMATION_EC");
                m_objDatabaseParameter.strTableHistoryAlarm = objINI.GetString(strSectionName, "strTableHistoryAlarm", "TABLE_HISTORY_ALARM");
                m_objDatabaseParameter.strTableHistoryOPCall = objINI.GetString(strSectionName, "strTableHistoryOPCall", "TABLE_HISTORY_OP_CALL");
                m_objDatabaseParameter.strTableHistoryInterlockCall = objINI.GetString(strSectionName, "strTableHistoryInterlockCall", "TABLE_HISTORY_INTERLOCK_CALL");
                m_objDatabaseParameter.strTableHistoryTerminalCall = objINI.GetString(strSectionName, "strTableHistoryTerminalCall", "TABLE_HISTORY_TERMINAL_CALL");
                m_objDatabaseParameter.strTableHistoryProcessDataCell = objINI.GetString(strSectionName, "strTableHistoryProcessDataCell", "TABLE_HISTORY_PROCESS_DATA_CELL");
                m_objDatabaseParameter.strTableHistoryProcessDataJudgement = objINI.GetString(strSectionName, "strTableHistoryProcessDataJudgement", "TABLE_HISTORY_PROCESS_DATA_JUDGEMENT");
                m_objDatabaseParameter.strTableHistoryProcessDataReader = objINI.GetString(strSectionName, "strTableHistoryProcessDataReader", "TABLE_HISTORY_PROCESS_DATA_READER");
                m_objDatabaseParameter.strTableHistoryProcessDataVisionResult = objINI.GetString(strSectionName, "strTableHistoryProcessDataVisionResult", "TABLE_HISTORY_PROCESS_DATA_VISION_RESULT");
                m_objDatabaseParameter.strTableHistoryProcessDataWorkOrder = objINI.GetString(strSectionName, "strTableHistoryProcessDataWorkOrder", "TABLE_HISTORY_PROCESS_DATA_WORK_ORDER");
                m_objDatabaseParameter.strTableHistoryCellTrackIn = objINI.GetString(strSectionName, "strTableHistoryCellTrackIn", "TABLE_HISTORY_CELL_TRACK_IN");
                m_objDatabaseParameter.strTableHistoryCellTrackOut = objINI.GetString(strSectionName, "strTableHistoryCellTrackOut", "TABLE_HISTORY_CELL_TRACK_OUT");
                m_objDatabaseParameter.strTableHistoryCellInput = objINI.GetString(strSectionName, "strTableHistoryCellInput", "TABLE_HISTORY_CELL_INPUT");
                m_objDatabaseParameter.strTableHistoryCellOutput = objINI.GetString(strSectionName, "strTableHistoryCellOutput", "TABLE_HISTORY_CELL_OUTPUT");
                m_objDatabaseParameter.strTableHistoryMCRStatus = objINI.GetString(strSectionName, "strTableHistoryMCRStatus", "TABLE_HISTORY_MCR_STATUS");
                m_objDatabaseParameter.strTableHistoryMachineResult = objINI.GetString(strSectionName, "strTableHistoryMachineResult", "TABLE_HISTORY_MACHINE_RESULT");
                m_objDatabaseParameter.strTableHistoryAlarmEvent = objINI.GetString(strSectionName, "strTableHistoryAlarmEvent", "TABLE_HISTORY_ALARM_EVENT");
                m_objDatabaseParameter.strTableHistoryEquipmentStopEvent = objINI.GetString(strSectionName, "strTableHistoryEquipmentStopEvent", "TABLE_HISTORY_EQUIPMENT_STOP_EVENT");
                m_objDatabaseParameter.strTableHistoryEquipmentTPCodeEvent = objINI.GetString(strSectionName, "strTableHistoryEquipmentTPCodeEvent", "TABLE_HISTORY_EQUIPMENT_TP_CODE_EVENT");
                m_objDatabaseParameter.strTableHistoryEquipmentUpperLossTimeEvent = objINI.GetString(strSectionName, "strTableHistoryEquipmentUpperLossTimeEvent", "TABLE_HISTORY_EQUIPMENT_UPPER_LOSS_TIME_EVENT");
                m_objDatabaseParameter.strTableHistoryEquipmentLowerLossTimeEvent = objINI.GetString(strSectionName, "strTableHistoryEquipmentLowerLossTimeEvent", "TABLE_HISTORY_EQUIPMENT_LOWER_LOSS_TIME_EVENT");
                m_objDatabaseParameter.strTableHistorySafetyPlcModifiedEvent = objINI.GetString(strSectionName, "strTableHistorySafetyPlcModifiedEvent", "TABLE_HISTORY_SAFETY_PLC_MODIFIED_EVENT");
                // Record
                m_objDatabaseParameter.strRecordInformationAlarm = objINI.GetString(strSectionName, "strRecordInformationAlarm", "RECORD_INFORMATION_ALARM");
                m_objDatabaseParameter.strRecordInformationEC = objINI.GetString(strSectionName, "strRecordInformationEC", "RECORD_INFORMATION_EC");
                m_objDatabaseParameter.strRecordInformationSVID = objINI.GetString(strSectionName, "strRecordInformationSVID", "RECORD_INFORMATION_SVID");
                m_objDatabaseParameter.strRecordInformationUIText = objINI.GetString(strSectionName, "strRecordInformationUIText", "RECORD_INFORMATION_UI_TEXT");
                m_objDatabaseParameter.strRecordInformationUser = objINI.GetString(strSectionName, "strRecordInformationUser", "RECORD_INFORMATION_USER");
                m_objDatabaseParameter.strRecordInformationUserMessage = objINI.GetString(strSectionName, "strRecordInformationUserMessage", "RECORD_INFORMATION_USER_MESSAGE");
                // Delete
                m_objDatabaseParameter.iDeletePeriodAlarm = objINI.GetInt32(strSectionName, "iDeletePeriodAlarm", 90);
                m_objDatabaseParameter.iDeletePeriodOPCall = objINI.GetInt32(strSectionName, "iDeletePeriodOPCall", 90);
                m_objDatabaseParameter.iDeletePeriodInterlockCall = objINI.GetInt32(strSectionName, "iDeletePeriodInterlockCall", 90);
                m_objDatabaseParameter.iDeletePeriodTerminalCall = objINI.GetInt32(strSectionName, "iDeletePeriodTerminalCall", 90);
                m_objDatabaseParameter.iDeletePeriodProcessDataCell = objINI.GetInt32(strSectionName, "iDeletePeriodProcessDataCell", 90);
                m_objDatabaseParameter.iDeletePeriodProcessDataJudgement = objINI.GetInt32(strSectionName, "iDeletePeriodProcessDataJudgement", 90);
                m_objDatabaseParameter.iDeletePeriodProcessDataReader = objINI.GetInt32(strSectionName, "iDeletePeriodProcessDataReader", 90);
                m_objDatabaseParameter.iDeletePeriodProcessDataVisionPixelPosition = objINI.GetInt32(strSectionName, "iDeletePeriodProcessDataVisionPixelPosition", 90);
                m_objDatabaseParameter.iDeletePeriodProcessDataVisionResult = objINI.GetInt32(strSectionName, "iDeletePeriodProcessDataVisionResult", 90);
                m_objDatabaseParameter.iDeletePeriodProcessDataWorkOrder = objINI.GetInt32(strSectionName, "iDeletePeriodProcessDataWorkOrder", 90);
                m_objDatabaseParameter.iDeletePeriodCellTrackIn = objINI.GetInt32(strSectionName, "iDeletePeriodCellTrackIn", 90);
                m_objDatabaseParameter.iDeletePeriodCellTrackOut = objINI.GetInt32(strSectionName, "iDeletePeriodCellTrackOut", 90);
                m_objDatabaseParameter.iDeletePeriodCellInput = objINI.GetInt32(strSectionName, "iDeletePeriodCellInput", 90);
                m_objDatabaseParameter.iDeletePeriodCellOutput = objINI.GetInt32(strSectionName, "iDeletePeriodCellOutput", 90);
                m_objDatabaseParameter.iDeletePeriodMCRStatus = objINI.GetInt32(strSectionName, "iDeletePeriodMCRStatus", 90);
                m_objDatabaseParameter.iDeletePeriodMachineResult = objINI.GetInt32(strSectionName, "iDeletePeriodMachineResult", 90);
                m_objDatabaseParameter.iDeletePeriodAlarmEvent = objINI.GetInt32(strSectionName, "iDeletePeriodAlarmEvent", 90);
                m_objDatabaseParameter.iDeletePeriodEquipmentStopEvent = objINI.GetInt32(strSectionName, "iDeletePeriodEquipmentStopEvent", 90);
                m_objDatabaseParameter.iDeletePeriodEquipmentTPCodeEvent = objINI.GetInt32(strSectionName, "iDeletePeriodEquipmentTPCodeEvent", 90);
                m_objDatabaseParameter.iDeletePeriodEquipmentUpperLossTimeEvent = objINI.GetInt32(strSectionName, "iDeletePeriodEquipmentUpperLossTimeEvent", 90);
                m_objDatabaseParameter.iDeletePeriodEquipmentLowerLossTimeEvent = objINI.GetInt32(strSectionName, "iDeletePeriodEquipmentLowerLossTimeEvent", 90);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터베이스 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveDatabaseParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string strSectionName = "DATABASE";

                // Database
                objINI.WriteValue(strSectionName, "strDatabaseHistory", m_objDatabaseParameter.strDatabaseHistory);
                objINI.WriteValue(strSectionName, "strDatabaseInformation", m_objDatabaseParameter.strDatabaseInformation);
                // Table
                objINI.WriteValue(strSectionName, "strTableInformationAlarm", m_objDatabaseParameter.strTableInformationAlarm);
                objINI.WriteValue(strSectionName, "strTableInformationSVID", m_objDatabaseParameter.strTableInformationSVID);
                objINI.WriteValue(strSectionName, "strTableInformationUIText", m_objDatabaseParameter.strTableInformationUIText);
                objINI.WriteValue(strSectionName, "strTableInformationUserMessage", m_objDatabaseParameter.strTableInformationUserMessage);
                objINI.WriteValue(strSectionName, "strTableInformationUser", m_objDatabaseParameter.strTableInformationUser);
                objINI.WriteValue(strSectionName, "strTableInformationEC", m_objDatabaseParameter.strTableInformationEC);
                objINI.WriteValue(strSectionName, "strTableHistoryAlarm", m_objDatabaseParameter.strTableHistoryAlarm);
                objINI.WriteValue(strSectionName, "strTableHistoryOPCall", m_objDatabaseParameter.strTableHistoryOPCall);
                objINI.WriteValue(strSectionName, "strTableHistoryInterlockCall", m_objDatabaseParameter.strTableHistoryInterlockCall);
                objINI.WriteValue(strSectionName, "strTableHistoryTerminalCall", m_objDatabaseParameter.strTableHistoryTerminalCall);
                objINI.WriteValue(strSectionName, "strTableHistoryProcessDataCell", m_objDatabaseParameter.strTableHistoryProcessDataCell);
                objINI.WriteValue(strSectionName, "strTableHistoryProcessDataJudgement", m_objDatabaseParameter.strTableHistoryProcessDataJudgement);
                objINI.WriteValue(strSectionName, "strTableHistoryProcessDataReader", m_objDatabaseParameter.strTableHistoryProcessDataReader);
                objINI.WriteValue(strSectionName, "strTableHistoryProcessDataVisionResult", m_objDatabaseParameter.strTableHistoryProcessDataVisionResult);
                objINI.WriteValue(strSectionName, "strTableHistoryProcessDataWorkOrder", m_objDatabaseParameter.strTableHistoryProcessDataWorkOrder);
                objINI.WriteValue(strSectionName, "strTableHistoryCellTrackIn", m_objDatabaseParameter.strTableHistoryCellTrackIn);
                objINI.WriteValue(strSectionName, "strTableHistoryCellTrackOut", m_objDatabaseParameter.strTableHistoryCellTrackOut);
                objINI.WriteValue(strSectionName, "strTableHistoryCellInput", m_objDatabaseParameter.strTableHistoryCellInput);
                objINI.WriteValue(strSectionName, "strTableHistoryCellOutput", m_objDatabaseParameter.strTableHistoryCellOutput);
                objINI.WriteValue(strSectionName, "strTableHistoryMCRStatus", m_objDatabaseParameter.strTableHistoryMCRStatus);
                objINI.WriteValue(strSectionName, "strTableHistoryMachineResult", m_objDatabaseParameter.strTableHistoryMachineResult);
                objINI.WriteValue(strSectionName, "strTableHistoryAlarmEvent", m_objDatabaseParameter.strTableHistoryAlarmEvent);
                objINI.WriteValue(strSectionName, "strTableHistoryEquipmentStopEvent", m_objDatabaseParameter.strTableHistoryEquipmentStopEvent);
                objINI.WriteValue(strSectionName, "strTableHistoryEquipmentTPCodeEvent", m_objDatabaseParameter.strTableHistoryEquipmentTPCodeEvent);
                objINI.WriteValue(strSectionName, "strTableHistoryEquipmentUpperLossTimeEvent", m_objDatabaseParameter.strTableHistoryEquipmentUpperLossTimeEvent);
                objINI.WriteValue(strSectionName, "strTableHistoryEquipmentLowerLossTimeEvent", m_objDatabaseParameter.strTableHistoryEquipmentLowerLossTimeEvent);
                objINI.WriteValue(strSectionName, "strTableHistorySafetyPlcModifiedEvent", m_objDatabaseParameter.strTableHistorySafetyPlcModifiedEvent);
                // Record
                objINI.WriteValue(strSectionName, "strRecordInformationAlarm", m_objDatabaseParameter.strRecordInformationAlarm);
                objINI.WriteValue(strSectionName, "strRecordInformationEC", m_objDatabaseParameter.strRecordInformationEC);
                objINI.WriteValue(strSectionName, "strRecordInformationSVID", m_objDatabaseParameter.strRecordInformationSVID);
                objINI.WriteValue(strSectionName, "strRecordInformationUIText", m_objDatabaseParameter.strRecordInformationUIText);
                objINI.WriteValue(strSectionName, "strRecordInformationUser", m_objDatabaseParameter.strRecordInformationUser);
                objINI.WriteValue(strSectionName, "strRecordInformationUserMessage", m_objDatabaseParameter.strRecordInformationUserMessage);
                // Delete
                objINI.WriteValue(strSectionName, "iDeletePeriodAlarm", m_objDatabaseParameter.iDeletePeriodAlarm);
                objINI.WriteValue(strSectionName, "iDeletePeriodOPCall", m_objDatabaseParameter.iDeletePeriodOPCall);
                objINI.WriteValue(strSectionName, "iDeletePeriodInterlockCall", m_objDatabaseParameter.iDeletePeriodInterlockCall);
                objINI.WriteValue(strSectionName, "iDeletePeriodTerminalCall", m_objDatabaseParameter.iDeletePeriodTerminalCall);
                objINI.WriteValue(strSectionName, "iDeletePeriodProcessDataCell", m_objDatabaseParameter.iDeletePeriodProcessDataCell);
                objINI.WriteValue(strSectionName, "iDeletePeriodProcessDataJudgement", m_objDatabaseParameter.iDeletePeriodProcessDataJudgement);
                objINI.WriteValue(strSectionName, "iDeletePeriodProcessDataReader", m_objDatabaseParameter.iDeletePeriodProcessDataReader);
                objINI.WriteValue(strSectionName, "iDeletePeriodProcessDataVisionPixelPosition", m_objDatabaseParameter.iDeletePeriodProcessDataVisionPixelPosition);
                objINI.WriteValue(strSectionName, "iDeletePeriodProcessDataVisionResult", m_objDatabaseParameter.iDeletePeriodProcessDataVisionResult);
                objINI.WriteValue(strSectionName, "iDeletePeriodProcessDataWorkOrder", m_objDatabaseParameter.iDeletePeriodProcessDataWorkOrder);
                objINI.WriteValue(strSectionName, "iDeletePeriodCellTrackIn", m_objDatabaseParameter.iDeletePeriodCellTrackIn);
                objINI.WriteValue(strSectionName, "iDeletePeriodCellTrackOut", m_objDatabaseParameter.iDeletePeriodCellTrackOut);
                objINI.WriteValue(strSectionName, "iDeletePeriodCellInput", m_objDatabaseParameter.iDeletePeriodCellInput);
                objINI.WriteValue(strSectionName, "iDeletePeriodCellOutput", m_objDatabaseParameter.iDeletePeriodCellOutput);
                objINI.WriteValue(strSectionName, "iDeletePeriodMCRStatus", m_objDatabaseParameter.iDeletePeriodMCRStatus);
                objINI.WriteValue(strSectionName, "iDeletePeriodMachineResult", m_objDatabaseParameter.iDeletePeriodMachineResult);
                objINI.WriteValue(strSectionName, "iDeletePeriodAlarmEvent", m_objDatabaseParameter.iDeletePeriodAlarmEvent);
                objINI.WriteValue(strSectionName, "iDeletePeriodEquipmentStopEvent", m_objDatabaseParameter.iDeletePeriodEquipmentStopEvent);
                objINI.WriteValue(strSectionName, "iDeletePeriodEquipmentTPCodeEvent", m_objDatabaseParameter.iDeletePeriodEquipmentTPCodeEvent);
                objINI.WriteValue(strSectionName, "iDeletePeriodEquipmentUpperLossTimeEvent", m_objDatabaseParameter.iDeletePeriodEquipmentUpperLossTimeEvent);
                objINI.WriteValue(strSectionName, "iDeletePeriodEquipmentLowerLossTimeEvent", m_objDatabaseParameter.iDeletePeriodEquipmentLowerLossTimeEvent);

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
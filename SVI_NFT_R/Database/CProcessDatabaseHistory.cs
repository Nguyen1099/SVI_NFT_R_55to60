using System;

namespace SVI_NFT_R
{
    public class CProcessDatabaseHistory : CDatabaseAbstract
    {
        /// <summary>
        /// Svid Monitoring 기능 잠금
        /// </summary>
        public bool SvidMonitoringLock
        {
            get
            {
                return m_objProcessDatabaseMonitorSVID.IsSvidMonitorLock;
            }
            set
            {
                m_objProcessDatabaseMonitorSVID.IsSvidMonitorLock = value;
            }
        }
        /// <summary>
        /// Monitor SVID
        /// </summary>
        private CProcessDatabaseMonitorSVID m_objProcessDatabaseMonitorSVID;
        /// <summary>
        /// History Delete
        /// </summary>
        private CProcessDatabaseHistoryDelete m_objProcessDatabaseHistoryDelete;
        /// <summary>
        /// History Alarm Event
        /// </summary>
        private CProcessDatabaseHistoryAlarmEvent m_objProcessDatabaseHistoryAlarmEvent;
        /// <summary>
        /// History Equipment Stop Event
        /// </summary>
        private CProcessDatabaseHistoryEquipmentStopEvent m_objProcessDatabaseHistoryEquipmentStopEvent;
        /// <summary>
        /// History Equipment Upper Loss Time Event
        /// </summary>
        private CProcessDatabaseHistoryEquipmentUpperLossTimeEvent m_objProcessDatabaseHistoryEquipmentUpperLossTimeEvent;
        /// <summary>
        /// History Equipment Lower Loss Time Event
        /// </summary>
        private CProcessDatabaseHistoryEquipmentLowerLossTimeEvent m_objProcessDatabaseHistoryEquipmentLowerLossTimeEvent;
        /// <summary>
        /// History Safety PLC Modified Event
        /// </summary>
        private CProcessDatabaseHistorySafetyPlcModifiedEvent m_objProcessDatabaseHistorySafetyPlcModifiedEvent;
        /// <summary>
        /// SQLite
        /// </summary>
        public CSQLite m_objSQLite;
        /// <summary>
        /// History Alarm
        /// </summary>
        public CManagerTable m_objManagerTableHistoryAlarm;
        /// <summary>
        /// History OP Call
        /// </summary>
        public CManagerTable m_objManagerTableHistoryOPCall;
        /// <summary>
        /// History Interlock Call
        /// </summary>
        public CManagerTable m_objManagerTableHistoryInterlockCall;
        /// <summary>
        /// History Terminal Call
        /// </summary>
        public CManagerTable m_objManagerTableHistoryTerminalCall;
        /// <summary>
        /// History Process Data Cell
        /// </summary>
        public CManagerTable m_objManagerTableHistoryProcessDataCell;
        /// <summary>
        /// History Process Data Judgement
        /// </summary>
        public CManagerTable m_objManagerTableHistoryProcessDataJudgement;
        /// <summary>
        /// History Process Data Reader
        /// </summary>
        public CManagerTable m_objManagerTableHistoryProcessDataReader;
        /// <summary>
        /// History Process Data Vision Result
        /// </summary>
        public CManagerTable m_objManagerTableHistoryProcessDataVisionResult;
        /// <summary>
        /// History Process Data Work Order
        /// </summary>
        public CManagerTable m_objManagerTableHistoryProcessDataWorkOrder;
        /// <summary>
        /// History Cell Track In
        /// </summary>
        public CManagerTable m_objManagerTableHistoryCellTrackIn;
        /// <summary>
        /// History Cell Track Out
        /// </summary>
        public CManagerTable m_objManagerTableHistoryCellTrackOut;
        /// <summary>
        /// History Cell Input
        /// </summary>
        public CManagerTable m_objManagerTableHistoryCellInput;
        /// <summary>
        /// History Cell Output
        /// </summary>
        public CManagerTable m_objManagerTableHistoryCellOutput;
        /// <summary>
        /// History MCR Status
        /// </summary>
        public CManagerTable m_objManagerTableHistoryMCRStatus;
        /// <summary>
        /// History Machine Result
        /// </summary>
        public CManagerTable m_objManagerTableHistoryMachineResult;
        /// <summary>
        /// History Alarm Event
        /// </summary>
        public CManagerTable m_objManagerTableHistoryAlarmEvent;
        /// <summary>
        /// History Equipment Stop Event
        /// </summary>
        public CManagerTable m_objManagerTableHistoryEquipmentStopEvent;
        /// <summary>
        /// History Equipment TP Code Event
        /// </summary>
        public CManagerTable m_objManagerTableHistoryEquipmentTPCodeEvent;
        /// <summary>
        /// History Equipment Upper Loss Time Event
        /// </summary>
        public CManagerTable m_objManagerTableHistoryEquipmentUpperLossTimeEvent;
        /// <summary>
        /// History Equipment Lower Loss Time Event
        /// </summary>
        public CManagerTable m_objManagerTableHistoryEquipmentLowerLossTimeEvent;
        /// <summary>
        /// History Safety PLC Modified Event
        /// </summary>
        public CManagerTable m_objManagerTableHistorySafetyPlcModifiedEvent;

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="objProcessDatabase"></param>
        /// <returns></returns>
        public bool Initialize(CProcessDatabase objProcessDatabase)
        {
            bool bReturn = false;

            do
            {
                // 상위 클래스 이어줌
                base.m_objProcessDatabase = objProcessDatabase;
                CConfig objConfig = objProcessDatabase.GetDocument().m_objConfig;
                // 데이터베이스 파라메터
                CConfig.CDatabaseParameter objDatabaseParameter = objConfig.GetDatabaseParameter();
                // SQLite 초기화
                m_objSQLite = new CSQLite();
                CErrorReturn objReturn = m_objSQLite.HLInitialize(string.Format(@"{0}\{1}.db3", objConfig.GetDatabaseHistoryPath(), objDatabaseParameter.strDatabaseHistory));
                if (true == objReturn.m_bError)
                    break;
                // SQLite Connect
                objReturn = m_objSQLite.HLConnect();
                if (true == objReturn.m_bError)
                    break;
                // History Alarm 초기화
                m_objManagerTableHistoryAlarm = new CManagerTable();
                if (false == m_objManagerTableHistoryAlarm.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryAlarm), ""))
                    break;
                // History OP Call
                m_objManagerTableHistoryOPCall = new CManagerTable();
                if (false == m_objManagerTableHistoryOPCall.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryOPCall), ""))
                    break;
                // History Interlock Call
                m_objManagerTableHistoryInterlockCall = new CManagerTable();
                if (false == m_objManagerTableHistoryInterlockCall.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryInterlockCall), ""))
                    break;
                if (true == objReturn.m_bError)
                    break;
                // History Terminal Call
                m_objManagerTableHistoryTerminalCall = new CManagerTable();
                if (false == m_objManagerTableHistoryTerminalCall.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryTerminalCall), ""))
                    break;
                // History Process Data Cell
                m_objManagerTableHistoryProcessDataCell = new CManagerTable();
                if (false == m_objManagerTableHistoryProcessDataCell.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryProcessDataCell), ""))
                    break;
                // History Process Data Judgement
                m_objManagerTableHistoryProcessDataJudgement = new CManagerTable();
                if (false == m_objManagerTableHistoryProcessDataJudgement.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryProcessDataJudgement), ""))
                    break;
                // History Process Data Reader
                m_objManagerTableHistoryProcessDataReader = new CManagerTable();
                if (false == m_objManagerTableHistoryProcessDataReader.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryProcessDataReader), ""))
                    break;
                // History Process Data Vision Result
                m_objManagerTableHistoryProcessDataVisionResult = new CManagerTable();
                if (false == m_objManagerTableHistoryProcessDataVisionResult.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryProcessDataVisionResult), ""))
                    break;
                // History Process Data Work Order
                m_objManagerTableHistoryProcessDataWorkOrder = new CManagerTable();
                if (false == m_objManagerTableHistoryProcessDataWorkOrder.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryProcessDataWorkOrder), ""))
                    break;
                // History Cell Track In
                m_objManagerTableHistoryCellTrackIn = new CManagerTable();
                if (false == m_objManagerTableHistoryCellTrackIn.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryCellTrackIn), ""))
                    break;
                // History Cell Track Out
                m_objManagerTableHistoryCellTrackOut = new CManagerTable();
                if (false == m_objManagerTableHistoryCellTrackOut.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryCellTrackOut), ""))
                    break;
                // History Cell Input
                m_objManagerTableHistoryCellInput = new CManagerTable();
                if (false == m_objManagerTableHistoryCellInput.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryCellInput), ""))
                    break;
                // History Cell Output
                m_objManagerTableHistoryCellOutput = new CManagerTable();
                if (false == m_objManagerTableHistoryCellOutput.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryCellOutput), ""))
                    break;
                // History MCR Status
                m_objManagerTableHistoryMCRStatus = new CManagerTable();
                if (false == m_objManagerTableHistoryMCRStatus.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryMCRStatus), ""))
                    break;
                // History Machine Result
                m_objManagerTableHistoryMachineResult = new CManagerTable();
                if (false == m_objManagerTableHistoryMachineResult.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryMachineResult), ""))
                    break;
                // History Alarm Event
                m_objManagerTableHistoryAlarmEvent = new CManagerTable();
                if (false == m_objManagerTableHistoryAlarmEvent.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryAlarmEvent), ""))
                    break;
                // History Equipment Stop Event
                m_objManagerTableHistoryEquipmentStopEvent = new CManagerTable();
                if (false == m_objManagerTableHistoryEquipmentStopEvent.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryEquipmentStopEvent), ""))
                    break;
                // History Equipment TP Code Event
                m_objManagerTableHistoryEquipmentTPCodeEvent = new CManagerTable();
                if (false == m_objManagerTableHistoryEquipmentTPCodeEvent.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryEquipmentTPCodeEvent), ""))
                    break;
                // History Equipment Upper Loss Time Event
                m_objManagerTableHistoryEquipmentUpperLossTimeEvent = new CManagerTable();
                if (false == m_objManagerTableHistoryEquipmentUpperLossTimeEvent.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryEquipmentUpperLossTimeEvent), ""))
                    break;
                // History Equipment Lower Loss Time Event
                m_objManagerTableHistoryEquipmentLowerLossTimeEvent = new CManagerTable();
                if (false == m_objManagerTableHistoryEquipmentLowerLossTimeEvent.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistoryEquipmentLowerLossTimeEvent), ""))
                    break;
                // History Safety PLC Modified Event
                m_objManagerTableHistorySafetyPlcModifiedEvent = new CManagerTable();
                if (false == m_objManagerTableHistorySafetyPlcModifiedEvent.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableHistorySafetyPlcModifiedEvent), ""))
                    break;
                // Process Monitor SVID 초기화
                m_objProcessDatabaseMonitorSVID = new CProcessDatabaseMonitorSVID();
                if (false == m_objProcessDatabaseMonitorSVID.Initialize(m_objProcessDatabase))
                    break;
                // Process History Delete 초기화
                m_objProcessDatabaseHistoryDelete = new CProcessDatabaseHistoryDelete();
                if (false == m_objProcessDatabaseHistoryDelete.Initialize(m_objProcessDatabase, m_objSQLite))
                    break;
                // Process History Alarm Event 초기화
                m_objProcessDatabaseHistoryAlarmEvent = new CProcessDatabaseHistoryAlarmEvent();
                if (false == m_objProcessDatabaseHistoryAlarmEvent.Initialize(m_objProcessDatabase))
                    break;
                // Process History Equipment Stop Event 초기화
                m_objProcessDatabaseHistoryEquipmentStopEvent = new CProcessDatabaseHistoryEquipmentStopEvent();
                if (false == m_objProcessDatabaseHistoryEquipmentStopEvent.Initialize(m_objProcessDatabase))
                    break;
                // Process History Equipment Upper Loss Time Event 초기화
                m_objProcessDatabaseHistoryEquipmentUpperLossTimeEvent = new CProcessDatabaseHistoryEquipmentUpperLossTimeEvent();
                if (false == m_objProcessDatabaseHistoryEquipmentUpperLossTimeEvent.Initialize(m_objProcessDatabase))
                    break;
                // Process History Equipment Lower Loss Time Event 초기화
                m_objProcessDatabaseHistoryEquipmentLowerLossTimeEvent = new CProcessDatabaseHistoryEquipmentLowerLossTimeEvent();
                if (false == m_objProcessDatabaseHistoryEquipmentLowerLossTimeEvent.Initialize(m_objProcessDatabase))
                    break;
                // Process History Safety PLC Modified Event 초기화
                m_objProcessDatabaseHistorySafetyPlcModifiedEvent = new CProcessDatabaseHistorySafetyPlcModifiedEvent();
                if (false == m_objProcessDatabaseHistorySafetyPlcModifiedEvent.Initialize(m_objProcessDatabase))
                    break;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제 함수
        /// </summary>
        public void Deinitialize()
        {
            // Process History Safety PLC Modified Event 해제
            if (null != m_objProcessDatabaseHistorySafetyPlcModifiedEvent)
            {
                m_objProcessDatabaseHistorySafetyPlcModifiedEvent.DeInitialize();
            }
            // Process History Equipment Upper Loss Time Event 해제
            if (null != m_objProcessDatabaseHistoryEquipmentUpperLossTimeEvent)
            {
                m_objProcessDatabaseHistoryEquipmentUpperLossTimeEvent.DeInitialize();
            }
            // Process History Equipment Lower Loss Time Event 해제
            if (null != m_objProcessDatabaseHistoryEquipmentLowerLossTimeEvent)
            {
                m_objProcessDatabaseHistoryEquipmentLowerLossTimeEvent.DeInitialize();
            }
            // Process History Equipment Stop Event 해제
            if (null != m_objProcessDatabaseHistoryEquipmentStopEvent)
            {
                m_objProcessDatabaseHistoryEquipmentStopEvent.DeInitialize();
            }
            // Process History Alarm Event 해제
            if (null != m_objProcessDatabaseHistoryAlarmEvent)
            {
                m_objProcessDatabaseHistoryAlarmEvent.DeInitialize();
            }
            // Process History Delete 해제
            if (null != m_objProcessDatabaseHistoryDelete)
            {
                m_objProcessDatabaseHistoryDelete.DeInitialize();
            }
            // Process Monitor SVID 해제
            if (null != m_objProcessDatabaseMonitorSVID)
            {
                m_objProcessDatabaseMonitorSVID.DeInitialize();
            }
            // History Alarm
            if (null != m_objManagerTableHistoryAlarm)
            {
                m_objManagerTableHistoryAlarm.HLDeInitialize();
            }
            // History OP Call
            if (null != m_objManagerTableHistoryOPCall)
            {
                m_objManagerTableHistoryOPCall.HLDeInitialize();
            }
            // History Interlock Call
            if (null != m_objManagerTableHistoryInterlockCall)
            {
                m_objManagerTableHistoryInterlockCall.HLDeInitialize();
            }
            // History Terminal Call
            if (null != m_objManagerTableHistoryTerminalCall)
            {
                m_objManagerTableHistoryTerminalCall.HLDeInitialize();
            }
            // History Process Data Cell
            if (null != m_objManagerTableHistoryProcessDataCell)
            {
                m_objManagerTableHistoryProcessDataCell.HLDeInitialize();
            }
            // History Process Data Judgement
            if (null != m_objManagerTableHistoryProcessDataJudgement)
            {
                m_objManagerTableHistoryProcessDataJudgement.HLDeInitialize();
            }
            // History Process Data Reader
            if (null != m_objManagerTableHistoryProcessDataReader)
            {
                m_objManagerTableHistoryProcessDataReader.HLDeInitialize();
            }
            // History Process Data Vision Result
            if (null != m_objManagerTableHistoryProcessDataVisionResult)
            {
                m_objManagerTableHistoryProcessDataVisionResult.HLDeInitialize();
            }
            // History Process Data Work Order
            if (null != m_objManagerTableHistoryProcessDataWorkOrder)
            {
                m_objManagerTableHistoryProcessDataWorkOrder.HLDeInitialize();
            }
            // History Cell Track In
            if (null != m_objManagerTableHistoryCellTrackIn)
            {
                m_objManagerTableHistoryCellTrackIn.HLDeInitialize();
            }
            // History Cell Track Out
            if (null != m_objManagerTableHistoryCellTrackOut)
            {
                m_objManagerTableHistoryCellTrackOut.HLDeInitialize();
            }
            // History Cell Input
            if (null != m_objManagerTableHistoryCellInput)
            {
                m_objManagerTableHistoryCellInput.HLDeInitialize();
            }
            // History Cell Output
            if (null != m_objManagerTableHistoryCellOutput)
            {
                m_objManagerTableHistoryCellOutput.HLDeInitialize();
            }
            // History MCR Status
            if (null != m_objManagerTableHistoryMCRStatus)
            {
                m_objManagerTableHistoryMCRStatus.HLDeInitialize();
            }
            // History Machine Result
            if (null != m_objManagerTableHistoryMachineResult)
            {
                m_objManagerTableHistoryMachineResult.HLDeInitialize();
            }
            // History Alarm Event
            if (null != m_objManagerTableHistoryAlarmEvent)
            {
                m_objManagerTableHistoryAlarmEvent.HLDeInitialize();
            }
            // History Equipment Stop Event
            if (null != m_objManagerTableHistoryEquipmentStopEvent)
            {
                m_objManagerTableHistoryEquipmentStopEvent.HLDeInitialize();
            }
            // History Equipment TP Code Event
            if (null != m_objManagerTableHistoryEquipmentTPCodeEvent)
            {
                m_objManagerTableHistoryEquipmentTPCodeEvent.HLDeInitialize();
            }
            // History Upper Equipment Loss Time Event
            if (null != m_objManagerTableHistoryEquipmentUpperLossTimeEvent)
            {
                m_objManagerTableHistoryEquipmentUpperLossTimeEvent.HLDeInitialize();
            }
            // History Lower Equipment Loss Time Event
            if (null != m_objManagerTableHistoryEquipmentLowerLossTimeEvent)
            {
                m_objManagerTableHistoryEquipmentLowerLossTimeEvent.HLDeInitialize();
            }
            // History Safety PLC Modified Event
            if (null != m_objManagerTableHistorySafetyPlcModifiedEvent)
            {
                m_objManagerTableHistorySafetyPlcModifiedEvent.HLDeInitialize();
            }
            if (null != m_objSQLite)
            {
                // SQLite Disconnect
                m_objSQLite.HLDisconnect();
                // SQLite 해제
                m_objSQLite.HLDeInitialize();
            }
        }

        public string GetLastTpmLossCode()
        {
            System.Data.DataTable readData = new System.Data.DataTable();
            string tableName = m_objManagerTableHistoryEquipmentTPCodeEvent.HLGetTableName();
            string orderByColName = m_objManagerTableHistoryEquipmentTPCodeEvent.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryEquipmentTPCodeEvent.DATE];
            if (m_objSQLite.HLReload($"SELECT * FROM {tableName} ORDER BY {orderByColName} DESC LIMIT 1", ref readData).m_bError == true)
            {
                return string.Empty;
            }
            if (readData.Rows.Count == 0)
            {
                return string.Empty;
            }
            return Convert.ToString(readData.Rows[0][(int)CDatabaseDefine.EHistoryEquipmentTPCodeEvent.TP_CODE]);
        }
    }
}
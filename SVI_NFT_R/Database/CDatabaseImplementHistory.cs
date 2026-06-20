using System;
using System.Text;

namespace SVI_NFT_R
{
    public class CDatabaseImplementHistory : CDatabaseImplementAbstract
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDatabaseImplementHistory(CDocument objDocument)
        {
            base.m_objDocument = objDocument;
        }

        /// <summary>
        /// 알람 히스토리 삽입
        /// </summary>
        /// <param name="objAlarm"></param>
        public override void SetInsertHistoryAlarm(object objAlarm)
        {
            CDatabaseSendMessage.CHistoryAlarm obj = objAlarm as CDatabaseSendMessage.CHistoryAlarm;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarm;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / int id
                    strQuery = string.Format("insert into {0} values ('{1}',{2})", objManagerTable.HLGetTableName(), obj.m_strAlarmDate, obj.m_iAlarmID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 오퍼레이션 콜 히스토리 삽입
        /// </summary>
        /// <param name="objOPCall"></param>
        public override void SetInsertHistoryOPCall(object objOPCall)
        {
            CDatabaseSendMessage.CHistoryOPCall obj = objOPCall as CDatabaseSendMessage.CHistoryOPCall;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryOPCall;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string id / string message
                    strQuery = string.Format("insert into {0} values ('{1}','{2}','{3}')", objManagerTable.HLGetTableName(), obj.m_strDate, obj.m_strID, obj.m_strMessage);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 인터락 콜 히스토리 삽입
        /// </summary>
        /// <param name="objInterlockCall"></param>
        public override void SetInsertHistoryInterlockCall(object objInterlockCall)
        {
            CDatabaseSendMessage.CHistoryInterlockCall obj = objInterlockCall as CDatabaseSendMessage.CHistoryInterlockCall;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryInterlockCall;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string id / string message
                    strQuery = string.Format("insert into {0} values ('{1}','{2}','{3}')", objManagerTable.HLGetTableName(), obj.m_strDate, obj.m_strID, obj.m_strMessage);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 터미널 콜 히스토리 삽입
        /// </summary>
        /// <param name="objTerminalCall"></param>
        public override void SetInsertHistoryTerminalCall(object objTerminalCall)
        {
            CDatabaseSendMessage.CHistoryTerminalCall obj = objTerminalCall as CDatabaseSendMessage.CHistoryTerminalCall;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryTerminalCall;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string id / string message
                    strQuery = string.Format("insert into {0} values ('{1}','{2}','{3}')", objManagerTable.HLGetTableName(), obj.m_strDate, obj.m_strID, obj.m_strMessage);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 셀 투입 히스토리 삽입
        /// </summary>
        /// <param name="objCellInput"></param>
        public override void SetInsertHistoryCellInput(object objCellInput)
        {
            CDatabaseSendMessage.CHistoryCellInput obj = objCellInput as CDatabaseSendMessage.CHistoryCellInput;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellInput;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string strinnerid / string cellid
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}', '{3}')", objManagerTable.HLGetTableName(),
                        DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), obj.m_strInnerID, obj.m_strCellID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 셀 배출 히스토리 삽입
        /// </summary>
        /// <param name="objCellOutput"></param>
        public override void SetInsertHistoryCellOutput(object objCellOutput)
        {
            CDatabaseSendMessage.CHistoryCellOutput obj = objCellOutput as CDatabaseSendMessage.CHistoryCellOutput;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellOutput;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string strinnerid / string cellid
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}', '{3}')", objManagerTable.HLGetTableName(),
                        DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), obj.m_strInnerID, obj.m_strCellID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 셀 트랙 인 히스토리 삽입
        /// </summary>
        /// <param name="objCellTrackIn"></param>
        public override void SetInsertHistoryCellTrackIn(object objCellTrackIn)
        {
            CDatabaseSendMessage.CHistoryCellTrackIn obj = objCellTrackIn as CDatabaseSendMessage.CHistoryCellTrackIn;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellTrackIn;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string strinnerid / string cellid
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}', '{3}')", objManagerTable.HLGetTableName(),
                        DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), obj.m_strInnerID, obj.m_strCellID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 셀 트랙 아웃 히스토리 삽입
        /// </summary>
        /// <param name="objCellTrackOut"></param>
        public override void SetInsertHistoryCellTrackOut(object objCellTrackOut)
        {
            CDatabaseSendMessage.CHistoryCellTrackOut obj = objCellTrackOut as CDatabaseSendMessage.CHistoryCellTrackOut;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellTrackOut;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string strinnerid / string cellid
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}', '{3}')", objManagerTable.HLGetTableName(),
                        DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), obj.m_strInnerID, obj.m_strCellID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// MCR 히스토리 삽입
        /// </summary>
        /// <param name="objMCRStatus"></param>
        public override void SetInsertHistoryMCRStatus(object objMCRStatus)
        {
            CDatabaseSendMessage.CHistoryMCRStatus obj = objMCRStatus as CDatabaseSendMessage.CHistoryMCRStatus;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMCRStatus;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string mcrid / string result
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}', '{3}')",
                        objManagerTable.HLGetTableName(),
                        DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), obj.m_strMCRID, obj.m_eMCRResult.ToString());
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// Machine Result 히스토리 삽입
        /// </summary>
        /// <param name="objMachineResult"></param>
        public override void SetInsertHistoryMachineResult(object objMachineResult)
        {
            CDatabaseSendMessage.CHistoryMachineResult obj = objMachineResult as CDatabaseSendMessage.CHistoryMachineResult;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMachineResult;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string innerid / string result
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}', '{3}')",
                        objManagerTable.HLGetTableName(),
                        DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), obj.m_strInnerID, obj.m_strMachineResult);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 알람 이벤트 삽입
        /// </summary>
        /// <param name="objAlarmEvent"></param>
        public override void SetInsertHistoryAlarmEvent(object objAlarmEvent)
        {
            CDatabaseSendMessage.CHistoryAlarmEvent obj = objAlarmEvent as CDatabaseSendMessage.CHistoryAlarmEvent;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / int event
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}')", objManagerTable.HLGetTableName(),
                        obj.m_strDate, obj.m_eEventValue.ToString());
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 설비 정지 이벤트 삽입
        /// </summary>
        /// <param name="objEquipmentStopEvent"></param>
        public override void SetInsertHistoryEquipmentStopEvent(object objEquipmentStopEvent)
        {
            CDatabaseSendMessage.CHistoryEquipementStopEvent obj = objEquipmentStopEvent as CDatabaseSendMessage.CHistoryEquipementStopEvent;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentStopEvent;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / int event
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}')", objManagerTable.HLGetTableName(),
                        obj.m_strDate, obj.m_eEventValue.ToString());
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 설비 TP CODE 이벤트 삽입
        /// </summary>
        /// <param name="objEquipmentTPCodeEvent"></param>
        public override void SetInsertHistoryEquipmentTPCodeEvent(object objEquipmentTPCodeEvent)
        {
            CDatabaseSendMessage.CHistoryEquipementTPCodeEvent obj = objEquipmentTPCodeEvent as CDatabaseSendMessage.CHistoryEquipementTPCodeEvent;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentTPCodeEvent;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / string tp code
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}')", objManagerTable.HLGetTableName(),
                        obj.m_strDate, obj.m_strTPCode);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// Upper 설비 지연 시간 이벤트 삽입
        /// </summary>
        /// <param name="objEquipmentUpperLossTimeEvent"></param>
        public override void SetInsertHistoryEquipmentUpperLossTimeEvent(object objEquipmentUpperLossTimeEvent)
        {
            CDatabaseSendMessage.CHistoryEquipmentUpperLossTimeEvent obj = objEquipmentUpperLossTimeEvent as CDatabaseSendMessage.CHistoryEquipmentUpperLossTimeEvent;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentUpperLossTimeEvent;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / int event
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}')", objManagerTable.HLGetTableName(),
                        obj.m_strDate, obj.m_eEventValue.ToString());
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// Lower 설비 지연 시간 이벤트 삽입
        /// </summary>
        /// <param name="objEquipmentLowerLossTimeEvent"></param>
        public override void SetInsertHistoryEquipmentLowerLossTimeEvent(object objEquipmentLowerLossTimeEvent)
        {
            CDatabaseSendMessage.CHistoryEquipmentLowerLossTimeEvent obj = objEquipmentLowerLossTimeEvent as CDatabaseSendMessage.CHistoryEquipmentLowerLossTimeEvent;

            string strQuery = null;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / int event
                    strQuery = string.Format("insert into {0} values ('{1}', '{2}')", objManagerTable.HLGetTableName(),
                        obj.m_strDate, obj.m_eEventValue.ToString());
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 세이프티PLC 수정 이벤트 삽입
        /// </summary>
        /// <param name="objSafetyPlcModifiedEvent"></param>
        public override void SetInsertHistorySafetyPlcModifiedEvent(object objSafetyPlcModifiedEvent)
        {
            CDatabaseSendMessage.CHistorySafetyPlcModifiedEvent obj = objSafetyPlcModifiedEvent as CDatabaseSendMessage.CHistorySafetyPlcModifiedEvent;
            CManagerTable objManagerTable = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistorySafetyPlcModifiedEvent;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // string date / int event
                    string strQuery = string.Format("insert into {0} values ('{1}', '{2}', '{3}')", objManagerTable.HLGetTableName(),
                        obj.LastModifiedTime,
                        obj.ID,
                        obj.SignatureCode
                        );
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(strQuery);
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// Process Data Cell 삽입
        /// </summary>
        /// <param name="objProcessData"></param>
        public override void SetInsertHistoryProcessData(object objProcessData)
        {
            CDatabaseSendMessage.CHistoryProcessData obj = objProcessData as CDatabaseSendMessage.CHistoryProcessData;

            var sbQuery = new StringBuilder();
            string strDate = DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT);
            CManagerTable objManagerTableCell = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataCell;
            CManagerTable objManagerTableJudgement = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataJudgement;
            CManagerTable objManagerTableReader = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataReader;
            CManagerTable objManagerTableVisionResult = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataVisionResult;
            CManagerTable objManagerTableWorkOrder = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataWorkOrder;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // Process Data Cell
                    // string date / string innerid / string cellID / string eqtoeqid / string carrierID / string productID / string stepID / string productType / string productKind / string productSpec
                    // double cellSize / double cellThickness / string comment / string cellInfoResult / string replyStatus
                    // string chname / string eqresult / string cimresult / string machineresult / int aligncount / string visionresultfront / string visionresultback
                    sbQuery.Clear();
                    sbQuery.AppendFormat("insert into {0} values (", objManagerTableCell.HLGetTableName());
                    sbQuery.AppendFormat("'{0}',", strDate);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.InnerID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.EqInterface.CellID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.CarrierID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.JobID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.ProductID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.StepID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.ProductType);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.ProductKind);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.ProductSpec);
                    sbQuery.AppendFormat("'{0:F3}',", obj.m_objData.Cim.CellSize);
                    sbQuery.AppendFormat("'{0:F3}',", obj.m_objData.Cim.CellThickness);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.Comment);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.CellInformationResult);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.ReplyStatus);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.CellLotInformationLotInfoResult);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.CellLotInformationDefectResult);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cim.CellLotInformationDefectComent);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.ChannelName);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.EqInterface.Judge);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Judgement.Judge);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.MachineResult);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.PreAlignResult.RetryCount);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.FrontInspResult);
                    sbQuery.AppendFormat("'{0}',", string.Join(",", obj.m_objData.Cell.FrontInspReasonCodes));
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.RearInspResult);
                    sbQuery.AppendFormat("'{0}',", string.Join(",", obj.m_objData.Cell.RearInspReasonCodes));
                    sbQuery.AppendFormat("'{0}')", obj.m_objData.Cell.PPID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                    // Process Data Judgement
                    // string date / string innerid / string cellId / string operatorId / string judge / string reasonCode / strnig description
                    sbQuery.Clear();
                    sbQuery.AppendFormat("insert into {0} values (", objManagerTableJudgement.HLGetTableName());
                    sbQuery.AppendFormat("'{0}',", strDate);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.InnerID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Judgement.OperatorID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Judgement.Judge);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Judgement.ReasonCode);
                    sbQuery.AppendFormat("'{0}')", obj.m_objData.Judgement.Description);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                    // Process Data Reader
                    // string date / string innerid / string cellId / string readerId / string readerResultCode
                    sbQuery.Clear();
                    sbQuery.AppendFormat("insert into {0} values (", objManagerTableReader.HLGetTableName());
                    sbQuery.AppendFormat("'{0}',", strDate);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.InnerID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Reader.ReaderID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Reader.ReaderResultCode);
                    sbQuery.AppendFormat("'{0}')", obj.m_objData.Reader.RetryCount);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                    // Process Data Vision Result
                    // string date / string innerid / string cellId / double revisionX / double revisionY / double revisionT / double totalRevisionX / double totalRevisionY
                    // double totalRevisionT / int result / string resultImagePath
                    sbQuery.Clear();
                    sbQuery.AppendFormat("insert into {0} values (", objManagerTableVisionResult.HLGetTableName());
                    sbQuery.AppendFormat("'{0}',", strDate);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.InnerID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("'{0:F3}',", obj.m_objData.PreAlignResult.RevisionX);
                    sbQuery.AppendFormat("'{0:F3}',", obj.m_objData.PreAlignResult.RevisionY);
                    sbQuery.AppendFormat("'{0:F3}',", obj.m_objData.PreAlignResult.RevisionT);
                    sbQuery.AppendFormat("'{0:F3}',", obj.m_objData.PreAlignResult.TotalRevisionX);
                    sbQuery.AppendFormat("'{0:F3}',", obj.m_objData.PreAlignResult.TotalRevisionY);
                    sbQuery.AppendFormat("'{0:F3}',", obj.m_objData.PreAlignResult.TotalRevisionT);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.PreAlignResult.Judge);
                    sbQuery.AppendFormat("'{0}')", obj.m_objData.PreAlignResult.ResultImagePath);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                    // Process Data Work Order
                    // string date / string innerid / string cellId / string processJob / int planQty / int processedQty
                    sbQuery.Clear();
                    sbQuery.AppendFormat("insert into {0} values (", objManagerTableWorkOrder.HLGetTableName());
                    sbQuery.AppendFormat("'{0}',", strDate);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.InnerID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("'{0}',", obj.m_objData.WorkOrder.ProcessJob);
                    sbQuery.AppendFormat("'{0:D}',", obj.m_objData.WorkOrder.PlanQty);
                    sbQuery.AppendFormat("'{0:D}')", obj.m_objData.WorkOrder.ProcessedQty);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// Process Data Cell 수정
        /// </summary>
        /// <param name="objProcessData"></param>
        public override void SetUpdateHistoryProcessData(object objProcessData)
        {
            CDatabaseSendMessage.CHistoryProcessData obj = objProcessData as CDatabaseSendMessage.CHistoryProcessData;

            var sbQuery = new StringBuilder();
            string strDate = DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT);
            CManagerTable objManagerTableCell = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataCell;
            CManagerTable objManagerTableJudgement = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataJudgement;
            CManagerTable objManagerTableReader = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataReader;
            CManagerTable objManagerTableVisionResult = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataVisionResult;
            CManagerTable objManagerTableWorkOrder = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataWorkOrder;
            lock (m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
            {
                // 트랜잭션 시작
                //var objTransaction = m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                try
                {
                    // Process Data Cell
                    // string date / string innerid / string cellID / string eqtoeqid / string carrierID / string productID / string stepID / string productType / string productKind / string productSpec
                    // double cellSize / double cellThickness / string comment / string cellInfoResult / string replyStatus
                    // string chname / string cimresult / string machineresult / int aligncount / string visionresultfront / string visionresultback
                    sbQuery.Clear();
                    var schemaNames = objManagerTableCell.HLGetTableSchemaName();
                    sbQuery.AppendFormat("update {0} set ", objManagerTableCell.HLGetTableName());
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.DATE], strDate);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_ID], obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.EQ_TO_EQ_ID], obj.m_objData.EqInterface.CellID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CARRIER_ID], obj.m_objData.Cell.CarrierID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.JOB_ID], obj.m_objData.Cell.JobID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.PRODUCT_ID], obj.m_objData.Cell.ProductID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.STEP_ID], obj.m_objData.Cell.StepID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.PRODUCT_TYPE], obj.m_objData.Cim.ProductType);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.PRODUCT_KIND], obj.m_objData.Cim.ProductKind);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.PRODUCT_SPEC], obj.m_objData.Cim.ProductSpec);
                    sbQuery.AppendFormat("{0} = {1:F3},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_SIZE], obj.m_objData.Cim.CellSize);
                    sbQuery.AppendFormat("{0} = {1:F3},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_THICKNESS], obj.m_objData.Cim.CellThickness);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.COMMENT], obj.m_objData.Cim.Comment);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_INFO_RESULT], obj.m_objData.Cim.CellInformationResult);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.REPLY_STATUS], obj.m_objData.Cim.ReplyStatus);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_LOT_INFORMAITON_LOTINFO_RESULT], obj.m_objData.Cim.CellLotInformationLotInfoResult);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_LOT_INFORMAITON_DEFECT_RESULT], obj.m_objData.Cim.CellLotInformationDefectResult);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CELL_LOT_INFORMAITON_DEFECT_COMENT], obj.m_objData.Cim.CellLotInformationDefectComent);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CH_NAME], obj.m_objData.Cell.ChannelName);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.EQ_TO_EQ_RESULT], obj.m_objData.EqInterface.Judge);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.CIM_RESULT], obj.m_objData.Judgement.Judge);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.MACHINE_RESULT], obj.m_objData.Cell.MachineResult);
                    sbQuery.AppendFormat("{0} = {1:D},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.ALIGN_COUNT], obj.m_objData.PreAlignResult.RetryCount);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.FRONT_INSP_RESULT], obj.m_objData.Cell.FrontInspResult);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.FRONT_INSP_REASONCODES], string.Join(",", obj.m_objData.Cell.FrontInspReasonCodes));
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.REAR_INSP_RESULT], obj.m_objData.Cell.RearInspResult);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.REAR_INSP_REASONCODES], string.Join(",", obj.m_objData.Cell.RearInspReasonCodes));
                    sbQuery.AppendFormat("{0} = '{1}' ", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.PPID], obj.m_objData.Cell.PPID);
                    sbQuery.AppendFormat("where {0} = '{1}';", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataCell.INNER_ID], obj.m_objData.Cell.InnerID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                    // Process Data Judgement
                    // string date / string innerid / string cellId / string operatorId / string judge / string reasonCode / strnig description
                    sbQuery.Clear();
                    schemaNames = objManagerTableJudgement.HLGetTableSchemaName();
                    sbQuery.AppendFormat("update {0} set ", objManagerTableJudgement.HLGetTableName());
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataJudgement.DATE], strDate);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataJudgement.CELL_ID], obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataJudgement.OPERATOR_ID], obj.m_objData.Judgement.OperatorID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataJudgement.JUDGE], obj.m_objData.Judgement.Judge);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataJudgement.REASON_CODE], obj.m_objData.Judgement.ReasonCode);
                    sbQuery.AppendFormat("{0} = '{1}' ", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataJudgement.DESCRIPTION], obj.m_objData.Judgement.Description);
                    sbQuery.AppendFormat("where {0} = '{1}';", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataJudgement.INNER_ID], obj.m_objData.Cell.InnerID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                    // Process Data Reader
                    // string date / string innerid / string cellId / string readerId / string readerResultCode
                    sbQuery.Clear();
                    schemaNames = objManagerTableReader.HLGetTableSchemaName();
                    sbQuery.AppendFormat("update {0} set ", objManagerTableReader.HLGetTableName());
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataReader.DATE], strDate);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataReader.CELL_ID], obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataReader.READER_ID], obj.m_objData.Reader.ReaderID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataReader.READER_RESULT_CODE], obj.m_objData.Reader.ReaderResultCode);
                    sbQuery.AppendFormat("{0} = '{1}' ", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataReader.RETRY_COUNT], obj.m_objData.Reader.RetryCount);
                    sbQuery.AppendFormat("where {0} = '{1}';", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataReader.INNER_ID], obj.m_objData.Cell.InnerID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                    // Process Data Vision Result
                    // string date / string innerid / string cellId / double revisionX / double revisionY / double revisionT / double totalRevisionX / double totalRevisionY
                    // double totalRevisionT / int result / string resultImagePath
                    sbQuery.Clear();
                    schemaNames = objManagerTableVisionResult.HLGetTableSchemaName();
                    sbQuery.AppendFormat("update {0} set ", objManagerTableVisionResult.HLGetTableName());
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.DATE], strDate);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.CELL_ID], obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("{0} = {1:F3},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.REVISION_X], obj.m_objData.PreAlignResult.RevisionX);
                    sbQuery.AppendFormat("{0} = {1:F3},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.REVISION_Y], obj.m_objData.PreAlignResult.RevisionY);
                    sbQuery.AppendFormat("{0} = {1:F3},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.REVISION_T], obj.m_objData.PreAlignResult.RevisionT);
                    sbQuery.AppendFormat("{0} = {1:F3},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.TOTAL_REVISION_X], obj.m_objData.PreAlignResult.TotalRevisionX);
                    sbQuery.AppendFormat("{0} = {1:F3},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.TOTAL_REVISION_Y], obj.m_objData.PreAlignResult.TotalRevisionY);
                    sbQuery.AppendFormat("{0} = {1:F3},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.TOTAL_REVISION_T], obj.m_objData.PreAlignResult.TotalRevisionT);
                    sbQuery.AppendFormat("{0} = {1:D},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.RESULT], (int)obj.m_objData.PreAlignResult.Judge);
                    sbQuery.AppendFormat("{0} = '{1}' ", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.RESULT_IMAGE_PATH], obj.m_objData.PreAlignResult.ResultImagePath);
                    sbQuery.AppendFormat("where {0} = '{1}';", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataVisionResult.INNER_ID], obj.m_objData.Cell.InnerID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                    // Process Data Work Order
                    // string date / string innerid / string cellId / string processJob / int planQty / int processedQty
                    sbQuery.Clear();
                    schemaNames = objManagerTableWorkOrder.HLGetTableSchemaName();
                    sbQuery.AppendFormat("update {0} set ", objManagerTableWorkOrder.HLGetTableName());
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.DATE], strDate);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.CELL_ID], obj.m_objData.Cell.CellID);
                    sbQuery.AppendFormat("{0} = '{1}',", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.PROCESS_JOB], obj.m_objData.WorkOrder.ProcessJob);
                    sbQuery.AppendFormat("{0} = {1:D},", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.PLAN_QTY], obj.m_objData.WorkOrder.PlanQty);
                    sbQuery.AppendFormat("{0} = {1:D} ", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.PROCESSED_QTY], obj.m_objData.WorkOrder.ProcessedQty);
                    sbQuery.AppendFormat("where {0} = '{1}';", schemaNames[(int)CDatabaseDefine.EHistoryProcessDataWorkOrder.INNER_ID], obj.m_objData.Cell.InnerID);
                    m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(sbQuery.ToString());
                }
                finally
                {
                    // 커밋
                    //m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(objTransaction);
                    //objTransaction.Dispose();
                }
            }
        }
    }
}
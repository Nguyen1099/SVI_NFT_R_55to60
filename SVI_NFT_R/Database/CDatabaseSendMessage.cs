using System;
using System.Collections.Concurrent;
using System.Data.SQLite;
using System.Threading;

namespace SVI_NFT_R
{
    public class CDatabaseSendMessage
    {
        private CDatabaseImplementAbstract m_objDatabaseImplement;

        #region 알람 데이터 클래스
        public class CHistoryAlarm
        {
            /// <summary>
            /// 발생 시간
            /// </summary>
            public string m_strAlarmDate;
            /// <summary>
            /// 알람 코드
            /// </summary>
            public int m_iAlarmID;

            public CHistoryAlarm(string strAlarmDate, int iAlarmID)
            {
                m_strAlarmDate = strAlarmDate;
                m_iAlarmID = iAlarmID;
            }
        }
        #endregion

        #region 오퍼레이션 콜 데이터 클래스
        public class CHistoryOPCall
        {
            /// <summary>발생 시간</summary>
            public string m_strDate;
            /// <summary>ID</summary>
            public string m_strID;
            /// <summary>메세지</summary>
            public string m_strMessage;

            public CHistoryOPCall(string strDate, string strID, string strMessage)
            {
                m_strDate = strDate;
                m_strID = strID;
                m_strMessage = strMessage;
            }
        }
        #endregion

        #region 인터락 콜 데이터 클래스
        public class CHistoryInterlockCall
        {
            /// <summary>발생 시간</summary>
            public string m_strDate;
            /// <summary>ID</summary>
            public string m_strID;
            /// <summary>메세지</summary>
            public string m_strMessage;

            public CHistoryInterlockCall(string strDate, string strID, string strMessage)
            {
                m_strDate = strDate;
                m_strID = strID;
                m_strMessage = strMessage;
            }
        }
        #endregion

        #region 터미널 콜 데이터 클래스
        public class CHistoryTerminalCall
        {
            /// <summary>발생 시간</summary>
            public string m_strDate;
            /// <summary>ID</summary>
            public string m_strID;
            /// <summary>메세지</summary>
            public string m_strMessage;

            public CHistoryTerminalCall(string strDate, string strID, string strMessage)
            {
                m_strDate = strDate;
                m_strID = strID;
                m_strMessage = strMessage;
            }
        }
        #endregion

        #region 셀 투입 데이터 클래스
        public class CHistoryCellInput
        {
            /// <summary>내부 아이디</summary>
            public string m_strInnerID;
            /// <summary>셀 아이디</summary>
            public string m_strCellID;

            public CHistoryCellInput(string strInnerID, string strCellID)
            {
                m_strInnerID = strInnerID;
                m_strCellID = strCellID;
            }
        }
        #endregion

        #region 셀 배출 데이터 클래스
        public class CHistoryCellOutput
        {
            /// <summary>내부 아이디</summary>
            public string m_strInnerID;
            /// <summary>셀 아이디</summary>
            public string m_strCellID;

            public CHistoryCellOutput(string strInnerID, string strCellID)
            {
                m_strInnerID = strInnerID;
                m_strCellID = strCellID;
            }
        }
        #endregion

        #region 셀 트랙 인 데이터 클래스
        public class CHistoryCellTrackIn
        {
            /// <summary>내부 아이디</summary>
            public string m_strInnerID;
            /// <summary>셀 아이디</summary>
            public string m_strCellID;

            public CHistoryCellTrackIn(string strInnerID, string strCellID)
            {
                m_strInnerID = strInnerID;
                m_strCellID = strCellID;
            }
        }
        #endregion

        #region 셀 트랙 아웃 데이터 클래스
        public class CHistoryCellTrackOut
        {
            /// <summary>내부 아이디</summary>
            public string m_strInnerID;
            /// <summary>셀 아이디</summary>
            public string m_strCellID;

            public CHistoryCellTrackOut(string strInnerID, string strCellID)
            {
                m_strInnerID = strInnerID;
                m_strCellID = strCellID;
            }
        }
        #endregion

        #region MCR 데이터 클래스
        public class CHistoryMCRStatus
        {
            /// <summary>MCR 아이디</summary>
            public string m_strMCRID;
            /// <summary>MCR 결과</summary>
            public CDatabaseDefine.EMcrResult m_eMCRResult;

            public CHistoryMCRStatus(string strMCRID, CDatabaseDefine.EMcrResult eMCRResult)
            {
                m_strMCRID = strMCRID;
                m_eMCRResult = eMCRResult;
            }
        }
        #endregion

        #region Machine Result 데이터 클래스
        public class CHistoryMachineResult
        {
            /// <summary>내부 아이디</summary>
            public string m_strInnerID;
            /// <summary>설비 결과</summary>
            public string m_strMachineResult;

            public CHistoryMachineResult(string strInnerID, string strMachineResult)
            {
                m_strInnerID = strInnerID;
                m_strMachineResult = strMachineResult;
            }
        }
        #endregion

        #region 알람 이벤트 데이터 클래스
        public class CHistoryAlarmEvent
        {
            /// <summary>발생 시간</summary>
            public string m_strDate;
            /// <summary>이벤트 값</summary>
            public CDatabaseDefine.EEventValue m_eEventValue;

            public CHistoryAlarmEvent(string strDate, CDatabaseDefine.EEventValue eEventValue)
            {
                m_strDate = strDate;
                m_eEventValue = eEventValue;
            }
        }
        #endregion

        #region 설비 정지 이벤트 데이터 클래스
        public class CHistoryEquipementStopEvent
        {
            /// <summary>발생 시간</summary>
            public string m_strDate;
            /// <summary>이벤트 값</summary>
            public CDatabaseDefine.EEventValue m_eEventValue;

            public CHistoryEquipementStopEvent(string strDate, CDatabaseDefine.EEventValue eEventValue)
            {
                m_strDate = strDate;
                m_eEventValue = eEventValue;
            }
        }
        #endregion

        #region 설비 TP CODE 이벤트 데이터 클래스
        public class CHistoryEquipementTPCodeEvent
        {
            /// <summary>발생 시간</summary>
            public string m_strDate;
            /// <summary>TP CODE</summary>
            public string m_strTPCode;

            public CHistoryEquipementTPCodeEvent(string strDate, string strTPCode)
            {
                m_strDate = strDate;
                m_strTPCode = strTPCode;
            }
        }
        #endregion

        #region Upper 설비 지연 시간 이벤트 데이터 클래스
        public class CHistoryEquipmentUpperLossTimeEvent
        {
            /// <summary>발생 시간</summary>
            public string m_strDate;
            /// <summary>이벤트 값</summary>
            public CDatabaseDefine.EEventValue m_eEventValue;

            public CHistoryEquipmentUpperLossTimeEvent(string strDate, CDatabaseDefine.EEventValue eEventValue)
            {
                m_strDate = strDate;
                m_eEventValue = eEventValue;
            }
        }
        #endregion

        #region Lower 설비 지연 시간 이벤트 데이터 클래스
        public class CHistoryEquipmentLowerLossTimeEvent
        {
            /// <summary>발생 시간</summary>
            public string m_strDate;
            /// <summary>이벤트 값</summary>
            public CDatabaseDefine.EEventValue m_eEventValue;

            public CHistoryEquipmentLowerLossTimeEvent(string strDate, CDatabaseDefine.EEventValue eEventValue)
            {
                m_strDate = strDate;
                m_eEventValue = eEventValue;
            }
        }
        #endregion

        #region 세이프티PLC 수정 이벤트 데이터 클래스
        public class CHistorySafetyPlcModifiedEvent
        {
            public string LastModifiedTime;
            public string ID;
            public string SignatureCode;

            public CHistorySafetyPlcModifiedEvent(string lastModifiedTime, string id, string signatureCode)
            {
                LastModifiedTime = lastModifiedTime;
                ID = id;
                SignatureCode = signatureCode;
            }
        }
        #endregion

        #region Process Data 데이터 클래스
        public class CHistoryProcessData
        {
            /// <summary>
            /// 셀 정의된 구조체 데이터
            /// </summary>
            public CellData.OneCell m_objData;

            public CHistoryProcessData(CellData.OneCell objData)
            {
                m_objData = objData;
            }
        }
        #endregion

        #region Process Data Cell 데이터 클래스
        public class CHistoryProcessDataCell
        {
            public CellData.OneCell m_objCell;

            public CHistoryProcessDataCell(CellData.OneCell objCell)
            {
                m_objCell = objCell;
            }
        }
        #endregion

        #region Process Data Judgement 데이터 클래스
        public class CHistoryProcessDataJudgement
        {
            public CellData.OneCell m_objJudgement;

            public CHistoryProcessDataJudgement(CellData.OneCell objJudgement)
            {
                m_objJudgement = objJudgement;
            }
        }
        #endregion

        #region Process Data Reader 데이터 클래스
        public class CHistoryProcessDataReader
        {
            public CellData.OneCell m_objReader;

            public CHistoryProcessDataReader(CellData.OneCell objReader)
            {
                m_objReader = objReader;
            }
        }
        #endregion

        #region Process Data Vision Pixel Position 데이터 클래스
        public class CHistoryProcessDataVisionPixelPosition
        {
            public CellData.OneCell m_objVisionPixelPosition;

            public CHistoryProcessDataVisionPixelPosition(CellData.OneCell objVisionPixelPosition)
            {
                m_objVisionPixelPosition = objVisionPixelPosition;
            }
        }
        #endregion

        #region Process Data Vision Result 데이터 클래스
        public class CHistoryProcessDataVisionResult
        {
            public CellData.OneCell m_objVisionResult;

            public CHistoryProcessDataVisionResult(CellData.OneCell objVisionResult)
            {
                m_objVisionResult = objVisionResult;
            }
        }
        #endregion

        #region Process Data Work Order 데이터 클래스
        public class CHistoryProcessDataWorkOrder
        {
            public CellData.OneCell m_objWorkOrder;

            public CHistoryProcessDataWorkOrder(CellData.OneCell objWorkOrder)
            {
                m_objWorkOrder = objWorkOrder;
            }
        }
        #endregion

        private volatile bool _shouldStop = false;
        private Thread _threadCommand;
        private ConcurrentQueue<EventItem> _queue = new ConcurrentQueue<EventItem>();
        public class EventItem
        {
            public Action<object> Method;
            public object Parameter;
            public EventItem(Action<object> method, object parameter)
            {
                Method = method;
                Parameter = parameter;
            }
        }

        /// <summary>
        /// 생성자 함수
        /// </summary>
        /// <param name="objDatabaseImplement"></param>
        /// <returns></returns>
        public CDatabaseSendMessage(CDatabaseImplementAbstract objDatabaseImplement)
        {
            m_objDatabaseImplement = objDatabaseImplement;
            _threadCommand = new Thread(new ThreadStart(RunCommand));
            _threadCommand.Start();

        }
        /// <summary>
        /// 해제 함수
        /// </summary>
        public void DeInitialize()
        {
            _shouldStop = true;
            _threadCommand.Join();
        }

        private void RunCommand()
        {
            //Thread.Sleep(1000);
            EventItem item;
            int commandCount = 0;
            SQLiteTransaction transaction = null;
            while (!_shouldStop)
            {
                try
                {
                    // 01. 큐가 비었다~
                    if (_queue.Count == 0)
                    {
                        if (commandCount > 0)
                        {
                            // EndTransaction;
                            lock (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
                            {
                                m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(transaction);
                            }
                            commandCount = 0;
                        }
                        Thread.Sleep(10);
                        continue;
                    }
                    // 02. 큐가 ... ... ... 전면 재검토 필요시점인듯..
                    if (commandCount > 100)
                    {
                        // EndTransaction;
                        lock (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
                        {
                            m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(transaction);
                        }
                        m_objDatabaseImplement.m_objDocument.SetUpdateLog
                            (CDefine.ELogType.LOG_ETC, string.Format("DatabaseSendMessage -> EndTransaction[{0}]", commandCount));

                        commandCount = 0;
                        Thread.Sleep(0);
                        continue;
                    }
                    // 02. 큐에는 데이터, Transaction은 시작전~
                    if (commandCount == 0)
                    {
                        //BeginTransaction
                        lock (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
                        {
                            transaction = m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                        }
                    }
                    bool result = _queue.TryDequeue(out item);
                    if (result)
                    {
                        item.Method(item.Parameter);
                        commandCount++;
                    }
                    else
                    {
                        m_objDatabaseImplement.m_objDocument.SetUpdateLog
                            (CDefine.ELogType.LOG_ETC, string.Format("DatabaseSendMessage -> Dequeue Fail!"));
                    }
                }
                catch (Exception ex)
                {
                    m_objDatabaseImplement.m_objDocument.SetUpdateLog
                        (CDefine.ELogType.LOG_ETC, string.Format("DatabaseSendMessage -> {0}", ex.Message));
                    continue;
                }
                Thread.Sleep(0);
            }
        }

        /// <summary>
        /// 알람 히스토리 삽입
        /// </summary>
        /// <param name="strAlarmDate"></param>
        /// <param name="iAlarmID"></param>
        /// <param name="alarmStatus"></param>
        public void SetInsertHistoryAlarm(string strAlarmDate, int iAlarmID)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryAlarm obj = new CHistoryAlarm(strAlarmDate, iAlarmID);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryAlarm, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryAlarm), obj);
        }

        /// <summary>
        /// 오퍼레이션 콜 히스토리 삽입
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strID"></param>
        /// <param name="strMessage"></param>
        public void SetInsertHistoryOPCall(string strDate, string strID, string strMessage)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryOPCall obj = new CHistoryOPCall(strDate, strID, strMessage);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryOPCall, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryOPCall), obj);
        }

        /// <summary>
        /// 인터락 콜 히스토리 삽입
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strID"></param>
        /// <param name="strMessage"></param>
        public void SetInsertHistoryInterlockCall(string strDate, string strID, string strMessage)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryInterlockCall obj = new CHistoryInterlockCall(strDate, strID, strMessage);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryInterlockCall, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryInterlockCall), obj);
        }

        /// <summary>
        /// 터미널 콜 히스토리 삽입
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strID"></param>
        /// <param name="strMessage"></param>
        public void SetInsertHistoryTerminalCall(string strDate, string strID, string strMessage)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryTerminalCall obj = new CHistoryTerminalCall(strDate, strID, strMessage);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryTerminalCall, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryTerminalCall), obj);
        }

        /// <summary>
        /// 셀 투입 히스토리 삽입
        /// </summary>
        /// <param name="strInnerID"></param>
        /// <param name="strCellID"></param>
        public void SetInsertHistoryCellInput(string strInnerID, string strCellID)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryCellInput obj = new CHistoryCellInput(strInnerID, strCellID);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryCellInput, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryCellInput), obj);
        }

        /// <summary>
        /// 셀 배출 히스토리 삽입
        /// </summary>
        /// <param name="strInnerID"></param>
        /// <param name="strCellID"></param>
        public void SetInsertHistoryCellOutput(string strInnerID, string strCellID)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryCellOutput obj = new CHistoryCellOutput(strInnerID, strCellID);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryCellOutput, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryCellOutput), obj);
        }

        /// <summary>
        /// 셀 트랙 인 히스토리 삽입
        /// </summary>
        /// <param name="strInnerID"></param>
        /// <param name="strCellID"></param>
        public void SetInsertHistoryCellTrackIn(string strInnerID, string strCellID)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryCellTrackIn obj = new CHistoryCellTrackIn(strInnerID, strCellID);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryCellTrackIn, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryCellTrackIn), obj);
        }

        /// <summary>
        /// 셀 트랙 아웃 히스토리 삽입
        /// </summary>
        /// <param name="strInnerID"></param>
        /// <param name="strCellID"></param>
        public void SetInsertHistoryCellTrackOut(string strInnerID, string strCellID)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryCellTrackOut obj = new CHistoryCellTrackOut(strInnerID, strCellID);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryCellTrackOut, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryCellTrackOut), obj);
        }

        /// <summary>
        /// MCR 히스토리 삽입
        /// </summary>
        /// <param name="strMCRID"></param>
        /// <param name="eMCRResult"></param>
        public void SetInsertHistoryMCRStatus(string strMCRID, CDatabaseDefine.EMcrResult eMCRResult)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryMCRStatus obj = new CHistoryMCRStatus(strMCRID, eMCRResult);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryMCRStatus, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryMCRStatus), obj);
        }

        /// <summary>
        /// Machine Result 히스토리 삽입
        /// </summary>
        /// <param name="strInnerID"></param>
        /// <param name="strMachineResult"></param>
        public void SetInsertHistoryMachineResult(string strInnerID, string strMachineResult)
        {
            if (m_objDatabaseImplement.m_objDocument.m_objProcessDatabase.IsInsertHistory() == false)
            {
                return;
            }

            CHistoryMachineResult obj = new CHistoryMachineResult(strInnerID, strMachineResult);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryMachineResult, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryMachineResult), obj);
        }

        /// <summary>
        /// 알람 이벤트 삽입
        /// </summary>
        /// <param name="objDate"></param>
        /// <param name="eEventValue"></param>
        public void SetInsertHistoryAlarmEvent(DateTime objDate, CDatabaseDefine.EEventValue eEventValue)
        {
            CHistoryAlarmEvent obj = new CHistoryAlarmEvent(objDate.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), eEventValue);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryAlarmEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryAlarmEvent), obj);
        }

        /// <summary>
        /// 알람 이벤트 삽입
        /// </summary>
        /// <param name="eEventValue"></param>
        public void SetInsertHistoryAlarmEvent(CDatabaseDefine.EEventValue eEventValue)
        {
            CHistoryAlarmEvent obj = new CHistoryAlarmEvent(DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), eEventValue);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryAlarmEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryAlarmEvent), obj);
        }

        /// <summary>
        /// 설비 정지 이벤트 삽입
        /// </summary>
        /// <param name="objDate"></param>
        /// <param name="eEventValue"></param>
        public void SetInsertHistoryEquipmentStopEvent(DateTime objDate, CDatabaseDefine.EEventValue eEventValue)
        {
            CHistoryEquipementStopEvent obj = new CHistoryEquipementStopEvent(objDate.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), eEventValue);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryEquipmentStopEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryEquipmentStopEvent), obj);
        }

        /// <summary>
        /// 설비 정지 이벤트 삽입
        /// </summary>
        /// <param name="eEventValue"></param>
        public void SetInsertHistoryEquipmentStopEvent(CDatabaseDefine.EEventValue eEventValue)
        {
            CHistoryEquipementStopEvent obj = new CHistoryEquipementStopEvent(DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), eEventValue);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryEquipmentStopEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryEquipmentStopEvent), obj);
        }

        /// <summary>
        /// 설비 TP CODE 이벤트 삽입
        /// </summary>
        /// <param name="strTPCode"></param>
        public void SetInsertHistoryEquipmentTPCodeEvent(string strTPCode)
        {
            CHistoryEquipementTPCodeEvent obj = new CHistoryEquipementTPCodeEvent(DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), strTPCode);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryEquipmentTPCodeEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryEquipmentTPCodeEvent), obj);
        }

        /// <summary>
        /// Lower 설비 지연 시간 이벤트 삽입
        /// </summary>
        /// <param name="objDate"></param>
        /// <param name="eEventValue"></param>
        public void SetInsertHistoryEquipmentLowerLossTimeEvent(DateTime objDate, CDatabaseDefine.EEventValue eEventValue)
        {
            CHistoryEquipmentLowerLossTimeEvent obj = new CHistoryEquipmentLowerLossTimeEvent(objDate.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), eEventValue);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryEquipmentLowerLossTimeEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryEquipmentLowerLossTimeEvent), obj);
        }

        /// <summary>
        /// Lower 설비 지연 시간 이벤트 삽입
        /// </summary>
        /// <param name="eEventValue"></param>
        public void SetInsertHistoryEquipmentLowerLossTimeEvent(CDatabaseDefine.EEventValue eEventValue)
        {
            CHistoryEquipmentLowerLossTimeEvent obj = new CHistoryEquipmentLowerLossTimeEvent(DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), eEventValue);
            ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryEquipmentLowerLossTimeEvent), obj);
        }

        /// <summary>
        /// Upper 설비 지연 시간 이벤트 삽입
        /// </summary>
        /// <param name="objDate"></param>
        /// <param name="eEventValue"></param>
        public void SetInsertHistoryEquipmentUpperLossTimeEvent(DateTime objDate, CDatabaseDefine.EEventValue eEventValue)
        {
            CHistoryEquipmentUpperLossTimeEvent obj = new CHistoryEquipmentUpperLossTimeEvent(objDate.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), eEventValue);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryEquipmentUpperLossTimeEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryEquipmentUpperLossTimeEvent), obj);
        }

        /// <summary>
        /// Upper 설비 지연 시간 이벤트 삽입
        /// </summary>
        /// <param name="eEventValue"></param>
        public void SetInsertHistoryEquipmentUpperLossTimeEvent(CDatabaseDefine.EEventValue eEventValue)
        {
            CHistoryEquipmentUpperLossTimeEvent obj = new CHistoryEquipmentUpperLossTimeEvent(DateTime.Now.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), eEventValue);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryEquipmentUpperLossTimeEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryEquipmentUpperLossTimeEvent), obj);
        }

        /// <summary>
        /// 세이프티PLC 수정 이벤트 삽입
        /// </summary>
        /// <param name="eEventValue"></param>
        public void SetInsertHistorySafetyPlcModifiedEvent(string id, DateTime lastModifiedTime, string signatureCode)
        {
            CHistorySafetyPlcModifiedEvent obj = new CHistorySafetyPlcModifiedEvent(lastModifiedTime.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), id, signatureCode);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistorySafetyPlcModifiedEvent, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryEquipmentUpperLossTimeEvent), obj);
        }

        /// <summary>
        /// Process Data Cell 삽입
        /// </summary>
        /// <param name="objData"></param>
        public void SetInsertHistoryProcessData(CellData.OneCell objData)
        {
            CHistoryProcessData obj = new CHistoryProcessData(objData);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetInsertHistoryProcessData, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetInsertHistoryProcessData), obj);
        }

        /// <summary>
        /// Process Data Cell 수정
        /// </summary>
        /// <param name="objData"></param>
        public void SetUpdateHistoryProcessData(CellData.OneCell objData)
        {
            CHistoryProcessData obj = new CHistoryProcessData(objData);
            _queue.Enqueue(new EventItem(m_objDatabaseImplement.SetUpdateHistoryProcessData, obj));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(m_objDatabaseImplement.SetUpdateHistoryProcessData), obj);
        }
    }
}
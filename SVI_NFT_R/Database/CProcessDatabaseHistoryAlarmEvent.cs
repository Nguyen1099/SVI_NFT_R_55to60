using System;
using System.Data;
using System.Globalization;
using System.Threading;

namespace SVI_NFT_R
{
    class CProcessDatabaseHistoryAlarmEvent : CDatabaseAbstract
    {
        /// <summary>
        /// 이전 삽입 날짜
        /// </summary>
        private DateTime m_objPreviousDate;
        /// <summary>
        /// SET, CLEAR 이벤트 상태
        /// </summary>
        private CDatabaseDefine.EEventValue m_eEventValue;

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
                // 프로세스 데이터베이스 이어줌
                base.m_objProcessDatabase = objProcessDatabase;
                // 데이터베이스에서 초기에 조회해서 SET, CLEAR 상태 확인
                DataTable objDataTable = new DataTable();
                string query = string.Format("SELECT * FROM {0} ORDER BY {1} DESC LIMIT 1;",
                    m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableName(),
                    m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryAlarmEvent.DATE]
                    );
                m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload(query, ref objDataTable);
                DataRow[] objDataRow = objDataTable.Select();
                try
                {
                    if (0 == objDataRow.Length)
                    {
                        m_objPreviousDate = DateTime.Now;
                        m_eEventValue = CDatabaseDefine.EEventValue.CLEAR;
                    }
                    else if (CDatabaseDefine.EEventValue.SET.ToString() == objDataRow[objDataRow.Length - 1].ItemArray[(int)CDatabaseDefine.EHistoryAlarmEvent.EVENT].ToString())
                    {
                        m_objPreviousDate = DateTime.ParseExact(objDataRow[objDataRow.Length - 1].ItemArray[(int)CDatabaseDefine.EHistoryAlarmEvent.DATE].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        m_eEventValue = CDatabaseDefine.EEventValue.SET;
                    }
                    else if (CDatabaseDefine.EEventValue.CLEAR.ToString() == objDataRow[objDataRow.Length - 1].ItemArray[(int)CDatabaseDefine.EHistoryAlarmEvent.EVENT].ToString())
                    {
                        m_objPreviousDate = DateTime.ParseExact(objDataRow[objDataRow.Length - 1].ItemArray[(int)CDatabaseDefine.EHistoryAlarmEvent.DATE].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        m_eEventValue = CDatabaseDefine.EEventValue.CLEAR;
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
                // 스레드 시작
                base.m_bThreadExit = false;
                base.m_ThreadProcess = new Thread(ThreadProcess);
                base.m_ThreadProcess.IsBackground = true;
                base.m_ThreadProcess.Start(this);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            base.m_bThreadExit = true;
            base.m_ThreadProcess.Join();
        }

        /// <summary>
        /// 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcess(object state)
        {
            CProcessDatabaseHistoryAlarmEvent pThis = (CProcessDatabaseHistoryAlarmEvent)state;

            while (false == pThis.m_bThreadExit)
            {
                // 알람 이벤트 삽입
                pThis.SetInsertAlarmEvent();

                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// 공유 메모리에 알람 유무를 보고 Set Clear 알람 이벤트를 Insert함
        /// </summary>
        private void SetInsertAlarmEvent()
        {
            CManagerTable objHistoryAlarmEvent = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent;

            do
            {
                try
                {
                    if (
                        null == m_objProcessDatabase.m_objDatabaseSendMessage
                        || false == m_objProcessDatabase.GetDocument().m_objAlarmList.IsInitialized
                        )
                    {
                        break;
                    }

                    // 날짜가 지난 상태
                    if (m_objPreviousDate.DayOfYear != DateTime.Now.DayOfYear)
                    {
                        // 바로 이전 알람 이벤트 SET인 경우
                        if (CDatabaseDefine.EEventValue.SET == m_eEventValue)
                        {
                            int days = DateTime.Now.DayOfYear - m_objPreviousDate.DayOfYear - 1;
                            lock (m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite)
                            {
                                var transaction = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLBeginTransaction();
                                try
                                {
                                    for (int i = days; i >= 0; i--)
                                    {
                                        // 금일 자정 1초 전 Insert Clear
                                        DateTime objDateTime = DateTime.Today;
                                        objDateTime = objDateTime.AddMilliseconds(-1.0).AddDays(-i);
                                        m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(
                                            string.Format("INSERT INTO {0} VALUES ('{1}', '{2}');",
                                            m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableName(),
                                            objDateTime.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                                            CDatabaseDefine.EEventValue.CLEAR
                                            ));
                                        objDateTime = DateTime.Today;
                                        objDateTime = objDateTime.AddDays(-i);
                                        m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(
                                            string.Format("INSERT INTO {0} VALUES ('{1}', '{2}');",
                                            m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableName(),
                                            objDateTime.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                                            CDatabaseDefine.EEventValue.SET
                                            ));
                                    }
                                }
                                finally
                                {
                                    m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLCommit(transaction);
                                    transaction.Dispose();
                                }
                            }
                            m_eEventValue = CDatabaseDefine.EEventValue.SET;
                            m_objPreviousDate = DateTime.Today;
                        }
                        else
                        {
                            m_objPreviousDate = DateTime.Today;
                        }
                    }
                    else
                    {
                        // 공유 메모리에 알람이 있으면 Set
                        if (true == m_objProcessDatabase.GetDocument().GetIsHeavyAlarm())
                        {
                            // 바로 이전 알람 이벤트 CLEAR인 경우
                            if (CDatabaseDefine.EEventValue.CLEAR == m_eEventValue)
                            {
                                // Insert Set
                                m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryAlarmEvent(CDatabaseDefine.EEventValue.SET);
                                m_eEventValue = CDatabaseDefine.EEventValue.SET;
                                m_objPreviousDate = DateTime.Now;
                            }
                        }
                        // 공유 메모리에 알람이 없으면 Clear
                        else
                        {
                            // 바로 이전 알람 이벤트 SET인 경우
                            if (CDatabaseDefine.EEventValue.SET == m_eEventValue)
                            {
                                // Insert Clear
                                m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryAlarmEvent(CDatabaseDefine.EEventValue.CLEAR);
                                m_eEventValue = CDatabaseDefine.EEventValue.CLEAR;
                                m_objPreviousDate = DateTime.Now;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }
    }
}
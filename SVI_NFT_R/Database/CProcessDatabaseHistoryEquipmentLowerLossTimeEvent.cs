using System;
using System.Data;
using System.Globalization;
using System.Threading;

namespace SVI_NFT_R
{
    class CProcessDatabaseHistoryEquipmentLowerLossTimeEvent : CDatabaseAbstract
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
        /// 딜레이 시간
        /// </summary>
        private int m_iDelayTime;

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
                // 딜레이 시간 설정 (ms)
                m_iDelayTime = Config.WaitTime.Equipment.UnloadInterfaceDelayTime.ToMilliseconds();
                // 데이터베이스에서 초기에 조회해서 SET, CLEAR 상태 확인
                DataTable objDataTable = new DataTable();
                string query = string.Format("SELECT * FROM {0} ORDER BY {1} DESC LIMIT 1;",
                    m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent.HLGetTableName(),
                    m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistoryEquipmentLowerLossTimeEvent.DATE]
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
                    else if (CDatabaseDefine.EEventValue.SET.ToString() == objDataRow[objDataRow.Length - 1].ItemArray[(int)CDatabaseDefine.EHistoryEquipmentLowerLossTimeEvent.EVENT].ToString())
                    {
                        m_objPreviousDate = DateTime.ParseExact(objDataRow[objDataRow.Length - 1].ItemArray[(int)CDatabaseDefine.EHistoryEquipmentLowerLossTimeEvent.DATE].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        m_eEventValue = CDatabaseDefine.EEventValue.SET;
                    }
                    else if (CDatabaseDefine.EEventValue.CLEAR.ToString() == objDataRow[objDataRow.Length - 1].ItemArray[(int)CDatabaseDefine.EHistoryEquipmentLowerLossTimeEvent.EVENT].ToString())
                    {
                        m_objPreviousDate = DateTime.ParseExact(objDataRow[objDataRow.Length - 1].ItemArray[(int)CDatabaseDefine.EHistoryEquipmentLowerLossTimeEvent.DATE].ToString(), CDatabaseDefine.DEF_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                        m_eEventValue = CDatabaseDefine.EEventValue.CLEAR;
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

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
        /// 스레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcess(object state)
        {
            CProcessDatabaseHistoryEquipmentLowerLossTimeEvent pThis = (CProcessDatabaseHistoryEquipmentLowerLossTimeEvent)state;

            while (false == pThis.m_bThreadExit)
            {
                // 뒷 설비 지연 시간 이벤트 삽입
                pThis.SetInsertLowerLossTimeEvent();

                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// 하류 설비와 인터페이스 중 지연 시간 이벤트를 체크
        /// </summary>
        private void SetInsertLowerLossTimeEvent()
        {
            CManagerTable objHistoryEquipmentLowerLossTimeEvent = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent;

            do
            {
                try
                {
                    if (null == m_objProcessDatabase.m_objDatabaseSendMessage)
                        break;

                    // 날짜가 지난 상태
                    if (m_objPreviousDate.DayOfYear != DateTime.Now.DayOfYear)
                    {
                        // 바로 이전 이벤트 SET인 경우
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
                                            m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent.HLGetTableName(),
                                            objDateTime.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT),
                                            CDatabaseDefine.EEventValue.CLEAR
                                            ));
                                        objDateTime = DateTime.Today;
                                        objDateTime = objDateTime.AddDays(-i);
                                        m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute(
                                            string.Format("INSERT INTO {0} VALUES ('{1}', '{2}');",
                                            m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent.HLGetTableName(),
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
                        // 설비 시작 상태
                        if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objProcessDatabase.GetDocument().m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                        {
                            // SET 조건
                            //Self 설비 ABLE 신호 ON & H / S 진행중 OFF
                            if (true == m_objProcessDatabase.GetDocument().m_objProcessMain.m_objProcessMotion.OutFlip.IsUnloadPending)
                            {
                                // 바로 이전 이벤트 CLEAR인 경우
                                if (CDatabaseDefine.EEventValue.CLEAR == m_eEventValue)
                                {
                                    // 딜레이를 보는 시간이 없으면 이벤트가 너무 자주 들어옴
                                    if (0 >= m_iDelayTime)
                                    {
                                        // Insert Set
                                        m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryEquipmentLowerLossTimeEvent(CDatabaseDefine.EEventValue.SET);
                                        m_eEventValue = CDatabaseDefine.EEventValue.SET;
                                        m_objPreviousDate = DateTime.Now;
                                        // 딜레이 시간 초기화
                                        m_iDelayTime = Config.WaitTime.Equipment.UnloadInterfaceDelayTime.ToMilliseconds();
                                    }
                                    else
                                    {
                                        // 딜레이 시간 Thread Time 만큼 차감
                                        m_iDelayTime -= 5;
                                    }
                                }
                            }
                            // CLEAR 조건
                            // Self 설비 ABLE 신호 ON & H/S 진행중 ON
                            else
                            {
                                // 바로 이전 이벤트 SET인 경우
                                if (CDatabaseDefine.EEventValue.SET == m_eEventValue)
                                {
                                    // Insert Clear
                                    m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryEquipmentLowerLossTimeEvent(CDatabaseDefine.EEventValue.CLEAR);
                                    m_eEventValue = CDatabaseDefine.EEventValue.CLEAR;
                                    m_objPreviousDate = DateTime.Now;
                                }
                                // 딜레이 시간 초기화
                                m_iDelayTime = Config.WaitTime.Equipment.UnloadInterfaceDelayTime.ToMilliseconds();
                            }
                        }
                        // CLEAR 조건
                        // 설비 시작 아닌 경우 or
                        // 설비 알람 상태 or
                        else if (
                            CCIMDefine.EMoveState.MOVE_STATE_PAUSE == m_objProcessDatabase.GetDocument().m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]
                            || true == m_objProcessDatabase.GetDocument().GetIsHeavyAlarm()
                            )
                        {
                            // 바로 이전 이벤트 SET인 경우
                            if (CDatabaseDefine.EEventValue.SET == m_eEventValue)
                            {
                                // Insert Clear
                                m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryEquipmentLowerLossTimeEvent(CDatabaseDefine.EEventValue.CLEAR);
                                m_eEventValue = CDatabaseDefine.EEventValue.CLEAR;
                                m_objPreviousDate = DateTime.Now;
                            }
                            // 딜레이 시간 초기화
                            m_iDelayTime = Config.WaitTime.Equipment.UnloadInterfaceDelayTime.ToMilliseconds();
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
using System;
using System.Threading;

namespace SVI_NFT_R
{
    class CProcessDatabaseHistoryDelete
    {
        /// <summary>
        /// 상위 클래스
        /// </summary>
        private CProcessDatabase m_objProcessDatabase;
        /// <summary>
        /// 시스템 타이머
        /// </summary>
        private Timer m_objSystemTimer;
        /// <summary>
        /// SQLite
        /// </summary>
        private CSQLite m_objSQLite;

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="objProcessDatabase"></param>
        /// <param name="objSQLite"></param>
        /// <returns></returns>
        public bool Initialize(CProcessDatabase objProcessDatabase, CSQLite objSQLite)
        {
            bool bReturn = false;

            do
            {
                // 프로세스 데이터베이스 이어줌
                m_objProcessDatabase = objProcessDatabase;
                // SQLite 이어줌
                m_objSQLite = objSQLite;
                if (null == m_objSystemTimer)
                {
                    // 타이머에 대한 콜백 메서드 정의
                    m_objSystemTimer = new Timer(SetDeleteHistory);
                    // 지연시간, 기간 (ms) 한 시간마다 주기로 호출되게 설정
                    m_objSystemTimer.Change(0, (int)new TimeSpan(1, 0, 0).TotalMilliseconds);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            if (null != m_objSystemTimer)
            {
                m_objSystemTimer.Change(Timeout.Infinite, Timeout.Infinite);
                m_objSystemTimer.Dispose();
                m_objSystemTimer = null;
            }
        }

        /// <summary>
        /// 히스토리 삭제 쿼리 날려줌
        /// </summary>
        /// <param name="state"></param>
        private void SetDeleteHistory(Object state)
        {
            CConfig.CDatabaseParameter objDatabaseParameter = m_objProcessDatabase.GetDocument().m_objConfig.GetDatabaseParameter();
            CManagerTable objHistoryAlarm = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarm;
            CManagerTable objHistoryOPCall = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryOPCall;
            CManagerTable objHistoryInterlockCall = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryInterlockCall;
            CManagerTable objHistoryTerminalCall = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryTerminalCall;
            CManagerTable objHistoryProcessDataCell = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataCell;
            CManagerTable objHistoryProcessDataJudgement = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataJudgement;
            CManagerTable objHistoryProcessDataReader = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataReader;
            CManagerTable objHistoryProcessDataVisionResult = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataVisionResult;
            CManagerTable objHistoryProcessDataWorkOrder = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataWorkOrder;
            CManagerTable objHistoryCellTrackIn = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellTrackIn;
            CManagerTable objHistoryCellTrackOut = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellTrackOut;
            CManagerTable objHistoryCellInput = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellInput;
            CManagerTable objHistoryCellOutput = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellOutput;
            CManagerTable objHistoryMCRStatus = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMCRStatus;
            CManagerTable objHistoryMachineResult = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMachineResult;
            CManagerTable objHistoryAlarmEvent = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent;
            CManagerTable objHistoryEquipmentStopEvent = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentStopEvent;
            CManagerTable objHistoryEquipmentTPCodeEvent = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentTPCodeEvent;
            CManagerTable objHistoryEquipmentUpperLossTimeEvent = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentUpperLossTimeEvent;
            CManagerTable objHistoryEquipmentLowerLossTimeEvent = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent;

            // 트랜잭션 시작
            lock (m_objSQLite)
            {
                var objTransaction = m_objSQLite.HLBeginTransaction();
                try
                {
                    // 특정 기간 이전 알람 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryAlarm, (int)CDatabaseDefine.EHistoryAlarm.DATE, objDatabaseParameter.iDeletePeriodAlarm);
                    // 특정 기간 이전 오퍼레이션 콜 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryOPCall, (int)CDatabaseDefine.EHistoryOPCall.DATE, objDatabaseParameter.iDeletePeriodOPCall);
                    // 특정 기간 이전 인터락 콜 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryInterlockCall, (int)CDatabaseDefine.EHistoryInterlockCall.DATE, objDatabaseParameter.iDeletePeriodInterlockCall);
                    // 특정 기간 이전 터미널 콜 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryTerminalCall, (int)CDatabaseDefine.EHistoryTerminalCall.DATE, objDatabaseParameter.iDeletePeriodTerminalCall);
                    // 특정 기간 이전 Process Data Cell 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryProcessDataCell, (int)CDatabaseDefine.EHistoryProcessDataCell.DATE, objDatabaseParameter.iDeletePeriodProcessDataCell);
                    // 특정 기간 이전 Process Data Judgement 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryProcessDataJudgement, (int)CDatabaseDefine.EHistoryProcessDataJudgement.DATE, objDatabaseParameter.iDeletePeriodProcessDataJudgement);
                    // 특정 기간 이전 Process Data Reader 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryProcessDataReader, (int)CDatabaseDefine.EHistoryProcessDataReader.DATE, objDatabaseParameter.iDeletePeriodProcessDataReader);
                    // 특정 기간 이전 Process Data Vision Result 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryProcessDataVisionResult, (int)CDatabaseDefine.EHistoryProcessDataVisionResult.DATE, objDatabaseParameter.iDeletePeriodProcessDataVisionResult);
                    // 특정 기간 이전 Process Data Work Order 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryProcessDataWorkOrder, (int)CDatabaseDefine.EHistoryProcessDataWorkOrder.DATE, objDatabaseParameter.iDeletePeriodProcessDataWorkOrder);
                    // 특정 기간 이전 Cell Track In 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryCellTrackIn, (int)CDatabaseDefine.EHistoryCellTrackIn.DATE, objDatabaseParameter.iDeletePeriodCellTrackIn);
                    // 특정 기간 이전 Cell Track Out 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryCellTrackOut, (int)CDatabaseDefine.EHistoryCellTrackOut.DATE, objDatabaseParameter.iDeletePeriodCellTrackOut);
                    // 특정 기간 이전 Cell Input 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryCellInput, (int)CDatabaseDefine.EHistoryCellInput.DATE, objDatabaseParameter.iDeletePeriodCellInput);
                    // 특정 기간 이전 Cell Output 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryCellOutput, (int)CDatabaseDefine.EHistoryCellOutput.DATE, objDatabaseParameter.iDeletePeriodCellOutput);
                    // 특정 기간 이전 MCR Status 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryMCRStatus, (int)CDatabaseDefine.EHistoryMCRStatus.DATE, objDatabaseParameter.iDeletePeriodMCRStatus);
                    // 특정 기간 이전 Machine Result 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryMachineResult, (int)CDatabaseDefine.EHistoryMachineResult.DATE, objDatabaseParameter.iDeletePeriodMachineResult);
                    // 특정 기간 이전 Alarm Event 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryAlarmEvent, (int)CDatabaseDefine.EHistoryAlarmEvent.DATE, objDatabaseParameter.iDeletePeriodAlarmEvent);
                    // 특정 기간 이전 Equipment Stop Event 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryEquipmentStopEvent, (int)CDatabaseDefine.EHistoryEquipmentStopEvent.DATE, objDatabaseParameter.iDeletePeriodEquipmentStopEvent);
                    // 특정 기간 이전 Equipment TP Code Event 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryEquipmentTPCodeEvent, (int)CDatabaseDefine.EHistoryEquipmentTPCodeEvent.DATE, objDatabaseParameter.iDeletePeriodEquipmentTPCodeEvent);
                    // 특정 기간 이전 Equipment Upper Loss Time Event 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryEquipmentUpperLossTimeEvent, (int)CDatabaseDefine.EHistoryEquipmentUpperLossTimeEvent.DATE, objDatabaseParameter.iDeletePeriodEquipmentUpperLossTimeEvent);
                    // 특정 기간 이전 Equipment Lower Loss Time Event 히스토리를 삭제하는 쿼리
                    SetDeleteHistory(objHistoryEquipmentLowerLossTimeEvent, (int)CDatabaseDefine.EHistoryEquipmentLowerLossTimeEvent.DATE, objDatabaseParameter.iDeletePeriodEquipmentLowerLossTimeEvent);
                }
                finally
                {
                    // 커밋
                    m_objSQLite.HLCommit(objTransaction);
                    objTransaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 히스토리 삭제 쿼리 날려줌
        /// </summary>
        /// <param name="objManagerTable"></param>
        /// <param name="iIndex"></param>
        /// <param name="iDeletePeriod"></param>
        private void SetDeleteHistory(CManagerTable objManagerTable, int iIndex, int iDeletePeriod)
        {
            // 금일을 기준으로 특정일을 계산해야함
            string strQuery = string.Format("DELETE FROM {0} WHERE {1} <= strftime('%d', 'now', 'localtime') - strftime('%d', {2});",
                objManagerTable.HLGetTableName(),
                iDeletePeriod,
                objManagerTable.HLGetTableSchemaName()[iIndex]
                );
            m_objSQLite.HLExecute(strQuery);
        }
    }
}
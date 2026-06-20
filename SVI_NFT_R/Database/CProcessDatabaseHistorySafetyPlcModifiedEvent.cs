using System;
using System.Data;
using System.Linq;
using System.Threading;

namespace SVI_NFT_R
{
    public class CProcessDatabaseHistorySafetyPlcModifiedEvent : CDatabaseAbstract
    {
        private string mLastSignatureCode = string.Empty;
        private readonly long mReadInterval = TimeSpan.FromSeconds(1d).Ticks;
        private long mLastTimestamp = Utils.GetTimestamp();

        public bool Initialize(CProcessDatabase objProcessDatabase)
        {
            m_objProcessDatabase = objProcessDatabase;

            mLastSignatureCode = GetLastSafetyPlcSignatureCode();

            m_bThreadExit = false;
            m_ThreadProcess = new Thread(ThreadProcess);
            m_ThreadProcess.IsBackground = true;
            m_ThreadProcess.Start();
            return true;
        }

        public void DeInitialize()
        {
            m_bThreadExit = true;
            m_ThreadProcess.Join();
        }

        private void ThreadProcess()
        {
            // 도큐먼트 초기화 대기
            while (m_bThreadExit == false
                && m_objProcessDatabase.GetDocument().IsInitialized == false
                )
            {
                Thread.Sleep(250);
            }

            while (m_bThreadExit == false)
            {
                SetInsertSafetyPlcModifiedEvent();
                Thread.Sleep(100);
            }
        }

        private void SetInsertSafetyPlcModifiedEvent()
        {
            if (GetElapsed(mLastTimestamp) < mReadInterval
                || CProcessMain.SerialReaders.SafetyPlc.Count == 0
                )
            {
                return;
            }
            mLastTimestamp = Utils.GetTimestamp();

            CDefine.ESafetyPlc firstKey = Enum.GetValues(typeof(CDefine.ESafetyPlc))
                .Cast<CDefine.ESafetyPlc>()
                .FirstOrDefault();
            var lastReadValue = CProcessMain.SerialReaders.SafetyPlc[firstKey].Value;
            if (lastReadValue.IsValid == false)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(lastReadValue.SignatureCode) == true
                || mLastSignatureCode == lastReadValue.SignatureCode
                )
            {
                return;
            }
            mLastSignatureCode = lastReadValue.SignatureCode;

            m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistorySafetyPlcModifiedEvent(firstKey.ToString(), lastReadValue.LastModifyDateTime, lastReadValue.SignatureCode);
        }

        private string GetLastSafetyPlcSignatureCode()
        {
            var readData = new DataTable();
            string tableName = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistorySafetyPlcModifiedEvent.HLGetTableName();
            string orderByColName = m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistorySafetyPlcModifiedEvent.HLGetTableSchemaName()[(int)CDatabaseDefine.EHistorySafetyPlcModifiedEvent.LAST_MODIFIED_TIME];
            if (m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLReload($"SELECT * FROM {tableName} ORDER BY {orderByColName} DESC LIMIT 1", ref readData).m_bError == true)
            {
                return string.Empty;
            }
            if (readData.Rows.Count == 0)
            {
                return string.Empty;
            }
            return Convert.ToString(readData.Rows[0][(int)CDatabaseDefine.EHistorySafetyPlcModifiedEvent.SIGNATURE_CODE]);
        }

        private long GetElapsed(long lastTimestamp)
        {
            return Utils.GetTimestamp() - lastTimestamp;
        }
    }
}
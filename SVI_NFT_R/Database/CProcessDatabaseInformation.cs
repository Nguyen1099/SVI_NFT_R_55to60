namespace SVI_NFT_R
{
    public class CProcessDatabaseInformation : CDatabaseAbstract
    {
        /// <summary>
        /// SQLite
        /// </summary>
        public CSQLite m_objSQLite;
        /// <summary>
        /// Information Alarm
        /// </summary>
        public CManagerTable m_objManagerTableInformationAlarm;
        /// <summary>
        /// Information SVID
        /// </summary>
        public CManagerTable m_objManagerTableInformationSVID;
        /// <summary>
        /// Information UI Text
        /// </summary>
        public CManagerTable m_objManagerTableInformationUIText;
        /// <summary>
        /// Information User Message
        /// </summary>
        public CManagerTable m_objManagerTableInformationUserMessage;
        /// <summary>
        /// Information User
        /// </summary>
        public CManagerTable m_objManagerTableInformationUser;

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
                CErrorReturn objReturn = m_objSQLite.HLInitialize(string.Format(@"{0}\{1}.db3", objConfig.GetCurrentPath(), objDatabaseParameter.strDatabaseInformation));
                if (true == objReturn.m_bError)
                    break;
                // SQLite Connect
                objReturn = m_objSQLite.HLConnect();
                if (true == objReturn.m_bError)
                    break;
                // Information Alarm 초기화
                m_objManagerTableInformationAlarm = new CManagerTable();
                if (false == m_objManagerTableInformationAlarm.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableInformationAlarm),
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseRecordPath(), objDatabaseParameter.strRecordInformationAlarm)))
                    break;
                // Information SVID 초기화
                m_objManagerTableInformationSVID = new CManagerTable();
                if (false == m_objManagerTableInformationSVID.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableInformationSVID),
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseRecordPath(), objDatabaseParameter.strRecordInformationSVID)))
                    break;
                // Information UI Text 초기화
                m_objManagerTableInformationUIText = new CManagerTable();
                if (false == m_objManagerTableInformationUIText.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableInformationUIText),
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseRecordPath(), objDatabaseParameter.strRecordInformationUIText)))
                    break;
                // Information User Message 초기화
                m_objManagerTableInformationUserMessage = new CManagerTable();
                if (false == m_objManagerTableInformationUserMessage.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableInformationUserMessage),
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseRecordPath(), objDatabaseParameter.strRecordInformationUserMessage)
                    ))
                    break;
                // Information User 초기화
                m_objManagerTableInformationUser = new CManagerTable();
                if (false == m_objManagerTableInformationUser.HLInitialize(
                    m_objSQLite,
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseTablePath(), objDatabaseParameter.strTableInformationUser),
                    string.Format(@"{0}\{1}.txt", objConfig.GetDatabaseRecordPath(), objDatabaseParameter.strRecordInformationUser)
                    // 테이블 데이터를 매번 덮어쓰지 않음
                    , bShouldOverwriteRecord: false
                    ))
                    break;

                // 테이블 전체 로드
                m_objManagerTableInformationAlarm.HLSetDataTableUpdate();
                m_objManagerTableInformationSVID.HLSetDataTableUpdate();
                m_objManagerTableInformationUIText.HLSetDataTableUpdate();
                m_objManagerTableInformationUserMessage.HLSetDataTableUpdate();
                m_objManagerTableInformationUser.HLSetDataTableUpdate();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제 함수
        /// </summary>
        public void Deinitialize()
        {
            // Information Alarm
            if (null != m_objManagerTableInformationAlarm)
            {
                m_objManagerTableInformationAlarm.HLDeInitialize();
            }
            // Information SVID
            if (null != m_objManagerTableInformationSVID)
            {
                m_objManagerTableInformationSVID.HLDeInitialize();
            }
            // Information UI Text
            if (null != m_objManagerTableInformationUIText)
            {
                m_objManagerTableInformationUIText.HLDeInitialize();
            }
            // Information User Message
            if (null != m_objManagerTableInformationUserMessage)
            {
                m_objManagerTableInformationUserMessage.HLDeInitialize();
            }
            // Information User
            if (null != m_objManagerTableInformationUser)
            {
                m_objManagerTableInformationUser.HLDeInitialize();
            }
            if (null != m_objSQLite)
            {
                // SQLite Disconnect
                m_objSQLite.HLDisconnect();
                // SQLite 해제
                m_objSQLite.HLDeInitialize();
            }
        }
    }
}
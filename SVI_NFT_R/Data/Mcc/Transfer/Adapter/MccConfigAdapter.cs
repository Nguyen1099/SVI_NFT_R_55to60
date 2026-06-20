namespace SVI_NFT_R
{
    public sealed class MccConfigAdapter : IMccConfig
    {
        private readonly CDocument mDocument;

        public MccConfigAdapter(CDocument document)
        {
            mDocument = document;
        }

        public string LocalPath
        {
            get
            {
                return mDocument.m_objConfig.GetMccOptionParameter().strLocalPath;
            }
        }

        public string BasicPath
        {
            get
            {
                return mDocument.m_objConfig.GetMccOptionParameter().strBasicPath;
            }
        }

        public string EquipmentCategoryName
        {
            get
            {
                var opt = mDocument.m_objConfig.GetMccOptionParameter();
                var sys = mDocument.m_objConfig.GetSystemParameter();
                // EquipmentID에서 카테고리 문자열 제거
                return sys.strEquipmentID.Replace(opt.strEquipmentCategoryName, string.Empty);
            }
        }

        public int LogUploadTimeMinutes
        {
            get
            {
                return mDocument.m_objConfig.GetMccOptionParameter().iLogUploadTime;
            }
        }

        public int LogDeletePeriodDays
        {
            get
            {
                return mDocument.m_objConfig.GetMccOptionParameter().iLogDeletePeriod;
            }
        }

        public int LoggingTimeMinutes
        {
            get
            {
                return mDocument.m_objConfig.GetMccOptionParameter().iLoggingTime;
            }
        }

        public bool IsMccEnabled
        {
            get
            {
                return mDocument.m_objConfig.GetMccOptionParameter().bMccEnabled;
            }
        }

        public string EquipmentId
        {
            get
            {
                return mDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            }
        }

        public ConnectInfo RemoteConnectInfo
        {
            get
            {
                var info = mDocument.m_objConfig.GetMccInitializeParameter().objConnectInformation;
                return new ConnectInfo
                {
                    HostName = info.strHostName,
                    HostPort = info.strHostPort,
                    UserName = info.strUserName,
                    UserPassword = info.strUserPassword
                };
            }
        }
    }
}

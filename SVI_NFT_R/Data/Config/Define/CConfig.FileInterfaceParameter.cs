using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// [공용] FTP 초기화 객체
        /// </summary>
        [Serializable]
        public class CFileInterfaceParameter
        {
            [Serializable]
            public struct structureConnectInformation
            {
                public string strHostName;
                public int strHostPort;
                public string strUserName;
                public string strUserPassword;

                public structureConnectInformation(structureConnectInformation objConnectInformation)
                {
                    strHostName = objConnectInformation.strHostName;
                    strHostPort = objConnectInformation.strHostPort;
                    strUserName = objConnectInformation.strUserName;
                    strUserPassword = objConnectInformation.strUserPassword;
                }
            }

            public structureConnectInformation objConnectInformation = new structureConnectInformation();
        }
    }
}
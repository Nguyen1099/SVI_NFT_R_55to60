using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        [Serializable]
        public class CMccInitializeParameter : CFileInterfaceParameter
        {
        }

        /// <summary>
        /// MCC 초기화 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CMccInitializeParameter GetMccInitializeParameter() => m_objMccInitalizeParameter;

        /// <summary>
        /// FTP 파라미터 선언
        /// </summary>
        private CMccInitializeParameter m_objMccInitalizeParameter = new CMccInitializeParameter();

        /// <summary>
        /// MCC 초기화 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadMccInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MCC";
                m_objMccInitalizeParameter.objConnectInformation.strHostName = objINI.GetString(sectionName, "objConnectInformation.strHostName", "127.0.0.1");
                m_objMccInitalizeParameter.objConnectInformation.strHostPort = objINI.GetInt32(sectionName, "objConnectInformation.strHostPort", 21);
                m_objMccInitalizeParameter.objConnectInformation.strUserName = objINI.GetString(sectionName, "objConnectInformation.strUserName", "SVI_NFT_F");
                m_objMccInitalizeParameter.objConnectInformation.strUserPassword = objINI.GetString(sectionName, "objConnectInformation.strUserPassword", "SVI_NFT_F");

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// MCC 초기화 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveMccInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MCC";
                objINI.WriteValue(sectionName, "objConnectInformation.strHostName", m_objMccInitalizeParameter.objConnectInformation.strHostName);
                objINI.WriteValue(sectionName, "objConnectInformation.strHostPort", m_objMccInitalizeParameter.objConnectInformation.strHostPort);
                objINI.WriteValue(sectionName, "objConnectInformation.strUserName", m_objMccInitalizeParameter.objConnectInformation.strUserName);
                objINI.WriteValue(sectionName, "objConnectInformation.strUserPassword", m_objMccInitalizeParameter.objConnectInformation.strUserPassword);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// MCC 초기화 파라미터 저장
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool SaveMccInitializeParameter(CMccInitializeParameter parameter)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MCC";
                objINI.WriteValue(sectionName, "objConnectInformation.strHostName", parameter.objConnectInformation.strHostName);
                objINI.WriteValue(sectionName, "objConnectInformation.strHostPort", parameter.objConnectInformation.strHostPort);
                objINI.WriteValue(sectionName, "objConnectInformation.strUserName", parameter.objConnectInformation.strUserName);
                objINI.WriteValue(sectionName, "objConnectInformation.strUserPassword", parameter.objConnectInformation.strUserPassword);
                m_objMccInitalizeParameter = parameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
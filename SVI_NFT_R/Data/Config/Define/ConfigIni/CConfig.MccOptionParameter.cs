using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// FTP 초기화 객체
        /// </summary>
        [Serializable]
        public class CMccOptionParameter
        {
            public string strLocalPath;
            public string strBasicPath;
            public string strEquipmentCategoryName;
            public int iLoggingTime;
            public int iInformationLoggingScanTime;
            public int iLogDeletePeriod;
            public int iLogUploadTime;
            public bool bMccEnabled;
        }

        /// <summary>
        /// MCC 옵션 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CMccOptionParameter GetMccOptionParameter() => m_objMccOptionParameter;

        /// <summary>
        /// MCC 관련 파라미터 선언
        /// </summary>
        private CMccOptionParameter m_objMccOptionParameter = new CMccOptionParameter();

        /// <summary>
        /// MCC 옵션 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadMccOptionParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MCC PARAMETER";
                m_objMccOptionParameter.strLocalPath = objINI.GetString(sectionName, "strLocalPath", "D:\\ITEM\\MccLog\\");
                m_objMccOptionParameter.strBasicPath = objINI.GetString(sectionName, "strBasicPath", "default\\");
                m_objMccOptionParameter.strEquipmentCategoryName = objINI.GetString("MCC PARAMETER", "strEquipmentCategoryName", "_RS01");
                m_objMccOptionParameter.iLoggingTime = objINI.GetInt32(sectionName, "iLoggingTime", 60);
                m_objMccOptionParameter.iLogDeletePeriod = objINI.GetInt32(sectionName, "iLogDeletePeriod", 30);
                m_objMccOptionParameter.iInformationLoggingScanTime = objINI.GetInt32(sectionName, "iInformationLoggingScanTime", 30);
                m_objMccOptionParameter.iLogUploadTime = objINI.GetInt32(sectionName, "iLogUploadTime", 60);
                m_objMccOptionParameter.bMccEnabled = objINI.GetBool(sectionName, "bMccEnabled", false);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// MCC 옵션 파라미터 저장
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool SaveMccOptionParameter(CMccOptionParameter parameter)
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MCC PARAMETER";
                objINI.WriteValue(sectionName, "strLocalPath", parameter.strLocalPath);
                objINI.WriteValue(sectionName, "strBasicPath", parameter.strBasicPath);
                objINI.WriteValue(sectionName, "strEquipmentCategoryName", parameter.strEquipmentCategoryName);
                objINI.WriteValue(sectionName, "iLoggingTime", parameter.iLoggingTime);
                objINI.WriteValue(sectionName, "iLogDeletePeriod", parameter.iLogDeletePeriod);
                objINI.WriteValue(sectionName, "iInformationLoggingScanTime", parameter.iInformationLoggingScanTime);
                objINI.WriteValue(sectionName, "iLogUploadTime", parameter.iLogUploadTime);
                objINI.WriteValue(sectionName, "bMccEnabled", parameter.bMccEnabled);
                m_objMccOptionParameter = parameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// MCC 옵션 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveMccOptionParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_CONFIG_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MCC PARAMETER";
                objINI.WriteValue(sectionName, "strLocalPath", m_objMccOptionParameter.strLocalPath);
                objINI.WriteValue(sectionName, "strBasicPath", m_objMccOptionParameter.strBasicPath);
                objINI.WriteValue(sectionName, "strEquipmentCategoryName", m_objMccOptionParameter.strEquipmentCategoryName);
                objINI.WriteValue(sectionName, "iLoggingTime", m_objMccOptionParameter.iLoggingTime);
                objINI.WriteValue(sectionName, "iLogDeletePeriod", m_objMccOptionParameter.iLogDeletePeriod);
                objINI.WriteValue(sectionName, "iInformationLoggingScanTime", m_objMccOptionParameter.iInformationLoggingScanTime);
                objINI.WriteValue(sectionName, "iLogUploadTime", m_objMccOptionParameter.iLogUploadTime);
                objINI.WriteValue(sectionName, "bMccEnabled", m_objMccOptionParameter.bMccEnabled);

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
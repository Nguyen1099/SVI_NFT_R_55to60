using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// MCR 파라미터
        /// </summary>
        [Serializable]
        public class CMCRParameter
        {
            public string strIPAddress;
            public string strPortNumber;
        }

        /// <summary>
        /// MCR 파라미터 리턴
        /// </summary>
        /// <param name="eMcr"></param>
        /// <returns></returns>
        public CMCRParameter GetMcrParameter(CDefine.EMcr eMcr) => m_objMcrParameter[(int)eMcr];

        /// <summary>
        /// MCR 파리미터 선언
        /// </summary>
        private CMCRParameter[] m_objMcrParameter = new CMCRParameter[Enum.GetNames(typeof(CDefine.EMcr)).Length];

        /// <summary>
        /// MCR 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadMCRParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                int count = Enum.GetNames(typeof(CDefine.EMcr)).Length;
                for (int i = 0; i < count; i++)
                {
                    if (m_objMcrParameter[i] == null)
                    {
                        m_objMcrParameter[i] = new CMCRParameter();
                    }
                    CDefine.EMcr index = (CDefine.EMcr)i;
                    string sectionName = $"MCR_{index}";
                    m_objMcrParameter[i].strIPAddress = objINI.GetString(sectionName, "strIPAddress", "127.0.0.1");
                    m_objMcrParameter[i].strPortNumber = objINI.GetString(sectionName, "strPortNumber", "21");
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// MCR 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveMCRParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                int count = Enum.GetNames(typeof(CDefine.EMcr)).Length;
                for (int i = 0; i < count; i++)
                {
                    CDefine.EMcr index = (CDefine.EMcr)i;
                    string sectionName = $"MCR_{index}";
                    objINI.WriteValue(sectionName, "strIPAddress", m_objMcrParameter[i].strIPAddress);
                    objINI.WriteValue(sectionName, "strPortNumber", m_objMcrParameter[i].strPortNumber);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// MCR 파라미터 저장
        /// </summary>
        /// <param name="objMCRParameter"></param>
        /// <param name="eMcr"></param>
        /// <returns></returns>
        public bool SaveMCRParameter(CMCRParameter objMCRParameter, CDefine.EMcr eMcr)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"MCR_{eMcr}";
                objINI.WriteValue(sectionName, "strIPAddress", objMCRParameter.strIPAddress);
                objINI.WriteValue(sectionName, "strPortNumber", objMCRParameter.strPortNumber);
                m_objMcrParameter[(int)eMcr] = objMCRParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
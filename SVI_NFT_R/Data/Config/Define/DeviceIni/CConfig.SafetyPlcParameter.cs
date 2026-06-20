using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 세이프티PLC 파라미터
        /// </summary>
        [Serializable]
        public class CSafetyPlcParameter
        {
            public string RemoteHostName = "192.168.1.68";
            public int RemoteHostPort = 9600;
            public byte NodeSafetyPlc = 68;
            public byte NodeSelf = 2;
        }

        /// <summary>
        /// 세이프티PLC 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CSafetyPlcParameter GetSafetyPlcParameter(CDefine.ESafetyPlc index) => m_objSafetyPlcParameter[(int)index];

        /// <summary>
        /// 세이프티PLC 파라미터 선언
        /// </summary>
        private readonly CSafetyPlcParameter[] m_objSafetyPlcParameter = new CSafetyPlcParameter[Enum.GetNames(typeof(CDefine.ESafetyPlc)).Length];

        /// <summary>
        /// 세이프티PLC 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadSafetyPlcParameter()
        {
            string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
            ClassINI objINI = new ClassINI(strPath);

            foreach (CDefine.ESafetyPlc index in Enum.GetValues(typeof(CDefine.ESafetyPlc)))
            {
                if (m_objSafetyPlcParameter[(int)index] == null)
                {
                    m_objSafetyPlcParameter[(int)index] = new CSafetyPlcParameter();
                }
                string sectionName = $"SAFETY_PLC_{index}";
                m_objSafetyPlcParameter[(int)index].RemoteHostName = objINI.GetString(sectionName, nameof(CSafetyPlcParameter.RemoteHostName), "192.168.0.100");
                m_objSafetyPlcParameter[(int)index].RemoteHostPort = objINI.GetInt32(sectionName, nameof(CSafetyPlcParameter.RemoteHostPort), 9600);
                m_objSafetyPlcParameter[(int)index].NodeSafetyPlc = Convert.ToByte(objINI.GetInt32(sectionName, nameof(CSafetyPlcParameter.NodeSafetyPlc), 100));
                m_objSafetyPlcParameter[(int)index].NodeSelf = Convert.ToByte(objINI.GetInt32(sectionName, nameof(CSafetyPlcParameter.NodeSelf), 2));
            }
            return true;
        }

        /// <summary>
        /// 세이프티PLC 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveSafetyPlcParameter()
        {
            string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
            ClassINI objINI = new ClassINI(strPath);

            foreach (CDefine.ESafetyPlc index in Enum.GetValues(typeof(CDefine.ESafetyPlc)))
            {
                string sectionName = $"SAFETY_PLC_{index}";
                objINI.WriteValue(sectionName, nameof(CSafetyPlcParameter.RemoteHostName), m_objSafetyPlcParameter[(int)index].RemoteHostName);
                objINI.WriteValue(sectionName, nameof(CSafetyPlcParameter.RemoteHostPort), m_objSafetyPlcParameter[(int)index].RemoteHostPort);
                objINI.WriteValue(sectionName, nameof(CSafetyPlcParameter.NodeSafetyPlc), m_objSafetyPlcParameter[(int)index].NodeSafetyPlc);
                objINI.WriteValue(sectionName, nameof(CSafetyPlcParameter.NodeSelf), m_objSafetyPlcParameter[(int)index].NodeSelf);
            }
            return true;
        }

        /// <summary>
        /// 세이프티PLC 파라미터 저장
        /// </summary>
        /// <param name="objSafetyPlcParameter"></param>
        /// <returns></returns>
        public bool SaveSafetyPlcParameter(CSafetyPlcParameter objSafetyPlcParameter, CDefine.ESafetyPlc index)
        {
            string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
            ClassINI objINI = new ClassINI(strPath);

            string sectionName = $"SAFETY_PLC_{index}";
            objINI.WriteValue(sectionName, nameof(CSafetyPlcParameter.RemoteHostName), objSafetyPlcParameter.RemoteHostName);
            objINI.WriteValue(sectionName, nameof(CSafetyPlcParameter.RemoteHostPort), objSafetyPlcParameter.RemoteHostPort);
            objINI.WriteValue(sectionName, nameof(CSafetyPlcParameter.NodeSafetyPlc), objSafetyPlcParameter.NodeSafetyPlc);
            objINI.WriteValue(sectionName, nameof(CSafetyPlcParameter.NodeSelf), objSafetyPlcParameter.NodeSelf);
            m_objSafetyPlcParameter[(int)index] = objSafetyPlcParameter.DeepClone();
            return true;
        }
    }
}
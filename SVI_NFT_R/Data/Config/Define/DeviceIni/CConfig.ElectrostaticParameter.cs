using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 저항측정기 초기화 객체
        /// </summary>
        [Serializable]
        public class CElectrostaticParameter : CommunicationParameter
        {
        }

        /// <summary>
        /// 저항측정기 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CElectrostaticParameter GetElectrostaticParameter(CDefine.EGms index) => m_objElectrostaticParameter[(int)index];

        /// <summary>
        /// 저항측정기 파라미터 선언
        /// </summary>
        private CElectrostaticParameter[] m_objElectrostaticParameter = new CElectrostaticParameter[Enum.GetNames(typeof(CDefine.EGms)).Length];

        /// <summary>
        /// 저항측정기 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadElectrostaticParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EGms index in Enum.GetValues(typeof(CDefine.EGms)))
                {
                    if (m_objElectrostaticParameter[(int)index] == null)
                    {
                        m_objElectrostaticParameter[(int)index] = new CElectrostaticParameter();
                    }
                    string sectionName = $"GMS_{index}";
                    m_objElectrostaticParameter[(int)index].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), 1);
                    m_objElectrostaticParameter[(int)index].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), "");
                    m_objElectrostaticParameter[(int)index].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 0);
                    m_objElectrostaticParameter[(int)index].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), $"COM{21 + (int)index}");
                    m_objElectrostaticParameter[(int)index].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 19200);
                    m_objElectrostaticParameter[(int)index].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objElectrostaticParameter[(int)index].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), 0);
                    m_objElectrostaticParameter[(int)index].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), 1);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 저항측정기 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveElectrostaticParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EGms index in Enum.GetValues(typeof(CDefine.EGms)))
                {
                    string sectionName = $"GMS_{index}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objElectrostaticParameter[(int)index].eType);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objElectrostaticParameter[(int)index].strSocketIPAddress);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objElectrostaticParameter[(int)index].iSocketPortNumber);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objElectrostaticParameter[(int)index].strSerialPortName);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objElectrostaticParameter[(int)index].iSerialPortBaudrate);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objElectrostaticParameter[(int)index].iSerialPortDataBits);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objElectrostaticParameter[(int)index].eParity);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objElectrostaticParameter[(int)index].eStopBits);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 저항측정기 파라미터 저장
        /// </summary>
        /// <param name="objElectrostaticParameter"></param>
        /// <returns></returns>
        public bool SaveElectrostaticParameter(CElectrostaticParameter objElectrostaticParameter, CDefine.EGms index)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"GMS_{index}";
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)objElectrostaticParameter.eType);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), objElectrostaticParameter.strSocketIPAddress);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), objElectrostaticParameter.iSocketPortNumber);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), objElectrostaticParameter.strSerialPortName);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), objElectrostaticParameter.iSerialPortBaudrate);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), objElectrostaticParameter.iSerialPortDataBits);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)objElectrostaticParameter.eParity);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)objElectrostaticParameter.eStopBits);
                m_objElectrostaticParameter[(int)index] = objElectrostaticParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
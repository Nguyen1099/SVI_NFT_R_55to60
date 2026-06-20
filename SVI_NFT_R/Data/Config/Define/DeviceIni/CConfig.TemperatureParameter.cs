using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 온도컨트롤러 파라미터
        /// </summary>
        [Serializable]
        public class CTemperatureParameter : CommunicationParameter
        {
        }

        /// <summary>
        /// 온도컨트롤러 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CTemperatureParameter GetTemperatureParameter(CDefine.ETemperatureController index) => m_objTemperatureParameter[(int)index];

        /// <summary>
        /// 온도컨트롤러 파라미터 선언
        /// </summary>
        private readonly CTemperatureParameter[] m_objTemperatureParameter = new CTemperatureParameter[Enum.GetNames(typeof(CDefine.ETemperatureController)).Length];

        /// <summary>
        /// 온도컨트롤러 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadTemperatureParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.ETemperatureController index in Enum.GetValues(typeof(CDefine.ETemperatureController)))
                {
                    if (m_objTemperatureParameter[(int)index] == null)
                    {
                        m_objTemperatureParameter[(int)index] = new CTemperatureParameter();
                    }
                    string sectionName = $"TEMPERATURE_NEOSHSD_{index}";
                    m_objTemperatureParameter[(int)index].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), 1);
                    m_objTemperatureParameter[(int)index].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), "");
                    m_objTemperatureParameter[(int)index].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 0);
                    m_objTemperatureParameter[(int)index].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), $"COM{8 + (int)index}");
                    m_objTemperatureParameter[(int)index].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 9600);
                    m_objTemperatureParameter[(int)index].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objTemperatureParameter[(int)index].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), 0);
                    m_objTemperatureParameter[(int)index].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), 2);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 온도컨트롤러 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveTemperatureParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.ETemperatureController index in Enum.GetValues(typeof(CDefine.ETemperatureController)))
                {
                    string sectionName = $"TEMPERATURE_NEOSHSD_{index}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objTemperatureParameter[(int)index].eType);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objTemperatureParameter[(int)index].strSocketIPAddress);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objTemperatureParameter[(int)index].iSocketPortNumber);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objTemperatureParameter[(int)index].strSerialPortName);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objTemperatureParameter[(int)index].iSerialPortBaudrate);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objTemperatureParameter[(int)index].iSerialPortDataBits);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objTemperatureParameter[(int)index].eParity);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objTemperatureParameter[(int)index].eStopBits);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 온도컨트롤러 파라미터 저장
        /// </summary>
        /// <param name="objTemperatureParameter"></param>
        /// <returns></returns>
        public bool SaveTemperatureParameter(CTemperatureParameter objTemperatureParameter, CDefine.ETemperatureController index)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"TEMPERATURE_NEOSHSD_{index}";
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)objTemperatureParameter.eType);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), objTemperatureParameter.strSocketIPAddress);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), objTemperatureParameter.iSocketPortNumber);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), objTemperatureParameter.strSerialPortName);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), objTemperatureParameter.iSerialPortBaudrate);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), objTemperatureParameter.iSerialPortDataBits);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)objTemperatureParameter.eParity);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)objTemperatureParameter.eStopBits);
                m_objTemperatureParameter[(int)index] = objTemperatureParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
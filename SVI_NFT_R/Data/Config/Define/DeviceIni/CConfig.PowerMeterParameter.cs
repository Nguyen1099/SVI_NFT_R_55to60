using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 적산량계 파라미터
        /// </summary>
        [Serializable]
        public class CPowerMeterParameter : CommunicationParameter
        {
            /// <summary>
            /// 2500/2550의 종류
            /// </summary>
            public int iUnitType;
            /// <summary>
            /// 2550의 유닛 ID
            /// </summary>
            public int iUnitID;
        }

        /// <summary>
        /// 적산량계 파라미터 리턴
        /// </summary>
        /// <param name="ePowerMeter"></param>
        /// <returns></returns>
        public CPowerMeterParameter GetPowerMeterParameter(CDefine.EPowerMeter ePowerMeter) => m_objPowerMeterParameter[(int)ePowerMeter];

        /// <summary>
        /// 적산량계파라미터 선언
        /// </summary>
        private readonly CPowerMeterParameter[] m_objPowerMeterParameter = new CPowerMeterParameter[Enum.GetNames(typeof(CDefine.EPowerMeter)).Length];

        /// <summary>
        /// 적산량계 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadPowerMeterParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EPowerMeter index in Enum.GetValues(typeof(CDefine.EPowerMeter)))
                {
                    if (m_objPowerMeterParameter[(int)index] == null)
                    {
                        m_objPowerMeterParameter[(int)index] = new CPowerMeterParameter();
                    }
                    string sectionName = $"POWER_METER_{index}";
                    m_objPowerMeterParameter[(int)index].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), 0);
                    m_objPowerMeterParameter[(int)index].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), "192.168.0.10");
                    m_objPowerMeterParameter[(int)index].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 502);
                    m_objPowerMeterParameter[(int)index].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), "");
                    m_objPowerMeterParameter[(int)index].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 0);
                    m_objPowerMeterParameter[(int)index].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objPowerMeterParameter[(int)index].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), 0);
                    m_objPowerMeterParameter[(int)index].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), 0);
                    m_objPowerMeterParameter[(int)index].iUnitType = objINI.GetInt32(sectionName, "iUnitType", 1);
                    m_objPowerMeterParameter[(int)index].iUnitID = objINI.GetInt32(sectionName, "iUnitID", 1);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 적산량계 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SavePowerMeterParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EPowerMeter index in Enum.GetValues(typeof(CDefine.EPowerMeter)))
                {
                    string sectionName = $"POWER_METER_{index}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objPowerMeterParameter[(int)index].eType);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objPowerMeterParameter[(int)index].strSocketIPAddress);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objPowerMeterParameter[(int)index].iSocketPortNumber);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objPowerMeterParameter[(int)index].strSerialPortName);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objPowerMeterParameter[(int)index].iSerialPortBaudrate);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objPowerMeterParameter[(int)index].iSerialPortDataBits);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objPowerMeterParameter[(int)index].eParity);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objPowerMeterParameter[(int)index].eStopBits);
                    objINI.WriteValue(sectionName, "iUnitType", m_objPowerMeterParameter[(int)index].iUnitType);
                    objINI.WriteValue(sectionName, "iUnitID", m_objPowerMeterParameter[(int)index].iUnitID);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 적산량계 파라미터 저장
        /// </summary>
        /// <param name="objPowerMeterParameter"></param>
        /// <param name="ePowerMeter"></param>
        /// <returns></returns>
        public bool SavePowerMeterParameter(CPowerMeterParameter objPowerMeterParameter, CDefine.EPowerMeter ePowerMeter)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);
                string sectionName = $"POWER_METER_{ePowerMeter}";
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)objPowerMeterParameter.eType);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), objPowerMeterParameter.strSocketIPAddress);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), objPowerMeterParameter.iSocketPortNumber);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), objPowerMeterParameter.strSerialPortName);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), objPowerMeterParameter.iSerialPortBaudrate);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), objPowerMeterParameter.iSerialPortDataBits);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)objPowerMeterParameter.eParity);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)objPowerMeterParameter.eStopBits);
                objINI.WriteValue(sectionName, "iUnitType", objPowerMeterParameter.iUnitType);
                objINI.WriteValue(sectionName, "iUnitID", objPowerMeterParameter.iUnitID);
                m_objPowerMeterParameter[(int)ePowerMeter] = objPowerMeterParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
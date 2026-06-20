using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        [Serializable]
        public class CMeasurementInitializeParameter : CommunicationParameter
        {
        }

        public CMeasurementInitializeParameter GetMeasurementInitializeParameter(CDefine.EMeasurement index) => m_objMeasurementInitializeParameter[(int)index];
        private CMeasurementInitializeParameter[] m_objMeasurementInitializeParameter = new CMeasurementInitializeParameter[Enum.GetNames(typeof(CDefine.EMeasurement)).Length];

        public bool LoadMeasurementInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                int defaultStartPortIndex = 10;
                foreach (CDefine.EMeasurement index in Enum.GetValues(typeof(CDefine.EMeasurement)))
                {
                    if (m_objMeasurementInitializeParameter[(int)index] == null)
                    {
                        m_objMeasurementInitializeParameter[(int)index] = new CMeasurementInitializeParameter();
                    }
                    string sectionName = $"MEASUREMENT_{index}";
                    m_objMeasurementInitializeParameter[(int)index].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), (int)CommunicationParameter.EType.TYPE_SERIAL_PORT);
                    //m_objMeasurementInitializeParameter[(int)index].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), $"192.168.0.1");
                    //m_objMeasurementInitializeParameter[(int)index].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 0);
                    m_objMeasurementInitializeParameter[(int)index].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), $"COM{defaultStartPortIndex + (int)index}");
                    m_objMeasurementInitializeParameter[(int)index].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 19200);
                    m_objMeasurementInitializeParameter[(int)index].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objMeasurementInitializeParameter[(int)index].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), (int)CommunicationParameter.ESerialPortParity.PARITY_NONE);
                    m_objMeasurementInitializeParameter[(int)index].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), (int)CommunicationParameter.ESerialPortStopBits.STOP_BITS_ONE);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private bool SaveMeasurementInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EMeasurement Index in Enum.GetValues(typeof(CDefine.EMeasurement)))
                {
                    int index = (int)Index;
                    string sectionName = $"MEASUREMENT_{Index}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objMeasurementInitializeParameter[index].eType);
                    //objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objMeasurementInitializeParameter[index].strSocketIPAddress);
                    //objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objMeasurementInitializeParameter[index].iSocketPortNumber);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objMeasurementInitializeParameter[index].strSerialPortName);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objMeasurementInitializeParameter[index].iSerialPortBaudrate);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objMeasurementInitializeParameter[index].iSerialPortDataBits);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objMeasurementInitializeParameter[index].eParity);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objMeasurementInitializeParameter[index].eStopBits);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public bool SaveMeasurementInitializeParameter(CDefine.EMeasurement index, CMeasurementInitializeParameter parameter)
        {

            string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
            ClassINI objINI = new ClassINI(strPath);

            string sectionName = $"MEASUREMENT_{index}";
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)parameter.eType);
            //objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), parameter.strSocketIPAddress);
            //objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), parameter.iSocketPortNumber);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), parameter.strSerialPortName);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), parameter.iSerialPortBaudrate);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), parameter.iSerialPortDataBits);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)parameter.eParity);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)parameter.eStopBits);
            m_objMeasurementInitializeParameter[(int)index] = parameter.DeepClone();

            return true;
        }
    }
}
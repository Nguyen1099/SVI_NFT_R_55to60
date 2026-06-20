using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 각도 센서 파라미터
        /// </summary>
        [Serializable]
        public class CAngleSensorParameter : CommunicationParameter
        {
        }

        /// <summary>
        /// 각도 센서 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CAngleSensorParameter GetAngleSensorParameter(CDefine.EAngularSensor index) => m_objAngleSensorParameter[(int)index];

        /// <summary>
        /// 각도 센서 파라미터 선언
        /// </summary>
        private CAngleSensorParameter[] m_objAngleSensorParameter = new CAngleSensorParameter[Enum.GetNames(typeof(CDefine.EAngularSensor)).Length];

        /// <summary>
        /// 각도 센서 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadAngleSensorParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EAngularSensor sensorIndex in Enum.GetValues(typeof(CDefine.EAngularSensor)))
                {
                    int index = (int)sensorIndex;
                    string sectionName = $"ANGLE_SENSOR_{sensorIndex}";
                    m_objAngleSensorParameter[index].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), 2);
                    m_objAngleSensorParameter[index].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), "");
                    m_objAngleSensorParameter[index].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 0);
                    m_objAngleSensorParameter[index].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), $"COM{14 + index}");
                    m_objAngleSensorParameter[index].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 19200);
                    m_objAngleSensorParameter[index].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objAngleSensorParameter[index].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), 0);
                    m_objAngleSensorParameter[index].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), 1);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 각도 센서 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveAngleSensorParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EAngularSensor sensorIndex in Enum.GetValues(typeof(CDefine.EAngularSensor)))
                {
                    int index = (int)sensorIndex;
                    string sectionName = $"ANGLE_SENSOR_{sensorIndex}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objAngleSensorParameter[index].eType);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objAngleSensorParameter[index].strSocketIPAddress);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objAngleSensorParameter[index].iSocketPortNumber);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objAngleSensorParameter[index].strSerialPortName);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objAngleSensorParameter[index].iSerialPortBaudrate);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objAngleSensorParameter[index].iSerialPortDataBits);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objAngleSensorParameter[index].eParity);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objAngleSensorParameter[index].eStopBits);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 각도 센서 파라미터 저장
        /// </summary>
        /// <param name="setParameter"></param>
        /// <returns></returns>
        public bool SaveAngleSensorParameter(CDefine.EAngularSensor sensorIndex, CAngleSensorParameter setParameter)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                int index = (int)sensorIndex;
                string sectionName = $"ANGLE_SENSOR_{sensorIndex}";
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)setParameter.eType);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), setParameter.strSocketIPAddress);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), setParameter.iSocketPortNumber);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), setParameter.strSerialPortName);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), setParameter.iSerialPortBaudrate);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), setParameter.iSerialPortDataBits);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)setParameter.eParity);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)setParameter.eStopBits);
                m_objAngleSensorParameter[index] = setParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
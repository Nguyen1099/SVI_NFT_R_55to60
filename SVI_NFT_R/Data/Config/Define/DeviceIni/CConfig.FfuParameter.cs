using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// FFU 파라미터
        /// </summary>
        [Serializable]
        public class CFfuParameter : CommunicationParameter
        {
            public int iUnitId;
        }

        /// <summary>
        /// FFU 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CFfuParameter GetFfuParameter(CDefine.EMcu index) => m_objFfuParameter[(int)index];

        /// <summary>
        /// FFU파라미터 선언
        /// </summary>
        private CFfuParameter[] m_objFfuParameter = new CFfuParameter[Enum.GetNames(typeof(CDefine.EMcu)).Length];

        /// <summary>
        /// FFU 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadFfuParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EMcu index in Enum.GetValues(typeof(CDefine.EMcu)))
                {
                    if (m_objFfuParameter[(int)index] == null)
                    {
                        m_objFfuParameter[(int)index] = new CFfuParameter();
                    }
                    string sectionName = $"FFU_MCU32_{index}";
                    m_objFfuParameter[(int)index].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), 1);
                    m_objFfuParameter[(int)index].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), "");
                    m_objFfuParameter[(int)index].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 0);
                    m_objFfuParameter[(int)index].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), $"COM{24 + (int)index}");
                    m_objFfuParameter[(int)index].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 9600);
                    m_objFfuParameter[(int)index].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objFfuParameter[(int)index].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), 0);
                    m_objFfuParameter[(int)index].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), 1);
                    m_objFfuParameter[(int)index].iUnitId = objINI.GetInt32(sectionName, nameof(CFfuParameter.iUnitId), 1);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FFU 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveFfuParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EMcu index in Enum.GetValues(typeof(CDefine.EMcu)))
                {
                    string sectionName = $"FFU_MCU32_{index}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objFfuParameter[(int)index].eType);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objFfuParameter[(int)index].strSocketIPAddress);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objFfuParameter[(int)index].iSocketPortNumber);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objFfuParameter[(int)index].strSerialPortName);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objFfuParameter[(int)index].iSerialPortBaudrate);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objFfuParameter[(int)index].iSerialPortDataBits);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objFfuParameter[(int)index].eParity);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objFfuParameter[(int)index].eStopBits);
                    objINI.WriteValue(sectionName, nameof(CFfuParameter.iUnitId), (int)m_objFfuParameter[(int)index].iUnitId);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FFU 파라미터 저장
        /// </summary>
        /// <param name="objFfuParameter"></param>
        /// <returns></returns>
        public bool SaveFfuParameter(CFfuParameter objFfuParameter, CDefine.EMcu index)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);
                string sectionName = $"FFU_MCU32_{index}";
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)objFfuParameter.eType);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), objFfuParameter.strSocketIPAddress);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), objFfuParameter.iSocketPortNumber);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), objFfuParameter.strSerialPortName);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), objFfuParameter.iSerialPortBaudrate);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), objFfuParameter.iSerialPortDataBits);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)objFfuParameter.eParity);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)objFfuParameter.eStopBits);
                objINI.WriteValue(sectionName, nameof(CFfuParameter.iUnitId), (int)objFfuParameter.iUnitId);
                m_objFfuParameter[(int)index] = objFfuParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 얼라인 초기화 설정
        /// </summary>
        [Serializable]
        public class CAlignInitializeParameter : CommunicationParameter
        {
        }

        /// <summary>
        /// 얼라인 초기화 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CAlignInitializeParameter GetAlignInitializeParameter(CDefine.EAlign index) => m_objAlignInitializeParameter[(int)index];

        /// <summary>
        /// 얼라인 초기화 파라미터 선언
        /// </summary>
        private CAlignInitializeParameter[] m_objAlignInitializeParameter = new CAlignInitializeParameter[Enum.GetNames(typeof(CDefine.EAlign)).Length];

        /// <summary>
        /// 얼라인 초기화 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadAlignInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EAlign deviceIndex in Enum.GetValues(typeof(CDefine.EAlign)))
                {
                    int index = (int)deviceIndex;
                    if (m_objAlignInitializeParameter[index] == null)
                    {
                        m_objAlignInitializeParameter[index] = new CAlignInitializeParameter();
                    }
                    string sectionName = $"ALIGN_INITIALIZE_{deviceIndex}";
                    m_objAlignInitializeParameter[index].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), 0);
                    m_objAlignInitializeParameter[index].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), "192.168.0.0");
                    m_objAlignInitializeParameter[index].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 10000);
                    m_objAlignInitializeParameter[index].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), "");
                    m_objAlignInitializeParameter[index].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 0);
                    m_objAlignInitializeParameter[index].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objAlignInitializeParameter[index].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), 0);
                    m_objAlignInitializeParameter[index].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), 0);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 얼라인 초기화 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveAlignInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EAlign deviceIndex in Enum.GetValues(typeof(CDefine.EAlign)))
                {
                    int index = (int)deviceIndex;
                    string sectionName = $"ALIGN_INITIALIZE_{deviceIndex}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objAlignInitializeParameter[index].eType);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objAlignInitializeParameter[index].strSocketIPAddress);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objAlignInitializeParameter[index].iSocketPortNumber);
                    //objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objAlignInitializeParameter[index].strSerialPortName);
                    //objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objAlignInitializeParameter[index].iSerialPortBaudrate);
                    //objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objAlignInitializeParameter[index].iSerialPortDataBits);
                    //objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objAlignInitializeParameter[index].eParity);
                    //objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objAlignInitializeParameter[index].eStopBits);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 얼라인 초기화 파라미터 저장
        /// </summary>
        /// <param name="initializeParameter"></param>
        /// <returns></returns>
        public bool SaveAlignInitializeParameter(CDefine.EAlign deviceIndex, CAlignInitializeParameter initializeParameter)
        {
            string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
            ClassINI objINI = new ClassINI(strPath);

            int index = (int)deviceIndex;
            string sectionName = $"ALIGN_INITIALIZE_{deviceIndex}";
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)initializeParameter.eType);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), initializeParameter.strSocketIPAddress);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), initializeParameter.iSocketPortNumber);
            //objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), initializeParameter.strSerialPortName);
            //objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), initializeParameter.iSerialPortBaudrate);
            //objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), initializeParameter.iSerialPortDataBits);
            //objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)initializeParameter.eParity);
            //objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)initializeParameter.eStopBits);
            m_objAlignInitializeParameter[index] = initializeParameter.DeepClone();

            return true;
        }
    }
}
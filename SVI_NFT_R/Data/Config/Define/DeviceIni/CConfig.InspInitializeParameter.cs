using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// SDV 삼성 광학계 초기화 설정
        /// </summary>
        [Serializable]
        public class CInspInitializeParameter : CommunicationParameter
        {
        }

        /// <summary>
        /// SDV 초기화 파라미터 리턴
        /// </summary>
        /// <returns></returns>
        public CInspInitializeParameter GetInspInitializeParameter(CDefine.EInspInterface index) => m_objInspInitializeParameter[(int)index];

        /// <summary>
        /// SDV 광학 초기화 파라미터 선언
        /// </summary>
        private readonly CInspInitializeParameter[] m_objInspInitializeParameter = new CInspInitializeParameter[Enum.GetNames(typeof(CDefine.EInspInterface)).Length];

        /// <summary>
        /// SDV 비전 초기화 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadInspInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EInspInterface AmoIndex in Enum.GetValues(typeof(CDefine.EInspInterface)))
                {
                    int index = (int)AmoIndex;
                    if (m_objInspInitializeParameter[index] == null)
                    {
                        m_objInspInitializeParameter[index] = new CInspInitializeParameter();
                    }
                    string sectionName = $"SVI_AMO_{AmoIndex}";
                    m_objInspInitializeParameter[index].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), 0);
                    m_objInspInitializeParameter[index].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), "192.168.0.0");
                    m_objInspInitializeParameter[index].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 10000);
                    m_objInspInitializeParameter[index].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), "");
                    m_objInspInitializeParameter[index].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 0);
                    m_objInspInitializeParameter[index].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objInspInitializeParameter[index].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), 0);
                    m_objInspInitializeParameter[index].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), 0);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// SDV 비전 초기화 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveInspInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                foreach (CDefine.EInspInterface AmoIndex in Enum.GetValues(typeof(CDefine.EInspInterface)))
                {
                    int index = (int)AmoIndex;
                    string sectionName = $"SVI_AMO_{AmoIndex}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objInspInitializeParameter[index].eType);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objInspInitializeParameter[index].strSocketIPAddress);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objInspInitializeParameter[index].iSocketPortNumber);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objInspInitializeParameter[index].strSerialPortName);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objInspInitializeParameter[index].iSerialPortBaudrate);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objInspInitializeParameter[index].iSerialPortDataBits);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objInspInitializeParameter[index].eParity);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objInspInitializeParameter[index].eStopBits);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// SDV 비전 초기화 파라미터 저장
        /// </summary>
        /// <param name="objSDVInitializeParameter"></param>     
        /// <returns></returns>
        public bool SaveInspInitializeParameter(CDefine.EInspInterface AmoIndex, CInspInitializeParameter objSDVInitializeParameter)
        {

            string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
            ClassINI objINI = new ClassINI(strPath);

            int index = (int)AmoIndex;
            string sectionName = $"SVI_AMO_{AmoIndex}";
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)objSDVInitializeParameter.eType);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), objSDVInitializeParameter.strSocketIPAddress);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), objSDVInitializeParameter.iSocketPortNumber);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), objSDVInitializeParameter.strSerialPortName);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), objSDVInitializeParameter.iSerialPortBaudrate);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), objSDVInitializeParameter.iSerialPortDataBits);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)objSDVInitializeParameter.eParity);
            objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)objSDVInitializeParameter.eStopBits);
            m_objInspInitializeParameter[index] = objSDVInitializeParameter.DeepClone();

            return true;
        }
    }
}
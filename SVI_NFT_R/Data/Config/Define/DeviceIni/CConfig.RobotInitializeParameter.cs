using System;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 나찌 TCP통신 모듈 데이터
        /// </summary>
        [Serializable]
        public class CNachiInitializeParameter : CommunicationParameter
        {
            public enum EDataEncoding
            {
                ENCODING_NONE = 0,
                ENCODING_DEFAULT,
                ENCODING_UCS2
            };

            public EDataEncoding eDataEncoding;
            public int iDataCount;
        }

        /// <summary>
        /// 나찌 통신 파라미터 리턴
        /// </summary>
        /// <param name="eNachiPosition"></param>
        /// <returns></returns>
        public CNachiInitializeParameter GetNachiInitializeParameter(CDefine.ENachiComm eNachiPosition) => m_objNachiInitializeParameter[(int)eNachiPosition];

        /// <summary>
        /// 나찌 TCP통신 파라미터 선언
        /// </summary>
        private readonly CNachiInitializeParameter[] m_objNachiInitializeParameter = new CNachiInitializeParameter[Enum.GetNames(typeof(CDefine.ENachiComm)).Length];

        /// <summary>
        /// 나찌 통신 초기화 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadNachiInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                int nachiCommCount = Enum.GetNames(typeof(CDefine.ENachiComm)).Length;
                for (int iLoopCount = 0; iLoopCount < nachiCommCount; iLoopCount++)
                {
                    if (m_objNachiInitializeParameter[iLoopCount] == null)
                    {
                        m_objNachiInitializeParameter[iLoopCount] = new CNachiInitializeParameter();
                    }
                    string sectionName = $"{(CDefine.ENachiComm)iLoopCount}";
                    m_objNachiInitializeParameter[iLoopCount].eType = (CommunicationParameter.EType)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eType), 1);
                    m_objNachiInitializeParameter[iLoopCount].eDataEncoding = (CNachiInitializeParameter.EDataEncoding)objINI.GetInt32(sectionName, "eDataEncoding", (int)CNachiInitializeParameter.EDataEncoding.ENCODING_NONE);
                    m_objNachiInitializeParameter[iLoopCount].strSocketIPAddress = objINI.GetString(sectionName, nameof(CommunicationParameter.strSocketIPAddress), "192.168.0.0");
                    m_objNachiInitializeParameter[iLoopCount].iSocketPortNumber = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSocketPortNumber), 1000);
                    m_objNachiInitializeParameter[iLoopCount].strSerialPortName = objINI.GetString(sectionName, nameof(CommunicationParameter.strSerialPortName), "");
                    m_objNachiInitializeParameter[iLoopCount].iSerialPortBaudrate = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), 0);
                    m_objNachiInitializeParameter[iLoopCount].iSerialPortDataBits = objINI.GetInt32(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), 8);
                    m_objNachiInitializeParameter[iLoopCount].eParity = (CommunicationParameter.ESerialPortParity)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eParity), 0);
                    m_objNachiInitializeParameter[iLoopCount].eStopBits = (CommunicationParameter.ESerialPortStopBits)objINI.GetInt32(sectionName, nameof(CommunicationParameter.eStopBits), 0);
                    switch ((CDefine.ENachiComm)iLoopCount)
                    {
                        case CDefine.ENachiComm.InNachiOffset:
                            m_objNachiInitializeParameter[iLoopCount].iDataCount = objINI.GetInt32(sectionName, "iDataCount", Constants.INITPARAM_IN_ROBOT_OFFSET_BYTE_COUNT);
                            break;

                        case CDefine.ENachiComm.InNachiRms:
                            m_objNachiInitializeParameter[iLoopCount].iDataCount = objINI.GetInt32(sectionName, "iDataCount", Constants.INITPARAM_IN_ROBOT_RMS_BYTE_COUNT);
                            break;

                        case CDefine.ENachiComm.OutNachiOffset:
                            m_objNachiInitializeParameter[iLoopCount].iDataCount = objINI.GetInt32(sectionName, "iDataCount", Constants.INITPARAM_OUT_ROBOT_OFFSET_BYTE_COUNT);
                            break;

                        case CDefine.ENachiComm.OutNachiRms:
                            m_objNachiInitializeParameter[iLoopCount].iDataCount = objINI.GetInt32(sectionName, "iDataCount", Constants.INITPARAM_OUT_ROBOT_RMS_BYTE_COUNT);
                            break;

                        default:
                            Debug.Assert(false);
                            break;
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 나찌 통신 초기화 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveNachiInitializeParameter()
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);
                int nachiCommCount = Enum.GetNames(typeof(CDefine.ENachiComm)).Length;
                for (int iLoopCount = 0; iLoopCount < nachiCommCount; iLoopCount++)
                {
                    string sectionName = $"{(CDefine.ENachiComm)iLoopCount}";
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)m_objNachiInitializeParameter[iLoopCount].eType);
                    objINI.WriteValue(sectionName, "eDataEncoding", (int)m_objNachiInitializeParameter[iLoopCount].eDataEncoding);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), m_objNachiInitializeParameter[iLoopCount].strSocketIPAddress);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), m_objNachiInitializeParameter[iLoopCount].iSocketPortNumber);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), m_objNachiInitializeParameter[iLoopCount].strSerialPortName);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), m_objNachiInitializeParameter[iLoopCount].iSerialPortBaudrate);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), m_objNachiInitializeParameter[iLoopCount].iSerialPortDataBits);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)m_objNachiInitializeParameter[iLoopCount].eParity);
                    objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)m_objNachiInitializeParameter[iLoopCount].eStopBits);
                    objINI.WriteValue(sectionName, "iDataCount", m_objNachiInitializeParameter[iLoopCount].iDataCount);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 나찌 통신 초기화 파라미터 저장
        /// </summary>
        /// <param name="eNachiPosition"></param>
        /// <param name="objNachiInitializeParameter"></param>
        /// <returns></returns>
        public bool SaveNachiInitializeParameter(CDefine.ENachiComm eNachiPosition, CNachiInitializeParameter objNachiInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_DEVICE_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"{eNachiPosition}";
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eType), (int)objNachiInitializeParameter.eType);
                objINI.WriteValue(sectionName, "eDataEncoding", (int)objNachiInitializeParameter.eDataEncoding);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSocketIPAddress), objNachiInitializeParameter.strSocketIPAddress);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSocketPortNumber), objNachiInitializeParameter.iSocketPortNumber);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.strSerialPortName), objNachiInitializeParameter.strSerialPortName);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortBaudrate), objNachiInitializeParameter.iSerialPortBaudrate);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.iSerialPortDataBits), objNachiInitializeParameter.iSerialPortDataBits);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eParity), (int)objNachiInitializeParameter.eParity);
                objINI.WriteValue(sectionName, nameof(CommunicationParameter.eStopBits), (int)objNachiInitializeParameter.eStopBits);
                objINI.WriteValue(sectionName, "iDataCount", objNachiInitializeParameter.iDataCount);
                m_objNachiInitializeParameter[(int)eNachiPosition] = objNachiInitializeParameter.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
using HLDevice.Abstract;
using SVI_NFT_R;

namespace HLDevice
{
    public class CDeviceFFU
    {
        public CDeviceFFUAbstract m_objFFU { get; private set; }
        private CDocument m_objDocument;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument document, CConfig.CFfuParameter objFfuParameter)
        {
            m_objDocument = document;

            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                m_objFFU = new FFU.CDeviceFFUVirtual();
            }
            else
            {
                m_objFFU = new FFU.CDeviceFFUMCU();
            }

            CDeviceFFUAbstract.CInitializeParameter objInitializeParameter = new CDeviceFFUAbstract.CInitializeParameter();
            objInitializeParameter.eType = (CDeviceFFUAbstract.CInitializeParameter.EType)objFfuParameter.eType;
            objInitializeParameter.strSocketIPAddress = objFfuParameter.strSocketIPAddress;
            objInitializeParameter.iSocketPortNumber = objFfuParameter.iSocketPortNumber;
            objInitializeParameter.strSerialPortName = objFfuParameter.strSerialPortName;
            objInitializeParameter.iSerialPortBaudrate = objFfuParameter.iSerialPortBaudrate;
            objInitializeParameter.iSerialPortDataBits = objFfuParameter.iSerialPortDataBits;
            objInitializeParameter.eParity = (CDeviceFFUAbstract.CInitializeParameter.ESerialPortParity)objFfuParameter.eParity;
            objInitializeParameter.eStopBits = (CDeviceFFUAbstract.CInitializeParameter.ESerialPortStopBits)objFfuParameter.eStopBits;
            objInitializeParameter.iUnitID = objFfuParameter.iUnitId;
            return m_objFFU.HLInitialize(objInitializeParameter);
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objFFU.HLDeInitialize();
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objFFU.HLGetVersion();
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public bool HLIsConnected()
        {
            return m_objFFU.HLIsConnected();
        }

        /// <summary>
        /// 설정된 속도값
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public bool HLGetRequiredSpeed(int iSlave, int iFFUIndex, out int iValue)
        {
            iValue = 0;
            return m_objFFU.HLGetRequiredSpeed(iSlave, iFFUIndex, out iValue);
        }

        /// <summary>
        /// 속도 설정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public bool HLSetRequiredSpeed(int iSlave, int iFFUIndex, int iValue)
        {
            return m_objFFU.HLSetRequiredSpeed(iSlave, iFFUIndex, iValue);
        }

        /// <summary>
        /// 전체 속도
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public bool HLSetAllRequiredSpeed(int iSlave, int iValue)
        {
            return m_objFFU.HLSetAllRequiredSpeed(iSlave, iValue);
        }

        /// <summary>
        /// 현재 속도 리딩
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public bool HLGetCurrentSpeed(int iSlave, int iFFUIndex, out int iValue)
        {
            return m_objFFU.HLGetCurrentSpeed(iSlave, iFFUIndex, out iValue);
        }


        /// <summary>
        /// 상태
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="objStatus"></param>
        /// <returns></returns>
        public bool HLGetStatus(int iSlave, int iFFUIndex, out HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus objStatus)
        {
            objStatus = new HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus();
            return m_objFFU.HLGetStatus(iSlave, iFFUIndex, out objStatus);
        }


        /// <summary>
        /// 리셋
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="bReset"></param>
        /// <returns></returns>
        public bool HLSetReset(int iSlave, int iFFUIndex, bool bReset)
        {
            return m_objFFU.HLSetReset(iSlave, iFFUIndex, bReset);
        }

        /// <summary>
        /// 전체 리셋
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="bReset"></param>
        /// <returns></returns>
        public bool HLSetAllReset(int iSlave, bool bReset)
        {
            return m_objFFU.HLSetAllReset(iSlave, bReset);
        }

        /// <summary>
        /// 모든 채널에 팬 속도 값
        /// </summary>
        /// <param name="readValues"></param>
        /// <returns></returns>
        public bool HLGetFanSpeedAll(out int[] readValues)
        {
            return m_objFFU.HLGetFanSpeedAll(out readValues);
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public HLDevice.Abstract.CDeviceFFUAbstract.CFFUError HLGetErrorCode()
        {
            return m_objFFU.HLGetErrorCode();
        }

        /// <summary>
        /// 에러 로그 인스턴스를 가져온다.
        /// </summary>
        /// <returns>null=에러 로그 기능 없음, other=성공</returns>
        public DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return m_objFFU.GetErrorLogInstanceOrNull();
        }

        /// <summary>
        /// 통신 패킷 송수신 로그 인스턴스를 가져온다.
        /// </summary>
        /// <returns>null=통신 로그 기능 없음, other=성공</returns>
        public DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return m_objFFU.GetCommLogInstanceOrNull();
        }
    }
}

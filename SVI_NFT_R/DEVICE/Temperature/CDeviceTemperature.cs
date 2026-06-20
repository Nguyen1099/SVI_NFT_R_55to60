using HLDevice.Abstract;
using SVI_NFT_R;

namespace HLDevice
{
    public class CDeviceTemperature
    {
        public CDeviceTemperatureAbstract m_objTemperature { get; private set; }
        private CDocument m_objDocument;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument document, CConfig.CTemperatureParameter objTemperatureParameter)
        {
            m_objDocument = document;

            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                m_objTemperature = new Temperature.CDeviceTemperatureVirtual();
            }
            else
            {
                m_objTemperature = new Temperature.CDeviceTemperatureTK4S();
            }

            CDeviceTemperatureAbstract.CInitializeParameter objInitializeParameter = new CDeviceTemperatureAbstract.CInitializeParameter();
            objInitializeParameter.eType = (CDeviceTemperatureAbstract.CInitializeParameter.EType)objTemperatureParameter.eType;
            objInitializeParameter.strSocketIPAddress = objTemperatureParameter.strSocketIPAddress;
            objInitializeParameter.iSocketPortNumber = objTemperatureParameter.iSocketPortNumber;
            objInitializeParameter.strSerialPortName = objTemperatureParameter.strSerialPortName;
            objInitializeParameter.iSerialPortBaudrate = objTemperatureParameter.iSerialPortBaudrate;
            objInitializeParameter.iSerialPortDataBits = objTemperatureParameter.iSerialPortDataBits;
            objInitializeParameter.eParity = (CDeviceTemperatureAbstract.CInitializeParameter.ESerialPortParity)objTemperatureParameter.eParity;
            objInitializeParameter.eStopBits = (CDeviceTemperatureAbstract.CInitializeParameter.ESerialPortStopBits)objTemperatureParameter.eStopBits;

            return m_objTemperature.HLInitialize(objInitializeParameter);
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objTemperature.HLDeInitialize();
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objTemperature.HLGetVersion();
        }

        /// <summary>
        /// 현재 온도값 리딩
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="dPVData"></param>
        /// <returns></returns>
        public bool HLGetPVData(int iSlave, out double dPVData)
        {
            return m_objTemperature.HLGetPVData(iSlave, out dPVData);
        }

        /// <summary>
        /// 현재 온도값 리딩
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="dPVData"></param>
        /// <returns></returns>
        public bool HLReadTemp(byte byteID, out double dTempData)
        {
            return m_objTemperature.HLReadTemp(byteID, out dTempData);
        }

        /// <summary>
        /// 설정된 온도값 리딩
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iInputData"></param>
        /// <returns></returns>
        public bool HLGetInputData(int iSlave, out int iInputData)
        {
            iInputData = 0;
            int iAddress = 1003;
            int iSettingData;
            if (m_objTemperature.HLReadInputRegisters(iSlave, iAddress, 1, out iSettingData) == false)
            {
                return false;
            }
            iInputData = iSettingData;
            return true;
        }


        /// <summary>
        /// 연결 상태 확인
        /// </summary>
        /// <returns></returns>
        public bool HLIsConnected()
        {
            return m_objTemperature.HLIsConnected();
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError HLGetErrorCode()
        {
            return m_objTemperature.HLGetErrorCode();
        }

        /// <summary>
        /// 에러 로그 인스턴스를 가져온다.
        /// </summary>
        /// <returns>null=에러 로그 기능 없음, other=성공</returns>
        public DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return m_objTemperature.GetErrorLogInstanceOrNull();
        }

        /// <summary>
        /// 통신 패킷 송수신 로그 인스턴스를 가져온다.
        /// </summary>
        /// <returns>null=통신 로그 기능 없음, other=성공</returns>
        public DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return m_objTemperature.GetCommLogInstanceOrNull();
        }
    }
}

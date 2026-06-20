using HLDevice.Abstract;
using SVI_NFT_R;

namespace HLDevice
{
    public class CDeviceElectrostatic
    {
        public CDeviceElectrostaticAbstract m_objElectrostatic { get; private set; }
        private CDocument m_objDocument;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument document, CConfig.CElectrostaticParameter objInitializeParameter)
        {
            m_objDocument = document;

            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                m_objElectrostatic = new Electrostatic.CDeviceElectrostaticVirtual();
            }
            else
            {
                m_objElectrostatic = new Electrostatic.CDeviceElectrostaticPGMS();
            }
            CDeviceElectrostaticAbstract.CInitializeParameter objDeviceElectrostaticParameter = new CDeviceElectrostaticAbstract.CInitializeParameter();
            CConfig.CElectrostaticParameter objElectrostaticParameter = objInitializeParameter;
            objDeviceElectrostaticParameter.eType = (CDeviceElectrostaticAbstract.CInitializeParameter.EType)objElectrostaticParameter.eType;
            objDeviceElectrostaticParameter.strSocketIPAddress = objElectrostaticParameter.strSocketIPAddress;
            objDeviceElectrostaticParameter.iSocketPortNumber = objElectrostaticParameter.iSocketPortNumber;
            objDeviceElectrostaticParameter.strSerialPortName = objElectrostaticParameter.strSerialPortName;
            objDeviceElectrostaticParameter.iSerialPortBaudrate = objElectrostaticParameter.iSerialPortBaudrate;
            objDeviceElectrostaticParameter.iSerialPortDataBits = objElectrostaticParameter.iSerialPortDataBits;
            objDeviceElectrostaticParameter.eParity = (CDeviceElectrostaticAbstract.CInitializeParameter.ESerialPortParity)objElectrostaticParameter.eParity;
            objDeviceElectrostaticParameter.eStopBits = (CDeviceElectrostaticAbstract.CInitializeParameter.ESerialPortStopBits)objElectrostaticParameter.eStopBits;

            return m_objElectrostatic.HLInitialize(objDeviceElectrostaticParameter);
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objElectrostatic.HLDeInitialize();
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objElectrostatic.HLGetVersion();
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public bool HLIsConnected()
        {
            return m_objElectrostatic.HLIsConnected();
        }

        /// <summary>
        /// 저항측정기에 사용될 닉네임 설정
        /// </summary>
        /// <param name="iAddress"></param>
        /// <param name="strNickName"></param>
        /// <returns></returns>
        public bool HLSaveNickName(int iAddress, string strNickName)
        {
            return m_objElectrostatic.HLSaveNickName(iAddress, strNickName);
        }

        /// <summary>
        /// 저항측정기에서 사용되는 닉네임 로드
        /// </summary>
        /// <param name="iAddress"></param>
        /// <param name="strNickName"></param>
        /// <returns></returns>
        public bool HLLoadNickName(int iAddress, out string strNickName)
        {
            return m_objElectrostatic.HLLoadNickName(iAddress, out strNickName);
        }

        /// <summary>
        /// 저항 측정값 읽기
        /// </summary>
        /// <param name="strNickName"></param>
        /// <param name="strResult"></param>
        /// <returns></returns>
        public bool HLReadChannel(string strNickName, out string[] strResult)
        {
            return m_objElectrostatic.HLReadChannel(strNickName, out strResult);
        }


        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError HLGetErrorCode()
        {
            return m_objElectrostatic.HLGetErrorCode();
        }

        /// <summary>
        /// 에러 로그 인스턴스를 가져온다.
        /// </summary>
        /// <returns>null=에러 로그 기능 없음, other=성공</returns>
        public DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return m_objElectrostatic.GetErrorLogInstanceOrNull();
        }

        /// <summary>
        /// 통신 패킷 송수신 로그 인스턴스를 가져온다.
        /// </summary>
        /// <returns>null=통신 로그 기능 없음, other=성공</returns>
        public DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return m_objElectrostatic.GetCommLogInstanceOrNull();
        }
    }
}

namespace HLDevice.Electrostatic
{
    public class CDeviceElectrostaticPGMS : Abstract.CDeviceElectrostaticAbstract
    {
        public readonly HLDeviceDLL.Electrostatic.PGMS.CDeviceElectrostaticPGMS m_objElectrostatic = new HLDeviceDLL.Electrostatic.PGMS.CDeviceElectrostaticPGMS();
        private readonly CElectrostaticError m_objError = new CElectrostaticError();

        /// <summary>
        /// 콜백 개체
        /// </summary>
        private CallBackFuntionReceiveData m_objCallback = null;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceElectrostaticPGMS()
        {
        }

        /// <summary>
        /// 해제
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objElectrostatic.GetVersion();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(HLDevice.Abstract.CDeviceElectrostaticAbstract.CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                HLDeviceDLL.Electrostatic.PGMS.CDeviceElectrostaticPGMSDefine.CInitializeParameter objParameter = new HLDeviceDLL.Electrostatic.PGMS.CDeviceElectrostaticPGMSDefine.CInitializeParameter();
                objParameter.eType = (HLDeviceDLL.Electrostatic.PGMS.CDeviceElectrostaticPGMSDefine.CInitializeParameter.enumType)objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = (HLDeviceDLL.Electrostatic.PGMS.CDeviceElectrostaticPGMSDefine.CInitializeParameter.enumSerialPortParity)objInitializeParameter.eParity;
                objParameter.eStopBits = (HLDeviceDLL.Electrostatic.PGMS.CDeviceElectrostaticPGMSDefine.CInitializeParameter.enumSerialPortStopBits)objInitializeParameter.eStopBits;

                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;

                if (false == m_objElectrostatic.HLInitialize(objParameter))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void HLDeInitialize()
        {
            m_objElectrostatic.HLDeInitialize();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
        {
            return m_objElectrostatic.HLIsConnected();
        }

        /// <summary>
        /// 콜백 연결
        /// </summary>
        /// <param name="objReceiveData"></param>
        public override void SetCallbackFunction(HLDevice.Abstract.CDeviceElectrostaticAbstract.CallBackFuntionReceiveData objReceiveData)
        {
            m_objCallback = objReceiveData;
        }

        /// <summary>
        /// PGMS에서 사용되는 닉네임 설정
        /// </summary>
        /// <param name="iAddress"></param>
        /// <param name="strNickName"></param>
        /// <returns></returns>
        public override bool HLSaveNickName(int iAddress, string strNickName)
        {
            bool bReturn = false;

            do
            {
                if (m_objElectrostatic.HLSaveNickName(iAddress, strNickName) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// PGMS에서 사용되는 닉네임 로드
        /// </summary>
        /// <param name="iAddress"></param>
        /// <param name="strNickName"></param>
        /// <returns></returns>
        public override bool HLLoadNickName(int iAddress, out string strNickName)
        {
            bool bReturn = false;
            strNickName = "";

            do
            {
                if (m_objElectrostatic.HLLoadNickName(iAddress, out strNickName) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// PGMS에서 측정한 저항값 리드
        /// </summary>
        /// <param name="strNickName"></param>
        /// <param name="strResult"></param>
        /// <returns></returns>
        public override bool HLReadChannel(string strNickName, out string[] strResult)
        {
            bool bReturn = false;

            do
            {
                if (m_objElectrostatic.HLReadChannel(strNickName, out strResult) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {
            HLDeviceDLL.Electrostatic.PGMS.CDeviceElectrostaticPGMSDefine.CPGMSError objError = m_objElectrostatic.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError HLGetErrorCode()
        {
            return (HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError)m_objError.Clone();
        }

        public override DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return m_objElectrostatic;
        }

        public override DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return m_objElectrostatic;
        }
    }
}

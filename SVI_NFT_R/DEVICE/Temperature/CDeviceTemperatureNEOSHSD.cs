namespace HLDevice.Temperature
{
    public class CDeviceTemperatureNEOSHSD : HLDevice.Abstract.CDeviceTemperatureAbstract
    {
        public readonly HLDeviceDLL.Temperature.NEOSHSD.CDeviceTemperatureNEOSHSD m_objTemperature = new HLDeviceDLL.Temperature.NEOSHSD.CDeviceTemperatureNEOSHSD();
        private readonly HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError m_objError = new HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceTemperatureNEOSHSD()
        {
        }

        /// <summary>
        /// 해제
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objTemperature.GetVersion();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(HLDevice.Abstract.CDeviceTemperatureAbstract.CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                HLDeviceDLL.Temperature.NEOSHSD.CDeviceTemperatureNEOSHSDDefine.CInitializeParameter objParameter = new HLDeviceDLL.Temperature.NEOSHSD.CDeviceTemperatureNEOSHSDDefine.CInitializeParameter();
                objParameter.eType = (HLDeviceDLL.Temperature.NEOSHSD.CDeviceTemperatureNEOSHSDDefine.CInitializeParameter.enumType)objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = (HLDeviceDLL.Temperature.NEOSHSD.CDeviceTemperatureNEOSHSDDefine.CInitializeParameter.enumSerialPortParity)objInitializeParameter.eParity;
                objParameter.eStopBits = (HLDeviceDLL.Temperature.NEOSHSD.CDeviceTemperatureNEOSHSDDefine.CInitializeParameter.enumSerialPortStopBits)objInitializeParameter.eStopBits;

                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;
                if (false == m_objTemperature.HLInitialize(objParameter))
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
            m_objTemperature.HLDeInitialize();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
        {
            return m_objTemperature.HLIsConnected();
        }

        /// <summary>
        /// 현재 온도값 리딩
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="dPVData"></param>
        /// <returns></returns>

        public override bool HLReadTemp(byte byteID, out double dTempData)
        {
            bool bReturn = false;
            dTempData = 0;

            do
            {
                if (m_objTemperature.HLReadTemp(byteID, out dTempData) == false)
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
            HLDeviceDLL.Temperature.NEOSHSD.CDeviceTemperatureNEOSHSDDefine.CNeoshsdError objError = m_objTemperature.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError HLGetErrorCode()
        {
            return (HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError)m_objError.Clone();
        }

        public override DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return m_objTemperature;
        }

        public override DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return m_objTemperature;
        }
    }
}

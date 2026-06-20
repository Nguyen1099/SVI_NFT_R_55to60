namespace HLDevice.Temperature
{
    public class CDeviceTemperatureTK4S : HLDevice.Abstract.CDeviceTemperatureAbstract
    {
        private readonly HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError m_objError = new HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError();
        private readonly HLDeviceDLL.Temperature.TK4S.CDeviceTemperatureTK4S m_objTemperature = new HLDeviceDLL.Temperature.TK4S.CDeviceTemperatureTK4S();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceTemperatureTK4S()
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
                HLDeviceDLL.Temperature.TK4S.CDeviceTemperatureTK4SDefine.CInitializeParameter objParameter = new HLDeviceDLL.Temperature.TK4S.CDeviceTemperatureTK4SDefine.CInitializeParameter();
                objParameter.eType = (HLDeviceDLL.Temperature.TK4S.CDeviceTemperatureTK4SDefine.CInitializeParameter.enumType)objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = (HLDeviceDLL.Temperature.TK4S.CDeviceTemperatureTK4SDefine.CInitializeParameter.enumSerialPortParity)objInitializeParameter.eParity;
                objParameter.eStopBits = (HLDeviceDLL.Temperature.TK4S.CDeviceTemperatureTK4SDefine.CInitializeParameter.enumSerialPortStopBits)objInitializeParameter.eStopBits;

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
        public override bool HLGetPVData(int iSlave, out double dPVData)
        {
            bool bReturn = false;

            do
            {
                if (m_objTemperature.HLGetPVData(iSlave, out dPVData) == false)
                {
                    MakeError();
                    break;
                }

                dPVData /= 10d;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLReadTemp(byte byteID, out double dTempData)
        {
            bool bReturn = false;

            do
            {
                string unitIndicator;
                if (m_objTemperature.HLReadTemp(byteID, out dTempData, out unitIndicator) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// 설정된 온도값 리딩
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="bReadData"></param>
        /// <returns></returns>
        public override bool HLReadCoils(int iSlave, int iAddress, int iReadCount, out bool[] bReadData)
        {
            bool bReturn = false;
            bReadData = null;

            do
            {
                if (false == m_objTemperature.HLReadCoils(iSlave, iAddress, iReadCount, out bReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// Modbus WriteSingleCoil
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public override bool HLWriteSingleCoil(int iSlave, int iAddress, bool bData)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objTemperature.HLWriteSingleCoil(iSlave, iAddress, bData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// Modbus ReadInputRegisters - double형 데이터 읽기
        /// 설명 : double형 데이터를 읽을려면 4를 읽어야 가능하기때문에 크기는 고정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="dReadData"></param>
        /// <returns></returns>
        public override bool HLReadInputRegisters(int iSlave, int iAddress, out double dReadData)
        {
            bool bReturn = false;
            dReadData = 0;

            do
            {
                if (false == m_objTemperature.HLReadInputRegisters(iSlave, iAddress, out dReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// Modbus ReadInputRegisters - float형 데이터 읽기
        /// 설명 : float형 데이터를 읽을려면 2를 읽어야 가능하기때문에 크기는 고정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="fReadData"></param>
        /// <returns></returns>
        public override bool HLReadInputRegisters(int iSlave, int iAddress, out float fReadData)
        {
            bool bReturn = false;
            fReadData = 0f;

            do
            {
                if (false == m_objTemperature.HLReadInputRegisters(iSlave, iAddress, out fReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// Modbus ReadInputRegisters - int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="iReadData"></param>
        /// <returns></returns>
        public override bool HLReadInputRegisters(int iSlave, int iAddress, int iReadCount, out int iReadData)
        {
            bool bReturn = false;
            iReadData = 0;

            do
            {
                if (false == m_objTemperature.HLReadInputRegisters(iSlave, iAddress, iReadCount, out iReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// Modbus ReadHoldingRegisters - int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="iReadData"></param>
        /// <returns></returns>
        public override bool HLReadHoldingRegisters(int iSlave, int iAddress, int iReadCount, out int iReadData)
        {
            bool bReturn = false;
            iReadData = 0;

            do
            {
                if (false == m_objTemperature.HLReadHoldingRegisters(iSlave, iAddress, iReadCount, out iReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// Modbus WriteHoldingRegisters
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iData"></param>
        /// <returns></returns>
        public override bool HLWriteHoldingRegisters(int iSlave, int iAddress, int iData)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objTemperature.HLWriteHoldingRegisters(iSlave, iAddress, iData))
                {
                    MakeError();
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
            HLDeviceDLL.Temperature.TK4S.CDeviceTemperatureTK4SDefine.CTk4SError objError = m_objTemperature.HLGetErrorCode();
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

    }
}

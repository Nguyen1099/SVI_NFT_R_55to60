namespace HLDevice.FFU
{
    public class CDeviceFFUMCU : HLDevice.Abstract.CDeviceFFUAbstract
    {
        private readonly HLDevice.Abstract.CDeviceFFUAbstract.CFFUError m_objError = new HLDevice.Abstract.CDeviceFFUAbstract.CFFUError();
        private readonly HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32 m_objFFU = new HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceFFUMCU()
        {
        }

        /// <summary>
        /// 버전정보
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objFFU.GetVersion();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(HLDevice.Abstract.CDeviceFFUAbstract.CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32Define.CInitializeParameter objParameter = new HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32Define.CInitializeParameter();
                objParameter.eType = (HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32Define.CInitializeParameter.enumType)objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = (HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32Define.CInitializeParameter.enumSerialPortParity)objInitializeParameter.eParity;
                objParameter.eStopBits = (HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32Define.CInitializeParameter.enumSerialPortStopBits)objInitializeParameter.eStopBits;

                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;

                if (false == m_objFFU.HLInitialize(objParameter))
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
            m_objFFU.HLDeInitialize();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
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
        public override bool HLGetRequiredSpeed(int iSlave, int iFFUIndex, out int iValue)
        {
            iValue = 0;
            bool bReturn = false;

            do
            {
                if (false == m_objFFU.HLGetRequiredSpeed(iSlave, iFFUIndex, out iValue))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 속도 설정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLSetRequiredSpeed(int iSlave, int iFFUIndex, int iValue)
        {
            bool bReturn = false;

            do
            {
                if (false == m_objFFU.HLSetRequiredSpeed(iSlave, iFFUIndex, iValue))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 전체 속도
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLSetAllRequiredSpeed(int iSlave, int iValue)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFFU.HLSetAllRequiredSpeed(iSlave, iValue))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 속도
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLGetCurrentSpeed(int iSlave, int iFFUIndex, out int iValue)
        {
            iValue = 0;
            bool bReturn = false;
            do
            {
                if (false == m_objFFU.HLGetCurrentSpeed(iSlave, iFFUIndex, out iValue))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// 상태
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="objStatus"></param>
        /// <returns></returns>
        public override bool HLGetStatus(int iSlave, int iFFUIndex, out HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus objStatus)
        {
            objStatus = new HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus();
            bool bReturn = false;

            do
            {
                HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32Define.CMCU32Status objMCU = new HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32Define.CMCU32Status();
                if (false == m_objFFU.HLGetStatus(iSlave, iFFUIndex, out objMCU))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// 리셋
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="bReset"></param>
        /// <returns></returns>
        public override bool HLSetReset(int iSlave, int iFFUIndex, bool bReset)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFFU.HLSetReset(iSlave, iFFUIndex, bReset))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 전체 리셋
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="bReset"></param>
        /// <returns></returns>
        public override bool HLSetAllReset(int iSlave, bool bReset)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFFU.HLSetAllReset(iSlave, bReset))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }



        /// <summary>
        /// 모드버스 ReadCoils
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
                if (false == m_objFFU.HLReadCoils(iSlave, iAddress, iReadCount, out bReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모드버스 WriteSingleCoil
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
                if (false == m_objFFU.HLWriteSingleCoil(iSlave, iAddress, bData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// double형 데이터 읽기
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
                if (false == m_objFFU.HLReadInputRegisters(iSlave, iAddress, out dReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// float형 데이터 읽기
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
                if (false == m_objFFU.HLReadInputRegisters(iSlave, iAddress, out fReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// int형 데이터 읽기
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
                if (false == m_objFFU.HLReadInputRegisters(iSlave, iAddress, iReadCount, out iReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// int형 데이터 읽기
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
                if (false == m_objFFU.HLReadHoldingRegisters(iSlave, iAddress, iReadCount, out iReadData))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="dReadData"></param>
        /// <returns></returns>
        public override bool HLReadHoldingRegisters(int iSlave, int iAddress, int iReadCount, out double dReadData)
        {
            bool bReturn = false;
            dReadData = 0;

            do
            {
                int[] iResult;
                if (false == m_objFFU.HLReadHoldingRegisters(iSlave, iAddress, iReadCount, out iResult))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="iReadData"></param>
        /// <returns></returns>
        public override bool HLReadHoldingRegisters(int iSlave, int iAddress, int iReadCount, out int[] iReadData)
        {
            bool bReturn = false;
            iReadData = null;
            do
            {
                if (false == m_objFFU.HLReadHoldingRegisters(iSlave, iAddress, iReadCount, out iReadData))
                {
                    MakeError();
                    break;
                }


                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
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
                if (false == m_objFFU.HLWriteHoldingRegisters(iSlave, iAddress, iData))
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
            HLDeviceDLL.FFU.MCU32.CDeviceFFUMCU32Define.CMCU32Error objError = m_objFFU.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override HLDevice.Abstract.CDeviceFFUAbstract.CFFUError HLGetErrorCode()
        {
            return (HLDevice.Abstract.CDeviceFFUAbstract.CFFUError)m_objError.Clone();
        }

    }
}

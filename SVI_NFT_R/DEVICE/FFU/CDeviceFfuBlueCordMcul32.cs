using HLDeviceDLL.FFU.BlueCordMcul32Total;
using System;
namespace HLDevice.FFU
{
    public class CDeviceFfuBlueCordMcul32 : Abstract.CDeviceFFUAbstract
    {
        public readonly CDeviceFfuBlueCordMcul32Total m_objFFU = new CDeviceFfuBlueCordMcul32Total();
        private readonly CFFUError m_objError = new CFFUError();

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
        public override bool HLInitialize(CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                CDeviceFfuBlueCordMcul32TotalDefine.CInitializeParameter objParameter = new CDeviceFfuBlueCordMcul32TotalDefine.CInitializeParameter();
                objParameter.eType = (CDeviceFfuBlueCordMcul32TotalDefine.CInitializeParameter.enumType)objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = (CDeviceFfuBlueCordMcul32TotalDefine.CInitializeParameter.enumSerialPortParity)objInitializeParameter.eParity;
                objParameter.eStopBits = (CDeviceFfuBlueCordMcul32TotalDefine.CInitializeParameter.enumSerialPortStopBits)objInitializeParameter.eStopBits;

                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;

                if (false == m_objFFU.HLInitialize(objParameter, bOldVersion: false))
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
            //CFFUStatus state;
            //HLGetStatus(1, 0, out state);
            //bool bRemote = state[CFFUStatus.EStatus.REMOTE_STAUS];
            //bool bConnected = m_objFFU.HLIsConnected() && bRemote;

            //return bConnected;

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
                // Not Supported Function
                m_objError.iReturnCode = -1;
                m_objError.strEventTime = DateTime.Now.ToLongTimeString();
                m_objError.strFunctionName = "HLGetRequiredSpeed";
                m_objError.strMessage = "지원하지 않는 기능입니다.";
                // 실패로 처리함.
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
                if (false == m_objFFU.HLSetFanSpeed(Convert.ToByte(iFFUIndex), iValue))
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
                // Not Supported Function
                m_objError.iReturnCode = -1;
                m_objError.strEventTime = DateTime.Now.ToLongTimeString();
                m_objError.strFunctionName = "HLSetAllRequiredSpeed";
                m_objError.strMessage = "지원하지 않는 기능입니다.";
                // 실패로 처리함.
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
                if (false == m_objFFU.HLGetFanSpeed(Convert.ToByte(iFFUIndex), out iValue))
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
        public override bool HLGetStatus(int iSlave, int iFFUIndex, out CFFUStatus objStatus)
        {
            objStatus = new CFFUStatus();
            bool bReturn = false;

            do
            {
                EAlarmCode status;
                if (false == m_objFFU.HLGetFanStatus(Convert.ToByte(iFFUIndex), out status))
                {
                    MakeError();
                    break;
                }
                if (status.HasFlag(EAlarmCode.Remote) == true)
                {
                    objStatus[CFFUStatus.EStatus.Remote] = true;
                }
                if (status.HasFlag(EAlarmCode.Remote) == true)
                {
                    objStatus[CFFUStatus.EStatus.MotorError] = true;
                }
                if (status == EAlarmCode.NotConnect)
                {
                    objStatus[CFFUStatus.EStatus.NotConnect] = true;
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
                // Not Supported Function
                // 지원하지 않는 기능이나 그냥 성공 했다고 처리함.
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
                // Not Supported Function
                // 지원하지 않는 기능이나 그냥 성공 했다고 처리함.
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모든 채널에 팬 속도 값
        /// </summary>
        /// <param name="readValues"></param>
        /// <returns></returns>
        public override bool HLGetFanSpeedAll(out int[] readValues)
        {
            return m_objFFU.HLGetFanSpeedAll(out readValues);
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
                if (false == m_objFFU.Protocol.TryReadCoils(iSlave, iAddress, iReadCount, out bReadData))
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
                if (false == m_objFFU.Protocol.TryWriteSingleCoil(iSlave, iAddress, bData))
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
                if (false == m_objFFU.Protocol.TryReadInputRegisters(iSlave, iAddress, 4, out int[] readValues))
                {
                    MakeError();
                    break;
                }
                dReadData = m_objFFU.Protocol.ConvertRegistersToDouble(readValues);

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
                if (false == m_objFFU.Protocol.TryReadInputRegisters(iSlave, iAddress, 2, out int[] readValues))
                {
                    MakeError();
                    break;
                }
                fReadData = m_objFFU.Protocol.ConvertRegistersToFloat(readValues);

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
                if (false == m_objFFU.Protocol.TryReadInputRegisters(iSlave, iAddress, 2, out int[] readValues))
                {
                    MakeError();
                    break;
                }
                iReadData = m_objFFU.Protocol.ConvertRegistersToInt(readValues);

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
                if (false == m_objFFU.Protocol.TryReadHoldingRegisters(iSlave, iAddress, 2, out int[] readValues))
                {
                    MakeError();
                    break;
                }
                iReadData = m_objFFU.Protocol.ConvertRegistersToInt(readValues);

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
                if (false == m_objFFU.Protocol.TryReadHoldingRegisters(iSlave, iAddress, 4, out int[] readValues))
                {
                    MakeError();
                    break;
                }
                dReadData = m_objFFU.Protocol.ConvertRegistersToDouble(readValues);

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
                if (false == m_objFFU.Protocol.TryReadHoldingRegisters(iSlave, iAddress, iReadCount, out iReadData))
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
                int[] writeValues = m_objFFU.Protocol.ConvertIntToRegisters(iData);
                if (false == m_objFFU.Protocol.TryWriteMultipleRegisters(iSlave, iAddress, writeValues))
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
            CDeviceFfuBlueCordMcul32TotalDefine.CBlueCordMcul32TotalError objError = m_objFFU.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override CFFUError HLGetErrorCode()
        {
            return (CFFUError)m_objError.Clone();
        }

        public override DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return m_objFFU;
        }

        public override DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return m_objFFU;
        }
    }
}

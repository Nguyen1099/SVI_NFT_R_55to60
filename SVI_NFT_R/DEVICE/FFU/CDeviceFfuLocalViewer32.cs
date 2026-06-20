using HLDeviceDLL.FFU.LocalViewer32;
using System;
namespace HLDevice.FFU
{
    public class CDeviceFfuLocalViewer32 : Abstract.CDeviceFFUAbstract
    {
        public readonly CDeviceMcuLocalViewer32 m_objFFU = new CDeviceMcuLocalViewer32();
        private readonly CFFUError m_objError = new CFFUError();

        /// <summary>
        /// 버전정보
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objFFU.HLGetVersion();
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
                CDeviceMcuLocalViewer32Define.CInitializeParameter objParameter = new CDeviceMcuLocalViewer32Define.CInitializeParameter();
                objParameter.eType = (CDeviceMcuLocalViewer32Define.CInitializeParameter.EType)objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = (CDeviceMcuLocalViewer32Define.CInitializeParameter.ESerialPortParity)objInitializeParameter.eParity;
                objParameter.eStopBits = (CDeviceMcuLocalViewer32Define.CInitializeParameter.ESerialPortStopBits)objInitializeParameter.eStopBits;
                objParameter.iUnitID = objInitializeParameter.iUnitID;

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
            bool bReturn = false;

            do
            {
                byte fanIndex = Convert.ToByte(iFFUIndex);
                if (m_objFFU.HLGetSettingFanSpeed(fanIndex, out iValue) == false)
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
                byte fanID = Convert.ToByte(iFFUIndex + 1);
                if (m_objFFU.HLSetFanSpeed(fanID, fanID, iValue) == false)
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
                if (m_objFFU.HLSetFanSpeed(1, 32, iValue) == false)
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
                EMcuStatus status;
                if (false == m_objFFU.HLGetFanStatus(Convert.ToByte(iFFUIndex), out status))
                {
                    MakeError();
                    break;
                }
                if (status.HasFlag(EMcuStatus.NotRemote) == true)
                {
                    objStatus[CFFUStatus.EStatus.NotRemote] = true;
                }
                if (status.HasFlag(EMcuStatus.PowerOn) == true)
                {
                    objStatus[CFFUStatus.EStatus.PowerOn] = true;
                }
                if (status == EMcuStatus.OverCurrent)
                {
                    objStatus[CFFUStatus.EStatus.OverCurrent] = true;
                }
                if (status == EMcuStatus.MotorAlarm)
                {
                    objStatus[CFFUStatus.EStatus.MotorAlarm] = true;
                }
                if (status == EMcuStatus.Connect)
                {
                    objStatus[CFFUStatus.EStatus.Connect] = true;
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
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {
            CDeviceMcuLocalViewer32Define.CMcuLocalViewer32Error objError = m_objFFU.HLGetErrorCode();
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

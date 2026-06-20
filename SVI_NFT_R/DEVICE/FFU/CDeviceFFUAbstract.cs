using System;

namespace HLDevice.Abstract
{
    public abstract class CDeviceFFUAbstract
    {
        /// <summary>
        /// 초기화 파라미터
        /// </summary>
        public class CInitializeParameter : ICloneable
        {
            /// <summary>
            /// 통신 타입
            /// </summary>
            public enum EType
            {
                TYPE_SOCKET_CLIENT = 0,
                TYPE_SOCKET_SERVER,
                TYPE_SERIAL_PORT,
                TYPE_FINAL
            };

            /// <summary>
            /// 시리얼포트 Parity
            /// </summary>
            public enum ESerialPortParity
            {
                PARITY_NONE = 0,
                PARITY_ODD,
                PARITY_EVEN,
                PARITY_MARK,
                PARITY_SPACE
            };

            /// <summary>
            /// 시리얼포트 StopBits
            /// </summary>
            public enum ESerialPortStopBits
            {
                STOP_BITS_NONE = 0,
                STOP_BITS_ONE,
                STOP_BITS_TWO,
                STOP_BITS_ONE_POINT_FIVE
            };

            public EType eType;
            public string strSocketIPAddress;
            public int iSocketPortNumber;


            public string strSerialPortName;
            public int iSerialPortBaudrate;
            public int iSerialPortDataBits;
            public ESerialPortParity eParity;
            public ESerialPortStopBits eStopBits;
            public int iUnitID;

            public object Clone()
            {
                CInitializeParameter objInitializeParameter = new CInitializeParameter();
                objInitializeParameter.eType = eType;
                objInitializeParameter.strSocketIPAddress = strSocketIPAddress;
                objInitializeParameter.iSocketPortNumber = iSocketPortNumber;

                objInitializeParameter.strSerialPortName = strSerialPortName;
                objInitializeParameter.iSerialPortBaudrate = iSerialPortBaudrate;
                objInitializeParameter.iSerialPortDataBits = iSerialPortDataBits;
                objInitializeParameter.eParity = eParity;
                objInitializeParameter.eStopBits = eStopBits;
                objInitializeParameter.iUnitID = iUnitID;
                return objInitializeParameter;
            }
        }

        public class CFFUStatus : ICloneable
        {
            public enum EStatus
            {
                AC_FAIL,
                OVER_HEAT,
                OVER_MOTOR,
                OVER_CURRENT,
                ERROR_COMMUNICATION,
                ETC1,
                ECT2,
                ETC3,
                REMOTE_STAUS,
                MOTOR_RUN,
                ECT4,
                ECT5,
                ECT6,
                ECT7,
                ECT8,
                ECT9,

                //+ Blue Cord MCUL32-TOTAL
                NotConnect,
                MotorError,
                Remote,
                //- Blue Cord MCUL32-TOTAL
                //+ Local Viewer 32
                NotRemote,
                PowerOn,
                OverCurrent,
                MotorAlarm,
                Connect,
                //- Local Viewer 32

                FINAL
            }

            private bool[] bStatus = new bool[(int)EStatus.FINAL];

            public bool this[EStatus eStatus]
            {
                get { return bStatus[(int)eStatus]; }
                set { bStatus[(int)eStatus] = value; }
            }

            public object Clone()
            {
                CFFUStatus objStatus = new CFFUStatus();
                objStatus.bStatus = (bool[])bStatus.Clone();

                return objStatus;
            }
        }

        /// <summary>
        /// 알람 발생 확인 클래스
        /// </summary>
        public class CFFUError : ICloneable
        {
            /// <summary>이벤트 발생 시간</summary>
            public string strEventTime;
            /// <summary>수행된 함수 이름</summary>
            public string strFunctionName;
            /// <summary>알람 리턴 결과</summary>
            public int iReturnCode;
            /// <summary>알람 메세지</summary>
            public string strMessage;

            public object Clone()
            {
                CFFUError objError = new CFFUError();
                objError.strEventTime = strEventTime;
                objError.strFunctionName = strFunctionName;
                objError.iReturnCode = iReturnCode;
                objError.strMessage = strMessage;

                return objError;
            }
        }

        /// <summary>
        /// 초기화 추상객체
        /// </summary>
        public abstract bool HLInitialize(CInitializeParameter objInitializeParameter);

        /// <summary>
        /// 해제 추상객체
        /// </summary>
        public abstract void HLDeInitialize();

        /// <summary>
        /// 버전 추상객체
        /// </summary>
        /// <returns></returns>
        public abstract string HLGetVersion();

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public virtual bool HLIsConnected()
        {
            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 팬 속도 요청
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public virtual bool HLGetRequiredSpeed(int iSlave, int iFFUIndex, out int iValue)
        {
            iValue = 0;
            bool bReturn = false;

            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 팬 속도 설정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public virtual bool HLSetRequiredSpeed(int iSlave, int iFFUIndex, int iValue)
        {
            bool bReturn = false;

            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모든 팬 속도 설정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public virtual bool HLSetAllRequiredSpeed(int iSlave, int iValue)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 팬 속도 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public virtual bool HLGetCurrentSpeed(int iSlave, int iFFUIndex, out int iValue)
        {
            iValue = 0;
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }


        /// <summary>
        /// 현재 상태
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="objStatus"></param>
        /// <returns></returns>
        public virtual bool HLGetStatus(int iSlave, int iFFUIndex, out HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus objStatus)
        {
            objStatus = new HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus();
            bool bReturn = false;

            do
            {

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
        public virtual bool HLSetReset(int iSlave, int iFFUIndex, bool bReset)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모든 채널 리셋
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="bReset"></param>
        /// <returns></returns>
        public virtual bool HLSetAllReset(int iSlave, bool bReset)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모든 채널에 팬 속도 값
        /// </summary>
        /// <param name="readValues"></param>
        /// <returns></returns>
        public virtual bool HLGetFanSpeedAll(out int[] readValues)
        {
            bool bReturn = false;
            readValues = new int[0];
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="bReadData"></param>
        /// <returns></returns>
        public virtual bool HLReadCoils(int iSlave, int iAddress, int iReadCount, out bool[] bReadData)
        {
            bool bReturn = false;
            bReadData = null;

            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public virtual bool HLWriteSingleCoil(int iSlave, int iAddress, bool bData)
        {
            bool bReturn = false;

            do
            {

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
        public virtual bool HLReadInputRegisters(int iSlave, int iAddress, out double dReadData)
        {
            bool bReturn = false;
            dReadData = 0;

            do
            {

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
        public virtual bool HLReadInputRegisters(int iSlave, int iAddress, out float fReadData)
        {
            bool bReturn = false;
            fReadData = 0f;

            do
            {

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
        public virtual bool HLReadInputRegisters(int iSlave, int iAddress, int iReadCount, out int iReadData)
        {
            bool bReturn = false;
            iReadData = 0;

            do
            {

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
        public virtual bool HLReadHoldingRegisters(int iSlave, int iAddress, int iReadCount, out int iReadData)
        {
            bool bReturn = false;
            iReadData = 0;

            do
            {

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
        public virtual bool HLReadHoldingRegisters(int iSlave, int iAddress, int iReadCount, out double dReadData)
        {
            bool bReturn = false;
            dReadData = 0;

            do
            {

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
        public virtual bool HLReadHoldingRegisters(int iSlave, int iAddress, int iReadCount, out int[] iReadData)
        {
            bool bReturn = false;
            iReadData = null;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iData"></param>
        /// <returns></returns>
        public virtual bool HLWriteHoldingRegisters(int iSlave, int iAddress, int iData)
        {
            bool bReturn = false;

            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public virtual HLDevice.Abstract.CDeviceFFUAbstract.CFFUError HLGetErrorCode()
        {
            HLDevice.Abstract.CDeviceFFUAbstract.CFFUError objError = new HLDevice.Abstract.CDeviceFFUAbstract.CFFUError();
            return objError;
        }

        public virtual DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return null;
        }

        public virtual DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return null;
        }
    }
}

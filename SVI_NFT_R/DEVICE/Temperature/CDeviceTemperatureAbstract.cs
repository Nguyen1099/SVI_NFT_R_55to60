using System;

namespace HLDevice.Abstract
{
    public abstract class CDeviceTemperatureAbstract
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

                return objInitializeParameter;
            }
        }

        /// <summary>
        /// 알람 발생 확인 클래스
        /// </summary>
        public class CTemperatureError : ICloneable
        {
            /// <summary>
            /// 이벤트 발생 시간
            /// </summary>
            public string strEventTime;
            /// <summary>
            /// 수행된 함수 이름
            /// </summary>
            public string strFunctionName;
            /// <summary>
            /// 알람 리턴 결과
            /// </summary>
            public int iReturnCode;
            /// <summary>
            /// 알람 메세지
            /// </summary>
            public string strMessage;

            public object Clone()
            {
                CTemperatureError objError = new CTemperatureError();
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
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="dPVData"></param>
        /// <returns></returns>
        public virtual bool HLGetPVData(int iSlave, out double dPVData)
        {
            bool bReturn = false;
            // dummy data
            dPVData = (1 * iSlave) + -9999;

            do
            {

            } while (false);

            return bReturn;
        }

        public virtual bool HLReadTemp(byte byteID, out double dTempData)
        {
            bool bReturn = false;
            // dummy data
            dTempData = (1 * byteID) + -9999;

            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="dInputData"></param>
        /// <returns></returns>
        public virtual bool HLGetInputData(int iSlave, out double dInputData)
        {
            bool bReturn = false;
            dInputData = 0;

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
        /// 설명 : double형 데이터를 읽을려면 4를 읽어야 가능하기때문에 크기는 고정
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
        /// 설명 : float형 데이터를 읽을려면 2를 읽어야 가능하기때문에 크기는 고정
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
        /// 설명 : int형 데이터를 읽을때 데이터 읽는 크기에 따라 결과값 다름
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
        /// 설명 : int형 데이터를 읽을려면 2를 읽어야 가능하기때문에 크기는 고정
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
        /// 설명 : int형 데이터를 읽을려면 2를 읽어야 가능하기때문에 크기는 고정
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
        public virtual HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError HLGetErrorCode()
        {
            HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError objError = new HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError();
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

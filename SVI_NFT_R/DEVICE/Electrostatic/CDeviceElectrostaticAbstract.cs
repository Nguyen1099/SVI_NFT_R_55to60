using System;

namespace HLDevice.Abstract
{
    public abstract class CDeviceElectrostaticAbstract
    {
        /// <summary>
        /// 초기화 파라미터
        /// </summary>
        public class CInitializeParameter
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
                objInitializeParameter.eType = this.eType;
                objInitializeParameter.strSocketIPAddress = this.strSocketIPAddress;
                objInitializeParameter.iSocketPortNumber = this.iSocketPortNumber;

                objInitializeParameter.strSerialPortName = this.strSerialPortName;
                objInitializeParameter.iSerialPortBaudrate = this.iSerialPortBaudrate;
                objInitializeParameter.iSerialPortDataBits = this.iSerialPortDataBits;
                objInitializeParameter.eParity = this.eParity;
                objInitializeParameter.eStopBits = this.eStopBits;

                return objInitializeParameter;
            }
        }

        /// <summary>
        /// 알람 발생 확인 클래스
        /// </summary>
        public class CElectrostaticError
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
                CElectrostaticError objError = new CElectrostaticError();
                objError.strEventTime = this.strEventTime;
                objError.strFunctionName = this.strFunctionName;
                objError.iReturnCode = this.iReturnCode;
                objError.strMessage = this.strMessage;

                return objError;
            }
        }

        /// <summary>
        /// 수신데이터 콜백
        /// </summary>
        public class CReceiveData
        {
            public string strData;
            public byte[] byteReceiveData = new byte[4096];
            public int iByteLength;

            public void clear()
            {
                strData = "";
                Array.Clear(byteReceiveData, 0, byteReceiveData.Length);
                iByteLength = 0;
            }

            public object Clone()
            {
                CReceiveData objData = new CReceiveData();
                objData.strData = this.strData;
                objData.byteReceiveData = this.byteReceiveData;
                objData.iByteLength = this.iByteLength;
                return objData;
            }
        }

        /// <summary>
        /// 델리게이트 선언
        /// </summary>
        public delegate void CallBackFuntionReceiveData(CReceiveData objReceiveData);

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
        /// 콜백 연결
        /// </summary>
        /// <param name="objReceiveData"></param>
        public virtual void SetCallbackFunction(CallBackFuntionReceiveData objReceiveData)
        {

        }

        /// <summary>
        /// 닉네임 저장
        /// </summary>
        /// <param name="iAddress"></param>
        /// <param name="strNickName"></param>
        /// <returns></returns>
        public virtual bool HLSaveNickName(int iAddress, string strNickName)
        {
            bool bReturn = false;

            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 닉네임 불러오기
        /// </summary>
        /// <param name="iAddress"></param>
        /// <param name="strNickName"></param>
        /// <returns></returns>
        public virtual bool HLLoadNickName(int iAddress, out string strNickName)
        {
            bool bReturn = false;
            strNickName = "";
            do
            {


            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 채널의 값을 읽는다
        /// </summary>
        /// <param name="strNickName"></param>
        /// <param name="strResult"></param>
        /// <returns></returns>
        public virtual bool HLReadChannel(string strNickName, out string[] strResult)
        {
            bool bReturn = false;
            strResult = null;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public virtual HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError HLGetErrorCode()
        {
            HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError objError = new HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError();
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

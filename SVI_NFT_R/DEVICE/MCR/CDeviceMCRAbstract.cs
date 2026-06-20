namespace HLDevice.Abstract
{
    public abstract class CDeviceMCRAbstract
    {

        /// <summary>
        /// 초기화 파라미터
        /// </summary>
        public class CInitializeParameter
        {
            public string strIPAddress;
            public string strPortNumber;

            public object Clone()
            {
                CInitializeParameter objInitializeParameter = new CInitializeParameter();
                objInitializeParameter.strIPAddress = strIPAddress;
                objInitializeParameter.strPortNumber = strPortNumber;
                return objInitializeParameter;
            }
        }

        /// <summary>
        /// 알람 발생 확인 클래스
        /// </summary>
        public class CMCRError
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
                CMCRError objError = new CMCRError();
                objError.strEventTime = this.strEventTime;
                objError.strFunctionName = this.strFunctionName;
                objError.iReturnCode = this.iReturnCode;
                objError.strMessage = this.strMessage;

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
        /// <param name="strReadData"></param>
        /// <returns></returns>
        public virtual bool HLReadData(ref string strReadData)
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
        /// <returns></returns>
        public virtual bool HLTrigger()
        {
            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 디바이스 이름을 읽어온다.
        /// </summary>
        /// <param name="deviceName">디바이스 이름</param>
        /// <returns>true=성공, false=실패</returns>
        public virtual bool TryGetDeviceName(out string deviceName)
        {
            bool bReturn = false;
            deviceName = string.Empty;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 디바이스 시리얼 넘버를 읽어온다.
        /// </summary>
        /// <param name="serialNo">디바이스 시리얼 넘버</param>
        /// <returns>true=성공, false=실패</returns>
        public virtual bool TryGetSerialNo(out string serialNo)
        {
            bool bReturn = false;
            serialNo = string.Empty;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public virtual HLDevice.Abstract.CDeviceMCRAbstract.CMCRError HLGetErrorCode()
        {
            HLDevice.Abstract.CDeviceMCRAbstract.CMCRError objError = new HLDevice.Abstract.CDeviceMCRAbstract.CMCRError();
            return objError;
        }
    }
}

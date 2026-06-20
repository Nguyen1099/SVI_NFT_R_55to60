namespace HLDevice.Electrostatic
{
    public class CDeviceElectrostaticVirtual : HLDevice.Abstract.CDeviceElectrostaticAbstract
    {
        HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError m_objError = new HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError();
        private CallBackFuntionReceiveData m_objCallback = null;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceElectrostaticVirtual()
        {
        }

        /// <summary>
        /// 해제
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return "1.0.0.1";
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
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void HLDeInitialize()
        {
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
        {
            return true;
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
        /// 닉네임 저장
        /// </summary>
        /// <param name="iAddress"></param>
        /// <param name="strNickName"></param>
        /// <returns></returns>
        public override bool HLSaveNickName(int iAddress, string strNickName)
        {
            bool bReturn = false;

            do
            {
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 닉네임 불러오기
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
                strNickName = "Virtual GMS";
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 채널값 읽기
        /// </summary>
        /// <param name="strNickName"></param>
        /// <param name="strResult"></param>
        /// <returns></returns>
        public override bool HLReadChannel(string strNickName, out string[] strResult)
        {
            bool bReturn = false;
            strResult = new string[5];

            do
            {
                strResult[0] = "0.023";
                strResult[1] = "0.123";
                strResult[2] = "0.223";
                strResult[3] = "0.323";
                strResult[4] = "0.423";

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {

        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError HLGetErrorCode()
        {
            return (HLDevice.Abstract.CDeviceElectrostaticAbstract.CElectrostaticError)m_objError.Clone();
        }
    }
}

namespace HLDevice.MCR
{
    public class CDeviceMCRCognex : HLDevice.Abstract.CDeviceMCRAbstract
    {
        HLDevice.Abstract.CDeviceMCRAbstract.CMCRError m_objError = new HLDevice.Abstract.CDeviceMCRAbstract.CMCRError();
        HLDeviceDLL.MCR.Cognex.HLDeviceMCRCognex m_objMCR = new HLDeviceDLL.MCR.Cognex.HLDeviceMCRCognex();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceMCRCognex()
        {
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objMCR.GetVersion();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(HLDevice.Abstract.CDeviceMCRAbstract.CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {

                HLDeviceDLL.MCR.Cognex.CDeviceMCRCognexDefine.CInitializeParameter objParameter = new HLDeviceDLL.MCR.Cognex.CDeviceMCRCognexDefine.CInitializeParameter();
                objParameter.strIPAddress = objInitializeParameter.strIPAddress;
                objParameter.strPortNumber = objInitializeParameter.strPortNumber;

                m_objMCR.HLInitialize(objParameter);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void HLDeInitialize()
        {
            m_objMCR.HLDeInitialize();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
        {
            return m_objMCR.HLIsConnected();
        }

        /// <summary>
        /// 데이터 리딩
        /// </summary>
        /// <param name="strReadData"></param>
        /// <returns></returns>
        public override bool HLReadData(ref string strReadData)
        {
            bool bReturn = false;
            strReadData = "";
            do
            {
                if (m_objMCR.HLReadData(ref strReadData) == false)
                {
                    MakeError();
                    break;
                }
                strReadData = strReadData.Replace("\r", "")
                    .Replace("\n", "");

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 트리거
        /// </summary>
        /// <returns></returns>
        public override bool HLTrigger()
        {
            bool bReturn = false;

            do
            {
                if (m_objMCR.HLTrigger() == false)
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool TryGetDeviceName(out string deviceName)
        {
            bool bReturn = false;
            deviceName = string.Empty;

            do
            {
                if (m_objMCR.HLDeviceName() == false)
                {
                    MakeError();
                    break;
                }
                if (HLReadData(ref deviceName) == false)
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool TryGetSerialNo(out string serialNo)
        {
            bool bReturn = false;
            serialNo = string.Empty;

            do
            {
                if (m_objMCR.HLSerialNo() == false)
                {
                    MakeError();
                    break;
                }
                if (HLReadData(ref serialNo) == false)
                {
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
            HLDeviceDLL.MCR.Cognex.CDeviceMCRCognexDefine.CMCRError objError = m_objMCR.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override HLDevice.Abstract.CDeviceMCRAbstract.CMCRError HLGetErrorCode()
        {
            return (HLDevice.Abstract.CDeviceMCRAbstract.CMCRError)m_objError.Clone();
        }
    }
}

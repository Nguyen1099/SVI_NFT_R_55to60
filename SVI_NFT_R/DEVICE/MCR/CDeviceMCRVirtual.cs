namespace HLDevice.MCR
{
    public class CDeviceMCRVirtual : HLDevice.Abstract.CDeviceMCRAbstract
    {

        HLDeviceDLL.MCR.Cognex.HLDeviceMCRCognex m_objMCR = new HLDeviceDLL.MCR.Cognex.HLDeviceMCRCognex();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceMCRVirtual()
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
        public override bool HLInitialize(HLDevice.Abstract.CDeviceMCRAbstract.CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {

                HLDeviceDLL.MCR.Cognex.CDeviceMCRCognexDefine.CInitializeParameter objParameter = new HLDeviceDLL.MCR.Cognex.CDeviceMCRCognexDefine.CInitializeParameter();
                objParameter.strIPAddress = objInitializeParameter.strIPAddress;
                objParameter.strPortNumber = objInitializeParameter.strPortNumber;


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
                strReadData = string.Format("MCR_READ_{0}", System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
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

                bReturn = true;
            } while (false);

            return bReturn;
        }

    }
}

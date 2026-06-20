using HLDevice.Abstract;
using SVI_NFT_R;

namespace HLDevice
{
    public class CDeviceMCR
    {
        CDeviceMCRAbstract m_objMCR;
        private CDocument m_objDocument;

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objMCR.HLGetVersion();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument document, CConfig.CMCRParameter objInitializeParameter)
        {
            m_objDocument = document;

            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                m_objMCR = new MCR.CDeviceMCRVirtual();
            }
            else
            {
                m_objMCR = new MCR.CDeviceMCRCognex();
            }

            var objMCRParameter = objInitializeParameter;
            var objDeviceMCRParameter = new CDeviceMCRAbstract.CInitializeParameter();
            objDeviceMCRParameter.strIPAddress = objMCRParameter.strIPAddress;
            objDeviceMCRParameter.strPortNumber = objMCRParameter.strPortNumber;

            return m_objMCR.HLInitialize(objDeviceMCRParameter);
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objMCR.HLDeInitialize();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public bool HLIsConnected()
        {
            return m_objMCR.HLIsConnected();
        }

        /// <summary>
        /// 데이터 리딩
        /// </summary>
        /// <param name="strReadData"></param>
        /// <returns></returns>
        public bool HLReadData(ref string strReadData)
        {
            strReadData = "";
            return m_objMCR.HLReadData(ref strReadData);
        }

        /// <summary>
        /// 트리거
        /// </summary>
        /// <returns></returns>
        public bool HLTrigger()
        {
            return m_objMCR.HLTrigger();
        }

        /// <summary>
        /// 디바이스 이름을 리턴한다.
        /// </summary>
        /// <param name="deviceName">디바이스 이름</param>
        /// <returns>true=성공, false=실패</returns>
        public bool TryGetDeviceName(out string deviceName)
        {
            return m_objMCR.TryGetDeviceName(out deviceName);
        }

        /// <summary>
        /// 디바이스 시리얼 넘버를 리턴한다.
        /// </summary>
        /// <param name="serialNo">디바이스 시리얼 넘버</param>
        /// <returns>true=성공, false=실패</returns>
        public bool TryGetSerialNo(out string serialNo)
        {
            return m_objMCR.TryGetSerialNo(out serialNo);
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public HLDevice.Abstract.CDeviceMCRAbstract.CMCRError HLGetErrorCode()
        {
            return m_objMCR.HLGetErrorCode();
        }
    }
}

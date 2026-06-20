using ENC.Device.Communication;
using SVI_NFT_R;
using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Diagnostics;
using System.Linq;

namespace HLDevice
{
    public class CDeviceNachiInterface : IHaveLogEvent
    {
        /// <summary>
        /// 수신 데이터
        /// </summary>
        public class CReceiveData
        {
            public string strData = string.Empty;
            public byte[] byteReceiveData = new byte[0];
            public int iByteLength = 0;
            public int iReceiveByteCount = 0;

            public EReceiveDataType eReceiveData = EReceiveDataType.RECEIVE_TYPE_UNKNOWN;
            public string strReceiveData = string.Empty;
        }
        public enum EReceiveDataType
        {
            RECEIVE_TYPE_UNKNOWN
        };
        public event EventHandler<string> OnEventOccured;
        public int m_nSendReceiveSize { get; private set; }
        private readonly CReceiveData m_objReceiveData = new CReceiveData();
        private Action<CReceiveData> m_objCallback = null;
        private CDeviceCommunication m_objCommunication;
        private CDocument m_objDocument;
        private readonly int sendNachiMaxSize = 1024;


        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objDocument"></param>
        /// <param name="objInitializeParameter"></param>
        /// <param name="bIsVirtual"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument objDocument, CConfig.CNachiInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                m_objDocument = objDocument;
                if (CDefine.ESimulationMode.SIMULATION_MODE_OFF == objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    var socketServer = new CDeviceCommunicationSocketServer();
                    socketServer.OnLogEvent += onLogEvent;
                    m_objCommunication = new CDeviceCommunication(socketServer);
                }
                else
                {
                    m_objCommunication = new CDeviceCommunication(new CDeviceCommunicationVirtual());
                }
                CDeviceCommunicationAbstract.CInitializeParameter objParameter = new CDeviceCommunicationAbstract.CInitializeParameter();
                objParameter.eType = (CDeviceCommunicationAbstract.CInitializeParameter.EType)objInitializeParameter.eType;
                objParameter.eDataEncoding = (CDeviceCommunicationAbstract.CInitializeParameter.EDataEncoding)objInitializeParameter.eDataEncoding;
                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;

                m_nSendReceiveSize = objInitializeParameter.iDataCount;

                m_objCommunication.SetCallbackFunction(onReceiveData);
                if (false == m_objCommunication.HLInitialize(objParameter)) break;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objCommunication.HLDeInitialize();
        }

        /// <summary>
        /// 연결 상태
        /// </summary>
        /// <returns></returns>
        public bool HLIsConnected()
        {
            return m_objCommunication.HLIsConnected();
        }

        /// <summary>
        /// 콜백 연결
        /// </summary>
        /// <param name="objReceiveData"></param>
        public void SetCallbackFunction(Action<CReceiveData> objReceiveData)
        {
            m_objCallback = objReceiveData;
        }

        /// <summary>
        /// 데이터 전송
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        public bool HLSend(int[] sendData)
        {
            bool bReturn = false;
            do
            {
                int sendByteLength = sendData.Length * sizeof(int);
                Debug.Assert(sendByteLength == m_nSendReceiveSize, $"설정한 송신값 범위를 벗어났습니다. sendReceiveSize({m_nSendReceiveSize}) != sendByteLength({sendByteLength})");
                // Little Endian -> Big Endian
                var convertSendData = sendData
                    .SelectMany(item => BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(item)));
                int index = 0;
                var query = convertSendData
                    .Skip(sendNachiMaxSize * index)
                    .Take(sendNachiMaxSize);
                while (query.Count() > 0)
                {
                    m_objCommunication.HLSend(query.ToArray());
                    index++;
                    query = convertSendData
                        .Skip(sendNachiMaxSize * index)
                        .Take(sendNachiMaxSize);
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 메세지 기다림
        /// </summary>
        /// <param name="eReceiveDataType"></param>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        protected bool WaitReceiveData(EReceiveDataType eReceiveDataType, int iTimeOut = 20000, int iSleepPeriod = 5)
        {
            bool bReturn = false;
            do
            {

                // 수신데이터 타입과 비교하여 수신될때 까지 기다림
                while (0 < iTimeOut && m_objReceiveData.eReceiveData != eReceiveDataType)
                {
                    System.Threading.Thread.Sleep(iSleepPeriod); iTimeOut -= iSleepPeriod;
                }

                // Timeout Check
                if (0 >= iTimeOut)
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private void onLogEvent(object sender, LogEventArgs e)
        {
            OnEventOccured?.Invoke(sender, e.LogMessage);
        }

        private void onReceiveData(CDeviceCommunicationAbstract.CReceiveData objData)
        {
            CReceiveData objReceiveData = new CReceiveData();
            objReceiveData.strReceiveData = m_objReceiveData.strReceiveData;
            objReceiveData.iByteLength = objData.byteReceiveData.Length;
            objReceiveData.byteReceiveData = (byte[])objData.byteReceiveData.Clone();
            objReceiveData.eReceiveData = m_objReceiveData.eReceiveData;
            objReceiveData.strData = m_objReceiveData.strData;
            objReceiveData.iReceiveByteCount = objData.iReceiveByteCount;
            m_objCallback?.Invoke(objReceiveData);
        }
    }

}

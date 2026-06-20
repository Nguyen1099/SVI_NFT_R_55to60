using System;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMCarrierInformationDownload : CCIMAbstract
    {
        private CarrierInformationDownload m_objCarrierInformationDownload;


        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMCarrierInformationDownload(CCIMDefine.structureCIMInitialize objCIMIntiialize)
        {
            m_objDocument = (CDocument)objCIMIntiialize.objDocument as CDocument;
            m_objCommunicator = (urLinkDllCommunicator)objCIMIntiialize.objCommunicator as urLinkDllCommunicator;
            m_strEquipmentID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            Initialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        protected override bool Initialize()
        {
            bool bResult = false;
            do
            {
                // Receive Event 등록
                //m_objCommunicator.OnReceiveRequest += Communicator_OnReceiveRequest;   

                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                // 데이터 생성.
                m_objCarrierInformationDownload = new CarrierInformationDownload();
                m_objCarrierInformationDownload.EQPID = m_strEquipmentID;

                // 쓰레드 종료 Flag
                m_bThreadExit = false;
                // 쓰레드 객체 생성.
                m_ThreadProcess = new System.Threading.Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start(this);

                m_bReceiveData = false;

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 리시브 이벤트 (리시브 된 메시지가 오면 해당 메시지에 맞게 파싱한다.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Communicator_OnReceiveRequest(object sender, ReqMsgInfo e)
        {
            try
            {
                // 특수한 경우임.
                //m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, e.RecevedXml_2);

                XmlNodeList xmlMsgnode = e.ReceviedXml.SelectNodes("/MESSAGE");
                foreach (XmlNode xn in xmlMsgnode)
                {
                    string eqpid = xn["EQPID"].InnerText;
                    string name = xn["NAME"].InnerText;
                    string transactionno = xn["TRANSACTIONNO"].InnerText;
                    if ("CARRIERINFODOWNLOAD" == name) // Trace Start Request
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                m_objCarrierInformationDownload.EQPID = eqpid;
                                m_objCarrierInformationDownload.NAME = name;
                                if ("CARRIERID" == tagName) m_objCarrierInformationDownload.BODY.CARRIERID = value;
                                else if ("CELLID" == tagName) m_objCarrierInformationDownload.BODY.CELLID = value;
                                else if ("PRODUCTID" == tagName) m_objCarrierInformationDownload.BODY.PRODUCTID = value;
                                else if ("PRODUCT_TYPE" == tagName) m_objCarrierInformationDownload.BODY.PRODUCT_TYPE = value;
                                else if ("PRODUCT_KIND" == tagName) m_objCarrierInformationDownload.BODY.PRODUCT_KIND = value;
                                else if ("PRODUCTSPEC" == tagName) m_objCarrierInformationDownload.BODY.PRODUCTSPEC = value;
                                else if ("STEPID" == tagName) m_objCarrierInformationDownload.BODY.STEPID = value;
                                else if ("PPID" == tagName) m_objCarrierInformationDownload.BODY.PPID = value;
                                else if ("CELL_SIZE" == tagName) m_objCarrierInformationDownload.BODY.CELL_SIZE = value;
                                else if ("CELL_THICKNESS" == tagName) m_objCarrierInformationDownload.BODY.CELL_THICKNESS = value;
                                else if ("COMMENT" == tagName) m_objCarrierInformationDownload.BODY.COMMENT = value;
                                else if ("CELLINFORESULT" == tagName) m_objCarrierInformationDownload.BODY.CELLINFORESULT = value;
                                else if ("INS_COUNT" == tagName) m_objCarrierInformationDownload.BODY.INS_COUNT = value;
                                else if ("REPLYSTATUS" == tagName) m_objCarrierInformationDownload.BODY.REPLYSTATUS = value;
                            }
                        }
                        m_bReceiveData = true;
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[RECV]\n" + e.RecevedXml_2);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void DeInitialize()
        {
            m_bThreadExit = true;
            m_ThreadProcess.Join();
        }

        /// <summary>
        /// 쓰레드 상태
        /// </summary>
        /// <returns></returns>
        public override CCIMDefine.EProcessState GetStatus()
        {
            return m_eStatus;
        }

        /// <summary>
        /// 리시브 된 데이터
        /// </summary>
        /// <returns></returns>
        public CarrierInformationDownload GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objCarrierInformationDownload;
        }

        /// <summary>
        /// 리시브 상태 확인
        /// </summary>
        /// <returns></returns>
        protected override bool IsStatus()
        {
            bool bResult = false;
            do
            {
                if (false == m_bReceiveData) break;
                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 작업 수행
        /// </summary>
        /// <returns></returns>
        protected override bool DoProcess()
        {
            bool bResult = false;

            do
            {
                m_bReceiveData = false;
                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcess(object state)
        {
            CProcessCIMCarrierInformationDownload pThis = (CProcessCIMCarrierInformationDownload)state;

            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsStatus())
                {
                    if (pThis.DoProcess())
                    {
                        pThis.m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}

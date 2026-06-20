using System;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMTerminalDisplayRequest : CCIMAbstract
    {
        public readonly CIM.LoginAcknowledge LoginAcknowledge = new CIM.LoginAcknowledge();
        public readonly CIM.LoginAcknowledge LogoutAcknowledge = new CIM.LoginAcknowledge();
        private TerminalDisplayRequest m_objTerminalDisplayRequest;
        private TerminalDisplayReply m_objTerminalDisplayReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMTerminalDisplayRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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

                // 쓰레드 종료 Flag
                m_bThreadExit = false;
                // 쓰레드 객체 생성.
                m_ThreadProcess = new Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start(this);
                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                // 데이터 생성.
                m_objTerminalDisplayRequest = new TerminalDisplayRequest();
                m_objTerminalDisplayReply = new TerminalDisplayReply();
                m_objTerminalDisplayRequest.EQPID = m_strEquipmentID;
                m_objTerminalDisplayReply.EQPID = m_strEquipmentID;

                m_bReceiveData = false;

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 리시브 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Communicator_OnReceiveRequest(object sender, ReqMsgInfo e)
        {
            try
            {
                XmlNodeList xmlMsgnode = e.ReceviedXml.SelectNodes("/MESSAGE");
                foreach (XmlNode xn in xmlMsgnode)
                {
                    string eqpid = xn["EQPID"].InnerText;
                    string name = xn["NAME"].InnerText;
                    string transactionno = xn["TRANSACTIONNO"].InnerText;
                    if ("TERMINALDISPLAYREQUEST" == name)
                    {
                        m_objTerminalDisplayRequest.EQPID = eqpid;
                        m_objTerminalDisplayRequest.NAME = name;
                        m_objTerminalDisplayRequest.TRANSACTIONNO = transactionno;

                        int iIndex = 0;
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        m_objTerminalDisplayRequest.BODY.TEXTS = new TerminalDisplayRequest._TerminalDisplayRequest._TEXT[BodyNode.ChildNodes.Count - 1];
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("TID" == tagName)
                                {
                                    m_objTerminalDisplayRequest.BODY.TID = value;
                                }
                                if ("TEXTS" == tagName)
                                {
                                    m_objTerminalDisplayRequest.BODY.TEXTS[iIndex++].TEXT = value;

                                    // 로그인/아웃 메시지 수신 처리
                                    if (value.StartsWith("LOGIN_") == true)
                                    {
                                        string[] splitText = value.Split('_');
                                        if (splitText.Length > 2)
                                        {
                                            LoginAcknowledge.ReceivedMessage = true;
                                            LoginAcknowledge.Message = value;
                                            LoginAcknowledge.Suceess = splitText[1].Contains("PASS");
                                        }
                                    }
                                    else if (value.StartsWith("LOGOUT_") == true)
                                    {
                                        string[] splitText = value.Split('_');
                                        if (splitText.Length > 2)
                                        {
                                            LogoutAcknowledge.ReceivedMessage = true;
                                            LogoutAcknowledge.Message = value;
                                            LogoutAcknowledge.Suceess = splitText[1].Contains("PASS");
                                        }
                                    }
                                }
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
        public TerminalDisplayRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objTerminalDisplayRequest;
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
                if (false == m_bReceiveData)
                    break;

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

                // 현재 설비 상태 보내기.
                m_objTerminalDisplayReply.TRANSACTIONNO = m_objTerminalDisplayRequest.TRANSACTIONNO;

                // Reply 보내기 전에 상태 변경해주자.
                m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;

                m_objTerminalDisplayReply.BODY.ACKC10 = string.Format("{0}", (int)CCIMDefine.ETerminalDisplayReply.TERMINAL_DISPLAY_REPLY_ACCEPTED);

                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetTerminalDisplayReply(m_objTerminalDisplayReply);

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
            CProcessCIMTerminalDisplayRequest pThis = (CProcessCIMTerminalDisplayRequest)state;

            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsStatus())
                {
                    if (pThis.DoProcess())
                    {
                        //pThis.m_eStatus = CCIMDefine.ProcessState.STS_RECEIVED;
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}
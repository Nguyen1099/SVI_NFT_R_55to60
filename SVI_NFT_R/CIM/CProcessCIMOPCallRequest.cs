using System;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMOPCallRequest : CCIMAbstract
    {
        OpCallRequest m_objOpCallRequest;
        OpCallReply m_objOpCallReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMOPCallRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_ThreadProcess = new System.Threading.Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start(this);
                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                // 데이터 생성.
                m_objOpCallRequest = new OpCallRequest();
                m_objOpCallReply = new OpCallReply();
                m_objOpCallRequest.EQPID = m_strEquipmentID;
                m_objOpCallReply.EQPID = m_strEquipmentID;

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
                    if ("OPCALLREQUEST" == name)
                    {
                        m_objOpCallRequest.EQPID = eqpid;
                        m_objOpCallRequest.NAME = name;
                        m_objOpCallRequest.TRANSACTIONNO = transactionno;

                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("OPCALL" == tagName) m_objOpCallRequest.BODY.OPCALL = value;
                                else if ("OPCALLID" == tagName) m_objOpCallRequest.BODY.OPCALLID = value;
                                else if ("MESSAGE" == tagName) m_objOpCallRequest.BODY.MESSAGE = value;
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
        /// 리스브 된 데이터
        /// </summary>
        /// <returns></returns>
        public OpCallRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objOpCallRequest;
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

                // 현재 설비 상태 보내기.
                m_objOpCallReply.TRANSACTIONNO = m_objOpCallRequest.TRANSACTIONNO;

                // 설비명이 같은지 확인.
                if (m_objOpCallRequest.EQPID == m_strEquipmentID)
                {
                    // Reply 보내기 전에 상태 변경해주자.
                    m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;

                    m_objOpCallReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EOpCallReply.OP_CALL_REPLY_ACCEPTED);
                }
                else
                {
                    m_objOpCallReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EOpCallReply.OP_CALL_REPLY_OTHER_ERROR);
                }

                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetOPCallReply(m_objOpCallReply);

                // OK 일 경우에만 설비 상태 변경.
                if (m_objOpCallReply.BODY.HCACK == string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_ACCEPTED))
                {
                    m_objDocument.m_eOpCallState = CCIMDefine.EOpcallState.OP_CALL_STATE_ON;
                }

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
            CProcessCIMOPCallRequest pThis = (CProcessCIMOPCallRequest)state;

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

using System;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMControlStateRequest : CCIMAbstract
    {
        ControlStateSetRequest m_objControlStateSetRequest;
        ControlStateSetReply m_objControlStateSetReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMControlStateRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objControlStateSetRequest = new ControlStateSetRequest();
                m_objControlStateSetReply = new ControlStateSetReply();
                m_objControlStateSetRequest.EQPID = m_strEquipmentID;
                m_objControlStateSetReply.EQPID = m_strEquipmentID;

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
                    if ("CONTROLSTATESETREQUEST" == name)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("CRST" == tagName)
                                {
                                    m_objControlStateSetRequest.EQPID = eqpid;
                                    m_objControlStateSetRequest.NAME = name;
                                    m_objControlStateSetRequest.TRANSACTIONNO = transactionno;
                                    m_objControlStateSetRequest.BODY.CRST = value;
                                    m_objControlStateSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EControlStateSetReply.CONTROL_STATE_SET_REPLY_ACCEPTED);
                                }
                                else
                                {
                                    m_objControlStateSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EControlStateSetReply.CONTROL_STATE_SET_REPLY_SPECIAL_REASON); // 시스템 시간 변경 실패
                                }
                            }
                        }
                        else
                        {
                            m_objControlStateSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EControlStateSetReply.CONTROL_STATE_SET_REPLY_SPECIAL_REASON); // 시스템 시간 변경 실패
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
        /// 리시브된 데이터
        /// </summary>
        /// <returns></returns>
        public ControlStateSetRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objControlStateSetRequest;
        }

        /// <summary>
        /// 리시브 상태 확인.
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
        /// 작업 수행.
        /// </summary>
        /// <returns></returns>
        protected override bool DoProcess()
        {
            bool bResult = false;
            do
            {
                m_bReceiveData = false;

                // 데이터 적용
                m_objDocument.m_eControlState = (CCIMDefine.EControlState)int.Parse(m_objControlStateSetRequest.BODY.CRST);

                // 현재 설비 상태 보내기.
                m_objControlStateSetReply.TRANSACTIONNO = m_objControlStateSetRequest.TRANSACTIONNO;
                m_objControlStateSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EControlStateSetReply.CONTROL_STATE_SET_REPLY_ACCEPTED);

                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetControlStateReply(m_objControlStateSetReply);

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
            CProcessCIMControlStateRequest pThis = (CProcessCIMControlStateRequest)state;

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

using System;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMInterlockRequest : CCIMAbstract
    {
        InterlockRequest m_objInterlockRequest;
        InterlockReply m_objInterlockReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMInterlockRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objInterlockRequest = new InterlockRequest();
                m_objInterlockReply = new InterlockReply();
                m_objInterlockRequest.EQPID = m_strEquipmentID;
                m_objInterlockReply.EQPID = m_strEquipmentID;

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
                    if ("INTERLOCKREQUEST" == name)
                    {
                        m_objInterlockRequest.EQPID = eqpid;
                        m_objInterlockRequest.NAME = name;
                        m_objInterlockRequest.TRANSACTIONNO = transactionno;

                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("INTERLOCK" == tagName) m_objInterlockRequest.BODY.INTERLOCK = value;
                                else if ("INTERLOCKID" == tagName) m_objInterlockRequest.BODY.INTERLOCKID = value;
                                else if ("MESSAGE" == tagName) m_objInterlockRequest.BODY.MESSAGE = value;
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
        public InterlockRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objInterlockRequest;
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
        /// <![CDATA[
        /// 'm_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;' 를 호출하면 다른 스레드에서 인터락 메시지 창을 띄운다.
        /// ]]>
        protected override bool DoProcess()
        {
            bool bResult = false;
            do
            {
                m_bReceiveData = false;

                m_objInterlockReply.TRANSACTIONNO = m_objInterlockRequest.TRANSACTIONNO;
                if (m_objInterlockRequest.EQPID == m_strEquipmentID)
                {
                    m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;
                    m_objInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_ACCEPTED);
                }
                else
                {
                    // 설비 아이디가 다르면 NACK = 4
                    m_objInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_OTHER_ERROR);
                }
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetInterlockReply(m_objInterlockReply);

                if (string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_ACCEPTED) == m_objInterlockReply.BODY.HCACK)
                {
                    // 기존 상태가 Interlock이 아니면 Cycle Stop 진행
                    if (
                        CDefine.ERunStatus.Stop != m_objDocument.GetRunStatus()
                        && CDefine.ERunStatus.Setup != m_objDocument.GetRunStatus()
                        )
                    {
                        m_objDocument.m_bInterLockHappened = true;
                        if (CDefine.ERunStatus.Stopping != m_objDocument.GetRunStatus())
                        {
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                    }
                    else
                    {
                        m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EInterlockState.INTERLOCK_STATE_ON;
                        //m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EMoveState.MOVE_STATE_PAUSE;
                    }
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
            CProcessCIMInterlockRequest pThis = (CProcessCIMInterlockRequest)state;

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

using System;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMEQStateRequest : CCIMAbstract
    {
        EQStateRequest m_objEQStateRequest;
        EQStateReply m_objEQStateReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMEQStateRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objEQStateRequest = new EQStateRequest();
                m_objEQStateReply = new EQStateReply();
                m_objEQStateRequest.EQPID = m_strEquipmentID;
                m_objEQStateReply.EQPID = m_strEquipmentID;

                m_bReceiveData = false;

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 리스브 이벤트
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
                    if ("EQSTATEREQUEST" == name)
                    {
                        m_objEQStateRequest.EQPID = eqpid;
                        m_objEQStateRequest.NAME = name;
                        m_objEQStateRequest.TRANSACTIONNO = transactionno;
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
        public EQStateRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objEQStateRequest;
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
                m_objEQStateReply.TRANSACTIONNO = m_objEQStateRequest.TRANSACTIONNO;
                m_objEQStateReply.BODY.AVAILABILITYSTATE = string.Format("{0}", (int)m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objEQStateReply.BODY.INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objEQStateReply.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objEQStateReply.BODY.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objEQStateReply.BODY.FRONTSTATE = string.Format("{0}", (int)m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objEQStateReply.BODY.REARSTATE = string.Format("{0}", (int)m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objEQStateReply.BODY.PP_SPLSTATE = string.Format("{0}", (int)m_objDocument.m_ePP_SPLState);

                // Reply 보고
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetEQStateReply(m_objEQStateReply);

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
            CProcessCIMEQStateRequest pThis = (CProcessCIMEQStateRequest)state;

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

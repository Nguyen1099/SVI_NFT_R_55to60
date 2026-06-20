using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMPPIDListRequest : CCIMAbstract
    {
        PPIDListRequest m_objPPIDListRequest;
        PPListReply m_objPPListReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMPPIDListRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objPPIDListRequest = new PPIDListRequest();
                m_objPPListReply = new PPListReply();
                m_objPPIDListRequest.EQPID = m_strEquipmentID;
                m_objPPListReply.EQPID = m_strEquipmentID;

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
                    if ("PPIDLISTREQUEST" == name)
                    {
                        m_objPPIDListRequest.EQPID = eqpid;
                        m_objPPIDListRequest.NAME = name;
                        m_objPPIDListRequest.TRANSACTIONNO = transactionno;

                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("PPID_TYPE" == tagName)
                                    m_objPPIDListRequest.BODY.PPID_TYPE = value;
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
        public PPIDListRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objPPIDListRequest;
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
                m_objPPListReply.TRANSACTIONNO = m_objPPIDListRequest.TRANSACTIONNO;

                m_objPPListReply.BODY.PPIDS = new List<string>();
                // PPID 리스트 찾기.

                // 현재 프로그램에 있는 PPID 리스트 가져오기.
                List<CConfig.CModelParameter> ListModel = m_objDocument.m_objModel.GetModelParameterList();
                for (int iLoopCount = 0; iLoopCount < ListModel.Count; iLoopCount++)
                {
                    // 상위에서 보내온 PPID Type과 같은 것만 보내자.
                    //                     if( m_objPPIDListRequest.BODY.PPID_TYPE == ListModel[ iLoopCount ].strName )
                    //                     {
                    m_objPPListReply.BODY.PPIDS.Add(ListModel[iLoopCount].strPPID);
                    //                    }
                }
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPPIDListReply(m_objPPListReply);
                m_objPPIDListRequest.BODY.PPID_TYPE = ""; // 데이터 초기화.
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
            CProcessCIMPPIDListRequest pThis = (CProcessCIMPPIDListRequest)state;

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

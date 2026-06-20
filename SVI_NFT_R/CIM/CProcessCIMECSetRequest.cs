using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMECSetRequest : CCIMAbstract
    {
        ECSetRequest m_objECSetRequest;
        ECSetReply m_objECSetReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMECSetRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objECSetRequest = new ECSetRequest();
                m_objECSetReply = new ECSetReply();
                m_objECSetRequest.EQPID = m_strEquipmentID;
                m_objECSetReply.EQPID = m_strEquipmentID;
                // EC 갯수에 따라 
                m_objECSetRequest.BODY.ECS.EC = new ECSetRequest._ECSetRequest._ECS._EC[2];
                m_objECSetReply.BODY.ECS.EC = new ECSetReply._ECSetReply._ECS._EC[2];
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
                    if ("ECSETREQUEST" == name)
                    {
                        m_objECSetRequest.EQPID = eqpid;
                        m_objECSetRequest.NAME = name;
                        m_objECSetRequest.TRANSACTIONNO = transactionno;
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
        public ECSetRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objECSetRequest;
        }

        /// <summary>
        /// 리스브 상태 확인
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
                m_objECSetReply.TRANSACTIONNO = m_objECSetRequest.TRANSACTIONNO;
                List<CConfig.CECListData> objECList = m_objDocument.GetECList();

                // 현재 EC 셋팅값 변경을 하는데 변경점에 문제 없는지 확인 후 변경하자.
                // 현재 변경이 안됨.
                for (int iLoopCount = 0; iLoopCount < m_objECSetRequest.BODY.ECS.EC.Length; iLoopCount++)
                {
                    for (int iLoopDB = 0; iLoopDB < objECList.Count; iLoopDB++)
                    {
                        if (objECList[iLoopDB].strECID == m_objECSetRequest.BODY.ECS.EC[iLoopCount].ECID
                            && objECList[iLoopDB].strECItemName == m_objECSetRequest.BODY.ECS.EC[iLoopCount].ECNAME)
                        {
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECID = objECList[iLoopDB].strECID;
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECNAME = objECList[iLoopDB].strECItemName;
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECSLL_ACK = string.Format("{0}", (int)CCIMDefine.ECSetReply.EC_SET_REPLY_ILLEGAL_VALUE);
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECSUL_ACK = string.Format("{0}", (int)CCIMDefine.ECSetReply.EC_SET_REPLY_ILLEGAL_VALUE);
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECWLL_ACK = string.Format("{0}", (int)CCIMDefine.ECSetReply.EC_SET_REPLY_ILLEGAL_VALUE);
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECWUL_ACK = string.Format("{0}", (int)CCIMDefine.ECSetReply.EC_SET_REPLY_ILLEGAL_VALUE);
                        }
                        else
                        {
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECID = m_objECSetRequest.BODY.ECS.EC[iLoopCount].ECID;
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECNAME = m_objECSetRequest.BODY.ECS.EC[iLoopCount].ECNAME;
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECSLL_ACK = string.Format("{0}", (int)CCIMDefine.ECSetReply.EC_SET_REPLY_ILLEGAL_VALUE);
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECSUL_ACK = string.Format("{0}", (int)CCIMDefine.ECSetReply.EC_SET_REPLY_ILLEGAL_VALUE);
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECWLL_ACK = string.Format("{0}", (int)CCIMDefine.ECSetReply.EC_SET_REPLY_ILLEGAL_VALUE);
                            m_objECSetReply.BODY.ECS.EC[iLoopCount].ECWUL_ACK = string.Format("{0}", (int)CCIMDefine.ECSetReply.EC_SET_REPLY_ILLEGAL_VALUE);
                        }
                    }
                }

                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetECSetReply(m_objECSetReply);

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
            CProcessCIMECSetRequest pThis = (CProcessCIMECSetRequest)state;

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

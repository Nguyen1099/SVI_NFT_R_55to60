using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMSpecificationversionStateRequest : CCIMAbstract
    {
        SpecificationversionStateRequest m_objSpecificationversionStateRequest;
        SpecificationversionStateReply m_objSpecificationversionStateReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMSpecificationversionStateRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objSpecificationversionStateRequest = new SpecificationversionStateRequest();
                m_objSpecificationversionStateReply = new SpecificationversionStateReply();
                m_objSpecificationversionStateReply.BODY.SPECS = new List<SpecificationversionStateReply._SpecificationversionStateReply.SPECList>();
                m_objSpecificationversionStateRequest.EQPID = m_strEquipmentID;
                m_objSpecificationversionStateReply.EQPID = m_strEquipmentID;
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
                    if ("SPECIFICATIONVERSIONSTATEREQUEST" == name)
                    {
                        m_objSpecificationversionStateRequest.EQPID = eqpid;
                        m_objSpecificationversionStateRequest.NAME = name;
                        m_objSpecificationversionStateRequest.TRANSACTIONNO = transactionno;
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
        public SpecificationversionStateRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objSpecificationversionStateRequest;
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
                m_objSpecificationversionStateReply.TRANSACTIONNO = m_objSpecificationversionStateRequest.TRANSACTIONNO;
                m_objDocument.m_objConfig.LoadSpecificationVersionStateParameter();
                var objSpecList = m_objDocument.m_objConfig.GetSpecificationVersionStateParameter();
                m_objSpecificationversionStateReply.BODY.SPECS = new List<SpecificationversionStateReply._SpecificationversionStateReply.SPECList>();

                foreach (var item in objSpecList.SpecList)
                {
                    var objSPECList = new SpecificationversionStateReply._SpecificationversionStateReply.SPECList()
                    {
                        SPECID = item.Key,
                        SPECDATA = item.Value
                    };
                    m_objSpecificationversionStateReply.BODY.SPECS.Add(objSPECList);
                }

                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetSpecificationversionStateReply(m_objSpecificationversionStateReply);

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
            CProcessCIMSpecificationversionStateRequest pThis = (CProcessCIMSpecificationversionStateRequest)state;

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

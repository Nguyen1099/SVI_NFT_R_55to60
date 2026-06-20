using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMPPParamRequest : CCIMAbstract
    {
        PPParamRequest m_objPPParamRequest;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMPPParamRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objPPParamRequest = new PPParamRequest();
                m_objPPParamRequest.EQPID = m_strEquipmentID;

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
                    if ("PPPARAMREQUEST" == name)
                    {
                        m_objPPParamRequest.EQPID = eqpid;
                        m_objPPParamRequest.NAME = name;
                        m_objPPParamRequest.TRANSACTIONNO = transactionno;

                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("PPID" == tagName) m_objPPParamRequest.BODY.PPID = value;
                                else if ("PPID_TYPE" == tagName) m_objPPParamRequest.BODY.PPID_TYPE = value;
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
        public PPParamRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objPPParamRequest;
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
            // PPLIST 데이터 가져오기.
            CConfig.CModelParameter objModelParameter = new CConfig.CModelParameter();
            PPParamReply objPPParamReply = new PPParamReply();
            objPPParamReply.BODY.PARAMS = new PPParamReply._PPParamReply.PARAM[1];
            //kjpark 20181130 CIM JobDownload Create
            objPPParamReply.BODY.PARAMS[0].PARAMNAME = "";
            objPPParamReply.BODY.PARAMS[0].PARAMVALUE = "";
            objPPParamReply.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            do
            {
                m_bReceiveData = false;

                // 현재 설비 상태 보내기.
                objPPParamReply.TRANSACTIONNO = m_objPPParamRequest.TRANSACTIONNO;

                // 모든 PPLIST 비교하면서 PPID 와 PPID_TYPE 비교한다.
                List<CConfig.CModelParameter> ListModel = m_objDocument.m_objModel.GetModelParameterList();
                for (int iLoopCount = 0; iLoopCount < ListModel.Count; iLoopCount++)
                {
                    // 상위에서 보내온 PPID TYPE이 같은 것만 체크
                    //                     if(m_objPPParamRequest.BODY.PPID_TYPE == ListModel[iLoopCount].strName)
                    //                     {
                    if (m_objPPParamRequest.BODY.PPID == ListModel[iLoopCount].strPPID)
                    {
                        objModelParameter = m_objDocument.m_objModel.GetModelParameter(m_objPPParamRequest.BODY.PPID);
                        break;
                    }
                    //                   }
                }

                // 해당 PPID가 있다면...
                //kjpark 20181130 CIM JobDownload Create
                if (string.IsNullOrWhiteSpace(objModelParameter.strPPID) == false)
                {
                    objPPParamReply.BODY.CCODE = "0";
                    objPPParamReply.BODY.SOFTREV = objModelParameter.strSoftRevision;
                    var paramEnum = Enum.GetValues(typeof(CCIMDefine.EPpidParamList));
                    objPPParamReply.BODY.PARAMS = new PPParamReply._PPParamReply.PARAM[paramEnum.Length];
                    foreach (CCIMDefine.EPpidParamList item in paramEnum)
                    {
                        objPPParamReply.BODY.PARAMS[(int)item].PARAMNAME = item.ToString();
                        objPPParamReply.BODY.PARAMS[(int)item].PARAMVALUE = objModelParameter[item];
                    }
                }
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPPParamReply(objPPParamReply);

                // 데이터 초기화.
                m_objPPParamRequest.BODY.PPID = " ";
                m_objPPParamRequest.BODY.PPID_TYPE = " ";
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
            CProcessCIMPPParamRequest pThis = (CProcessCIMPPParamRequest)state;

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

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMTraceRequest : CCIMAbstract
    {
        private TraceStartRequest m_objTraceStartRequest;
        private TraceStartReply m_objTraceStartReply;
        private TraceStopRequest m_objTraceStopRequest;
        private TraceStopReply m_objTraceStopReply;
        private TraceDataReport m_objTraceDataReport;

        private bool m_bTraceDataStart;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMTraceRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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

                // TraceData 시작 Flag
                m_bTraceDataStart = false;

                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                // 데이터 생성.
                m_objTraceStartRequest = new TraceStartRequest();
                m_objTraceStartReply = new TraceStartReply();
                m_objTraceStopRequest = new TraceStopRequest();
                m_objTraceStopReply = new TraceStopReply();
                m_objTraceDataReport = new TraceDataReport();
                m_objTraceStartRequest.EQPID = m_strEquipmentID;
                m_objTraceStartReply.EQPID = m_strEquipmentID;
                m_objTraceStopRequest.EQPID = m_strEquipmentID;
                m_objTraceStopReply.EQPID = m_strEquipmentID;
                m_objTraceDataReport.EQPID = m_strEquipmentID;

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
                    if ("TRACESTARTREQUEST" == name) // Trace Start Request
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("INTERVALTIME" == tagName)
                                {
                                    m_objTraceStartRequest.EQPID = eqpid;
                                    m_objTraceStartRequest.NAME = name;
                                    m_objTraceStartRequest.TRANSACTIONNO = transactionno;
                                    m_objTraceStartRequest.BODY.INTERVALTIME = value;

                                    m_objTraceStartReply.TRANSACTIONNO = m_objTraceStartRequest.TRANSACTIONNO;
                                    m_objTraceStartReply.BODY.RESULT = "OK";
                                    m_objTraceStartReply.BODY.REASON = "";

                                    m_bTraceDataStart = true;
                                }
                                else
                                {
                                    m_objTraceStartReply.BODY.RESULT = "NG";
                                    m_objTraceStartReply.BODY.REASON = "IntervalTime is Invalid TagName.";
                                }
                            }

                            // Reply 보고
                            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetTraceStartReply(m_objTraceStartReply);

                        }
                        m_bReceiveData = true;
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[RECV]\n" + e.RecevedXml_2);
                    }
                    else if ("TRACESTOPREQUEST" == name) // Trace Stop Request
                    {
                        m_objTraceStopRequest.EQPID = eqpid;
                        m_objTraceStopRequest.NAME = name;
                        m_objTraceStopRequest.TRANSACTIONNO = transactionno;

                        m_objTraceStopReply.TRANSACTIONNO = m_objTraceStopRequest.TRANSACTIONNO;
                        m_objTraceStopReply.BODY.RESULT = "OK";
                        m_objTraceStopReply.BODY.REASON = "";

                        // Reply 보고
                        m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetTraceStopReply(m_objTraceStopReply);

                        m_bTraceDataStart = false;

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
            m_bTraceDataStart = false;
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
        public TraceStartRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objTraceStartRequest;
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
                if (false == m_bTraceDataStart)
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
                try
                {
                    m_bReceiveData = false;
                    // Trace Data 보내기.
                    m_objTraceDataReport.BODY.SV = string.Empty;
                    Thread.Sleep(int.Parse(m_objTraceStartRequest.BODY.INTERVALTIME));
                    // SV 데이터 넣기.
                    List<object> objSVIDList = m_objDocument.GetSVIDList();
                    var sbSV = new StringBuilder();
                    for (int iLoopCount = 0; iLoopCount < objSVIDList.Count; iLoopCount++)
                    {
                        if (null == objSVIDList[iLoopCount])
                        {
                            sbSV.AppendFormat("{0}", 0);
                        }
                        else
                        {
                            string strType = (objSVIDList[iLoopCount].GetType()).Name;
                            if (strType == "Double")
                            {
                                sbSV.AppendFormat("{0:F3}", objSVIDList[iLoopCount]);
                            }
                            else
                            {
                                sbSV.AppendFormat("{0}", objSVIDList[iLoopCount]);
                            }
                        }

                        // 마지막이면 콤마를 붙이지 않음
                        if (iLoopCount != objSVIDList.Count - 1)
                        {
                            sbSV.AppendFormat(",");
                        }
                    }
                    m_objTraceDataReport.BODY.SV = sbSV.ToString();

                    // CIM 보고하자.
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetTraceDataReport(m_objTraceDataReport);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
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
            CProcessCIMTraceRequest pThis = (CProcessCIMTraceRequest)state;

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

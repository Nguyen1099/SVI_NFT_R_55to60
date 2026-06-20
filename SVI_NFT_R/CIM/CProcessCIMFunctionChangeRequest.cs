using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMFunctionChangeRequest : CCIMAbstract
    {
        FunctionChangeRequest m_objFunctionChangeRequest;
        FunctionChangeReply m_objFunctionChangeReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMFunctionChangeRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objFunctionChangeRequest = new FunctionChangeRequest();
                m_objFunctionChangeReply = new FunctionChangeReply();
                m_objFunctionChangeRequest.EQPID = m_strEquipmentID;
                m_objFunctionChangeReply.EQPID = m_strEquipmentID;

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
                    if ("FUNCTIONCHANGEREQUEST" == name)
                    {
                        m_objFunctionChangeRequest.EQPID = eqpid;
                        m_objFunctionChangeRequest.NAME = name;
                        m_objFunctionChangeRequest.TRANSACTIONNO = transactionno;

                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("UNITID" == tagName) m_objFunctionChangeRequest.BODY.UNITID = value;
                                else if ("EFID" == tagName) m_objFunctionChangeRequest.BODY.EFID = value;
                                else if ("EFST" == tagName) m_objFunctionChangeRequest.BODY.EFST = value;
                                else if ("MESSAGE" == tagName) m_objFunctionChangeRequest.BODY.MESSAGE = value;

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
        public FunctionChangeRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objFunctionChangeRequest;
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
            var objCIMParameter = m_objDocument.m_objConfig.GetCimParameter().DeepClone();

            do
            {
                m_bReceiveData = false;
                int receiveEfid;
                int.TryParse(m_objFunctionChangeRequest.BODY.EFID, out receiveEfid);
                var eReceiveEfid = (CDialogEQPFunctionList.EFName)(receiveEfid - 1);

                // 현재 설비 상태 보내기.
                m_objFunctionChangeReply.TRANSACTIONNO = m_objFunctionChangeRequest.TRANSACTIONNO;
                if ((int)CDialogEQPFunctionList.EFName.EF_NAME_FINAL <= (int)eReceiveEfid)
                {
                    m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_NOT_FOUND_FUNCTION);
                }
                // EFID,EFST 값 없음 NACK = 2
                else if (
                    string.IsNullOrWhiteSpace(m_objFunctionChangeRequest.BODY.EFID) == true
                    || string.IsNullOrWhiteSpace(m_objFunctionChangeRequest.BODY.EFST) == true
                    )
                {
                    m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_NOT_SUPPORTED);
                }
                // EFID 이미 설정된 값을 상위에서 받았을 때 NACK = 6
                else if (m_objFunctionChangeRequest.BODY.EFST == objCIMParameter.strEFValueBuffer[(int)eReceiveEfid])
                {
                    m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_SAME_VALUE);
                }
                else
                {
                    // EFID 설정 할 수 없는 값을 상위에서 받았을 때 NACK = 4
                    switch (eReceiveEfid)
                    {
                        case CDialogEQPFunctionList.EFName.EF_NAME_CELL_TRACKING:
                            {
                                var correctNames = Enum.GetNames(typeof(CDialogEQPFunctionList.EFSTOnOffTrace));
                                if (false == correctNames.Contains(m_objFunctionChangeRequest.BODY.EFST)
                                    || false == CDialogEQPFunctionList.CanEditEfstCombination(eReceiveEfid, m_objFunctionChangeRequest.BODY.EFST)
                                    )
                                {
                                    m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_NOT_FOUND_FUNCTION);
                                    break;
                                }
                                m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_ACCEPTED);
                            }
                            break;
                        case CDialogEQPFunctionList.EFName.EF_NAME_TRACKING_CONTROL:
                            {
                                var correctNames = Enum.GetNames(typeof(CDialogEQPFunctionList.EFSTTrackingControl));
                                if (false == correctNames.Contains(m_objFunctionChangeRequest.BODY.EFST)
                                    || false == CDialogEQPFunctionList.CanEditEfstCombination(eReceiveEfid, m_objFunctionChangeRequest.BODY.EFST)
                                    )
                                {
                                    m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_NOT_FOUND_FUNCTION);
                                    break;
                                }
                                m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_ACCEPTED);
                            }
                            break;
                        case CDialogEQPFunctionList.EFName.EF_NAME_CELL_MCR_MODE:
                            {
                                var correctNames = Enum.GetNames(typeof(CDialogEQPFunctionList.EFSTUse));
                                if (false == correctNames.Contains(m_objFunctionChangeRequest.BODY.EFST)
                                    || false == CDialogEQPFunctionList.CanEditEfstCombination(eReceiveEfid, m_objFunctionChangeRequest.BODY.EFST)
                                    )
                                {
                                    m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_NOT_FOUND_FUNCTION);
                                    break;
                                }
                                m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_ACCEPTED);
                            }
                            break;
                        case CDialogEQPFunctionList.EFName.EF_NAME_INTERLOCK_CONTROL:
                            {
                                var correctNames = Enum.GetNames(typeof(CDialogEQPFunctionList.EFSTInterlockControl));
                                if (false == correctNames.Contains(m_objFunctionChangeRequest.BODY.EFST)
                                    || false == CDialogEQPFunctionList.CanEditEfstCombination(eReceiveEfid, m_objFunctionChangeRequest.BODY.EFST)
                                    )
                                {
                                    m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_NOT_FOUND_FUNCTION);
                                    break;
                                }
                                m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_ACCEPTED);
                            }
                            break;
                        default:
                            // 1, 2, 4, 10 을 제외한 EFID를 변경하려고 했을 때에는 NACK = 5
                            m_objFunctionChangeReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_NOT_FOUND_FUNCTION);
                            break;
                    }
                }
                // Reply 보고
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetFunctionChangeReply(m_objFunctionChangeReply);

                // Reply 보고가 Change 보고보다 늦게 들어온다고 수정요청함 (2022-04-08 SDV CIM QUAL)
                Thread.Sleep(100);

                if (string.Format("{0}", (int)CCIMDefine.EFunctionChangeReply.FUNCTION_CHANGE_REPLY_ACCEPTED) == m_objFunctionChangeReply.BODY.HCACK)
                {
                    // 변경된 EF list를 전이도에 맞게 변경한다.
                    CDialogEQPFunctionList.UpdateEfstValue(eReceiveEfid, m_objFunctionChangeRequest.BODY.EFST, ref objCIMParameter.strEFValueBuffer);

                    // 최종 변경 주체 업데이트
                    objCIMParameter.strEFValueChangeByWho = "HOST";

                    // 백업
                    m_objDocument.m_objConfig.SaveCimParameter(objCIMParameter);

                    // 일단 버퍼에 변경사항을 적용하고 CIM 보고는 다른곳에서 체크하여 진행함
                }

                bResult = true;
            } while (false);
            return bResult;
        }

        private bool IsEfidChangeReport()
        {
            bool bResult = false;

            do
            {
                if (false == m_objDocument.m_objProcessCIM.IsInitialized)
                {
                    break;
                }

                // 설비 내에 셀이 없을 때에 EFID 값을 적용한다.
                if (CCIMDefine.ERunState.RUN_STATE_IDLE != m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                {
                    break;
                }

                // EFID 변경 사항이 없으면 보고하지 않는다.
                bool bChanged = false;
                var objCIMParameter = (CConfig.CCimParameter)m_objDocument.m_objConfig.GetCimParameter();
                for (int iLoopCount = 0; iLoopCount < (int)CCIMDefine.EFunctionID.EFID_FINAL - 1; iLoopCount++)
                {
                    if (objCIMParameter.strEFValueBuffer[iLoopCount] != objCIMParameter.strEFValue[iLoopCount])
                    {
                        bChanged = true;
                    }
                }
                if (false == bChanged)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        private void DoProcessEfidChangeReport()
        {
            var objCIMParameter = m_objDocument.m_objConfig.GetCimParameter().DeepClone();

            // 메시지 생성
            var objFunctionChangeEvent = new FunctionChangeEvent();
            objFunctionChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            List<FunctionChangeEvent._FunctionChangeEvent.FUNCTION> reportFunctionList = new List<FunctionChangeEvent._FunctionChangeEvent.FUNCTION>();
            for (int iLoopCount = 0; iLoopCount < (int)CCIMDefine.EFunctionID.EFID_FINAL - 1; iLoopCount++)
            {
                if (objCIMParameter.strEFValueBuffer[iLoopCount] != objCIMParameter.strEFValue[iLoopCount])
                {
                    var addItem = new FunctionChangeEvent._FunctionChangeEvent.FUNCTION();
                    addItem.BYWHO = objCIMParameter.strEFValueChangeByWho;
                    addItem.EFID = string.Format("{0}", iLoopCount + 1);
                    addItem.OLDEFST = objCIMParameter.strEFValue[iLoopCount];
                    addItem.NEWEFST = objCIMParameter.strEFValueBuffer[iLoopCount];
                    objCIMParameter.strEFValue[iLoopCount] = objCIMParameter.strEFValueBuffer[iLoopCount];
                    reportFunctionList.Add(addItem);
                }
            }
            objFunctionChangeEvent.BODY.FUNCTIONS = reportFunctionList.ToArray();
            // CIM 보고
            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetFunctionChangeEvent(objFunctionChangeEvent);

            // 백업
            m_objDocument.m_objConfig.SaveCimParameter(objCIMParameter);
        }

        /// <summary>
        /// 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcess(object state)
        {
            CProcessCIMFunctionChangeRequest pThis = (CProcessCIMFunctionChangeRequest)state;

            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsStatus())
                {
                    if (pThis.DoProcess())
                    {
                        pThis.m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;
                    }
                }

                if (pThis.IsEfidChangeReport())
                {
                    pThis.DoProcessEfidChangeReport();
                }

                Thread.Sleep(10);
            }
        }
    }
}

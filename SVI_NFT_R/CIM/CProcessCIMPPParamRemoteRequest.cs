using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMPPParamRemoteRequest : CCIMAbstract
    {
        private PPChangeRemoteEvent m_objPPParamRmoteRequest;
        private PPChangeRemoteEventReply m_objPPParamRmoteRequestReply;
        private bool m_bSpecOver;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMPPParamRemoteRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                // 데이터 생성.
                m_objPPParamRmoteRequest = new PPChangeRemoteEvent();
                m_objPPParamRmoteRequest.BODY.COMMANDS = new PPChangeRemoteEvent._PPChangeRemoteEvent.COMMAND[1];
                m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS = new PPChangeRemoteEvent._PPChangeRemoteEvent.COMMAND.PARAM[Enum.GetNames(typeof(CCIMDefine.EPpidParamList)).Length];
                m_objPPParamRmoteRequest.EQPID = m_strEquipmentID;

                m_objPPParamRmoteRequestReply = new PPChangeRemoteEventReply();
                m_objPPParamRmoteRequestReply.EQPID = m_strEquipmentID;

                m_bReceiveData = false;

                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;

                // Receive Event 등록
                //m_objCommunicator.OnReceiveRequest += Communicator_OnReceiveRequest;

                // 쓰레드 종료 Flag
                m_bThreadExit = false;
                // 쓰레드 객체 생성.
                m_ThreadProcess = new Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start(this);

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
            // * m_bReceiveData 를 true로 변경하면 상위에 ACK7 응답을 보고한다

            try
            {
                XmlNodeList xmlMsgnode = e.ReceviedXml.SelectNodes("/MESSAGE");
                foreach (XmlNode xn in xmlMsgnode)
                {
                    string eqpid = xn["EQPID"].InnerText;
                    string name = xn["NAME"].InnerText;
                    string transactionno = xn["TRANSACTIONNO"].InnerText;
                    if (m_objPPParamRmoteRequest.NAME == name)
                    {
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[RECV]\n" + e.RecevedXml_2);
                        m_bSpecOver = false;

                        m_objPPParamRmoteRequest.EQPID = eqpid;
                        if (string.IsNullOrWhiteSpace(m_objPPParamRmoteRequest.EQPID) == true)
                        {
                            m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.EQPID_IS_NOT_EXIST.ToString();
                            m_bSpecOver = true;
                            m_bReceiveData = true;
                            return;
                        }
                        if (m_objPPParamRmoteRequest.EQPID != m_strEquipmentID)
                        {
                            m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.EQPID_IS_NOT_EXIST.ToString();
                            m_bSpecOver = true;
                            m_bReceiveData = true;
                            return;
                        }
                        m_objPPParamRmoteRequest.TRANSACTIONNO = transactionno;
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (false == BodyNode.HasChildNodes)
                        {
                            m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_IT_DOES_NOT_EXIST_OR_THERE_IS_NO_PPID_VALUE.ToString();
                            m_bSpecOver = true;
                            m_bReceiveData = true;
                            return;
                        }
                        foreach (XmlNode item in BodyNode.ChildNodes)
                        {
                            string tagName = item.Name;
                            string value = item.InnerText;
                            if ("PPID" == tagName)
                            {
                                m_objPPParamRmoteRequest.BODY.PPID = value;
                            }
                            // [ PPID_TYPE ]
                            // 1 = EQP에서 관리하는 PPID
                            // 2 = HOST로부터 전송 받아 관리하는 PPID
                            else if ("PPID_TYPE" == tagName)
                            {
                                m_objPPParamRmoteRequest.BODY.PPID_TYPE = value;
                                if (m_objPPParamRmoteRequest.BODY.PPID_TYPE != string.Format("{0}", 1))
                                {
                                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PPID_TYPE_OR_CCODE_IS_NOT_MATCH.ToString();
                                    m_bSpecOver = true;
                                    m_bReceiveData = true;
                                    return;
                                }
                            }
                            else if ("PPID_NUMBER" == tagName)
                            {
                                m_objPPParamRmoteRequest.BODY.PPID_NUMBER = value;
                            }
                            else if ("MODULEID" == tagName)
                            {
                                m_objPPParamRmoteRequest.BODY.MODULEID = value;
                            }
                            else if ("COMMANDS" == tagName)
                            {
                                XmlNode BodyNodeCommand = item.SelectSingleNode("COMMAND");
                                {
                                    foreach (XmlNode itemCommand in BodyNodeCommand.ChildNodes)
                                    {
                                        string tagNameCommand = itemCommand.Name;
                                        string valueCommand = itemCommand.InnerText;
                                        if ("CCODE" == tagNameCommand)
                                        {
                                            m_objPPParamRmoteRequest.BODY.COMMANDS[0].CCODE = valueCommand;
                                        }
                                        else if ("PARAMS" == tagNameCommand)
                                        {
                                            int commandCode;
                                            int.TryParse(m_objPPParamRmoteRequest.BODY.COMMANDS[0].CCODE, out commandCode);
                                            if (false == doParsingAndValidationParameters(itemCommand, commandCode))
                                            {
                                                m_bSpecOver = true;
                                                m_bReceiveData = true;
                                                return;
                                            }

                                            switch (commandCode)
                                            {
                                                case CIM.CommandCode.CREATE:
                                                    if (false == setRecipeCreate())
                                                    {
                                                        m_bSpecOver = true;
                                                    }
                                                    break;
                                                case CIM.CommandCode.MODIFY:
                                                    if (false == setRecipeModify())
                                                    {
                                                        m_bSpecOver = true;
                                                    }
                                                    break;
                                                case CIM.CommandCode.DELETE:
                                                    if (false == setRecipeDelete())
                                                    {
                                                        m_bSpecOver = true;
                                                    }
                                                    break;
                                                case CIM.CommandCode.DEFAULT:
                                                default:
                                                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PPID_TYPE_OR_CCODE_IS_NOT_MATCH.ToString();
                                                    m_bSpecOver = true;
                                                    break;
                                            }
                                            m_bReceiveData = true;
                                        }
                                    }
                                }
                            }
                        }
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
        public PPChangeRemoteEvent GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objPPParamRmoteRequest;
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
                {
                    break;
                }

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
            m_bReceiveData = false;
            bool bResult = false;
            do
            {
                // PP PARAM RELPY(ACK7) 보고
                m_objPPParamRmoteRequestReply.TRANSACTIONNO = m_objPPParamRmoteRequest.TRANSACTIONNO;
                if (m_bSpecOver == false)
                {
                    m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.ACCEPTED.ToString();
                }
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPPParamSetReply(m_objPPParamRmoteRequestReply);

                // PP CHANGE EVENT 보고
                if (CIM.Acknowledge7.ACCEPTED.ToString() == m_objPPParamRmoteRequestReply.BODY.ACKC7)
                {
                    int commandCode;
                    int.TryParse(m_objPPParamRmoteRequest.BODY.COMMANDS[0].CCODE, out commandCode);
                    switch (commandCode)
                    {
                        case CIM.CommandCode.CREATE:
                        case CIM.CommandCode.DELETE:
                        case CIM.CommandCode.MODIFY:
                            {
                                PPChangeEvent sendData = new PPChangeEvent();
                                sendData.EQPID = m_objPPParamRmoteRequest.EQPID;
                                sendData.BODY.MODE = commandCode.ToString();
                                sendData.BODY.PPID = m_objPPParamRmoteRequest.BODY.PPID;
                                sendData.BODY.PPID_TYPE = m_objPPParamRmoteRequest.BODY.PPID_TYPE;
                                sendData.BODY.PPID_NUMBER = m_objPPParamRmoteRequest.BODY.PPID_NUMBER;
                                sendData.BODY.MODULEID = m_objPPParamRmoteRequest.BODY.MODULEID;
                                sendData.BODY.COMMANDS = new PPChangeEvent._PPChangeEvent.COMMAND[1];
                                sendData.BODY.COMMANDS[0].CCODE = commandCode.ToString();
                                sendData.BODY.COMMANDS[0].PARAMS = new PPChangeEvent._PPChangeEvent.COMMAND.PARAM[m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS.Length];
                                for (int i = 0; i < sendData.BODY.COMMANDS[0].PARAMS.Length; i++)
                                {
                                    sendData.BODY.COMMANDS[0].PARAMS[i].PARAMNAME = m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS[i].PARAMNAME;
                                    sendData.BODY.COMMANDS[0].PARAMS[i].PARAMVALUE = m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS[i].PARAMVALUE;
                                }
                                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPPChangeEvent(sendData);
                            }
                            break;
                        case CIM.CommandCode.DEFAULT:
                        default:
                            break;
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
            CProcessCIMPPParamRemoteRequest pThis = (CProcessCIMPPParamRemoteRequest)state;

            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsStatus())
                {
                    if (pThis.DoProcess())
                    {
                        pThis.m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;
                    }
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 레시피 생성
        /// </summary>
        /// <returns></returns>
        private bool setRecipeCreate()
        {
            CModel objModel = m_objDocument.m_objModel;
            CConfig objConfig = m_objDocument.m_objConfig;
            string strSetPpid = m_objPPParamRmoteRequest.BODY.PPID;
            if (true == string.IsNullOrWhiteSpace(strSetPpid))
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_IT_DOES_NOT_EXIST_OR_THERE_IS_NO_PPID_VALUE.ToString();
                return false;
            }
            // 기존에 존재하지 않는 PPID만 생성 할 수 있다
            if (true == m_objDocument.m_objModel.GetPPIDOverlap(strSetPpid))
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PPID_ALREADY_EXISTS.ToString();
                return false;
            }
            if (
                CDefine.ERunStatus.Start == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.Stopping == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus()
                )
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.THE_EQUIPMENT_CAN_ONLY_BE_PERFORMED_IN_A_PAUSE_STATE.ToString();
                return false;
            }
            if (m_objDocument.m_objModel.IsSelectJobChangeTpmLossCode() == false)
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                return false;
            }
            // Check PPID_NUMBER
            string ppidNumberRawdata = m_objPPParamRmoteRequest.BODY.PPID_NUMBER;
            int ppidNumber = 0;
            string ppidSubscript = string.Empty;
            var currentOffsetParameter = m_objDocument.m_objConfig.GetModelOffsetParameter();
            {
                if (tryPasrePpidNumber(ppidNumberRawdata, out ppidNumber, out ppidSubscript) == false)
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                    return false;
                }
                // N: 값 초기화 후 생성
                // O: 기존 값 참조 생성
                if (ppidSubscript != "N" && ppidSubscript != "O")
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                    return false;
                }
                if (m_objDocument.m_objModel.IsDuiplicateIndex(ppidNumber) == true)
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PPID_ALREADY_EXISTS.ToString();
                    return false;
                }
                if (m_objDocument.m_objModel.InRangeModelCapacity(ppidNumber) == false)
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PPID_ALREADY_EXISTS.ToString();
                    return false;
                }
                if (
                    m_objDocument.m_objModel.InRangeRmsOnly(ppidNumber) == true
                    && strSetPpid.StartsWith("TT_") == true
                    )
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.EQPID_IS_NOT_EXIST.ToString();
                    return false;
                }
                if (
                    m_objDocument.m_objModel.InRangeShare(ppidNumber) == true
                    && strSetPpid.StartsWith("TT_") == false
                    )
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.EQPID_IS_NOT_EXIST.ToString();
                    return false;
                }
            }

            string strExistFilePath = Path.Combine(objConfig.GetModelPath(), objConfig.GetSystemParameter().strPPID);
            string strNewFilePath = Path.Combine(objConfig.GetModelPath(), strSetPpid);
            // 폴더 복사
            Constants.CopyFolderRecursive(strExistFilePath, strNewFilePath);
            objModel.SetPPIDMatch(strSetPpid, ppidNumber);

            CConfig.CModelParameter objModelParameter = objModel.GetModelParameter(strSetPpid);
            objModelParameter.UpdateTime = DateTime.Now;
            objModelParameter.strSoftRevision = "1";
            double.TryParse(m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS[(int)CCIMDefine.EPpidParamList.CELL_WIDTH].PARAMVALUE, out objModelParameter.dWidth);
            double.TryParse(m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS[(int)CCIMDefine.EPpidParamList.CELL_HEIGHT].PARAMVALUE, out objModelParameter.dHeight);

            // 모델 파라미터 저장
            m_objDocument.m_objConfig.SaveModelParameter(objModelParameter, strSetPpid);

            // 모델 옵셋 파라미터 리셋
            if (ppidSubscript == "N")
            {
                m_objDocument.m_objConfig.ResetModelOffsetParameter(ppidNumber);
            }

            // ! 레시피에 모터 티칭 정보가 들어갈 경우 아래 주석처럼 추가함
            //// 중요! 모델 로드시 모터 파라메터를 모델 정보와 일치시켜야한다
            //{
            //    var motor = new HLDevice.CDeviceMotor(new HLDevice.Motor.CDeviceMotorVirtual());
            //    string modelPath = m_objDocument.m_objConfig.GetModelPath();
            //    motor.HLInitialize(new CDeviceMotorAbstract.CMotorInitializeParameter()
            //    {
            //        iAxisNo = 0,
            //        iBoardNo = 0,
            //        strMotorName = CProcessMotion.EMotor.AZONE_INSPECTION_STAGE_P1_X1A.ToString(),
            //        strModelName = strSetPpid,
            //        strFilePath = modelPath
            //    });
            //    CDeviceMotorAbstract.CMotorPosition objMotorPosition = motor.HLGetMotorPosition();
            //    objMotorPosition.dPosition[(int)InspectionStageMotorXYT.EPosition.InspectionStart] = objModelParameter.dTriggerStartPosTopP1;
            //    objMotorPosition.dPosition[(int)InspectionStageMotorXYT.EPosition.InspectionEnd] = objModelParameter.dTriggerEndPosTopP1;
            //    motor.HLSaveMotorPositionParameter(modelPath, strSetPpid, objMotorPosition);
            //    motor.HLDeInitialize();
            //}

            m_objDocument.m_objModel.IsRecipeChanged = true;
            m_objDocument.m_objModel.IsOffsetChanged = true;
            return true;
        }

        /// <summary>
        /// 레시피 삭제
        /// </summary>
        /// <returns></returns>
        private bool setRecipeDelete()
        {
            CModel objModel = m_objDocument.m_objModel;
            CConfig objConfig = m_objDocument.m_objConfig;
            string strSetPpid = m_objPPParamRmoteRequest.BODY.PPID;
            string strCurrentPpid = objConfig.GetSystemParameter().strPPID;
            if (
                true == string.IsNullOrWhiteSpace(strSetPpid)
                || false == m_objDocument.m_objModel.GetPPIDOverlap(strSetPpid)
                )
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_IT_DOES_NOT_EXIST_OR_THERE_IS_NO_PPID_VALUE.ToString();
                return false;
            }
            // 선택되지 않은 PPID만 삭제 할 수 있다
            if (true == Equals(strSetPpid, strCurrentPpid))
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.CURRENT_PPID_CANNOT_BE_DELETED.ToString();
                return false;
            }
            if (
                CDefine.ERunStatus.Start == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.Stopping == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus()
                )
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.THE_EQUIPMENT_CAN_ONLY_BE_PERFORMED_IN_A_PAUSE_STATE.ToString();
                return false;
            }
            if (m_objDocument.m_objModel.IsSelectJobChangeTpmLossCode() == false)
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                return false;
            }
            // Check PPID_NUMBER
            CConfig.CModelParameter objModelParameter = objModel.GetModelParameter(strSetPpid);
            string ppidNumberRawdata = m_objPPParamRmoteRequest.BODY.PPID_NUMBER;
            int ppidNumber = 0;
            string ppidSubscript = string.Empty;
            {
                if (tryPasrePpidNumber(ppidNumberRawdata, out ppidNumber, out ppidSubscript) == false)
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                    return false;
                }
                // D: 값 초기화 후 삭제 
                // G: 값 유지 후 삭제
                if (ppidSubscript != "D" && ppidSubscript != "G")
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                    return false;
                }
                if (objModelParameter.Index != ppidNumber)
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.EQPID_IS_NOT_EXIST.ToString();
                    return false;
                }
            }

            // 삭제하기 전에 삭제한 값을 보고하기 위한 변수에 값을 넣어준다
            var paramValues = Enum.GetValues(typeof(CCIMDefine.EPpidParamList));
            m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS = new PPChangeRemoteEvent._PPChangeRemoteEvent.COMMAND.PARAM[0];
            //m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS = new PPChangeRemoteEvent._PPChangeRemoteEvent.COMMAND.PARAM[paramValues.Length];
            //foreach (CCIMDefine.EPpidParamList paramValue in paramValues)
            //{
            //    m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS[(int)paramValue].PARAMNAME = paramValue.ToString();
            //    m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS[(int)paramValue].PARAMVALUE = objModelParameter[paramValue];
            //}

            // 모델 파일 삭제
            string strPath = string.Format(@"{0}\{1}", m_objDocument.m_objConfig.GetModelPath(), strSetPpid);
            m_objDocument.m_objModel.SetDirectoryDelete(strPath);

            // 모델 옵셋 파라미터 리셋
            if (ppidSubscript == "D")
            {
                m_objDocument.m_objConfig.ResetModelOffsetParameter(ppidNumber);
            }

            m_objDocument.m_objModel.IsRecipeChanged = true;
            m_objDocument.m_objModel.IsOffsetChanged = true;
            return true;
        }

        /// <summary>
        /// 레시피 수정
        /// </summary>
        /// <returns></returns>
        private bool setRecipeModify()
        {
            CModel objModel = m_objDocument.m_objModel;
            CConfig objConfig = m_objDocument.m_objConfig;
            string strSetPpid = m_objPPParamRmoteRequest.BODY.PPID;
            string strCurrentPpid = objConfig.GetSystemParameter().strPPID;
            if (
                true == string.IsNullOrWhiteSpace(strSetPpid)
                || false == m_objDocument.m_objModel.GetPPIDOverlap(strSetPpid)
                )
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_IT_DOES_NOT_EXIST_OR_THERE_IS_NO_PPID_VALUE.ToString();
                return false;
            }
            if (Equals(strSetPpid, strCurrentPpid) == false)
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.ONLY_PARAMETERS_OF_THE_CURRENT_PPID_CAN_BE_MODIFIED.ToString();
                return false;
            }
            if (
                CDefine.ERunStatus.Start == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.Stopping == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus()
                )
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.THE_EQUIPMENT_CAN_ONLY_BE_PERFORMED_IN_A_PAUSE_STATE.ToString();
                return false;
            }
            if (m_objDocument.m_objModel.IsSelectJobChangeTpmLossCode() == false)
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                return false;
            }
            // Check PPID_NUMBER
            CConfig.CModelParameter objModelParameter = objModel.GetModelParameter(strSetPpid);
            string ppidNumberRawdata = m_objPPParamRmoteRequest.BODY.PPID_NUMBER;
            int ppidNumber = 0;
            string ppidSubscript = string.Empty;
            {
                if (tryPasrePpidNumber(ppidNumberRawdata, out ppidNumber, out ppidSubscript) == false)
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                    return false;
                }
                // C: 변경
                if (ppidSubscript != "C")
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.PERMISSION_NOT_GRANTED.ToString();
                    return false;
                }
                if (objModelParameter.Index != ppidNumber)
                {
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.EQPID_IS_NOT_EXIST.ToString();
                    return false;
                }
            }

            // 모델 파일 수정 및 저장
            int equalParameterCount = 0;
            for (int i = 0; i < m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS.Length; i++)
            {
                string paramName = m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS[i].PARAMNAME;
                var paramEnum = (CCIMDefine.EPpidParamList)Enum.Parse(typeof(CCIMDefine.EPpidParamList), paramName);
                string paramValue = m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS[i].PARAMVALUE;
                double setParameter;
                double.TryParse(paramValue, out setParameter);
                string compareValue = string.Format("{0:0.000}", setParameter);
                if (objModelParameter[paramEnum] == compareValue)
                {
                    equalParameterCount++;
                }
                objModelParameter[paramEnum] = compareValue;
            }

            // 요청 받은 파라메터가 모두 같은 경우 NACK 처리한다
            if (equalParameterCount == m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS.Length)
            {
                m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_THERE_IS_A_PARAMETER_ABNORMALITY.ToString();
                return false;
            }

            int currentRevision;
            int.TryParse(objModelParameter.strSoftRevision, out currentRevision);
            objModelParameter.strSoftRevision = (++currentRevision).ToString();
            m_objDocument.m_objConfig.SaveModelParameter(objModelParameter, strSetPpid);

            // ! 레시피에 모터 티칭 정보가 들어갈 경우 아래 주석처럼 추가함
            //// 중요! 모델 로드시 모터 파라메터를 모델 정보와 일치시켜야한다
            //{
            //    var motor = new HLDevice.CDeviceMotor(new HLDevice.Motor.CDeviceMotorVirtual());
            //    string modelPath = m_objDocument.m_objConfig.GetModelPath();
            //    motor.HLInitialize(new CDeviceMotorAbstract.CMotorInitializeParameter()
            //    {
            //        iAxisNo = 0,
            //        iBoardNo = 0,
            //        strMotorName = CProcessMotion.EMotor.AZONE_INSPECTION_STAGE_P1_X1A.ToString(),
            //        strModelName = strSetPpid,
            //        strFilePath = modelPath
            //    });
            //    CDeviceMotorAbstract.CMotorPosition objMotorPosition = motor.HLGetMotorPosition();
            //    objMotorPosition.dPosition[(int)InspectionStageMotorXYT.EPosition.InspectionStart] = objModelParameter.dTriggerStartPosTopP1;
            //    objMotorPosition.dPosition[(int)InspectionStageMotorXYT.EPosition.InspectionEnd] = objModelParameter.dTriggerEndPosTopP1;
            //    motor.HLSaveMotorPositionParameter(modelPath, strSetPpid, objMotorPosition);
            //    motor.HLDeInitialize();
            //}

            m_objDocument.m_objModel.IsRecipeChanged = true;
            m_objDocument.m_objModel.IsOffsetChanged = true;
            return true;
        }

        /// <summary>
        /// Xml Node를 파싱하여 값이 유효한지 체크하고 유효하다면 m_objPPParamRmoteRequestReply 에 파싱한다 
        /// </summary>
        /// <param name="itemCommand"></param>
        /// <param name="commandCode"></param>
        /// <returns>함수 성공 여부</returns>
        private bool doParsingAndValidationParameters(XmlNode itemCommand, int commandCode)
        {
            switch (commandCode)
            {
                case CIM.CommandCode.DELETE:
                    // 삭제시에는 파라메터 유효성 검사를 실시하지 않음
                    return true;
                case CIM.CommandCode.CREATE:
                case CIM.CommandCode.MODIFY:
                    if (Enum.GetNames(typeof(CCIMDefine.EPpidParamList)).Length != itemCommand.ChildNodes.Count)
                    {
                        m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_THERE_IS_A_PARAMETER_ABNORMALITY.ToString();
                        return false;
                    }
                    break;
                default:
                    break;
            }

            int index = 0;
            var list = new List<string>();
            var addParameters = new PPChangeRemoteEvent._PPChangeRemoteEvent.COMMAND.PARAM[itemCommand.ChildNodes.Count];
            foreach (XmlNode itemParam in itemCommand.ChildNodes)
            {
                XmlNode BodyNodeName = itemParam.SelectSingleNode("PARAMNAME");
                XmlNode BodyNodeValue = itemParam.SelectSingleNode("PARAMVALUE");
                try
                {
                    // 중복되는 이름이 내려 왔는지 확인
                    if (list.Contains(BodyNodeName.InnerText))
                    {
                        // 중복된 이름이 내려올 경우,
                        m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_THERE_IS_A_PARAMETER_ABNORMALITY.ToString();
                        return false;
                    }
                    list.Add(BodyNodeName.InnerText);
                    // 파싱 실패시 익셉션이 발생함
                    var paramEnum = (CCIMDefine.EPpidParamList)Enum.Parse(typeof(CCIMDefine.EPpidParamList), BodyNodeName.InnerText);
                    if (false == CCIMDefine.RECIPE_PARAMS[paramEnum].CheckInRange(BodyNodeValue.InnerText))
                    {
                        // 입력 값이 범위를 넘었을 경우
                        m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_THERE_IS_A_PARAMETER_ABNORMALITY.ToString();
                        return false;
                    }
                    addParameters[index].PARAMNAME = BodyNodeName.InnerText;
                    addParameters[index].PARAMVALUE = BodyNodeValue.InnerText;
                    index++;
                }
                catch
                {
                    // 등록되지 않은 파라메터 이름을 수신했을 경우
                    m_objPPParamRmoteRequestReply.BODY.ACKC7 = CIM.Acknowledge7.IF_THERE_IS_A_PARAMETER_ABNORMALITY.ToString();
                    return false;
                }
            }

            m_objPPParamRmoteRequest.BODY.COMMANDS[0].PARAMS = addParameters;
            return true;
        }

        /// <summary>
        /// 수신 받은 PPID_NUMBER를 'PPID 번호'와 'PPID 첨자'로 파싱함
        /// </summary>
        /// <param name="rawdata">수신 받은 PPID_NUMBER</param>
        /// <param name="number">PPID 번호</param>
        /// <param name="subscript">PPID 첨자</param>
        /// <returns>true = 성공, false = 실패</returns>
        private bool tryPasrePpidNumber(string rawdata, out int number, out string subscript)
        {
            subscript = string.Empty;
            number = -1;

            // {PPID 번호}['N', 'O', 'D', 'G', 'C']
            Regex regex = new Regex(@"(^\d+)(\D$)");
            var match = regex.Match(rawdata);
            if (match.Success == false)
            {
                return false;
            }

            if (int.TryParse(match.Groups[1].ToString(), out number) == false)
            {
                return false;
            }
            subscript = match.Groups[2].ToString();

            return true;
        }
    }
}
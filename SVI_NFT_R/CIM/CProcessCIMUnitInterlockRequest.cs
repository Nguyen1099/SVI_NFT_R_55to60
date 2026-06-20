using System;
using System.Linq;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMUnitInterlockRequest : CCIMAbstract
    {
        private enum EInterlock
        {
            None = 0,

            /// <summary>
            /// 후 공정으로 Cell 반송을 중지하고 자공정의 cell가공을 완료한 시점에 정지
            /// </summary>
            TransferStop,
            /// <summary>
            /// 전(상류) 공정으로부터의 Cell/Panel Loading을 중지하고 자공정의 Cell/Panel 가공을 완료하여 후(하류) 공정으로 배출을 완료 한 시점에 정지
            /// </summary>
            LoadingStop,
            /// <summary>
            /// Host로부터 Command 수신 받은 시점에 설비가 동작 중이던 Step을 완료한 시점에 정지
            /// </summary>
            StepStop,
            /// <summary>
            /// Host로부터 Command 수신 받은 시점에 설비가 판단하여 Cell/Panel 반송과 동작 Step/Cycle을 중지/정지
            /// </summary>
            OwnStop,

            /// <summary>
            /// Force Interlock 해제를 허용함
            /// </summary>
            /// <remarks>
            /// Force Interlock 이후 Release Command 수신 전 Start를 할 수 없음
            /// </remarks>
            ReleaseForceInterlock
        }
        private enum ETarget
        {
            None = 0,

            Equipment,
            Unit
        }
        private UnitInterlockRequest m_objUnitInterlockRequest;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMUnitInterlockRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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

                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                // 데이터 생성.
                m_objUnitInterlockRequest = new UnitInterlockRequest();
                m_objUnitInterlockRequest.EQPID = m_strEquipmentID;

                m_bReceiveData = false;

                // 쓰레드 종료 Flag
                m_bThreadExit = false;
                // 쓰레드 객체 생성.
                m_ThreadProcess = new Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start();

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
                    if ("UNITINTERLOCKREQUEST" == name)
                    {
                        m_objUnitInterlockRequest.EQPID = eqpid;
                        m_objUnitInterlockRequest.NAME = name;
                        m_objUnitInterlockRequest.TRANSACTIONNO = transactionno;

                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("RCMD" == tagName)
                                    m_objUnitInterlockRequest.BODY.RCMD = value;
                                else if ("INTERLOCK" == tagName)
                                    m_objUnitInterlockRequest.BODY.INTERLOCK = value;
                                else if ("INTERLOCKID" == tagName)
                                    m_objUnitInterlockRequest.BODY.INTERLOCKID = value;
                                else if ("UNITID" == tagName)
                                    m_objUnitInterlockRequest.BODY.UNITID = value;
                                else if ("MESSAGE" == tagName)
                                    m_objUnitInterlockRequest.BODY.MESSAGE = value;

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
        public UnitInterlockRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objUnitInterlockRequest;
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
        /// <![CDATA[
        /// 'm_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;' 를 호출하면 다른 스레드에서 인터락 메시지 창을 띄운다.
        /// ]]>
        protected override bool DoProcess()
        {
            bool bResult = false;

            do
            {
                m_bReceiveData = false;

                ETarget target = ETarget.None;
                EInterlock interlock = EInterlock.None;
                CCIMDefine.EUnit unitIndex = 0;
                bool bIsForceInterlock = false;
                // 메시지 응답 처리
                if (SetSubCommandSendReply(ref target, ref interlock, ref unitIndex, ref bIsForceInterlock) == false)
                {
                    break;
                }

                // Show Unit Interlock Message UI
                m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;

                // Force Interlock 처리
                if (interlock == EInterlock.ReleaseForceInterlock)
                {
                    SetSubCommandReleaseForceInterlock();
                }
                else if (bIsForceInterlock == true)
                {
                    SetSubCommandSetForceInterlock();
                }

                // Unit Interlock 동작
                if (target == ETarget.Equipment)
                {
                    SetSubCommandEquipmentLoadingStop(interlock);
                }
                if (target == ETarget.Unit)
                {
                    SetSubCommandUnitInterlock(interlock, unitIndex);
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        private bool SetSubCommandSendReply(ref ETarget target, ref EInterlock interlock, ref CCIMDefine.EUnit unitIndex, ref bool bIsForceInterlock)
        {
            var unitInterlockReply = new UnitInterlockReply();
            unitInterlockReply.EQPID = m_strEquipmentID;
            unitInterlockReply.TRANSACTIONNO = m_objUnitInterlockRequest.TRANSACTIONNO;
            unitInterlockReply.BODY.RCMD = m_objUnitInterlockRequest.BODY.RCMD;
            unitInterlockReply.BODY.HCACK = string.Empty;
            string unitId = m_objUnitInterlockRequest.BODY.UNITID;
            string remoteCommand = m_objUnitInterlockRequest.BODY.RCMD;

            try
            {
                string interlockControlValue = m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_INTERLOCK_CONTROL];
                switch (remoteCommand)
                {
                    case "11":
                    case "41":
                        // 설비에 설정된 Function이 아닐 때 HCACK '5'로 응답함
                        if (interlockControlValue != "TRANSFER")
                        {
                            unitInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_NOT_FOUND_FUNCTION);
                            return false;
                        }
                        interlock = EInterlock.TransferStop;
                        break;

                    case "12":
                    case "42":
                        // 설비에 설정된 Function이 아닐 때 HCACK '5'로 응답함
                        if (interlockControlValue != "LOADING")
                        {
                            unitInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_NOT_FOUND_FUNCTION);
                            return false;
                        }
                        interlock = EInterlock.LoadingStop;
                        break;

                    case "13":
                    case "43":
                        // 설비에 설정된 Function이 아닐 때 HCACK '5'로 응답함
                        if (interlockControlValue != "STEP")
                        {
                            unitInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_NOT_FOUND_FUNCTION);
                            return false;
                        }
                        interlock = EInterlock.StepStop;
                        break;

                    case "14":
                    case "44":
                        // 설비에 설정된 Function이 아닐 때 HCACK '5'로 응답함
                        if (interlockControlValue != "OWN")
                        {
                            unitInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_NOT_FOUND_FUNCTION);
                            return false;
                        }
                        interlock = EInterlock.OwnStop;
                        break;

                    case "45":
                        interlock = EInterlock.ReleaseForceInterlock;
                        break;

                    default:
                        // 처리 할 수 없는 RCMD를 받았을 때 HCACK '?'로 응답함
                        unitInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_REJECT);
                        return false;
                }
                bIsForceInterlock = remoteCommand.StartsWith("4");

                target = ETarget.None;
                var unitIndexes = Enum.GetValues(typeof(CCIMDefine.EUnit))
                    .Cast<CCIMDefine.EUnit>()
                    .Where(i => unitId == m_objDocument.m_objProcessCIM.GetUnitId(i));
                // EQP LOADING STOP인 경우 허용되는 UNITID는 [EQPID or Null or  '-']이다
                if (m_objDocument.m_objProcessCIM.GetIsEqLoadingStopFromUnitId(unitId) == true)
                {
                    target = ETarget.Equipment;
                }
                // 설비에 등록된 UNITID 일 때
                if (unitIndexes.Count() > 0)
                {
                    unitIndex = unitIndexes.First();
                    target = ETarget.Unit;
                }

                // 설비에 등록되지 않은 UNITID 일때 Unit Interlock 요청을 받으면 HCACK '4'로 응답함
                if (target == ETarget.None)
                {
                    unitInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_OTHER_ERROR);
                    return false;
                }

                // Unit Puase 상태에서 Unit Interlock 요청을 받으면 HCACK '6'으로 응답함
                if (target == ETarget.Unit
                    && UnitManager.Units[unitIndex].GetStatus() == UnitGroup.EStatus.Pause
                    )
                {
                    unitInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_THIS_IS_NOT_SUPPORT_MODE);
                    return false;
                }

                // Unit Interlock 처리가 가능한 경우 HCACK '0'으로 응답함
                unitInterlockReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EInterlockReply.INTERLOCK_REPLY_ACCEPTED);
                return true;
            }
            finally
            {
                // 응답 데이터 전송
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetUnitInterlockReply(unitInterlockReply);
            }
        }

        private bool SetSubCommandSetForceInterlock()
        {
            /* CIM CHECK LIST에는 시나리오가 있으나 URCIS에서는 한 번도 사용한 이력이 없는 시나리오라고 함. 
             * 그래서 CIM 검수시 지적사항이 나오면 구현 예정. 
             * (20230316 URCIS 최영탁K에게 유선상으로 확인한 내용) 
             */
            return true;
        }

        private bool SetSubCommandReleaseForceInterlock()
        {
            /* CIM CHECK LIST에는 시나리오가 있으나 URCIS에서는 한 번도 사용한 이력이 없는 시나리오라고 함. 
             * 그래서 CIM 검수시 지적사항이 나오면 구현 예정. 
             * (20230316 URCIS 최영탁K에게 유선상으로 확인한 내용) 
             */
            return true;
        }

        private bool SetSubCommandEquipmentLoadingStop(EInterlock interlock)
        {
            switch (interlock)
            {
                case EInterlock.LoadingStop:
                    // [ LOADING STOP의 동작 정의 ]
                    //  Case1. RUN STATE가 RUN 상태일 때 
                    //   1. 메시지 디스플레이
                    //   2. 시료 투입 정지
                    //   3. RUN STATE가 IDLE이 될 때까지 시퀀스 진행
                    //   4. INTERLOCK 상태 DOWN으로 변경
                    //   5. 정지
                    //   6. 메시지 컨펌 (언제 컨펌해도 상관 없음)
                    //   7. 설비 다시 런 시작
                    //  Case2. RUN STATE가 IDLE 상태일 때
                    //   1. 메시지 디스플레이
                    //   2. INTERLOCK 상태 DOWN으로 변경
                    //   3. 정지
                    //   4. 메시지 컨펌 (언제 컨펌해도 상관 없음)
                    //   5. 설비 다시 런 시작
                    if (CCIMDefine.ERunState.RUN_STATE_RUN == m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                    {
                        // 설비 내부에 CELL이 있을 경우 Loading Stop 진행
                        if (CDefine.ERunStatus.Stop != m_objDocument.GetRunStatus()
                            && CDefine.ERunStatus.Stopping != m_objDocument.GetRunStatus()
                            && CDefine.ERunStatus.Setup != m_objDocument.GetRunStatus()
                            && CDefine.ERunStatus.LoadingStop != m_objDocument.GetRunStatus()
                            )
                        {
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.LoadingStop);
                        }
                        m_objDocument.m_bLoadingStopFinish = false;
                        m_objDocument.m_bUnitInterLockHappened = true;
                    }
                    else if (CCIMDefine.ERunState.RUN_STATE_IDLE == m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                    {
                        // 설비 내부에 CELL이 없을 경우 Cycle Stop 진행
                        if (CDefine.ERunStatus.Stop != m_objDocument.GetRunStatus()
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
                        }
                    }
                    break;

                case EInterlock.StepStop:
                    // Cycle Stop 진행
                    if (CDefine.ERunStatus.Stop != m_objDocument.GetRunStatus()
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
                    }
                    break;

                default:
                    return false;
            }

            return true;
        }

        private bool SetSubCommandUnitInterlock(EInterlock interlock, CCIMDefine.EUnit unitIndex)
        {
            switch (interlock)
            {
                case EInterlock.LoadingStop:
                case EInterlock.StepStop:
                    UnitManager.Units[unitIndex].SetCommand(UnitGroup.ECommand.Interlock);
                    break;

                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadProcess()
        {
            while (m_bThreadExit == false)
            {
                if (IsStatus() == true)
                {
                    if (DoProcess() == true)
                    {
                        //m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}

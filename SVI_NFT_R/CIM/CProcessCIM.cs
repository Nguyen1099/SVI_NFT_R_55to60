using ENC.MemoryMap.Manager;
using ENC.MemoryMap.Pages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIM
    {
        // enum
        private enum State
        {
            AVAILABILITY_STATE = 0,
            INTERLOCK_STATE,
            MOVE_STATE,
            RUN_STATE,
            FRONT_STATE,
            REAR_STATE,
            STATE_FINAL
        };

        // Request 메시지
        public CProcessCIMCellJobProcessRequest m_objProcessCIMCellJobProcessRequest;
        public CProcessCIMControlStateRequest m_objProcessCIMControlStateRequest;
        public CProcessCIMConveyorMoveStateRequest m_objProcessCIMConveyorMoveStateRequest;
        public CProcessCIMCurrentAlarmListRequest m_objProcessCIMCurrentAlarmListRequest;
        public CProcessCIMCurrentPPIDRequest m_objProcessCIMCurrentPPIDRequest;
        public CProcessCIMDateAndTimeSetRequest m_objProcessCIMDateAndTimeSetRequest;
        public CProcessCIMECListRequest m_objProcessCIMECListRequest;
        public CProcessCIMECSetRequest m_objProcessCIMECSetRequest;
        public CProcessCIMEQStateRequest m_objProcessCIMEQStateRequest;
        public CProcessCIMFunctionChangeRequest m_objProcessCIMFunctionChangeRequest;
        public CProcessCIMFunctionStateRequest m_objProcessCIMFunctionStateRequest;
        public CProcessCIMInterlockRequest m_objProcessCIMInterlockRequest;
        public CProcessCIMOPCallRequest m_objProcessCIMOPCallRequest;
        public CProcessCIMPPIDListRequest m_objProcessCIMPPIDListRequest;
        public CProcessCIMPPParamRequest m_objProcessCIMPPParamRequest;
        public CProcessCIMPPParamRemoteRequest m_objProcessCIMPPParamRemoteRequest;
        public CProcessCIMSpecificationversionStateRequest m_objProcessCIMPSpecificationversionStateRequest;
        public CProcessCIMTerminalDisplayRequest m_objProcessCIMTerminalDisplayRequest;
        public CProcessCIMTraceRequest m_objProcessCIMTraceRequest;
        public CProcessCIMUnitInterlockRequest m_objProcessCIMUnitInterlockRequest;
        public CProcessCIMUnitStateRequest m_objProcessCIMUnitStateRequest;
        public CProcessCIMCellInformationDownload m_objProcessCIMCellInfomationDownload;
        public CProcessCIMCarrierInformationDownload m_objProcessCIMCarrierInformationDownload;
        public CProcessCIMCellLotInformationReply m_objProcessCIMCellLotInfomationReply;
        public CProcessCIMCellLotInformationDownload m_objProcessCIMCellLotInfomationDownload;
        public CProcessCIMEquipmentApproveSend m_objProcessCIMEquipmentApproveSend;

        // Send 메시지.
        public CCIMSendMessage m_objCIMSendMessage;
        // 쓰레드 
        private Thread m_ThreadOPCall;
        private Thread m_ThreadInterlock;
        private Thread m_ThreadUnitInterlock;
        private Thread m_ThreadTerminal;
        private Thread m_ThreadEQState;
        private Thread m_ThreadTPConnectState;
        private bool m_bThreadExit;
        UnitInterlockRequest m_objUnitInterlockRequest = new UnitInterlockRequest();
        private bool mbEquipmentStateReportInterlock = false;
        private ManualResetEvent mSyncEquipmentStateReport = new ManualResetEvent(false);
        private AutoResetEvent mEquipmentStateReportSleep = new AutoResetEvent(false);
        private class InterlockMessage
        {
            public DateTime DateTime { get; set; }
            public string InterlockID { get; set; }
            public string Message { get; set; }
            public string Rcmd { get; set; }
        }
        private class UnitInterlockMessage
        {
            public DateTime DateTime { get; set; }
            public string Rcmd { get; set; }
            public string Interlock { get; set; }
            public string InterlockID { get; set; }
            public string UnitID { get; set; }
            public string Message { get; set; }
        }
        // Document
        private CDocument m_objDocument;
        private Queue<InterlockMessage> mInterlockMessageBuffer = new Queue<InterlockMessage>();
        private Queue<UnitInterlockMessage> mUnitInterlockMessageBuffer = new Queue<UnitInterlockMessage>();
        // 통신 객체
        private urLinkDllCommunicator _objCommunicator;
        public urLinkDllCommunicator m_objCommunicator
        {
            get { return _objCommunicator; }
        }
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public bool Initialize(object objDocument)
        {
            bool bResult = false;
            do
            {
                CCIMDefine.structureCIMInitialize objCIMInitialize = new CCIMDefine.structureCIMInitialize();
                // Document 가져오기.
                m_objDocument = (CDocument)objDocument;
                // Communicator 가져오기.
                if (null != m_objCommunicator)
                    DisConnect();
                _objCommunicator = new urLinkDllCommunicator();
                objCIMInitialize.objCommunicator = m_objCommunicator;
                // 이벤트 등록
                m_objCommunicator.OnConnected += Communicator_OnConnected;
                m_objCommunicator.OnDisconnected += Communicator_OnDisconnected;
                m_objCommunicator.OnReceiveRequest += Communicator_OnReceiveRequest;
                objCIMInitialize.objDocument = objDocument;

                // Request 메시지 처리하는 객체
                m_objProcessCIMCellJobProcessRequest = new CProcessCIMCellJobProcessRequest(objCIMInitialize);
                m_objProcessCIMControlStateRequest = new CProcessCIMControlStateRequest(objCIMInitialize);
                m_objProcessCIMConveyorMoveStateRequest = new CProcessCIMConveyorMoveStateRequest(objCIMInitialize);
                m_objProcessCIMCurrentAlarmListRequest = new CProcessCIMCurrentAlarmListRequest(objCIMInitialize);
                m_objProcessCIMCurrentPPIDRequest = new CProcessCIMCurrentPPIDRequest(objCIMInitialize);
                m_objProcessCIMDateAndTimeSetRequest = new CProcessCIMDateAndTimeSetRequest(objCIMInitialize);
                m_objProcessCIMECListRequest = new CProcessCIMECListRequest(objCIMInitialize);
                m_objProcessCIMECSetRequest = new CProcessCIMECSetRequest(objCIMInitialize);
                m_objProcessCIMEQStateRequest = new CProcessCIMEQStateRequest(objCIMInitialize);
                m_objProcessCIMFunctionChangeRequest = new CProcessCIMFunctionChangeRequest(objCIMInitialize);
                m_objProcessCIMFunctionStateRequest = new CProcessCIMFunctionStateRequest(objCIMInitialize);
                m_objProcessCIMInterlockRequest = new CProcessCIMInterlockRequest(objCIMInitialize);
                m_objProcessCIMOPCallRequest = new CProcessCIMOPCallRequest(objCIMInitialize);
                m_objProcessCIMPPIDListRequest = new CProcessCIMPPIDListRequest(objCIMInitialize);
                m_objProcessCIMPPParamRequest = new CProcessCIMPPParamRequest(objCIMInitialize);
                m_objProcessCIMPPParamRemoteRequest = new CProcessCIMPPParamRemoteRequest(objCIMInitialize);
                m_objProcessCIMPSpecificationversionStateRequest = new CProcessCIMSpecificationversionStateRequest(objCIMInitialize);
                m_objProcessCIMTerminalDisplayRequest = new CProcessCIMTerminalDisplayRequest(objCIMInitialize);
                m_objProcessCIMTraceRequest = new CProcessCIMTraceRequest(objCIMInitialize);
                m_objProcessCIMUnitInterlockRequest = new CProcessCIMUnitInterlockRequest(objCIMInitialize);
                m_objProcessCIMUnitStateRequest = new CProcessCIMUnitStateRequest(objCIMInitialize);
                m_objProcessCIMCellInfomationDownload = new CProcessCIMCellInformationDownload(objCIMInitialize);
                m_objProcessCIMCarrierInformationDownload = new CProcessCIMCarrierInformationDownload(objCIMInitialize);
                m_objProcessCIMCellLotInfomationReply = new CProcessCIMCellLotInformationReply(objCIMInitialize);
                m_objProcessCIMCellLotInfomationDownload = new CProcessCIMCellLotInformationDownload(objCIMInitialize);
                m_objProcessCIMEquipmentApproveSend = new CProcessCIMEquipmentApproveSend(objCIMInitialize);
                // Send Message 관련 객체
                CCIMImplementSVI_M objCIMImplementSVI_TM = new CCIMImplementSVI_M(objCIMInitialize);
                m_objCIMSendMessage = new CCIMSendMessage(objCIMImplementSVI_TM);

                // 쓰레드 종료
                m_bThreadExit = false;
                // OP CALL 감시 쓰레드
                m_ThreadOPCall = new Thread(ThreadProcessOPCall);
                m_ThreadOPCall.IsBackground = true;
                m_ThreadOPCall.Start(this);
                // Interlock 감시 쓰레드
                m_ThreadInterlock = new Thread(ThreadProcessInterlock);
                m_ThreadInterlock.IsBackground = true;
                m_ThreadInterlock.Start(this);
                // Unit Interlock 감시 쓰레드
                m_ThreadUnitInterlock = new Thread(ThreadProcessUnitInterlock);
                m_ThreadUnitInterlock.IsBackground = true;
                m_ThreadUnitInterlock.Start(this);
                // Terminal 감시 쓰레드
                m_ThreadTerminal = new Thread(ThreadProcessTerminal);
                m_ThreadTerminal.IsBackground = true;
                m_ThreadTerminal.Start(this);
                // EQ State 감시 쓰레드
                m_ThreadEQState = new Thread(ThreadProcessEQState);
                m_ThreadEQState.IsBackground = true;
                m_ThreadEQState.Start(this);
                // TP 연결 상태 확인.
                m_ThreadTPConnectState = new Thread(ThreadTPConnectState);
                m_ThreadTPConnectState.IsBackground = true;
                m_ThreadTPConnectState.Start(this);

                IsInitialized = true;

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            m_bThreadExit = true;
            m_objProcessCIMTraceRequest.DeInitialize();
            m_objCIMSendMessage.DeInitialize();
            m_objProcessCIMCellJobProcessRequest.DeInitialize();
            m_objProcessCIMControlStateRequest.DeInitialize();
            m_objProcessCIMConveyorMoveStateRequest.DeInitialize();
            m_objProcessCIMCurrentAlarmListRequest.DeInitialize();
            m_objProcessCIMCurrentPPIDRequest.DeInitialize();
            m_objProcessCIMDateAndTimeSetRequest.DeInitialize();
            m_objProcessCIMECListRequest.DeInitialize();
            m_objProcessCIMECSetRequest.DeInitialize();
            m_objProcessCIMEQStateRequest.DeInitialize();
            m_objProcessCIMFunctionChangeRequest.DeInitialize();
            m_objProcessCIMFunctionStateRequest.DeInitialize();
            m_objProcessCIMInterlockRequest.DeInitialize();
            m_objProcessCIMOPCallRequest.DeInitialize();
            m_objProcessCIMPPIDListRequest.DeInitialize();
            m_objProcessCIMPPParamRequest.DeInitialize();
            m_objProcessCIMPPParamRemoteRequest.DeInitialize();
            m_objProcessCIMPSpecificationversionStateRequest.DeInitialize();
            m_objProcessCIMTerminalDisplayRequest.DeInitialize();
            m_objProcessCIMUnitInterlockRequest.DeInitialize();
            m_objProcessCIMUnitStateRequest.DeInitialize();
            m_objProcessCIMCellInfomationDownload.DeInitialize();
            m_objProcessCIMCarrierInformationDownload.DeInitialize();
            m_objProcessCIMCellLotInfomationReply.DeInitialize();
            m_objProcessCIMCellLotInfomationDownload.DeInitialize();
            m_objProcessCIMEquipmentApproveSend.DeInitialize();

            m_ThreadOPCall.Join();
            m_ThreadInterlock.Join();
            m_ThreadTerminal.Join();
            m_ThreadUnitInterlock.Join();
            m_ThreadEQState.Join();
            m_ThreadTPConnectState.Join();
            DisConnect();
            mSyncEquipmentStateReport.Dispose();
            mEquipmentStateReportSleep.Dispose();

            IsInitialized = false;
        }

        /// <summary>
        /// 유알시스와 연결
        /// </summary>
        public void Connect()
        {
            // 접속 중 다이얼로그 
            //m_objDocument.GetMainFrame().ShowCIMConnect( true );

            urLinkDllCommunicatorInfo CommunicatorInfo = new urLinkDllCommunicatorInfo();
            CommunicatorInfo.Active = false;
            CommunicatorInfo.IP = m_objDocument.m_objConfig.GetCimParameter().strIPAddress; // IP 주소
            CommunicatorInfo.Port = m_objDocument.m_objConfig.GetCimParameter().iPortNo;    // Port 번호

            // 접속 시작.
            m_objCommunicator.Info = CommunicatorInfo;
            m_objCommunicator.Open();
        }

        /// <summary>
        /// 연결 끊기
        /// </summary>
        public void DisConnect()
        {
            m_objCommunicator.Close();
            m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, "HOST DISCONNECT.");
        }

        public void BeginSyncEquipmentStateReport()
        {
            mbEquipmentStateReportInterlock = true;
            mSyncEquipmentStateReport.WaitOne();
        }

        public void EndSyncEquipmentStateReport()
        {
            mbEquipmentStateReportInterlock = false;
            mEquipmentStateReportSleep.Set();
        }

        public void WaitSyncEquipmentStateReport() => mSyncEquipmentStateReport.WaitOne();

        /// <summary>
        /// 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Communicator_OnConnected(object sender, EventArgs e)
        {
            m_objDocument.m_bCIMConnected = true;
            m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, "CIM is Connected.");
        }

        /// <summary>
        /// 미 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Communicator_OnDisconnected(object sender, EventArgs e)
        {
            m_objDocument.m_bCIMConnected = false;
            m_objDocument.m_eControlState = CCIMDefine.EControlState.CRST_OFFLINE;
            m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, "CIM is Disconnected.");
            m_objDocument.SetForcedAlarm(CAlarmDefine.EAlarmList.CO_TRANSFER_CIM_N_CIM_DISCONNECTED);
        }

        /// <summary>
        /// OP CALL 감시 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcessOPCall(object state)
        {
            CProcessCIM pThis = (CProcessCIM)state;
            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsState(CCIMDefine.EMessageType.MESSAGE_TYPE_OP_CALL))
                {
                    pThis.DoProcessMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_OP_CALL);
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Interlock 감시 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcessInterlock(object state)
        {
            CProcessCIM pThis = (CProcessCIM)state;
            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsState(CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK_BUFFERING))
                {
                    pThis.DoProcessMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK_BUFFERING);
                }
                else if (pThis.IsState(CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK))
                {
                    pThis.DoProcessMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK);
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Unit Interlock 감시 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcessUnitInterlock(object state)
        {
            CProcessCIM pThis = (CProcessCIM)state;
            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsState(CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK_BUFFERING))
                {
                    pThis.DoProcessMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK_BUFFERING);
                }
                else if (pThis.IsState(CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK))
                {
                    pThis.DoProcessMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK);
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 터미널 메시지 감시 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcessTerminal(object state)
        {
            CProcessCIM pThis = (CProcessCIM)state;
            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsState(CCIMDefine.EMessageType.MESSAGE_TYPE_TERMINAL))
                {
                    pThis.DoProcessMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_TERMINAL);
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// EQ State 감시 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcessEQState(object state)
        {
            CProcessCIM pThis = (CProcessCIM)state;

            while (false == pThis.m_bThreadExit)
            {
                // 무언정지 상태 체크
                //pThis.DoProcessCheckSilenceStopState();
                // 하트비트 상태 체크
                //pThis.DoProcessCheckHeartBeatState();
                // 장비내 셀유무 체크.
                pThis.DoProcessCheckCell();
                // 장비 상태 변경 체크.
                pThis.DoProcessCheckEQState();
                // 모터 모션 상태 확인
                //pThis.DoProcessCheckMoveState();
                // 컨베어 상태 확인
                pThis.DoProcessCheckConveyorState();

                pThis.mEquipmentStateReportSleep.WaitOne(500);
            }
        }

        /// <summary>
        /// TP 연결 상태 확인.
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadTPConnectState(object state)
        {
            CProcessCIM pThis = (CProcessCIM)state;

            while (false == pThis.m_bThreadExit)
            {
                pThis.DoProcessCheckTPConnect();
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// TP 연결 상태 확인
        /// </summary>
        private void DoProcessCheckTPConnect()
        {
            if (CCIMDefine.EProcessState.STS_RECEIVED == m_objProcessCIMControlStateRequest.GetStatus())
            {
                ControlStateSetRequest recvData = m_objProcessCIMControlStateRequest.GetReceivedData();
                if (true == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM)
                {
                    if (CCIMDefine.EControlState.CRST_ONLINE_REMOTE != m_objDocument.m_eControlState)
                    {
                        m_objDocument.SetForcedAlarm(CAlarmDefine.EAlarmList.CO_TRANSFER_CIM_N_HOST_DISCONNECTED);
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, "Host is Disconnected.");
                    }
                }
            }
        }

        /// <summary>
        /// 컨베어 상태 체크.
        /// </summary>
        private void DoProcessCheckConveyorState()
        {
            /*
            // 모터 상태 가져오기.
            CDeviceMotorAbstract.CMotorStatus objMotorStatus = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objProcessManagerOutConveyor.m_objProcessMotionOutConveyor.m_objMotorOutConveyorX.HLGetMotorStatus();
            // 컨베어 상태 확인.
            if(m_objDocument.GetRunMode() == CDefine.enumRunMode.RUN_MODE_START)
            {
                // 모터 상태가 Inposition 상태일 경우.
                if(objMotorStatus.bInposition == true)
                {
                    m_objDocument.m_eConveyorMoveState[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorMoveState.CONVEYOR_MOVE_STATE_STOP;
                    m_objDocument.m_eConveyorStopReason[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorStopReason.CONVEYOR_STOP_REASON_RUN_WAIT;
                }
                else
                {
                    m_objDocument.m_eConveyorMoveState[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorMoveState.CONVEYOR_MOVE_STATE_MOVE;
                }
            }
            // 알람 발생하여 정지할 경우.
            else if(m_objDocument.GetIsAlarmMessage())
            {
                m_objDocument.m_eConveyorMoveState[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorMoveState.CONVEYOR_MOVE_STATE_STOP;
                m_objDocument.m_eConveyorStopReason[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorStopReason.CONVEYOR_STOP_REASON_ALARM_STOP;
            }
            // 하류 설비가 다운인 경우. 
            else if(m_objDocument.m_eRearState[(int)CCIMDefine.PresentState.CURRENT_STATE] == CCIMDefine.RearState.REAR_STATE_DOWN)
            {
                m_objDocument.m_eConveyorMoveState[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorMoveState.CONVEYOR_MOVE_STATE_STOP;
                m_objDocument.m_eConveyorStopReason[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorStopReason.CONVEYOR_STOP_REASON_LOW_DOWN;
            }
            // 그외 사용자가 정지한 경우.   
            else if(m_objDocument.GetRunMode() == CDefine.enumRunMode.RUN_MODE_STOP)
            {
                m_objDocument.m_eConveyorMoveState[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorMoveState.CONVEYOR_MOVE_STATE_STOP;
                m_objDocument.m_eConveyorStopReason[(int)CCIMDefine.PresentState.CURRENT_STATE] = CCIMDefine.ConveyorStopReason.CONVEYOR_STOP_REASON_OPERATOR_STOP;
            }
             * */
        }

        /// <summary>
        /// 장비내 셀 존재 유무 체크
        /// </summary>
        private void DoProcessCheckCell()
        {
            bool bIsCell = CellDataManager.Cells.Values.IsCellExistFromList();

            // 셀이 존재하면 Run State를 Run으로 변경 없으면 IDLE로 변경한다.
            // 2023-05-25 CIM 사전검수 Track Out 후 상태변경되게끔 변경
            if (
                true == bIsCell
                || true == m_objDocument.m_objProcessMain.m_objProcessMotion.TrackOut.IsBusy
                )
            {
                m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.ERunState.RUN_STATE_RUN;
            }
            else
            {
                m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.ERunState.RUN_STATE_IDLE;
            }
        }

        /// <summary>
        /// 장비 상태 변경 체크
        /// </summary>
        private void DoProcessCheckEQState()
        {
            mSyncEquipmentStateReport.Reset();
            bool[] bChange = { false, false, false, false, false, false };
            do
            {
                if (true == mbEquipmentStateReportInterlock)
                {
                    break;
                }

                // Availability State
                if (m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE] != m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE])
                {
                    m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE];
                    bChange[(int)State.AVAILABILITY_STATE] = true;
                }
                else
                {
                    bChange[(int)State.AVAILABILITY_STATE] = false;
                }
                // Interlock State
                if (m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE] != m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE])
                {
                    m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE];
                    bChange[(int)State.INTERLOCK_STATE] = true;
                }
                else
                {
                    bChange[(int)State.INTERLOCK_STATE] = false;
                }
                // Move State
                if (m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] != m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE])
                {
                    m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE];
                    bChange[(int)State.MOVE_STATE] = true;
                }
                else
                {
                    bChange[(int)State.MOVE_STATE] = false;
                }
                // Run State
                if (m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE] != m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE])
                {
                    m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE];
                    bChange[(int)State.RUN_STATE] = true;
                }
                else
                {
                    bChange[(int)State.RUN_STATE] = false;
                }

                // Front State
                if (m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.CURRENT_STATE] != m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE])
                {
                    m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.CURRENT_STATE];
                    bChange[(int)State.FRONT_STATE] = true;
                }
                else
                {
                    bChange[(int)State.FRONT_STATE] = false;
                }

                // Rear State
                if (m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.CURRENT_STATE] != m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE])
                {
                    m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.CURRENT_STATE];
                    bChange[(int)State.REAR_STATE] = true;
                }
                else
                {
                    bChange[(int)State.REAR_STATE] = false;
                }

                int iCount = 0;
                // 상태가 변경된 갯수 카운트.
                for (int iLoopCount = 0; iLoopCount < (int)State.STATE_FINAL; iLoopCount++)
                {
                    if (bChange[iLoopCount] == true)
                        iCount++;
                }

                // 변경된 상태가 없으면 
                if (iCount == 0)
                    break;

                // 두개 상태가 변경되었을 경우.
                if (iCount == 2)
                {
                    // 인터락 변경시...
                    if (bChange[(int)State.INTERLOCK_STATE] == true)
                    {
                        InterlockAndMoveStateChangeEvent objInterlockAndMoveStateChangeEvent = new InterlockAndMoveStateChangeEvent();
                        objInterlockAndMoveStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        objInterlockAndMoveStateChangeEvent.BODY.INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                        objInterlockAndMoveStateChangeEvent.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                        objInterlockAndMoveStateChangeEvent.BODY.REASONCODE = " ";
                        objInterlockAndMoveStateChangeEvent.BODY.DESCRIPTION = " ";
                        m_objCIMSendMessage.SetInterlockAndMoveStateChangeEvnet(objInterlockAndMoveStateChangeEvent);
                    }
                    else
                    {
                        // Availability And Move State가 동시 변경 되었거나 
                        if (m_objDocument.GetIsAlarmMessage() || (bChange[(int)State.AVAILABILITY_STATE] && bChange[(int)State.MOVE_STATE]))
                        {
                            AvailabilityAndMoveStateChangeEvent objAvailabilityAndMoveStateChangeEvent = new AvailabilityAndMoveStateChangeEvent();
                            objAvailabilityAndMoveStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                            objAvailabilityAndMoveStateChangeEvent.BODY.AVAILABILITYSTATE = string.Format("{0}", (int)m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objAvailabilityAndMoveStateChangeEvent.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objAvailabilityAndMoveStateChangeEvent.BODY.REASONCODE = " ";
                            objAvailabilityAndMoveStateChangeEvent.BODY.DESCRIPTION = " ";
                            m_objCIMSendMessage.SetAvailabilityAndMoveStateChangeEvent(objAvailabilityAndMoveStateChangeEvent);
                        }
                        else
                        {
                            EQStateChangeEvent objEQStateChangeEvent = new EQStateChangeEvent();
                            objEQStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                            objEQStateChangeEvent.BODY.AVAILABILITYSTATE = string.Format("{0}", (int)m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objEQStateChangeEvent.BODY.INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objEQStateChangeEvent.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objEQStateChangeEvent.BODY.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objEQStateChangeEvent.BODY.FRONTSTATE = string.Format("{0}", (int)m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objEQStateChangeEvent.BODY.REARSTATE = string.Format("{0}", (int)m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objEQStateChangeEvent.BODY.PP_SPLSTATE = string.Format("{0}", (int)m_objDocument.m_ePP_SPLState);
                            objEQStateChangeEvent.BODY.REASONCODE = " ";
                            objEQStateChangeEvent.BODY.DESCRIPTION = " ";
                            m_objCIMSendMessage.SetEqStateChangeEvent(objEQStateChangeEvent);
                        }
                    }
                }
                else if (iCount == 1) // 여러 상태들중 변경된 상태가 한개일 경우.
                {
                    if (bChange[(int)State.AVAILABILITY_STATE]) // Availability State
                    {
                        AvailabilityStateChangeEvent objAvailabilityStateChangeEvent = new AvailabilityStateChangeEvent();
                        objAvailabilityStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        objAvailabilityStateChangeEvent.BODY.AVAILABILITYSTATE = string.Format("{0}", (int)m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                        objAvailabilityStateChangeEvent.BODY.REASONCODE = " ";
                        objAvailabilityStateChangeEvent.BODY.DESCRIPTION = " ";
                        m_objCIMSendMessage.SetAvailabityStateChangeEvent(objAvailabilityStateChangeEvent);
                    }
                    else if (bChange[(int)State.INTERLOCK_STATE]) // Interlock State
                    {
                        InterlockStateChangeEvent objInterlockStateChangeEvent = new InterlockStateChangeEvent();
                        objInterlockStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        objInterlockStateChangeEvent.BODY.INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                        objInterlockStateChangeEvent.BODY.REASONCODE = " ";
                        objInterlockStateChangeEvent.BODY.DESCRIPTION = " ";
                        m_objCIMSendMessage.SetInterlockStateChangeEvent(objInterlockStateChangeEvent);
                    }
                    else if (bChange[(int)State.MOVE_STATE]) // Move State
                    {
                        if (bChange[(int)State.INTERLOCK_STATE] == false)
                        {
                            MoveStateChangeEvent objMoveStateChangeEvent = new MoveStateChangeEvent();
                            objMoveStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                            objMoveStateChangeEvent.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objMoveStateChangeEvent.BODY.REASONCODE = " ";
                            objMoveStateChangeEvent.BODY.DESCRIPTION = " ";
                            // MOVE STATE 하나만 바뀌는 상황은 TPM LOSS CODE를 띄우는 상황임
                            // !정지시에는 TPM LOSS CODE에서 설비 상태를 보고하기 때문에 설비 상태 변경을 보고하지 않음
                            if (m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.EMoveState.MOVE_STATE_RUNNING)
                            {
                                m_objCIMSendMessage.SetMoveStateChangeEvent(objMoveStateChangeEvent);
                            }
                        }
                        // 인터락 변경시...
                        else
                        {
                            InterlockAndMoveStateChangeEvent objInterlockAndMoveStateChangeEvent = new InterlockAndMoveStateChangeEvent();
                            objInterlockAndMoveStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                            objInterlockAndMoveStateChangeEvent.BODY.INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objInterlockAndMoveStateChangeEvent.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                            objInterlockAndMoveStateChangeEvent.BODY.REASONCODE = " ";
                            objInterlockAndMoveStateChangeEvent.BODY.DESCRIPTION = " ";
                            m_objCIMSendMessage.SetInterlockAndMoveStateChangeEvnet(objInterlockAndMoveStateChangeEvent);
                        }
                    }
                    else if (bChange[(int)State.RUN_STATE]) // Run State
                    {
                        RunStateChangeEvent objRunStateChangeEvent = new RunStateChangeEvent();
                        objRunStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        objRunStateChangeEvent.BODY.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                        objRunStateChangeEvent.BODY.REASONCODE = " ";
                        objRunStateChangeEvent.BODY.DESCRIPTION = " ";
                        m_objCIMSendMessage.SetRunStateChangeEvent(objRunStateChangeEvent);
                    }
                    else if (bChange[(int)State.FRONT_STATE]) // Front State
                    {
                        FrontStateChangeEvent objFrontStateChangeEvent = new FrontStateChangeEvent();
                        objFrontStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        objFrontStateChangeEvent.BODY.FRONTSTATE = string.Format("{0}", (int)m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                        objFrontStateChangeEvent.BODY.REASONCODE = " ";
                        objFrontStateChangeEvent.BODY.DESCRIPTION = " ";
                        m_objCIMSendMessage.SetFrontStateChangeEvent(objFrontStateChangeEvent);
                    }
                    else if (bChange[(int)State.FRONT_STATE]) // Front State
                    {
                        RearStateChangeEvent objRearStateChangeEvent = new RearStateChangeEvent();
                        objRearStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        objRearStateChangeEvent.BODY.REARSTATE = string.Format("{0}", (int)m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                        objRearStateChangeEvent.BODY.REASONCODE = " ";
                        objRearStateChangeEvent.BODY.DESCRIPTION = " ";
                        m_objCIMSendMessage.SetRearStateChangeEvent(objRearStateChangeEvent);
                    }
                }
                else if (iCount > 0) //  두개 이상의 상태가 변경되었을 경우.
                {
                    EQStateChangeEvent objEQStateChangeEvent = new EQStateChangeEvent();
                    objEQStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    objEQStateChangeEvent.BODY.AVAILABILITYSTATE = string.Format("{0}", (int)m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    objEQStateChangeEvent.BODY.INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    objEQStateChangeEvent.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    objEQStateChangeEvent.BODY.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    objEQStateChangeEvent.BODY.FRONTSTATE = string.Format("{0}", (int)m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    objEQStateChangeEvent.BODY.REARSTATE = string.Format("{0}", (int)m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    objEQStateChangeEvent.BODY.PP_SPLSTATE = string.Format("{0}", (int)m_objDocument.m_ePP_SPLState);
                    objEQStateChangeEvent.BODY.REASONCODE = " ";
                    objEQStateChangeEvent.BODY.DESCRIPTION = " ";
                    m_objCIMSendMessage.SetEqStateChangeEvent(objEQStateChangeEvent);
                }

            } while (false);

            // 공유 메모리에 현재 장비 상태 업데이트.
            CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            for (int iLoopCount = 0; iLoopCount < (int)CCIMDefine.EPresentState.PRESENT_STATE_FINAL; iLoopCount++)
            {
                objMachineStatus.m_eAvailabilityState = m_objDocument.m_eAvailabilityState;
                objMachineStatus.m_eInterlockState = m_objDocument.m_eInterlockState;
                objMachineStatus.m_eMoveState = m_objDocument.m_eMoveState;
                objMachineStatus.m_eRunState = m_objDocument.m_eRunState;
                objMachineStatus.m_eFrontState = m_objDocument.m_eFrontState;
                objMachineStatus.m_eRearState = m_objDocument.m_eRearState;
                objMachineStatus.m_ePP_SPLState = m_objDocument.m_ePP_SPLState;
                objMachineStatus.m_eOpCallState = m_objDocument.m_eOpCallState;
                objMachineStatus.m_eConveyorMoveState = m_objDocument.m_eConveyorMoveState;
                objMachineStatus.m_eConveyorStopReason = m_objDocument.m_eConveyorStopReason;
                objMachineStatus.m_eUnitMoveState = m_objDocument.m_eUnitMoveState;
                objMachineStatus.m_eUnitInterlockState = m_objDocument.m_eUnitInterlockState;
            }
            objMachineStatus.m_eHeartBeat = m_objDocument.m_eHeartBeat[(int)CCIMDefine.EPresentState.CURRENT_STATE];
            objMachineStatus.m_eRunMode = m_objDocument.GetRunStatus();
            //objMachineStatus.m_eControlState = m_objDocument.m_eControlState;
            objMMFManagerMachineStatus[0].SetMachineStatus(objMachineStatus);
            if (bChange[(int)State.AVAILABILITY_STATE] == true)
            {
                Thread.Sleep(100);
            }
            mSyncEquipmentStateReport.Set();
        }

        public void UnitStatusChangeReport(CCIMDefine.EUnit eUnit)
        {
            // 유닛 상태 테크
            UnitStateChangeEvent objUnitStateChangeEvent = new UnitStateChangeEvent();
            objUnitStateChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            objUnitStateChangeEvent.BODY.UNITID = GetUnitId(eUnit);
            objUnitStateChangeEvent.BODY.AVAILABILITYSTATE = "2";
            objUnitStateChangeEvent.BODY.INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eUnitInterlockState[(int)eUnit][(int)CCIMDefine.EPresentState.CURRENT_STATE]);
            objUnitStateChangeEvent.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eUnitMoveState[(int)eUnit][(int)CCIMDefine.EPresentState.CURRENT_STATE]);
            objUnitStateChangeEvent.BODY.RUNSTATE = string.Format("{0}", (int)CCIMDefine.ERunState.RUN_STATE_RUN);
            objUnitStateChangeEvent.BODY.FRONTSTATE = string.Format("{0}", (int)CCIMDefine.EFrontEquipmentState.FRONT_STATE_UP);
            objUnitStateChangeEvent.BODY.REARSTATE = string.Format("{0}", (int)CCIMDefine.ERearEquipmentState.REAR_STATE_UP);
            objUnitStateChangeEvent.BODY.PP_SPLSTATE = string.Format("{0}", (int)CCIMDefine.EPP_SPLState.PP_SPL_STATE_NORMAL);
            objUnitStateChangeEvent.BODY.REASONCODE = " ";
            objUnitStateChangeEvent.BODY.DESCRIPTION = " ";
            m_objCIMSendMessage.SetUnitStateChangeEvent(objUnitStateChangeEvent);
            //m_objDocument.m_eUnitMoveState[(int)eUnit][(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = m_objDocument.m_eUnitMoveState[(int)eUnit][(int)CCIMDefine.EPresentState.CURRENT_STATE];
            //m_objDocument.m_eUnitInterlockState[(int)eUnit][(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = m_objDocument.m_eUnitInterlockState[(int)eUnit][(int)CCIMDefine.EPresentState.CURRENT_STATE];
        }

        public string GetUnitId(CCIMDefine.EUnit unitIndex)
        {
            return $"{m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID}_{unitIndex}";
        }

        public bool GetIsEqLoadingStopFromUnitId(string unitId)
        {
            // EQP LOADING STOP인 경우 허용되는 UNITID는 [EQPID or Null or  '-']이다
            if (unitId != m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID
                && string.IsNullOrWhiteSpace(unitId) == false
                && unitId != "-"
                )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// OP CALL 상태가 Received 인지 체크
        /// </summary>
        /// <param name="eMessageType"></param>
        /// <returns></returns>
        private bool IsState(CCIMDefine.EMessageType eMessageType)
        {
            bool bResult = false;
            do
            {
                if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_OP_CALL)
                {
                    if (CCIMDefine.EProcessState.STS_RECEIVED != m_objProcessCIMOPCallRequest.GetStatus())
                    {
                        break;
                    }
                    if (CCIMDefine.EOpcallState.OP_CALL_STATE_ON != m_objDocument.m_eOpCallState)
                    {
                        break;
                    }
                }
                else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK_BUFFERING)
                {
                    if (CCIMDefine.EProcessState.STS_RECEIVED != m_objProcessCIMInterlockRequest.GetStatus())
                    {
                        break;
                    }
                }
                else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK)
                {
                    if (0 == mInterlockMessageBuffer.Count)
                    {
                        break;
                    }
                }
                else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_TERMINAL)
                {
                    if (CCIMDefine.EProcessState.STS_RECEIVED != m_objProcessCIMTerminalDisplayRequest.GetStatus())
                    {
                        break;
                    }
                }
                else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK_BUFFERING)
                {
                    if (CCIMDefine.EProcessState.STS_RECEIVED != m_objProcessCIMUnitInterlockRequest.GetStatus())
                    {
                        break;
                    }
                }
                else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK)
                {
                    if (0 == mUnitInterlockMessageBuffer.Count)
                    {
                        break;
                    }
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 메시지 출력.
        /// </summary>
        /// <param name="eMessageType"></param>
        private void DoProcessMessage(CCIMDefine.EMessageType eMessageType)
        {
            if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_OP_CALL)
            {
                OpCallRequest objOPCallRequest = new OpCallRequest();
                objOPCallRequest.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                // OP 발생 데이터 가져오기.
                objOPCallRequest = m_objProcessCIMOPCallRequest.GetReceivedData();
                // 다이얼로그 표시.
                m_objDocument.GetMainFrame().ShowMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_OP_CALL, DateTime.Now, "", objOPCallRequest.BODY.OPCALLID, objOPCallRequest.BODY.MESSAGE);
            }
            else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK_BUFFERING)
            {
                var interlockRequest = m_objProcessCIMInterlockRequest.GetReceivedData();
                interlockRequest.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                InterlockMessage addMessage = new InterlockMessage()
                {
                    DateTime = DateTime.Now,
                    InterlockID = interlockRequest.BODY.INTERLOCKID,
                    Message = interlockRequest.BODY.MESSAGE
                };
                mInterlockMessageBuffer.Enqueue(addMessage);
            }
            else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK)
            {
                while (mInterlockMessageBuffer.Count > 0)
                {
                    var getMessage = mInterlockMessageBuffer.Dequeue();
                    if (getMessage.Rcmd != "12")
                    {
                        if (CCIMDefine.EInterlockState.INTERLOCK_STATE_ON != m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                        {
                            mInterlockMessageBuffer.Enqueue(getMessage);
                            break;
                        }
                    }
                    // 다이얼로그 표시.
                    m_objDocument.GetMainFrame().ShowMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK, getMessage.DateTime, "", getMessage.InterlockID, getMessage.Message);
                }
            }
            else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_TERMINAL)
            {
                TerminalDisplayRequest objTerminalDisplayRequest = new TerminalDisplayRequest();
                objTerminalDisplayRequest.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                // OP 발생 데이터 가져오기.
                objTerminalDisplayRequest = m_objProcessCIMTerminalDisplayRequest.GetReceivedData();
                // 다이얼로그 표시.
                for (int iLoopCount = 0; iLoopCount < objTerminalDisplayRequest.BODY.TEXTS.Length; iLoopCount++)
                {
                    m_objDocument.GetMainFrame().ShowMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_TERMINAL, DateTime.Now, "", objTerminalDisplayRequest.BODY.TID, objTerminalDisplayRequest.BODY.TEXTS[iLoopCount].TEXT);
                }
            }
            else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK_BUFFERING)
            {
                var unitInterlockRequest = m_objProcessCIMUnitInterlockRequest.GetReceivedData();
                unitInterlockRequest.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;

                // EQ Loading Stop, Unit Interlock 구분                
                if (GetIsEqLoadingStopFromUnitId(unitInterlockRequest.BODY.UNITID) == true)
                {
                    InterlockMessage addMessage = new InterlockMessage()
                    {
                        DateTime = DateTime.Now,
                        InterlockID = unitInterlockRequest.BODY.INTERLOCKID,
                        Message = unitInterlockRequest.BODY.MESSAGE,
                        Rcmd = unitInterlockRequest.BODY.RCMD
                    };
                    mInterlockMessageBuffer.Enqueue(addMessage);
                }
                else
                {
                    UnitInterlockMessage addMessage = new UnitInterlockMessage()
                    {
                        DateTime = DateTime.Now,
                        Rcmd = unitInterlockRequest.BODY.RCMD,
                        Interlock = unitInterlockRequest.BODY.INTERLOCK,
                        UnitID = unitInterlockRequest.BODY.UNITID,
                        InterlockID = unitInterlockRequest.BODY.INTERLOCKID,
                        Message = unitInterlockRequest.BODY.MESSAGE
                    };
                    mUnitInterlockMessageBuffer.Enqueue(addMessage);
                }
            }
            else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK)
            {
                while (mUnitInterlockMessageBuffer.Count > 0)
                {
                    var getMessage = mUnitInterlockMessageBuffer.Dequeue();
                    // 다이얼로그 표시.
                    m_objDocument.GetMainFrame().ShowMessage(CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK, getMessage.DateTime, getMessage.UnitID, getMessage.InterlockID, getMessage.Message);
                }
            }
        }

        private void Communicator_OnReceiveRequest(object sender, ReqMsgInfo e)
        {
            var xmlMsgNode = e.ReceviedXml.SelectNodes("/MESSAGE/NAME");
            foreach (XmlNode xn in xmlMsgNode)
            {
                string name = xn.InnerText;
                switch (name)
                {
                    case "CARRIERINFODOWNLOAD":
                        m_objProcessCIMCarrierInformationDownload.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "CELLINFODOWNLOAD":
                        m_objProcessCIMCellInfomationDownload.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "CELLJOBPROCESSREQUEST":
                        m_objProcessCIMCellJobProcessRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "CONTROLSTATESETREQUEST":
                        m_objProcessCIMControlStateRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "CONVEYORMOVESTATEREPLY":
                        m_objProcessCIMConveyorMoveStateRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "CURRENTALARMLISTREQUEST":
                        m_objProcessCIMCurrentAlarmListRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "CURRENTPPIDREQUEST":
                        m_objProcessCIMCurrentPPIDRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "DATEANDTIMESETREQUEST":
                        m_objProcessCIMDateAndTimeSetRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "ECLISTREQUEST":
                        m_objProcessCIMECListRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "ECSETREQUEST":
                        m_objProcessCIMECSetRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "EQSTATEREQUEST":
                        m_objProcessCIMEQStateRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "FUNCTIONCHANGEREQUEST":
                        m_objProcessCIMFunctionChangeRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "FUNCTIONSTATEREQUEST":
                        m_objProcessCIMFunctionStateRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "INTERLOCKREQUEST":
                        m_objProcessCIMInterlockRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "OPCALLREQUEST":
                        m_objProcessCIMOPCallRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "PPIDLISTREQUEST":
                        m_objProcessCIMPPIDListRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "SECUREDPPPARAMSETREQUEST":
                        m_objProcessCIMPPParamRemoteRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "PPPARAMREQUEST":
                        m_objProcessCIMPPParamRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "SPECIFICATIONVERSIONSTATEREQUEST":
                        m_objProcessCIMPSpecificationversionStateRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "TERMINALDISPLAYREQUEST":
                        m_objProcessCIMTerminalDisplayRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "TRACESTARTREQUEST":
                        m_objProcessCIMTraceRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "UNITINTERLOCKREQUEST":
                        m_objProcessCIMUnitInterlockRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "UNITSTATEREQUEST":
                        m_objProcessCIMUnitStateRequest.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "CELLLOTINFOREPLY":
                        m_objProcessCIMCellLotInfomationReply.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "CELLLOTINFODOWNLOAD":
                        m_objProcessCIMCellLotInfomationDownload.Communicator_OnReceiveRequest(sender, e);
                        break;
                    case "EQAPPROVESEND":
                        m_objProcessCIMEquipmentApproveSend.Communicator_OnReceiveRequest(sender, e);
                        break;
                }
            }
        }
    }
}

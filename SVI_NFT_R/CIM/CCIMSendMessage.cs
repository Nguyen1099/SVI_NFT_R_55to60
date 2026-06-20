using System.Threading;

namespace SVI_NFT_R
{
    public class CCIMSendMessage
    {
        private CCIMImplementAbstract m_objCIMImplement;
        private System.Collections.Concurrent.ConcurrentQueue<SendMessage> mSendMessageQueue = new System.Collections.Concurrent.ConcurrentQueue<SendMessage>();
        private Thread mThreadSendMessage;
        private bool mbThreadExit = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMImplement"></param>
        /// <returns></returns>
        public CCIMSendMessage(CCIMImplementAbstract objCIMImplement)
        {
            m_objCIMImplement = objCIMImplement;

            Initialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bResult = false;
            do
            {
                mThreadSendMessage = new Thread(threadSendMessage);
                mThreadSendMessage.Start();

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            mbThreadExit = true;
            mThreadSendMessage.Join();
        }

        /// <summary>
        /// 알람 레포트
        /// </summary>
        /// <param name="objAlarmReport"></param>
        /// <returns></returns>
        public bool SetAlarmReport(AlarmReport objAlarmReport)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetAlarmReport, objAlarmReport));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 장비 상태 변경
        /// </summary>
        /// <param name="objAvailabilityAndMoveStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetAvailabilityAndMoveStateChangeEvent(AvailabilityAndMoveStateChangeEvent objAvailabilityAndMoveStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetAvailabilityAndMoveStateChangeEvent, objAvailabilityAndMoveStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 셀 이벤트
        /// </summary>
        /// <param name="eCellEvent"></param>
        /// <param name="objCellTrackingEvent"></param>
        /// <returns></returns>
        public bool SetCellEvent(CCIMDefine.ECellEvent eCellEvent, object objCellTrackingEvent)
        {
            bool bResult = false;
            do
            {
                if (eCellEvent == CCIMDefine.ECellEvent.CELL_EVENT_PROCESS_START)
                {
                    mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCellInEvent, objCellTrackingEvent));
                }
                else if (eCellEvent == CCIMDefine.ECellEvent.CELL_EVENT_PROCESS_END)
                {
                    mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCellOutEvent, objCellTrackingEvent));
                }

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 컨베어 정지 상태 보고
        /// </summary>
        /// <param name="objConveyorMoveStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetConveyorMoveStateChangeEvent(ConveyorMoveStateChangeEvent objConveyorMoveStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetConveyorMoveStateChangeEvent, objConveyorMoveStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 현재 PPID 변경 보고
        /// </summary>
        /// <param name="objCurrentPPIDChangeEvent"></param>
        /// <returns></returns>
        public bool SetCurrentPPIDChangeEvent(CurrentPPIDChangeEvent objCurrentPPIDChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCurrentPPIDChangeEvent, objCurrentPPIDChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 현재 PPID 파라미터 변경 보고
        /// </summary>
        /// <param name="objCurrentPPIDParamChangeEvent"></param>
        /// <returns></returns>
        public bool SetCurrentPPIDParamChangeEvent(CurrentPPIDParamChangeEvent objCurrentPPIDParamChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCurrentPPIDParamChangeEvent, objCurrentPPIDParamChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// EC 값 변경 보고
        /// </summary>
        /// <param name="objECChangeEvent"></param>
        /// <returns></returns>
        public bool SetEcChangeEvent(ECChangeEvent objECChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetEcChangeEvent, objECChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 장비 상태 변경 보고
        /// </summary>
        /// <param name="objEQStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetEqStateChangeEvent(EQStateChangeEvent objEQStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetEqStateChangeEvent, objEQStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 앞 유닛 상태 변경 보고
        /// </summary>
        /// <param name="objFrontStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetFrontStateChangeEvent(FrontStateChangeEvent objFrontStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetFrontStateChangeEvent, objFrontStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Function 변경 보고
        /// </summary>
        /// <param name="objFunctionChangeEvent"></param>
        /// <returns></returns>
        public bool SetFunctionChangeEvent(FunctionChangeEvent objFunctionChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetFunctionChangeEvent, objFunctionChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 검사 결과 보고
        /// </summary>
        /// <param name="objInspectionResultEvent"></param>
        /// <returns></returns>
        public bool SetInspectionResultEvent(InspectionResultEvent objInspectionResultEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetInspectionResultEvent, objInspectionResultEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 인터락 및 Move 상태 변경 보고
        /// </summary>
        /// <param name="objInterlockAndMoveStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetInterlockAndMoveStateChangeEvnet(InterlockAndMoveStateChangeEvent objInterlockAndMoveStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetInterlockAndMoveStateChangeEvnet, objInterlockAndMoveStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 인터락 컴펌 보고
        /// </summary>
        /// <param name="objInterlockConfirmEvent"></param>
        /// <returns></returns>
        public bool SetInterlockConfirmEvent(InterlockConfirmEvent objInterlockConfirmEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetInterlockConfirmEvent, objInterlockConfirmEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 인터락 상태 변경 보고
        /// </summary>
        /// <param name="objInterlockStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetInterlockStateChangeEvent(InterlockStateChangeEvent objInterlockStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetInterlockStateChangeEvent, objInterlockStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Loss Code 보고
        /// </summary>
        /// <param name="objLossCodeEvent"></param>
        /// <returns></returns>
        public bool SetLossCodeEvent(LossCodeEvent objLossCodeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetLossCodeEvent, objLossCodeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Operator Information 보고
        /// </summary>
        /// <param name="objOperatorInfoEvent"></param>
        /// <returns></returns>
        public bool SetOperatorInfoEvent(OperatorInfoEvent objOperatorInfoEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetOperatorInfoEvent, objOperatorInfoEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Availabity 상태 변경 보고
        /// </summary>
        /// <param name="objAvailabilityStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetAvailabityStateChangeEvent(AvailabilityStateChangeEvent objAvailabilityStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetAvailabityStateChangeEvent, objAvailabilityStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Move 상태 변경 보고.
        /// </summary>
        /// <param name="objMoveStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetMoveStateChangeEvent(MoveStateChangeEvent objMoveStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetMoveStateChangeEvent, objMoveStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// OP Call 메시지에 대한 컴펌 보고.
        /// </summary>
        /// <param name="objOpCallConfirmEvent"></param>
        /// <returns></returns>
        public bool SetOPCallConfirmEvent(OpCallConfirmEvent objOpCallConfirmEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetOPCallConfirmEvent, objOpCallConfirmEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// PPID 변경 보고한다.
        /// </summary>
        /// <param name="objPPChangeEvent"></param>
        /// <returns></returns>
        public bool SetPPChangeEvent(PPChangeEvent objPPChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetPPChangeEvent, objPPChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 뒷 유닛 상태 변경 보고
        /// </summary>
        /// <param name="objRearStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetRearStateChangeEvent(RearStateChangeEvent objRearStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetRearStateChangeEvent, objRearStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Run 상태 변경 보고
        /// </summary>
        /// <param name="objRunStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetRunStateChangeEvent(RunStateChangeEvent objRunStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetRunStateChangeEvent, objRunStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 샘플런, 노말런 상태 변경 보고
        /// </summary>
        /// <param name="objPP_SPLStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetSampleLotStateChangeEvent(PP_SPLStateChangeEvent objPP_SPLStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetSampleLotStateChangeEvent, objPP_SPLStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Tact 보고
        /// </summary>
        /// <param name="objTactEvent"></param>
        /// <returns></returns>
        public bool SetTactEvent(TactEvent objTactEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetTactEvent, objTactEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 트레이스 데이터 보고
        /// </summary>
        /// <param name="objTraceDataReport"></param>
        /// <returns></returns>
        public bool SetTraceDataReport(TraceDataReport objTraceDataReport)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetTraceDataReport, objTraceDataReport));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 유닛 인터락 메시지에 대한 컴펌 보고.
        /// </summary>
        /// <param name="objUnitInterlockConfirmEvent"></param>
        /// <returns></returns>
        public bool SetUnitInterlockConfirmEvent(UnitInterlockConfirmEvent objUnitInterlockConfirmEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetUnitInterlockConfirmEvent, objUnitInterlockConfirmEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 유닛 상태 변경 보고
        /// </summary>
        /// <param name="objUnitStateChangeEvent"></param>
        /// <returns></returns>
        public bool SetUnitStateChangeEvent(UnitStateChangeEvent objUnitStateChangeEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetUnitStateChangeEvent, objUnitStateChangeEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// PP Param 에 대한 상위 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objPPParamReply"></param>
        /// <returns></returns>
        public bool SetPPParamReply(PPParamReply objPPParamReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetPPParamReply, objPPParamReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Cell Job Process 메시지에 대한 응답 보고
        /// </summary>
        /// <param name="objCellJobProcessReply"></param>
        /// <returns></returns>
        public bool SetCellJobProcessReply(CellJobProcessReply objCellJobProcessReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCellJobProcessReply, objCellJobProcessReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Control State 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objControlStateSetReply"></param>
        /// <returns></returns>
        public bool SetControlStateReply(ControlStateSetReply objControlStateSetReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetControlStateReply, objControlStateSetReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 컨베어 이동 상태 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objConveyorMoveStateReply"></param>
        /// <returns></returns>
        public bool SetConveyorMoveStateReply(ConveyorMoveStateReply objConveyorMoveStateReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetConveyorMoveStateReply, objConveyorMoveStateReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 현재 알람 발생 리스트 요구에 대한 응답 보고.
        /// </summary>
        /// <param name="objCurrentAlarmListReply"></param>
        /// <returns></returns>
        public bool SetCurrentAlarmListReply(CurrentAlarmListReply objCurrentAlarmListReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCurrentAlarmListReply, objCurrentAlarmListReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 현재 PPID 요구에대한 응답 보고.
        /// </summary>
        /// <param name="objCurrentPPIDReply"></param>
        /// <returns></returns>
        public bool SetCurrentPPIDReply(CurrentPPIDReply objCurrentPPIDReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCurrentPPIDReply, objCurrentPPIDReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 날짜, 시간 변경 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objDateAndTimeSetReply"></param>
        /// <returns></returns>
        public bool SetDateAndTimeSetReply(DateAndTimeSetReply objDateAndTimeSetReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetDateAndTimeSetReply, objDateAndTimeSetReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// EC 리스트 요구에 대한 응답 보고.
        /// </summary>
        /// <param name="objECListReply"></param>
        /// <returns></returns>
        public bool SetECListReply(ECListReply objECListReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetECListReply, objECListReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// EC 파라미터 변경 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objECSetReply"></param>
        /// <returns></returns>
        public bool SetECSetReply(ECSetReply objECSetReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetECSetReply, objECSetReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 장비 상태 요구에 대한 응답 보고.
        /// </summary>
        /// <param name="objEQStateReply"></param>
        /// <returns></returns>
        public bool SetEQStateReply(EQStateReply objEQStateReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetEQStateReply, objEQStateReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Function 변경 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objFunctionChangeReply"></param>
        /// <returns></returns>
        public bool SetFunctionChangeReply(FunctionChangeReply objFunctionChangeReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetFunctionChangeReply, objFunctionChangeReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Function 상태 요구 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objFunctionStateReply"></param>
        /// <returns></returns>
        public bool SetFunctionStateReply(FunctionStateReply objFunctionStateReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetFunctionStateReply, objFunctionStateReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 인터락 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objInterlockReply"></param>
        /// <returns></returns>
        public bool SetInterlockReply(InterlockReply objInterlockReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetInterlockReply, objInterlockReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 오피콜 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objOpCallReply"></param>
        /// <returns></returns>
        public bool SetOPCallReply(OpCallReply objOpCallReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetOPCallReply, objOpCallReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// PPID 리스트 요구 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objPPListReply"></param>
        /// <returns></returns>
        public bool SetPPIDListReply(PPListReply objPPListReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetPPIDListReply, objPPListReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 터미널 메시지에 대한 응답 보고.
        /// </summary>
        /// <param name="objTerminalDisplayReply"></param>
        /// <returns></returns>
        public bool SetTerminalDisplayReply(TerminalDisplayReply objTerminalDisplayReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetTerminalDisplayReply, objTerminalDisplayReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 리모트 PPID 응답
        /// </summary>
        /// <param name="objPPParamRmoteRequestReply"></param>
        /// <returns></returns>
        public bool SetPPParamSetReply(PPChangeRemoteEventReply objPPParamRmoteRequestReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetPPParamSetReply, objPPParamRmoteRequestReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 유닛 인터락 메시지에 대한 응답 보고
        /// </summary>
        /// <param name="objUnitInterlockReply"></param>
        /// <returns></returns>
        public bool SetUnitInterlockReply(UnitInterlockReply objUnitInterlockReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetUnitInterlockReply, objUnitInterlockReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 유닛 상태 요구 메시지에 대한 응답보고.
        /// </summary>
        /// <param name="objUnitStateReply"></param>
        /// <returns></returns>
        public bool SetUnitStateReply(UnitStateReply objUnitStateReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetUnitStateReply, objUnitStateReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 트레이스 데이터 시작 메시지에 대한 응답보고.
        /// </summary>
        /// <param name="objTraceStartReply"></param>
        /// <returns></returns>
        public bool SetTraceStartReply(TraceStartReply objTraceStartReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetTraceStartReply, objTraceStartReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 트레이스 데이터 정지 메시지에 대한 응답보고.
        /// </summary>
        /// <param name="objTraceStopReply"></param>
        /// <returns></returns>
        public bool SetTraceStopReply(TraceStopReply objTraceStopReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCellJobProcessReply, objTraceStopReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        public bool SetCellInfomationEvent(CellInfomationEvent objCellInformationEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCellInfomationEvent, objCellInformationEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        public bool SetCarrierValidationCheckRequest(CarrierValidationCheckRequest objCarrierValidationCheck)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCarrierValidationCheckRequest, objCarrierValidationCheck));

                bResult = true;
            } while (false);
            return bResult;
        }

        public bool SetEquipmentSpecificStepEvent(EQSpecificStepEvent objEQSpecificStepEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetEquipmentSpecificStepEvent, objEQSpecificStepEvent));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// Performance Loss Event
        /// </summary>
        /// <param name="objPerformanceLossEvent"></param>
        /// <returns></returns>
        public bool SetPerformanceLossEvent(PerformanceLossEvent objPerformanceLossEvent)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetPerformanceLossEvent, objPerformanceLossEvent));

                bResult = true;
            } while (false);
            return bResult;
        }
        /// <summary>
        /// 버전 정보 요청
        /// </summary>
        /// <param name="objSpecificationversionStateRequest"></param>
        /// <returns></returns>
        public bool SetSpecificationversionStateReply(SpecificationversionStateReply objSpecificationversionStateReply)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetSpecificationversionStateReply, objSpecificationversionStateReply));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 셀 LOT 정보 요청
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SetCellLotInformationRequest(CellLotInformationRequest data)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetCellLotInformationRequest, data));

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SetEquipmentApproveReply(EquipmentApproveReply data)
        {
            bool bResult = false;
            do
            {
                mSendMessageQueue.Enqueue(new SendMessage(m_objCIMImplement.SetEquipmentApproveReply, data));

                bResult = true;
            } while (false);
            return bResult;
        }

        private void threadSendMessage()
        {
            int sleepCount = 0;
            while (
                false == mbThreadExit
                || 0 < mSendMessageQueue.Count
                )
            {
                SendMessage sendMessage;
                if (true == mSendMessageQueue.TryDequeue(out sendMessage))
                {
                    sendMessage.SendCallback.Invoke(sendMessage.SendParameter);
                    sendMessage.SetSendFinish();
                }

                sleepCount++;
                if (5 < sleepCount)
                {
                    Thread.Sleep(1);
                    sleepCount = 0;
                }
            }
        }
    }
}

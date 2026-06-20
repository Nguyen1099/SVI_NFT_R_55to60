using System;
using System.Xml.Serialization;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CCIMImplementSVI_M : CCIMImplementAbstract
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMInitialize"></param>
        /// <returns></returns>
        public CCIMImplementSVI_M(CCIMDefine.structureCIMInitialize objCIMInitialize)
        {
            m_objDocument = (CDocument)objCIMInitialize.objDocument as CDocument;
            m_objCommunicator = (urLinkDllCommunicator)objCIMInitialize.objCommunicator as urLinkDllCommunicator;
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
                if (false == base.Initialize()) break;

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void DeInitialize()
        {
            base.DeInitialize();
        }

        /// <summary>
        /// 알람 레포트 보고.
        /// </summary>
        /// <param name="objAlarmReport"></param>
        public override void SetAlarmReport(object objAlarmReport)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((AlarmReport)objAlarmReport).GetType());
                    serializer.Serialize(stringwriter, ((AlarmReport)objAlarmReport), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "ALARM(S5F1)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Availability 및 Move 상태 보고.
        /// </summary>
        /// <param name="objAvailabilityAndMoveStateChangeEvent"></param>
        public override void SetAvailabilityAndMoveStateChangeEvent(object objAvailabilityAndMoveStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((AvailabilityAndMoveStateChangeEvent)objAvailabilityAndMoveStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((AvailabilityAndMoveStateChangeEvent)objAvailabilityAndMoveStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Availability 상태 보고.
        /// </summary>
        /// <param name="objAvailabilityStateChangeEvent"></param>
        public override void SetAvailabityStateChangeEvent(object objAvailabilityStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((AvailabilityStateChangeEvent)objAvailabilityStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((AvailabilityStateChangeEvent)objAvailabilityStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "AvailabityStateChange\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Track In 보고
        /// </summary>
        /// <param name="objCellTrackingEvent"></param>
        public override void SetCellInEvent(object objCellTrackingEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((CellTrackInEvent)objCellTrackingEvent).GetType());
                    serializer.Serialize(stringwriter, ((CellTrackInEvent)objCellTrackingEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "TRACKIN(401)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Track Out 보고
        /// </summary>
        /// <param name="objCellTrackingEvent"></param>
        public override void SetCellOutEvent(object objCellTrackingEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((CellTrackOutEvent)objCellTrackingEvent).GetType());
                    serializer.Serialize(stringwriter, ((CellTrackOutEvent)objCellTrackingEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "TRACKOUT(406)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 컨베이 이동 상태의 변경을 보고
        /// </summary>
        /// <param name="objConveyorMoveStateChangeEvent"></param>
        public override void SetConveyorMoveStateChangeEvent(object objConveyorMoveStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((ConveyorMoveStateChangeEvent)objConveyorMoveStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((ConveyorMoveStateChangeEvent)objConveyorMoveStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 현재 사용 중인 PPID의 변경 보고.
        /// </summary>
        /// <param name="objCurrentPPIDChangeEvent"></param>
        public override void SetCurrentPPIDChangeEvent(object objCurrentPPIDChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((CurrentPPIDChangeEvent)objCurrentPPIDChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((CurrentPPIDChangeEvent)objCurrentPPIDChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "RMS CHANGE(S6F11 107)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }
        /// <summary>
        /// 현재 사용 중인 PPID의 파라미터 변경 보고.
        /// </summary>
        /// <param name="objCurrentPPIDParamChangeEvent"></param>
        public override void SetCurrentPPIDParamChangeEvent(object objCurrentPPIDParamChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((CurrentPPIDParamChangeEvent)objCurrentPPIDParamChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((CurrentPPIDParamChangeEvent)objCurrentPPIDParamChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "RMS PARAM CHANGE(S7F107)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// EC 데이터 변경 보고.
        /// </summary>
        /// <param name="objECChangeEvent"></param>
        public override void SetEcChangeEvent(object objECChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((ECChangeEvent)objECChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((ECChangeEvent)objECChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "ECM CHANGE(S2F30)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 장비 상태의 변경을 보고.
        /// </summary>
        /// <param name="objEQStateChangeEvent"></param>
        public override void SetEqStateChangeEvent(object objEQStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((EQStateChangeEvent)objEQStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((EQStateChangeEvent)objEQStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "EQ STATE(101)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 앞 유닛의 상태 변경을 보고한다.
        /// </summary>
        /// <param name="objFrontStateChangeEvent"></param>
        public override void SetFrontStateChangeEvent(object objFrontStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((FrontStateChangeEvent)objFrontStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((FrontStateChangeEvent)objFrontStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Function의 변경을 보고
        /// </summary>
        /// <param name="objFunctionChangeEvent"></param>
        public override void SetFunctionChangeEvent(object objFunctionChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((FunctionChangeEvent)objFunctionChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((FunctionChangeEvent)objFunctionChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "FUNCTION CHANGE(S2F41)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 검사 결과 보고
        /// </summary>
        /// <param name="objInspectionResultEvent"></param>
        public override void SetInspectionResultEvent(object objInspectionResultEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((InspectionResultEvent)objInspectionResultEvent).GetType());
                    serializer.Serialize(stringwriter, ((InspectionResultEvent)objInspectionResultEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "INSPECTION RESULT(S6F11 609)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 인터락 및 무브 상태 변경 보고
        /// </summary>
        /// <param name="objInterlockAndMoveStateChangeEvent"></param>
        public override void SetInterlockAndMoveStateChangeEvnet(object objInterlockAndMoveStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((InterlockAndMoveStateChangeEvent)objInterlockAndMoveStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((InterlockAndMoveStateChangeEvent)objInterlockAndMoveStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "INTEROCK(S2F41)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 인터락 확인한 것을 보고
        /// </summary>
        /// <param name="objInterlockConfirmEvent"></param>
        public override void SetInterlockConfirmEvent(object objInterlockConfirmEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((InterlockConfirmEvent)objInterlockConfirmEvent).GetType());
                    serializer.Serialize(stringwriter, ((InterlockConfirmEvent)objInterlockConfirmEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "INTEROCK CONFIRM(S6F11 502)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 인터락 상태 변경 보고
        /// </summary>
        /// <param name="objInterlockStateChangeEvent"></param>
        public override void SetInterlockStateChangeEvent(object objInterlockStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((InterlockStateChangeEvent)objInterlockStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((InterlockStateChangeEvent)objInterlockStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "INTEROCK CHANGE(S1F5)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Loss Code 이벤트 보고
        /// </summary>
        /// <param name="objLossCodeEvent"></param>
        public override void SetLossCodeEvent(object objLossCodeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((LossCodeEvent)objLossCodeEvent).GetType());
                    serializer.Serialize(stringwriter, ((LossCodeEvent)objLossCodeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "TPCODE(S6F11 606)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Operator Information 이벤트 보고
        /// </summary>
        /// <param name="objOperatorInfoEvent"></param>
        public override void SetOperatorInfoEvent(object objOperatorInfoEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(OperatorInfoEvent));
                    serializer.Serialize(stringwriter, ((OperatorInfoEvent)objOperatorInfoEvent), ns);
                    m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 무브 상태 변경 보고
        /// </summary>
        /// <param name="objMoveStateChangeEvent"></param>
        public override void SetMoveStateChangeEvent(object objMoveStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((MoveStateChangeEvent)objMoveStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((MoveStateChangeEvent)objMoveStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "MOVE STATE CHANGE\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Op Call 메시지 확인시 보고
        /// </summary>
        /// <param name="objOpCallConfirmEvent"></param>
        public override void SetOPCallConfirmEvent(object objOpCallConfirmEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((OpCallConfirmEvent)objOpCallConfirmEvent).GetType());
                    serializer.Serialize(stringwriter, ((OpCallConfirmEvent)objOpCallConfirmEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "OPCALL CONFIRM(S6F11 501)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// PPID 변경시 보고
        /// </summary>
        /// <param name="objPPChangeEvent"></param>
        public override void SetPPChangeEvent(object objPPChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((PPChangeEvent)objPPChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((PPChangeEvent)objPPChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "RMS CHANGE(S7F101)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 뒷 유닛 상태 변경시 보고
        /// </summary>
        /// <param name="objRearStateChangeEvent"></param>
        public override void SetRearStateChangeEvent(object objRearStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((RearStateChangeEvent)objRearStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((RearStateChangeEvent)objRearStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 런 상태 변경시 보고
        /// </summary>
        /// <param name="objRunStateChangeEvent"></param>
        public override void SetRunStateChangeEvent(object objRunStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((RunStateChangeEvent)objRunStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((RunStateChangeEvent)objRunStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "IDLE\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 샘플 Lot 상태 변경시 보고
        /// </summary>
        /// <param name="objPP_SPLStateChangeEvent"></param>
        public override void SetSampleLotStateChangeEvent(object objPP_SPLStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((PP_SPLStateChangeEvent)objPP_SPLStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((PP_SPLStateChangeEvent)objPP_SPLStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Tact 이벤트 발생시 보고
        /// </summary>
        /// <param name="objTactEvent"></param>
        public override void SetTactEvent(object objTactEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((TactEvent)objTactEvent).GetType());
                    serializer.Serialize(stringwriter, ((TactEvent)objTactEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Trace Data 레포트 보고
        /// </summary>
        /// <param name="objTraceDataReport"></param>
        public override void SetTraceDataReport(object objTraceDataReport)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((TraceDataReport)objTraceDataReport).GetType());
                    serializer.Serialize(stringwriter, ((TraceDataReport)objTraceDataReport), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_TRACE, stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 유닛 인터락 메시지 확인시 보고
        /// </summary>
        /// <param name="objUnitInterlockConfirmEvent"></param>
        public override void SetUnitInterlockConfirmEvent(object objUnitInterlockConfirmEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((UnitInterlockConfirmEvent)objUnitInterlockConfirmEvent).GetType());
                    serializer.Serialize(stringwriter, ((UnitInterlockConfirmEvent)objUnitInterlockConfirmEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "UNIT INTERLOCK CONFIRM(S6F11 514)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 유닛 상태 변경시 보고
        /// </summary>
        /// <param name="objUnitStateChangeEvent"></param>
        public override void SetUnitStateChangeEvent(object objUnitStateChangeEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(((UnitStateChangeEvent)objUnitStateChangeEvent).GetType());
                    serializer.Serialize(stringwriter, ((UnitStateChangeEvent)objUnitStateChangeEvent), ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + "UNIT STATE CHANGE(S6F11 102)\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// PPID 파라미터 리퀘스트에 대한 응답 보고
        /// </summary>
        /// <param name="objPPParamReply"></param>
        public override void SetPPParamReply(object objPPParamReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(PPParamReply));
                    serializer.Serialize(stringwriter, objPPParamReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// Cell Job Process Request 에 대한 응답 보고
        /// </summary>
        /// <param name="objCellJobProcessReply"></param>
        public override void SetCellJobProcessReply(object objCellJobProcessReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(CellJobProcessReply));
                    serializer.Serialize(stringwriter, objCellJobProcessReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);

                }

            } while (false);
        }

        /// <summary>
        /// 컨트롤 상태에 대한 응답 보고
        /// </summary>
        /// <param name="objControlStateSetReply"></param>
        public override void SetControlStateReply(object objControlStateSetReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(ControlStateSetReply));
                    serializer.Serialize(stringwriter, objControlStateSetReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 컨베이어 이동 상태 리퀘스트에 대한 응답 보고
        /// </summary>
        /// <param name="objConveyorMoveStateReply"></param>
        public override void SetConveyorMoveStateReply(object objConveyorMoveStateReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(ConveyorMoveStateReply));
                    serializer.Serialize(stringwriter, objConveyorMoveStateReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 현재 알람 리스트의 내용 보고
        /// </summary>
        /// <param name="objCurrentAlarmListReply"></param>
        public override void SetCurrentAlarmListReply(object objCurrentAlarmListReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(CurrentAlarmListReply));
                    serializer.Serialize(stringwriter, objCurrentAlarmListReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 현재 사용중인 PPID 요청 대한 응답 보고.
        /// </summary>
        /// <param name="objCurrentPPIDReply"></param>
        public override void SetCurrentPPIDReply(object objCurrentPPIDReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(CurrentPPIDReply));
                    serializer.Serialize(stringwriter, objCurrentPPIDReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 날짜와 시간 변경 요청에 대한 응답 보고.
        /// </summary>
        /// <param name="objDateAndTimeSetReply"></param>
        public override void SetDateAndTimeSetReply(object objDateAndTimeSetReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(DateAndTimeSetReply));
                    serializer.Serialize(stringwriter, objDateAndTimeSetReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 EC LIST 요청에 대한 응답 보고
        /// </summary>
        /// <param name="objECListReply"></param>
        public override void SetECListReply(object objECListReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(ECListReply));
                    serializer.Serialize(stringwriter, objECListReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 EC LIST의 내용을 변경 요청에 대한 응답 보고.
        /// </summary>
        /// <param name="objECSetReply"></param>
        public override void SetECSetReply(object objECSetReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(ECSetReply));
                    serializer.Serialize(stringwriter, objECSetReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 설비 상태 요청에 대한 응답 보고
        /// </summary>
        /// <param name="objEQStateReply"></param>
        public override void SetEQStateReply(object objEQStateReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(EQStateReply));
                    serializer.Serialize(stringwriter, objEQStateReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 Function 변경 요청에 대한 응답 보고
        /// </summary>
        /// <param name="objFunctionChangeReply"></param>
        public override void SetFunctionChangeReply(object objFunctionChangeReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(FunctionChangeReply));
                    serializer.Serialize(stringwriter, objFunctionChangeReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 Function 상태 요청에 대한 응답 보고
        /// </summary>
        /// <param name="objFunctionStateReply"></param>
        public override void SetFunctionStateReply(object objFunctionStateReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(FunctionStateReply));
                    serializer.Serialize(stringwriter, objFunctionStateReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 인터락 메시지에 대한 응답 보고
        /// </summary>
        /// <param name="objInterlockReply"></param>
        public override void SetInterlockReply(object objInterlockReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(InterlockReply));
                    serializer.Serialize(stringwriter, objInterlockReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 오피콜 메시지에 대한 응답 보고
        /// </summary>
        /// <param name="objOpCallReply"></param>
        public override void SetOPCallReply(object objOpCallReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(OpCallReply));
                    serializer.Serialize(stringwriter, objOpCallReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 PPID 리스트 요청에 대한 응답 보고.
        /// </summary>
        /// <param name="objPPListReply"></param>
        public override void SetPPIDListReply(object objPPListReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(PPListReply));
                    serializer.Serialize(stringwriter, (PPListReply)objPPListReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 터미널 메시지 올때 응답 보고.
        /// </summary>
        /// <param name="objTerminalDisplayReply"></param>
        public override void SetTerminalDisplayReply(object objTerminalDisplayReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(TerminalDisplayReply));
                    serializer.Serialize(stringwriter, objTerminalDisplayReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        //kjpark 20181130 CIM JobDownload Create
        public override void SetPPParamSetReply(object objPPParamRmoteRequestReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(PPChangeRemoteEventReply));
                    serializer.Serialize(stringwriter, objPPParamRmoteRequestReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }
        /// <summary>
        /// 상위에서 유닛 인터락 메시지 올때 응답 보고
        /// </summary>
        /// <param name="objUnitInterlockReply"></param>
        public override void SetUnitInterlockReply(object objUnitInterlockReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(UnitInterlockReply));
                    serializer.Serialize(stringwriter, objUnitInterlockReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 유닛 상태 요청 응답 보고
        /// </summary>
        /// <param name="objUnitStateReply"></param>
        public override void SetUnitStateReply(object objUnitStateReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(UnitStateReply));
                    serializer.Serialize(stringwriter, objUnitStateReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 트레이스 데이터 시작 요청에 대한 응답 보고.
        /// </summary>
        /// <param name="objTraceStartReply"></param>
        public override void SetTraceStartReply(object objTraceStartReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(TraceStartReply));
                    serializer.Serialize(stringwriter, objTraceStartReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());

                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        /// <summary>
        /// 상위에서 트레이스 데이터 정지 요청 응답 보고.
        /// </summary>
        /// <param name="objTraceStopReply"></param>
        public override void SetTraceStopReply(object objTraceStopReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(TraceStopReply));
                    serializer.Serialize(stringwriter, objTraceStopReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        public override void SetCellInfomationEvent(object objCellInformationEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(CellInfomationEvent));
                    serializer.Serialize(stringwriter, objCellInformationEvent, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        public override void SetCarrierValidationCheckRequest(object objCarrierValidationCheck)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(CarrierValidationCheckRequest));
                    serializer.Serialize(stringwriter, objCarrierValidationCheck, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        public override void SetEquipmentSpecificStepEvent(object EquipmentSpecificStep)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(EQSpecificStepEvent));
                    serializer.Serialize(stringwriter, EquipmentSpecificStep, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }
        public override void SetPerformanceLossEvent(object PerformanceLossEvent)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(PerformanceLossEvent));
                    serializer.Serialize(stringwriter, PerformanceLossEvent, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

        public override void SetCellLotInformationRequest(object cellLotInformationRequest)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(CellLotInformationRequest));
                    serializer.Serialize(stringwriter, cellLotInformationRequest, ns);
                    m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        public override void SetEquipmentApproveReply(object equipmentApproveReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(EquipmentApproveReply));
                    serializer.Serialize(stringwriter, equipmentApproveReply, ns);
                    m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 상위에서 SPECIFICATION VERSION STATEREQUEST 요청에 대한 응답 보고
        /// </summary>
        /// <param name="SetSpecificationversionStateRequest"></param>
        public override void SetSpecificationversionStateReply(object objSpecificationversionStateReply)
        {
            do
            {
                try
                {
                    ExtentendStringWriter stringwriter = new ExtentendStringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(SpecificationversionStateReply));
                    serializer.Serialize(stringwriter, objSpecificationversionStateReply, ns);
                    this.m_objCommunicator.SendString(stringwriter.ToString());
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[SEND]\n" + stringwriter.ToString());
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

            } while (false);
        }

    }
}

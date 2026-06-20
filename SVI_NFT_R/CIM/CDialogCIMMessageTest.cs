using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogCIMMessageTest : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 임의 인터락 데이터 리스트
        /// </summary>
        private List<CModelData> m_ListData;

        /// <summary>
        /// 메시지 종류
        /// </summary>
        public enum EMessageType
        {
            CELL_INFOMATION_EVENT = 0,
            CELL_IN_EVENT,
            CELL_OUT_EVENT,
            INSP_RESULT_EVENT,
            ALARM_REPORT,
            TACT_EVENT,
            EQ_STATE_CHANGE_EVENT,
            AVAILABILITY_STATE_CHANGE_EVENT,
            INTERLOCK_STATE_CHANGE_EVENT,
            MOVE_STATE_CHANGE_EVENT,
            RUN_STATE_CHANGE_EVENT,
            FRONT_STATE_CHANGE_EVENT,
            REAR_STATE_CHANGE_EVENT,
            SAMPLE_LOT_STATE_CHANGE_EVENT,
            UNIT_STATE_CHANGE_EVENT,
            CONVEYOR_MOVE_STATE_CHANGE_EVENT,
            PP_CHANGE_EVENT,
            CURRENT_PPID_CHANGE_EVENT,
            CURRENT_PPID_PARAM_CHANGE_EVENT,
            OPCALL_CONFIRM_EVENT,
            INTERLOCK_CONFIRM_EVENT,
            UNIT_INTERLOCK_CONFIRM_EVENT,
            AVAILABILITY_AND_MOVE_STATE_CHANGE_EVENT,
            INTERLOCK_AND_MOVE_STATE_CHANGE_EVENT,
            EC_CHANGE_EVENT,
            TRACE_DATA_REPORT,
            EQ_SPECIFIC_STEP_EVENT,
            PERFORMANCE_LOSS_EVENT,
        }

        /// <summary>
        /// 리스트 칼럼 정의
        /// </summary>
        private enum EMessageListColumn
        {
            DATENAME,
            DATAVALUE,
            MESSAGE_LIST_FINAL
        };
        /// <summary>
        /// Document
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 심 메시지 타입
        /// </summary>
        private EMessageType m_eMessageType;
        /// <summary>
        /// 메시지 객체
        /// </summary>
        private object m_objSendMessage;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <param name="eMessageType"></param>
        /// <returns></returns>
        public CDialogCIMMessageTest(CDocument objDocument, EMessageType eMessageType)
        {
            InitializeComponent();
            m_ListData = new List<CModelData>();

            m_objDocument = objDocument as CDocument;
            m_eMessageType = eMessageType;
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogCIMMessageTest_Load(object sender, EventArgs e)
        {
            Initialize();
        }


        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogCIMMessageTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeInitialize();
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
                // 폼 초기화
                if (false == InitializeDialog()) break;

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {

        }

        /// <summary>
        /// 폼 초기화
        /// </summary>
        /// <returns></returns>
        public bool InitializeDialog()
        {
            bool bReturn = false;

            do
            {
                // 알람 리스트 그리드 뷰 초기화
                string[] strOpCallList = { EMessageListColumn.DATENAME.ToString(), EMessageListColumn.DATAVALUE.ToString() };
                if (false == InitializeGridView(this.dataGridViewMessage, strOpCallList)) break;

                InitializeMessage();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetTimer(true);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 메시지 초기화
        /// </summary>
        private void InitializeMessage()
        {
            switch (m_eMessageType)
            {
                case EMessageType.CELL_INFOMATION_EVENT:
                    {
                        if (m_eMessageType == EMessageType.CELL_INFOMATION_EVENT)
                        {
                            m_objSendMessage = new CellInfomationEvent();
                            ((CellInfomationEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        }
                        else
                        {
                            m_objSendMessage = new CarrierValidationCheckRequest();
                            ((CarrierValidationCheckRequest)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        }
                    }

                    break;
                case EMessageType.CELL_IN_EVENT:
                    m_objSendMessage = new CellTrackInEvent();
                    ((CellTrackInEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((CellTrackInEvent)m_objSendMessage).BODY.EVENT = "3";
                    ((CellTrackInEvent)m_objSendMessage).BODY.UNITID = "1";
                    ((CellTrackInEvent)m_objSendMessage).BODY.UNITST = "1";
                    ((CellTrackInEvent)m_objSendMessage).BODY.EQST.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    ((CellTrackInEvent)m_objSendMessage).BODY.EQST.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.CELLID = "TEST_A";
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.PPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.STEPID = "2";
                    ((CellTrackInEvent)m_objSendMessage).BODY.WORKORDER.PROCESSJOB = "1";
                    ((CellTrackInEvent)m_objSendMessage).BODY.WORKORDER.PLANQTY = "1000";
                    ((CellTrackInEvent)m_objSendMessage).BODY.WORKORDER.PROCESSEDQTY = "1";
                    ((CellTrackInEvent)m_objSendMessage).BODY.READER.READERID = "1";
                    ((CellTrackInEvent)m_objSendMessage).BODY.READER.READERRESULTCODE = "TEST_A";

                    break;
                case EMessageType.CELL_OUT_EVENT:
                    m_objSendMessage = new CellTrackOutEvent();
                    ((CellTrackOutEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((CellTrackOutEvent)m_objSendMessage).BODY.EVENT = "4";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.UNITID = "1";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.UNITST = "1";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.EQST.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    ((CellTrackOutEvent)m_objSendMessage).BODY.EQST.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.CELLID = "TEST_A";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.PPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.STEPID = "2";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.WORKORDER.PROCESSJOB = "1";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.WORKORDER.PLANQTY = "1000";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.WORKORDER.PROCESSEDQTY = "1";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.READER.READERID = "1";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.READER.READERRESULTCODE = "TEST_A";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.OPERATORID = m_objDocument.GetUserInformation().m_strID;
                    ((CellTrackOutEvent)m_objSendMessage).BODY.DV = new List<CellTrackOutEvent._CellTrackOutEvent.DVList>();
                    //for (int iLoopCount = 0; iLoopCount < Enum.GetNames(typeof(CStructureInfoData.EDVList)).Length; iLoopCount++)
                    //{
                    //    CellTrackOutEvent._CellTrackOutEvent.DVList objDVList = new CellTrackOutEvent._CellTrackOutEvent.DVList();
                    //    objDVList.DVNAME = ((CStructureInfoData.EDVList)iLoopCount).ToString();
                    //    objDVList.DVVAL = " ";
                    //    ((CellTrackOutEvent)m_objSendMessage).BODY.DV.Add(objDVList);
                    //}
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.OPERATORID = m_objDocument.GetUserInformation().m_strID;
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.JUDGE = CDefine.CIM_JUDGE_OUT;
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.REASONCODE = "1";
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.DESCRIPTION = "1";

                    break;
                case EMessageType.INSP_RESULT_EVENT:
                    m_objSendMessage = new InspectionResultEvent();
                    ((InspectionResultEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.ALARM_REPORT:
                    m_objSendMessage = new AlarmReport();
                    ((AlarmReport)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.TACT_EVENT:
                    m_objSendMessage = new TactEvent();
                    ((TactEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.EQ_STATE_CHANGE_EVENT:
                    m_objSendMessage = new EQStateChangeEvent();
                    ((EQStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.AVAILABILITY_STATE_CHANGE_EVENT:
                    m_objSendMessage = new AvailabilityStateChangeEvent();
                    ((AvailabilityStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.INTERLOCK_STATE_CHANGE_EVENT:
                    m_objSendMessage = new InterlockStateChangeEvent();
                    ((InterlockStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.MOVE_STATE_CHANGE_EVENT:
                    m_objSendMessage = new MoveStateChangeEvent();
                    ((MoveStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.RUN_STATE_CHANGE_EVENT:
                    m_objSendMessage = new RunStateChangeEvent();
                    ((RunStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.FRONT_STATE_CHANGE_EVENT:
                    m_objSendMessage = new FrontStateChangeEvent();
                    ((FrontStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.REAR_STATE_CHANGE_EVENT:
                    m_objSendMessage = new RearStateChangeEvent();
                    ((RearStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.SAMPLE_LOT_STATE_CHANGE_EVENT:
                    m_objSendMessage = new PP_SPLStateChangeEvent();
                    ((PP_SPLStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.UNIT_STATE_CHANGE_EVENT:
                    m_objSendMessage = new UnitStateChangeEvent();
                    ((UnitStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.CONVEYOR_MOVE_STATE_CHANGE_EVENT:
                    m_objSendMessage = new ConveyorMoveStateChangeEvent();
                    ((ConveyorMoveStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.PP_CHANGE_EVENT:
                    m_objSendMessage = new PPChangeEvent();
                    ((PPChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS = new PPChangeEvent._PPChangeEvent.COMMAND[1];
                    ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[0].PARAMS = new PPChangeEvent._PPChangeEvent.COMMAND.PARAM[6];
                    ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[0].CCODE = " ";
                    for (int iLoopCount = 0; iLoopCount < ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[0].PARAMS.Length; iLoopCount++)
                    {
                        ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[0].PARAMS[iLoopCount].PARAMNAME = " ";
                        ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[0].PARAMS[iLoopCount].PARAMVALUE = " ";
                    }
                    break;
                case EMessageType.CURRENT_PPID_CHANGE_EVENT:
                    m_objSendMessage = new CurrentPPIDChangeEvent();
                    ((CurrentPPIDChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.CURRENT_PPID_PARAM_CHANGE_EVENT:
                    m_objSendMessage = new CurrentPPIDParamChangeEvent();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS = new CurrentPPIDParamChangeEvent._CurrentPPIDParamChangeEvent.PARAM[6];
                    for (int iLoopCount = 0; iLoopCount < ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS.Length; iLoopCount++)
                    {
                        ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[iLoopCount].PARAMNAME = " ";
                        ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[iLoopCount].PARAMVALUE = " ";
                    }
                    break;
                case EMessageType.OPCALL_CONFIRM_EVENT:
                    m_objSendMessage = new OpCallConfirmEvent();
                    ((OpCallConfirmEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((OpCallConfirmEvent)m_objSendMessage).BODY.OPCALLS = new OpCallConfirmEvent._OpCallConfirmEvent.OPCALL[1];
                    for (int iLoopCount = 0; iLoopCount < 1; iLoopCount++)
                    {
                        ((OpCallConfirmEvent)m_objSendMessage).BODY.OPCALLS[0].MESSAGE = " ";
                        ((OpCallConfirmEvent)m_objSendMessage).BODY.OPCALLS[0].OPCALLID = " ";
                    }
                    break;
                case EMessageType.INTERLOCK_CONFIRM_EVENT:
                    m_objSendMessage = new InterlockConfirmEvent();
                    ((InterlockConfirmEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((InterlockConfirmEvent)m_objSendMessage).BODY.INTERLOCKS = new InterlockConfirmEvent._InterlockConfirmEvent.InterlockConfirm[1];
                    for (int iLoopCount = 0; iLoopCount < 1; iLoopCount++)
                    {
                        ((InterlockConfirmEvent)m_objSendMessage).BODY.INTERLOCKS[0].MESSAGE = " ";
                        ((InterlockConfirmEvent)m_objSendMessage).BODY.INTERLOCKS[0].INTERLOCKID = " ";
                    }
                    break;
                case EMessageType.UNIT_INTERLOCK_CONFIRM_EVENT:
                    m_objSendMessage = new UnitInterlockConfirmEvent();
                    ((UnitInterlockConfirmEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((UnitInterlockConfirmEvent)m_objSendMessage).BODY.INTERLOCKS = new UnitInterlockConfirmEvent._UnitInterlockConfirmEvent.INTERLOCK[1];
                    break;
                case EMessageType.AVAILABILITY_AND_MOVE_STATE_CHANGE_EVENT:
                    m_objSendMessage = new AvailabilityAndMoveStateChangeEvent();
                    ((AvailabilityAndMoveStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.INTERLOCK_AND_MOVE_STATE_CHANGE_EVENT:
                    m_objSendMessage = new InterlockAndMoveStateChangeEvent();
                    ((InterlockAndMoveStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.EC_CHANGE_EVENT:
                    m_objSendMessage = new ECChangeEvent();
                    ((ECChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((ECChangeEvent)m_objSendMessage).BODY.ECS = new ECChangeEvent._ECChangeEvent.EC[2];
                    for (int iLoopCount = 0; iLoopCount < ((ECChangeEvent)m_objSendMessage).BODY.ECS.Length; iLoopCount++)
                    {
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECID = " ";
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECNAME = " ";
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECDEF = " ";
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECSLL = " ";
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECSUL = " ";
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECWLL = " ";
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECWUL = " ";
                    }
                    break;
                case EMessageType.TRACE_DATA_REPORT:
                    m_objSendMessage = new TraceDataReport();
                    ((TraceDataReport)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    break;
                case EMessageType.EQ_SPECIFIC_STEP_EVENT:
                    //m_objSendMessage = new EQSpecificStepEvent();
                    //((EQSpecificStepEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    //((EQSpecificStepEvent)m_objSendMessage).NAME = "";
                    //((EQSpecificStepEvent)m_objSendMessage).TRANSACTIONNO = "";
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.CELLID = "";
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.PRODUCTID = "";
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.STEPID = "";
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.OPTIONINFO = "";
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.DESCRIPTION = "";

                    //var m_objProcessManagerInspectionStage = m_objDocument.m_objProcessMain.m_objProcessMotion.AoiStage;
                    //var data = m_objProcessManagerInspectionStage.Inspect.MapRecipeValidationData.Map;
                    //var recipeValidationItemsQuery =
                    //    from i in data
                    //    select new EQSpecificStepEvent._EQSpecificStepEvent.DVList()
                    //    {
                    //        ITEM_NAME = i.Key,
                    //        ITEM_VALUE = i.Value
                    //    };
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.ITEMS = recipeValidationItemsQuery.ToArray();
                    break;
                case EMessageType.PERFORMANCE_LOSS_EVENT:
                    m_objSendMessage = new PerformanceLossEvent();
                    ((PerformanceLossEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS = new PerformanceLossEvent._PerformanceLossEvent.LOSSOBJECT[1];
                    for (int iLoopCount = 0; iLoopCount < ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS.Length; iLoopCount++)
                    {
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].H_TACT_TIME = " ";
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].E_TACT_TIME = " ";
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].S_TIME = " ";
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].E_TIME = " ";
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].L_CODE = " ";
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].L_TEXT = " ";
                    }
                    break;
            }

            ShowDisplay((m_objSendMessage));

        }
        /// <summary>
        /// 데이터 리스트에 보여주기.
        /// </summary>
        /// <param name="EventMessage"></param>
        public void ShowDisplay(object EventMessage)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            CCIMAbstract.ExtentendStringWriter stringwriter = new CCIMAbstract.ExtentendStringWriter();
            XmlSerializer serializer;
            XmlNodeList xmlMsgList;
            ns.Add("", "");

            // 데이터 직렬화 시켜 XML로 변환.
            // 데이터 이름, 값 뽑아내기.
            switch (m_eMessageType)
            {
                case EMessageType.CELL_INFOMATION_EVENT:
                    {
                        if (m_eMessageType == EMessageType.CELL_INFOMATION_EVENT)
                        {
                            serializer = new XmlSerializer(((CellInfomationEvent)m_objSendMessage).GetType());
                            serializer.Serialize(stringwriter, ((CellInfomationEvent)m_objSendMessage), ns);
                            xmlDocument.LoadXml(stringwriter.ToString());

                            xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                            foreach (XmlNode xn in xmlMsgList)
                            {
                                XmlNode BodyNode = xn.SelectSingleNode("BODY");
                                if (BodyNode.HasChildNodes)
                                {
                                    foreach (XmlNode item in BodyNode.ChildNodes)
                                    {
                                        SetListData(item.Name, item.InnerText);
                                    }
                                }

                            }
                        }
                        else
                        {
                            serializer = new XmlSerializer(((CarrierValidationCheckRequest)m_objSendMessage).GetType());
                            serializer.Serialize(stringwriter, ((CarrierValidationCheckRequest)m_objSendMessage), ns);
                            xmlDocument.LoadXml(stringwriter.ToString());

                            xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                            foreach (XmlNode xn in xmlMsgList)
                            {
                                XmlNode BodyNode = xn.SelectSingleNode("BODY");
                                if (BodyNode.HasChildNodes)
                                {
                                    foreach (XmlNode item in BodyNode.ChildNodes)
                                    {
                                        SetListData(item.Name, item.InnerText);
                                    }
                                }

                            }
                        }
                    }


                    break;
                case EMessageType.CELL_IN_EVENT:
                    serializer = new XmlSerializer(((CellTrackInEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((CellTrackInEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());
                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            SetListData(BodyNode["EVENT"].Name, BodyNode["EVENT"].InnerText);
                            SetListData(BodyNode["UNITID"].Name, BodyNode["UNITID"].InnerText);
                            SetListData(BodyNode["UNITST"].Name, BodyNode["UNITST"].InnerText);

                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "EQST")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        foreach (XmlNode xmlCell in item.ChildNodes)
                                        {
                                            SetListData(xmlCell.Name, xmlCell.InnerText);
                                        }
                                    }
                                }
                                else if (item.Name == "CELL")
                                {
                                    XmlNode CellNode = item;
                                    foreach (XmlNode xmlCell in CellNode.ChildNodes)
                                    {
                                        SetListData(xmlCell.Name, xmlCell.InnerText);
                                    }
                                }
                                else if (item.Name == "WORKORDER")
                                {
                                    XmlNode CellNode = item;
                                    foreach (XmlNode xmlCell in CellNode.ChildNodes)
                                    {
                                        SetListData(xmlCell.Name, xmlCell.InnerText);
                                    }
                                }
                                else if (item.Name == "READER")
                                {
                                    XmlNode CellNode = item;
                                    foreach (XmlNode xmlCell in CellNode.ChildNodes)
                                    {
                                        SetListData(xmlCell.Name, xmlCell.InnerText);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case EMessageType.CELL_OUT_EVENT:
                    serializer = new XmlSerializer(((CellTrackOutEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((CellTrackOutEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());
                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            SetListData(BodyNode["EVENT"].Name, BodyNode["EVENT"].InnerText);
                            SetListData(BodyNode["UNITID"].Name, BodyNode["UNITID"].InnerText);
                            SetListData(BodyNode["UNITST"].Name, BodyNode["UNITST"].InnerText);

                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "EQST")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        foreach (XmlNode xmlCell in item.ChildNodes)
                                        {
                                            SetListData(xmlCell.Name, xmlCell.InnerText);
                                        }
                                    }
                                }
                                else if (item.Name == "CELL")
                                {
                                    XmlNode CellNode = item;
                                    foreach (XmlNode xmlCell in CellNode.ChildNodes)
                                    {
                                        SetListData(xmlCell.Name, xmlCell.InnerText);
                                    }
                                }
                                else if (item.Name == "WORKORDER")
                                {
                                    XmlNode CellNode = item;
                                    foreach (XmlNode xmlCell in CellNode.ChildNodes)
                                    {
                                        SetListData(xmlCell.Name, xmlCell.InnerText);
                                    }
                                }
                                else if (item.Name == "READER")
                                {
                                    XmlNode CellNode = item;
                                    foreach (XmlNode xmlCell in CellNode.ChildNodes)
                                    {
                                        SetListData(xmlCell.Name, xmlCell.InnerText);
                                    }
                                }
                                else if (item.Name == "DVS")
                                {
                                    XmlNodeList DvsNodeList = item.ChildNodes;
                                    foreach (XmlNode DvNode in DvsNodeList)
                                    {
                                        if (DvNode.Name == "DV")
                                        {
                                            foreach (XmlNode DVNode in DvNode.ChildNodes)
                                            {
                                                SetListData(DVNode.Name, DVNode.InnerText);
                                            }
                                        }
                                    }
                                }
                                else if (item.Name == "JUDGEMENT")
                                {
                                    XmlNode CellNode = item;
                                    foreach (XmlNode xmlCell in CellNode.ChildNodes)
                                    {
                                        SetListData(xmlCell.Name, xmlCell.InnerText);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case EMessageType.INSP_RESULT_EVENT:
                    serializer = new XmlSerializer(((InspectionResultEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((InspectionResultEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;


                case EMessageType.ALARM_REPORT:
                    serializer = new XmlSerializer(((AlarmReport)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((AlarmReport)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.TACT_EVENT:
                    serializer = new XmlSerializer(((TactEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((TactEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.EQ_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((EQStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((EQStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.AVAILABILITY_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((AvailabilityStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((AvailabilityStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.INTERLOCK_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((InterlockStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((InterlockStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.MOVE_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((MoveStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((MoveStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.RUN_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((RunStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((RunStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.FRONT_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((FrontStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((FrontStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.REAR_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((RearStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((RearStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.SAMPLE_LOT_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((PP_SPLStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((PP_SPLStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.CONVEYOR_MOVE_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((ConveyorMoveStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((ConveyorMoveStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.UNIT_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((UnitStateChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((UnitStateChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }

                    }
                    break;
                case EMessageType.PP_CHANGE_EVENT:
                    serializer = new XmlSerializer(((PPChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((PPChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());
                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "MODE" || item.Name == "PPID" || item.Name == "PPID_TYPE")
                                {
                                    SetListData(item.Name, item.InnerText);
                                }
                                else if (item.Name == "COMMANDS")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        int iIndex = 1;
                                        foreach (XmlNode itemPPST in CellNode.ChildNodes)
                                        {
                                            foreach (XmlNode itemCommand in itemPPST.ChildNodes)
                                            {
                                                SetListData(itemCommand.Name, itemCommand.InnerText);
                                                foreach (XmlNode itemParams in itemCommand.ChildNodes)
                                                {
                                                    foreach (XmlNode itemParam in itemParams.ChildNodes)
                                                    {
                                                        SetListData(itemParam.Name + string.Format("[ {0} ]", iIndex), itemParam.InnerText);
                                                    }
                                                    iIndex++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    break;
                case EMessageType.CURRENT_PPID_CHANGE_EVENT:
                    serializer = new XmlSerializer(((CurrentPPIDChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((CurrentPPIDChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }
                    }
                    break;
                case EMessageType.CURRENT_PPID_PARAM_CHANGE_EVENT:
                    serializer = new XmlSerializer(((CurrentPPIDParamChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((CurrentPPIDParamChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());
                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "PPST")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        foreach (XmlNode itemPPST in CellNode.ChildNodes)
                                        {
                                            SetListData(itemPPST.Name, itemPPST.InnerText);
                                        }
                                    }
                                }
                                else if (item.Name == "PARAMS")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        int iIndex = 1;
                                        foreach (XmlNode itemPPST in CellNode.ChildNodes)
                                        {
                                            foreach (XmlNode itemParam in itemPPST.ChildNodes)
                                            {
                                                SetListData(itemParam.Name + string.Format("[ {0} ]", iIndex), itemParam.InnerText);
                                            }
                                            iIndex++;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    break;
                case EMessageType.OPCALL_CONFIRM_EVENT:
                    serializer = new XmlSerializer(((OpCallConfirmEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((OpCallConfirmEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());
                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (xn.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "OPCALLS")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        int iIndex = 1;
                                        foreach (XmlNode itemPPST in CellNode.ChildNodes)
                                        {
                                            foreach (XmlNode itemParam in itemPPST.ChildNodes)
                                            {
                                                SetListData(itemParam.Name + string.Format("[ {0} ]", iIndex), itemParam.InnerText);
                                            }
                                            iIndex++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case EMessageType.INTERLOCK_CONFIRM_EVENT:
                    serializer = new XmlSerializer(((InterlockConfirmEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((InterlockConfirmEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());
                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (xn.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "INTERLOCKS")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        int iIndex = 1;
                                        foreach (XmlNode itemPPST in CellNode.ChildNodes)
                                        {
                                            foreach (XmlNode itemParam in itemPPST.ChildNodes)
                                            {
                                                SetListData(itemParam.Name + string.Format("[ {0} ]", iIndex), itemParam.InnerText);
                                            }
                                            iIndex++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case EMessageType.UNIT_INTERLOCK_CONFIRM_EVENT:
                    break;
                case EMessageType.AVAILABILITY_AND_MOVE_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((CurrentPPIDChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((CurrentPPIDChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }
                    }
                    break;
                case EMessageType.INTERLOCK_AND_MOVE_STATE_CHANGE_EVENT:
                    serializer = new XmlSerializer(((CurrentPPIDChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((CurrentPPIDChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }
                    }
                    break;
                case EMessageType.EC_CHANGE_EVENT:
                    serializer = new XmlSerializer(((ECChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((ECChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());
                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "ECS")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        int iIndex = 1;
                                        foreach (XmlNode xmlECS in item.ChildNodes)
                                        {
                                            foreach (XmlNode itemEC in xmlECS.ChildNodes)
                                            {
                                                SetListData(itemEC.Name + string.Format("[ {0} ]", iIndex), itemEC.InnerText);
                                            }
                                            iIndex++;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    break;
                case EMessageType.TRACE_DATA_REPORT:
                    serializer = new XmlSerializer(((CurrentPPIDChangeEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((CurrentPPIDChangeEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                SetListData(item.Name, item.InnerText);
                            }
                        }
                    }
                    break;

                case EMessageType.EQ_SPECIFIC_STEP_EVENT:
                    serializer = new XmlSerializer(((EQSpecificStepEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((EQSpecificStepEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "ITEMS")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        foreach (XmlNode xmlCell in item.ChildNodes)
                                        {
                                            SetListData(xmlCell.FirstChild.InnerText, xmlCell.LastChild.InnerText);
                                        }
                                    }
                                }
                                else
                                {
                                    SetListData(item.Name, item.InnerText);
                                }
                            }
                        }

                    }
                    break;

                case EMessageType.PERFORMANCE_LOSS_EVENT:
                    serializer = new XmlSerializer(((PerformanceLossEvent)m_objSendMessage).GetType());
                    serializer.Serialize(stringwriter, ((PerformanceLossEvent)m_objSendMessage), ns);
                    xmlDocument.LoadXml(stringwriter.ToString());

                    xmlMsgList = xmlDocument.GetElementsByTagName("MESSAGE");
                    foreach (XmlNode xn in xmlMsgList)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                if (item.Name == "LOSSOBJECTS")
                                {
                                    XmlNode CellNode = item;
                                    if (CellNode.HasChildNodes)
                                    {
                                        int iIndex = 1;
                                        foreach (XmlNode xmlLOSSOBJECTS in item.ChildNodes)
                                        {
                                            foreach (XmlNode itemLOSSOBJECT in xmlLOSSOBJECTS.ChildNodes)
                                            {
                                                SetListData(itemLOSSOBJECT.Name + string.Format("[ {0} ]", iIndex), itemLOSSOBJECT.InnerText);
                                            }
                                            iIndex++;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    break;
            }
        }

        /// <summary>
        /// 버튼 언어 변경.
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                // 설정 언어에 따라서 그리드 뷰나 버튼에 폰트가 변경되어야 할 수도 있음.
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(this.dataGridViewMessage, 12.0);


                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 타이머 시작 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            if (true == bTimer)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
        }

        /// <summary>
        /// 그리드 뷰 초기화
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        private bool InitializeGridView(DataGridView objGridView, string[] strColumnName)
        {
            bool bReturn = false;

            do
            {
                // 그리드 뷰 기본 스타일 초기화
                if (false == base.InitializeGridView(objGridView)) break;

                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
                {
                    objGridView.Columns.Add(strColumnName[iLoopColumn], strColumnName[iLoopColumn]);
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;

                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 그리드 뷰 데이터 적용.
        /// </summary>
        /// <param name="objListData"></param>
        public void SetDataGridViewData(List<CModelData> objListData)
        {
            bool bCompare = true;

            do
            {
                // 현재 그리드 뷰에 알람 리스트와 비교 (길이 다르면 바로 갱신)
                if (objListData.Count == this.dataGridViewMessage.RowCount)
                {
                    for (int iLoopList = 0; iLoopList < objListData.Count; iLoopList++)
                    {
                        if (objListData[iLoopList].m_strDateName != this.dataGridViewMessage[0, iLoopList].Value.ToString())
                        {
                            bCompare = false;
                            break;
                        }
                    }
                }
                else
                {
                    bCompare = false;
                }

                // 데이터 같으면 다시 쓸 필요 없음
                if (true == bCompare) break;

                // 갱신하는 부분
                lock (this.dataGridViewMessage)
                {
                    this.dataGridViewMessage.Rows.Clear();

                    for (int iLoopList = 0; iLoopList < objListData.Count; iLoopList++)
                    {
                        string[] strRowData = new string[(int)EMessageListColumn.MESSAGE_LIST_FINAL];
                        strRowData[(int)EMessageListColumn.DATENAME] = objListData[iLoopList].m_strDateName;
                        strRowData[(int)EMessageListColumn.DATAVALUE] = objListData[iLoopList].m_strDataValue;
                        this.dataGridViewMessage.Rows.Add(strRowData);
                    }
                }

            } while (false);
        }

        /// <summary>
        /// 타이머 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            SetDataGridViewData(m_ListData);
        }

        /// <summary>
        /// 리스트 데이터 적용
        /// </summary>
        /// <param name="strDataName"></param>
        /// <param name="strDataValue"></param>
        public void SetListData(string strDataName, string strDataValue)
        {
            do
            {
                m_ListData.Add(new CModelData(
                    strDataName,
                    strDataValue));

            } while (false);
        }

        /// <summary>
        /// 메시지 함수
        /// </summary>
        public void SendMessage()
        {
            int iIndex = 0;
            switch (m_eMessageType)
            {
                case EMessageType.CELL_INFOMATION_EVENT:
                    {
                        if (m_eMessageType == EMessageType.CELL_INFOMATION_EVENT)
                        {
                            ((CellInfomationEvent)m_objSendMessage).BODY.CARRIERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                            ((CellInfomationEvent)m_objSendMessage).BODY.CELLID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellInfomationEvent(((CellInfomationEvent)m_objSendMessage));
                        }
                        else
                        {
                            ((CarrierValidationCheckRequest)m_objSendMessage).BODY.CARRIERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                            ((CarrierValidationCheckRequest)m_objSendMessage).BODY.READERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                            ((CarrierValidationCheckRequest)m_objSendMessage).BODY.OPTIONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCarrierValidationCheckRequest(((CarrierValidationCheckRequest)m_objSendMessage));
                        }
                    }


                    break;
                case EMessageType.CELL_IN_EVENT:
                    ((CellTrackInEvent)m_objSendMessage).BODY.EVENT = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.UNITID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.UNITST = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.EQST.MOVESTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.EQST.RUNSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.EQST.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.EQST.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.CARRIERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.CELLID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.PPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.PRODUCTID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.STEPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.PRODUCTTYPE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.PRODUCTKIND = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.PRODUCTSPEC = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.CELLSIZE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.CELLTHICKNESS = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.COMMENT = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.CELLINFORESULT = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.CELL.REPLYSTATUS = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.WORKORDER.PROCESSJOB = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.WORKORDER.PLANQTY = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.WORKORDER.PROCESSEDQTY = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.READER.READERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackInEvent)m_objSendMessage).BODY.READER.READERRESULTCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellEvent(CCIMDefine.ECellEvent.CELL_EVENT_PROCESS_START, ((CellTrackInEvent)m_objSendMessage));
                    break;
                case EMessageType.CELL_OUT_EVENT:
                    ((CellTrackOutEvent)m_objSendMessage).BODY.EVENT = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.UNITID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.UNITST = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.EQST.MOVESTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.EQST.RUNSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.EQST.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.EQST.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.CARRIERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.CELLID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.PPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.PRODUCTID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.STEPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.PRODUCTTYPE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.PRODUCTKIND = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.PRODUCTSPEC = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.CELLSIZE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.CELLTHICKNESS = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.COMMENT = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.CELLINFORESULT = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.CELL.REPLYSTATUS = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.WORKORDER.PROCESSJOB = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.WORKORDER.PLANQTY = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.WORKORDER.PROCESSEDQTY = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.READER.READERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.READER.READERRESULTCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    for (int iLoopCount = 0; iLoopCount < ((CellTrackOutEvent)m_objSendMessage).BODY.DV.Count; iLoopCount++)
                    {
                        CellTrackOutEvent._CellTrackOutEvent.DVList dvList = new CellTrackOutEvent._CellTrackOutEvent.DVList();
                        dvList.DVNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        dvList.DVVAL = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((CellTrackOutEvent)m_objSendMessage).BODY.DV[iLoopCount] = dvList;
                    }
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.OPERATORID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.JUDGE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CellTrackOutEvent)m_objSendMessage).BODY.JUDGEMENT.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellEvent(CCIMDefine.ECellEvent.CELL_EVENT_PROCESS_END, ((CellTrackOutEvent)m_objSendMessage));
                    break;
                case EMessageType.INSP_RESULT_EVENT:
                    ((InspectionResultEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((InspectionResultEvent)m_objSendMessage).BODY.CARRIERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InspectionResultEvent)m_objSendMessage).BODY.CELLID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InspectionResultEvent)m_objSendMessage).BODY.PROCESSNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InspectionResultEvent)m_objSendMessage).BODY.PROCESSFLAG = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InspectionResultEvent)m_objSendMessage).BODY.JUDGE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InspectionResultEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InspectionResultEvent)m_objSendMessage).BODY.OPERID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InspectionResultEvent)m_objSendMessage).BODY.SENDUNIQUEINFO = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    //kjpark 20190123 V1.17 이상은 8개 필드 사용
                    ((InspectionResultEvent)m_objSendMessage).BODY.REVUNIQUEINFO = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetInspectionResultEvent(((InspectionResultEvent)m_objSendMessage));
                    break;

                case EMessageType.ALARM_REPORT:
                    ((AlarmReport)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((AlarmReport)m_objSendMessage).BODY.ALID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((AlarmReport)m_objSendMessage).BODY.ALST = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetAlarmReport(((AlarmReport)m_objSendMessage));
                    break;
                case EMessageType.TACT_EVENT:
                    ((TactEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((TactEvent)m_objSendMessage).BODY.LOCATION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((TactEvent)m_objSendMessage).BODY.TIME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetTactEvent(((TactEvent)m_objSendMessage));
                    break;
                case EMessageType.EQ_STATE_CHANGE_EVENT:
                    ((EQStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((EQStateChangeEvent)m_objSendMessage).BODY.AVAILABILITYSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((EQStateChangeEvent)m_objSendMessage).BODY.INTERLOCKSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((EQStateChangeEvent)m_objSendMessage).BODY.MOVESTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((EQStateChangeEvent)m_objSendMessage).BODY.RUNSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((EQStateChangeEvent)m_objSendMessage).BODY.FRONTSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((EQStateChangeEvent)m_objSendMessage).BODY.REARSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((EQStateChangeEvent)m_objSendMessage).BODY.PP_SPLSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((EQStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((EQStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetEqStateChangeEvent(((EQStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.AVAILABILITY_STATE_CHANGE_EVENT:
                    ((AvailabilityStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((AvailabilityStateChangeEvent)m_objSendMessage).BODY.AVAILABILITYSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((AvailabilityStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((AvailabilityStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetAvailabityStateChangeEvent(((AvailabilityStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.INTERLOCK_STATE_CHANGE_EVENT:
                    ((InterlockStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((InterlockStateChangeEvent)m_objSendMessage).BODY.INTERLOCKSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InterlockStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((InterlockStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetInterlockStateChangeEvent(((InterlockStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.MOVE_STATE_CHANGE_EVENT:
                    ((MoveStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((MoveStateChangeEvent)m_objSendMessage).BODY.MOVESTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((MoveStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((MoveStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetMoveStateChangeEvent(((MoveStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.RUN_STATE_CHANGE_EVENT:
                    ((RunStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((RunStateChangeEvent)m_objSendMessage).BODY.RUNSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((RunStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((RunStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetRunStateChangeEvent(((RunStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.FRONT_STATE_CHANGE_EVENT:
                    ((FrontStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((FrontStateChangeEvent)m_objSendMessage).BODY.FRONTSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((FrontStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((FrontStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetFrontStateChangeEvent(((FrontStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.REAR_STATE_CHANGE_EVENT:
                    ((RearStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((RearStateChangeEvent)m_objSendMessage).BODY.REARSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((RearStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((RearStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetRearStateChangeEvent(((RearStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.SAMPLE_LOT_STATE_CHANGE_EVENT:
                    ((PP_SPLStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((PP_SPLStateChangeEvent)m_objSendMessage).BODY.PP_SPLSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((PP_SPLStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((PP_SPLStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetSampleLotStateChangeEvent(((PP_SPLStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.UNIT_STATE_CHANGE_EVENT:
                    ((UnitStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.AVAILABILITYSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.INTERLOCKSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.MOVESTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.RUNSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.FRONTSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.REARSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.PP_SPLSTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.REASONCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((UnitStateChangeEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetUnitStateChangeEvent(((UnitStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.CONVEYOR_MOVE_STATE_CHANGE_EVENT:
                    ((ConveyorMoveStateChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((ConveyorMoveStateChangeEvent)m_objSendMessage).BODY.MOVESTATE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((ConveyorMoveStateChangeEvent)m_objSendMessage).BODY.STOPREASON = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetConveyorMoveStateChangeEvent(((ConveyorMoveStateChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.PP_CHANGE_EVENT:
                    ((PPChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((PPChangeEvent)m_objSendMessage).BODY.MODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((PPChangeEvent)m_objSendMessage).BODY.PPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((PPChangeEvent)m_objSendMessage).BODY.PPID_TYPE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    for (int iLoopCount = 0; iLoopCount < ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS.Length; iLoopCount++)
                    {
                        ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[iLoopCount].CCODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();

                        for (int iLoopParam = 0; iLoopParam < ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[iLoopCount].PARAMS.Length; iLoopParam++)
                        {
                            ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[iLoopCount].PARAMS[iLoopParam].PARAMNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                            ((PPChangeEvent)m_objSendMessage).BODY.COMMANDS[iLoopCount].PARAMS[iLoopParam].PARAMVALUE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        }
                    }

                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPPChangeEvent(((PPChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.CURRENT_PPID_CHANGE_EVENT:
                    ((CurrentPPIDChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((CurrentPPIDChangeEvent)m_objSendMessage).BODY.PPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDChangeEvent)m_objSendMessage).BODY.PPID_TYPE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDChangeEvent)m_objSendMessage).BODY.OLD_PPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCurrentPPIDChangeEvent((CurrentPPIDChangeEvent)m_objSendMessage);
                    break;
                case EMessageType.CURRENT_PPID_PARAM_CHANGE_EVENT:
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PPST.PPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PPST.PPIDST = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[0].PARAMNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[0].PARAMVALUE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[1].PARAMNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[1].PARAMVALUE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[2].PARAMNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[2].PARAMVALUE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[3].PARAMNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[3].PARAMVALUE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[4].PARAMNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[4].PARAMVALUE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[5].PARAMNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    ((CurrentPPIDParamChangeEvent)m_objSendMessage).BODY.PARAMS[5].PARAMVALUE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCurrentPPIDParamChangeEvent((CurrentPPIDParamChangeEvent)m_objSendMessage);
                    break;
                case EMessageType.OPCALL_CONFIRM_EVENT:
                    break;
                case EMessageType.INTERLOCK_CONFIRM_EVENT:
                    break;
                case EMessageType.UNIT_INTERLOCK_CONFIRM_EVENT:
                    break;
                case EMessageType.AVAILABILITY_AND_MOVE_STATE_CHANGE_EVENT:
                    break;
                case EMessageType.INTERLOCK_AND_MOVE_STATE_CHANGE_EVENT:
                    break;
                case EMessageType.EC_CHANGE_EVENT:
                    ((ECChangeEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    for (int iLoopCount = 0; iLoopCount < ((ECChangeEvent)m_objSendMessage).BODY.ECS.Length; iLoopCount++)
                    {
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECNAME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECDEF = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECSLL = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECSUL = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECWLL = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((ECChangeEvent)m_objSendMessage).BODY.ECS[iLoopCount].ECWUL = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    }
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetEcChangeEvent(((ECChangeEvent)m_objSendMessage));
                    break;
                case EMessageType.TRACE_DATA_REPORT:
                    ((TraceDataReport)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    ((TraceDataReport)m_objSendMessage).BODY.SV = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetTraceDataReport(((TraceDataReport)m_objSendMessage));
                    break;
                case EMessageType.EQ_SPECIFIC_STEP_EVENT:
                    //((EQSpecificStepEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.CELLID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.PPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.PRODUCTID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.STEPID = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.OPTIONINFO = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.DESCRIPTION = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();

                    //var m_objProcessManagerInspectionStage = m_objDocument.m_objProcessMain.m_objProcessMotion.AoiStage;
                    //var data = m_objProcessManagerInspectionStage.Inspect.MapRecipeValidationData.Map;
                    //((EQSpecificStepEvent)m_objSendMessage).BODY.ITEMS = new EQSpecificStepEvent._EQSpecificStepEvent.DVList[data.Count];
                    //for (int i = 0; i < data.Count; i++)
                    //{
                    //    ((EQSpecificStepEvent)m_objSendMessage).BODY.ITEMS[i].ITEM_NAME = this.dataGridViewMessage[(int)EMessageListColumn.DATENAME, iIndex].Value.ToString();
                    //    ((EQSpecificStepEvent)m_objSendMessage).BODY.ITEMS[i].ITEM_VALUE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    //}
                    //m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetEquipmentSpecificStepEvent(((EQSpecificStepEvent)m_objSendMessage));
                    break;
                case EMessageType.PERFORMANCE_LOSS_EVENT:
                    ((PerformanceLossEvent)m_objSendMessage).EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    for (int iLoopCount = 0; iLoopCount < ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS.Length; iLoopCount++)
                    {
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].H_TACT_TIME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].E_TACT_TIME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].S_TIME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].E_TIME = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].L_CODE = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                        ((PerformanceLossEvent)m_objSendMessage).BODY.LOSSOBJECTS[iLoopCount].L_TEXT = this.dataGridViewMessage[(int)EMessageListColumn.DATAVALUE, iIndex++].Value.ToString();
                    }
                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPerformanceLossEvent(((PerformanceLossEvent)m_objSendMessage));
                    break;

            }
        }
        /// <summary>
        /// 메시지 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendMessage_Click(object sender, EventArgs e)
        {
            SendMessage();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //CDialogCIMMessageTest.ActiveForm.Close();
            this.Close();
        }
        /// <summary>
        /// 종료 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //CDialogCIMMessageTest.ActiveForm.Close();
            this.Close();
        }

        /// <summary>
        /// 리스트 뷰 더블 클릭시 이벤트 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewMessage_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView objGridView = sender as DataGridView;
            // 위치 셀을 더블 클릭하면 키보드, 키패드 띄워서 데이터 받아서 다시 해당 셀에 집어넣음
            do
            {
                try
                {
                    int iRow = objGridView.CurrentCell.RowIndex;
                    int iColumn = objGridView.CurrentCell.ColumnIndex;
                    // 선택한 칼럼이 이름이면 키보드
                    if ((int)EMessageListColumn.DATAVALUE == iColumn)
                    {
                        using (var objKeyboard = new FormKeyBoard())
                        {
                            if (System.Windows.Forms.DialogResult.OK == objKeyboard.ShowDialog())
                            {
                                objGridView[iColumn, iRow].Value = objKeyboard.m_strReturnValue;
                            }
                        }
                    }
                    // 그 외 칼럼은 키패드
                    else
                    {
                        break;
                    }
                    objGridView.ClearSelection();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 임의 데이터 클래스 구현
        /// </summary>
        public class CModelData
        {
            /// <summary>
            /// 인터락 ID
            /// </summary>
            public string m_strDateName;
            /// <summary>
            /// 인터락 메시지
            /// </summary>
            public string m_strDataValue;

            public CModelData(string strDateName, string strDataValue)
            {
                m_strDateName = strDateName;
                m_strDataValue = strDataValue;
            }
        }
    }
}

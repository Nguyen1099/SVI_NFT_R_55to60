using SVI_NFT_R.UI.Dialog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    /// <summary>
    /// 메인 프레임 폼
    /// </summary>
    public partial class CMainFrame : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 타이틀 폼
        /// </summary>
        private CFormCommon m_objTitle;
        /// <summary>
        /// 뷰 폼
        /// </summary>
        private CFormCommon m_objView;
        /// <summary>
        /// 메뉴 폼
        /// </summary>
        private CFormCommon m_objMenu;
        /// <summary>
        /// OP Call Message Dialog
        /// </summary>
        public CDialogOperatorCallMessage m_objDialogOperatorCallMessage;
        /// <summary>
        /// Inerlock Message Dialog
        /// </summary>
        public CDialogInterlockMessage m_objDialogInterlockMessage;
        /// <summary>
        /// Terminal Message Dialog
        /// </summary>
        public CDialogTerminalMessage m_objDialogTerminalMessage;
        /// <summary>
        /// Unit Interlock Message Dialog
        /// </summary>
        public CDialogUnitInterlockMessage m_objDialogUnitInterlockMessage;
        /// <summary>
        /// Wait Dialog
        /// </summary>
        public CDialogWait m_objDialogWait;
        /// <summary>
        /// 인터락 관련 다이얼로그 Display 델리게이트.
        /// </summary>
        public delegate void DelegateShowMessage(DateTime time, string unitid, string id, string message);
        /// <summary>
        /// FormView에 있는 폼을 알람폼으로 변경해줌
        /// </summary>
        public delegate void DelegateSetChangeAlarmForm();
        /// <summary>
        /// FormView에 있는 폼을 알람폼으로 변경해줌
        /// </summary>
        public DelegateSetChangeAlarmForm m_delegateSetChangeAlarmForm;
        /// <summary>
        /// FormView에 있는 폼을 이니셜라이즈폼으로 변경해줌
        /// </summary>
        public DelegateSetChangeAlarmForm m_delegateSetChangeInitilizeForm;
        /// <summary>
        /// 현재 표시되고 있는 폼 정보를 갖고 있음
        /// </summary>
        private CFormCommon m_objCurrentForm;
        /// <summary>
        /// 조작금지 다이얼로그
        /// </summary>
        public CDialogNoOperation m_objDialogNoOperation;
        /// <summary>
        /// 도어 + 조작금지 다이얼로그
        /// </summary>
        public CDialogDoor m_objDialogSafety;
        /// <summary>
        /// CIM 연결 대기 다이얼로그
        /// </summary>
        public CDialogCIMConnectWait m_objDialogCIMConnectWait;
        /// <summary>
        /// CIM 접속 해제 다이얼로그
        /// </summary>
        public CDialogCIMDisconnected m_objDialogCIMDisconnected;
        /// <summary>
        /// 조작금지 다이얼로그 델리게이트
        /// </summary>
        public delegate void DelegateNoOperationDialogShow(bool bShow);
        /// <summary>
        /// Safety 다이얼로그 델리게이트
        /// </summary>
        public delegate void DelegateSafetyDialogShow();
        /// <summary>
        /// 패스워드 다이얼로그 델리게이트 (NoOperation)
        /// </summary>
        public delegate void DelegateNoOperationPassWordDialogShow();
        /// <summary>
        /// CIM 연결 다이얼로그 Display 델리게이드
        /// </summary>
        public delegate void DelegateShowCIMConnect(bool bShow);
        /// <summary>
        /// CIM 접속 해제 다이얼로그 Display 델리게이드
        /// </summary>
        public delegate void DelegateShowCIMDisConnect(bool bShow, string message);
        /// <summary>
        /// 대기 다이얼로그 델리게이트
        /// </summary>
        public delegate CDialogWait DelegateWaitMessage(bool bShow, string message);
        /// <summary>
        /// 대기 다이얼로그 델리게이트
        /// </summary>
        public delegate void DelegateLossCode(bool bShow, string message);
        /// <summary>
        /// 로그인 다이얼로그 델리게이트
        /// </summary>
        public delegate void DelegateLoginDialogShow();
        /// <summary>
        /// 로그인 다이얼로그
        /// </summary>
        public CDialogLogin m_objDialogLogin;
        private readonly List<InternalCheckGroupSet> mCheckGroupItems = new List<InternalCheckGroupSet>(128);

        private class InternalCheckGroupSet
        {
            public CDefine.ECheckGroup GroupIndex { get; private set; }
            public string ItemIndex { get; private set; }
            public UiAsset.IoIndicatorSet Data { get; private set; }

            public InternalCheckGroupSet(CDefine.ECheckGroup groupIndex, string itemIndex, UiAsset.IoIndicatorSet data)
            {
                GroupIndex = groupIndex;
                ItemIndex = itemIndex.ToString();
                Data = data;
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        public CMainFrame()
        {
            InitializeComponent();

            //Size = new Size(1280, 1024);
            Size = new Size(1920, 1080);
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CMainFrame_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            try
            {
                // 도큐먼트 클래스 생성
                m_objDocument = new CDocument();
                m_objDocument.Initialize();
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, "Program Start");

                // 설비 상태 점검 그룹 리스트 초기화
                SetCheckGroupItems();

                // 폼 초기화
                if (false == InitializeForm())
                    throw new Exception();

                if (true == m_objDocument.IsMasterLogin)
                {
                    CUserInformation userInformation = new CUserInformation
                    {
                        m_eAuthorityLevel = CDefine.EUserAuthorityLevel.MASTER,
                        m_strID = "enc_dev",
                        m_strName = "enc_dev",
                        m_strPassword = "1"
                    };
                    m_objDocument.SetLogin(userInformation);
                }

                m_objDocument.m_objProcessCIM.Connect();
            }
            catch (Exception e)
            {
                ExceptionTool.ShowExceptionDialog(e, "Mainframe Initialize fail");
                Environment.Exit(0);
            }

            return true;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, "Program End");
            m_objDocument.DeInitialize();
        }

        /// <summary>
        /// 폼 초기화
        /// </summary>
        /// <returns></returns>
        public bool InitializeForm()
        {
            bool bReturn = false;

            do
            {
                m_objCurrentForm = this;
                // 알람 폼 전환 델리게이트 생성
                m_delegateSetChangeAlarmForm = new DelegateSetChangeAlarmForm(SetChangeAlarmForm);
                m_delegateSetChangeInitilizeForm = new DelegateSetChangeAlarmForm(SetChangeInitializeForm);
                // 메인 폼 패널에 타이틀 / 몸통 / 메뉴 형식으로 붙인다.
                m_objTitle = new CFormTitle(m_objDocument);
                m_objView = new CFormView(m_objDocument);
                m_objMenu = new CFormMenu(m_objDocument);
                SetFormDockStyle(m_objTitle, this.panelTitle);
                SetFormDockStyle(m_objView, this.panelView);
                SetFormDockStyle(m_objMenu, this.panelMenu);
                CFormInterface objInterface = m_objTitle as CFormInterface;
                if (null != objInterface)
                {
                    objInterface.SetChangeLanguage();
                    objInterface = null;
                }
                objInterface = m_objMenu as CFormInterface;
                if (null != objInterface)
                {
                    objInterface.SetChangeLanguage();
                    objInterface = null;
                }

                // 인터락 메시지 관련 다이얼로그
                m_objDialogOperatorCallMessage = new CDialogOperatorCallMessage(m_objDocument);
                m_objDialogInterlockMessage = new CDialogInterlockMessage(m_objDocument);
                m_objDialogTerminalMessage = new CDialogTerminalMessage(m_objDocument);
                m_objDialogUnitInterlockMessage = new CDialogUnitInterlockMessage(m_objDocument);
                m_objDialogCIMConnectWait = new CDialogCIMConnectWait(m_objDocument);
                m_objDialogCIMDisconnected = new CDialogCIMDisconnected(m_objDocument);
                m_objDialogWait = new CDialogWait(m_objDocument);
                m_objDialogNoOperation = new CDialogNoOperation(m_objDocument);
                m_objDialogSafety = new CDialogDoor(m_objDocument);
                // 공유 메모리에 알람이 있는 경우 알람 폼으로 전환
                if (true == m_objDocument.GetIsAlarmMessage())
                {
                    // 알람폼 전환
                    m_objDocument.SetChangeAlarmForm();
                    // 알람 뷰 가져옴
                    m_objDocument.SetAlarmMessageUpdate();
                }

                // 컨펌하지 않은 메시지가 있다면 창을 다시 띄운다.
                if (0 < m_objDialogOperatorCallMessage.m_ListOpCall.Count)
                {
                    m_objDialogOperatorCallMessage.Show();
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.OpCall);
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                }
                if (0 < m_objDialogInterlockMessage.m_ListInterlock.Count)
                {
                    m_objDialogInterlockMessage.Show();
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.Interlock);
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                }
                if (0 < m_objDialogUnitInterlockMessage.m_ListInterlock.Count)
                {
                    m_objDialogUnitInterlockMessage.Show();
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.UnitInterlock);
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                }

                // 로그인 다이얼로그
                m_objDialogLogin = new CDialogLogin(m_objDocument);

                // 화면 잠금 관련 UI를 미리 로딩시켜 놓는다.
                {
                    Form[] lockScreens = new Form[]
                    {
                        m_objDialogSafety,
                        m_objDialogNoOperation
                    };
                    foreach (Form screen in lockScreens)
                    {
                        screen.TopLevel = false;
                        Controls.Add(screen);
                        // ! PM UI에 초기 로딩 시간이 오래걸려서 미리 로딩시켜 놓기 위한 트릭
                        screen.Show();
                        screen.Hide();
                    }
                }

                bReturn = true;
            } while (false);


            return bReturn;
        }

        public IEnumerable<UiAsset.IoIndicatorSet> GetCheckGroupItemsFromGroupIndex(CDefine.ECheckGroup groupIndex)
        {
            return mCheckGroupItems
                .Where(i => i.GroupIndex == groupIndex)
                .Select(i => i.Data)
                .ToArray();
        }

        public UiAsset.IoIndicatorSet GetCheckGroupItemFromIoIndex(CDeviceIODefine.EDigitalInput ioIndex)
        {
            string ioIndexText = ioIndex.ToString();

            return mCheckGroupItems
                .Where(i => i.ItemIndex == ioIndexText)
                .FirstOrDefault()
                .Data;
        }

        public CDefine.ECheckGroup GetCheckGroupFromIoIndex(CDeviceIODefine.EDigitalInput ioIndex)
        {
            string ioIndexText = ioIndex.ToString();

            return mCheckGroupItems
                .Where(i => i.ItemIndex == ioIndexText)
                .Select(i => i.GroupIndex)
                .FirstOrDefault();
        }

        /// <summary>
        /// 설비 상태 점검 그룹 리스트 초기화
        /// </summary>
        private void SetCheckGroupItems()
        {
            var checkGroupItems = new[]
            {
                new { Group = CDefine.ECheckGroup.AirAndVacuumSupply, Uiid = new string[] { "Normal", "Less" }, bNormalIsTrue = true, Index = CDeviceIODefine.EDigitalInput.X_MAIN_CDA_PRESSURE_SENSOR },
                new { Group = CDefine.ECheckGroup.AirAndVacuumSupply, Uiid = new string[] { "Normal", "Less" }, bNormalIsTrue = true, Index = CDeviceIODefine.EDigitalInput.X_MAIN_VACUUM_PRESSURE_SENSOR },
                new { Group = CDefine.ECheckGroup.AirAndVacuumSupply, Uiid = new string[] { "Normal", "Less" }, bNormalIsTrue = true, Index = CDeviceIODefine.EDigitalInput.X_INSP_STAGE_VAC_PRESSURE_SENSOR },

                new { Group = CDefine.ECheckGroup.ElectricitySupply, Uiid = new string[] { "CpOn", "CpOff" }, bNormalIsTrue = true, Index = CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON },

                new { Group = CDefine.ECheckGroup.Fan, Uiid = new string[] { "Normal", "Stop" }, bNormalIsTrue = true, Index = CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_FAN_1_NORMAL },
                new { Group = CDefine.ECheckGroup.Fan, Uiid = new string[] { "Normal", "Stop" }, bNormalIsTrue = true, Index = CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_FAN_2_NORMAL },

                new { Group = CDefine.ECheckGroup.EMS, Uiid = new string[] { "Normal", "Press" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_FRONT_EMERGENCY_SWITCH_PUSH },
                new { Group = CDefine.ECheckGroup.EMS, Uiid = new string[] { "Normal", "Press" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_REAR_EMERGENCY_SWITCH_PUSH },

                new { Group = CDefine.ECheckGroup.Door, Uiid = new string[] { "DoorClose", "DoorOpen" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_1_OPEN },
                new { Group = CDefine.ECheckGroup.Door, Uiid = new string[] { "DoorClose", "DoorOpen" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_2_OPEN },
                new { Group = CDefine.ECheckGroup.Door, Uiid = new string[] { "DoorClose", "DoorOpen" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_3_OPEN },
                new { Group = CDefine.ECheckGroup.Door, Uiid = new string[] { "DoorClose", "DoorOpen" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_4_OPEN },
                new { Group = CDefine.ECheckGroup.Door, Uiid = new string[] { "DoorClose", "DoorOpen" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_1_OPEN },
                new { Group = CDefine.ECheckGroup.Door, Uiid = new string[] { "DoorClose", "DoorOpen" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_2_OPEN },
                new { Group = CDefine.ECheckGroup.Door, Uiid = new string[] { "DoorClose", "DoorOpen" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_3_OPEN },

                new { Group = CDefine.ECheckGroup.HeatAndSmokeDetector, Uiid = new string[] { "Normal", "OverTemp" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_OVER_TEMP_ALARM },
                new { Group = CDefine.ECheckGroup.HeatAndSmokeDetector, Uiid = new string[] { "Normal", "OverTemp" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_PC_RACK_OVER_TEMP_ALARM },
                new { Group = CDefine.ECheckGroup.HeatAndSmokeDetector, Uiid = new string[] { "Normal", "SmokeDetect" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_SMOKE_ALARM },

                new { Group = CDefine.ECheckGroup.Etc, Uiid = new string[] { "Auto", "Teach" }, bNormalIsTrue = false, Index = CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH },
                new { Group = CDefine.ECheckGroup.Etc, Uiid = new string[] { "Normal", "DoorForceOpen" }, bNormalIsTrue = true, Index = CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_DOOR_FORCE_OPEN_NORMAL },
                new { Group = CDefine.ECheckGroup.Etc, Uiid = new string[] { "Normal", "EMS" }, bNormalIsTrue = true, Index = CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_EMS_NORMAL },
            };

            // 생성
            mCheckGroupItems.AddRange(checkGroupItems.Select(i => new InternalCheckGroupSet(i.Group, i.Index.ToString(), CreateCheckGroupItemFromIoIndex(i.Index, i.Uiid[0], i.Uiid[1], i.bNormalIsTrue))));

            // 로봇 EMS 생성
            foreach (CProcessMotion.ERobot robotIndex in Enum.GetValues(typeof(CProcessMotion.ERobot)))
            {
                string itemId = $"{robotIndex}_{DEVICE.Nachi.SignalNames.Bit.EInput.CONTROLLER_EMS}";
                mCheckGroupItems.Add(new InternalCheckGroupSet(CDefine.ECheckGroup.EMS, itemId, CreateCheckGroupItemFromFunc(() => m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot[robotIndex].Status.IsControllerEmsOn == false, itemId, "Normal", "Press", string.Empty)));
                itemId = $"{robotIndex}_{DEVICE.Nachi.SignalNames.Bit.EInput.PENDANT_EMS}";
                mCheckGroupItems.Add(new InternalCheckGroupSet(CDefine.ECheckGroup.EMS, itemId, CreateCheckGroupItemFromFunc(() => m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot[robotIndex].Status.IsPendantEmsOn == false, itemId, "Normal", "Press", string.Empty)));
            }
        }

        private UiAsset.IoIndicatorSet CreateCheckGroupItemFromIoIndex(CDeviceIODefine.EDigitalInput inputIndex, string stateTrueUiid, string stateFalseUiid, bool bNormalIsTrue)
        {
            // !!! uiGroup을 "EquipmentInspection"로 고정한 이유는 다른 UI에서 공용으로 사용 할 수 있는 항목이기 때문에 공통으로 사용하기 위함

            UiAsset.IoIndicatorSet ioIndicatorSet = new UiAsset.IoIndicatorSet();
            ioIndicatorSet.Initialize("EquipmentInspection", inputIndex.ToString(), () => m_objDocument.GetIOBit(inputIndex) == bNormalIsTrue);
            ioIndicatorSet.InitializeStateTrue(stateTrueUiid, m_colorOn);
            ioIndicatorSet.InitializeStateFalse(stateFalseUiid, m_colorOff);
            ioIndicatorSet.StateSubText = $" ({m_objDocument.m_objConfig.GetIOParameter().objIOParameter.Values.Where(item => item.strIOName == inputIndex.ToString()).First().strIndex})";
            return ioIndicatorSet;
        }

        private UiAsset.IoIndicatorSet CreateCheckGroupItemFromFunc(Func<bool> getIo, string titleUiid, string stateTrueUiid, string stateFalseUiid, string stateSubText)
        {
            // !!! uiGroup을 "EquipmentInspection"로 고정한 이유는 다른 UI에서 공용으로 사용 할 수 있는 항목이기 때문에 공통으로 사용하기 위함

            UiAsset.IoIndicatorSet ioIndicatorSet = new UiAsset.IoIndicatorSet();
            ioIndicatorSet.Initialize("EquipmentInspection", titleUiid, getIo);
            ioIndicatorSet.InitializeStateTrue(stateTrueUiid, m_colorOn);
            ioIndicatorSet.InitializeStateFalse(stateFalseUiid, m_colorOff);
            ioIndicatorSet.StateSubText = stateSubText;
            return ioIndicatorSet;
        }

        /// <summary>
        /// 폼 스타일 달라붙음
        /// </summary>
        /// <param name="objForm"></param>
        /// <param name="objPanel"></param>
        public void SetFormDockStyle(Form objForm, Panel objPanel)
        {
            objForm.Owner = this;
            objForm.TopLevel = false;
            objForm.Visible = true;
            objForm.Dock = DockStyle.Fill;
            objPanel.Controls.Add(objForm);
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool result = false;

            do
            {
                var formIF = m_objTitle as CFormInterface;
                if (null == formIF)
                {
                    break;
                }
                formIF.SetChangeLanguage();

                formIF = m_objView as CFormInterface;
                if (null == formIF)
                {
                    break;
                }
                formIF.SetChangeLanguage();

                formIF = m_objMenu as CFormInterface;
                if (null == formIF)
                {
                    break;
                }
                formIF.SetChangeLanguage();

                if (null != m_objDialogLogin)
                {
                    m_objDialogLogin.SetChangeLanguage();
                }
                result = true;
            } while (false);

            return result;
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
        }

        /// <summary>
        /// Title 포인터 넘김
        /// </summary>
        /// <returns></returns>
        public Form GetFormTitle()
        {
            return m_objTitle;
        }

        /// <summary>
        /// body 포인터 넘김
        /// </summary>
        /// <returns></returns>
        public Form GetFormView()
        {
            return m_objView;
        }

        /// <summary>
        /// Menu 포인터 넘김
        /// </summary>
        /// <returns></returns>
        public Form GetFormMenu()
        {
            return m_objMenu;
        }

        public void SetDelegateNoOperationDialogShow(bool bShow)
        {
            Invoke(new DelegateNoOperationDialogShow(SetNoOperationDialogShow), new object[] { bShow });
        }

        public void SetDelegateSafetyDialogShow()
        {
            Invoke(new DelegateSafetyDialogShow(SetSafetyDialogShow));
        }

        public void SetDelegateNoOperationPasswordDialogShow()
        {
            Invoke(new DelegateNoOperationPassWordDialogShow(SetNoOperationPasswordDialogShow));
        }

        /// <summary>
        /// 대기 다이얼로그 호출
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="strMessage"></param>
        public CDialogWait ShowWaitMessage(bool bShow, string strMessage = "")
        {
            return (CDialogWait)this.Invoke(new DelegateWaitMessage(WaitMessage), new object[] { bShow, strMessage });
        }

        /// <summary>
        /// LossCode 다이얼로그 호출
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="strMessage"></param>
        public void ShowLossCode(bool bShow, string strMessage = "")
        {
            this.Invoke(new DelegateLossCode(SetLossCode), new object[] { bShow, strMessage });
        }

        /// <summary>
        /// 로그인 다이얼로그 호출
        /// </summary>
        public void SetDelegateLoginDialogShow()
        {
            this.Invoke(new DelegateLoginDialogShow(SetLoginDialogShow));
        }

        /// <summary>
        /// 인터락 관련 다이얼로그 호출
        /// </summary>
        /// <param name="eMessageType"></param>
        /// <param name="time"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void ShowMessage(CCIMDefine.EMessageType eMessageType, DateTime time, string unitid, string id, string message)
        {
            if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_OP_CALL)
            {
                this.BeginInvoke(new DelegateShowMessage(ShowOPCallMessage), new object[] { time, unitid, id, message });
            }
            else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_INTERLOCK)
            {
                this.BeginInvoke(new DelegateShowMessage(ShowInterlockMessage), new object[] { time, unitid, id, message });
            }
            else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_TERMINAL)
            {
                this.BeginInvoke(new DelegateShowMessage(ShowTerminalMessage), new object[] { time, unitid, id, message });
            }
            else if (eMessageType == CCIMDefine.EMessageType.MESSAGE_TYPE_UNIT_INTERLOCK)
            {
                this.BeginInvoke(new DelegateShowMessage(ShowUnitInterlockMessage), new object[] { time, unitid, id, message });
            }
        }

        /// <summary>
        /// OP CALL 다이얼로그 보여주기.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        private void ShowOPCallMessage(DateTime time, string unitid, string id, string message)
        {
            // 데이터베이스에 이력 추가
            m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryOPCall(time.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), id, message);
            m_objDialogOperatorCallMessage.Show();
            m_objDialogOperatorCallMessage.SetListData(time, id, message);
            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.OpCall);
            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
        }

        /// <summary>
        /// Interlock 다이얼로그 보여주기.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        private void ShowInterlockMessage(DateTime time, string unitid, string id, string message)
        {
            // 데이터베이스에 이력 추가
            m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryInterlockCall(time.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), id, message);
            m_objDialogInterlockMessage.Show();
            m_objDialogInterlockMessage.SetListData(time, id, message);
            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.Interlock);
            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
        }

        /// <summary>
        /// Terminal 다이얼로그 보여주기.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        private void ShowTerminalMessage(DateTime time, string unitid, string id, string message)
        {
            // 데이터베이스에 이력 추가
            m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryTerminalCall(time.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), id, message);
            m_objDialogTerminalMessage.Show();
            m_objDialogTerminalMessage.SetListData(time, id, message);
        }

        /// <summary>
        /// Unit Interlock 다이얼로그 보여주기.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        private void ShowUnitInterlockMessage(DateTime time, string unitid, string id, string message)
        {
            // 데이터베이스에 이력 추가
            m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryInterlockCall(time.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), id, message);
            m_objDialogUnitInterlockMessage.Show();
            m_objDialogUnitInterlockMessage.SetListData(time, unitid, id, message);
            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.UnitInterlock);
            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
        }

        /// <summary>
        /// Wait 다이얼로그 보여주기.
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="strMessage"></param>
        private CDialogWait WaitMessage(bool bShow, string strMessage = "")
        {
            if (false == bShow)
            {
                m_objDialogWait.Hide();
            }
            else
            {
                if (m_objDialogWait.Visible == false)
                {
                    m_objDialogWait.Show();
                }
                if (string.IsNullOrEmpty(strMessage) == false)
                {
                    m_objDialogWait.SetText(strMessage);
                }
            }
            return m_objDialogWait;
        }

        /// <summary>
        /// Loss Code 보고.
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="strMessage"></param>
        public void SetLossCode(bool bShow, string strMessage = "")
        {
            // LossCode 
            using (CDialogLossCode objDialogLossCode = new CDialogLossCode(m_objDocument))
            {
                if (objDialogLossCode.ShowDialog() == DialogResult.OK)
                {
                    LossCodeEvent objLossCodeEvent = new LossCodeEvent();
                    objLossCodeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                    objLossCodeEvent.BODY.EQST.MOVESTATE = string.Format("{0}", (int)CCIMDefine.EMoveState.MOVE_STATE_PAUSE);
                    objLossCodeEvent.BODY.EQST.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]); // 사용 안함.
                    objLossCodeEvent.BODY.EQST.REASONCODE = " ";
                    objLossCodeEvent.BODY.EQST.DESCRIPTION = " ";
                    objLossCodeEvent.BODY.LOSS.LOSSCODE = string.Format("{0:D5}", (int)objDialogLossCode.m_eCodeNumber);
                    objLossCodeEvent.BODY.LOSS.LOSSDESCRIPTION = objDialogLossCode.m_strDescription;

                    m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetLossCodeEvent(objLossCodeEvent);
                    // jht Insert TP Code
                    m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryEquipmentTPCodeEvent(objLossCodeEvent.BODY.LOSS.LOSSCODE);
                    m_objDocument.m_strTPCode = objLossCodeEvent.BODY.LOSS.LOSSCODE;
                }
            }
        }

        /// <summary>
        /// 로그인 다이얼로그 띄워줌
        /// </summary>
        private void SetLoginDialogShow()
        {
            // 데이터 및 상태 초기화
            m_objDialogLogin.SetClear();
            // 로그인 창 띄움
            m_objDialogLogin.Show();
            m_objView.Enabled = false;
            m_objTitle.Enabled = false;
            m_objMenu.Enabled = false;
            // 현재 창 조작 안되도록 막음  
        }

        /// <summary>
        /// 조작금지 다이얼로그 띄워줌
        /// </summary>
        private void SetNoOperationDialogShow(bool bShow)
        {
            if (bShow == true)
            {
                m_objDialogNoOperation.Location = new Point(0, 0);
                m_objDialogNoOperation.BringToFront();
                m_objDialogNoOperation.Show();
            }
            else
            {
                m_objDialogNoOperation.Hide();
            }
        }

        private void SetSafetyDialogShow()
        {
            m_objDialogSafety.Location = new Point(0, 0);
            m_objDialogSafety.BringToFront();
            m_objDialogSafety.Show();
        }

        /// <summary>
        /// 패스워드 다이얼로그 띄워줌 (NoOperation)
        /// </summary>
        private void SetNoOperationPasswordDialogShow()
        {
            using (CDialogPassword dialog = new CDialogPassword(m_objDocument))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                SetNoOperationDialogShow(false);
            }
        }

        /// <summary>
        /// 알람폼으로 변경
        /// </summary>
        public void SetChangeAlarmForm()
        {
            CFormView objView = m_objView as CFormView;
            objView.SetChangeForm(CDefine.EFormView.FORM_VIEW_ALARM);
        }

        /// <summary>
        /// 이니셜라이즈폼으로 변경
        /// </summary>
        public void SetChangeInitializeForm()
        {
            CFormView objView = m_objView as CFormView;
            objView.SetChangeInitializeForm();
        }

        /// <summary>
        /// 택타임 조회 폼으로 변경
        /// </summary>
        public void SetChangeStatisticsTactTimeForm()
        {
            CFormView objView = m_objView as CFormView;
            Invoke(new Action(() => objView.SetChangeStatisticsTactTimeForm()));
        }

        /// <summary>
        /// 생상량 조회 폼으로 변경
        /// </summary>
        public void SetChangeStatisticsProductionForm()
        {
            CFormView objView = m_objView as CFormView;
            Invoke(new Action(() => objView.SetChangeStatisticsProductionForm()));
        }

        /// <summary>
        /// 알람 조회 폼으로 변경
        /// </summary>
        public void SetChangeStatisticsAlarmForm()
        {
            CFormView objView = m_objView as CFormView;
            Invoke(new Action(() => objView.SetChangeStatisticsAlarmForm()));
        }

        /// <summary>
        /// 현재 폼 링크를 설정
        /// </summary>
        /// <param name="objForm"></param>
        public void SetCurrentForm(CFormCommon objForm)
        {
            do
            {
                if (m_objCurrentForm == objForm)
                    break;
                m_objCurrentForm = objForm;
            } while (false);
        }

        /// <summary>
        /// 현재 폼 링크를 반환
        /// </summary>
        /// <returns></returns>
        public CFormCommon GetCurrentForm()
        {
            return m_objCurrentForm;
        }

        /// <summary>
        /// 인터락 관련 다이얼로그 호출
        /// </summary>
        /// <param name="bShow"></param>
        public void ShowCIMConnect(bool bShow)
        {
            this.Invoke(new DelegateShowCIMConnect(ShowCIMConnectedWait), new object[] { bShow });
        }

        /// <summary>
        /// TP 연결 상태 확인 다이얼로그 호출
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="strComment"></param>
        public void ShowCIMDisConnect(bool bShow, string strComment)
        {
            this.BeginInvoke(new DelegateShowCIMDisConnect(ShowCIMDisConnected), new object[] { bShow, strComment });
        }

        /// <summary>
        /// CIM 연결 상태 확인 다이얼로그 호출
        /// </summary>
        /// <param name="bShow"></param>
        public void ShowCIMDisConnect(bool bShow)
        {
            this.BeginInvoke(new DelegateShowCIMConnect(ShowCIMDisConnected), new object[] { bShow });
        }

        /// <summary>
        /// CIM 연결 대기 다이얼로그 호출
        /// </summary>
        /// <param name="bConnected"></param>
        private void ShowCIMConnectedWait(bool bConnected)
        {
            do
            {
                if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
                {
                    break;
                }

                if (bConnected)
                {
                    m_objDialogCIMConnectWait.Show();
                }
                else
                {
                    m_objDialogCIMConnectWait.Hide();
                }
            } while (false);
        }

        /// <summary>
        /// CIM 연결 끊김 다이얼로그 호출
        /// </summary>
        /// <param name="bConnected"></param>
        /// <param name="strComment"></param>
        private void ShowCIMDisConnected(bool bConnected, string strComment)
        {
            m_objDialogCIMDisconnected.SetComment(strComment);
            if (bConnected)
            {
                m_objDialogCIMDisconnected.Show();
            }
            else
            {
                m_objDialogCIMDisconnected.Hide();
            }
        }

        /// <summary>
        /// CIM 연결 상태 다이얼로그 호출
        /// </summary>
        /// <param name="bConnected"></param>
        private void ShowCIMDisConnected(bool bConnected)
        {
            if (bConnected)
            {
                m_objDialogCIMDisconnected.Show();
            }
            else
            {
                m_objDialogCIMDisconnected.Hide();
            }
        }

        public CDialogNotification ShowNotificationDialog(CAlarmDefine.EMessageList messageIndex, params object[] args)
        {
            CDialogNotification dialogHandle = null;
            Invoke(new Action(() =>
            {
                var viewUI = m_objView as CFormView;
                viewUI.SetChangeMainFlowForm();

                var mainFlowUI = GetCurrentForm() as CFormMainFlow;
                dialogHandle = mainFlowUI.ShowNotificationDialog(messageIndex, args);
            }));
            return dialogHandle;
        }

        public void CloseNotificationDialog(CDialogNotification dialogHandle)
        {
            if (dialogHandle == null || dialogHandle.IsDisposed)
            {
                return;
            }

            Invoke(new Action(() =>
            {
                if (dialogHandle.IsDisposed == false)
                {
                    dialogHandle.CloseDialog();
                }
            }));
        }

        /// <summary>
        /// ALT + F4로 폼 종료되는거 막음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CMainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON != objMachineStatus.m_eProgramExitStatus)
            {
                e.Cancel = true;
            }

            if (Keys.Alt == CDialogLogin.ModifierKeys || CDialogLogin.ModifierKeys == Keys.F4)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 키 다운 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CMainFrame_KeyDown(object sender, KeyEventArgs e)
        {
            if (true == e.Alt)
            {
                string strLog;
                switch (e.KeyCode)
                {
                    case Keys.L:
                        // 버튼 로그 추가
                        strLog = string.Format("[{0}] [{1}]", "Logout_Click", true);
                        m_objDocument.SetUpdateButtonLog(this, strLog);

                        m_objDocument.SetLogout();

                        strLog = string.Format("[{0}] [{1}]", "Logout_Click", false);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                        break;
                    case Keys.C:
                        m_objDocument.EquipmentDump("UserCapture");
                        break;
                }
            }
        }
    }
}

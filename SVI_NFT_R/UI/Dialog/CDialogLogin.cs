using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogLogin : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 로그 아웃 유무
        /// </summary>
        private readonly bool m_bLogout;
        private Color mUnderLineSelectionColor = Color.PowderBlue;
        private readonly Color mUnderLineNonSelectionColor = Color.LightGray;
        private CDefine.EUserAuthorityLevel mSelectUserLevel = CDefine.EUserAuthorityLevel.ENGINEER;
        private bool mbPasswordShow = false;
#if LOGIN_VER2
        private readonly bool mbLoginVersion2 = true;
#else
        private readonly bool mbLoginVersion2 = false;
#endif
        private bool mbMessageDialogShow = false;

        public bool IsCanceled { get; private set; }
        public bool IsCheckOnlyMode { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogLogin(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            m_bLogout = false;
            IsCheckOnlyMode = false;
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <param name="bLogout"></param>
        /// <returns></returns>
        public CDialogLogin(CDocument objDocument, bool bLogout)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            m_bLogout = bLogout;
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogLogin_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();

            IsCanceled = false;
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 해제
            DeInitialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        private bool Initialize()
        {
            bool bReturn = false;

            do
            {
                // 폼 초기화
                if (false == InitializeForm())
                    break;
                // 로그 아웃 true이면 로그 아웃
                if (true == m_bLogout)
                {
                    m_objDocument.SetLogout();
                }

                bReturn = true;
            } while (false);

            return bReturn;
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
        public bool InitializeForm()
        {
            bool bReturn = false;

            do
            {
                // 패스워드는 표시 안되게
                TextPassword.PasswordChar = '＊';
                // 현재 로그인 된 유저 정보를 얻어서 뿌려줘야 함.
                TextID.Text = "";
                TextPassword.Text = "";
                // 포커스 ID 버튼으로 이동
                ActiveControl = TextID;
                // 버튼 색상 정의
                SetButtonColor();
                // 언어 변경
                SetChangeLanguage();

                if (m_objDocument.IsMasterLoginEnabled)
                {
                    mUnderLineSelectionColor = Color.LightSeaGreen;
                }
                else
                {
                    mUnderLineSelectionColor = Color.PowderBlue;
                }

                SetClear();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = true;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            SetButtonColor(BtnTitleLogin, Color.Black, BtnTitleLogin.BackColor);
            SetButtonColor(BtnTitleLoginVer2, Color.Black, BtnTitleLoginVer2.BackColor);
            SetButtonForeColor(BtnTitleID, Color.Black);
            SetButtonForeColor(BtnTitlePassword, Color.Black);
            SetButtonBackColor(BtnPasswordModeSelectRm, m_colorNormal);
            SetButtonBackColor(BtnPasswordModeSelectEqp, m_colorClick);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(btnSelectOperator.Name)
                    && false == btn.Name.Equals(btnSelectEngineer.Name)
                    && false == btn.Name.Equals(btnSelectMaster.Name)
                    && false == btn.Name.Equals(btnPasswordShow.Name)
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                SetButtonChangeLanguage(BtnTitleID);
                SetButtonChangeLanguage(BtnTitlePassword);
                SetLabelChangeLanguage(lblSubTitleSelectUserLevel);
                SetButtonChangeLanguage(BtnCancel);
                SetButtonChangeLanguage(BtnBuzzerOff);
                lblSubTitlePassword.Text = m_objDocument.GetDatabaseUIText("BtnTitlePassword", Name);
                SetButtonText(BtnCancelVer2, m_objDocument.GetDatabaseUIText("BtnCancel", Name));
                SetButtonText(BtnBuzzerOffVer2, m_objDocument.GetDatabaseUIText("BtnBuzzerOff", Name));
                SetButtonChangeLanguage(btnSelectOperator);
                SetButtonChangeLanguage(btnSelectEngineer);
                SetButtonChangeLanguage(btnSelectMaster);
                SetControlChangeLanguage(LblTitlePasswordModeSelect);
                if (true == IsCheckOnlyMode)
                {
                    SetButtonText(BtnTitleLogin, m_objDocument.GetDatabaseUIText("BtnTitleUserCheck", Name));
                    SetButtonText(BtnTitleLoginVer2, m_objDocument.GetDatabaseUIText("BtnTitleUserCheck", Name));
                    SetButtonText(BtnLogin, m_objDocument.GetDatabaseUIText("BtnCheck", Name));
                    SetButtonText(BtnLoginVer2, m_objDocument.GetDatabaseUIText("BtnCheck", Name));
                }
                else
                {
                    SetButtonChangeLanguage(BtnTitleLogin);
                    SetButtonText(BtnTitleLoginVer2, m_objDocument.GetDatabaseUIText("BtnTitleLogin", Name));
                    SetButtonChangeLanguage(BtnLogin);
                    SetButtonText(BtnLoginVer2, m_objDocument.GetDatabaseUIText("BtnLogin", Name));
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(Button objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetControlChangeLanguage(Control control)
        {
            control.Text = m_objDocument.GetDatabaseUIText(control.Name, Name);
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetLabelChangeLanguage(Label label)
        {
            string uiText = m_objDocument.GetDatabaseUIText(label.Name, Name);
            if (label.Text != uiText)
            {
                label.Text = uiText;
            }
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
        }

        /// <summary>
        /// 로그인에 사용한 데이터 초기화
        /// </summary>
        public void SetClear()
        {
            if (m_objDocument.IsMasterLoginEnabled)
            {
                mUnderLineSelectionColor = Color.LightSeaGreen;
            }
            else
            {
                mUnderLineSelectionColor = Color.PowderBlue;
            }
            SetButtonColor();
            SetChangeLanguage();
            // 포커스 ID 버튼으로 이동
            ActiveControl = TextID;
            // 데이터 클리어
            TextID.Text = "";
            TextPassword.Text = "";

            panelBase.BackColor = Color.White;
            pnlLoginVersion1.Visible = false;
            pnlLoginVersion2.Visible = false;
            if (mbLoginVersion2 == false)
            {
                pnlLoginVersion1.Visible = true;
                Size = pnlLoginVersion1.Size;
            }
            else
            {
                selectUserLevel(CDefine.EUserAuthorityLevel.ENGINEER);
                pnlLoginVersion2.Visible = true;
                Size = pnlLoginVersion2.Size;
            }
            Size = new Size(Width, Height + PnlPasswordModeSelect.Height);

            // 화면 중앙
            CenterToScreen();
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            Enabled = false;

            string strID = TextID.Text;
            string strPassword = TextPassword.Text;

            do
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}]", "BtnLogin_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);
                // 마스터 로그인 상태 초기화
                m_objDocument.IsMasterLogin = false;
                if (
                    (
                    true == string.IsNullOrEmpty(strID)
                    && strPassword == string.Format("wpdjqntj{0:MM}", DateTime.Now)
                    && true == m_objDocument.IsMasterLoginEnabled
                    )
                    ||
                    (
                    mbLoginVersion2 == true
                    && mSelectUserLevel == CDefine.EUserAuthorityLevel.MASTER
                    && strPassword == $"{DateTime.Now:yyyyMMdd}"
                    && true == m_objDocument.IsMasterLoginEnabled
                    )
                    )
                {
                    if (false == IsCheckOnlyMode)
                    {
                        // 무사히 로그인 한 경우 해당 로그인 정보를 도큐먼트에 올리고 로그인 창을 닫아줌
                        CUserInformation objUserInformation = new CUserInformation();
                        objUserInformation.m_strID = "enc_dev";
                        objUserInformation.m_strName = "enc_dev";
                        objUserInformation.m_strPassword = "1";
                        objUserInformation.m_eAuthorityLevel = CDefine.EUserAuthorityLevel.MASTER;
                        m_objDocument.IsMasterLogin = true;
                        m_objDocument.IsMasterLoginEnabled = false;
                        m_objDocument.SetLogin(objUserInformation);
                    }
                }
                else
                {
                    // 로그인 진행
                    if (await ProcessLogin(strID, strPassword) == false)
                    {
                        break;
                    }
                }

                DialogResult = DialogResult.OK;

                // 현재 창 조작되도록 다시 해제
                m_objDocument.GetMainFrame().GetFormView().Enabled = true;
                m_objDocument.GetMainFrame().GetFormTitle().Enabled = true;
                m_objDocument.GetMainFrame().GetFormMenu().Enabled = true;
                IsCanceled = false;

                // 창 숨김
                Hide();
            } while (false);

            m_objDocument.IsMasterLoginEnabled = false;
            mUnderLineSelectionColor = Color.PowderBlue;
            if (mUnderLineNonSelectionColor != BtnIDFocus.BackColor)
            {
                SetButtonBackColor(BtnIDFocus, mUnderLineSelectionColor);
            }
            if (mUnderLineNonSelectionColor != BtnPasswordFocus.BackColor)
            {
                SetButtonBackColor(BtnPasswordFocus, mUnderLineSelectionColor);
            }

            Enabled = true;
        }

        private async Task<bool> ProcessLogin(string id, string password)
        {
            CUserInformation userInfo = null;

            if (string.IsNullOrWhiteSpace(id) == true)
            {
                mbMessageDialogShow = true;
                // Msg: 아이디가 없습니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NOT_ID);
                ActiveControl = TextID;
                await Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                    mbMessageDialogShow = false;
                });
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) == true)
            {
                mbMessageDialogShow = true;
                // Msg: 비밀번호가 없습니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NOT_PASSWORD);
                ActiveControl = TextPassword;
                await Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                    mbMessageDialogShow = false;
                });
                return false;
            }

            // 유저 데이터베이스 리스트에 아이디랑 암호가 일치하는지 확인 후에 Login 해야 함
            userInfo = m_objDocument.GetDatabaseUser(id);
            CCryptography objEncoder = new CCryptography();

            // 유저 아이디가 없는 경우 null 값
            if (userInfo == null)
            {
                // CIM 미사용 모드 일 땐 내부 DB만 사용함
                //if (m_objDocument.m_objConfig.GetOptionParameter().bUseCIM == false)
                {
                    mbMessageDialogShow = true;
                    // Msg: 유저 아이디가 일치하지 않습니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.USER_ID_DO_NOT_MATCH);
                    ActiveControl = TextID;
                    TextID.Select(0, TextID.Text.Length);
                    await Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(100);
                        mbMessageDialogShow = false;
                    });
                    return false;
                }

                //// CIM Login 처리
                //var cim = m_objDocument.m_objProcessCIM;

                //// 데이터 초기화
                //cim.m_objProcessCIMTerminalDisplayRequest.LoginAcknowledge.Reset();
                //cim.m_objProcessCIMEquipmentApproveSend.GetReceivedData();

                //// TC에 로그인 요청
                //OperatorInfoEvent operatorInformation = new OperatorInfoEvent();
                //operatorInformation.BODY.OPTIONINFO = "LOGIN";
                //operatorInformation.BODY.OPERATORID = id;
                //operatorInformation.BODY.PASSWORD = password;
                //cim.m_objCIMSendMessage.SetOperatorInfoEvent(operatorInformation);

                //// 응답 대기
                //m_objDocument.GetMainFrame().ShowWaitMessage(true, "WAIT FOR LOGIN REPLY");
                //TimeSpan timeout = Config.WaitTime.CommTimeout.CimLoginTimeout.ToTimeSpan();
                //if (await Task.Factory.StartNew(() => cim.m_objProcessCIMEquipmentApproveSend.WaitForReceive(timeout)) == false)
                //{
                //    mbMessageDialogShow = true;
                //    // Msg: TC에서 응답이 없습니다.
                //    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOGIN_FAILED_TC_TIMEOUT);
                //    await Task.Factory.StartNew(() =>
                //    {
                //        Thread.Sleep(100);
                //        mbMessageDialogShow = false;
                //    });
                //    m_objDocument.GetMainFrame().ShowWaitMessage(false);
                //    return false;
                //}
                //m_objDocument.GetMainFrame().ShowWaitMessage(false);

                //var reply = cim.m_objProcessCIMEquipmentApproveSend.GetReceivedData();

                //// 로그인 성공 여부 확인
                //if (reply.BODY.RCMD != "31")
                //{
                //    // 터미널 메시지 팝업
                //    return false;
                //}

                //userInfo = new CUserInformation();
                //userInfo.m_strID = id;
                //userInfo.m_strName = id;
                //userInfo.m_strPassword = objEncoder.SetEncoding(password);
                //userInfo.m_eAuthorityLevel = CDefine.EUserAuthorityLevel.OPERATOR;
                //userInfo.IsLoginFromCimInformation = true;
            }

            // 타겟 패스워드도 인코딩해서 문자열 일치하는지 비교
            if (userInfo.m_strPassword != objEncoder.SetEncoding(password))
            {
                mbMessageDialogShow = true;
                // Msg: 로그인 정보가 일치하지 않습니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THE_LOGIN_INFORMATION_DOES_NOT_MATCH);
                ActiveControl = TextPassword;
                TextPassword.Select(0, TextPassword.Text.Length);
                await Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                    mbMessageDialogShow = false;
                });
                return false;
            }

            // 버튼 로그
            m_objDocument.SetUpdateButtonLog(this, $"[BtnLogin_Click] [Login ID: {userInfo.m_strID} Name: {userInfo.m_strName} Level: {userInfo.m_eAuthorityLevel}]");

            if (false == IsCheckOnlyMode)
            {
                // 무사히 로그인 한 경우 해당 로그인 정보를 도큐먼트에 올리고 로그인 창을 닫아줌
                m_objDocument.SetLogin(userInfo);
            }

            return true;
        }

        /// <summary>
        /// 사용 안함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnCancel_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
            DialogResult = DialogResult.Cancel;
            IsCanceled = true;
            Hide();
        }

        /// <summary>
        /// 로그 아웃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnLogout_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
            m_objDocument.SetLogout();
        }

        /// <summary>
        /// 엔터 누르면 로그인, ESC 누르면 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextID_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    BtnLogin_Click(sender, e);
                    break;
                case Keys.Escape:
                    BtnCancel_Click(sender, e);
                    break;
            }
        }

        /// <summary>
        /// 엔터 누르면 로그인, ESC 누르면 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextPassword_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    BtnLogin_Click(sender, e);
                    break;
                case Keys.Escape:
                    BtnCancel_Click(sender, e);
                    break;
            }
        }

        /// <summary>
        /// ALT + F4로 폼 종료되는거 막음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON != objMachineStatus.m_eProgramExitStatus)
            {
                e.Cancel = true;
            }

            if (Keys.Alt == ModifierKeys || ModifierKeys == Keys.F4)
            {
                e.Cancel = true;
            }
        }

        private void TextID_Enter(object sender, EventArgs e)
        {
            TextID.Select(0, TextID.Text.Length);
            SetButtonBackColor(BtnIDFocus, mUnderLineSelectionColor);
        }

        private void TextPassword_Enter(object sender, EventArgs e)
        {
            TextPassword.Select(0, TextPassword.Text.Length);
            SetButtonBackColor(BtnPasswordFocus, mUnderLineSelectionColor);
        }

        private void TextID_MouseClick(object sender, MouseEventArgs e)
        {
            TextID.Select(0, TextID.Text.Length);
        }

        private void TextPassword_MouseClick(object sender, MouseEventArgs e)
        {
            TextPassword.Select(0, TextPassword.Text.Length);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
            {
                SetButtonBackColor(BtnBuzzerOff, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnBuzzerOff, m_colorClick);
            }

            if (mbLoginVersion2 == true)
            {
                StringBuilder passwordString = new StringBuilder();
                if (mbPasswordShow == true)
                {
                    passwordString.Append(TextPassword.Text);
                }
                else
                {
                    for (int i = 0; i < TextPassword.Text.Length; i++)
                    {
                        passwordString.Append('＊');
                    }
                }
                SetButtonText(btnPasswordDisplay, passwordString.ToString());
            }
        }

        private void BtnBuzzerOff_Click(object sender, EventArgs e)
        {
            do
            {
                // 알람 없는 경우 
                if (false == m_objDocument.GetIsAlarmMessage())
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                    break;
                }

                if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                }
                else
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0} : {1}]", "BtnBuzzerOff_Click", m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus().ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private void TextID_Leave(object sender, EventArgs e)
        {
            SetButtonBackColor(BtnIDFocus, mUnderLineNonSelectionColor);
            SetButtonBackColor(BtnPasswordFocus, mUnderLineNonSelectionColor);
        }

        private void btnSelectOperator_Click(object sender, EventArgs e)
        {
            selectUserLevel(CDefine.EUserAuthorityLevel.OPERATOR);
        }

        private void btnSelectEngineer_Click(object sender, EventArgs e)
        {
            selectUserLevel(CDefine.EUserAuthorityLevel.ENGINEER);
        }

        private void btnSelectMaster_Click(object sender, EventArgs e)
        {
            selectUserLevel(CDefine.EUserAuthorityLevel.MASTER);
        }

        private void selectUserLevel(CDefine.EUserAuthorityLevel userLevel)
        {
            mSelectUserLevel = userLevel;

            btnSelectOperator.BackColor = Color.White;
            btnSelectEngineer.BackColor = Color.White;
            btnSelectMaster.BackColor = Color.White;

            var currentFocusLocation = btnSelectFocus.Location;
            switch (mSelectUserLevel)
            {
                case CDefine.EUserAuthorityLevel.OPERATOR:
                    btnSelectOperator.BackColor = Color.LightGreen;
                    currentFocusLocation.X = btnSelectOperator.Location.X;
                    TextID.Text = "OP";
                    break;
                case CDefine.EUserAuthorityLevel.ENGINEER:
                    btnSelectEngineer.BackColor = Color.LightGreen;
                    currentFocusLocation.X = btnSelectEngineer.Location.X;
                    TextID.Text = "ENG";
                    break;
                case CDefine.EUserAuthorityLevel.MASTER:
                    btnSelectMaster.BackColor = Color.LightGreen;
                    currentFocusLocation.X = btnSelectMaster.Location.X;
                    TextID.Text = "MASTER";
                    break;
            }
            btnSelectFocus.Location = currentFocusLocation;
            TextPassword.Clear();
        }

        private void btnNumPad_Click(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            TextPassword.Text += button.Text;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            TextPassword.Clear();
        }

        private void btnPasswordShow_MouseDown(object sender, MouseEventArgs e)
        {
            mbPasswordShow = true;
        }

        private void btnPasswordShow_MouseUp(object sender, MouseEventArgs e)
        {
            mbPasswordShow = false;
        }

        private void CDialogLogin_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.M:
                    if (false == e.Alt)
                    {
                        return;
                    }

                    // 마스터 로그인 진입 Blocking
                    m_objDocument.IsMasterLoginEnabled = !m_objDocument.IsMasterLoginEnabled;
                    if (m_objDocument.IsMasterLoginEnabled)
                    {
                        mUnderLineSelectionColor = Color.LightSeaGreen;
                        ActiveControl = TextPassword;
                    }
                    else
                    {
                        mUnderLineSelectionColor = Color.PowderBlue;
                    }
                    if (mUnderLineNonSelectionColor != BtnIDFocus.BackColor)
                    {
                        SetButtonBackColor(BtnIDFocus, mUnderLineSelectionColor);
                    }
                    if (mUnderLineNonSelectionColor != BtnPasswordFocus.BackColor)
                    {
                        SetButtonBackColor(BtnPasswordFocus, mUnderLineSelectionColor);
                    }
                    if (mUnderLineNonSelectionColor != btnSelectFocus.BackColor)
                    {
                        SetButtonBackColor(btnSelectFocus, mUnderLineSelectionColor);
                    }
                    break;
            }

            if (
                mbLoginVersion2 == true
                && mbMessageDialogShow == false
                )
            {
                switch (e.KeyCode)
                {
                    case Keys.D0:
                    case Keys.NumPad0:
                        TextPassword.Text += "0";
                        break;
                    case Keys.D1:
                    case Keys.NumPad1:
                        TextPassword.Text += "1";
                        break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        TextPassword.Text += "2";
                        break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        TextPassword.Text += "3";
                        break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        TextPassword.Text += "4";
                        break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        TextPassword.Text += "5";
                        break;
                    case Keys.D6:
                    case Keys.NumPad6:
                        TextPassword.Text += "6";
                        break;
                    case Keys.D7:
                    case Keys.NumPad7:
                        TextPassword.Text += "7";
                        break;
                    case Keys.D8:
                    case Keys.NumPad8:
                        TextPassword.Text += "8";
                        break;
                    case Keys.D9:
                    case Keys.NumPad9:
                        TextPassword.Text += "9";
                        break;
                    case Keys.Back:
                        if (TextPassword.Text.Length > 0)
                        {
                            TextPassword.Text = TextPassword.Text.Substring(0, TextPassword.Text.Length - 1);
                        }
                        break;
                    case Keys.Escape:
                        BtnCancel_Click(this, EventArgs.Empty);
                        break;
                    case Keys.Space:
                    case Keys.Enter:
                        BtnLogin_Click(this, EventArgs.Empty);
                        break;
                    case Keys.Tab:
                        if (e.Shift == true)
                        {
                            if (mSelectUserLevel == CDefine.EUserAuthorityLevel.OPERATOR)
                            {
                                selectUserLevel(CDefine.EUserAuthorityLevel.MASTER);
                            }
                            else
                            {
                                selectUserLevel(--mSelectUserLevel);
                            }
                        }
                        else
                        {
                            if (mSelectUserLevel == CDefine.EUserAuthorityLevel.MASTER)
                            {
                                selectUserLevel(CDefine.EUserAuthorityLevel.OPERATOR);
                            }
                            else
                            {
                                selectUserLevel(++mSelectUserLevel);
                            }
                        }
                        break;
                }
            }
            else if (
                mbLoginVersion2 == false
                && mbMessageDialogShow == false
                )
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        BtnCancel_Click(this, EventArgs.Empty);
                        break;
                    case Keys.Enter:
                        BtnLogin_Click(this, EventArgs.Empty);
                        break;
                }
            }
        }
    }
}
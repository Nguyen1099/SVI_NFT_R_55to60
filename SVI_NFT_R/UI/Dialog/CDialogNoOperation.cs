using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogNoOperation : CFormCommon
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument mDocument;
        private CDefine.ELanguage mLastLanguage = CDefine.ELanguage.LANGUAGE_FINAL;
        private bool mPasswordShow = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogNoOperation(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            mDocument = objDocument;
            InitializeComponent();

            // ! 초기 위치 지정
            Size = PnlKeepCoverOpen.Size;
            PnlKeepCoverOpen.Location = new Point();
            PnlDoNotOperate.Location = new Point();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScale();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // ! Main Frame을 가리도록 크기와 위치를 조정함
            Size = mDocument.GetMainFrame().Size;
            Location = mDocument.GetMainFrame().Location;

            SetImage(bImagePM: false);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible == true)
            {
                ActiveControl = TextPassword;
            }
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
                {
                    break;
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
                SetButtonColor();
                Timer.Start();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public void SetImage(bool bImagePM)
        {
            PnlDoNotOperate.Visible = bImagePM == true;
            PnlKeepCoverOpen.Visible = bImagePM == false;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            SetButtonBackColor(BtnPasswordDisplay, m_colorLabelData);

            SetButtonBackColor(BtnClose, m_colorNormal);

            SetButtonBackColor(BtnLanguage, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(nameof(BtnPasswordDisplay))
                    && false == btn.Name.Equals(nameof(BtnTouch))
                    && false == btn.Name.Equals(nameof(BtnPasswordShow))
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
                SetButtonChangeLanguage(BtnClose);
                SetButtonChangeLanguage(BtnBuzzerOff);

                // 언어 변경
                {
                    mLastLanguage = mDocument.m_objConfig.GetOptionParameter().eLanguage;
                    if (CDefine.ELanguage.LANGUAGE_KOREA == mLastLanguage)
                    {
                        SetButtonText(BtnLanguage, "KOREA");
                    }
                    else if (CDefine.ELanguage.LANGUAGE_CHINA == mLastLanguage)
                    {
                        SetButtonText(BtnLanguage, "CHINA");
                    }
                    else if (CDefine.ELanguage.LANGUAGE_ENGLISH == mLastLanguage)
                    {
                        SetButtonText(BtnLanguage, "ENGLISH");
                    }
                    else if (CDefine.ELanguage.LANGUAGE_VIETNAM == mLastLanguage)
                    {
                        SetButtonText(BtnLanguage, "VIETNAM");
                    }
                }

                // 조작 금지 다국어 및 폰트 크기 맞춤
                {
                    // + Keep Cover Open
                    SetLabelChangeLanguage(LblKeepCoverOpenTitle);
                    SetLabelChangeLanguage(LblKeepCoverOpenDescription1);
                    SetLabelChangeLanguage(LblKeepCoverOpenDescription2);
                    if (mDocument.GetMainFrame().Size == new Size(1920, 1080))
                    {
                        if (mDocument.m_objConfig.GetOptionParameter().eLanguage == CDefine.ELanguage.LANGUAGE_KOREA)
                        {
                            LblKeepCoverOpenTitle.Font = new Font("Cascadia Code", LblKeepCoverOpenTitle.Font.Size, LblKeepCoverOpenTitle.Font.Style);
                            LblKeepCoverOpenDescription1.Font = new Font("Cascadia Code", LblKeepCoverOpenDescription1.Font.Size, LblKeepCoverOpenDescription1.Font.Style);
                            LblKeepCoverOpenDescription2.Font = new Font("Cascadia Code", LblKeepCoverOpenDescription2.Font.Size, LblKeepCoverOpenDescription2.Font.Style);
                        }
                        else
                        {
                            LblKeepCoverOpenTitle.Font = new Font("Microsoft Sans Serif", LblKeepCoverOpenTitle.Font.Size, LblKeepCoverOpenTitle.Font.Style);
                            LblKeepCoverOpenDescription1.Font = new Font("Microsoft Sans Serif", LblKeepCoverOpenDescription1.Font.Size, LblKeepCoverOpenDescription1.Font.Style);
                            LblKeepCoverOpenDescription2.Font = new Font("Microsoft Sans Serif", LblKeepCoverOpenDescription2.Font.Size, LblKeepCoverOpenDescription2.Font.Style);
                        }
                    }
                    else
                    {
                    }

                    // + Do Not Operate
                    SetLabelChangeLanguage(LblDoNotOperateTitle);
                    SetLabelChangeLanguage(LblDoNotOperateDescription1);
                    SetLabelChangeLanguage(LblDoNotOperateDescription2);
                    if (mDocument.GetMainFrame().Size == new Size(1920, 1080))
                    {
                        if (mDocument.m_objConfig.GetOptionParameter().eLanguage == CDefine.ELanguage.LANGUAGE_KOREA)
                        {
                            LblDoNotOperateTitle.Font = new Font("Cascadia Code", LblDoNotOperateTitle.Font.Size, LblDoNotOperateTitle.Font.Style);
                            LblDoNotOperateDescription1.Font = new Font("Cascadia Code", LblDoNotOperateDescription1.Font.Size, LblDoNotOperateDescription1.Font.Style);
                            LblDoNotOperateDescription2.Font = new Font("Cascadia Code", LblDoNotOperateDescription2.Font.Size, LblDoNotOperateDescription2.Font.Style);
                        }
                        else
                        {
                            LblDoNotOperateTitle.Font = new Font("Microsoft Sans Serif", LblDoNotOperateTitle.Font.Size, LblDoNotOperateTitle.Font.Style);
                            LblDoNotOperateDescription1.Font = new Font("Microsoft Sans Serif", LblDoNotOperateDescription1.Font.Size, LblDoNotOperateDescription1.Font.Style);
                            LblDoNotOperateDescription2.Font = new Font("Microsoft Sans Serif", LblDoNotOperateDescription2.Font.Size, LblDoNotOperateDescription2.Font.Style);
                        }
                    }
                    else
                    {
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private void SetButtonChangeLanguage(Button objButton)
        {
            SetButtonText(objButton, mDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            SetButtonText(objButton, mDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetLabelChangeLanguage(Label objLabel)
        {
            string uiText = mDocument.GetDatabaseUIText(objLabel.Name, Name);
            if (uiText != objLabel.Text)
            {
                objLabel.Text = uiText;
            }
        }

        private void CDialogLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 프로그램이 켜져있는 동안 폼 닫지 않음
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus;
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerActionUpdatePassword();
            if (mLastLanguage != mDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                SetChangeLanguage();
            }
        }

        private void TimerActionUpdatePassword()
        {
            if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == mDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
            {
                SetButtonBackColor(BtnBuzzerOff, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnBuzzerOff, m_colorClick);
            }

            if (ActiveControl == TextPassword)
            {
                SetButtonBackColor(BtnPasswordDisplay, m_colorNormal);
                SetButtonBackColor(BtnPasswordShow, m_colorNormal);
                SetButtonBackColor(BtnTouch, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnPasswordDisplay, Color.White);
                SetButtonBackColor(BtnPasswordShow, Color.White);
                SetButtonBackColor(BtnTouch, Color.White);
            }

            StringBuilder passwordString = new StringBuilder();
            if (mPasswordShow == true)
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
            SetButtonText(BtnPasswordDisplay, passwordString.ToString());
        }

        private void BtnLanguage_Click(object sender, EventArgs e)
        {
            CDialogSelectLanguage objDialogSelectLanguage = new CDialogSelectLanguage(mDocument);
            objDialogSelectLanguage.InitializeFormBaseButton((Button)sender);
            objDialogSelectLanguage.Show();
        }

        private void BtnBuzzerOff_Click(object sender, EventArgs e)
        {
            // 알람 없는 경우 
            if (mDocument.GetIsAlarmMessage() == false)
            {
                mDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                return;
            }

            if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == mDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
            {
                mDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
            }
            else
            {
                mDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
            }
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{mDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus()}]");
        }

        private void BtnPasswordShow_MouseDown(object sender, MouseEventArgs e)
        {
            mPasswordShow = true;
        }

        private void BtnPasswordShow_MouseUp(object sender, MouseEventArgs e)
        {
            mPasswordShow = false;
        }

        private void BtnTouch_Click(object sender, EventArgs e)
        {
            TextPassword.Text = "";
            var dialog = new PopupKeyPad(mDocument)
            {
                Value = TextPassword.Text
            };
            dialog.ValueChanged += (s, ev) =>
            {
                TextPassword.Text = dialog.Value;
            };
            dialog.CheckOK += (s, ev) =>
            {
                BtnClose_Click(this, EventArgs.Empty);
            };
            dialog.OkButtonText = BtnClose.Text;
            dialog.InitializeFromBaseControl(BtnPasswordDisplay, ratioX: 3, ratioY: 4);
            dialog.Show();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (mDocument.IsMasterLogin == true)
            {
                Hide();
                TextPassword.Text = "";
                return;
            }

            string password = TextPassword.Text;
            if (string.IsNullOrWhiteSpace(password) == true)
            {
                // Msg: 비밀번호가 없습니다.
                mDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NOT_PASSWORD);
                return;
            }
            if (CDefine.EquipmentPasswordNumbers.Contains(password) == false)
            {
                // Msg: 로그인 정보가 일치하지 않습니다.
                mDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THE_LOGIN_INFORMATION_DOES_NOT_MATCH);
                TextPassword.Select(0, TextPassword.Text.Length);
                return;
            }
            Hide();
            TextPassword.Text = "";
            mDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
        }

        private void BtnPasswordDisplay_Click(object sender, EventArgs e)
        {
            TextPassword.Text = "";
            ActiveControl = TextPassword;
        }

        private void PnlKeepCoverOpenImage_MouseClick(object sender, MouseEventArgs e)
        {
            //SetImage(bImagePM: true);
        }

        private void PnlDoNotOperateImage_MouseClick(object sender, MouseEventArgs e)
        {
            //SetImage(bImagePM: false);
        }

        private void TextPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                case (char)Keys.Space:
                    e.Handled = true;
                    BtnClose_Click(this, EventArgs.Empty);
                    break;

                default:
                    // 숫자와 백스페이스만 허용
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void TextPassword_LostFocus(object sender, EventArgs e)
        {
            // `TextPassword`가 포커스를 잃었을 때, 비밀번호를 초기화하고 다시 포커스를 설정합니다.
            ActiveControl = TextPassword;
            TextPassword.Text = "";
        }
    }
}
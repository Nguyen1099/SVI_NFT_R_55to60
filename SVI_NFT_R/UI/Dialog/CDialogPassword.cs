using System;
using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogPassword : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        private readonly string[] m_strPasswords;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogPassword(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            m_strPasswords = CDefine.EquipmentPasswordNumbers.ToArray();
            InitializeComponent();
        }
        public CDialogPassword(CDocument objDocument, string[] strPasswords)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            m_strPasswords = strPasswords;
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
                // 패스워드는 표시 안되게
                TextPassword.PasswordChar = '＊';
                TextPassword.Text = "";
                // 포커스 Password 버튼으로 이동
                this.ActiveControl = this.TextPassword;
                // 버튼 색상 정의
                SetButtonColor();
                // 언어 변경
                SetChangeLanguage();

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
            SetButtonColor(this.BtnTitleConfirm, System.Drawing.Color.Black, BtnTitleConfirm.BackColor, true);
            base.SetButtonForeColor(this.BtnTitlePassword, System.Drawing.Color.Black);
            base.SetButtonBackColor(this.BtnOk, m_colorNormal);
            base.SetButtonBackColor(this.BtnCancel, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(nameof(BtnTouch))
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
                SetButtonChangeLanguage(this.BtnTitleConfirm);
                SetButtonChangeLanguage(this.BtnTitlePassword);
                SetButtonChangeLanguage(this.BtnOk);
                SetButtonChangeLanguage(this.BtnCancel);

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
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
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
        /// 엔터 누르면 로그인, ESC 누르면 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextPassword_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Space:
                    e.Handled = true;
                    BtnOk_Click(sender, e);
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
            if (Keys.Alt == ModifierKeys || Keys.F4 == ModifierKeys)
            {
                e.Cancel = true;
            }
        }

        private void TextPassword_MouseClick(object sender, MouseEventArgs e)
        {
            TextPassword.Select(0, TextPassword.Text.Length);
        }

        private void TextPassword_Enter(object sender, EventArgs e)
        {
            TextPassword.Select(0, TextPassword.Text.Length);
            SetButtonBackColor(BtnPasswordFocus, System.Drawing.Color.PowderBlue);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
        }


        private void TextID_Leave(object sender, EventArgs e)
        {
            SetButtonBackColor(BtnPasswordFocus, System.Drawing.Color.LightGray);
        }

        /// <summary>
        /// 확인
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            do
            {
                if ("" == TextPassword.Text)
                {
                    // Msg: 비밀번호가 없습니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NOT_PASSWORD);
                    ActiveControl = TextPassword;
                    break;
                }

                if (Array.IndexOf(m_strPasswords, TextPassword.Text) == -1)
                {
                    // Msg: 로그인 정보가 일치하지 않습니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THE_LOGIN_INFORMATION_DOES_NOT_MATCH);
                    ActiveControl = TextPassword;
                    TextPassword.Select(0, TextPassword.Text.Length);
                    break;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}]", "BtnOk_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);

                DialogResult = DialogResult.OK;
                Close();
            } while (false);
        }

        /// <summary>
        /// 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}]", "BtnCancel_Click");
            m_objDocument.SetUpdateButtonLog(this, strLog);
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnTouch_Click(object sender, EventArgs e)
        {
            var dialog = new PopupKeyPad(m_objDocument)
            {
                Value = TextPassword.Text
            };
            dialog.ValueChanged += (s, ev) =>
            {
                TextPassword.Text = dialog.Value;
            };
            dialog.CheckOK += (s, ev) =>
            {
                BtnOk_Click(this, EventArgs.Empty);
            };
            dialog.OkButtonText = BtnOk.Text;
            dialog.InitializeFromBaseControl(PnlKeyPadSize, ratioX: 3, ratioY: 4);
            dialog.Show();
        }
    }
}
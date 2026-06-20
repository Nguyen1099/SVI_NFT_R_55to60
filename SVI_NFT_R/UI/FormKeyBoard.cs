using SVI_NFT_R;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UiAsset
{
    public partial class FormKeyBoard : CFormCommon
    {

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        /// <summary>
        /// Shift 키 눌렀을 때
        /// </summary>
        private bool _bShift;
        public bool m_bShift
        {
            get
            {
                return _bShift;
            }
            set
            {
                _bShift = value;
            }
        }

        /// <summary>
        /// CTRL 키 눌렀을 때
        /// </summary>
        private bool _bCtrl;
        public bool m_bCtrl
        {
            get
            {
                return _bCtrl;
            }
            set
            {
                _bCtrl = value;
            }
        }

        /// <summary>
        /// ALT 키 눌렀을 때
        /// </summary>
        private bool _bAlt;
        public bool m_bAlt
        {
            get
            {
                return _bAlt;
            }
            set
            {
                _bAlt = value;
            }
        }

        /// <summary>
        /// CapLock 키 눌렀을 때
        /// </summary>
        private bool _bCapsLock;
        public bool m_bCapsLock
        {
            get
            {
                return _bCapsLock;
            }
            set
            {
                _bCapsLock = value;
            }
        }

        /// <summary>
        /// Quotes 
        /// </summary>
        private string _bQuotes;
        public string m_bQuotes
        {
            get
            {
                return _bQuotes;
            }
            set
            {
                _bQuotes = value;
            }
        }

        /// <summary>
        /// 키보드 최종 리턴 값
        /// </summary>
        private string _strReturnValue;
        public string m_strReturnValue
        {
            get
            {
                return _strReturnValue;
            }
            set
            {
                _strReturnValue = value;
            }
        }

        private readonly string m_strTitle = "KEY BOARD";
        private readonly string m_strOriginalValue;

        /// <summary>
        /// 키보드 생성자
        /// </summary>
        public FormKeyBoard(string originalValue = "", string strTitle = "")
        {
            if (false == string.IsNullOrEmpty(strTitle))
            {
                m_strTitle = strTitle;
            }
            InitializeComponent();
            m_strOriginalValue = originalValue;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bReturn = false;
            do
            {
                InitializeButton();
                m_bShift = m_bCapsLock = m_bCtrl = m_bAlt = false;
                m_bCapsLock = true;

                m_bQuotes = BtnKeyBoardCharQuotes.Text;
                Reload();
                EditKeyBoardDisplay.Text = m_strOriginalValue;
                EditKeyBoardDisplay.Focus();
                BtnKeyBoardTitle.Text = m_strTitle;

                timer.Interval = 100;
                timer.Enabled = true;
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 버튼 초기화
        /// </summary>
        /// <returns></returns>
        public bool InitializeButton()
        {
            bool bReturn = false;
            do
            {
                SetButtonBackColor(BtnKeyBoardTitle, m_colorLabel);

                var imageButtons = Controls.GetChildControlListByType(typeof(ImageButton));
                foreach (ImageButton btn in imageButtons)
                {
                    SetButtonBackColor(btn, m_colorNormal);
                }

                // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
                var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
                foreach (SpeedButton btn in speedButtons)
                {
                    if (
                        null != btn
                        // 클릭이 기능이 있는 버튼은 스킵함
                        //&& false == btn.Name.Equals(BtnDisplayOriginValue.Name)
                        )
                    {
                        btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                        btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                        btn.BackColorChanged += NonClickableButton_BackColorChanged;
                        btn.Cursor = Cursors.Default;
                    }
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// Shift / CapLock 키 눌렀을때 다시 표시.
        /// </summary>
        private void Reload()
        {
            if (true == m_bShift)
            {
                BtnKeyBoardCharAccent.SetText("~");
                BtnKeyBoardChar0.SetText(")");
                BtnKeyBoardChar1.SetText("!");
                BtnKeyBoardChar2.SetText("@");
                BtnKeyBoardChar3.SetText("#");
                BtnKeyBoardChar4.SetText("$");
                BtnKeyBoardChar5.SetText("%");
                BtnKeyBoardChar6.SetText("^");
                BtnKeyBoardChar7.SetText("&");
                BtnKeyBoardChar8.SetText("*");
                BtnKeyBoardChar9.SetText("(");
                BtnKeyBoardHyphen.SetText("_");
                BtnKeyBoardEqual.SetText("+");
                BtnKeyBoardCharLeftBracket.SetText("{");
                BtnKeyBoardCharRightBracket.SetText("}");
                BtnKeyBoardCharSemicolon.SetText(":");
                BtnKeyBoardCharQuotes.SetText(m_bQuotes);
                BtnKeyBoardCharComma.SetText("<");
                BtnKeyBoardCharPoint.SetText(">");
                BtnKeyBoardCharSlash.SetText("?");
                BtnKeyBoardCharBackSlash.SetText("|");

                if (true == m_bCapsLock)
                {
                    ToLowerKeys();
                }
                else
                {
                    ToUpperKeys();
                }
            }
            else
            {
                BtnKeyBoardCharAccent.SetText("`");
                BtnKeyBoardChar0.SetText("0");
                BtnKeyBoardChar1.SetText("1");
                BtnKeyBoardChar2.SetText("2");
                BtnKeyBoardChar3.SetText("3");
                BtnKeyBoardChar4.SetText("4");
                BtnKeyBoardChar5.SetText("5");
                BtnKeyBoardChar6.SetText("6");
                BtnKeyBoardChar7.SetText("7");
                BtnKeyBoardChar8.SetText("8");
                BtnKeyBoardChar9.SetText("9");
                BtnKeyBoardHyphen.SetText("-");
                BtnKeyBoardEqual.SetText("=");
                BtnKeyBoardCharLeftBracket.SetText("[");
                BtnKeyBoardCharRightBracket.SetText("]");
                BtnKeyBoardCharSemicolon.SetText(";");
                BtnKeyBoardCharQuotes.SetText("'");
                BtnKeyBoardCharComma.SetText(",");
                BtnKeyBoardCharPoint.SetText(".");
                BtnKeyBoardCharSlash.SetText("/");
                BtnKeyBoardCharBackSlash.SetText("\\");

                if (true == m_bCapsLock)
                {
                    ToUpperKeys();
                }
                else
                {
                    ToLowerKeys();
                }
            }
            EditKeyBoardDisplay.Focus();
        }

        private void ToUpperKeys()
        {
            BtnKeyBoardCharQ.SetText(BtnKeyBoardCharQ.Text.ToUpper());
            BtnKeyBoardCharW.SetText(BtnKeyBoardCharW.Text.ToUpper());
            BtnKeyBoardCharE.SetText(BtnKeyBoardCharE.Text.ToUpper());
            BtnKeyBoardCharR.SetText(BtnKeyBoardCharR.Text.ToUpper());
            BtnKeyBoardCharT.SetText(BtnKeyBoardCharT.Text.ToUpper());
            BtnKeyBoardCharY.SetText(BtnKeyBoardCharY.Text.ToUpper());
            BtnKeyBoardCharU.SetText(BtnKeyBoardCharU.Text.ToUpper());
            BtnKeyBoardCharI.SetText(BtnKeyBoardCharI.Text.ToUpper());
            BtnKeyBoardCharO.SetText(BtnKeyBoardCharO.Text.ToUpper());
            BtnKeyBoardCharP.SetText(BtnKeyBoardCharP.Text.ToUpper());
            BtnKeyBoardCharA.SetText(BtnKeyBoardCharA.Text.ToUpper());
            BtnKeyBoardCharS.SetText(BtnKeyBoardCharS.Text.ToUpper());
            BtnKeyBoardCharD.SetText(BtnKeyBoardCharD.Text.ToUpper());
            BtnKeyBoardCharF.SetText(BtnKeyBoardCharF.Text.ToUpper());
            BtnKeyBoardCharG.SetText(BtnKeyBoardCharG.Text.ToUpper());
            BtnKeyBoardCharH.SetText(BtnKeyBoardCharH.Text.ToUpper());
            BtnKeyBoardCharJ.SetText(BtnKeyBoardCharJ.Text.ToUpper());
            BtnKeyBoardCharK.SetText(BtnKeyBoardCharK.Text.ToUpper());
            BtnKeyBoardCharL.SetText(BtnKeyBoardCharL.Text.ToUpper());
            BtnKeyBoardCharZ.SetText(BtnKeyBoardCharZ.Text.ToUpper());
            BtnKeyBoardCharX.SetText(BtnKeyBoardCharX.Text.ToUpper());
            BtnKeyBoardCharC.SetText(BtnKeyBoardCharC.Text.ToUpper());
            BtnKeyBoardCharV.SetText(BtnKeyBoardCharV.Text.ToUpper());
            BtnKeyBoardCharB.SetText(BtnKeyBoardCharB.Text.ToUpper());
            BtnKeyBoardCharN.SetText(BtnKeyBoardCharN.Text.ToUpper());
            BtnKeyBoardCharM.SetText(BtnKeyBoardCharM.Text.ToUpper());
        }

        private void ToLowerKeys()
        {
            BtnKeyBoardCharQ.SetText(BtnKeyBoardCharQ.Text.ToLower());
            BtnKeyBoardCharW.SetText(BtnKeyBoardCharW.Text.ToLower());
            BtnKeyBoardCharE.SetText(BtnKeyBoardCharE.Text.ToLower());
            BtnKeyBoardCharR.SetText(BtnKeyBoardCharR.Text.ToLower());
            BtnKeyBoardCharT.SetText(BtnKeyBoardCharT.Text.ToLower());
            BtnKeyBoardCharY.SetText(BtnKeyBoardCharY.Text.ToLower());
            BtnKeyBoardCharU.SetText(BtnKeyBoardCharU.Text.ToLower());
            BtnKeyBoardCharI.SetText(BtnKeyBoardCharI.Text.ToLower());
            BtnKeyBoardCharO.SetText(BtnKeyBoardCharO.Text.ToLower());
            BtnKeyBoardCharP.SetText(BtnKeyBoardCharP.Text.ToLower());
            BtnKeyBoardCharA.SetText(BtnKeyBoardCharA.Text.ToLower());
            BtnKeyBoardCharS.SetText(BtnKeyBoardCharS.Text.ToLower());
            BtnKeyBoardCharD.SetText(BtnKeyBoardCharD.Text.ToLower());
            BtnKeyBoardCharF.SetText(BtnKeyBoardCharF.Text.ToLower());
            BtnKeyBoardCharG.SetText(BtnKeyBoardCharG.Text.ToLower());
            BtnKeyBoardCharH.SetText(BtnKeyBoardCharH.Text.ToLower());
            BtnKeyBoardCharJ.SetText(BtnKeyBoardCharJ.Text.ToLower());
            BtnKeyBoardCharK.SetText(BtnKeyBoardCharK.Text.ToLower());
            BtnKeyBoardCharL.SetText(BtnKeyBoardCharL.Text.ToLower());
            BtnKeyBoardCharZ.SetText(BtnKeyBoardCharZ.Text.ToLower());
            BtnKeyBoardCharX.SetText(BtnKeyBoardCharX.Text.ToLower());
            BtnKeyBoardCharC.SetText(BtnKeyBoardCharC.Text.ToLower());
            BtnKeyBoardCharV.SetText(BtnKeyBoardCharV.Text.ToLower());
            BtnKeyBoardCharB.SetText(BtnKeyBoardCharB.Text.ToLower());
            BtnKeyBoardCharN.SetText(BtnKeyBoardCharN.Text.ToLower());
            BtnKeyBoardCharM.SetText(BtnKeyBoardCharM.Text.ToLower());
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardEsc_Click(object sender, EventArgs e)
        {
            FormKeyPad.ActiveForm.DialogResult = DialogResult.Cancel;
            FormKeyPad.ActiveForm.Close();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardTAB_Click(object sender, EventArgs e)
        {
            string strbuf, strbuff;
            strbuf = EditKeyBoardDisplay.Text;
            strbuff = string.Format("{0}{1}", strbuf, "    ");
            EditKeyBoardDisplay.Text = strbuff;

            EditKeyBoardDisplay.Select(0, strbuff.Length);
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCapsLock_Click(object sender, EventArgs e)
        {
            if (true == m_bCapsLock)
            {
                m_bCapsLock = false;
            }
            else
            {
                m_bCapsLock = true;
            }
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardLeftShift_Click(object sender, EventArgs e)
        {
            if (true == m_bShift)
            {
                m_bShift = false;
            }
            else
            {
                m_bShift = true;
            }

            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardCtrl_Click(object sender, EventArgs e)
        {
            if (true == m_bCtrl)
            {
                m_bCtrl = false;
            }
            else
            {
                m_bCtrl = true;
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardAlt_Click(object sender, EventArgs e)
        {
            if (true == m_bAlt)
            {
                m_bAlt = false;
            }
            else
            {
                m_bAlt = true;
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardSpace_Click(object sender, EventArgs e)
        {
            EditKeyBoardDisplay.Text += " ";
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardClear_Click(object sender, EventArgs e)
        {
            EditKeyBoardDisplay.Text = "";
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardRightShift_Click(object sender, EventArgs e)
        {
            if (true == m_bShift)
            {
                m_bShift = false;
            }
            else
            {
                m_bShift = true;
            }

            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardEnter_Click(object sender, EventArgs e)
        {
            m_strReturnValue = EditKeyBoardDisplay.Text;
            FormKeyPad.ActiveForm.DialogResult = DialogResult.OK;
            FormKeyPad.ActiveForm.Close();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyBoardBackSpace_Click(object sender, EventArgs e)
        {
            string strConvertText = "";

            if (0 == EditKeyBoardDisplay.Text.Length)
                return;
            if (1 <= EditKeyBoardDisplay.Text.Length)
            {
                strConvertText = EditKeyBoardDisplay.Text.Substring(0, EditKeyBoardDisplay.Text.Length - 1);
                EditKeyBoardDisplay.Text = strConvertText;
                EditKeyBoardDisplay.Focus();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormKeyBoard_Load(object sender, EventArgs e)
        {
            Initialize();
            EditKeyBoardDisplay.Text = m_strOriginalValue;
            EditKeyBoardDisplay.Focus();
        }

        /// <summary>
        /// 키 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormKeyBoard_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    BtnKeyBoardEsc_Click(sender, EventArgs.Empty);
                    break;
                case Keys.Enter:
                    BtnKeyBoardEnter_Click(sender, EventArgs.Empty);
                    break;
            }
        }

        /// <summary>
        /// 타이머 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 캡스락
            if (true == m_bCapsLock)
            {
                SetButtonBackColor(BtnKeyBoardCapsLock, m_colorClick);
            }
            else
            {
                SetButtonBackColor(BtnKeyBoardCapsLock, m_colorNormal);
            }

            // 쉬프트
            if (true == m_bShift)
            {
                SetButtonBackColor(BtnKeyBoardLeftShift, m_colorClick);
                SetButtonBackColor(BtnKeyBoardRightShift, m_colorClick);
            }
            else
            {
                SetButtonBackColor(BtnKeyBoardLeftShift, m_colorNormal);
                SetButtonBackColor(BtnKeyBoardRightShift, m_colorNormal);
            }

            // 컨트롤
            if (true == m_bCtrl)
            {
                SetButtonBackColor(BtnKeyBoardCtrl, m_colorClick);
            }
            else
            {
                SetButtonBackColor(BtnKeyBoardCtrl, m_colorNormal);
            }

            // 알트
            if (true == m_bAlt)
            {
                SetButtonBackColor(BtnKeyBoardAlt, m_colorClick);
            }
            else
            {
                SetButtonBackColor(BtnKeyBoardAlt, m_colorNormal);
            }
        }

        private void BtnKeyBoardChar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ImageButton btn = sender as ImageButton;
                switch (btn.Text.ToUpper())
                {
                    case "A":
                        if (true == m_bCtrl)
                        {
                            m_bCtrl = false;
                            EditKeyBoardDisplay.Focus();
                            EditKeyBoardDisplay.Select(0, EditKeyBoardDisplay.Text.Length);

                        }
                        else
                        {
                            EditKeyBoardDisplay.Text += BtnKeyBoardCharA.Text;
                            EditKeyBoardDisplay.Focus();
                        }
                        break;
                    case "X":
                        if (true == m_bCtrl)
                        {
                            m_bCtrl = false;
                            EditKeyBoardDisplay.Cut();
                        }
                        else
                        {
                            EditKeyBoardDisplay.Text += btn.Text;
                        }
                        break;
                    case "C":
                        if (true == m_bCtrl)
                        {
                            m_bCtrl = false;
                            EditKeyBoardDisplay.Copy();
                        }
                        else
                        {
                            EditKeyBoardDisplay.Text += btn.Text;
                        }
                        break;
                    case "V":
                        if (true == m_bCtrl)
                        {
                            m_bCtrl = false;
                            EditKeyBoardDisplay.Paste();
                        }
                        else
                        {
                            EditKeyBoardDisplay.Text += btn.Text;
                        }
                        break;
                    default:
                        EditKeyBoardDisplay.Text += btn.Text;
                        break;
                }
                EditKeyBoardDisplay.Focus();
            }
        }

        private void BtnKeyBoardBackSpace_MouseUp(object sender, MouseEventArgs e)
        {
            string strConvertText = "";

            if (0 == EditKeyBoardDisplay.Text.Length)
                return;
            if (1 <= EditKeyBoardDisplay.Text.Length)
            {
                strConvertText = EditKeyBoardDisplay.Text.Substring(0, EditKeyBoardDisplay.Text.Length - 1);
                EditKeyBoardDisplay.Text = strConvertText;
                EditKeyBoardDisplay.Focus();
            }
        }

        private void BtnKeyBoardSpace_MouseUp(object sender, MouseEventArgs e)
        {
            EditKeyBoardDisplay.Text += " ";
            EditKeyBoardDisplay.Focus();
        }

        private void BtnKeyBoardChar_Click(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            switch (btn.Text.ToUpper())
            {
                case "A":
                    if (true == m_bCtrl)
                    {
                        m_bCtrl = false;
                        EditKeyBoardDisplay.Focus();
                        EditKeyBoardDisplay.Select(0, EditKeyBoardDisplay.Text.Length);

                    }
                    else
                    {
                        EditKeyBoardDisplay.Text += BtnKeyBoardCharA.Text;
                        EditKeyBoardDisplay.Focus();
                    }
                    break;
                case "X":
                    if (true == m_bCtrl)
                    {
                        m_bCtrl = false;
                        EditKeyBoardDisplay.Cut();
                    }
                    else
                    {
                        EditKeyBoardDisplay.Text += btn.Text;
                    }
                    break;
                case "C":
                    if (true == m_bCtrl)
                    {
                        m_bCtrl = false;
                        EditKeyBoardDisplay.Copy();
                    }
                    else
                    {
                        EditKeyBoardDisplay.Text += btn.Text;
                    }
                    break;
                case "V":
                    if (true == m_bCtrl)
                    {
                        m_bCtrl = false;
                        EditKeyBoardDisplay.Paste();
                    }
                    else
                    {
                        EditKeyBoardDisplay.Text += btn.Text;
                    }
                    break;
                default:
                    EditKeyBoardDisplay.Text += btn.Text;
                    break;
            }
            EditKeyBoardDisplay.Focus();
        }
    }

    internal static class ImageButtonExtendFunction
    {
        public static void SetText(this ImageButton btn, string setText)
        {
            if (
                btn.Text != setText
                || btn.ButtonText != setText
                )
            {
                btn.Text = setText;
                btn.ButtonText = setText;
            }
        }
    }
}

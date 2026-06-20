using SVI_NFT_R;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace UiAsset
{
    public partial class FormKeyPad : CFormCommon
    {

        /// <summary>
        /// 계산 방법
        /// </summary>
        private enum ECalulation
        {
            CAL_NONE = 0,
            CAL_PLUS,
            CAL_MINUS,
            CAL_DEVISION,
            CAL_MULTIPLY
        }

        /// <summary>
        /// 계산 모드
        /// </summary>
        private ECalulation m_eCalulation;
        private double _dValue;
        public double m_dValue
        {
            get
            {
                return _dValue;
            }
            set
            {
                _dValue = value;
            }
        }

        /// <summary>
        /// 바뀌기 전 값
        /// </summary>
        private double _dOriginValue;
        public double m_dOriginValue
        {
            get
            {
                return _dOriginValue;
            }
            set
            {
                _dOriginValue = value;
            }
        }

        /// <summary>
        /// 바뀔 값
        /// </summary>
        private string _strInputData;
        public string m_strInputData
        {
            get
            {
                return _strInputData;
            }
            set
            {
                _strInputData = value;
            }
        }

        /// <summary>
        /// 리턴 값
        /// </summary>
        private double _dResultValue;
        public double m_dResultValue
        {
            get
            {
                return _dResultValue;
            }
            set
            {
                _dResultValue = value;
            }
        }
        /// <summary>
        /// 타이틀 문구
        /// </summary>
        private readonly string m_strTitle = "KEY PAD";
        /// <summary>
        /// 계산기능 사용 여부
        /// </summary>
        public bool m_bCalMode { get; set; }

        public double m_dMaxValue;
        public double m_dMinValue;
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="dOriginValue"> 현재 변경되기 전의 값을 넣어준다.</param>
        public FormKeyPad(double dOriginValue, string strTitle = "")
        {
            InitializeComponent();

            m_dOriginValue = dOriginValue;
            if (false == string.IsNullOrEmpty(strTitle))
            {
                m_strTitle = strTitle;
            }
            m_bCalMode = true;
            m_dMaxValue = m_dMinValue = 0xffffffff;
            // Form 초기화
            Initialize();
        }

        public FormKeyPad(double dOriginValue, double dMinValue, double dMaxValue, string strTitle = "")
        {
            InitializeComponent();

            m_dOriginValue = dOriginValue;
            if (false == string.IsNullOrEmpty(strTitle))
            {
                m_strTitle = strTitle;
            }
            m_bCalMode = true;

            m_dMaxValue = dMaxValue;
            m_dMinValue = dMinValue;
            // Form 초기화
            Initialize();
        }

        public FormKeyPad(int nOriginValue, int nMinValue, int nMaxValue, string strTitle = "")
        {
            InitializeComponent();

            m_dOriginValue = nOriginValue;
            if (false == string.IsNullOrEmpty(strTitle))
            {
                m_strTitle = strTitle;
            }
            m_bCalMode = true;

            m_dMaxValue = nMaxValue;
            m_dMinValue = nMinValue;
            // Form 초기화
            Initialize();
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
                string strOriginValue;
                strOriginValue = string.Format("{0:F3}", m_dOriginValue);
                if (0 != m_dOriginValue)
                {
                    m_strInputData = string.Format("{0:F3}", m_dOriginValue);
                }
                else
                {
                    m_strInputData = "0";
                }

                m_eCalulation = ECalulation.CAL_NONE;

                Reload();

                BtnDisplayOriginValue.Text = strOriginValue;

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
                SetButtonBackColor(BtnKeyPadTitle, m_colorLabel);
                SetButtonBackColor(BtnDisPlayKeyValue, m_colorLabelData);
                SetButtonBackColor(BtnDisplayOriginValue, m_colorLabelData);

                // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
                var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
                foreach (SpeedButton btn in speedButtons)
                {
                    if (
                        null != btn
                        // 클릭이 기능이 있는 버튼은 스킵함
                        && false == btn.Name.Equals(BtnDisplayOriginValue.Name)
                        && false == btn.Name.Equals(BtnKeyPadTitle.Name)
                        && false == btn.Name.Equals(BtnDisPlayKeyValue.Name)
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
        /// 계산 모드에 따라 버튼 이름 및 활성화.
        /// </summary>
        private void Reload()
        {
            if (false == m_bCalMode)
            {
                BtnKeyPadDivision.Enabled = false;
                BtnKeyPadMutiply.Enabled = false;
                BtnKeyPadHyphen.Enabled = false;
                BtnKeyPadPlus.Enabled = false;
                BtnKeyPadEquals.Enabled = false;
            }

            BtnDisPlayKeyValue.Text = m_strInputData;
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadClear_Click(object sender, EventArgs e)
        {
            m_strInputData = "0";
            m_dValue = 0;
            m_eCalulation = ECalulation.CAL_NONE;
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadBackSpace_Click(object sender, EventArgs e)
        {
            if (0 == m_strInputData.Length)
            {
                return;
            }
            if (1 == m_strInputData.Length)
            {
                m_strInputData = "0";
            }
            if (1 < m_strInputData.Length)
            {
                string strConvertText = m_strInputData.Substring(0, m_strInputData.Length - 1);
                m_strInputData = strConvertText;
            }

            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadDivision_Click(object sender, EventArgs e)
        {
            if (false == m_bCalMode)
            {
                return;
            }
            if ("0" == m_strInputData && 0 == m_dValue)
            {
                return;
            }
            double dValue;
            double.TryParse(m_strInputData, out dValue);
            m_dValue = dValue;
            m_eCalulation = ECalulation.CAL_DEVISION;
            m_strInputData = "0";
        }


        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMutiply_Click(object sender, EventArgs e)
        {
            if (false == m_bCalMode)
            {
                return;
            }
            if ("0" == m_strInputData && 0 == m_dValue)
            {
                return;
            }
            double dValue;
            double.TryParse(m_strInputData, out dValue);
            m_dValue = dValue;
            m_eCalulation = ECalulation.CAL_MULTIPLY;
            m_strInputData = "0";
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadHyphen_Click(object sender, EventArgs e)
        {
            if (false == m_bCalMode)
            {
                return;
            }
            if ("0" == m_strInputData && 0 == m_dValue)
            {
                return;
            }
            else
            {
                double dValue;
                double.TryParse(m_strInputData, out dValue);
                m_dValue = dValue;
                m_eCalulation = ECalulation.CAL_MINUS;
                m_strInputData = "0";
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlus_Click(object sender, EventArgs e)
        {
            if (false == m_bCalMode)
            {
                return;
            }
            if ("0" == m_strInputData && 0 == m_dValue)
            {
                return;
            }
            double dValue;
            double.TryParse(m_strInputData, out dValue);
            m_dValue = dValue;
            m_eCalulation = ECalulation.CAL_PLUS;
            m_strInputData = "0";
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadEquals_Click(object sender, EventArgs e)
        {
            double dValue = Convert.ToDouble(m_strInputData);

            switch (m_eCalulation)
            {
                case ECalulation.CAL_PLUS:
                    m_dValue += dValue;
                    break;
                case ECalulation.CAL_MINUS:
                    m_dValue -= dValue;
                    break;
                case ECalulation.CAL_MULTIPLY:
                    m_dValue *= dValue;
                    break;
                case ECalulation.CAL_DEVISION:
                    m_dValue /= dValue;
                    break;
                case ECalulation.CAL_NONE:
                    break;
            }

            m_eCalulation = ECalulation.CAL_NONE;

            m_strInputData = Convert.ToString(m_dValue);

            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadOK_Click(object sender, EventArgs e)
        {
            if (m_eCalulation != ECalulation.CAL_NONE)
            {
                MessageBox.Show("Is Calculating.");
                return;
            }

            double dValue = Convert.ToDouble(m_strInputData);

            if (
                m_dMaxValue != 0xffffffff
                && m_dMinValue != 0xffffffff
                )
            {
                if (
                    dValue > m_dMaxValue
                    || dValue < m_dMinValue
                    )
                {
                    m_strInputData = string.Format("{0:F3}", m_dOriginValue);
                    Reload();
                    MessageBox.Show(string.Format("Out of range MIN = {0}, MAX = {1}", m_dMinValue, m_dMaxValue));
                    return;
                }
            }


            m_dValue = dValue;
            m_strInputData = string.Format("{0:F3}", m_dValue);
            m_dResultValue = m_dValue;
            DialogResult = DialogResult.OK;

            //FormKeyPad.ActiveForm.Close();
            Close();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPoint_Click(object sender, EventArgs e)
        {
            int iIndex = m_strInputData.IndexOf(".");
            if (-1 == iIndex)
            {
                m_strInputData += ".";
                Reload();
            }
            if (m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "0.";
                Reload();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar0_Click(object sender, EventArgs e)
        {
            bool bEquals = m_strInputData.Equals("0");
            if (false == bEquals)
            {
                if (m_strInputData == string.Format("{0:F3}", m_dOriginValue))
                {
                    m_strInputData = "";
                }
                m_strInputData += "0";
                Reload();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar1_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "1";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar2_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "2";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar3_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "3";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar4_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "4";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar5_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "5";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar6_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "6";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar7_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "7";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar8_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "8";
            Reload();
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadChar9_Click(object sender, EventArgs e)
        {
            if (true == m_strInputData.Equals("0") || m_strInputData == string.Format("{0:F3}", m_dOriginValue))
            {
                m_strInputData = "";
            }
            m_strInputData += "9";
            Reload();
        }

        /// <summary>
        /// -0.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMinusDotOne_Click(object sender, EventArgs e)
        {
            m_strInputData = string.Format("{0:F3}", double.Parse(m_strInputData) - 0.1);
            Reload();
        }

        /// <summary>
        /// +0.1 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlusDotOne_Click(object sender, EventArgs e)
        {
            m_strInputData = string.Format("{0:F3}", double.Parse(m_strInputData) + 0.1);
            Reload();
        }

        /// <summary>
        /// -0.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMinusDotFive_Click(object sender, EventArgs e)
        {
            m_strInputData = string.Format("{0:F3}", double.Parse(m_strInputData) - 0.5);
            Reload();
        }

        /// <summary>
        /// +0.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlusDotFive_Click(object sender, EventArgs e)
        {
            m_strInputData = string.Format("{0:F3}", double.Parse(m_strInputData) + 0.5);
            Reload();
        }

        /// <summary>
        /// -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMinusOne_Click(object sender, EventArgs e)
        {
            m_strInputData = string.Format("{0:F3}", double.Parse(m_strInputData) - 1.0);
            Reload();
        }

        /// <summary>
        /// +1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlusOne_Click(object sender, EventArgs e)
        {
            m_strInputData = string.Format("{0:F3}", double.Parse(m_strInputData) + 1.0);
            Reload();
        }

        /// <summary>
        /// -10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadMinusTen_Click(object sender, EventArgs e)
        {
            m_strInputData = string.Format("{0:F3}", double.Parse(m_strInputData) - 10.0);
            Reload();
        }

        /// <summary>
        /// +10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadPlusTen_Click(object sender, EventArgs e)
        {
            m_strInputData = string.Format("{0:F3}", double.Parse(m_strInputData) + 10.0);
            Reload();
        }

        /// <summary>
        /// -/+ 기호 변환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKeyPadMinusPlus_Click(object sender, EventArgs e)
        {
            if ("0" == m_strInputData)
            {
                return;
            }
            else
            {
                m_strInputData = string.Format("{0}", double.Parse(m_strInputData) * -1.0);
                Reload();
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKeyPadCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            //FormKeyPad.ActiveForm.Close();
            Close();
        }

        /// <summary>
        /// 타이머 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            SetButtonText(BtnKeyPadTitle, m_strTitle);
            switch (m_eCalulation)
            {
                case ECalulation.CAL_NONE:
                    SetButtonBackColor(BtnKeyPadDivision, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadMutiply, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadHyphen, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadPlus, Color.LightSteelBlue);
                    break;
                case ECalulation.CAL_PLUS:
                    SetButtonBackColor(BtnKeyPadDivision, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadMutiply, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadHyphen, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadPlus, Color.LightSlateGray);
                    break;
                case ECalulation.CAL_MINUS:
                    SetButtonBackColor(BtnKeyPadDivision, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadMutiply, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadHyphen, Color.LightSlateGray);
                    SetButtonBackColor(BtnKeyPadPlus, Color.LightSteelBlue);
                    break;
                case ECalulation.CAL_DEVISION:
                    SetButtonBackColor(BtnKeyPadDivision, Color.LightSlateGray);
                    SetButtonBackColor(BtnKeyPadMutiply, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadHyphen, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadPlus, Color.LightSteelBlue);
                    break;
                case ECalulation.CAL_MULTIPLY:
                    SetButtonBackColor(BtnKeyPadDivision, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadMutiply, Color.LightSlateGray);
                    SetButtonBackColor(BtnKeyPadHyphen, Color.LightSteelBlue);
                    SetButtonBackColor(BtnKeyPadPlus, Color.LightSteelBlue);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        private void BtnKeyPadChar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ImageButton btn = sender as ImageButton;
                if (
                    true == m_strInputData.Equals("0")
                    || m_strInputData == string.Format("{0:F3}", m_dOriginValue)
                    )
                {
                    m_strInputData = "";
                }
                m_strInputData += btn.ButtonText;
                Reload();
            }
        }

        private void BtnDisplayOriginValue_Click(object sender, EventArgs e)
        {
            m_strInputData = BtnDisplayOriginValue.Text;
            Reload();
        }

        private void BtnKeyPadBackSpace_MouseUp(object sender, MouseEventArgs e)
        {
            if (0 == m_strInputData.Length)
            {
                return;
            }
            if (1 == m_strInputData.Length)
            {
                m_strInputData = "0";
            }
            if (1 < m_strInputData.Length)
            {
                string strConvertText = m_strInputData.Substring(0, m_strInputData.Length - 1);
                m_strInputData = strConvertText;
            }

            Reload();
        }

        private void BtnSetMemory_Click(object sender, EventArgs e)
        {
            BtnDisplayOriginValue.Text = m_strInputData;
        }

        private void BtnKeyPadChar_Click(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            if (
                true == m_strInputData.Equals("0")
                || m_strInputData == string.Format("{0:F3}", m_dOriginValue)
                )
            {
                m_strInputData = "";
            }
            m_strInputData += btn.ButtonText;
            Reload();
        }

        private void BtnDisPlayKeyValue_Click(object sender, EventArgs e)
        {
            copyToClipboard();
        }

        private void BtnKeyPadTitle_Click(object sender, EventArgs e)
        {
            pasteFromClipboard();
        }

        private void FormKeyPad_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                case Keys.Enter:
                    BtnKeyPadOK_Click(sender, EventArgs.Empty);
                    break;
                case Keys.Escape:
                    BtnKeyPadCancel_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad0:
                case Keys.D0:
                    BtnKeyPadChar0_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad1:
                case Keys.D1:
                    BtnKeyPadChar1_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad2:
                case Keys.D2:
                    BtnKeyPadChar2_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad3:
                case Keys.D3:
                    BtnKeyPadChar3_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad4:
                case Keys.D4:
                    BtnKeyPadChar4_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad5:
                case Keys.D5:
                    BtnKeyPadChar5_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad6:
                case Keys.D6:
                    BtnKeyPadChar6_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad7:
                case Keys.D7:
                    BtnKeyPadChar7_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad8:
                case Keys.D8:
                    BtnKeyPadChar8_Click(sender, EventArgs.Empty);
                    break;
                case Keys.NumPad9:
                case Keys.D9:
                    BtnKeyPadChar9_Click(sender, EventArgs.Empty);
                    break;
                case Keys.OemPeriod:
                    BtnKeyPadPoint_Click(sender, EventArgs.Empty);
                    break;
                case Keys.Back:
                    if (true == e.Shift)
                    {
                        BtnKeyPadClear_Click(sender, EventArgs.Empty);
                    }
                    else
                    {
                        BtnKeyPadBackSpace_Click(sender, EventArgs.Empty);
                    }
                    break;
                case Keys.Add:
                    BtnKeyPadPlus_Click(sender, EventArgs.Empty);
                    break;
                case Keys.Subtract:
                    BtnKeyPadHyphen_Click(sender, EventArgs.Empty);
                    break;
                case Keys.Multiply:
                    BtnKeyPadMutiply_Click(sender, EventArgs.Empty);
                    break;
                case Keys.Divide:
                    BtnKeyPadDivision_Click(sender, EventArgs.Empty);
                    break;
            }

            // 복사
            if (
                e.Control == true
                && e.KeyCode == Keys.C
                )
            {
                copyToClipboard();
            }
            // 붙여넣기
            if (
                e.Control == true
                && e.KeyCode == Keys.V
                )
            {
                pasteFromClipboard();
            }
        }

        private void copyToClipboard()
        {
            try
            {
                Clipboard.SetText(m_strInputData);
                DialogResult = DialogResult.Cancel;
            }
            catch
            {
                // + Main Thread가 아닌데서 {Clipboard}에 접근하면 예외가 발생함. 그 상황이면 클립보드 복사 기능 사용 못 함
            }
        }

        private void pasteFromClipboard()
        {
            try
            {
                string clipboardText = Clipboard.GetText();
                double convertValue;
                if (double.TryParse(clipboardText, out convertValue) == true)
                {
                    m_strInputData = string.Format("{0:F3}", convertValue);

                    // + 계산중에 붙여넣기를 하면 계산을 끝내고 결과를 보여줌
                    if (m_eCalulation != ECalulation.CAL_NONE)
                    {
                        BtnKeyPadEquals_Click(this, EventArgs.Empty);
                    }
                    // + 계산중이 아니면 붙여넣기한 값으로 설정함
                    else
                    {
                        BtnKeyPadOK_Click(this, EventArgs.Empty);
                    }
                }
            }
            catch
            {
                // + Main Thread가 아닌데서 {Clipboard}에 접근하면 예외가 발생함. 그 상황이면 클립보드 붙여넣기 기능 사용 못 함
            }
        }
    }
}

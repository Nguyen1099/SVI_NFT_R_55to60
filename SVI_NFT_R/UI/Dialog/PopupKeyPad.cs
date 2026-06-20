using System;
using System.Drawing;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    public partial class PopupKeyPad : CFormCommon
    {
        public string Value { get; set; } = string.Empty;
        public string OkButtonText
        {
            get => BtnOK.Text;
            set => BtnOK.Text = value;
        }
        public EventHandler OnClose;
        public EventHandler CheckOK;
        public EventHandler<string> ValueChanged;
        private readonly CDocument mDocument;

        public PopupKeyPad(CDocument objDocument)
        {
            mDocument = objDocument;
            InitializeComponent();
        }

        public void InitializeFromBaseControl(Control control, int ratioX, int ratioY)
        {
            PnlBase.Size = new Size(control.Width, control.Width * ratioY / ratioX);
            Rectangle rectControl = new Rectangle(control.Location, control.Size);
            Location = control.Parent.PointToScreen(new Point(rectControl.Left, rectControl.Bottom));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Initialize();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            RaiseOnClose();
            DeInitialize();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Size = PnlBase.Size;

            SetControlTextFitting(BtnKey0, 9f, 12f);
            SetControlTextFitting(BtnKey1, 9f, 12f);
            SetControlTextFitting(BtnKey2, 9f, 12f);
            SetControlTextFitting(BtnKey3, 9f, 12f);
            SetControlTextFitting(BtnKey4, 9f, 12f);
            SetControlTextFitting(BtnKey5, 9f, 12f);
            SetControlTextFitting(BtnKey6, 9f, 12f);
            SetControlTextFitting(BtnKey7, 9f, 12f);
            SetControlTextFitting(BtnKey8, 9f, 12f);
            SetControlTextFitting(BtnKey9, 9f, 12f);
            SetControlTextFitting(BtnBackSpace, 9f, 12f);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            // + 폼에 포커스가 사라지면 자동으로 닫는다
            Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    BtnKey0.PerformClick();
                    break;

                case Keys.D1:
                case Keys.NumPad1:
                    BtnKey1.PerformClick();
                    break;

                case Keys.D2:
                case Keys.NumPad2:
                    BtnKey2.PerformClick();
                    break;

                case Keys.D3:
                case Keys.NumPad3:
                    BtnKey3.PerformClick();
                    break;

                case Keys.D4:
                case Keys.NumPad4:
                    BtnKey4.PerformClick();
                    break;

                case Keys.D5:
                case Keys.NumPad5:
                    BtnKey5.PerformClick();
                    break;

                case Keys.D6:
                case Keys.NumPad6:
                    BtnKey6.PerformClick();
                    break;

                case Keys.D7:
                case Keys.NumPad7:
                    BtnKey7.PerformClick();
                    break;

                case Keys.D8:
                case Keys.NumPad8:
                    BtnKey8.PerformClick();
                    break;

                case Keys.D9:
                case Keys.NumPad9:
                    BtnKey9.PerformClick();
                    break;

                case Keys.Back:
                    BtnBackSpace.PerformClick();
                    break;

                case Keys.Escape:
                    Close();
                    break;

                case Keys.Space:
                case Keys.Enter:
                    RaiseCheckOK();
                    Close();
                    break;
            }
        }

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

        private void DeInitialize()
        {
            Timer.Stop();
            ValueChanged = null;
            CheckOK = null;
            OnClose = null;
        }

        private bool InitializeForm()
        {
            // 버튼 색상 정의
            SetButtonColor();

            Timer.Interval = 100;
            Timer.Start();

            return true;
        }

        private void SetButtonColor()
        {
            SetButtonBackColor(BtnClear, Color.MistyRose);
            SetButtonBackColor(BtnBackSpace, Color.MistyRose);

            SetButtonBackColor(BtnKey0, m_colorNormal);
            SetButtonBackColor(BtnKey1, m_colorNormal);
            SetButtonBackColor(BtnKey2, m_colorNormal);
            SetButtonBackColor(BtnKey3, m_colorNormal);
            SetButtonBackColor(BtnKey4, m_colorNormal);
            SetButtonBackColor(BtnKey5, m_colorNormal);
            SetButtonBackColor(BtnKey6, m_colorNormal);
            SetButtonBackColor(BtnKey7, m_colorNormal);
            SetButtonBackColor(BtnKey8, m_colorNormal);
            SetButtonBackColor(BtnKey9, m_colorNormal);

            SetButtonBackColor(BtnOK, Color.LightSteelBlue);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            BtnOK.Visible = CheckOK != null;
        }

        private void RaiseValueChanged(string value)
        {
            ValueChanged?.Invoke(this, value);
        }

        private void RaiseCheckOK()
        {
            CheckOK?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseOnClose()
        {
            OnClose?.Invoke(this, EventArgs.Empty);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Value = string.Empty;
            RaiseValueChanged(Value);
        }

        private void BtnBackSpace_Click(object sender, EventArgs e)
        {
            if (Value.Length == 0)
            {
                return;
            }
            Value = Value.Substring(0, Value.Length - 1);
            RaiseValueChanged(Value);
        }

        private void BtnKey0_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            Value += control.Text;
            RaiseValueChanged(Value);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            RaiseCheckOK();
            Close();
        }
    }
}

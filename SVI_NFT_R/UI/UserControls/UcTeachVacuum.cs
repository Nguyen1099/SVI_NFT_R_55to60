using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    [DefaultEvent(nameof(ToggleAnalogDataView))]
    public partial class UcTeachVacuum : UserControl
    {
        [DefaultValue(false)]
        public bool IsHideDeviceName
        {
            get
            {
                return mbIsHideDeviceName;
            }
            set
            {
                if (mbIsHideDeviceName != value)
                {
                    mbIsHideDeviceName = value;
                    BtnDeviceName.Visible = mbIsHideDeviceName == false;
                    if (mbIsHideDeviceName == true)
                    {
                        PnlLayout.ColumnStyles[0].SizeType = SizeType.Absolute;
                        PnlLayout.ColumnStyles[0].Width = 0f;
                    }
                    else
                    {
                        PnlLayout.ColumnStyles[0].SizeType = SizeType.Percent;
                        PnlLayout.ColumnStyles[0].Width = 13.95f;
                    }
                    PnlLayout.Invalidate();
                }
            }
        }
        private bool mbIsHideDeviceName = false;
        [DefaultValue(false)]
        public bool IsHideSensor
        {
            get
            {
                return mbIsHideSensor;
            }
            set
            {
                if (mbIsHideSensor != value)
                {
                    mbIsHideSensor = value;
                    BtnSensor.Visible = mbIsHideSensor == false;
                    if (mbIsHideSensor == true)
                    {
                        PnlLayout.ColumnStyles[6].SizeType = SizeType.Absolute;
                        PnlLayout.ColumnStyles[6].Width = 0f;
                        PnlLayout.ColumnStyles[7].SizeType = SizeType.Absolute;
                        PnlLayout.ColumnStyles[7].Width = 0f;
                    }
                    else
                    {
                        PnlLayout.ColumnStyles[6].SizeType = SizeType.Percent;
                        PnlLayout.ColumnStyles[6].Width = 13.95f;
                        PnlLayout.ColumnStyles[7].SizeType = SizeType.Absolute;
                        PnlLayout.ColumnStyles[7].Width = 3f;
                    }
                    PnlLayout.Invalidate();
                }
            }
        }
        private bool mbIsHideSensor = false;
        public string DeviceNameText
        {
            get
            {
                return BtnDeviceName.Text;
            }
            set
            {
                SetControlText(BtnDeviceName, value);
            }
        }
        public Color DeviceNameBackColor
        {
            get
            {
                return BtnDeviceName.BackColor;
            }
            set
            {
                SetControlBackColor(BtnDeviceName, value);
            }
        }
        public event EventHandler ToggleAnalogDataView
        {
            add
            {
                LblDisplayAnalog1.Click += value;
                LblDisplayAnalog2.Click += value;
                LblDisplayAnalog3.Click += value;
                LblDisplayAnalog4.Click += value;
                BtnDisplayAnalog.Click += value;
            }
            remove
            {
                LblDisplayAnalog1.Click -= value;
                LblDisplayAnalog2.Click -= value;
                LblDisplayAnalog3.Click -= value;
                LblDisplayAnalog4.Click -= value;
                BtnDisplayAnalog.Click -= value;
            }
        }
        private CDocument mDocument = null;
        private CProcessMotion.EVacuum mVacuumIndex = 0;
        private CDeviceIODefine.EDigitalInput mSensorInput;
        private bool mbUseDetectSensor = false;
        private Form mOwner = null;
        private Func<bool> mIsBlowAutoOff = () => true;
        private Func<bool> mIsAnalogDataView = () => false;
        private Thread mThreadBlow = null;
        private Label[] mAnalogDisplayLabels;
        private readonly Font mAnalogDisplayFont = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);

        public UcTeachVacuum()
        {
            InitializeComponent();
        }

        public void Initialize(Form owner, CDocument document, CProcessMotion.EVacuum vacuumIndex, CDeviceIODefine.EDigitalInput sensorInput, Func<bool> isBlowAutoOff, Func<bool> isAnalogDataView)
        {
            mOwner = owner;
            mDocument = document;
            mVacuumIndex = vacuumIndex;
            mSensorInput = sensorInput;
            mIsBlowAutoOff = isBlowAutoOff;
            mIsAnalogDataView = isAnalogDataView;
            mAnalogDisplayLabels = new Label[]
            {
                LblDisplayAnalog1,
                LblDisplayAnalog2,
                LblDisplayAnalog3,
                LblDisplayAnalog4
            };
            mbUseDetectSensor = true;

            UpdateUI();
        }

        public void Initialize(Form owner, CDocument document, CProcessMotion.EVacuum vacuumIndex, Func<bool> isBlowAutoOff, Func<bool> isAnalogDataView)
        {
            mOwner = owner;
            mDocument = document;
            mVacuumIndex = vacuumIndex;
            mIsBlowAutoOff = isBlowAutoOff;
            mIsAnalogDataView = isAnalogDataView;
            mAnalogDisplayLabels = new Label[]
            {
                LblDisplayAnalog1,
                LblDisplayAnalog2,
                LblDisplayAnalog3,
                LblDisplayAnalog4
            };
            mbUseDetectSensor = false;
            PnlLayout.ColumnStyles[6].SizeType = SizeType.Absolute;
            PnlLayout.ColumnStyles[6].Width = 0;
            PnlLayout.ColumnStyles[7].Width = 0;

            UpdateUI();
        }

        public void UpdateUI()
        {
            if (mDocument == null || GetVacuum() == null)
            {
                return;
            }

            // 진공 상태 표시
            CVacuumAbstract.EVacuumCommand captureCommand = GetVacuum().Command;
            CVacuumAbstract.EVacuumStatus captureStatus = GetVacuum().Status;
            CFormCommon.SetButtonColor(BtnVacuum, BtnVacuum.ForeColor, captureCommand == CVacuumAbstract.EVacuumCommand.CMD_ON ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, captureCommand == CVacuumAbstract.EVacuumCommand.CMD_ON);
            CFormCommon.SetButtonColor(BtnBlow, BtnBlow.ForeColor, captureCommand == CVacuumAbstract.EVacuumCommand.CMD_BLOW ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, captureCommand == CVacuumAbstract.EVacuumCommand.CMD_BLOW);
            CFormCommon.SetButtonColor(BtnDisplayAnalog, BtnDisplayAnalog.ForeColor, captureStatus == CVacuumAbstract.EVacuumStatus.STS_ON ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, true);

            // 센서 표시
            if (mbUseDetectSensor == true)
            {
                bool sensorOn = false;
                mDocument.m_objProcessMain.m_objIO.HLGetDigitalBit(mSensorInput, ref sensorOn);
                SetControlBackColor(BtnSensor, sensorOn == true ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal);
            }

            // Analog Vacuum
            {
                string[] analogInputIoNames = GetVacuum().GetInitializeParameter().strVacuumAnalogInputIO;
                bool bIsAnalogDataView = mIsAnalogDataView.Invoke();
                double getValue = 0d;
                for (int i = 0; i < analogInputIoNames.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(analogInputIoNames[i]) == true)
                    {
                        if (mAnalogDisplayLabels[i].Visible == true)
                        {
                            mAnalogDisplayLabels[i].Visible = false;
                        }
                        continue;
                    }
                    if (mAnalogDisplayLabels[i].Visible == false)
                    {
                        mAnalogDisplayLabels[i].Visible = true;
                    }

                    if (bIsAnalogDataView == true)
                    {
                        if (mDocument.m_objProcessMain.m_objIO.HLGetAnalog(analogInputIoNames[i], ref getValue) == false)
                        {
                            continue;
                        }

                        string displayValue = $"{getValue:0.000}V";
                        if (mAnalogDisplayLabels[i].Text != displayValue)
                        {
                            mAnalogDisplayLabels[i].Text = displayValue;
                        }
                    }
                    else
                    {
                        CDeviceIODefine.EAnalogInput ioIndex = (CDeviceIODefine.EAnalogInput)Enum.Parse(typeof(CDeviceIODefine.EAnalogInput), analogInputIoNames[i]);

                        string displayValue = $"{mDocument.GetVoltageTokPaVacuum(ioIndex):0.0}kPa";
                        if (mAnalogDisplayLabels[i].Text != displayValue)
                        {
                            mAnalogDisplayLabels[i].Text = displayValue;
                        }
                    }

                    bool readValue = false;
                    mDocument.m_objProcessMain.m_objIO.HLGetDigitalBit(GetVacuum().GetInitializeParameter().strVacuumInputIO[i], ref readValue);
                    Color foreColor = readValue ? Color.Black : captureCommand == CVacuumAbstract.EVacuumCommand.CMD_ON ? Color.Red : Color.Black;
                    if (mAnalogDisplayLabels[i].ForeColor != foreColor)
                    {
                        mAnalogDisplayLabels[i].ForeColor = foreColor;
                    }
                    if (mAnalogDisplayLabels[i].BackColor != BtnDisplayAnalog.BaseColor)
                    {
                        mAnalogDisplayLabels[i].BackColor = BtnDisplayAnalog.BaseColor;
                    }
                }
            }
        }

        public void SetChangeLanguage()
        {
            SetControlText(BtnVacuum, mDocument.GetDatabaseUIText($"{Name}_Vacuum", mOwner.Name));
            SetControlText(BtnBlow, mDocument.GetDatabaseUIText($"{Name}_Blow", mOwner.Name));
            SetControlText(BtnSensor, mDocument.GetDatabaseUIText($"{Name}_Sensor", mOwner.Name));
            SetControlText(BtnDeviceName, mDocument.GetDatabaseUIText($"{Name}_DeviceName", mOwner.Name));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // 스케일 변경시에도 아날로그 표시부는 같은 크기를 유지하도록 소스 코드로 강제로 크기, 위치, 폰트를 재설정 함
            SetChildrenSize();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            // 스케일 변경시에도 아날로그 표시부는 같은 크기를 유지하도록 소스 코드로 강제로 크기, 위치, 폰트를 재설정 함
            SetChildrenSize();
        }

        private void SetChildrenSize()
        {
            // 스케일 변경시에도 아날로그 표시부는 같은 크기를 유지하도록 소스 코드로 강제로 크기, 위치, 폰트를 재설정 함
            {
                Rectangle analogDisplayArea = new Rectangle(0, 0, BtnDisplayAnalog.Width - BtnDisplayAnalog.CornerRadius, BtnDisplayAnalog.Height);
                LblDisplayAnalog1.Font
                    = LblDisplayAnalog2.Font
                    = LblDisplayAnalog3.Font
                    = LblDisplayAnalog4.Font
                    = mAnalogDisplayFont;
                LblDisplayAnalog1.Size
                    = LblDisplayAnalog2.Size
                    = LblDisplayAnalog3.Size
                    = LblDisplayAnalog4.Size
                    = new Size(analogDisplayArea.Width / 2 - 2, analogDisplayArea.Height / 2 - 2);
                LblDisplayAnalog1.Location = new Point(1, 1);
                LblDisplayAnalog2.Location = new Point(analogDisplayArea.Width / 2, 1);
                LblDisplayAnalog3.Location = new Point(1, analogDisplayArea.Height / 2 + 1);
                LblDisplayAnalog4.Location = new Point(analogDisplayArea.Width / 2 + 1, analogDisplayArea.Height / 2 + 1);
            }
        }

        private CVacuumAbstract GetVacuum()
        {
            return mDocument?.m_objProcessMain.m_objProcessMotion.m_objVacuum[mVacuumIndex];
        }

        private void SetControlText(Control control, string text)
        {
            if (control.Text != text)
            {
                control.Text = text;
            }
        }

        private void SetControlBackColor(Control control, Color backColor)
        {
            if (control.BackColor != backColor)
            {
                control.BackColor = backColor;
            }
        }

        private async void BtnVacuum_Click(object sender, EventArgs e)
        {
            if (mDocument == null || GetVacuum() == null)
            {
                return;
            }

            if (GetVacuum().Command != CVacuumAbstract.EVacuumCommand.CMD_ON)
            {
                await Task.Factory.StartNew(() => GetVacuum().SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_ON, CVacuumAbstract.ESensorCheck.IGNORE));
                mDocument.SetUpdateButtonLog(mOwner, $"[{MethodBase.GetCurrentMethod().Name}] [{mVacuumIndex}] [Vacuum Command : {CVacuumAbstract.EVacuumCommand.CMD_ON}] [Sensor Ignore : {CVacuumAbstract.ESensorCheck.IGNORE}]");
            }
            else
            {
                GetVacuum().SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
                mDocument.SetUpdateButtonLog(mOwner, $"[{MethodBase.GetCurrentMethod().Name}] [{mVacuumIndex}] [Vacuum Command : {CVacuumAbstract.EVacuumCommand.CMD_OFF}] [Sensor Ignore : {CVacuumAbstract.ESensorCheck.IGNORE}]");
            }
        }

        private async void BtnBlow_Click(object sender, EventArgs e)
        {
            if (mDocument == null || GetVacuum() == null)
            {
                return;
            }

            if (mIsBlowAutoOff?.Invoke() == true)
            {
                if (mThreadBlow == null)
                {
                    mThreadBlow = new Thread(() =>
                    {
                        GetVacuum().SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_BLOW);
                        GetVacuum().SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
                        mThreadBlow = null;
                    });
                    mThreadBlow.Start();
                    mDocument.SetUpdateButtonLog(mOwner, $"[{MethodBase.GetCurrentMethod().Name}] [{mVacuumIndex}] [Vacuum Command : {CVacuumAbstract.EVacuumCommand.CMD_BLOW}] [Sensor Ignore : {CVacuumAbstract.ESensorCheck.IGNORE}]");
                }
            }
            else
            {
                if (GetVacuum().Command != CVacuumAbstract.EVacuumCommand.CMD_BLOW)
                {
                    await Task.Factory.StartNew(() => GetVacuum().SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_BLOW, CVacuumAbstract.ESensorCheck.IGNORE));
                    mDocument.SetUpdateButtonLog(mOwner, $"[{MethodBase.GetCurrentMethod().Name}] [{mVacuumIndex}] [Vacuum Command : {CVacuumAbstract.EVacuumCommand.CMD_BLOW}] [Sensor Ignore : {CVacuumAbstract.ESensorCheck.IGNORE}]");
                }
                else
                {
                    GetVacuum().SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
                    mDocument.SetUpdateButtonLog(mOwner, $"[{MethodBase.GetCurrentMethod().Name}] [{mVacuumIndex}] [Vacuum Command : {CVacuumAbstract.EVacuumCommand.CMD_OFF}] [Sensor Ignore : {CVacuumAbstract.ESensorCheck.IGNORE}]");
                }
            }
        }
    }
}

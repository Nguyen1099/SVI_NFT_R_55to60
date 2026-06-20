using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    public partial class UcTeachCylinder : UserControl
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
                    btnDeviceName.Visible = mbIsHideDeviceName == false;
                    if (mbIsHideDeviceName == true)
                    {
                        pnlLayout.ColumnStyles[0].SizeType = SizeType.Absolute;
                        pnlLayout.ColumnStyles[0].Width = 0f;
                    }
                    else
                    {
                        pnlLayout.ColumnStyles[0].SizeType = SizeType.Percent;
                        pnlLayout.ColumnStyles[0].Width = 30f;
                    }
                    pnlLayout.Invalidate();
                }
            }
        }
        private bool mbIsHideDeviceName = false;
        public string DeviceNameText
        {
            get
            {
                return btnDeviceName.Text;
            }
            set
            {
                setControlText(btnDeviceName, value);
            }
        }
        public Color DeviceNameBackColor
        {
            get
            {
                return btnDeviceName.BackColor;
            }
            set
            {
                setControlBackColor(btnDeviceName, value);
            }
        }
        private CCylinderAbstract mCylinder => mDocument?.m_objProcessMain.m_objProcessMotion.m_objCylinder[mCylinderIndex];
        private CDocument mDocument = null;
        private CProcessMotion.ECylinder mCylinderIndex = 0;
        private Form mOwner = null;
        private Func<CProcessMotion.ECylinder, CCylinderAbstract.ECylinderCommand, bool> mCylinderInterlock;
        private bool bHasSensor = false;

        public UcTeachCylinder()
        {
            InitializeComponent();
        }

        public void Initialize(Form owner, CDocument document, CProcessMotion.ECylinder cylinderIndex, Func<CProcessMotion.ECylinder, CCylinderAbstract.ECylinderCommand, bool> cylinderInterlock)
        {
            mOwner = owner;
            mDocument = document;
            mCylinderIndex = cylinderIndex;
            mCylinderInterlock = cylinderInterlock;
            bHasSensor = mCylinder.HasSensor();
        }

        public void UpdateUI()
        {
            if (mDocument == null || mCylinder == null)
            {
                return;
            }

            if (bHasSensor == true)
            {
                var getStatus = mCylinder.Status;
                var getCommand = mCylinder.Command;
                switch (mCylinder.GetCylinderType())
                {
                    default:
                    case CCylinderAbstract.ECylinderType.UpDown:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_UP == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_UP == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_UP == getStatus);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_DOWN == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_DOWN == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_DOWN == getStatus);
                        break;
                    case CCylinderAbstract.ECylinderType.LeftRigth:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_LEFT == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_LEFT == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_LEFT == getStatus);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_RIGHT == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_RIGHT == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_RIGHT == getStatus);
                        break;
                    case CCylinderAbstract.ECylinderType.ForwardBackward:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_FORWARD == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_FORWARD == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_FORWARD == getStatus);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_BACKWARD == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_BACKWARD == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_BACKWARD == getStatus);
                        break;
                    case CCylinderAbstract.ECylinderType.CwCcw:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_CW == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_CW == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_CW == getStatus);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_CCW == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_CCW == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_CCW == getStatus);
                        break;
                    case CCylinderAbstract.ECylinderType.ReturnTurn:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_RETURN == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_RETURN == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_RETURN == getStatus);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderStatus.STS_TURN == getStatus) ? CFormCommon.m_colorOn
                                                                                     : (CCylinderAbstract.ECylinderCommand.CMD_TURN == getCommand && CCylinderAbstract.ECylinderStatus.STS_UNKNOWN == getStatus) ? CFormCommon.m_colorRed
                                                                                     : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderStatus.STS_TURN == getStatus);
                        break;
                }
            }
            else
            {
                var getCommand = mCylinder.Command;
                switch (mCylinder.GetCylinderType())
                {
                    default:
                    case CCylinderAbstract.ECylinderType.UpDown:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_UP == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_UP == getCommand);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_DOWN == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_DOWN == getCommand);
                        break;
                    case CCylinderAbstract.ECylinderType.LeftRigth:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_LEFT == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_LEFT == getCommand);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_RIGHT == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_RIGHT == getCommand);
                        break;
                    case CCylinderAbstract.ECylinderType.ForwardBackward:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_FORWARD == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_FORWARD == getCommand);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_BACKWARD == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_BACKWARD == getCommand);
                        break;
                    case CCylinderAbstract.ECylinderType.CwCcw:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_CW == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_CW == getCommand);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_CCW == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_CCW == getCommand);
                        break;
                    case CCylinderAbstract.ECylinderType.ReturnTurn:
                        CFormCommon.SetButtonColor(btnAction1, btnAction1.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_RETURN == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_RETURN == getCommand);
                        CFormCommon.SetButtonColor(btnAction2, btnAction2.ForeColor, (CCylinderAbstract.ECylinderCommand.CMD_TURN == getCommand) ? CFormCommon.m_colorOn : CFormCommon.m_colorNormal, CCylinderAbstract.ECylinderCommand.CMD_TURN == getCommand);
                        break;
                }
            }
        }

        public void SetChangeLanguage()
        {
            setControlText(btnAction1, mDocument.GetDatabaseUIText($"{Name}_Action1", mOwner.Name));
            setControlText(btnAction2, mDocument.GetDatabaseUIText($"{Name}_Action2", mOwner.Name));
            setControlText(btnDeviceName, mDocument.GetDatabaseUIText($"{Name}_DeviceName", mOwner.Name));
        }

        private void setControlText(Control control, string text)
        {
            if (control.Text != text)
            {
                control.Text = text;
            }
        }

        private void setControlBackColor(Control control, Color backColor)
        {
            if (control.BackColor != backColor)
            {
                control.BackColor = backColor;
            }
        }

        private void btnAction1_Click(object sender, EventArgs e)
        {
            var setCmd = default(CCylinderAbstract.ECylinderCommand);
            switch (mCylinder.GetCylinderType())
            {
                default:
                case CCylinderAbstract.ECylinderType.UpDown:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_UP;
                    break;
                case CCylinderAbstract.ECylinderType.LeftRigth:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_LEFT;
                    break;
                case CCylinderAbstract.ECylinderType.ForwardBackward:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_FORWARD;
                    break;
                case CCylinderAbstract.ECylinderType.CwCcw:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_CW;
                    break;
                case CCylinderAbstract.ECylinderType.ReturnTurn:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_RETURN;
                    break;
            }
            if (mCylinderInterlock?.Invoke(mCylinderIndex, setCmd) == false)
            {
                return;
            }
            mCylinder.SetCylinderCommand(setCmd, CCylinderAbstract.ESensorCheck.IGNORE);
            mDocument.SetUpdateButtonLog(mOwner, $"[{MethodBase.GetCurrentMethod().Name}] [{mCylinderIndex}] [Cylinder Command : {setCmd}] [Sensor Ignore : {CCylinderAbstract.ESensorCheck.IGNORE}]");
        }

        private void btnAction2_Click(object sender, EventArgs e)
        {
            var setCmd = default(CCylinderAbstract.ECylinderCommand);
            switch (mCylinder.GetCylinderType())
            {
                default:
                case CCylinderAbstract.ECylinderType.UpDown:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_DOWN;
                    break;
                case CCylinderAbstract.ECylinderType.LeftRigth:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_RIGHT;
                    break;
                case CCylinderAbstract.ECylinderType.ForwardBackward:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_BACKWARD;
                    break;
                case CCylinderAbstract.ECylinderType.CwCcw:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_CCW;
                    break;
                case CCylinderAbstract.ECylinderType.ReturnTurn:
                    setCmd = CCylinderAbstract.ECylinderCommand.CMD_TURN;
                    break;
            }
            if (mCylinderInterlock?.Invoke(mCylinderIndex, setCmd) == false)
            {
                return;
            }
            mCylinder.SetCylinderCommand(setCmd, CCylinderAbstract.ESensorCheck.IGNORE);
            mDocument.SetUpdateButtonLog(mOwner, $"[{MethodBase.GetCurrentMethod().Name}] [{mCylinderIndex}] [Cylinder Command : {setCmd}] [Sensor Ignore : {CCylinderAbstract.ESensorCheck.IGNORE}]");
        }
    }
}

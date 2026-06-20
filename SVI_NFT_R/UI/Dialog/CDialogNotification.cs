using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R.UI.Dialog
{
    public partial class CDialogNotification : CFormCommon, CFormInterface
    {
        public CAlarmDefine.EMessageList MessageIndex => mMessageIndex;
        public object[] MessageArgs => mMessageArgs;
        public bool IsVisibleTimeShow { get; set; } = false;
        public bool ShouldBeHiden { get; set; } = true;
        private readonly CDocument mDocument;
        private bool mbIsDragWindow = false;
        private Point mDragWindowStartOffset;
        private CAlarmDefine.EMessageList mMessageIndex;
        private object[] mMessageArgs = new object[0];
        private DateTime mVisibleStartDateTime = DateTime.Now;
        private string mResourceElapsed = "Elapsed: ";

        public CDialogNotification(CDocument document)
        {
            mDocument = document;

            InitializeComponent();

            PnlDisplayArea.Location = new Point(1, 1);
            Width = PnlDisplayArea.Width + 2;
            Height = PnlDisplayArea.Height + 2;
            Width += PnlDisplayArea.Width - DisplayRectangle.Width + 2;
            Height += PnlDisplayArea.Height - DisplayRectangle.Height + 2;
        }

        public void SetTimer(bool bTimer)
        {
            if (bTimer == true)
            {
                UpdateUI();
            }

            timer.Enabled = bTimer;
        }

        public void SetVisible(bool bVisible)
        {
        }

        public bool SetChangeLanguage()
        {
            SetButtonText(BtnTitle, mDocument.GetDatabaseUIText(BtnTitle.Name, Name));
            mResourceElapsed = mDocument.GetDatabaseUIText(nameof(mResourceElapsed), Name);

            if (mMessageIndex != 0)
            {
                try
                {
                    CManagerTable objManagerTable = mDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUserMessage;
                    int index = (int)mDocument.m_objConfig.GetOptionParameter().eLanguage;
                    var languageIndex = new[]
                    {
                        new { Alarm = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_KOREA, AlarmAction = (int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_KOREA, Message = (int)CDatabaseDefine.EInformationUserMessage.TEXT_KOREA },
                        new { Alarm = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_CHINA, AlarmAction = (int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_CHINA, Message = (int)CDatabaseDefine.EInformationUserMessage.TEXT_CHINA },
                        new { Alarm = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_ENGLISH, AlarmAction = (int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_ENGLISH, Message = (int)CDatabaseDefine.EInformationUserMessage.TEXT_ENGLISH },
                        new { Alarm = (int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_VIETNAM, AlarmAction = (int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_VIETNAM, Message = (int)CDatabaseDefine.EInformationUserMessage.TEXT_VIETNAM },
                    };
                    DataRow[] objDataRow;
                    DataTable objDataTable;
                    objDataTable = objManagerTable.HLGetDataTable();
                    objDataRow = objDataTable.Select(string.Format("{0} = '{1}'", objManagerTable.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationUserMessage.ID], (int)mMessageIndex));
                    string format = objDataRow[0].ItemArray[languageIndex[index].Message].ToString().Replace("\\n", Environment.NewLine);
                    SetControlText(LblContent, string.Format(format, Resource.GetAll(mMessageArgs)));
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            }

            FittingUi();

            return true;
        }

        public void CenterToOwner()
        {
            if (Owner == null)
            {
                return;
            }

            Location = new Point(Owner.Bounds.Width / 2 - Width / 2, Owner.Bounds.Height / 2 - Height / 2);
        }

        public void SetMessage(CAlarmDefine.EMessageList messageIndex, params object[] args)
        {
            if (Visible == false)
            {
                mVisibleStartDateTime = DateTime.Now;
            }
            if (mMessageIndex == messageIndex
                && mMessageArgs.Length == args.Length
                && Enumerable.SequenceEqual(mMessageArgs, args) == true
                )
            {
                return;
            }
            mMessageIndex = messageIndex;
            mMessageArgs = args;
            SetChangeLanguage();
        }

        public void CloseDialog()
        {
            SetTimer(false);
            Hide();
        }

        private void FittingUi()
        {
            PnlDisplayArea.Location = new Point(1, 1);
            Width = PnlDisplayArea.Width + 2;
            Height = PnlDisplayArea.Height + 2;
            Width += PnlDisplayArea.Width - DisplayRectangle.Width + 2;
            Height += PnlDisplayArea.Height - DisplayRectangle.Height + 2;
            ImgDisplay.ZoomToFit();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScale();
            Initialize();
        }

        protected override void OnClosed(EventArgs e)
        {
            DeInitialize();
            base.OnClosed(e);
        }

        private bool Initialize()
        {
            // 폼 초기화
            if (InitializeForm() == false)
            {
                return false;
            }
            return true;
        }

        private void DeInitialize()
        {
            SetTimer(false);
        }

        private bool InitializeForm()
        {
            // 폼 중앙에서 생성
            CenterToParent();
            // 버튼 색상 정의
            SetButtonColor();
            // 언어 변경
            SetChangeLanguage();
            // 타이머 제어
            timer.Interval = 100;

            return true;
        }

        private void SetButtonColor()
        {
            BackColor = Color.OrangeRed;
            SetButtonBackColor(BtnTitle, Color.OrangeRed);
            SetButtonForeColor(BtnTitle, Color.White);
            SetButtonBackColor(BtnVisibleElapsed, Color.WhiteSmoke);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(nameof(BtnTitle))
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            SetControlVisible(BtnVisibleElapsed, IsVisibleTimeShow);
            if (IsVisibleTimeShow == true)
            {
                SetButtonText(BtnVisibleElapsed, $"{mResourceElapsed}{(DateTime.Now - mVisibleStartDateTime).TotalSeconds,5:0.0} sec");
            }
        }

        private void BtnTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mDragWindowStartOffset = new Point(e.X, e.Y);
                mbIsDragWindow = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                CenterToOwner();
            }
        }

        private void BtnTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (mbIsDragWindow == false)
            {
                return;
            }

            int x = e.X;
            int y = e.Y;
            Point setPoint = Owner.PointToClient(PointToScreen(new Point(x - mDragWindowStartOffset.X, y - mDragWindowStartOffset.Y)));
            if (setPoint.X < 0)
            {
                setPoint.X = 0;
            }
            else if (setPoint.X > Owner.Bounds.Width - Width)
            {
                setPoint.X = Owner.Bounds.Width - Width;
            }
            if (setPoint.Y < 0)
            {
                setPoint.Y = 0;
            }
            else if (setPoint.Y > Owner.Bounds.Height - Height)
            {
                setPoint.Y = Owner.Bounds.Height - Height;
            }
            Location = setPoint;
        }

        private void BtnTitle_MouseUp(object sender, MouseEventArgs e)
        {
            mbIsDragWindow = false;
        }

        private void TxbContent_Enter(object sender, EventArgs e)
        {
            // ! TxbContent로 포커스가 생기지 않도록하는 트릭
            ImgDisplay.Focus();
        }
    }
}

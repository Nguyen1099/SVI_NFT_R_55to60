using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UiAsset
{
    public partial class FormFlagSelect : Form
    {
        public string[] ResultNames { get; private set; } = new string[0];
        public int[] ResultIndexs { get; private set; } = new int[0];
        public string TitleText
        {
            get
            {
                return lblTitle.Text;
            }
            set
            {
                if (lblTitle.Text != value)
                {
                    lblTitle.Text = value;
                }
            }
        }
        public string SelectText
        {
            get
            {
                return btnSelect.Text;
            }
            set
            {
                if (btnSelect.Text != value)
                {
                    btnSelect.Text = value;
                }
            }
        }
        public string CancelText
        {
            get
            {
                return btnCancel.Text;
            }
            set
            {
                if (btnCancel.Text != value)
                {
                    btnCancel.Text = value;
                }
            }
        }
        private Color mSelectColor = Color.Gold;
        private Color mNonSelectColor = Color.FromArgb(230, 230, 230);
        private readonly HashSet<int> mSelectItems = new HashSet<int>();
        private readonly HashSet<string> mOriginalItemNames;
        private readonly List<ImageButton> mEnumButtons = new List<ImageButton>();
        private readonly string[] mEnumNames;
        private readonly HashSet<string> mIgnoreItemOrNull;

        public FormFlagSelect(string[] enumNames, string[] initialItemNames, HashSet<string> ignoreItemsOrNull = null)
        {
            InitializeComponent();

            mOriginalItemNames = new HashSet<string>(initialItemNames);
            mEnumNames = enumNames;
            mIgnoreItemOrNull = ignoreItemsOrNull;

            makeButtons();
        }

        protected override void OnShown(EventArgs e)
        {
            timer.Enabled = true;

            base.OnShown(e);
        }

        private void makeButtons()
        {
            pnlContent.SuspendLayout();
            SuspendLayout();

            ImageButton beforeButton = btnFirstItem;
            ImageButton newButton = btnFirstItem;
            var currentFormSize = Size;
            for (int i = 0; i < mEnumNames.Length; i++)
            {
                if (
                    mIgnoreItemOrNull != null
                    && mIgnoreItemOrNull.Contains(mEnumNames[i]) == true
                    )
                {
                    continue;
                }
                if (mEnumButtons.Count > 0)
                {
                    newButton = new ImageButton();
                    currentFormSize.Height += beforeButton.Size.Height + 6;
                    var currentSideBarSize = pnlItemSideBar.Size;
                    currentSideBarSize.Height += beforeButton.Size.Height + 6;
                    pnlItemSideBar.Size = currentSideBarSize;
                    var location = beforeButton.Location;
                    location.Offset(0, beforeButton.Size.Height + 6);
                    newButton.Location = location;

                    // AutoScroll 옵션으로 Scroll이 생성 될 때 아래쪽에 마진을 주기 위한 트릭
                    location.Offset(0, beforeButton.Size.Height);
                    pnlBottomSpace.Location = location;
                }
                newButton.Name = $"btnItem{mEnumNames[i]}";
                newButton.CornerRound = beforeButton.CornerRound;
                newButton.Anchor = beforeButton.Anchor;
                newButton.Size = beforeButton.Size;
                newButton.Font = beforeButton.Font;
                newButton.ButtonText = mEnumNames[i];
                newButton.TextAlign = beforeButton.TextAlign;
                newButton.Click += btnItem_Click;
                newButton.Tag = i;
                if (mOriginalItemNames.Contains(mEnumNames[i]) == true)
                {
                    mSelectItems.Add(i);
                }
                pnlContent.Controls.Add(newButton);
                mEnumButtons.Add(newButton);
                beforeButton = newButton;
            }

            pnlContent.ResumeLayout(false);
            ResumeLayout(false);
            Size = currentFormSize;

            // 창을 띄우기 전에 초기값 기준으로 버튼 색을 변경함
            timer_Tick(timer, EventArgs.Empty);
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(((ImageButton)sender).Tag);

            // Select Toggle
            if (mSelectItems.Contains(index) == true)
            {
                mSelectItems.Remove(index);
            }
            else
            {
                mSelectItems.Add(index);
            }
        }

        private void FormFlagSelect_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                    SelectToggleFromIndex(0);
                    break;

                case Keys.D2:
                    SelectToggleFromIndex(1);
                    break;

                case Keys.D3:
                    SelectToggleFromIndex(2);
                    break;

                case Keys.D4:
                    SelectToggleFromIndex(3);
                    break;

                case Keys.D5:
                    SelectToggleFromIndex(4);
                    break;

                case Keys.D6:
                    SelectToggleFromIndex(5);
                    break;

                case Keys.D7:
                    SelectToggleFromIndex(6);
                    break;

                case Keys.D8:
                    SelectToggleFromIndex(7);
                    break;

                case Keys.D9:
                    SelectToggleFromIndex(8);
                    break;

                case Keys.Space:
                case Keys.Enter:
                    MakeResult();
                    DialogResult = DialogResult.OK;
                    break;

                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void SelectToggleFromIndex(int index)
        {
            if (mEnumNames.Length < (index + 1))
            {
                return;
            }

            // Select Toggle
            if (mSelectItems.Contains(index) == true)
            {
                mSelectItems.Remove(index);
            }
            else
            {
                mSelectItems.Add(index);
            }
        }

        private void MakeResult()
        {
            ResultIndexs = mSelectItems.ToArray();
            ResultNames = mSelectItems.Select(i => mEnumNames[i]).ToArray();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            MakeResult();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            foreach (ImageButton button in mEnumButtons)
            {
                int index = Convert.ToInt32(button.Tag);
                setImageButtonBaseColor(button, mSelectItems.Contains(index) == true ? mSelectColor : mNonSelectColor);
            }
        }

        private void setImageButtonBaseColor(ImageButton button, Color color)
        {
            if (button.BaseColor != color)
            {
                button.BaseColor = color;
            }
        }
    }
}
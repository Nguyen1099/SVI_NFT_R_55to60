using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UiAsset
{
    public partial class FormEnumSelect : Form
    {
        public string ResultName { get; private set; } = string.Empty;
        public int ResultIndex { get; private set; } = -1;
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
        private readonly string mOriginalItemName;
        private readonly List<ImageButton> mEnumButtons = new List<ImageButton>();
        private readonly string[] mEnumNames;
        private readonly HashSet<string> mIgnoreItemOrNull;

        public FormEnumSelect(string[] enumNames, string originalItemName = "", HashSet<string> ignoreItemsOrNull = null)
        {
            InitializeComponent();

            mOriginalItemName = originalItemName;
            mEnumNames = enumNames;
            mIgnoreItemOrNull = ignoreItemsOrNull;

            makeButtons();
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
                if (mOriginalItemName == mEnumNames[i])
                {
                    newButton.BaseColor = Color.Gold;
                }
                pnlContent.Controls.Add(newButton);
                mEnumButtons.Add(newButton);
                beforeButton = newButton;
            }

            pnlContent.ResumeLayout(false);
            ResumeLayout(false);
            Size = currentFormSize;
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ResultName = ((ImageButton)sender).ButtonText;
            ResultIndex = Convert.ToInt32(((ImageButton)sender).Tag);
            DialogResult = DialogResult.OK;
        }

        private void FormEnumSelect_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                    SelectResultFromIndex(0);
                    break;

                case Keys.D2:
                    SelectResultFromIndex(1);
                    break;

                case Keys.D3:
                    SelectResultFromIndex(2);
                    break;

                case Keys.D4:
                    SelectResultFromIndex(3);
                    break;

                case Keys.D5:
                    SelectResultFromIndex(4);
                    break;

                case Keys.D6:
                    SelectResultFromIndex(5);
                    break;

                case Keys.D7:
                    SelectResultFromIndex(6);
                    break;

                case Keys.D8:
                    SelectResultFromIndex(7);
                    break;

                case Keys.D9:
                    SelectResultFromIndex(8);
                    break;

                case Keys.Space:
                case Keys.Enter:
                    if (string.IsNullOrWhiteSpace(mOriginalItemName) == true)
                    {
                        DialogResult = DialogResult.Cancel;
                        break;
                    }
                    ResultName = mOriginalItemName;
                    ResultIndex = Array.FindIndex(mEnumNames, i => i == mOriginalItemName);
                    DialogResult = DialogResult.OK;
                    break;

                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void SelectResultFromIndex(int index)
        {
            if (mEnumNames.Length < (index + 1))
            {
                return;
            }
            ResultIndex = index;
            ResultName = mEnumNames[ResultIndex];
            DialogResult = DialogResult.OK;
        }
    }
}
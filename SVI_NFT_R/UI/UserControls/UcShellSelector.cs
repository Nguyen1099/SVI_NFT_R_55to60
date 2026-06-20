using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UiAsset;


namespace SVI_NFT_R
{
    public partial class UcShellSelector : UserControl
    {
        private readonly Font mDisplayFont = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        public class EventArgsSelect : EventArgs
        {
            public int PocketIndex { get; set; }
            public bool IsSelected { get; set; }
        }

        public int PocketRow
        {
            get { return mPocketRow; }
            set
            {
                bool mChanged = mPocketRow != value;
                mPocketRow = value;
                Utils.InRange(ref mPocketRow, 1, 5);
                if (mChanged) resize();
            }
        }
        public int PocketColumn
        {
            get { return mPocketColumn; }
            set
            {
                bool mChanged = mPocketColumn != value;
                mPocketColumn = value;
                Utils.InRange(ref mPocketColumn, 1, 5);
                if (mChanged) resize();
            }
        }
        public Padding PocketMargin
        {
            get { return mPocketMargin; }
            set
            {
                mPocketMargin = value;
                for (int i = 0; i < mPocketButtons.Length; i++)
                {
                    mPocketButtons[i].Margin = mPocketMargin;
                }
                resize();
            }
        }
        public Color BackgroundColor
        {
            get { return pnlBase.BackColor; }
            set
            {
                if (pnlBase.BackColor != value)
                {
                    BackColor = value;
                    pnlBase.BackColor = value;
                }
            }
        }
        public event EventHandler<EventArgsSelect> PocketSelectChanged;
        private int mPocketRow = 4;
        private int mPocketColumn = 2;
        private Padding mPocketMargin;
        private SpeedButton[] mPocketButtons;
        private bool[] mPocketSelected;

        public UcShellSelector()
        {
            InitializeComponent();

            mPocketButtons = Controls
                .GetChildControlListByType(typeof(SpeedButton))
                .Cast<SpeedButton>()
                .ToArray();

            mPocketMargin = mPocketButtons[0].Margin;

            mPocketSelected = new bool[mPocketButtons.Length];

            resize();
        }

        public void SetButtonText(int pocketIndex, string text)
        {
            if (mPocketButtons[pocketIndex].Text != text)
            {
                mPocketButtons[pocketIndex].Text = text;
            }
        }

        public void SetButtonForeColor(int pocketIndex, Color color)
        {
            if (mPocketButtons[pocketIndex].ForeColor != color)
            {
                mPocketButtons[pocketIndex].ForeColor = color;
            }
        }

        public void SetButtonBackColor(int pocketIndex, Color color)
        {
            if (mPocketButtons[pocketIndex].BackColor != color)
            {
                mPocketButtons[pocketIndex].BackColor = color;
            }
        }

        public void SetButtonSelected(int pocketIndex, bool selected) => mPocketSelected[pocketIndex] = selected;

        public bool IsButtonSelected(int pocketIndex) => mPocketSelected[pocketIndex];

        public void ClearButtonSelection()
        {
            for (int i = 0; i < mPocketSelected.Length; i++)
            {
                mPocketSelected[i] = false;
            }
        }


        private void resize()
        {
            int buttonWidth = (pnlBase.Width - pnlBase.Padding.Left - pnlBase.Padding.Right) / mPocketColumn - (mPocketButtons[0].Margin.Left + mPocketButtons[0].Margin.Right);
            int buttonHeight = (pnlBase.Height - pnlBase.Padding.Top - pnlBase.Padding.Bottom) / mPocketRow - (mPocketButtons[0].Margin.Top + mPocketButtons[0].Margin.Bottom);
            int maxLength = mPocketRow * mPocketColumn;
            for (int i = 0; i < mPocketButtons.Length; i++)
            {
                mPocketButtons[i].Width = buttonWidth;
                mPocketButtons[i].Height = buttonHeight;
                mPocketButtons[i].Visible = i < maxLength;
            }
        }

        private void BtnPocket_Click(object sender, EventArgs e)
        {
            var btn = sender as SpeedButton;
            int pocketIndex = Convert.ToInt32(btn.Tag);
            var args = new EventArgsSelect() { PocketIndex = pocketIndex, IsSelected = IsButtonSelected(pocketIndex) };
            PocketSelectChanged?.Invoke(this, args);
            SetButtonSelected(args.PocketIndex, args.IsSelected);
        }

        private void UcShellSelector_Resize(object sender, EventArgs e)
        {
            resize();
        }

    }
}

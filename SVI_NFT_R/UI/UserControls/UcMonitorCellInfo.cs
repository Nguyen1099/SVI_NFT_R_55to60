using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R.UI.UserControls
{
    public partial class UcMonitorCellInfo : UserControl
    {
        public SpeedButton[] CellButtons { get; private set; }

        public UcMonitorCellInfo(int cellCount)
        {
            InitializeComponent();
            CreateCellButtons(cellCount);
            FittingWidth();
        }

        private void CreateCellButtons(int count)
        {
            CellButtons = Enumerable.Range(1, count)
                .Select(i => new SpeedButton() { Name = $"BtnP{i}", Text = $"P{i}", Size = BtnCellSample.Size, Margin = BtnCellSample.Margin, Font = BtnCellSample.Font, ForeColor = BtnCellSample.ForeColor, BackColor = BtnCellSample.BackColor })
                .ToArray();

            foreach (var button in CellButtons)
            {
                PnlBase.Controls.Add(button);
            }
        }

        private void FittingWidth()
        {
            int widthSum = 0;
            foreach (Control control in PnlBase.Controls)
            {
                if (control.Visible == false)
                {
                    continue;
                }
                widthSum += control.Width + control.Margin.Left + control.Margin.Right;
            }
            Width = widthSum;
        }
    }
}

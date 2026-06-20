using System;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class UcTrayModelData : UserControl
    {
        public class EventArgsSelect : EventArgs
        {
            public string EventCode { get; set; }
            public int PoketIndex { get; set; }
        }

        public string Title
        {
            get { return lblPoketName.Text; }
            set { if (lblPoketName.Text != value) lblPoketName.Text = value; }
        }

        public string Label
        {
            get { return btnTitleLabelValue.Text; }
            set { if (btnTitleLabelValue.Text != value) btnTitleLabelValue.Text = value; }
        }

        public int PoketIndex
        {
            get { return nPoketIndex; }
            set
            {
                if (nPoketIndex != value) nPoketIndex = value;
                lblPoketIndex.Text = string.Format("{0}", nPoketIndex);
            }
        }

        public string PoketOffsetX
        {
            get { return btnTitleOffsetXValue.Text; }
            set { if (btnTitleOffsetXValue.Text != value) btnTitleOffsetXValue.Text = value; }
        }

        public string PoketOffsetY
        {
            get { return btnTitleOffsetYValue.Text; }
            set { if (btnTitleOffsetYValue.Text != value) btnTitleOffsetYValue.Text = value; }
        }

        public string PoketOffsetT
        {
            get { return btnTitleOffsetTValue.Text; }
            set { if (btnTitleOffsetTValue.Text != value) btnTitleOffsetTValue.Text = value; }
        }

        public event EventHandler<EventArgsSelect> PocketSelectChanged;
        private int nPoketIndex;

        public UcTrayModelData()
        {
            InitializeComponent();
        }

        private void BtnOffsetX_Click(object sender, EventArgs e)
        {
            var btn = sender as SpeedButton;
            var args = new EventArgsSelect() { EventCode = "X", PoketIndex = nPoketIndex };
            PocketSelectChanged?.Invoke(this, args);
        }

        private void BtnOffsetY_Click(object sender, EventArgs e)
        {
            var btn = sender as SpeedButton;
            var args = new EventArgsSelect() { EventCode = "Y", PoketIndex = nPoketIndex };
            PocketSelectChanged?.Invoke(this, args);
        }

        private void BtnOffsetT_Click(object sender, EventArgs e)
        {
            var btn = sender as SpeedButton;
            var args = new EventArgsSelect() { EventCode = "T", PoketIndex = nPoketIndex };
            PocketSelectChanged?.Invoke(this, args);
        }

        private void BtnLabel_Click(object sender, EventArgs e)
        {
            var btn = sender as SpeedButton;
            var args = new EventArgsSelect() { EventCode = "LABEL", PoketIndex = nPoketIndex };
            PocketSelectChanged?.Invoke(this, args);
        }
    }
}

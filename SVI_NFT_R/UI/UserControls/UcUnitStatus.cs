using System.ComponentModel;
using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    public partial class UcUnitStatus : UserControl
    {
        [Category("Code")]
        public string Title
        {
            get
            {
                return LblTitle.Text;
            }
            set
            {
                LblTitle.Text = value;
            }
        }
        public new Padding Margin
        {
            get
            {
                return base.Margin;
            }
            set
            {
                base.Margin = value;
                PnlTitleAvailabilityBase.Margin = value;
                PnlAvailability.Margin = value;
                PnlTitleInterlockBase.Margin = value;
                PnlInterlockBase.Margin = value;
                PnlTitleMoveBase.Margin = value;
                PnlMoveBase.Margin = value;
                PnlTitleRunBase.Margin = value;
                PnlRunBase.Margin = value;
                LblTitle.Margin = value;
                BtnUnitUse.Margin = value;
                BtnUnitPause.Margin = value;
                Refresh();
            }
        }

        public UcUnitStatus()
        {
            InitializeComponent();
        }
    }
}

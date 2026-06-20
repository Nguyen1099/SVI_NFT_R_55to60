using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    public partial class UcSettingAlign : UserControl
    {
        public new object Tag
        {
            set
            {
                base.Tag = value;
                BtnInterlockX.Tag = value;
                BtnInterlockY.Tag = value;
                BtnInterlockT.Tag = value;
                BtnInterlockScore.Tag = value;
                BtnSelectModeUseVision.Tag = value;
                BtnRetryCount.Tag = value;
                BtnToleranceX.Tag = value;
                BtnToleranceY.Tag = value;
                BtnToleranceT.Tag = value;
            }
            get
            {
                return base.Tag;
            }
        }

        public new string Text
        {
            get
            {
                return BtnTitle.Text;
            }
            set
            {
                if (BtnTitle.Text != value)
                {
                    BtnTitle.Text = value;
                }
            }
        }

        public UcSettingAlign()
        {
            InitializeComponent();
        }
    }
}

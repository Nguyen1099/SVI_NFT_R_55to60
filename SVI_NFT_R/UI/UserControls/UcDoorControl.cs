using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    public partial class UcDoorControl : UserControl
    {
        public new object Tag
        {
            get
            {
                return base.Tag;
            }
            set
            {
                base.Tag = value;
                BtnTitle.Tag = value;
                BtnDoorOpen.Tag = value;
                BtnDoorLock.Tag = value;
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

        public bool HasOutsideKey
        {
            set
            {
                int setValue = value ? 0 : -1;
                if (BtnTitle.ImageIndex != setValue)
                {
                    BtnTitle.ImageIndex = setValue;
                }
            }
        }

        public UcDoorControl()
        {
            InitializeComponent();
        }
    }
}

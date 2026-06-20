using System.Drawing;
using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    public partial class UcDoorStatus : UserControl
    {
        public string TitleText
        {
            get { return btnTitle.Text; }
            set
            {
                if (btnTitle.Text != value)
                {
                    btnTitle.Text = value;
                }
            }
        }

        public Color TitleForeColor
        {
            get { return btnTitle.ForeColor; }
            set
            {
                if (btnTitle.ForeColor != value)
                {
                    btnTitle.ForeColor = value;
                }
            }
        }

        public Color TitleBackColor
        {
            get { return btnTitle.BackColor; }
            set
            {
                if (btnTitle.BackColor != value)
                {
                    btnTitle.BackColor = value;
                }
            }
        }

        public Font TitleFont
        {
            get { return btnTitle.Font; }
            set
            {
                if (btnTitle.Font.Equals(value) == false)
                {
                    btnTitle.Font = value;
                }
            }
        }

        public string MessageText
        {
            get { return btnMessage.Text; }
            set
            {
                if (btnMessage.Text != value)
                {
                    btnMessage.Text = value;
                }
            }
        }

        public Color MessageForeColor
        {
            get { return btnMessage.ForeColor; }
            set
            {
                if (btnMessage.ForeColor != value)
                {
                    btnMessage.ForeColor = value;
                }
            }
        }

        public Color MessageBackColor
        {
            get { return btnMessage.BackColor; }
            set
            {
                if (btnMessage.BackColor != value)
                {
                    btnMessage.BackColor = value;
                }
            }
        }

        public Font MessageFont
        {
            get { return btnMessage.Font; }
            set
            {
                if (btnMessage.Font.Equals(value) == false)
                {
                    btnMessage.Font = value;
                }
            }
        }

        public UcDoorStatus()
        {
            InitializeComponent();
        }
    }
}

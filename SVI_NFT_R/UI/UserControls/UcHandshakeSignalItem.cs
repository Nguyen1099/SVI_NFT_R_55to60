using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    public partial class UcHandshakeSignalItem : UserControl
    {
        [Category("Setting")]
        [DefaultValue(0.1)]
        [Description("인디게이터 두께 (비율)")]
        public float IndicatorThicknessRatio
        {
            get
            {
                return mIndicatorThicknessRatio;
            }
            set
            {
                if (mIndicatorThicknessRatio != value)
                {
                    mIndicatorThicknessRatio = value;
                    updateIndicatorSize();
                }
            }
        }
        private float mIndicatorThicknessRatio = 0.1f;

        [Category("Setting")]
        [DefaultValue("SIGNAL NAME")]
        [Description("인디게이터 텍스트")]
        public string Title
        {
            get
            {
                return LblTitle.Text;
            }
            set
            {
                if (LblTitle.Text != value)
                {
                    LblTitle.Text = value;
                }
            }
        }

        public new Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = LblTitle.Font = value;
            }
        }

        public UcHandshakeSignalItem()
        {
            InitializeComponent();
            updateIndicatorSize();
        }

        private void updateIndicatorSize()
        {
            PnlLayout.ColumnStyles[0].Width = PnlLayout.Width * mIndicatorThicknessRatio;
            PnlLayout.ColumnStyles[2].Width = PnlLayout.Width * mIndicatorThicknessRatio;

            PnlLayout.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            updateIndicatorSize();
        }
    }
}

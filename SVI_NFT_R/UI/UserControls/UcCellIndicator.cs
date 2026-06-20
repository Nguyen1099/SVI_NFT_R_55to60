using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    [DefaultEvent(nameof(Click))]
    public partial class UcCellIndicator : UserControl
    {
        [Category("Setting")]
        [DefaultValue(true)]
        [Description("상단에 배치")]
        public bool PlaceTopIndicator
        {
            get
            {
                return mPlaceTop;
            }
            set
            {
                mPlaceTop = value;
                UpdateIndicatorSize();
            }
        }
        private bool mPlaceTop = true;

        [Category("Setting")]
        [DefaultValue(true)]
        [Description("하단에 배치")]
        public bool PlaceBottomIndicator
        {
            get
            {
                return mPlaceBottom;
            }
            set
            {
                mPlaceBottom = value;
                UpdateIndicatorSize();
            }
        }
        private bool mPlaceBottom = true;

        [Category("Setting")]
        [DefaultValue(true)]
        [Description("좌측에 배치")]
        public bool PlaceLeftIndicator
        {
            get
            {
                return mPlaceLeft;
            }
            set
            {
                mPlaceLeft = value;
                UpdateIndicatorSize();
            }
        }
        private bool mPlaceLeft = true;

        [Category("Setting")]
        [DefaultValue(true)]
        [Description("우측에 배치")]
        public bool PlaceRightIndicator
        {
            get
            {
                return mPlaceRight;
            }
            set
            {
                mPlaceRight = value;
                UpdateIndicatorSize();
            }
        }
        private bool mPlaceRight = true;

        [Category("Setting")]
        [DefaultValue(5)]
        [Description("인디케이터와 셀데이터 사이에 간격")]
        public int IndicatorGap
        {
            get
            {
                return mIndicatorGap;
            }
            set
            {
                if (mIndicatorGap != value)
                {
                    mIndicatorGap = value;
                    UpdateIndicatorSize();
                }
            }
        }
        private int mIndicatorGap = 5;

        [Category("Setting")]
        [DefaultValue(10)]
        [Description("인디케이터 두께")]
        public int IndicatorThickness
        {
            get
            {
                return mIndicatorThickness;
            }
            set
            {
                if (mIndicatorThickness != value)
                {
                    mIndicatorThickness = value;
                    UpdateIndicatorSize();
                }
            }
        }
        private int mIndicatorThickness = 10;

        [Category("Setting")]
        [DefaultValue("1")]
        [Description("셀데이터 택스트")]
        public string Title
        {
            get
            {
                return BtnCellData.Text;
            }
            set
            {
                if (BtnCellData.Text != value)
                {
                    BtnCellData.Text = value;
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
                base.Font = BtnCellData.Font = value;
            }
        }

        public new event EventHandler Click
        {
            add
            {
                BtnCellData.Click += value;
            }
            remove
            {
                BtnCellData.Click -= value;
            }
        }

        public new object Tag
        {
            get
            {
                return BtnCellData.Tag;
            }
            set
            {
                BtnCellData.Tag = value;
            }
        }

        [Category("Setting Sensor Iamge")]
        public bool IsSensorOn
        {
            get
            {
                return mbIsSensorOn;
            }
            set
            {
                if (mbIsSensorOn == value)
                {
                    return;
                }
                mbIsSensorOn = value;
                BtnCellData.Invalidate();
            }
        }
        private bool mbIsSensorOn = false;

        [Category("Setting Sensor Iamge")]
        public Image SensorOnImage
        {
            get
            {
                return mSensorOnImage;
            }
            set
            {
                mSensorOnImage = value;
                BtnCellData.Invalidate();
            }
        }
        private Image mSensorOnImage = null;

        [Category("Setting Sensor Iamge")]
        public Image SensorOffImage
        {
            get
            {
                return mSensorOffImage;
            }
            set
            {
                mSensorOffImage = value;
                BtnCellData.Invalidate();
            }
        }
        private Image mSensorOffImage = null;

        [Category("Setting Sensor Iamge")]
        public Size SensorImageSize
        {
            get
            {
                return mSensorImageSize;
            }
            set
            {
                mSensorImageSize = value;
                BtnCellData.Invalidate();
            }
        }
        private Size mSensorImageSize = new Size(32, 32);

        [Category("Setting Sensor Iamge")]
        public Padding SensorImageMargin
        {
            get
            {
                return mSensorImageMargin;
            }
            set
            {
                mSensorImageMargin = value;
                BtnCellData.Invalidate();
            }
        }
        private Padding mSensorImageMargin = new Padding(3);

        [Category("Setting Sensor Iamge")]
        public ContentAlignment SensorImageAlign
        {
            get
            {
                return mSensorImageAlign;
            }
            set
            {
                mSensorImageAlign = value;
                BtnCellData.Invalidate();
            }
        }
        private ContentAlignment mSensorImageAlign = ContentAlignment.TopLeft;

        public UcCellIndicator()
        {
            InitializeComponent();
            UpdateIndicatorSize();
        }

        private void UpdateIndicatorSize()
        {
            BtnIndicatorTop.Visible = PlaceTopIndicator;
            BtnIndicatorBottom.Visible = PlaceBottomIndicator;
            BtnIndicatorLeft.Visible = PlaceLeftIndicator;
            BtnIndicatorRight.Visible = PlaceRightIndicator;

            // Column 설정
            pnlLayout.ColumnStyles[0].Width = PlaceLeftIndicator ? mIndicatorThickness : 0;  // Left Indicator 두께
            pnlLayout.ColumnStyles[1].Width = PlaceLeftIndicator ? mIndicatorGap : 0;        // Left Gap
            pnlLayout.ColumnStyles[3].Width = PlaceRightIndicator ? mIndicatorGap : 0;       // Right Gap
            pnlLayout.ColumnStyles[4].Width = PlaceRightIndicator ? mIndicatorThickness : 0; // Right Indicator 두께

            // Row 설정
            pnlLayout.RowStyles[0].Height = PlaceTopIndicator ? mIndicatorThickness : 0;     // Top Indicator
            pnlLayout.RowStyles[1].Height = PlaceTopIndicator ? mIndicatorGap : 0;           // Gap
            pnlLayout.RowStyles[3].Height = mIndicatorGap;                          // Gap
            pnlLayout.RowStyles[5].Height = mIndicatorGap;                          // Gap
            pnlLayout.RowStyles[7].Height = PlaceBottomIndicator ? mIndicatorGap : 0;        // Gap
            pnlLayout.RowStyles[8].Height = PlaceBottomIndicator ? mIndicatorThickness : 0;  // Bottom Indicator

            pnlLayout.Invalidate();
        }

        private void BtnCellData_Paint(object sender, PaintEventArgs e)
        {
            var status = e.Graphics.Save();
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            if (IsSensorOn == true
                && SensorOnImage != null
                )
            {
                e.Graphics.DrawImage(SensorOnImage, GetSensorImageRectangle());
            }
            else if (IsSensorOn == false
                && SensorOffImage != null
                )
            {
                e.Graphics.DrawImage(SensorOffImage, GetSensorImageRectangle());
            }
            e.Graphics.Restore(status);
        }

        private Rectangle GetSensorImageRectangle()
        {
            const int BASE_X = 1;
            const int BASE_Y = 1;
            int clientWidth = BtnCellData.Width;
            int clientHeight = BtnCellData.Height;
            int left = 0;
            switch (SensorImageAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    left = BASE_X + SensorImageMargin.Left;
                    break;

                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    left = (clientWidth / 2) - (SensorImageSize.Width / 2);
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    left = clientWidth - (BASE_X + SensorImageMargin.Right + SensorImageSize.Width);
                    break;
            }
            int top = 0;
            switch (SensorImageAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    top = BASE_Y + SensorImageMargin.Top;
                    break;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    top = (clientHeight / 2) - (SensorImageSize.Height / 2);
                    break;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    top = clientHeight - (BASE_Y + SensorImageMargin.Bottom + SensorImageSize.Height);
                    break;
            }
            return new Rectangle(left, top, SensorImageSize.Width, SensorImageSize.Height);
        }
    }
}

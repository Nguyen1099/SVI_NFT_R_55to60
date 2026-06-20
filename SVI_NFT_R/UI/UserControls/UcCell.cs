using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SVI_NFT_R.UI.UserControls
{
    [DefaultEvent(nameof(Click))]
    public partial class UcCell : UserControl
    {
        public enum ERotate
        {
            R0 = 0,
            R90,
            R180,
            R270,
        }

        public enum EPlaceHolder
        {
            LeftTop = 0,
            RightTop,
            LeftBottom,
            RightBottom
        }

        [Category("Setting")]
        [DefaultValue(EPlaceHolder.LeftTop)]
        [Description("회전시 고정점")]
        public EPlaceHolder PlaceHolder
        {
            get
            {
                return mPlaceHolder;
            }
            set
            {
                mPlaceHolder = value;
            }
        }
        private EPlaceHolder mPlaceHolder = EPlaceHolder.LeftTop;

        [Category("Setting")]
        [DefaultValue(ERotate.R0)]
        [Description("시계 방향으로 회전")]
        public ERotate Rotate
        {
            get
            {
                return mRotate;
            }
            set
            {
                if (mRotate != value)
                {
                    var loc = Location;
                    switch (mPlaceHolder)
                    {
                        case EPlaceHolder.RightTop:
                            loc.Offset(Width, 0);
                            break;

                        case EPlaceHolder.LeftBottom:
                            loc.Offset(0, Height);
                            break;

                        case EPlaceHolder.RightBottom:
                            loc.Offset(Width, Height);
                            break;

                        case EPlaceHolder.LeftTop:
                        default:
                            break;
                    }

                    switch (value)
                    {
                        case ERotate.R0:
                            {
                                pnlLayout.SetColumn(BtnIndicator, 0);
                                pnlLayout.SetRow(BtnIndicator, 0);
                                pnlLayout.SetColumnSpan(BtnIndicator, 1);
                                pnlLayout.SetRowSpan(BtnIndicator, 5);
                                pnlLayout.SetColumn(pnlCellData, 2);
                                pnlLayout.SetRow(pnlCellData, 0);
                                pnlLayout.SetColumnSpan(pnlCellData, 3);
                                pnlLayout.SetRowSpan(pnlCellData, 5);
                            }
                            break;
                        case ERotate.R90:
                            {
                                pnlLayout.SetColumn(BtnIndicator, 0);
                                pnlLayout.SetRow(BtnIndicator, 0);
                                pnlLayout.SetColumnSpan(BtnIndicator, 5);
                                pnlLayout.SetRowSpan(BtnIndicator, 1);
                                pnlLayout.SetColumn(pnlCellData, 0);
                                pnlLayout.SetRow(pnlCellData, 2);
                                pnlLayout.SetColumnSpan(pnlCellData, 5);
                                pnlLayout.SetRowSpan(pnlCellData, 3);
                            }
                            break;
                        case ERotate.R180:
                            {
                                pnlLayout.SetColumn(BtnIndicator, 4);
                                pnlLayout.SetRow(BtnIndicator, 0);
                                pnlLayout.SetColumnSpan(BtnIndicator, 1);
                                pnlLayout.SetRowSpan(BtnIndicator, 5);
                                pnlLayout.SetColumn(pnlCellData, 0);
                                pnlLayout.SetRow(pnlCellData, 0);
                                pnlLayout.SetColumnSpan(pnlCellData, 3);
                                pnlLayout.SetRowSpan(pnlCellData, 5);
                            }
                            break;
                        case ERotate.R270:
                            {
                                pnlLayout.SetColumn(BtnIndicator, 0);
                                pnlLayout.SetRow(BtnIndicator, 4);
                                pnlLayout.SetColumnSpan(BtnIndicator, 5);
                                pnlLayout.SetRowSpan(BtnIndicator, 1);
                                pnlLayout.SetColumn(pnlCellData, 0);
                                pnlLayout.SetRow(pnlCellData, 0);
                                pnlLayout.SetColumnSpan(pnlCellData, 5);
                                pnlLayout.SetRowSpan(pnlCellData, 3);
                            }
                            break;
                    }
                    pnlLayout.Invalidate();
                    mRotate = value;
                    updateLogicalSize();

                    switch (mPlaceHolder)
                    {
                        case EPlaceHolder.RightTop:
                            loc.Offset(-Width, 0);
                            break;

                        case EPlaceHolder.LeftBottom:
                            loc.Offset(0, -Height);
                            break;

                        case EPlaceHolder.RightBottom:
                            loc.Offset(-Width, -Height);
                            break;

                        case EPlaceHolder.LeftTop:
                        default:
                            break;
                    }
                    if (Equals(Location, loc) == false)
                    {
                        Location = loc;
                    }
                }
                mRotate = value;
            }
        }
        private ERotate mRotate = ERotate.R0;
        private bool mShouldUpdateSize = true;

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
                    updateIndicatorSize();
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
                    updateIndicatorSize();
                }
            }
        }
        private int mIndicatorThickness = 10;

        public bool UseLogicalSizing
        {
            get
            {
                return mbUseLogicalSizing;
            }
            set
            {
                if (mbUseLogicalSizing != value)
                {
                    mbUseLogicalSizing = value;
                    updateLogicalSize();
                }
            }
        }
        private bool mbUseLogicalSizing = false;

        [Category("Setting")]
        [Description("R0 기준에서 논리적인 너비")]
        public int LogicalWidth
        {
            get
            {
                return mLogicalWidth;
            }
            set
            {
                if (mLogicalWidth != value)
                {
                    switch (mRotate)
                    {
                        case ERotate.R0:
                        case ERotate.R180:
                            Width = value;
                            break;
                        case ERotate.R90:
                        case ERotate.R270:
                            Height = value;
                            break;
                    }
                    updateLogicalSize();
                }
                mLogicalWidth = value;
            }
        }
        private int mLogicalWidth = 10;

        [Category("Setting")]
        [Description("R0 기준에서 논리적인 높이")]
        public int LogicalHeight
        {
            get
            {
                return mLogicalHeight;
            }
            set
            {
                if (mLogicalHeight != value)
                {
                    switch (mRotate)
                    {
                        case ERotate.R0:
                        case ERotate.R180:
                            Height = value;
                            break;
                        case ERotate.R90:
                        case ERotate.R270:
                            Width = value;
                            break;
                    }
                    updateLogicalSize();
                }
                mLogicalHeight = value;
            }
        }
        private int mLogicalHeight = 10;

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

        public UcCell()
        {
            InitializeComponent();
            updateIndicatorSize();
            OnResize(EventArgs.Empty);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (mShouldUpdateSize)
            {
                switch (mRotate)
                {
                    case ERotate.R0:
                    case ERotate.R180:
                        mLogicalWidth = Width;
                        mLogicalHeight = Height;
                        break;
                    case ERotate.R90:
                    case ERotate.R270:
                        mLogicalWidth = Height;
                        mLogicalHeight = Width;
                        break;
                }
            }
        }

        private void updateLogicalSize()
        {
            mShouldUpdateSize = false;
            if (mbUseLogicalSizing == true)
            {
                switch (mRotate)
                {
                    case ERotate.R0:
                    case ERotate.R180:
                        Width = mLogicalWidth;
                        Height = mLogicalHeight;
                        break;
                    case ERotate.R90:
                    case ERotate.R270:
                        Height = mLogicalWidth;
                        Width = mLogicalHeight;
                        break;
                }
            }
            mShouldUpdateSize = true;
        }

        private void updateIndicatorSize()
        {
            pnlLayout.ColumnStyles[1].Width = mIndicatorGap;
            pnlLayout.ColumnStyles[3].Width = mIndicatorGap;
            pnlLayout.RowStyles[1].Height = mIndicatorGap;
            pnlLayout.RowStyles[3].Height = mIndicatorGap;

            pnlLayout.ColumnStyles[0].Width = mIndicatorThickness;
            pnlLayout.ColumnStyles[4].Width = mIndicatorThickness;
            pnlLayout.RowStyles[0].Height = mIndicatorThickness;
            pnlLayout.RowStyles[4].Height = mIndicatorThickness;

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

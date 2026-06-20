using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace UiAsset
{
    [DesignerCategory("code")]
    public class LineBetweenControls : Component
    {
        [Category("Appearance")]
        public Control LineBeginControl
        {
            get
            {
                return mLineBeginControl;
            }
            set
            {
                if (mLineBeginControl != null)
                {
                    mLineBeginControl.Move -= LineEdgeControl_Move;
                    mLineBeginControl.Resize -= LineEdgeControl_Resize;
                }
                if (value != null)
                {
                    value.Move += LineEdgeControl_Move;
                    value.Resize += LineEdgeControl_Resize;
                }
                mLineBeginControl = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public Control LineEndControl
        {
            get
            {
                return mLineEndControl;
            }
            set
            {
                if (mLineEndControl != null)
                {
                    mLineEndControl.Move -= LineEdgeControl_Move;
                    mLineEndControl.Resize -= LineEdgeControl_Resize;
                }
                if (value != null)
                {
                    value.Move += LineEdgeControl_Move;
                    value.Resize += LineEdgeControl_Resize;
                }
                mLineEndControl = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public Control DrawControl
        {
            get
            {
                return mDrawControl;
            }
            set
            {
                if (mDrawControl != null)
                {
                    mDrawControl.Paint -= DrawControl_Paint;
                }
                if (value != null)
                {
                    value.Paint += DrawControl_Paint;
                }
                mDrawControl = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public Color LineColor
        {
            get
            {
                return mLineColor;
            }
            set
            {
                mLineColor = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public float LineWidth
        {
            get
            {
                return mLineWidth;
            }
            set
            {
                mLineWidth = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public int LineBeginMargin
        {
            get
            {
                return mLineBeginMargin;
            }
            set
            {
                mLineBeginMargin = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public int LineEndMargin
        {
            get
            {
                return mLineEndMargin;
            }
            set
            {
                mLineEndMargin = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public int LineBeginOffset
        {
            get
            {
                return mLineBeginOffset;
            }
            set
            {
                mLineBeginOffset = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public int LineEndOffset
        {
            get
            {
                return mLineEndOffset;
            }
            set
            {
                mLineEndOffset = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public LineCap LineBeginCap
        {
            get
            {
                return mLineBeginCap;
            }
            set
            {
                mLineBeginCap = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public LineCap LineEndCap
        {
            get
            {
                return mLineEndCap;
            }
            set
            {
                mLineEndCap = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public ContentAlignment LineBeginAttachPosition
        {
            get
            {
                return mLineBeginAttachPosition;
            }
            set
            {
                mLineBeginAttachPosition = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public ContentAlignment LineEndAttachPosition
        {
            get
            {
                return mLineEndAttachPosition;
            }
            set
            {
                mLineEndAttachPosition = value;
                Invalidate();
            }
        }
        [Category("Appearance")]
        public bool LineVisible
        {
            get
            {
                return mLineVisible;
            }
            set
            {
                mLineVisible = value;
                Invalidate();
            }
        }
        private Control mLineBeginControl = null;
        private Control mLineEndControl = null;
        private Control mDrawControl = null;
        private ContentAlignment mLineBeginAttachPosition = ContentAlignment.MiddleCenter;
        private ContentAlignment mLineEndAttachPosition = ContentAlignment.MiddleCenter;
        private Color mLineColor = Color.FromArgb(230, Color.Red);
        private float mLineWidth = 4f;
        private int mLineBeginMargin = 1;
        private int mLineEndMargin = 1;
        private int mLineBeginOffset = 20;
        private int mLineEndOffset = 20;
        private LineCap mLineBeginCap = LineCap.Round;
        private LineCap mLineEndCap = LineCap.ArrowAnchor;
        private bool mLineVisible = true;
        private Pen mPen;

        public LineBetweenControls()
        {
        }

        public LineBetweenControls(IContainer container)
            : this()
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            container.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (mLineBeginControl != null)
            {
                mLineBeginControl.Move -= LineEdgeControl_Move;
                mLineBeginControl.Resize -= LineEdgeControl_Resize;
            }
            if (mLineEndControl != null)
            {
                mLineEndControl.Move -= LineEdgeControl_Move;
                mLineEndControl.Resize -= LineEdgeControl_Resize;
            }
            if (mDrawControl != null)
            {
                mDrawControl.Paint -= DrawControl_Paint;
            }
            try
            {
                Invalidate();
            }
            catch
            {
            }

            base.Dispose(disposing);
        }

        private void LineEdgeControl_Move(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void LineEdgeControl_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void DrawControl_Paint(object sender, PaintEventArgs e)
        {
            if (mLineVisible == false)
            {
                return;
            }

            Point[] points = new Point[4];
            int startIndex = 0;
            if (TryGetLineAttachPoints(mLineBeginControl, mLineBeginAttachPosition, mLineBeginMargin, mLineBeginOffset, true, points, ref startIndex) == false)
            {
                return;
            }
            if (TryGetLineAttachPoints(mLineEndControl, mLineEndAttachPosition, mLineEndMargin, mLineEndOffset, false, points, ref startIndex) == false)
            {
                return;
            }

            GraphicsState graphicsState = e.Graphics.Save();
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.InterpolationMode = InterpolationMode.Bicubic;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                e.Graphics.DrawLines(mPen, points.ToArray());
            }
            e.Graphics.Restore(graphicsState);
        }

        private void Invalidate()
        {
            if (mPen == null
                || mPen.Color != mLineColor
                || mPen.Width != mLineWidth
                || mPen.StartCap != mLineBeginCap
                || mPen.EndCap != mLineEndCap
                )
            {
                mPen = new Pen(mLineColor, mLineWidth)
                {
                    StartCap = mLineBeginCap,
                    EndCap = mLineEndCap
                };
            }

            DrawControl?.Invalidate();
        }

        private static bool TryGetLineAttachPoints(Control control, ContentAlignment position, int margin, int offset, bool bIsStart, Point[] points, ref int startIndex)
        {
            if (control == null)
            {
                return false;
            }

            switch (position)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomRight:
                    offset = 0;
                    break;
            }

            int calcMargin = bIsStart == true ? margin : margin + offset;
            Point point;
            if (TryGetLineAttachPoint(control, position, calcMargin, out point) == false)
            {
                return false;
            }
            points[startIndex++] = point;

            calcMargin = bIsStart == true ? margin + offset : margin;
            if (TryGetLineAttachPoint(control, position, calcMargin, out point) == false)
            {
                return false;
            }
            points[startIndex++] = point;

            return true;
        }

        private static bool TryGetLineAttachPoint(Control control, ContentAlignment position, int margin, out Point point)
        {
            if (control == null)
            {
                point = Point.Empty;
                return false;
            }

            switch (position)
            {
                case ContentAlignment.TopLeft:
                    point = new Point(control.Left - margin, control.Top - margin);
                    break;

                case ContentAlignment.TopCenter:
                    point = new Point(control.Left + control.Width / 2, control.Top - margin);
                    break;

                case ContentAlignment.TopRight:
                    point = new Point(control.Left + control.Width + margin, control.Top - margin);
                    break;

                case ContentAlignment.MiddleLeft:
                    point = new Point(control.Left - margin, control.Top + control.Height / 2);
                    break;

                case ContentAlignment.MiddleCenter:
                    point = new Point(control.Left + control.Width / 2, control.Top + control.Height / 2);
                    break;

                case ContentAlignment.MiddleRight:
                    point = new Point(control.Left + control.Width + margin, control.Top + control.Height / 2);
                    break;

                case ContentAlignment.BottomLeft:
                    point = new Point(control.Left - margin, control.Top + control.Height + margin);
                    break;

                case ContentAlignment.BottomCenter:
                    point = new Point(control.Left + control.Width / 2, control.Top + control.Height + margin);
                    break;

                case ContentAlignment.BottomRight:
                    point = new Point(control.Left + control.Width + margin, control.Top + control.Height + margin);
                    break;

                default:
                    point = Point.Empty;
                    return false;
            }
            return true;
        }
    }
}

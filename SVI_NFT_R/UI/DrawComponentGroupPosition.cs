using HLDevice;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    /// <summary>
    /// UI에 모터 위치를 표시하는 클래스입니다.
    /// </summary>
    public class DrawComponentGroupPosition
    {
        /// <summary>
        /// true 선택시 X축 표시 방향을 뒤집습니다. (기본값 false : 모니터 기준 좌측이 "-", 우측이 "+")
        /// </summary>
        public bool HorizontalFlip { get; set; }
        /// <summary>
        /// true 선택시 Y축 표시 방향을 뒤집습니다. (기본값 false : 모니터 기준 위쪽이 "-", 아래쪽이 "+")
        /// </summary>
        public bool VerticalFlip { get; set; }
        /// <summary>
        /// 컴포넌트 그룹의 x 위치를 요청하는 함수의 대리자 (출력값 범위는 0d ~ 1d로 해야됨)
        /// </summary>
        public Func<double> RequestGetRatioPositionX { get; set; }
        /// <summary>
        /// 컴포넌트 그룹의 y 위치를 요청하는 함수의 대리자 (출력값 범위는 0d ~ 1d로 해야됨)
        /// </summary>
        public Func<double> RequestGetRatioPositionY { get; set; }
        /// <summary>
        /// 컴포넌트 그룹의 옵셋을 요청하는 함수의 대리자 (출력값이 null이면 이전 값을 유지함)
        /// </summary>
        public Func<UI.OffsetData> RequestGetOffsetData { get; set; }
        /// <summary>
        /// 런타임 중에 좌표를 획득 할 수 있는 모드: 런타임중에 컨트롤을 마우스로 움직여서 좌표를 획득 할 수 있음
        /// </summary>
        public bool IsRuntimeDesignMode { get; set; } = false;
        /// <summary>
        /// 태그
        /// </summary>
        public object Tag { get; set; } = null;
        private readonly Control[] mDisplayControlGroup;
        private readonly Control mBoundaryControl;
        private Point[] mDisplayControlGroupBaseLocation;
        private Rectangle mLimitArea;
        private double mLastRatioPositionX = 0.5d;
        private double mLastRatioPositionY = 0.5d;
        private UI.OffsetData mLastOffset = new UI.OffsetData();
        private readonly ManualResetEventSlim mOneCycleUnlock = new ManualResetEventSlim(true);
        private readonly ManualResetEventSlim mOneCycleEnd = new ManualResetEventSlim(true);
        private Point[] mMouseDownPoints = new Point[0];
        private Point mMouseOffset = Point.Empty;
        private Rectangle mRectRuntimeDesignMode = Rectangle.Empty;
        private static bool mbInitialize = false;
        private static Control[] mSelectControls = new Control[0];
        private static Control mSelectBoundaryControl = null;
        private static Rectangle mSelectLimitArea = Rectangle.Empty;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="boundaryControl">이동 범위를 의미하는 컨트롤러 (컨트롤러는 자동으로 숨겨짐)</param>
        /// <param name="displayControlGroup">모터 위치를 표시하는 컨트롤러 그룹입니다. 디자인한 배열을 유지한채 위치를 표시합니다.</param>
        public DrawComponentGroupPosition(Control boundaryControl, Control[] displayControlGroup)
        {
            // 변수 초기화
            HorizontalFlip = false;
            VerticalFlip = false;
            boundaryControl.Visible = false;
            mDisplayControlGroup = displayControlGroup;
            RequestGetRatioPositionX = null;
            RequestGetRatioPositionY = null;
            RequestGetOffsetData = null;
            mBoundaryControl = boundaryControl;

            foreach (Control controlGroup in mDisplayControlGroup)
            {
                controlGroup.MouseDown += ControlGroup_MouseDown;
                controlGroup.MouseMove += ControlGroup_MouseMove;
                controlGroup.MouseUp += ControlGroup_MouseUp;
            }
            mBoundaryControl.BackColor = Color.FromArgb(50, mBoundaryControl.BackColor);
            mBoundaryControl.Parent.Paint += BoundaryControlParent_Paint;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, mBoundaryControl.Parent, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, mBoundaryControl, new object[] { true });
            mBoundaryControl.Parent.Controls.AddRange(mDisplayControlGroup);
            foreach (Control displayControl in mDisplayControlGroup)
            {
                displayControl.BringToFront();
            }

            if (mbInitialize == false)
            {
                mbInitialize = true;
                Application.OpenForms[0].KeyDown += DrawComponentGroupPosition_KeyDown;
            }

            UpdateLayout();
        }

        public void LockOneCycle()
        {
            mOneCycleUnlock.Reset();
            mOneCycleEnd.Wait();
        }

        public void UnlockOneCycle()
        {
            mOneCycleUnlock.Set();
        }

        /// <summary>
        /// 레이아웃 정보 업데이트합니다.
        /// </summary>
        /// <remarks>
        /// 그룹 컨트롤의 레이아웃이 변경 되었을 때 내부 Limit이나 기준점 정보를 업데이트 합니다.
        /// </remarks>
        public void UpdateLayout()
        {
            var boundaryRectangle = new Rectangle(mBoundaryControl.Location, mBoundaryControl.Size);

            // 컨트롤 그룹 외각의 범위를 구한다.
            int maxX = int.MinValue;
            int minX = int.MaxValue;
            int maxY = int.MinValue;
            int minY = int.MaxValue;
            foreach (var item in mDisplayControlGroup)
            {
                if (maxX < item.Location.X + item.Size.Width)
                {
                    maxX = item.Location.X + item.Size.Width;
                }
                if (minX > item.Location.X)
                {
                    minX = item.Location.X;
                }
                if (maxY < item.Location.Y + item.Size.Height)
                {
                    maxY = item.Location.Y + item.Size.Height;
                }
                if (minY > item.Location.Y)
                {
                    minY = item.Location.Y;
                }
            }
            var displayControlGroupBoundaryRectangle = new Rectangle(
                boundaryRectangle.X, // X
                boundaryRectangle.Y, // Y
                maxX - minX, // Width
                maxY - minY // Height
                );

            // 컨트롤의 실제 이동 가능한 범위를 구한다.
            mLimitArea = new Rectangle(
                boundaryRectangle.X, // X
                boundaryRectangle.Y, // Y
                boundaryRectangle.Width - displayControlGroupBoundaryRectangle.Width, // Width
                boundaryRectangle.Height - displayControlGroupBoundaryRectangle.Height // Height
                );

            // 현재 위치(좌측 상단)를 기준 위치로 저장한다.
            int offsetX = boundaryRectangle.X - minX;
            int offsetY = boundaryRectangle.Y - minY;
            mDisplayControlGroupBaseLocation = new Point[mDisplayControlGroup.Length];
            for (int i = 0; i < mDisplayControlGroupBaseLocation.Length; i++)
            {
                Point currentPos = mDisplayControlGroup[i].Location;
                currentPos.Offset(offsetX, offsetY);
                mDisplayControlGroupBaseLocation[i] = currentPos;
            }
            mRectRuntimeDesignMode = displayControlGroupBoundaryRectangle;
        }

        /// <summary>
        /// 모터에서 정보를 읽어서 컨트롤 그룹의 위치를 업데이트합니다.
        /// </summary>
        public void Reflesh()
        {
            if (mBoundaryControl.Visible != IsRuntimeDesignMode)
            {
                mBoundaryControl.Visible = IsRuntimeDesignMode;
            }
            if (IsRuntimeDesignMode == true)
            {
                return;
            }

            // BEGIN ONE CYCLE
            mOneCycleUnlock.Wait();
            mOneCycleEnd.Reset();

            int offsetX = 0;
            int offsetY = 0;

            if (null == RequestGetOffsetData)
            {
                var getRatioPositionX = RequestGetRatioPositionX;
                if (null != getRatioPositionX)
                {
                    double positionRate = getRatioPositionX.Invoke();
                    if (
                        0.0d <= positionRate
                        && 1d >= positionRate
                        )
                    {
                        mLastRatioPositionX = positionRate;
                    }
                    else
                    {
                        positionRate = mLastRatioPositionX;
                    }

                    // X 축 옵셋값을 구한다.
                    offsetX = (int)(mLimitArea.Width * positionRate);
                    if (true == HorizontalFlip)
                    {
                        // 방향 맞춤
                        offsetX *= -1;
                        // 시작위치 적용
                        offsetX += mLimitArea.Width;
                    }
                }

                var getRatioPositionY = RequestGetRatioPositionY;
                if (null != getRatioPositionY)
                {
                    double positionRate = getRatioPositionY.Invoke();
                    if (
                        0.0d <= positionRate
                        && 1d >= positionRate
                        )
                    {
                        mLastRatioPositionY = positionRate;
                    }
                    else
                    {
                        positionRate = mLastRatioPositionY;
                    }

                    // Y 축 옵셋값을 구한다.
                    offsetY = (int)(mLimitArea.Height * positionRate);
                    if (true == VerticalFlip)
                    {
                        // 방향 맞춤
                        offsetY *= -1;
                        // 시작위치 적용
                        offsetY += mLimitArea.Height;
                    }
                }
            }
            else
            {
                var offset = RequestGetOffsetData.Invoke();
                UI.OffsetItem offsetItemX;
                if (
                    null == offset
                    || UI.EOffsetItem.Skip == offset.X.Type
                    )
                {
                    offsetItemX = mLastOffset.X;
                }
                else
                {
                    offsetItemX = offset.X;
                }
                switch (offsetItemX.Type)
                {
                    case UI.EOffsetItem.Offset:
                        mLastOffset.X = offsetItemX;
                        offsetX = ((int)mLastOffset.X.Value > mLimitArea.Width) ? mLimitArea.Width : (int)mLastOffset.X.Value;
                        break;
                    case UI.EOffsetItem.ReverseOffset:
                        mLastOffset.X = offsetItemX;
                        offsetX = ((int)mLastOffset.X.Value > mLimitArea.Width) ? mLimitArea.Width : (int)mLastOffset.X.Value;
                        offsetX = mLimitArea.Width - offsetX;
                        break;
                    case UI.EOffsetItem.Ratio:
                        mLastOffset.X = offsetItemX;
                        offsetX = (int)(mLimitArea.Width * offsetItemX.Value);
                        break;
                }
                if (true == HorizontalFlip)
                {
                    offsetX *= -1;
                    offsetX += mLimitArea.Width;
                }

                UI.OffsetItem offsetItemY;
                if (
                    null == offset
                    || UI.EOffsetItem.Skip == offset.Y.Type
                    )
                {
                    offsetItemY = mLastOffset.Y;
                }
                else
                {
                    offsetItemY = offset.Y;
                }
                switch (offsetItemY.Type)
                {
                    case UI.EOffsetItem.Offset:
                        mLastOffset.Y = offsetItemY;
                        offsetY = ((int)mLastOffset.Y.Value > mLimitArea.Height) ? mLimitArea.Height : (int)mLastOffset.Y.Value;
                        break;
                    case UI.EOffsetItem.ReverseOffset:
                        mLastOffset.Y = offsetItemY;
                        offsetY = ((int)mLastOffset.Y.Value > mLimitArea.Height) ? mLimitArea.Height : (int)mLastOffset.Y.Value;
                        offsetY = mLimitArea.Height - offsetY;
                        break;
                    case UI.EOffsetItem.Ratio:
                        mLastOffset.Y = offsetItemY;
                        offsetY = (int)(mLimitArea.Height * offsetItemY.Value);
                        break;
                }
                if (true == VerticalFlip)
                {
                    offsetY *= -1;
                    offsetY += mLimitArea.Height;
                }
            }

            // 컨트롤 그룹의 위치를 옵셋 위치로 이동한다.
            for (int i = 0; i < mDisplayControlGroupBaseLocation.Length; i++)
            {
                Point newLocation = mDisplayControlGroupBaseLocation[i];
                newLocation.Offset(offsetX, offsetY);
                if (mDisplayControlGroup[i].Location != newLocation)
                {
                    mDisplayControlGroup[i].Location = newLocation;
                }
            }

            // END ONE CYCLE
            mOneCycleEnd.Set();
        }

        /// <summary>
        /// 모터 객체에서 현재 위치의 비율값을 얻어온다.
        /// </summary>
        /// <param name="motor">모터 객체</param>
        /// <returns>0d ~ 1d</returns>
        public static double GetRatioPositionFromMotorObject(CDeviceMotor motor)
        {
            double result;

            do
            {
                var motorPositionInformation = motor.HLGetMotorPosition();
                double minPosition = double.MaxValue;
                double maxPosition = double.MinValue;
                for (int i = 0; i < motorPositionInformation.dPosition.Length; i++)
                {
                    if (true == string.IsNullOrEmpty(motorPositionInformation.strPositionName[i]))
                    {
                        break;
                    }
                    if (minPosition > motorPositionInformation.dPosition[i])
                    {
                        minPosition = motorPositionInformation.dPosition[i];
                    }
                    if (maxPosition < motorPositionInformation.dPosition[i])
                    {
                        maxPosition = motorPositionInformation.dPosition[i];
                    }
                }
                //var operationParameter = motor.HLGetMotorOperationParameter();
                //if (operationParameter.dLimitSoftwareMinus < operationParameter.dLimitSoftwarePlus)
                //{
                //    minPosition = minPosition > operationParameter.dLimitSoftwareMinus ? operationParameter.dLimitSoftwareMinus : minPosition;
                //    maxPosition = maxPosition < operationParameter.dLimitSoftwarePlus ? operationParameter.dLimitSoftwarePlus : maxPosition;
                //}
                //else
                //{
                //    minPosition = minPosition < operationParameter.dLimitSoftwarePlus ? operationParameter.dLimitSoftwarePlus : minPosition;
                //    maxPosition = maxPosition > operationParameter.dLimitSoftwareMinus ? operationParameter.dLimitSoftwareMinus : maxPosition;
                //}
                double currentPosition = motor.HLGetMotorStatus().dEncoderPosition;
                if (currentPosition > maxPosition)
                {
                    currentPosition = maxPosition;
                }
                else if (currentPosition < minPosition)
                {
                    currentPosition = minPosition;
                }
                double currentPositionDelta = currentPosition - minPosition;
                double positionRateBase = maxPosition - minPosition;
                double positionRate = 0;
                if (0 != positionRateBase)
                {
                    positionRate = currentPositionDelta / positionRateBase;
                }

                result = positionRate;
            } while (false);

            return result;
        }

        private void DrawComponentGroupPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsRuntimeDesignMode == false)
            {
                return;
            }
            if (mSelectControls.Length == 0)
            {
                return;
            }

            int offsetX = 0;
            int offsetY = 0;
            switch (e.KeyCode)
            {
                case Keys.A:
                    offsetX = -1;
                    break;

                case Keys.D:
                    offsetX = 1;
                    break;

                case Keys.W:
                    offsetY = -1;
                    break;

                case Keys.S:
                    offsetY = 1;
                    break;

                default:
                    return;
            }
            for (int i = 0; i < mSelectControls.Length; i++)
            {
                Point newLocation = mSelectControls[i].Location;
                newLocation.Offset(offsetX, offsetY);
                newLocation.X = Math.Max(newLocation.X, mSelectBoundaryControl.Location.X);
                newLocation.X = Math.Min(newLocation.X, mSelectBoundaryControl.Location.X + mSelectLimitArea.Width);
                newLocation.Y = Math.Max(newLocation.Y, mSelectBoundaryControl.Location.Y);
                newLocation.Y = Math.Min(newLocation.Y, mSelectBoundaryControl.Location.Y + mSelectLimitArea.Height);
                if (mSelectControls[i].Location != newLocation)
                {
                    mSelectControls[i].Location = newLocation;
                }
            }
            Point delta = mSelectControls.First().Location - new Size(mSelectBoundaryControl.Location);
            //string result = $"new UI.OffsetData() {{ X = UI.OffsetItem.Offset({delta.X}f), Y = UI.OffsetItem.Offset({delta.Y}f) }};";
            float limitWidth = mSelectLimitArea.Width == 0 ? 1f : mSelectLimitArea.Width;
            float limitHeight = mSelectLimitArea.Height == 0 ? 1f : mSelectLimitArea.Height;
            string result = $"new UI.OffsetData() {{ X = UI.OffsetItem.Ratio({delta.X / limitWidth}f), Y = UI.OffsetItem.Ratio({delta.Y / limitHeight}f) }};";
            Clipboard.SetText(result);
            Debug.WriteLine(result);
        }

        private void BoundaryControlParent_Paint(object sender, PaintEventArgs e)
        {
            if (IsRuntimeDesignMode == false)
            {
                return;
            }
            if (mMouseDownPoints.Length == 0)
            {
                return;
            }

            e.Graphics.FillRectangle(Brushes.YellowGreen, mRectRuntimeDesignMode);
        }

        private void ControlGroup_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsRuntimeDesignMode == false)
            {
                return;
            }
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            mSelectControls = mDisplayControlGroup;
            mSelectBoundaryControl = mBoundaryControl;
            mSelectLimitArea = mLimitArea;
            mMouseDownPoints = new Point[mDisplayControlGroup.Length];
            for (int i = 0; i < mMouseDownPoints.Length; i++)
            {
                mMouseDownPoints[i] = Point.Empty;
            }
            Control ctrl = (Control)sender;
            Point parentPointToScreen = ctrl.PointToScreen(e.Location);
            Point pointToClient = ctrl.Parent.PointToClient(parentPointToScreen);
            mMouseOffset = new Point(ctrl.Location.X - pointToClient.X, ctrl.Location.Y - pointToClient.Y);
        }

        private void ControlGroup_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsRuntimeDesignMode == false)
            {
                return;
            }
            if (mMouseDownPoints.Length == 0)
            {
                return;
            }

            Control groupControl = mDisplayControlGroup.First();
            for (int i = 0; i < mDisplayControlGroup.Length; i++)
            {
                Point newLocation = mDisplayControlGroup[i].Location;
                newLocation.Offset(e.X, e.Y);
                newLocation.Offset(mMouseOffset);
                newLocation.X = Math.Max(newLocation.X, mBoundaryControl.Location.X);
                newLocation.X = Math.Min(newLocation.X, mBoundaryControl.Location.X + mLimitArea.Width);
                newLocation.Y = Math.Max(newLocation.Y, mBoundaryControl.Location.Y);
                newLocation.Y = Math.Min(newLocation.Y, mBoundaryControl.Location.Y + mLimitArea.Height);
                mMouseDownPoints[i] = newLocation;
            }
            if (mRectRuntimeDesignMode.X != mMouseDownPoints.First().X
                || mRectRuntimeDesignMode.Y != mMouseDownPoints.First().Y
                )
            {
                mRectRuntimeDesignMode.X = mMouseDownPoints.First().X;
                mRectRuntimeDesignMode.Y = mMouseDownPoints.First().Y;
                mBoundaryControl.Parent.Refresh();
            }
        }

        private void ControlGroup_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsRuntimeDesignMode == false)
            {
                return;
            }
            if (mMouseDownPoints.Length == 0)
            {
                return;
            }

            for (int i = 0; i < mDisplayControlGroup.Length; i++)
            {
                if (mMouseDownPoints[i] != Point.Empty
                    && mDisplayControlGroup[i].Location != mMouseDownPoints[i]
                    )
                {
                    mDisplayControlGroup[i].Location = mMouseDownPoints[i];
                }
            }
            mMouseDownPoints = new Point[0];
            Point delta = mDisplayControlGroup.First().Location - new Size(mBoundaryControl.Location);
            //string result = $"new UI.OffsetData() {{ X = UI.OffsetItem.Offset({delta.X}f), Y = UI.OffsetItem.Offset({delta.Y}f) }};";
            float limitWidth = mLimitArea.Width == 0 ? 1f : mLimitArea.Width;
            float limitHeight = mLimitArea.Height == 0 ? 1f : mLimitArea.Height;
            string result = $"new UI.OffsetData() {{ X = UI.OffsetItem.Ratio({delta.X / limitWidth}f), Y = UI.OffsetItem.Ratio({delta.Y / limitHeight}f) }};";
            Clipboard.SetText(result);
            Debug.WriteLine(result);
            mBoundaryControl.Refresh();
        }
    }
}

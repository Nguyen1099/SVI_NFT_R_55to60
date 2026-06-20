using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SVI_NFT_R
{

    class DrawImageToPictureBox
    {
        public class DrawImagePoint
        {
            public int width;
            public int height;
            public Point startPoint;
            public DrawImagePoint(int width_, int height_, Point startPoint_)
            {
                width = width_;
                height = height_;
                startPoint = startPoint_;
            }

            public DrawImagePoint(Point startPoint, Point endPoint)
            {
                this.startPoint = new Point(Math.Min(startPoint.X, endPoint.X), Math.Min(startPoint.Y, endPoint.Y));
                width = Math.Abs(endPoint.X - startPoint.X);
                height = Math.Abs(endPoint.Y - startPoint.Y);
            }
        }

        #region 맴버변수
        public string mStrCurrentPosition = "";

        private PictureBox mPictureBox;
        private Timer mBlinkTimer = null;
        private bool mbMouseDown = false;
        private bool mbUseDraw = false;
        private bool mbBlink = false;
        private bool mbDrawTick = false;

        private Image mOriginalImage;
        //private Image mCropImage;
        private string mImageFileName;

        private int mMaxDrawNumber = 0;
        private int mCurrentDrawNumber = 0;

        private int[] mWidthRatio;
        private int[] mHightRatio;
        private Point[] mRectanglePointRatio;
        private int[] mWidth;
        private int[] mHight;
        private Point[] mRectanglePoint;
        private Point[] mStartPoint;
        private Point[] mEndPoint;
        private double[] mDrawStartWidthPercent;
        private double[] mDrawStartHeightPercent;
        private double[] mDrawEndWidthPercent;
        private double[] mDrawEndHeightPercent;
        #endregion

        #region 프로퍼티

        public DrawImagePoint currentDrawPoint { get { return new DrawImagePoint(mStartPoint[mCurrentDrawNumber], mEndPoint[mCurrentDrawNumber]); } }

        public int setCurrentDrawNumber
        {
            set
            {
                if (0 <= value && value < mMaxDrawNumber && mOriginalImage != null)
                {
                    mCurrentDrawNumber = value;
                }
            }
        }
        #endregion

        #region Enum
        enum ESaveFileForm
        {
            DrawNumber = 0,
            Name,
            Value,
        }
        #endregion

        /// <summary>
        /// 클래스 생성자
        /// </summary>
        /// <param name="_pictureBox"></param>
        public DrawImageToPictureBox(PictureBox _pictureBox)
        {
            //받아온 PictureBox 저장, 설정, 이벤트 구독
            mPictureBox = _pictureBox;
            mPictureBox.MouseMove += new MouseEventHandler(this.PictureBox_MouseMove);
            mPictureBox.MouseDown += new MouseEventHandler(this.PictureBox_MouseDown);
            //마우스업 이벤트
            mPictureBox.MouseUp += new MouseEventHandler((s, e) =>
            {
                if (e.Button == MouseButtons.Left && mOriginalImage != null)
                {
                    mbMouseDown = false;
                }
            });
            //pivtureBox Stretch 설정
            mPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            //Draw Interval 타이머생성
            Timer drawTimer = new Timer();
            drawTimer.Enabled = true;
            drawTimer.Interval = 50;
            drawTimer.Tick += new EventHandler((s, e) =>
            {

                if (!mbDrawTick)
                {
                    mbDrawTick = true;
                }
            });
            //Blink를 위한 타이머 생성및 초기화
            mBlinkTimer = new Timer();
            mBlinkTimer.Enabled = false;
            mBlinkTimer.Interval = 500;
            mBlinkTimer.Tick += new System.EventHandler((s, e) =>
            {
                if (mbUseDraw == true)
                {
                    return;
                }
                else if (mbBlink == true)
                {
                    mbBlink = false;
                    ImageDrawRectangle(mPictureBox, mOriginalImage);
                }
                else
                {
                    mbBlink = true;
                    ImageDrawRectangle(mPictureBox, mOriginalImage);
                }
            });
        }

        /// <summary>
        /// 이미지 변경
        /// </summary>
        /// <param name="_originalImage"></param>
        /// <param name="_filename"></param>
        public void InputNewImage(Image _originalImage, string _filename)
        {
            InputNewImage(_originalImage, _filename, 1);
            mCurrentDrawNumber = 0;
        }

        /// <summary>
        /// 이미지 변경
        /// </summary>
        /// <param name="_originalImage"></param>
        /// <param name="_filename"></param>
        /// <param name="_maxDrawImageNumber"></param>
        public void InputNewImage(Image _originalImage, string _filename, int _maxDrawImageNumber)
        {
            if (null != mOriginalImage)
            {
                mOriginalImage.Dispose();
            }

            mImageFileName = string.Format("({0})", _filename);
            //숫자 지정이 0보다 작으면 안됨
            if (_maxDrawImageNumber < 0)
            {
                _maxDrawImageNumber = 1;
                mCurrentDrawNumber = 0;
            }
            mMaxDrawNumber = _maxDrawImageNumber;
            //맴버 변수 배열 생성
            mWidthRatio = new int[mMaxDrawNumber];
            mHightRatio = new int[mMaxDrawNumber];
            mRectanglePointRatio = new System.Drawing.Point[mMaxDrawNumber];
            mWidth = new int[mMaxDrawNumber];
            mHight = new int[mMaxDrawNumber];
            mRectanglePoint = new System.Drawing.Point[mMaxDrawNumber];
            mStartPoint = new System.Drawing.Point[mMaxDrawNumber];
            mEndPoint = new System.Drawing.Point[mMaxDrawNumber];
            mDrawStartWidthPercent = new double[mMaxDrawNumber];
            mDrawStartHeightPercent = new double[mMaxDrawNumber];
            mDrawEndWidthPercent = new double[mMaxDrawNumber];
            mDrawEndHeightPercent = new double[mMaxDrawNumber];
            //맴버 변수 배열 초기화
            for (int i = 0; i < mMaxDrawNumber; i++)
            {
                mWidthRatio[i] = 0;
                mHightRatio[i] = 0;
                mRectanglePointRatio[i] = new System.Drawing.Point(0, 0);
                mStartPoint[i] = new System.Drawing.Point(-1, -1);
                mEndPoint[i] = new System.Drawing.Point(0, 0);
                mWidth[i] = 0;
                mHight[i] = 0;
                mRectanglePoint[i] = new System.Drawing.Point(0, 0);
                mDrawStartWidthPercent[i] = 0;
                mDrawStartHeightPercent[i] = 0;
                mDrawEndWidthPercent[i] = 0;
                mDrawEndHeightPercent[i] = 0;
            }

            //OriginalImage 저장
            mOriginalImage = _originalImage;
            mPictureBox.Image = mOriginalImage;
        }

        /// <summary>
        /// 현재 Position 저장
        /// </summary>
        public void SavePosition()
        {
            if (mOriginalImage == null || string.IsNullOrEmpty(mImageFileName))
            {
                return;
            }
            string path = Application.StartupPath + string.Format(@"\SavePosition\{0}.dat", mImageFileName);
            string dirPath = Application.StartupPath + @"\SavePosition\";
            DirectoryInfo di = new DirectoryInfo(dirPath);
            if (di.Exists == false)
            {
                di.Create();
            }

            List<string> stringList = new List<string>();
            for (int j = 0; j < mMaxDrawNumber; j++)
            {
                stringList.Add(string.Format("{0},widthRatio,{1}", j, Convert.ToString(mWidthRatio[j])));
                stringList.Add(string.Format("{0},heightRatio,{1}", j, Convert.ToString(mHightRatio[j])));
                stringList.Add(string.Format("{0},rectanglePointRatio_X,{1}", j, Convert.ToString(mRectanglePointRatio[j].X)));
                stringList.Add(string.Format("{0},rectanglePointRatio_Y,{1}", j, Convert.ToString(mRectanglePointRatio[j].Y)));
                stringList.Add(string.Format("{0},width,{1}", j, Convert.ToString(mWidth[j])));
                stringList.Add(string.Format("{0},height,{1}", j, Convert.ToString(mHight[j])));
                stringList.Add(string.Format("{0},rectanglePoint_X,{1}", j, Convert.ToString(mRectanglePoint[j].X)));
                stringList.Add(string.Format("{0},rectanglePoint_Y,{1}", j, Convert.ToString(mRectanglePoint[j].Y)));
                stringList.Add(string.Format("{0},drawStartWidthPercent,{1}", j, Convert.ToString(mDrawStartWidthPercent[j])));
                stringList.Add(string.Format("{0},drawStartHeightPercent,{1}", j, Convert.ToString(mDrawStartHeightPercent[j])));
                stringList.Add(string.Format("{0},drawEndWidthPercent,{1}", j, Convert.ToString(mDrawEndWidthPercent[j])));
                stringList.Add(string.Format("{0},drawEndHeightPercent,{1}", j, Convert.ToString(mDrawEndHeightPercent[j])));
                stringList.Add(string.Format("{0},startPoint_X,{1}", j, Convert.ToString(mStartPoint[j].X)));
                stringList.Add(string.Format("{0},startPoint_Y,{1}", j, Convert.ToString(mStartPoint[j].Y)));
                stringList.Add(string.Format("{0},endPoint_X,{1}", j, Convert.ToString(mEndPoint[j].X)));
                stringList.Add(string.Format("{0},endPoint_Y,{1}", j, Convert.ToString(mEndPoint[j].Y)));
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (string line in stringList)
                {
                    sw.WriteLine(line);
                }
            }

        }

        /// <summary>
        /// 저장해놓은 Position을 불러온다
        /// </summary>
        public void LoadPosition()
        {
            if (mOriginalImage == null || string.IsNullOrEmpty(mImageFileName))
            {
                return;
            }
            string path = Application.StartupPath + string.Format(@"\SavePosition\{0}.dat", mImageFileName);
            string dirPath = Application.StartupPath + @"\SavePosition\";
            DirectoryInfo di = new DirectoryInfo(dirPath);
            if (di.Exists == false)
            {
                di.Create();
            }
            try
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] splitStr = line.Split(',');
                    int i = 0;
                    if (splitStr.Length == 3)
                    {
                        if (int.TryParse(splitStr[(int)ESaveFileForm.DrawNumber], out i))
                        {
                            switch (splitStr[(int)ESaveFileForm.Name])
                            {
                                case "widthRatio":
                                    mWidthRatio[i] = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "heightRatio":
                                    mHightRatio[i] = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "rectanglePointRatio_X":
                                    mRectanglePointRatio[i].X = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "rectanglePointRatio_Y":
                                    mRectanglePointRatio[i].Y = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "width":
                                    mWidth[i] = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "height":
                                    mHight[i] = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "rectanglePoint_X":
                                    mRectanglePoint[i].X = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "rectanglePoint_Y":
                                    mRectanglePoint[i].Y = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "drawStartWidthPercent":
                                    mDrawStartWidthPercent[i] = Convert.ToDouble(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "drawStartHeightPercent":
                                    mDrawStartHeightPercent[i] = Convert.ToDouble(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "drawEndWidthPercent":
                                    mDrawEndWidthPercent[i] = Convert.ToDouble(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "drawEndHeightPercent":
                                    mDrawEndHeightPercent[i] = Convert.ToDouble(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "startPoint_X":
                                    mStartPoint[i].X = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "startPoint_Y":
                                    mStartPoint[i].Y = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "endPoint_X":
                                    mEndPoint[i].X = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                case "endPoint_Y":
                                    mEndPoint[i].Y = Convert.ToInt32(splitStr[(int)ESaveFileForm.Value]);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                ImageDrawRectangle(mPictureBox, mOriginalImage);
            }
            catch (FileNotFoundException ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 사각형 그리기
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="image"></param>
        private void ImageDrawRectangle(PictureBox pictureBox, Image image)
        {
            ImageDrawRectangle(pictureBox, image, mStartPoint[mCurrentDrawNumber], mEndPoint[mCurrentDrawNumber]);
        }

        /// <summary>
        /// 사각형 그리기
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="image"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public void ImageDrawRectangle(PictureBox pictureBox, Image image, DrawImagePoint drawPoint)
        {
            Point endPoint = new Point(drawPoint.startPoint.X + drawPoint.width, drawPoint.startPoint.Y + drawPoint.height);

            mDrawStartWidthPercent[mCurrentDrawNumber] = (double)drawPoint.startPoint.X / (double)mPictureBox.Width;
            mDrawStartHeightPercent[mCurrentDrawNumber] = (double)drawPoint.startPoint.Y / (double)mPictureBox.Height;
            mDrawEndWidthPercent[mCurrentDrawNumber] = (double)endPoint.X / (double)mPictureBox.Width;
            mDrawEndHeightPercent[mCurrentDrawNumber] = (double)endPoint.Y / (double)mPictureBox.Height;

            mStartPoint[mCurrentDrawNumber] = drawPoint.startPoint;
            mEndPoint[mCurrentDrawNumber] = endPoint;
            mbDrawTick = true;
            ImageDrawRectangle(pictureBox, image, drawPoint.startPoint, endPoint);
        }

        /// <summary>
        /// 사각형 그리기
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="image"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        private void ImageDrawRectangle(PictureBox pictureBox, Image image, System.Drawing.Point startPoint, System.Drawing.Point endPoint)
        {
            // 이미지가 없으면 동작안함
            if (image == null || !mbDrawTick)
            {
                return;
            }
            mbDrawTick = false;
            //변수 초기화
            System.Drawing.Point drawStartPointRatio = new System.Drawing.Point(0, 0);
            System.Drawing.Point drawEndPointRatio = new System.Drawing.Point(0, 0);

            //Point가 PictureBox를 벗어나면 안된다.
            if (startPoint.X < 0)
            {
                startPoint.X = 0;
            }
            if (endPoint.X < 0)
            {
                endPoint.X = 0;
            }
            if (startPoint.Y < 0)
            {
                startPoint.Y = 0;
            }
            if (endPoint.Y < 0)
            {
                endPoint.Y = 0;
            }
            if (pictureBox.Width < startPoint.X)
            {
                startPoint.X = pictureBox.Width;
            }
            if (pictureBox.Width < endPoint.X)
            {
                endPoint.X = pictureBox.Width;
            }
            if (pictureBox.Height < startPoint.Y)
            {
                startPoint.Y = pictureBox.Height;
            }
            if (pictureBox.Height < endPoint.Y)
            {
                endPoint.Y = pictureBox.Height;
            }


            Bitmap bmp = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height));

            //Pen 초기화
            Pen p = new Pen(Color.Red, 2);
            //p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //비율계산

            double heightRatio = (double)image.Size.Height / (double)pictureBox.Size.Height;
            double widthRatio = (double)image.Size.Width / (double)pictureBox.Size.Width;

            //비율 계산 한 X,Y값 이미지 자르는데 사용
            drawStartPointRatio.X = Convert.ToInt32(startPoint.X * widthRatio);
            drawStartPointRatio.Y = Convert.ToInt32(startPoint.Y * heightRatio);
            drawEndPointRatio.X = Convert.ToInt32(endPoint.X * widthRatio);
            drawEndPointRatio.Y = Convert.ToInt32(endPoint.Y * heightRatio);

            //사각형 Position 계산
            for (int i = 0; i < mMaxDrawNumber; i++)
            {
                if (mStartPoint[i].X < mEndPoint[i].X)
                {
                    //PictureBox 위에 그리는데 사용
                    mRectanglePoint[i].X = Convert.ToInt32(mPictureBox.Width * mDrawStartWidthPercent[i]);
                    mWidth[i] = Convert.ToInt32(mPictureBox.Width * mDrawEndWidthPercent[i]) - Convert.ToInt32(mPictureBox.Width * mDrawStartWidthPercent[i]);

                }
                else
                {
                    //PictureBox 위에 그리는데 사용
                    mRectanglePoint[i].X = Convert.ToInt32(mPictureBox.Width * mDrawEndWidthPercent[i]);
                    mWidth[i] = Convert.ToInt32(mPictureBox.Width * mDrawStartWidthPercent[i]) - Convert.ToInt32(mPictureBox.Width * mDrawEndWidthPercent[i]);
                }
                if (mStartPoint[i].Y < mEndPoint[i].Y)
                {
                    //PictureBox 위에 그리는데 사용
                    mRectanglePoint[i].Y = Convert.ToInt32(mPictureBox.Height * mDrawStartHeightPercent[i]);
                    mHight[i] = Convert.ToInt32(mPictureBox.Height * mDrawEndHeightPercent[i]) - Convert.ToInt32(mPictureBox.Height * mDrawStartHeightPercent[i]);
                }
                else
                {
                    //PictureBox 위에 그리는데 사용
                    mRectanglePoint[i].Y = Convert.ToInt32(mPictureBox.Height * mDrawEndHeightPercent[i]);
                    mHight[i] = Convert.ToInt32(mPictureBox.Height * mDrawStartHeightPercent[i]) - Convert.ToInt32(mPictureBox.Height * mDrawEndHeightPercent[i]);
                }
            }
            if (startPoint.X < endPoint.X)
            {
                //이미지 자르는데 사용
                mRectanglePointRatio[mCurrentDrawNumber].X = drawStartPointRatio.X;
                mWidthRatio[mCurrentDrawNumber] = drawEndPointRatio.X - drawStartPointRatio.X;
            }
            else
            {
                //이미지 자르는데 사용                
                mRectanglePointRatio[mCurrentDrawNumber].X = drawEndPointRatio.X;
                mWidthRatio[mCurrentDrawNumber] = drawStartPointRatio.X - drawEndPointRatio.X;
            }
            if (startPoint.Y < endPoint.Y)
            {
                //이미지 자르는데 사용                
                mRectanglePointRatio[mCurrentDrawNumber].Y = drawStartPointRatio.Y;
                mHightRatio[mCurrentDrawNumber] = drawEndPointRatio.Y - drawStartPointRatio.Y;
            }
            else
            {
                //이미지 자르는데 사용
                mRectanglePointRatio[mCurrentDrawNumber].Y = drawEndPointRatio.Y;
                mHightRatio[mCurrentDrawNumber] = drawStartPointRatio.Y - drawEndPointRatio.Y;
            }
            //그리기
            for (int i = 0; i < mMaxDrawNumber; i++)
            {
                if (mWidth[i] != 0 && mHight[i] != 0)
                {
                    if (mbBlink)
                    {
                        if (mCurrentDrawNumber != i)
                        {
                            g.DrawRectangle(p, mRectanglePoint[i].X, mRectanglePoint[i].Y, mWidth[i], mHight[i]);
                        }
                    }
                    else
                    {
                        g.DrawRectangle(p, mRectanglePoint[i].X, mRectanglePoint[i].Y, mWidth[i], mHight[i]);
                    }
                    using (Font font = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel))
                    {
                        TextRenderer.DrawText(g, Convert.ToString(i + 1), font, mRectanglePoint[i], Color.Blue);
                    }
                }
            }
            //마무리
            pictureBox.Image = bmp;
            g.Dispose();
        }

        /// <summary>
        /// 현재 그린 이미지 초기화
        /// </summary>
        public void DrawClearAll()
        {
            if (mOriginalImage == null)
            {
                return;
            }
            BlinkOnOff(false);
            mPictureBox.Image = mOriginalImage;
            for (int i = 0; i < mMaxDrawNumber; i++)
            {
                mWidthRatio[i] = 0;
                mHightRatio[i] = 0;
                mRectanglePointRatio[i] = new System.Drawing.Point(0, 0);
                mWidth[i] = 0;
                mHight[i] = 0;
                mRectanglePoint[i] = new System.Drawing.Point(0, 0);
                mDrawStartWidthPercent[i] = 0;
                mDrawStartHeightPercent[i] = 0;
                mDrawEndWidthPercent[i] = 0;
                mDrawEndHeightPercent[i] = 0;
                mStartPoint[i] = new System.Drawing.Point(0, 0);
                mEndPoint[i] = new System.Drawing.Point(0, 0);
            }

        }

        /// <summary>
        /// 생성자로 받아온 PictureBox MouseMove이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {

            int intX = 0;
            int intY = 0;

            System.Drawing.Point realImagePoint = new System.Drawing.Point();
            if (mOriginalImage != null)
            {
                //비율계산
                double heightRatio = (double)mOriginalImage.Size.Height / (double)mPictureBox.Size.Height;
                double widthRatio = (double)mOriginalImage.Size.Width / (double)mPictureBox.Size.Width;
                realImagePoint.X = Convert.ToInt32(e.Location.X * widthRatio);
                realImagePoint.Y = Convert.ToInt32(e.Location.Y * heightRatio);
                //pictureBox와 실제 이미지 좌표
                mStrCurrentPosition = string.Format("x : {0} y : {1}\nx : {2} y : {3}", e.Location.X.ToString(), e.Location.Y.ToString(), realImagePoint.X.ToString(), realImagePoint.Y.ToString());
            }

            if (mbMouseDown && mOriginalImage != null)
            {
                if (mStartPoint[mCurrentDrawNumber].X == -1 || mStartPoint[mCurrentDrawNumber].Y == -1)
                {
                    LoadPosition();
                    return;
                }
                mDrawEndWidthPercent[mCurrentDrawNumber] = (double)e.X / (double)mPictureBox.Width;
                mDrawEndHeightPercent[mCurrentDrawNumber] = (double)e.Y / (double)mPictureBox.Height;
                if (mDrawEndWidthPercent[mCurrentDrawNumber] < 0)
                {
                    mDrawEndWidthPercent[mCurrentDrawNumber] = 0;
                }
                if (1 < mDrawEndWidthPercent[mCurrentDrawNumber])
                {
                    mDrawEndWidthPercent[mCurrentDrawNumber] = 1;
                }
                if (mDrawEndHeightPercent[mCurrentDrawNumber] < 0)
                {
                    mDrawEndHeightPercent[mCurrentDrawNumber] = 0;
                }
                if (1 < mDrawEndHeightPercent[mCurrentDrawNumber])
                {
                    mDrawEndHeightPercent[mCurrentDrawNumber] = 1;
                }

                intX = e.X;
                intY = e.Y;

                mEndPoint[mCurrentDrawNumber] = new System.Drawing.Point(intX, intY);
                ImageDrawRectangle(mPictureBox, mOriginalImage);
                mbUseDraw = true;
            }
            else
            {
                mbUseDraw = false;
            }
        }

        /// <summary>
        /// 생성자로 받아온 PictureBox MouseDown이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int intX = 0;
            int intY = 0;

            if (e.Button == MouseButtons.Left && mOriginalImage != null)
            {
                mbMouseDown = true;

                mDrawStartWidthPercent[mCurrentDrawNumber] = (double)e.X / (double)mPictureBox.Width;
                mDrawStartHeightPercent[mCurrentDrawNumber] = (double)e.Y / (double)mPictureBox.Height;
                mDrawEndWidthPercent[mCurrentDrawNumber] = (double)e.X / (double)mPictureBox.Width;
                mDrawEndHeightPercent[mCurrentDrawNumber] = (double)e.Y / (double)mPictureBox.Height;
                if (mDrawStartWidthPercent[mCurrentDrawNumber] < 0)
                {
                    mDrawStartWidthPercent[mCurrentDrawNumber] = 0;
                }
                if (1 < mDrawStartWidthPercent[mCurrentDrawNumber])
                {
                    mDrawStartWidthPercent[mCurrentDrawNumber] = 1;
                }
                if (mDrawStartHeightPercent[mCurrentDrawNumber] < 0)
                {
                    mDrawStartHeightPercent[mCurrentDrawNumber] = 0;
                }
                if (1 < mDrawStartHeightPercent[mCurrentDrawNumber])
                {
                    mDrawStartHeightPercent[mCurrentDrawNumber] = 1;
                }
                if (mDrawEndWidthPercent[mCurrentDrawNumber] < 0)
                {
                    mDrawEndWidthPercent[mCurrentDrawNumber] = 0;
                }
                if (1 < mDrawEndWidthPercent[mCurrentDrawNumber])
                {
                    mDrawEndWidthPercent[mCurrentDrawNumber] = 1;
                }
                if (mDrawEndHeightPercent[mCurrentDrawNumber] < 0)
                {
                    mDrawEndHeightPercent[mCurrentDrawNumber] = 0;
                }
                if (1 < mDrawEndHeightPercent[mCurrentDrawNumber])
                {
                    mDrawEndHeightPercent[mCurrentDrawNumber] = 1;
                }

                intX = e.X;
                intY = e.Y;
                mStartPoint[mCurrentDrawNumber] = new System.Drawing.Point(intX, intY);
                mEndPoint[mCurrentDrawNumber] = new System.Drawing.Point(intX, intY);
                mbBlink = false;
                ImageDrawRectangle(mPictureBox, mOriginalImage);
            }

        }

        /// <summary>
        /// BlinkOnOff Toggle
        /// </summary>
        public void BlinkOnOff()
        {
            if (mOriginalImage == null)
            {
                return;
            }
            if (mBlinkTimer.Enabled == true)
            {
                BlinkOnOff(false);
                ImageDrawRectangle(mPictureBox, mOriginalImage);
            }
            else
            {
                BlinkOnOff(true);
                ImageDrawRectangle(mPictureBox, mOriginalImage);
            }
        }

        /// <summary>
        /// BlinkOnOff 지정
        /// </summary>
        /// <param name="on"></param>
        public void BlinkOnOff(bool on)
        {
            if (mOriginalImage == null)
            {
                return;
            }
            if (on)
            {
                mBlinkTimer.Enabled = true;
                mbBlink = true;
            }
            else
            {
                mBlinkTimer.Enabled = false;
                //종료할때 그리고 종료
                if (mbBlink == true)
                {
                    mbBlink = false;
                }
                ImageDrawRectangle(mPictureBox, mOriginalImage);
            }
        }


        /// <summary>
        /// 이미지 다시그리기
        /// </summary>
        public void Refresh()
        {
            if (mOriginalImage == null)
            {
                return;
            }
            mbDrawTick = true;
            ImageDrawRectangle(mPictureBox, mOriginalImage);
        }

    }
}

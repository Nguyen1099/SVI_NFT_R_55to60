
using SVI_NFT_R.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace SVI_NFT_R
{
    public partial class CDialogAlarmPosition : Form
    {
        DrawImageToPictureBox drawImage;

        public DrawAlarmPosition drawPoint;
        public class DrawAlarmPosition
        {
            public float X;
            public float Y;
            public float width;
            public float hight;
            public DrawAlarmPosition(float X_, float Y_, float width_, float hight_)
            {
                X = X_;
                Y = Y_;
                width = width_;
                hight = hight_;
            }
        }

        public CDialogAlarmPosition(float X, float Y, float width, float hight)
        {
            InitializeComponent();
            drawPoint = new DrawAlarmPosition(X, Y, width, hight);
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            var returnPoint = drawImage.currentDrawPoint;
            drawPoint = new DrawAlarmPosition(returnPoint.startPoint.X / (float)pictureBox1.Width, returnPoint.startPoint.Y / (float)pictureBox1.Height, returnPoint.width / (float)pictureBox1.Width, returnPoint.height / (float)pictureBox1.Height);
            DialogResult = DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void CDialogAlarmPosition_Load(object sender, EventArgs e)
        {
            drawImage = new DrawImageToPictureBox(pictureBox1);
            drawImage.InputNewImage(Resources.TOP_VIEW_FLOW, "");
            drawImage.DrawClearAll();
            drawImage.ImageDrawRectangle(pictureBox1, Resources.TOP_VIEW_FLOW, new DrawImageToPictureBox.DrawImagePoint(Convert.ToInt32(pictureBox1.Width * drawPoint.width), Convert.ToInt32(pictureBox1.Height * drawPoint.hight), new Point(Convert.ToInt32(pictureBox1.Width * drawPoint.X), Convert.ToInt32(pictureBox1.Height * drawPoint.Y))));
        }
    }
}

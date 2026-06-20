using System.Windows.Forms;

namespace UiAsset
{
    public class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel() : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}

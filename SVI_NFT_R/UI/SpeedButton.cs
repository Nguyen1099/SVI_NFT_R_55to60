using System;
using System.Windows.Forms;

namespace UiAsset
{
    /// <summary>
    /// 버튼을 클릭한 뒤 버튼 주위에 테두리가 생기는 않는 버튼 스타일
    /// </summary>
    public class SpeedButton : Button
    {
        public SpeedButton() : base()
        {
            FlatStyle = FlatStyle.Flat;
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.StandardDoubleClick, true);
            TabStop = false;
            Cursor = Cursors.Hand;
        }

        public new void PerformClick()
        {
            if (Visible && Enabled)
            {
                ResetFlagsandPaint();
                OnClick(EventArgs.Empty);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        public sealed class AutoScale
        {
            private SizeF _formSize;
            private List<Control> _controls;
            private List<Rectangle> _controlList;
            private List<int> _controlFontSize;

            public AutoScale()
            {
                _controls = new List<Control>();
                _controlList = new List<Rectangle>();
                _controlFontSize = new List<int>();
            }

            public void SetInitialSize(Form form)
            {
                _formSize = form.ClientSize;
                var controls = GetAllControls(form);

                _controls.Clear();
                _controlList.Clear();
                _controlFontSize.Clear();

                _controls.AddRange(controls);

                foreach (Control control in _controls)
                {
                    _controlList.Add(control.Bounds);
                    _controlFontSize.Add((int)control.Font.Height);
                }
            }

            public void SetInitialSizeByControl(Form form, Control control)
            {
                _formSize = form.ClientSize;
                var controls = GetAllControls(control);

                _controls.Clear();
                _controlList.Clear();
                _controlFontSize.Clear();

                _controls.AddRange(controls);

                foreach (Control ctrl in _controls)
                {
                    _controlList.Add(ctrl.Bounds);
                    _controlFontSize.Add((int)ctrl.Font.Height);
                }
            }

            public void Resize(Form form)
            {
                if (form.ClientSize.Width == 0
                    && form.ClientSize.Height == 0
                    )
                {
                    return;
                }

                double ratioWidth = Math.Round(form.ClientSize.Width / (double)_formSize.Width, 2);
                double ratioHeight = Math.Round(form.ClientSize.Height / (double)_formSize.Height, 2);

                for (int i = 0; i < _controls.Count; ++i)
                {
                    Control control = _controls[i];

                    control.Font = new Font(control.Font.FontFamily,
                        (float)(((Convert.ToDouble(_controlFontSize[i]) * ratioWidth) / 3.55) + ((Convert.ToDouble(_controlFontSize[i]) * ratioHeight) / 3.55)),
                        control.Font.Style,
                        control.Font.Unit,
                        control.Font.GdiCharSet,
                        control.Font.GdiVerticalFont
                        );

                    if (control.Parent != null
                        && control.Parent is UserControl
                        )
                    {
                        continue;
                    }

                    Size _controlSize = new Size((int)(_controlList[i].Width * ratioWidth), (int)(_controlList[i].Height * ratioHeight));
                    Point _controlposition = new Point(
                        (int)(_controlList[i].X * ratioWidth),
                        (int)(_controlList[i].Y * ratioHeight));
                    control.Bounds = new Rectangle(_controlposition, _controlSize);
                }
            }

            private static IEnumerable<Control> GetAllControls(Control control)
            {
                return control.Controls.Cast<Control>()
                    .SelectMany(item => GetAllControls(item))
                    .Concat(control.Controls.Cast<Control>())
                    .Where(item => item.Name != string.Empty);
            }
        }
    }
}

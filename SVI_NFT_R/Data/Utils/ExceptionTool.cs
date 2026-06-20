using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    public static class ExceptionTool
    {
        public static void ShowExceptionDialog(Exception e, string title)
        {
            StringBuilder sb = new StringBuilder();
            StackTrace trace = new StackTrace(e, true);
            sb.Append(title).AppendLine()
                .Append("Please check the message below").AppendLine();
            int count = 0;
            foreach (StackFrame sf in trace.GetFrames())
            {
                count++;
                sb.AppendFormat("[Depth:{0}, Line:{1}, Method:{2}, File:{3}]"
                    , count, sf.GetFileLineNumber(), sf.GetMethod().Name, Path.GetFileName(sf.GetFileName()))
                    .AppendLine();
            }
            sb.AppendFormat("[Message:{0}]", e.Message);

            MessageBox.Show(null, sb.ToString(), "Program will terminate", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}

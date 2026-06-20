using System;
using System.Runtime.InteropServices;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        /// <summary>
        /// 키보드와 마우스에대한 입력이 없었던 시간을 반환한다.
        /// </summary>
        /// <returns>키보드와 마우스에대한 입력이 없었던 시간</returns>
        public static TimeSpan GetUiInputIdleTime()
        {
            TimeSpan result = TimeSpan.Zero;

            do
            {
                var lastInputInfo = default(NativeMethods.LASTINPUTINFO);
                lastInputInfo.cbSize = NativeMethods.LASTINPUTINFO.SizeOf;
                lastInputInfo.dwTime = 0;
                if (false == NativeMethods.GetLastInputInfo(ref lastInputInfo))
                {
                    break;
                }
                result = TimeSpan.FromMilliseconds(Environment.TickCount - lastInputInfo.dwTime);
            } while (false);

            return result;
        }

        private partial class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool GetLastInputInfo(ref LASTINPUTINFO lastInputInfo);

            [StructLayout(LayoutKind.Sequential)]
            public struct LASTINPUTINFO
            {
                public static readonly uint SizeOf = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO));

                [MarshalAs(UnmanagedType.U4)]
                public uint cbSize;
                [MarshalAs(UnmanagedType.U4)]
                public uint dwTime;
            }
        }
    }
}

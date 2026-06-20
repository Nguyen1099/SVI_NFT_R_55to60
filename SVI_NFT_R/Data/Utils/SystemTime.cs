using System;
using System.Runtime.InteropServices;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        /// <summary>
        /// 시스템 시간을 설정한다. 관리자 권한으로 프로그램이 실행된 상태에서만 시간이 변경 된다
        /// </summary>
        /// <param name="changeDateTime">변경할 날짜와 시간</param>
        /// <returns>함수 실행 결과</returns>
        public static bool SetLocalTime(DateTime changeDateTime)
        {
            bool bResult = false;

            do
            {
                NativeMethods.SYSTEMTIME setTime = new NativeMethods.SYSTEMTIME();
                setTime.FromDateTime(changeDateTime);
                NativeMethods.SetLocalTime(ref setTime);

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 시스템 시간을 읽어온다
        /// </summary>
        /// <returns>시스템 날짜와 시간</returns>
        public static DateTime GetSystemTime()
        {
            NativeMethods.SYSTEMTIME getTime = new NativeMethods.SYSTEMTIME();
            NativeMethods.GetSystemTime(ref getTime);
            return getTime.ToDateTime();
        }

        private partial class NativeMethods
        {
            /// <summary>
            /// SYSTEMTIME structure with some useful methods
            /// </summary>
            public struct SYSTEMTIME
            {
                public ushort wYear;
                public ushort wMonth;
                public ushort wDayOfWeek;
                public ushort wDay;
                public ushort wHour;
                public ushort wMinute;
                public ushort wSecond;
                public ushort wMilliseconds;

                /// <summary>
                /// Convert form System.DateTime
                /// </summary>
                /// <param name="time"></param>
                public void FromDateTime(DateTime time)
                {
                    wYear = (ushort)time.Year;
                    wMonth = (ushort)time.Month;
                    wDayOfWeek = (ushort)time.DayOfWeek;
                    wDay = (ushort)time.Day;
                    wHour = (ushort)time.Hour;
                    wMinute = (ushort)time.Minute;
                    wSecond = (ushort)time.Second;
                    wMilliseconds = (ushort)time.Millisecond;
                }

                /// <summary>
                /// Convert to System.DateTime
                /// </summary>
                /// <returns></returns>
                public DateTime ToDateTime()
                {
                    return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
                }

                /// <summary>
                /// STATIC: Convert to System.DateTime
                /// </summary>
                /// <param name="time"></param>
                /// <returns></returns>
                public static DateTime ToDateTime(SYSTEMTIME time)
                {
                    return time.ToDateTime();
                }
            }

            [DllImport("kernel32.dll")]
            public static extern void GetSystemTime(ref SYSTEMTIME lpSystemTime);
            [DllImport("kernel32.dll")]
            public static extern uint SetSystemTime(ref SYSTEMTIME lpSystemTime);
            [DllImport("kernel32.dll")]
            public static extern uint SetLocalTime(ref SYSTEMTIME lpSystemTime);
        }
    }
}

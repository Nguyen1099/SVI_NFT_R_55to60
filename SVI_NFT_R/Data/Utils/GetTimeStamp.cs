using System;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        private static double FREQ_RATE = TimeSpan.FromSeconds(1d).Ticks / Stopwatch.Frequency;

        public static long GetTimestamp()
        {
            return Convert.ToInt64(Stopwatch.GetTimestamp() * FREQ_RATE);
        }
    }
}

using System;

namespace Utils
{
    public static partial class TimedAlarmActivator
    {
        private class Statistics
        {
            public DateTime LastLogTime { get; set; }
            public int ALID { get; }
            public string ALTX { get; }
            public int HuntingCount { get; set; }
            public int TimedAlarmExpiredCount { get; set; }

            public Statistics(int alid, string altx)
            {
                ALID = alid;
                ALTX = altx;
                HuntingCount = 0;
                TimedAlarmExpiredCount = 0;
            }
        }
    }
}
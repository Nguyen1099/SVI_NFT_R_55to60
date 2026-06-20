using System;

namespace Utils
{
    public static partial class TimedAlarmActivator
    {
        private class TimedAlarm
        {
            public int ALID { get; }
            public string ALTX { get; }
            public DateTime StartTime { get; set; }
            public bool IsActive { get; set; }
            public TimeSpan ActivationTime => DateTime.Now - StartTime;

            public TimedAlarm(int alid, string altx)
            {
                ALID = alid;
                ALTX = altx;
                IsActive = false;
            }

            public void Start()
            {
                StartTime = DateTime.Now;
                IsActive = true;
            }

            public void Stop()
            {
                IsActive = false;
            }

            public bool IsExpired(TimeSpan duration)
            {
                return IsActive && (DateTime.Now - StartTime) >= duration;
            }
        }
    }
}
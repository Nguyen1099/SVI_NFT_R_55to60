using System;

namespace FileClean.Define
{
    /// <summary>
    /// 감시 스케쥴
    /// </summary>
    public sealed class ScheduleItemSet
    {
        /// <summary>
        /// 설정
        /// </summary>
        public ConfigSet Config { get; private set; }
        /// <summary>
        /// 마지막 스캔 시간
        /// </summary>
        public DateTime LastScanTime { get; set; } = DateTime.MinValue;

        public ScheduleItemSet(ConfigSet config)
        {
            Config = config;
        }
    }
}

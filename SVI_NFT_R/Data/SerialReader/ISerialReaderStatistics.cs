using System;

namespace SVI_NFT_R.Data
{
    public interface ISerialReaderStatistics
    {
        object Index { get; }
        string Name { get; }
        int StatisticsOkCount { get; }
        int StatisticsNgCount { get; }
        int StatisticsAlarmCount { get; }
        TimeSpan StatisticsAlarmDuration { get; }
        bool IsReadingFail { get; }

        void Clear();
    }
}

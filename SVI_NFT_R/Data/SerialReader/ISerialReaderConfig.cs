using System;

namespace SVI_NFT_R.Data
{
    public interface ISerialReaderConfig
    {
        object Index { get; }
        string Name { get; }
        TimeSpan ReadingFailIgnoreDuration { get; set; }
        TimeSpan ReadingFailIgnoreDurationMinValue { get; }
        TimeSpan ReadingFailIgnoreDurationMaxValue { get; }
    }
}
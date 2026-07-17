using System;

namespace Mcc
{
    public sealed class MccLogData
    {
        public DateTime WriteDateTime { get; private set; }
        public string WriteLogMessage { get; private set; }

        public MccLogData(DateTime writeDateTime, string logMessage)
        {
            WriteDateTime = writeDateTime;
            WriteLogMessage = logMessage;
        }
    }
}

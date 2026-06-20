using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SVI_NFT_R.Data
{
    public class SerialReader<TReadValue> : ISerialReaderStatistics, ISerialReaderInitializer, ISerialReaderConfig, ISerialReaderReportCsv
    {
        public bool IsInitialized { get; private set; } = false;
        public object Index { get; set; }
        public string Name { get; set; }
        public TReadValue Value { get; private set; }
        public TReadValue ReadingFailDefualtValue { get; set; }
        public TimeSpan ReadingFailIgnoreDuration
        {
            get
            {
                return mReadingFailIgnoreDuration;
            }
            set
            {
                if (value < ReadingFailIgnoreDurationMinValue)
                {
                    value = ReadingFailIgnoreDurationMinValue;
                }
                else if (value > ReadingFailIgnoreDurationMaxValue)
                {
                    value = ReadingFailIgnoreDurationMaxValue;
                }
                bool bIsChanged = (mReadingFailIgnoreDuration != value);
                if (bIsChanged == false)
                {
                    return;
                }

                mReadingFailIgnoreDuration = value;

                if (mReadingFailTimeStopwatch.IsRunning == true)
                {
                    mStatisticsAlarmDuration += mReadingFailTimeStopwatch.Elapsed;
                    mReadingFailTimeStopwatch.Reset();
                }
                mStatisticsReadingFailStartTime = TimeSpan.MaxValue.Ticks;
                IsReadingFail = false;
            }
        }
        public TimeSpan ReadingFailIgnoreDurationMinValue => TimeSpan.FromSeconds(1.5d);
        public TimeSpan ReadingFailIgnoreDurationMaxValue => TimeSpan.FromSeconds(300d);
        public int StatisticsOkCount => mStatisticsOkCount;
        public int StatisticsNgCount => mStatisticsNgCount;
        public int StatisticsAlarmCount => mStatisticsAlarmCount;
        public TimeSpan StatisticsAlarmDuration => mStatisticsAlarmDuration + mReadingFailTimeStopwatch.Elapsed;
        public bool IsReadingFail { get; private set; } = false;
        public event EventHandler OnReport;
        public event EventHandler<ReadingEventArgs<TReadValue>> OnReading;
        private int mStatisticsOkCount = 0;
        private int mStatisticsNgCount = 0;
        private int mStatisticsAlarmCount = 0;
        private TimeSpan mStatisticsAlarmDuration = TimeSpan.Zero;
        private TimeSpan mReadingFailIgnoreDuration = TimeSpan.FromSeconds(30d); /* TimeSpan.FromSeconds(1.5d); */
        private int mStatisticsLastReportHour;
        private long mStatisticsReadingFailStartTime;
        private readonly Stopwatch mReadingFailTimeStopwatch = new Stopwatch();
        private long mLastReadingTime;
        private bool mbShouldStop;
        private readonly TimeSpan mReadingInterval = TimeSpan.FromSeconds(1d);
        private Thread mThreadReading;
        private string mBackupPath;
        private ClassINI mBackupIni;

        public bool Initilaize()
        {
            if (IsInitialized == true)
            {
                return false;
            }

            mBackupPath = Path.Combine(".", "cache", "SerialStatisticsBackup.ini");
            string directoryName = Path.GetDirectoryName(mBackupPath);
            if (Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }
            mBackupIni = new ClassINI(mBackupPath);
            Restore();

            Value = ReadingFailDefualtValue;
            mbShouldStop = false;
            mThreadReading = new Thread(ThreadReading);
            mThreadReading.Start();

            IsInitialized = true;
            return true;
        }

        public void DeInitilaize()
        {
            if (IsInitialized == false)
            {
                return;
            }
            mbShouldStop = true;
            mThreadReading.Join();

            IsInitialized = false;
        }

        public static string GetHeaderForCsvReport()
        {
            return $"Time,Name,Ok (Count),Ng (Count),Alarm (Duration),Alarm (Count)";
        }

        public string GetOneLineForCsvReport()
        {
            return $"{mStatisticsLastReportHour:00}H~{DateTime.Now.Hour:00}H,{Name},{mStatisticsOkCount},{mStatisticsNgCount},{StatisticsAlarmDuration:hh\\:mm\\\"ss\\'f},{mStatisticsAlarmCount}";
        }

        public void Clear()
        {
            if (mReadingFailTimeStopwatch.IsRunning == true)
            {
                mReadingFailTimeStopwatch.Restart();
            }
            mStatisticsOkCount = 0;
            mStatisticsNgCount = 0;
            mStatisticsAlarmCount = 0;
            mStatisticsAlarmDuration = TimeSpan.Zero;
            mStatisticsLastReportHour = DateTime.Now.Hour;
        }

        private void Restore()
        {
            string sectionName = Name;
            mStatisticsOkCount = mBackupIni.GetInt32(sectionName, nameof(mStatisticsOkCount), 0);
            mStatisticsNgCount = mBackupIni.GetInt32(sectionName, nameof(mStatisticsNgCount), 0);
            mStatisticsAlarmCount = mBackupIni.GetInt32(sectionName, nameof(mStatisticsAlarmCount), 0);
            TimeSpan.TryParse(mBackupIni.GetString(sectionName, nameof(mStatisticsAlarmDuration), TimeSpan.Zero.ToString()), out mStatisticsAlarmDuration);
            mStatisticsLastReportHour = mBackupIni.GetInt32(sectionName, nameof(mStatisticsLastReportHour), DateTime.Now.Hour);
        }

        private void Backup()
        {
            try
            {
                string sectionName = Name;
                mBackupIni.WriteValue(sectionName, nameof(mStatisticsOkCount), mStatisticsOkCount);
                mBackupIni.WriteValue(sectionName, nameof(mStatisticsNgCount), mStatisticsNgCount);
                mBackupIni.WriteValue(sectionName, nameof(mStatisticsAlarmCount), mStatisticsAlarmCount);
                mBackupIni.WriteValue(sectionName, nameof(mStatisticsAlarmDuration), StatisticsAlarmDuration.ToString());
                mBackupIni.WriteValue(sectionName, nameof(mStatisticsLastReportHour), mStatisticsLastReportHour);
            }
            catch (Exception ex)
            {
                // ! 간헐적으로 File IO 관련 예외가 발생하여 예외처리함
                LogWrite.Exception(ex);
            }
        }

        private long GetElapsed(long startTime)
        {
            return SVI_NFT_R.Utils.GetTimestamp() - startTime;
        }

        private void ThreadReading()
        {
            mLastReadingTime = TimeSpan.Zero.Ticks;
            mStatisticsReadingFailStartTime = TimeSpan.MaxValue.Ticks;

            while (mbShouldStop == false)
            {
                Thread.Sleep(100);

                if (mStatisticsLastReportHour != DateTime.Now.Hour)
                {
                    RaiseReportEvent();

                    Clear();

                    Backup();
                }
                if (GetElapsed(mLastReadingTime) < mReadingInterval.Ticks)
                {
                    continue;
                }
                mLastReadingTime = SVI_NFT_R.Utils.GetTimestamp();

                TReadValue readValue;
                if (RaiseReadingEvent(out readValue) == false)
                {
                    mStatisticsNgCount++;
                    if (mStatisticsReadingFailStartTime == TimeSpan.MaxValue.Ticks)
                    {
                        mStatisticsReadingFailStartTime = SVI_NFT_R.Utils.GetTimestamp();
                    }
                    if (mReadingFailTimeStopwatch.IsRunning == false)
                    {
                        mReadingFailTimeStopwatch.Restart();
                    }
                    if (GetElapsed(mStatisticsReadingFailStartTime) > mReadingFailIgnoreDuration.Ticks)
                    {
                        Value = ReadingFailDefualtValue;
                        mStatisticsReadingFailStartTime = TimeSpan.MaxValue.Ticks;
                        mStatisticsAlarmCount++;
                        IsReadingFail = true;
                    }
                    Backup();
                    continue;
                }

                if (mReadingFailTimeStopwatch.IsRunning == true)
                {
                    mStatisticsAlarmDuration += mReadingFailTimeStopwatch.Elapsed;
                    mReadingFailTimeStopwatch.Reset();
                }
                mStatisticsReadingFailStartTime = TimeSpan.MaxValue.Ticks;
                mStatisticsOkCount++;
                Value = readValue;
                IsReadingFail = false;
                Backup();
                continue;
            }
        }

        private void RaiseReportEvent()
        {
            OnReport.Invoke(this, EventArgs.Empty);
        }

        private bool RaiseReadingEvent(out TReadValue readValue)
        {
            ReadingEventArgs<TReadValue> args = new ReadingEventArgs<TReadValue>(Index, Name);
            OnReading.Invoke(this, args);
            readValue = args.Value;
            return args.Result;
        }
    }
}
using ENC.LogDLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils
{
    public static partial class TimedAlarmActivator
    {
        private const string LOG_EVENT = "LOG_TIMED_ALARM_EVENT";
        private const string LOG_STATISTICS = "LOG_TIMED_ALARM_STATISTICS";
        private const string LOG_ETC = "LOG_ETC";
        private const string BACKUP_DIRECTORY_NAME = @".\cache\TimedAlarmActivator\";
        private const string TIMED_ALARM_FILE_NAME = @"Backup_TimedAlarm.txt";
        private const string STATISTICS_FILE_NAME = @"Backup_Statistics.txt";
        private const string SETTING_FILE_NAME = @"Setting_TimedAlarmDuration(sec).txt";
        private static Dictionary<string, TimedAlarm> mTimedAlarms;
        private static Dictionary<string, Statistics> mStatistics;
        private static LogManager mLogManager;
        private static bool mIsInitialized = false;
        private static TimeSpan mBackupInterval = TimeSpan.FromSeconds(1d);
        private static DateTime mLastBackupTime = DateTime.Now;
        private static TimeSpan mTimedAlarmDuration = TimeSpan.FromSeconds(1d);

        public static void Initialize(LogManager logManager, string logFolderName)
        {
            if (mIsInitialized == true)
            {
                return;
            }

            mTimedAlarms = new Dictionary<string, TimedAlarm>();
            mStatistics = new Dictionary<string, Statistics>();
            mLogManager = logManager;
            mLogManager.Initialize(LOG_EVENT, logFolderName, true);
            mLogManager.Initialize(LOG_STATISTICS, logFolderName, true);
            mLogManager.SetHeader(LOG_EVENT, string.Join(",",
                "DateTime",
                "ALID",
                "ALTX",
                "Message"
                ));
            mLogManager.SetHeader(LOG_STATISTICS, string.Join(",",
                "Time",
                "ALID",
                "ALTX",
                "Hunting (Count)",
                "TimedAlarmExpired (Count)",
                "SettingDuration (sec)"
                ));

            UpdateTimedAlarmDuration();
            Reload();
            mIsInitialized = true;
            EventLog(0, "0", $"<<<<< `{nameof(TimedAlarmActivator)}` Initialized >>>>>");
        }

        public static void DeInitialize()
        {
            if (mIsInitialized == false)
            {
                return;
            }

            EventLog(0, "0", $">>>>>> `{nameof(TimedAlarmActivator)}` DeInitializd <<<<<");
            Backup(bForceExecute: true);
            mLogManager = null;
            mTimedAlarms.Clear();
            mStatistics.Clear();
            mIsInitialized = false;
        }

        public static void Backup(bool bForceExecute = false)
        {
            if (Directory.Exists(BACKUP_DIRECTORY_NAME) == false)
            {
                Directory.CreateDirectory(BACKUP_DIRECTORY_NAME);
            }

            if (bForceExecute == false
                && mLastBackupTime + mBackupInterval > DateTime.Now
                )
            {
                return;
            }
            mLastBackupTime = DateTime.Now;

            UpdateTimedAlarmDuration();

            try
            {
                string timedAlarmFilePath = Path.Combine(BACKUP_DIRECTORY_NAME, TIMED_ALARM_FILE_NAME);
                using (var writer = new StreamWriter(timedAlarmFilePath))
                {
                    foreach (var timedAlarm in mTimedAlarms.Values)
                    {
                        writer.WriteLine(string.Join(",",
                            timedAlarm.ALID,
                            timedAlarm.ALTX,
                            timedAlarm.StartTime,
                            timedAlarm.IsActive
                            ));
                    }
                }

                if (mStatistics.Values.Any(statistics => DateTime.Now.Hour - statistics.LastLogTime.Hour > 0) == true)
                {
                    foreach (var statistics in mStatistics.OrderBy(x => x.Value.ALID).Select(x => x.Value))
                    {
                        StatisticsLog(statistics);
                        statistics.LastLogTime = DateTime.Now;
                        statistics.HuntingCount = 0;
                        statistics.TimedAlarmExpiredCount = 0;
                    }
                }

                string statisticsFilePath = Path.Combine(BACKUP_DIRECTORY_NAME, STATISTICS_FILE_NAME);
                using (var writer = new StreamWriter(statisticsFilePath))
                {
                    foreach (var statistics in mStatistics.Values)
                    {
                        writer.WriteLine(string.Join(",",
                            statistics.LastLogTime,
                            statistics.ALID,
                            statistics.ALTX,
                            statistics.HuntingCount,
                            statistics.TimedAlarmExpiredCount
                            ));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog(ex);
            }
        }

        public static void AddTimedAlarm(object alarmId)
        {
            string altx = alarmId.ToString();
            if (mTimedAlarms.ContainsKey(altx) == false)
            {
                int alid = Convert.ToInt32(alarmId);
                var timedAlarm = new TimedAlarm(alid, altx);
                mTimedAlarms[altx] = timedAlarm;
            }
            if (mStatistics.ContainsKey(altx) == false)
            {
                int alid = Convert.ToInt32(alarmId);
                var statistics = new Statistics(alid, altx);
                statistics.LastLogTime = DateTime.Now;
                mStatistics[altx] = statistics;
            }
        }

        public static bool IsTimedAlarmExpired(object alarmId)
        {
            string altx = alarmId.ToString();
            var timedAlarm = mTimedAlarms[altx];
            if (timedAlarm.IsActive == false)
            {
                timedAlarm.Start();
                EventLog(timedAlarm.ALID, timedAlarm.ALTX, "Timed-alarm started.");
            }
            return timedAlarm.IsExpired(mTimedAlarmDuration);
        }

        public static void StopTimedAlarm(object alarmId)
        {
            string altx = alarmId.ToString();
            var timedAlarm = mTimedAlarms[altx];
            if (timedAlarm.IsActive == false)
            {
                return;
            }

            var statistics = mStatistics[timedAlarm.ALTX];
            if (timedAlarm.IsExpired(mTimedAlarmDuration) == true)
            {
                statistics.TimedAlarmExpiredCount++;
                EventLog(timedAlarm.ALID, timedAlarm.ALTX, $"Timed-alarm stopped. (activation-time: {timedAlarm.ActivationTime.TotalSeconds:0.000} sec)");
            }
            else
            {
                statistics.HuntingCount++;
                EventLog(timedAlarm.ALID, timedAlarm.ALTX, $"Detect hunting stopped. (activation-time: {timedAlarm.ActivationTime.TotalSeconds:0.000} / {mTimedAlarmDuration.TotalSeconds:0.000} sec)");
            }
            timedAlarm.Stop();
        }

        private static void UpdateTimedAlarmDuration()
        {
            try
            {
                string settingFilePath = Path.Combine(BACKUP_DIRECTORY_NAME, SETTING_FILE_NAME);
                if (File.Exists(settingFilePath) == false)
                {
                    using (var writer = new StreamWriter(settingFilePath))
                    {
                        writer.WriteLine(mTimedAlarmDuration.TotalSeconds);
                    }
                }
                else
                {
                    using (var reader = new StreamReader(settingFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (double.TryParse(line, out double value) == false)
                            {
                                continue;
                            }
                            mTimedAlarmDuration = TimeSpan.FromSeconds(value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionLog(ex);
            }
        }

        private static void Reload()
        {
            if (Directory.Exists(BACKUP_DIRECTORY_NAME) == false)
            {
                Directory.CreateDirectory(BACKUP_DIRECTORY_NAME);
            }

            try
            {
                string statisticsFilePath = Path.Combine(BACKUP_DIRECTORY_NAME, STATISTICS_FILE_NAME);
                if (File.Exists(statisticsFilePath))
                {
                    using (var reader = new StreamReader(statisticsFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 5)
                            {
                                DateTime lastLogTime = DateTime.Parse(parts[0]);
                                int alid = int.Parse(parts[1]);
                                string altx = parts[2];
                                int huntingCount = int.Parse(parts[3]);
                                int timedAlarmExpiredCount = int.Parse(parts[4]);
                                var statistics = new Statistics(alid, altx);
                                statistics.LastLogTime = lastLogTime;
                                statistics.HuntingCount = huntingCount;
                                statistics.TimedAlarmExpiredCount = timedAlarmExpiredCount;
                                mStatistics[altx] = statistics;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog(ex);
            }
        }

        private static void EventLog(int alid, string altx, string message)
        {
            mLogManager.WriteLog(LOG_EVENT, string.Join(",",
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]",
                alid,
                altx,
                message
                ));
        }

        private static void StatisticsLog(Statistics statistics)
        {
            mLogManager.WriteLog(LOG_STATISTICS, string.Join(",",
                $"{statistics.LastLogTime.Hour:00}H~{DateTime.Now.Hour:00}H",
                statistics.ALID,
                statistics.ALTX,
                statistics.HuntingCount,
                statistics.TimedAlarmExpiredCount,
                $"{mTimedAlarmDuration.TotalSeconds:0.000}"
                ));
        }

        private static void ExceptionLog(Exception ex, string comment = "")
        {
            StringBuilder sb = new StringBuilder();
            StackTrace trace = new StackTrace(ex, true);
            sb.Append(comment).AppendLine();
            int count = 0;
            foreach (StackFrame sf in trace.GetFrames())
            {
                count++;
                sb.AppendFormat(
                    "[Depth:{0}, Line:{1}, Method:{2}, File:{3}]",
                    count,
                    sf.GetFileLineNumber(),
                    sf.GetMethod().Name,
                    Path.GetFileName(sf.GetFileName())
                    ).AppendLine();
            }
            sb.AppendFormat("[Message:{0}]", ex.Message);

            mLogManager.WriteLog(LOG_ETC, sb.ToString());
        }
    }
}
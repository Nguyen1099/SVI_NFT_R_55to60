using FileClean.Define;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FileClean
{
    /// <summary>
    /// 설정된 폴더를 감시하여 보관 시간이 지난 파일 및 폴더를 삭제하는 클래스
    /// </summary>
    public static partial class Scheduler
    {
        /// <summary>
        /// 초기화 여부
        /// </summary>
        public static bool IsInitialized { get; private set; } = false;
        /// <summary>
        /// 코드로 예약된 설정 리스트
        /// </summary>
        public static List<ConfigSet> ReservedConfigItems { get; private set; } = new List<ConfigSet>();
        private static List<ConfigSet> mConfigItems = new List<ConfigSet>();
        private static List<ScheduleItemSet> mScheduleItems = new List<ScheduleItemSet>();
        private static Thread mThreadProcess;
        private static bool mbThreadExit = false;
        private static readonly string CONFIG_PATH = $@".\Config_FileCleanScheduler.json";
        private static bool mbForcedScan = false;
        private static readonly object mLock = new object();

        /// <summary>
        /// 초기화 및 스케쥴러 시작
        /// </summary>
        /// <returns>성공 여부</returns>
        public static bool Initailize()
        {
            if (IsInitialized == true)
            {
                return false;
            }

            ReloadConfig();

            mbThreadExit = false;
            mThreadProcess = new Thread(threadProcess);
            mThreadProcess.IsBackground = true;
            mThreadProcess.Start();

            IsInitialized = true;
            return true;
        }

        /// <summary>
        /// 스케쥴러 정리
        /// </summary>
        public static void DeInitailize()
        {
            mbThreadExit = true;
            mThreadProcess.Join();

            IsInitialized = false;
        }

        /// <summary>
        /// 구성 파일을 읽어와 관리 리스트를 업데이트 한다
        /// </summary>
        public static void ReloadConfig()
        {
            lock (mLock)
            {
                if (File.Exists(CONFIG_PATH) == true)
                {
                    string readJsonString = File.ReadAllText(CONFIG_PATH);
                    mConfigItems = Newtonsoft.Json.JsonConvert.DeserializeObject(readJsonString, typeof(List<ConfigSet>)) as List<ConfigSet>;
                }
                else
                {
                    mConfigItems.Add(new ConfigSet());
                    string writeJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(mConfigItems, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(CONFIG_PATH, writeJsonString);
                }

                mScheduleItems.Clear();
                foreach (var config in mConfigItems)
                {
                    if (config.ScanHours < 1)
                    {
                        config.ScanHours = 1;
                    }
                    if (config.KeepDays < 1)
                    {
                        config.KeepDays = 1;
                    }
                    mScheduleItems.Add(new ScheduleItemSet(config));
                }
                foreach (var config in ReservedConfigItems)
                {
                    if (config.ScanHours < 1)
                    {
                        config.ScanHours = 1;
                    }
                    if (config.KeepDays < 1)
                    {
                        config.KeepDays = 1;
                    }
                    mScheduleItems.Add(new ScheduleItemSet(config));
                }
            }
        }

        /// <summary>
        /// 마지막 스캔 시간을 무시하고 즉시 파일 스캔을 진행한다
        /// </summary>
        public static void ForcedScan()
        {
            lock (mLock)
            {
                mbForcedScan = true;
            }
        }

        private static void doProcessFileCleanSchedule()
        {
            DateTime now = DateTime.Now;
            foreach (var item in mScheduleItems)
            {
                Thread.Sleep(0);
                if (item.Config.IsUse == false)
                {
                    continue;
                }
                if (Directory.Exists(item.Config.ScanPath) == false)
                {
                    continue;
                }
                TimeSpan scanTime = now - item.LastScanTime;
                if (
                    scanTime.TotalHours < item.Config.ScanHours
                    && mbForcedScan == false
                    )
                {
                    continue;
                }
                item.LastScanTime = now;

                // 폴더 삭제
                DirectoryInfo directoryInfo = new DirectoryInfo(item.Config.ScanPath);
                var directorysInfo = directoryInfo.GetDirectories();
                foreach (var directory in directorysInfo)
                {
                    TimeSpan creationTime = now - directory.CreationTime;
                    if (creationTime.Days >= item.Config.KeepDays)
                    {
                        try
                        {
                            directory.Delete(true);
                        }
                        catch
                        {
                        }
                    }
                }
                // 파일 삭제
                var filesInfo = directoryInfo.GetFiles();
                foreach (var file in filesInfo)
                {
                    TimeSpan creationTime = now - file.CreationTime;
                    if (creationTime.Days >= item.Config.KeepDays)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private static void threadProcess()
        {
            // 첫 스캔까지 1초 지연
            SpinWait.SpinUntil(() => mbThreadExit, TimeSpan.FromSeconds(1));
            while (mbThreadExit == false)
            {
                lock (mLock)
                {
                    doProcessFileCleanSchedule();
                    Thread.Sleep(100);
                    mbForcedScan = false;
                }
            }
        }
    }
}

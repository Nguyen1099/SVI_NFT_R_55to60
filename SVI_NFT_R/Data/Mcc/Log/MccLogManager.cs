using Mcc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SVI_NFT_R
{
    public static class MccLogManager
    {
        public static InShuttleMccLogItems InShuttle { get; private set; } = new InShuttleMccLogItems();
        public static InRobotMccLogItems InRobot { get; private set; } = new InRobotMccLogItems();
        public static InspStageMccLogItems InspStage { get; private set; } = new InspStageMccLogItems();
        public static OutRobotMccLogItems OutRobot { get; private set; } = new OutRobotMccLogItems();
        public static OutFlipMccLogItems OutFlip { get; private set; } = new OutFlipMccLogItems();

        //public static OutConveyorMccLogItems OutConveyor { get; private set; } = new OutConveyorMccLogItems();

        public static OutShuttleMccLogItems OutShuttle { get; private set; } = new OutShuttleMccLogItems();
        public static InspectMccLogItems Inspect { get; private set; } = new InspectMccLogItems();
        public static ConcurrentQueue<MccLogData> LogDataQueue { get; private set; } = new ConcurrentQueue<MccLogData>();
        public static bool IsInitialized { get; private set; } = false;
        public static bool ShouldWriteAlarmActionEndLog
        {
            get => Convert.ToBoolean(mBackupData.GetValue(nameof(ShouldWriteAlarmActionEndLog), false));
            set => mBackupData.SetValue(nameof(ShouldWriteAlarmActionEndLog), value);
        }
        private static readonly string REGISTRY_PATH = Path.Combine("ENC", Program.ID, "Data", nameof(MccLogManager));
        private static readonly Settings.RegistryComponent mBackupData = new Settings.RegistryComponent(REGISTRY_PATH);

        public static bool Initialize(CDocument document)
        {
            InShuttle.Initialize(document);
            InRobot.Initialize(document);
            InspStage.Initialize(document);
            OutRobot.Initialize(document);
            OutFlip.Initialize(document);
            //OutConveyor.Initialize(document);
            OutShuttle.Initialize(document);
            Inspect.Initialize(document);
            IsInitialized = true;
            return true;
        }

        public static void DeInitialize()
        {
            InShuttle.DeInitialize();
            InRobot.DeInitialize();
            InspStage.DeInitialize();
            OutRobot.DeInitialize();
            OutFlip.DeInitialize();
            //OutConveyor.DeInitialize();
            OutShuttle.DeInitialize();
            Inspect.DeInitialize();
            IsInitialized = false;
        }

        public static IEnumerable<IMccLogItem> GetAlarmActions()
        {
            var items = InShuttle.ALARM
                .Concat(InRobot.ALARM)
                .Concat(InspStage.ALARM)
                .Concat(OutRobot.ALARM)
                .Concat(OutFlip.ALARM)
                //.Concat(OutConveyor.ALARM);
                .Concat(OutShuttle.ALARM);
            foreach (var item in items)
            {
                yield return item;
            }
        }

        public static IEnumerable<IMccLogItem> GetAlarmStopActions()
        {
            var items = InShuttle.ALARM_STOP
                .Concat(InRobot.ALARM_STOP)
                .Concat(InspStage.ALARM_STOP)
                .Concat(OutRobot.ALARM_STOP)
                .Concat(OutFlip.ALARM_STOP)
                //.Concat(OutConveyor.ALARM_STOP);  
                .Concat(OutShuttle.ALARM_STOP);

            foreach (var item in items)
            {
                yield return item;
            }
        }

        public static string GetSystemNo(string alphabat, int systemNo)
        {
            //int systemIndex = systemNo - 1;
            //return $"{((systemIndex / 99) + 1):0}{alphabat}{((systemIndex % 99) + 1):00}";
            return $"{alphabat}{systemNo:00}";
        }
    }
}

using System;

namespace SVI_NFT_R.Config
{
    public static partial class WaitTime
    {
        /// <summary>
        /// 기타 시간 카테고리를 정의한 클래스
        /// </summary>
        [Category(Name = "Etc")]
        public static class Etc
        {
            public static IReadOnlyWaitTime AutoLogout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Minute, defaultValue: TimeSpan.FromMinutes(10d));
            public static IReadOnlyWaitTime SilentStopTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Minute, defaultValue: TimeSpan.FromMinutes(10d));
            public static IReadOnlyWaitTime NoOperationDisplayDelay { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Minute, defaultValue: TimeSpan.FromMinutes(1d));
            public static IReadOnlyWaitTime CellDetectSensorSensingTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(1d));
            [IgnoreSetting]
            public static IReadOnlyWaitTime AftpLinkedInspectionResultFileStoreDays { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Day, defaultValue: TimeSpan.FromDays(30d));
            // 설비 효율 관리를위해 Track Out시간을 제어함 (Giap, 허태영 그룹장 2024-02-19 불합리 요청)
            [IgnoreSetting]
            public static IReadOnlyWaitTime CellTrackOutDelayTime { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(1d));
            public static IReadOnlyWaitTime LowEnergyModeChangeDelay { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Minute, defaultValue: TimeSpan.FromMinutes(5d));
            //public static IReadOnlyWaitTime Sample { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));
        }
    }
}

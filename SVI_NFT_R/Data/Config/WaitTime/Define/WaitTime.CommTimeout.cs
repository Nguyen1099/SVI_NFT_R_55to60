using System;

namespace SVI_NFT_R.Config
{
    public static partial class WaitTime
    {
        /// <summary>
        /// 통신 대기시간 카테고리를 정의한 클래스
        /// </summary>
        [Category(Name = "CommTimeout")]
        public static class CommTimeout
        {
            public static IReadOnlyWaitTime InspReplyTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));
            public static IReadOnlyWaitTime InspGrabEndReplyTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));
            public static IReadOnlyWaitTime InspResultTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(10d));
            public static IReadOnlyWaitTime RobotTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(10d));
            public static IReadOnlyWaitTime AlignVisionTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));
            public static IReadOnlyWaitTime CimTrackingTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(10d));
            [IgnoreSetting]
            public static IReadOnlyWaitTime CimLoginTimeout { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(3d));

            //public static IReadOnlyWaitTime Sample { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));
        }
    }
}

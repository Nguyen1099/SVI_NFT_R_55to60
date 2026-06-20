using System;

namespace SVI_NFT_R.Config
{
    public static partial class WaitTime
    {
        /// <summary>
        /// 설비 인터페이스 카테고리를 정의한 클래스
        /// </summary>
        [Category(Name = "Equipment")]
        public static class Equipment
        {
            public static IReadOnlyWaitTime LoadInterfaceDelayTime { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));
            public static IReadOnlyWaitTime UnloadInterfaceDelayTime { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));
            public static IReadOnlyWaitTime UnloadInterfacePendingTime { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));

            //public static IReadOnlyWaitTime Sample { get; private set; } = new ImplWaitTime(unit: ETimeUnit.Second, defaultValue: TimeSpan.FromSeconds(5d));
        }
    }
}

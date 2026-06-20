using System;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>
    /// 셀 배출 이벤트 파라메터
    /// </summary>
    public class CellOutputEventArgs : EventArgs
    {
        /// <summary>
        /// 배출 간격
        /// </summary>
        public TimeSpan OutputCycleTime { get; set; } = TimeSpan.Zero;
        /// <summary>
        /// 대기 시간을 제외한 순수 동작 시간
        /// </summary>
        public TimeSpan PureCycleTime { get; set; } = TimeSpan.Zero;
        /// <summary>
        /// 투입 대기 시간
        /// </summary>
        public TimeSpan WaitLoadTime { get; set; } = TimeSpan.Zero;
        /// <summary>
        /// 배출 대기 시간
        /// </summary>
        public TimeSpan WaitUnloadTime { get; set; } = TimeSpan.Zero;
        /// <summary>
        /// 검사 결고 대기 시간
        /// </summary>
        public TimeSpan WaitResultTime { get; set; } = TimeSpan.Zero;
        /// <summary>
        /// 사용자 정의 시간
        /// </summary>
        public List<TimeSpan> UserDefineTimesOrNull { get; set; } = null;
    }
}

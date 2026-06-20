using System;

namespace Utils
{
    /// <summary>
    /// 로그 쓰기 이벤트 파라메터
    /// </summary>
    public class WritingLogEventArgs : EventArgs
    {
        /// <summary>
        /// 택타임 로거 객체
        /// </summary>
        public IReadOnlyTactLogger TactLogger { get; set; }
    }
}

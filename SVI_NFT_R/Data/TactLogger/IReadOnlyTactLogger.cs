using System;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>
    /// 읽기 전용 택타임 로거의 인터페이스
    /// </summary>
    public interface IReadOnlyTactLogger
    {
        /// <summary>
        /// 태그
        /// </summary>
        object Tag { get; }
        /// <summary>
        /// 유닛 이름
        /// </summary>
        string UnitName { get; }
        /// <summary>
        /// CSV로그 파일 이름
        /// </summary>
        string LogFileName { get; }
        /// <summary>
        /// 마지막으로 로그를 기록한 시간
        /// </summary>
        DateTime LastLoggingTime { get; }
        /// <summary>
        /// 대기 시간을 제외한 순수 동작 시간 (단위: 초)
        /// </summary>
        double LastPureCycleTact { get; }
        /// <summary>
        /// [동기화용] 로그가 잠겨져 있는지 여부
        /// </summary>
        bool IsLock { get; }
        /// <summary>
        /// 동작 리스트
        /// </summary>
        IReadOnlyDictionary<string, IReadOnlyTactLoggerSet> Items { get; }
        /// <summary>
        /// 로그 쓰기 이벤트
        /// </summary>
        event EventHandler<WritingLogEventArgs> WritingLog;
    }
}

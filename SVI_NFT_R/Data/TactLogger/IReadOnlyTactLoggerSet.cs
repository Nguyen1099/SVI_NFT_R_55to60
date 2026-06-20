using System;

namespace Utils
{
    /// <summary>
    /// 읽기 전용 택타임 항목의 인터페이스
    /// </summary>
    public interface IReadOnlyTactLoggerSet
    {
        /// <summary>
        /// 로그를 쓸 수있는지 여부 (Begin과 End를 정상적으로 진행했거나, Skip 이나 Empty를 진행한 경우 로그를 쓸 수 있음)
        /// </summary>
        bool CanLogging { get; }
        /// <summary>
        /// Skip 이나 Empty를 진행한지 여부
        /// </summary>
        bool IsSkipOrEmpty { get; }
        /// <summary>
        /// Begin 기록 여부
        /// </summary>
        bool IsWriteBegin { get; }
        /// <summary>
        /// End 기록 여부
        /// </summary>
        bool IsWriteEnd { get; }
        /// <summary>
        /// 아이템을 구별할 고유 이름
        /// </summary>
        string ID { get; }
        /// <summary>
        /// CSV 로그 해더 제목
        /// </summary>
        string HeaderName { get; }
        /// <summary>
        /// 동작 구분
        /// </summary>
        EAction Type { get; }
        /// <summary>
        /// 시간을 반환함
        /// </summary>
        TimeSpan Duration { get; }
        // https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET
        /// <summary>
        /// 시간 계산용 객체
        /// </summary>
        Itenso.TimePeriod.TimeRange TimeRange { get; }

        /// <summary>
        /// 시간을 초단위로 형식에 맞게 문자열을 반환함
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}

using System;

namespace SVI_NFT_R
{
    /// <summary>
    /// 대기시간을 사용 할 때 인터페이스
    /// </summary>
    public interface IReadOnlyWaitTime
    {
        /// <summary>
        /// 대기시간 인덱스
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 대기시간 카테고리
        /// </summary>
        string Category { get; }
        /// <summary>
        /// 대기시간 TimeSpan
        /// </summary>
        TimeSpan Value { get; }
    }
}
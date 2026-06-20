using System;

namespace EqToEq
{
    /// <summary>
    /// 위치 정보
    /// </summary>
    [Flags]
    public enum EPosition
    {
        /// <summary>
        /// 없음
        /// </summary>
        None = 0,

        /// <summary>
        /// P1
        /// </summary>
        P1 = 1 << 0,

        /// <summary>
        /// P2
        /// </summary>
        P2 = 1 << 1,

        /// <summary>
        /// 없음
        /// </summary>
        Empty = 1 << 31
    }
}
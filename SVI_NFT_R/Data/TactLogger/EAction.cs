using System;

namespace Utils
{
    /// <summary>
    /// 동작
    /// </summary>
    [Flags]
    public enum EAction
    {
        /// <summary>
        /// 대기 동작 (순수 택타임에 반영하지 않음)
        /// </summary>
        Waiting = 0x1 << 0,

        /// <summary>
        /// 실제 동작
        /// </summary>
        Moving = 0x1 << 1,

        // Reserved: 2 ~ 9

        // 다용도 플래그: 10 ~ 31
        Flag01 = 0x1 << 10,
        Flag02 = 0x1 << 11,
        Flag03 = 0x1 << 12,
        Flag04 = 0x1 << 13,
        Flag05 = 0x1 << 14,
        Flag06 = 0x1 << 15,
        Flag07 = 0x1 << 16,
        Flag08 = 0x1 << 17,
        Flag09 = 0x1 << 18,
        Flag10 = 0x1 << 19,
        Flag11 = 0x1 << 20,
        Flag12 = 0x1 << 21,
        Flag13 = 0x1 << 22,
        Flag14 = 0x1 << 23,
        Flag15 = 0x1 << 24,
        Flag16 = 0x1 << 25,
        Flag17 = 0x1 << 26,
        Flag18 = 0x1 << 27,
        Flag19 = 0x1 << 28,
        Flag20 = 0x1 << 29,
        Flag21 = 0x1 << 30,
        Flag22 = 0x1 << 31,
    }
}
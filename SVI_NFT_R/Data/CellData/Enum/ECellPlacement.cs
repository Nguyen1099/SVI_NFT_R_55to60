using System;

namespace SVI_NFT_R.CellData
{
    [Flags]
    public enum ECellPlacement
    {
        Empty = 1 << 0,

        LeftTop = 1 << 1,
        Top = 1 << 2,
        RightTop = 1 << 3,

        Left = 1 << 4,
        Center = 1 << 5,
        Right = 1 << 6,

        LeftBottom = 1 << 7,
        Bottom = 1 << 8,
        RightBottom = 1 << 9,

        P1 = 1 << 10,
        P2 = 1 << 11,
        P3 = 1 << 12,
        P4 = 1 << 13,
        P5 = 1 << 14,
        P6 = 1 << 15,
        P7 = 1 << 16,
        P8 = 1 << 17,
        P9 = 1 << 18,
        P10 = 1 << 19,

        S1 = 1 << 20,
        S2 = 1 << 21,
        S3 = 1 << 22,
        S4 = 1 << 23,
        S5 = 1 << 24,
        S6 = 1 << 25,
        S7 = 1 << 26,
        S8 = 1 << 27,
        S9 = 1 << 28,
        S10 = 1 << 29,

        Full = 1 << 31
    }
}

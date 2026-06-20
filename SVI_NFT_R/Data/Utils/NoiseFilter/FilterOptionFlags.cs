using System;

namespace SVI_NFT_R
{
    [Flags]
    public enum FilterOptionFlags
    {
        RisingEdge = 1 << 0,
        FallingEdge = 1 << 1
    }
}

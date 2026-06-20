using System;

namespace SVI_NFT_R
{
    [Flags]
    public enum EEmsGroupFlags
    {
        None = 0,

        Loader = 1 << 0,
        Unloader = 1 << 1,
    }
}
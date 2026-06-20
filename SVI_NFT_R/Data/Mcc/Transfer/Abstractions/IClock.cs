using System;

namespace SVI_NFT_R
{
    public interface IClock
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}

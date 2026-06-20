using System;

namespace SVI_NFT_R
{
    public sealed class SystemClock : IClock
    {
        public DateTime Now => DateTime.Now;
        public DateTime Today => DateTime.Today;
    }
}

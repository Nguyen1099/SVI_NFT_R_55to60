using System;

namespace SVI_NFT_R.UI
{
    public class SwapPairEventArgs : EventArgs
    {
        public ESwapOrigin Status { get; private set; }

        public SwapPairEventArgs(ESwapOrigin status)
        {
            Status = status;
        }
    }
}

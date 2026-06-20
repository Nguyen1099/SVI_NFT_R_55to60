using System;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public class RmsDataChangedEventArgs : EventArgs
    {
        public RmsData RmsData { get; private set; }

        public RmsDataChangedEventArgs(RmsData rmsData)
        {
            RmsData = rmsData;
        }
    }
}

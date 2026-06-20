using System;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public interface IHaveLogEvent
    {
        event EventHandler<string> OnEventOccured;
    }
}

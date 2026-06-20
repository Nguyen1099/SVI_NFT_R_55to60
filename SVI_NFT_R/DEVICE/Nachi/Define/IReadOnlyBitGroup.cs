
using System;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public interface IReadOnlyBitGroup
    {
        int Value { get; }
        string[] SignalNames { get; }

        bool WaitForTargetValue(int targetValue, TimeSpan timeout, Func<bool> cancel);
    }
}

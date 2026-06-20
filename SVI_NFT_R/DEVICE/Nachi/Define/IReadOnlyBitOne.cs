
using System;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public interface IReadOnlyBitOne
    {
        bool Value { get; }
        string SignalName { get; }

        bool WaitForTargetValue(bool targetValue, TimeSpan timeout, Func<bool> cancel);
    }
}

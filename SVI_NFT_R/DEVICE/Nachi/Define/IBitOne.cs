
using System;
using System.Threading.Tasks;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public interface IBitOne
    {
        bool Value { get; set; }
        string SignalName { get; }

        void Set();

        void Clear();

        bool Toggle();

        Task OnePulseAsync(bool pulseSignal, TimeSpan pulseWidth);
    }
}

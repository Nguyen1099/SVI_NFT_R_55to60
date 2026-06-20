using System.Collections.Generic;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public interface IInputHandler
    {
        IReadOnlyDictionary<SignalNames.Bit.EInput, IReadOnlyBitOne> BB { get; }
        IReadOnlyDictionary<SignalNames.Bit.EInputGroup, IReadOnlyBitGroup> BG { get; }
        IReadOnlyDictionary<SignalNames.Word.EInput, IReadOnlyBitOne> WB { get; }
        IReadOnlyDictionary<SignalNames.Word.EInputGroup, IReadOnlyBitGroup> WG { get; }
    }
}
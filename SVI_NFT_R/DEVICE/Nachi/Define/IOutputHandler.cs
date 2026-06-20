using System.Collections.Generic;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public interface IOutputHandler
    {
        IReadOnlyDictionary<SignalNames.Bit.EOutput, IBitOne> BB { get; }
        IReadOnlyDictionary<SignalNames.Bit.EOutputGroup, IBitGroup> BG { get; }
        IReadOnlyDictionary<SignalNames.Word.EOutput, IBitOne> WB { get; }
        IReadOnlyDictionary<SignalNames.Word.EOutputGroup, IBitGroup> WG { get; }
    }
}
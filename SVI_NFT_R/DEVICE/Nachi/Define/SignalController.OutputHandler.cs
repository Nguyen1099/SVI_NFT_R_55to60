using System;
using System.Collections.Generic;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public partial class SignalController
    {
        private sealed class OutputHandler : IOutputHandler
        {
            public IReadOnlyDictionary<SignalNames.Bit.EOutput, IBitOne> BB => mBitArea;
            public IReadOnlyDictionary<SignalNames.Bit.EOutputGroup, IBitGroup> BG => mBitAreaGroup;
            public IReadOnlyDictionary<SignalNames.Word.EOutput, IBitOne> WB => mWordArea;
            public IReadOnlyDictionary<SignalNames.Word.EOutputGroup, IBitGroup> WG => mWordAreaGroup;
            private readonly Dictionary<SignalNames.Bit.EOutput, IBitOne> mBitArea = new Dictionary<SignalNames.Bit.EOutput, IBitOne>();
            private readonly Dictionary<SignalNames.Bit.EOutputGroup, IBitGroup> mBitAreaGroup = new Dictionary<SignalNames.Bit.EOutputGroup, IBitGroup>();
            private readonly Dictionary<SignalNames.Word.EOutput, IBitOne> mWordArea = new Dictionary<SignalNames.Word.EOutput, IBitOne>();
            private readonly Dictionary<SignalNames.Word.EOutputGroup, IBitGroup> mWordAreaGroup = new Dictionary<SignalNames.Word.EOutputGroup, IBitGroup>();

            public OutputHandler(CDocument document, string signalNamePrefix, bool bIsVirtual)
            {
                //////////////////////////////////////////////////////////////////////////
                // Bit Area
                foreach (SignalNames.Bit.EOutput index in Enum.GetValues(typeof(SignalNames.Bit.EOutput)))
                {
                    mBitArea.Add(index, new BitAreaBitOne(document, $"{signalNamePrefix}_{index}", bIsVirtual));
                }
                // Bit Area Group
                foreach (SignalNames.Bit.EOutputGroup index in Enum.GetValues(typeof(SignalNames.Bit.EOutputGroup)))
                {
                    int signalCount = SignalNames.Bit.OutputGroupSignalCount[index];
                    string[] signalNames = new string[signalCount];
                    for (int i = 0; i < signalCount; i++)
                    {
                        signalNames[i] = $"{signalNamePrefix}_{index}_{i + 1}";
                    }
                    mBitAreaGroup.Add(index, new BitAreaBitGroup(document, signalNames, bIsVirtual));
                }
                // Word Area
                foreach (SignalNames.Word.EOutput index in Enum.GetValues(typeof(SignalNames.Word.EOutput)))
                {
                    mWordArea.Add(index, new WordAreaBitOne(document, $"{signalNamePrefix}_{index}", bIsVirtual));
                }
                // Word Area Group
                foreach (SignalNames.Word.EOutputGroup index in Enum.GetValues(typeof(SignalNames.Word.EOutputGroup)))
                {
                    int signalCount = SignalNames.Word.OutputGroupSignalCount[index];
                    string[] signalNames = new string[signalCount];
                    for (int i = 0; i < signalCount; i++)
                    {
                        signalNames[i] = $"{signalNamePrefix}_{index}_{i + 1}";
                    }
                    mWordAreaGroup.Add(index, new WordAreaBitGroup(document, signalNames, bIsVirtual));
                }
                //////////////////////////////////////////////////////////////////////////
            }
        }
    }
}

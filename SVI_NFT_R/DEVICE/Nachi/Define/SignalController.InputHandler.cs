using System;
using System.Collections.Generic;
namespace SVI_NFT_R.DEVICE.Nachi
{
    public partial class SignalController
    {
        private sealed class InputHandler : IInputHandler
        {
            public IReadOnlyDictionary<SignalNames.Bit.EInput, IReadOnlyBitOne> BB => mBitArea;
            public IReadOnlyDictionary<SignalNames.Bit.EInputGroup, IReadOnlyBitGroup> BG => mBitAreaGroup;
            public IReadOnlyDictionary<SignalNames.Word.EInput, IReadOnlyBitOne> WB => mWordArea;
            public IReadOnlyDictionary<SignalNames.Word.EInputGroup, IReadOnlyBitGroup> WG => mWordAreaGroup;
            private readonly Dictionary<SignalNames.Bit.EInput, IReadOnlyBitOne> mBitArea = new Dictionary<SignalNames.Bit.EInput, IReadOnlyBitOne>();
            private readonly Dictionary<SignalNames.Bit.EInputGroup, IReadOnlyBitGroup> mBitAreaGroup = new Dictionary<SignalNames.Bit.EInputGroup, IReadOnlyBitGroup>();
            private readonly Dictionary<SignalNames.Word.EInput, IReadOnlyBitOne> mWordArea = new Dictionary<SignalNames.Word.EInput, IReadOnlyBitOne>();
            private readonly Dictionary<SignalNames.Word.EInputGroup, IReadOnlyBitGroup> mWordAreaGroup = new Dictionary<SignalNames.Word.EInputGroup, IReadOnlyBitGroup>();

            public InputHandler(CDocument document, string signalNamePrefix, bool bIsVirtual)
            {
                //////////////////////////////////////////////////////////////////////////
                // Bit Area
                foreach (SignalNames.Bit.EInput index in Enum.GetValues(typeof(SignalNames.Bit.EInput)))
                {
                    mBitArea.Add(index, new BitAreaBitOne(document, $"{signalNamePrefix}_{index}", bIsVirtual));
                }
                // Bit Area Group
                foreach (SignalNames.Bit.EInputGroup index in Enum.GetValues(typeof(SignalNames.Bit.EInputGroup)))
                {
                    int signalCount = SignalNames.Bit.InputGroupSignalCount[index];
                    string[] signalNames = new string[signalCount];
                    for (int i = 0; i < signalCount; i++)
                    {
                        signalNames[i] = $"{signalNamePrefix}_{index}_{i + 1}";
                    }
                    mBitAreaGroup.Add(index, new BitAreaBitGroup(document, signalNames, bIsVirtual));
                }
                // Word Area
                foreach (SignalNames.Word.EInput index in Enum.GetValues(typeof(SignalNames.Word.EInput)))
                {
                    mWordArea.Add(index, new WordAreaBitOne(document, $"{signalNamePrefix}_{index}", bIsVirtual));
                }
                // Word Area Group
                foreach (SignalNames.Word.EInputGroup index in Enum.GetValues(typeof(SignalNames.Word.EInputGroup)))
                {
                    int signalCount = SignalNames.Word.InputGroupSignalCount[index];
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

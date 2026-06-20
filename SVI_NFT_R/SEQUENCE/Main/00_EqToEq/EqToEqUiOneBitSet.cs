using ENC.IO.Common;

namespace EqToEq
{
    public sealed class EqToEqUiOneBitSet
    {
        public bool IsReadOnly { get; }
        public string Name => HandlingData.Information.ID;
        public string Label => $"B{HandlingData.StartByteIndex * 8 + HandlingData.StartBitIndex:X4}";
        public string Comment => HandlingData.Information.Comment;
        public string Description => HandlingData.Information.Description;
        public bool Value { get => HandlingData.Value; set => HandlingData.Value = value; }
        public IHandlingData<bool> HandlingData { get; }

        public EqToEqUiOneBitSet(IHandlingData<bool> handlingData, bool bIsReadOnly)
        {
            IsReadOnly = bIsReadOnly;
            HandlingData = handlingData;
        }

        public bool Toggle()
        {
            bool setValue = !HandlingData.Value;
            HandlingData.Value = setValue;
            return setValue;
        }
    }
}

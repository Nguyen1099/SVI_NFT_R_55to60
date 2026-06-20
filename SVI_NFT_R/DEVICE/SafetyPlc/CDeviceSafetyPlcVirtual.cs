using System;

namespace HLDevice.SafetyPLC
{
    public class CDeviceSafetyPlcVirtual : Abstract.CDeviceSafetyPlcAbstract
    {
        public override bool TryReadSignatureCode(out ISafetyPlcSignatureData data)
        {
            data = new ImplSafetyPlcSignatureData()
            {
                IsValid = true,
                ErrorReason = string.Empty,
                SignatureCode = string.Empty,
                LastModifyDateTime = DateTime.MinValue
            };
            return true;
        }
    }
}

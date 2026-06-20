using DeviceDLL.SafetyPlc.Omron.G9SP;

namespace HLDevice.SafetyPLC
{
    public class CDeviceSafetyPlcOmronG9SP : Abstract.CDeviceSafetyPlcAbstract
    {
        public readonly DeviceSafetyPlcOmronG9SP mSafetyPlc = new DeviceSafetyPlcOmronG9SP();
        private readonly CSafetyPlcError mError = new CSafetyPlcError();

        public override bool HLInitialize(CInitializeParameter initializeParameter)
        {
            bool bReturn = false;

            do
            {
                InitializeParameter parameter = new InitializeParameter();
                parameter.RemoteHostName = initializeParameter.RemoteHostName;
                parameter.RemoteHostPort = initializeParameter.RemoteHostPort;
                parameter.NodeSafetyPlc = initializeParameter.NodeSafetyPlc;
                parameter.NodeSelf = initializeParameter.NodeSelf;
                parameter.ReceiveTimeout = initializeParameter.ReceiveTimeout;
                if (mSafetyPlc.Initialize(parameter) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override void HLDeInitialize()
        {
            mSafetyPlc.DeInitialize();
        }

        public override string HLGetVersion()
        {
            return mSafetyPlc.GetVersion();
        }

        public override CSafetyPlcError HLGetErrorCode()
        {
            return (CSafetyPlcError)mError.Clone();
        }

        public override bool TryReadSignatureCode(out ISafetyPlcSignatureData data)
        {
            DeviceDLL.SafetyPlc.Omron.G9SP.Define.ISafetyPlcSignatureData readData;
            if (mSafetyPlc.TryReadSafetyPlcSignatureData(out readData) == false)
            {
                data = new ImplSafetyPlcSignatureData();
                MakeError();
                return false;
            }
            data = new ImplSafetyPlcSignatureData()
            {
                IsValid = readData.IsValid,
                ErrorReason = readData.ErrorReason,
                LastModifyDateTime = readData.LastModifyDateTime,
                SignatureCode = readData.SignatureCode
            };
            return true;
        }

        public override DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return mSafetyPlc;
        }

        public override DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return mSafetyPlc;
        }

        private void MakeError()
        {
            var errorCode = mSafetyPlc.GetErrorCode();
            mError.ReturnCode = errorCode.ReturnCode;
            mError.EventTime = errorCode.EventTime;
            mError.FunctionName = errorCode.FunctionName;
            mError.Message = errorCode.Message;
        }
    }
}

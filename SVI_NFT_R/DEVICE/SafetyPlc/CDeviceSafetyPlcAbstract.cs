using System;

namespace HLDevice.Abstract
{
    public abstract class CDeviceSafetyPlcAbstract
    {
        public class CInitializeParameter : ICloneable
        {
            public string RemoteHostName;
            public int RemoteHostPort;
            public byte NodeSafetyPlc;
            public byte NodeSelf;
            public TimeSpan ReceiveTimeout;

            public object Clone()
            {
                return new CInitializeParameter
                {
                    RemoteHostName = RemoteHostName,
                    RemoteHostPort = RemoteHostPort,
                    NodeSafetyPlc = NodeSafetyPlc,
                    NodeSelf = NodeSelf,
                    ReceiveTimeout = ReceiveTimeout
                };
            }
        }

        public class CSafetyPlcError : ICloneable
        {
            public string EventTime;
            public string FunctionName;
            public int ReturnCode;
            public string Message;

            public virtual object Clone()
            {
                return new CSafetyPlcError
                {
                    EventTime = EventTime,
                    FunctionName = FunctionName,
                    ReturnCode = ReturnCode,
                    Message = Message
                };
            }
        }

        public interface ISafetyPlcSignatureData
        {
            bool IsValid { get; }
            string ErrorReason { get; }
            string SignatureCode { get; }
            DateTime LastModifyDateTime { get; }
        }

        protected class ImplSafetyPlcSignatureData : ISafetyPlcSignatureData
        {
            public bool IsValid { get; set; } = false;
            public string ErrorReason { get; set; } = string.Empty;
            public string SignatureCode { get; set; } = string.Empty;
            public DateTime LastModifyDateTime { get; set; } = DateTime.MinValue;
        }

        public virtual bool HLInitialize(CInitializeParameter objInitializeParameter)
        {
            return true;
        }

        public virtual void HLDeInitialize()
        {
        }

        public virtual string HLGetVersion() => "0.0.0.0";

        public virtual CSafetyPlcError HLGetErrorCode()
        {
            CSafetyPlcError objError = new CSafetyPlcError();
            return objError;
        }

        public virtual bool TryReadSignatureCode(out ISafetyPlcSignatureData data)
        {
            data = new ImplSafetyPlcSignatureData();
            return true;
        }

        public virtual DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return null;
        }

        public virtual DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return null;
        }
    }
}

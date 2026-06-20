using HLDevice.Abstract;
using HLDevice.SafetyPLC;
using SVI_NFT_R;
using System;
using static HLDevice.Abstract.CDeviceSafetyPlcAbstract;

namespace HLDevice
{
    public class CDeviceSafetyPlc
    {
        private CDeviceSafetyPlcAbstract mSafetyPlc;
        private CDocument m_objDocument;

        public bool HLInitialize(CDocument document, CConfig.CSafetyPlcParameter parameter)
        {
            m_objDocument = document;

            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                mSafetyPlc = new CDeviceSafetyPlcOmronG9SP();
            }
            else
            {
                mSafetyPlc = new CDeviceSafetyPlcOmronG9SP();
            }

            CInitializeParameter initializeParameter = new CInitializeParameter();
            initializeParameter.RemoteHostName = parameter.RemoteHostName;
            initializeParameter.RemoteHostPort = parameter.RemoteHostPort;
            initializeParameter.NodeSafetyPlc = parameter.NodeSafetyPlc;
            initializeParameter.NodeSelf = parameter.NodeSelf;
            initializeParameter.ReceiveTimeout = TimeSpan.FromMilliseconds(1000d);
            return mSafetyPlc.HLInitialize(initializeParameter);
        }

        public void HLDeInitialize()
        {
            mSafetyPlc.HLDeInitialize();
        }

        public string HLGetVersion()
        {
            return mSafetyPlc.HLGetVersion();
        }

        public CSafetyPlcError HLGetErrorCode()
        {
            return mSafetyPlc.HLGetErrorCode();
        }

        public bool TryReadSignatureCode(out ISafetyPlcSignatureData data)
        {
            return mSafetyPlc.TryReadSignatureCode(out data);
        }

        public DeviceCommon.IHaveErrorLogEvent GetErrorLogInstanceOrNull()
        {
            return mSafetyPlc.GetErrorLogInstanceOrNull();
        }

        public DeviceCommon.IHaveCommLogEvent GetCommLogInstanceOrNull()
        {
            return mSafetyPlc.GetCommLogInstanceOrNull();
        }
    }
}

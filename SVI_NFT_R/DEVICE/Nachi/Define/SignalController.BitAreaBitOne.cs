using HLDevice;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public partial class SignalController
    {
        private sealed class BitAreaBitOne : IBitOne, IReadOnlyBitOne, IHaveLogEvent
        {
            public bool Value
            {
                get
                {
                    bool currentData;
                    mCCLink.HLGetInterfaceBit(SignalName, out currentData);
                    return currentData;
                }
                set
                {
                    bool currentData;
                    mCCLink.HLGetInterfaceBit(SignalName, out currentData);
                    if (currentData != value)
                    {
                        mCCLink.HLSetInterfaceBit(SignalName, value);
                        raiseOnEventOccured($"[Set] {SignalName}:{value}");
                    }
                }
            }
            public string SignalName { get; private set; }
            public event EventHandler<string> OnEventOccured;
            private readonly CDocument mDocument;
            private readonly CDeviceInterface mCCLink;
            private readonly bool mbIsVirtual;

            public BitAreaBitOne(CDocument document, string signalName, bool bIsVirtual)
            {
                mDocument = document;
                mCCLink = bIsVirtual ? mDocument.m_objProcessMain.GetVirtualCcLinkVer2() : mDocument.m_objProcessMain.m_objCCLinkVer2;
                SignalName = signalName;
                mbIsVirtual = bIsVirtual;
            }

            public void Set()
            {
                Value = true;
            }

            public void Clear()
            {
                Value = false;
            }

            public bool Toggle()
            {
                bool setValue = !Value;
                Value = setValue;
                return setValue;
            }

            public bool WaitForTargetValue(bool targetValue, TimeSpan timeout, Func<bool> cancel)
            {
                raiseOnEventOccured($"[Begin] {nameof(WaitForTargetValue)}[{SignalName}] Target:{targetValue}");
                if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || mbIsVirtual == true
                    )
                {
                    Value = targetValue;
                }
                bool bCancel = cancel.Invoke();
                bool bResult = SpinWait.SpinUntil(() =>
                {
                    bCancel = cancel.Invoke();
                    if (bCancel == true) return true;
                    if (Value != targetValue) return false;
                    return true;
                }, timeout);

                string resultTitle;
                if (bCancel == true)
                {
                    resultTitle = "Cancel";
                }
                else
                {
                    resultTitle = bResult ? "End" : "Failed";
                }
                raiseOnEventOccured($"[{resultTitle}] {nameof(WaitForTargetValue)}[{SignalName}]");
                if (bCancel == true)
                {
                    return false;
                }
                return bResult;
            }

            public Task OnePulseAsync(bool pulseSignal, TimeSpan pulseWidth)
            {
                return Task.Factory.StartNew(() =>
                {
                    if (pulseSignal == Value)
                    {
                        Value = !pulseSignal;
                        Thread.Sleep(pulseWidth);
                    }
                    Value = pulseSignal;
                    Thread.Sleep(pulseWidth);
                    Value = !pulseSignal;
                });
            }

            private void raiseOnEventOccured(string logMessage)
            {
                OnEventOccured?.Invoke(this, logMessage);
            }
        }
    }

}

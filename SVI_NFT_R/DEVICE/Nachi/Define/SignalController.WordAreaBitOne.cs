using HLDevice;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public partial class SignalController
    {
        private sealed class WordAreaBitOne : IBitOne, IReadOnlyBitOne, IHaveLogEvent
        {
            public bool Value
            {
                get
                {
                    int currentData;
                    mCCLink.HLGetInterfaceValue(SignalName, out currentData);
                    int bitMask = 1 << StartOffset;
                    int maskingData = currentData & bitMask;
                    return maskingData == bitMask;
                }
                set
                {
                    if (value == true)
                    {
                        int currentData;
                        int bitMask = 1 << StartOffset;
                        lock (mWordAreaWritingLock)
                        {
                            mCCLink.HLGetInterfaceValue(SignalName, out currentData);
                            int maskingData = currentData & bitMask;
                            if (maskingData != bitMask)
                            {
                                currentData |= bitMask;
                                mCCLink.HLSetInterfaceValue(SignalName, currentData);
                                raiseOnEventOccured($"[Set] {SignalName}:{value}");
                            }
                        }
                    }
                    else
                    {
                        int currentData;
                        int bitMask = 1 << StartOffset;
                        lock (mWordAreaWritingLock)
                        {
                            mCCLink.HLGetInterfaceValue(SignalName, out currentData);
                            int maskingData = currentData & bitMask;
                            if (maskingData != 0)
                            {
                                currentData &= ~bitMask;
                                mCCLink.HLSetInterfaceValue(SignalName, currentData);
                                raiseOnEventOccured($"[Set] {SignalName}:{value}");
                            }
                        }
                    }
                }
            }
            public string SignalName { get; private set; }
            public int StartOffset { get; private set; }
            public event EventHandler<string> OnEventOccured;
            private readonly CDocument mDocument;
            private readonly CDeviceInterface mCCLink;
            private readonly object mWordAreaWritingLock;
            private readonly bool mbIsVirtual;

            public WordAreaBitOne(CDocument document, string signalName, bool bIsVirtual)
            {
                mDocument = document;
                mCCLink = bIsVirtual ? mDocument.m_objProcessMain.GetVirtualCcLinkVer2() : mDocument.m_objProcessMain.m_objCCLinkVer2;
                SignalName = signalName;
                StartOffset = Convert.ToInt32(mCCLink.GetInitializeParameter.objInterfaceParameterAnalog[SignalName].iStartOffset);
                mWordAreaWritingLock = GetWordWriteLockObject(mCCLink.GetInitializeParameter.objInterfaceParameterAnalog[SignalName].iDataAddress);
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
                bool setValue;
                lock (mWordAreaWritingLock)
                {
                    setValue = Value;
                    Value = !setValue;
                }
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

using HLDevice;
using System;
using System.Diagnostics;
using System.Threading;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public partial class SignalController
    {
        private sealed class BitAreaBitGroup : IBitGroup, IReadOnlyBitGroup, IHaveLogEvent
        {
            public int Value
            {
                get
                {
                    int result = 0;
                    foreach (var signalName in mHighToLowSignalNames)
                    {
                        result <<= 1;
                        bool currentData;
                        mCCLink.HLGetInterfaceBit(signalName, out currentData);
                        if (true == currentData)
                        {
                            result += 1;
                        }
                    }
                    return result;
                }
                set
                {
                    inputValueRangeCheck(value);
                    bool bChanged = false;
                    int setValue = value;
                    foreach (var signalName in mLowToHighSignalNames)
                    {
                        bool setData = false;
                        if ((setValue & 1) == 1)
                        {
                            setData = true;
                        }
                        bool getData;
                        mCCLink.HLGetInterfaceBit(signalName, out getData);
                        if (getData != setData)
                        {
                            mCCLink.HLSetInterfaceBit(signalName, setData);
                            bChanged = true;
                        }
                        setValue >>= 1;
                    }
                    if (bChanged == true)
                    {
                        raiseOnEventOccured($"[Set] {SignalNames[0]}:{value}");
                    }
                }
            }
            public string[] SignalNames { get; private set; }
            public int MaxValue { get; private set; }
            public event EventHandler<string> OnEventOccured;
            private readonly string[] mLowToHighSignalNames;
            private readonly string[] mHighToLowSignalNames;
            private readonly CDocument mDocument;
            private readonly CDeviceInterface mCCLink;
            private readonly bool mbIsVirtual;

            public BitAreaBitGroup(CDocument document, string[] signalNames, bool bIsVirtual)
            {
                mbIsVirtual = bIsVirtual;
                mDocument = document;
                mCCLink = bIsVirtual ? mDocument.m_objProcessMain.GetVirtualCcLinkVer2() : mDocument.m_objProcessMain.m_objCCLinkVer2;
                SignalNames = signalNames;
                mLowToHighSignalNames = SignalNames;
                mHighToLowSignalNames = new string[mLowToHighSignalNames.Length];
                SignalNames.CopyTo(mHighToLowSignalNames, 0);
                Array.Reverse(mHighToLowSignalNames);
                MaxValue = ~(int.MaxValue << SignalNames.Length);
                Debug.Assert(signalNames.Length <= 32, "그룹화 할 수 있는 크기는 1 DWORD를 초과 할 수 없습니다.");
            }

            public bool WaitForTargetValue(int targetValue, TimeSpan timeout, Func<bool> cancel)
            {
                inputValueRangeCheck(targetValue);
                raiseOnEventOccured($"[Begin] {nameof(WaitForTargetValue)}[{SignalNames[0]}] Target:{targetValue}");
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
                raiseOnEventOccured($"[{resultTitle}] {nameof(WaitForTargetValue)}[{SignalNames[0]}]");
                if (bCancel == true)
                {
                    return false;
                }
                return bResult;
            }

            private void inputValueRangeCheck(int input)
            {
                Debug.Assert(input <= MaxValue, "입력값 범위 초과");
                Debug.Assert(input >= 0, "입력값 범위 초과");
            }

            private void raiseOnEventOccured(string logMessage)
            {
                OnEventOccured?.Invoke(this, logMessage);
            }
        }
    }

}

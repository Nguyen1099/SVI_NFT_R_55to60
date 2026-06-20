using HLDevice;
using System;
using System.Diagnostics;
using System.Threading;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public partial class SignalController
    {
        private sealed class WordAreaBitGroup : IBitGroup, IReadOnlyBitGroup, IHaveLogEvent
        {
            public int Value
            {
                get
                {
                    int currentData;
                    mCCLink.HLGetInterfaceValue(SignalNames[0], out currentData);
                    currentData >>= StartOffset;
                    currentData &= MaxValue;
                    return currentData;
                }
                set
                {
                    inputValueRangeCheck(value);
                    int clearBitMask = ~((~(-1 << SignalNames.Length)) << StartOffset);
                    int setBitMask = value << StartOffset;
                    int currentData;
                    bool bChanged = false;
                    lock (mWordAreaWritingLock)
                    {
                        mCCLink.HLGetInterfaceValue(SignalNames[0], out currentData);
                        int backupData = currentData;
                        currentData &= clearBitMask;
                        currentData |= setBitMask;
                        mCCLink.HLSetInterfaceValue(SignalNames[0], currentData);
                        if (backupData != currentData)
                        {
                            bChanged = true;
                        }
                    }
                    if (bChanged == true)
                    {
                        raiseOnEventOccured($"[Set] {SignalNames[0]}:{value}");
                    }
                }
            }
            public string[] SignalNames { get; private set; }
            public int StartOffset { get; private set; }
            public int MaxValue { get; private set; }
            public event EventHandler<string> OnEventOccured;
            private readonly string[] mLowToHighSignalNames;
            private readonly string[] mHighToLowSignalNames;
            private readonly CDocument mDocument;
            private readonly CDeviceInterface mCCLink;
            private readonly object mWordAreaWritingLock;
            private readonly bool mbIsVirtual;

            public WordAreaBitGroup(CDocument document, string[] signalNames, bool bIsVirtual)
            {
                Debug.Assert(signalNames.Length <= 16, "그룹화 할 수 있는 크기는 1 WORD를 초과 할 수 없습니다.");
                mDocument = document;
                mCCLink = bIsVirtual ? mDocument.m_objProcessMain.GetVirtualCcLinkVer2() : mDocument.m_objProcessMain.m_objCCLinkVer2;
                SignalNames = signalNames;
                mLowToHighSignalNames = SignalNames;
                mHighToLowSignalNames = new string[mLowToHighSignalNames.Length];
                SignalNames.CopyTo(mHighToLowSignalNames, 0);
                Array.Reverse(mHighToLowSignalNames);
                MaxValue = ~(int.MaxValue << SignalNames.Length);
                mbIsVirtual = bIsVirtual;

                var initializeParameter = mCCLink.GetInitializeParameter;
                int dataAddress = initializeParameter.objInterfaceParameterAnalog[SignalNames[0]].iDataAddress;
                int minValue = int.MaxValue;
                foreach (var signalName in SignalNames)
                {
                    if (initializeParameter.objInterfaceParameterAnalog.ContainsKey(signalName) == false)
                    {
                        return;
                    }
                    Debug.Assert(dataAddress == initializeParameter.objInterfaceParameterAnalog[signalName].iDataAddress, "한 워드 영역에 대해서만 그룹화 할 수 있습니다.");
                    if (minValue > initializeParameter.objInterfaceParameterAnalog[signalName].iStartOffset)
                    {
                        minValue = Convert.ToInt32(initializeParameter.objInterfaceParameterAnalog[signalName].iStartOffset);
                    }
                }
                StartOffset = minValue;
                mWordAreaWritingLock = GetWordWriteLockObject(dataAddress);
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

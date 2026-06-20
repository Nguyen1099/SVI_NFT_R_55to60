using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        public sealed class DigitalSignalObserver
        {
            public bool IsInitialized { get; private set; } = false;
            public IReadOnlyList<IDigitalSignalObserverItem> ObserverList => mObserverList;
            public event EventHandler<IDigitalSignalObserverItem> SignalChanged;
            private sealed class InternalDigitalSignalObserverItem : IDigitalSignalObserverItem
            {
                public bool LastSignal { get; private set; } = false;
                public object Tag { get; private set; } = null;
                public string Name { get; private set; } = string.Empty;
                public string Comment { get; private set; } = string.Empty;
                public Func<bool> SignalGetter { get; private set; } = null;

                public InternalDigitalSignalObserverItem(object tag, string name, string comment, Func<bool> signalGetter, bool lastSignal = false)
                {
                    Tag = tag;
                    Name = name;
                    Comment = comment;
                    LastSignal = lastSignal;
                    SignalGetter = signalGetter;
                }

                public bool GetSignalChanged()
                {
                    bool bChanged = false;
                    var captureEvent = SignalGetter;
                    if (captureEvent != null)
                    {
                        bool getSignal = captureEvent.Invoke();
                        bChanged = LastSignal != getSignal;
                        if (bChanged == true)
                        {
                            LastSignal = getSignal;
                        }
                    }
                    return bChanged;
                }
            }
            private List<InternalDigitalSignalObserverItem> mObserverList = new List<InternalDigitalSignalObserverItem>();
            private bool mbShouldStop = false;
            private Thread mThreadSignalObserver;
            private int mPollingTime;

            public bool Initialize(IEnumerable<DigitalSignalObserverOption> options, int pollingTime = 10)
            {
                if (IsInitialized == true)
                {
                    return false;
                }

                Utils.InRange(ref pollingTime, 0, int.MaxValue);
                mPollingTime = pollingTime;
                mObserverList = options.Select(item => new InternalDigitalSignalObserverItem(item.Tag, item.Name, item.Comment, item.SignalGetter)).ToList();

                mbShouldStop = false;
                mThreadSignalObserver = new Thread(threadSignalObserver);
                mThreadSignalObserver.Start();

                IsInitialized = true;
                return true;
            }

            public void DeInitialize()
            {
                if (IsInitialized == false)
                {
                    return;
                }

                mbShouldStop = true;
                mThreadSignalObserver.Join();

                IsInitialized = false;
            }

            private void threadSignalObserver()
            {
                // 초기값 설정
                for (int i = 0; i < mObserverList.Count; i++)
                {
                    mObserverList[i].GetSignalChanged();
                }

                while (mbShouldStop == false)
                {
                    Thread.Sleep(mPollingTime);
                    for (int i = 0; i < mObserverList.Count; i++)
                    {
                        if (mObserverList[i].GetSignalChanged() == true)
                        {
                            raiseSignalChangedEvent(mObserverList[i]);
                        }
                    }
                }
            }

            private void raiseSignalChangedEvent(InternalDigitalSignalObserverItem item)
            {
                SignalChanged?.Invoke(this, new InternalDigitalSignalObserverItem(item.Tag, item.Name, item.Comment, null, item.LastSignal));
            }
        }

        public interface IDigitalSignalObserverItem
        {
            object Tag { get; }
            string Name { get; }
            string Comment { get; }
            bool LastSignal { get; }
        }

        public sealed class DigitalSignalObserverOption
        {
            public object Tag { get; set; } = null;
            public string Name { get; set; } = string.Empty;
            public string Comment { get; set; } = string.Empty;
            public Func<bool> SignalGetter { get; set; } = null;

            public DigitalSignalObserverOption(object tag, string name, string comment, Func<bool> signalGetter)
            {
                Tag = tag;
                Name = name;
                Comment = comment;
                SignalGetter = signalGetter;
            }
        }
    }

}

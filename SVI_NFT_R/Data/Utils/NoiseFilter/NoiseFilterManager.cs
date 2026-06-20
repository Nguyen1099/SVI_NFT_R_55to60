using System;
using System.Collections.Generic;
using System.Threading;

namespace SVI_NFT_R
{
    public static class NoiseFilterManager
    {
        public static bool IsInitialized { get; private set; } = false;
        public static IReadOnlyDictionary<string, NoiseFilter> Items => mItems;
        private static object mLock = new object();
        private static readonly Dictionary<string, NoiseFilter> mItems = new Dictionary<string, NoiseFilter>();
        private static readonly Thread mThreadUpdate = new Thread(threadUpdate);
        private static bool mbThreadExit = false;

        public static bool Initialize()
        {
            if (IsInitialized == true)
            {
                return false;
            }

            mbThreadExit = false;
            mThreadUpdate.IsBackground = true;
            mThreadUpdate.Start();

            IsInitialized = true;
            return true;
        }

        public static void DeInitialize()
        {
            if (IsInitialized == false)
            {
                return;
            }

            mbThreadExit = true;
            mThreadUpdate.Join();

            IsInitialized = false;
        }

        public static void Add(string id, Func<bool> getSignal, TimeSpan filterTime, FilterOptionFlags filterOption = FilterOptionFlags.RisingEdge | FilterOptionFlags.FallingEdge)
        {
            lock (mLock)
            {
                mItems.Add(id, new NoiseFilter(getSignal, filterTime, filterOption));
            }
        }

        public static void Remove(string id)
        {
            lock (mLock)
            {
                if (mItems.ContainsKey(id) == true)
                {
                    mItems.Remove(id);
                }
            }
        }

        private static void threadUpdate()
        {
            SpinWait spinWait = new SpinWait();
            while (mbThreadExit == false)
            {
                lock (mLock)
                {
                    foreach (var item in mItems)
                    {
                        item.Value.Update();
                        spinWait.SpinOnce();
                    }
                }
                Thread.Sleep(5);
                spinWait.Reset();
            }
        }
    }
}

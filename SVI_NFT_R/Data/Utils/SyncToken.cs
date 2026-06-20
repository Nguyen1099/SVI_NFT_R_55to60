using System.Threading;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        public class SyncToken
        {
            public ManualResetEventSlim SyncRoot => mSyncRoot;
            private readonly ManualResetEventSlim mSyncRoot = new ManualResetEventSlim(true);

            public void WaitingUnlock()
            {
                mSyncRoot.Wait();
            }

            public void Lock()
            {
                mSyncRoot.Reset();
            }

            public void Unlock()
            {
                mSyncRoot.Set();
            }

            public bool IsLock()
            {
                return mSyncRoot.IsSet == false;
            }
        }
    }
}

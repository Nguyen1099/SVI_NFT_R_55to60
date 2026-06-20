namespace SVI_NFT_R
{
    public static partial class InterferenceRegion
    {
        private sealed class ImplInterferenceRegionPermit : IInterferenceRegionPermit
        {
            public string Name => mName;
            public bool IsEnter => mbIsEnter;
            private readonly string mName;
            private bool mbIsEnter = false;
            private readonly object mSyncRoot = new object();

            public ImplInterferenceRegionPermit(string name)
            {
                mName = name;
            }

            public bool TryEnter()
            {
                // 이미 다른 곳에서 간섭 영역을 점유하고 있는 경우
                if (mbIsEnter == true)
                {
                    return false;
                }

                lock (mSyncRoot)
                {
                    // 경쟁 상태에서 다른 곳에서 간섭 영역을 먼저 점유한 경우
                    if (mbIsEnter == true)
                    {
                        return false;
                    }
                    mbIsEnter = true;
                }

                return true;
            }

            public void Exit()
            {
                mbIsEnter = false;
            }
        }
    }

}
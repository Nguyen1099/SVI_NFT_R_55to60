using System;

namespace SVI_NFT_R
{
    public class NoiseFilter
    {
        public TimeSpan FilterTime { get; set; } = TimeSpan.Zero;
        public FilterOptionFlags FilterOption { get; set; } = FilterOptionFlags.RisingEdge & FilterOptionFlags.FallingEdge;
        public bool Signal => mbLastSignal;
        public int RisingEdgeCount { get; set; } = 0;
        public int FallingEdgeCount { get; set; } = 0;
        private TimeSpan mDelayTime => DateTime.Now - mChangeStartTime;
        private bool mbLastSignal;
        private Func<bool> mGetSignal;
        private DateTime mChangeStartTime = DateTime.MinValue;

        public NoiseFilter(Func<bool> getSignal, TimeSpan filterTime, FilterOptionFlags filterOption = FilterOptionFlags.RisingEdge | FilterOptionFlags.FallingEdge)
        {
            mGetSignal = getSignal;
            FilterTime = filterTime;
            FilterOption = filterOption;
            mbLastSignal = mGetSignal.Invoke();
            RisingEdgeCount = 0;
            FallingEdgeCount = 0;
        }

        public void Update()
        {
            bool bSignal = mGetSignal.Invoke();
            if (bSignal != mbLastSignal)
            {
                if (mChangeStartTime == DateTime.MinValue)
                {
                    mChangeStartTime = DateTime.Now;
                }
                if (
                    FilterOption.HasFlag(FilterOptionFlags.RisingEdge) == true
                    && bSignal == true
                    )
                {
                    if (mDelayTime > FilterTime)
                    {
                        setLastSignal(bSignal);
                    }
                }
                else if (
                    FilterOption.HasFlag(FilterOptionFlags.FallingEdge) == true
                    && bSignal == false
                    )
                {
                    if (mDelayTime > FilterTime)
                    {
                        setLastSignal(bSignal);
                    }
                }
                else
                {
                    setLastSignal(bSignal);
                }
            }
            else
            {
                if (mChangeStartTime != DateTime.MinValue)
                {
                    mChangeStartTime = DateTime.MinValue;
                }
            }
        }

        private void setLastSignal(bool signal)
        {
            mbLastSignal = signal;
            unchecked
            {
                if (mbLastSignal == true)
                {
                    RisingEdgeCount++;
                }
                else
                {
                    FallingEdgeCount++;
                }
            }
        }
    }
}

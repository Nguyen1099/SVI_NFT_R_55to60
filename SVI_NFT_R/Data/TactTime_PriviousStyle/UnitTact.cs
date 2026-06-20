using System;
using System.Collections.Generic;

namespace SVI_NFT_R.TactTime
{
    public class UnitTact
    {
        public string UnitName { get; set; }
        public TimeSpan PureUnitTactTime
        {
            get
            {
                TimeSpan result = mPureTactTime;
                if (result == TimeSpan.Zero)
                {
                    result = UnitTactTime - InputDelayTime - OutputDelayTime - UserStopTime;
                }
                return result;
            }
            set
            {
                mPureTactTime = value;
            }
        }
        public TimeSpan UnitTactTime { get; set; }
        public TimeSpan InputDelayTime { get; set; }
        public TimeSpan OutputDelayTime { get; set; }
        public TimeSpan UserStopTime { get; set; }
        public Dictionary<string, string> Rawdata { get; set; }
        private TimeSpan mPureTactTime = TimeSpan.Zero;
    }
}

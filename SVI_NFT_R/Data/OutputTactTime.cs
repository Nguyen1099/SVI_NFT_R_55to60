using System;

namespace SVI_NFT_R
{
    [Serializable]
    public class OutputTactTime
    {
        public int CellCount { get; set; }
        public TimeSpan Elapsed { get; set; }
        public TimeSpan WaitLoading { get; set; }
        public TimeSpan WaitUnloading { get; set; }
        public TimeSpan InspectionElapsed { get; set; }
    }
}

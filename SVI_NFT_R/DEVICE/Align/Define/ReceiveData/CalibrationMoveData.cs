using System;

namespace SVI_NFT_R.DEVICE.Align
{
    public partial class ReceiveData
    {
        [Serializable]
        public class CalibrationMoveData : KeepData
        {
            public int Index { get; set; } = 0;
            public int X { get; set; } = 0;
            public int Y { get; set; } = 0;
            public int T { get; set; } = 0;
        }
    }
}
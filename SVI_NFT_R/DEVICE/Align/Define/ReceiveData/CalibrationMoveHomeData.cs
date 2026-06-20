using System;

namespace SVI_NFT_R.DEVICE.Align
{
    public partial class ReceiveData
    {
        [Serializable]
        public class CalibrationMoveHomeData : KeepData
        {
            public int Index { get; set; } = 0;
        }
    }
}
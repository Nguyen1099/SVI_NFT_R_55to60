using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class AlignData : KeepData
        {
            public AlignOffset PositionA { get; set; }
            public AlignOffset PositionB { get; set; }

            public AlignData()
            {
                PositionA = new AlignOffset();
                PositionB = new AlignOffset();
                ResetRegistTime();
            }
        }
    }
}

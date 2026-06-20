using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class AmoGrabEndData : KeepData
        {
            public CellInformation PositionA { get; set; }
            public CellInformation PositionB { get; set; }

            public AmoGrabEndData()
            {
                PositionA = new CellInformation();
                PositionB = new CellInformation();
                ResetRegistTime();
            }
        }
    }
}

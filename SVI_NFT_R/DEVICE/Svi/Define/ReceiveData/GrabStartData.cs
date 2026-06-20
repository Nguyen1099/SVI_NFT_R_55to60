using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class GrabStartData : KeepData
        {
            public CellInformation PositionA { get; set; }
            public CellInformation PositionB { get; set; }
            public int SelectIndex { get; set; }

            public GrabStartData()
            {
                PositionA = new CellInformation();
                PositionB = new CellInformation();
                SelectIndex = 0;
                ResetRegistTime();
            }
        }
    }
}

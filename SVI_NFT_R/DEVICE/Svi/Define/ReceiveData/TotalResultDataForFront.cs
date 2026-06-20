using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class TotalResultDataForFront : KeepData
        {
            public string CellID { get; set; }
            public TotalResult Result { get; set; }

            public TotalResultDataForFront()
            {
                CellID = string.Empty;
                Result = new TotalResult();
                ResetRegistTime();
            }
        }
    }
}

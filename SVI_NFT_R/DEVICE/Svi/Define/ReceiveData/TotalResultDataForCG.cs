using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class TotalResultDataForCG : KeepData
        {
            public string CellID { get; set; }
            public TotalResult Result { get; set; }

            public TotalResultDataForCG()
            {
                CellID = string.Empty;
                Result = new TotalResult();
                ResetRegistTime();
            }
        }
    }
}

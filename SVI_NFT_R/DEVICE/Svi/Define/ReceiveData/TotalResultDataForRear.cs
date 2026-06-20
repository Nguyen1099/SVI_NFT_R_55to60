using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class TotalResultDataForRear : KeepData
        {
            public string InnerID { get; set; }
            public string CellID { get; set; }
            public TotalResult FrontResult { get; set; }
            public TotalResult RearResult { get; set; }

            public TotalResultDataForRear()
            {
                InnerID = string.Empty;
                CellID = string.Empty;
                FrontResult = new TotalResult();
                RearResult = new TotalResult();
                ResetRegistTime();
            }
        }
    }
}

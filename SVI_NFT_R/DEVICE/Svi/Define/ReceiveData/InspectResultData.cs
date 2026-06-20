using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class InspectResultData : KeepData
        {
            public string InnerID { get; set; }
            public string CellID { get; set; }
            public string Result { get; set; }

            public InspectResultData()
            {
                InnerID = string.Empty;
                CellID = string.Empty;
                Result = "BIN2";
                ResetRegistTime();
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class LightDataForDvReport : KeepData
        {
            public string InnerID { get; set; }
            public string CellID { get; set; }
            public Dictionary<string, int> Data { get; set; }

            public LightDataForDvReport()
            {
                CellID = string.Empty;
                InnerID = string.Empty;
                Data = new Dictionary<string, int>();
                ResetRegistTime();
            }
        }
    }
}

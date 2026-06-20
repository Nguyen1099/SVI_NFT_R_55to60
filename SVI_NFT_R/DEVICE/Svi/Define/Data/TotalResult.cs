using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    [Serializable]
    public class TotalResult
    {
        public string Result { get; set; }
        public string[] DefectNames { get; set; }

        public TotalResult()
        {
            Result = "BIN2";
            DefectNames = new string[]
            {
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
            };
        }
    }
}

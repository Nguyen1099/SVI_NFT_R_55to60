using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class AutoReviewData : KeepData
        {
            public string CellID { get; set; }
            public string Result { get; set; }
            public int Count { get { return (null != DefectPoints) ? DefectPoints.Length : 0; } }
            public DefectPoint[] DefectPoints { get; set; }

            public AutoReviewData()
            {
                CellID = string.Empty;
                Result = "BIN1";
                DefectPoints = new DefectPoint[0];
                ResetRegistTime();
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SVI_NFT_R.CellData
{
    public partial class OneCell
    {
        [Serializable]
        public class AlignResultData
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public ECellPlacement CameraIndex { get; set; } = ECellPlacement.P1;
            public double Score { get; set; } = 0d;
            public double RevisionX { get; set; } = 0d;
            public double RevisionY { get; set; } = 0d;
            public double RevisionT { get; set; } = 0d;
            public double TotalRevisionX { get; set; } = 0d;
            public double TotalRevisionY { get; set; } = 0d;
            public double TotalRevisionT { get; set; } = 0d;
            [JsonConverter(typeof(StringEnumConverter))]
            public EJudge Judge { get; set; } = EJudge.None;
            public string ResultImagePath { get; set; } = string.Empty;
            public int RetryCount { get; set; } = 0;
        }
    }

}

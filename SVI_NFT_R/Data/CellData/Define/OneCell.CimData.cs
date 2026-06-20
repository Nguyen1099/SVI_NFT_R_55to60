using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace SVI_NFT_R.CellData
{
    public partial class OneCell
    {
        [Serializable]
        public class CimData
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public ETrackingResult TrackingResult { get; set; } = ETrackingResult.None;
            public string CellLotInformationLotInfoResult { get; set; } = string.Empty;
            public string CellLotInformationDefectResult { get; set; } = string.Empty;
            public string CellLotInformationDefectComent { get; set; } = string.Empty;
            public string CellInformationResult { get; set; } = string.Empty;

            public string ProductType { get; set; } = " ";
            public string ProductKind { get; set; } = " ";
            public string ProductSpec { get; set; } = " ";
            public double CellSize { get; set; } = 0d;
            public double CellThickness { get; set; } = 0d;
            public string Comment { get; set; } = " ";
            public string ReplyStatus { get; set; } = " ";
            public string ReplyCellID { get; set; } = " ";
            public Dictionary<string, string> DvList { get; set; } = new Dictionary<string, string>();
        }
    }

}

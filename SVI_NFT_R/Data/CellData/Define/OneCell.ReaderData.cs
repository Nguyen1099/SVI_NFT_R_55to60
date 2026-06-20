using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SVI_NFT_R.CellData
{
    public partial class OneCell
    {
        [Serializable]
        public class ReaderData
        {
            /// <summary>Cell ID를 읽어오는 방식</summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public EReaderType ReaderType { get; set; } = EReaderType.None;
            /// <summary>MCR Reader의 위치/순서 정보</summary>
            public string ReaderID { get; set; } = string.Empty;
            /// <summary>MCR Reading 결과 값</summary>
            public string ReaderResultCode { get; set; } = string.Empty;
            /// <summary>MCR 재시도 횟수 정보</summary>
            public int RetryCount { get; set; } = 0;
        }
    }

}

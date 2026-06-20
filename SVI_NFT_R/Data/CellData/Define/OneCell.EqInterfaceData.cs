using System;

namespace SVI_NFT_R.CellData
{
    public partial class OneCell
    {
        [Serializable]
        public class EqInterfaceData
        {
            /// <summary> 앞 설비에서 멜섹 인터페이스로 넘어오는 ID </summary>
            public string CellID { get; set; } = string.Empty;
            /// <summary> 앞 설비에서 멜섹 인터페이스로 넘어오는 판정 값 (현재는 없으므로 공백 보냄) </summary>
            public string Judge { get; set; } = string.Empty;
            /// <summary> 앞 설비에서 멜섹 인터페이스로 넘어오는 불량명 (현재는 없으므로 공백 보냄) </summary>
            public string ReasonCode { get; set; } = string.Empty;
        }
    }

}


using System;

namespace SVI_NFT_R.CellData
{
    public partial class OneCell
    {
        [Serializable]
        public class JudgementData
        {
            /// <summary>작업자 ID</summary>
            public string OperatorID { get; set; } = " ";
            /// <summary>제품 판정 값</summary>
            public string Judge { get; set; } = " ";
            /// <summary>사유 코드</summary>
            public string ReasonCode { get; set; } = " ";
            /// <summary>설비 설명, 장비 내 Unit Process No</summary>
            public string Description { get; set; } = " ";
        }
    }

}

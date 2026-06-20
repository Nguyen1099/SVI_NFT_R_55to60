using System;

namespace SVI_NFT_R.CellData
{
    public partial class OneCell
    {
        [Serializable]
        public class WorkOrderData
        {
            /// <summary>진행 Job 정보</summary>
            public string ProcessJob { get; set; } = " ";
            /// <summary>계획 수량 정보</summary>
            public int PlanQty { get; set; } = 0;
            /// <summary>진행 수량 정보</summary>
            public int ProcessedQty { get; set; } = 0;
        }
    }

}

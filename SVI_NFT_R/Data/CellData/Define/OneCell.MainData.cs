using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SVI_NFT_R.CellData
{
    public partial class OneCell
    {
        [Serializable]
        public class MainData
        {
            /// <summary> 설비 내 독립적인 Unique ID </summary>
            public string InnerID { get; set; } = string.Empty;
            /// <summary> Cell 별로 부여된 Unique ID (MCR 리딩값) </summary>
            public string CellID { get; set; } = string.Empty;
            /// <summary> 자재 위치 Right - CN1(A), Left - CN2(B) </summary>
            public string ChannelName { get; set; } = string.Empty;
            /// <summary> Carrier ID </summary>
            public string CarrierID { get; set; } = string.Empty;
            /// <summary> Cell의 제품 정보 </summary>
            public string ProductID { get; set; } = string.Empty;
            /// <summary> Cell의 작업 Unique ID (TC 수신값) </summary>
            public string JobID { get; set; } = string.Empty;
            /// <summary> Cell의 Step Id </summary>
            public string StepID { get; set; } = string.Empty;
            /// <summary> PPID (레시피 ID) </summary>
            public string PPID { get; set; } = string.Empty;
            /// <summary> 설비 최종 판정 결과 (OK, NG) </summary>
            public string MachineResult { get; set; } = string.Empty;
            /// <summary> 투입셔틀 간이 광학계 검사 실행 결과 </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public EStatus SubOpticsInspStatus { get; set; } = EStatus.None;
            /// <summary> 얼라인 실행 결과 </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public EStatus PreAlignStatus { get; set; } = EStatus.None;
            /// <summary> MCR 촬상 실행 결과 </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public EStatus McrStatus { get; set; } = EStatus.None;
            /// <summary> 검사기와 셀데이터 동기화 실행 결과 </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public EStatus InspCellDataSyncStatus { get; set; } = EStatus.None;
            /// <summary> 검사 완료 여부 </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public EStatus InspStatus { get; set; } = EStatus.None;
            /// <summary> 정면 Inspection 결과 </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public EInspectionResult FrontInspResult { get; set; } = EInspectionResult.None;
            /// <summary> 정면 Inspection 리즌 코드 </summary>
            public string[] FrontInspReasonCodes { get; set; } = new string[0];
            /// <summary> 배면 Inspection 결과 </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public EInspectionResult RearInspResult { get; set; } = EInspectionResult.None;
            /// <summary> 배면 Inspection 리즌 코드 </summary>
            public string[] RearInspReasonCodes { get; set; } = new string[0];
            /// <summary> 투입 시간 </summary>
            public DateTime InputTime = DateTime.Now;
            /// <summary> Cylinder Status </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public ECylinderStatus CylinderStatus { get; set; } = ECylinderStatus.None;
        }
    }
}

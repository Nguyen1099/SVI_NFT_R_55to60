using SVI_NFT_R;

namespace Mcc
{
    //public sealed class OutConveyorMccLogItems
    //{
    //    public IMccLogItem[] INIT_UNIT { get; private set; } = new IMccLogItem[2];
    //    public IMccLogItem[] JOB_CHANGE { get; private set; } = new IMccLogItem[2];
    //    public IMccLogItem[] COMPONENT_IN { get; private set; } = new IMccLogItem[2];
    //    public IMccLogItem[] CV_RUN { get; private set; } = new IMccLogItem[2];
    //    public IMccLogItem[] OUT_WAIT_TIME { get; private set; } = new IMccLogItem[2];
    //    public IMccLogItem[] COMPONENT_OUT { get; private set; } = new IMccLogItem[2];
    //    public IMccLogItem[] IN_WAIT_TIME { get; private set; } = new IMccLogItem[2];
    //    public IMccLogItem[] ALARM { get; set; } = new IMccLogItem[2];
    //    public IMccLogItem[] ALARM_STOP { get; set; } = new IMccLogItem[2];

    //    public bool IsInitialized { get; private set; } = false;

    //    public bool Initialize(CDocument document)
    //    {
    //        // 인스턴스 생성
    //        var Modules = new[]
    //            {
    //                UnitName.OutConveyor01,
    //                UnitName.OutConveyor02
    //            };
    //        var LD = new[]
    //            {
    //                new { From = UnitName.OutFlip01, To = Modules[0] },
    //                new { From = UnitName.OutFlip02, To = Modules[1] }
    //            };
    //        var Proc = new[]
    //            {
    //                new { From = Modules[0], To = Modules[0] },
    //                new { From = Modules[1], To = Modules[1] }
    //            };
    //        var ULD = new[]
    //            {
    //                new { From = Modules[0], To = UnitName.Lower01 },
    //                new { From = Modules[1], To = UnitName.Lower02 },
    //            };
    //        string[] systemNoPrefixs = new string[]
    //            {
    //                "1K",
    //                "1L"
    //            };
    //        for (int p = 0; p < 2; p++)
    //        {
    //            int systemNo = 1;
    //            INIT_UNIT[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"INIT_{Modules[p]}", Modules[p], Proc[p].From, Proc[p].To);
    //            JOB_CHANGE[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"JOB_CHANGE", Modules[p], Proc[p].From, Proc[p].To);

    //            systemNo = 10;
    //            COMPONENT_IN[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"COMPONENT_IN", Modules[p], LD[p].From, LD[p].To);

    //            CV_RUN[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"CV_RUN", Modules[p], Proc[p].From, Proc[p].To);
                
    //            OUT_WAIT_TIME[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"OUT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);

    //            COMPONENT_OUT[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"COMPONENT_OUT", Modules[p], ULD[p].From, ULD[p].To);
                
    //            IN_WAIT_TIME[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"IN_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);

    //            ALARM[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"ALARM", Modules[p], Proc[p].From, Proc[p].To);
    //            ALARM_STOP[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"ALARM_STOP", Modules[p], Proc[p].From, Proc[p].To);
    //        }
    //        IsInitialized = true;
    //        return true;
    //    }

    //    public void DeInitialize()
    //    {
    //        IsInitialized = false;
    //    }
    //}
}

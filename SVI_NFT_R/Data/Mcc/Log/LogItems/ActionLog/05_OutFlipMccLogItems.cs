using SVI_NFT_R;
using SVI_NFT_R.CellData;

namespace Mcc
{
    public sealed class OutFlipMccLogItems
    {
        public IMccLogItem[] INIT_UNIT { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] COMPONENT_IN { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] GET_VAC_ON { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] MOVE_ULD_WAIT_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] OUT_WAIT_TIME { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] COMPONENT_OUT { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] PUT_MOVE_ULD_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] PUT_VAC_OFF { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] PUT_MOVE_ULD_UP_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] MOVE_LD_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] IN_WAIT_TIME { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] TR_ACT_WAIT_TIME_COMPONENT_IN { get; set; } = new IMccLogItem[2];
        public IMccLogItem[] ALARM { get; set; } = new IMccLogItem[2];
        public IMccLogItem[] ALARM_STOP { get; set; } = new IMccLogItem[2];

        public bool IsInitialized { get; private set; } = false;

        public bool Initialize(CDocument document)
        {
            // 인스턴스 생성
            var process = EProcess.OutFlip;
            var Modules = new[]
                {
                    UnitName.OutFlip01,
                    UnitName.OutFlip02
                };
            var LD = new[]
                {
                    new { From = UnitName.OutRobot01, To = Modules[0] },
                    new { From = UnitName.OutRobot02, To = Modules[1] }
                };
            var Proc = new[]
                {
                    new { From = Modules[0], To = Modules[0] },
                    new { From = Modules[1], To = Modules[1] }
                };
            var ULD = new[]
                {
                    new { From = Modules[0], To = UnitName.OutShuttle01 },
                    new { From = Modules[1], To = UnitName.OutShuttle02 }
                };
            string[] systemNoPrefixs = new string[]
                {
                    "1I",
                    "1J"
                };
            for (int p = 0; p < 2; p++)
            {
                int systemNo = 1;
                INIT_UNIT[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"INIT", Modules[p], Proc[p].From, Proc[p].To);

                systemNo = 10;
                COMPONENT_IN[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"COMPONENT_IN", Modules[p], LD[p].From, LD[p].To);
                GET_VAC_ON[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"GET_VAC_ON", Modules[p], LD[p].From, LD[p].To);
                if (p == 0)
                {
                    TR_ACT_WAIT_TIME_COMPONENT_IN[1] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"TR52_ACT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
                }

                MOVE_ULD_WAIT_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"MOVE_ULD_WAIT_POS", Modules[p], Proc[p].From, Proc[p].To);
                OUT_WAIT_TIME[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"OUT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);

                COMPONENT_OUT[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"COMPONENT_OUT", Modules[p], ULD[p].From, ULD[p].To);
                PUT_MOVE_ULD_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"PUT_MOVE_ULD_POS", Modules[p], ULD[p].From, ULD[p].To);
                PUT_VAC_OFF[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"PUT_VAC_OFF", Modules[p], ULD[p].From, ULD[p].To);
                PUT_MOVE_ULD_UP_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"PUT_MOVE_ULD_UP_POS", Modules[p], ULD[p].From, ULD[p].To);

                MOVE_LD_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"MOVE_LD_POS", Modules[p], Proc[p].From, Proc[p].To);
                IN_WAIT_TIME[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"IN_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
                if (p == 1)
                {
                    TR_ACT_WAIT_TIME_COMPONENT_IN[0] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"TR51_ACT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
                }

                ALARM[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"ALARM", Modules[p], Proc[p].From, Proc[p].To);
                ALARM_STOP[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"ALARM_STOP", Modules[p], Proc[p].From, Proc[p].To);
            }
            IsInitialized = true;
            return true;
        }

        public void DeInitialize()
        {
            IsInitialized = false;
        }
    }
}

using SVI_NFT_R;
using SVI_NFT_R.CellData;

namespace Mcc
{
    public sealed class InspStageMccLogItems
    {
        public IMccLogItem[] INIT_UNIT { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] COMPONENT_IN { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] GET_VAC_ON { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] CIM_TRACKING_PROCESS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] VISION_INSP_PROCESS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] MOVE_Y_LD_TO_IS21_INSP_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] VISION_OPTIC_INSP_PROCESS_1 { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] MOVE_Y_LD_TO_IS22_INSP_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] MOVE_Y_IS21_INSP_TO_IS22_INSP_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] VISION_OPTIC_INSP_PROCESS_2{ get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] MOVE_Y_IS21_INSP_TO_ULD_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] MOVE_Y_IS22_INSP_TO_ULD_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] OUT_WAIT_TIME { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] COMPONENT_OUT { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] PUT_VAC_OFF { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] MOVE_X_LD_POS { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] IN_WAIT_TIME { get; private set; } = new IMccLogItem[2];
        public IMccLogItem[] TR_ACT_WAIT_TIME_COMPONENT_IN { get; set; } = new IMccLogItem[2];
        public IMccLogItem[] TR_ACT_WAIT_TIME_COMPONENT_OUT { get; set; } = new IMccLogItem[2];
        public IMccLogItem[] ALARM { get; set; } = new IMccLogItem[2];
        public IMccLogItem[] ALARM_STOP { get; set; } = new IMccLogItem[2];
        public bool IsInitialized { get; private set; } = false;

        public bool Initialize(CDocument document)
        {
            // 인스턴스 생성
            var loadProcess = EProcess.InRobot;
            var process = EProcess.InspStage;
            var Modules = new[]
            {
                UnitName.InspectionStage01,
                UnitName.InspectionStage02
            };
            var LD = new[]
            {
                new { From = UnitName.InRobot01, To = Modules[0] },
                new { From = UnitName.InRobot02, To = Modules[1] }
            };
            var Proc = new[]
            {
                new { From = Modules[0], To = Modules[0] },
                new { From = Modules[1], To = Modules[1] }
            };
            var ULD = new[]
            {
                new { From = Modules[0], To = UnitName.OutRobot01 },
                new { From = Modules[1], To = UnitName.OutRobot02 }
            };
            string[] systemNoPrefixs = new string[]
            {
                "1E",
                "1F"
            };

            for (int p = 0; p < 2; ++p)
            {
                int systemNo = 1;
                INIT_UNIT[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"INIT", Modules[p], Proc[p].From, Proc[p].To);

                systemNo = 10;
                COMPONENT_IN[p] = new MccActionLogItem(document, loadProcess, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"COMPONENT_IN", Modules[p], LD[p].From, LD[p].To);
                GET_VAC_ON[p] = new MccActionLogItem(document, loadProcess, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"GET_VAC_ON", Modules[p], LD[p].From, LD[p].To);
                if (p == 0)
                {
                    TR_ACT_WAIT_TIME_COMPONENT_IN[1] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"TR42_ACT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
                }

                CIM_TRACKING_PROCESS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"CIM_TRACKING_PROCESS", Modules[p], Proc[p].From, Proc[p].To);
                VISION_INSP_PROCESS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"VISION_INSP_PROCESS", Modules[p], Proc[p].From, Proc[p].To);
                MOVE_Y_LD_TO_IS21_INSP_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"MOVE_Y_LD_TO_IS21_INSP_POS", Modules[p], Proc[p].From, Proc[p].To);
                VISION_OPTIC_INSP_PROCESS_1[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"VISION_OPTIC_INSP_PROCESS", Modules[p], Proc[p].From, Proc[p].To);
                MOVE_Y_LD_TO_IS22_INSP_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"MOVE_Y_LD_TO_IS22_INSP_POS", Modules[p], Proc[p].From, Proc[p].To);
                MOVE_Y_IS21_INSP_TO_IS22_INSP_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"MOVE_Y_IS21_INSP_TO_IS22_INSP_POS", Modules[p], Proc[p].From, Proc[p].To);
                VISION_OPTIC_INSP_PROCESS_2[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"VISION_OPTIC_INSP_PROCESS", Modules[p], Proc[p].From, Proc[p].To);
                MOVE_Y_IS21_INSP_TO_ULD_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"MOVE_Y_IS21_INSP_TO_ULD_POS", Modules[p], Proc[p].From, Proc[p].To);
                MOVE_Y_IS22_INSP_TO_ULD_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"MOVE_Y_IS22_INSP_TO_ULD_POS", Modules[p], Proc[p].From, Proc[p].To);

                OUT_WAIT_TIME[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"OUT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
                if (p == 1)
                {
                    TR_ACT_WAIT_TIME_COMPONENT_OUT[0] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"TR51_ACT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
                }

                COMPONENT_OUT[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"COMPONENT_OUT", Modules[p], ULD[p].From, ULD[p].To);
                PUT_VAC_OFF[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"PUT_VAC_OFF", Modules[p], ULD[p].From, ULD[p].To);
                if (p == 0)
                {
                    TR_ACT_WAIT_TIME_COMPONENT_OUT[1] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"TR52_ACT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
                }

                MOVE_X_LD_POS[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"MOVE_Y_LD_POS", Modules[p], Proc[p].From, Proc[p].To);
                IN_WAIT_TIME[p] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"IN_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
                if (p == 1)
                {
                    TR_ACT_WAIT_TIME_COMPONENT_IN[0] = new MccActionLogItem(document, process, p, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"TR41_ACT_WAIT_TIME", Modules[p], Proc[p].From, Proc[p].To);
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

using SVI_NFT_R;

namespace Mcc
{
    public sealed class InspectMccLogItems
    {
        public IMccLogItem[] INIT_UNIT { get; private set; } = new IMccLogItem[1];
        public IMccLogItem[] JOB_CHANGE { get; private set; } = new IMccLogItem[1];
        public IMccLogItem[] INSP_PROCESS_1 { get; private set; } = new IMccLogItem[1];
        public IMccLogItem[] VISION_SCAN_BEFORE_WAIT_TIME { get; private set; } = new IMccLogItem[1];
        public IMccLogItem[] VISION_SCAN { get; private set; } = new IMccLogItem[1];
        public IMccLogItem[] VISION_SCAN_AFTER_WAIT_TIME { get; private set; } = new IMccLogItem[1];
        public IMccLogItem[] INSP_PROCESS_2 { get; private set; } = new IMccLogItem[1];
        public IMccLogItem[] INSP_WAIT_TIME { get; private set; } = new IMccLogItem[1];
        //public IMccLogItem[][] INSP_ALGORITHM { get; private set; } = new IMccLogItem[][] { new IMccLogItem[2] };
        //public IMccLogItem[][] INSP_CELL_SIGNALING_POINT { get; private set; } = new IMccLogItem[][] { new IMccLogItem[2] };
        public bool IsInitialized { get; private set; } = false;

        public bool Initialize(CDocument document)
        {
            var Module = new[]
            {
                UnitName.Inspect01,
            };
            var Proc = new[]
            {
                new { From = Module[0], To = Module[0] },
            };
            string[] systemNoPrefixs = new string[]
            {
                "1M",
            };

            for (int p = 0; p < 1; ++p)
            {
                int systemNo = 1;
                INIT_UNIT[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"INIT", Module[p], Proc[p].From, Proc[p].To);
                JOB_CHANGE[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"JOB_CHANGE", Module[p], Proc[p].From, Proc[p].To);

                systemNo = 10;
                INSP_PROCESS_1[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"INSP_PROCESS", Module[p], Proc[p].From, Proc[p].To);
                VISION_SCAN_BEFORE_WAIT_TIME[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", "VISION_SCAN_BEFORE_WAIT_TIME", Module[p], Proc[p].From, Proc[p].To);
                VISION_SCAN[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", "VISION_SCAN", Module[p], Proc[p].From, Proc[p].To);
                VISION_SCAN_AFTER_WAIT_TIME[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", "VISION_SCAN_AFTER_WAIT_TIME", Module[p], Proc[p].From, Proc[p].To);
                INSP_PROCESS_2[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"INSP_PROCESS", Module[p], Proc[p].From, Proc[p].To);
                INSP_WAIT_TIME[p] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", $"INSP_WAIT_TIME", Module[p], Proc[p].From, Proc[p].To);
                //INSP_ALGORITHM[p][0] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", "INSP_ALGORITHM_1", Module[p], Proc[p].From, Proc[p].To);
                //INSP_ALGORITHM[p][1] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", "INSP_ALGORITHM_2", Module[p], Proc[p].From, Proc[p].To);
                //INSP_CELL_SIGNALING_POINT[p][0] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", "INSP_CH1_CELL_SIGNALING_POINT", Module[p], Proc[p].From, Proc[p].To);
                //INSP_CELL_SIGNALING_POINT[p][1] = new MccActionLogItem(document, $"{MccLogManager.GetSystemNo(systemNoPrefixs[p], systemNo++)}_", "INSP_CH2_CELL_SIGNALING_POINT", Module[p], Proc[p].From, Proc[p].To);
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

using System;

namespace SVI_NFT_R.CellData
{
    [Serializable]
    public partial class OneCell
    {
        public bool IsUse { get; set; } = false;
        public MainData Cell { get; private set; } = new MainData();
        public CimData Cim { get; private set; } = new CimData();
        public WorkOrderData WorkOrder { get; private set; } = new WorkOrderData();
        public ReaderData Reader { get; private set; } = new ReaderData();
        public JudgementData Judgement { get; private set; } = new JudgementData();
        public AlignResultData PreAlignResult { get; private set; } = new AlignResultData();
        public EqInterfaceData EqInterface { get; private set; } = new EqInterfaceData();

        public void Reset()
        {
            IsUse = false;
            Cell = new MainData();
            Cim = new CimData();
            WorkOrder = new WorkOrderData();
            Reader = new ReaderData();
            Judgement = new JudgementData();
            PreAlignResult = new AlignResultData();
            EqInterface = new EqInterfaceData();
        }
    }
}

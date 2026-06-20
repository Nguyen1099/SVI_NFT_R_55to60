namespace SVI_NFT_R.DEVICE.Nachi
{
    public class ProcessPreOrder
    {
        public static ProcessPreOrder Empty => new ProcessPreOrder(EMethod.None, ETool.None, EStage.None);
        public EMethod Method { get; set; }
        public ETool ToolIndex { get; set; }
        public EStage StageIndex { get; set; }

        public ProcessPreOrder(EMethod method, ETool toolIndex, EStage stageIndex)
        {
            Method = method;
            ToolIndex = toolIndex;
            StageIndex = stageIndex;
        }

        public bool IsEmpty()
        {
            if (Method != EMethod.None)
            {
                return false;
            }
            if (ToolIndex != ETool.None)
            {
                return false;
            }
            if (StageIndex != EStage.None)
            {
                return false;
            }
            return true;
        }
    }
}

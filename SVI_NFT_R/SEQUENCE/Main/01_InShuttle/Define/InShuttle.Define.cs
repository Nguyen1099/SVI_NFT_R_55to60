using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InShuttle
    {
        private sealed class Define
        {
            public EProcess ProcessIndex { get; private set; }
            public int[] PickerPositions { get; private set; }
            public int StageMotorPosition { get; private set; }
            public int AlignPosition { get; private set; }
            public int[] InspectPositions { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        ProcessIndex = EProcess.InShuttle;
                        PickerPositions = new int[] { 0, 1 };
                        StageMotorPosition = 0;
                        AlignPosition = 0;
                        InspectPositions = new int[] { 0 };
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        }

        private Define mDefine;
    }
}
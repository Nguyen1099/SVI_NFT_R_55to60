using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutShuttle
    {
        private sealed class Define
        {
            public EProcess ProcessIndex { get; private set; }
            public int[] PickerPositions { get; private set; }
            public int StageMotorPosition { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        ProcessIndex = EProcess.OutShuttle;
                        PickerPositions = new int[] { 0, 1 };
                        StageMotorPosition = 0;
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
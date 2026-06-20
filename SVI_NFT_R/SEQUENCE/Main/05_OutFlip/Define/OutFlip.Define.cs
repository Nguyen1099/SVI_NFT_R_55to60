using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutFlip
    {
        private sealed class Define
        {
            public EProcess ProcessIndex { get; private set; }
            public int[] PickerPositions { get; private set; }
            public int SensorPositon { get; private set; }
            public int MotorPosition { get; private set; }
            public int[] TurnMotorPositions { get; private set; }
            public int DownMotorPosition { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        ProcessIndex = EProcess.OutFlip;
                        PickerPositions = new int[] { 0, 1, 2, 3 };
                        SensorPositon = 0;
                        MotorPosition = 0;
                        TurnMotorPositions = new int[] { 0, 1 };
                        DownMotorPosition = 0;
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

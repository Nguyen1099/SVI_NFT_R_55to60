using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutRobot
    {
        private sealed class Define
        {
            public EProcess ProcessIndex { get; private set; }
            public int[] TurnCylinderPositions { get; private set; }
            public int[] PickerPositions { get; private set; }
            public int NachiPosition { get; private set; }
            public OutRobotTurnCylinder.ECommand LoadPositionCylinder { get; private set; } = OutRobotTurnCylinder.ECommand.Turn;
            public OutRobotTurnCylinder.ECommand UnloadPositionCylinder { get; private set; } = OutRobotTurnCylinder.ECommand.Turn;

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        ProcessIndex = EProcess.OutRobot;
                        PickerPositions = new int[] { 0, 1 };
                        TurnCylinderPositions = new int[] { 0, 1 };
                        NachiPosition = 0;
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
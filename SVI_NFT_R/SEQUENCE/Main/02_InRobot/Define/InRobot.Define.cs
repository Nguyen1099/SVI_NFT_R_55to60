using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InRobot
    {
        private sealed class Define
        {
            public EProcess ProcessIndex { get; private set; }
            public int[] PickerPositions { get; private set; }
            public int[] TurnCylinderPositions { get; private set; }
            public int AlignSendPosition { get; private set; }
            public int[] McrPositions { get; private set; }
            public int NachiPosition { get; private set; }
            public InRobotTurnCylinder.ECommand LoadPositionCylinder { get; private set; } = InRobotTurnCylinder.ECommand.Return;
            public InRobotTurnCylinder.ECommand McrPositionCylinder { get; private set; } = InRobotTurnCylinder.ECommand.Return;
            public InRobotTurnCylinder.ECommand UnloadPositionCylinder { get; private set; } = InRobotTurnCylinder.ECommand.Turn;

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        ProcessIndex = EProcess.InRobot;
                        PickerPositions = new int[] { 0, 1 };
                        TurnCylinderPositions = new int[] { 0, 1 };
                        AlignSendPosition = 0;
                        McrPositions = new int[] { 0, 1 };
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
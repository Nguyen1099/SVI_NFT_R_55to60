using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Nachi;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InRobot
    {
        internal sealed class RobotSequenceItemSet
        {
            public InRobotPicker[] RobotPickers { get; private set; }
            public CellDataHandler[] TargetCells { get; private set; }
            public CVacuum[] TargetVacuums { get; private set; }
            public EMethod MethodIndex { get; private set; }
            public ETool ToolIndex { get; private set; }
            public EStage StageIndex { get; set; }
            private readonly InRobot mManager;
            private readonly EProcess mTargetProcessIndex;

            public RobotSequenceItemSet(InRobot manager, EMethod methodIndex, EProcess targetProcessIndex)
            {
                mManager = manager;
                MethodIndex = methodIndex;
                mTargetProcessIndex = targetProcessIndex;
            }

            public void SetRobotToolIndex(ETool toolIndex)
            {
                switch (mTargetProcessIndex)
                {
                    case EProcess.InShuttle:
                        SetInShuttleRobotToolIndex(toolIndex);
                        break;

                    case EProcess.InRobot:
                        SetInRobotRobotToolIndex(toolIndex);
                        break;

                    case EProcess.InspStage:
                        SetInspStageRobotToolIndex(toolIndex);
                        break;
                }
            }

            public void CellDataMove()
            {
                Debug.Assert(MethodIndex != EMethod.None);
                // Cell to Cell 이동
                switch (MethodIndex)
                {
                    case EMethod.Get:
                        for (int i = 0; i < TargetCells.Length; i++)
                        {
                            CellDataManager.DataMove(TargetCells[i], RobotPickers[i].CellPort);
                        }
                        break;

                    case EMethod.Put:
                        for (int i = 0; i < RobotPickers.Length; i++)
                        {
                            CellDataManager.DataMove(RobotPickers[i].CellPort, TargetCells[i]);
                        }
                        break;
                }
            }

            private void SetInShuttleRobotToolIndex(ETool toolIndex)
            {
                ToolIndex = toolIndex;
                switch (ToolIndex)
                {
                    case ETool.Tool1:
                        RobotPickers = new InRobotPicker[] { mManager.Pickers[0] };
                        TargetCells = new CellDataHandler[] { CellDataManager.ProcessCells[mTargetProcessIndex][0] };
                        StageIndex = EStage.Stage1;
                        break;

                    case ETool.Tool2:
                        RobotPickers = new InRobotPicker[] { mManager.Pickers[1] };
                        TargetCells = new CellDataHandler[] { CellDataManager.ProcessCells[mTargetProcessIndex][1] };
                        StageIndex = EStage.Stage2;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            private void SetInRobotRobotToolIndex(ETool toolIndex)
            {
                ToolIndex = toolIndex;
                switch (ToolIndex)
                {
                    case ETool.Tool1:
                        RobotPickers = new InRobotPicker[] { mManager.Pickers[0] };
                        TargetCells = new CellDataHandler[] { CellDataManager.ProcessCells[mTargetProcessIndex][0] };
                        StageIndex = EStage.Stage1;
                        break;

                    case ETool.Tool2:
                        RobotPickers = new InRobotPicker[] { mManager.Pickers[1] };
                        TargetCells = new CellDataHandler[] { CellDataManager.ProcessCells[mTargetProcessIndex][1] };
                        StageIndex = EStage.Stage2;
                        break;

                    case ETool.Tool12:
                        RobotPickers = new InRobotPicker[] { mManager.Pickers[0], mManager.Pickers[1] };
                        TargetCells = new CellDataHandler[] { CellDataManager.ProcessCells[mTargetProcessIndex][0], CellDataManager.ProcessCells[mTargetProcessIndex][1] };
                        StageIndex = EStage.Stage1;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            private void SetInspStageRobotToolIndex(ETool toolIndex)
            {
                ToolIndex = toolIndex;
                switch (ToolIndex)
                {
                    case ETool.Tool1:
                        RobotPickers = new InRobotPicker[] { mManager.Pickers[0] };
                        TargetCells = new CellDataHandler[] { CellDataManager.ProcessCells[mTargetProcessIndex][0] };
                        StageIndex = EStage.Stage1;
                        break;

                    case ETool.Tool2:
                        RobotPickers = new InRobotPicker[] { mManager.Pickers[1] };
                        TargetCells = new CellDataHandler[] { CellDataManager.ProcessCells[mTargetProcessIndex][1] };
                        StageIndex = EStage.Stage2;
                        break;

                    case ETool.Tool12:
                        RobotPickers = new InRobotPicker[] { mManager.Pickers[0], mManager.Pickers[1] };
                        TargetCells = new CellDataHandler[] { CellDataManager.ProcessCells[mTargetProcessIndex][0], CellDataManager.ProcessCells[mTargetProcessIndex][1] };
                        StageIndex = EStage.Stage1;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        }
    }
}
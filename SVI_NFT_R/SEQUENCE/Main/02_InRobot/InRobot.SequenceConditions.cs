using SVI_NFT_R.CellData;
using SVI_NFT_R.EHS;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class InRobot : CProcessAbstract
    {
        private enum EAutoSequence
        {
            LoadGetWait = 0,
            LoadGet,
            Mcr,
            UnloadPutWait,
            UnloadPut
        }

        private bool IsRobot(EAutoSequence sequenceIndex)
        {
            switch (sequenceIndex)
            {
                case EAutoSequence.LoadGetWait:
                    return CheckIsLoadGetWaitCondition();

                case EAutoSequence.LoadGet:
                    return CheckIsLoadGetCondition();

                case EAutoSequence.Mcr:
                    return CheckIsMcrCondition();

                case EAutoSequence.UnloadPutWait:
                    return CheckIsUnloadPutWaitCondition();

                case EAutoSequence.UnloadPut:
                    return CheckIsUnloadPutCondition();

                default:
                    Debug.Assert(false);
                    break;
            }
            return true;
        }

        private bool CheckIsLoadGetWaitCondition()
        {
            if (Nachi.GetStatus() == InRobotNachi.EStatus.LoadProcessBegin
                || Nachi.GetStatus() == InRobotNachi.EStatus.McrProcessBegin
                || Nachi.GetStatus() == InRobotNachi.EStatus.UnloadProcessBegin
                )
            {
                return false;
            }
            if (CellContainer.IsCellExistFromList() == true)
            {
                return false;
            }
            if (CanMoving() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsLoadGetCondition()
        {
            if (Nachi.GetStatus() != InRobotNachi.EStatus.LoadProcessBegin)
            {
                return false;
            }
            var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
            if (inShuttle.IsStageMotorXStatusAll(InShuttleMotorX.EStatus.UnloadPosition) == false)
            {
                return false;
            }
            int inShuttleExistCellCount = inShuttle.CellContainer.GetExistCellCount();
            if (inShuttleExistCellCount == 0)
            {
                return false;
            }
            if (inShuttleExistCellCount != GetInShuttleAlignDoneCount())
            {
                return false;
            }
            if (CanMoving() == false)
            {
                return false;
            }
            if (InterferenceRegion.InShuttleAlignArea.TryEnter() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsMcrCondition()
        {
            if (Nachi.GetStatus() == InRobotNachi.EStatus.LoadProcessBegin
                || Nachi.GetStatus() == InRobotNachi.EStatus.McrProcessBegin
                || Nachi.GetStatus() == InRobotNachi.EStatus.UnloadProcessBegin
                )
            {
                return false;
            }
            if (CellContainer.IsCellExistFromList() == false)
            {
                return false;
            }
            if (CellContainer.GetExistCellCount() == GetInRobotMcrDoneCount())
            {
                return false;
            }
            if (CanMoving() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadPutWaitCondition()
        {
            if (Nachi.GetStatus() == InRobotNachi.EStatus.LoadProcessBegin
                || Nachi.GetStatus() == InRobotNachi.EStatus.McrProcessBegin
                || Nachi.GetStatus() == InRobotNachi.EStatus.UnloadProcessBegin
                )
            {
                return false;
            }
            if (CellContainer.IsCellExistFromList() == false)
            {
                return false;
            }
            if (CellContainer.GetExistCellCount() != GetInRobotMcrDoneCount())
            {
                return false;
            }
            if (CanMoving() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadPutCondition()
        {
            if (Nachi.GetStatus() != InRobotNachi.EStatus.UnloadProcessBegin)
            {
                return false;
            }
            if (CellContainer.IsCellExistFromList() == false)
            {
                return false;
            }
            if (CellContainer.GetExistCellCount() != GetInRobotMcrDoneCount())
            {
                return false;
            }
            var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
            if (inspStage.CellContainer.IsCellExistFromList() == true)
            {
                return false;
            }
            if (inspStage.IsStageMotorYStatusAll(InspStageMotorY.EStatus.LoadPosition) == false)
            {
                return false;
            }
            if (CanMoving() == false)
            {
                return false;
            }
            if (InterferenceRegion.InspectionStageArea.TryEnter() == false)
            {
                return false;
            }
            var outRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot;
            if (outRobot.Nachi.IsLoadInterlock() == true)
            {
                return false;
            }
            return true;
        }

        private bool CanMoving()
        {
            if (Nachi.CanMovingGroupByEhsPolicy() == false)
            {
                return false;
            }
            return true;
        }

        private int GetInShuttleAlignDoneCount()
        {
            return CellDataManager.ProcessCells[EProcess.InShuttle].GetExistCellList().Where(item => item.Data.Cell.PreAlignStatus == EStatus.Done).Count();
        }

        private int GetInRobotMcrDoneCount()
        {
            return CellContainer.GetExistCellList().Where(item => item.Data.Cell.McrStatus == EStatus.Done).Count();
        }
    }
}
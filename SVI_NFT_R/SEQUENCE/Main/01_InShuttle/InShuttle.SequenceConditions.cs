using SVI_NFT_R.CellData;
using SVI_NFT_R.EHS;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class InShuttle
    {
        private enum EAutoSequence
        {
            Load = 0,
            LoadInterface,
            LoadInterfaceFinish,
            Scan,
            AlignWait,
            Align,
            Unload
        }

        public bool IsStageMotorXStatusAll(InShuttleMotorX.EStatus status)
        {
            return MotorStageX.GetStatus() == status;
        }

        private bool IsInShuttle(EAutoSequence sequenceIndex)
        {
            switch (sequenceIndex)
            {
                case EAutoSequence.Load:
                    return CheckIsLoadCondition();

                case EAutoSequence.LoadInterface:
                    return CheckIsLoadInterfaceCondition();

                case EAutoSequence.LoadInterfaceFinish:
                    return CheckIsLoadInterfaceFinishCondition();

                case EAutoSequence.Scan:
                    return CheckIsScanCondition();

                case EAutoSequence.AlignWait:
                    return CheckIsAlignWaitCondition();

                case EAutoSequence.Align:
                    return CheckIsAlignCondition();

                case EAutoSequence.Unload:
                    return CheckIsUnloadCondition();

                default:
                    Debug.Assert(false);
                    break;
            }
            return true;
        }

        private bool CheckIsLoadCondition()
        {
            if (IsStageMotorXStatusAll(InShuttleMotorX.EStatus.LoadPosition) == true)
            {
                return false;
            }
            if (CellContainer.GetExistCellCount() != 0)
            {
                return false;
            }
            if (CanMovingStage() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsLoadInterfaceCondition()
        {
            if (IsStageMotorXStatusAll(InShuttleMotorX.EStatus.LoadPosition) == false)
            {
                return false;
            }
            if (CellContainer.IsCellExistFromList() == true)
            {
                return false;
            }
            var upper = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface;
            if (upper.IsHandShaking == false)
            {
                return false;
            }
            if (mbStartLoadInterface == true)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsLoadInterfaceFinishCondition()
        {
            if (CellContainer.IsCellExistFromList() == false)
            {
                return false;
            }
            var upper = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface;
            if (upper.IsHandShaking == true)
            {
                return false;
            }
            if (mbStartLoadInterface == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsScanCondition()
        {
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
            {
                return false;
            }
            if (existCellCount == GetScanDoneCellCount())
            {
                return false;
            }
            if (CanMovingStage() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsAlignWaitCondition()
        {
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
            {
                return false;
            }
            if (IsStageMotorXStatusAll(InShuttleMotorX.EStatus.AlignPosition) == true)
            {
                return false;
            }
            if (existCellCount != GetScanDoneCellCount())
            {
                return false;
            }
            if (existCellCount == GetAlignDoneCellCount())
            {
                return false;
            }
            if (CanMovingStage() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsAlignCondition()
        {
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
            {
                return false;
            }
            if (IsStageMotorXStatusAll(InShuttleMotorX.EStatus.AlignPosition) == false)
            {
                return false;
            }
            if (existCellCount != GetScanDoneCellCount())
            {
                return false;
            }
            if (existCellCount == GetAlignDoneCellCount())
            {
                return false;
            }
            var inRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.InRobot;
            int inRobotExistCellCount = inRobot.CellContainer.GetExistCellCount();
            if (inRobotExistCellCount != GetInRobotMcrDoneCellCount())
            {
                return false;
            }
            if (InterferenceRegion.InShuttleAlignArea.IsEnter == true)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadCondition()
        {
            if (IsStageMotorXStatusAll(InShuttleMotorX.EStatus.UnloadPosition) == true)
            {
                return false;
            }
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
            {
                return false;
            }
            if (existCellCount != GetScanDoneCellCount())
            {
                return false;
            }
            if (existCellCount != GetAlignDoneCellCount())
            {
                return false;
            }
            if (CanMovingStage() == false)
            {
                return false;
            }
            return true;
        }

        private bool CanMovingStage()
        {
            if (MotorStageX.CanMovingGroupByEhsPolicy() == false)
            {
                return false;
            }
            var upper = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface;
            if (upper.CheckUpperEqInterfaceInterlocked() == true)
            {
                return false;
            }
            if (upper.IsHandShaking == true
                || mbStartLoadInterface == true
                )
            {
                return false;
            }
            return true;
        }

        private int GetScanDoneCellCount()
        {
            return CellContainer.GetExistCellList().Where(i => i.Data.Cell.SubOpticsInspStatus == EStatus.Done).Count();
        }
        private int GetAlignDoneCellCount()
        {
            return CellContainer.GetExistCellList().Where(i => i.Data.Cell.PreAlignStatus == EStatus.Done).Count();
        }
        private int GetInRobotMcrDoneCellCount()
        {
            return CellDataManager.ProcessCells[EProcess.InRobot].GetExistCellList().Where(item => item.Data.Cell.McrStatus == EStatus.Done).Count();
        }
    }
}
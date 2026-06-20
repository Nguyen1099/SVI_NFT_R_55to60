using SVI_NFT_R.CellData;
using SVI_NFT_R.EHS;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class InspStage
    {
        private enum EAutoSequence
        {
            Load = 0,
            Scan,
            Unload
        }

        public bool IsStageMotorYStatusAll(InspStageMotorY.EStatus status)
        {
            return MotorStageY.GetStatus() == status;
        }

        private bool IsInspStage(EAutoSequence sequenceIndex)
        {
            switch (sequenceIndex)
            {
                case EAutoSequence.Load:
                    return CheckIsLoadCondition();

                case EAutoSequence.Scan:
                    return CheckIsScanCondition();

                case EAutoSequence.Unload:
                    return CheckIsUnloadCondition();

                default:
                    Debug.Assert(false);
                    break;
            }
            return false;
        }

        private bool CheckIsLoadCondition()
        {
            if (IsStageMotorYStatusAll(InspStageMotorY.EStatus.LoadPosition) == true)
            {
                return false;
            }
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount != 0)
            {
                return false;
            }
            if (CanMovingStage() == false)
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
            if (existCellCount == GetGrabDoneCellCount(CellContainer))
            {
                return false;
            }
            if (CanMovingStage() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadCondition()
        {
            if (IsStageMotorYStatusAll(InspStageMotorY.EStatus.UnloadPosition) == true)
            {
                return false;
            }
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
            {
                return false;
            }
            if (existCellCount != GetGrabDoneCellCount(CellContainer))
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
            if (MotorStageY.CanMovingGroupByEhsPolicy() == false)
            {
                return false;
            }
            return true;
        }

        private int GetGrabDoneCellCount(IReadOnlyList<CellDataHandler> cellContainer)
        {
            return cellContainer.GetExistCellList().Where(i => i.Data.Cell.InspStatus == EStatus.Done).Count();
        }
    }
}
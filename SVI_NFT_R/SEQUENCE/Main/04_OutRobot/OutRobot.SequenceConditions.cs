using SVI_NFT_R.CellData;
using SVI_NFT_R.EHS;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class OutRobot : CProcessAbstract
    {
        private enum EAutoSequence
        {
            LoadGetWait = 0,
            LoadGet,
            UnloadPutWait,
            WaitInspResult,
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

                case EAutoSequence.UnloadPutWait:
                    return CheckIsUnloadPutWaitCondition();

                case EAutoSequence.WaitInspResult:
                    return CheckIsWaitInspResultCondition();

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
            if (Nachi.GetStatus() == OutRobotNachi.EStatus.LoadProcessBegin
                || Nachi.GetStatus() == OutRobotNachi.EStatus.UnloadProcessBegin
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
            if (Nachi.GetStatus() != OutRobotNachi.EStatus.LoadProcessBegin)
            {
                return false;
            }
            if (CellContainer.IsCellExistFromList() == true)
            {
                return false;
            }
            var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
            if (inspStage.IsStageMotorYStatusAll(InspStageMotorY.EStatus.UnloadPosition) == false)
            {
                return false;
            }
            int inspStageExistCellCount = inspStage.CellContainer.GetExistCellCount();
            if (inspStageExistCellCount == 0)
            {
                return false;
            }
            if (inspStageExistCellCount != GetGrabDoneCellCount(inspStage.CellContainer))
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
            var inRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.InRobot;
            if (inRobot.Nachi.IsUnloadInterlock() == true)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadPutWaitCondition()
        {
            if (Nachi.GetStatus() == OutRobotNachi.EStatus.LoadProcessBegin
                || Nachi.GetStatus() == OutRobotNachi.EStatus.UnloadProcessBegin
                )
            {
                return false;
            }
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
            {
                return false;
            }
            if (CanMoving() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsWaitInspResultCondition()
        {
            if (Nachi.GetStatus() != OutRobotNachi.EStatus.UnloadProcessBegin)
            {
                return false;
            }
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
            {
                return false;
            }
            if (existCellCount == GetInspResultReceiveCount())
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadPutCondition()
        {
            if (Nachi.GetStatus() != OutRobotNachi.EStatus.UnloadProcessBegin)
            {
                return false;
            }
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
            {
                return false;
            }
            if (existCellCount != GetInspResultReceiveCount())
            {
                return false;
            }
            var outFlip = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip;
            if (outFlip.CellContainer.IsCellExistFromList() == true)
            {
                return false;
            }
            bool bIsWaitUnloadPosition = outFlip.MotorZ.GetStatus() == OutFlipMotorZ.EStatus.UpPosition
                && outFlip.MotorRs.All(motor => motor.GetStatus() == OutFlipMotorR.EStatus.LoadPosition);
            if (bIsWaitUnloadPosition == false)
            {
                return false;
            }
            if (CanMoving() == false)
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

        private int GetGrabDoneCellCount(IReadOnlyList<CellDataHandler> cellContainer)
        {
            return cellContainer.GetExistCellList().Where(i => i.Data.Cell.InspStatus == EStatus.Done).Count();
        }

        private int GetInspResultReceiveCount()
        {
            return CellContainer.GetExistCellList().Where(i => i.Data.Cell.FrontInspResult != EInspectionResult.None && i.Data.Cell.RearInspResult != EInspectionResult.None).Count();
        }
    }
}
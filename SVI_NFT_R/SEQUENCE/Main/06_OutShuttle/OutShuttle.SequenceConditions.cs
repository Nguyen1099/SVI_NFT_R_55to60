using SVI_NFT_R.CellData;
using SVI_NFT_R.EHS;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class OutShuttle
    {
        private enum EAutoSequence
        {
            Load = 0,
            UnloadWait,
            UnloadInterface,
            UnloadInterfaceFinish,
        }

        public bool IsStageMotorXStatusAll(OutShuttleMotorX.EStatus status)
        {
            return MotorStageX.GetStatus() == status;
        }

        private bool IsOutShuttle(EAutoSequence sequenceIndex)
        {
            switch (sequenceIndex)
            {
                case EAutoSequence.Load:
                    return CheckIsLoadCondition();

                case EAutoSequence.UnloadWait:
                    return CheckIsUnloadWaitCondition();

                case EAutoSequence.UnloadInterface:
                    return CheckIsUnloadInterfaceCondition();

                case EAutoSequence.UnloadInterfaceFinish:
                    return CheckIsUnloadInterfaceFinishCondition();

                default:
                    Debug.Assert(false);
                    break;
            }
            return true;
        }                                                               

        private bool CheckIsLoadCondition()
        {
            if (IsStageMotorXStatusAll(OutShuttleMotorX.EStatus.LoadPosition) == true)
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

        private bool CheckIsUnloadWaitCondition()
        {
            if (IsStageMotorXStatusAll(OutShuttleMotorX.EStatus.UnloadPosition) == true)
            {
                return false;
            }
            if (CellContainer.IsCellExistFromList() == true)
            {
                return false;
            }
            var lower = m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface;
            if (lower.IsHandShaking == false)
            {
                return false;
            }
            if (mbStartUnloadInterface == true)
            {
                return false;
            }
            if (CanMovingStage() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadInterfaceCondition()
        {
            if (IsStageMotorXStatusAll(OutShuttleMotorX.EStatus.UnloadPosition) == false)
            {
                return false;
            }
            if (CellContainer.IsCellExistFromList() == false)
            {
                return false;
            }
            var lower = m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface;
            if (lower.IsHandShaking == true)
            {
                return false;
            }
            if (mbStartUnloadInterface == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadInterfaceFinishCondition()
        {
            if (IsStageMotorXStatusAll(OutShuttleMotorX.EStatus.UnloadPosition) == true)
            {
                return false;
            }
            int existCellCount = CellContainer.GetExistCellCount();
            if (existCellCount == 0)
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
            var lower = m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface;
            if (lower.CheckLowerEqInterfaceInterlocked() == true)
            {
                return false;
            }
            if (lower.IsHandShaking == true
                || mbStartUnloadInterface == true
                )
            {
                return false;
            }
            return true;
        }

    }
}
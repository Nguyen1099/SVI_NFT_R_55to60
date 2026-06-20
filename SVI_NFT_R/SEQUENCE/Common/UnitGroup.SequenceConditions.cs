using System.Diagnostics;
using System.Linq;
using System.Threading;
using static SVI_NFT_R.CCIMDefine;

namespace SVI_NFT_R
{
    public partial class UnitGroup
    {
        private enum EAutoSequence
        {
            StopInput = 0,
            ResumeInput,
            CimReportUseStatus,
            CimReportPauseStatus,
            CimReportInterlockStatus
        }

        private bool IsUnitGroup(EAutoSequence sequence)
        {
            Thread.Sleep(0);
            switch (sequence)
            {
                case EAutoSequence.StopInput:
                    return CheckIsStopInputCondition();

                case EAutoSequence.ResumeInput:
                    return CheckIsResumeInputCondition();

                case EAutoSequence.CimReportUseStatus:
                    return CheckIsCimReportUseStatusCondition();

                case EAutoSequence.CimReportPauseStatus:
                    return CheckIsCimReportPauseStatusCondition();

                case EAutoSequence.CimReportInterlockStatus:
                    return CheckIsCimReportInterlockStatusCondition();

                default:
                    Debug.Assert(false);
                    break;
            }
            return false;
        }

        private bool CheckIsStopInputCondition()
        {
            if (InputUnitNode.CanInput == false)
            {
                return false;
            }
            if (mbShouldCycleStop == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsResumeInputCondition()
        {
            if (InputUnitNode.CanInput == true)
            {
                return false;
            }
            if (mbShouldCycleStop == true)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsCimReportUseStatusCondition()
        {
            if (mbShouldCycleStop == true
                || mbShouldInterlockReport == true
                )
            {
                return false;
            }
            if (InputUnitNode.CanInput == false)
            {
                return false;
            }
            if (GetCimReportedUseStatus() == true)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsCimReportPauseStatusCondition()
        {
            if (mbShouldCycleStop == false
                || mbShouldInterlockReport == true
                )
            {
                return false;
            }
            if (InputUnitNode.CanInput == true)
            {
                return false;
            }
            if (GetCycleStopFinished() == false)
            {
                return false;
            }
            if (GetCimReportedPauseStatus() == true)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsCimReportInterlockStatusCondition()
        {
            if (mbShouldCycleStop == false
                || mbShouldInterlockReport == false
                )
            {
                return false;
            }
            if (InputUnitNode.CanInput == true)
            {
                return false;
            }
            if (GetCycleStopFinished() == false)
            {
                return false;
            }
            if (GetCimReportedInterlockStatus() == true)
            {
                return false;
            }
            return true;
        }

        private bool GetCycleStopFinished()
        {
            return UnitNodes.All(i => i.IsIdle && i.IsEmpty);
        }

        private bool GetCimReportedUseStatus()
        {
            int unitIndex = (int)UnitIndex;
            int currentStateIndex = (int)EPresentState.CURRENT_STATE;
            if (mDocument.m_eUnitMoveState[unitIndex][currentStateIndex] != EMoveState.MOVE_STATE_RUNNING)
            {
                return false;
            }
            if (mDocument.m_eUnitInterlockState[unitIndex][currentStateIndex] != EInterlockState.INTERLOCK_STATE_OFF)
            {
                return false;
            }
            return true;
        }

        private bool GetCimReportedPauseStatus()
        {
            int unitIndex = (int)UnitIndex;
            int currentStateIndex = (int)EPresentState.CURRENT_STATE;
            if (mDocument.m_eUnitMoveState[unitIndex][currentStateIndex] != EMoveState.MOVE_STATE_PAUSE)
            {
                return false;
            }
            if (mDocument.m_eUnitInterlockState[unitIndex][currentStateIndex] != EInterlockState.INTERLOCK_STATE_OFF)
            {
                return false;
            }
            return true;
        }

        private bool GetCimReportedInterlockStatus()
        {
            int unitIndex = (int)UnitIndex;
            int currentStateIndex = (int)EPresentState.CURRENT_STATE;
            if (mDocument.m_eUnitMoveState[unitIndex][currentStateIndex] != EMoveState.MOVE_STATE_PAUSE)
            {
                return false;
            }
            if (mDocument.m_eUnitInterlockState[unitIndex][currentStateIndex] != EInterlockState.INTERLOCK_STATE_ON)
            {
                return false;
            }
            return true;
        }
    }
}
using SVI_NFT_R.CellData;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R
{
    public partial class InspStage
    {
        public enum EBatch
        {
            None = 0,

            MoveLoadPosition,
            MoveGrabP1Position,
            MoveGrabP2Position,
            MoveUnloadPosition,
            GrabProcess
        }
        public EBatch BatchCommand { get; set; }

        private bool DoManualStageMove(InspStageMotorY.ECommand command)
        {
            if (SetSubCommandStageMove(command) == false)
            {
                return false;
            }
            return true;
        }

        private bool DoManualGrabProcess()
        {
            //if (TryGetCellPlacementFromVacuum(out ECellPlacement cellPlacement) == false)
            //{
            //    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NO_CELL_IN_THE_ROBOT_TOOL_LOAD_THE_CELL_INTO_THE_ROBOT_TOOL_AND_TRY_AGAIN);
            //    return false;
            //}
            // 2026-03-11 SDV Ha 요청사항 : Manual Scan 동작 자재 유무 상관 없이 무조건 진행하도록 변경
            ECellPlacement cellPlacement = ECellPlacement.P1 | ECellPlacement.P2 | ECellPlacement.Full;
            Inspects.ForEach(x => x.SetGrabInformationTemporaryId(cellPlacement));
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    InspStageMotorY.ECommand grabPosition = 0;
                    switch (i)
                    {
                        case 0:
                            if (cellPlacement.HasFlag(ECellPlacement.P1) == false)
                            {
                                continue;
                            }
                            grabPosition = InspStageMotorY.ECommand.GrabP1Position;
                            break;
                        case 1:
                            if (cellPlacement.HasFlag(ECellPlacement.P2) == false)
                            {
                                continue;
                            }
                            grabPosition = InspStageMotorY.ECommand.GrabP2Position;
                            break;
                        default:
                            Debug.Assert(false);
                            return false;
                    }
                    if (SetSubCommandStageMove(grabPosition) == false)
                    {
                        return false;
                    }
                    var taskInspectScanBeforeWait = SetSubCommandInspectScanBeforeWait(i);
                    Task.WaitAll(taskInspectScanBeforeWait);
                    if (taskInspectScanBeforeWait.Result == false)
                    {
                        return false;
                    }
                }
            }
            finally
            {
                Inspects.ForEach(x => x.ResetGrabInformation());
            }

            return true;
        }

        private bool SetSubCommandStageMove(InspStageMotorY.ECommand command)
        {
            // 인터락 체크
            if (m_objDocument.GetRunStatus() == CDefine.ERunStatus.Stop)
            {
                if (MotorStageY.m_objInterlock.CheckMotionClassInterlock(MotorStageY.MotorIndex.ToString(), (int)command) == false)
                {
                    return false;
                }
            }
            // 위치 이동
            MotorStageY.SetCommand(command);
            if (MotorStageY.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private Task<bool> SetSubCommandInspectScanBeforeWait(int selectIndex)
        {
            return Task.Run(() =>
            {
                Inspects.ForEach(x => x.SetCommand(InspStageInspect.ECommand.Check));
                if (Inspects.WaitForEndProcess() == false)
                {
                    return false;
                }
                Inspects.ForEach(x => x.SetCommand(selectIndex == 0 ? InspStageInspect.ECommand.GrabP1 : InspStageInspect.ECommand.GrabP2));
                if (Inspects.WaitForEndProcess() == false)
                {
                    return false;
                }
                return true;
            });
        }

        private bool SetSubCommandMainFrameInvoke(Func<bool> action, bool bHandleWaitMessageUI = true)
        {
            if (bHandleWaitMessageUI == true)
            {
                m_objDocument.GetMainFrame().ShowWaitMessage(false);
            }
            try
            {
                bool bResult = false;
                m_objDocument.GetMainFrame().Invoke(new Action(() => bResult = action.Invoke()));
                return bResult;
            }
            finally
            {
                if (bHandleWaitMessageUI == true)
                {
                    m_objDocument.GetMainFrame().ShowWaitMessage(true);
                }
            }
        }

        private void ThreadManualProcess()
        {
            while (mbThreadExit == false)
            {
                Thread.Sleep(10);
                if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Stop)
                {
                    continue;
                }

                switch (BatchCommand)
                {
                    case EBatch.None:
                        continue;

                    case EBatch.MoveLoadPosition:
                        if (DoManualStageMove(InspStageMotorY.ECommand.LoadPosition) == false)
                        {
                            break;
                        }
                        break;

                    case EBatch.MoveGrabP1Position:
                        if (DoManualStageMove(InspStageMotorY.ECommand.GrabP1Position) == false)
                        {
                            break;
                        }
                        break;

                    case EBatch.MoveGrabP2Position:
                        if (DoManualStageMove(InspStageMotorY.ECommand.GrabP2Position) == false)
                        {
                            break;
                        }
                        break;

                    case EBatch.MoveUnloadPosition:
                        if (DoManualStageMove(InspStageMotorY.ECommand.UnloadPosition) == false)
                        {
                            break;
                        }
                        break;

                    case EBatch.GrabProcess:
                        if (DoManualGrabProcess() == false)
                        {
                            break;
                        }
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
                m_objDocument.SetRunStatus(CDefine.ERunStatus.Stop);
                BatchCommand = EBatch.None;
            }
        }
    }
}
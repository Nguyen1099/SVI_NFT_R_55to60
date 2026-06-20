using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class InShuttle
    {
        public enum EBatch
        {
            None = 0,

            AlignProcess,
            MoveLoadPosition,
            MoveScanStartWaitPosition,
            MoveScanTriggerStartPosition,
            MoveScanTriggerEndPosition,
            MoveAlignPosition,
            MoveUnloadPosition,
            ScanProcess,
            AlignGrabP1,
            AlignGrabP2
        }
        public EBatch BatchCommand { get; set; } = EBatch.None;
        private string mLastSelectPosition = string.Empty;

        public bool DoManualStageMove(InShuttleMotorX.ECommand command)
        {
            if (SetSubCommandStageMove(command) == false)
            {
                return false;
            }

            return true;
        }

        private bool DoManualScanProcess()
        {
            //if (TryGetCellPlacementFromVacuum(out ECellPlacement cellPlacement) == false)
            //{
            //    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NO_CELL_IN_THE_ROBOT_TOOL_LOAD_THE_CELL_INTO_THE_ROBOT_TOOL_AND_TRY_AGAIN);
            //    return false;
            //}

            if (MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), (int)InShuttleMotorX.ECommand.ScanStartWaitPosition) == false)
            {
                return false;
            }
            //m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_INSP_STAGE_HIGH_VAC_PRESSURE_MODE_SELECT.ToString(), false);
            try
            {
                // 2026-03-11 SDV Ha 요청사항 : Manual Scan 동작 자재 유무 상관 없이 무조건 진행하도록 변경
                ECellPlacement cellPlacement = ECellPlacement.P1 | ECellPlacement.P2 | ECellPlacement.Full;
                Inspects.ForEach(x => x.SetGrabInformationTemporaryId(cellPlacement));
                var taskInspectScanBeforeWait = SetSubCommandInspectScanBeforeWait();
                MotorStageX.SetCommand(InShuttleMotorX.ECommand.ScanStartWaitPosition);
                Task.WaitAll(taskInspectScanBeforeWait);
                if (MotorStageX.WaitForEndProcess() == false)
                {
                    return false;
                }
                if (taskInspectScanBeforeWait.Result == false)
                {
                    return false;
                }

                if (MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), (int)InShuttleMotorX.ECommand.UnloadPosition) == false)
                {
                    return false;
                }
                MotorStageX.SetCommand(InShuttleMotorX.ECommand.Scan);
                if (MotorStageX.WaitForCommandSync(Constants.PASS_BY_SCAN_TRIGGER_END_POSITION) == false)
                {
                    return false;
                }

                Inspects.ForEach(x => x.SetCommand(InShuttleInspect.ECommand.GrabEnd));
                if (Inspects.WaitForEndProcess() == false
                    || MotorStageX.WaitForEndProcess() == false
                    )
                {
                    return false;
                }
            }
            finally
            {
                //m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_INSP_STAGE_HIGH_VAC_PRESSURE_MODE_SELECT.ToString(), true);
            }

            if (MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), (int)InShuttleMotorX.ECommand.LoadPosition) == false)
            {
                return false;
            }
            MotorStageX.SetCommand(InShuttleMotorX.ECommand.LoadPosition);
            if (MotorStageX.WaitForEndProcess() == false)
            {
                return false;
            }

            return true;
        }

        public bool DoManualAlign(ECellPlacement cellPlacement)
        {
            if (cellPlacement.HasFlag(ECellPlacement.Empty) == true)
            {
                return false;
            }
            if (cellPlacement.HasFlag(ECellPlacement.P1) == true)
            {
                AlignVision.SetAlignInformationTemporaryId(ECellPlacement.P1);
            }
            if (cellPlacement.HasFlag(ECellPlacement.P2) == true)
            {
                AlignVision.SetAlignInformationTemporaryId(ECellPlacement.P2);
            }
            // 얼라인
            AlignVision.SetCommand(InShuttleAlignVision.ECommand.Align);
            if (AlignVision.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private bool SetSubCommandStageMove(InShuttleMotorX.ECommand command)
        {
            // 인터락 체크
            if (m_objDocument.GetRunStatus() == CDefine.ERunStatus.Stop)
            {
                if (MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), (int)command) == false)
                {
                    return false;
                }
            }
            // 위치 이동
            MotorStageX.SetCommand(command);
            if (MotorStageX.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private Task<bool> SetSubCommandInspectScanBeforeWait()
        {
            return Task.Run(() =>
            {
                Inspects.ForEach(x => x.SetCommand(InShuttleInspect.ECommand.GrabStart));
                if (Inspects.WaitForEndProcess() == false)
                {
                    return false;
                }
                return true;
            });
        }

        private bool SetSubCommandSelectPosition(out ECellPlacement cellPlacement)
        {
            m_objDocument.GetMainFrame().ShowWaitMessage(false);
            string selectName = string.Empty;
            cellPlacement = 0;
            try
            {
                m_objDocument.GetMainFrame().Invoke(new Action(() =>
                {
                    using (var dialog = new FormEnumSelect(new string[] { "P1", "P2", "FULL" }, mLastSelectPosition))
                    {
                        dialog.TitleText = $"SELECT POSITION ({Resource.Get(nameof(EProcess.InShuttle))})";
                        if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            return;
                        }

                        selectName = dialog.ResultName;
                    }
                }));
            }
            finally
            {
                m_objDocument.GetMainFrame().ShowWaitMessage(true);
            }
            switch (selectName)
            {
                case "P1":
                    cellPlacement |= ECellPlacement.P1;
                    break;

                case "P2":
                    cellPlacement |= ECellPlacement.P2;
                    break;

                case "FULL":
                    cellPlacement |= ECellPlacement.P1;
                    cellPlacement |= ECellPlacement.P2;
                    break;
            }
            return cellPlacement != 0;
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

                    case EBatch.AlignProcess:
                        if (
                            SetSubCommandSelectPosition(out ECellPlacement selectCellPlacement) == false
                            || DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false
                            || DoManualAlign(selectCellPlacement) == false
                            )
                        {
                            break;
                        }
                        break;

                    case EBatch.MoveLoadPosition:
                        DoManualStageMove(InShuttleMotorX.ECommand.LoadPosition);
                        break;

                    case EBatch.MoveScanStartWaitPosition:
                        DoManualStageMove(InShuttleMotorX.ECommand.ScanStartWaitPosition);
                        break;

                    case EBatch.MoveScanTriggerStartPosition:
                        DoManualStageMove(InShuttleMotorX.ECommand.ScanTriggerStartPosition);
                        break;

                    case EBatch.MoveScanTriggerEndPosition:
                        DoManualStageMove(InShuttleMotorX.ECommand.ScanTriggerEndPosition);
                        break;

                    case EBatch.MoveAlignPosition:
                        DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition);
                        break;

                    case EBatch.MoveUnloadPosition:
                        DoManualStageMove(InShuttleMotorX.ECommand.UnloadPosition);
                        break;

                    case EBatch.ScanProcess:
                        DoManualScanProcess();
                        break;

                    case EBatch.AlignGrabP1:
                        DoManualAlign(ECellPlacement.P1);
                        break;

                    case EBatch.AlignGrabP2:
                        DoManualAlign(ECellPlacement.P2);
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
using SVI_NFT_R.CellData;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class OutFlip
    {
        public enum EBatch
        {
            None = 0,

            SetZero,
            LoadWait,
            Unload
        }
        public EBatch BatchCommand { get; set; }

        public bool DoManualSetZero()
        {
            // 인터락 체크
            if (MotorConveyorX.m_objInterlock.CheckMotionClassInterlock(MotorConveyorX.MotorIndex.ToString(), (int)OutFlipMotorX.ECommand.PositionZeroSet) == false)
            {
                return false;
            }
            // Set Zero
            MotorConveyorX.SetCommand(OutFlipMotorX.ECommand.PositionZeroSet);
            if (false == MotorConveyorX.WaitForEndProcess())
            {
                return false;
            }

            return true;
        }

        public bool DoManualLoadWait()
        {
            // 모터 상승 위치 이동
            if (SetSubCommandMoveZ(OutFlipMotorZ.ECommand.UpPosition) == false)
            {
                return false;
            }
            // 모터 로드 위치 이동
            if (SetSubCommandMoveRs(OutFlipMotorR.ECommand.LoadPosition) == false)
            {
                return false;
            }
            return true;
        }

        public bool DoManualUnload()
        {
            if (TryGetCellPlacementFromVacuum(out ECellPlacement cellPlacement) == false)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NO_CELL_IN_THE_ROBOT_TOOL_LOAD_THE_CELL_INTO_THE_ROBOT_TOOL_AND_TRY_AGAIN);
                return false;
            }
            try
            {
                // 모터 상승 위치 이동
                if (SetSubCommandMoveZ(OutFlipMotorZ.ECommand.UpPosition) == false)
                {
                    return false;
                }
                // 모터 로드 위치 이동
                if (SetSubCommandMoveRs(OutFlipMotorR.ECommand.UnloadPosition) == false)
                {
                    return false;
                }
                // 모터 하강 위치 이동
                if (SetSubCommandMoveZ(OutFlipMotorZ.ECommand.DownPosition) == false)
                {
                    return false;
                }
                Array.ForEach(Pickers, picker => picker.SetCommand(OutFlipPicker.ECommand.Place));
                if (Pickers.WaitForEndProcess() == false)
                {
                    return false;
                }
                // 모터 상승 위치 이동
                if (SetSubCommandMoveZ(OutFlipMotorZ.ECommand.UpPosition) == false)
                {
                    return false;
                }
                // Set Zero
                MotorConveyorX.SetCommand(OutFlipMotorX.ECommand.PositionZeroSet);
                if (false == MotorConveyorX.WaitForEndProcess())
                {
                    return false;
                }
            }
            finally
            {
                // 모터 상승 위치 이동
                SetSubCommandMoveZ(OutFlipMotorZ.ECommand.UpPosition);
            }
            return true;
        }

        private bool SetSubCommandMoveZ(OutFlipMotorZ.ECommand command)
        {
            // 인터락 체크
            if (MotorZ.m_objInterlock.CheckMotionClassInterlock(MotorZ.MotorIndex.ToString(), (int)command) == false)
            {
                return false;
            }
            MotorZ.SetCommand(command);
            return MotorZ.WaitForEndProcess();
        }

        private bool SetSubCommandMoveRs(OutFlipMotorR.ECommand command)
        {
            // 인터락 체크
            if (MotorRs.Any(motor => motor.m_objInterlock.CheckMotionClassInterlock(motor.MotorIndex.ToString(), (int)command) == false))
            {
                return false;
            }
            MotorRs.ForEach(motor => motor.SetCommand(command));
            return MotorRs.WaitForEndProcess();
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

                    case EBatch.SetZero:
                        DoManualSetZero();
                        break;

                    case EBatch.LoadWait:
                        DoManualLoadWait();
                        break;

                    case EBatch.Unload:
                        DoManualUnload();
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

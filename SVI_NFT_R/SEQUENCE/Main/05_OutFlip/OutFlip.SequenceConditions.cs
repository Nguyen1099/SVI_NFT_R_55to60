using System;
using SVI_NFT_R.EHS;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class OutFlip : CProcessAbstract
    {
        private enum EAutoSequence
        {
            LoadWait = 0,
            UnloadWait,
            ShowNotification,
            CloseNotification,
            Unload,
        }

        private enum EOutConveyorSequence
        {
            Stop = 0,
            Run,
            Mcc,
        }

        public bool IsExternalConveyorRun
        {
            get
            {
                bool bExternalConveyorRun = false;
                // 컨베이어 구동 가능 상태 플래그 (ON:구동가능, OFF:구동불가능)
                m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_LOWER_CONVEYOR_RUN_ABLE, ref bExternalConveyorRun);
                if (
                    true == m_objDocument.IsManualInputMode
                    || m_objDocument.IsCimQualMode == true
                    || CDefine.ERunMode.UnlinkDryrun == m_objDocument.GetRunMode()
                    )
                {
                    bExternalConveyorRun = true;
                }
                return bExternalConveyorRun;
            }
        }

        private bool IsOutFlip(EAutoSequence sequenceIndex)
        {
            switch (sequenceIndex)
            {
                case EAutoSequence.LoadWait:
                    return CheckIsLoadWaitCondition();

                case EAutoSequence.UnloadWait:
                    return CheckIsUnloadWaitCondition();

                case EAutoSequence.ShowNotification:
                    return CheckIsShowNotificationCondition();

                case EAutoSequence.CloseNotification:
                    return CheckIsCloseNotificationCondition();

                case EAutoSequence.Unload:
                    return CheckIsUnloadCondition();
                default:
                    Debug.Assert(false);
                    break;
            }
            return true;
        }

        private bool IsOutConveyor(EOutConveyorSequence condition)
        {
            switch (condition)
            {
                case EOutConveyorSequence.Stop:
                    return CheckIsOutConveyorStopCondition();

                case EOutConveyorSequence.Run:
                    return CheckIsOutConveyorRunCondition();

                case EOutConveyorSequence.Mcc:
                    return CheckIsOutConveyorMccCondition();

                default:
                    Debug.Assert(false);
                    break;
            }
            return true;
        }

        private bool CheckIsLoadWaitCondition()
        {
            bool bIsWaitLoadPosition = MotorZ.GetStatus() == OutFlipMotorZ.EStatus.UpPosition
                && MotorRs.All(motor => motor.GetStatus() == OutFlipMotorR.EStatus.LoadPosition);
            // 모터가 로드 위치다
            if (bIsWaitLoadPosition == true
                // !!! 언로드 완료시 모터 위치에따라 해당 시퀀스를 타지 않았을 때 택타임 로그가 꼬이기 때문에 추가한 조건임
                && IsCompleteWriteCycleLog == true
                )
            {
                return false;
            }
            // Out Flip에 Cell Data가 있다
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

        private bool CheckIsUnloadWaitCondition()
        {
            // 모터가 언로드 위치다
            if (IsWaitUnloadPosition == true)
            {
                return false;
            }
            // Out Flip에 Cell Data가 있다
            if (CellContainer.IsCellExistFromList() == false)
            {
                return false;
            }
            // Out Robot이 Unload Interlock 상태다
            if (m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot.Nachi.IsUnloadInterlock() == true)
            {
                return false;
            }
            if (CanMoving() == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsShowNotificationCondition()
        {
            mNotificationMessage = 0;
            if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Start)
            {
                return false;
            }
            //if (MotorConveyorX.CanMovingGroupByEhsPolicy() == false)
            //{
            //    return false;
            //}
            if (IsOtherEquipmentPendingTimeout() == true)
            {
                mNotificationMessage = CAlarmDefine.EMessageList.LOWER_EQUIPMENT_INTERFACE_PENDING_PARAM1;
            }

            if (mNotificationMessage == 0)
            {
                return false;
            }
            if (mDialogNotificationOrNull != null
                && mDialogNotificationOrNull.MessageIndex == mNotificationMessage
                )
            {
                return false;
            }
            return true;
        }

        public bool IsOtherEquipmentPendingTimeout()
        {
            // true=타임아웃 발생, false=정상
            if (IsUnloadPending == true)
            {
                if (DateTime.Now - mLastPendingDateTime < Config.WaitTime.Equipment.UnloadInterfacePendingTime.ToTimeSpan())
                {
                    return false;
                }
            }
            else
            {
                mLastPendingDateTime = DateTime.Now;
                return false;
            }
            return true;
        }

        private bool CheckIsCloseNotificationCondition()
        {
            if (mNotificationMessage != 0)
            {
                return false;
            }
            if (mDialogNotificationOrNull == null)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsUnloadCondition()
        {
            if (mDialogNotificationOrNull != null)
            {
                return false;
            }
            // Out Flip에 Cell Data가 없다
            if (CellContainer.IsCellExistFromList() == false)
            {
                return false;
            }
            // 모터가 언로드 위치가 아니다
            if (IsWaitUnloadPosition == false)
            {
                return false;
            }
            // 아웃 컨베이어가 언로드 위치가 아니다
            //if (MotorConveyorX.IsArrivalUnloadPosition() == false)
            //{
            //    return false;
            //}
            if (CanMoving() == false)
            {
                return false;
            }
            return true;
        }

        private bool CanMoving()
        {
            //if (MotorConveyorX.CanMovingGroupByEhsPolicy() == false)
            //{
            //    return false;
            //}
            return true;
        }

        private bool CheckIsOutConveyorStopCondition()
        {
            // 매뉴얼 동작으로 제어권을 가져 갔을 때
            if (IsOutConveyorAutoMode == false)
            {
                return false;
            }
            // 외부 컨베이어가 동작중이다
            //if (IsExternalConveyorRun == true)
            //{
            //    // 언로드 위치에 도착하지 못했다
            //    if (MotorConveyorX.IsArrivalUnloadPosition() == false)
            //    {
            //        return false;
            //    }
            //}
            // 컨베이어가 동작중이 아니다
            //if (MotorConveyorX.IsRunning() == false)
            //{
            //    // ! MCC 로그 동기를 맞추기 위해 외부 요인에 의해서 정지된 경우가 아니라도 한 번은 실행되어야함
            //    if (mConveyorRun == false)
            //    {
            //        return false;
            //    }
            //}
            return true;
        }

        private bool CheckIsOutConveyorRunCondition()
        {
            // 매뉴얼 동작으로 제어권을 가져 갔을 때
            if (IsOutConveyorAutoMode == false)
            {
                return false;
            }
            // 아웃 플립에서 아웃 컨베이어로 셀 이동중이다
            if (IsUnloadOutFlipToOutConveyor == true)
            {
                return false;
            }
            // 언로드 위치에 도착했다
            //if (MotorConveyorX.IsArrivalUnloadPosition() == true)
            //{
            //    return false;
            //}
            //// 컨베이어가 동작중이다
            //if (MotorConveyorX.IsRunning() == true)
            //{
            //    // ! MCC 로그 동기를 맞추기 위해 외부 요인에 의해서 시작된 경우가 아니라도 한 번은 실행되어야함
            //    if (mConveyorRun == true)
            //    {
            //        return false;
            //    }
            //}
            // 외부 컨베이어가 동작중이 아니다
            if (IsExternalConveyorRun == false)
            {
                return false;
            }
            // 모터가 상승 위치가 아니다
            if (MotorZ.IsInposition(OutFlipMotorZ.ECommand.UpPosition) == false)
            {
                return false;
            }
            return true;
        }

        private bool CheckIsOutConveyorMccCondition()
        {
            // 매뉴얼 동작으로 제어권을 가져 갔을 때
            if (IsOutConveyorAutoMode == false)
            {
                return false;
            }
            // 아웃 플립에서 아웃 컨베이어로 셀 이동중이다
            if (IsUnloadOutFlipToOutConveyor == true)
            {
                return false;
            }
            if (mConveyorCellExist.All(x => x == false) == true)
            {
                return false;
            }
            return true;
        }
    }
}

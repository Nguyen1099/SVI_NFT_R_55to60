using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class OutRobot
    {
        private Func<bool> mRobotCycleEnd = null;

        private void SubSequenceSetPreOrder(List<RobotSequenceItemSet> workOrders, int currentIndex)
        {
            /// <![CDATA[
            /// - 나찌 로봇 택타임을 줄이기 위한 트릭임
            /// - OneByOne 시퀀스와 같이 퍼밋을 연속으로 사용하는 시퀀스에서 택타임 감소 효과가 있음 (구간당 100~200ms 정도에 이득)
            ///
            /// 하는 일
            /// - 로봇이 다운 위치에 도착 했을 때 다음 위치 선택 신호와 퍼밋을 미리 켜줌
            /// ]]>
            // Set Pre-Order
            int nextIndex = currentIndex + 1;
            if (nextIndex < workOrders.Count)
            {
                var nextItem = workOrders[nextIndex];
                Nachi.ProcessPreOrder.Method = nextItem.MethodIndex;
                Nachi.ProcessPreOrder.ToolIndex = nextItem.ToolIndex;
                Nachi.ProcessPreOrder.StageIndex = nextItem.StageIndex;
            }
            else
            {
                Nachi.ProcessPreOrder = ProcessPreOrder.Empty;
            }
        }

        private void SetPreOrderToProcessBegin(OutRobotNachi.ECommand command, List<RobotSequenceItemSet> workOrders)
        {
            /// <![CDATA[
            /// - 나찌 로봇 택타임을 줄이기 위한 트릭임
            /// - 다음 퍼밋 동작이 예측 가능한 시퀀스에 넣어주면 택타임 감소 효과가 있음 (구간당 100~200ms 정도에 이득)
            /// - 이 함수를 사용 하지 않아도 동작상에 문제는 없음
            ///
            /// 하는 일
            /// - ProcessBegin을 하기 전에 퍼밋에 Select 신호를 미리 켜줌
            /// ]]>
            Debug.Assert(command.ToString().Contains("ProcessBegin"), $"ProcessBegin 명령에만 사용 가능합니다. [입력값:{command}]");
            var firstItem = workOrders.FirstOrDefault();
            if (firstItem == null)
            {
                return;
            }
            Nachi.ProcessPreOrder.Method = firstItem.MethodIndex;
            Nachi.ProcessPreOrder.ToolIndex = firstItem.ToolIndex;
            Nachi.ProcessPreOrder.StageIndex = firstItem.StageIndex;
            Nachi.SetProcessPreOrder[command].Invoke();
            Nachi.ProcessPreOrder = ProcessPreOrder.Empty;
        }

        private void SetPreOrderToProcessBegin(OutRobotNachi.ECommand command, RobotSequenceItemSet workOrders)
        {
            /// <![CDATA[
            /// - 나찌 로봇 택타임을 줄이기 위한 트릭임
            /// - 다음 퍼밋 동작이 예측 가능한 시퀀스에 넣어주면 택타임 감소 효과가 있음 (구간당 100~200ms 정도에 이득)
            /// - 이 함수를 사용 하지 않아도 동작상에 문제는 없음
            ///
            /// 하는 일
            /// - ProcessBegin을 하기 전에 퍼밋에 Select 신호를 미리 켜줌
            /// ]]>
            Debug.Assert(command.ToString().Contains("ProcessBegin"), $"ProcessBegin 명령에만 사용 가능합니다. [입력값:{command}]");
            Nachi.ProcessPreOrder.Method = workOrders.MethodIndex;
            Nachi.ProcessPreOrder.ToolIndex = workOrders.ToolIndex;
            Nachi.ProcessPreOrder.StageIndex = workOrders.StageIndex;
            Nachi.SetProcessPreOrder[command].Invoke();
            Nachi.ProcessPreOrder = ProcessPreOrder.Empty;
        }

        private bool SubPreOrderToProcessExit(OutRobotNachi.ECommand command, List<RobotSequenceItemSet> workOrders = null, int currentIndex = -1)
        {
            /// <![CDATA[
            /// - 나찌 로봇 택타임을 줄이기 위한 트릭임
            /// - 다음 종료 동작이 예측 가능한 시퀀스에 넣어주면 택타임 감소 효과가 있음 (구간당 100~200ms 정도에 이득)
            /// - 이 함수를 사용 하지 않아도 동작상에 문제는 없음
            ///
            /// 하는 일
            /// - CycleEnd를 하기 전에 Exit 신호를 미리 켜줌
            /// ]]>
            if (workOrders != null
                && currentIndex != -1
                && workOrders.Count != currentIndex + 1
                )
            {
                return true;
            }

            Nachi.SetProcessExitPreOrder[command].Invoke();
            return true;
        }

        private bool SubSequenceCycleEnd()
        {
            if (mRobotCycleEnd?.Invoke() == false)
            {
                mRobotCycleEnd = null;
                return false;
            }
            mRobotCycleEnd = null;
            return true;
        }

        public bool TryGetCellPlacementFromVacuum(out ECellPlacement cellPlacement)
        {
            cellPlacement = 0;
            int cellCount = 0;
            foreach (var picker in Pickers)
            {
                if (picker.Vacuum.Status != CVacuumAbstract.EVacuumStatus.STS_ON)
                {
                    continue;
                }
                cellPlacement |= picker.CellPort.CellPlacement;
                ++cellCount;
            }
            if (cellCount == 0)
            {
                cellPlacement = ECellPlacement.Empty;
                return false;
            }
            if (cellCount == Pickers.Length)
            {
                cellPlacement |= ECellPlacement.Full;
            }
            return true;
        }

        private bool TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, EMethod methodIndex, EProcess fromProcessIndex, ECellPlacement robotCellPlacement, ECellPlacement otherCellPlacement)
        {
            workOrders = new List<RobotSequenceItemSet>(2);
            if (methodIndex == EMethod.None)
            {
                return false;
            }
            if (robotCellPlacement.HasFlag(ECellPlacement.Empty) == true
                && otherCellPlacement.HasFlag(ECellPlacement.Empty) == true
                )
            {
                return false;
            }
            ECellPlacement source;
            ECellPlacement destination;
            if (methodIndex == EMethod.Put)
            {
                source = robotCellPlacement;
                destination = otherCellPlacement;
            }
            else /*if (methodIndex == EMethod.Get)*/
            {
                source = otherCellPlacement;
                destination = robotCellPlacement;
            }

            if (source.HasFlag(ECellPlacement.P1) == true
                && destination.HasFlag(ECellPlacement.P1) == false
                )
            {
                var workOrder = new RobotSequenceItemSet(this, methodIndex, fromProcessIndex) { StageIndex = EStage.Stage1 };
                workOrder.SetRobotToolIndex(ETool.Tool1);
                workOrders.Add(workOrder);
            }
            if (source.HasFlag(ECellPlacement.P2) == true
                && destination.HasFlag(ECellPlacement.P2) == false
                )
            {
                var workOrder = new RobotSequenceItemSet(this, methodIndex, fromProcessIndex) { StageIndex = EStage.Stage2 };
                workOrder.SetRobotToolIndex(ETool.Tool2);
                workOrders.Add(workOrder);
            }
            return workOrders.Count != 0;
        }

        private bool TryGetAtOnceWorkOrders(out List<RobotSequenceItemSet> workOrders, EMethod methodIndex, EProcess fromProcessIndex, ECellPlacement robotCellPlacement, ECellPlacement otherCellPlacement)
        {
            workOrders = new List<RobotSequenceItemSet>(2);
            if (methodIndex == EMethod.None)
            {
                return false;
            }
            if (robotCellPlacement.HasFlag(ECellPlacement.Empty) == true
                && otherCellPlacement.HasFlag(ECellPlacement.Empty) == true
                )
            {
                return false;
            }

            ECellPlacement source;
            ECellPlacement destination;
            if (methodIndex == EMethod.Put)
            {
                source = robotCellPlacement;
                destination = otherCellPlacement;
            }
            else /*if (methodIndex == EMethod.Get)*/
            {
                source = otherCellPlacement;
                destination = robotCellPlacement;
            }

            if (source.HasFlag(ECellPlacement.P1 | ECellPlacement.P2) == true
                && destination.HasFlag(ECellPlacement.P1 | ECellPlacement.P2) == false
                )
            {
                var workOrder = new RobotSequenceItemSet(this, methodIndex, fromProcessIndex) { StageIndex = EStage.Stage1 };
                workOrder.SetRobotToolIndex(ETool.Tool12);
                workOrders.Add(workOrder);
            }
            else if (source.HasFlag(ECellPlacement.P1) == true
                && destination.HasFlag(ECellPlacement.P1) == false
                )
            {
                var workOrder = new RobotSequenceItemSet(this, methodIndex, fromProcessIndex) { StageIndex = EStage.Stage1 };
                workOrder.SetRobotToolIndex(ETool.Tool1);
                workOrders.Add(workOrder);
            }
            else if (source.HasFlag(ECellPlacement.P2) == true
                && destination.HasFlag(ECellPlacement.P2) == false
                )
            {
                var workOrder = new RobotSequenceItemSet(this, methodIndex, fromProcessIndex) { StageIndex = EStage.Stage2 };
                workOrder.SetRobotToolIndex(ETool.Tool2);
                workOrders.Add(workOrder);
            }

            return workOrders.Count != 0;
        }

        private bool SubSequenceLoad(EMethod methodIndex, ETool toolIndex, EStage stageIndex, bool bCheckManualInterlock = false)
        {
            bool bWriteMccLog = methodIndex == EMethod.Get
                && bCheckManualInterlock == false;
            OutRobotPicker[] selectRobotPickers = null;
            InspStagePicker[] selectOtherPickers = null;
            var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
            int otherPickerindex = 0;
            switch (toolIndex)
            {
                case ETool.Tool1:
                    selectRobotPickers = new OutRobotPicker[] { Pickers[0] };
                    selectOtherPickers = new InspStagePicker[] { inspStage.Pickers[0] };
                    otherPickerindex = 0;
                    break;

                case ETool.Tool2:
                    selectRobotPickers = new OutRobotPicker[] { Pickers[1] };
                    selectOtherPickers = new InspStagePicker[] { inspStage.Pickers[1] };
                    otherPickerindex = 1;
                    break;

                default:
                    if (bCheckManualInterlock == true)
                    {
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_NOT_SUPPORT_SELECT_COMBINATION_REQUEST);
                        return false;
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                    break;
            }

            // Manual Interlock Check
            if (bCheckManualInterlock == true)
            {
                if (Nachi.m_objInterlock.CheckMotionClassInterlock(string.Empty, (int)OutRobotNachi.ECommand.LoadPermit) == false)
                {
                    return false;
                }
            }

            // Execute
            Nachi.ProcessMethodIndex = methodIndex;
            Nachi.ProcessToolIndex = toolIndex;
            Nachi.ProcessStageIndex = stageIndex;
            if (bWriteMccLog == true)
            {
                Array.ForEach(selectRobotPickers, picker => Nachi.SetMccLogItem(MccLogManager.OutRobot.GET_MOVE_LD_POS[picker.CellPort.PositionIndex]));
            }
            Nachi.SetCommand(OutRobotNachi.ECommand.LoadPermit);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            if (methodIndex == EMethod.Get)
            {
                if (bWriteMccLog == true)
                {
                    Array.ForEach(selectRobotPickers, picker => picker.SetMccLogItem(MccLogManager.OutRobot.GET_VAC_ON[picker.CellPort.PositionIndex]));
                }
                Array.ForEach(selectRobotPickers, picker => picker.SetCommand(OutRobotPicker.ECommand.Pick));
                if (selectRobotPickers.WaitForEndProcess() == false)
                {
                    return false;
                }
                if (bWriteMccLog == true)
                {
                    //Array.ForEach(selectOtherPickers, picker => picker.SetMccLogItem(MccLogManager.InspStage.PUT_VAC_OFF[picker.CellPort.PositionIndex]));
                    MccLogManager.InspStage.PUT_VAC_OFF[otherPickerindex].WriteStart();
                }
                Array.ForEach(selectOtherPickers, picker => picker.SetCommand(InspStagePicker.ECommand.Place));
                if (selectOtherPickers.WaitForEndProcess() == false)
                {
                    return false;
                }
                if (bWriteMccLog == true)
                {
                    MccLogManager.InspStage.PUT_VAC_OFF[otherPickerindex].WriteEnd();
                }
            }
            else if (methodIndex == EMethod.Put)
            {
                Array.ForEach(selectOtherPickers, picker => picker.SetCommand(InspStagePicker.ECommand.Pick));
                if (selectOtherPickers.WaitForEndProcess() == false)
                {
                    return false;
                }
                Array.ForEach(selectRobotPickers, picker => picker.SetCommand(OutRobotPicker.ECommand.Place));
                if (selectRobotPickers.WaitForEndProcess() == false)
                {
                    return false;
                }
            }

            mRobotCycleEnd = () =>
            {
                if (bWriteMccLog == true)
                {
                    Array.ForEach(selectRobotPickers, picker => Nachi.SetMccLogItem(MccLogManager.OutRobot.GET_MOVE_LD_UP_POS[picker.CellPort.PositionIndex]));
                }
                Nachi.SetCommand(OutRobotNachi.ECommand.LoadCycleEnd);
                if (Nachi.WaitForEndProcess() == false)
                {
                    return false;
                }

                return true;
            };
            return true;
        }

        private bool SubSequenceUnload(EMethod methodIndex, ETool toolIndex, EStage stageIndex, DEVICE.Nachi.ERobotCylinder cylinderIndex, bool bCheckManualInterlock = false)
        {
            bool bWriteMccLog = methodIndex == EMethod.Put
                && bCheckManualInterlock == false;
            OutRobotPicker[] selectRobotPickers = null;
            OutFlipPicker[] selectOtherPickers = null;
            var outFlip = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip;

            switch (toolIndex)
            {

                case ETool.Tool1:
                    selectRobotPickers = new OutRobotPicker[] { Pickers[0] };
                    var outflipPickerP1 = cylinderIndex == ERobotCylinder.Turn ? outFlip.Pickers[0] : outFlip.Pickers[1];
                    selectOtherPickers = new OutFlipPicker[] { outflipPickerP1 };
                    break;

                case ETool.Tool2:
                    selectRobotPickers = new OutRobotPicker[] { Pickers[1] };
                    var outflipPickerP2 = cylinderIndex == ERobotCylinder.Turn ? outFlip.Pickers[2] : outFlip.Pickers[3];
                    selectOtherPickers = new OutFlipPicker[] { outflipPickerP2 };
                    break;

                default:
                    if (bCheckManualInterlock == true)
                    {
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.ROBOT_NOT_SUPPORT_SELECT_COMBINATION_REQUEST);
                        return false;
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                    break;
            }

            // Manual Interlock Check
            if (bCheckManualInterlock == true)
            {
                if (Nachi.m_objInterlock.CheckMotionClassInterlock(string.Empty, (int)OutRobotNachi.ECommand.UnloadPermit) == false)
                {
                    return false;
                }
            }

            // Execute
            Nachi.ProcessMethodIndex = methodIndex;
            Nachi.ProcessToolIndex = toolIndex;
            Nachi.ProcessStageIndex = stageIndex;
            if (bWriteMccLog == true)
            {
                Array.ForEach(selectRobotPickers, picker => Nachi.SetMccLogItem(MccLogManager.OutRobot.PUT_MOVE_ULD_POS[picker.CellPort.PositionIndex]));
            }
            Nachi.SetCommand(OutRobotNachi.ECommand.UnloadPermit);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }

            if (methodIndex == EMethod.Get)
            {
                Array.ForEach(selectRobotPickers, picker => picker.SetCommand(OutRobotPicker.ECommand.Pick));
                if (selectRobotPickers.WaitForEndProcess() == false)
                {
                    return false;
                }
                Array.ForEach(selectOtherPickers, picker => picker.SetCommand(OutFlipPicker.ECommand.Place));
                if (selectOtherPickers.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            else if (methodIndex == EMethod.Put)
            {
                if (bWriteMccLog == true)
                {
                    Array.ForEach(selectOtherPickers, picker => picker.SetMccLogItem(MccLogManager.OutFlip.GET_VAC_ON[picker.CellPort.PositionIndex]));
                }
                Array.ForEach(selectOtherPickers, picker => picker.SetCommand(OutFlipPicker.ECommand.Pick));
                if (selectOtherPickers.WaitForEndProcess() == false)
                {
                    return false;
                }
                if (bWriteMccLog == true)
                {
                    Array.ForEach(selectRobotPickers, picker => picker.SetMccLogItem(MccLogManager.OutRobot.PUT_VAC_OFF[picker.CellPort.PositionIndex]));
                }
                Array.ForEach(selectRobotPickers, picker => picker.SetCommand(OutRobotPicker.ECommand.Place));
                if (selectRobotPickers.WaitForEndProcess() == false)
                {
                    return false;
                }
            }
            mRobotCycleEnd = () =>
            {
                if (bWriteMccLog == true)
                {
                    Array.ForEach(selectRobotPickers, picker => Nachi.SetMccLogItem(MccLogManager.OutRobot.PUT_MOVE_ULD_UP_POS[picker.CellPort.PositionIndex]));
                }
                Nachi.SetCommand(OutRobotNachi.ECommand.UnloadCycleEnd);
                if (Nachi.WaitForEndProcess() == false)
                {
                    return false;
                }

                return true;
            };
            return true;
        }
    }
}
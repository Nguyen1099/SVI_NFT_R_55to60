using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class OutRobot
    {
        public enum EBatch
        {
            None = 0,

            LoadGet,
            LoadPut,
            UnloadGet,
            UnloadPut,
        }
        public EBatch BatchCommand { get; set; } = EBatch.None;
        private string mLastSelectPosition = string.Empty;
        private ERobotCylinder mSelectCylinderIndex = 0;

        private bool DoManualLoad(EMethod method)
        {
            TryGetCellPlacementFromVacuum(out ECellPlacement robotCellPlacement);
            var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
            inspStage.TryGetCellPlacementFromVacuum(out ECellPlacement otherCellPlacement);
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, method, EProcess.InspStage, robotCellPlacement, otherCellPlacement) == false)
            {
                return false;
            }

            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)OutRobotNachi.ECommand.LoadProcessBegin) == false)
            {
                return false;
            }
            Array.ForEach(TurnCylinders, cylinder => cylinder.SetCommand(mDefine.LoadPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetCommand(OutRobotNachi.ECommand.LoadProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            try
            {
                foreach (var workOrder in workOrders)
                {
                    if (SubSequenceLoad(workOrder.MethodIndex, workOrder.ToolIndex, workOrder.StageIndex, true) == false)
                    {
                        return false;
                    }
                    if (SubSequenceCycleEnd() == false)
                    {
                        return false;
                    }
                }
            }
            finally
            {
                Nachi.SetCommand(OutRobotNachi.ECommand.LoadProcessExit);
                Nachi.WaitForEndProcess();
            }
            return true;
        }

        private bool DoManualUnload(EMethod method)
        {
            TryGetCellPlacementFromVacuum(out ECellPlacement robotCellPlacement);
            var outFlip = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip;
            outFlip.TryGetCellPlacementFromVacuum(out ECellPlacement otherCellPlacement);
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, method, EProcess.OutFlip, robotCellPlacement, otherCellPlacement) == false)
            {
                return false;
            }

            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)OutRobotNachi.ECommand.UnloadProcessBegin) == false)
            {
                return false;
            }
            Array.ForEach(TurnCylinders, cylinder => cylinder.SetCommand(mDefine.UnloadPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetCommand(OutRobotNachi.ECommand.UnloadProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            try
            {
                Array.ForEach(TurnCylinders, cylinder => cylinder.SetCommand(mSelectCylinderIndex == ERobotCylinder.Return ? OutRobotTurnCylinder.ECommand.Return : OutRobotTurnCylinder.ECommand.Turn));
                if (TurnCylinders.WaitForEndProcess() == false)
                {
                    return false;
                }
                foreach (var workOrder in workOrders)
                {
                    if (SubSequenceUnload(workOrder.MethodIndex, workOrder.ToolIndex, workOrder.StageIndex, mSelectCylinderIndex, true) == false)
                    {
                        return false;
                    }
                    if (SubSequenceCycleEnd() == false)
                    {
                        return false;
                    }
                }
            }
            finally
            {
                Nachi.SetCommand(OutRobotNachi.ECommand.UnloadProcessExit);
                Nachi.WaitForEndProcess();
            }
            return true;
        }

        private bool SetSubCommandSelectCylinder(ref ERobotCylinder selectCylinderIndex)
        {
            m_objDocument.GetMainFrame().ShowWaitMessage(false);
            string selectName = string.Empty;
            try
            {
                m_objDocument.GetMainFrame().Invoke(new Action(() =>
                {
                    using (var dialog = new FormEnumSelect(new string[] { "RETURN", "TURN" }, mLastSelectPosition))
                    {
                        dialog.TitleText = $"SELECT CYLINDER ({Resource.Get(nameof(EProcess.OutRobot))})";
                        if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            return;
                        }

                        mLastSelectPosition = selectName = dialog.ResultName;
                    }
                }));
            }
            finally
            {
                m_objDocument.GetMainFrame().ShowWaitMessage(true);
            }
            switch (selectName)
            {
                case "RETURN":
                    selectCylinderIndex = ERobotCylinder.Return;
                    break;

                case "TURN":
                    selectCylinderIndex = ERobotCylinder.Turn;
                    break;
            }
            return string.IsNullOrEmpty(selectName) == false;

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

                    case EBatch.LoadGet:
                        DoManualLoad(EMethod.Get);
                        break;

                    case EBatch.LoadPut:
                        DoManualLoad(EMethod.Put);
                        break;

                    case EBatch.UnloadGet:
                        DoManualUnload(EMethod.Get);
                        break;

                    case EBatch.UnloadPut:
                        {
                            if (SetSubCommandSelectCylinder(ref mSelectCylinderIndex) == false
                                || DoManualUnload(EMethod.Put) == false
                                )
                            {
                                break;
                            }
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
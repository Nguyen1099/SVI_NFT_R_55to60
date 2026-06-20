using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class InRobot
    {
        public enum EBatch
        {
            None = 0,

            LoadGet,
            LoadPut,
            McrPut,
            UnloadGet,
            UnloadPut,

            McrGrabP1,
            McrGrabP2,

            AlignZeroSend,
            AlignSend,
            AlignCalibration,
            Align_Verification_Process,
            Align_Verification_Process2,

            Robot_Verification_Process,
            Robot_Verification_Process2,
        }
        public EBatch BatchCommand { get; set; } = EBatch.None;
        private string mLastSelectPosition = string.Empty;

        private bool mbRunningAlignCalibration = false;
        private readonly ManualResetEventSlim mAlignCalibrationCancel = new ManualResetEventSlim(false);
        private readonly ManualResetEventSlim mReceiveAlignCalibrationStart = new ManualResetEventSlim(false);
        private readonly ManualResetEventSlim mReceiveAlignCalibrationFinish = new ManualResetEventSlim(false);
        private readonly ManualResetEventSlim mReceiveAlignError = new ManualResetEventSlim(false);
        private readonly ManualResetEventSlim mAlignCalibrationMoveDone = new ManualResetEventSlim(false);

        private readonly ManualResetEventSlim mAlignVerificationCancel = new ManualResetEventSlim(false);

        private bool DoManualLoad(EMethod method)
        {
            TryGetCellPlacementFromVacuum(out ECellPlacement robotCellPlacement);
            var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
            inShuttle.TryGetCellPlacementFromVacuum(out ECellPlacement inShuttleCellPlacement);
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, method, EProcess.InShuttle, robotCellPlacement, inShuttleCellPlacement) == false)
            {
                return false;
            }

            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)InRobotNachi.ECommand.LoadProcessBegin) == false)
            {
                return false;
            }
            TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.LoadPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetCommand(InRobotNachi.ECommand.LoadProcessBegin);
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
                Nachi.SetCommand(InRobotNachi.ECommand.LoadProcessExit);
                Nachi.WaitForEndProcess();
            }
            return true;
        }

        private bool DoManualLoad(EMethod method, ECellPlacement robotCellPlacement, ECellPlacement otherCellPlacement)
        {
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, method, EProcess.InShuttle, robotCellPlacement, otherCellPlacement) == false)
            {
                return false;
            }

            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)InRobotNachi.ECommand.LoadProcessBegin) == false)
            {
                return false;
            }
            TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.LoadPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetCommand(InRobotNachi.ECommand.LoadProcessBegin);
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
                Nachi.SetCommand(InRobotNachi.ECommand.LoadProcessExit);
                Nachi.WaitForEndProcess();
            }
            return true;
        }

        private bool DoManualMcr()
        {
            if (TryGetCellPlacementFromVacuum(out ECellPlacement robotCellPlacement) == false)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NO_CELL_IN_THE_ROBOT_TOOL_LOAD_THE_CELL_INTO_THE_ROBOT_TOOL_AND_TRY_AGAIN);
                return false;
            }
            if (TryGetAtOnceWorkOrders(out List<RobotSequenceItemSet> workOrders, EMethod.Put, EProcess.InRobot, robotCellPlacement, ECellPlacement.Empty) == false)
            {
                return false;
            }

            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)InRobotNachi.ECommand.McrProcessBegin) == false)
            {
                return false;
            }
            TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.McrPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetCommand(InRobotNachi.ECommand.McrProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            try
            {
                foreach (var workOrder in workOrders)
                {
                    if (SubSequenceMcr(workOrder.MethodIndex, workOrder.ToolIndex, workOrder.StageIndex, true) == false)
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
                Nachi.SetCommand(InRobotNachi.ECommand.McrProcessExit);
                Nachi.WaitForEndProcess();
            }
            return true;
        }

        private bool DoManualUnload(EMethod method)
        {
            TryGetCellPlacementFromVacuum(out ECellPlacement robotCellPlacement);
            var inspStage = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage;
            inspStage.TryGetCellPlacementFromVacuum(out ECellPlacement inspStageCellPlacement);
            if (TryGetOneByOneWorkOrders(out List<RobotSequenceItemSet> workOrders, method, EProcess.InspStage, robotCellPlacement, inspStageCellPlacement) == false)
            {
                return false;
            }

            if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)InRobotNachi.ECommand.UnloadProcessBegin) == false)
            {
                return false;
            }
            TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.UnloadPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetCommand(InRobotNachi.ECommand.UnloadProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            try
            {
                foreach (var workOrder in workOrders)
                {
                    if (SubSequenceUnload(workOrder.MethodIndex, workOrder.ToolIndex, workOrder.StageIndex, true) == false)
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
                Nachi.SetCommand(InRobotNachi.ECommand.UnloadProcessExit);
                Nachi.WaitForEndProcess();
            }
            return true;
        }

        private bool DoManualAlignSend()
        {
            AlignSend.AlignData.AlignTool1.X = mPreAlign.AlignResults[0].X;
            AlignSend.AlignData.AlignTool1.Y = mPreAlign.AlignResults[0].Y;
            AlignSend.AlignData.AlignTool1.T = mPreAlign.AlignResults[0].T;
            AlignSend.AlignData.AlignTool2.X = mPreAlign.AlignResults[1].X;
            AlignSend.AlignData.AlignTool2.Y = mPreAlign.AlignResults[1].Y;
            AlignSend.AlignData.AlignTool2.T = mPreAlign.AlignResults[1].T;
            AlignSend.SetCommand(InRobotSendAlign.ECommand.AlignDataSend);
            if (AlignSend.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private bool DoManualAlignZeroSend()
        {
            AlignSend.SetCommand(InRobotSendAlign.ECommand.AlignZeroDataSend);
            if (AlignSend.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private bool DoManualAlignCalibration()
        {
            ManualResetEventSlim alignCalibrationCancel = mAlignCalibrationCancel;
            ManualResetEventSlim alignCalibrationStart = mReceiveAlignCalibrationStart;
            ManualResetEventSlim alignCalibrationFinish = mReceiveAlignCalibrationFinish;
            ManualResetEventSlim alignError = mReceiveAlignError;
            ManualResetEventSlim alignCalibrationMoveDone = mAlignCalibrationMoveDone;
            Action<bool> setRunningAlignCalibrationFlag = value => mbRunningAlignCalibration = value;
            alignCalibrationCancel.Reset();
            alignCalibrationStart.Reset();
            alignCalibrationFinish.Reset();
            alignError.Reset();
            // ! 이동 명령을 받지 않고 캘리브레이션이 완료된 경우 종료되지 않는 문제가 있어서 초기상태를 Set으로 변경함
            alignCalibrationMoveDone.Set();

            try
            {
                // -> (캘리브레이션 준비 버튼 클릭)
                // -> (인터락 체크)
                if (Nachi.m_objInterlock.CheckMotionClassInterlock(Nachi.RobotIndex.ToString(), (int)InRobotNachi.ECommand.Initialize) == false)
                {
                    return false;
                }
                // -> (로봇 초기화)
                m_objDocument.GetMainFrame().ShowWaitMessage(true, $" ALIGN CALIBRATION{Environment.NewLine}[INITIALIZE]");
                Nachi.SetCommand(InRobotNachi.ECommand.Initialize);
                if (Nachi.WaitForEndProcess() == false)
                {
                    return false;
                }
                AlignSend.SetCommand(InRobotSendAlign.ECommand.AlignZeroDataSend);
                if (AlignSend.WaitForEndProcess() == false)
                {
                    return false;
                }
                setRunningAlignCalibrationFlag.Invoke(true);

                var dialog = m_objDocument.GetMainFrame().ShowWaitMessage(true, $" ALIGN CALIBRATION{Environment.NewLine}[WAIT FOR START REQUEST]");
                dialog.CanCancel = true;
                dialog.CancelClick += Dialog_CancelClick;
                // -> [캘리브레이션 요청 받음]
                WaitHandle.WaitAny(new WaitHandle[] { alignCalibrationStart.WaitHandle, alignCalibrationCancel.WaitHandle });
                if (alignCalibrationCancel.IsSet == true)
                {
                    return false;
                }
                // -> (캘리브레이션 요청 응답)

                // !!! 이동은 이벤트 핸들러에서 처리함 !!!
                dialog.CanCancel = false;
                dialog.CancelClick -= Dialog_CancelClick;
                m_objDocument.GetMainFrame().ShowWaitMessage(true, $" ALIGN CALIBRATION{Environment.NewLine}[WAIT FOR NEXT COMMAND]");
                // -> ...

                // -> [캘리브레이션 완료 요청 받음]
                // !!! 멈출 수 있는 상황: 캘리브레이션 완료 요청 받음, 설비 에러 발생, 얼라인 에러 받음 !!!
                WaitHandle.WaitAny(new WaitHandle[] { alignCalibrationFinish.WaitHandle, alignCalibrationCancel.WaitHandle, alignError.WaitHandle });
                if (alignCalibrationCancel.IsSet == true || alignError.IsSet == true)
                {
                    return false;
                }
                // -> (캘리브레이션 완료 응답)
                // -> (로봇 매뉴얼 모드 종료)
                alignCalibrationMoveDone.Wait();
            }
            finally
            {
                setRunningAlignCalibrationFlag.Invoke(false);
                var dialog = m_objDocument.GetMainFrame().ShowWaitMessage(false);
                dialog.CancelClick -= Dialog_CancelClick;
                dialog.CanCancel = false;
                // 옵셋값 초기화
                AlignSend.SetCommand(InRobotSendAlign.ECommand.AlignZeroDataSend);
                AlignSend.WaitForEndProcess();
            }

            return true;
        }

        private void Dialog_CancelClick(object sender, EventArgs e)
        {
            mAlignCalibrationCancel.Set();
        }

        private void Verification_CancelClick(object sender, EventArgs e)
        {
            mAlignVerificationCancel.Set();
        }

        private void PreAlign_ReceiveCalibrationStart(object sender, EventArgs e)
        {
            ManualResetEventSlim alignCalibrationStart = mReceiveAlignCalibrationStart;
            Func<bool> getRunningAlignCalibrationFlag = () => mbRunningAlignCalibration;

            if (getRunningAlignCalibrationFlag.Invoke() == false)
            {
                mPreAlign.AlignInterface.HLSendErrorReportRequest(DEVICE.Align.EWait.Skip);
            }

            alignCalibrationStart.Set();
        }

        private void PreAlign_ReceiveCalibrationMove(object sender, DEVICE.Align.ReceiveData.CalibrationMoveData e)
        {
            ManualResetEventSlim alignCalibrationCancel = mAlignCalibrationCancel;
            ManualResetEventSlim alignCalibrationMoveDone = mAlignCalibrationMoveDone;
            Func<bool> getRunningAlignCalibrationFlag = () => mbRunningAlignCalibration;

            // -> [위치 이동 요청 받음]
            // -> (위치 이동 요청 응답)
            if (getRunningAlignCalibrationFlag.Invoke() == false)
            {
                mPreAlign.AlignInterface.HLSendErrorReportRequest(DEVICE.Align.EWait.Skip);
                return;
            }
            alignCalibrationMoveDone.Reset();

            ETool eTool;
            EStage eStage;
            switch (e.Index)
            {
                case 0:
                    AlignSend.AlignData.AlignTool1.CameraIndex = ECellPlacement.P1;
                    eTool = ETool.Tool1;
                    eStage = EStage.Stage1;
                    break;

                case 1:
                    AlignSend.AlignData.AlignTool2.CameraIndex = ECellPlacement.P2;
                    eTool = ETool.Tool2;
                    eStage = EStage.Stage2;
                    break;

                default:
                    mPreAlign.AlignInterface.HLSendErrorReportRequest(DEVICE.Align.EWait.Skip);
                    alignCalibrationCancel.Set();
                    alignCalibrationMoveDone.Set();
                    return;
            }

            m_objDocument.GetMainFrame().ShowWaitMessage(true, $" ALIGN CALIBRATION{Environment.NewLine}[MOVING]{Environment.NewLine}[GET - CAM: {e.Index}]");
            // -> (셔틀 배출 위치 이동)
            var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.UnloadPosition) == false)
            {
                mPreAlign.AlignInterface.HLSendErrorReportRequest(DEVICE.Align.EWait.Skip);
                alignCalibrationCancel.Set();
                alignCalibrationMoveDone.Set();
                return;
            }
            // -> (자재 로딩)
            if (DoManualAlignCalibrationLoad(EMethod.Get, eStage, eTool) == false)
            {
                mPreAlign.AlignInterface.HLSendErrorReportRequest(DEVICE.Align.EWait.Skip);
                alignCalibrationCancel.Set();
                alignCalibrationMoveDone.Set();
                return;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, $" ALIGN CALIBRATION{Environment.NewLine}[MOVING]{Environment.NewLine}[PUT - CAM: {e.Index}, X: {e.X / 1000d:0.000}, Y: {e.Y / 1000d:0.000}, T: {e.T / 1000d:0.000}]");
            // -> (Offset 반영)
            AlignSend.AlignData.AlignTool1.X = AlignSend.AlignData.AlignTool2.X = e.X / 1000d;
            AlignSend.AlignData.AlignTool1.Y = AlignSend.AlignData.AlignTool2.Y = e.Y / 1000d;
            AlignSend.AlignData.AlignTool1.T = AlignSend.AlignData.AlignTool2.T = e.T / 1000d;
            AlignSend.SetCommand(InRobotSendAlign.ECommand.CalibrationDataSend);
            if (AlignSend.WaitForEndProcess() == false)
            {
                mPreAlign.AlignInterface.HLSendErrorReportRequest(DEVICE.Align.EWait.Skip);
                alignCalibrationCancel.Set();
                alignCalibrationMoveDone.Set();
                return;
            }
            // -> (자재 배출 및 얼라인 위치 이동)
            if (
                DoManualAlignCalibrationLoad(EMethod.Put, eStage, eTool) == false
                || inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false
                )
            {
                mPreAlign.AlignInterface.HLSendErrorReportRequest(DEVICE.Align.EWait.Skip);
                alignCalibrationCancel.Set();
                alignCalibrationMoveDone.Set();
                return;
            }

            // -> (이동 완료 송신)
            alignCalibrationMoveDone.Set();
            mPreAlign.TaskCalibrationMoveCompleteRequest();
            // -> [이동 완료 수신]
            m_objDocument.GetMainFrame().ShowWaitMessage(true, $" ALIGN CALIBRATION{Environment.NewLine}[WAIT FOR NEXT COMMAND]{Environment.NewLine}[CAM: {e.Index}, X: {e.X / 1000d:0.000}, Y: {e.Y / 1000d:0.000}, T: {e.T / 1000d:0.000}]");
        }

        private void PreAlign_ReceiveCalibrationFinish(object sender, EventArgs e)
        {
            ManualResetEventSlim alignCalibrationFinish = mReceiveAlignCalibrationFinish;
            Func<bool> getRunningAlignCalibrationFlag = () => mbRunningAlignCalibration;
            if (getRunningAlignCalibrationFlag.Invoke() == false)
            {
                mPreAlign.AlignInterface.HLSendErrorReportRequest(DEVICE.Align.EWait.Skip);
            }

            alignCalibrationFinish.Set();
        }

        private void PreAlign_ReceiveAlignError(object sender, EventArgs e)
        {
            mReceiveAlignError.Set();
        }

        public bool DoManualAlignVerficationProcess()
        {
            if (SetSubCommandSelectPartialPosition(out ECellPlacement robotCellPlacement) == false)
            {
                return false;
            }

            m_objDocument.GetMainFrame().ShowWaitMessage(true, "ROBOT ALIGN DATA RESET");
            // 얼라인 데이터 리셋
            if (DoManualAlignZeroSend() == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN SHUTTLE MOVE ALIGN");
            // 셔틀 얼라인 위치 이동
            var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
            {
                return false;
            }
            // 보정전
            mPreAlign.SetAlignInformationTemporaryId(robotCellPlacement, "VERIFY_BEFORE");
            mPreAlign.SetCommand(InShuttleAlignVision.ECommand.Align);
            if (mPreAlign.WaitForEndProcess() == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN SHUTTLE MOVE UNLOAD");
            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.UnloadPosition) == false)
            {
                return false;
            }
            // 얼라인 데이터 전송
            if (DoManualAlignSend() == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN ROBOT GET");
            // Pick-up
            if (DoManualLoad(EMethod.Get, ECellPlacement.Empty, robotCellPlacement) == false)
            {
                return false;
            }
            // 얼라인 데이터 리셋
            if (DoManualAlignZeroSend() == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN ROBOT PUT");
            // Place
            if (DoManualLoad(EMethod.Put, robotCellPlacement, ECellPlacement.Empty) == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN SHUTTLE MOVE ALIGN");
            // 셔틀 얼라인 위치 이동
            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
            {
                return false;
            }
            // 보정후
            mPreAlign.SetAlignInformationTemporaryId(robotCellPlacement, "VERIFY_AFTER");
            mPreAlign.SetCommand(InShuttleAlignVision.ECommand.Align);
            if (mPreAlign.WaitForEndProcess() == false)
            {
                return false;
            }

            return true;
        }

        private bool SetSubCommandRepeatSequence(string message, Func<bool> step)
        {
            var dialog = m_objDocument.GetMainFrame().ShowWaitMessage(true, message);
            dialog.CanCancel = true;
            dialog.CancelClick += Verification_CancelClick;

            bool result = step() && mAlignVerificationCancel.IsSet == false;

            dialog.CanCancel = false;
            dialog.CancelClick -= Verification_CancelClick;

            if (result == false)
            {
                m_objDocument.GetMainFrame().ShowWaitMessage(false);
            }

            return result;
        }

        public bool DoManualAlignVerficationProcess2()
        {
            // 위치 선택
            if (SetSubCommandSelectPartialPosition(out ECellPlacement robotCellPlacement) == false)
            {
                return false;
            }
            // 반복 횟수 선택
            if (SetSubCommandSelectRepeatCount(0) == false)
            {
                return false;
            }

            // x, y, t 랜덤 오프셋 선택
            if (SetSubCommandSelectRandomValue(0, 1.0, 999.0, "X Random 값 설정") == false)
            {
                return false;
            }

            if (SetSubCommandSelectRandomValue(1, 1.0, 999.0, "Y Random 값 설정") == false)
            {
                return false;
            }

            if (SetSubCommandSelectRandomValue(2, 1.0, 999.0, "T Random 값 설정") == false)
            {
                return false;
            }

            m_objDocument.GetMainFrame().ShowWaitMessage(true);
            mAlignVerificationCancel.Reset();

            var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;

            for (int i = 0; i < mRepeatCounts[0]; i++)
            {
                // 셔틀 언로드 위치 이동
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[0]}] IN SHUTTLE MOVE UNLOAD",
                        () => inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.UnloadPosition)) == false)
                {
                    return false;
                }

                // 얼라인 데이터 리셋
                if (SetSubCommandRepeatSequence($"[{i + 1}/{mRepeatCounts[0]}] ROBOT ALIGN DATA RESET", () => DoManualAlignZeroSend()) == false)
                {
                    return false;
                }

                // Robot Pick-up
                if (SetSubCommandRepeatSequence($"[{i + 1}/{mRepeatCounts[0]}] IN ROBOT GET", () => DoManualLoad(EMethod.Get, ECellPlacement.Empty, robotCellPlacement)) == false)
                {
                    return false;
                }

                // Robot Random Offset Apply & Place
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[0]}] IN ROBOT PUT",
                        () =>
                        {
                            // 랜덤 Offset 적용
                            ApplyAlignOffset(robotCellPlacement, mAlignOffsetCounts[0], mAlignOffsetCounts[1], mAlignOffsetCounts[2]);
                            AlignSend.SetCommand(InRobotSendAlign.ECommand.AlignDataSend);

                            if (AlignSend.WaitForEndProcess() == false)
                            {
                                return false;
                            }
                            // Place
                            return DoManualLoad(EMethod.Put, robotCellPlacement, ECellPlacement.Empty);
                        }) == false)
                {
                    return false;
                }

                // Align
                if (SetSubCommandRepeatSequence(
                    $"[{i + 1}/{mRepeatCounts[0]}] IN SHUTTLE MOVE ALIGN",
                    () =>
                    {
                        // 셔틀 얼라인 위치 이동
                        if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
                        {
                            return false;
                        }
                        // 보정전
                        mPreAlign.SetAlignInformationTemporaryId(robotCellPlacement, "VERIFY_BEFORE");
                        mPreAlign.SetCommand(InShuttleAlignVision.ECommand.Align);
                        return mPreAlign.WaitForEndProcess();
                    }) == false)
                {
                    return false;
                }

                // 셔틀 Unload 위치 이동
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[0]}] IN SHUTTLE MOVE UNLOAD",
                        () =>
                        {
                            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.UnloadPosition) == false)
                            {
                                return false;
                            }
                            // 얼라인 데이터 전송
                            return DoManualAlignSend();
                        }) == false)
                {
                    return false;
                }

                // Robot Algin Offset Apply & Pick
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[0]}] IN ROBOT GET",
                        () =>
                        {
                            // Pick-up
                            if (DoManualLoad(EMethod.Get, ECellPlacement.Empty, robotCellPlacement) == false)
                            {
                                return false;
                            }
                            // 얼라인 데이터 리셋
                            return DoManualAlignZeroSend();
                        }) == false)
                {
                    return false;
                }

                // Robot Place
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[0]}] IN ROBOT PUT",
                        () => DoManualLoad(EMethod.Put, robotCellPlacement, ECellPlacement.Empty)) == false)
                {
                    return false;
                }

                // 검증 Align
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[0]}] IN SHUTTLE MOVE ALIGN",
                        () =>
                        {
                            // 셔틀 얼라인 위치 이동
                            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
                            {
                                return false;
                            }
                            // 보정후
                            mPreAlign.SetAlignInformationTemporaryId(robotCellPlacement, "VERIFY_AFTER");
                            mPreAlign.SetCommand(InShuttleAlignVision.ECommand.Align);
                            return mPreAlign.WaitForEndProcess();
                        }) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool DoManualRobotVerificationProcess()
        {
            // 위치 선택
            if (SetSubCommandSelectPartialPosition(out ECellPlacement robotCellPlacement) == false)
            {
                return false;
            }

            m_objDocument.GetMainFrame().ShowWaitMessage(true, "ROBOT ALIGN DATA RESET");
            // 얼라인 데이터 리셋
            if (DoManualAlignZeroSend() == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN SHUTTLE MOVE ALIGN");
            // 셔틀 얼라인 위치 이동
            var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;
            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
            {
                return false;
            }
            // Pick-up 전
            mPreAlign.SetAlignInformationTemporaryId(robotCellPlacement, "ROBOT_VERIFY_BEFORE");
            mPreAlign.SetCommand(InShuttleAlignVision.ECommand.Align);
            if (mPreAlign.WaitForEndProcess() == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN SHUTTLE MOVE UNLOAD");
            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.UnloadPosition) == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN ROBOT GET");
            // Pick-up
            if (DoManualLoad(EMethod.Get, ECellPlacement.Empty, robotCellPlacement) == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN ROBOT PUT");
            // Place
            if (DoManualLoad(EMethod.Put, robotCellPlacement, ECellPlacement.Empty) == false)
            {
                return false;
            }
            m_objDocument.GetMainFrame().ShowWaitMessage(true, "IN SHUTTLE MOVE ALIGN");
            // 셔틀 얼라인 위치 이동
            if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
            {
                return false;
            }
            // Place 후
            mPreAlign.SetAlignInformationTemporaryId(robotCellPlacement, "ROBOT_VERIFY_AFTER");
            mPreAlign.SetCommand(InShuttleAlignVision.ECommand.Align);
            if (mPreAlign.WaitForEndProcess() == false)
            {
                return false;
            }

            return true;
        }

        private bool DoManualRobotVerificationProcess2()
        {
            if (SetSubCommandSelectPartialPosition(out ECellPlacement robotCellPlacement) == false)
            {
                return false;
            }

            // 반복 횟수 선택
            if (SetSubCommandSelectRepeatCount(1) == false)
            {
                return false;
            }

            mAlignVerificationCancel.Reset();
            var inShuttle = m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle;

            for (int i = 0; i < mRepeatCounts[1]; i++)
            {
                // 얼라인 데이터 리셋
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[1]}] ROBOT ALIGN DATA RESET",
                        () => DoManualLoad(EMethod.Put, robotCellPlacement, ECellPlacement.Empty)) == false)
                {
                    return false;
                }

                if (SetSubCommandRepeatSequence(
                    $"[{i + 1}/{mRepeatCounts[1]}] IN SHUTTLE MOVE ALIGN",
                    () =>
                    {
                        // 셔틀 얼라인 위치 이동
                        if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
                        {
                            return false;
                        }
                        // Pick-up 전
                        mPreAlign.SetAlignInformationTemporaryId(robotCellPlacement, "VERIFY_BEFORE");
                        mPreAlign.SetCommand(InShuttleAlignVision.ECommand.Align);
                        return mPreAlign.WaitForEndProcess();
                    }) == false)
                {
                    return false;
                }

                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[1]}] IN SHUTTLE MOVE UNLOAD",
                        () => inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.UnloadPosition)) == false)
                {
                    return false;
                }

                // Pick-up
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[1]}] IN ROBOT GET",
                        () => DoManualLoad(EMethod.Get, ECellPlacement.Empty, robotCellPlacement)) == false)
                {
                    return false;
                }

                // Place
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[1]}] IN ROBOT PUT",
                        () => DoManualLoad(EMethod.Put, robotCellPlacement, ECellPlacement.Empty)) == false)
                {
                    return false;
                }

                if (SetSubCommandRepeatSequence(
                    $"[{i + 1}/{mRepeatCounts[1]}] IN SHUTTLE MOVE ALIGN",
                    () =>
                    {
                        // 셔틀 얼라인 위치 이동
                        if (inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.AlignPosition) == false)
                        {
                            return false;
                        }
                        // Place 후
                        mPreAlign.SetAlignInformationTemporaryId(robotCellPlacement, "ROBOT_VERIFY_AFTER");
                        mPreAlign.SetCommand(InShuttleAlignVision.ECommand.Align);
                        return mPreAlign.WaitForEndProcess();
                    }) == false)
                {
                    return false;
                }

                // 셔틀 언로드 위치 이동
                if (SetSubCommandRepeatSequence(
                        $"[{i + 1}/{mRepeatCounts[1]}] IN SHUTTLE MOVE UNLOAD",
                        () => inShuttle.DoManualStageMove(InShuttleMotorX.ECommand.UnloadPosition)) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool DoManualAlignCalibrationLoad(EMethod eMethod, EStage eStage, ETool eTool)
        {
            if (Nachi.m_objInterlock.CheckMotionClassInterlock(string.Empty, (int)InRobotNachi.ECommand.LoadProcessBegin) == false)
            {
                return false;
            }
            TurnCylinders.ForEach(cylinder => cylinder.SetCommand(mDefine.LoadPositionCylinder));
            if (TurnCylinders.WaitForEndProcess() == false)
            {
                return false;
            }
            Nachi.SetCommand(InRobotNachi.ECommand.LoadProcessBegin);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            if (SubSequenceLoad(eMethod, eTool, eStage, true) == false)
            {
                return false;
            }
            if (SubSequenceCycleEnd() == false)
            {
                return false;
            }
            Nachi.SetCommand(InRobotNachi.ECommand.LoadProcessExit);
            if (Nachi.WaitForEndProcess() == false)
            {
                return false;
            }
            return true;
        }

        private bool SetSubCommandSelectPartialPosition(out ECellPlacement selectCellPlacement)
        {
            selectCellPlacement = ECellPlacement.Empty;
            m_objDocument.GetMainFrame().ShowWaitMessage(false);
            string selectName = string.Empty;
            try
            {
                m_objDocument.GetMainFrame().Invoke(new Action(() =>
                {
                    using (var dialog = new FormEnumSelect(new string[] { "P1", "P2" }, mLastSelectPosition))
                    {
                        dialog.TitleText = $"SELECT POSITION";
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
                switch (selectName)
                {
                    case "P1":
                        selectCellPlacement = ECellPlacement.P1;
                        break;

                    case "P2":
                        selectCellPlacement = ECellPlacement.P2;
                        break;
                }
            }

            return string.IsNullOrEmpty(selectName) == false;
        }

        private bool SetSubCommandSelectRepeatCount(int select)
        {
            using (FormKeyPad dialog = new FormKeyPad(mRepeatCounts[select], 1, 9999, $"반복 횟수 설정"))
            {
                dialog.TopMost = true;
                dialog.BringToFront();
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }
                mRepeatCounts[select] = Convert.ToInt32(dialog.m_dResultValue);
            }

            return true;
        }

        private bool SetSubCommandSelectRandomValue(int result, double min, double max, string title)
        {
            using (FormKeyPad dialog = new FormKeyPad(mAlignOffsetCounts[result], min, max, title))
            {
                dialog.TopMost = true;
                dialog.BringToFront();
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                mAlignOffsetCounts[result] = Convert.ToDouble(dialog.m_dResultValue);
            }
            return true;
        }

        private void ApplyAlignOffset(ECellPlacement placement, double x, double y, double t)
        {
            Random rand = new Random();
            double offsetX = rand.NextDouble() * x;
            double offsetY = rand.NextDouble() * y;
            double offsetT = rand.NextDouble() * t;

            switch (placement)
            {
                case ECellPlacement.P1:
                    AlignSend.AlignData.AlignTool1.X = offsetX;
                    AlignSend.AlignData.AlignTool1.Y = offsetY;
                    AlignSend.AlignData.AlignTool1.T = offsetT;
                    break;

                case ECellPlacement.P2:
                    AlignSend.AlignData.AlignTool2.X = offsetX;
                    AlignSend.AlignData.AlignTool2.Y = offsetY;
                    AlignSend.AlignData.AlignTool2.T = offsetT;
                    break;

                default:
                    Debug.Assert(false);
                    break;
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

                    case EBatch.LoadGet:
                        DoManualLoad(EMethod.Get);
                        break;

                    case EBatch.LoadPut:
                        DoManualLoad(EMethod.Put);
                        break;

                    case EBatch.McrPut:
                        DoManualMcr();
                        break;

                    case EBatch.UnloadGet:
                        DoManualUnload(EMethod.Get);
                        break;

                    case EBatch.UnloadPut:
                        DoManualUnload(EMethod.Put);
                        break;

                    case EBatch.McrGrabP1:
                        Mcrs[0].SetCommand(InRobotMcr.ECommand.GrabOneshot);
                        Mcrs[0].WaitForEndProcess();
                        break;

                    case EBatch.McrGrabP2:
                        Mcrs[1].SetCommand(InRobotMcr.ECommand.GrabOneshot);
                        Mcrs[1].WaitForEndProcess();
                        break;

                    case EBatch.AlignZeroSend:
                        DoManualAlignZeroSend();
                        break;

                    case EBatch.AlignSend:
                        DoManualAlignSend();
                        break;

                    case EBatch.AlignCalibration:
                        DoManualAlignCalibration();
                        break;

                    case EBatch.Align_Verification_Process:
                        DoManualAlignVerficationProcess();
                        break;

                    case EBatch.Align_Verification_Process2:
                        DoManualAlignVerficationProcess2();
                        break;

                    case EBatch.Robot_Verification_Process:
                        DoManualRobotVerificationProcess();
                        break;

                    case EBatch.Robot_Verification_Process2:
                        DoManualRobotVerificationProcess2();
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
using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class InRobot
    {
        private Thread mThreadPendantRemoteControl;
        private readonly Dictionary<CProcessMotion.EVacuum, Thread> mBlowingItems = new Dictionary<CProcessMotion.EVacuum, Thread>();

        private void SetRemoteControlVacuum(CProcessMotion.EVacuum vacuumIndex, CVacuumAbstract.EVacuumCommand command)
        {
            switch (command)
            {
                case CVacuumAbstract.EVacuumCommand.CMD_ON:
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_BUTTON_OPERATION, $"NachiPendantRemoteControl({Nachi.RobotIndex}) -> [{vacuumIndex}] -> [{command}]");
                    m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[vacuumIndex].SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_ON, CVacuumAbstract.ESensorCheck.IGNORE);
                    break;

                case CVacuumAbstract.EVacuumCommand.CMD_BLOW:
                    if (mBlowingItems.ContainsKey(vacuumIndex) == true)
                    {
                        return;
                    }
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_BUTTON_OPERATION, $"NachiPendantRemoteControl({Nachi.RobotIndex}) -> [{vacuumIndex}] -> [{command}]");
                    mBlowingItems[vacuumIndex] = new Thread(() =>
                    {
                        m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[vacuumIndex].SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_BLOW);
                        m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[vacuumIndex].SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
                        mBlowingItems.Remove(vacuumIndex);
                    });
                    mBlowingItems[vacuumIndex].Start();
                    break;
            }
        }

        private void SetEqSignalAcknowledge(CProcessMotion.EVacuum vacuumIndex, IBitOne acknowledgeSignal)
        {
            CVacuumAbstract.EVacuumStatus getStatus = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[vacuumIndex].Status;
            acknowledgeSignal.Value = getStatus == CVacuumAbstract.EVacuumStatus.STS_ON;
        }

        private void SetRemoteControlCylinder(CProcessMotion.ECylinder cylinderIndex, CCylinderAbstract.ECylinderCommand command)
        {
            m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_BUTTON_OPERATION, $"NachiPendantRemoteControl({Nachi.RobotIndex}) -> [{cylinderIndex}] -> [{command}]");
            m_objDocument.m_objProcessMain.m_objProcessMotion.m_objCylinder[cylinderIndex].SetCylinderCommand(command, CCylinderAbstract.ESensorCheck.IGNORE);
        }

        private void ThreadPendantRemoteControlProcess()
        {
            SpinWait.SpinUntil(() => m_objDocument.IsInitialized);
            /////////////////////////////////////////////////////////////
            // + 개발자가 설비에 맞게 수정해야 할 영역
            // RobotSignal: 로봇에서 요청한 신호 조합
            // SetCommand: 신호 조합을 받았을 때 동작
            var toolVacOnReqeust = Nachi.Robot.Signals.CreateBitAreaBitOne($"{Nachi.Robot.Signals.SignalNamePrefix}_TOOL_VAC_ON") as IReadOnlyBitOne;
            var stageVacOnRequest = Nachi.Robot.Signals.CreateBitAreaBitOne($"{Nachi.Robot.Signals.SignalNamePrefix}_P4_STG_VAC_ON") as IReadOnlyBitOne;
            var toolVacOffRequest = Nachi.Robot.Signals.CreateBitAreaBitOne($"{Nachi.Robot.Signals.SignalNamePrefix}_TOOL_VAC_OFF") as IReadOnlyBitOne;
            var stageVacOffRequest = Nachi.Robot.Signals.CreateBitAreaBitOne($"{Nachi.Robot.Signals.SignalNamePrefix}_P4_STG_VAC_OFF") as IReadOnlyBitOne;
            var toolVacOnAcknowledge = Nachi.Robot.Signals.CreateBitAreaBitOne($"{Nachi.Robot.Signals.SignalNamePrefix}_TOOL_VAC_ON_STATUS") as IBitOne;
            var stageVacOnAcknowledge = Nachi.Robot.Signals.CreateBitAreaBitOne($"{Nachi.Robot.Signals.SignalNamePrefix}_P4_STG_VAC_ON_STATUS") as IBitOne;
            var robotVacuumP1 = CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P1;
            var robotVacuumP2 = CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P2;
            var stageVacuumP1 = CProcessMotion.EVacuum.INSP_STAGE_VACUUM_P1;
            var stageVacuumP2 = CProcessMotion.EVacuum.INSP_STAGE_VACUUM_P2;
            var remoteActionItems = new[]
            {
                // 진공
                new {
                    RobotSignal = new Func<bool>(() => toolVacOnReqeust.Value == true),
                    SetCommand = new Action(() => SetRemoteControlVacuum(robotVacuumP1, CVacuumAbstract.EVacuumCommand.CMD_ON))
                },
                new {
                    RobotSignal = new Func<bool>(() => toolVacOnReqeust.Value == true),
                    SetCommand = new Action(() => SetRemoteControlVacuum(robotVacuumP2, CVacuumAbstract.EVacuumCommand.CMD_ON))
                },
                new {
                    RobotSignal = new Func<bool>(() => stageVacOnRequest.Value == true),
                    SetCommand = new Action(() => SetRemoteControlVacuum(stageVacuumP1, CVacuumAbstract.EVacuumCommand.CMD_ON))
                },
                new {
                    RobotSignal = new Func<bool>(() => stageVacOnRequest.Value == true),
                    SetCommand = new Action(() => SetRemoteControlVacuum(stageVacuumP2, CVacuumAbstract.EVacuumCommand.CMD_ON))
                },
                // 파기
                new {
                    RobotSignal = new Func<bool>(() => toolVacOffRequest.Value == true),
                    SetCommand = new Action(() => SetRemoteControlVacuum(robotVacuumP1, CVacuumAbstract.EVacuumCommand.CMD_BLOW))
                },
                new {
                    RobotSignal = new Func<bool>(() => toolVacOffRequest.Value == true),
                    SetCommand = new Action(() => SetRemoteControlVacuum(robotVacuumP2, CVacuumAbstract.EVacuumCommand.CMD_BLOW))
                },
                new {
                    RobotSignal = new Func<bool>(() => stageVacOffRequest.Value == true),
                    SetCommand = new Action(() => SetRemoteControlVacuum(stageVacuumP1, CVacuumAbstract.EVacuumCommand.CMD_BLOW))
                },
                new {
                    RobotSignal = new Func<bool>(() => stageVacOffRequest.Value == true),
                    SetCommand = new Action(() => SetRemoteControlVacuum(stageVacuumP2, CVacuumAbstract.EVacuumCommand.CMD_BLOW))
                },
            };
            var acknowledgeItems = new[]
            {
                new
                {
                    Vacuum = robotVacuumP1,
                    Signal = toolVacOnAcknowledge
                },
                new
                {
                    Vacuum = robotVacuumP2,
                    Signal = toolVacOnAcknowledge
                },
                new
                {
                    Vacuum = stageVacuumP1,
                    Signal = stageVacOnAcknowledge
                },
                new
                {
                    Vacuum = stageVacuumP2,
                    Signal = stageVacOnAcknowledge
                },
            };
            // - 개발자가 설비에 맞게 수정해야 할 영역
            /////////////////////////////////////////////////////////////
            bool[] bLastStatus = new bool[remoteActionItems.Length];
            while (mbThreadExit == false)
            {
                Thread.Sleep(10);

                // 리모트 컨트롤 기능 미사용 상태다
                if (m_objDocument.m_objConfig.GetOptionParameter().bUseNachiPendantRemoteControl == false)
                {
                    Array.ForEach(bLastStatus, i => i = false);
                    continue;
                }

                // 설비가 정지 상태가 아니다
                if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Stop)
                {
                    Array.ForEach(bLastStatus, i => i = false);
                    continue;
                }

                // 로봇이 티치 상태가 아니다
                if (Nachi.Robot.Status.IsTeachModeEntry == false)
                {
                    Array.ForEach(bLastStatus, i => i = false);
                    continue;
                }

                // 진공 상태 업데이트
                foreach (var item in acknowledgeItems)
                {
                    SetEqSignalAcknowledge(item.Vacuum, item.Signal);
                }

                for (int i = 0; i < remoteActionItems.Length; i++)
                {
                    var item = remoteActionItems[i];
                    bool getStatus = item.RobotSignal.Invoke();
                    if (bLastStatus[i] == getStatus)
                    {
                        continue;
                    }
                    bLastStatus[i] = getStatus;
                    if (bLastStatus[i] == false)
                    {
                        continue;
                    }
                    item.SetCommand.Invoke();
                }
            }

            Nachi.Robot.Signals.DisposeSignalHandler(toolVacOnReqeust as IHaveLogEvent);
            Nachi.Robot.Signals.DisposeSignalHandler(stageVacOnRequest as IHaveLogEvent);
            Nachi.Robot.Signals.DisposeSignalHandler(toolVacOffRequest as IHaveLogEvent);
            Nachi.Robot.Signals.DisposeSignalHandler(stageVacOffRequest as IHaveLogEvent);
            Nachi.Robot.Signals.DisposeSignalHandler(toolVacOnAcknowledge as IHaveLogEvent);
            Nachi.Robot.Signals.DisposeSignalHandler(stageVacOnAcknowledge as IHaveLogEvent);
        }
    }
}
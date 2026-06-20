using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InShuttleMotorX
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CProcessMotion.EMotor MotorIndex { get; private set; }
            public CProcessMotion.EMotor TriggerModuleIndex { get; private set; }
            public CDefine.EInspectType InspectType { get; private set; }
            public bool IsUsingTrigger { get; private set; }
            public CAlarmDefine.EAlarmList AlarmHomeMotionFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmLoadPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmScanStartWaitPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmScanTriggerStartPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmScanTriggerEndPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAlignPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmUnloadPositionMoveFail { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_01_IN_SHUTTLE;
                        MotorIndex = CProcessMotion.EMotor.IN_SHUTTLE_X1;
                        TriggerModuleIndex = CProcessMotion.EMotor.IN_SHUTTLE_X1_TRIGGER_MODULE;
                        InspectType = CDefine.EInspectType.Main;
                        IsUsingTrigger = true;
                        AlarmHomeMotionFail = CAlarmDefine.EAlarmList.MO_IN_SHUTTLE_X1_HOME_MOTION;
                        AlarmLoadPositionMoveFail = CAlarmDefine.EAlarmList.MO_IN_SHUTTLE_X1_LOAD_MOVE;
                        AlarmScanStartWaitPositionMoveFail = CAlarmDefine.EAlarmList.MO_IN_SHUTTLE_X1_SCAN_START_WAIT_MOVE;
                        AlarmScanTriggerStartPositionMoveFail = CAlarmDefine.EAlarmList.MO_IN_SHUTTLE_X1_SCAN_TRIGGER_START_MOVE;
                        AlarmScanTriggerEndPositionMoveFail = CAlarmDefine.EAlarmList.MO_IN_SHUTTLE_X1_SCAN_TRIGGER_END_MOVE;
                        AlarmAlignPositionMoveFail = CAlarmDefine.EAlarmList.MO_IN_SHUTTLE_X1_ALIGN_MOVE;
                        AlarmUnloadPositionMoveFail = CAlarmDefine.EAlarmList.MO_IN_SHUTTLE_X1_UNLOAD_MOVE;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        }

        private Define mDefine;
    }
}
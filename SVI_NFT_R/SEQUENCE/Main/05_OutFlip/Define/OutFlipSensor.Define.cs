using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutFlipSensor
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CDeviceIODefine.EDigitalInput CellDetectSensor1 { get; private set; }
            public CDeviceIODefine.EDigitalInput CellDetectSensor2 { get; private set; }
            public CDeviceIODefine.EDigitalInput ConveyorBlockSensor { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCheckInputFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCheckBlockedFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCheckOutputCellP1 { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCheckOutputCellP2 { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCheckOutputCellAll { get; private set; }
            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_05_OUT_FLIP;
                        CellDetectSensor1 = CDeviceIODefine.EDigitalInput.X_OUT_CONVEYOR_P1_CELL_DETECT_SENSOR;
                        CellDetectSensor2 = CDeviceIODefine.EDigitalInput.X_OUT_CONVEYOR_P2_CELL_DETECT_SENSOR;
                        //ConveyorBlockSensor = CDeviceIODefine.EDigitalInput.X_OUT_CONVEYOR_CELL_OVERLAP_SENSOR;
                        //AlarmCheckInputFail = CAlarmDefine.EAlarmList.SE_UNLOADER_CONVEYOR_N_CELL_EXIST_CHECK;
                        //AlarmCheckBlockedFail = CAlarmDefine.EAlarmList.SE_UNLOADER_CONVEYOR_N_OUTPUT_BLOCKED_CHECK;
                        //AlarmCheckOutputCellP1 = CAlarmDefine.EAlarmList.SE_UNLOADER_CONVEYOR_P1_CELL_NOT_EXIST_CHECK;
                        //AlarmCheckOutputCellP2 = CAlarmDefine.EAlarmList.SE_UNLOADER_CONVEYOR_P2_CELL_NOT_EXIST_CHECK;
                        //AlarmCheckOutputCellAll = CAlarmDefine.EAlarmList.SE_UNLOADER_CONVEYOR_ALL_CELL_NOT_EXIST_CHECK;

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

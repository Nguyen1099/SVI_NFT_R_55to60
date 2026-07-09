using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutShuttlePicker
    {
        private sealed class Define
        {
            public CProcessMotion.EVacuum VacuumIndex { get; private set; }
            public CDeviceIODefine.EDigitalInput CellDetectSensor { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCheckFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmCheckSensorFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmOverlapFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmOverlapSensorFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmVacuumFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmBlowFail { get; private set; }
            public CellDataHandler CellPort { get; internal set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        VacuumIndex = CProcessMotion.EVacuum.OUT_SHUTTLE_VACUUM_P1;
                        CellDetectSensor = CDeviceIODefine.EDigitalInput.X_OUT_SHUTTLE_P1_CELL_DETECT_SENSOR;
                        AlarmCheckFail = CAlarmDefine.EAlarmList.SE_OUT_SHUTTLE_P1_VACUUM_CHECK;
                        AlarmCheckSensorFail = CAlarmDefine.EAlarmList.SE_OUT_SHUTTLE_P1_SENSOR_CHECK;
                        AlarmOverlapFail = CAlarmDefine.EAlarmList.SE_OUT_SHUTTLE_P1_VACUUM_OVERLAP;
                        AlarmOverlapSensorFail = CAlarmDefine.EAlarmList.SE_OUT_SHUTTLE_P1_SENSOR_OVERLAP;
                        AlarmVacuumFail = CAlarmDefine.EAlarmList.VC_OUT_SHUTTLE_P1_VACUUM;
                        AlarmBlowFail = CAlarmDefine.EAlarmList.VC_OUT_SHUTTLE_P1_BLOW;
                        CellPort = CellDataManager.Cells[ECellData.OutShuttleP1];
                        break;
                    case 1:
                        VacuumIndex = CProcessMotion.EVacuum.OUT_SHUTTLE_VACUUM_P2;
                        CellDetectSensor = CDeviceIODefine.EDigitalInput.X_OUT_SHUTTLE_P2_CELL_DETECT_SENSOR;
                        AlarmCheckFail = CAlarmDefine.EAlarmList.SE_OUT_SHUTTLE_P2_VACUUM_CHECK;
                        AlarmCheckSensorFail = CAlarmDefine.EAlarmList.SE_OUT_SHUTTLE_P2_SENSOR_CHECK;
                        AlarmOverlapFail = CAlarmDefine.EAlarmList.SE_OUT_SHUTTLE_P2_VACUUM_OVERLAP;
                        AlarmOverlapSensorFail = CAlarmDefine.EAlarmList.SE_OUT_SHUTTLE_P2_SENSOR_OVERLAP;
                        AlarmVacuumFail = CAlarmDefine.EAlarmList.VC_OUT_SHUTTLE_P2_VACUUM;
                        AlarmBlowFail = CAlarmDefine.EAlarmList.VC_OUT_SHUTTLE_P2_BLOW;
                        CellPort = CellDataManager.Cells[ECellData.OutShuttleP2];
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
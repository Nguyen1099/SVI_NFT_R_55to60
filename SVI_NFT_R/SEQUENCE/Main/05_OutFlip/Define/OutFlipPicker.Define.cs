using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutFlipPicker
    {
        private sealed class Define
        {
            public CProcessMotion.EVacuum VacuumIndex { get; private set; }
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
                        VacuumIndex = CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P1_1;
                        AlarmCheckFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P1_VACUUM_CHECK;
                        AlarmCheckSensorFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P1_SENSOR_CHECK;
                        AlarmOverlapFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P1_VACUUM_OVERLAP;
                        AlarmOverlapSensorFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P1_SENSOR_OVERLAP;
                        AlarmVacuumFail = CAlarmDefine.EAlarmList.VC_OUT_FLIP_P1_VACUUM;
                        AlarmBlowFail = CAlarmDefine.EAlarmList.VC_OUT_FLIP_P1_BLOW;
                        CellPort = CellDataManager.Cells[ECellData.OutFlipP1];
                        break;
                    case 1:
                        VacuumIndex = CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P1_2;
                        AlarmCheckFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P1_VACUUM_CHECK;
                        AlarmCheckSensorFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P1_SENSOR_CHECK;
                        AlarmOverlapFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P1_VACUUM_OVERLAP;
                        AlarmOverlapSensorFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P1_SENSOR_OVERLAP;
                        AlarmVacuumFail = CAlarmDefine.EAlarmList.VC_OUT_FLIP_P1_VACUUM;
                        AlarmBlowFail = CAlarmDefine.EAlarmList.VC_OUT_FLIP_P1_BLOW;
                        CellPort = CellDataManager.Cells[ECellData.OutFlipP1];
                        break;
                    case 2:
                        VacuumIndex = CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P2_1;
                        AlarmCheckFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P2_VACUUM_CHECK;
                        AlarmCheckSensorFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P2_SENSOR_CHECK;
                        AlarmOverlapFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P2_VACUUM_OVERLAP;
                        AlarmOverlapSensorFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P2_SENSOR_OVERLAP;
                        AlarmVacuumFail = CAlarmDefine.EAlarmList.VC_OUT_FLIP_P2_VACUUM;
                        AlarmBlowFail = CAlarmDefine.EAlarmList.VC_OUT_FLIP_P2_BLOW;
                        CellPort = CellDataManager.Cells[ECellData.OutFlipP2];
                        break;
                    case 3:
                        VacuumIndex = CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P2_2;
                        AlarmCheckFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P2_VACUUM_CHECK;
                        AlarmCheckSensorFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P2_SENSOR_CHECK;
                        AlarmOverlapFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P2_VACUUM_OVERLAP;
                        AlarmOverlapSensorFail = CAlarmDefine.EAlarmList.SE_OUT_FLIP_P2_SENSOR_OVERLAP;
                        AlarmVacuumFail = CAlarmDefine.EAlarmList.VC_OUT_FLIP_P2_VACUUM;
                        AlarmBlowFail = CAlarmDefine.EAlarmList.VC_OUT_FLIP_P2_BLOW;
                        CellPort = CellDataManager.Cells[ECellData.OutFlipP2];
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

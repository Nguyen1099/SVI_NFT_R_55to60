using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutFlipMotorR
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CProcessMotion.EMotor MotorIndex { get; private set; }
            public CAlarmDefine.EAlarmList AlarmHomeMotionFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmLoadPositionMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmUnloadPositionMoveFail { get; private set; }
            public CellDataHandler CellPort { get; internal set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_05_OUT_FLIP;
                        MotorIndex = CProcessMotion.EMotor.OUT_FLIP_R1;
                        AlarmHomeMotionFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_R1_HOME_MOTION;
                        AlarmLoadPositionMoveFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_R1_LOAD_MOVE;
                        AlarmUnloadPositionMoveFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_R1_UNLOAD_MOVE;
                        CellPort = CellDataManager.Cells[ECellData.OutFlipP1];
                        break;

                    case 1:
                        LogType = CDefine.ELogType.LOG_PROCESS_05_OUT_FLIP;
                        MotorIndex = CProcessMotion.EMotor.OUT_FLIP_R2;
                        AlarmHomeMotionFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_R2_HOME_MOTION;
                        AlarmLoadPositionMoveFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_R2_LOAD_MOVE;
                        AlarmUnloadPositionMoveFail = CAlarmDefine.EAlarmList.MO_OUT_FLIP_R2_UNLOAD_MOVE;
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
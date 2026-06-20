using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutRobotTurnCylinder
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CProcessMotion.ECylinder CylinderIndex { get; private set; }
            public CAlarmDefine.EAlarmList AlarmTurnFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmReturnFail { get; private set; }
            public CellDataHandler CellPort { get; internal set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_04_OUT_ROBOT;
                        CylinderIndex = CProcessMotion.ECylinder.OUT_ROBOT_TURN_CYLINDER_P1;
                        AlarmTurnFail = CAlarmDefine.EAlarmList.CY_TRANSFER_ROBOT_OUT_P1_T_TURN;
                        AlarmReturnFail = CAlarmDefine.EAlarmList.CY_TRANSFER_ROBOT_OUT_P1_T_RETURN;
                        CellPort = CellDataManager.Cells[ECellData.OutRobotP1];
                        break;
                    case 1:
                        LogType = CDefine.ELogType.LOG_PROCESS_04_OUT_ROBOT;
                        CylinderIndex = CProcessMotion.ECylinder.OUT_ROBOT_TURN_CYLINDER_P2;
                        AlarmTurnFail = CAlarmDefine.EAlarmList.CY_TRANSFER_ROBOT_OUT_P2_T_TURN;
                        AlarmReturnFail = CAlarmDefine.EAlarmList.CY_TRANSFER_ROBOT_OUT_P2_T_RETURN;
                        CellPort = CellDataManager.Cells[ECellData.OutRobotP2];
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
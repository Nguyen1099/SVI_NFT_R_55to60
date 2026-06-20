using SVI_NFT_R.CellData;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InRobotMcr
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CDefine.EMcr McrIndex { get; private set; }
            public CAlarmDefine.EAlarmList AlarmGrabFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmResultTimeout { get; private set; }
            public CAlarmDefine.EAlarmList AlarmGrabSuccessivelyFail { get; private set; }
            public CellDataHandler CellPort { get; internal set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_02_IN_ROBOT;
                        McrIndex = CDefine.EMcr.P1;
                        AlarmGrabFail = CAlarmDefine.EAlarmList.VI_LOADER_MCR_P1_GRAB_TIMEOUT;
                        AlarmResultTimeout = CAlarmDefine.EAlarmList.VI_LOADER_MCR_P1_RESULT_TIMEOUT;
                        AlarmGrabSuccessivelyFail = CAlarmDefine.EAlarmList.VI_LOADER_MCR_P1_CONTINUOUS_FAILURE;
                        CellPort = CellDataManager.Cells[ECellData.InRobotP1];
                        break;
                    case 1:
                        LogType = CDefine.ELogType.LOG_PROCESS_02_IN_ROBOT;
                        McrIndex = CDefine.EMcr.P2;
                        AlarmGrabFail = CAlarmDefine.EAlarmList.VI_LOADER_MCR_P2_GRAB_TIMEOUT;
                        AlarmResultTimeout = CAlarmDefine.EAlarmList.VI_LOADER_MCR_P2_RESULT_TIMEOUT;
                        AlarmGrabSuccessivelyFail = CAlarmDefine.EAlarmList.VI_LOADER_MCR_P2_CONTINUOUS_FAILURE;
                        CellPort = CellDataManager.Cells[ECellData.InRobotP2];
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

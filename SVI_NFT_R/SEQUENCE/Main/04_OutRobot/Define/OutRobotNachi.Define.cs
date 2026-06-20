using SVI_NFT_R.DEVICE.Nachi;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class OutRobotNachi
    {
        private sealed class Define
        {
            public CDefine.ELogType LogType { get; private set; }
            public CProcessMotion.ERobot RobotIndex { get; private set; }
            public CAlarmDefine.EAlarmList AlarmInitializeFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmRunProcessSkipFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmJcmModeEntryFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmJcmModeMoveFail { get; private set; }
            public CAlarmDefine.EAlarmList AlarmJcmModeExitFail { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_04_OUT_ROBOT;
                        RobotIndex = CProcessMotion.ERobot.OUT_ROBOT;
                        AlarmInitializeFail = CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_N_INITIALIZE;
                        AlarmRunProcessSkipFail = CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_PROCESS_SKIP;
                        AlarmJcmModeEntryFail = CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_ENTER_JCM_MODE;
                        AlarmJcmModeMoveFail = CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_JCM_MODE_MOVE;
                        AlarmJcmModeExitFail = CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_EXIT_JCM_MODE;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            public int GetAlarmProcessBeginFail(ERobotProcess robotProcessIndex)
            {
                switch (robotProcessIndex)
                {
                    case ERobotProcess.P1:
                        return (int)CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_LOAD_BEGIN;

                    case ERobotProcess.P4:
                        return (int)CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_UNLOAD_BEGIN;

                    default:
                        Debug.Assert(false);
                        break;
                }
                return (int)CAlarmDefine.EAlarmList.NOT_REGISTED_ALARM;
            }

            public int GetAlarmProcessPermitFail(ERobotProcess robotProcessIndex)
            {
                switch (robotProcessIndex)
                {
                    case ERobotProcess.P1:
                        return (int)CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_LOAD_PERMIT;

                    case ERobotProcess.P4:
                        return (int)CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_UNLOAD_PERMIT;

                    default:
                        Debug.Assert(false);
                        break;
                }
                return (int)CAlarmDefine.EAlarmList.NOT_REGISTED_ALARM;
            }

            public int GetAlarmProcessExitFail(ERobotProcess robotProcessIndex)
            {
                switch (robotProcessIndex)
                {
                    case ERobotProcess.P1:
                        return (int)CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_LOAD_EXIT;

                    case ERobotProcess.P4:
                        return (int)CAlarmDefine.EAlarmList.MO_TRANSFER_ROBOT_OUT_UNLOAD_EXIT;

                    default:
                        Debug.Assert(false);
                        break;
                }
                return (int)CAlarmDefine.EAlarmList.NOT_REGISTED_ALARM;
            }
        }

        private Define mDefine;
    }
}
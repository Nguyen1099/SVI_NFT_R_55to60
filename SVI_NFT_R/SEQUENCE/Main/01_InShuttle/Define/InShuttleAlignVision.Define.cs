using SVI_NFT_R.CellData;
using System;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public partial class InShuttleAlignVision
    {
        private sealed class Define
        {
            public CDefine.EAlign AlignIndex { get; private set; }
            public bool IsPrimaryDevice { get; private set; }
            public CDefine.ELogType LogType { get; private set; }
            public ECellPlacement[] CameraIndexes { get; private set; }
            public CAlarmDefine.EAlarmList AlarmDisconnected { get; private set; }
            public CAlarmDefine.EAlarmList AlarmReceiveVisionErrorSystemNotRunning { get; private set; }
            public CAlarmDefine.EAlarmList AlarmReceiveVisionErrorCamera { get; private set; }
            public CAlarmDefine.EAlarmList AlarmReceiveVisionErrorLightController { get; private set; }
            public CAlarmDefine.EAlarmList AlarmReceiveVisionErrorLightTempOver { get; private set; }
            public CAlarmDefine.EAlarmList AlarmReceiveVisionError { get; private set; }
            public CAlarmDefine.EAlarmList AlarmModelListTimeout { get; private set; }
            public CAlarmDefine.EAlarmList AlarmModelNotFound { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAlignStartTimeout { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAlignCompleteTimeout { get; private set; }
            public CAlarmDefine.EAlarmList AlarmAlignCompleteValidationFail { get; private set; }
            public CAlarmDefine.EAlarmList[] AlarmInterlockX { get; private set; }
            public CAlarmDefine.EAlarmList[] AlarmInterlockY { get; private set; }
            public CAlarmDefine.EAlarmList[] AlarmInterlockT { get; private set; }
            public CAlarmDefine.EAlarmList[] AlarmInterlockScore { get; private set; }
            public CAlarmDefine.EAlarmList AlarmRetryCountOver { get; private set; }
            public Func<CellDataHandler, OneCell.AlignResultData> GetAlignResultData { get; private set; }

            public Define(int position)
            {
                switch (position)
                {
                    case 0:
                        LogType = CDefine.ELogType.LOG_PROCESS_01_IN_SHUTTLE;
                        AlignIndex = CDefine.EAlign.PreAlign;
                        CameraIndexes = new ECellPlacement[]
                        {
                            ECellPlacement.P1,
                            ECellPlacement.P2
                        };
                        AlarmDisconnected = CAlarmDefine.EAlarmList.CO_PRE_ALIGN_N_DISCONNECTED;
                        AlarmReceiveVisionError = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_N_RECEIVE_ALIGN_ERROR;
                        AlarmReceiveVisionErrorSystemNotRunning = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_ERROR_SYSTEM_NOT_RUNNING;
                        AlarmReceiveVisionErrorCamera = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_ERROR_CAMERA;
                        AlarmReceiveVisionErrorLightController = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_ERROR_LIGHT_CONTROLLER;
                        AlarmReceiveVisionErrorLightTempOver = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_ERROR_LIGHT_TEMP_OVER;
                        AlarmModelListTimeout = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_N_MODEL_LIST_TIMEOUT;
                        AlarmModelNotFound = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_N_MODEL_NOT_FOUND;
                        AlarmAlignStartTimeout = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_START_TIMEOUT;
                        AlarmAlignCompleteTimeout = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_COMPLETE_TIMEOUT;
                        AlarmAlignCompleteValidationFail = CAlarmDefine.EAlarmList.CO_PRE_ALIGN_COMPLETE_VALIDATION_FAIL;
                        AlarmInterlockX = new CAlarmDefine.EAlarmList[]
                        {
                            CAlarmDefine.EAlarmList.VI_PRE_ALIGN_P1_OVERRANGE_X,
                            CAlarmDefine.EAlarmList.VI_PRE_ALIGN_P2_OVERRANGE_X
                        };
                        AlarmInterlockY = new CAlarmDefine.EAlarmList[]
                        {
                            CAlarmDefine.EAlarmList.VI_PRE_ALIGN_P1_OVERRANGE_Y,
                            CAlarmDefine.EAlarmList.VI_PRE_ALIGN_P2_OVERRANGE_Y
                        };
                        AlarmInterlockT = new CAlarmDefine.EAlarmList[]
                        {
                            CAlarmDefine.EAlarmList.VI_PRE_ALIGN_P1_OVERRANGE_T,
                            CAlarmDefine.EAlarmList.VI_PRE_ALIGN_P2_OVERRANGE_T
                        };
                        AlarmInterlockScore = new CAlarmDefine.EAlarmList[]
                        {
                            CAlarmDefine.EAlarmList.VI_PRE_ALIGN_P1_OVERRANGE_SCORE,
                            CAlarmDefine.EAlarmList.VI_PRE_ALIGN_P2_OVERRANGE_SCORE
                        };
                        AlarmRetryCountOver = CAlarmDefine.EAlarmList.VI_PRE_ALIGN_RETRY_COUNTOVER;
                        GetAlignResultData = new Func<CellDataHandler, OneCell.AlignResultData>((CellDataHandler cellDataHandler) =>
                        {
                            return cellDataHandler.Data.PreAlignResult;
                        });
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }

                IsPrimaryDevice = true;
            }
        }

        private Define mDefine;
    }
}

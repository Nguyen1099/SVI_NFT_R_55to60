using SVI_NFT_R;
using SVI_NFT_R.CellData;
using System;
using System.Collections.Generic;
using System.Threading;

public static partial class TrackInManager
{
    [Serializable]
    private sealed class TaskTrackInSet
    {
        private enum EStatus
        {
            CellInfoRequest = 0,
            WaitCellInfoDownload,
            CellLotInfoRequestForCheckStepId,
            WaitCellLotInfoDownloadForCheckStepId,
            TrackInEvent,
            WaitJobProcessRequest,
            Finish,

            // Abnormal case
            Finish_TrackInSkip,
            Finish_CellLotInfoDownloadFailed,
            Finish_CellInfoDownloadFailed,
        }

        public bool ShouldRemove { get; private set; } = false;
        public bool IsWaitCellInfoDownload => mCurrentStatus == EStatus.WaitCellInfoDownload;
        public bool IsWaitJobProcessRequest => mCurrentStatus == EStatus.WaitJobProcessRequest;
        public bool IsFinished => mCurrentStatus >= EStatus.Finish;
        public int PositionIndex { get; private set; }
        public string InnerID { get; private set; }
        public string CellID { get; private set; }
        public DateTime CreationTime { get; private set; }
        public event EventHandler DataChanged;
        private EStatus mCurrentStatus = EStatus.CellInfoRequest;
        private ETrackingResult mTrackingResult = ETrackingResult.None;
        private long mWaitStartTime = DateTime.MaxValue.Ticks;
        private long mTimeoutDelay = DateTime.MinValue.Ticks;
        private readonly CellInfomationEvent mCellInformationEvent = new CellInfomationEvent();
        private CellInfomationDownload mCellInformationDownload = null;
        private readonly CellTrackInEvent mCellTrackInEvent = new CellTrackInEvent();
        private CellJobProcessRequest mCellJobProcessRequest = null;
        private CellJobProcessReply mCellJobProcessReply = new CellJobProcessReply();
        private readonly CellLotInformationRequest mCellLotInformationRequestForCheckStepId = new CellLotInformationRequest();
        private CellLotInformationDownload mCellLotInformationDownloadForCheckStepId = null;

        public TaskTrackInSet(CellDataHandler cellData)
        {
            CreationTime = DateTime.Now;
            // "입력된 셀 데이터로부터 내부 데이터 초기화"
            InnerID = cellData.GetInnerID();
            CellID = cellData.GetCellID();
            PositionIndex = cellData.PositionIndex;
            // "CellInfomationEvent"
            mCellInformationEvent.BODY.CELLID = CellID;
            // "CellTrackInEvent"
            mCellTrackInEvent.BODY.EVENT = string.Format("{0}", (int)CCIMDefine.ECellEvent.CELL_EVENT_PROCESS_START);
            mCellTrackInEvent.BODY.UNITID = "0";
            mCellTrackInEvent.BODY.UNITST = "1";
            mCellTrackInEvent.BODY.EQST.REASONCODE = "1";
            mCellTrackInEvent.BODY.EQST.DESCRIPTION = "1";
            mCellTrackInEvent.BODY.CELL.CELLID = CellID;
            mCellTrackInEvent.BODY.WORKORDER.PROCESSJOB = cellData.Data.WorkOrder.ProcessJob;
            mCellTrackInEvent.BODY.WORKORDER.PLANQTY = cellData.Data.WorkOrder.PlanQty.ToString();
            mCellTrackInEvent.BODY.WORKORDER.PROCESSEDQTY = cellData.Data.WorkOrder.ProcessedQty.ToString();
            mCellTrackInEvent.BODY.READER.READERID = cellData.Data.Reader.ReaderID;
            mCellTrackInEvent.BODY.READER.READERRESULTCODE = cellData.Data.Reader.ReaderResultCode;
            mCellTrackInEvent.BODY.DV = new List<CellTrackInEvent._CellTrackInEvent.DVList>()
            {
                new CellTrackInEvent._CellTrackInEvent.DVList() { DVNAME = " ", DVVAL = " " }
            };
            // "CellLotInformationRequest for Check Step ID"
            mCellLotInformationRequestForCheckStepId.BODY.OPTIONCODE = "LOTINFO";
            mCellLotInformationRequestForCheckStepId.BODY.CELLIDS.Add(CellID);
        }

        public void DoProcess(CDocument document)
        {
            switch (mCurrentStatus)
            {
                case EStatus.CellInfoRequest:
                    DoProcessCellInfoRequest(document);
                    break;

                case EStatus.WaitCellInfoDownload:
                    DoProcessWaitCellInfoDownload(document);
                    break;

                case EStatus.CellLotInfoRequestForCheckStepId:
                    DoProcessCellLotInfoRequestForCheckStepId(document);
                    break;

                case EStatus.WaitCellLotInfoDownloadForCheckStepId:
                    DoProcessWaitCellLotInfoDownloadForCheckStepId(document);
                    break;

                case EStatus.TrackInEvent:
                    DoProcessTrackInEvent(document);
                    break;

                case EStatus.WaitJobProcessRequest:
                    DoProcessWaitJobProcessRequest(document);
                    break;

                default:
                    break;
            }
        }

        public void WaitForEndProcess(CellDataHandler cellData)
        {
            WaitForJobFinish();

            // Cell Data에 결과 반영
            cellData.Data.Cim.TrackingResult = mTrackingResult;
            if (mCurrentStatus == EStatus.Finish)
            {
                cellData.Data.Cim.CellInformationResult = mCellInformationDownload.BODY.CELLINFORESULT;
                cellData.Data.Cell.CarrierID = GetCarrierID();

                cellData.Data.Cim.CellLotInformationLotInfoResult = (mCellLotInformationDownloadForCheckStepId != null) ? "0" : "1";

                cellData.Data.Cim.ReplyCellID = (mCellJobProcessRequest == null) ? " " : mCellJobProcessRequest.BODY.CELLID;
                cellData.Data.Cell.ProductID = GetProductID();
                cellData.Data.Cell.StepID = GetStepID();
                cellData.Data.Cell.JobID = GetJobID();
            }

            ShouldRemove = true;
        }

        public void WaitForJobFinish()
        {
            int finishIndex = ((int)EStatus.Finish);

            // Job Finish 대기
            SpinWait.SpinUntil(() => ((int)mCurrentStatus) >= finishIndex);
        }

        private void DoProcessCellInfoRequest(CDocument m_objDocument)
        {
            if (
                m_objDocument.m_objConfig.GetOptionParameter().bUseCIM == false
                || m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_MCR_MODE] != CDialogEQPFunctionList.EFSTUse.USE.ToString()
                )
            {
                // "TrackIn 스킵"
                mCurrentStatus = EStatus.Finish_TrackInSkip;
                RasieDataChangedEvent();
                return;
            }

            if (m_objDocument.m_objConfig.GetOptionParameter().bUseCellInfomationResultData == false)
            {
                // "CellInfomationDownload 대기 안하고 성공 처리"
                mCellInformationDownload = new CellInfomationDownload();
                mCellInformationDownload.BODY.CELLINFORESULT = "0";
                mCurrentStatus = EStatus.CellLotInfoRequestForCheckStepId;
                RasieDataChangedEvent();
                return;
            }

            mCellInformationEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;

            mCurrentStatus = EStatus.WaitCellInfoDownload;
            mWaitStartTime = SVI_NFT_R.Utils.GetTimestamp();
            mTimeoutDelay = SVI_NFT_R.Config.WaitTime.CommTimeout.CimTrackingTimeout.ToTimeSpan().Ticks;
            RasieDataChangedEvent();

            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellInfomationEvent(mCellInformationEvent);
        }

        private void DoProcessWaitCellInfoDownload(CDocument m_objDocument)
        {
            CProcessCIMCellInformationDownload.BackupCellInfomationDownload getValue;
            if (m_objDocument.m_objProcessCIM.m_objProcessCIMCellInfomationDownload.MapCellInfomationDownload.TryGetValue(CellID, out getValue) == false)
            {
                if (GetElapsed() > mTimeoutDelay)
                {
                    m_objDocument.m_objProcessCIM.m_objProcessCIMCellInfomationDownload.MapCellInfomationDownload.Remove(CellID);
                    mTrackingResult = ETrackingResult.Skip;
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellInfomationDownload '{CellID}' TimeOut");
                    m_objDocument.SetForcedAlarm(AlarmDefine.CellInfoDownloadTimeoutAlarms[PositionIndex]);
                    mCurrentStatus = EStatus.Finish_CellInfoDownloadFailed;
                    RasieDataChangedEvent();
                }
                return;
            }
            m_objDocument.m_objProcessCIM.m_objProcessCIMCellInfomationDownload.MapCellInfomationDownload.Remove(CellID);
            mCellInformationDownload = getValue.Data;

            if (
                mCellInformationDownload.BODY.CELLINFORESULT != "0"
                && mCellInformationDownload.BODY.CELLINFORESULT != "42"
                )
            {
                mTrackingResult = ETrackingResult.Skip;
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellInfomationDownload '{CellID}' Fail");
                m_objDocument.SetForcedAlarm(AlarmDefine.CellInfoDownloadFailedAlarms[PositionIndex]);
                mCurrentStatus = EStatus.Finish_CellInfoDownloadFailed;
                RasieDataChangedEvent();
                return;
            }

            m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellInfomationDownload '{CellID}' OK = '{mCellInformationDownload.BODY.CELLINFORESULT}' ProductID = '{mCellInformationDownload.BODY.PRODUCTID}' CarrierID = '{mCellInformationDownload.BODY.CARRIERID}'");
            mCurrentStatus = EStatus.CellLotInfoRequestForCheckStepId;
            mWaitStartTime = DateTime.MaxValue.Ticks;
            RasieDataChangedEvent();
        }

        private void DoProcessCellLotInfoRequestForCheckStepId(CDocument m_objDocument)
        {
            if (m_objDocument.m_objConfig.GetOptionParameter().bUseCellLotInformation == false)
            {
                mCellLotInformationDownloadForCheckStepId = new CellLotInformationDownload();
                mCurrentStatus = EStatus.TrackInEvent;
                RasieDataChangedEvent();
                return;
            }

            mCellLotInformationRequestForCheckStepId.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;

            mCurrentStatus = EStatus.WaitCellLotInfoDownloadForCheckStepId;
            mWaitStartTime = SVI_NFT_R.Utils.GetTimestamp();
            mTimeoutDelay = SVI_NFT_R.Config.WaitTime.CommTimeout.CimTrackingTimeout.ToTimeSpan().Ticks;
            RasieDataChangedEvent();

            m_objDocument.m_objProcessCIM.m_objProcessCIMCellLotInfomationDownload.MapCellLotInformationDownloadLotInfo.Remove(CellID);
            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellLotInformationRequest(mCellLotInformationRequestForCheckStepId);

            return;
        }

        private void DoProcessWaitCellLotInfoDownloadForCheckStepId(CDocument m_objDocument)
        {
            CProcessCIMCellLotInformationDownload.BackupCellLotInformationDownload getValue;
            if (m_objDocument.m_objProcessCIM.m_objProcessCIMCellLotInfomationDownload.MapCellLotInformationDownloadLotInfo.TryGetValue(CellID, out getValue) == false)
            {
                if (GetElapsed() > mTimeoutDelay)
                {
                    m_objDocument.m_objProcessCIM.m_objProcessCIMCellLotInfomationDownload.MapCellLotInformationDownloadLotInfo.Remove(CellID);
                    mTrackingResult = ETrackingResult.Skip;
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellLotInfomationDownload [LOTINFO] '{CellID}' TRACKING_SKIP TimeOut");
                    m_objDocument.SetAlarmEvent(AlarmDefine.CellLotInformationTimeoutAlarms[PositionIndex]);
                    mCurrentStatus = EStatus.Finish_CellLotInfoDownloadFailed;
                    RasieDataChangedEvent();
                }
                return;
            }
            m_objDocument.m_objProcessCIM.m_objProcessCIMCellLotInfomationDownload.MapCellLotInformationDownloadLotInfo.Remove(CellID);
            mCellLotInformationDownloadForCheckStepId = getValue.Data;

            if (mCellLotInformationDownloadForCheckStepId.BODY.CELLLOTS.Count == 0)
            {
                mTrackingResult = ETrackingResult.Skip;
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellLotInfomationDownload [LOTINFO] '{CellID}' TRACKING_SKIP Receive Data Is Empty '{mCellLotInformationDownloadForCheckStepId.BODY.CELLLOTS.Count}'");
                mCurrentStatus = EStatus.Finish_CellLotInfoDownloadFailed;
                RasieDataChangedEvent();
                return;
            }
            if (mCellLotInformationDownloadForCheckStepId.BODY.CELLLOTS[0].STEPID != m_objDocument.m_objConfig.GetSystemParameter().strStepID)
            {
                mTrackingResult = ETrackingResult.Skip;
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellLotInfomationDownload [LOTINFO] '{CellID}' TRACKING_SKIP StepID Missmatch");
                m_objDocument.SetAlarmEvent(AlarmDefine.CellLotInformationFailedAlarms[PositionIndex]);
                mCurrentStatus = EStatus.Finish_CellLotInfoDownloadFailed;
                RasieDataChangedEvent();
                return;
            }
            if (mCellLotInformationDownloadForCheckStepId.BODY.CELLLOTS[0].COMMENT != "PASS")
            {
                mTrackingResult = ETrackingResult.Skip;
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellLotInfomationDownload [LOTINFO] '{CellID}' TRACKING_SKIP Comment Is Not Pass");
                m_objDocument.SetAlarmEvent(AlarmDefine.CellLotInformationFailedAlarms[PositionIndex]);
                mCurrentStatus = EStatus.Finish_CellLotInfoDownloadFailed;
                RasieDataChangedEvent();
                return;
            }

            m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellLotInfomationDownload [LOTINFO] '{CellID}' OK");
            mCurrentStatus = EStatus.TrackInEvent;
            mWaitStartTime = DateTime.MaxValue.Ticks;
            RasieDataChangedEvent();
        }

        private void DoProcessTrackInEvent(CDocument m_objDocument)
        {
            // "TrackIn 메시지 업데이트"
            mCellTrackInEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            mCellTrackInEvent.BODY.CELL.CARRIERID = GetCarrierID();
            mCellTrackInEvent.BODY.CELL.PPID = m_objDocument.m_objConfig.GetSystemParameter().strPPID;
            mCellTrackInEvent.BODY.CELL.PRODUCTID = GetProductID();
            mCellTrackInEvent.BODY.EQST.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
            mCellTrackInEvent.BODY.EQST.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
            if (m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_TRACKING_CONTROL] == CDialogEQPFunctionList.EFSTTrackingControl.NOTHING.ToString())
            {
                mCellTrackInEvent.BODY.READER.READERRESULTCODE = CCIMDefine.ReaderResultCode.VALID_FUNC_OFF;
            }
            else if (m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_MCR_MODE] == CDialogEQPFunctionList.EFSTUse.NOTHING.ToString())
            {
                mCellTrackInEvent.BODY.READER.READERRESULTCODE = CCIMDefine.ReaderResultCode.MCR_READ_OFF;
            }

            try
            {
                string trackingControl = m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_TRACKING_CONTROL];
                if (
                    trackingControl == CDialogEQPFunctionList.EFSTTrackingControl.TKOUT.ToString()
                    || trackingControl == CDialogEQPFunctionList.EFSTTrackingControl.NOTHING.ToString()
                    )
                {
                    mTrackingResult = ETrackingResult.OK;
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellJobProcess TrackingControl = 'TKOUT or NOTHING' '{CellID}' TRACKING_IN_OK");
                    mCurrentStatus = EStatus.Finish;
                    RasieDataChangedEvent();
                    return;
                }

                if (m_objDocument.m_objConfig.GetOptionParameter().bUseJobProcess == false)
                {
                    mTrackingResult = ETrackingResult.OK;
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellJobProcess NotUsed '{CellID}' TRACKING_IN_OK");
                    mCurrentStatus = EStatus.Finish;
                    RasieDataChangedEvent();
                    return;
                }

                mCurrentStatus = EStatus.WaitJobProcessRequest;
                mWaitStartTime = SVI_NFT_R.Utils.GetTimestamp();
                mTimeoutDelay = SVI_NFT_R.Config.WaitTime.CommTimeout.CimTrackingTimeout.ToTimeSpan().Ticks;
                RasieDataChangedEvent();
            }
            finally
            {
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellEvent(CCIMDefine.ECellEvent.CELL_EVENT_PROCESS_START, mCellTrackInEvent);
                m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryCellTrackIn(InnerID, CellID);
            }
        }

        private void DoProcessWaitJobProcessRequest(CDocument m_objDocument)
        {
            CProcessCIMCellJobProcessRequest.BackupCellJobProcessRequest getValue;
            if (m_objDocument.m_objProcessCIM.m_objProcessCIMCellJobProcessRequest.MapCellJobProcessRequest.TryGetValue(CellID, out getValue) == false)
            {
                if (GetElapsed() > mTimeoutDelay)
                {
                    m_objDocument.m_objProcessCIM.m_objProcessCIMCellJobProcessRequest.MapCellJobProcessRequest.Remove(CellID);
                    mTrackingResult = ETrackingResult.Timeout;
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellJobProcess '{CellID}' TRACKING_TIME_OUT");
                    m_objDocument.SetForcedAlarm(AlarmDefine.JobProcessRequestTimeoutAlarms[PositionIndex]);
                    mCurrentStatus = EStatus.Finish;
                    mWaitStartTime = DateTime.MaxValue.Ticks;
                    RasieDataChangedEvent();
                }
                return;
            }
            m_objDocument.m_objProcessCIM.m_objProcessCIMCellJobProcessRequest.MapCellJobProcessRequest.Remove(CellID);
            mCellJobProcessRequest = getValue.Data;
            mCellJobProcessReply.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            mCellJobProcessReply.TRANSACTIONNO = mCellJobProcessRequest.TRANSACTIONNO;

            if (mCellJobProcessRequest.BODY.RCMD != string.Format("{0}", (int)CCIMDefine.ECellJobProcessCmd.CELL_JOB_PROCESS_CMD_START))
            {
                mTrackingResult = ETrackingResult.Cancel;
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellJobProcess '{CellID}' TRACKING_FAILED RCMD = '{mCellJobProcessRequest.BODY.RCMD}'");
                m_objDocument.SetForcedAlarm(AlarmDefine.JobProcessRequestFailedAlarms[PositionIndex]);
                mCellJobProcessReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EControlStateSetReply.CONTROL_STATE_SET_REPLY_ACCEPTED);
            }
            else
            {
                mTrackingResult = ETrackingResult.OK;
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM_INFORMATION, $"CellJobProcess '{CellID}' TRACKING_IN_OK ProductID = '{mCellJobProcessRequest.BODY.PRODUCTID}' StepID = '{mCellJobProcessRequest.BODY.STEPID}' JobID = '{mCellJobProcessRequest.BODY.JOBID}'");
                mCellJobProcessReply.BODY.HCACK = string.Format("{0}", (int)CCIMDefine.EControlStateSetReply.CONTROL_STATE_SET_REPLY_ACCEPTED);
            }
            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellJobProcessReply(mCellJobProcessReply);

            mCurrentStatus = EStatus.Finish;
            mWaitStartTime = DateTime.MaxValue.Ticks;
            RasieDataChangedEvent();
        }

        private void RasieDataChangedEvent()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        private long GetElapsed()
        {
            return SVI_NFT_R.Utils.GetTimestamp() - mWaitStartTime;
        }

        private string GetCarrierID()
        {
            if (mCellInformationDownload != null
                && string.IsNullOrWhiteSpace(mCellInformationDownload.BODY.CARRIERID) == false
                )
            {
                return mCellInformationDownload.BODY.CARRIERID;
            }
            return " ";
        }

        private string GetProductID()
        {
            if (mCellJobProcessRequest != null
                && string.IsNullOrWhiteSpace(mCellJobProcessRequest.BODY.PRODUCTID) == false
                )
            {
                return mCellJobProcessRequest.BODY.PRODUCTID;
            }
            else if (mCellInformationDownload != null
                && string.IsNullOrWhiteSpace(mCellInformationDownload.BODY.PRODUCTID) == false
                )
            {
                return mCellInformationDownload.BODY.PRODUCTID;
            }
            return " ";
        }

        private string GetJobID()
        {
            if (mCellJobProcessRequest != null
                && string.IsNullOrWhiteSpace(mCellJobProcessRequest.BODY.JOBID) == false
                )
            {
                return mCellJobProcessRequest.BODY.JOBID;
            }
            return " ";
        }

        private string GetStepID()
        {
            if (mCellJobProcessRequest != null
                && string.IsNullOrWhiteSpace(mCellJobProcessRequest.BODY.STEPID) == false
                )
            {
                return mCellJobProcessRequest.BODY.STEPID;
            }
            return " ";
        }
    }
}
using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Svi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SVI_NFT_R
{
    public class TrackOut
    {
        public enum EUnloadResult
        {
            None = 0,
            OK,
            NG
        };

        public bool IsInitialized { get; private set; }
        public bool IsBusy { get { return 0 != mTrackOutQueue.Map.Count; } }
        private CDocument m_objDocument;
        private BackupFileMap<string, OneCell> mTrackOutQueue = new BackupFileMap<string, OneCell>();
        private readonly BackupFileMap<string, OneCell> mManualTrackOutQueue = new BackupFileMap<string, OneCell>();
        private Thread mThreadTrackOutProcess;
        private bool mbThreadExit;
        private const string BACKUP_FILE_NAME_TRACKOUT_QUEUE = @".\cache\TrackOutQueue.bak";
        private const string BACKUP_FILE_NAME_MANUAL_TRACKOUT_QUEUE = @".\cache\ManualTrackOutQueue.bak";

        public bool Initialize(CDocument objDocument)
        {
            bool bResult = false;

            do
            {
                if (true == IsInitialized)
                {
                    break;
                }

                m_objDocument = objDocument;

                mTrackOutQueue.Initialize(BACKUP_FILE_NAME_TRACKOUT_QUEUE);
                mManualTrackOutQueue.Initialize(BACKUP_FILE_NAME_MANUAL_TRACKOUT_QUEUE);

                mbThreadExit = false;
                mThreadTrackOutProcess = new Thread(ThreadTrackOutProcess);
                mThreadTrackOutProcess.Start();

                IsInitialized = true;
                bResult = true;
            } while (false);

            return bResult;
        }

        public void DeInitialize()
        {
            if (false == IsInitialized)
            {
                return;
            }

            mbThreadExit = true;
            mThreadTrackOutProcess.Join();

            mTrackOutQueue.DeInitialize();
            mManualTrackOutQueue.DeInitialize();

            IsInitialized = false;
        }

        /// <summary>
        /// 트랙아웃 할 셀 정보를 큐에 등록함
        /// </summary>
        /// <param name="cellInformation"></param>
        public void TrackOutRequestPushInQueue(OneCell cellInformation)
        {
            if (cellInformation.IsCellExist() == true)
            {
                mTrackOutQueue.Add(cellInformation.GetInspectionKey(), cellInformation);
            }
        }

        /// <summary>
        /// 해당 셀 정보에대한 매뉴얼 트랙아웃 요청을 등록한다
        /// </summary>
        /// <param name="cellData">셀 데이터</param>
        public void ManualTrackOutRequestPushInQueue(OneCell cellInformation)
        {
            if (cellInformation.IsCellExist() == true)
            {
                mManualTrackOutQueue.Add(cellInformation.GetInspectionKey(), cellInformation.DeepClone());
            }
        }

        /// <summary>
        /// 검사 결과 조합 ( Front or Rear )
        /// </summary>
        /// <param name="ePosition"></param>
        /// <returns></returns>
        public EUnloadResult CheckInspectResultCombination(OneCell cellData)
        {
            EUnloadResult eUnloadResult = EUnloadResult.None;

            if (cellData.IsUse == false)
            {
                return eUnloadResult;
            }

            if (
                cellData.Cell.FrontInspResult == EInspectionResult.BIN1
                && cellData.Cell.RearInspResult == EInspectionResult.BIN1
                )
            {
                eUnloadResult = EUnloadResult.OK;
                cellData.Cell.MachineResult = CDefine.DEFECT_MACHINE_RESULT_OK;
                cellData.Judgement.Judge = m_objDocument.m_objConfig.GetCrucialOptionParameter().strBinPrimeMode == "ON" ? CDefine.CIM_JUDGE_GOOD : CDefine.CIM_JUDGE_RETEST;
                cellData.Judgement.ReasonCode
                    = cellData.Judgement.Description
                    = CDefine.CIM_REASONCODE_GOOD;
            }
            else if (
                cellData.Cell.FrontInspResult == EInspectionResult.REJECT
                || cellData.Cell.RearInspResult == EInspectionResult.REJECT
                )
            {
                eUnloadResult = EUnloadResult.NG;
                cellData.Cell.MachineResult = CDefine.DEFECT_MACHINE_RESULT_NG;
                cellData.Judgement.Judge = m_objDocument.m_objConfig.GetCrucialOptionParameter().strBinPrimeMode == "ON" ? CDefine.CIM_JUDGE_LOTLOSS : CDefine.CIM_JUDGE_RETEST;
                cellData.Judgement.ReasonCode
                    = cellData.Judgement.Description
                    = GetTrackOutReasonCode(cellData);
            }
            else
            {
                eUnloadResult = EUnloadResult.NG;
                cellData.Cell.MachineResult = CDefine.DEFECT_MACHINE_RESULT_NG;
                cellData.Judgement.Judge = CDefine.CIM_JUDGE_RETEST;
                cellData.Judgement.ReasonCode
                    = cellData.Judgement.Description
                    = GetTrackOutReasonCode(cellData);
#if CIM_V3_GAMMA
                if (cellData.Cell.FrontInspResult == EInspectionResult.BIN2
                    || cellData.Cell.RearInspResult == EInspectionResult.BIN2
                    )
                {
                    cellData.Cim.DvList["PRE_JUDGE"] = "BIN2";
                }
#endif
            }

            // MCR 촬상에 실패 했으면 검사 결과에 상관없이 돌려서 나간다
            if (CCIMDefine.ReaderResultCode.OK != cellData.Reader.ReaderResultCode)
            {
                eUnloadResult = EUnloadResult.NG;
            }

            return eUnloadResult;
        }

        private static string GetTrackOutReasonCode(OneCell cellData)
        {
            if (
                cellData.Cell.FrontInspResult == EInspectionResult.BIN1
                || cellData.Cell.RearInspResult == EInspectionResult.REJECT
                )
            {
                return cellData.Cell.RearInspReasonCodes.FirstOrDefault(x => string.IsNullOrWhiteSpace(x) == false) ?? " ";
            }

            return cellData.Cell.FrontInspReasonCodes.Concat(cellData.Cell.RearInspReasonCodes).FirstOrDefault(x => string.IsNullOrWhiteSpace(x) == false) ?? " ";
        }

        /// <summary>
        /// 등록된 셀에 트랙아웃이 완료될때까지 대기한다
        /// </summary>
        /// <param name="timeout">대기 타임 아웃</param>
        /// <returns><para>true:정상</para><para>false:타임아웃</para></returns>
        public bool WaitForEndProcess(int timeout) => SpinWait.SpinUntil(() =>
        {
            Thread.Sleep(10);
            return IsBusy == false;
        }, timeout);

        /// <summary>
        /// 셀 정보를 CIM에 트랙아웃 이벤트로 보고합니다.
        /// </summary>
        /// <param name="eProcess"></param>
        /// <param name="ePos"></param>
        private void DoProcessSendTrackOut(OneCell cellData)
        {
            if (cellData.IsUse == false)
            {
                return;
            }

            string strMCRUseMode = m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_MCR_MODE];
            if (CDialogEQPFunctionList.EFSTUse.USE.ToString() == strMCRUseMode)
            {
                if (
                    ETrackingResult.Skip != cellData.Cim.TrackingResult
                    && ETrackingResult.None != cellData.Cim.TrackingResult
                    )
                {
                    if (ETrackingResult.Cancel == cellData.Cim.TrackingResult)
                    {
                        // TrackOut 보고 (406)
                        SendTrackOutData(cellData, ETrackingResult.Cancel);
                    }
                    else if (ETrackingResult.Timeout == cellData.Cim.TrackingResult)
                    {
                        // TrackOut 보고 (406)
                        SendTrackOutData(cellData, ETrackingResult.Timeout);
                    }
                    else if (CCIMDefine.ReaderResultCode.OK == cellData.Reader.ReaderResultCode)
                    {
                        // 검사결과보고 (609)
                        SendInspectionResultData(cellData);
                        Thread.Sleep(10);
                        // Recipe 보고 (405)
                        SendRecipeValidation(cellData);
                        Thread.Sleep(10);
                        // PerformanceLoos 보고
                        SendPerformanceLoss(cellData);
                        Thread.Sleep(10);
                        // TrackOut 보고 (406)
                        SendTrackOutData(cellData, ETrackingResult.OK);
                        Thread.Sleep(10);
                        // TrackOut 파일 생성
                        WriteTrackOutDataFile(cellData);
                    }
                }
            }
        }

        /// <summary>
        /// 셀 정보를 CIM에 매뉴얼 트랙아웃 이벤트로 보고합니다.
        /// </summary>
        /// <param name="eProcess"></param>
        /// <param name="ePos"></param>
        private void DoProcessSendManualTrackOut(OneCell cellData)
        {
            if (cellData.IsUse == false)
            {
                return;
            }

            string strMCRUseMode = m_objDocument.m_objConfig.GetCimParameter().strEFValue[(int)CDialogEQPFunctionList.EFName.EF_NAME_CELL_MCR_MODE];
            if (CDialogEQPFunctionList.EFSTUse.USE.ToString() == strMCRUseMode)
            {
                if (
                    ETrackingResult.Skip != cellData.Cim.TrackingResult
                    && ETrackingResult.None != cellData.Cim.TrackingResult
                    )
                {
                    if (CCIMDefine.ReaderResultCode.OK == cellData.Reader.ReaderResultCode)
                    {
                        // TrackOut 보고 (406)
                        SendTrackOutData(cellData, ETrackingResult.OK);
                    }
                }
            }
        }

        /// <summary>
        /// CellData에 DV리스트를 채워넣는다.
        /// </summary>
        /// <param name="cellData"></param>
        private void SetDvListUpdate(OneCell cellData)
        {
            if (cellData.IsUse == false)
            {
                return;
            }

            cellData.Cim.DvList["TEST_A_JUDGE"] = " ";
            cellData.Cim.DvList["TEST_A_REASONCODE"] = " ";
            cellData.Cim.DvList["TEST_A_DESCRIPTION"] = " ";
            cellData.Cim.DvList["FINAL_TESTER"] = cellData.Cell.ChannelName;
        }

        /// <summary>
        /// 트렉인 데이터
        /// </summary>
        /// <param name="cellData"></param>
        /// <param name="eTrackingResult"></param>
        private void SendTrackOutData(OneCell cellData, ETrackingResult eTrackingResult)
        {
            if (
                false == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM
                // #CIM_TEST
                //|| CDefine.enumRunModeType.RUN_MODE_TYPE_REAL_RUN != m_objDocument.GetRunModeType()
                )
            {
                return;
            }

            // 셀 트렉인 데이터
            var objData = new CellTrackOutEvent();
            objData.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            objData.BODY.EVENT = string.Format("{0}", (int)CCIMDefine.ECellEvent.CELL_EVENT_PROCESS_END);
            objData.BODY.UNITID = "0";
            objData.BODY.UNITST = "1"; // 0:IDLE 1:RUN 2:DOWN
            objData.BODY.EQST.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
            objData.BODY.EQST.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
            objData.BODY.EQST.REASONCODE = " ";     // 나중에 변경 될 수 있음.
            objData.BODY.EQST.DESCRIPTION = " ";    // 나중에 변경 될 수 있음.

            objData.BODY.CELL.CARRIERID = cellData.Cell.CarrierID;
            objData.BODY.CELL.CELLID = cellData.Cell.CellID;
            objData.BODY.CELL.PPID = m_objDocument.m_objConfig.GetSystemParameter().strPPID;
            objData.BODY.CELL.PRODUCTID = cellData.Cell.ProductID;

            objData.BODY.CELL.STEPID = cellData.Cell.StepID;
            // 신규 추가 - " "으로 처리.
            objData.BODY.CELL.PRODUCTTYPE = " ";
            objData.BODY.CELL.PRODUCTKIND = " ";
            objData.BODY.CELL.PRODUCTSPEC = " ";
            objData.BODY.CELL.CELLSIZE = " ";
            objData.BODY.CELL.CELLTHICKNESS = " ";
            objData.BODY.CELL.COMMENT = " ";
            objData.BODY.CELL.CELLINFORESULT = " ";
            objData.BODY.CELL.REPLYSTATUS = " ";

            objData.BODY.WORKORDER.PROCESSJOB = string.Format("{0}", (int)EProcess.OutRobot);
            objData.BODY.WORKORDER.PLANQTY = string.Format("{0}", cellData.WorkOrder.PlanQty);
            objData.BODY.WORKORDER.PROCESSEDQTY = string.Format("{0}", cellData.WorkOrder.ProcessedQty);

            objData.BODY.READER.READERID = " ";
            objData.BODY.READER.READERRESULTCODE = " ";

            objData.BODY.JUDGEMENT.OPERATORID = " ";
            if (ETrackingResult.OK != eTrackingResult)
            {
                objData.BODY.JUDGEMENT.JUDGE = CDefine.CIM_JUDGE_OUT;
                objData.BODY.JUDGEMENT.REASONCODE = string.IsNullOrEmpty(cellData.Judgement.ReasonCode) ? " " : cellData.Judgement.ReasonCode;
#if !CIM_V3_GAMMA
                // V1, V3-5F
                if (ETrackingResult.Timeout == eTrackingResult)
                {
                    objData.BODY.JUDGEMENT.DESCRIPTION = "CELL_VALIDATION_TIMEOUT";
                }
                else if (ETrackingResult.Skip == eTrackingResult)
                {
                    objData.BODY.JUDGEMENT.DESCRIPTION = "CELL_VALIDATION_FAIL";
                }
                else if (ETrackingResult.Cancel == eTrackingResult)
                {
                    objData.BODY.JUDGEMENT.DESCRIPTION = "VALIDATION_NG";
                    objData.BODY.JUDGEMENT.REASONCODE = "USF56";
                }
#else
                // V3 (G Project)
                objData.BODY.JUDGEMENT.DESCRIPTION = "VALIDATION_NG";
                objData.BODY.JUDGEMENT.REASONCODE = "GSCNWIP";
#endif
            }
            else
            {
                objData.BODY.JUDGEMENT.JUDGE = cellData.Judgement.Judge;
                objData.BODY.JUDGEMENT.REASONCODE = cellData.Judgement.ReasonCode;
                objData.BODY.JUDGEMENT.DESCRIPTION = cellData.Judgement.Description;
            }

            //////////////////////////////////////////////////////////////////////////
            // DV
            cellData.Cim.DvList["TEST_A_JUDGE"] = objData.BODY.JUDGEMENT.JUDGE;
            cellData.Cim.DvList["TEST_A_REASONCODE"] = objData.BODY.JUDGEMENT.REASONCODE;
            cellData.Cim.DvList["TEST_A_DESCRIPTION"] = objData.BODY.JUDGEMENT.DESCRIPTION;
            objData.BODY.DV = new List<CellTrackOutEvent._CellTrackOutEvent.DVList>();
            var processMotionSviInspectionInterface = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage.Inspects
                .First(x => x.IsPrimary());
            var data = processMotionSviInspectionInterface.MapRecipeValidationData.Map;
            foreach (var item in data)
            {
                cellData.Cim.DvList[item.Key] = item.Value;
            }
            string key = cellData.GetInspectionKey();
            ReceiveData.LightDataForDvReport visionLightValue;
            if (false == processMotionSviInspectionInterface.MapLightDataForDvReport.TryGetValue(key, out visionLightValue))
            {
                visionLightValue = new ReceiveData.LightDataForDvReport();
            }
            foreach (var item in visionLightValue.Data)
            {
                cellData.Cim.DvList[item.Key] = item.Value.ToString();
            }
            processMotionSviInspectionInterface.MapLightDataForDvReport.Remove(key);
            foreach (var item in cellData.Cim.DvList)
            {
                var objDVList = new CellTrackOutEvent._CellTrackOutEvent.DVList()
                {
                    DVNAME = item.Key,
                    DVVAL = item.Value
                };
                objData.BODY.DV.Add(objDVList);
            }
            //////////////////////////////////////////////////////////////////////////

            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellEvent(CCIMDefine.ECellEvent.CELL_EVENT_PROCESS_END, objData);
            m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryCellTrackOut(cellData.Cell.InnerID, cellData.Cell.CellID);
        }

        /// <summary>
        /// 검사 결과 데이터
        /// </summary>
        /// <param name="ePosition"></param>
        private void SendInspectionResultData(OneCell cellData)
        {
            if (false == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM)
            {
                return;
            }

            // MCR 리딩 실패 또는 RCMD 22가 아닐 경우 보고한다.
            if (
                CCIMDefine.ReaderResultCode.OK == cellData.Reader.ReaderResultCode
                && ETrackingResult.OK == cellData.Cim.TrackingResult
                )
            {
                var objData = new InspectionResultEvent();
                objData.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;

                objData.BODY.CARRIERID = " ";
                objData.BODY.CELLID = cellData.Cell.CellID;
                objData.BODY.PROCESSNAME = "SVI";
                objData.BODY.PROCESSFLAG = "Y";
                objData.BODY.OPERID = m_objDocument.GetUserInformation().m_strID;
                objData.BODY.SENDUNIQUEINFO = " ";
                objData.BODY.REVUNIQUEINFO = " ";
                switch (cellData.Cell.RearInspResult)
                {
                    case EInspectionResult.BIN1:
                        objData.BODY.JUDGE = CDefine.CIM_JUDGE_GOOD;
                        objData.BODY.REASONCODE = " ";
                        break;
                    case EInspectionResult.REJECT:
                        objData.BODY.JUDGE = CDefine.CIM_JUDGE_LOTLOSS;
#if !CIM_V3_GAMMA
                        objData.BODY.REASONCODE = cellData.Cell.RearInspReasonCodes.FirstOrDefault(x => string.IsNullOrWhiteSpace(x) == false) ?? " ";
#else
                        objData.BODY.REASONCODE = " ";
#endif
                        break;
                    case EInspectionResult.BIN2:
                    case EInspectionResult.BIN3:
                    case EInspectionResult.None:
                    default:
                        objData.BODY.JUDGE = CDefine.CIM_JUDGE_RETEST;
#if !CIM_V3_GAMMA
                        objData.BODY.REASONCODE = cellData.Cell.RearInspReasonCodes.FirstOrDefault(x => string.IsNullOrWhiteSpace(x) == false) ?? " ";
#else
                        objData.BODY.REASONCODE = CDefine.CIM_REASONCODE_NG;
#endif
                        break;
                }

                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetInspectionResultEvent(objData);
            }
        }

        private void SendRecipeValidation(OneCell cellData)
        {
            if (
                false == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM
                || false == m_objDocument.m_objConfig.GetOptionParameter().bUseRecipeValidation
                // #CIM_TEST
                //|| CDefine.enumRunModeType.RUN_MODE_TYPE_REAL_RUN != m_objDocument.GetRunModeType()
                )
            {
                return;
            }

            // MCR 리딩 실패 또는 RCMD 22가 아닐 경우 보고한다.
            if (
                CCIMDefine.ReaderResultCode.OK == cellData.Reader.ReaderResultCode
                && ETrackingResult.OK == cellData.Cim.TrackingResult
                )
            {
                var objData = new EQSpecificStepEvent();
                objData.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                objData.BODY.CELLID = cellData.Cell.CellID;
                objData.BODY.PPID = m_objDocument.m_objConfig.GetSystemParameter().strPPID;
                objData.BODY.PRODUCTID = cellData.Cell.ProductID;
                objData.BODY.STEPID = cellData.Cell.StepID;
                objData.BODY.OPTIONINFO = "JOBFILE";
                objData.BODY.DESCRIPTION = " ";
                objData.BODY.ITEMS = new List<EQSpecificStepEvent._EQSpecificStepEvent.DVList>();
                var processMotionSviInspectionInterface = m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage.Inspects
                    .First(x => x.IsPrimary());
                var data = processMotionSviInspectionInterface.MapRecipeValidationData.Map;
                foreach (var item in data)
                {
                    var objDVList = new EQSpecificStepEvent._EQSpecificStepEvent.DVList()
                    {
                        ITEM_NAME = item.Key,
                        ITEM_VALUE = item.Value
                    };
                    objData.BODY.ITEMS.Add(objDVList);
                }

                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetEquipmentSpecificStepEvent(objData);
            }
        }

        private void SendPerformanceLoss(OneCell cellData)
        {
            var objData = new PerformanceLossEvent();
            objData.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            objData.BODY.CELLINFOSET.CELLID = cellData.Cell.CellID;
            objData.BODY.LOSSOBJECTS = new PerformanceLossEvent._PerformanceLossEvent.LOSSOBJECT[1];
            objData.BODY.LOSSOBJECTS[0].H_TACT_TIME = "14";
            objData.BODY.LOSSOBJECTS[0].E_TACT_TIME = "14.5";
            objData.BODY.LOSSOBJECTS[0].S_TIME = DateTime.Now.ToString("yyyyMMddHHmmsss.fff");
            Thread.Sleep(200);
            objData.BODY.LOSSOBJECTS[0].E_TIME = DateTime.Now.ToString("yyyyMMddHHmmsss.fff");
            objData.BODY.LOSSOBJECTS[0].L_CODE = "testCode";
            objData.BODY.LOSSOBJECTS[0].L_TEXT = "TestText";
            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPerformanceLossEvent(objData);
        }

        private void WriteTrackOutDataFile(OneCell cellData)
        {
            if (
                false == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM
                // #CIM_TEST
                //|| CDefine.enumRunModeType.RUN_MODE_TYPE_REAL_RUN != m_objDocument.GetRunModeType()
                )
            {
                return;
            }

            if (true == string.IsNullOrWhiteSpace(m_objDocument.m_objConfig.GetTrackOutPath()))
            {
                return;
            }

            //D:\TRACKOUT INFO\TRACKOUT_CELLID_INNERID.txt
            var strPath = string.Format(@"{0}\TRACKOUT_{1}_{2}.txt", m_objDocument.m_objConfig.GetTrackOutPath(), cellData.Cell.CellID, cellData.Cell.InnerID);
            ClassINI objINI = new ClassINI(strPath);
            objINI.WriteValue("DATA", "EQP_ID", m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID);
            objINI.WriteValue("DATA", "CELL_ID", cellData.Cell.CellID);
            objINI.WriteValue("DATA", "INNER_ID", cellData.Cell.InnerID);
            objINI.WriteValue("DATA", "RESULT", cellData.Judgement.Judge);
        }

        private void ThreadTrackOutProcess()
        {
            while (
                null == m_objDocument.m_objProcessCIM
                || false == m_objDocument.m_objProcessCIM.IsInitialized
                || null == m_objDocument.m_objProcessDatabase
                || false == m_objDocument.m_bIsDatabaseInitialized
                )
            {
                Thread.Sleep(100);
            }

            while (false == mbThreadExit)
            {
                Thread.Sleep(10);

                // 트랙 아웃 진행
                if (0 < mTrackOutQueue.Map.Count)
                {
                    KeyValuePair<string, OneCell> firstItem;
                    lock (mTrackOutQueue.MapLock)
                    {
                        firstItem = mTrackOutQueue.Map.First();
                    }

                    // 검사 결과 대기
                    var cellInformation = firstItem.Value;

                    // DV 리스트 업데이트
                    SetDvListUpdate(cellInformation);

                    // 트랙 아웃 진행
                    DoProcessSendTrackOut(cellInformation);

                    // 데이터 베이스 업데이트
                    m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryMachineResult(cellInformation.GetInnerID(), cellInformation.Cell.MachineResult);
                    m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetUpdateHistoryProcessData(cellInformation);

                    // 데이터 삭제
                    mTrackOutQueue.Remove(firstItem.Key);
                }

                if (0 < mManualTrackOutQueue.Map.Count)
                {
                    KeyValuePair<string, OneCell> firstItem;
                    lock (mManualTrackOutQueue.MapLock)
                    {
                        firstItem = mManualTrackOutQueue.Map.First();
                    }
                    OneCell cellInformation = firstItem.Value;

                    // DV 리스트 업데이트
                    SetDvListUpdate(cellInformation);

                    // 매뉴얼 트랙아웃 진행
                    DoProcessSendManualTrackOut(cellInformation);

                    // 데이터 베이스 업데이트
                    m_objDocument.m_objProcessDatabase.m_objDatabaseSendMessage.SetUpdateHistoryProcessData(cellInformation);

                    // 데이터 삭제
                    mManualTrackOutQueue.Remove(firstItem.Key);
                }
            }
        }
    }
}
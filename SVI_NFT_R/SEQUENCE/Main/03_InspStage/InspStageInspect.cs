using HLDevice;
using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Svi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R
{
    public partial class InspStageInspect : CProcessAbstract
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            Check,
            GrabP1,
            GrabP2,
            SendVacuumOn,
            SendVacuumOff,
            LightIntensityRequest,
            LightTemperatureRequest,
            CellDataSync
        }

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            Check,
            GrabP1,
            GrabP2,
            SendVacuumOn,
            SendVacuumOff,
            LightIntensityRequest,
            LightTemperatureRequest,
            CellDataSync
        }

        public CDeviceSviInterface InspInterface { get; private set; }
        public IBackupFileMapUserInterface<string, string> MapRecipeValidationData { get { return mMapRecipeValidationData; } }
        public IBackupFileMapUserInterface<string, ReceiveData.LightDataForDvReport> MapLightDataForDvReport { get { return mMapLightDataForDvReport; } }
        public ReceiveData.LightDataForDvReport LastLightDataForFdc { get; private set; }
        public int[] LastLightIntensityData { get { return (int[])mLastLightIntensityData.Clone(); } }
        public int[] LastLightTemperatureData { get { return (int[])mLastLightTemperatureData.Clone(); } }
        public ReceiveData.AlignData LastAlignData { get { return mLastAlignData; } }
        public EPower LastLaserStatus { get { return mLastLaserStatus; } }
        public DEVICE.Svi.EStatus LastStatus { get { return mLastStatus; } }
        public event EventHandler<ReceiveData.TotalResultDataForRear> OnReceiveTotalResult;
        private const string ID = nameof(InspStageInspect);
        private ECommand mCommand;
        private EStatus mStatus;
        private int mConsecutiveResultWaitTimeoutCount;
        //////////////////////////////////////////////////////////////////////////
        // 수신 데이터 저장
        private SendData.GrabStartData mGrabStartRequest = null;
        private ReceiveData.GrabStartData mGrabStartAcknowlege = null;
        private ReceiveData.GrabEndData mGrabEndAcknowlege = null;
        private ReceiveData.AlignData mLastAlignData = new ReceiveData.AlignData();
        private int[] mLastLightIntensityData = new int[0];
        private int[] mLastLightTemperatureData = new int[0];
        private DEVICE.Svi.EStatus mLastStatus;
        private EPower mLastLaserStatus;
        private string mLastStatusErrorMessage;
        private SendData.CellDataSyncData mCellDataSyncRequest = null;
        private ReceiveData.CellDataSyncData mCellDataSyncAcknowlege = null;
        //////////////////////////////////////////////////////////////////////////
        // 데이터 백업 및 복구
        private BackupFileMap<string, ReceiveData.InspectResultData> mMapInspectResultData = new BackupFileMap<string, ReceiveData.InspectResultData>();
        private BackupFileMap<string, ReceiveData.TotalResultDataForRear> mMapTotalResultData = new BackupFileMap<string, ReceiveData.TotalResultDataForRear>();
        private BackupFileMap<string, string> mMapRecipeValidationData = new BackupFileMap<string, string>();
        private BackupFileMap<string, ReceiveData.LightDataForDvReport> mMapLightDataForDvReport = new BackupFileMap<string, ReceiveData.LightDataForDvReport>();
        private string BACKUP_FILE_NAME_INSPECT_RESULT => $@".\cache\{ID}_InspectResult({mDefine.InspIndex}).bak";
        private string BACKUP_FILE_NAME_TOTAL_RESULT => $@".\cache\{ID}_TotalResult({mDefine.InspIndex}).bak";
        private string BACKUP_FILE_NAME_RECIPE_VALIDATION => $@".\cache\{ID}_RecipeValidation({mDefine.InspIndex}).bak";
        private string BACKUP_FILE_NAME_DV_REPORT_LIGHT_DATA => $@".\cache\{ID}_LightDataForDvReport({mDefine.InspIndex}).bak";
        private readonly Stopwatch mEquipmentStopStopwatch = Stopwatch.StartNew();
        public override string ToString() => $"{ID},{mDefine.InspIndex},{mCommand},{mStatus}";

        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmObject = ID;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                m_objDocument = document;
                m_objConfig = m_objDocument.m_objConfig;
                mDefine = new Define(position);

                // 검사 인터페이스 객체 연결
                InspInterface = m_objDocument.m_objProcessMain.m_objInspInterfaces[mDefine.InspIndex];
                bool mbConnectedBeforeRegistEventHandler = InspInterface.HLIsConnected();
                // 수신 데이터 콜백 함수 등록
                DEVICE.Svi.ExtentionMethods.ParsingError += InspInterface_OnParsingError;
                InspInterface.AddCallbackFunction(InspInterface_OnReceiveData);
                InspInterface.OnConnected += InspInterface_OnConnected;
                InspInterface.OnDisconnected += InspInterface_OnDisconnected;
                // 쓰레드 생성 및 시작
                mThreadProcess = new Thread(ThreadProcess);
                mThreadProcess.Start();

                mMapInspectResultData.AfterAdd += InspectResultDataMap_OnAfterAdd;
                mMapTotalResultData.AfterAdd += TotalResultData_OnAfterAdd;
                mMapLightDataForDvReport.AfterAdd += LightDataForDvReport_OnAfterAdd;
                mMapRecipeValidationData.CreateFileDefaultOrNull = new ConcurrentDictionary<string, string>()
                {
                    ["DECISIONLOGIC"] = "868359BD5963E9B6209A1C1073CBB28C",
                    ["DL"] = "A0CC9840B19DC6AC4D5391EC7E4CC2B0",
                    ["RCP"] = "810B03CFC361B275AA16B18312286D4D",
                    ["RCP_NAME"] = "MF-DP116-190815_DL"
                };

                mMapInspectResultData.Initialize(BACKUP_FILE_NAME_INSPECT_RESULT);
                mMapTotalResultData.Initialize(BACKUP_FILE_NAME_TOTAL_RESULT);
                mMapLightDataForDvReport.Initialize(BACKUP_FILE_NAME_DV_REPORT_LIGHT_DATA);
                mMapRecipeValidationData.Initialize(BACKUP_FILE_NAME_RECIPE_VALIDATION);

                mMapInspectResultData.ForEach((key, value) => value.ResetRegistTime());
                mMapTotalResultData.ForEach((key, value) => value.ResetRegistTime());
                mMapLightDataForDvReport.ForEach((key, value) => value.ResetRegistTime());

                mConsecutiveResultWaitTimeoutCount = 0;

                if (mMapLightDataForDvReport.Map.Count == 0)
                {
                    LastLightDataForFdc = new ReceiveData.LightDataForDvReport();
                }
                else
                {
                    LastLightDataForFdc = mMapLightDataForDvReport.Map.Values.Last();
                }

                // 이벤트 핸들러를 등록 하기 전에 통신이 연결 돼 있었으면 동기화 진행
                if (mbConnectedBeforeRegistEventHandler == true)
                {
                    // 검사기와 상태를 동기화 한다
                    InspInterface_OnConnected(this, EventArgs.Empty);
                }

                m_objDocument.m_objProcessMain.m_objProcessMotion.AllMotions.Add(this);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override void DeInitialize()
        {
            mbThreadExit = true;

            mThreadProcess.Join();

            // 수신 데이터 콜백 함수 등록 해제
            InspInterface.OnConnected -= InspInterface_OnConnected;
            InspInterface.OnDisconnected -= InspInterface_OnDisconnected;
            InspInterface.RemoveCallbackFunction(InspInterface_OnReceiveData);
            DEVICE.Svi.ExtentionMethods.ParsingError -= InspInterface_OnParsingError;

            mMapInspectResultData.DeInitialize();
            mMapTotalResultData.DeInitialize();
            mMapRecipeValidationData.DeInitialize();
            mMapLightDataForDvReport.DeInitialize();

            mMapLightDataForDvReport.AfterAdd -= LightDataForDvReport_OnAfterAdd;
            mMapTotalResultData.AfterAdd -= TotalResultData_OnAfterAdd;
            mMapInspectResultData.AfterAdd -= InspectResultDataMap_OnAfterAdd;
        }

        public bool SetCommand(ECommand eCommand)
        {
            mCommand = eCommand;
            return true;
        }

        public ECommand GetCommand()
        {
            return mCommand;
        }

        public EStatus GetStatus()
        {
            return mStatus;
        }

        public void SetGrabInformation(IEnumerable<CellDataHandler> cellContainer)
        {
            CellInformation[] cellInformations = new CellInformation[]
            {
                new CellInformation(),
                new CellInformation()
            };

            foreach (var cellDataHandler in cellContainer.GetExistCellList())
            {
                int index = cellDataHandler.PositionIndex;
                //cellInformations[index].InnerID = cellInformations[index].CellID = cellDataHandler.GetInspectionKey();
                cellInformations[index].InnerID = cellDataHandler.GetInnerID();
                cellInformations[index].CellID = cellDataHandler.GetInspectionKey(bUseJobID: false);
                SetProcessLog(string.Format("CheckCellData -> Position[{0}] Ready -> Inner:{1}, Mcr:{2}, InspKey:{3})", index, cellInformations[index].InnerID, cellInformations[index].CellID, cellDataHandler.GetInspectionKey()));
            }

            mGrabStartRequest = new SendData.GrabStartData()
            {
                PositionA = cellInformations[0],
                PositionB = cellInformations[1]
            };
        }

        public void ResetGrabInformation()
        {
            mGrabStartRequest = null;
            mGrabStartAcknowlege = null;
            mGrabEndAcknowlege = null;
        }

        /// <summary>
        /// [매뉴얼용] 매뉴얼 촬상을 위해 임시 CellID를 부여한다
        /// </summary>
        /// <param name="cellPlace">
        /// 셀 위치를 지정함 (예: P1만 촬상 cellPlace = ECellPlacement.P1, 동시 촬상 cellPlace = ECellPlacement.P1 | ECellPlacement.P2)
        /// </param>
        public void SetGrabInformationTemporaryId(ECellPlacement cellPlace, string postfix = "TEST")
        {
            CellInformation[] cellInformations = new CellInformation[]
            {
                new CellInformation(),
                new CellInformation()
            };
            if (cellPlace.HasFlag(ECellPlacement.P1) == true)
            {
                cellInformations[0].InnerID = cellInformations[0].CellID = $"P1_{DateTime.Now:yyyyMMddHHmmssfff}_{postfix}";
            }
            if (cellPlace.HasFlag(ECellPlacement.P2) == true)
            {
                cellInformations[1].InnerID = cellInformations[1].CellID = $"P2_{DateTime.Now:yyyyMMddHHmmssfff}_{postfix}";
            }
            mGrabStartRequest = new SendData.GrabStartData()
            {
                PositionA = cellInformations[0],
                PositionB = cellInformations[1]
            };
        }

        public void SetCellDataSyncInformation(CellDataHandler cellDataHandler)
        {
            mCellDataSyncRequest = new SendData.CellDataSyncData()
            {
                CellID = cellDataHandler.GetInnerID(),
                InnerID = cellDataHandler.GetInnerID(),
                JobID = cellDataHandler.GetInnerID()
            };
            if (cellDataHandler.Data.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK
                && string.IsNullOrWhiteSpace(cellDataHandler.Data.Cell.JobID) == false
                )
            {
                mCellDataSyncRequest.JobID = cellDataHandler.Data.Cell.JobID;
            }
            if (cellDataHandler.Data.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK
                && string.IsNullOrWhiteSpace(cellDataHandler.Data.Cell.CellID) == false
                )
            {
                mCellDataSyncRequest.CellID = cellDataHandler.Data.Cell.CellID;
            }
        }

        public void ResetCellDataSyncInformation()
        {
            mCellDataSyncRequest = null;
            mCellDataSyncAcknowlege = null;
        }

        public override bool WaitForEndProcess()
        {
            bool bResult = false;

            do
            {
                while (GetCommand() != ECommand.Idle
                    || m_objDocument.GetRunStatus() == CDefine.ERunStatus.Pause
                    )
                {
                    Thread.Sleep(WAIT_FOR_END_PROCESS_PERIOD_TIME);
                }
                if (GetStatus() == EStatus.Stop)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        public bool SetWaitInspectResult(ref OneCell cellInformation, bool bMachineStoppingCheck = true)
        {
            bool bReturn = false;
            SetProcessLog("[START]");
            //ReceiveData.InspectResultData inspectResult = null;
            // TOTAL RESULT USED
            ReceiveData.TotalResultDataForRear inspectResult = null;
            string key = cellInformation.GetInspectionKey(bUseJobID: false);
            try
            {
                if (
                    cellInformation.Cell.FrontInspResult != EInspectionResult.None
                    && cellInformation.Cell.RearInspResult != EInspectionResult.None
                    )
                {
                    bReturn = true;
                    return true;
                }
#if !SCT_AMO
                if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() != CDefine.ERunMode.RealRun
                    )
                {
                    SetProcessLog($"WaitInspectionResult({key}) -> Dry Run");
                    inspectResult = new ReceiveData.TotalResultDataForRear()
                    {
                        CellID = key
                    };
                    inspectResult.FrontResult.DefectNames[0] = "DRY_RUN";
                    inspectResult.RearResult.DefectNames[0] = "DRY_RUN";
                    bReturn = true;
                    return true;
                }
#endif
                if (m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE)
                {
                    SetProcessLog($"WaitInspectionResult({key}) -> Not Use");
                    inspectResult = new ReceiveData.TotalResultDataForRear()
                    {
                        CellID = key
                    };
                    inspectResult.FrontResult.DefectNames[0] = "VISION_NOT_USE_MODE";
                    inspectResult.RearResult.DefectNames[0] = "VISION_NOT_USE_MODE";
                    bReturn = true;
                    return true;
                }
                if (m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_BY_PASS)
                {
                    SetProcessLog($"WaitInspectionResult({key}) -> Bypass");
                    inspectResult = new ReceiveData.TotalResultDataForRear()
                    {
                        CellID = key
                    };
                    inspectResult.FrontResult.DefectNames[0] = "VISION_BYPASS_MODE";
                    inspectResult.RearResult.DefectNames[0] = "VISION_BYPASS_MODE";
                    bReturn = true;
                    return true;
                }

                long timeoutTicks = Config.WaitTime.CommTimeout.InspResultTimeout.ToTimeSpan().Ticks;
                long startTimeStamp = Utils.GetTimestamp();
                while (true)
                {
                    Thread.Sleep(1);
                    if (bMachineStoppingCheck == true
                        && m_objDocument.GetRunStatus() == CDefine.ERunStatus.Stopping
                        )
                    {
                        return true;
                    }
                    if (Utils.GetTimestamp() - startTimeStamp > timeoutTicks)
                    {
                        mConsecutiveResultWaitTimeoutCount++;
                        // n회 이상 연속으로 실패하면 중알람 처리
                        if (m_objDocument.m_objConfig.GetOptionParameter().iResultWaitAlarmCount <= mConsecutiveResultWaitTimeoutCount)
                        {
                            SetProcessLog($"WaitInspectionResult({key}) -> Timeout Alarm");
                            mConsecutiveResultWaitTimeoutCount = 0;
                            m_objDocument.SetForcedAlarm(mDefine.AlarmTotalResultSuccessivelyTimeout);
                            return false;
                        }
                        else
                        {
                            SetProcessLog($"WaitInspectionResult({key}) -> Timeout Warning");
                            // 경알람 발생 후 진행
                            // AlarmMsg: 외관 검사기 종합 결과 응답 타임아웃 입니다
                            m_objDocument.SetForcedAlarm(mDefine.AlarmTotalResultTimeout);
                            // n회 연속으로 타임아웃이 발생하지 않았다면 BIN2로 처리함
                            inspectResult = new ReceiveData.TotalResultDataForRear()
                            {
                                CellID = key
                            };
                            inspectResult.FrontResult.DefectNames[0] = "RESULT_WAIT_TIMEOUT";
                            inspectResult.RearResult.DefectNames[0] = "RESULT_WAIT_TIMEOUT";
                            break;
                        }
                    }
                    if (mMapTotalResultData.TryGetValue(key, out inspectResult) == true)
                    {
                        mConsecutiveResultWaitTimeoutCount = 0;
                        mMapInspectResultData.Remove(key);
                        mMapTotalResultData.Remove(key);
                        var mapCellInformationDownlaod = m_objDocument.m_objProcessCIM.m_objProcessCIMCellLotInfomationDownload.MapCellLotInformationDownloadDefect;
                        mapCellInformationDownlaod.Remove(key);
                        break;
                    }
                }

                bReturn = true;
                return true;
            }
            finally
            {
                if (inspectResult != null)
                {
                    string resultString = inspectResult.FrontResult.Result;
                    cellInformation.Cell.FrontInspResult = CellDataManager.GetEnumFromResultString(ref resultString);
                    cellInformation.Cell.FrontInspReasonCodes = inspectResult.FrontResult.DefectNames;
                    resultString = inspectResult.RearResult.Result;
                    cellInformation.Cell.RearInspResult = CellDataManager.GetEnumFromResultString(ref resultString);
                    cellInformation.Cell.RearInspReasonCodes = inspectResult.RearResult.DefectNames;
                    SetProcessLog(string.Format("InspectResult({0}) -> Inner:{1},Mcr:{2},FrontResult:{3},FrontDefectCodes:{4},RearResult:{5},RearDefectCodes:{6}", cellInformation.Cell.ChannelName, inspectResult.CellID, inspectResult.CellID, cellInformation.Cell.FrontInspResult, string.Join("|", cellInformation.Cell.FrontInspReasonCodes), cellInformation.Cell.RearInspResult, string.Join("|", cellInformation.Cell.RearInspReasonCodes)));
                }
                SetProcessLog("[END] " + bReturn);
            }
        }

        public bool IsPrimary()
        {
            if (mDefine.InspIndex != CDefine.EInspInterface.PC1)
            {
                return false;
            }
            return true;
        }

        private bool SetGrab(int selectIndex)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog($"[START] SelectIndex: {selectIndex}");

            do
            {
#if !SCT_AMO
                if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() != CDefine.ERunMode.RealRun
                    )
                {
                    bReturn = true;
                    break;
                }
#endif
                if (m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE)
                {
                    bReturn = true;
                    break;
                }
                try
                {
                    mGrabStartRequest.SelectIndex = selectIndex;
                    SetProcessLog(string.Format("CheckCellData"));
                    // 시작 메시지를 보냈는지 확인
                    var requestDataCapture = mGrabStartRequest;
                    var acknowlegeDataCapture = mGrabStartAcknowlege;
                    if (
                        null != requestDataCapture
                        && null != acknowlegeDataCapture
                        && requestDataCapture.PositionA.InnerID == acknowlegeDataCapture.PositionA.InnerID
                        && requestDataCapture.PositionB.InnerID == acknowlegeDataCapture.PositionB.InnerID
                        && requestDataCapture.PositionA.CellID == acknowlegeDataCapture.PositionA.CellID
                        && requestDataCapture.PositionB.CellID == acknowlegeDataCapture.PositionB.CellID
                        && requestDataCapture.SelectIndex == acknowlegeDataCapture.SelectIndex
                        )
                    {
                        SetProcessLog(string.Format("AlreadyReceiveGrabStartData -> Skip -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3},SelectIndex:{4}", acknowlegeDataCapture.PositionA.InnerID, acknowlegeDataCapture.PositionA.CellID, acknowlegeDataCapture.PositionB.InnerID, acknowlegeDataCapture.PositionB.CellID, acknowlegeDataCapture.SelectIndex));
                        bReturn = true;
                        break;
                    }

                    // 데이터 초기화
                    SetProcessLog(string.Format("ResetData"));
                    mGrabStartAcknowlege = null;
                    mGrabEndAcknowlege = null;

                    // 사이클 타임
                    if (IsPrimary() == true)
                    {
                        if (mGrabStartRequest.PositionA.IsNoCell() == false
                            && mGrabStartRequest.SelectIndex == 0
                            )
                        {
                            CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(mGrabStartRequest.PositionA.CellID);
                            cycleTactItem.strInnerID = mGrabStartRequest.PositionA.InnerID;
                            cycleTactItem.strCellID = mGrabStartRequest.PositionA.CellID;
                            cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_START_TIME].SetTime();
                        }
                        if (mGrabStartRequest.PositionB.IsNoCell() == false
                            && mGrabStartRequest.SelectIndex == 1
                            )
                        {
                            CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(mGrabStartRequest.PositionB.CellID);
                            cycleTactItem.strInnerID = mGrabStartRequest.PositionB.InnerID;
                            cycleTactItem.strCellID = mGrabStartRequest.PositionB.CellID;
                            cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_START_TIME].SetTime();
                        }
                    }

                    SetProcessLog(string.Format("SendGrabStart"));
                    if (false == InspInterface.HLSendGrabStart(mGrabStartRequest, EWait.ForReceive, Config.WaitTime.CommTimeout.InspReplyTimeout.ToMilliseconds(), 1))
                    {
                        SetProcessLog(string.Format("SendGrabStart -> Error -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3},SelectIndex:{4}", mGrabStartRequest.PositionA.InnerID, mGrabStartRequest.PositionA.CellID, mGrabStartRequest.PositionB.InnerID, mGrabStartRequest.PositionB.CellID, mGrabStartRequest.SelectIndex));
                        if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                        {
                            // AlarmMsg: 검사 스테이지 인터페이스 그랩 시작 Send 실패 하였습니다.
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmGrabStartSendFail;
                            break;
                        }
                    }

                    SetProcessLog(string.Format("ReceiveDataValidation"));
                    if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                    {
                        if (
                            mGrabStartRequest.PositionA.InnerID != mGrabStartAcknowlege.PositionA.InnerID
                            || mGrabStartRequest.PositionB.InnerID != mGrabStartAcknowlege.PositionB.InnerID
                            || mGrabStartRequest.PositionA.CellID != mGrabStartAcknowlege.PositionA.CellID
                            || mGrabStartRequest.PositionB.CellID != mGrabStartAcknowlege.PositionB.CellID
                            || mGrabStartRequest.SelectIndex != mGrabStartAcknowlege.SelectIndex
                            )
                        {
                            SetProcessLog(string.Format("ReceiveDataValidation -> Error -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3},SelectIndex:{4}", mGrabStartAcknowlege.PositionA.InnerID, mGrabStartAcknowlege.PositionA.CellID, mGrabStartAcknowlege.PositionB.InnerID, mGrabStartAcknowlege.PositionB.CellID, mGrabStartAcknowlege.SelectIndex));
                            // AlarmMsg: 검사 스테이지 인터페이스 그랩 시작 ReceiveData가 정확하지 않습니다.
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmGrabStartValidationFail;
                            break;
                        }
                    }

                    // 사이클 타임
                    if (IsPrimary() == true)
                    {
                        if (mGrabStartRequest.PositionA.IsNoCell() == false
                            && mGrabStartRequest.SelectIndex == 0
                            )
                        {
                            CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(mGrabStartRequest.PositionA.CellID);
                            cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_END_START_TIME].SetTime();
                        }
                        if (mGrabStartRequest.PositionB.IsNoCell() == false
                            && mGrabStartRequest.SelectIndex == 1
                            )
                        {
                            CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(mGrabStartRequest.PositionB.CellID);
                            cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_END_START_TIME].SetTime();
                        }
                    }

                    SetProcessLog(string.Format("WaitForReceiveGrabEnd"));
                    if (false == InspInterface.WaitReceiveFlag(CDeviceSviInterface.EReceiveDataType.GrabEndRequest, Config.WaitTime.CommTimeout.InspGrabEndReplyTimeout.ToMilliseconds(), 1))
                    {
                        SetProcessLog(string.Format("WaitForReceiveGrabEnd -> Error -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3},SelectIndex:{4}", mGrabStartRequest.PositionA.InnerID, mGrabStartRequest.PositionA.CellID, mGrabStartRequest.PositionB.InnerID, mGrabStartRequest.PositionB.CellID, mGrabStartRequest.SelectIndex));
                        if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                        {
                            // AlarmMsg: 검사 스테이지 인터페이스 그랩 완료 Send 실패 하였습니다.
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmGrabEndTimeout;
                            break;
                        }
                    }

                    SetProcessLog(string.Format("ReceiveDataValidation"));
                    if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                    {
                        if (
                            mGrabStartRequest.PositionA.InnerID != mGrabEndAcknowlege.PositionA.InnerID
                            || mGrabStartRequest.PositionB.InnerID != mGrabEndAcknowlege.PositionB.InnerID
                            || mGrabStartRequest.PositionA.CellID != mGrabEndAcknowlege.PositionA.CellID
                            || mGrabStartRequest.PositionB.CellID != mGrabEndAcknowlege.PositionB.CellID
                            || mGrabStartRequest.SelectIndex != mGrabEndAcknowlege.SelectIndex
                            )
                        {
                            SetProcessLog(string.Format("ReceiveDataValidation -> Error -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3},SelectIndex:{4}", mGrabEndAcknowlege.PositionA.InnerID, mGrabEndAcknowlege.PositionA.CellID, mGrabEndAcknowlege.PositionB.InnerID, mGrabEndAcknowlege.PositionB.CellID, mGrabEndAcknowlege.SelectIndex));
                            // AlarmMsg: 검사 스테이지 인터페이스 그랩 완료 ReceiveData가 정확하지 않습니다.
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmGrabEndValidationFail;
                            break;
                        }
                    }

                    //////////////////////////////////////////////////////////////////////////
                    // 아래 동작은 중요하지 않음으로 에러 체크는 하지 않음
                    SetProcessLog("SendLightIntensity");
                    InspInterface.HLSendLightIntensityRequest(EWait.Skip);
                    SetProcessLog("SendLightTemperature");
                    InspInterface.HLSendLightTemperatureRequest(EWait.Skip);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
                bReturn = true;
            } while (false);

            SetProcessLog($"[END] SelectIndex: {selectIndex}, {bReturn}");
            return bReturn;
        }

        private bool SetCellDataSync()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog($"[START]");

            do
            {
#if !SCT_AMO
                if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() != CDefine.ERunMode.RealRun
                    )
                {
                    bReturn = true;
                    break;
                }
#endif
                if (m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE)
                {
                    bReturn = true;
                    break;
                }

                // 데이터 초기화
                SetProcessLog(string.Format("ResetData"));
                mCellDataSyncAcknowlege = null;

                SetProcessLog(string.Format("SendCellDataSync -> CellID: {0}, InnerID: {1}, JobID: {2}", mCellDataSyncRequest.CellID, mCellDataSyncRequest.InnerID, mCellDataSyncRequest.JobID));
                if (false == InspInterface.HLSendCellDataSync(mCellDataSyncRequest, EWait.ForReceive, Config.WaitTime.CommTimeout.InspReplyTimeout.ToMilliseconds(), 1))
                {
                    SetProcessLog(string.Format("SendCellDataSync -> Error -> CellID: {0}, InnerID: {1}, JobID: {2}", mCellDataSyncRequest.CellID, mCellDataSyncRequest.InnerID, mCellDataSyncRequest.JobID));
                    if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                    {
                        // AlarmMsg: 검사 스테이지 인터페이스 셀 데이터 동기화 Send 실패 하였습니다.
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCellDataSyncSendFail;
                        break;
                    }
                }

                SetProcessLog(string.Format("ReceiveDataValidation"));
                if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                {
                    if (mCellDataSyncRequest.CellID != mCellDataSyncAcknowlege.CellID
                        || mCellDataSyncRequest.InnerID != mCellDataSyncAcknowlege.InnerID
                        || mCellDataSyncRequest.JobID != mCellDataSyncAcknowlege.JobID
                        )
                    {
                        SetProcessLog(string.Format("ReceiveDataValidation -> Error -> CellID: {0}, InnerID: {1}, JobID: {2}", mCellDataSyncAcknowlege.CellID, mCellDataSyncAcknowlege.InnerID, mCellDataSyncAcknowlege.JobID));
                        // AlarmMsg: 검사 스테이지 인터페이스 셀 데이터 동기화 ReceiveData가 정확하지 않습니다.
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCellDataSyncValidationFail;
                        break;
                    }
                }
                bReturn = true;
            } while (false);

            SetProcessLog($"[END] {bReturn}");
            return bReturn;
        }

        private bool SetCheck()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            do
            {
#if !SCT_AMO
                if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.GetRunMode() != CDefine.ERunMode.RealRun
                    )
                {
                    bReturn = true;
                    break;
                }
#endif
                if (m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode == CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE)
                {
                    bReturn = true;
                    break;
                }

                SetProcessLog("SendCheckStatus");
                if (false == InspInterface.HLSendStateCheckRequest(EWait.ForReceive, Config.WaitTime.CommTimeout.InspReplyTimeout.ToMilliseconds(), 1))
                {
                    SetProcessLog("SendCheckStatus -> Error");
                    if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                    {
                        // AlarmMsg: 검사 스테이지 인터페이스 상태 확인 Send 실패 하였습니다.
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCheckSendFail;
                        break;
                    }
                }

                // 현재 SDV 상태 변경
                SetProcessLog(string.Format("SendCheckStatus -> Result -> {0}", mLastStatus));

                // 에러가 발생 했어도 Error Skip 모드에서 통과
                if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                {
                    // 현재 준비 상태가 아닐 경우 진행 할 수 없음
                    if (
                        DEVICE.Svi.EStatus.Check != mLastStatus
                        && DEVICE.Svi.EStatus.Idle != mLastStatus
                        )
                    {
                        // AlarmMsg: 검사 스테이지 검사 준비 상태가 아닙니다
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmNotReady;
                        break;
                    }
                }
                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn);
            return bReturn;
        }

        private bool SetRequestLightIntensity()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            do
            {
                if (
                    CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                    && CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE != m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode
                    )
                {
                    if (CDefine.ERunMode.RealRun == m_objDocument.GetRunMode())
                    {
                        SetProcessLog("SendLightIntensity");
                        if (false == InspInterface.HLSendLightIntensityRequest(EWait.Skip))
                        {
                            SetProcessLog("SendLightIntensity -> Error");
                            if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                            {
                                // AlarmMsg: 검사 스테이지 조명 광량 Send 실패 하였습니다.
                                m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmLightIntencitySendFail;
                                break;
                            }
                        }
                    }
                }
                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn);
            return bReturn;
        }

        private bool SetRequestLightTemperature()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            do
            {
                if (
                    CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                    && CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE != m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode
                    )
                {
                    if (CDefine.ERunMode.RealRun == m_objDocument.GetRunMode())
                    {
                        SetProcessLog("SendLightTemp");
                        if (false == InspInterface.HLSendLightTemperatureRequest(EWait.Skip))
                        {
                            SetProcessLog("SendLightTemp -> Error");
                            if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                            {
                                // AlarmMsg: 검사 스테이지 조명 온도 Send 실패 하였습니다.
                                m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmLightTemperatureSendFail;
                                break;
                            }
                        }
                    }
                }
                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn);
            return bReturn;
        }

        private bool IsCleanMapData(string key, TimeSpan registElapsed, HashSet<string> allCellID)
        {
            TimeSpan timeout = TimeSpan.FromHours(24);
            bool bEquipmentRunning = (
                m_objDocument.GetRunStatus() == CDefine.ERunStatus.Start
                || m_objDocument.GetRunStatus() == CDefine.ERunStatus.LoadingStop
                || m_objDocument.GetRunStatus() == CDefine.ERunStatus.Stopping
                );
            if (timeout < registElapsed
                && allCellID.Contains(key) == false
                && bEquipmentRunning == true
                )
            {
                return true;
            }
            return false;
        }

        private void InspectResultDataMap_OnAfterAdd(object sender, ConcurrentDictionary<string, ReceiveData.InspectResultData> e)
        {
            HashSet<string> allCellID = CellDataManager.AllInspectionKeys;
            var dataKeys = e.Keys.ToArray();
            foreach (string key in dataKeys)
            {
                if (IsCleanMapData(key, e[key].RegistElapsed, allCellID) == true)
                {
                    e.TryRemove(key, out ReceiveData.InspectResultData removeData);
                }
            }
        }

        private void TotalResultData_OnAfterAdd(object sender, ConcurrentDictionary<string, ReceiveData.TotalResultDataForRear> e)
        {
            HashSet<string> allCellID = CellDataManager.AllInspectionKeys;
            var dataKeys = e.Keys.ToArray();
            foreach (string key in dataKeys)
            {
                if (IsCleanMapData(key, e[key].RegistElapsed, allCellID) == true)
                {
                    e.TryRemove(key, out ReceiveData.TotalResultDataForRear removeData);
                }
            }
        }

        private void LightDataForDvReport_OnAfterAdd(object sender, ConcurrentDictionary<string, ReceiveData.LightDataForDvReport> e)
        {
            HashSet<string> allCellID = CellDataManager.AllInspectionKeys;
            var dataKeys = e.Keys.ToArray();
            foreach (string key in dataKeys)
            {
                if (IsCleanMapData(key, e[key].RegistElapsed, allCellID) == true)
                {
                    e.TryRemove(key, out ReceiveData.LightDataForDvReport removeData);
                }
            }
        }

        private void InspInterface_OnConnected(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                SpinWait.SpinUntil(() => m_objDocument.IsInitialized);
                // 검사기와 상태를 동기화 한다
                InspInterface.HLSendTimeSync(DateTime.Now);
            });
        }

        private void InspInterface_OnDisconnected(object sender, EventArgs e)
        {
            if (m_objDocument.GetRunStatus() == CDefine.ERunStatus.Start
                || m_objDocument.GetRunStatus() == CDefine.ERunStatus.LoadingStop
                )
            {
                if (m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode != CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE)
                {
                    m_objDocument.SetForcedAlarm(mDefine.AlarmDisconnected);
                }
            }
        }

        private void InspInterface_OnReceiveData(CDeviceSviInterface.CReceiveData receiveData)
        {
            ReceiveData.GrabStartData grabStartData;
            ReceiveData.GrabEndData grabEndData;
            ReceiveData.InspectResultData inspectResultData;
            ReceiveData.TotalResultDataForRear totalResultData;
            ReceiveData.AlignData alignData;
            ReceiveData.LightDataForDvReport lightDataForDvReport;
            ReceiveData.RecipeValidationData recipeValidationData;
            ReceiveData.AutoReviewData autoReviewData;
            int[] lightIntensity;
            int[] lightTemperature;
            string recipeName;
            string errorMessage;
            DEVICE.Svi.EStatus status;
            EPower laserStatus;
            EAlarmType alarmType;
            bool bIsRequestHandled = true;
            bool bIsAcknowledgeHandled = true;

            // 응답 메시지 처리
            switch (receiveData.eReceiveData)
            {
                case CDeviceSviInterface.EReceiveDataType.GrabStartAcknowledge:
                    if (true == receiveData.TryParseGrabStartData(out grabStartData))
                    {
                        // 사이클 타임
                        if (IsPrimary() == true)
                        {
                            if (grabStartData.PositionA.IsNoCell() == false
                                && grabStartData.SelectIndex == 0
                                )
                            {
                                CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(grabStartData.PositionA.CellID);
                                cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_END_TIME].SetTime();
                            }
                            if (grabStartData.PositionB.IsNoCell() == false
                                && grabStartData.SelectIndex == 1
                                )
                            {
                                CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(grabStartData.PositionB.CellID);
                                cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_END_TIME].SetTime();
                            }
                        }
                        mGrabStartAcknowlege = grabStartData;
                    }
                    // 비전 로그
                    SetProcessLog(string.Format("ReceiveGrabStart -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.CurrentRecipeInformationRequestAcknowledge:
                    if (true == receiveData.TryParseCurrentRecipeInformationRequestData(out recipeName, out lightIntensity))
                    {
                        //CConfig.CModelParameter changeModelParameter = m_objDocument.m_objModel.GetModelParameter( m_objConfig.GetSystemParameter().strPPID ).DeepClone();
                        //CConfig.CModelParameter currentModelParameter = m_objDocument.m_objModel.GetModelParameter( m_objConfig.GetSystemParameter().strPPID ).DeepClone();
                        //changeModelParameter.strJavasRecipeName = recipeName;
                        //for( int i = 0; i < changeModelParameter.dJavasLightParam.Length; i++ ) {
                        //	if( i < lightIntensity.Length ) {
                        //		changeModelParameter.dJavasLightParam[ i ] = Convert.ToDouble( lightIntensity[ i ] );
                        //	} else {
                        //		changeModelParameter.dJavasLightParam[ i ] = 0d;
                        //	}
                        //}
                        //bool bChanged = false;
                        //foreach( CCIMDefine.EPpidParamList item in Enum.GetValues( typeof( CCIMDefine.EPpidParamList ) ) ) {
                        //	if( changeModelParameter[ item ] != currentModelParameter[ item ] ) {
                        //		bChanged = true;
                        //		break;
                        //	}
                        //}
                        //// 변경사항이 있을때에만 CIM에 보고함
                        //if( true == bChanged ) {
                        //	//////////////////////////////////////////////////////////////////////////
                        //	// 비동기 처리 등록
                        //	Task.Run( () => {
                        //		// 설비 PAUSE 상태 대기
                        //		while( CCIMDefine.EMoveState.MOVE_STATE_PAUSE != m_objDocument.m_eMoveState[ ( int )CCIMDefine.EPresentState.CURRENT_STATE ] ) {
                        //			Thread.Sleep( 500 );
                        //		}
                        //		// 레시피 저장 및 CIM 보고
                        //		m_objDocument.m_objRecipe.SetSaveRecipeAndReportCim( changeModelParameter );
                        //		// 레시피 UI 업데이트
                        //		m_objDocument.m_objModel.IsRecipeChanged = true;
                        //	} );
                        //	//////////////////////////////////////////////////////////////////////////

                        //	// 만약 설비 RUNNING 상태에서 요청을 받으면 중알람을 발생시켜 설비를 PAUSE 상태로 변경시킴
                        //	if( CCIMDefine.EMoveState.MOVE_STATE_PAUSE != m_objDocument.m_eMoveState[ ( int )CCIMDefine.EPresentState.CURRENT_STATE ] ) {
                        //		m_objDocument.SetAlarmEvent( mDefine.AlarmRunningPpidChangeRequest );
                        //	}
                        //}
                    }
                    SetProcessLog(string.Format("ReceiveJavasRecipeValue -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.ErrorMessageAcknowledge:
                    // No Response
                    break;

                case CDeviceSviInterface.EReceiveDataType.InitializeMessageAcknowledge:
                    // No Response
                    break;

                case CDeviceSviInterface.EReceiveDataType.LightIntensityAcknowledge:
                    if (true == receiveData.TryParseLightIntensityData(out lightIntensity))
                    {
                        mLastLightIntensityData = lightIntensity;
                    }
                    break;

                case CDeviceSviInterface.EReceiveDataType.LightTemperatureAcknowledge:
                    if (true == receiveData.TryParseLightTemperatureData(out lightTemperature))
                    {
                        mLastLightTemperatureData = lightTemperature;
                    }
                    break;

                case CDeviceSviInterface.EReceiveDataType.TimeSyncAcknowledge:
                    // No Response
                    break;

                case CDeviceSviInterface.EReceiveDataType.VacuumAcknowledge:
                    // No Response
                    break;

                case CDeviceSviInterface.EReceiveDataType.LaserCheckAcknowledge:
                    if (true == receiveData.TryParseCheckLaserStatusData(out laserStatus))
                    {
                        mLastLaserStatus = laserStatus;
                        //if (IsPrimary() == true)
                        //{
                        //    if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY) == CDefine.OFF
                        //        && laserStatus == EPower.On
                        //        )
                        //    {
                        //        m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY, CDefine.ON);
                        //    }
                        //    else if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY) == CDefine.ON
                        //        && laserStatus == EPower.Off
                        //        )
                        //    {
                        //        m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY, CDefine.OFF);
                        //    }
                        //}
                    }
                    SetProcessLog(string.Format("ReceiveCheckLaserStatus -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.LaserInterlockAcknowledge:
                    // No Response
                    break;

                case CDeviceSviInterface.EReceiveDataType.StateCheckAcknowledge:
                    if (true == receiveData.TryParseCheckStatusData(out status, out errorMessage))
                    {
                        mLastStatus = status;
                        if (status == DEVICE.Svi.EStatus.Error)
                        {
                            mLastStatusErrorMessage = errorMessage;
                        }
                    }
                    SetProcessLog(string.Format("ReceiveStatusCheck -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.CellDataSyncAcknowledge:
                    if (receiveData.TryParseCellDataSyncData(out mCellDataSyncAcknowlege) == true)
                    {
                        // No Additional Action
                    }
                    SetProcessLog(string.Format("ReceiveCellDataSync -> {0}", receiveData.strReceiveData));
                    break;

                default:
                    bIsAcknowledgeHandled = false;
                    break;
            }

            // 요청 메시지 처리
            switch (receiveData.eReceiveData)
            {
                case CDeviceSviInterface.EReceiveDataType.GrabEndRequest:
                    if (true == receiveData.TryParseGrabEndData(out grabEndData))
                    {
                        // 사이클 타임
                        if (IsPrimary() == true)
                        {
                            if (grabEndData.PositionA.IsNoCell() == false
                                && grabEndData.SelectIndex == 0
                                )
                            {
                                CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(grabEndData.PositionA.CellID);
                                cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_END_END_TIME].SetTime();
                            }
                            if (grabEndData.PositionB.IsNoCell() == false
                                && grabEndData.SelectIndex == 1
                                )
                            {
                                CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(grabEndData.PositionB.CellID);
                                cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_END_END_TIME].SetTime();
                            }
                        }
                        mGrabEndAcknowlege = grabEndData;
                    }
                    // 비전 로그
                    SetProcessLog(string.Format("ReceiveGrabEnd -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.AlignDataRequest:
                    if (true == receiveData.TryParseAlignData(out alignData))
                    {
                        mLastAlignData = alignData;
                        // 얼라인 데이터 로그
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_ALIGN_DATA, string.Format(",{0},JAVAS,,{1},,{2}", CDefine.EPosition.A, alignData.PositionA.X / 1000d, alignData.PositionA.T / 1000d));
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_ALIGN_DATA, string.Format(",{0},JAVAS,,{1},,{2}", CDefine.EPosition.B, alignData.PositionB.X / 1000d, alignData.PositionB.T / 1000d));
                    }
                    // 비전 로그
                    SetProcessLog(string.Format("ReceiveAlignData -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.AutoReviewDataRequest:
                    if (true == receiveData.TryParseAutoReviewData(out autoReviewData))
                    {
                        // Not Used
                    }
                    // 비전 로그
                    SetProcessLog(string.Format("ReceiveAutoReview -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.CheckLaserAbleRequest:
                    // #확인받을것
                    // COVER DOWN이여도 사용 가능(인터락이 없다고함..Maybe)
                    // 슬라이드 감지 센서
                    // 슬라이드가 왼쪽으로 열리면 ON, 슬라이드가 오른쪽으로 닫히면 OFF
                    //if (
                    //    m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_LASER_COVER_UP) == true//Cover가 UP되어있으면 true
                    //    && // Door Slide
                    //    (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_OPEN) == false //Front door Open check.
                    //    || m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_OPEN) == false) //Rear door Open check.
                    //    )
                    {
                        InspInterface.HLSendLaserAbleAcknowlege(EPossible.Able);
                    }
                    //else
                    //{
                    //    m_objSviInterface.HLSendLaserAbleAcknowlege(EPossible.Disable);
                    //}
                    break;

                case CDeviceSviInterface.EReceiveDataType.LaserStatusDataRequest:
                    if (true == receiveData.TryParseLaserStatusData(out laserStatus))
                    {
                        mLastLaserStatus = laserStatus;
                        //if (IsPrimary() == true)
                        //{
                        //    if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY) == CDefine.OFF
                        //        && laserStatus == EPower.On
                        //        )
                        //    {
                        //        m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY, CDefine.ON);
                        //    }
                        //    else if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY) == CDefine.ON
                        //        && laserStatus == EPower.Off
                        //        )
                        //    {
                        //        m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY, CDefine.OFF);
                        //    }
                        //}
                    }
                    SetProcessLog(string.Format("ReceiveLaserStatus -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.ErrorMessageRequest:
                    if (true == receiveData.TryParseErrorMessageData(out errorMessage))
                    {
                        if (-1 != errorMessage.IndexOf("SAME_POSITION_DEFECT"))
                        {
                            // AlarmMsg: JAVAS 외관 검사 결과 동일한 위치에 손상이 발생 하였습니다.
                            m_objDocument.SetAlarmEvent(mDefine.AlarmJavasSamePositionDefect);
                            // 로그 기록
                            SetProcessLog(string.Format("ReceiveJavasSamePositionDefectAlarm -> {0}", errorMessage));
                        }
                        else
                        {
                            // AlarmMsg: JAVAS 외관 검사기에서 Error가 발생 하였습니다.
                            m_objDocument.SetAlarmEvent(mDefine.AlarmJavasError);
                            // 로그 기록
                            SetProcessLog(string.Format("ReceiveJavasUnownAlarm -> {0}", errorMessage));
                        }
                    }
                    break;

                case CDeviceSviInterface.EReceiveDataType.ResultDataRequest:
                    if (true == receiveData.TryParseInspectResultData(out inspectResultData))
                    {
                    }
                    SetProcessLog(string.Format("ReceiveInspectResult -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.TotalResultDataRequest:
                    if (true == receiveData.TryParseTotalResultDataForRear(out totalResultData))
                    {
                        OnReceiveTotalResult?.Invoke(this, totalResultData);
                        mMapTotalResultData.Add(totalResultData.CellID, totalResultData);
                        // 사이클 택타임 기록
                        if (IsPrimary() == true)
                        {
                            CCycleTact<CDefine.ECycleTactJavasInspection> cycleTactItem = m_objDocument.m_objCycleTactTime.GetJavasCycleTactItem(totalResultData.CellID);
                            cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_RESULT_TIME].SetTime();
                            cycleTactItem.strInspection_Class = totalResultData.RearResult.Result;
                            cycleTactItem.strInspection_Defect = string.Join(".", totalResultData.RearResult.DefectNames);
                            m_objDocument.m_objCycleTactTime.SetJavasCycleTactLog(m_objDocument, totalResultData.CellID);
                            var tact = TactTime.Manager.GetTactOrNull(new string[] { totalResultData.CellID });
                            if (tact != null)
                            {
                                tact.InspectionTact = cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_RESULT_TIME].GetTime() - cycleTactItem.dicTactTime[CDefine.ECycleTactJavasInspection.INSP_GRAB_START_START_TIME].GetTime();
                                m_objDocument.m_objTactTime.SetOutputTactTime(tact);
                            }
                        }
                    }
                    SetProcessLog(string.Format("ReceiveTotalResult -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.LightForDvListRequest:
                    if (true == receiveData.TryParseLightDataForDvReport(out lightDataForDvReport))
                    {
                        mMapLightDataForDvReport.Add(lightDataForDvReport.CellID, lightDataForDvReport);
                        LastLightDataForFdc = lightDataForDvReport;
                    }
                    SetProcessLog(string.Format("ReceiveLightIntensityForDvReport -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.JavasSoftwareErrorRequest:
                    if (true == receiveData.TryParseJavasSoftwareErrorData(out alarmType, out errorMessage))
                    {
                        if (EAlarmType.Heavy == alarmType)
                        {
                            if (-1 != errorMessage.IndexOf("CAM"))
                            {
                                // AlarmMsg: 검사PC 카메라에 이상이 있습니다.
                                m_objDocument.SetAlarmEvent(mDefine.AlarmJavasCameraCheck);
                                // 로그 기록
                                SetProcessLog(string.Format("ReceiveJavasCameraAlarm -> {0}", receiveData.strReceiveData));
                            }
                            else if (-1 != errorMessage.IndexOf("LIGHT"))
                            {
                                // AlarmMsg: 검사PC 조명에 이상이 있습니다.
                                m_objDocument.SetAlarmEvent(mDefine.AlarmJavasLightControllerCheck);
                                // 로그 기록
                                SetProcessLog(string.Format("ReceiveJavasLightControllerAlarm -> {0}", receiveData.strReceiveData));
                            }
                        }
                        else if (EAlarmType.Light == alarmType)
                        {
                        }
                    }
                    break;

                case CDeviceSviInterface.EReceiveDataType.RecipeValidationDataRequest:
                    if (true == receiveData.TryParseRecipeValidationData(out recipeValidationData))
                    {
                        mMapRecipeValidationData.SetDictonary(recipeValidationData.Data);
                    }
                    SetProcessLog(string.Format("ReceiveRecipeValidation -> {0}", receiveData.strReceiveData));
                    break;

                default:
                    bIsRequestHandled = false;
                    break;
            }

            // 처리된 패킷이 없는 경우 반환함
            if (!bIsAcknowledgeHandled && !bIsRequestHandled)
            {
                return;
            }

            // 패킷 처리 완료 플래그 업데이트
            InspInterface.SetMessageProcessDone(receiveData);
        }

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                mDefine.LogType,
                string.Format("{0}({2}) -> {1}", callerMemberName, logMessage, mDefine.InspIndex)
                );
        }

        private void InspInterface_OnParsingError(object sender, string e)
        {
            if (sender is CDeviceSviInterface.CReceiveData packet
                && packet.LogType == InspInterface.LogType
                )
            {
                m_objDocument.SetUpdateLog(InspInterface.LogType, e);
            }
        }

        private void ThreadProcess()
        {
            while (mbThreadExit == false)
            {
                Thread.Sleep(10);

                if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Start
                    || m_objDocument.GetRunStatus() != CDefine.ERunStatus.LoadingStop
                    || m_objDocument.GetRunStatus() != CDefine.ERunStatus.Stopping
                    )
                {
                    if (mEquipmentStopStopwatch.IsRunning == false)
                    {
                        mEquipmentStopStopwatch.Start();
                    }
                }
                else
                {
                    if (mEquipmentStopStopwatch.IsRunning == true)
                    {
                        mEquipmentStopStopwatch.Stop();
                        mMapInspectResultData.ForEach((key, value) => value.EquipmentStopTime += mEquipmentStopStopwatch.Elapsed);
                        mEquipmentStopStopwatch.Reset();
                    }
                }

                if (mCommand == ECommand.Idle)
                {
                    continue;
                }

                RaiseMccLogWhenJobsBegined();
                mStatus = EStatus.Unknown;
                if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
                {
                    Thread.Sleep(CDefine.DEF_SIMULATION_SLEEP_TIME);
                }
                switch (mCommand)
                {
                    case ECommand.Check:
                        if (true == SetCheck())
                        {
                            mStatus = EStatus.Check;
                        }
                        break;

                    case ECommand.GrabP1:
                        if (true == SetGrab(0))
                        {
                            mStatus = EStatus.GrabP1;
                        }
                        break;

                    case ECommand.GrabP2:
                        if (true == SetGrab(1))
                        {
                            mStatus = EStatus.GrabP2;
                        }
                        break;

                    case ECommand.SendVacuumOn:
                        mStatus = EStatus.SendVacuumOn;
                        break;

                    case ECommand.SendVacuumOff:
                        mStatus = EStatus.SendVacuumOff;
                        break;

                    case ECommand.LightIntensityRequest:
                        if (true == SetRequestLightIntensity())
                        {
                            mStatus = EStatus.LightIntensityRequest;
                        }
                        break;

                    case ECommand.LightTemperatureRequest:
                        if (true == SetRequestLightTemperature())
                        {
                            mStatus = EStatus.LightTemperatureRequest;
                        }
                        break;

                    case ECommand.CellDataSync:
                        if (true == SetCellDataSync())
                        {
                            mStatus = EStatus.CellDataSync;
                        }
                        break;

                    case ECommand.Stop:
                        // 사용자 정지 체크
                        mStatus = EStatus.Stop;
                        break;
                }
                RaiseMccLogWhenJobsFinished();

                // 날라온 Command에 대한 알람이 있었는지 체크. STS_UNKNOWN 그대로이면 알람.
                if (mStatus == EStatus.Unknown)
                {
                    mStatus = EStatus.Error;
                    m_objDocument.SetAlarmEvent(m_objAlarmStructure);
                    int iReturn = (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP;
                    // 알람에 대해 작업자 입력에 따른 처리
                    switch (iReturn)
                    {
                        // 해당 커맨드에 대한 재시도
                        case (int)CDefine.EErrorProcess.ERROR_PROCESS_RETRY:
                            continue;
                        // 작업자가 알람 처리를 정상적으로 했다는 가정 하에 커맨드에 맞는 상태로 변경
                        case (int)CDefine.EErrorProcess.ERROR_PROCESS_CONTINUE:
                            if (ECommand.Check == mCommand)
                            {
                                mStatus = EStatus.Check;
                            }
                            else if (ECommand.GrabP1 == mCommand)
                            {
                                mStatus = EStatus.GrabP1;
                            }
                            else if (ECommand.GrabP2 == mCommand)
                            {
                                mStatus = EStatus.GrabP2;
                            }
                            else if (ECommand.SendVacuumOn == mCommand)
                            {
                                mStatus = EStatus.SendVacuumOn;
                            }
                            else if (ECommand.SendVacuumOff == mCommand)
                            {
                                mStatus = EStatus.SendVacuumOff;
                            }
                            else if (ECommand.LightIntensityRequest == mCommand)
                            {
                                mStatus = EStatus.LightIntensityRequest;
                            }
                            else if (ECommand.LightTemperatureRequest == mCommand)
                            {
                                mStatus = EStatus.LightTemperatureRequest;
                            }
                            else if (ECommand.CellDataSync == mCommand)
                            {
                                mStatus = EStatus.CellDataSync;
                            }
                            break;
                        // 에러에 대한 설비 정지
                        case (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP:
                            mCommand = ECommand.Stop;
                            continue;
                    }
                }

                mCommand = ECommand.Idle;
            }
        }
    }
}
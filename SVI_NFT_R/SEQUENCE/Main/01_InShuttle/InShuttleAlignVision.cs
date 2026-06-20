using HLDevice;
using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Align;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R
{
    public partial class InShuttleAlignVision : CProcessAbstract
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            Align,
            CheckExistModel
        };

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            Align,
            CheckExistModel
        };

        public CDefine.EAlign AlignIndex => mDefine.AlignIndex;
        public CDeviceAlignInterface AlignInterface { get; private set; }
        public AlignResult[] AlignResults { get; private set; }
        public double[] LightTemperatureValues => mLightTemperatureValues;
        public double[] LightLevelValues => mLightLevelValues;
        public SendData.AlignStartData AlignStartRequestData => mAlignStartRequest;
        public event EventHandler ReceiveCalibrationStart;
        public event EventHandler<ReceiveData.CalibrationMoveData> ReceiveCalibrationMove;
        public event EventHandler ReceiveCalibrationFinish;
        public event EventHandler ReceiveAlignError;
        private const string ID = nameof(InShuttleAlignVision);
        private ECommand mCommand;
        private EStatus mStatus;
        //////////////////////////////////////////////////////////////////////////
        // 송신 데이터 저장
        private SendData.AlignStartData mAlignStartRequest = new SendData.AlignStartData();
        //////////////////////////////////////////////////////////////////////////
        // 수신 데이터 저장
        private ReceiveData.AlignStartData mAlignStartAcknowledge = null;
        private ReceiveData.AlignCompleteData mAlignCompleteAcknowledge = null;
        private readonly Dictionary<string, ReceiveData.AlignResultData> mAlignResultAcknowledge = new Dictionary<string, ReceiveData.AlignResultData>();
        private HashSet<string> mModelNames = new HashSet<string>();
        private double[] mLightTemperatureValues = new double[0];
        private double[] mLightLevelValues = new double[0];
        private bool mbReceiveVisionError = false;
        public override string ToString() => $"{ID},{AlignIndex},{mCommand},{mStatus}";

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

                AlignResults = new AlignResult[]
                {
                    new AlignResult(mDefine.AlignIndex, m_objConfig),
                    new AlignResult(mDefine.AlignIndex, m_objConfig)
                };

                // 검사 인터페이스 객체 연결
                AlignInterface = m_objDocument.m_objProcessMain.m_objAlignInterfaces[mDefine.AlignIndex];
                bool mbConnectedBeforeRegistEventHandler = AlignInterface.HLIsConnected();
                // 수신 데이터 콜백 함수 등록
                DEVICE.Align.ExtensionMethods.ParsingError += OnParsingError;
                AlignInterface.SetCallbackFunction(OnReceiveData);
                AlignInterface.OnConnected += OnConnected;
                AlignInterface.OnDisconnected += OnDisconnected;
                // 쓰레드 생성 및 시작
                mThreadProcess = new Thread(ThreadProcess);
                mThreadProcess.Start();

                // 이벤트 핸들러를 등록 하기 전에 통신이 연결 돼 있었으면 동기화 진행
                if (true == mbConnectedBeforeRegistEventHandler)
                {
                    // 검사기와 상태를 동기화 한다
                    OnConnected(this, EventArgs.Empty);
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
            AlignInterface.OnConnected -= OnConnected;
            AlignInterface.OnDisconnected -= OnDisconnected;
            AlignInterface.SetCallbackFunction(null);
            DEVICE.Align.ExtensionMethods.ParsingError -= OnParsingError;
        }

        public bool SetCommand(ECommand eCommand)
        {
            Debug.Assert(mCommand == ECommand.Idle);
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

        public override bool WaitForEndProcess()
        {
            bool bResult = false;

            do
            {
                while (mCommand != ECommand.Idle
                    || m_objDocument.GetRunStatus() == CDefine.ERunStatus.Pause
                    )
                {
                    Thread.Sleep(WAIT_FOR_END_PROCESS_PERIOD_TIME);
                }
                if (mStatus == EStatus.Stop)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 입력된 셀 컬랙션을 기준으로 얼라인 데이터를 셋팅한다
        /// </summary>
        /// <param name="cellContainer">적용할 셀 데이터 컬랙션</param>
        public void SetAlignInformation(IReadOnlyList<CellDataHandler> cellContainer)
        {
            mAlignStartRequest = new SendData.AlignStartData();
            string modelID = m_objConfig.GetSystemParameter().strPPID;
            foreach (var cellDataHandler in cellContainer)
            {
                if (cellDataHandler.IsCellExist() == false)
                {
                    continue;
                }

                switch (cellDataHandler.PositionIndex)
                {
                    case 0:
                        mAlignStartRequest.P1ModelID = modelID;
                        mAlignStartRequest.P1InnerID = cellDataHandler.GetInnerID();
                        mAlignStartRequest.P1CellID = cellDataHandler.GetInspectionKey();
                        SetProcessLog($"SetAlignInformation -> ReadyP1(ModelID: {modelID}, InnerID: {mAlignStartRequest.P1InnerID}, CellID: {mAlignStartRequest.P1CellID})");
                        break;

                    case 1:
                        mAlignStartRequest.P2ModelID = modelID;
                        mAlignStartRequest.P2InnerID = cellDataHandler.GetInnerID();
                        mAlignStartRequest.P2CellID = cellDataHandler.GetInspectionKey();
                        SetProcessLog($"SetAlignInformation -> ReadyP2(ModelID: {modelID}, InnerID: {mAlignStartRequest.P2InnerID}, CellID: {mAlignStartRequest.P2CellID})");
                        break;

                    default:
                        break;
                }
            }
        }

        public void SetAlignInformation(CellDataHandler cellDataHandler)
        {
            SetAlignInformation(new CellDataHandler[] { cellDataHandler });
        }

        /// <summary>
        /// [매뉴얼용] 매뉴얼 촬상을 위해 임시 CellID를 부여한다
        /// </summary>
        /// <param name="cellPlace">
        /// 셀 위치를 지정함 (예: P1만 촬상 cellPlace = ECellPlacement.P1, 동시 촬상 cellPlace = ECellPlacement.P1 | ECellPlacement.P2)
        /// </param>
        public void SetAlignInformationTemporaryId(ECellPlacement cellPlace, string postfix = "TEST")
        {
            mAlignStartRequest = new SendData.AlignStartData();
            string modelID = m_objConfig.GetSystemParameter().strPPID;
            if (cellPlace.HasFlag(ECellPlacement.P1) == true)
            {
                mAlignStartRequest.P1ModelID = modelID;
                mAlignStartRequest.P1InnerID = mAlignStartRequest.P1CellID = $"P1_{DateTime.Now:yyyyMMddHHmmssfff}_{postfix}";
                SetProcessLog($"SetAlignInformaitionTemporaryId -> ReadyP1(ModelID: {modelID}, InnerID: {mAlignStartRequest.P1InnerID}, CellID: {mAlignStartRequest.P1CellID}");
            }
            if (cellPlace.HasFlag(ECellPlacement.P2) == true)
            {
                mAlignStartRequest.P2ModelID = modelID;
                mAlignStartRequest.P2InnerID = mAlignStartRequest.P2CellID = $"P2_{DateTime.Now:yyyyMMddHHmmssfff}_{postfix}";
                SetProcessLog($"SetAlignInformaitionTemporaryId -> ReadyP2(ModelID: {modelID}, InnerID: {mAlignStartRequest.P2InnerID}, CellID: {mAlignStartRequest.P2CellID}");
            }
        }

        /// <summary>
        /// 셀 데이터에 얼라인 결과 값을 적용한다
        /// </summary>
        /// <param name="cellContainer">적용할 셀 데이터 컬랙션</param>
        public void ApplyAlignResultDataToCellData(IReadOnlyList<CellDataHandler> cellContainer)
        {
            foreach (var cellDataHandler in cellContainer)
            {
                int index = cellDataHandler.PositionIndex;
                if (cellDataHandler.IsCellExist() == false)
                {
                    continue;
                }
                var alignResult = GetAlignResultDataFromCellData(cellDataHandler);
                alignResult.CameraIndex = mDefine.CameraIndexes[index];
                alignResult.TotalRevisionX += alignResult.RevisionX = AlignResults[index].X;
                alignResult.TotalRevisionY += alignResult.RevisionY = AlignResults[index].Y;
                alignResult.TotalRevisionT += alignResult.RevisionT = AlignResults[index].T;
                alignResult.Score = AlignResults[index].Score;
                alignResult.Judge = AlignResults[index].CanUse ? EJudge.OK : EJudge.NG;
                //alignResult.RetryCount = 0;
                //cellDataHandler.Data.Cell.PreAlignStatus = CellData.EStatus.Done;
                cellDataHandler.IsChanged = true;

                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_ALIGN_DATA, $",{alignResult.CameraIndex},{mDefine.AlignIndex},{alignResult.Score:0.0}%,{alignResult.RevisionX:0.000},{alignResult.RevisionY:0.000},{alignResult.RevisionT:0.000}");
            }
        }

        public void ApplyAlignResultDataToCellData(CellDataHandler cellDataHandler)
        {
            ApplyAlignResultDataToCellData(new CellDataHandler[] { cellDataHandler });
        }

        public OneCell.AlignResultData GetAlignResultDataFromCellData(CellDataHandler cellDataHandler) => mDefine.GetAlignResultData(cellDataHandler);

        public bool ShouldProcessCheck(string innerID, string cellID, OneCell.AlignResultData alignResult, ref int tryIndex)
        {
            if (m_objConfig.GetAlignOptionParameter(AlignIndex).RetryCount == 0)
            {
                SetProcessLog($"[SKIP] NotUse");
                return false;
            }
            tryIndex = 0;
            if (CheckTolerance(innerID, cellID, alignResult, ref tryIndex) == true)
            {
                // ! 얼라인 값이 공차 안에 들어와도 첫 번째 리트라이는 무조건 실행하도록 함
                //setProcessLog($"[SKIP] Pass");
                //return false;
            }
            return true;
        }

        public bool CheckRetryCountOverAndIncreaseIndex(ref int tryIndex)
        {
            if (tryIndex >= m_objConfig.GetAlignOptionParameter(AlignIndex).RetryCount)
            {
                SetProcessLog($"[FAILED] AlignRetryCountOver");
                m_objDocument.SetAlarmEvent(mDefine.AlarmRetryCountOver);
                return false;
            }
            tryIndex++;
            return true;
        }

        public bool CheckTolerance(string innerID, string cellID, OneCell.AlignResultData alignResult, ref int tryIndex)
        {
            SetProcessLog($"[START] TryIndex: {tryIndex}");
            bool bSpecOver = false;
            if (Math.Abs(alignResult.RevisionX) > m_objConfig.GetAlignOptionParameter(AlignIndex).ToleranceX)
            {
                SetProcessLog($"CheckTolerance.X -> OverRange(InnerID: {innerID}, CellID: {cellID}, RevisionX: {alignResult.RevisionX})");
                bSpecOver = true;
            }
            if (Math.Abs(alignResult.RevisionY) > m_objConfig.GetAlignOptionParameter(AlignIndex).ToleranceY)
            {
                SetProcessLog($"CheckTolerance.Y -> OverRange(InnerID: {innerID}, CellID: {cellID}, RevisionY: {alignResult.RevisionY})");
                bSpecOver = true;
            }
            if (Math.Abs(alignResult.RevisionT) > m_objConfig.GetAlignOptionParameter(AlignIndex).ToleranceT)
            {
                SetProcessLog($"CheckTolerance.T -> OverRange(InnerID: {innerID}, CellID: {cellID}, RevisionT: {alignResult.RevisionT})");
                bSpecOver = true;
            }
            if (bSpecOver == true)
            {
                SetProcessLog($"[END] false");
                return false;
            }
            SetProcessLog($"Pass(InnerID: {innerID}, CellID: {cellID}, RevisionXYT: ({alignResult.RevisionX}, {alignResult.RevisionY}, {alignResult.RevisionT}) TotalRevisionXYT: ({alignResult.TotalRevisionX}, {alignResult.TotalRevisionX}, {alignResult.TotalRevisionX}))");
            SetProcessLog($"[END] true");
            return true;
        }

        public Task<bool> TaskLightTemperatureRequest()
        {
            // ! 여러 툴이 통신 인터페이스를 공유 할 때 0번 툴에서만 요청 처리함
            Debug.Assert(mDefine.IsPrimaryDevice);
            string methodName = MethodBase.GetCurrentMethod().Name;
            return Task.Run(() =>
            {
                bool bResult = false;
                SetProcessLog($"[START]", methodName);
                try
                {
                    if (AlignInterface.HLSendLightTemperatureRequest(EWait.ForReceive, Config.WaitTime.CommTimeout.AlignVisionTimeout.ToMilliseconds()) == false)
                    {
                        SetProcessLog($"[TIMEOUT]", methodName);
                        return false;
                    }
                    bResult = true;
                    return true;
                }
                finally
                {
                    SetProcessLog($"[END] {bResult}", methodName);
                }
            });
        }

        public Task<bool> TaskLightLevelRequest()
        {
            // ! 여러 툴이 통신 인터페이스를 공유 할 때 0번 툴에서만 요청 처리함
            Debug.Assert(mDefine.IsPrimaryDevice);
            string methodName = MethodBase.GetCurrentMethod().Name;
            return Task.Run(() =>
            {
                bool bResult = false;
                SetProcessLog($"[START]", methodName);
                try
                {
                    if (AlignInterface.HLSendLightLevelRequest(EWait.ForReceive, Config.WaitTime.CommTimeout.AlignVisionTimeout.ToMilliseconds()) == false)
                    {
                        SetProcessLog($"[TIMEOUT]", methodName);
                        return false;
                    }
                    bResult = true;
                    return true;
                }
                finally
                {
                    SetProcessLog($"[END] {bResult}", methodName);
                }
            });
        }

        public Task<bool> TaskCalibrationMoveCompleteRequest()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            return Task.Run(() =>
            {
                bool bResult = false;
                SetProcessLog($"[START]", methodName);
                try
                {
                    if (AlignInterface.HLSendCalibrationMoveCompleteRequest(EWait.ForReceive, Config.WaitTime.CommTimeout.AlignVisionTimeout.ToMilliseconds()) == false)
                    {
                        SetProcessLog($"[TIMEOUT]", methodName);
                        return false;
                    }
                    bResult = true;
                    return true;
                }
                finally
                {
                    SetProcessLog($"[END] {bResult}", methodName);
                }
            });
        }

        private bool SetCheckExistModel()
        {
            // ! 여러 툴이 통신 인터페이스를 공유 할 때 0번 툴에서만 요청 처리함
            if (mDefine.IsPrimaryDevice == false)
            {
                return true;
            }
            /// <![CDATA[
            /// 얼라인 프로그램에 현재 PPID가 존재하는지 확인하는 기능
            ///
            /// * AutoRun 중에는 PPID가 변경 될 수 없는 구조임으로 Restart시에만 체크함
            /// ]]>
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            try
            {
                if (m_objConfig.GetAlignOptionParameter(AlignIndex).bUseVision == false)
                {
                    SetProcessLog("[SKIP] NotUse");
                    return true;
                }
#if !SCT_ALIGN
                if (m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
                {
                    SetProcessLog("[SKIP] SimulationMode");
                    return true;
                }
                if (m_objDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
                {
                    SetProcessLog("[SKIP] UnlinkDryrun");
                    return true;
                }
#endif
                if (AlignInterface.HLIsConnected() == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmDisconnected;
                    return false;
                }

                SetProcessLog("SendModelListRequest");
                if (AlignInterface.HLSendModelListRequest(EWait.ForReceive, Config.WaitTime.CommTimeout.AlignVisionTimeout.ToMilliseconds()) == false)
                {
                    SetProcessLog("SendModelListRequest -> Timeout");
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmModelListTimeout;
                    return false;
                }

                SetProcessLog("WaitReceiveModelListResult");
                if (AlignInterface.WaitReceiveFlag(CDeviceAlignInterface.EReceiveDataType.ModelListResultRequest, Config.WaitTime.CommTimeout.AlignVisionTimeout.ToMilliseconds()) == false)
                {
                    SetProcessLog("WaitReceiveModelListResult -> Timeout");
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmModelListTimeout;
                    return false;
                }

                SetProcessLog("WaitReceiveModelListResult");
                var modelNames = mModelNames;
                if (modelNames.Contains(m_objDocument.m_objConfig.GetSystemParameter().strPPID) == false)
                {
                    SetProcessLog($"WaitReceiveModelListResult -> CanNotFoundModel");
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmModelNotFound;
                    return false;
                }

                bReturn = true;
                return true;
            }
            finally
            {
                SetProcessLog("[END] " + bReturn);
            }
        }

        private bool SetAlign()
        {
            /// <![CDATA[
            /// 다음 동작들을 순차적으로 실행하는 기능
            ///  1. 얼라인 요청
            ///  2. 얼라인 완료 대기
            ///  3. 얼라인 결과 확인
            ///  4. 인터락 확인
            ///
            /// * 동작을 실행하기 전에 SetAlignInformation or SetAlignInformaitionTemporaryId 함수로 정보를 설정해야함
            /// * 동작이 정상처리된 경우 ApplyAlignResultDataToCellData 함수로 셀 데이터에 결과를 업데이트해야함
            /// ]]>
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            try
            {
                if (m_objConfig.GetAlignOptionParameter(AlignIndex).bUseVision == false)
                {
                    SetProcessLog("[SKIP] NotUse");
                    Array.ForEach(AlignResults, item => item.SetValue(100d, 0d, 0d, 0d));
                    Thread.Sleep(500);
                    return true;
                }
#if !SCT_ALIGN
                if (m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
                {
                    SetProcessLog("[SKIP] SimulationMode");
                    Array.ForEach(AlignResults, item => item.SetValue(100d, 0d, 0d, 0d));
                    Thread.Sleep(500);
                    return true;
                }

                if (m_objDocument.GetRunMode() == CDefine.ERunMode.UnlinkDryrun)
                {
                    SetProcessLog("[SKIP] UnlinkDryrun");
                    Array.ForEach(AlignResults, item => item.SetValue(100d, 0d, 0d, 0d));
                    Thread.Sleep(500);
                    return true;
                }
#endif
                if (AlignInterface.HLIsConnected() == false)
                {
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmDisconnected;
                    return false;
                }

                SetProcessLog("ResetData");
                mbReceiveVisionError = false;
                mAlignStartAcknowledge = null;
                mAlignCompleteAcknowledge = null;
                mAlignResultAcknowledge.Clear();

                SetProcessLog("SendAlignStart");
                var captureRequest = mAlignStartRequest;
                if (captureRequest.P1InnerID != CDeviceAlignInterface.NOCELL)
                {
                    AlignResults[0].Reset();
                }
                if (captureRequest.P2InnerID != CDeviceAlignInterface.NOCELL)
                {
                    AlignResults[1].Reset();
                }
                if (AlignInterface.HLSendAlignStartRequest(captureRequest, EWait.Skip) == false)
                {
                    SetProcessLog("SendAlignStart -> Timeout");
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAlignStartTimeout;
                    return false;
                }

                SetProcessLog("WaitReceiveAlignComplete");
                TimeSpan timeout = Config.WaitTime.CommTimeout.AlignVisionTimeout.ToTimeSpan();
                if (SpinWait.SpinUntil(() => (AlignInterface.GetReceiveFlag(CDeviceAlignInterface.EReceiveDataType.AlignCompleteRequest) && AlignInterface.GetReceiveFlag(CDeviceAlignInterface.EReceiveDataType.AlignResultRequest)) || mbReceiveVisionError, timeout) == false)
                {
                    SetProcessLog("WaitReceiveAlignComplete -> Timeout");
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAlignCompleteTimeout;
                    return false;
                }
                if (mbReceiveVisionError == true)
                {
                    SetProcessLog("WaitReceiveAlignComplete -> ReceiveVisionError");
                    return false;
                }

                var captureAcknowledge = mAlignCompleteAcknowledge;
                if (captureAcknowledge == null
                    || captureAcknowledge.P1ModelID != captureRequest.P1ModelID
                    || captureAcknowledge.P1InnerID != captureRequest.P1InnerID
                    || captureAcknowledge.P1CellID != captureRequest.P1CellID
                    || captureAcknowledge.P2ModelID != captureRequest.P2ModelID
                    || captureAcknowledge.P2InnerID != captureRequest.P2InnerID
                    || captureAcknowledge.P2CellID != captureRequest.P2CellID
                    )
                {
                    SetProcessLog($"AlignCompleteAcknowledge -> ValidationFail");
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAlignCompleteValidationFail;
                    return false;
                }

                var existInnerIds = captureAcknowledge.GetExistInnerIds();
                if (existInnerIds.Count != mAlignResultAcknowledge.Count)
                {
                    SetProcessLog($"AlignResultAcknowledge -> AlignResultCountMissmatch");
                    m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAlignCompleteValidationFail;
                    return false;
                }

                m_objAlarmStructure.iAlarmCode = (int)CAlarmDefine.EAlarmList.NOT_REGISTED_ALARM;
                foreach (string innerID in existInnerIds)
                {
                    if (mAlignResultAcknowledge.ContainsKey(innerID) == false)
                    {
                        SetProcessLog($"AlignResultAcknowledge -> AlignResultNotFound(InnerID: {innerID})");
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAlignCompleteValidationFail;
                        continue;
                    }

                    ReceiveData.AlignResultData alignResultData = mAlignResultAcknowledge[innerID];
                    int index = 0;
                    if (innerID == captureRequest.P1InnerID)
                    {
                        index = 0;
                    }
                    else if (innerID == captureRequest.P2InnerID)
                    {
                        index = 1;
                    }
                    AlignResults[index].SetValue(alignResultData.Score / 1000d, alignResultData.X / 1000d, alignResultData.Y / 1000d, alignResultData.T / 1000d);
                    AlignResult alignResult = AlignResults[index];
                    SetProcessLog($"ALIGN_DATA[{index}] = {{InnerID: {innerID}, CellID: {alignResultData.CellID}, Score: {alignResult.Score}, X: {alignResult.X}, Y: {alignResult.Y}, T: {alignResult.T}}}");
                    if (alignResult.IsInterlockX == true)
                    {
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmInterlockX[index];
                        SetProcessLog($"CheckInterlock.X -> OverRange(InnerID: {innerID}, CellID: {alignResultData.CellID})");
                    }
                    if (alignResult.IsInterlockY == true)
                    {
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmInterlockY[index];
                        SetProcessLog($"CheckInterlock.Y -> OverRange(InnerID: {innerID}, CellID: {alignResultData.CellID})");
                    }
                    if (alignResult.IsInterlockT == true)
                    {
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmInterlockT[index];
                        SetProcessLog($"CheckInterlock.T -> OverRange(InnerID: {innerID}, CellID: {alignResultData.CellID})");
                    }
                    if (alignResult.IsInterlockScore == true)
                    {
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmInterlockScore[index];
                        SetProcessLog($"CheckInterlock.Score -> OverRange(InnerID: {innerID}, CellID: {alignResultData.CellID})");
                    }
                }
                if (m_objAlarmStructure.iAlarmCode != (int)CAlarmDefine.EAlarmList.NOT_REGISTED_ALARM)
                {
                    return false;
                }

                bReturn = true;
                return true;
            }
            finally
            {
                SetProcessLog("[END] " + bReturn);
            }
        }

        private void OnConnected(object sender, EventArgs e)
        {
            if (mDefine.IsPrimaryDevice == false)
            {
                return;
            }

            Task.Run(() =>
            {
                SpinWait.SpinUntil(() => m_objDocument.IsInitialized);
                Thread.Sleep(1000);
                // 동기화
                AlignInterface.HLSendTimeSyncRequest(DateTime.Now);
            });

            mbReceiveVisionError = false;
            mAlignStartAcknowledge = null;
            mAlignCompleteAcknowledge = null;
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            if (mDefine.IsPrimaryDevice == false)
            {
                return;
            }

            if (m_objDocument.GetRunStatus() == CDefine.ERunStatus.Start
                || m_objDocument.GetRunStatus() == CDefine.ERunStatus.LoadingStop
                )
            {
                m_objDocument.SetAlarmEvent(mDefine.AlarmDisconnected);
            }
        }

        private void RaiseReceiveCalibrationStartEvent() => ReceiveCalibrationStart?.Invoke(this, EventArgs.Empty);

        private void RaiseReceiveCalibrationMoveEvent(ReceiveData.CalibrationMoveData data) => ReceiveCalibrationMove?.BeginInvoke(this, data, null, null);

        private void RaiseReceiveCalibrationFinishEvent() => ReceiveCalibrationFinish?.Invoke(this, EventArgs.Empty);

        private void RaiseReceiveAlignErrorEvent() => ReceiveAlignError?.Invoke(this, EventArgs.Empty);

        private void OnReceiveData(CDeviceAlignInterface.CReceiveData receiveData)
        {
            int index;
            ReceiveData.AlignStartData alignStartData;
            // 응답 메시지 처리
            switch (receiveData.eReceiveData)
            {
                case CDeviceAlignInterface.EReceiveDataType.AlignStartAcknowledge:
                    if (receiveData.TryParseAlignStartData(out alignStartData) == true)
                    {
                        mAlignStartAcknowledge = alignStartData;
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.CalibrationMoveCompleteAcknowledge:
                    if (mDefine.IsPrimaryDevice == true)
                    {
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.EquipmentErrorAcknowledge:
                    if (mDefine.IsPrimaryDevice == true)
                    {
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.TimeSyncAcknowledge:
                    if (mDefine.IsPrimaryDevice == true)
                    {
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.ModelListAcknowledge:
                    if (mDefine.IsPrimaryDevice == true)
                    {
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.LightTemperatureAcknowledge:
                    if (mDefine.IsPrimaryDevice == true)
                    {
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.LightLevelAcknowledge:
                    if (mDefine.IsPrimaryDevice == true)
                    {
                        break;
                    }
                    return;
            }

            string[] modelNames;
            double[] values;
            ReceiveData.CalibrationMoveData calibrationMoveData;
            ReceiveData.CalibrationMoveHomeData calibrationMoveHomeData;
            ReceiveData.AlignResultData alignResultData;
            ReceiveData.AlignCompleteData alignCompleteData;
            // 요청 메시지 처리
            switch (receiveData.eReceiveData)
            {
                case CDeviceAlignInterface.EReceiveDataType.AlignCompleteRequest:
                    if (receiveData.TryParseAlignCompleteData(out alignCompleteData) == true)
                    {
                        mAlignCompleteAcknowledge = alignCompleteData;
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.AlignResultRequest:
                    if (receiveData.TryParseAlignResultData(out alignResultData) == true)
                    {
                        mAlignResultAcknowledge[alignResultData.InnerID] = alignResultData;
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.CalibrationStartRequest:
                    if (receiveData.TryParseCalibrationStartData() == true)
                    {
                        // 캘리브레이션 준비
                        // + 로봇 메뉴얼 모드 진입과 연결
                        // + 이동 불가 및 실패시 에러 발생 패킷 날림
                        RaiseReceiveCalibrationStartEvent();
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.CalibrationMoveHomeRequest:
                    if (receiveData.TryParseCalibrationMoveHomeData(out calibrationMoveHomeData) == true)
                    {
                        // 얼라인 위치로 이동
                        // + 티칭 위치 이동 기능과 연결
                        // + 이동 불가 및 실패시 에러 발생 패킷 날림
                        calibrationMoveData = new ReceiveData.CalibrationMoveData() { Index = calibrationMoveHomeData.Index };
                        RaiseReceiveCalibrationMoveEvent(calibrationMoveData);
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.CalibrationMoveOffsetRequest:
                    if (receiveData.TryParseCalibrationMoveData(out calibrationMoveData) == true)
                    {
                        // 얼라인 위치 기준 옵셋 위치로 이동
                        // + 티칭 위치 옵셋 이동 기능과 연결
                        // + 이동 불가 및 실패시 에러 발생 패킷 날림
                        RaiseReceiveCalibrationMoveEvent(calibrationMoveData);
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.CalibrationCompleteRequest:
                    if (receiveData.TryParseCalibrationCompleteData(out index) == true)
                    {
                        // 캘리브레이션 정리
                        // + 로봇 메뉴얼 모드 정리와 연결
                        RaiseReceiveCalibrationFinishEvent();
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.AlignErrorRequest:
                    if (mDefine.IsPrimaryDevice == true
                        && receiveData.TryParseAlignErrorData(out string errorIdOrNull) == true
                        )
                    {
                        CAlarmDefine.EAlarmList alarmCode;
                        switch (errorIdOrNull)
                        {
                            case "SYSTEM_NOT_RUNNING":
                                alarmCode = mDefine.AlarmReceiveVisionErrorSystemNotRunning;
                                break;

                            case "CAMERA":
                                alarmCode = mDefine.AlarmReceiveVisionErrorCamera;
                                break;

                            case "LIGHT_CONTROLLER":
                                alarmCode = mDefine.AlarmReceiveVisionErrorLightController;
                                break;

                            case "LIGHT_TEMP_OVER":
                                alarmCode = mDefine.AlarmReceiveVisionErrorLightTempOver;
                                break;

                            default:
                                alarmCode = mDefine.AlarmReceiveVisionError;
                                break;
                        }
                        m_objDocument.GetMainFrame().BeginInvoke(new Action(() => m_objDocument.SetAlarmEvent(alarmCode)));
                        mbReceiveVisionError = true;
                        RaiseReceiveAlignErrorEvent();
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.ModelListResultRequest:
                    if (mDefine.IsPrimaryDevice == true
                        && receiveData.TryParseModelListResultData(out modelNames) == true
                        )
                    {
                        mModelNames = new HashSet<string>(modelNames);
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.LightTemperatureResultRequest:
                    if (mDefine.IsPrimaryDevice == true
                        && receiveData.TryParseLightTemperatureResultData(out values) == true
                        )
                    {
                        mLightTemperatureValues = values;
                        break;
                    }
                    return;

                case CDeviceAlignInterface.EReceiveDataType.LightLevelResultRequest:
                    if (mDefine.IsPrimaryDevice == true
                        && receiveData.TryParseLightLevelResultData(out values) == true
                        )
                    {
                        mLightLevelValues = values;
                        break;
                    }
                    return;
            }

            SetProcessLog($"Receive({receiveData.eReceiveData}) -> {receiveData.strReceiveData}");
            // 패킷 처리 완료 플래그 업데이트
            AlignInterface.SetMessageProcessDone(receiveData);
        }

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                mDefine.LogType,
                string.Format("{0}({1}) -> {2}", callerMemberName, AlignIndex, logMessage)
                );
        }

        private void OnParsingError(object sender, string e)
        {
            if (sender is CDeviceAlignInterface.CReceiveData packet
                && packet.LogType == AlignInterface.LogType
                )
            {
                m_objDocument.SetUpdateLog(AlignInterface.LogType, e);
            }
        }

        private void ThreadProcess()
        {
            while (mbThreadExit == false)
            {
                Thread.Sleep(10);
                if (mCommand == ECommand.Idle)
                {
                    continue;
                }

                RaiseMccLogWhenJobsBegined();
                mStatus = EStatus.Unknown;
                if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    Thread.Sleep(CDefine.DEF_SIMULATION_SLEEP_TIME);
                }
                switch (mCommand)
                {
                    case ECommand.Align:
                        if (SetAlign() == true)
                        {
                            mStatus = EStatus.Align;
                        }
                        break;

                    case ECommand.CheckExistModel:
                        if (SetCheckExistModel() == true)
                        {
                            mStatus = EStatus.CheckExistModel;
                        }
                        break;

                    case ECommand.Stop:
                        mStatus = EStatus.Stop;
                        break;
                }

                if (mStatus == EStatus.Unknown)
                {
                    mStatus = EStatus.Error;
                    m_objDocument.SetAlarmEvent(m_objAlarmStructure);
                    //////////////////////////////////////////////////////////////////////////
                    /// <![CDATA[
                    ///  중알람 발생시 설비를 정지하지 않고 메시지 창을 띄워서
                    /// 사용자가 재시도, 무시, 정지 상황을 판단하여 동작을 선택하여
                    /// 처리하는 기능이나 실제로 현장에 적용된적은 없다
                    /// ]]>
                    int iReturn = (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP;
                    switch (iReturn)
                    {
                        case (int)CDefine.EErrorProcess.ERROR_PROCESS_RETRY:
                            continue;

                        case (int)CDefine.EErrorProcess.ERROR_PROCESS_CONTINUE:
                            switch (mCommand)
                            {
                                case ECommand.Align:
                                    mStatus = EStatus.Align;
                                    break;

                                case ECommand.CheckExistModel:
                                    mStatus = EStatus.CheckExistModel;
                                    break;
                            }
                            break;

                        case (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP:
                            mCommand = ECommand.Stop;
                            continue;
                    }
                    //////////////////////////////////////////////////////////////////////////
                }

                mCommand = ECommand.Idle;
                RaiseMccLogWhenJobsFinished();
            }
        }
    }
}

using HLDevice;
using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Svi;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class InShuttleInspect : CProcessAbstract
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            GrabStart,
            GrabEnd
        }

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            GrabStart,
            GrabEnd
        }

        public CDeviceSviInterface SubOpticsInspInterface { get; private set; }
        private const string ID = nameof(InShuttleInspect);
        private ECommand mCommand;
        private EStatus mStatus;
        //////////////////////////////////////////////////////////////////////////
        // 수신 데이터 저장
        private SendData.AmoGrabStartData mGrabStartRequest = null;
        private ReceiveData.AmoGrabStartData mGrabStartAcknowlege = null;
        private SendData.AmoGrabEndData mGrabEndRequest = null;
        private ReceiveData.AmoGrabEndData mGrabEndAcknowlege = null;
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

                // 검사 인터페이스 객체 연결 (주된 처리는 검사 스테이지 쪽에서 처리되고 이곳에서는 간이 검사 트리거용으로만 활용)
                SubOpticsInspInterface = m_objDocument.m_objProcessMain.m_objInspInterfaces[mDefine.InspIndex];
                SubOpticsInspInterface.AddCallbackFunction(InspInterface_OnReceiveData);
                // 쓰레드 생성 및 시작
                mThreadProcess = new Thread(ThreadProcess);
                mThreadProcess.Start();

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
            SubOpticsInspInterface.RemoveCallbackFunction(InspInterface_OnReceiveData);
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
                // ! 간이 검사기는 MCR 촬상 전에 진행되어 InnerID로 검사를 진행함
                //cellInformations[index].InnerID = cellInformations[index].CellID = cellDataHandler.GetInspectionKey();
                cellInformations[index].InnerID = cellDataHandler.GetInnerID();
                cellInformations[index].CellID = cellDataHandler.GetInspectionKey(bUseJobID: false);
                SetProcessLog(string.Format("CheckCellData -> Position[{0}] Ready -> Inner:{1}, Mcr:{2}, InspKey:{3})", index, cellInformations[index].InnerID, cellInformations[index].CellID, cellDataHandler.GetInspectionKey()));
            }

            mGrabStartRequest = new SendData.AmoGrabStartData()
            {
                PositionA = cellInformations[0],
                PositionB = cellInformations[1]
            };
            mGrabEndRequest = new SendData.AmoGrabEndData()
            {
                PositionA = cellInformations[0],
                PositionB = cellInformations[1]
            };
        }

        public void ResetGrabInformation()
        {
            mGrabStartRequest = null;
            mGrabStartAcknowlege = null;
            mGrabEndRequest = null;
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
            mGrabStartRequest = new SendData.AmoGrabStartData()
            {
                PositionA = cellInformations[0],
                PositionB = cellInformations[1]
            };
            mGrabEndRequest = new SendData.AmoGrabEndData()
            {
                PositionA = cellInformations[0],
                PositionB = cellInformations[1]
            };
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

        public bool IsPrimary()
        {
            if (mDefine.InspIndex != CDefine.EInspInterface.PC1)
            {
                return false;
            }
            return true;
        }

        private bool SetGrabStart()
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
                try
                {
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
                        )
                    {
                        SetProcessLog(string.Format("AlreadyReceiveAmoGrabStartData -> Skip -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3}", acknowlegeDataCapture.PositionA.InnerID, acknowlegeDataCapture.PositionA.CellID, acknowlegeDataCapture.PositionB.InnerID, acknowlegeDataCapture.PositionB.CellID));
                        bReturn = true;
                        break;
                    }

                    // 데이터 초기화
                    SetProcessLog(string.Format("ResetData"));
                    mGrabStartAcknowlege = null;

                    SetProcessLog(string.Format("SendAmoGrabStart"));
                    if (false == SubOpticsInspInterface.HLSendAmoGrabStart(mGrabStartRequest, EWait.ForReceive, Config.WaitTime.CommTimeout.InspReplyTimeout.ToMilliseconds(), 1))
                    {
                        SetProcessLog(string.Format("SendAmoGrabStart -> Error -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3}", mGrabStartRequest.PositionA.InnerID, mGrabStartRequest.PositionA.CellID, mGrabStartRequest.PositionB.InnerID, mGrabStartRequest.PositionB.CellID));
                        if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                        {
                            // AlarmMsg: 검사 스테이지 인터페이스 그랩 시작 Send 실패 하였습니다.
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAmoGrabStartSendFail;
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
                            )
                        {
                            SetProcessLog(string.Format("ReceiveDataValidation -> Error -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3}", mGrabStartAcknowlege.PositionA.InnerID, mGrabStartAcknowlege.PositionA.CellID, mGrabStartAcknowlege.PositionB.InnerID, mGrabStartAcknowlege.PositionB.CellID));
                            // AlarmMsg: 검사 스테이지 인터페이스 그랩 시작 ReceiveData가 정확하지 않습니다.
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAmoGrabStartValidationFail;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn);
            return bReturn;
        }

        private bool SetGrabEnd()
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
                SetProcessLog(string.Format("CheckCellData"));
                // 종료 메시지를 보냈는지 확인
                var requestDataCapture = mGrabEndRequest;
                var acknowlegeDataCapture = mGrabEndAcknowlege;
                if (
                    null != requestDataCapture
                    && null != acknowlegeDataCapture
                    && requestDataCapture.PositionA.InnerID == acknowlegeDataCapture.PositionA.InnerID
                    && requestDataCapture.PositionB.InnerID == acknowlegeDataCapture.PositionB.InnerID
                    && requestDataCapture.PositionA.CellID == acknowlegeDataCapture.PositionA.CellID
                    && requestDataCapture.PositionB.CellID == acknowlegeDataCapture.PositionB.CellID
                    )
                {
                    SetProcessLog(string.Format("AlreadyReceiveGrabEndData -> Skip -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3}", acknowlegeDataCapture.PositionA.InnerID, acknowlegeDataCapture.PositionA.CellID, acknowlegeDataCapture.PositionB.InnerID, acknowlegeDataCapture.PositionB.CellID));
                    bReturn = true;
                    break;
                }

                // 데이터 초기화
                SetProcessLog(string.Format("ResetData"));
                mGrabEndAcknowlege = null;

                SetProcessLog(string.Format("SendAmoGrabEnd"));
                if (false == SubOpticsInspInterface.HLSendAmoGrabEnd(mGrabEndRequest, EWait.ForReceive, Config.WaitTime.CommTimeout.InspGrabEndReplyTimeout.ToMilliseconds(), 1))
                {
                    SetProcessLog(string.Format("SendAmoGrabEnd -> Error -> InnerA:{0},McrA:{1},InnerB:{2},McrB{3}", mGrabEndRequest.PositionA.InnerID, mGrabEndRequest.PositionA.CellID, mGrabEndRequest.PositionB.InnerID, mGrabEndRequest.PositionB.CellID));
                    if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                    {
                        // AlarmMsg: 검사 스테이지 인터페이스 그랩 완료 Send 실패 하였습니다.
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAmoGrabEndSendFail;
                        break;
                    }
                }

                SetProcessLog(string.Format("ReceiveDataValidation"));
                if (CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP != m_objDocument.m_objConfig.GetInspOptionParameter(mDefine.InspectType).eInspectionMode)
                {
                    if (
                        mGrabEndRequest.PositionA.InnerID != mGrabEndAcknowlege.PositionA.InnerID
                        || mGrabEndRequest.PositionB.InnerID != mGrabEndAcknowlege.PositionB.InnerID
                        || mGrabEndRequest.PositionA.CellID != mGrabEndAcknowlege.PositionA.CellID
                        || mGrabEndRequest.PositionB.CellID != mGrabEndAcknowlege.PositionB.CellID
                        )
                    {
                        SetProcessLog(string.Format("ReceiveDataValidation -> Error -> InnerA:{0},McrA:{1},InnerB:{2},McrB:{3}", mGrabEndAcknowlege.PositionA.InnerID, mGrabEndAcknowlege.PositionA.CellID, mGrabEndAcknowlege.PositionB.InnerID, mGrabEndAcknowlege.PositionB.CellID));
                        // AlarmMsg: 검사 스테이지 인터페이스 그랩 완료 ReceiveData가 정확하지 않습니다.
                        m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAmoGrabEndValidationFail;
                        break;
                    }
                }

                bReturn = true;
            } while (false);

            SetProcessLog("[END] " + bReturn);
            return bReturn;
        }

        private void InspInterface_OnReceiveData(CDeviceSviInterface.CReceiveData receiveData)
        {
            ReceiveData.AmoGrabStartData grabStartData;
            ReceiveData.AmoGrabEndData grabEndData;
            bool bIsRequestHandled = true;
            bool bIsAcknowledgeHandled = true;

            // 응답 메시지 처리
            switch (receiveData.eReceiveData)
            {
                case CDeviceSviInterface.EReceiveDataType.AmoGrabStartAcknowledge:
                    if (receiveData.TryParseAmoGrabStartData(out grabStartData) == true)
                    {
                        mGrabStartAcknowlege = grabStartData;
                    }
                    // 비전 로그
                    SetProcessLog(string.Format("ReceiveAmoGrabStart -> {0}", receiveData.strReceiveData));
                    break;

                case CDeviceSviInterface.EReceiveDataType.AmoGrabEndAcknowledge:
                    if (receiveData.TryParseAmoGrabEndData(out grabEndData) == true)
                    {
                        mGrabEndAcknowlege = grabEndData;
                    }
                    SetProcessLog(string.Format("ReceiveAmoGrabEnd -> {0}", receiveData.strReceiveData));
                    break;

                default:
                    bIsAcknowledgeHandled = false;
                    break;
            }

            // 요청 메시지 처리
            switch (receiveData.eReceiveData)
            {
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
            SubOpticsInspInterface.SetMessageProcessDone(receiveData);
        }

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                mDefine.LogType,
                string.Format("{0}({2}) -> {1}", callerMemberName, logMessage, mDefine.InspIndex)
                );
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
                if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
                {
                    Thread.Sleep(CDefine.DEF_SIMULATION_SLEEP_TIME);
                }
                switch (mCommand)
                {

                    case ECommand.GrabStart:
                        if (true == SetGrabStart())
                        {
                            mStatus = EStatus.GrabStart;
                        }
                        break;

                    case ECommand.GrabEnd:
                        if (true == SetGrabEnd())
                        {
                            mStatus = EStatus.GrabEnd;
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
                            if (ECommand.GrabStart == mCommand)
                            {
                                mStatus = EStatus.GrabStart;
                            }
                            else if (ECommand.GrabEnd == mCommand)
                            {
                                mStatus = EStatus.GrabEnd;
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
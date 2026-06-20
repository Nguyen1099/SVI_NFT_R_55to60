using HLDevice;
using SVI_NFT_R.CellData;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class InRobotMcr : CProcessAbstract
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            GrabOneshot,
            GrabSequence
        };
        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            GrabOneshot,
            GrabSequence
        };
        public CDefine.EMcr McrIndex => mDefine.McrIndex;
        public CellDataHandler CellPort => mDefine.CellPort;
        public string GrabResult { get; private set; }
        public CDeviceMCR MCR { get; private set; }
        private const string ID = nameof(InRobotMcr);
        private ECommand mCommand;
        private EStatus mStatus;
        private int mMcrAlarmCount;
        public override string ToString() => $"{ID},{McrIndex},{mCommand},{mStatus}";

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

                MCR = m_objDocument.m_objProcessMain.m_objMcr[mDefine.McrIndex];

                // 알람 발생 카운트 초기화
                mMcrAlarmCount = 0;
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
        }

        private bool SetMcrGrabSequence()
        {
            if (CellPort.Data.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK)
            {
                return true;
            }

            bool bReturn = false;
            string strData = "";
            int iMCRRetryCount = 0;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            SetProcessLog("[START]");

            do
            {
                if (
                    CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                    && CDefine.ERunMode.RealRun == m_objDocument.GetRunMode()
                    //&& CDialogEQPFunctionList.EFSTUse.USE.ToString() == m_objDocument.m_objConfig.GetCIMParameter().strEFValue[ ( int )CDialogEQPFunctionList.EFName.EF_NAME_CELL_MCR_MODE ]
                    && true == m_objDocument.m_objConfig.GetOptionParameter().bUseAutoMCR
                    // #MFQ_MCR_SKIP
                    // 일단 MFQ에서는 MCR을 찍지 않는 걸로 한다
                    //&& false == m_objDocument.m_bUseMfqMode
                    )
                {
                    do
                    {
                        // MCR 사용 유무 판단  
                        // MCC
                        //m_objDocument.m_objMCC.SetMCCStart(CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    CMCC.enumActionNameDI.DI01_MCR_READING + (int)m_ePosition,
                        //    CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    m_objDocument.m_objStructure.m_objProcessData[(int)CStructureInfoData.enumProcess.PROCESS_NACHI_IN_ROBOT].objPosition[(int)m_ePosition].objCell.strCellID);

                        SetProcessLog("SetTrigger TryNo: " + iMCRRetryCount.ToString());
                        // MCR 번호.
                        CellPort.Data.Reader.ReaderID = string.Format("{0}", (int)McrIndex + 1);
                        // MCR 그랩.
                        MCR.HLTrigger();

                        // MCC
                        //m_objDocument.m_objMCC.SetMCCEnd(CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    CMCC.enumActionNameDI.DI01_MCR_READING + (int)m_ePosition,
                        //    CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    m_objDocument.m_objStructure.m_objProcessData[(int)CStructureInfoData.enumProcess.PROCESS_NACHI_IN_ROBOT].objPosition[(int)m_ePosition].objCell.strCellID);

                        // MCC
                        //m_objDocument.m_objMCC.SetMCCStart(CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    CMCC.enumActionNameDI.DI01_RESULT_WAIT,
                        //    CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    m_objDocument.m_objStructure.m_objProcessData[(int)CStructureInfoData.enumProcess.PROCESS_NACHI_IN_ROBOT].objPosition[(int)m_ePosition].objCell.strCellID);

                        SetProcessLog("WaitForMcrData");
                        MCR.HLReadData(ref strData);

                        // MCC
                        //m_objDocument.m_objMCC.SetMCCEnd(CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    CMCC.enumActionNameDI.DI01_RESULT_WAIT,
                        //    CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    CMCC.enumActionModuleID.DI01 + (int)m_ePosition,
                        //    m_objDocument.m_objStructure.m_objProcessData[(int)CStructureInfoData.enumProcess.PROCESS_NACHI_IN_ROBOT].objPosition[(int)m_ePosition].objCell.strCellID);

                        // MCR 로그
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_DEVICE_MCR, strData);

                        // NG ( 검색 결과 No Read 일 경우 or 공백 일 경우 )
                        if (0 <= strData.ToUpper().IndexOf("NO READ") || 0 == strData.Length)
                        {
                            SetProcessLog("WaitForMcrData -> Error");
                            // MCR 리딩 실패. 
                            CellPort.Data.Cell.CellID = string.Format("MCR{1}_NG_{0:yyyyMMddHHmmssfff}", DateTime.Now, McrIndex);
                            CellPort.Data.Reader.ReaderResultCode = CCIMDefine.ReaderResultCode.ERROR;
                            CellPort.Data.Reader.RetryCount = iMCRRetryCount;

                            // 재시도 횟수가 남았으면 딜레이를 준다
                            if (
                                true == m_objDocument.m_objConfig.GetOptionParameter().bUseMCRRetry
                                && 0 != iMCRRetryCount
                                && iMCRRetryCount < m_objDocument.m_objConfig.GetOptionParameter().iMCRRetryCount
                                )
                            {
                                Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            // OK
                            CellPort.Data.Cell.CellID = strData;
                            CellPort.Data.Reader.ReaderResultCode = CCIMDefine.ReaderResultCode.OK;
                            CellPort.Data.Reader.RetryCount = iMCRRetryCount;
                        }
                        iMCRRetryCount++;
                    } while (
                        true == m_objDocument.m_objConfig.GetOptionParameter().bUseMCRRetry
                        && iMCRRetryCount <= m_objDocument.m_objConfig.GetOptionParameter().iMCRRetryCount
                        && CCIMDefine.ReaderResultCode.OK != CellPort.Data.Reader.ReaderResultCode
                        );

                    // 최종 리트라이 실패시 경알람 발생
                    if (CCIMDefine.ReaderResultCode.ERROR == CellPort.Data.Reader.ReaderResultCode)
                    {
                        m_objDocument.SetAlarmEvent((0 == strData.Length) ? mDefine.AlarmResultTimeout : mDefine.AlarmGrabFail);
                    }

                    // MCR 중알람 발생 확인
                    if (
                        true == m_objDocument.m_objConfig.GetOptionParameter().bUseMCRHeavyAlarm
                        && CCIMDefine.ReaderResultCode.OK != CellPort.Data.Reader.ReaderResultCode
                        )
                    {
                        // MCR 실패 카운트 증가
                        mMcrAlarmCount++;

                        // MCR 실패 카운트가 설정된 알람 발생 카운트 횟수와 비교 해서 같거나 클 경우 알람 발생 한다.
                        if (mMcrAlarmCount >= m_objDocument.m_objConfig.GetOptionParameter().iMCRAlarmCount)
                        {
                            // MCR 실패 카운트 초기화
                            mMcrAlarmCount = 0;
                            // 중알람 발생
                            // MCR 리딩 실패 카운트가 설정된 알람 발생 카운트를 초과하였습니다.
                            m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmGrabSuccessivelyFail;
                            break;
                        }
                        // 리딩 성공 시 실패 카운트 초기화 ( 연속 적으로 MCR 리딩이 실패 했을 경우에만 카운트 한다 )    
                    }
                    else
                    {
                        // MCR 실패 카운트 초기화
                        mMcrAlarmCount = 0;
                    }
                }
                // 시뮬레이션 이거나 MCR 미사용 모드면 무조건 OK 처리함   
                else
                {
                    // #CIM_TEST_INPUT_MCR_POINT
                    //if (m_ePosition == CDefine.EPosition.POSITION_A)
                    //{
                    //    m_objDocument.m_objStructure.m_objProcessData[(int)CStructureInfoData.EProcess.PROCESS_NACHI_IN_ROBOT].objPosition[(int)m_ePosition].objCell.strCellID = "MCRA";
                    //}
                    //else
                    //{
                    //    m_objDocument.m_objStructure.m_objProcessData[(int)CStructureInfoData.EProcess.PROCESS_NACHI_IN_ROBOT].objPosition[(int)m_ePosition].objCell.strCellID = "MCRB";
                    //}
                    CellPort.Data.Cell.CellID = string.Format("MCR{1}_{0:yyyyMMddHHmmssfff}", DateTime.Now, McrIndex);
                    CellPort.Data.Reader.ReaderResultCode = CCIMDefine.ReaderResultCode.OK;
                    CellPort.Data.Reader.RetryCount = iMCRRetryCount;
                    // MCR SIMULATION DELAY
                    Thread.Sleep(300);
                }
                bReturn = true;

            } while (false);

            CellPort.IsChanged = true;
            GrabResult = strData;
            SetProcessLog("[END] " + bReturn);
            return bReturn;
        }

        private bool SetMcrGrabOneshot()
        {
            bool bReturn = false;
            string strData = "";
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            GrabResult = "";

            do
            {
                MCR.HLTrigger();
                Thread.Sleep(100);
                MCR.HLReadData(ref strData);

                bReturn = true;
            } while (false);

            GrabResult = strData;
            return bReturn;
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

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                mDefine.LogType,
                string.Format("{0}({1}) -> {2}", callerMemberName, McrIndex, logMessage)
                );
        }

        public override bool WaitForEndProcess()
        {
            bool bResult = false;

            do
            {
                while (
                    ECommand.Idle != GetCommand()
                    || CDefine.ERunStatus.Pause == m_objDocument.GetRunStatus()
                    )
                {
                    Thread.Sleep(WAIT_FOR_END_PROCESS_PERIOD_TIME);
                }
                if (EStatus.Stop == GetStatus())
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        private void ThreadProcess()
        {
            while (false == mbThreadExit)
            {
                if (ECommand.Idle != mCommand)
                {
                    RaiseMccLogWhenJobsBegined();
                    mStatus = EStatus.Unknown;
                    if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                    {
                        Thread.Sleep(CDefine.DEF_SIMULATION_SLEEP_TIME);
                    }
                    switch (mCommand)
                    {
                        case ECommand.GrabOneshot:
                            if (true == SetMcrGrabOneshot())
                            {
                                mStatus = EStatus.GrabOneshot;
                            }
                            break;
                        case ECommand.GrabSequence:
                            if (true == SetMcrGrabSequence())
                            {
                                mStatus = EStatus.GrabSequence;
                            }
                            break;
                    }

                    if (ECommand.Stop == mCommand)
                    {
                        // 사용자 정지 체크
                        mStatus = EStatus.Stop;
                    }
                    else
                    {
                        // 날라온 Command에 대한 알람이 있었는지 체크. STS_UNKNOWN 그대로이면 알람.
                        if (EStatus.Unknown == mStatus)
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
                                    if (ECommand.GrabSequence == mCommand)
                                    {
                                        mStatus = EStatus.GrabSequence;
                                    }
                                    break;
                                // 에러에 대한 설비 정지
                                case (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP:
                                    mCommand = ECommand.Stop;
                                    continue;
                            }
                        }
                    }
                    RaiseMccLogWhenJobsFinished();
                    mCommand = ECommand.Idle;
                }

                Thread.Sleep(10);
            }
        }
    }
}

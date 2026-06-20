using SVI_NFT_R.CellData;
using SVI_NFT_R.DEVICE.Align;
using SVI_NFT_R.DEVICE.Nachi;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class InRobotSendAlign : CProcessAbstract
    {
        public enum ECommand
        {
            Idle = 0,
            Stop,

            AlignDataSend,
            AlignZeroDataSend,
            CalibrationDataSend,
        };

        public enum EStatus
        {
            Unknown = 0,
            Error,
            Stop,

            AlignDataSend,
            AlignZeroDataSend,
            CalibrationDataSend,
        };

        public CDeviceNachi Robot { get; private set; }
        public NachiRobotAlignData AlignData { get; private set; } = new NachiRobotAlignData();
        private const string ID = nameof(InRobotSendAlign);
        private ECommand mCommand;
        private EStatus mStatus;
        public override string ToString() => $"{ID},,{mCommand},{mStatus}";

        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmObject = ID;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                m_objDocument = document;
                mDefine = new Define(position);

                Robot = m_objDocument.m_objProcessMain.m_objProcessMotion.InRobot.Nachi.Robot;

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

        public void SetAlignDataFromCellData(IReadOnlyList<CellDataHandler> cellContainer)
        {
            SetAlignDataReset();

            foreach (var cellDataHandler in cellContainer.GetExistCellList())
            {
                var alignResultData = cellDataHandler.Data.PreAlignResult;
                var alignData = new NachiRobotAlignData.OffsetData()
                {
                    X = alignResultData.TotalRevisionX,
                    Y = alignResultData.TotalRevisionY,
                    T = alignResultData.TotalRevisionT,
                    CameraIndex = alignResultData.CameraIndex,
                    Score = alignResultData.Score
                };
                switch (cellDataHandler.PositionIndex)
                {
                    case 0:
                        AlignData.AlignTool1 = alignData;
                        break;

                    case 1:
                        AlignData.AlignTool2 = alignData;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        }

        public void SetAlignDataFromAlignResult(ETool toolIndex, AlignResult alignResult)
        {
            SetAlignDataReset();

            switch (toolIndex)
            {
                case ETool.Tool1:
                    AlignData.AlignTool1.X = alignResult.X;
                    AlignData.AlignTool1.Y = alignResult.Y;
                    AlignData.AlignTool1.T = alignResult.T;
                    AlignData.AlignTool1.CameraIndex = ECellPlacement.P1;
                    AlignData.AlignTool1.Score = alignResult.Score;
                    break;

                case ETool.Tool2:
                    AlignData.AlignTool2.X = alignResult.X;
                    AlignData.AlignTool2.Y = alignResult.Y;
                    AlignData.AlignTool2.T = alignResult.T;
                    AlignData.AlignTool2.CameraIndex = ECellPlacement.P2;
                    AlignData.AlignTool2.Score = alignResult.Score;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }

        public void SetAlignDataReset()
        {
            AlignData = new NachiRobotAlignData();
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
                    Thread.Sleep(10);
                }
                if (EStatus.Stop == GetStatus())
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        private bool SetAlignDataSend()
        {
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            NachiRobotAlignData alignData = AlignData.DeepClone();

            if (Robot.SendAlignData(alignData) == false)
            {
                m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAlignDataSendFail;
                return false;
            }
            SetProcessLog($"\n\t\t\tT1 X:{alignData.AlignTool1.X} ( mm )\tT1 Y:{alignData.AlignTool1.Y} ( mm )\tT1 T:{alignData.AlignTool1.T} ( º )\n\t\t\tT2 X:{alignData.AlignTool2.X} ( mm )\tT2 Y:{alignData.AlignTool2.Y} ( mm )\tT2 T:{alignData.AlignTool2.T} ( º )");
            return true;
        }

        private bool SetAlignZeroDataSend()
        {
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            NachiRobotAlignData alignData = new NachiRobotAlignData();
            AlignData = alignData;

            if (Robot.SendAlignData(alignData) == false)
            {
                m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmAlignZeroDataSendFail;
                return false;
            }
            SetProcessLog($"\n\t\t\tT1 X:{alignData.AlignTool1.X} ( mm )\tT1 Y:{alignData.AlignTool1.Y} ( mm )\tT1 T:{alignData.AlignTool1.T} ( º )\n\t\t\tT2 X:{alignData.AlignTool2.X} ( mm )\tT2 Y:{alignData.AlignTool2.Y} ( mm )\tT2 T:{alignData.AlignTool2.T} ( º )");

            return true;
        }

        private bool SetCalibrationDataSend()
        {
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            NachiRobotAlignData alignData = AlignData.DeepClone();

            if (Robot.SendCalibrationData(alignData) == false)
            {
                m_objAlarmStructure.iAlarmCode = (int)mDefine.AlarmCalibrationDataSendFail;
                return false;
            }
            SetProcessLog($"\n\t\t\tT1 X:{alignData.AlignTool1.X} ( mm )\tT1 Y:{alignData.AlignTool1.Y} ( mm )\tT1 T:{alignData.AlignTool1.T} ( º )\n\t\t\tT2 X:{alignData.AlignTool2.X} ( mm )\tT2 Y:{alignData.AlignTool2.Y} ( mm )\tT2 T:{alignData.AlignTool2.T} ( º )");

            return true;
        }

        private void SetProcessLog(string logMessage, [CallerMemberName] string callerMemberName = "")
        {
            m_objDocument.SetUpdateLog(
                mDefine.LogType,
                string.Format("{0} -> {1}", callerMemberName, logMessage)
                );
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
                        case ECommand.AlignDataSend:
                            if (true == SetAlignDataSend())
                                mStatus = EStatus.AlignDataSend;
                            break;

                        case ECommand.AlignZeroDataSend:
                            if (true == SetAlignZeroDataSend())
                                mStatus = EStatus.AlignZeroDataSend;
                            break;

                        case ECommand.CalibrationDataSend:
                            if (true == SetCalibrationDataSend())
                                mStatus = EStatus.CalibrationDataSend;
                            break;

                        case ECommand.Idle:
                        case ECommand.Stop:
                            break;

                        default:
                            Debug.Assert(false);
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
                                    if (ECommand.AlignDataSend == mCommand)
                                        mStatus = EStatus.AlignDataSend;
                                    if (ECommand.AlignZeroDataSend == mCommand)
                                        mStatus = EStatus.AlignZeroDataSend;
                                    if (ECommand.CalibrationDataSend == mCommand)
                                        mStatus = EStatus.CalibrationDataSend;
                                    break;
                                // 에러에 대한 설비 정지
                                case (int)CDefine.EErrorProcess.ERROR_PROCESS_STOP:
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
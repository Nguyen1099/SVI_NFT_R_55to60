using HLDevice;
using SVI_NFT_R.DEVICE.Nachi;
using SVI_NFT_R.DEVICE.Nachi.SignalNames;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace SVI_NFT_R
{
    public partial class CDeviceNachi
    {
        public bool IsInitialized { get; private set; } = false;
        public SignalController Signals { get; private set; }
        public RobotStatus Status { get; private set; }
        public string RobotPrefix { get; private set; }
        public CDeviceNachiInterface OffsetInterface { get; private set; }
        public CDeviceNachiInterface RmsInterface { get; private set; }
        public CDeviceNachiInterface AlignInterface { get; private set; }
        public IReadOnlyList<ERobotProcess> AllRobotProcess { get; private set; }
        public RmsData ReceiveDataRms => mReceiveDataRms;
        public event EventHandler<RmsDataChangedEventArgs> RmsDataChanged;
        public bool IsNeedSendOffsetData { get; set; } = true;
        public bool IsNeedReceiveRmsData { get; set; } = true;
        private CDocument mDocument;
        private CDefine.ELogType mLogTarget;
        private readonly TimeSpan mPulseWidthTime = TimeSpan.FromMilliseconds(500);
        private TimeSpan mRobotTimeout => Config.WaitTime.CommTimeout.RobotTimeout.ToTimeSpan();
        private TimeSpan mInitialTimeout => Config.WaitTime.CommTimeout.RobotTimeout.ToTimeSpan() >= TimeSpan.FromSeconds(20d) ? Config.WaitTime.CommTimeout.RobotTimeout.ToTimeSpan() : TimeSpan.FromSeconds(20d);
        private bool mbReceivedDataRms = false;
        private List<byte> mRecieveRmsData;
        private RmsData mReceiveDataRms = new RmsData();
        private const string BACKUP_RMS_RECIEVE_DATA_PATH = @".\cache\";
        private string mBackupRmsReceiveDataFileName = string.Empty;
        private FileStream mBackupRmsFileStream;
        private bool mbIsVirtual;

        public bool Initialize(CDocument document, RobotInitialOption option)
        {
            if (true == IsInitialized)
            {
                return false;
            }
            mbIsVirtual = option.IsVirtual;
            mDocument = document;
            RobotPrefix = option.RobotPrefix;
            OffsetInterface = option.OffsetInterface;
            OffsetInterface.OnEventOccured += onEventOccured;
            RmsInterface = option.RmsInterfaceOrNull;
            if (null != RmsInterface)
            {
                RmsInterface.OnEventOccured += onEventOccured;
                RmsInterface.SetCallbackFunction(onReceiveDataFromRmsInterface);
            }
            AlignInterface = option.AlignInterfaceOrNull;
            if (null != AlignInterface)
            {
                AlignInterface.OnEventOccured += onEventOccured;
            }
            mLogTarget = option.LogTarget;
            AllRobotProcess = option.AllRobotProcess;
            Signals = new SignalController(mDocument, RobotPrefix, option.IsVirtual);
            Signals.OnEventOccured += onEventOccured;
            Status = new RobotStatus(Signals);
            mRecieveRmsData = new List<byte>();

            mBackupRmsReceiveDataFileName = $"({option.RobotPrefix})RobotLastReceiveRmsData.bak";
            mBackupRmsFileStream = File.Open(Path.Combine(BACKUP_RMS_RECIEVE_DATA_PATH, mBackupRmsReceiveDataFileName), FileMode.OpenOrCreate);
            restoreRmsDataFromFile();

            IsInitialized = true;
            return true;
        }

        public void DeInitialize()
        {
            if (false == IsInitialized)
            {
                return;
            }

            OffsetInterface.OnEventOccured -= onEventOccured;
            if (null != AlignInterface)
            {
                AlignInterface.OnEventOccured -= onEventOccured;
            }
            if (null != RmsInterface)
            {
                RmsInterface.OnEventOccured -= onEventOccured;
                RmsInterface.SetCallbackFunction(null);
            }
            Signals.OnEventOccured -= onEventOccured;

            mBackupRmsFileStream.Dispose();
            mBackupRmsFileStream = null;

            IsInitialized = false;
        }

        public bool SetRobotInitial(CConfig.RobotModelParameter initializeParameter, bool bSendOffsetData, bool bReceiveRmsData)
        {
            JcmExitMode();
            onEventOccured(this, $"\t[BEGIN] {nameof(SetRobotInitial)}");
            // [모든 출력 신호 리셋]
            {
                // [Sequence Signals Clear]
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P1].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P2].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P3].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P4].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P5].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P6].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P7].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P8].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P9].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P10].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P11].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P12].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P13].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P14].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P15].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P16].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P17].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P18].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P19].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P20].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P1_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P2_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P3_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P4_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P5_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P6_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P7_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P8_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P9_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P10_PERMIT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P1_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P2_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P3_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P4_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P5_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P6_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P7_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P8_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P9_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.P10_VISION_USE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL_1_VACUUM_ON_DO].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL_1_VACUUM_OFF_DO].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL_2_VACUUM_ON_DO].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL_2_VACUUM_OFF_DO].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL_3_VACUUM_ON_DO].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL_3_VACUUM_OFF_DO].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL_4_VACUUM_ON_DO].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL_4_VACUUM_OFF_DO].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.GET_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.PUT_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL1_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL2_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL12_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL3_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL4_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.TOOL34_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STAGE1_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STAGE2_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STAGE3_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STAGE4_SELECT].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.T1_ALIGN_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.T2_ALIGN_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.T3_ALIGN_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.T4_ALIGN_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.T1_MCR_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.T2_MCR_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.T3_MCR_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.T4_MCR_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STG1_VAC_ON_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STG1_VAC_OFF_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STG2_VAC_ON_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STG2_VAC_OFF_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STG3_VAC_ON_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STG3_VAC_OFF_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STG4_VAC_ON_ACK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.STG4_VAC_OFF_ACK].Value = CDefine.OFF;

                // [Control Signals Clear]
                Signals.Y.BB[Bit.EOutput.EXTERNAL_PLAY_START_U1].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.PLAY_STOP_U1].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.FAILURE_RESET].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.MOTOR_ON_EXTERNAL].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.MOTOR_OFF_EXTERNAL].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.REDUCE_SPEED].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.PAUSE].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.EXTERNAL_RESET].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.ORIGIN_RETURN].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.CYCLE_STOP].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.AUTO_RETRY_REQUEST].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.SET_STEP_REQUEST_CLEAR].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.RECIPE_OK].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.JCM_MODE_REQ].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.JCM_PROCESS_EXIT_REQ].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.JCM_CHECK_TEACHING_GO_REQ].Value = CDefine.OFF;
                Signals.Y.BG[Bit.EOutputGroup.JCM_CHECK_TEACHING_REQ].Value = (int)EJcmCommand.None;
                Signals.Y.BB[Bit.EOutput.JCM_SAVE_REQ].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.JCM_NO_SAVE_REQ].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.PROCESS_EXIT_REQ].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.PROCESS_SKIP_REQ].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.PROCESS_ERROR_REQ].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.COMPLETE_RETURN].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.ERROR_CODE_RETURN].Value = CDefine.OFF;
                Signals.Y.WG[Word.EOutputGroup.RECIPE_NO_BITS].Value = 0;
                Signals.Y.BG[Bit.EOutputGroup.PROGRAM_SELECT_BIT_U1].Value = 0;
                Signals.Y.WB[Word.EOutput.U4_RECEIVE_REQ1].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.U5_SEND_REQ1].Value = CDefine.OFF;
                Signals.Y.WB[Word.EOutput.INTERFERENCE_REGION1_PERMIT].Value = CDefine.ON;
                Signals.Y.WB[Word.EOutput.INTERFERENCE_REGION2_PERMIT].Value = CDefine.ON;
                Signals.Y.WB[Word.EOutput.INTERFERENCE_REGION3_PERMIT].Value = CDefine.ON;
                // PULSE 타입으로 동작하는 신호가 켜진 상태에서 진행하면 로봇이 OFF를 인지 하기 전에 ON이 돼서 초기화가 진행 안되는 버그가 있어서 딜레이 추가함 (EXTERNAL_RESET)
                Thread.Sleep(100);
            }

            // [구동 ON]
            {
                // (I_002) 'Ext.unit play stop U1'    (On)    (PC ───▷ ROBOT)    외부에서 로봇을 멈추기위해 사용하는 신호
                Signals.Y.BB[Bit.EOutput.PLAY_STOP_U1].Value = CDefine.ON;
                // (I_007) 'Pause'                    (On)    (PC ───▷ ROBOT)    재생중인 로봇(기동중로봇)프로그램을 일시정지시키는 신호
                Signals.Y.BB[Bit.EOutput.PAUSE].Value = CDefine.ON;
                // (I_012) 'Program select bits U1 1' (On)    (PC ───▷ ROBOT)    메인프로그램 선택 bit
                Signals.Y.BG[Bit.EOutputGroup.PROGRAM_SELECT_BIT_U1].Value = 1;
                // ! 처음 Initial 동작 이후 지령없이 다시 Initial 할 경우 (I_008) 'External reset' 선입력 방지를 위한 딜레이 추가
                Thread.Sleep(500);
                // (I_008) 'External reset'           (On)    (PC ───▷ ROBOT)    로봇이상해제 신호
                Signals.Y.BB[Bit.EOutput.EXTERNAL_RESET].Value = CDefine.ON;
                Signals.Y.BB[Bit.EOutput.SET_STEP_REQUEST_CLEAR].Value = CDefine.ON;
                // (O_013) 'Ext.rest ackno.'          (On)    (PC ◁─── ROBOT)    0.2초후에 꺼짐
                // ! 리셋 응답 신호가 0.2초 펄스 타입이라서 간헐적으로 신호를 놓치는 현상이 발생하여 500ms 고정 딜레이로 대체함
                Thread.Sleep(500);
                // (I_008) 'External reset'           (Off)   (PC ───▷ ROBOT)    로봇이상해제 신호
                Signals.Y.BB[Bit.EOutput.EXTERNAL_RESET].Value = CDefine.OFF;
                Signals.Y.BB[Bit.EOutput.SET_STEP_REQUEST_CLEAR].Value = CDefine.OFF;
                // 마그넷 스위치 이상 에러 방지용
                Thread.Sleep(500);
                // (I_004) 'Motors ON external'       (On)    (PC ───▷ ROBOT)    움직일수있게하는 모터가 켜진다.
                Signals.Y.BB[Bit.EOutput.MOTOR_ON_EXTERNAL].Value = CDefine.ON;
                // (O_006) 'Motors energized'         (On)    (PC ◁─── ROBOT)    외부기기기에서 운전준비를 ON하기위한 신호
                if (Signals.X.BB[Bit.EInput.MOTOR_ENERGIZED].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
                {
                    return false;
                }
                // (I_004) 'Motors ON external'       (Off)   (PC ───▷ ROBOT)    움직일수있게하는 모터가 꺼진다.
                Signals.Y.BB[Bit.EOutput.MOTOR_ON_EXTERNAL].Value = CDefine.OFF;
                // [Delay 0.2s ~ 1s]
                Thread.Sleep(1000);
                // (I_001) 'Ext.play start U1'        (On)    (PC ───▷ ROBOT)    외부컨트롤러로 로봇을 움직이게하기 위한 신호
                Signals.Y.BB[Bit.EOutput.EXTERNAL_PLAY_START_U1].Value = CDefine.ON;
                // (O_003) 'Robot running U1'         (On)    (PC ◁─── ROBOT)    오토상태
                if (Signals.X.BB[Bit.EInput.RUNNING_U1].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
                {
                    return false;
                }
                // (O_004) 'Under stopping'           (Off)   (PC ◁─── ROBOT)    멈춤
                if (Signals.X.BB[Bit.EInput.UNDER_STOPPING].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false)
                {
                    return false;
                }
                // (I_001) 'Ext.play start U1'        (Off)   (PC ───▷ ROBOT)    기동중 상태, 신호 대기 中
                Signals.Y.BB[Bit.EOutput.EXTERNAL_PLAY_START_U1].Value = CDefine.OFF;
            }

            // [Origin Return OR Initialization]
            {
                // (O_153) 'Current recipe bits 1'    (Off)   (PC ◁─── ROBOT)    Recipe 출력신호 초기화
                // ~
                // (O_160) 'Current recipe bits 128'  (Off)   (PC ◁─── ROBOT)
                if (Signals.X.WG[Word.EInputGroup.CURRENT_RECIPE_BITS].WaitForTargetValue(0, mRobotTimeout, CheckCancel) == false)
                {
                    return false;
                }
                // (I_153) 'RecipeNo bits 1'          (On)    (PC ───▷ ROBOT)    Recipe 선택비트 신호
                // ~
                // (I_160) 'RecipeNo bits 128'        (On)    (PC ───▷ ROBOT)
                Signals.Y.WG[Word.EOutputGroup.RECIPE_NO_BITS].Value = initializeParameter.RecipeNo;
                // (O_153) 'Current recipe bits 1'    (On)    (PC ◁─── ROBOT)    Recipe 출력신호
                // ~
                // (O_160) 'Current recipe bits 128'  (On)    (PC ◁─── ROBOT)
                if (Signals.X.WG[Word.EInputGroup.CURRENT_RECIPE_BITS].WaitForTargetValue(initializeParameter.RecipeNo, mRobotTimeout, CheckCancel) == false)
                {
                    return false;
                }
                mbReceivedDataRms = false;
                mRecieveRmsData.Clear();
                // (I_188) 'U4_Receive1'              (On)    (PC ───▷ ROBOT)    Socket 통신 Receive(Offset)
                Signals.Y.WB[Word.EOutput.U4_RECEIVE_REQ1].Value = bSendOffsetData == true;
                // (I_185) 'U5_Send1'                 (On)    (PC ───▷ ROBOT)    Socket 통신 Send(RMS)
                Signals.Y.WB[Word.EOutput.U5_SEND_REQ1].Value = bReceiveRmsData == true;
                // (I_011) 'recipe change ok'         (On)    (PC ───▷ ROBOT)    Recipe 번호 체크 완료 신호
                Signals.Y.BB[Bit.EOutput.RECIPE_OK].OnePulseAsync(CDefine.ON, mPulseWidthTime);
                // [로봇 홈 위치 이동중]
                // (O_010) 'Home position U1 1'       (On)    (PC ◁─── ROBOT)    로봇 홈 위치 도착
                if (Signals.X.BB[Bit.EInput.HOME_POSITION_U1_1].WaitForTargetValue(CDefine.ON, mInitialTimeout, CheckCancel) == false)
                {
                    return false;
                }
                // [옵셋 데이터 전송]
                bool bIsSendOffsetFailed = false;
                if (bSendOffsetData == true)
                {
                    // (O_187) 'U4_Running'               (On)    (PC ◁─── ROBOT)    Socket 통신 Receive(Offset) 시작
                    // (O_188) 'U4_Receive_Comp1'         (On)    (PC ◁─── ROBOT)    Offset Data Receive 대기중
                    // [Offset Data 완료]
                    // (O_187) 'U4_Running'               (Off)   (PC ◁─── ROBOT)    Socket 통신 Receive(Offset) 종료
                    // (O_188) 'U4_Receive_Comp1'         (Off)   (PC ◁─── ROBOT)    Offset Data Receive 종료
                    int[] alignOffsetData = initializeParameter.OffsetData.Values.Select(offset => Convert.ToInt32(offset * 1000d)).ToArray();
                    bIsSendOffsetFailed = offsetDataSend(alignOffsetData) == false;
                    Signals.X.WB[Word.EInput.U4_RUNNING].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel);
                }
                // [RMS 데이터 수신]
                bool bIsReceiveRmsFailed = false;
                if (bReceiveRmsData == true)
                {
                    // (O_184) 'U5_Running'               (On)    (PC ◁─── ROBOT)    Socket 통신 Send(RMS) 시작
                    // (O_185) 'U5_Send_Comp1'            (On)    (PC ◁─── ROBOT)    Robot Position Data(RMS) Send 대기중
                    // [Robot Position Data(RMS) 완료]
                    // (O_184) 'U5_Running'               (Off)   (PC ◁─── ROBOT)    Socket 통신 Send(RMS) 종료
                    // (O_185) 'U5_Send_Comp1'            (Off)   (PC ◁─── ROBOT)    Robot Position Data(RMS) Send 종료
                    bIsReceiveRmsFailed = rmsDataReceive() == false;
                    Signals.X.WB[Word.EInput.U5_RUNNING].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel);
                }
                // [소켓 통신 에러 체크]
                var captureErrorCode = Status.CurrentErrorCode;
                if (captureErrorCode != 0)
                {
                    onEventOccured(this, $"[ERROR] {captureErrorCode}");
                    Signals.Y.WB[Word.EOutput.ERROR_CODE_RETURN].OnePulseAsync(CDefine.ON, mPulseWidthTime);
                }
                if (captureErrorCode == EErrorCode.U4CommnunicationError
                    || captureErrorCode == EErrorCode.U5CommnunicationError
                    || bIsSendOffsetFailed == true
                    || bIsReceiveRmsFailed == true
                    )
                {
                    return false;
                }
                // (O_145) 'ROBOR 1 Current Pos #1'   (On)    (PC ◁─── ROBOT)    Current Pos Bit 이니셜 완료 신호 바이너리 출력 (합 21)
                // (O_147) 'ROBOR 1 Current Pos #3'   (On)    (PC ◁─── ROBOT)
                // (O_149) 'ROBOR 1 Current Pos #5'   (On)    (PC ◁─── ROBOT)
                if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(21, mRobotTimeout, CheckCancel) == false)
                {
                    return false;
                }
                // (I_151) 'Complete Return'          (On)    (PC ───▷ ROBOT)    이니셜 완료 확인 체크
                Signals.Y.WB[Word.EOutput.COMPLETE_RETURN].Value = CDefine.ON;
                // (O_145) 'ROBOR 1 Current Pos #1'   (Off)   (PC ◁─── ROBOT)    Current Pos Bit 신호 초기화
                // (O_147) 'ROBOR 1 Current Pos #3'   (Off)   (PC ◁─── ROBOT)
                // (O_149) 'ROBOR 1 Current Pos #5'   (Off)   (PC ◁─── ROBOT)
                if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(0, mRobotTimeout, CheckCancel) == false)
                {
                    return false;
                }
                Signals.Y.WB[Word.EOutput.COMPLETE_RETURN].Value = CDefine.OFF;
                // [이니셜 완료]		
            }

            // [가상모드 처리]
            {
                Signals.X.BB[Bit.EInput.PLAY_BACK_MODE].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P1_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P2_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P3_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P4_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P5_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P6_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P7_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P8_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P9_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
                Signals.X.BB[Bit.EInput.P10_INTERLOCK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel);
            }

            onEventOccured(this, $"\t[END] {nameof(SetRobotInitial)}");
            return true;
        }

        public bool ProcessPreOrder(ERobotProcess processIndex, ProcessPreOrder processPreOrder, bool bUsePreOrderPermit = false)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(ProcessPreOrder)}");
            Debug.Assert(AllRobotProcess.Contains(processIndex), $"현재 로봇에서 지원하지 않는 로봇 프로그램 요청함 [{processIndex}]");
            RobotProgramController programController = new RobotProgramController(Signals, processIndex);
            // 사용case1. [[[ProcessBegin 전 '위치 선택 신호'를 먼저 켜줄 때]]]
            // 사용case2. [[[로봇과 Vacuum신호 H/S 첫번째 요청 신호 수신 후 '위치 선택 신호'와 '퍼밋 신호'를 먼저 켜줄 때]]] (로봇 다운 위치 이동 완료 후)
            programController.SetSelectArea(processPreOrder.Method, processPreOrder.ToolIndex, processPreOrder.StageIndex);
            if (bUsePreOrderPermit == true)
            {
                programController.ControlPermit.Value = processPreOrder.IsEmpty() == true ? CDefine.OFF : CDefine.ON;
            }
            onEventOccured(this, $"\t[END] {nameof(ProcessPreOrder)}");
            return true;
        }

        public bool ProcessBegin(ERobotProcess processIndex)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(ProcessBegin)}");
            Debug.Assert(AllRobotProcess.Contains(processIndex), $"현재 로봇에서 지원하지 않는 로봇 프로그램 요청함 [{processIndex}]");
            RobotProgramController programController = new RobotProgramController(Signals, processIndex);
            programController.ControlMovingCondition.Value = CDefine.ON;
            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(programController.MOVING_CONDITION_INDEX, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (programController.RobotInterlock.WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.X.BB[Bit.EInput.BUSY_SIGNALS].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel);
            onEventOccured(this, $"\t[END] {nameof(ProcessBegin)}");
            return true;
        }

        public bool ProcessPermit(ERobotProcess processIndex, EMethod method, ETool toolIndex, EStage stageIndex, bool bUseAlign)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(ProcessPermit)} {processIndex}/{method}/{toolIndex}/{stageIndex}");
            Debug.Assert(AllRobotProcess.Contains(processIndex), $"현재 로봇에서 지원하지 않는 로봇 프로그램 요청함 [{processIndex}]");
            RobotProgramController programController = new RobotProgramController(Signals, processIndex);
            if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
            {
                // ! 시뮬레이션 모드에서 테스트중에는 개발자의 실수를 막기위해 Assert를 사용함
                Debug.Assert(Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value == programController.MOVING_CONDITION_INDEX, $"구동중이지 않은 프로세스에 요청함 [{processIndex}]");
            }
            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value != programController.MOVING_CONDITION_INDEX)
            {
                onEventOccured(this, $"[ERROR] 구동중이지 않은 프로세스에 요청함 [{processIndex}]");
                return false;
            }

            // !!!툴 진공 신호가 안맞는 상태에서 진입하면 로봇에서 알람 발생함
            Signals.Y.BB[Bit.EOutput.TOOL_1_VACUUM_ON_DO].Value = (method == EMethod.Put);
            Signals.Y.BB[Bit.EOutput.TOOL_2_VACUUM_ON_DO].Value = (method == EMethod.Put);
            Signals.Y.BB[Bit.EOutput.TOOL_3_VACUUM_ON_DO].Value = (method == EMethod.Put);
            Signals.Y.BB[Bit.EOutput.TOOL_4_VACUUM_ON_DO].Value = (method == EMethod.Put);
            Signals.Y.BB[Bit.EOutput.TOOL_1_VACUUM_OFF_DO].Value = (method == EMethod.Get);
            Signals.Y.BB[Bit.EOutput.TOOL_2_VACUUM_OFF_DO].Value = (method == EMethod.Get);
            Signals.Y.BB[Bit.EOutput.TOOL_3_VACUUM_OFF_DO].Value = (method == EMethod.Get);
            Signals.Y.BB[Bit.EOutput.TOOL_4_VACUUM_OFF_DO].Value = (method == EMethod.Get);
            programController.ControlVisionUse.Value = bUseAlign;
            programController.SetSelectArea(method, toolIndex, stageIndex);
            if (programController.WaitForSelectAreaAck(method, toolIndex, stageIndex) == false)
            {
                return false;
            }
            programController.ControlPermit.Value = CDefine.ON;

            // [[[로봇과 Vacuum신호 H/S 진행 시작 전 시점]]]

            onEventOccured(this, $"\t[END] {nameof(ProcessPermit)}");
            return true;
        }

        public bool ProcessCycleEnd(ERobotProcess processIndex)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(ProcessCycleEnd)}");
            Debug.Assert(AllRobotProcess.Contains(processIndex), $"현재 로봇에서 지원하지 않는 로봇 프로그램 요청함 [{processIndex}]");
            RobotProgramController programController = new RobotProgramController(Signals, processIndex);
            if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
            {
                // ! 시뮬레이션 모드에서 테스트중에는 개발자의 실수를 막기위해 Assert를 사용함
                Debug.Assert(Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value == programController.MOVING_CONDITION_INDEX, $"구동중이지 않은 프로세스에 요청함 [{processIndex}]");
            }
            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value != programController.MOVING_CONDITION_INDEX)
            {
                onEventOccured(this, $"[ERROR] 구동중이지 않은 프로세스에 요청함 [{processIndex}]");
                return false;
            }

            // [[[로봇과 Vacuum신호 H/S 완료 후 시점]]]

            // 로봇 컴플리트 신호 대기
            if (programController.RobotComplete.WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.WB[Word.EOutput.COMPLETE_RETURN].Value = CDefine.ON;
            if (programController.RobotComplete.WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.WB[Word.EOutput.COMPLETE_RETURN].Value = CDefine.OFF;
            //Signals.X.BB[Bit.EInput.BUSY_SIGNALS].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel);
            onEventOccured(this, $"\t[END] {nameof(ProcessCycleEnd)}");
            return true;
        }

        public bool ProcessExitPreOrder(ERobotProcess processIndex)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(ProcessExitPreOrder)}");
            Debug.Assert(AllRobotProcess.Contains(processIndex), $"현재 로봇에서 지원하지 않는 로봇 프로그램 요청함 [{processIndex}]");
            RobotProgramController programController = new RobotProgramController(Signals, processIndex);
            // 사용case1. [[[마지막 CycleEnd 후 '프로세스 종료 신호'를 먼저 켜줄 때]]]
            programController.ControlMovingCondition.Value = CDefine.OFF;
            programController.ControlPermit.Value = CDefine.OFF;
            Signals.Y.WB[Word.EOutput.PROCESS_EXIT_REQ].Value = CDefine.ON;
            onEventOccured(this, $"\t[END] {nameof(ProcessExitPreOrder)}");
            return true;
        }

        public bool ProcessExit(ERobotProcess processIndex)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(ProcessExit)}");
            Debug.Assert(AllRobotProcess.Contains(processIndex), $"현재 로봇에서 지원하지 않는 로봇 프로그램 요청함 [{processIndex}]");
            RobotProgramController programController = new RobotProgramController(Signals, processIndex);
            //if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_OFF)
            //{
            //    Debug.Assert(Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value == programController.MOVING_CONDITION_INDEX, $"구동중이지 않은 프로세스에 요청함 [{processIndex}]");
            //}
            programController.ControlMovingCondition.Value = CDefine.OFF;
            programController.ControlPermit.Value = CDefine.OFF;
            Signals.Y.WB[Word.EOutput.PROCESS_EXIT_REQ].Value = CDefine.ON;
            if (Signals.X.WB[Word.EInput.PROCESS_EXIT_ACK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.WB[Word.EOutput.PROCESS_EXIT_REQ].Value = CDefine.OFF;
            if (Signals.X.WB[Word.EInput.PROCESS_EXIT_ACK].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            // 신호 클리어
            internalSelectionSignalsClear();
            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(0, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (programController.RobotInterlock.WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.X.BB[Bit.EInput.BUSY_SIGNALS].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel);
            onEventOccured(this, $"\t[END] {nameof(ProcessExit)}");
            return true;
        }

        public bool SendAlignData(NachiRobotAlignData sendData)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(SendAlignData)}");

            uint convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool1.X) * 1000d);
            byte[] toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL1_X_MINUS].Value = (sendData.AlignTool1.X < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_X_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_X_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));
            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool1.Y) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL1_Y_MINUS].Value = (sendData.AlignTool1.Y < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_Y_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_Y_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));
            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool1.T) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL1_T_MINUS].Value = (sendData.AlignTool1.T < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_T_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_T_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));

            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool2.X) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL2_X_MINUS].Value = (sendData.AlignTool2.X < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_X_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_X_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));
            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool2.Y) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL2_Y_MINUS].Value = (sendData.AlignTool2.Y < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_Y_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_Y_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));
            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool2.T) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL2_T_MINUS].Value = (sendData.AlignTool2.T < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_T_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_T_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));

            onEventOccured(this, $"\t[END] {nameof(SendAlignData)}");
            return true;
        }

        public bool SendCalibrationData(NachiRobotAlignData sendData)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(SendCalibrationData)}");

            uint convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool1.X) * 1000d);
            byte[] toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL1_X_MINUS].Value = (sendData.AlignTool1.X < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_X_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_X_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));
            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool1.Y) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL1_Y_MINUS].Value = (sendData.AlignTool1.Y < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_Y_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_Y_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));
            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool1.T) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL1_T_MINUS].Value = (sendData.AlignTool1.T < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_T_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_T_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));

            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool2.X) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL2_X_MINUS].Value = (sendData.AlignTool2.X < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_X_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_X_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));
            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool2.Y) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL2_Y_MINUS].Value = (sendData.AlignTool2.Y < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_Y_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_Y_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));
            convertToolData = Convert.ToUInt32(Math.Abs(sendData.AlignTool2.T) * 1000d);
            toolDataBits = BitConverter.GetBytes(convertToolData);
            Signals.Y.WB[Word.EOutput.ALIGN_TOOL2_T_MINUS].Value = (sendData.AlignTool2.T < 0d);
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_T_LOW].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 0));
            Signals.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_T_HIGH].Value = Convert.ToInt32(BitConverter.ToUInt16(toolDataBits, 2));

            onEventOccured(this, $"\t[END] {nameof(SendCalibrationData)}");
            return true;
        }

        public bool SetRobotOverrideSpeed(int overrideSpeedPercent)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(SetRobotOverrideSpeed)}");
            // 0은 오버라이드 설정을 하지 않는다는 뜻 속도가 0%가 되지는 않는다.
            // 100 이상 값은 100으로 바뀐다.(TP에 설정된 값에 %로 속도가 조정된다. (1~100))
            //   EX ) TP : 50% 설정 : 50% = 실제 속도 : 25%
            Utils.InRange(ref overrideSpeedPercent, 0, 100);
            Signals.Y.BG[Bit.EOutputGroup.SPEED_OVERRIDE_INPUT].Value = overrideSpeedPercent;
            onEventOccured(this, $"\t[END] {nameof(SetRobotOverrideSpeed)}");
            return true;
        }

        public bool SetHomeProcess(int[] alignOffsetData)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(SetHomeProcess)}");
            // 초기화 시퀀스 진행
            Signals.Y.BB[Bit.EOutput.PLAY_STOP_U1].Value = CDefine.OFF;
            if (Signals.X.BB[Bit.EInput.UNDER_STOPPING].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.BB[Bit.EOutput.PLAY_STOP_U1].Value = CDefine.ON;
            Signals.Y.BB[Bit.EOutput.EXTERNAL_RESET].Value = CDefine.ON;
            if (Signals.X.BB[Bit.EInput.EXTERNAL_REST_ACK_NO].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.BB[Bit.EOutput.EXTERNAL_RESET].Value = CDefine.OFF;
            if (Signals.X.BB[Bit.EInput.EXTERNAL_REST_ACK_NO].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.BB[Bit.EOutput.ORIGIN_RETURN].Value = CDefine.ON;
            Signals.Y.BB[Bit.EOutput.EXTERNAL_PLAY_START_U1].Value = CDefine.ON;
            if (Signals.X.BB[Bit.EInput.HOME_POSITION_U1_1].WaitForTargetValue(CDefine.ON, mInitialTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.BB[Bit.EOutput.ORIGIN_RETURN].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.EXTERNAL_PLAY_START_U1].Value = CDefine.OFF;

            onEventOccured(this, $"\t[END] {nameof(SetHomeProcess)}");
            return true;
        }

        public bool SetPause()
        {
            // B 접점
            Signals.Y.BB[Bit.EOutput.PAUSE].Value = CDefine.OFF;
            return true;
        }

        public bool SetResume()
        {
            // B 접점
            Signals.Y.BB[Bit.EOutput.PAUSE].Value = CDefine.ON;
            return true;
        }

        public bool SetAlarmReset()
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(SetAlarmReset)}");
            Signals.Y.BB[Bit.EOutput.FAILURE_RESET].OnePulseAsync(CDefine.ON, mPulseWidthTime);
            return Signals.X.BB[Bit.EInput.FAILURE].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel);
        }

        public bool CanJcmEnterMode() => (
            Status.IsPlayBackModeEntry == true
            && Status.IsRunningU1 == true
            && Status.IsHomePositionU11 == true
            && Status.IsJcmMode == false
            );

        public bool JcmEnterMode()
        {
            // 가능 여부 확인
            if (CanJcmEnterMode() == false)
            {
                return true;
            }
            onEventOccured(this, $"\t[BEGIN] {MethodBase.GetCurrentMethod().Name}");
            Signals.Y.BB[Bit.EOutput.JCM_MODE_REQ].Value = CDefine.ON;
            if (Signals.X.BB[Bit.EInput.HOME_POSITION_U1_1].WaitForTargetValue(CDefine.ON, mInitialTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (Signals.X.BB[Bit.EInput.JCM_MODE_ACK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }

            onEventOccured(this, $"\t[END] {MethodBase.GetCurrentMethod().Name}");
            return true;
        }

        public bool CanJcmExitMode() => (
            Status.IsPlayBackModeEntry == true
            && Status.IsRunningU1 == true
            && Status.IsJcmMode == true
            );

        public bool JcmExitMode()
        {
            // 가능 여부 확인
            if (CanJcmExitMode() == false)
            {
                return true;
            }
            onEventOccured(this, $"\t[BEGIN] {MethodBase.GetCurrentMethod().Name}");

            // Clear Signals
            Signals.Y.BB[Bit.EOutput.JCM_PROCESS_EXIT_REQ].Value = CDefine.OFF;
            Signals.Y.BG[Bit.EOutputGroup.JCM_CHECK_TEACHING_REQ].Value = (int)EJcmCommand.None;
            Signals.Y.BB[Bit.EOutput.JCM_CHECK_TEACHING_GO_REQ].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.JCM_SAVE_REQ].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.JCM_NO_SAVE_REQ].Value = CDefine.OFF;

            // Down 위치면 No Save
            if (JcmCheckTeachingNoSave() == false)
            {
                return false;
            }

            // Process 진행 중이었으면 종료 처리
            if (JcmEndProcess() == false)
            {
                return false;
            }

            Signals.Y.BB[Bit.EOutput.JCM_MODE_REQ].Value = CDefine.OFF;
            if (Signals.X.BB[Bit.EInput.JCM_MODE_ACK].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (Signals.X.BB[Bit.EInput.HOME_POSITION_U1_1].WaitForTargetValue(CDefine.ON, mInitialTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(21, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.WB[Word.EOutput.COMPLETE_RETURN].Value = CDefine.ON;
            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(0, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.WB[Word.EOutput.COMPLETE_RETURN].Value = CDefine.OFF;

            onEventOccured(this, $"\t[END] {MethodBase.GetCurrentMethod().Name}");
            return true;
        }

        public bool CanJcmSelectProcess(ERobotProcess processIndex)
        {
            RobotProgramController programController = new RobotProgramController(Signals, processIndex);
            return (
                Status.IsPlayBackModeEntry == true
                && Status.IsRunningU1 == true
                && Status.IsJcmMode == true
                && Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value == programController.MOVING_CONDITION_INDEX
                );
        }

        public bool JcmSelectProcess(ERobotProcess processIndex)
        {
            RobotProgramController programController = new RobotProgramController(Signals, processIndex);
            // 가능 여부 확인
            if (CanJcmSelectProcess(processIndex) == false)
            {
                if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value == programController.MOVING_CONDITION_INDEX)
                {
                    return true;
                }

                JcmEndProcess();
            }
            onEventOccured(this, $"\t[BEGIN] {MethodBase.GetCurrentMethod().Name}");
            programController.ControlMovingCondition.Value = CDefine.ON;
            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(programController.MOVING_CONDITION_INDEX, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            programController.ControlMovingCondition.Value = CDefine.OFF;

            onEventOccured(this, $"\t[END] {MethodBase.GetCurrentMethod().Name}");
            return true;
        }

        public bool CanJcmEndProcess() => (
            Status.IsPlayBackModeEntry == true
            && Status.IsRunningU1 == true
            && Status.IsJcmMode == true
            && Status.IsJcmSelectProcess == true
            );

        public bool JcmEndProcess()
        {
            // 가능 여부 확인
            if (CanJcmEndProcess() == false)
            {
                return true;
            }
            onEventOccured(this, $"\t[BEGIN] {MethodBase.GetCurrentMethod().Name}");
            Signals.Y.BB[Bit.EOutput.JCM_PROCESS_EXIT_REQ].Value = CDefine.ON;
            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(0, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.BB[Bit.EOutput.JCM_PROCESS_EXIT_REQ].Value = CDefine.OFF;

            if (Signals.X.BG[Bit.EInputGroup.JCM_CHECK_TEACHING_ACK].WaitForTargetValue((int)EJcmCommand.None, mRobotTimeout, CheckCancel) == false
                || Signals.X.BB[Bit.EInput.JCM_CHECK_TEACHING_GO_ACK].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false
                )
            {
                return false;
            }

            onEventOccured(this, $"\t[END] {MethodBase.GetCurrentMethod().Name}");
            return true;
        }

        public bool CanJcmCheckTeaching() => (
            Status.IsPlayBackModeEntry == true
            && Status.IsRunningU1 == true
            && Status.IsJcmMode == true
            && Status.IsJcmSelectProcess == true
            && Status.IsJcmCheckTeaching == false
            );

        public bool JcmCheckTeaching(EJcmCommand commandIndex)
        {
            // 가능 여부 확인
            if (CanJcmCheckTeaching() == false)
            {
                return true;
            }
            onEventOccured(this, $"\t[BEGIN] {MethodBase.GetCurrentMethod().Name}");
            IBitGroup checkTeachingRequest = Signals.Y.BG[Bit.EOutputGroup.JCM_CHECK_TEACHING_REQ];
            IReadOnlyBitGroup checkTeachingAcknowledge = Signals.X.BG[Bit.EInputGroup.JCM_CHECK_TEACHING_ACK];
            int targetCommand = (int)commandIndex;
            if (targetCommand == (int)EJcmCommand.None)
            {
                Debug.Assert(false, $"JCM모드에서 지원하지 않는 조합입니다. JcmCommand: {commandIndex}");
                return false;
            }
            checkTeachingRequest.Value = targetCommand;
            if (checkTeachingAcknowledge.WaitForTargetValue(targetCommand, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            Signals.Y.BB[Bit.EOutput.JCM_CHECK_TEACHING_GO_REQ].Value = CDefine.ON;
            if (Signals.X.BB[Bit.EInput.JCM_CHECK_TEACHING_GO_ACK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            checkTeachingRequest.Value = (int)EJcmCommand.None;
            Signals.Y.BB[Bit.EOutput.JCM_CHECK_TEACHING_GO_REQ].Value = CDefine.OFF;

            onEventOccured(this, $"\t[END] {MethodBase.GetCurrentMethod().Name}");
            return true;
        }

        public bool CanJcmEndCheckTeaching() => (
            Status.IsPlayBackModeEntry == true
            && Status.IsRunningU1 == true
            && Status.IsJcmMode == true
            && Status.IsJcmSelectProcess == true
            && Status.IsJcmCheckTeaching == true
            );

        public bool JcmCheckTeachingSave()
        {
            // 가능 여부 확인
            if (CanJcmEndCheckTeaching() == false)
            {
                return true;
            }
            onEventOccured(this, $"\t[BEGIN] {MethodBase.GetCurrentMethod().Name}");
            Signals.Y.BB[Bit.EOutput.JCM_SAVE_REQ].Value = CDefine.ON;
            if (Signals.X.BG[Bit.EInputGroup.JCM_CHECK_TEACHING_ACK].WaitForTargetValue((int)EJcmCommand.None, mRobotTimeout, CheckCancel) == false
                || Signals.X.BB[Bit.EInput.JCM_CHECK_TEACHING_GO_ACK].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false
                )
            {
                return false;
            }
            Signals.Y.BB[Bit.EOutput.JCM_SAVE_REQ].Value = CDefine.OFF;

            onEventOccured(this, $"\t[END] {MethodBase.GetCurrentMethod().Name}");
            return true;
        }

        public bool JcmCheckTeachingNoSave()
        {
            // 가능 여부 확인
            if (CanJcmEndCheckTeaching() == false)
            {
                return true;
            }
            onEventOccured(this, $"\t[BEGIN] {MethodBase.GetCurrentMethod().Name}");
            Signals.Y.BB[Bit.EOutput.JCM_NO_SAVE_REQ].Value = CDefine.ON;
            if (Signals.X.BG[Bit.EInputGroup.JCM_CHECK_TEACHING_ACK].WaitForTargetValue((int)EJcmCommand.None, mRobotTimeout, CheckCancel) == false
                || Signals.X.BB[Bit.EInput.JCM_CHECK_TEACHING_GO_ACK].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false
                )
            {
                return false;
            }
            Signals.Y.BB[Bit.EOutput.JCM_NO_SAVE_REQ].Value = CDefine.OFF;

            onEventOccured(this, $"\t[END] {MethodBase.GetCurrentMethod().Name}");
            return true;
        }

        public bool SetOtherProcessSkipIfRunning(ERobotProcess processIndex)
        {
            onEventOccured(this, $"\t[BEGIN] {nameof(SetOtherProcessSkipIfRunning)}");
            Debug.Assert(AllRobotProcess.Contains(processIndex), $"현재 로봇에서 지원하지 않는 로봇 프로그램 요청함 [{processIndex}]");

            if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value == 0)
            {
                return true;
            }

            foreach (ERobotProcess robotProcessIndex in AllRobotProcess)
            {
                if (robotProcessIndex == processIndex)
                {
                    continue;
                }
                IBitOne movingCondition = null;
                switch (robotProcessIndex)
                {
                    case ERobotProcess.P1:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P1];
                        break;
                    case ERobotProcess.P2:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P2];
                        break;
                    case ERobotProcess.P3:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P3];
                        break;
                    case ERobotProcess.P4:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P4];
                        break;
                    case ERobotProcess.P5:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P5];
                        break;
                    case ERobotProcess.P6:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P6];
                        break;
                    case ERobotProcess.P7:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P7];
                        break;
                    case ERobotProcess.P8:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P8];
                        break;
                    case ERobotProcess.P9:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P9];
                        break;
                    case ERobotProcess.P10:
                        movingCondition = Signals.Y.WB[Word.EOutput.MOVING_CONDITION_P10];
                        break;
                    default:
                        Debug.Assert(false, "구현안됨");
                        break;
                }

                if (CDefine.ON == movingCondition.Value)
                {
                    movingCondition.Value = CDefine.OFF;
                    Signals.Y.WB[Word.EOutput.PROCESS_SKIP_REQ].Value = CDefine.ON;
                    if (Signals.X.WB[Word.EInput.PROCESS_SKIP_ACK].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
                    {
                        return false;
                    }
                    Signals.Y.WB[Word.EOutput.PROCESS_SKIP_REQ].Value = CDefine.OFF;
                    if (Signals.X.WB[Word.EInput.PROCESS_SKIP_ACK].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false)
                    {
                        return false;
                    }
                    if (Signals.X.WG[Word.EInputGroup.CURRENT_POS_BITS].WaitForTargetValue(0, mRobotTimeout, CheckCancel) == false)
                    {
                        return false;
                    }
                    // 신호 클리어
                    internalSelectionSignalsClear();
                    break;
                }
            }

            onEventOccured(this, $"\t[END] {nameof(SetOtherProcessSkipIfRunning)}");
            return true;
        }

        public bool CheckCancel() => Signals.X.BB[Bit.EInput.EMERGENCY_STOP].Value;

        private void internalSelectionSignalsClear()
        {
            Signals.Y.BB[Bit.EOutput.GET_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.PUT_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.TOOL1_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.TOOL2_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.TOOL12_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.TOOL3_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.TOOL4_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.TOOL34_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.STAGE1_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.STAGE2_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.STAGE3_SELECT].Value = CDefine.OFF;
            Signals.Y.BB[Bit.EOutput.STAGE4_SELECT].Value = CDefine.OFF;
        }

        private bool offsetDataSend(object sendOffsetData)
        {
            Signals.Y.WB[Word.EOutput.U4_RECEIVE_REQ1].Value = CDefine.ON;
            if (Signals.X.WB[Word.EInput.U4_RUNNING].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode != CDefine.ESimulationMode.SIMULATION_MODE_ON
                && mbIsVirtual == false
                && SpinWait.SpinUntil(() => (true == OffsetInterface.HLIsConnected() || true == CheckCancel()), mRobotTimeout) == false)
            {
                return false;
            }
            if (Signals.X.WB[Word.EInput.U4_RECEIVE_ACK1].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            var alignOffsetData = sendOffsetData as int[];
            if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode != CDefine.ESimulationMode.SIMULATION_MODE_ON
                && mbIsVirtual == false
                && OffsetInterface.HLSend(alignOffsetData) == false
                )
            {
                return false;
            }
            Signals.Y.WB[Word.EOutput.U4_RECEIVE_REQ1].Value = CDefine.OFF;
            if (Signals.X.WB[Word.EInput.U4_RECEIVE_ACK1].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode != CDefine.ESimulationMode.SIMULATION_MODE_ON
                && mbIsVirtual == false
                && SpinWait.SpinUntil(() => (false == OffsetInterface.HLIsConnected() || true == CheckCancel()), mRobotTimeout) == false
                )
            {
                return false;
            }
            return true;
        }

        private bool rmsDataReceive()
        {
            if (RmsInterface == null)
            {
                return true;
            }
            Signals.Y.WB[Word.EOutput.U5_SEND_REQ1].Value = CDefine.ON;
            if (Signals.X.WB[Word.EInput.U5_RUNNING].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode != CDefine.ESimulationMode.SIMULATION_MODE_ON
                && mbIsVirtual == false
                && SpinWait.SpinUntil(() => (true == RmsInterface.HLIsConnected() || true == CheckCancel()), mRobotTimeout) == false
                )
            {
                return false;
            }
            if (Signals.X.WB[Word.EInput.U5_SEND_ACK1].WaitForTargetValue(CDefine.ON, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode != CDefine.ESimulationMode.SIMULATION_MODE_ON
                && mbIsVirtual == false
                && SpinWait.SpinUntil(() => (true == mbReceivedDataRms || true == CheckCancel()), mRobotTimeout) == false
                )
            {
                return false;
            }
            Signals.Y.WB[Word.EOutput.U5_SEND_REQ1].Value = CDefine.OFF;
            if (Signals.X.WB[Word.EInput.U5_SEND_ACK1].WaitForTargetValue(CDefine.OFF, mRobotTimeout, CheckCancel) == false)
            {
                return false;
            }
            if (mDocument.m_objConfig.GetSystemParameter().eSimulationMode != CDefine.ESimulationMode.SIMULATION_MODE_ON
                && mbIsVirtual == false
                && SpinWait.SpinUntil(() => (false == RmsInterface.HLIsConnected() || true == CheckCancel()), mRobotTimeout) == false
                )
            {
                return false;
            }
            return true;
        }

        private void onEventOccured(object sender, string e)
        {
            mDocument.SetUpdateLog(mLogTarget, e);
        }

        private void onReceiveDataFromRmsInterface(CDeviceNachiInterface.CReceiveData obj)
        {
            byte[] byteReceiveData = new byte[obj.iReceiveByteCount];
            Array.Copy(obj.byteReceiveData, 0, byteReceiveData, 0, obj.iReceiveByteCount);
            mRecieveRmsData.AddRange(byteReceiveData);
            if (mRecieveRmsData.Count != RmsInterface.m_nSendReceiveSize)
            {
                return;
            }
            var receiveData = mRecieveRmsData.ToArray();
            int dataSize = sizeof(int);
            int dataLength = RmsInterface.m_nSendReceiveSize / dataSize;
            var setRawdata = new int[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                setRawdata[i] = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(receiveData, i * dataSize));
            }
            bool bChanged = mReceiveDataRms.SetRawdata(setRawdata);
            backupRmsDataToFile(setRawdata);
            if (bChanged == true)
            {
                string destinationFolder = $@"{Constants.BACKUP_PATH}\RobotRms\{DateTime.Now:yyyy-MM-dd}\({RobotPrefix})RobotRmsChanged {DateTime.Now:HH_mm_ss_fff}.csv";
                if (Directory.Exists(Path.GetDirectoryName(destinationFolder)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFolder));
                }
                StringBuilder sb = new StringBuilder(8192);
                sb.AppendLine("X (mm),Y (mm),Z (mm),Rx (deg),Ry (deg),Rz (deg)");
                for (int i = 0; i < mReceiveDataRms.ToolDownData.Count; i++)
                {
                    sb.AppendLine(mReceiveDataRms.ToolDownData[i].ToString());
                }
                using (var writeFile = File.AppendText(destinationFolder))
                {
                    writeFile.Write(sb.ToString());
                }
                onEventOccured(this, $"RmsData is changed. BackupPath = '{destinationFolder}'");

                // Fire RmsDataChanged Event
                RmsDataChanged?.Invoke(this, new RmsDataChangedEventArgs(mReceiveDataRms));
            }
            mbReceivedDataRms = true;
        }
        private void restoreRmsDataFromFile()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            try
            {
                if (mBackupRmsFileStream.Length != 0)
                {
                    mBackupRmsFileStream.Seek(0, SeekOrigin.Begin);
                    var restoreData = serializer.Deserialize(mBackupRmsFileStream);
                    mReceiveDataRms.SetRawdata(restoreData as int[]);
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private void backupRmsDataToFile(int[] rmsRawdata)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            try
            {
                mBackupRmsFileStream.Seek(0, SeekOrigin.Begin);
                serializer.Serialize(mBackupRmsFileStream, rmsRawdata);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }
    }

    public sealed class RobotInitialOption
    {
        public bool IsVirtual { get; set; }
        public string RobotPrefix { get; set; }
        public CDefine.ELogType LogTarget { get; set; }
        public CDeviceNachiInterface OffsetInterface { get; set; }
        public CDeviceNachiInterface AlignInterfaceOrNull { get; set; }
        public CDeviceNachiInterface RmsInterfaceOrNull { get; set; }
        public IReadOnlyList<ERobotProcess> AllRobotProcess { get; set; }
    }
}

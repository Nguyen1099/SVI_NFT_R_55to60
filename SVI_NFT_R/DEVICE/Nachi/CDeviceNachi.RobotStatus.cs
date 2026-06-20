using SVI_NFT_R.DEVICE.Nachi;
using SVI_NFT_R.DEVICE.Nachi.SignalNames;
using System;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class CDeviceNachi
    {
        public class RobotStatus
        {
            public bool IsTeachModeEntry => mSignalController.X.BB[Bit.EInput.TEACH_MODE].Value;
            public bool IsPlayBackModeEntry => mSignalController.X.BB[Bit.EInput.PLAY_BACK_MODE].Value;
            public bool IsRunningU1 => mSignalController.X.BB[Bit.EInput.RUNNING_U1].Value;
            public bool IsUnderStopping => mSignalController.X.BB[Bit.EInput.UNDER_STOPPING].Value;
            public bool IsInterlockAlarm => mSignalController.X.BB[Bit.EInput.INTERLOCK_ALARM].Value;
            public bool IsMotorEnergized => mSignalController.X.BB[Bit.EInput.MOTOR_ENERGIZED].Value;
            public bool IsProgramEndedU1 => mSignalController.X.BB[Bit.EInput.PROGRAM_ENDED_U1].Value;
            public bool IsFailure => mSignalController.X.BB[Bit.EInput.FAILURE].Value;
            public bool IsBatteryWarningM1 => mSignalController.X.BB[Bit.EInput.BATTERY_WARNING_M1].Value;
            public bool IsHomePositionU11 => mSignalController.X.BB[Bit.EInput.HOME_POSITION_U1_1].Value;
            public bool IsHomePositionU12 => mSignalController.X.BB[Bit.EInput.HOME_POSITION_U1_2].Value;
            public bool IsProgramSelected => mSignalController.X.BB[Bit.EInput.PROGRAM_SELECTED].Value;
            public bool IsStateOutput1 => mSignalController.X.BB[Bit.EInput.STATE_OUTPUT_1].Value;
            public bool IsEmergencyStop => mSignalController.X.BB[Bit.EInput.EMERGENCY_STOP].Value;
            public bool IsUnitReady => mSignalController.X.BB[Bit.EInput.UNIT_READY].Value;
            public bool IsBusy => mSignalController.X.BB[Bit.EInput.BUSY_SIGNALS].Value;
            public bool IsP1Interlock => mSignalController.X.BB[Bit.EInput.P1_INTERLOCK].Value;
            public bool IsP2Interlock => mSignalController.X.BB[Bit.EInput.P2_INTERLOCK].Value;
            public bool IsP3Interlock => mSignalController.X.BB[Bit.EInput.P3_INTERLOCK].Value;
            public bool IsP4Interlock => mSignalController.X.BB[Bit.EInput.P4_INTERLOCK].Value;
            public bool IsP5Interlock => mSignalController.X.BB[Bit.EInput.P5_INTERLOCK].Value;
            public bool IsP6Interlock => mSignalController.X.BB[Bit.EInput.P6_INTERLOCK].Value;
            public bool IsP7Interlock => mSignalController.X.BB[Bit.EInput.P7_INTERLOCK].Value;
            public bool IsP8Interlock => mSignalController.X.BB[Bit.EInput.P8_INTERLOCK].Value;
            public bool IsP9Interlock => mSignalController.X.BB[Bit.EInput.P9_INTERLOCK].Value;
            public bool IsP10Interlock => mSignalController.X.BB[Bit.EInput.P10_INTERLOCK].Value;
            public bool IsPendantEmsOn => mSignalController.X.BB[Bit.EInput.PENDANT_EMS].Value;
            public bool IsControllerEmsOn => mSignalController.X.BB[Bit.EInput.CONTROLLER_EMS].Value;
            public bool ShouldHomingAfterTeaching => mSignalController.X.BB[Bit.EInput.SHOULD_HOMING_AFTER_TEACHING].Value;
            public int WaitSignalNumberU1 => mSignalController.X.WG[Word.EInputGroup.WAIT_SIGNAL_NUM_U1].Value;
            public int CurrentPosition => mSignalController.X.WG[Word.EInputGroup.CURRENT_POS_BITS].Value;
            public int CurrentRecipe => mSignalController.X.WG[Word.EInputGroup.CURRENT_RECIPE_BITS].Value;
            public EErrorCode CurrentErrorCode => (EErrorCode)mSignalController.X.WG[Word.EInputGroup.ERROR_CODE_BITS].Value;
            public double Tool1AlignData_X
            {
                get
                {
                    byte[] convertDataBits = new byte[4];
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_X_LOW].Value), 0, convertDataBits, 0, 2);
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_X_HIGH].Value), 0, convertDataBits, 2, 2);
                    uint value = BitConverter.ToUInt32(convertDataBits, 0);
                    double minus = mSignalController.Y.WB[Word.EOutput.ALIGN_TOOL1_X_MINUS].Value ? -1d : 1d;
                    return value / 1000d * minus;
                }
            }
            public double Tool1AlignData_Y
            {
                get
                {
                    byte[] convertDataBits = new byte[4];
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_Y_LOW].Value), 0, convertDataBits, 0, 2);
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_Y_HIGH].Value), 0, convertDataBits, 2, 2);
                    uint value = BitConverter.ToUInt32(convertDataBits, 0);
                    double minus = mSignalController.Y.WB[Word.EOutput.ALIGN_TOOL1_Y_MINUS].Value ? -1d : 1d;
                    return value / 1000d * minus;
                }
            }
            public double Tool1AlignData_T
            {
                get
                {
                    byte[] convertDataBits = new byte[4];
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_T_LOW].Value), 0, convertDataBits, 0, 2);
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL1_T_HIGH].Value), 0, convertDataBits, 2, 2);
                    uint value = BitConverter.ToUInt32(convertDataBits, 0);
                    double minus = mSignalController.Y.WB[Word.EOutput.ALIGN_TOOL1_T_MINUS].Value ? -1d : 1d;
                    return value / 1000d * minus;
                }
            }
            public double Tool2AlignData_X
            {
                get
                {
                    byte[] convertDataBits = new byte[4];
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_X_LOW].Value), 0, convertDataBits, 0, 2);
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_X_HIGH].Value), 0, convertDataBits, 2, 2);
                    uint value = BitConverter.ToUInt32(convertDataBits, 0);
                    double minus = mSignalController.Y.WB[Word.EOutput.ALIGN_TOOL2_X_MINUS].Value ? -1d : 1d;
                    return value / 1000d * minus;
                }
            }
            public double Tool2AlignData_Y
            {
                get
                {
                    byte[] convertDataBits = new byte[4];
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_Y_LOW].Value), 0, convertDataBits, 0, 2);
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_Y_HIGH].Value), 0, convertDataBits, 2, 2);
                    uint value = BitConverter.ToUInt32(convertDataBits, 0);
                    double minus = mSignalController.Y.WB[Word.EOutput.ALIGN_TOOL2_Y_MINUS].Value ? -1d : 1d;
                    return value / 1000d * minus;
                }
            }
            public double Tool2AlignData_T
            {
                get
                {
                    byte[] convertDataBits = new byte[4];
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_T_LOW].Value), 0, convertDataBits, 0, 2);
                    Array.Copy(BitConverter.GetBytes(mSignalController.Y.WG[Word.EOutputGroup.ALIGN_TOOL2_T_HIGH].Value), 0, convertDataBits, 2, 2);
                    uint value = BitConverter.ToUInt32(convertDataBits, 0);
                    double minus = mSignalController.Y.WB[Word.EOutput.ALIGN_TOOL2_T_MINUS].Value ? -1d : 1d;
                    return value / 1000d * minus;
                }
            }
            public bool IsJcmMode => mSignalController.X.BB[Bit.EInput.JCM_MODE_ACK].Value == CDefine.ON;
            public bool IsJcmSelectProcess => CurrentPosition != 0;
            public bool IsJcmCheckTeaching => mSignalController.X.BG[Bit.EInputGroup.JCM_CHECK_TEACHING_ACK].Value > (int)EJcmCommand.None;
            public EJcmCommand IsJcmCheckTeachingTool => (EJcmCommand)mSignalController.X.BG[Bit.EInputGroup.JCM_CHECK_TEACHING_ACK].Value;
            public bool IsInterferenceRegion1Permit
            {
                get
                {
                    return mSignalController.Y.WB[Word.EOutput.INTERFERENCE_REGION1_PERMIT].Value;
                }
                set
                {
                    mSignalController.Y.WB[Word.EOutput.INTERFERENCE_REGION1_PERMIT].Value = value;
                }
            }
            public bool IsInterferenceRegion2Permit
            {
                get
                {
                    return mSignalController.Y.WB[Word.EOutput.INTERFERENCE_REGION2_PERMIT].Value;
                }
                set
                {
                    mSignalController.Y.WB[Word.EOutput.INTERFERENCE_REGION2_PERMIT].Value = value;
                }
            }
            public bool IsInterferenceRegion3Permit
            {
                get
                {
                    return mSignalController.Y.WB[Word.EOutput.INTERFERENCE_REGION3_PERMIT].Value;
                }
                set
                {
                    mSignalController.Y.WB[Word.EOutput.INTERFERENCE_REGION3_PERMIT].Value = value;
                }
            }
            private readonly IReadOnlyBitOne[][] mHsRobotVacuumReqItems;
            private readonly IReadOnlyBitOne[][] mHsRobotBlowReqItems;
            private readonly IReadOnlyBitOne[][] mHsAlignReqItems;
            private readonly IReadOnlyBitOne[][] mHsMcrReqItems;
            private readonly IReadOnlyBitOne[][] mHsStageVacuumReqItems;
            private readonly IReadOnlyBitOne[][] mHsStageBlowReqItems;
            private readonly IBitOne[][] mHsRobotVacuumAckItems;
            private readonly IBitOne[][] mHsRobotBlowAckItems;
            private readonly IBitOne[][] mHsAlignAckItems;
            private readonly IBitOne[][] mHsMcrAckItems;
            private readonly IBitOne[][] mHsStageVacuumAckItems;
            private readonly IBitOne[][] mHsStageBlowAckItems;
            private readonly SignalController mSignalController;

            public RobotStatus(SignalController signalController)
            {
                mSignalController = signalController;

                // Handshake 신호 초기화
                {
                    //////////////////////////////////////////////////////////////////////////
                    // Request Items
                    // + ToolIndex 기준
                    mHsRobotVacuumReqItems = new IReadOnlyBitOne[][]
                    {
                        null,
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_1_VACUUM_ON_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_2_VACUUM_ON_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_1_VACUUM_ON_DI], mSignalController.X.BB[Bit.EInput.TOOL_2_VACUUM_ON_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_3_VACUUM_ON_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_4_VACUUM_ON_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_3_VACUUM_ON_DI], mSignalController.X.BB[Bit.EInput.TOOL_4_VACUUM_ON_DI] }
                    };
                    // + ToolIndex 기준
                    mHsRobotBlowReqItems = new IReadOnlyBitOne[][]
                    {
                        null,
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_1_VACUUM_OFF_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_2_VACUUM_OFF_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_1_VACUUM_OFF_DI], mSignalController.X.BB[Bit.EInput.TOOL_2_VACUUM_OFF_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_3_VACUUM_OFF_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_4_VACUUM_OFF_DI] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.TOOL_3_VACUUM_OFF_DI], mSignalController.X.BB[Bit.EInput.TOOL_4_VACUUM_OFF_DI] }
                    };
                    // + StageIndex 기준
                    mHsStageVacuumReqItems = new IReadOnlyBitOne[][]
                    {
                        null,
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.STG1_VAC_ON_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.STG2_VAC_ON_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.STG3_VAC_ON_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.STG4_VAC_ON_REQ] },
                    };
                    // + StageIndex 기준
                    mHsStageBlowReqItems = new IReadOnlyBitOne[][]
                    {
                        null,
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.STG1_VAC_OFF_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.STG2_VAC_OFF_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.STG3_VAC_OFF_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.STG4_VAC_OFF_REQ] },
                    };
                    // + ToolIndex 기준
                    mHsAlignReqItems = new IReadOnlyBitOne[][]
                    {
                        null,
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T1_ALIGN_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T2_ALIGN_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T1_ALIGN_REQ], mSignalController.X.BB[Bit.EInput.T2_ALIGN_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T3_ALIGN_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T4_ALIGN_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T3_ALIGN_REQ], mSignalController.X.BB[Bit.EInput.T4_ALIGN_REQ] }
                    };
                    // + ToolIndex 기준
                    mHsMcrReqItems = new IReadOnlyBitOne[][]
                    {
                        null,
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T1_MCR_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T2_MCR_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T1_MCR_REQ], mSignalController.X.BB[Bit.EInput.T2_MCR_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T3_MCR_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T4_MCR_REQ] },
                        new IReadOnlyBitOne[] { mSignalController.X.BB[Bit.EInput.T3_MCR_REQ], mSignalController.X.BB[Bit.EInput.T4_MCR_REQ] }
                    };
                    //////////////////////////////////////////////////////////////////////////

                    //////////////////////////////////////////////////////////////////////////
                    // Ack Items
                    // + ToolIndex 기준
                    mHsRobotVacuumAckItems = new IBitOne[][]
                    {
                        null,
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_1_VACUUM_ON_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_2_VACUUM_ON_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_1_VACUUM_ON_DO], mSignalController.Y.BB[Bit.EOutput.TOOL_2_VACUUM_ON_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_3_VACUUM_ON_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_4_VACUUM_ON_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_3_VACUUM_ON_DO], mSignalController.Y.BB[Bit.EOutput.TOOL_4_VACUUM_ON_DO] }
                    };
                    // + ToolIndex 기준
                    mHsRobotBlowAckItems = new IBitOne[][]
                    {
                        null,
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_1_VACUUM_OFF_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_2_VACUUM_OFF_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_1_VACUUM_OFF_DO], mSignalController.Y.BB[Bit.EOutput.TOOL_2_VACUUM_OFF_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_3_VACUUM_OFF_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_4_VACUUM_OFF_DO] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.TOOL_3_VACUUM_OFF_DO], mSignalController.Y.BB[Bit.EOutput.TOOL_4_VACUUM_OFF_DO] }
                    };
                    // + StageIndex 기준
                    mHsStageVacuumAckItems = new IBitOne[][]
                    {
                        null,
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.STG1_VAC_ON_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.STG2_VAC_ON_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.STG3_VAC_ON_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.STG4_VAC_ON_ACK] },
                    };
                    // + StageIndex 기준
                    mHsStageBlowAckItems = new IBitOne[][]
                    {
                        null,
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.STG1_VAC_OFF_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.STG2_VAC_OFF_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.STG3_VAC_OFF_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.STG4_VAC_OFF_ACK] },
                    };
                    // + ToolIndex 기준
                    mHsAlignAckItems = new IBitOne[][]
                    {
                        null,
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T1_ALIGN_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T2_ALIGN_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T1_ALIGN_ACK], mSignalController.Y.BB[Bit.EOutput.T2_ALIGN_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T3_ALIGN_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T4_ALIGN_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T3_ALIGN_ACK], mSignalController.Y.BB[Bit.EOutput.T4_ALIGN_ACK] }
                    };
                    // + ToolIndex 기준
                    mHsMcrAckItems = new IBitOne[][]
                    {
                        null,
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T1_MCR_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T2_MCR_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T1_MCR_ACK], mSignalController.Y.BB[Bit.EOutput.T2_MCR_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T3_MCR_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T4_MCR_ACK] },
                        new IBitOne[] { mSignalController.Y.BB[Bit.EOutput.T3_MCR_ACK], mSignalController.Y.BB[Bit.EOutput.T4_MCR_ACK] }
                    };
                    //////////////////////////////////////////////////////////////////////////
                }
            }

            public RobotHandshakeHandler GetRobotHandshakeHandler(ERobotProcess processIndex, EMethod methodIndex, ETool toolIndex, EStage stageIndex)
            {
                /// <![CDATA[
                /// - "2021-10-28" 로봇 IO공용맵 적용으로 함수 위치를 Device 쪽으로 이관함
                /// - [필수] robotArrivalDownReq: 신호가 켜지면 로봇이 다운 위치에 도착하여 작업할 준비가 된 시점임
                /// - [필수] controlArrivalDownAck: 다운 위치에서 처리를 완료하고 로봇을 업 위치로 보내기 위한 신호임
                /// - [옵션] robotPostActionReq, controlPostActionAck: 신호는 옵션으로 Align(P2)과 MCR(P3)에서 사용하지 않음
                /// - 시퀀스:
                ///     robotArrivalDownReq신호 ON 대기
                ///     -> 로봇 다운 위치 이동 완료
                ///     -> 작업 (Pick&Place, Align, MCR)
                ///     -> controlArrivalDownAck신호 ON
                ///     -> robotArrivalDownReq신호 OFF 대기
                ///     -> controlArrivalDownAck신호 OFF
                ///     -> [옵션] robotPostActionReq ON 대기
                ///     -> [옵션] controlPostActionAck ON
                ///     -> [옵션] robotPostActionReq OFF 대기
                ///     -> [옵션] controlPostActionAck OFF
                ///     -> 로봇 업 위치 이동 시작
                /// ]]>
                Debug.Assert(methodIndex != EMethod.None);
                IReadOnlyBitOne[] robotArrivalDownReq = null;
                IBitOne[] controlArrivalDownAck = null;
                IReadOnlyBitOne[] robotPostActionReq = null;
                IBitOne[] controlPostActionAck = null;
                switch (processIndex)
                {
                    case ERobotProcess.P1:
                    case ERobotProcess.P4:
                    case ERobotProcess.P5:
                    case ERobotProcess.P6:
                    case ERobotProcess.P7:
                    case ERobotProcess.P8:
                    case ERobotProcess.P9:
                    case ERobotProcess.P10:
                        {
                            Debug.Assert(stageIndex != EStage.None);
                            IReadOnlyBitOne[] stageBlowReqItems = null;
                            IBitOne[] stageBlowAckItems = null;
                            IReadOnlyBitOne[] stageVacuumReqItems = null;
                            IBitOne[] stageVacuumAckItems = null;
                            if (toolIndex == ETool.Tool12)
                            {
                                if (stageIndex == EStage.Stage1 || stageIndex == EStage.Stage2)
                                {
                                    stageBlowReqItems = mHsStageBlowReqItems[(int)EStage.Stage1]
                                        .Concat(mHsStageBlowReqItems[(int)EStage.Stage2])
                                        .ToArray();
                                    stageBlowAckItems = mHsStageBlowAckItems[(int)EStage.Stage1]
                                        .Concat(mHsStageBlowAckItems[(int)EStage.Stage2])
                                        .ToArray();
                                    stageVacuumReqItems = mHsStageVacuumReqItems[(int)EStage.Stage1]
                                        .Concat(mHsStageVacuumReqItems[(int)EStage.Stage2])
                                        .ToArray();
                                    stageVacuumAckItems = mHsStageVacuumAckItems[(int)EStage.Stage1]
                                        .Concat(mHsStageVacuumAckItems[(int)EStage.Stage2])
                                        .ToArray();
                                }
                                else if (stageIndex == EStage.Stage3 || stageIndex == EStage.Stage4)
                                {
                                    stageBlowReqItems = mHsStageBlowReqItems[(int)EStage.Stage3]
                                        .Concat(mHsStageBlowReqItems[(int)EStage.Stage4])
                                        .ToArray();
                                    stageBlowAckItems = mHsStageBlowAckItems[(int)EStage.Stage3]
                                        .Concat(mHsStageBlowAckItems[(int)EStage.Stage4])
                                        .ToArray();
                                    stageVacuumReqItems = mHsStageVacuumReqItems[(int)EStage.Stage3]
                                        .Concat(mHsStageVacuumReqItems[(int)EStage.Stage4])
                                        .ToArray();
                                    stageVacuumAckItems = mHsStageVacuumAckItems[(int)EStage.Stage3]
                                        .Concat(mHsStageVacuumAckItems[(int)EStage.Stage4])
                                        .ToArray();
                                }
                            }
                            else if (toolIndex == ETool.Tool34)
                            {
                                if (stageIndex == EStage.Stage1 || stageIndex == EStage.Stage2)
                                {
                                    stageBlowReqItems = mHsStageBlowReqItems[(int)EStage.Stage1]
                                        .Concat(mHsStageBlowReqItems[(int)EStage.Stage2])
                                        .ToArray();
                                    stageBlowAckItems = mHsStageBlowAckItems[(int)EStage.Stage1]
                                        .Concat(mHsStageBlowAckItems[(int)EStage.Stage2])
                                        .ToArray();
                                    stageVacuumReqItems = mHsStageVacuumReqItems[(int)EStage.Stage1]
                                        .Concat(mHsStageVacuumReqItems[(int)EStage.Stage2])
                                        .ToArray();
                                    stageVacuumAckItems = mHsStageVacuumAckItems[(int)EStage.Stage1]
                                        .Concat(mHsStageVacuumAckItems[(int)EStage.Stage2])
                                        .ToArray();
                                }
                                else if (stageIndex == EStage.Stage3 || stageIndex == EStage.Stage4)
                                {
                                    stageBlowReqItems = mHsStageBlowReqItems[(int)EStage.Stage3]
                                        .Concat(mHsStageBlowReqItems[(int)EStage.Stage4])
                                        .ToArray();
                                    stageBlowAckItems = mHsStageBlowAckItems[(int)EStage.Stage3]
                                        .Concat(mHsStageBlowAckItems[(int)EStage.Stage4])
                                        .ToArray();
                                    stageVacuumReqItems = mHsStageVacuumReqItems[(int)EStage.Stage3]
                                        .Concat(mHsStageVacuumReqItems[(int)EStage.Stage4])
                                        .ToArray();
                                    stageVacuumAckItems = mHsStageVacuumAckItems[(int)EStage.Stage3]
                                        .Concat(mHsStageVacuumAckItems[(int)EStage.Stage4])
                                        .ToArray();
                                }
                            }
                            else
                            {
                                stageBlowReqItems = mHsStageBlowReqItems[(int)stageIndex];
                                stageBlowAckItems = mHsStageBlowAckItems[(int)stageIndex];
                                stageVacuumReqItems = mHsStageVacuumReqItems[(int)stageIndex];
                                stageVacuumAckItems = mHsStageVacuumAckItems[(int)stageIndex];
                            }

                            if (methodIndex == EMethod.Get)
                            {
                                robotArrivalDownReq = mHsRobotVacuumReqItems[(int)toolIndex];
                                controlArrivalDownAck = mHsRobotVacuumAckItems[(int)toolIndex];
                                robotPostActionReq = stageBlowReqItems;
                                controlPostActionAck = stageBlowAckItems;
                            }
                            else if (methodIndex == EMethod.Put)
                            {
                                robotArrivalDownReq = stageVacuumReqItems;
                                controlArrivalDownAck = stageVacuumAckItems;
                                robotPostActionReq = mHsRobotBlowReqItems[(int)toolIndex];
                                controlPostActionAck = mHsRobotBlowAckItems[(int)toolIndex];
                            }
                        }
                        break;

                    case ERobotProcess.P2:
                        // + [고정] Align 영역
                        robotArrivalDownReq = mHsAlignReqItems[(int)toolIndex];
                        controlArrivalDownAck = mHsAlignAckItems[(int)toolIndex];
                        // + PostAction 없음
                        break;

                    case ERobotProcess.P3:
                        // + [고정] MCR 영역
                        robotArrivalDownReq = mHsMcrReqItems[(int)toolIndex];
                        controlArrivalDownAck = mHsMcrAckItems[(int)toolIndex];
                        // + PostAction 없음
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
                return new RobotHandshakeHandler(
                    signalController: mSignalController,
                    robotArrivalDownReq: robotArrivalDownReq ?? new IReadOnlyBitOne[0],
                    controlArrivalDownAck: controlArrivalDownAck ?? new IBitOne[0],
                    robotPostActionReq: robotPostActionReq ?? new IReadOnlyBitOne[0],
                    controlPostActionAck: controlPostActionAck ?? new IBitOne[0]
                    );

                // - 아래 두가지 예제는 같은 표현이다
                // [case.?? 연산자 사용]    result = robotArrivalDownReq ?? new IReadOnlyBitOne[0]; 
                // [case.if else 문 사용]   if (robotArrivalDownReq != null) result = robotArrivalDownReq; else result = IReadOnlyBitOne[0];
            }
        }

        private class RobotProgramController
        {
            public ERobotProcess ProcessIndex { get; private set; }
            public IBitOne ControlMovingCondition { get; private set; }
            public IBitOne ControlPermit { get; private set; }
            public IBitOne ControlVisionUse { get; private set; }
            public IReadOnlyBitOne RobotInterlock { get; private set; }
            public IReadOnlyBitOne RobotComplete { get; private set; }
            public readonly int MOVING_CONDITION_INDEX;
            private readonly SignalController mSignalController;

            public RobotProgramController(SignalController signalController, ERobotProcess robotProcessIndex)
            {
                mSignalController = signalController;
                ProcessIndex = robotProcessIndex;

                switch (ProcessIndex)
                {
                    case ERobotProcess.P1:
                        MOVING_CONDITION_INDEX = 1;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P1];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P1_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P1_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P1_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P1_COMPLETE];
                        break;
                    case ERobotProcess.P2:
                        MOVING_CONDITION_INDEX = 2;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P2];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P2_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P2_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P2_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P2_COMPLETE];
                        break;
                    case ERobotProcess.P3:
                        MOVING_CONDITION_INDEX = 3;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P3];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P3_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P3_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P3_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P3_COMPLETE];
                        break;
                    case ERobotProcess.P4:
                        MOVING_CONDITION_INDEX = 4;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P4];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P4_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P4_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P4_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P4_COMPLETE];
                        break;
                    case ERobotProcess.P5:
                        MOVING_CONDITION_INDEX = 5;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P5];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P5_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P5_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P5_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P5_COMPLETE];
                        break;
                    case ERobotProcess.P6:
                        MOVING_CONDITION_INDEX = 6;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P6];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P6_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P6_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P6_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P6_COMPLETE];
                        break;
                    case ERobotProcess.P7:
                        MOVING_CONDITION_INDEX = 7;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P7];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P7_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P7_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P7_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P7_COMPLETE];
                        break;
                    case ERobotProcess.P8:
                        MOVING_CONDITION_INDEX = 8;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P8];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P8_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P8_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P8_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P8_COMPLETE];
                        break;
                    case ERobotProcess.P9:
                        MOVING_CONDITION_INDEX = 9;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P9];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P9_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P9_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P9_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P9_COMPLETE];
                        break;
                    case ERobotProcess.P10:
                        MOVING_CONDITION_INDEX = 10;
                        ControlMovingCondition = signalController.Y.WB[Word.EOutput.MOVING_CONDITION_P10];
                        ControlPermit = signalController.Y.BB[Bit.EOutput.P10_PERMIT];
                        ControlVisionUse = signalController.Y.BB[Bit.EOutput.P10_VISION_USE];
                        RobotInterlock = signalController.X.BB[Bit.EInput.P10_INTERLOCK];
                        RobotComplete = signalController.X.BB[Bit.EInput.P10_COMPLETE];
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            public bool SetSelectArea(EMethod method, ETool toolIndex, EStage stageIndex)
            {
                mSignalController.Y.BB[Bit.EOutput.GET_SELECT].Value = method == EMethod.Get;
                mSignalController.Y.BB[Bit.EOutput.PUT_SELECT].Value = method == EMethod.Put;

                mSignalController.Y.BB[Bit.EOutput.TOOL1_SELECT].Value = toolIndex == ETool.Tool1;
                mSignalController.Y.BB[Bit.EOutput.TOOL2_SELECT].Value = toolIndex == ETool.Tool2;
                mSignalController.Y.BB[Bit.EOutput.TOOL12_SELECT].Value = toolIndex == ETool.Tool12;
                mSignalController.Y.BB[Bit.EOutput.TOOL3_SELECT].Value = toolIndex == ETool.Tool3;
                mSignalController.Y.BB[Bit.EOutput.TOOL4_SELECT].Value = toolIndex == ETool.Tool4;
                mSignalController.Y.BB[Bit.EOutput.TOOL34_SELECT].Value = toolIndex == ETool.Tool34;

                mSignalController.Y.BB[Bit.EOutput.STAGE1_SELECT].Value = stageIndex == EStage.Stage1;
                mSignalController.Y.BB[Bit.EOutput.STAGE2_SELECT].Value = stageIndex == EStage.Stage2;
                mSignalController.Y.BB[Bit.EOutput.STAGE3_SELECT].Value = stageIndex == EStage.Stage3;
                mSignalController.Y.BB[Bit.EOutput.STAGE4_SELECT].Value = stageIndex == EStage.Stage4;

                return true;
            }

            public bool WaitForSelectAreaAck(EMethod method, ETool toolIndex, EStage stageIndex)
            {
                switch (method)
                {
                    case EMethod.Get:
                        if (mSignalController.X.BB[Bit.EInput.GET_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case EMethod.Put:
                        if (mSignalController.X.BB[Bit.EInput.PUT_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
                switch (toolIndex)
                {
                    case ETool.Tool1:
                        if (mSignalController.X.BB[Bit.EInput.TOOL1_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case ETool.Tool2:
                        if (mSignalController.X.BB[Bit.EInput.TOOL2_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case ETool.Tool12:
                        if (mSignalController.X.BB[Bit.EInput.TOOL12_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case ETool.Tool3:
                        if (mSignalController.X.BB[Bit.EInput.TOOL3_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case ETool.Tool4:
                        if (mSignalController.X.BB[Bit.EInput.TOOL4_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case ETool.Tool34:
                        if (mSignalController.X.BB[Bit.EInput.TOOL34_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
                switch (stageIndex)
                {
                    case EStage.Stage1:
                        if (mSignalController.X.BB[Bit.EInput.STAGE1_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case EStage.Stage2:
                        if (mSignalController.X.BB[Bit.EInput.STAGE2_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case EStage.Stage3:
                        if (mSignalController.X.BB[Bit.EInput.STAGE3_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    case EStage.Stage4:
                        if (mSignalController.X.BB[Bit.EInput.STAGE4_SELECT_ACK].WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel) == false) return false;
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
                return true;
            }

            private bool checkCancel() => mSignalController.X.BB[Bit.EInput.EMERGENCY_STOP].Value;
        }
    }
}

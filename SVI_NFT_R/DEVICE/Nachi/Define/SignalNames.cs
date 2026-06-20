using System.Collections.Generic;

namespace SVI_NFT_R.DEVICE.Nachi.SignalNames
{
    public static class Bit
    {
        public enum EInput
        {
            TEACH_MODE,
            PLAY_BACK_MODE,
            RUNNING_U1,
            UNDER_STOPPING,
            INTERLOCK_ALARM,
            MOTOR_ENERGIZED,
            PROGRAM_ENDED_U1,
            FAILURE,
            BATTERY_WARNING_M1,
            HOME_POSITION_U1_1,
            HOME_POSITION_U1_2,
            PROGRAM_SELECTED,
            EXTERNAL_REST_ACK_NO,
            STATE_OUTPUT_1,
            EMERGENCY_STOP,
            UNIT_READY,
            BUSY_SIGNALS,
            PENDANT_EMS,
            CONTROLLER_EMS,
            SHOULD_HOMING_AFTER_TEACHING,
            TOOL_1_VACUUM_ON_DI,
            TOOL_1_VACUUM_OFF_DI,
            TOOL_2_VACUUM_ON_DI,
            TOOL_2_VACUUM_OFF_DI,
            TOOL_3_VACUUM_ON_DI,
            TOOL_3_VACUUM_OFF_DI,
            TOOL_4_VACUUM_ON_DI,
            TOOL_4_VACUUM_OFF_DI,
            GET_SELECT_ACK,
            PUT_SELECT_ACK,
            TOOL1_SELECT_ACK,
            TOOL2_SELECT_ACK,
            TOOL12_SELECT_ACK,
            TOOL3_SELECT_ACK,
            TOOL4_SELECT_ACK,
            TOOL34_SELECT_ACK,
            STAGE1_SELECT_ACK,
            STAGE2_SELECT_ACK,
            STAGE3_SELECT_ACK,
            STAGE4_SELECT_ACK,
            T1_ALIGN_REQ,
            T2_ALIGN_REQ,
            T3_ALIGN_REQ,
            T4_ALIGN_REQ,
            T1_MCR_REQ,
            T2_MCR_REQ,
            T3_MCR_REQ,
            T4_MCR_REQ,
            STG1_VAC_ON_REQ,
            STG1_VAC_OFF_REQ,
            STG2_VAC_ON_REQ,
            STG2_VAC_OFF_REQ,
            STG3_VAC_ON_REQ,
            STG3_VAC_OFF_REQ,
            STG4_VAC_ON_REQ,
            STG4_VAC_OFF_REQ,
            P1_INTERLOCK,
            P1_COMPLETE,
            P2_INTERLOCK,
            P2_COMPLETE,
            P3_INTERLOCK,
            P3_COMPLETE,
            P4_INTERLOCK,
            P4_COMPLETE,
            P5_INTERLOCK,
            P5_COMPLETE,
            P6_INTERLOCK,
            P6_COMPLETE,
            P7_INTERLOCK,
            P7_COMPLETE,
            P8_INTERLOCK,
            P8_COMPLETE,
            P9_INTERLOCK,
            P9_COMPLETE,
            P10_INTERLOCK,
            P10_COMPLETE,
            JCM_CHECK_TEACHING_GO_ACK,
            JCM_MODE_ACK,
            JCM_PROCESS_EXIT_ACK,
            JCM_SAVE_ACK,
            JCM_NO_SAVE_ACK
        }

        public enum EInputGroup
        {
            JCM_CHECK_TEACHING_ACK
        }
        public static IReadOnlyDictionary<EInputGroup, int> InputGroupSignalCount => mInputGroupSignalCount;
        private static readonly Dictionary<EInputGroup, int> mInputGroupSignalCount = new Dictionary<EInputGroup, int>()
        {
            { EInputGroup.JCM_CHECK_TEACHING_ACK, 4}
        };

        public enum EOutput
        {
            EXTERNAL_PLAY_START_U1,
            PLAY_STOP_U1,
            FAILURE_RESET,
            MOTOR_ON_EXTERNAL,
            MOTOR_OFF_EXTERNAL,
            REDUCE_SPEED,
            PAUSE,
            EXTERNAL_RESET,
            ORIGIN_RETURN,
            CYCLE_STOP,
            RECIPE_OK,
            PROGRAM_SELECT_BIT_U1_1,
            PROGRAM_SELECT_BIT_U1_2,
            PROGRAM_SELECT_BIT_U1_3,
            PROGRAM_SELECT_BIT_U1_4,
            PROGRAM_SELECT_BIT_U1_5,
            PROGRAM_SELECT_BIT_U1_6,
            PROGRAM_SELECT_BIT_U1_7,
            PROGRAM_SELECT_BIT_U1_8,
            SPEED_OVERRIDE_INPUT_1,
            SPEED_OVERRIDE_INPUT_2,
            SPEED_OVERRIDE_INPUT_3,
            SPEED_OVERRIDE_INPUT_4,
            SPEED_OVERRIDE_INPUT_5,
            SPEED_OVERRIDE_INPUT_6,
            SPEED_OVERRIDE_INPUT_7,
            SPEED_OVERRIDE_INPUT_8,
            AUTO_RETRY_REQUEST,
            SET_STEP_REQUEST_CLEAR,
            TOOL_1_VACUUM_ON_DO,
            TOOL_1_VACUUM_OFF_DO,
            TOOL_2_VACUUM_ON_DO,
            TOOL_2_VACUUM_OFF_DO,
            TOOL_3_VACUUM_ON_DO,
            TOOL_3_VACUUM_OFF_DO,
            TOOL_4_VACUUM_ON_DO,
            TOOL_4_VACUUM_OFF_DO,
            GET_SELECT,
            PUT_SELECT,
            TOOL1_SELECT,
            TOOL2_SELECT,
            TOOL12_SELECT,
            TOOL3_SELECT,
            TOOL4_SELECT,
            TOOL34_SELECT,
            STAGE1_SELECT,
            STAGE2_SELECT,
            STAGE3_SELECT,
            STAGE4_SELECT,
            T1_ALIGN_ACK,
            T2_ALIGN_ACK,
            T3_ALIGN_ACK,
            T4_ALIGN_ACK,
            T1_MCR_ACK,
            T2_MCR_ACK,
            T3_MCR_ACK,
            T4_MCR_ACK,
            STG1_VAC_ON_ACK,
            STG1_VAC_OFF_ACK,
            STG2_VAC_ON_ACK,
            STG2_VAC_OFF_ACK,
            STG3_VAC_ON_ACK,
            STG3_VAC_OFF_ACK,
            STG4_VAC_ON_ACK,
            STG4_VAC_OFF_ACK,
            P1_PERMIT,
            P1_VISION_USE,
            P2_PERMIT,
            P2_VISION_USE,
            P3_PERMIT,
            P3_VISION_USE,
            P4_PERMIT,
            P4_VISION_USE,
            P5_PERMIT,
            P5_VISION_USE,
            P6_PERMIT,
            P6_VISION_USE,
            P7_PERMIT,
            P7_VISION_USE,
            P8_PERMIT,
            P8_VISION_USE,
            P9_PERMIT,
            P9_VISION_USE,
            P10_PERMIT,
            P10_VISION_USE,
            JCM_CHECK_TEACHING_GO_REQ,
            JCM_MODE_REQ,
            JCM_PROCESS_EXIT_REQ,
            JCM_SAVE_REQ,
            JCM_NO_SAVE_REQ
        }

        public enum EOutputGroup
        {
            PROGRAM_SELECT_BIT_U1,
            SPEED_OVERRIDE_INPUT,
            JCM_CHECK_TEACHING_REQ
        }
        public static IReadOnlyDictionary<EOutputGroup, int> OutputGroupSignalCount => mOutputGroupSignalCount;
        private static readonly Dictionary<EOutputGroup, int> mOutputGroupSignalCount = new Dictionary<EOutputGroup, int>
        {
            { EOutputGroup.PROGRAM_SELECT_BIT_U1, 8},
            { EOutputGroup.SPEED_OVERRIDE_INPUT, 8},
            { EOutputGroup.JCM_CHECK_TEACHING_REQ, 4}
        };
    }

    public static class Word
    {
        public enum EInput
        {
            U5_RUNNING,
            U5_SEND_ACK1,
            U4_RUNNING,
            U4_RECEIVE_ACK1,
            PROCESS_SKIP_ACK,
            PROCESS_ERROR_ACK,
            PROCESS_EXIT_ACK
        }

        public enum EInputGroup
        {
            WAIT_SIGNAL_NUM_U1,
            CURRENT_POS_BITS,
            CURRENT_RECIPE_BITS,
            ERROR_CODE_BITS
        }
        public static IReadOnlyDictionary<EInputGroup, int> InputGroupSignalCount => mInputGroupSignalCount;
        private static readonly Dictionary<EInputGroup, int> mInputGroupSignalCount = new Dictionary<EInputGroup, int>
        {
            { EInputGroup.WAIT_SIGNAL_NUM_U1, 16 },
            { EInputGroup.CURRENT_POS_BITS, 5 },
            { EInputGroup.CURRENT_RECIPE_BITS, 8 },
            { EInputGroup.ERROR_CODE_BITS, 8 }
        };

        public enum EOutput
        {
            MOVING_CONDITION_P1,
            MOVING_CONDITION_P2,
            MOVING_CONDITION_P3,
            MOVING_CONDITION_P4,
            MOVING_CONDITION_P5,
            MOVING_CONDITION_P6,
            MOVING_CONDITION_P7,
            MOVING_CONDITION_P8,
            MOVING_CONDITION_P9,
            MOVING_CONDITION_P10,
            MOVING_CONDITION_P11,
            MOVING_CONDITION_P12,
            MOVING_CONDITION_P13,
            MOVING_CONDITION_P14,
            MOVING_CONDITION_P15,
            MOVING_CONDITION_P16,
            MOVING_CONDITION_P17,
            MOVING_CONDITION_P18,
            MOVING_CONDITION_P19,
            MOVING_CONDITION_P20,
            COMPLETE_RETURN,
            ERROR_CODE_RETURN,
            INTERFERENCE_REGION1_PERMIT,
            INTERFERENCE_REGION2_PERMIT,
            INTERFERENCE_REGION3_PERMIT,
            U5_SEND_REQ1,
            U4_RECEIVE_REQ1,
            PROCESS_SKIP_REQ,
            PROCESS_EXIT_REQ,
            PROCESS_ERROR_REQ,
            ALIGN_TOOL1_X_MINUS,
            ALIGN_TOOL1_Y_MINUS,
            ALIGN_TOOL1_T_MINUS,
            ALIGN_TOOL2_X_MINUS,
            ALIGN_TOOL2_Y_MINUS,
            ALIGN_TOOL2_T_MINUS
        }

        public enum EOutputGroup
        {
            RECIPE_NO_BITS,
            ALIGN_TOOL1_X_LOW,
            ALIGN_TOOL1_X_HIGH,
            ALIGN_TOOL1_Y_LOW,
            ALIGN_TOOL1_Y_HIGH,
            ALIGN_TOOL1_T_LOW,
            ALIGN_TOOL1_T_HIGH,
            ALIGN_TOOL2_X_LOW,
            ALIGN_TOOL2_X_HIGH,
            ALIGN_TOOL2_Y_LOW,
            ALIGN_TOOL2_Y_HIGH,
            ALIGN_TOOL2_T_LOW,
            ALIGN_TOOL2_T_HIGH,
        }
        public static IReadOnlyDictionary<EOutputGroup, int> OutputGroupSignalCount => mOutputGroupSignalCount;
        private static readonly Dictionary<EOutputGroup, int> mOutputGroupSignalCount = new Dictionary<EOutputGroup, int>
        {
            { EOutputGroup.RECIPE_NO_BITS, 8 },
            { EOutputGroup.ALIGN_TOOL1_X_LOW, 16 },
            { EOutputGroup.ALIGN_TOOL1_X_HIGH, 15 },
            { EOutputGroup.ALIGN_TOOL1_Y_LOW, 16 },
            { EOutputGroup.ALIGN_TOOL1_Y_HIGH, 15 },
            { EOutputGroup.ALIGN_TOOL1_T_LOW, 16 },
            { EOutputGroup.ALIGN_TOOL1_T_HIGH, 15 },
            { EOutputGroup.ALIGN_TOOL2_X_LOW, 16 },
            { EOutputGroup.ALIGN_TOOL2_X_HIGH, 15 },
            { EOutputGroup.ALIGN_TOOL2_Y_LOW, 16 },
            { EOutputGroup.ALIGN_TOOL2_Y_HIGH, 15 },
            { EOutputGroup.ALIGN_TOOL2_T_LOW, 16 },
            { EOutputGroup.ALIGN_TOOL2_T_HIGH, 15 }
        };
    }

}

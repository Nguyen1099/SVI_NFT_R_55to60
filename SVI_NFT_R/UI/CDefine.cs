using System;
using System.Collections.Generic;
using System.IO;

namespace SVI_NFT_R
{
    public static class CDefine
    {
        #region Path = "{AppRunning}/"
        public const string DEF_CONFIG_INI = "Config.ini";
        public const string DEF_DEVICE_INI = "Device.ini";
        public const string DEF_DLL_VERSION_INI = "DllVersion.ini";
        public const string DEF_VIRTUAL_INI = "Virtual.ini";
        #endregion
        #region Path = "{ItemFolder}/INI/"
        public const string DEF_IO_DAT = "IO.dat";
        public const string DEF_CCLINK_IE_ANALOG_DAT = "CCLinkIE_Analog.dat";
        public const string DEF_CCLINK_IE_DIGITAL_DAT = "CCLinkIE_Digital.dat";
        public const string DEF_CCLINK_VER2_DIGITAL_DAT = "CCLinkVer2_Digital.dat";
        public const string DEF_CCLINK_VER2_ANALOG_DAT = "CCLinkVer2_Analog.dat";
        public const string DEF_REASONCODEMAP = "ReasonCodeMap.txt";
        public const string DEF_BIN2OUTPUTDEFECTNAMES = "Bin2OutputDefectNames.txt";
        public const string DEF_AUTOREVIEWDEFECTPOINTSPRESET = "AutoReviewDefectPointsPreset.txt";
        public const string DEF_IO_ANALOG_CALIBRATION_INI = "IO_ANALOG_CALIBRATION.ini";
        public const string DEF_IO_INDIVIDUAL_ANALOG_CALIBRATION_TXT = "IO_INDIVIDUAL_ANALOG_CALIBRATION.txt";
        public const string DEF_SPECIFICATION_VERSIONSTATE_PARAMETER_INI = "Specification_VersionState_Parameter.ini";
        #endregion
        #region Path = "{ItemFolder}/DATA/"
        public const string DEF_CYLINDER_DAT = "Cylinder.dat";
        public const string DEF_VACUUM_DAT = "Vacuum.dat";
        public const string DEF_CRUCIAL_OPTION_INI = "Crucial_Option.ini";
        public const string DEF_ECM_DAT = "Ecm.dat";
        #endregion
        #region Path = "{ItemFolder}/DATA/{ModelFolder}/"
        public const string DEF_MODEL_INI = "Model.ini";
        public const string DEF_ALIGN_DAT = "Align.dat";
        public const string DEF_ROBOT_MODEL_DAT = "RobotModel.dat";
        public const string DEF_VISION_DAT = "Vision.dat";
        public const string DEF_MODEL_OFFSET_DAT = "ModelOffset.dat";
        #endregion
        public const int DEF_SIMULATION_SLEEP_TIME = 100;

        public const bool ON = true;
        public const bool OFF = false;

        public const string UNIT_MILLIMETER = "mm";
        public const string UNIT_ANGULAR = "deg";

        /// <summary>
        /// 원점 이동 동작의 위치 번호 (SetHome 동작)
        /// </summary>
        public const int ORIGIN_POSITION_NO = int.MaxValue;

        public const int WAIT_INTERLOCK_POSITION_NO = int.MaxValue - 1;

        /// <summary>
        /// 원점 이동 동작의 위치 번호2 (SetupMotion, TeachData에 Origin 동작)
        /// </summary>
        public const int MANUAL_ORIGIN_POSITION_NO = int.MaxValue - 2;

        // G: GOOD, N: NOGOOD, S: SCRAP, L: LOTLOSS, R: RETEST, O: OUT
        public const string CIM_JUDGE_OUT = "O";
        public const string CIM_JUDGE_RETEST = "R";
        public const string CIM_JUDGE_LOTLOSS = "L";
        public const string CIM_JUDGE_SCRAP = "S";
        public const string CIM_JUDGE_NOGOOD = "N";
        public const string CIM_JUDGE_GOOD = "G";
        public const string CIM_JUDGE_T = "T";

        public const string CIM_REASONCODE_BINPRIME = "BINPRIME";
        public const string CIM_REASONCODE_GOOD = "BIN1";
        public const string CIM_REASONCODE_NG = "BIN2";
        public const string CIM_REASONCODE_TRACKIN_CANCEL = "TRACK_IN_CANCEL";

        public const string DEFECT_MACHINE_RESULT_OK = "OK";
        public const string DEFECT_MACHINE_RESULT_NG = "NG";

        public const string DEVICE_GMS = "GMS";
        public const string DEVICE_MCU = "MCU";
        public const string DEVICE_TEMPERATURE = "TEMPERATURE";
        public const string DEVICE_SAFETYPLC = "SAFETYPLC";

        public static readonly HashSet<string> EquipmentPasswordNumbers = new HashSet<string>() { "3333", "7860", "4781", "135789" };

        public const string TACT_LOG_HEADER = "DateTime,CellCount,InnerID_A,CellID_A,OutTact,InspectionTact,WaitLoading,WaitUnloading,SlowPureTactUnitName,SlowPureTactUnitTime,FastPureTactUnitName,FastPureTactUnitTime";

        /// <summary>
        /// Form View 에서 전환하는 Form 목록
        /// </summary>
        public enum EFormView
        {
            FORM_VIEW_MAIN = 0,
            FORM_VIEW_ALARM,
            FORM_VIEW_STATISTICS,
            FORM_VIEW_TEACH,
            FORM_VIEW_SETUP,
            FORM_VIEW_CONFIG,
            FORM_VIEW_FINAL
        };
        /// <summary>
        /// Form View - Main 에서 전환하는 Form 목록
        /// </summary>
        public enum EFormViewMain
        {
            FORM_VIEW_MAIN_FLOW = 0,
            FORM_VIEW_MAIN_CIM,
            FORM_VIEW_MAIN_FINAL
        };
        /// <summary>
        /// Form View - Main - CIM 에서 전환하는 System Parameter 목록
        /// </summary>
        public enum ECimSystemMenu
        {
            FORM_VIEW_MAIN_CIM_FDC = 0,
            FORM_VIEW_MAIN_CIM_EC,
            FORM_VIEW_MAIN_CIM_FINAL
        };
        /// <summary>
        /// Form View - Statistics 에서 전환하는 Form 목록
        /// </summary>
        public enum EFormViewStatistics
        {
            FORM_VIEW_STATISTICS_PRODUCTION = 0,
            FORM_VIEW_STATISTICS_ALARM,
            FORM_VIEW_STATISTICS_TACT_TIME,
            FORM_VIEW_STATISTICS_CIM_MESSAGE,
            FORM_VIEW_STATISTICS_CELL_LOG_ITEM,
            FORM_VIEW_STATISTICS_EQP_LOSS,
            FORM_VIEW_STATISTICS_EQ_TO_EQ_LOSS,
            FORM_VIEW_STATISTICS_EQUIPMENT_OPERATION_MONITORING,
            FORM_VIEW_STATISTICS_DB_CELL,
            FORM_VIEW_STATISTICS_DB_PROCESS_DATA,
            FORM_VIEW_STATISTICS_DB_EVENT,
            FORM_VIEW_STATISTICS_FINAL
        };
        /// <summary>
        /// Form View - Teach 에서 전환하는 Form 목록 (이 부분은 언제든지 변경이 될 수 있음)
        /// </summary>
        public enum EFormViewTeach
        {
            FORM_VIEW_TEACH_IN_SHUTTLE = 0,
            FORM_VIEW_TEACH_IN_ROBOT,
            FORM_VIEW_TEACH_INSP_STAGE,
            FORM_VIEW_TEACH_OUT_ROBOT,
            FORM_VIEW_TEACH_OUT_FLIP,
            FORM_VIEW_TEACH_CYLINDER,
            FORM_VIEW_TEACH_VACUUM,
            FORM_VIEW_TEACH_FINAL
        };
        /// <summary>
        /// Form View - Setup 에서 전환하는 Form 목록
        /// </summary>
        public enum EFormViewSetup
        {
            FORM_VIEW_SETUP_INITIALIZE = 0,
            FORM_VIEW_SETUP_IO,
            FORM_VIEW_SETUP_MOTION,
            FORM_VIEW_SETUP_NACHI,
            FORM_VIEW_SETUP_NACHI_OFFSET,
            FORM_VIEW_SETUP_DB,
            FORM_VIEW_SETUP_VISION,
            FORM_VIEW_SETUP_ALIGN_VISION,
            FORM_VIEW_SETUP_EQ_TO_EQ,
            FORM_VIEW_SETUP_DEVICE,
            FORM_VIEW_SETUP_MONITOR,
            FORM_VIEW_SETUP_FINAL
        };
        /// <summary>
        /// Form View - Config 에서 전환하는 Form 목록
        /// </summary>
        public enum EFormViewConfig
        {
            FORM_VIEW_CONFIG_OPTION = 0,
            FORM_VIEW_CONFIG_RECIPE,
            FORM_VIEW_CONFIG_OFFSET,
            FORM_VIEW_CONFIG_WAIT_TIME_SETTING,
            FORM_VIEW_CONFIG_SIGNAL_TOWER,
            FORM_VIEW_CONFIG_MCC_LOG,
            FORM_VIEW_CONFIG_FINAL
        };
        /// <summary>
        /// Form View - Setup - DB 에서 사용하는 DB 목록
        /// </summary>
        public enum ESetupDB
        {
            SETUP_DB_ALARM = 0,
            SETUP_DB_UI_TEXT,
            SETUP_DB_USER_MESSAGE,
            SETUP_DB_USER,
            SETUP_DB_FINAL
        };
        /// <summary>
        /// Form View - Statistics - DB Cell 에서 사용하는 DB 목록
        /// </summary>
        public enum EStatisticsDBCell
        {
            STATISTICS_DB_CELL_INPUT = 0,
            STATISTICS_DB_CELL_OUTPUT,
            STATISTICS_DB_CELL_TRACK_IN,
            STATISTICS_DB_CELL_TRACK_OUT,
            STATISTICS_DB_MCR_STATUS,
            STATISTICS_DB_MACHINE_RESULT,
            STATISTICS_DB_CELL_FINAL
        };

        /// <summary>
        /// Form View - Statistics - DB Process Data 에서 사용하는 DB 목록
        /// </summary>
        public enum EStatisticsDBProcessData
        {
            STATISTICS_DB_PROCESS_DATA_CELL = 0,
            STATISTICS_DB_PROCESS_DATA_JUDGEMENT,
            STATISTICS_DB_PROCESS_DATA_READER,
            STATISTICS_DB_PROCESS_DATA_VISION_RESULT,
            STATISTICS_DB_PROCESS_DATA_WORK_ORDER,
            //STATISTICS_DB_PROCESS_DATA_TACT_TIME,
            STATISTICS_DB_PROCESS_DATA_FINAL
        };

        /// <summary>
        /// Form View - Statistics - DB Event 에서 사용하는 DB 목록
        /// </summary>
        public enum EStatisticsDBEvent
        {
            STATISTICS_DB_EVENT_ALARM = 0,
            STATISTICS_DB_EVENT_EQUIPMENT_STOP,
            STATISTICS_DB_EVENT_EQUIPMENT_TP_CODE,
            STATISTICS_DB_EVENT_EQUIPMENT_UPPER_LOSS_TIME,
            STATISTICS_DB_EVENT_EQUIPMENT_LOWER_LOSS_TIME,
            STATISTICS_DB_EVENT_SAFETY_PLC_MODIFIED_TIME,
            STATISTICS_DB_EVENT_FINAL
        };
        /// <summary>
        /// Form View - Statistics - DB Message 에서 사용하는 DB 목록
        /// </summary>
        public enum EStatisticsDBMessage
        {
            STATISTICS_DB_MESSAGE_OP_CALL = 0,
            STATISTICS_DB_MESSAGE_INTERLOCK_CALL,
            STATISTICS_DB_MESSAGE_TERMINAL_CALL,
            STATISTICS_DB_MESSAGE_FINAL
        };

        //[Flags] Vacuum 관련 모델별 관리가 필요한 경우에 참조
        //public enum EAoiStageVacuumFlags
        //{
        //    SOL1 = 1 << 0,
        //    SOL2 = 1 << 1,
        //    SOL3 = 1 << 2,
        //    SOL4 = 1 << 3
        //}


        /// <summary>
        /// 설비 점검 그룹
        /// </summary>
        public enum ECheckGroup
        {
            /// <summary>
            /// 공압과 진공 공급
            /// </summary>
            AirAndVacuumSupply = 0,

            /// <summary>
            /// 전기 공급
            /// </summary>
            ElectricitySupply,

            /// <summary>
            /// 팬
            /// </summary>
            Fan,

            /// <summary>
            /// EMS
            /// </summary>
            EMS,

            /// <summary>
            /// 도어
            /// </summary>
            Door,

            /// <summary>
            /// 다인수 키
            /// </summary>
            MaintKey,

            /// <summary>
            /// 열연 감지기
            /// </summary>
            HeatAndSmokeDetector,

            /// <summary>
            /// 센서
            /// </summary>
            Sensor,

            /// <summary>
            /// 기타
            /// </summary>
            Etc
        }

        /// <summary>
        /// Alarm Type
        /// </summary>
        public enum EAlarmType
        {
            ALARM_INFORMATION = 0,
            ALARM_QUESTION,
            ALARM_WARNING,

            ALARM_ALARM,

            ALARM_INTERLOCK,

            ALARM_FINAL
        };
        /// <summary>
        /// Error Process Type
        /// </summary>
        public enum EErrorProcess
        {
            ERROR_PROCESS_RETRY,
            ERROR_PROCESS_CONTINUE,
            ERROR_PROCESS_STOP,
            ERROR_PROCESS_FINAL
        };
        /// <summary>
        /// 한 유닛에 물류가 두개씩 적재
        /// </summary>
        public enum EPosition
        {
            A = 0,
            B,
            Final
        };
        /// <summary>
        /// 설비 운용
        /// </summary>
        public enum ERunStatus
        {
            /// <summary>
            /// 설비 정지 상태 or 배치 동작 진행중
            /// </summary>
            Stop = 0,
            /// <summary>
            /// 설비 자동 동작중
            /// </summary>
            Start,
            /// <summary>
            /// 설비 정지 진행중 (모든 자동 프로세스가 정지하면 RUN_MODE_STOP 상태로 변함)
            /// </summary>
            Stopping,
            /// <summary>
            /// 현재 대기중인 프로세스를 즉시 중지함
            /// </summary>
            Pause,
            /// <summary>
            /// 재시작 동작 진행중
            /// </summary>
            Setup,
            /// <summary>
            /// 이니셜라이즈 진행중
            /// </summary>
            Initialize,
            /// <summary>
            /// 사이클 스탑 진행중 (설비 내부에 모든 CELL이 배출되고 모든 자동 프로세스가 정지하면 RUN_MODE_STOP 상태로 변함)
            /// </summary>
            LoadingStop
        };
        /// <summary>
        /// 설비 운용 모드
        /// </summary>
        public enum ERunMode
        {
            RealRun = 0,
            UnlinkDryrun
        };
        /// <summary>
        /// 언어 모드
        /// </summary>
        public enum ELanguage
        {
            LANGUAGE_KOREA = 0,
            LANGUAGE_CHINA,
            LANGUAGE_ENGLISH,
            LANGUAGE_VIETNAM,
            LANGUAGE_FINAL
        };
        /// <summary>
        /// Simulation Mode
        /// </summary>
        public enum ESimulationMode
        {
            SIMULATION_MODE_OFF = 0,
            SIMULATION_MODE_ON,
            SIMULATION_MODE_FINAL
        };
        /// <summary>
        /// Move Mode Type
        /// </summary>
        public enum EMoveModeType
        {
            MOVE_MODE_TYPE_MANUAL = 0,
            MOVE_MODE_TYPE_AUTO,
            MOVE_MODE_TYPE_FINAL
        };
        /// <summary>
        /// 설비 방향
        /// </summary>
        public enum EMachineFrontRear
        {
            MACHINE_FRONT = 0,
            MACHINE_REAR,
        };
        /// <summary>
        /// 세이프티 키 상태
        /// </summary>
        public enum ESafetyKey
        {
            SAFETY_KEY_UNKNOWN = 0,
            SAFETY_KEY_AUTO,
            SAFETY_KEY_TEACH,
            SAFETY_KEY_FINAL
        };
        /// <summary>
        /// 세이프티 키 언락 상태
        /// </summary>
        public enum ESafetyKeyUnlock
        {
            SAFETY_KEY_UNLOCK_UNKNOWN = 0,
            SAFETY_KEY_UNLOCK_ON,
            SAFETY_KEY_UNLOCK_OFF,
            SAFETY_KEY_UNLOCK_FINAL
        };

        /// <summary>
        /// 각도 센서 디바이스
        /// </summary>
        public enum EAngularSensor
        {
        }

        /// <summary>
        /// 갭 측정 디바이스
        /// </summary>
        public enum EMeasurement
        {
        }

        /// <summary>
        /// 온도 컨트롤러 카운트
        /// </summary>
        public enum ETemperatureController
        {
            MainGruop = 0,
        }

        public enum ETemperature
        {
            // !!! Case 1.
            // !!! NEOS-HSD 모델은 SlaveID가 0부터 시작한다
            // !!! NEOS-HSD200 모델은 SlaveID가 1부터 시작한다
            // !!! Case 2.
            // !!! Modbus를 사용하는 디바이스(TK4S 등)에 SlaveID는 1부터 시작한다 (범위: 1-247)
            MainElectronicBox = 1,
            PcRack,
        }

        /// <summary>
        /// GMS(저항 측정 컨트롤러) 디바이스
        /// </summary>
        public enum EGms
        {
        }

        /// <summary>
        /// MCU(FFU 컨트롤러) 디바이스
        /// </summary>
        public enum EMcu
        {
            MainGruop = 0,
        }

        /// <summary>
        /// 얼라인
        /// </summary>
        public enum EAlign
        {
            PreAlign = 0,
        }

        /// <summary>
        /// 적산량계 카운트
        /// </summary>
        public enum EPowerMeter
        {
            POWER_METER_UPS = 0,
            POWER_METER_GPS,
        }

        /// <summary>
        /// MCR 카운트
        /// </summary>
        public enum EMcr
        {
            P1 = 0,
            P2
        }

        /// <summary>
        /// 로봇 통신
        /// </summary>
        public enum ENachiComm
        {
            InNachiOffset = 0,
            InNachiRms,
            OutNachiOffset,
            OutNachiRms
        }

        /// <summary>
        /// 검사기 통신
        /// </summary>
        public enum EInspInterface
        {
            PC1 = 0,
        }

        /// <summary>
        /// 검사기 타입
        /// </summary>
        public enum EInspectType
        {
            Main = 0,
        }

        /// <summary>
        /// 세이프티PLC
        /// </summary>
        public enum ESafetyPlc
        {
        }

        /// <summary>
        /// 알람 구조체
        /// </summary>
        public struct structureAlarmInformation
        {
            /// <summary>
            /// Alarm 레벨
            /// </summary>
            public EAlarmType eAlarmLevel;
            /// <summary>
            /// Alarm 코드
            /// </summary>
            public int iAlarmCode;
            /// <summary>
            /// 알람 객체
            /// </summary>
            public string strAlarmObject;
            /// <summary>
            /// 알람 함수
            /// </summary>
            public string strAlarmFunction;
            /// <summary>
            /// 알람 설명
            /// </summary>
            public string strAlarmDescription;
        }
        /// <summary>
        /// 설비 램프 상태
        /// </summary>
        public enum ELampSituation
        {
            LAMP_SITUATION_STAND_BY = 0,
            LAMP_SITUATION_RUNNING,
            LAMP_SITUATION_IDLE,
            LAMP_SITUATION_STOP,
            LAMP_SITUATION_RUN_DELAY,
            LAMP_SITUATION_FAULT,
            LAMP_SITUATION_PM,
            LAMP_SITUATION_OP_CALL,
            LAMP_SITUATION_INTERLOCK,
            LAMP_SITUATION_SHUT_DOWN,
            LAMP_SITUATION_LIGHT_ALARM,
            LAMP_SITUATION_FINAL
        };
        /// <summary>
        /// 설비 램프 색상
        /// </summary>
        public enum ELampColor
        {
            LAMP_COLOR_RED = 0,
            LAMP_COLOR_YELLOW,
            LAMP_COLOR_GREEN,
            LAMP_COLOR_FINAL
        };
        /// <summary>
        /// 설비 램프에 들어갈 상태값
        /// </summary>
        public enum ELampValue
        {
            LAMP_VALUE_OFF = 0,
            LAMP_VALUE_ON,
            LAMP_VALUE_BLINK,
            LAMP_VALUE_FINAL
        };
        /// <summary>
        /// 설비 부저에 들어갈 상태값
        /// </summary>
        public enum EBuzzerValue
        {
            BUZZER_VALUE_OFF = 0,
            BUZZER_VALUE_ON,
            BUZZER_VALUE_FINAL
        };
        /// <summary>
        /// 설비 부저에 재생될 목소리 타입
        /// </summary>
        public enum EBuzzerVoice
        {
            NONE = 0,
            DOOR_UNLOCK,
            KEY_NO_OWN,
            DOOR_LOCK,
            OPERATOR_CHECK,
            BUZZER
        }
        /// <summary>
        /// 유저 권한 레벨
        /// </summary>
        public enum EUserAuthorityLevel
        {
            OPERATOR = 0,
            ENGINEER,
            MASTER,
            USER_AUTHORITY_LEVEL_FINAL
        };
        /// <summary>
        /// 설비 프로그램 정상 종료
        /// </summary>
        public enum EProgramExitStatus
        {
            PROGRAM_EXIT_STATUS_OFF = 0,
            PROGRAM_EXIT_STATUS_ON
        };
        /// <summary>
        /// 로그 종류
        /// </summary>
        public enum ELogType
        {
            LOG_SYSTEM,
            LOG_PROCESS_INTERFACE_LOAD,
            LOG_PROCESS_01_IN_SHUTTLE,
            LOG_PROCESS_02_IN_ROBOT,
            LOG_PROCESS_03_INSP_STAGE,
            LOG_PROCESS_04_OUT_ROBOT,
            LOG_PROCESS_05_OUT_FLIP,
            LOG_PROCESS_INTERFACE_UNLOAD,
            LOG_BUTTON_OPERATION,
            LOG_TEACH_DATA,
            LOG_ALIGN_DATA,
            LOG_VISION_IN_PRE_ALIGN,
            LOG_VISION_AMO_INSPECTION_PC1,
            LOG_ROBOT_IN_ROBOT,
            LOG_ROBOT_OUT_ROBOT,
            LOG_CIM,
            LOG_CIM_INFORMATION,
            LOG_CIM_TRACE,
            LOG_CIM_AFTP_LINKED_INSPECTION,
            LOG_DEVICE_MCR,
            LOG_ETC,

            //////////////////////////////////////////////////////////////////////////
            // CSV Log
            LOG_SIGNAL_INTERFACE_LOAD,
            LOG_CYCLE_TACT_01_IN_SHUTTLE,
            LOG_CYCLE_TACT_02_IN_ROBOT,
            LOG_CYCLE_TACT_03_INSP_STAGE,
            LOG_CYCLE_TACT_04_OUT_ROBOT,
            LOG_CYCLE_TACT_05_OUT_FLIP,
            LOG_CYCLE_TACT_90_JAVAS_INSPECTION,
            LOG_TACT_TIME,
            LOG_COMM_ERROR_TEMPERATURE,
            LOG_COMM_ERROR_MCU,
            LOG_COMM_ERROR_GMS,
            LOG_COMM_ERROR_SAFETYPLC,
            LOG_COMM_STATISTICS_TEMPERATURE,
            LOG_COMM_STATISTICS_MCU,
            LOG_COMM_STATISTICS_GMS,
            LOG_COMM_STATISTICS_SAFETYPLC,
        };

        public enum ECycleTactInShuttle
        {
            //////////////////////////////////////////////////////////////////////////
            // Tact Time
            INNER_ID_P1 = 0,
            INNER_ID_P2,
            CELL_ID_P1,
            CELL_ID_P2,
            PURE_CYCLE_TACT,
            CYCLE_TACT_TIME,
            USER_STOP_TIME,
            WAIT_LOADING,
            MOVE_LOADING,
            WAIT_SCAN,
            MOVE_STANDBY_SCAN,
            MOVE_SCAN,
            WAIT_ALIGN,
            MOVE_ALIGN,
            WAIT_GRAB_ALIGN,
            GRAB_ALIGN,
            WAIT_UNLOAD,
            MOVE_UNLOAD,
            WAIT_UNLOADING,
            MOVE_UNLOADING,
            WAIT_LOAD,
            MOVE_LOAD,

            RAWDATA,
            //////////////////////////////////////////////////////////////////////////
            // Raw Data
            CYCLE_START_TIME,
            CYCLE_END_TIME,

            WAIT_LOADING_START_TIME,
            WAIT_LOADING_END_TIME,
            MOVE_LOADING_START_TIME,
            MOVE_LOADING_END_TIME,
            WAIT_SCAN_START_TIME,
            WAIT_SCAN_END_TIME,
            MOVE_STANDBY_SCAN_START_TIME,
            MOVE_STANDBY_SCAN_END_TIME,
            MOVE_SCAN_START_TIME,
            MOVE_SCAN_END_TIME,
            WAIT_ALIGN_START_TIME,
            WAIT_ALIGN_END_TIME,
            MOVE_ALIGN_START_TIME,
            MOVE_ALIGN_END_TIME,
            WAIT_GRAB_ALIGN_START_TIME,
            WAIT_GRAB_ALIGN_END_TIME,
            GRAB_ALIGN_START_TIME,
            GRAB_ALIGN_END_TIME,
            WAIT_UNLOAD_START_TIME,
            WAIT_UNLOAD_END_TIME,
            MOVE_UNLOAD_START_TIME,
            MOVE_UNLOAD_END_TIME,
            WAIT_UNLOADING_START_TIME,
            WAIT_UNLOADING_END_TIME,
            MOVE_UNLOADING_START_TIME,
            MOVE_UNLOADING_END_TIME,
            WAIT_LOAD_START_TIME,
            WAIT_LOAD_END_TIME,
            MOVE_LOAD_START_TIME,
            MOVE_LOAD_END_TIME,
        }

        public enum ECycleTactInRobot
        {
            //////////////////////////////////////////////////////////////////////////
            // Tact Time
            INNER_ID_P1 = 0,
            INNER_ID_P2,
            CELL_ID_P1,
            CELL_ID_P2,
            PURE_CYCLE_TACT,
            CYCLE_TACT_TIME,
            USER_STOP_TIME,
            WAIT_LOADING,
            MOVE_LOADING,
            MOVE_LOADING_EXIT,
            WAIT_MCR_APPROACH,
            MOVE_MCR_APPROACH,
            WAIT_MCR,
            MOVE_MCR,
            WAIT_UNLOAD_APPROACH,
            MOVE_UNLOAD_APPROACH,
            WAIT_UNLOADING,
            MOVE_UNLOADING,
            MOVE_UNLOADING_EXIT,
            WAIT_LOAD_APPROACH,
            MOVE_LOAD_APPROACH,

            RAWDATA,
            //////////////////////////////////////////////////////////////////////////
            // Raw Data
            CYCLE_START_TIME,
            CYCLE_END_TIME,

            WAIT_LOADING_START_TIME,
            WAIT_LOADING_END_TIME,
            MOVE_LOADING_START_TIME,
            MOVE_LOADING_END_TIME,
            MOVE_LOADING_EXIT_START_TIME,
            MOVE_LOADING_EXIT_END_TIME,
            WAIT_MCR_APPROACH_START_TIME,
            WAIT_MCR_APPROACH_END_TIME,
            MOVE_MCR_APPROACH_START_TIME,
            MOVE_MCR_APPROACH_END_TIME,
            WAIT_MCR_START_TIME,
            WAIT_MCR_END_TIME,
            MOVE_MCR_START_TIME,
            MOVE_MCR_END_TIME,
            WAIT_UNLOAD_APPROACH_START_TIME,
            WAIT_UNLOAD_APPROACH_END_TIME,
            MOVE_UNLOAD_APPROACH_START_TIME,
            MOVE_UNLOAD_APPROACH_END_TIME,
            WAIT_UNLOADING_START_TIME,
            WAIT_UNLOADING_END_TIME,
            MOVE_UNLOADING_START_TIME,
            MOVE_UNLOADING_END_TIME,
            MOVE_UNLOADING_EXIT_START_TIME,
            MOVE_UNLOADING_EXIT_END_TIME,
            WAIT_LOAD_APPROACH_START_TIME,
            WAIT_LOAD_APPROACH_END_TIME,
            MOVE_LOAD_APPROACH_START_TIME,
            MOVE_LOAD_APPROACH_END_TIME,
        }

        public enum ECycleTactInspStage
        {
            //////////////////////////////////////////////////////////////////////////
            // Tact Time
            INNER_ID_P1 = 0,
            INNER_ID_P2,
            CELL_ID_P1,
            CELL_ID_P2,
            PURE_CYCLE_TACT,
            CYCLE_TACT_TIME,
            USER_STOP_TIME,
            WAIT_LOADING,
            MOVE_LOADING,
            WAIT_GRAB,
            MOVE_GRAB_P1,
            MOVE_GRAB_P2,
            WAIT_UNLOAD,
            MOVE_UNLOAD,
            WAIT_UNLOADING,
            MOVE_UNLOADING,
            WAIT_LOAD,
            MOVE_LOAD,

            RAWDATA,
            //////////////////////////////////////////////////////////////////////////
            // Raw Data
            CYCLE_START_TIME,
            CYCLE_END_TIME,
            WAIT_LOADING_START_TIME,
            WAIT_LOADING_END_TIME,
            MOVE_LOADING_START_TIME,
            MOVE_LOADING_END_TIME,
            WAIT_GRAB_START_TIME,
            WAIT_GRAB_END_TIME,
            MOVE_GRAB_P1_START_TIME,
            MOVE_GRAB_P1_END_TIME,
            MOVE_GRAB_P2_START_TIME,
            MOVE_GRAB_P2_END_TIME,
            WAIT_UNLOAD_START_TIME,
            WAIT_UNLOAD_END_TIME,
            MOVE_UNLOAD_START_TIME,
            MOVE_UNLOAD_END_TIME,
            WAIT_UNLOADING_START_TIME,
            WAIT_UNLOADING_END_TIME,
            MOVE_UNLOADING_START_TIME,
            MOVE_UNLOADING_END_TIME,
            WAIT_LOAD_START_TIME,
            WAIT_LOAD_END_TIME,
            MOVE_LOAD_START_TIME,
            MOVE_LOAD_END_TIME,
        }

        public enum ECycleTactOutRobot
        {
            //////////////////////////////////////////////////////////////////////////
            // Tact Time
            INNER_ID_P1 = 0,
            INNER_ID_P2,
            CELL_ID_P1,
            CELL_ID_P2,
            PURE_CYCLE_TACT,
            CYCLE_TACT_TIME,
            USER_STOP_TIME,
            WAIT_LOADING,
            MOVE_LOADING,
            MOVE_LOADING_EXIT,
            WAIT_UNLOAD_APPROACH,
            MOVE_UNLOAD_APPROACH,
            WAIT_INSP_RESULT,
            MOVE_INSP_RESULT,
            WAIT_UNLOADING,
            MOVE_UNLOADING,
            MOVE_UNLOADING_EXIT,
            WAIT_LOAD_APPROACH,
            MOVE_LOAD_APPROACH,

            RAWDATA,
            //////////////////////////////////////////////////////////////////////////
            // Raw Data
            CYCLE_START_TIME,
            CYCLE_END_TIME,

            WAIT_LOADING_START_TIME,
            WAIT_LOADING_END_TIME,
            MOVE_LOADING_START_TIME,
            MOVE_LOADING_END_TIME,
            MOVE_LOADING_EXIT_START_TIME,
            MOVE_LOADING_EXIT_END_TIME,
            WAIT_UNLOAD_APPROACH_START_TIME,
            WAIT_UNLOAD_APPROACH_END_TIME,
            MOVE_UNLOAD_APPROACH_START_TIME,
            MOVE_UNLOAD_APPROACH_END_TIME,
            WAIT_INSP_RESULT_START_TIME,
            WAIT_INSP_RESULT_END_TIME,
            MOVE_INSP_RESULT_START_TIME,
            MOVE_INSP_RESULT_END_TIME,
            WAIT_UNLOADING_START_TIME,
            WAIT_UNLOADING_END_TIME,
            MOVE_UNLOADING_START_TIME,
            MOVE_UNLOADING_END_TIME,
            MOVE_UNLOADING_EXIT_START_TIME,
            MOVE_UNLOADING_EXIT_END_TIME,
            WAIT_LOAD_APPROACH_START_TIME,
            WAIT_LOAD_APPROACH_END_TIME,
            MOVE_LOAD_APPROACH_START_TIME,
            MOVE_LOAD_APPROACH_END_TIME,
        }

        public enum ECycleTactOutFlip
        {
            //////////////////////////////////////////////////////////////////////////
            // Tact Time
            INNER_ID_P1 = 0,
            INNER_ID_P2,
            CELL_ID_P1,
            CELL_ID_P2,
            PURE_CYCLE_TACT,
            CYCLE_TACT_TIME,
            USER_STOP_TIME,
            WAIT_LOADING,
            MOVE_LOADING,
            WAIT_UNLOAD_READY,
            MOVE_UNLOAD_READY,
            WAIT_UNLOADING,
            MOVE_UNLOADING,
            WAIT_RETURN,
            MOVE_RETURN,

            RAWDATA,
            //////////////////////////////////////////////////////////////////////////
            // Raw Data
            CYCLE_START_TIME,
            CYCLE_END_TIME,
            WAIT_LOADING_START_TIME,
            WAIT_LOADING_END_TIME,
            MOVE_LOADING_START_TIME,
            MOVE_LOADING_END_TIME,
            WAIT_UNLOAD_READY_START_TIME,
            WAIT_UNLOAD_READY_END_TIME,
            MOVE_UNLOAD_READY_START_TIME,
            MOVE_UNLOAD_READY_END_TIME,
            WAIT_UNLOADING_START_TIME,
            WAIT_UNLOADING_END_TIME,
            MOVE_UNLOADING_START_TIME,
            MOVE_UNLOADING_END_TIME,
            WAIT_RETURN_START_TIME,
            WAIT_RETURN_END_TIME,
            MOVE_RETURN_START_TIME,
            MOVE_RETURN_END_TIME,
        }

        public enum ECycleTactJavasInspection
        {
            //////////////////////////////////////////////////////////////////////////
            // Tact Time
            INNER_ID = 0,
            CELL_ID,
            INSP_GRAB,
            INSP_GRAB_START_WAITING,
            INSP_GRAB_END_WAITING,
            INSP_RESULT_WAITING,
            INSP_CLASS,
            INSP_DEFECT,

            RAWDATA,
            //////////////////////////////////////////////////////////////////////////
            // Raw Data  
            INSP_GRAB_START_START_TIME,
            INSP_GRAB_START_END_TIME,
            INSP_GRAB_END_START_TIME,
            INSP_GRAB_END_END_TIME,
            INSP_GRAB_RESULT_TIME,
        }
    }
}

internal static class Constants
{
    //////////////////////////////////////////////////////////////////////////
    // Nachi Offset 및 Align Data size를 Nachi I/O Map 부분의 SOCKET MAP 참조
    public const int INITPARAM_IN_ROBOT_OFFSET_BYTE_COUNT = 1884;
    public const int INITPARAM_IN_ROBOT_RMS_BYTE_COUNT = 5928;
    public const int INITPARAM_OUT_ROBOT_OFFSET_BYTE_COUNT = 1884;
    public const int INITPARAM_OUT_ROBOT_RMS_BYTE_COUNT = 5928;
    //////////////////////////////////////////////////////////////////////////
    // WaitForCommandSync에 사용되는 키 정의
    // 검사 스테이지가 Scan 트리거 종료 위치를 지나간 시점
    public const string PASS_BY_SCAN_TRIGGER_END_POSITION = "PASS_BY_SCAN_TRIGGER_END_POSITION";
    /// <![CDATA[
    /// [진공 시퀀스]
    ///     1. 하강 위치 이동 (실린더, 로봇 등..)
    ///     2. 진공 신호 셋팅
    ///     3. 진공 센서 감지 대기
    ///     4. 상승 위치 이동 (실린더, 로봇 등..)
    /// 
    /// [파기 시퀀스]
    ///     1. 하강 위치 이동 (실린더, 로봇 등..)
    ///     2. 파기 신호 셋팅
    ///     3. 파기 센서 감지 대기
    ///     4. 파기 안정화 시간 대기
    ///     5. 상승 위치 이동 (실린더, 로봇 등..)
    /// ]]>
    //////////////////////////////////////////////////////////////////////////

    public const string BACKUP_PATH = @"D:\Backup";
    public static void SaveBackupFile(string strPath)
    {
        DateTime thisMoment = DateTime.Now;

        string destinationDirectory = $@"{BACKUP_PATH}\{thisMoment:yyyy-MM-dd}";
        if (Directory.Exists(destinationDirectory) == false)
        {
            Directory.CreateDirectory(destinationDirectory);
        }

        string fileName = Path.GetFileNameWithoutExtension(strPath);
        string extensionName = Path.GetExtension(strPath);
        string strBackupPath = $@"{destinationDirectory}\{thisMoment:HH_mm_ss_fff} {fileName}";
        if (string.IsNullOrWhiteSpace(extensionName) == false)
        {
            strBackupPath += $"{extensionName}";
        }
        if (File.Exists(strBackupPath) == true)
        {
            for (int i = 0; i < 999; i++)
            {
                strBackupPath = $@"{destinationDirectory}\{thisMoment:HH_mm_ss_fff} {fileName} ({i + 1})";
                if (string.IsNullOrWhiteSpace(extensionName) == false)
                {
                    strBackupPath += $"{extensionName}";
                }
                if (File.Exists(strBackupPath) == false)
                {
                    break;
                }
                if (i == 999)
                {
                    return;
                }
            }
        }
        if (File.Exists(strPath) == true)
        {
            File.Copy(strPath, strBackupPath);
        }
    }

    public static void SaveBackupDir(string strPPID, string strFullPath)
    {
        DateTime thisMoment = DateTime.Now;
        string destinationFolder = $@"{BACKUP_PATH}\{thisMoment:yyyy-MM-dd}\Recipe\{thisMoment:HH_mm_ss_fff} {strPPID}";
        CopyFolderRecursive(strFullPath, destinationFolder);
    }

    public static void CopyFolderRecursive(string sourceFolder, string destinationFolder)
    {
        if (Directory.Exists(destinationFolder) == false)
        {
            Directory.CreateDirectory(destinationFolder);
        }
        string[] files = Directory.GetFiles(sourceFolder);
        foreach (string file in files)
        {
            string name = Path.GetFileName(file);
            string dest = Path.Combine(destinationFolder, name);
            File.Copy(file, dest);
        }
        string[] folders = Directory.GetDirectories(sourceFolder);
        foreach (string folder in folders)
        {
            string name = Path.GetFileName(folder);
            string dest = Path.Combine(destinationFolder, name);
            CopyFolderRecursive(folder, dest);
        }
    }
}

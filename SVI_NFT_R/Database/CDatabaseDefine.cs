using System;

namespace SVI_NFT_R
{
    public class CDatabaseDefine
    {
        public const string DEF_RECORD_READ_LIMIT = "LIMIT 50000";
        /// <summary>
        /// 날짜 포멧
        /// </summary>
        public const string DEF_DATE_FORMAT = "yyyy-MM-dd";
        /// <summary>
        /// 날짜 시간 포멧
        /// </summary>
        public const string DEF_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
        public const string DEF_TACT_TIME_FORMAT = @"hh\:mm\:ss\.fff";
        /// <summary>
        /// 오름차순
        /// </summary>
        public const string DEF_ASC = "ASC";
        /// <summary>
        /// 내림차순
        /// </summary>
        public const string DEF_DESC = "DESC";

        /// <summary>
        /// Table Information Alarm Schema
        /// </summary>
        public enum EInformationAlarm
        {
            ID,
            ALTX,
            ALARM_TEXT_KOREA,
            ALARM_TEXT_CHINA,
            ALARM_TEXT_ENGLISH,
            ALARM_TEXT_VIETNAM,
            ALARM_ACTION_TEXT_KOREA,
            ALARM_ACTION_TEXT_CHINA,
            ALARM_ACTION_TEXT_ENGLISH,
            ALARM_ACTION_TEXT_VIETNAM,
            LEVEL,
            UNIT,
            BOX_TYPE,
            BOX_RECTANGLE_X,
            BOX_RECTANGLE_Y,
            BOX_RECTANGLE_WIDTH,
            BOX_RECTANGLE_HEIGHT
        };

        /// <summary>
        /// Table Information SVID Schema
        /// </summary>
        public enum EInformationSvid
        {
            SVID,
            SVNAME,
            NAME_LENGTH,
            UNIT,
            MIN_VALUE,
            MAX_VALUE,
            APPROPRIATE_MIN_VALUE,
            APPROPRIATE_VALUE,
            APPROPRIATE_MAX_VALUE,
            DESCRIPTION,
            REMARKS
        };

        /// <summary>
        /// Table Information UI Text Schema
        /// </summary>
        public enum EInformationUIText
        {
            IDX,
            ID,
            FORM_NAME,
            TEXT_KOREA,
            TEXT_CHINA,
            TEXT_ENGLISH,
            TEXT_VIETNAM
        };

        /// <summary>
        /// Table Information User Message Schema
        /// </summary>
        public enum EInformationUserMessage
        {
            ID,
            TEXT_KOREA,
            TEXT_CHINA,
            TEXT_ENGLISH,
            TEXT_VIETNAM
        };

        /// <summary>
        /// Table Information User Schema
        /// </summary>
        public enum EInformationUser
        {
            ID,
            PASSWORD,
            NAME,
            DEPARTMENT,
            POSITION,
            AUTHORITY_LEVEL
        };

        /// <summary>
        /// Table EC Information Schema
        /// </summary>
        public enum EInformationEC
        {
            ECID,
            EC_ITEM_NAME,
            ECDEF,
            ECSLL,
            ECSUL,
            ECWLL,
            ECWUL,
        };

        /// <summary>
        /// Table History Alarm Schema
        /// </summary>
        public enum EHistoryAlarm
        {
            DATE,
            ID,
            STATUS
        };

        /// <summary>
        /// Table History Alarm 상태 정의
        /// </summary>
        public enum EHistoryAlarmStatus
        {
            SET,
            CLEAR
        };

        /// <summary>
        /// Table History OP Call Schema
        /// </summary>
        public enum EHistoryOPCall
        {
            DATE,
            ID,
            MESSAGE
        };

        /// <summary>
        /// Table History Interlock Call Schema
        /// </summary>
        public enum EHistoryInterlockCall
        {
            DATE,
            ID,
            MESSAGE
        };

        /// <summary>
        /// Table History Terminal Call Schema
        /// </summary>
        public enum EHistoryTerminalCall
        {
            DATE,
            ID,
            MESSAGE
        };

        /// <summary>
        /// Table History Process Data Cell Schema
        /// </summary>
        public enum EHistoryProcessDataCell
        {
            DATE,
            INNER_ID,
            CELL_ID,
            EQ_TO_EQ_ID,
            CARRIER_ID,
            JOB_ID,
            PRODUCT_ID,
            STEP_ID,
            PRODUCT_TYPE,
            PRODUCT_KIND,
            PRODUCT_SPEC,
            CELL_SIZE,
            CELL_THICKNESS,
            COMMENT,
            CELL_INFO_RESULT,
            REPLY_STATUS,
            CELL_LOT_INFORMAITON_LOTINFO_RESULT,
            CELL_LOT_INFORMAITON_DEFECT_RESULT,
            CELL_LOT_INFORMAITON_DEFECT_COMENT,
            CH_NAME,
            EQ_TO_EQ_RESULT,
            CIM_RESULT,
            MACHINE_RESULT,
            ALIGN_COUNT,
            FRONT_INSP_RESULT,
            FRONT_INSP_REASONCODES,
            REAR_INSP_RESULT,
            REAR_INSP_REASONCODES,
            PPID,
        };

        /// <summary>
        /// Table History Process Data Judgement Schema
        /// </summary>
        public enum EHistoryProcessDataJudgement
        {
            DATE,
            INNER_ID,
            CELL_ID,
            OPERATOR_ID,
            JUDGE,
            REASON_CODE,
            DESCRIPTION
        };

        /// <summary>
        /// Table History Process Data Reader Schema
        /// </summary>
        public enum EHistoryProcessDataReader
        {
            DATE,
            INNER_ID,
            CELL_ID,
            READER_ID,
            READER_RESULT_CODE,
            RETRY_COUNT
        };

        /// <summary>
        /// Table History Process Data Vision Pixel Position Schema
        /// </summary>
        public enum EHistoryProcessDataVisionPixelPosition
        {
            DATE,
            INNER_ID,
            CELL_ID,
            PIXEL_POSITION_X,
            PIXEL_POSITION_Y,
            ANGLE_DEGREE,
            ANGLE_RADIAN,
            SCORE
        };

        /// <summary>
        /// Table History Process Data Vision Result Schema
        /// </summary>
        public enum EHistoryProcessDataVisionResult
        {
            DATE,
            INNER_ID,
            CELL_ID,
            REVISION_X,
            REVISION_Y,
            REVISION_T,
            TOTAL_REVISION_X,
            TOTAL_REVISION_Y,
            TOTAL_REVISION_T,
            RESULT,
            RESULT_IMAGE_PATH,
        };

        /// <summary>
        /// Table History Process Data Work Order Schema
        /// </summary>
        public enum EHistoryProcessDataWorkOrder
        {
            DATE,
            INNER_ID,
            CELL_ID,
            PROCESS_JOB,
            PLAN_QTY,
            PROCESSED_QTY
        };

        /// <summary>
        /// Table History Proess Data Tact Time Schema
        /// </summary>
        public enum EHistoryProcessDataTactTime
        {
            DATE,
            INNER_ID,
            CELL_ID,
            TACT_TIME_DATA_START
        }

        /// <summary>
        /// Table History Cell Track In Schema
        /// </summary>
        public enum EHistoryCellTrackIn
        {
            DATE,
            INNER_ID,
            CELL_ID
        };

        /// <summary>
        /// Table History Cell Track Out Schema
        /// </summary>
        public enum EHistoryCellTrackOut
        {
            DATE,
            INNER_ID,
            CELL_ID
        };

        /// <summary>
        /// Table History Cell Input Schema
        /// </summary>
        public enum EHistoryCellInput
        {
            DATE,
            INNER_ID,
            CELL_ID
        };

        /// <summary>
        /// Table History Cell Output Schema
        /// </summary>
        public enum EHistoryCellOutput
        {
            DATE,
            INNER_ID,
            CELL_ID
        };

        /// <summary>
        /// Table History MCR Status
        /// </summary>
        public enum EHistoryMCRStatus
        {
            DATE,
            MCR_ID,
            RESULT
        };

        /// <summary>
        /// Table History Machine Result
        /// </summary>
        public enum EHistoryMachineResult
        {
            DATE,
            INNER_ID,
            RESULT
        };

        /// <summary>
        /// Table History Alarm Event Schema
        /// </summary>
        public enum EHistoryAlarmEvent
        {
            DATE,
            EVENT
        };

        /// <summary>
        /// Table History Equipment Stop Event Schema
        /// </summary>
        public enum EHistoryEquipmentStopEvent
        {
            DATE,
            EVENT
        };

        /// <summary>
        /// Table History Equipment TP Code Event Schema
        /// </summary>
        public enum EHistoryEquipmentTPCodeEvent
        {
            DATE,
            TP_CODE
        };

        /// <summary>
        /// Table History Equipment Upper Loss Time Event Schema
        /// </summary>
        public enum EHistoryEquipmentUpperLossTimeEvent
        {
            DATE,
            EVENT
        };

        /// <summary>
        /// Table History Equipment Lower Loss Time Event Schema
        /// </summary>
        public enum EHistoryEquipmentLowerLossTimeEvent
        {
            DATE,
            EVENT
        };

        /// <summary>
        /// Table History Safety PLC Modified Event Schema
        /// </summary>
        public enum EHistorySafetyPlcModifiedEvent
        {
            LAST_MODIFIED_TIME,
            ID,
            SIGNATURE_CODE
        };

        /// <summary>
        /// 이벤트 테이블에서 사용하는 값
        /// </summary>
        public enum EEventValue
        {
            SET = 0,
            CLEAR
        };

        /// <summary>
        /// MCR 결과값에서 사용하는 값
        /// </summary>
        public enum EMcrResult
        {
            OK = 0,
            NG
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
        /// 설비 운영 모니터링 항목 (생산 현황)
        /// </summary>
        public enum EProductionStatusList
        {
            OUT = 0,
            BIN1,
            BIN2,
            BIN3,
            REJECT,
            PRODUCTION_STATUS_LIST_FINAL
        };

        /// <summary>
        /// 설비 운영 모니터링 항목 (가동 현황)
        /// </summary>
        public enum EOperationStatusList
        {
            OPERATION_TIME = 0,
            EQUIPMENT_DOWN_TIME,
            STANDARD_TIME,
            REAL_PRODUCTION_TIME,
            STANDARD_PRODUCTION_TIME,
            TIME_OPERATION_RATE,
            PERFORMANCE_UTILIZATION_RATE,
            OPERATION_STATUS_LIST_FINAL
        };

        /// <summary>
        /// 설비 운영 모니터링 항목 (비가동 현황)
        /// </summary>
        public enum ENonOperationStatusList
        {
            COUNT = 0,
            TIME,
            ALARM_COUNT,
            ALARM_TIME,
            EQUIPMENT_STOP_COUNT,
            EQUIPMENT_STOP_TIME,
            EQUIPMENT_UPPER_LOSS_COUNT,
            EQUIPMENT_UPPER_LOSS_TIME,
            EQUIPMENT_LOWER_LOSS_COUNT,
            EQUIPMENT_LOWER_LOSS_TIME,
            NON_OPERATION_STATUS_LIST_FINAL
        };

        /// <summary>
        /// 설비 운영 모니터링 항목 (순간 정지 현황)
        /// </summary>
        public enum EMomentaryStopStatusList
        {
            COUNT = 0,
            TIME,
            ALARM_COUNT,
            ALARM_TIME,
            EQUIPMENT_STOP_COUNT,
            EQUIPMENT_STOP_TIME,
            EQUIPMENT_UPPER_LOSS_COUNT,
            EQUIPMENT_UPPER_LOSS_TIME,
            EQUIPMENT_LOWER_LOSS_COUNT,
            EQUIPMENT_LOWER_LOSS_TIME,
            MOMENTARY_STOP_STATUS_LIST_FINAL
        };

        /// <summary>
        /// 설비 운영 모니터링 항목 (장애 조치 시간)
        /// </summary>
        public enum ETimeToRefairList
        {
            MTBF = 0,
            MTTR,
            TIME_TO_REFAIR_LIST_FINAL
        };

        /// <summary>
        /// 설비 운영 모니터링 항목 (MCR 현황)
        /// </summary>
        public enum EMcrStatusList
        {
            COUNT = 0,
            READING_COUNT,
            READING_RATE,
            MCR_STATUS_LIST_FINAL
        };

        /// <summary>
        /// 알람 레벨 리스트 (알람 정의에 있는 테이블에 LEVEL 칼럼에서 사용)
        /// </summary>
        public enum EAlarmLevelList
        {
            LIGHT = 0,
            HEAVY,
            ALARM_LEVEL_LIST_FINAL
        };

        /// <summary>
        /// 알람 박스 타입 리스트 (알람 정의에 있는 테이블에 BOX_TYPE 칼럼에서 사용)
        /// </summary>
        public enum EAlarmBoxTypeList
        {
            SQUARE = 0,
            ALARM_BOX_TYPE_LIST_FINAL
        }

        /// <summary>
        /// SVID List 에 들어가는 값's 정의
        /// </summary>
        public enum ESvidList
        {
            UTIL_CELL_ID,
            UTIL_STEP_ID,
            USE_IN_ROBOT_MCR,
            USE_INSP_STAGE_MAIN_VISION,
            RUN_EFU,
            EFU_RPM_1,
            MAIN_ELECTRICAL_TEMP,
            MAIN_PCRACK_TEMP,
            USE_IN_SHUTTLE_PRE_ALIGN,
            IN_SHUTTLE_PRE_ALIGN_SCORE_SETTING,
            IN_SHUTTLE_P1_PRE_ALIGN_CELL_ID,
            IN_SHUTTLE_P1_PRE_ALIGN_STEP_ID,
            IN_SHUTTLE_P1_PRE_ALIGN_DATA_SCORE,
            IN_SHUTTLE_P1_PRE_ALIGN_DATA_X,
            IN_SHUTTLE_P1_PRE_ALIGN_DATA_Y,
            IN_SHUTTLE_P1_PRE_ALIGN_DATA_T,
            IN_SHUTTLE_P2_PRE_ALIGN_CELL_ID,
            IN_SHUTTLE_P2_PRE_ALIGN_STEP_ID,
            IN_SHUTTLE_P2_PRE_ALIGN_DATA_SCORE,
            IN_SHUTTLE_P2_PRE_ALIGN_DATA_X,
            IN_SHUTTLE_P2_PRE_ALIGN_DATA_Y,
            IN_SHUTTLE_P2_PRE_ALIGN_DATA_T,
            MAIN_VACUUM_PRESSURE,
            INSP_STAGE_MAIN_VACUUM_PRESSURE,
            MAIN_CDA_PRESSURE,
            IN_SHUTTLE_P1_CELL_ID,
            IN_SHUTTLE_P1_STEP_ID,
            IN_SHUTTLE_P1_VACUUM_PRESSURE,
            IN_SHUTTLE_P2_CELL_ID,
            IN_SHUTTLE_P2_STEP_ID,
            IN_SHUTTLE_P2_VACUUM_PRESSURE,
            IN_ROBOT_P1_CELL_ID,
            IN_ROBOT_P1_STEP_ID,
            IN_ROBOT_P1_VACUUM_PRESSURE,
            IN_ROBOT_P2_CELL_ID,
            IN_ROBOT_P2_STEP_ID,
            IN_ROBOT_P2_VACUUM_PRESSURE,
            INSP_STAGE_P1_CELL_ID,
            INSP_STAGE_P1_STEP_ID,
            INSP_STAGE_P1_VACUUM_PRESSURE,
            INSP_STAGE_P2_CELL_ID,
            INSP_STAGE_P2_STEP_ID,
            INSP_STAGE_P2_VACUUM_PRESSURE,
            OUT_ROBOT_P1_CELL_ID,
            OUT_ROBOT_P1_STEP_ID,
            OUT_ROBOT_P1_VACUUM_PRESSURE,
            OUT_ROBOT_P2_CELL_ID,
            OUT_ROBOT_P2_STEP_ID,
            OUT_ROBOT_P2_VACUUM_PRESSURE,
            OUT_FLIP_P1_1_CELL_ID,
            OUT_FLIP_P1_1_STEP_ID,
            OUT_FLIP_P1_1_VACUUM_PRESSURE,
            OUT_FLIP_P1_2_CELL_ID,
            OUT_FLIP_P1_2_STEP_ID,
            OUT_FLIP_P1_2_VACUUM_PRESSURE,
            OUT_FLIP_P2_1_CELL_ID,
            OUT_FLIP_P2_1_STEP_ID,
            OUT_FLIP_P2_1_VACUUM_PRESSURE,
            OUT_FLIP_P2_2_CELL_ID,
            OUT_FLIP_P2_2_STEP_ID,
            OUT_FLIP_P2_2_VACUUM_PRESSURE,
            OUT_FLIP_CELL_OUT_DELAY_TIME,
            OUT_SHUTTLE_P1_CELL_ID,
            OUT_SHUTTLE_P1_STEP_ID,
            OUT_SHUTTLE_P1_VACUUM_PRESSURE,
            OUT_SHUTTLE_P2_CELL_ID,
            OUT_SHUTTLE_P2_STEP_ID,
            OUT_SHUTTLE_P2_VACUUM_PRESSURE,
            SINGLE_PHASE_POWER_1,
            INSTANTANEOUS_POWER_1,
            R_PHASE_VOLTAGE_1,
            R_PHASE_CURRENT_1,
            R_PHASE_VOLTAGE_VTHD_1,
            R_PHASE_CURRENT_ITDD_1,
            SINGLE_PHASE_POWER_2,
            INSTANTANEOUS_POWER_2,
            R_PHASE_VOLTAGE_2,
            R_PHASE_CURRENT_2,
            R_PHASE_VOLTAGE_VTHD_2,
            R_PHASE_CURRENT_ITDD_2,
        }

        /// <summary>
        /// 공유 메모리에 올라오는 알람 자료를 리스트 형태로 갖고 있어야 해당 셀을 클릭했을 때 자료를 보여주기 용이함
        /// </summary>
        public class CAlarmData
        {
            /// <summary>
            /// 알람 발생 시간
            /// </summary>
            public DateTime tAlarmOccuredDate;
            /// <summary>알람 발생 시간</summary>
            public string strAlarmDate;
            /// <summary>알람 아이디</summary>
            public int iAlarmID;
            /// <summary>알람 고유명</summary>
            public string ALTX;
            /// <summary>알람 레벨</summary>
            public EAlarmLevelList eAlarmLevel;
            /// <summary>알람 해당 유닛</summary>
            public string strAlarmUnit;
            /// <summary>알람 표시될 박스종류</summary>
            public EAlarmBoxTypeList eAlarmBoxType;
            /// <summary>알람 표시할 박스 시작점 X</summary>
            public double iAlarmBoxRectangleX;
            /// <summary>알람 표시할 박스 시작점 Y</summary>
            public double iAlarmBoxRectangleY;
            /// <summary>알람 표시할 박스 시작점 으로부터 넓이</summary>
            public double iAlarmBoxRectangleWidth;
            /// <summary>알람 표시할 박스 시작점 으로부터 높이</summary>
            public double iAlarmBoxRectangleHeight;
            /// <summary>알람 내용</summary>
            public string[] strAlarmText;
            /// <summary>알람 내용</summary>
            public string[] strAlarmActionText;

            public CAlarmData()
            {
                tAlarmOccuredDate = default(DateTime);
                strAlarmDate = "";
                iAlarmID = 0;
                ALTX = "";
                eAlarmLevel = 0;
                strAlarmUnit = "";
                eAlarmBoxType = 0;
                iAlarmBoxRectangleX = 0;
                iAlarmBoxRectangleY = 0;
                iAlarmBoxRectangleWidth = 0;
                iAlarmBoxRectangleHeight = 0;
                strAlarmText = new string[(int)ELanguage.LANGUAGE_FINAL];
                strAlarmActionText = new string[(int)ELanguage.LANGUAGE_FINAL];
                for (int iLoopLanguage = 0; iLoopLanguage < (int)ELanguage.LANGUAGE_FINAL; iLoopLanguage++)
                {
                    strAlarmText[iLoopLanguage] = "";
                    strAlarmActionText[iLoopLanguage] = "";
                }
            }

            public object Clone()
            {
                CAlarmData objData = new CAlarmData();

                objData.tAlarmOccuredDate = this.tAlarmOccuredDate;
                objData.strAlarmDate = this.strAlarmDate;
                objData.iAlarmID = this.iAlarmID;
                objData.ALTX = this.ALTX;
                objData.eAlarmLevel = this.eAlarmLevel;
                objData.strAlarmUnit = this.strAlarmUnit;
                objData.eAlarmBoxType = this.eAlarmBoxType;
                objData.iAlarmBoxRectangleX = this.iAlarmBoxRectangleX;
                objData.iAlarmBoxRectangleY = this.iAlarmBoxRectangleY;
                objData.iAlarmBoxRectangleWidth = this.iAlarmBoxRectangleWidth;
                objData.iAlarmBoxRectangleHeight = this.iAlarmBoxRectangleHeight;
                objData.strAlarmText = new string[(int)ELanguage.LANGUAGE_FINAL];
                objData.strAlarmActionText = new string[(int)ELanguage.LANGUAGE_FINAL];
                for (int iLoopLanguage = 0; iLoopLanguage < (int)ELanguage.LANGUAGE_FINAL; iLoopLanguage++)
                {
                    objData.strAlarmText[iLoopLanguage] = this.strAlarmText[iLoopLanguage];
                    objData.strAlarmActionText[iLoopLanguage] = this.strAlarmActionText[iLoopLanguage];
                }

                return objData;
            }
        }
    }
}
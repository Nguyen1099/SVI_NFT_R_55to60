using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SVI_NFT_R
{
    public class CCIMDefine
    {
        /// <summary>
        /// Unit 정의
        /// </summary>
        public enum EUnit
        {
            ST01 = 0,
        };

        /// <summary>
        /// Avilability State
        /// 설비 가용 상태
        /// </summary>
        public enum EPresentState
        {
            CURRENT_STATE = 0,
            PREVIOUS_STATE,
            PRESENT_STATE_FINAL
        };

        /// <summary>
        /// Heart Beat
        /// </summary>
        public enum EHeartBeat
        {
            HEART_BEAT_OFF = 0,
            HEART_BEAT_ON
        }

        /// <summary>
        /// 무언정지
        /// </summary>
        public enum ESilenceStop
        {
            SILENCE_STOP_OFF = 0,
            SILENCE_STOP_ON
        }

        /// <summary>
        /// Avilability State
        /// 설비 가용 상태
        /// </summary>
        public enum EAvailabilityState
        {
            AVAILABILITY_STATE_DOWN = 1,
            AVAILABILITY_STATE_UP
        };

        /// <summary>
        /// Interlock State
        /// Interlock 가용 상태
        /// </summary>
        public enum EInterlockState
        {
            INTERLOCK_STATE_ON = 1,
            INTERLOCK_STATE_OFF
        };

        /// <summary>
        /// Move State
        /// 설비의 동작 상태
        /// </summary>
        public enum EMoveState
        {
            MOVE_STATE_PAUSE = 1,
            MOVE_STATE_RUNNING
        };

        /// <summary>
        /// Run State
        /// 설비내 Cell 존재 유무
        /// </summary>
        public enum ERunState
        {
            RUN_STATE_IDLE = 1,
            RUN_STATE_RUN
        };

        /// <summary>
        /// Front State
        /// Inline 설비의 경우 상류 설비 물류 상태
        /// </summary>
        public enum EFrontEquipmentState
        {
            FRONT_STATE_DOWN = 1,
            FRONT_STATE_UP
        };

        /// <summary>
        /// Rear State
        /// Inline 설비의 경우 하류 설비 물류 상태
        /// </summary>
        public enum ERearEquipmentState
        {
            REAR_STATE_DOWN = 1,
            REAR_STATE_UP
        };

        /// <summary>
        /// PP SPL State
        /// Sample Run-Normal Run 상태 구분
        /// </summary>
        public enum EPP_SPLState
        {
            PP_SPL_STATE_PP_SPL = 1,
            PP_SPL_STATE_NORMAL
        };

        /// <summary>
        /// Opcall State
        /// </summary>
        public enum EOpcallState
        {
            OP_CALL_STATE_ON = 1,
            OP_CALL_STATE_OFF
        }

        /// <summary>
        /// PM Mode On/Off 상태
        /// </summary>
        public enum EPmMode
        {
            PM_MODE_ON = 1,
            PM_MODE_OFF
        }

        /// <summary>
        /// TIACK
        /// </summary>
        public enum EDateAndTimeSetReply
        {
            DATE_AND_TIME_SET_REPLY_ACCEPTED = 0,
            DATE_AND_TIME_SET_REPLY_FORMET_ERROR
        };

        /// <summary>
        /// EFID
        /// </summary>
        public enum EFunctionID
        {
            EFID_CELL_TRACKING = 1,
            EFID_TRACKING_CONTROL,
            EFID_MATERIAL_TRACKING,
            EFID_CELL_MCR_MODE,
            EFID_MATERIAL_MCR_MODE,
            EFID_LOT_ASSIGN_INFORMATION,
            EFID_AGV_ACCESS_MODE,
            EFID_AREA_SENSOR_MODE,
            EFID_SORT_MODE,
            EFID_INTERLOCK_CONTROL,
            EFID_LOADER_LOAD_PORT_MCR_MODE,
            EFID_LOADER_USE_PORT_MCR_MODE,
            EFID_UNLOADER_USE_PORT_MCR_MODE,
            EFID_APC_MODE,
            EFID_MULTI_PASS_MODE,
            EFID_FINAL
        };

        /// <summary>
        /// Function Change Reply
        /// </summary>
        public enum EFunctionChangeReply
        {
            FUNCTION_CHANGE_REPLY_ACCEPTED = 0,
            FUNCTION_CHANGE_REPLY_INVALIID_CELL_ID,
            FUNCTION_CHANGE_REPLY_NOT_SUPPORTED,
            FUNCTION_CHANGE_REPLY_REJECTED,
            FUNCTION_CHANGE_REPLY_OTHER_ERROR,
            FUNCTION_CHANGE_REPLY_NOT_FOUND_FUNCTION,
            FUNCTION_CHANGE_REPLY_SAME_VALUE
        };

        /// <summary>
        /// Conveyor MOVE State
        /// </summary>
        public enum EConveyorMoveState
        {
            CONVEYOR_MOVE_STATE_STOP = 1,
            CONVEYOR_MOVE_STATE_MOVE
        };

        /// <summary>
        /// Conveyor STOP REASON
        /// </summary>
        public enum EConveyorStopReason
        {
            CONVEYOR_STOP_REASON_RUN_WAIT = 1,
            CONVEYOR_STOP_REASON_ALARM_STOP,
            CONVEYOR_STOP_REASON_OPERATOR_STOP,
            CONVEYOR_STOP_REASON_PREVENT_LOSS,
            CONVEYOR_STOP_REASON_LOW_DOWN
        };

        /// <summary>
        /// CONTROL STATE
        /// </summary>
        public enum EControlState
        {
            CRST_OFFLINE = 0,
            CRST_ONLINE_REMOTE,
            CRST_ONLINE_LOCAL
        };

        /// <summary>
        /// Control State Set Reply
        /// </summary>
        public enum EControlStateSetReply
        {
            CONTROL_STATE_SET_REPLY_ACCEPTED = 0,
            CONTROL_STATE_SET_REPLY_SPECIAL_REASON = 1,
            CONTROL_STATE_SET_REPLY_NOT_SUPPORTED = 6,
            CONTROL_STATE_SET_REPLY_EQUIPMENT_BUSY = 7
        };

        /// <summary>
        /// Cell Event
        /// </summary>
        public enum ECellEvent
        {
            CELL_EVENT_IN = 1,
            CELL_EVENT_OUT,
            CELL_EVENT_PROCESS_START,
            CELL_EVENT_PROCESS_END,
            CELL_EVENT_SCRAP,
            CELL_EVENT_UNSCRAP,
            CELL_EVENT_CARRIER_ASSIGN,
            CELL_EVENT_CARRIER_RELEASE,
            CELL_EVENT_CONFIRM_OUT
        };

        /// <summary>
        /// UNIT STATE
        /// </summary>
        public enum EUnitState
        {
            UNIT_STATE_IDLE = 1,
            UNIT_STATE_RUN,
            UNIT_STATE_DOWN
        };

        /// <summary>
        /// CELL JOB PROCESS CMD
        /// </summary>
        public enum ECellJobProcessCmd
        {
            CELL_JOB_PROCESS_CMD_START = 21,
            CELL_JOB_PROCESS_CMD_CANCEL,
            CELL_JOB_PROCESS_CMD_PAUSE,
            CELL_JOB_PROCESS_CMD_RESUME
        };

        /// <summary>
        /// Cell Job Process Reply
        /// </summary>
        public enum ECellJobProcessReply
        {
            CELL_JOB_PROCESS_REPLY_ACCEPTED = 0,
            CELL_JOB_PROCESS_REPLY_INVALID_JOB_ID,
            CELL_JOB_PROCESS_REPLY_CELL_ID,
            CELL_JOB_PROCESS_REPLY_REJECT,
            CELL_JOB_PROCESS_REPLY_OTHER_ERROR
        };

        /// <summary>
        /// PPID CHANGE EVENT MODE
        /// </summary>
        public enum EPpidChangeMode
        {
            PPID_CHANGE_MODE_CREATE = 1,
            PPID_CHANGE_MODE_DELETE,
            PPID_CHANGE_MODE_MODIFY
        };

        /// <summary>
        /// CURRENT PPID PARAM STATE
        /// </summary>
        public enum ECurrentPpidParamState
        {
            PPID_STATE_EMPTIED = 1,
            PPID_STATE_INITIALIZED,
            PPID_STATE_CHANGED,
            PPID_STATE_PARAMETER_CHANGED
        };

        /// <summary>
        /// OP CALL Reply
        /// </summary>
        public enum EOpCallReply
        {
            OP_CALL_REPLY_ACCEPTED = 0,
            OP_CALL_REPLY_INVALID_JOB_ID,
            OP_CALL_REPLY_CELL_ID,
            OP_CALL_REPLY_REJECT,
            OP_CALL_REPLY_OTHER_ERROR
        };

        /// <summary>
        /// Interlock Reply
        /// </summary>
        public enum EInterlockReply
        {
            INTERLOCK_REPLY_ACCEPTED = 0,
            INTERLOCK_REPLY_INVALID_JOB_ID,
            INTERLOCK_REPLY_CELL_ID,
            INTERLOCK_REPLY_REJECT,
            INTERLOCK_REPLY_OTHER_ERROR,
            INTERLOCK_REPLY_NOT_FOUND_FUNCTION,
            INTERLOCK_REPLY_THIS_IS_NOT_SUPPORT_MODE,
        };

        /// <summary>
        /// Terminal Display Reply
        /// </summary>
        public enum ETerminalDisplayReply
        {
            TERMINAL_DISPLAY_REPLY_ACCEPTED = 0,
            TERMINAL_DISPLAY_REPLY_ERROR
        };

        /// <summary>
        /// Alarm State
        /// </summary>
        public enum EAlarmState
        {
            ALARM_STATE_SET = 1,
            ALARM_STATE_RESET
        };

        /// <summary>
        /// EC Set Reply
        /// </summary>
        public enum ECSetReply
        {
            EC_SET_REPLY_ACCEPTED = 0,
            EC_SET_REPLY_ILLEGAL_VALUE
        };

        /// <summary>
        /// Reader Result Code
        /// </summary>
        public static class ReaderResultCode
        {
            /// <summary>
            /// Reader 정상으로 값을 읽은 값
            /// </summary>
            public static string OK = "0";
            /// <summary>
            /// Reader 비정상으로 값을 읽은 값
            /// </summary>
            public static string ERROR = "1";
            /// <summary>
            /// H/W 기능 미사용
            /// </summary>
            public static string MCR_READ_OFF = "2";
            /// <summary>
            /// Cell Tracking 기능 미사용
            /// </summary>
            public static string VALID_FUNC_OFF = "3";
        }

        /// <summary>
        /// Process State
        /// </summary>
        public enum EProcessState
        {
            STS_UNKNOW = 0,
            STS_ERROR,
            STS_READY,
            STS_RECEIVED,
            STS_FINAL
        };

        public enum ELotInfoReceiveStatus
        {
            NONE = 0,
            TIME_OUT,
            FAIL,
            PASS,
        }

        /// <summary>
        /// 메시지 종류
        /// </summary>
        public enum EMessageType
        {
            MESSAGE_TYPE_OP_CALL = 0,
            MESSAGE_TYPE_INTERLOCK_BUFFERING,
            MESSAGE_TYPE_INTERLOCK,
            MESSAGE_TYPE_TERMINAL,
            MESSAGE_TYPE_UNIT_INTERLOCK_BUFFERING,
            MESSAGE_TYPE_UNIT_INTERLOCK,
        };

        /// <summary>
        /// PPID Param List
        /// </summary>
        public enum EPpidParamList
        {
            CELL_WIDTH,
            CELL_HEIGHT,
        };

        public enum EECParamList
        {
            ECID,
            EC_ITEM_NAME,
            ECDEF,
            ECSLL,
            ECSUL,
            ECWLL,
            ECWUL,
            UNIT,
            DATA_TYPE,
            DECIMAL_POINT_POSITION,
        };

        public enum EECNameList
        {
            AXIS_X_SHUTTLE_REPEATABILITY,
            AXIS_REVIEW_STAGE_REPEATABILITY,
            MAX_COUNT,
        };

        public struct structureCIMInitialize
        {
            public object objCommunicator;
            public object objDocument;
        }

        public static IReadOnlyDictionary<EPpidParamList, RecipeParameter> RECIPE_PARAMS { get { return mRecipeParams; } }
        private static readonly Dictionary<EPpidParamList, RecipeParameter> mRecipeParams = new Dictionary<EPpidParamList, RecipeParameter>()
        {
            { EPpidParamList.CELL_WIDTH, new RecipeParameter(EPpidParamList.CELL_WIDTH.ToString(), "mm", 50d, 800d)},
            { EPpidParamList.CELL_HEIGHT, new RecipeParameter(EPpidParamList.CELL_HEIGHT.ToString(), "mm", 50d, 800d)},
        };

        /// <summary>
        /// CIM 레시피 파라메터
        /// </summary>
        public class RecipeParameter
        {
            /// <summary>
            /// 파라메터 이름
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// 파라메터 유닛
            /// </summary>
            public string Unit { get; private set; }
            /// <summary>
            /// 범위 확인 사용 여부 (문자열 파라메터의 경우 범위 확인을 하지 않음)
            /// </summary>
            public bool IsUseRangeCheck { get; private set; }
            /// <summary>
            /// 최대값
            /// </summary>
            public double MaxValue { get; private set; }
            /// <summary>
            /// 최소값
            /// </summary>
            public double MinValue { get; private set; }

            /// <summary>
            /// 생성자
            /// </summary>
            /// <param name="name"></param>
            /// <param name="unit"></param>
            /// <param name="minValue"></param>
            /// <param name="maxValue"></param>
            internal RecipeParameter(string name, string unit, double minValue = double.MinValue, double maxValue = double.MinValue)
            {
                Name = name;
                Unit = unit;
                MaxValue = maxValue;
                MinValue = minValue;
                IsUseRangeCheck = (
                    double.MinValue != maxValue
                    && double.MinValue != minValue
                    );
            }

            /// <summary>
            /// 입력된 값이 허용하는 범위 내에 있는지 체크한다.
            /// </summary>
            /// <param name="value">확인 할 값</param>
            /// <returns>true = 범위 내 값</returns>
            public bool CheckInRange(string value)
            {
                bool bResult = false;

                do
                {
                    if (true == IsUseRangeCheck)
                    {
                        double checkValue;
                        if (
                            false == double.TryParse(value, out checkValue)
                            || MaxValue < checkValue
                            || MinValue > checkValue
                            )
                        {
                            break;
                        }
                    }

                    bResult = true;
                } while (false);

                return bResult;
            }

            /// <summary>
            /// 파라메터의 범위를 문자열로 표시한다
            /// </summary>
            /// <returns></returns>
            public string GetRangeString()
            {
                return IsUseRangeCheck ? string.Format("Range : {0} ~ {1}", MinValue, MaxValue) : "-";
            }
        }
    }

    // Data 
    #region Date And Time Set Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class DateAndTimeSetRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _DateAndTimeSetRequest BODY;
        // 데이터 초기화
        public DateAndTimeSetRequest()
        {
            this.EQPID = " ";
            this.NAME = "DATEANDTIMESETREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.TIME = " ";
        }
        // Body 관련 구조체
        public struct _DateAndTimeSetRequest
        {
            public string TIME; // YYYYMMDDhhmmss
        }
    }
    #endregion
    #region Date And Time Set Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class DateAndTimeSetReply
    {
        // 데이터 초기화
        public DateAndTimeSetReply()
        {
            this.EQPID = " ";
            this.NAME = "DATEANDTIMESETREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.TIACK = " ";
        }

        // Body 관련 구조체
        public struct _DateAndTimeSetReply
        {
            public string TIACK;//0: Accepted, 1 Format Error
        }

        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _DateAndTimeSetReply BODY;
    }
    #endregion

    #region EQ State Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class EQStateRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public EQStateRequest()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATEREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion
    #region EQ STATE REPLY
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class EQStateReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _EQStateReply BODY;

        // Body 관련 구조체
        public struct _EQStateReply
        {
            public string AVAILABILITYSTATE;//1: DOWN, 2: UP
            public string INTERLOCKSTATE;//1: ON, 2: OFF
            public string MOVESTATE;//1: PAUSE, 2: RUNNING
            public string RUNSTATE;//1: IDLE, 2: RUN
            public string FRONTSTATE;//1: DOWN, 2: UP
            public string REARSTATE;//1: DOWN, 2: UP
            public string PP_SPLSTATE;//1: PP/SPL, 2: NORMAL
        }

        // 데이터 초기화
        public EQStateReply()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATEREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.AVAILABILITYSTATE = " ";
            this.BODY.INTERLOCKSTATE = " ";
            this.BODY.MOVESTATE = " ";
            this.BODY.RUNSTATE = " ";
            this.BODY.FRONTSTATE = " ";
            this.BODY.REARSTATE = " ";
            this.BODY.PP_SPLSTATE = " ";
        }
    }
    #endregion

    #region Unit State Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class UnitStateReply
    {
        // 데이터 초기화
        public UnitStateReply()
        {
            this.EQPID = " ";
            this.NAME = "UNITSTATEREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.AVAILABILITYSTATE = " ";
            this.BODY.INTERLOCKSTATE = " ";
            this.BODY.MOVESTATE = " ";
            this.BODY.RUNSTATE = " ";
            this.BODY.FRONTSTATE = " ";
            this.BODY.REARSTATE = " ";
            this.BODY.PP_SPLSTATE = " ";
            this.BODY.UNITS = new _UnitStateReply.UNIT[Enum.GetNames(typeof(CCIMDefine.EUnit)).Length];
            for (int iLoopCount = 0; iLoopCount < Enum.GetNames(typeof(CCIMDefine.EUnit)).Length; iLoopCount++)
            {
                this.BODY.UNITS[iLoopCount].AVAILABILITYSTATE = " ";
                this.BODY.UNITS[iLoopCount].INTERLOCKSTATE = " ";
                this.BODY.UNITS[iLoopCount].MOVESTATE = " ";
                this.BODY.UNITS[iLoopCount].RUNSTATE = " ";
                this.BODY.UNITS[iLoopCount].FRONTSTATE = " ";
                this.BODY.UNITS[iLoopCount].REARSTATE = " ";
                this.BODY.UNITS[iLoopCount].PP_SPLSTATE = " ";
            }
        }

        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _UnitStateReply BODY;

        // Body 관련 구조체
        public struct _UnitStateReply
        {
            public string AVAILABILITYSTATE;//1: DOWN, 2: UP
            public string INTERLOCKSTATE;//1: ON, 2: OFF
            public string MOVESTATE;//1: PAUSE, 2: RUNNING
            public string RUNSTATE;//1: IDLE, 2: RUN
            public string FRONTSTATE;//1: DOWN, 2: UP
            public string REARSTATE;//1: DOWN, 2: UP
            public string PP_SPLSTATE;//1: PP/SPL, 2: NORMAL
            public UNIT[] UNITS;
            public struct UNIT
            {
                public string UNITID;
                public string AVAILABILITYSTATE;//1: DOWN, 2: UP
                public string INTERLOCKSTATE;//1: ON, 2: OFF
                public string MOVESTATE;//1: PAUSE, 2: RUNNING
                public string RUNSTATE;//1: IDLE, 2: RUN
                public string FRONTSTATE;//1: DOWN, 2: UP
                public string REARSTATE;//1: DOWN, 2: UP
                public string PP_SPLSTATE;//1: PP/SPL, 2: NORMAL
            }
        }
    }
    #endregion
    #region Unit State Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class UnitStateRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public UnitStateRequest()
        {
            this.EQPID = " ";
            this.NAME = "UNITSTATEREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion

    #region Function State Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class FunctionStateRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public FunctionStateRequest()
        {
            this.EQPID = " ";
            this.NAME = "FUNCTIONSTATEREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion
    #region Function State Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class FunctionStateReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _FunctionStateReply BODY;

        // 데이터 초기화
        public FunctionStateReply()
        {
            this.EQPID = " ";
            this.NAME = "FUNCTIONSTATEREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.FUNCTIONS = new _FunctionStateReply.FUNCTION[10]; // Function List는 10개 있음.
            for (int iLoopCount = 0; iLoopCount < 10; iLoopCount++)
            {
                this.BODY.FUNCTIONS[iLoopCount].EFID = string.Format("{0}", iLoopCount + 1);
                this.BODY.FUNCTIONS[iLoopCount].EFST = " ";
            }
        }

        // Body 관련 구조체
        public struct _FunctionStateReply
        {
            public FUNCTION[] FUNCTIONS;

            public struct FUNCTION
            {
                public string EFID;
                public string EFST;
            }
        }
    }
    #endregion

    #region Conveyor Move State Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ConveyorMoveStateRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public ConveyorMoveStateRequest()
        {
            this.EQPID = " ";
            this.NAME = "CONVEYORMOVESTATEREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion
    #region Conveyor Move State Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ConveyorMoveStateReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _ConveyorMoveStateReply BODY;

        // 데이터 초기화
        public ConveyorMoveStateReply()
        {
            this.EQPID = " ";
            this.NAME = "CONVEYORMOVESTATEREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.MOVESTATE = " ";
            this.BODY.STOPREASON = " ";
        }

        // Body 관련 구조체
        public struct _ConveyorMoveStateReply
        {
            public string MOVESTATE; // 1 : STOP, 2 : MOVE
            public string STOPREASON; // 1 : Run 대기, 2 : 알람정지, 3 : 작업자정지, 4 : 유실방지, 5 : 하류 설비 진행불가.
        }
    }
    #endregion

    #region Current Alarm List Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CurrentAlarmListRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public CurrentAlarmListRequest()
        {
            this.EQPID = " ";
            this.NAME = "CURRENTALARMLISTREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion
    #region Current Alarm List Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CurrentAlarmListReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CurrentAlarmListReply BODY;
        // 데이터 초기화
        public CurrentAlarmListReply()
        {
            this.EQPID = " ";
            this.NAME = "CURRENTALARMLISTREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.ALID = " ";
        }

        // Body 관련 구조체
        public struct _CurrentAlarmListReply
        {
            public string ALID; // AlarmID 리스트를 ,로 구분 
        }
    }
    #endregion

    #region EC List Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ECListRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public ECListRequest()
        {
            this.EQPID = " ";
            this.NAME = "ECLISTREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion
    #region EC List Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ECListReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _ECListReply BODY;
        // 데이터 초기화
        public ECListReply()
        {
            this.EQPID = " ";
            this.NAME = "ECLISTREPLY";
            this.TRANSACTIONNO = "0";
        }
        // Body 관련 구조체
        public struct _ECListReply
        {
            public struct EC
            {
                public string ECID;
                public string ECNAME;
                public string ECDEF; // Equipment Constant Set Value
                public string ECSLL; // Equipment Constant Stop Low Limit
                public string ECSUL; // Equipment Constant Stop Upper Limit
                public string ECWLL; // Equipment Constant Warning Low Limit
                public string ECWUL; // Equipment Constant Warning Upper Limit
            }
            public EC[] ECS;
        }
    }
    #endregion

    #region Current PPID Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CurrentPPIDRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public CurrentPPIDRequest()
        {
            this.EQPID = " ";
            this.NAME = "CURRENTPPIDREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion
    #region Current PPID Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CurrentPPIDReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CurrentPPIDReply BODY;
        // 데이터 초기화
        public CurrentPPIDReply()
        {
            this.EQPID = " ";
            this.NAME = "CURRENTPPIDREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.PPID = " ";
            this.BODY.PPID_TYPE = " ";
        }
        // Body 관련 구조체
        public struct _CurrentPPIDReply
        {
            public string PPID;
            public string PPID_TYPE;
        }
    }
    #endregion

    #region Control State Set Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ControlStateSetRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _ControlStateSetRequest BODY;
        // 데이터 초기화
        public ControlStateSetRequest()
        {
            this.EQPID = " ";
            this.NAME = "CONTROLSTATESETREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.CRST = "0";
        }
        // Body 관련 구조체
        public struct _ControlStateSetRequest
        {
            public string CRST; //0: OFFLINE, 1: ONLINEREMOTE, 2: ONLINELOCAL
        }
    }
    #endregion
    #region Control State Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ControlStateSetReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _ControlStateSetReply BODY;
        // 데이터 초기화
        public ControlStateSetReply()
        {
            this.EQPID = " ";
            this.NAME = "CONTROLSTATESETREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.TIACK = " ";
        }

        // Body 관련 구조체
        public struct _ControlStateSetReply
        {
            public string TIACK; // 0: Accepted, 1: Special Reason, 6: Not Supported, 7: Equipment Busy
        }
    }
    #endregion

    #region Cell Job Process Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CellJobProcessRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CellJobProcessRequest BODY;

        // 데이터 초기화
        public CellJobProcessRequest()
        {
            this.EQPID = " ";
            this.NAME = "CELLJOBPROCESSREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.RCMD = " "; // 21: Start, 22: Cancel, 23: Pause, 24: Resume
            this.BODY.JOBID = " ";
            this.BODY.CELLID = " ";
            this.BODY.PRODUCTID = " ";
            this.BODY.STEPID = " ";
            this.BODY.ACTIONTYPE = " ";
        }

        [Serializable]
        public struct _CellJobProcessRequest
        {
            public string RCMD; // 21: Start, 22: Cancel, 23: Pause, 24: Resume
            public string JOBID;
            public string CELLID;
            public string PRODUCTID;
            public string STEPID;
            public string ACTIONTYPE;
        }
    }
    #endregion
    #region Cell Job Process Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CellJobProcessReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CellJobProcessReply BODY;
        // 데이터 초기화
        public CellJobProcessReply()
        {
            this.EQPID = " ";
            this.NAME = "CELLJOBPROCESSREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.HCACK = " ";
        }
        [Serializable]
        public struct _CellJobProcessReply
        {
            public string HCACK; // 0:Accepted, 1:Invalid Job ID, 2:Invalid Cell ID, 3:Reject(Already in desired condition), 4: Other Error
        }
    }
    #endregion

    #region PPID List Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PPIDListRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PPIDListRequest BODY;
        // 데이터 초기화
        public PPIDListRequest()
        {
            this.EQPID = " ";
            this.NAME = "PPIDLISTREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.PPID_TYPE = " ";
        }
        public struct _PPIDListRequest
        {
            public string PPID_TYPE;
        }
    }
    #endregion
    #region PPID List Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PPListReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PPListReply BODY;
        // 데이터 초기화
        public PPListReply()
        {
            this.EQPID = " ";
            this.NAME = "PPIDLISTREPLY";
            this.TRANSACTIONNO = "0";
        }
        // Body 관련 구조체
        public struct _PPListReply
        {
            [XmlArray("PPIDS")]
            [XmlArrayItem("PPID")]
            public List<string> PPIDS;
        }
    }
    #endregion

    #region PP Param request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PPParamRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PPParamRequest BODY;
        // 데이터 초기화
        public PPParamRequest()
        {
            this.EQPID = " ";
            this.NAME = "PPPARAMREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.PPID = " ";
            this.BODY.PPID_TYPE = " ";
        }
        public struct _PPParamRequest
        {
            public string PPID;
            public string PPID_TYPE;
        }
    }
    #endregion
    #region PP Param Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PPParamReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PPParamReply BODY;
        // 데이터 초기화
        public PPParamReply()
        {
            this.EQPID = " ";
            this.NAME = "PPPARAMREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.SOFTREV = " ";
            this.BODY.CCODE = " ";
        }
        // Body 관련 구조체
        public struct _PPParamReply
        {
            public string SOFTREV; // 설비의 Software version
            public string CCODE;
            public PARAM[] PARAMS;
            public struct PARAM
            {
                public string PARAMNAME;
                public string PARAMVALUE;
            }
        }
    }
    #endregion

    #region OP Call request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class OpCallRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _OpCallRequest BODY;
        // 데이터 초기화
        public OpCallRequest()
        {
            this.EQPID = " ";
            this.NAME = "OPCALLREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.OPCALL = " ";
            this.BODY.OPCALLID = " ";
            this.BODY.MESSAGE = " ";
        }
        public struct _OpCallRequest
        {
            public string OPCALL;
            public string OPCALLID;
            public string MESSAGE;
        }
    }
    #endregion
    #region OP CALL Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class OpCallReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _OpCallReply BODY;
        // 데이터 초기화
        public OpCallReply()
        {
            this.EQPID = " ";
            this.NAME = "OPCALLREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.HCACK = " ";
        }
        // Body 관련 구조체
        public struct _OpCallReply
        {
            public string HCACK; // 0:Accepted, 1:Invalid Cell ID, 2:Not Supported, 3:Rejected, 4: Other Error
        }
    }
    #endregion

    #region InterlockRequest
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class InterlockRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _InterlockRequest BODY;
        // 데이터 초기화
        public InterlockRequest()
        {
            this.EQPID = " ";
            this.NAME = "INTERLOCKREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.INTERLOCK = " ";
            this.BODY.INTERLOCKID = " ";
            this.BODY.MESSAGE = " ";
        }
        public struct _InterlockRequest
        {
            public string INTERLOCK;
            public string INTERLOCKID;
            public string MESSAGE;
        }
    }
    #endregion
    #region Interlock Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class InterlockReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _InterlockReply BODY;
        // 데이터 초기화
        public InterlockReply()
        {
            this.EQPID = " ";
            this.NAME = "INTERLOCKREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.HCACK = " ";
        }
        // Body 관련 구조체
        public struct _InterlockReply
        {
            public string HCACK; // 0:Accepted, 1:Invalid Cell ID, 2:Not Supported, 3:Rejected, 4: Other Error
        }
    }

    #endregion

    #region Function Change Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class FunctionChangeRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _FunctionChangeRequest BODY;
        // 데이터 초기화
        public FunctionChangeRequest()
        {
            this.EQPID = " ";
            this.NAME = "FUNCTIONCHANGEREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.UNITID = " ";
            this.BODY.EFID = " ";
            this.BODY.EFST = " ";
            this.BODY.MESSAGE = " ";
        }
        public struct _FunctionChangeRequest
        {
            public string UNITID;
            public string EFID;
            public string EFST;
            public string MESSAGE;
        }
    }
    #endregion
    #region Function Change Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class FunctionChangeReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _FunctionChangeReply BODY;
        // 데이터 초기화
        public FunctionChangeReply()
        {
            this.EQPID = " ";
            this.NAME = "FUNCTIONCHANGEREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.HCACK = " ";
        }
        // Body 관련 구조체
        public struct _FunctionChangeReply
        {
            public string HCACK; // 0:Accepted, 1:Invalid Cell ID, 2:Not Supported, 3:Rejected, 4: Other Error
        }
    }
    #endregion

    #region Unit Interlock Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class UnitInterlockRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _UnitInterlockRequest BODY;
        // 데이터 초기화
        public UnitInterlockRequest()
        {
            this.EQPID = " ";
            this.NAME = "UNITINTERLOCKREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.RCMD = " ";
            this.BODY.INTERLOCK = " ";
            this.BODY.UNITID = " ";
            this.BODY.INTERLOCKID = " ";
            this.BODY.MESSAGE = " ";
        }
        public struct _UnitInterlockRequest
        {
            public string RCMD;
            public string INTERLOCK;
            public string UNITID;
            public string INTERLOCKID;
            public string MESSAGE;
        }
    }
    #endregion
    #region Unit Interlock Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class UnitInterlockReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _UnitInterlockReply BODY;
        // 데이터 초기화
        public UnitInterlockReply()
        {
            this.EQPID = " ";
            this.NAME = "UNITINTERLOCKREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.RCMD = " ";
            this.BODY.HCACK = " ";
        }
        // Body 관련 구조체
        public struct _UnitInterlockReply
        {
            public string RCMD; // 12 : Loading Stop
            public string HCACK; // 0:Accepted, 1:Invalid Cell ID, 2:Not Supported, 3:Rejected, 4: Other Error
        }
    }
    #endregion

    #region Terminal Display Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class TerminalDisplayRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _TerminalDisplayRequest BODY;
        // 데이터 초기화
        public TerminalDisplayRequest()
        {
            this.EQPID = " ";
            this.NAME = "TERMINALDISPLAYREQUEST";
            this.TRANSACTIONNO = "0";
        }
        public struct _TerminalDisplayRequest
        {
            public string TID;
            public _TEXT[] TEXTS;
            public struct _TEXT
            {
                public string TEXT;
            }
        }
    }
    #endregion
    #region Termina Display Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class TerminalDisplayReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _TerminalDisplayReply BODY;
        // 데이터 초기화
        public TerminalDisplayReply()
        {
            this.EQPID = " ";
            this.NAME = "TERMINALDISPLAYREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.ACKC10 = " ";
        }
        // Body 관련 구조체
        public struct _TerminalDisplayReply
        {
            public string ACKC10; // 0:Accepted, 1:Error
        }
    }
    #endregion

    #region EC Set Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ECSetRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _ECSetRequest BODY;
        // 데이터 초기화
        public ECSetRequest()
        {
            this.EQPID = " ";
            this.NAME = "ECSETREQUEST";
            this.TRANSACTIONNO = "0";
        }

        public struct _ECSetRequest
        {
            public struct _ECS
            {
                public struct _EC
                {
                    public string ECID;
                    public string ECNAME;
                    public string ECDEF;
                    public string ECSLL;
                    public string ECSUL;
                    public string ECWLL;
                    public string ECWUL;
                }
                public _EC[] EC;
            }
            public _ECS ECS;
        }
    }
    #endregion
    #region EC SET Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ECSetReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _ECSetReply BODY;
        // 데이터 초기화
        public ECSetReply()
        {
            this.EQPID = " ";
            this.NAME = "ECSETREPLY";
            this.TRANSACTIONNO = "0";
        }
        // Body 관련 구조체
        public struct _ECSetReply
        {
            public struct _ECS
            {
                public struct _EC
                {
                    public string ECID;
                    public string ECNAME;
                    public string ECDEF_ACK; // 0:Accepted, 1:Illegal Value
                    public string ECSLL_ACK; // 0:Accepted, 1:Illegal Value
                    public string ECSUL_ACK; // 0:Accepted, 1:Illegal Value
                    public string ECWLL_ACK; // 0:Accepted, 1:Illegal Value
                    public string ECWUL_ACK; // 0:Accepted, 1:Illegal Value
                }

                public _EC[] EC;
            }

            public _ECS ECS;
        }
    }
    #endregion

    #region Trace Start Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class TraceStartRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _TraceStartRequest BODY;
        // 데이터 초기화
        public TraceStartRequest()
        {
            this.EQPID = " ";
            this.NAME = "TRACESTARTREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.INTERVALTIME = "1000";
        }
        // Body 관련 구조체
        public struct _TraceStartRequest
        {
            public string INTERVALTIME;
        }
    }
    #endregion
    #region Trace Start Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class TraceStartReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _TraceStartReply BODY;
        // 데이터 초기화
        public TraceStartReply()
        {
            this.EQPID = " ";
            this.NAME = "TRACESTARTREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.RESULT = " ";
            this.BODY.REASON = " ";
        }
        // Body 관련 구조체
        public struct _TraceStartReply
        {
            public string RESULT;
            public string REASON; // Result가 Ng인 경우 왜 Ng인지 쓰자. 
        }
    }
    #endregion

    #region Trace Stop Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class TraceStopRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public TraceStopRequest()
        {
            this.EQPID = " ";
            this.NAME = "TRACESTOPREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion
    #region Trace Stop Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class TraceStopReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _TraceStopReply BODY;
        // 데이터 초기화
        public TraceStopReply()
        {
            this.EQPID = " ";
            this.NAME = "TRACESTOPREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.RESULT = " ";
            this.BODY.REASON = " ";
        }
        // Body 관련 구조체
        public struct _TraceStopReply
        {
            public string RESULT;
            public string REASON; // Result가 Ng인 경우 왜 Ng인지 쓰자. 
        }
    }
    #endregion

    #region Trace Data Report
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class TraceDataReport
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _TraceDataReport BODY;
        // 데이터 초기화
        public TraceDataReport()
        {
            this.EQPID = " ";
            this.NAME = "TRACEDATAREPORT";
            this.TRANSACTIONNO = "0";
            this.BODY.SV = " ";
        }
        // Body 관련 구조체
        public struct _TraceDataReport
        {
            public string SV;// 모든 SV값을 ,구분자를 사용하여 일괄 보고
        }
    }
    #endregion

    #region Cell Infomation Event Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CellInfomationEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CellInfomationEvent BODY;

        // 데이터 초기화
        public CellInfomationEvent()
        {
            this.EQPID = " ";
            this.NAME = "CELLINFOREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.CARRIERID = " ";
            this.BODY.CELLID = " ";
        }

        // Body 관련 구조체
        [Serializable]
        public struct _CellInfomationEvent
        {
            public string CARRIERID;
            public string CELLID;
        }
    }
    #endregion
    #region Cell Infomation Event Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CellInfomationEventReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CellInfomationEventReply BODY;
        // 데이터 초기화
        public CellInfomationEventReply()
        {
            this.EQPID = " ";
            this.NAME = "CELLINFOREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.HCACK = " ";
        }
        // Body 관련 구조체
        public struct _CellInfomationEventReply
        {
            public string HCACK; // 0:Accepted, 1:Invalid Cell ID, 2:Not Supported, 3:Rejected, 4: Other Error
        }
    }
    #endregion
    #region Cell Infomation Download Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CellInfomationDownload
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CellInfomationDownload BODY;

        // 데이터 초기화
        public CellInfomationDownload()
        {
            this.EQPID = " ";
            this.NAME = "CELLINFOREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.CARRIERID = " ";
            this.BODY.CELLID = " ";
            this.BODY.PRODUCTID = " ";
            this.BODY.CELLINFORESULT = " ";
        }

        // Body 관련 구조체
        [Serializable]
        public struct _CellInfomationDownload
        {
            public string CARRIERID;
            public string CELLID;
            public string PRODUCTID;
            public string CELLINFORESULT;
        }
    }
    #endregion


    #region Carrier Validation CheckRequest
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CarrierValidationCheckRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CarrierValidationCheckRequest BODY;
        // 데이터 초기화
        public CarrierValidationCheckRequest()
        {
            this.EQPID = " ";
            this.NAME = "CARRIERVALIDATIONREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.CARRIERID = " ";
            this.BODY.OPTIONCODE = " ";
            this.BODY.READERID = " ";
        }
        // Body 관련 구조체
        public struct _CarrierValidationCheckRequest
        {
            public string CARRIERID;
            public string OPTIONCODE;
            public string READERID;//MCR ID (예: 1, 2, …)
        }
    }
    #endregion
    #region Carrier Validation CheckRequest Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CarrierValidationCheckRequestReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CarrierValidationCheckRequestReply BODY;
        // 데이터 초기화
        public CarrierValidationCheckRequestReply()
        {
            this.EQPID = " ";
            this.NAME = "CARRIERVALIDATIONREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.HCACK = " ";
        }
        // Body 관련 구조체
        public struct _CarrierValidationCheckRequestReply
        {
            public string HCACK; // 0:Accepted, 1:Invalid Cell ID, 2:Not Supported, 3:Rejected, 4: Other Error
        }
    }
    #endregion
    #region Carrier Information Download
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CarrierInformationDownload
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CarrierInformationDownload BODY;
        // 데이터 초기화
        public CarrierInformationDownload()
        {
            this.EQPID = " ";
            this.NAME = "CARRIERINFODOWNLOAD";
            this.TRANSACTIONNO = "0";
            this.BODY.CARRIERID = " ";
            this.BODY.CELLID = " ";
            this.BODY.PRODUCTID = " ";
            this.BODY.PRODUCT_TYPE = " ";
            this.BODY.PRODUCT_KIND = " ";
            this.BODY.PRODUCTSPEC = " ";
            this.BODY.STEPID = " ";
            this.BODY.PPID = " ";
            this.BODY.CELL_SIZE = " ";
            this.BODY.CELL_THICKNESS = " ";
            this.BODY.COMMENT = " ";
            this.BODY.CELLINFORESULT = " ";
            this.BODY.INS_COUNT = " ";
            this.BODY.REPLYSTATUS = " ";
        }
        // Body 관련 구조체
        public struct _CarrierInformationDownload
        {
            public string CARRIERID;
            public string CELLID;
            public string PRODUCTID;
            public string PRODUCT_TYPE;
            public string PRODUCT_KIND;
            public string PRODUCTSPEC;
            public string STEPID;
            public string PPID;
            public string CELL_SIZE;
            public string CELL_THICKNESS;
            public string COMMENT;
            public string CELLINFORESULT;
            public string INS_COUNT;
            public string REPLYSTATUS;
            [XmlArray("ITEMS")]
            [XmlArrayItem("ITEM")]
            public List<DVList> ITEM;
            public struct DVList
            {
                public string ITEM_NAME;
                public string ITEM_VALUE;
            }

        }
    }
    #endregion
    #region Message Event
    #region Cell Tracking Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CellTrackOutEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CellTrackOutEvent BODY;
        // 데이터 초기화
        public CellTrackOutEvent()
        {
            this.EQPID = " ";
            this.NAME = "CELLEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.EVENT = " ";//string.Format("{0}", (int)CCIMDefine.CellEvent.CELL_EVENT_PROCESS_END);
            this.BODY.UNITID = " ";
            this.BODY.UNITST = " ";
            this.BODY.EQST.MOVESTATE = " ";
            this.BODY.EQST.RUNSTATE = " ";
            this.BODY.EQST.REASONCODE = " ";
            this.BODY.EQST.DESCRIPTION = " ";
            this.BODY.CELL.CARRIERID = " ";
            this.BODY.CELL.CARRIERID = " ";
            this.BODY.CELL.CELLID = " ";
            this.BODY.CELL.PPID = " ";
            this.BODY.CELL.PRODUCTID = " ";
            this.BODY.CELL.PRODUCTKIND = " ";
            this.BODY.CELL.PRODUCTSPEC = " ";
            this.BODY.CELL.PRODUCTTYPE = " ";
            this.BODY.CELL.STEPID = " ";
            this.BODY.CELL.CELLSIZE = " ";
            this.BODY.CELL.CELLTHICKNESS = " ";
            this.BODY.CELL.COMMENT = " ";
            this.BODY.CELL.CELLINFORESULT = " ";
            this.BODY.CELL.REPLYSTATUS = " ";
            this.BODY.WORKORDER.PROCESSJOB = " ";
            this.BODY.WORKORDER.PLANQTY = " ";
            this.BODY.WORKORDER.PROCESSEDQTY = " ";
            this.BODY.READER.READERID = " ";
            this.BODY.READER.READERRESULTCODE = " ";
            this.BODY.JUDGEMENT.OPERATORID = " ";
            this.BODY.JUDGEMENT.JUDGE = " ";
            this.BODY.JUDGEMENT.REASONCODE = " ";
            this.BODY.JUDGEMENT.DESCRIPTION = " ";
        }

        // Body 관련 구조체
        public struct _CellTrackOutEvent
        {
            public string EVENT; // 1: In, 2: Out, 3: Process Start, 4: Process End, 5: Scrap, 6: Unscrap, 7: Carrier Assign, 8: Carrier Release, 9: Confirm Out
            public string UNITID;
            public string UNITST; // 0: IDLE, 1: RUN, 2: DOWN
            public _EQST EQST;
            public _CELL CELL;
            public _WORKORDER WORKORDER;
            public _READER READER;
            [XmlArray("DVS")]
            [XmlArrayItem("DV")]
            public List<DVList> DV; // Event : 4인 경우만 해당.
            public _JUDGEMENT JUDGEMENT; // Event : 4,5,6인 경우만 해당.

            public struct _EQST
            {
                public string MOVESTATE;
                public string RUNSTATE;
                public string REASONCODE;
                public string DESCRIPTION;
            }

            public struct _CELL
            {
                public string CARRIERID;
                public string CELLID;
                public string PPID;
                public string PRODUCTID;
                public string STEPID;
                public string PRODUCTTYPE;
                public string PRODUCTKIND;
                public string PRODUCTSPEC;
                public string CELLSIZE;
                public string CELLTHICKNESS;
                public string COMMENT;
                public string CELLINFORESULT;
                public string REPLYSTATUS;
            }

            public struct _WORKORDER
            {
                public string PROCESSJOB;
                public string PLANQTY;
                public string PROCESSEDQTY;
            }

            public struct _READER
            {
                public string READERID;
                public string READERRESULTCODE;
            }

            public struct DVList
            {
                public string DVNAME;
                public string DVVAL;
            }

            public struct _JUDGEMENT
            {
                public string OPERATORID;
                public string JUDGE;
                public string REASONCODE;
                public string DESCRIPTION;
            }
        }
    }
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CellTrackInEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CellTrackInEvent BODY;

        // 데이터 초기화
        public CellTrackInEvent()
        {
            this.EQPID = " ";
            this.NAME = "CELLEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.EVENT = " ";
            this.BODY.UNITID = " ";
            this.BODY.UNITST = " ";
            this.BODY.EQST.MOVESTATE = " ";
            this.BODY.EQST.RUNSTATE = " ";
            this.BODY.EQST.REASONCODE = " ";
            this.BODY.EQST.DESCRIPTION = " ";
            this.BODY.CELL.CARRIERID = " ";
            this.BODY.CELL.CARRIERID = " ";
            this.BODY.CELL.CELLID = " ";
            this.BODY.CELL.PPID = " ";
            this.BODY.CELL.PRODUCTID = " ";
            this.BODY.CELL.PRODUCTKIND = " ";
            this.BODY.CELL.PRODUCTSPEC = " ";
            this.BODY.CELL.PRODUCTTYPE = " ";
            this.BODY.CELL.STEPID = " ";
            this.BODY.CELL.CELLSIZE = " ";
            this.BODY.CELL.CELLTHICKNESS = " ";
            this.BODY.CELL.COMMENT = " ";
            this.BODY.CELL.CELLINFORESULT = " ";
            this.BODY.CELL.REPLYSTATUS = " ";
            this.BODY.WORKORDER.PROCESSJOB = " ";
            this.BODY.WORKORDER.PLANQTY = " ";
            this.BODY.WORKORDER.PROCESSEDQTY = " ";
            this.BODY.READER.READERID = " ";
            this.BODY.READER.READERRESULTCODE = " ";
            this.BODY.JUDGEMENT.OPERATORID = " ";
            this.BODY.JUDGEMENT.JUDGE = " ";
            this.BODY.JUDGEMENT.REASONCODE = " ";
            this.BODY.JUDGEMENT.DESCRIPTION = " ";
        }

        // Body 관련 구조체
        [Serializable]
        public struct _CellTrackInEvent
        {
            public string EVENT; // 1: In, 2: Out, 3: Process Start, 4: Process End, 5: Scrap, 6: Unscrap, 7: Carrier Assign, 8: Carrier Release, 9: Confirm Out
            public string UNITID;
            public string UNITST; // 0: IDLE, 1: RUN, 2: DOWN
            public _EQST EQST;
            public _CELL CELL;
            public _WORKORDER WORKORDER;
            public _READER READER;
            [XmlArray("DVS")]
            [XmlArrayItem("DV")]
            public List<DVList> DV; // Event :4인 경우만 해당
            public _JUDGEMENT JUDGEMENT; // Event : 4,5,6인 경우만 해당.

            [Serializable]
            public struct _EQST
            {
                public string MOVESTATE;
                public string RUNSTATE;
                public string REASONCODE;
                public string DESCRIPTION;
            }

            [Serializable]
            public struct _CELL
            {
                public string CARRIERID;
                public string CELLID;
                public string PPID;
                public string PRODUCTID;
                public string STEPID;
                public string PRODUCTTYPE;
                public string PRODUCTKIND;
                public string PRODUCTSPEC;
                public string CELLSIZE;
                public string CELLTHICKNESS;
                public string COMMENT;
                public string CELLINFORESULT;
                public string REPLYSTATUS;
            }

            [Serializable]
            public struct _WORKORDER
            {
                public string PROCESSJOB;
                public string PLANQTY;
                public string PROCESSEDQTY;
            }

            [Serializable]
            public struct _READER
            {
                public string READERID;
                public string READERRESULTCODE;
            }

            [Serializable]
            public struct DVList
            {
                public string DVNAME;
                public string DVVAL;
            }

            [Serializable]
            public struct _JUDGEMENT
            {
                public string OPERATORID;
                public string JUDGE;
                public string REASONCODE;
                public string DESCRIPTION;
            }
        }
    }
    #endregion

    #region Inspection Result Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class InspectionResultEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _InspectionResultEvent BODY;
        // 데이터 초기화
        public InspectionResultEvent()
        {
            this.EQPID = " ";
            this.NAME = "INSPRESULTEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.CARRIERID = " ";
            this.BODY.CELLID = " ";
            this.BODY.PROCESSNAME = " ";
            this.BODY.PROCESSFLAG = " ";
            this.BODY.JUDGE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.OPERID = " ";
            //kjpark 20190123 V1.17 이상은 8개 필드 사용
            this.BODY.SENDUNIQUEINFO = " ";
            this.BODY.REVUNIQUEINFO = " ";
        }
        // Body 관련 구조체
        public struct _InspectionResultEvent
        {
            public string CARRIERID;
            public string CELLID;
            public string PROCESSNAME;
            public string PROCESSFLAG;  // Y: Finished inspection process successfully, N: Failed inspection process
            public string JUDGE;        // G: GOOD, N: NOGOOD, S: SCRAP, L: LOTLOSS, R: RETEST, O: OUT
            public string REASONCODE;
            public string OPERID;
            public string SENDUNIQUEINFO;
            //kjpark 20190123 V1.17 이상은 8개 필드 사용
            public string REVUNIQUEINFO;
        }
    }
    #endregion

    #region Recipe Remote Control
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PPChangeRemoteEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PPChangeRemoteEvent BODY;
        // 데이터 초기화
        public PPChangeRemoteEvent()
        {
            this.EQPID = " ";
            this.NAME = "SECUREDPPPARAMSETREQUEST";
            this.TRANSACTIONNO = "0";
            this.BODY.MODULEID = " ";
            this.BODY.PPID = " ";
            this.BODY.PPID_TYPE = " ";
            this.BODY.PPID_NUMBER = " ";
        }

        // Body 관련 구조체
        public struct _PPChangeRemoteEvent
        {
            public string MODULEID;
            public string PPID;
            public string PPID_TYPE;
            public string PPID_NUMBER;
            public COMMAND[] COMMANDS;

            public struct COMMAND
            {
                public string CCODE;
                public PARAM[] PARAMS;

                public struct PARAM
                {
                    public string PARAMNAME;
                    public string PARAMVALUE;
                }
            }
        }
    }
    #endregion
    #region Recipe Remote Control Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PPChangeRemoteEventReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PPChangeRemoteEventReply BODY;
        // 데이터 초기화
        public PPChangeRemoteEventReply()
        {
            this.EQPID = " ";
            this.NAME = "SECUREDPPPARAMSETREPLY";
            this.TRANSACTIONNO = "0";
            this.BODY.ACKC7 = " ";
        }

        // Body 관련 구조체
        public struct _PPChangeRemoteEventReply
        {
            public string ACKC7;
        }
    }
    #endregion

    #region PP Change Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PPChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PPChangeEvent BODY;
        // 데이터 초기화
        public PPChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "SECUREDPPCHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.MODE = " ";
            this.BODY.MODULEID = " ";
            this.BODY.PPID = " ";
            this.BODY.PPID_TYPE = " ";
            this.BODY.PPID_NUMBER = " ";
        }

        // Body 관련 구조체
        public struct _PPChangeEvent
        {
            public string MODE;         // 1:Create 2:Delete 3:Modify
            public string MODULEID;
            public string PPID;
            public string PPID_TYPE;
            public string PPID_NUMBER;
            public COMMAND[] COMMANDS;

            public struct COMMAND
            {
                public string CCODE;
                public PARAM[] PARAMS;

                public struct PARAM
                {
                    public string PARAMNAME;
                    public string PARAMVALUE;
                }
            }
        }
    }
    #endregion

    #region Current PPID Change Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CurrentPPIDChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CurrentPPIDChangeEvent BODY;
        // 데이터 초기화
        public CurrentPPIDChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "CURRENTPPIDCHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.PPID = " ";
            this.BODY.PPID_TYPE = " ";
            this.BODY.OLD_PPID = " ";
        }
        // Body 관련 구조체
        public struct _CurrentPPIDChangeEvent
        {
            public string PPID;         // 1:Create 2:Delete 3:Modify
            public string PPID_TYPE;
            public string OLD_PPID;
        }
    }
    #endregion

    #region Current PPID Param Change Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class CurrentPPIDParamChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _CurrentPPIDParamChangeEvent BODY;
        // 데이터 초기화
        public CurrentPPIDParamChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "CURRENTPPIDPARAMCHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.PPST.PPID = " ";
            this.BODY.PPST.PPIDST = " ";
        }
        // Body 관련 구조체
        public struct _CurrentPPIDParamChangeEvent
        {
            public _PPST PPST;
            public PARAM[] PARAMS;

            public struct _PPST
            {
                public string PPID;
                public string PPIDST; // 1:Emptied, 2:Initialized, 3:Changed, 4:Parameter Changed
            }

            public struct PARAM
            {
                public string PARAMNAME;
                public string PARAMVALUE;
            }
        }
    }
    #endregion

    #region Op Call Confirm Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class OpCallConfirmEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _OpCallConfirmEvent BODY;
        // 데이터 초기화
        public OpCallConfirmEvent()
        {
            this.EQPID = " ";
            this.NAME = "OPCALLCONFIRMEVENT";
            this.TRANSACTIONNO = "0";
        }
        // Body 관련 구조체
        public struct _OpCallConfirmEvent
        {
            public struct OPCALL
            {
                public string OPCALLID;
                public string MESSAGE;
            }

            public OPCALL[] OPCALLS;
        }
    }
    #endregion

    #region Interlock Confirm Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class InterlockConfirmEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _InterlockConfirmEvent BODY;
        // 데이터 초기화
        public InterlockConfirmEvent()
        {
            this.EQPID = " ";
            this.NAME = "INTERLOCKCONFIRMEVENT";
            this.TRANSACTIONNO = "0";
        }
        // Body 관련 구조체
        public struct _InterlockConfirmEvent
        {
            public struct InterlockConfirm
            {
                public string INTERLOCKID;
                public string MESSAGE;
            }
            public InterlockConfirm[] INTERLOCKS;
        }
    }
    #endregion

    #region EQ State Change Event - Interlock & Move
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class InterlockAndMoveStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _InterlockAndMoveStateChangeEvent BODY;
        // 데이터 초기화
        public InterlockAndMoveStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.INTERLOCKSTATE = " ";
            this.BODY.MOVESTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _InterlockAndMoveStateChangeEvent
        {
            public string INTERLOCKSTATE;//1: ON, 2: OFF
            public string MOVESTATE;//1: PAUSE, 2: RUNNING
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region Function Change Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class FunctionChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _FunctionChangeEvent BODY;
        // 데이터 초기화
        public FunctionChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "FUNCTIONCHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.FUNCTIONS = new _FunctionChangeEvent.FUNCTION[10];
            for (int iLoopCount = 0; iLoopCount < 10; iLoopCount++)
            {
                this.BODY.FUNCTIONS[iLoopCount].BYWHO = " ";
                this.BODY.FUNCTIONS[iLoopCount].EFID = " ";
                this.BODY.FUNCTIONS[iLoopCount].NEWEFST = " ";
                this.BODY.FUNCTIONS[iLoopCount].OLDEFST = " ";
            }
        }
        // Body 관련 구조체
        public struct _FunctionChangeEvent
        {
            public struct FUNCTION
            {
                public string BYWHO;
                public string EFID;
                public string NEWEFST;
                public string OLDEFST;
            }

            public FUNCTION[] FUNCTIONS;
        }
    }
    #endregion

    #region Unit Interlock Confirm Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class UnitInterlockConfirmEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _UnitInterlockConfirmEvent BODY;
        // 데이터 초기화
        public UnitInterlockConfirmEvent()
        {
            this.EQPID = " ";
            this.NAME = "UNITINTERLOCKCONFIRMEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.UNITID = " ";
            this.BODY.CELLID = " ";
            this.BODY.PPID = " ";
            this.BODY.PRODUCTID = " ";
            this.BODY.STEPID = " ";
        }
        // Body 관련 구조체
        public struct _UnitInterlockConfirmEvent
        {
            public struct INTERLOCK
            {
                public string INTERLOCKID;
                public string MESSAGE;
            }
            public string UNITID;
            public string CELLID;
            public string PPID;
            public string PRODUCTID;
            public string STEPID;
            public INTERLOCK[] INTERLOCKS;
        }
    }
    #endregion

    #region EC Change Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ECChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _ECChangeEvent BODY;
        // 데이터 초기화
        public ECChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "ECCHANGEEVENT";
            this.TRANSACTIONNO = "0";
        }
        // Body 관련 구조체
        public struct _ECChangeEvent
        {
            public struct EC
            {
                public string ECID;
                public string ECNAME;
                public string ECDEF;
                public string ECSLL;
                public string ECSUL;
                public string ECWLL;
                public string ECWUL;
            }

            public EC[] ECS;
        }
    }
    #endregion

    #region EQ State Change Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class EQStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _EQStateChangeEvent BODY;
        // 데이터 초기화
        public EQStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.AVAILABILITYSTATE = " ";
            this.BODY.INTERLOCKSTATE = " ";
            this.BODY.MOVESTATE = " ";
            this.BODY.RUNSTATE = " ";
            this.BODY.FRONTSTATE = " ";
            this.BODY.REARSTATE = " ";
            this.BODY.PP_SPLSTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _EQStateChangeEvent
        {
            public string AVAILABILITYSTATE;//1: DOWN, 2: UP
            public string INTERLOCKSTATE;//1: ON, 2: OFF
            public string MOVESTATE;//1: PAUSE, 2: RUNNING
            public string RUNSTATE;//1: IDLE, 2: RUN
            public string FRONTSTATE;//1: DOWN, 2: UP
            public string REARSTATE;//1: DOWN, 2: UP
            public string PP_SPLSTATE;//1: PP/SPL, 2: NORMAL
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region EQ State Change Event - Availability
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class AvailabilityStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _AvailabilityStateChangeEvent BODY;
        // 데이터 초기화
        public AvailabilityStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.AVAILABILITYSTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _AvailabilityStateChangeEvent
        {
            public string AVAILABILITYSTATE;//1: DOWN, 2: UP
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region EQ State Change Event - Interlock
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class InterlockStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _InterlockStateChangeEvent BODY;
        // 데이터 초기화
        public InterlockStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.INTERLOCKSTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _InterlockStateChangeEvent
        {
            public string INTERLOCKSTATE;//1: ON, 2: OFF
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region EQ State Change Event - Move State
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class MoveStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _MoveStateChangeEvent BODY;
        // 데이터 초기화
        public MoveStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.MOVESTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _MoveStateChangeEvent
        {
            public string MOVESTATE;//1: PAUSE, 2: RUNNING
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region EQ State Change Event - Run State
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class RunStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _RunStateChangeEvent BODY;
        // 데이터 초기화
        public RunStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.RUNSTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _RunStateChangeEvent
        {
            public string RUNSTATE;//1: IDLE, 2: RUN
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region EQ State Change Event - Front State
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class FrontStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _FrontStateChangeEvent BODY;
        // 데이터 초기화
        public FrontStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.FRONTSTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _FrontStateChangeEvent
        {
            public string FRONTSTATE;//1: DOWN, 2: UP
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region EQ State Change Event - Rear State
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class RearStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _RearStateChangeEvent BODY;
        // 데이터 초기화
        public RearStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.REARSTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _RearStateChangeEvent
        {
            public string REARSTATE;//1: DOWN, 2: UP
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region EQ State Change Event - PP_SPL State
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PP_SPLStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PP_SPLStateChangeEvent BODY;
        // 데이터 초기화
        public PP_SPLStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.PP_SPLSTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _PP_SPLStateChangeEvent
        {
            public string PP_SPLSTATE;//1: PP/SPL, 2: NORMAL
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region Unit State Change Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class UnitStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _UnitStateChangeEvent BODY;
        // 데이터 초기화
        public UnitStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "UNITSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.UNITID = " ";
            this.BODY.AVAILABILITYSTATE = " ";
            this.BODY.INTERLOCKSTATE = " ";
            this.BODY.MOVESTATE = " ";
            this.BODY.RUNSTATE = " ";
            this.BODY.FRONTSTATE = " ";
            this.BODY.REARSTATE = " ";
            this.BODY.PP_SPLSTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _UnitStateChangeEvent
        {
            public string UNITID;
            public string AVAILABILITYSTATE;//1: DOWN, 2: UP
            public string INTERLOCKSTATE;//1: ON, 2: OFF
            public string MOVESTATE;//1: PAUSE, 2: RUNNING
            public string RUNSTATE;//1: IDLE, 2: RUN
            public string FRONTSTATE;//1: DOWN, 2: UP
            public string REARSTATE;//1: DOWN, 2: UP
            public string PP_SPLSTATE;//1: PP/SPL, 2: NORMAL
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region Conveyor Move State Change Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class ConveyorMoveStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _ConveyorMoveStateChangeEvent BODY;
        // 데이터 초기화
        public ConveyorMoveStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "CONVEYORMOVESTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.MOVESTATE = " ";
            this.BODY.STOPREASON = " ";
        }
        // Body 관련 구조체
        public struct _ConveyorMoveStateChangeEvent
        {
            public string MOVESTATE;
            public string STOPREASON;
        }
    }
    #endregion

    #region Alarm Report
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class AlarmReport
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _AlarmReport BODY;
        // 데이터 초기화
        public AlarmReport()
        {
            this.EQPID = " ";
            this.NAME = "ALARMREPORT";
            this.TRANSACTIONNO = "0";
            this.BODY.ALID = " ";
            this.BODY.ALST = " ";
        }
        // Body 관련 구조체
        public struct _AlarmReport
        {
            public string ALID;
            public string ALST; // 1 : Set, 2 : Reset
        }
    }
    #endregion

    #region EQ State Change Event - Availability & Move
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class AvailabilityAndMoveStateChangeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _AvailabilityAndMoveStateChangeEvent BODY;
        // 데이터 초기화
        public AvailabilityAndMoveStateChangeEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSTATECHANGEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.AVAILABILITYSTATE = " ";
            this.BODY.MOVESTATE = " ";
            this.BODY.REASONCODE = " ";
            this.BODY.DESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _AvailabilityAndMoveStateChangeEvent
        {
            public string AVAILABILITYSTATE;//1: DOWN, 2: UP
            public string MOVESTATE;//1: DOWN, 2: UP
            public string REASONCODE; // EQ Vendor should provides reason code & text
            public string DESCRIPTION;
        }
    }
    #endregion

    #region Loss Code Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class LossCodeEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _LossCodeEvent BODY;
        // 데이터 초기화
        public LossCodeEvent()
        {
            this.EQPID = " ";
            this.NAME = "LOSSCODEEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.EQST.MOVESTATE = " ";
            this.BODY.EQST.RUNSTATE = " "; // 사용 안함.
            this.BODY.EQST.REASONCODE = " ";
            this.BODY.EQST.DESCRIPTION = " ";
            this.BODY.LOSS.LOSSCODE = " ";
            this.BODY.LOSS.LOSSDESCRIPTION = " ";
        }
        // Body 관련 구조체
        public struct _LossCodeEvent
        {
            public struct _EQST
            {
                public string MOVESTATE;
                public string RUNSTATE; // 사용 안함
                public string REASONCODE;
                public string DESCRIPTION;
            }

            public struct _LOSS
            {
                public string LOSSCODE;
                public string LOSSDESCRIPTION;
            }

            public _EQST EQST;
            public _LOSS LOSS;
        }
    }
    #endregion

    #region Operator Information Report
    [XmlRoot("MESSAGE")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class OperatorInfoEvent
    {
        public string EQPID { get; set; }
        public string NAME { get; set; }
        public string TRANSACTIONNO { get; set; }
        [Category("BODY")]
        public Body BODY { get; set; }

        public OperatorInfoEvent()
        {
            this.SetAllStringMemberValues(" ");
            BODY = new Body();
            NAME = "OPERATORINFOEVENT";
            TRANSACTIONNO = "0";
        }

        [XmlRoot("MESSAGE")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Serializable]
        public class Body
        {
            public string OPTIONINFO { get; set; }
            public string COMMENT { get; set; }
            public string OPERATORID { get; set; }
            public string PASSWORD { get; set; }

            public Body()
            {
                this.SetAllStringMemberValues(" ");
            }
        }
    }
    #endregion

    #region Equipment Approve Send
    [XmlRoot("MESSAGE")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class EquipmentApproveSend
    {
        public string EQPID { get; set; }
        public string NAME { get; set; }
        public string TRANSACTIONNO { get; set; }
        [Category("BODY")]
        public Body BODY { get; set; }

        public EquipmentApproveSend()
        {
            this.SetAllStringMemberValues(" ");
            BODY = new Body();
            NAME = "EQAPPROVESEND";
            TRANSACTIONNO = "0";
        }

        [XmlRoot("MESSAGE")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Serializable]
        public class Body
        {
            /// <summary>
            /// 31: Approved, 32: Not Approved
            /// </summary>
            public string RCMD { get; set; }
            /// <summary>
            /// LOGIN, LOGOUT
            /// </summary>
            public string APPROVECODE { get; set; }
            /// <summary>
            /// Location
            /// </summary>
            public string APPROVEINFO { get; set; }
            /// <summary>
            /// Operator ID
            /// </summary>
            public string APPROVEID { get; set; }
            /// <summary>
            /// OPER, HOST, EQP
            /// </summary>
            public string BYWHO { get; set; }
            /// <summary>
            /// Reason
            /// </summary>
            public string APPROVETEXT { get; set; }

            public Body()
            {
                this.SetAllStringMemberValues(" ");
            }
        }
    }
    #endregion

    #region Equipment Approve Reply
    [XmlRoot("MESSAGE")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class EquipmentApproveReply
    {
        public string EQPID { get; set; }
        public string NAME { get; set; }
        public string TRANSACTIONNO { get; set; }
        [Category("BODY")]
        public Body BODY { get; set; }

        public EquipmentApproveReply()
        {
            this.SetAllStringMemberValues(" ");
            BODY = new Body();
            NAME = "EQAPPROVEREPLY";
            TRANSACTIONNO = "0";
        }

        [XmlRoot("MESSAGE")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Serializable]
        public class Body
        {
            /// <summary>
            /// 0:Accepted, 1:Invalid Cell ID, 2:Not Supported, 3:Rejected, 4: Other Error
            /// </summary>
            public string HCACK { get; set; }

            public Body()
            {
                this.SetAllStringMemberValues(" ");
            }
        }
    }
    #endregion

    #region Tact Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class TactEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _TactEvent BODY;
        // 데이터 초기화
        public TactEvent()
        {
            this.EQPID = " ";
            this.NAME = "TACTEVENT";
            this.TRANSACTIONNO = "0";
            this.BODY.LOCATION = " ";
            this.BODY.TIME = " ";
        }
        // Body 관련 구조체
        public struct _TactEvent
        {
            public string LOCATION;
            public string TIME;
        }
    }
    #endregion

    #region Equipment Specific Step Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class EQSpecificStepEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _EQSpecificStepEvent BODY;
        // 데이터 초기화
        public EQSpecificStepEvent()
        {
            this.EQPID = " ";
            this.NAME = "EQSPECIFICSTEPEVENT";
            this.TRANSACTIONNO = "0";

            this.BODY.CELLID = " ";
            this.BODY.PPID = " ";
            this.BODY.PRODUCTID = " ";
            this.BODY.STEPID = " ";
            this.BODY.OPTIONINFO = " ";
            this.BODY.DESCRIPTION = " ";
        }
        public struct _EQSpecificStepEvent
        {
            public string CELLID;
            public string PPID;
            public string PRODUCTID;
            public string STEPID;
            public string OPTIONINFO;
            public string DESCRIPTION;
            [XmlArray("ITEMS")]
            [XmlArrayItem("ITEM")]
            public List<DVList> ITEMS;
            public struct DVList
            {
                public string ITEM_NAME;
                public string ITEM_VALUE;
            }
        }
    }
    #endregion

    #region Performance Loss Event
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class PerformanceLossEvent
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _PerformanceLossEvent BODY;
        // 데이터 초기화
        public PerformanceLossEvent()
        {
            this.EQPID = " ";
            this.NAME = "PERFORMANCELOSSEVENT";
            this.TRANSACTIONNO = "0";

            this.BODY.CELLINFOSET.CELLID = " ";
            this.BODY.CELLINFOSET.PPID = " ";
            this.BODY.CELLINFOSET.PRODUCTID = " ";
            this.BODY.CELLINFOSET.STEPID = " ";
        }
        public struct _PerformanceLossEvent
        {
            public _CELLINFOSE CELLINFOSET;

            public struct _CELLINFOSE
            {
                public string CELLID;
                public string PPID;
                public string PRODUCTID;
                public string STEPID;
            }

            public struct LOSSOBJECT
            {
                public string H_TACT_TIME;
                public string E_TACT_TIME;
                public string S_TIME;
                public string E_TIME;
                public string L_CODE;
                public string L_TEXT;
            }

            public LOSSOBJECT[] LOSSOBJECTS;
        }
    }
    #endregion

    #region Cell Lot Information Request
    [XmlRoot("MESSAGE")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class CellLotInformationRequest
    {
        public string EQPID { get; set; }
        public string NAME { get; set; }
        public string TRANSACTIONNO { get; set; }
        [Category("BODY")]
        public Body BODY { get; set; }

        public CellLotInformationRequest()
        {
            this.SetAllStringMemberValues(" ");
            BODY = new Body();
            NAME = "CELLLOTINFOREQUEST";
            TRANSACTIONNO = "0";
        }

        [XmlRoot("MESSAGE")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Serializable]
        public class Body
        {
            public string OPTIONCODE { get; set; }
            [XmlArrayItem("CELLID")]
            public Collection<string> CELLIDS { get; set; }

            public Body()
            {
                this.SetAllStringMemberValues(" ");
                CELLIDS = new Collection<string>();
            }
        }
    }
    #endregion

    #region Cell Lot Information Reply
    [XmlRoot("MESSAGE")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class CellLotInformationReply
    {
        public string EQPID { get; set; }
        public string NAME { get; set; }
        public string TRANSACTIONNO { get; set; }
        [Category("BODY")]
        public Body BODY { get; set; }

        public CellLotInformationReply()
        {
            this.SetAllStringMemberValues(" ");
            BODY = new Body();
            NAME = "CELLLOTINFOREPLY";
            TRANSACTIONNO = "0";
        }

        [XmlRoot("MESSAGE")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Serializable]
        public class Body
        {
            public string ACKC6 { get; set; }

            public Body()
            {
                this.SetAllStringMemberValues(" ");
            }
        }
    }
    #endregion

    #region Cell Lot Information Download
    [XmlRoot("MESSAGE")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class CellLotInformationDownload
    {
        public string EQPID { get; set; }
        public string NAME { get; set; }
        public string TRANSACTIONNO { get; set; }
        [Category("BODY")]
        public Body BODY { get; set; }

        public CellLotInformationDownload()
        {
            this.SetAllStringMemberValues(" ");
            BODY = new Body();
            NAME = "CELLLOTINFODOWNLOAD";
            TRANSACTIONNO = "0";
        }

        [XmlRoot("MESSAGE")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Serializable]
        public class Body
        {
            [XmlArrayItem("CELLLOT")]
            public Collection<CellLotInfoSet> CELLLOTS { get; set; }

            public Body()
            {
                CELLLOTS = new Collection<CellLotInfoSet>();
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Serializable]
        public class CellLotInfoSet
        {
            public string CELLID { get; set; }
            public string CASSETTEID { get; set; }
            public string BATCHLOT { get; set; }
            public string PRODUCTID { get; set; }
            public string PRODUCT_TYPE { get; set; }
            public string PRODUCT_KIND { get; set; }
            public string PRODUCTSPEC { get; set; }
            public string STEPID { get; set; }
            public string PPID { get; set; }
            public string CELL_SIZE { get; set; }
            public string CELL_THICKNESS { get; set; }
            public string COMMENT { get; set; }
            [XmlArrayItem("ITEM")]
            public Collection<ItemSet> ITEMS { get; set; }

            public CellLotInfoSet()
            {
                this.SetAllStringMemberValues(" ");
                ITEMS = new Collection<ItemSet>();
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Serializable]
        public class ItemSet
        {
            public string ITEM_NAME { get; set; }
            public string ITEM_VALUE { get; set; }

            public ItemSet()
            {
                this.SetAllStringMemberValues(" ");
            }
        }
    }
    #endregion
    #region Specification/Version State Request
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class SpecificationversionStateRequest
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public string BODY;
        // 데이터 초기화
        public SpecificationversionStateRequest()
        {
            this.EQPID = " ";
            this.NAME = "SPECIFICATIONVERSIONSTATEREQUEST";
            this.TRANSACTIONNO = "0";
        }
    }
    #endregion

    #region Specification/Version State Request Reply
    [Serializable]
    [XmlRoot("MESSAGE")]
    public class SpecificationversionStateReply
    {
        public string EQPID;
        public string NAME;
        public string TRANSACTIONNO;
        public _SpecificationversionStateReply BODY;
        // 데이터 초기화
        public SpecificationversionStateReply()
        {
            this.EQPID = " ";
            this.NAME = "SPECIFICATIONVERSIONSTATEREPLY";
            this.TRANSACTIONNO = "0";
        }
        // Body 관련 구조체
        public struct _SpecificationversionStateReply
        {
            [XmlArray("SPECS")]
            [XmlArrayItem("SPEC")]
            public List<SPECList> SPECS;
            public struct SPECList
            {
                public string SPECID;
                public string SPECDATA;
            }
        }
    }
    #endregion

    #endregion
}

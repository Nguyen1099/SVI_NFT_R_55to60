using ENC.IO;
using ENC.IO.Common;
using ENC.IO.DebugKit;
using ENC.LogDLL;
using ENC.MemoryMap.Manager;
using ENC.MemoryMap.Pages;
using HLDevice.Abstract;
using Mcc;
using SVI_NFT_R.DEVICE.Nachi;
using SVI_NFT_R.EqToEq.SdvGammaFoldableLine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    public class CDocument
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        internal static extern IntPtr FindWindow(string strClassName, string strWindowName);
        private CDefine.ERunMode m_eRunMode;
        private CDefine.ERunStatus m_eRunStatus;
        private CDefine.EMoveModeType m_eMoveModeType;
        private bool _bCIMConnected;
        private CCIMDefine.EAvailabilityState[] _eAvailabilityState = new CCIMDefine.EAvailabilityState[2];
        private CCIMDefine.EInterlockState[] _eInterlockState = new CCIMDefine.EInterlockState[2];
        private CCIMDefine.EMoveState[] _eMoveState = new CCIMDefine.EMoveState[2];
        private CCIMDefine.ERunState[] _eRunState = new CCIMDefine.ERunState[2];
        private CCIMDefine.EFrontEquipmentState[] _eFrontState = new CCIMDefine.EFrontEquipmentState[2];
        private CCIMDefine.ERearEquipmentState[] _eRearState = new CCIMDefine.ERearEquipmentState[2];
        private CCIMDefine.EPP_SPLState _ePP_SPLState = new CCIMDefine.EPP_SPLState();
        private CCIMDefine.EControlState _eControlState = new CCIMDefine.EControlState();
        private CCIMDefine.EConveyorMoveState[] _eConveyorMoveState = new CCIMDefine.EConveyorMoveState[2];
        private CCIMDefine.EConveyorStopReason[] _eConveyorStopReason = new CCIMDefine.EConveyorStopReason[2];
        private CCIMDefine.EOpcallState _eOpCallState = new CCIMDefine.EOpcallState();
        private CCIMDefine.EPmMode _ePMMode = new CCIMDefine.EPmMode();
        private CCIMDefine.EMoveState[][] _eUnitMoveState = new CCIMDefine.EMoveState[Enum.GetNames(typeof(CCIMDefine.EUnit)).Length][];
        private CCIMDefine.EInterlockState[][] _eUnitInterlockState = new CCIMDefine.EInterlockState[Enum.GetNames(typeof(CCIMDefine.EUnit)).Length][];
        private CCIMDefine.EHeartBeat[] _eHeartBeat = new CCIMDefine.EHeartBeat[2];
        private List<object> m_objSVIDList = new List<object>();
        private List<object> m_objCIMMessageList = new List<object>();
        private bool _bUnitInterLockHappened = false;
        private bool _bLoadingStopFinish = false;
        private bool _bForceStopButton = false;
        private bool mIsManualInputMode = false;
        private bool mIsMasterLogin = false;
        private bool mIsMasterLoginEnabled = false;
        /// <summary>
        /// 현재 로그인된 유저 정보
        /// </summary>
        private CUserInformation m_objUserInformation;
        /// <summary>
        /// Log 관련 Library 추가
        /// </summary>
        private LogManager m_objLogManager = new LogManager();
        /// <summary>
        /// Config 클래스
        /// </summary>
        public CConfig m_objConfig;
        /// <summary>
        /// 모델 클래스
        /// </summary>
        public CModel m_objModel;
        public CRecipe m_objRecipe;
        /// <summary>
        /// 프로세스 메인
        /// </summary>
        public CProcessMain m_objProcessMain;
        /// <summary>
        /// 프로세스 심
        /// </summary>
        public CProcessCIM m_objProcessCIM;
        /// <summary>
        /// 프로세스 데이터베이스
        /// </summary>
        public CProcessDatabase m_objProcessDatabase;
        /// <summary>
        /// 택타임 클래스
        /// </summary>
        public CTactTime m_objTactTime;
        /// <summary>
        /// 사이클 택타임 로그 클래스
        /// </summary>
        public CCycleTactTime m_objCycleTactTime;
        /// <summary>
        /// CIM 상위 연결 상태
        /// </summary>
        public bool m_bCIMConnected
        {
            get { return _bCIMConnected; }
            set { _bCIMConnected = value; }
        }
        /// <summary>
        /// 설비 가용 상태
        /// </summary>
        public CCIMDefine.EAvailabilityState[] m_eAvailabilityState
        {
            get { return _eAvailabilityState; }
            set { _eAvailabilityState = value; }
        }
        /// <summary>
        /// 인터락 상태
        /// </summary>
        public CCIMDefine.EInterlockState[] m_eInterlockState
        {
            get { return _eInterlockState; }
            set { _eInterlockState = value; }
        }
        /// <summary>
        /// 설비 동작 상태
        /// </summary>
        public CCIMDefine.EMoveState[] m_eMoveState
        {
            get { return _eMoveState; }
            set { _eMoveState = value; }
        }
        /// <summary>
        /// 설비내 Cell 존재 유무
        /// </summary>
        public CCIMDefine.ERunState[] m_eRunState
        {
            get { return _eRunState; }
            set { _eRunState = value; }
        }
        /// <summary>
        /// 상류 설비 물류 상태
        /// </summary>
        public CCIMDefine.EFrontEquipmentState[] m_eFrontState
        {
            get { return _eFrontState; }
            set { _eFrontState = value; }
        }
        /// <summary>
        /// 하류 설비 물류 상태
        /// </summary>
        public CCIMDefine.ERearEquipmentState[] m_eRearState
        {
            get { return _eRearState; }
            set { _eRearState = value; }
        }
        /// <summary>
        /// Sample Run-Normal Run 상태 구분
        /// </summary>
        public CCIMDefine.EPP_SPLState m_ePP_SPLState
        {
            get { return _ePP_SPLState; }
            set { _ePP_SPLState = value; }
        }
        /// <summary>
        /// CONTROL STATE
        /// </summary>
        public CCIMDefine.EControlState m_eControlState
        {
            get { return _eControlState; }
            set { _eControlState = value; }
        }
        /// <summary>
        /// Conveyor MOVE State
        /// </summary>
        public CCIMDefine.EConveyorMoveState[] m_eConveyorMoveState
        {
            get { return _eConveyorMoveState; }
            set { _eConveyorMoveState = value; }
        }
        /// <summary>
        /// Conveyor STOP REASON
        /// </summary>
        public CCIMDefine.EConveyorStopReason[] m_eConveyorStopReason
        {
            get { return _eConveyorStopReason; }
            set { _eConveyorStopReason = value; }
        }
        /// <summary>
        /// Opcall State
        /// </summary>
        public CCIMDefine.EOpcallState m_eOpCallState
        {
            get { return _eOpCallState; }
            set { _eOpCallState = value; }
        }
        /// <summary>
        /// PM 모드 상태
        /// </summary>
        public CCIMDefine.EPmMode m_ePMMode
        {
            get { return _ePMMode; }
            set { _ePMMode = value; }
        }
        /// <summary>
        /// Unit Move 상태
        /// </summary>
        public CCIMDefine.EMoveState[][] m_eUnitMoveState
        {
            get { return _eUnitMoveState; }
            set { _eUnitMoveState = value; }
        }
        /// <summary>
        /// Unit Interlock 상태
        /// </summary>
        public CCIMDefine.EInterlockState[][] m_eUnitInterlockState
        {
            get { return _eUnitInterlockState; }
            set { _eUnitInterlockState = value; }
        }
        /// <summary>
        /// 하트 비트 상태
        /// </summary>
        public CCIMDefine.EHeartBeat[] m_eHeartBeat
        {
            get { return _eHeartBeat; }
            set { _eHeartBeat = value; }
        }
        public bool IsSmokeOrOvertempAlarmHappened { get; set; } = false;
        public bool m_bUnitInterLockHappened
        {
            get { return _bUnitInterLockHappened; }
            set { _bUnitInterLockHappened = value; }
        }
        public bool m_bForceStopButton
        {
            get { return _bForceStopButton; }
            set { _bForceStopButton = value; }
        }
        public bool m_bLoadingStopFinish
        {
            get { return _bLoadingStopFinish; }
            set { _bLoadingStopFinish = value; }
        }
        public bool m_bInterLockHappened = false;
        /// <summary>
        /// MFQ A POSITION 투입
        /// </summary>
        public bool m_bMfqInputPositionA = true;
        /// <summary>
        /// MFQ B POSITION 투입
        /// </summary>
        public bool m_bMfqInputPositionB = true;
        /// <summary>
        /// 보고된 TP CODE
        /// </summary>
        public string m_strTPCode = "00000";
        /// <summary>
        /// EFU 동작 RPM
        /// </summary>
        public int mEfuRunningRpm;
        public bool m_bIsDatabaseInitialized = false;
        /// <summary>
        /// 알람 Rawdata 테이블 (사용할 일이 많음으로 프로그램 실행 초기에 데이터베이스에서 읽어 메모리에 올려놓고 변경시에만 새로 불러와 사용한다)
        /// </summary>
        public DataTable AlarmDataTable { get { return m_objAlarmDataTable; } }
        /// <summary>
        /// 알람 리스트
        /// </summary>
        public Data.AlarmList m_objAlarmList = new Data.AlarmList();
        private DataTable m_objAlarmDataTable = new DataTable();
        public HashSet<CAlarmDefine.EAlarmList> NotReportAlarmList { get; private set; }
        public event EventHandler<EventArgsLogEvent> OnLogging;
        public class EventArgsLogEvent : EventArgs
        {
            public CDefine.ELogType LogType { get; private set; }
            public string LogMessage { get; private set; }

            public EventArgsLogEvent(CDefine.ELogType logType, string logMessage)
            {
                LogType = logType;
                LogMessage = logMessage;
            }
        }
        public int MfqInputCount = 0;
        public bool IsInitialized { get; private set; } = false;
        /// <summary>
        /// 수동 투입 모드
        /// </summary>
        public bool IsManualInputMode
        {
            get
            {
                return mIsManualInputMode;
            }
            set
            {
                mIsManualInputMode = IsAfterShipping ? false : value;
            }
        }
        /// <summary>
        /// 마스터 로그인 유무 (초기 셋업용, 개발자용 특수 권한)
        /// </summary>
        public bool IsMasterLogin
        {
            get
            {
                return mIsMasterLogin;
            }
            set
            {
                mIsMasterLogin = IsAfterShipping ? false : value;
            }
        }
        /// <summary>
        /// 마스터 로그인 사용 유무
        /// </summary>
        public bool IsMasterLoginEnabled
        {
            get
            {
                return mIsMasterLoginEnabled;
            }
            set
            {
                mIsMasterLoginEnabled = IsAfterShipping ? false : value;
            }
        }
        /// <summary>
        /// 자동 로그아웃 사용 여부
        /// </summary>
        public bool IsAutoLogoutNotUsed { get; set; } = false;
        /// <summary>
        /// 출하 후인지 여부
        /// </summary>
        public static bool IsAfterShipping => DateTime.Now > DateTime.Parse(Properties.Resources.ShippingDate);
        /// <summary>
        /// CIM Qual Mode 사용 여부
        /// </summary>
        public bool IsCimQualMode { get; set; } = false;

        /// <summary>
        /// 생성자
        /// </summary>
        public CDocument()
        {
            m_eMoveModeType = CDefine.EMoveModeType.MOVE_MODE_TYPE_MANUAL;
            m_eRunMode = CDefine.ERunMode.RealRun;
            m_objUserInformation = new CUserInformation();

            for (int i = 0; i < Enum.GetNames(typeof(CCIMDefine.EUnit)).Length; i++)
            {
                _eUnitMoveState[i] = new CCIMDefine.EMoveState[2];
                _eUnitInterlockState[i] = new CCIMDefine.EInterlockState[2];
            }

            // CIM에 보고하지 않는 알람 리스트 (특수한 경우)
            NotReportAlarmList = new HashSet<CAlarmDefine.EAlarmList>()
            {
                CAlarmDefine.EAlarmList.CO_TRANSFER_CIM_N_HOST_DISCONNECTED
            };
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
#if USE_HIGH_RESOLUTION_TIMER
            TimeBeginPeriod(1u);
#endif
            var args = Environment.GetCommandLineArgs().Select(item => item.ToUpper());
            IsMasterLogin = args.Contains("/DEVMODE");
            IsAutoLogoutNotUsed = args.Contains("/AUTOLOGOUTDISABLE");
            IsManualInputMode = args.Contains("/MANUALINPUTMODE");
            IsCimQualMode = args.Contains("/CIMQUAL");
            // MFQ 모드 플레그 설정 (명령줄 인수를 파싱하여 "/MFQ" 가 있으면 MFQ 모드로 설정함)
            if (args.Contains("/MFQ") == true)
            {
                IsManualInputMode = true;
                IsMasterLogin = true;
                IsAutoLogoutNotUsed = true;
            }

            EHS.ExtensionMethods.SetDocument(this);

            NoiseFilterManager.Initialize();

            // 파일 클린 스케쥴러 초기화
            FileClean.Scheduler.Initailize();

            // Config 객체 생성
            m_objConfig = new CConfig();
            if (false == m_objConfig.Initialize())
                throw new Exception();

            // 데이터 맵핑 파일 로드
            {
                var errorsAndWarnings = new List<ErrorSet>();
                var option = new DataMappingAssignOptionSet
                {
                    DataFileFormat = EDataFileFormat.Csv,
                    DataFileExt = ".txt"
                };
                string[] mappingDataDirectoryPaths = new string[]
                {
                    "./IoMappingData/EqToEq/"
                };
                option.DataBlockBaseDirectoryPaths.AddRange(mappingDataDirectoryPaths);
                option.DataGroupBaseDirectoryPaths.AddRange(mappingDataDirectoryPaths);
                DataMappingAssignManager.Initialize(option, errorsAndWarnings);
                // 에러 디스플레이
                if (errorsAndWarnings.Count > 0)
                {
                    errorsAndWarnings.ShowErrorDisplayUi();
                }

                // 주소 할당 디버그 텍스트 출력 (필요시 주석 해제)
                //HandlingDataVisualizerOptionSet bitDebugOption = new HandlingDataVisualizerOptionSet()
                //{
                //    AddressFormat = EAddressFormat.Bit
                //};
                //Debug.WriteLine(typeof(global::EqToEq.BitDeviceMap).GetHandlingDataDebugText(bitDebugOption));
                //HandlingDataVisualizerOptionSet wordDebugOption = new HandlingDataVisualizerOptionSet()
                //{
                //    AddressFormat = EAddressFormat.Word
                //};
                //Debug.WriteLine(typeof(global::EqToEq.WordDeviceMap).GetHandlingDataDebugText(wordDebugOption));
            }

            // 셀 데이터 매니저 초기화
            if (false == CellDataManager.Initialize(this))
            {
                throw new Exception("Cell Data Manager Initialize Failed");
            }

            // 모델 초기화
            m_objModel = new CModel(this);
            if (false == m_objModel.Initialize())
                throw new Exception();

            // MCC 로그 매니저 초기화
            if (false == MccLogManager.Initialize(this))
            {
                throw new Exception();
            }

            // 택타임 초기화
            m_objTactTime = new CTactTime();
            if (false == m_objTactTime.Initialize(this))
                throw new Exception();

            // 택타임 최대값 설정
            m_objTactTime.SetOutputTactTimeMaxCount(m_objConfig.GetSystemParameter().iTactTimeMaxCount);

            // 사이클 택타임 초기화
            m_objCycleTactTime = new CCycleTactTime();

            // 장비 상태 초기화
            InitializeEQPState();

            // ENC 로그 실행
            if (false == SetCreateLog())
                throw new Exception();

            Resource.Initialize(this);

            m_objLogManager.SetHeader(CDefine.ELogType.LOG_SIGNAL_INTERFACE_LOAD.ToString(), LoadInterface.SIGNAL_DUMP_LOG_CSV_HEADER());
            m_objLogManager.SetHeader(m_objCycleTactTime.m_tactInShuttle.LogType.ToString(), m_objCycleTactTime.m_tactInShuttle.strHeader);
            m_objLogManager.SetHeader(m_objCycleTactTime.m_tactInRobot.LogType.ToString(), m_objCycleTactTime.m_tactInRobot.strHeader);
            m_objLogManager.SetHeader(m_objCycleTactTime.m_tactInspStage.LogType.ToString(), m_objCycleTactTime.m_tactInspStage.strHeader);
            m_objLogManager.SetHeader(m_objCycleTactTime.m_tactOutRobot.LogType.ToString(), m_objCycleTactTime.m_tactOutRobot.strHeader);
            m_objLogManager.SetHeader(m_objCycleTactTime.m_TactOutFlip.LogType.ToString(), m_objCycleTactTime.m_TactOutFlip.strHeader);
            m_objLogManager.SetHeader(m_objCycleTactTime.m_tactJavasInspection.LogType.ToString(), m_objCycleTactTime.m_tactJavasInspection.strHeader);
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_TACT_TIME.ToString(), CDefine.TACT_LOG_HEADER);
            string columnHeader = "Time,Name,Error Message";
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_COMM_ERROR_GMS.ToString(), columnHeader);
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_COMM_ERROR_MCU.ToString(), columnHeader);
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_COMM_ERROR_TEMPERATURE.ToString(), columnHeader);
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_COMM_ERROR_SAFETYPLC.ToString(), columnHeader);
            columnHeader = Data.SerialReader<double>.GetHeaderForCsvReport();
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_COMM_STATISTICS_GMS.ToString(), columnHeader);
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_COMM_STATISTICS_MCU.ToString(), columnHeader);
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_COMM_STATISTICS_TEMPERATURE.ToString(), columnHeader);
            m_objLogManager.SetHeader(CDefine.ELogType.LOG_COMM_STATISTICS_SAFETYPLC.ToString(), columnHeader);

            global::Utils.TimedAlarmActivator.Initialize(m_objLogManager, m_objConfig.GetLogPath());

            // 프로세스 메인 초기화
            m_objProcessMain = new CProcessMain();
            if (false == m_objProcessMain.Initialize(this))
            {
                this.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, "ProcessMain Initialize Fail");
                throw new Exception();
            }

            // 프로세스 심 초기화
            m_objProcessCIM = new CProcessCIM();
            if (false == m_objProcessCIM.Initialize(this))
            {
                this.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, "ProcessCIM Initialize Fail");
                throw new Exception();
            }

            Thread.Sleep(1000);

            // 프로세스 데이터베이스 초기화
            m_objProcessDatabase = new CProcessDatabase();
            if (false == m_objProcessDatabase.Initialize(this))
            {
                this.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, "ProcessDataBase Initialize Fail");
                throw new Exception();
            }

            // FFU FAN 속도 초기화
            try
            {
                DataTable dt = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationSVID.HLGetDataTable();
                mEfuRunningRpm = Convert.ToInt32(dt.Rows[(int)CDatabaseDefine.ESvidList.EFU_RPM_1][(int)CDatabaseDefine.EInformationSvid.APPROPRIATE_MIN_VALUE]);
            }
            catch
            {
                mEfuRunningRpm = 800;
            }

            m_bIsDatabaseInitialized = true;

            m_objRecipe = new CRecipe(this);
            if (false == m_objRecipe.Initialize())
                throw new Exception();

            // ALARM 리스트 초기화
            {
                // 알람 리스트 로드
                ReloadAlarmList();
                // 클리어 되지 않은 알람 히스토리 로드    
                m_objAlarmList.Initilaize(this);
                // 프로그램 재구동시 클리어 되지 않은 알람이 있으면 램프 히스토리를 등록한다
                if (0 < m_objAlarmList.Count)
                {
                    m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.Fault);
                }

                // 알람이 발생하여 정지 중일 때 프로그램이 강제 종료 됐을 때에 대한 예외처리
                if (true == m_objAlarmList.IsHaveyAlarmSet)
                {
                    var captureState = m_eAvailabilityState.DeepClone();
                    for (int i = 0; i < captureState.Length; i++)
                    {
                        captureState[i] = CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_DOWN;
                    }
                    m_eAvailabilityState = captureState;
                }
            }

            // 프로그램 최초 구동시 CIM 사용 모드면 CIM_IS_DISCONNECTED 알람을 셋해준다
            // Urcis 프로그램에는 프로그램 연결이 끊어지면 자동으로 TC에 'CIM_IS_DISCONNECTED' 알람을 셋해주게 되어있어 정합성을 맞추기 위해 추가함
            if (true == m_objConfig.GetOptionParameter().bUseCIM)
            {
                SetForcedAlarm(CAlarmDefine.EAlarmList.CO_TRANSFER_CIM_N_CIM_DISCONNECTED);
            }

            // EC List Set
            SetECList(bReportCim: false);

            UpdateReservedFileCleanSchedule();

            IsInitialized = true;
            return true;
        }

        /// <summary>
        /// 알람 리스트를 다시 불러온다.
        /// </summary>
        public DataTable ReloadAlarmList()
        {
            m_objAlarmDataTable = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetDataTable();
            return m_objAlarmDataTable;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            // 택타임 해제
            m_objTactTime.DeInitialize();
            // 프로세스 데이터베이스 해제
            m_objProcessDatabase.DeInitialize();
            // 프로세스 메인 해제
            m_objProcessMain.DeInitialize();
            global::Utils.TimedAlarmActivator.DeInitialize();
            // 프로세스 심 해제.
            m_objProcessCIM.DeInitialize();
            // MCC 로그 매니져 해제
            MccLogManager.DeInitialize();
            // 모델 해제
            m_objModel.DeInitialize();
            // ENC 로그 종료
            m_objLogManager.DeInitialize();
            // 알람 리스트 해제
            m_objAlarmList.DeInitialize();

            // 셀 데이터 매니저 해제
            CellDataManager.DeInitialize();

            DataMappingAssignManager.DeInitialize();

            // Config 해제
            m_objConfig.DeInitialize();

            // 파일 클린 스케쥴러 정리
            FileClean.Scheduler.DeInitailize();

            NoiseFilterManager.DeInitialize();
            // 생산량 카운터 해제
            ProductCounter.DeInitialize();
            Resource.DeInitialize();

            IsInitialized = false;
#if USE_HIGH_RESOLUTION_TIMER
            TimeEndPeriod(1u);
#endif
        }

        public void UpdateReservedFileCleanSchedule()
        {
            FileClean.Scheduler.ReservedConfigItems.Clear();
            FileClean.Scheduler.ReservedConfigItems.Add(new FileClean.Define.ConfigSet()
            {
                IsUse = true,
                ScanPath = $@"D:\LINKED_INSP\",
                ScanHours = 1,
                KeepDays = Convert.ToInt32(Config.WaitTime.Etc.AftpLinkedInspectionResultFileStoreDays.Value.TotalDays)
            });
            FileClean.Scheduler.ReloadConfig();
        }

        /// <summary>
        /// 장비 상태 초기화
        /// </summary>
        private void InitializeEQPState()
        {
            // 장비 상태 초기화시 공유메모리의 데이터 가져오자.
            CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            for (int iLoopCount = 0; iLoopCount < (int)CCIMDefine.EPresentState.PRESENT_STATE_FINAL; iLoopCount++)
            {
                m_eAvailabilityState[iLoopCount] = objMachineStatus.m_eAvailabilityState[iLoopCount];
                if (
                    CCIMDefine.EInterlockState.INTERLOCK_STATE_ON != objMachineStatus.m_eInterlockState[iLoopCount]
                    || CCIMDefine.EInterlockState.INTERLOCK_STATE_OFF != objMachineStatus.m_eInterlockState[iLoopCount]
                    )
                {
                    m_eInterlockState[iLoopCount] = CCIMDefine.EInterlockState.INTERLOCK_STATE_OFF;
                }
                else
                {
                    m_eInterlockState[iLoopCount] = objMachineStatus.m_eInterlockState[iLoopCount];
                }

                m_eMoveState[iLoopCount] = CCIMDefine.EMoveState.MOVE_STATE_PAUSE;//objMachineStatus.m_eMoveState[ iLoopCount ];
                m_eRunState[iLoopCount] = objMachineStatus.m_eRunState[iLoopCount];
                m_eFrontState[iLoopCount] = CCIMDefine.EFrontEquipmentState.FRONT_STATE_UP;//objMachineStatus.m_eFrontState[ iLoopCount ];
                m_eRearState[iLoopCount] = CCIMDefine.ERearEquipmentState.REAR_STATE_UP;//objMachineStatus.m_eRearState[ iLoopCount ];
                m_ePP_SPLState = CCIMDefine.EPP_SPLState.PP_SPL_STATE_NORMAL;//m_objConfig.GetOptionParameter().eUsePP_SPL;
                m_eConveyorMoveState[iLoopCount] = objMachineStatus.m_eConveyorMoveState[iLoopCount];
                m_eConveyorStopReason[iLoopCount] = objMachineStatus.m_eConveyorStopReason[iLoopCount];
                m_eOpCallState = objMachineStatus.m_eOpCallState;
                m_ePMMode = CCIMDefine.EPmMode.PM_MODE_OFF;
                m_eHeartBeat[iLoopCount] = CCIMDefine.EHeartBeat.HEART_BEAT_OFF;
                objMachineStatus.m_eProgramExitStatus = CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_OFF;
            }
            for (int i = 0; i < Enum.GetNames(typeof(CCIMDefine.EUnit)).Length; i++)
            {
                for (int iLoopCount = 0; iLoopCount < (int)CCIMDefine.EPresentState.PRESENT_STATE_FINAL; iLoopCount++)
                {
                    m_eUnitMoveState[i][iLoopCount] = CCIMDefine.EMoveState.MOVE_STATE_RUNNING; // objMachineStatus.m_eUnitMoveState[i][iLoopCount];
                    if (
                        CCIMDefine.EInterlockState.INTERLOCK_STATE_ON != objMachineStatus.m_eUnitInterlockState[i][iLoopCount]
                        || CCIMDefine.EInterlockState.INTERLOCK_STATE_OFF != objMachineStatus.m_eUnitInterlockState[i][iLoopCount]
                        )
                    {
                        m_eUnitInterlockState[i][iLoopCount] = CCIMDefine.EInterlockState.INTERLOCK_STATE_OFF;
                    }
                    else
                    {
                        m_eUnitInterlockState[i][iLoopCount] = objMachineStatus.m_eUnitInterlockState[i][iLoopCount];
                    }
                }
            }
            objMMFManagerMachineStatus[0].SetMachineStatus(objMachineStatus);
        }

        /// <summary>
        /// ENC 로그 실행
        /// </summary>
        /// <returns>실행 성공 여부</returns>
        public bool SetCreateLog()
        {
            bool bReturn = false;

            do
            {
                // 30일 삭제 설정 -> 90일로 변경
                //m_objLogManager.SetLogDeleteDays(30);
                m_objLogManager.SetLogDeleteDays(90);

                // 로그 초기화
                foreach (CDefine.ELogType logType in Enum.GetValues(typeof(CDefine.ELogType)))
                {
                    if (logType == CDefine.ELogType.LOG_SIGNAL_INTERFACE_LOAD
                        || logType == CDefine.ELogType.LOG_CYCLE_TACT_01_IN_SHUTTLE
                        || logType == CDefine.ELogType.LOG_CYCLE_TACT_02_IN_ROBOT
                        || logType == CDefine.ELogType.LOG_CYCLE_TACT_03_INSP_STAGE
                        || logType == CDefine.ELogType.LOG_CYCLE_TACT_04_OUT_ROBOT
                        || logType == CDefine.ELogType.LOG_CYCLE_TACT_05_OUT_FLIP
                        || logType == CDefine.ELogType.LOG_CYCLE_TACT_90_JAVAS_INSPECTION
                        || logType == CDefine.ELogType.LOG_TACT_TIME
                        || logType == CDefine.ELogType.LOG_COMM_ERROR_GMS
                        || logType == CDefine.ELogType.LOG_COMM_ERROR_MCU
                        || logType == CDefine.ELogType.LOG_COMM_ERROR_TEMPERATURE
                        || logType == CDefine.ELogType.LOG_COMM_ERROR_SAFETYPLC
                        || logType == CDefine.ELogType.LOG_COMM_STATISTICS_GMS
                        || logType == CDefine.ELogType.LOG_COMM_STATISTICS_MCU
                        || logType == CDefine.ELogType.LOG_COMM_STATISTICS_TEMPERATURE
                        || logType == CDefine.ELogType.LOG_COMM_STATISTICS_SAFETYPLC
                        )
                    {
                        if (false == m_objLogManager.Initialize(logType.ToString(), m_objConfig.GetLogPath(), true))
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (false == m_objLogManager.Initialize(logType.ToString(), m_objConfig.GetLogPath()))
                        {
                            break;
                        }
                    }
                }
                LogWrite.Initialize(this);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public double CountTactTime(DateTime before, DateTime after)
        {
            double dTactTime = 0d;

            if (false == after.Equals(default(DateTime))
                && false == before.Equals(default(DateTime))
                )
            {
                long ullDif = after.ToFileTime() - before.ToFileTime();
                dTactTime = ullDif / 10000d / 1000d;
                // 1시간 limit
                if (dTactTime > 3600d || dTactTime < 0d)
                {
                    dTactTime = 0d;
                }
            }

            return dTactTime;
        }

        /// <summary>
        /// 현재 설정된 런모드 타입을 읽어온다
        /// </summary>
        /// <returns>현재 런모드 타입</returns>
        public CDefine.ERunMode GetRunMode()
        {
            return m_eRunMode;
        }

        /// <summary>
        /// 런모드 타입을 설정한다
        /// </summary>
        /// <param name="eRunMode">런모드 타입</param>
        public void SetRunMode(CDefine.ERunMode eRunMode)
        {
            m_eRunMode = eRunMode;
        }

        /// <summary>
        /// 현재 런모드를 읽어온다
        /// </summary>
        /// <returns>현재 런모드</returns>
        public CDefine.ERunStatus GetRunStatus()
        {
            return m_eRunStatus;
        }

        /// <summary>
        /// 런모드를 설정한다
        /// </summary>
        /// <param name="eRunStatus">런모드</param>
        public void SetRunStatus(CDefine.ERunStatus eRunStatus)
        {
            m_eRunStatus = eRunStatus;
        }

        /// <summary>
        /// 현재 이동 모드를 읽어온다 (자동, 수동)
        /// </summary>
        /// <returns>현재 이동 모드</returns>
        public CDefine.EMoveModeType GetMoveModeType()
        {
            return m_eMoveModeType;
        }

        /// <summary>
        /// 이동 모드를 설정한다
        /// </summary>
        /// <param name="eMoveModeType">이동 모드</param>
        public void SetMoveModeType(CDefine.EMoveModeType eMoveModeType)
        {
            m_eMoveModeType = eMoveModeType;
        }

        /// <summary>
        /// 특정 폼이 열려있는지 확인함
        /// </summary>
        /// <param name="strTypeName">폼 타입 이름</param>
        /// <returns>폼 열림 상태</returns>
        public bool GetIsForm(string strTypeName)
        {
            bool bReturn = false;

            try
            {
                FormCollection objOpenForms = Application.OpenForms;
                int iCount = objOpenForms.Count;
                for (int iLoopCount = 0; iLoopCount < iCount; iLoopCount++)
                {
                    if (strTypeName == objOpenForms[iLoopCount].GetType().Name)
                    {
                        bReturn = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            return bReturn;
        }

        /// <summary>
        /// 메인 프레임 인스턴스 받음
        /// </summary>
        /// <returns>메인 프레임 인스턴스</returns>
        public CMainFrame GetMainFrame()
        {
            CMainFrame objMain = null;

            try
            {
                FormCollection objOpenForms = Application.OpenForms;
                int iCount = objOpenForms.Count;
                for (int iLoopCount = 0; iLoopCount < iCount; iLoopCount++)
                {
                    if (null != (objMain = objOpenForms[iLoopCount] as CMainFrame))
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            return objMain;
        }

        /// <summary>
        /// 공유 메모리에서 알람 유무 체크
        /// </summary>
        /// <returns>알람 상태 여부</returns>
        public bool GetIsAlarmMessage()
        {
            bool bReturn = false;

            bReturn = (0 < m_objAlarmList.Count);

            return bReturn;
        }

        /// <summary>
        /// 공유 메모리에 헤비 알람 유무 체크
        /// </summary>
        /// <returns>중알람 상태 여부</returns>
        public bool GetIsHeavyAlarm()
        {
            bool bReturn = false;

            bReturn = m_objAlarmList.IsHaveyAlarmSet;

            return bReturn;
        }

        public bool GetisLightAlarm()
        {
            bool bReturn = false;

            bReturn = m_objAlarmList.IsLightAlarmSet;

            return bReturn;
        }

        /// <summary>
        /// Alarm Code로 현재 알람이 발생했는지 체크하는 알람.
        /// </summary>
        /// <param name="eAlarmCode"></param>
        /// <returns></returns>
        public bool GetIsOnAlarm(CAlarmDefine.EAlarmList eAlarmCode)
        {
            return m_objAlarmList.GetIsOnAlarm(eAlarmCode);
        }

        /// <summary>
        /// 알람 폼으로 전환
        /// </summary>
        public void SetChangeAlarmForm()
        {
            CMainFrame objMain = null;

            do
            {
                if (null == (objMain = GetMainFrame()))
                {
                    break;
                }
                // 이미 알람폼인 경우 전환할 필요 없음
                if (null != objMain.GetCurrentForm() as CFormAlarm)
                {
                    break;
                }
                // 뷰가 알람 폼이 아니면 전환
                var setChangeAlarmForm = objMain.m_delegateSetChangeAlarmForm;
                if (null != setChangeAlarmForm)
                {
                    objMain.Invoke(setChangeAlarmForm);
                }
            } while (false);
        }

        /// <summary>
        /// 이니셜라이즈 폼으로 전환
        /// </summary>
        public void SetChangeInitializeForm()
        {
            CMainFrame objMain = null;

            do
            {
                if (null == (objMain = GetMainFrame()))
                {
                    break;
                }
                // 뷰가 이니셜라이즈 폼이 아니면 전환
                objMain.Invoke(objMain.m_delegateSetChangeInitilizeForm);
            } while (false);
        }

        /// <summary>
        /// 알람 갱신 토글
        /// </summary>
        public void SetAlarmMessageUpdate()
        {
            CMainFrame objMain = null;
            CFormAlarm objAlarm = null;

            do
            {
                if (null == (objMain = GetMainFrame()))
                {
                    break;
                }
                if (null == (objAlarm = objMain.GetCurrentForm() as CFormAlarm))
                {
                    break;
                }
                // 알람 갱신 토글
                objAlarm.Invoke(objAlarm.m_delegateSetAlarmMessageUpdate);
            } while (false);
        }

        /// <summary>
        /// 일반 메시지 (User Message DB에 있는 메시지에 다이얼로그를 띄운다.)
        /// </summary>
        /// <param name="eAlarmType">Message Dialog Type</param>
        /// <param name="eMessageID">메세지 ID</param>
        /// <returns>Message Dialog의 결과</returns>
        public DialogResult SetMessage(CDefine.EAlarmType eAlarmType, CAlarmDefine.EMessageList eMessageID, params object[] args)
        {
            CDefine.structureAlarmInformation objAlarmInformation = new CDefine.structureAlarmInformation();
            objAlarmInformation.eAlarmLevel = eAlarmType;
            objAlarmInformation.iAlarmCode = (int)eMessageID;
            objAlarmInformation.strAlarmObject = "";
            objAlarmInformation.strAlarmFunction = "";
            objAlarmInformation.strAlarmDescription = "";
            DialogResult result = 0;
            GetMainFrame().Invoke(new Action(() =>
            {
                using (var objMessage = new CDialogMessage(this, objAlarmInformation, false, args))
                {
                    objMessage.TopMost = true;
                    result = objMessage.ShowDialog();
                }
            }));
            return result;
        }

        /// <summary>
        /// 인터록 메시지 (자동 동작중이면 CIM에 보고하고 아니면 메시지 다이얼 로그만 띄운다)
        /// </summary>
        /// <param name="eInterlockID">인터록 메시지 ID</param>`
        public void SetAlarmEvent(CAlarmDefine.EAlarmList eInterlockID)
        {
            CDefine.structureAlarmInformation objAlarmInformation = new CDefine.structureAlarmInformation();
            objAlarmInformation.eAlarmLevel = CDefine.EAlarmType.ALARM_ALARM;
            objAlarmInformation.iAlarmCode = (int)eInterlockID;
            objAlarmInformation.strAlarmObject = "";
            objAlarmInformation.strAlarmFunction = "";
            objAlarmInformation.strAlarmDescription = "";
            SetAlarmEvent(objAlarmInformation);
        }

        /// <summary>
        /// 알람 메시지 (자동 동작중이면 CIM에 보고하고 아니면 메시지 다이얼 로그만 띄운다)
        /// </summary>
        /// <param name="objAlarmInformation">structureAlarmInformation의 객체</param>
        /// <remarks>
        /// 자동 동작중 - CIM에 알람을 보고한다
        /// 자동 이외의 상태 - 메시지 다이얼 로그를 띄운다
        /// </remarks>
        public void SetAlarmEvent(CDefine.structureAlarmInformation objAlarmInformation)
        {
            switch (GetRunStatus())
            {
                // 자동 동작중
                case CDefine.ERunStatus.Start:
                case CDefine.ERunStatus.Stopping:
                case CDefine.ERunStatus.LoadingStop:
                    // CIM에 보고한다
                    SetForcedAlarm((CAlarmDefine.EAlarmList)objAlarmInformation.iAlarmCode);
                    break;
                // 자동 이외의 상태 (배치 동작중, 초기화중, 메뉴얼 동작중 등...)
                default:
                    GetMainFrame().Invoke(new Action(() =>
                    {
                        using (var objMessage = new CDialogMessage(this, objAlarmInformation, true))
                        {
                            objMessage.TopMost = true;
                            // 다이얼 로그를 띄운다
                            objMessage.ShowDialog();
                        }
                    }));
                    break;
            }
        }

        /// <summary>
        /// 알람 메세지 (호출시 상태에 관계없이 무조건 중알람을 발생 시키고, CIM에 보고한다)
        /// </summary>
        /// <param name="eAlarmID">알람 ID</param>
        /// <remarks>
        /// 상태에 관계없이 중알람을 발생시켜야하는 세이프티 관련 알람에만 사용한다.
        /// </remarks>
        public void SetForcedAlarm(CAlarmDefine.EAlarmList eAlarmID)
        {
            if (
                m_bIsDatabaseInitialized == false
                || m_objAlarmList.IsInitialized == false
                )
            {
                Thread.Sleep(100);
                return;
            }

            do
            {
                int alarmID = (int)eAlarmID;
                // 알람 아이디가 0보다 작으면 스킵
                if (0 >= alarmID)
                {
                    break;
                }

                lock (m_objAlarmList)
                {
                    try
                    {
                        //if (false == m_bIsHeavyAlarmSet)
                        {
                            // 헤비 알람 체크
                            if (
                                true == m_objAlarmList.GetIsHeavyAlarmCode(eAlarmID)
                                && true == m_objAlarmList.GetIsOnAlarm(eAlarmID)
                                )
                            {
                                break;
                            }
                        }

                        // 발생 알람 리스트에 추가한다.
                        DateTime alarmOccuredTime = m_objAlarmList.CreateAlarmOccuredDateTime();
                        m_objAlarmList.Add(alarmOccuredTime, eAlarmID);
                        // 처음 알람이 발생한 경우 덤프를 남긴다       
                        if (
                            0 < m_objAlarmList.Count
                            && true == m_objAlarmList.IsHaveyAlarmSet
                            && m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE] != CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_DOWN
                            )
                        {
                            EquipmentDump("FIRST_HEAVY_ALARM_OCCURED");
                            MccLogManager.GetAlarmActions().BatchWriteStart();
                            MccLogManager.ShouldWriteAlarmActionEndLog = true;
                        }
                        // 알람폼 전환
                        SetChangeAlarmForm();
                        // 알람 뷰 가져옴
                        SetAlarmMessageUpdate();
                        // 새로운 알람이 발생하면 부져를 다시킴
                        m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.Fault);
                        m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                        // 알람 히스토리 삽입
                        m_objProcessDatabase.m_objDatabaseSendMessage.SetInsertHistoryAlarm(alarmOccuredTime.ToString(CDatabaseDefine.DEF_DATE_TIME_FORMAT), alarmID);
                        // CIM 보고
                        if (false == NotReportAlarmList.Contains(eAlarmID))
                        {
                            AlarmReport objAlarmReport = new AlarmReport();
                            objAlarmReport.EQPID = m_objConfig.GetSystemParameter().strEquipmentID;
                            objAlarmReport.BODY.ALID = string.Format("{0}", alarmID);
                            objAlarmReport.BODY.ALST = string.Format("{0}", (int)CCIMDefine.EAlarmState.ALARM_STATE_SET);
                            m_objProcessCIM.m_objCIMSendMessage.SetAlarmReport(objAlarmReport);
                        }

                        // 중알람일 경우
                        if (true == m_objAlarmList.IsHaveyAlarmSet)
                        {
                            // 설비를 정지한다.
                            switch (GetRunStatus())
                            {
                                case CDefine.ERunStatus.Pause:
                                case CDefine.ERunStatus.Stop:
                                    // 정지 상태면 상태를 변경하지 않음
                                    m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_DOWN;
                                    break;
                                case CDefine.ERunStatus.LoadingStop:
                                case CDefine.ERunStatus.Start:
                                case CDefine.ERunStatus.Initialize:
                                case CDefine.ERunStatus.Setup:
                                    // 동작중이면 정지 진행중 상태로 변경
                                    SetRunStatus(CDefine.ERunStatus.Stopping);
                                    break;
                                case CDefine.ERunStatus.Stopping:
                                    break;
                            }
                        }

                        SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, string.Format("[ALARM] [ALID:{0:00000}] [ALTX:{1}]", alarmID, eAlarmID));
                    }
                    catch (Exception ex)
                    {
                        LogWrite.Exception(ex);
                    }
                }
            } while (false);
        }

        /// <summary>
        /// 모든 알람 메세지 클리어
        /// </summary>
        public void SetAlarmClear()
        {
            lock (m_objAlarmList)
            {
                foreach (var alarm in m_objAlarmList.SetAlarms)
                {
                    if (false == NotReportAlarmList.Contains(alarm.Value))
                    {
                        // CIM 보고
                        AlarmReport objAlarmReport = new AlarmReport();
                        objAlarmReport.EQPID = m_objConfig.GetSystemParameter().strEquipmentID;
                        objAlarmReport.BODY.ALID = string.Format("{0}", (int)alarm.Value);
                        objAlarmReport.BODY.ALST = string.Format("{0}", (int)CCIMDefine.EAlarmState.ALARM_STATE_RESET);
                        m_objProcessCIM.m_objCIMSendMessage.SetAlarmReport(objAlarmReport);
                    }
                }

                // 알람 발생 리스트 초기화
                m_objAlarmList.Clear();
            }
        }

        /// <summary>
        /// 알람 메세지 클리어 (개별. 해당 ID에 일치하는 알람만 삭제)
        /// </summary>
        /// <param name="alarmDateTime">알람 발생 시간</param>
        /// <param name="iAlarmID">알람 ID</param>
        public void SetAlarmClear(DateTime alarmDateTime, int iAlarmID)
        {
            if (0 < m_objAlarmList.Count)
            {
                lock (m_objAlarmList)
                {
                    var alarmID = (CAlarmDefine.EAlarmList)iAlarmID;
                    if (
                        true == m_objAlarmList.GetIsOnAlarm(alarmID)
                        && false == NotReportAlarmList.Contains(alarmID)
                        )
                    {
                        // CIM 보고
                        AlarmReport objAlarmReport = new AlarmReport();
                        objAlarmReport.EQPID = m_objConfig.GetSystemParameter().strEquipmentID;
                        objAlarmReport.BODY.ALID = string.Format("{0}", iAlarmID);
                        objAlarmReport.BODY.ALST = string.Format("{0}", (int)CCIMDefine.EAlarmState.ALARM_STATE_RESET);
                        m_objProcessCIM.m_objCIMSendMessage.SetAlarmReport(objAlarmReport);
                    }

                    // 알람 발생 리스트 초기화
                    m_objAlarmList.Remove(alarmDateTime, (CAlarmDefine.EAlarmList)iAlarmID);
                }
            }
        }

        /// <summary>
        /// SVID 리스트 갱신
        /// </summary>
        /// <param name="objSVIDList">SVID 리스트</param>
        public void SetSvidList(List<object> objSVIDList)
        {
            CMainFrame objMain = null;
            CFormMainCIM objCIM = null;

            do
            {
                if (null == (objMain = GetMainFrame()))
                {
                    break;
                }
                m_objSVIDList = objSVIDList;
                if (null == (objCIM = objMain.GetCurrentForm() as CFormMainCIM))
                {
                    break;
                }
                // 현재 폼이 Main CIM인 경우 SVID List 델리게이트 호출
                if (null == objCIM.m_delegateSetSVIDListUpdate)
                {
                    break;
                }
                objCIM.Invoke(objCIM.m_delegateSetSVIDListUpdate, objSVIDList);
            } while (false);
        }

        /// <summary>
        /// 현재 SVID 리스트를 읽어온다
        /// </summary>
        /// <returns>현재 SVID 리스트</returns>
        public List<object> GetSVIDList()
        {
            return m_objSVIDList;
        }

        /// <summary>
        /// 로그메세지 전송 (Button Operation 전용)
        /// </summary>
        /// <param name="objForm">Form의 객체</param>
        /// <param name="strButton">버튼 이름</param>
        public void SetUpdateButtonLog(Form objForm, string strButton)
        {
            SetUpdateLog(CDefine.ELogType.LOG_BUTTON_OPERATION, string.Format("[{0}] [{1}] {2}", GetUserInformation().m_strID, objForm.Name, strButton));
        }

        /// <summary>
        /// 초기화 프로세스 로그 추가
        /// </summary>
        /// <param name="logMessage">초기화 로그 메세지</param>
        public void SetInitializeProcessLog(string logMessage)
        {
            CMainFrame objMain = null;
            CFormSetupInitialize objInitialize = null;
            do
            {
                if (
                    null == (objMain = GetMainFrame())
                    || null == (objInitialize = objMain.GetCurrentForm() as CFormSetupInitialize)
                    )
                {
                    break;
                }
                objInitialize.SetInitializeProcessLog(logMessage);
            } while (false);
        }

        /// <summary>
        /// 초기화 에러 로그 추가
        /// </summary>
        /// <param name="logMessage">초기화 로그 메세지</param>
        public void SetInitializeErrorLog(string logMessage)
        {
            CMainFrame objMain = null;
            CFormSetupInitialize objInitialize = null;
            do
            {
                if (
                    null == (objMain = GetMainFrame())
                    || null == (objInitialize = objMain.GetCurrentForm() as CFormSetupInitialize)
                    )
                {
                    break;
                }
                objInitialize.SetInitializeErrorLog(logMessage);
            } while (false);
        }

        /// <summary>
        /// 로그메세지 전송
        /// </summary>
        /// <param name="eLogType">저장할 로그 종류</param>
        /// <param name="strLogMessage">로그 메세지</param>
        public void SetUpdateLog(CDefine.ELogType eLogType, string strLogMessage)
        {
            do
            {
                m_objLogManager.WriteLog(eLogType.ToString(), strLogMessage);
                MtbiDataCollector.WriteLog(eLogType, strLogMessage);
                OnLogging?.BeginInvoke(this, new EventArgsLogEvent(eLogType, strLogMessage), null, null);

            } while (false);
        }

        /// <summary>
        /// UI 텍스트 데이터 테이블에서 해당 ID에 해당하는 문자열을 리턴
        /// </summary>
        /// <param name="strID">UI 텍스트 ID</param>
        /// <param name="strFormName">폼 이름</param>
        /// <returns>UI 텍스트 문자열</returns>
        public string GetDatabaseUIText(string strID, string strFormName)
        {
            string strReturn = "";
            CDefine.ELanguage eLanguage = m_objConfig.GetOptionParameter().eLanguage;
            CManagerTable objManagerTable = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUIText;

            do
            {
                try
                {
                    DataTable objDataTable = objManagerTable.HLGetDataTable();
                    DataRow[] objDataRow = objDataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}'",
                        objManagerTable.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationUIText.ID], strID,
                        objManagerTable.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationUIText.FORM_NAME], strFormName));

                    // 언어에 따라서 변경
                    if (CDefine.ELanguage.LANGUAGE_KOREA == eLanguage)
                    {
                        strReturn = objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUIText.TEXT_KOREA].ToString();
                    }
                    else if (CDefine.ELanguage.LANGUAGE_CHINA == eLanguage)
                    {
                        strReturn = objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUIText.TEXT_CHINA].ToString();
                    }
                    else if (CDefine.ELanguage.LANGUAGE_ENGLISH == eLanguage)
                    {
                        strReturn = objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUIText.TEXT_ENGLISH].ToString();
                    }
                    else if (CDefine.ELanguage.LANGUAGE_VIETNAM == eLanguage)
                    {
                        strReturn = objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUIText.TEXT_VIETNAM].ToString();
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    strReturn = "Not Defined Message";
                }
            } while (false);

            return strReturn;
        }

        /// <summary>
        /// 유저 메세지 데이터 테이블에서 해당 ID에 해당하는 문자열을 리턴
        /// </summary>
        /// <param name="iID">유져 메세지 ID</param>
        /// <returns>유저 메세지 문자열</returns>
        public string GetDatabaseUserMessage(int iID)
        {
            return GetDatabaseUserMessage(iID, m_objConfig.GetOptionParameter().eLanguage);
        }

        /// <summary>
        /// 유저 메세지 데이터 테이블에서 해당 ID에 해당하는 문자열을 리턴
        /// </summary>
        /// <param name="iID">유져 메세지 ID</param>
        /// <param name="eLanguage">언어 종류</param>
        /// <returns>유저 메세지 문자열</returns>
        public string GetDatabaseUserMessage(int iID, CDefine.ELanguage eLanguage)
        {
            string strReturn = "";
            CManagerTable objManagerTable = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUserMessage;

            do
            {
                try
                {
                    DataTable objDataTable = objManagerTable.HLGetDataTable();
                    DataRow[] objDataRow = objDataTable.Select(string.Format("{0} = '{1}'", objManagerTable.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationUserMessage.ID], iID));
                    // 언어에 따라서 변경
                    if (0 != objDataRow.Length)
                    {
                        if (CDefine.ELanguage.LANGUAGE_KOREA == eLanguage)
                        {
                            strReturn = objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUserMessage.TEXT_KOREA].ToString();
                        }
                        else if (CDefine.ELanguage.LANGUAGE_CHINA == eLanguage)
                        {
                            strReturn = objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUserMessage.TEXT_CHINA].ToString();
                        }
                        else if (CDefine.ELanguage.LANGUAGE_ENGLISH == eLanguage)
                        {
                            strReturn = objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUserMessage.TEXT_ENGLISH].ToString();
                        }
                        else if (CDefine.ELanguage.LANGUAGE_VIETNAM == eLanguage)
                        {
                            strReturn = objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUserMessage.TEXT_VIETNAM].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);

            return strReturn;
        }

        /// <summary>
        /// 해당 유저 아이디에 해당하는 유저 레코드를 뽑아냄
        /// </summary>
        /// <param name="strID">유저 ID</param>
        /// <returns>유저 레코드</returns>
        public CUserInformation GetDatabaseUser(string strID)
        {
            CUserInformation objUserInformation = null;
            CManagerTable objManagerTable = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUser;

            do
            {
                try
                {
                    DataTable objDataTable = objManagerTable.HLGetDataTable();
                    DataRow[] objDataRow = objDataTable.Select(string.Format("{0} = '{1}'", objManagerTable.HLGetTableSchemaName()[objManagerTable.HLGetPrimaryKey()], strID));

                    CDefine.EUserAuthorityLevel eUserAuthorityLevel = CDefine.EUserAuthorityLevel.OPERATOR;
                    if (CDefine.EUserAuthorityLevel.OPERATOR.ToString() == objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUser.AUTHORITY_LEVEL].ToString())
                    {
                        eUserAuthorityLevel = CDefine.EUserAuthorityLevel.OPERATOR;
                    }
                    else if (CDefine.EUserAuthorityLevel.ENGINEER.ToString() == objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUser.AUTHORITY_LEVEL].ToString())
                    {
                        eUserAuthorityLevel = CDefine.EUserAuthorityLevel.ENGINEER;
                    }
                    else if (CDefine.EUserAuthorityLevel.MASTER.ToString() == objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUser.AUTHORITY_LEVEL].ToString())
                    {
                        eUserAuthorityLevel = CDefine.EUserAuthorityLevel.MASTER;
                    }

                    // 유저 정보 집어넣음 id / name / password / level
                    objUserInformation = new CUserInformation(
                        objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUser.ID].ToString(),
                        objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUser.NAME].ToString(),
                        objDataRow[0].ItemArray[(int)CDatabaseDefine.EInformationUser.PASSWORD].ToString(),
                        eUserAuthorityLevel);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);

            return objUserInformation;
        }

        /// <summary>
        /// 현재 유저 정보 확인
        /// </summary>
        /// <returns>현재 유저 정보</returns>
        public CUserInformation GetUserInformation()
        {
            return m_objUserInformation.Clone() as CUserInformation;
        }

        /// <summary>
        /// 입력된 유저 정보로 로그인
        /// </summary>
        /// <param name="objUserInformation">유저 정보</param>
        public void SetLogin(CUserInformation objUserInformation)
        {
            m_objUserInformation = objUserInformation.Clone() as CUserInformation;
            // 로그인 로그
            string strAuthorityLevel = "";
            switch (m_objUserInformation.m_eAuthorityLevel)
            {
                case CDefine.EUserAuthorityLevel.OPERATOR:
                    strAuthorityLevel = "OPERATOR";
                    break;
                case CDefine.EUserAuthorityLevel.ENGINEER:
                    strAuthorityLevel = "ENGINEER";
                    break;
                case CDefine.EUserAuthorityLevel.MASTER:
                    strAuthorityLevel = "MASTER";
                    break;
            }
            string strLog = string.Format("[LOGIN] ID : {0} NAME : {1} LEVEL : {2}", m_objUserInformation.m_strID, m_objUserInformation.m_strName, strAuthorityLevel);
            SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, strLog);
            // 유저 권한에 따른 리소스 상태 변경 호출
            SetResourceControl();
        }

        /// <summary>
        /// 유저 권한에 따른 리소스 상태 변경 호출 (상태 변경은 각 폼에서 적용)
        /// </summary>
        public void SetResourceControl()
        {
            // 현재 폼에서 유저 권한에 따른 버튼 상태 변경 델리게이트가 생성되어 있으면 호출
            CFormCommon objForm = GetMainFrame().GetCurrentForm();
            if (null != objForm)
            {
                if (null != objForm.m_delegateSetResourceControl)
                {
                    objForm.Invoke(objForm.m_delegateSetResourceControl);
                }
            }
            // 메뉴 폼에서 유저 권한에 따른 버튼 상태 변경 델리게이트가 생성되어 있으면 호출
            CFormCommon objMenu = GetMainFrame().GetFormMenu() as CFormCommon;
            if (null != objMenu)
            {
                if (null != objMenu.m_delegateSetResourceControl)
                {
                    objMenu.Invoke(objMenu.m_delegateSetResourceControl);
                }
            }
        }

        private Task mLogoutProcess;

        /// <summary>
        /// 로그아웃
        /// </summary>
        public void SetLogout()
        {
            if (mLogoutProcess == null
                || mLogoutProcess.Status == TaskStatus.RanToCompletion
                )
            {
                mLogoutProcess = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        // CIM 정보로 로그인한 경우
                        if (
                            m_objUserInformation.IsLoginFromCimInformation == true
                            && m_objConfig.GetOptionParameter().bUseCIM == true
                            )
                        {
                            var cim = m_objProcessCIM;
                            cim.m_objProcessCIMTerminalDisplayRequest.LogoutAcknowledge.Reset();
                            cim.m_objProcessCIMEquipmentApproveSend.GetReceivedData();
                            OperatorInfoEvent operatorInformation = new OperatorInfoEvent();
                            operatorInformation.BODY.OPTIONINFO = "LOGOUT";
                            CCryptography objEncoder = new CCryptography();
                            operatorInformation.BODY.OPERATORID = m_objUserInformation.m_strID;
                            operatorInformation.BODY.PASSWORD = objEncoder.SetDecoding(m_objUserInformation.m_strPassword);
                            cim.m_objCIMSendMessage.SetOperatorInfoEvent(operatorInformation);

                            // 응답 대기
                            GetMainFrame().ShowWaitMessage(true, "WAIT FOR LOGOUT REPLY");
                            TimeSpan timeout = Config.WaitTime.CommTimeout.CimLoginTimeout.ToTimeSpan();
                            if (cim.m_objProcessCIMEquipmentApproveSend.WaitForReceive(timeout) == false)
                            {
                                // Msg: TC에서 응답이 없습니다.
                                SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOGOUT_FAILED_TC_TIMEOUT);
                                GetMainFrame().ShowWaitMessage(false);
                                return;
                            }
                            GetMainFrame().ShowWaitMessage(false);

                            var reply = cim.m_objProcessCIMEquipmentApproveSend.GetReceivedData();

                            // 로그아웃 성공 여부 확인
                            if (reply.BODY.RCMD != "31")
                            {
                                // 터미널 메시지 팝업
                                return;
                            }
                        }

                        m_objUserInformation = new CUserInformation();
                        IsMasterLogin = false;
                        string strLog = string.Format("[LOGOUT]");
                        SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, strLog);
                    }
                    finally
                    {
                        mLogoutProcess = null;
                    }
                });
            }
        }

        /// <summary>
        /// EC 리스트 데이터 전체 빼옴
        /// </summary>
        /// <returns>EC 리스트 데이터</returns>
        public List<CConfig.CECListData> GetECList()
        {
            List<CConfig.CECListData> objECList = new List<CConfig.CECListData>();

            do
            {
                try
                {
                    var objEcmParameter = m_objConfig.GetEcmParameter();
                    foreach (CConfig.EEquipmentEcmList ecmIndex in Enum.GetValues(typeof(CConfig.EEquipmentEcmList)))
                    {
                        CConfig.CECListData objEC = new CConfig.CECListData();

                        objEC.strECID = objEcmParameter.ECID[ecmIndex].ToString();
                        objEC.strECItemName = objEcmParameter.ECItemName[ecmIndex];
                        objEC.strECDef = objEcmParameter.ECDef[ecmIndex].ToString();
                        objEC.strECSll = objEcmParameter.ECSll[ecmIndex].ToString();
                        objEC.strECSul = objEcmParameter.ECSul[ecmIndex].ToString();
                        objEC.strECWll = objEcmParameter.ECWll[ecmIndex].ToString();
                        objEC.strECWul = objEcmParameter.ECWul[ecmIndex].ToString();
                        objECList.Add(objEC);
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);

            return objECList;
        }

        /// <summary>
        /// EC 리스트 변경사항 저장 (DataBase 항목제외, DataBase 항목은 Setup DB에서 접근)
        /// </summary>
        /// <param name="ecmParameterOrNull"></param>
        /// <returns>EC 리스트 데이터</returns>
        public void SetECList(CConfig.CEcmParameter ecmParameterOrNull = null, bool bReportCim = true)
        {
            var objECListBefore = GetECList();
            if (ecmParameterOrNull == null)
            {
                ecmParameterOrNull = m_objConfig.GetEcmParameter().DeepClone();
            }

            // InShuttle
            {
                var inShuttle = m_objProcessMain.m_objProcessMotion.InShuttle;
                var motorPositionX = inShuttle.MotorStageX.Axis.HLGetMotorPosition();
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_SHUTTLE_LOAD_X_POS, motorPositionX, (int)InShuttleMotorX.EPosition.Load);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_SHUTTLE_WAIT_SCAN_X_POS, motorPositionX, (int)InShuttleMotorX.EPosition.ScanStartWait);
                // ! `IN_SHUTTLE_SCAN_TRIGGER_START_X_POSI`항목은 속도 값이 없는 항목으로 `SetEcmDataMotorPosition`함수를 사용함
                SetEcmDataMotorPosition(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_SHUTTLE_SCAN_TRIGGER_START_X_POS, motorPositionX, (int)InShuttleMotorX.EPosition.ScanTriggerStart);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_SHUTTLE_SCAN_TRIGGER_END_X_POS, motorPositionX, (int)InShuttleMotorX.EPosition.ScanTriggerEnd);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_SHUTTLE_ALIGN_X_POS, motorPositionX, (int)InShuttleMotorX.EPosition.Align);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_SHUTTLE_UNLOAD_X_POS, motorPositionX, (int)InShuttleMotorX.EPosition.Unload);
            }
            // InspStage
            {
                var inspStage = m_objProcessMain.m_objProcessMotion.InspStage;
                var motorPositionY = inspStage.MotorStageY.Axis.HLGetMotorPosition();
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.INSP_STAGE_LOAD_Y_POS, motorPositionY, (int)InspStageMotorY.EPosition.Load);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.INSP_STAGE_P1_GRAB_Y_POS, motorPositionY, (int)InspStageMotorY.EPosition.GrabP1);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.INSP_STAGE_P2_GRAB_Y_POS, motorPositionY, (int)InspStageMotorY.EPosition.GrabP2);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.INSP_STAGE_UNLOAD_Y_POS, motorPositionY, (int)InspStageMotorY.EPosition.Unload);
            }
            // OutFlip
            {
                var outFlip = m_objProcessMain.m_objProcessMotion.OutFlip;
                var motorPositionR1 = outFlip.MotorRs[0].Axis.HLGetMotorPosition();
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_FLIP_LOAD_R1_POS, motorPositionR1, (int)OutFlipMotorR.EPosition.Load);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_FLIP_UNLOAD_R1_POS, motorPositionR1, (int)OutFlipMotorR.EPosition.Unload);
                var motorPositionR2 = outFlip.MotorRs[1].Axis.HLGetMotorPosition();
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_FLIP_LOAD_R2_POS, motorPositionR2, (int)OutFlipMotorR.EPosition.Load);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_FLIP_UNLOAD_R2_POS, motorPositionR2, (int)OutFlipMotorR.EPosition.Unload);
                var motorPositionZ = outFlip.MotorZ.Axis.HLGetMotorPosition();
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_FLIP_UP_Z_POS, motorPositionZ, (int)OutFlipMotorZ.EPosition.Up);
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_FLIP_DOWN_Z_POS, motorPositionZ, (int)OutFlipMotorZ.EPosition.Down);
                var motorPositionX = outFlip.MotorConveyorX.Axis.HLGetMotorPosition();
                SetEcmDataMotor(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_CONVEYOR_UNLOAD_X_POS, motorPositionX, (int)OutFlipMotorX.EPosition.Unload);
            }
            // InRobot
            {
                var inRobotRmsData = m_objProcessMain.m_objProcessMotion.InRobot.Nachi.Robot.ReceiveDataRms;
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_HOME_POS_X, inRobotRmsData, ERobotProcess.None, ERmsDataPosition.Home);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_PICKUP_APP_POS_X, inRobotRmsData, ERobotProcess.P1, ERmsDataPosition.Approach);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_PICKUP_IN_WAIT_POS_X, inRobotRmsData, ERobotProcess.P1, ERmsDataPosition.InWait);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_PICKUP_OUT_WAIT_POS_X, inRobotRmsData, ERobotProcess.P1, ERmsDataPosition.OutWait);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T1_S1_PICKUP_UP_POS_X, inRobotRmsData, ERobotProcess.P1, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T1_S1_PICKUP_DOWN_POS_X, inRobotRmsData, ERobotProcess.P1, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T2_S2_PICKUP_UP_POS_X, inRobotRmsData, ERobotProcess.P1, ETool.Tool2, EStage.Stage2);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T2_S2_PICKUP_DOWN_POS_X, inRobotRmsData, ERobotProcess.P1, ETool.Tool2, EStage.Stage2);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_MCR_APP_POS_X, inRobotRmsData, ERobotProcess.P3, ERmsDataPosition.Approach);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_MCR_IN_WAIT_POS_X, inRobotRmsData, ERobotProcess.P3, ERmsDataPosition.InWait);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_MCR_OUT_WAIT_POS_X, inRobotRmsData, ERobotProcess.P3, ERmsDataPosition.OutWait);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T1_S1_MCR_UP_POS_X, inRobotRmsData, ERobotProcess.P3, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T1_S1_MCR_DOWN_POS_X, inRobotRmsData, ERobotProcess.P3, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T2_S2_MCR_UP_POS_X, inRobotRmsData, ERobotProcess.P3, ETool.Tool2, EStage.Stage2);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T2_S2_MCR_DOWN_POS_X, inRobotRmsData, ERobotProcess.P3, ETool.Tool2, EStage.Stage2);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T12_S12_MCR_UP_POS_X, inRobotRmsData, ERobotProcess.P3, ETool.Tool12, EStage.Stage1);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T12_S12_MCR_DOWN_POS_X, inRobotRmsData, ERobotProcess.P3, ETool.Tool12, EStage.Stage1);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_PLACE_APP_POS_X, inRobotRmsData, ERobotProcess.P4, ERmsDataPosition.Approach);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_PLACE_IN_WAIT_POS_X, inRobotRmsData, ERobotProcess.P4, ERmsDataPosition.InWait);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_PLACE_OUT_WAIT_POS_X, inRobotRmsData, ERobotProcess.P4, ERmsDataPosition.OutWait);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T1_S1_PLACE_UP_POS_X, inRobotRmsData, ERobotProcess.P4, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T1_S1_PLACE_DOWN_POS_X, inRobotRmsData, ERobotProcess.P4, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T2_S2_PLACE_UP_POS_X, inRobotRmsData, ERobotProcess.P4, ETool.Tool2, EStage.Stage2);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.IN_ROBOT_T2_S2_PLACE_DOWN_POS_X, inRobotRmsData, ERobotProcess.P4, ETool.Tool2, EStage.Stage2);
            }
            // OutRobot
            {
                var outRobotRmsData = m_objProcessMain.m_objProcessMotion.OutRobot.Nachi.Robot.ReceiveDataRms;
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_HOME_POS_X, outRobotRmsData, ERobotProcess.None, ERmsDataPosition.Home);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_PICKUP_APP_POS_X, outRobotRmsData, ERobotProcess.P1, ERmsDataPosition.Approach);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_PICKUP_IN_WAIT_POS_X, outRobotRmsData, ERobotProcess.P1, ERmsDataPosition.InWait);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_PICKUP_OUT_WAIT_POS_X, outRobotRmsData, ERobotProcess.P1, ERmsDataPosition.OutWait);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_T1_S1_PICKUP_UP_POS_X, outRobotRmsData, ERobotProcess.P1, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_T1_S1_PICKUP_DOWN_POS_X, outRobotRmsData, ERobotProcess.P1, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_T2_S2_PICKUP_UP_POS_X, outRobotRmsData, ERobotProcess.P1, ETool.Tool2, EStage.Stage2);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_T2_S2_PICKUP_DOWN_POS_X, outRobotRmsData, ERobotProcess.P1, ETool.Tool2, EStage.Stage2);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_PLACE_APP_POS_X, outRobotRmsData, ERobotProcess.P4, ERmsDataPosition.Approach);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_PLACE_IN_WAIT_POS_X, outRobotRmsData, ERobotProcess.P4, ERmsDataPosition.InWait);
                SetEcmDataRobot(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_PLACE_OUT_WAIT_POS_X, outRobotRmsData, ERobotProcess.P4, ERmsDataPosition.OutWait);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_T1_S1_PLACE_UP_POS_X, outRobotRmsData, ERobotProcess.P4, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_T1_S1_PLACE_DOWN_POS_X, outRobotRmsData, ERobotProcess.P4, ETool.Tool1, EStage.Stage1);
                SetEcmDataRobotUp(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_T2_S2_PLACE_UP_POS_X, outRobotRmsData, ERobotProcess.P4, ETool.Tool2, EStage.Stage2);
                SetEcmDataRobotDown(ecmParameterOrNull, CConfig.EEquipmentEcmList.OUT_ROBOT_T2_S2_PLACE_DOWN_POS_X, outRobotRmsData, ERobotProcess.P4, ETool.Tool2, EStage.Stage2);
            }

            m_objConfig.SaveEcmParameter(ecmParameterOrNull);

            if (bReportCim == true)
            {
                var objECListAfter = GetECList();
                if (ChangeECData(objECListBefore, objECListAfter) == true)
                {
                    ECChangeEvent objEcChangeEvent = new ECChangeEvent();
                    objEcChangeEvent.BODY.ECS = new ECChangeEvent._ECChangeEvent.EC[objECListAfter.Count];
                    objEcChangeEvent.EQPID = m_objConfig.GetSystemParameter().strEquipmentID;

                    for (int iLoopCount = 0; iLoopCount < objECListAfter.Count; iLoopCount++)
                    {
                        objEcChangeEvent.BODY.ECS[iLoopCount].ECID = objECListAfter[iLoopCount].strECID;
                        objEcChangeEvent.BODY.ECS[iLoopCount].ECNAME = objECListAfter[iLoopCount].strECItemName;
                        objEcChangeEvent.BODY.ECS[iLoopCount].ECDEF = objECListAfter[iLoopCount].strECDef;
                        objEcChangeEvent.BODY.ECS[iLoopCount].ECSLL = objECListAfter[iLoopCount].strECSll;
                        objEcChangeEvent.BODY.ECS[iLoopCount].ECSUL = objECListAfter[iLoopCount].strECSul;
                        objEcChangeEvent.BODY.ECS[iLoopCount].ECWLL = objECListAfter[iLoopCount].strECWll;
                        objEcChangeEvent.BODY.ECS[iLoopCount].ECWUL = objECListAfter[iLoopCount].strECWul;
                    }

                    m_objProcessCIM.m_objCIMSendMessage.SetEcChangeEvent(objEcChangeEvent);
                }
            }
        }

        private void SetEcmDataMotor(CConfig.CEcmParameter ecmParameter, CConfig.EEquipmentEcmList startIndex, CDeviceMotorAbstract.CMotorPosition positionData, int positionIndex)
        {
            ecmParameter.ECDef[startIndex + 0] = positionData.dPosition[positionIndex];
            ecmParameter.ECDef[startIndex + 1] = positionData.dVelocity[positionIndex];
        }

        private void SetEcmDataMotorPosition(CConfig.CEcmParameter ecmParameter, CConfig.EEquipmentEcmList startIndex, CDeviceMotorAbstract.CMotorPosition positionData, int positionIndex)
        {
            ecmParameter.ECDef[startIndex + 0] = positionData.dPosition[positionIndex];
        }

        private void SetEcmDataRobot(CConfig.CEcmParameter ecmParameter, CConfig.EEquipmentEcmList startIndex, RmsData rmsData, ERobotProcess robotProcess, ERmsDataPosition rmsDataType)
        {
            var selectData = robotProcess == ERobotProcess.None ? rmsData.ToolDownData.GetRmsAfterProcessData(rmsDataType) : rmsData.ToolDownData.GetRmsData(robotProcess, rmsDataType);
            ecmParameter.ECDef[startIndex + 0] = selectData.X;
            ecmParameter.ECDef[startIndex + 1] = selectData.Y;
            ecmParameter.ECDef[startIndex + 2] = selectData.Rz;
            ecmParameter.ECDef[startIndex + 3] = selectData.Z;
        }

        private void SetEcmDataRobotUp(CConfig.CEcmParameter ecmParameter, CConfig.EEquipmentEcmList startIndex, RmsData rmsData, ERobotProcess robotProcess, ETool toolIndex, EStage stageIndex)
        {
            var selectData = rmsData.ToolDownData.GetRmsUpData(robotProcess, toolIndex, stageIndex);
            ecmParameter.ECDef[startIndex + 0] = selectData.X;
            ecmParameter.ECDef[startIndex + 1] = selectData.Y;
            ecmParameter.ECDef[startIndex + 2] = selectData.Rz;
            ecmParameter.ECDef[startIndex + 3] = selectData.Z;
        }

        private void SetEcmDataRobotDown(CConfig.CEcmParameter ecmParameter, CConfig.EEquipmentEcmList startIndex, RmsData rmsData, ERobotProcess robotProcess, ETool toolIndex, EStage stageIndex)
        {
            var selectData = rmsData.ToolDownData.GetRmsDownData(robotProcess, toolIndex, stageIndex);
            ecmParameter.ECDef[startIndex + 0] = selectData.X;
            ecmParameter.ECDef[startIndex + 1] = selectData.Y;
            ecmParameter.ECDef[startIndex + 2] = selectData.Rz;
            ecmParameter.ECDef[startIndex + 3] = selectData.Z;
        }

        public bool ChangeECData(List<CConfig.CECListData> beforeData, List<CConfig.CECListData> afterData)
        {
            bool bChanged = false;
            int iLoopCount = 0;

            foreach (CConfig.CECListData objEC in afterData)
            {
                if (
                    beforeData[iLoopCount].strECDef != objEC.strECDef
                    || beforeData[iLoopCount].strECSll != objEC.strECSll
                    || beforeData[iLoopCount].strECSul != objEC.strECSul
                    || beforeData[iLoopCount].strECWll != objEC.strECWll
                    || beforeData[iLoopCount].strECWul != objEC.strECWul
                   )
                {
                    bChanged = true;
                    break;
                }
                iLoopCount++;
            }
            return bChanged;
        }

        /// <summary>
        /// 아날로그 값을 RPM으로 바꾸는 공식
        /// </summary>
        /// <param name="dVoltage">아날로그 값</param>
        /// <returns></returns>
        public double GetVoltageToRPM(CDeviceIODefine.EAnalogInput ioIndex)
        {
            double dReturn = 0.0;

            do
            {
                double readValue = 0;
                m_objProcessMain.m_objIO.HLGetAnalog(ioIndex, ref readValue);
                var calibrationParameter = m_objConfig.GetAnalogCalibrationParameterAirPressure((int)ioIndex, CConfig.AnalogCalibrationParameter.EType.Type3);
                dReturn = calibrationParameter.Convert(readValue);

                // 낮은 값 필터링
                if (dReturn < 0d)
                {
                    dReturn = 0d;
                }

            } while (false);

            return dReturn;
        }

        /// <summary>
        /// 아날로그 값을 MPa로 바꾸는 공식
        /// </summary>
        /// <param name="dVoltage">아날로그 값</param>
        /// <returns></returns>
        public double GetVoltageToMPa(CDeviceIODefine.EAnalogInput ioIndex)
        {
            double dReturn = 0.0;

            do
            {
                double readValue = 0;
                m_objProcessMain.m_objIO.HLGetAnalog(ioIndex, ref readValue);
                var calibrationParameter = m_objConfig.GetAnalogCalibrationParameterAirPressure((int)ioIndex, CConfig.AnalogCalibrationParameter.EType.Type2);
                dReturn = calibrationParameter.Convert(readValue);

                // 낮은 값 필터링
                if (dReturn < 0d)
                {
                    dReturn = 0d;
                }

            } while (false);

            return dReturn;
        }

        /// <summary>
        /// 아날로그 값을 KPa로 바꾸는 공식
        /// </summary>
        /// <param name="dVoltage">아날로그 값</param>
        /// <returns></returns>
        public double GetVoltageTokPa(CDeviceIODefine.EAnalogInput ioIndex)
        {
            double dReturn = 0.0;

            do
            {
                double readValue = 0;
                m_objProcessMain.m_objIO.HLGetAnalog(ioIndex, ref readValue);
                var calibrationParameter = m_objConfig.GetAnalogCalibrationParameterAirPressure((int)ioIndex, CConfig.AnalogCalibrationParameter.EType.Type1);
                dReturn = calibrationParameter.Convert(readValue);
            } while (false);

            return dReturn;
        }

        /// <summary>
        /// 아날로그 값을 KPa로 바꾸는 공식
        /// </summary>
        /// <param name="dVoltage">아날로그 값</param>
        /// <returns></returns>
        public double GetVoltageTokPaVacuum(CDeviceIODefine.EAnalogInput ioIndex)
        {
            double dReturn = 0.0;

            do
            {
                double readValue = 0;
                m_objProcessMain.m_objIO.HLGetAnalog(ioIndex, ref readValue);
                var calibrationParameter = m_objConfig.GetAnalogCalibrationParameterAirPressure((int)ioIndex, CConfig.AnalogCalibrationParameter.EType.Type1);
                dReturn = calibrationParameter.Convert(readValue);

                // 낮은 값 필터링
                if (dReturn > -1d)
                {
                    dReturn = 0d;
                }
            } while (false);

            return dReturn;
        }

        public bool SetIOBit(CDeviceIODefine.EDigitalOutput eIO, bool bResult)
        {
            return m_objProcessMain.m_objIO.HLSetDigitalBit(eIO, bResult);
        }

        public bool SetIOBit(CDeviceIODefine.EDigitalInput eIO, bool bResult)
        {
            return m_objProcessMain.m_objIO.HLSetDigitalBit(eIO, bResult);
        }

        public bool GetIOBit(CDeviceIODefine.EDigitalOutput eIO)
        {
            bool bReturn = false;
            m_objProcessMain.m_objIO.HLGetDigitalBit(eIO, ref bReturn);
            return bReturn;
        }

        public bool GetIOBit(CDeviceIODefine.EDigitalInput eIO)
        {
            bool bReturn = false;
            m_objProcessMain.m_objIO.HLGetDigitalBit(eIO, ref bReturn);
            return bReturn;
        }

        public void RunCellLogToExcel(string strPath)
        {
            Process SelfProcess = new Process();
            SelfProcess.StartInfo.FileName = strPath;
            SelfProcess.StartInfo.WorkingDirectory = strPath;
            SelfProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            SelfProcess.Start();
        }

        public void AddMccLogData(Mcc.MccLogData logData)
        {
            if (MccLogManager.IsInitialized == true)
            {
                switch (m_eRunStatus)
                {
                    case CDefine.ERunStatus.Start:
                    case CDefine.ERunStatus.Stopping:
                    case CDefine.ERunStatus.LoadingStop:
                        MccLogManager.LogDataQueue.Enqueue(logData);
                        break;
                    default:
                        if (
                            logData.WriteLogMessage.Contains("_INIT_") == true
                            || logData.WriteLogMessage.Contains("_CHANGE_") == true
                            || logData.WriteLogMessage.Contains("_JOB_CHANGE") == true
                            || logData.WriteLogMessage.Contains("_ALARM_STOP") == true
                            || logData.WriteLogMessage.Contains("_ALARM") == true
                            )
                        {
                            MccLogManager.LogDataQueue.Enqueue(logData);
                        }
                        break;
                }
            }
        }

        public string GetUserMessage(CAlarmDefine.EMessageList messageFormatIndex, params object[] args)
        {
            int languageIndex;
            switch (m_objConfig.GetOptionParameter().eLanguage)
            {
                case CDefine.ELanguage.LANGUAGE_CHINA:
                    languageIndex = (int)CDatabaseDefine.EInformationUserMessage.TEXT_CHINA;
                    break;
                case CDefine.ELanguage.LANGUAGE_ENGLISH:
                    languageIndex = (int)CDatabaseDefine.EInformationUserMessage.TEXT_ENGLISH;
                    break;
                case CDefine.ELanguage.LANGUAGE_VIETNAM:
                    languageIndex = (int)CDatabaseDefine.EInformationUserMessage.TEXT_VIETNAM;
                    break;

                case CDefine.ELanguage.LANGUAGE_KOREA:
                default:
                    languageIndex = (int)CDatabaseDefine.EInformationUserMessage.TEXT_KOREA;
                    break;
            }

            try
            {
                CManagerTable database = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationUserMessage;
                var dataTable = database.HLGetDataTable();
                var dataRow = dataTable.Select(string.Format("{0} = '{1}'", database.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationUserMessage.ID], (int)messageFormatIndex));
                string format = dataRow.First().ItemArray[languageIndex].ToString().Replace("\\n", Environment.NewLine);
                return string.Format(format, Resource.GetAll(args));
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
                return "❌ - User message load failed.";
            }
        }

        /// <summary>
        /// 설비 최초 중알람 발생시 설비 상태를 덤프함
        /// </summary>
        /// <param name="dumpReason">덤프 발생 이유를 받아서 덤프 로그에 남긴다</param>
        public void EquipmentDump(string dumpReason)
        {
            bool bDumpFinished = false;
            Stopwatch sw = new Stopwatch();
            sw.Restart();

            StringBuilder sbDumpLog = new StringBuilder();
            string dumpLogMessage = string.Format("Equipment Dump Process Start. {0}", dumpReason);
            sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);

            string savePath = string.Format("{0}\\{1:yyyy-MM-dd}\\EQUIPMENT_DUMP\\{1:HH_mm_ss_fff} {2}", m_objConfig.GetLogPath(), DateTime.Now, dumpReason);
            try
            {
                if (false == Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                // 현재 스크린샷 저장
                {
                    // 주화면의 크기 정보 읽기
                    Rectangle rect = Screen.PrimaryScreen.Bounds;
                    // 픽셀 포맷 정보 얻기 (Optional)
                    int bitsPerPixel = Screen.PrimaryScreen.BitsPerPixel;
                    PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
                    if (bitsPerPixel <= 16)
                    {
                        pixelFormat = PixelFormat.Format16bppRgb565;
                    }
                    if (bitsPerPixel == 24)
                    {
                        pixelFormat = PixelFormat.Format24bppRgb;
                    }
                    // 화면 크기만큼의 Bitmap 생성
                    using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, pixelFormat))
                    {
                        // Bitmap 이미지 변경을 위해 Graphics 객체 생성
                        using (Graphics gr = Graphics.FromImage(bmp))
                        {
                            // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                            gr.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
                        }
                        // Bitmap 데이타를 파일로 저장
                        string outputFileName = string.Format("{0}\\Dump_SCREENSHOT.png", savePath, DateTime.Now);
                        bmp.Save(outputFileName);
                        dumpLogMessage = string.Format("Save Screenshot. {0}", outputFileName);
                        sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                    }
                }
                // DIO 
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("LABLE,NAME,SIGNAL");
                var ioParameter = m_objConfig.GetIOParameter().objIOParameter;
                foreach (var param in ioParameter)
                {
                    if (
                        param.Value.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_DI
                        || param.Value.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_DO
                        )
                    {
                        bool bSignal = false;
                        m_objProcessMain.m_objIO.HLGetDigitalBit(param.Value.strIOName, ref bSignal);
                        sb.AppendLine(string.Format("{0},{1},{2}", param.Value.strIndex, param.Value.strIOName, bSignal));
                    }
                }
                string fileName = string.Format("{0}\\Dump_DIO.csv", savePath);
                File.WriteAllText(fileName, sb.ToString());
                dumpLogMessage = string.Format("Save DIO Dump File. {0}", fileName);
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                // AIO 
                sb.Clear();
                sb.AppendLine("LABLE,NAME,VALUE");
                foreach (var param in ioParameter)
                {
                    if (param.Value.eIOType == CConfig.CIOInitializeParameter.EIOType.IO_TYPE_AI)
                    {
                        double analogValue = 0.0;
                        m_objProcessMain.m_objIO.HLGetAnalog(param.Value.strIOName, ref analogValue);
                        sb.AppendLine(string.Format("{0},{1},{2}", param.Value.strIndex, param.Value.strIOName, analogValue));
                    }
                }
                fileName = string.Format("{0}\\Dump_AIO.csv", savePath);
                File.WriteAllText(fileName, sb.ToString());
                dumpLogMessage = string.Format("Save AIO Dump File. {0}", fileName);
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                // CCLINK IE(INTERFACE)
                sb.Clear();
                sb.AppendLine("GROUP,CATEGORY,TYPE,LABLE,NAME,VALUE");
                foreach (var item in global::EqToEq.EqToEqUiDataManager.Groups.Values)
                {
                    foreach (var leftBit in item.LeftBits)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", item.GroupName, item.LeftName, "BIT", leftBit.Label, leftBit.Name, leftBit.Value));
                    }
                    foreach (var leftData in item.LeftDatas)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", item.GroupName, item.LeftName, "WORD", leftData.Label, leftData.Name, leftData.Value));
                    }
                    foreach (var rightBit in item.RightBits)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", item.GroupName, item.RightName, "BIT", rightBit.Label, rightBit.Name, rightBit.Value));
                    }
                    foreach (var rightData in item.RightDatas)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", item.GroupName, item.RightName, "WORD", rightData.Label, rightData.Name, rightData.Value));
                    }
                }
                fileName = string.Format("{0}\\Dump_CCLINK_IE(INTERFACE).csv", savePath);
                File.WriteAllText(fileName, sb.ToString());
                dumpLogMessage = string.Format("Save CCLINK IE(INTERFACE) Dump File. {0}", fileName);
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                // CCLINK VER2(ROBOT) Digital 
                sb.Clear();
                sb.AppendLine("LABLE,NAME,SIGNAL");
                var cclinkParameterDigital = m_objConfig.GetCCLinkVer2Parameter().objInterfaceParameterDigital;
                foreach (var param in cclinkParameterDigital)
                {
                    bool bSignal;
                    m_objProcessMain.m_objCCLinkVer2.HLGetInterfaceBit(param.Value.strDataName, out bSignal);
                    sb.AppendLine(string.Format("{0},{1},{2}", param.Value.strIndex, param.Value.strDataName, bSignal));
                }
                fileName = string.Format("{0}\\Dump_CCLINK_VER2 DIGITAL.csv", savePath);
                File.WriteAllText(fileName, sb.ToString());
                dumpLogMessage = string.Format("Save CCLINK VER2(ROBOT)_DIGITAL Dump File. {0}", fileName);
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                // CCLINK VER2(ROBOT) Analog 
                sb.Clear();
                sb.AppendLine("LABLE,NAME,SIGNAL");
                var cclinkParameterAnalog = m_objConfig.GetCCLinkVer2Parameter().objInterfaceParameterAnalog;
                int analogIndex = 0;
                foreach (var param in cclinkParameterAnalog)
                {
                    bool bSignal;
                    m_objProcessMain.m_objCCLinkVer2.HLGetInterfaceValue(param.Value.strDataName, out bSignal, analogIndex % 16);
                    sb.AppendLine(string.Format("{0},{1},{2}", param.Value.strIndex, param.Value.strDataName, bSignal));
                    analogIndex++;
                }
                fileName = string.Format("{0}\\Dump_CCLINK_VER2 ANALOG.csv", savePath);
                File.WriteAllText(fileName, sb.ToString());
                dumpLogMessage = string.Format("Save CCLINK VER2(ROBOT)_ANALOG Dump File. {0}", fileName);
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                // 알람
                {
                    var alarmLogItems = from setAlarm in m_objAlarmList.SetAlarms
                                        join alarmData in m_objAlarmDataTable.Select() on (int)setAlarm.Value equals Convert.ToInt32(alarmData[(int)CDatabaseDefine.EInformationAlarm.ID])
                                        select new { ID = (int)setAlarm.Value, AlarmText = Convert.ToString(alarmData[(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_ENGLISH]), AlarmLevel = Convert.ToString(alarmData[(int)CDatabaseDefine.EInformationAlarm.LEVEL]) };
                    sb.Clear();
                    sb.AppendLine("ID,ALTX,LEVEL");
                    foreach (var alarmData in alarmLogItems)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2}",
                            alarmData.ID,
                            alarmData.AlarmText,
                            alarmData.AlarmLevel
                            ));
                    }
                    fileName = string.Format("{0}\\Dump_ALARM.csv", savePath);
                    File.WriteAllText(fileName, sb.ToString());
                    dumpLogMessage = string.Format("Save ALARM Dump File. {0}", fileName);
                    sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                }
                // MOTOR 
                sb.Clear();
                foreach (CProcessMotion.EMotor motor in Enum.GetValues(typeof(CProcessMotion.EMotor)))
                {
                    var motorStatus = m_objProcessMain.m_objProcessMotion.m_objMotor[motor].HLGetMotorStatus();
                    sb.AppendLine($"<!-- {motor} -->");
                    sb.AppendLine(Newtonsoft.Json.JsonConvert.SerializeObject(motorStatus, Newtonsoft.Json.Formatting.Indented));
                }
                fileName = string.Format("{0}\\Dump_MOTOR.json", savePath, DateTime.Now);
                dumpLogMessage = string.Format("Save MOTOR Dump File. {0}", fileName);
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                File.WriteAllText(fileName, sb.ToString());
                // CELL 
                sb.Clear();
                foreach (var item in CellDataManager.Cells)
                {
                    if (item.Value.IsCellExist() == false)
                    {
                        continue;
                    }
                    sb.AppendLine($"<!-- {item.Key} -->");
                    sb.AppendLine(Newtonsoft.Json.JsonConvert.SerializeObject(item.Value.Data, Newtonsoft.Json.Formatting.Indented));
                }
                fileName = string.Format("{0}\\Dump_CELL.json", savePath);
                File.WriteAllText(fileName, sb.ToString());
                dumpLogMessage = string.Format("Save CELL Dump File. {0}", fileName);
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                // PROCESS
                sb.Clear();
                fileName = string.Format("{0}\\Dump_PROCESS.csv", savePath);
                sb.AppendLine($"ProcessName,Index,Command,Status");
                foreach (var process in m_objProcessMain.m_objProcessMotion.AllMotions)
                {
                    sb.AppendLine(process.ToString());
                }
                File.WriteAllText(fileName, sb.ToString());
                dumpLogMessage = string.Format("Save PROCESS Dump File. {0}", fileName);
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                // 현재 설정 파일 복사
                {
                    string settingFilePath = savePath + "\\Dump_CONFIG";
                    if (false == Directory.Exists(settingFilePath))
                    {
                        Directory.CreateDirectory(settingFilePath);
                    }
                    string sourceModelFolderPath = m_objConfig.GetModelPath() + "\\" + m_objConfig.GetSystemParameter().strPPID;
                    string destModelFolderPath = settingFilePath + "\\" + m_objConfig.GetSystemParameter().strPPID;
                    if (false == Directory.Exists(destModelFolderPath))
                    {
                        Directory.CreateDirectory(destModelFolderPath);
                    }
                    if (true == Directory.Exists(sourceModelFolderPath))
                    {
                        // 모델 폴더 복사
                        dumpLogMessage = string.Format("Save MODEL_FOLDER Copy.");
                        sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                        string[] files = Directory.GetFiles(sourceModelFolderPath);
                        // Copy the files and overwrite destination files if they already exist.
                        foreach (string s in files)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string copyFileName = Path.GetFileName(s);
                            string destFile = Path.Combine(destModelFolderPath, copyFileName);
                            File.Copy(s, destFile, true);
                        }
                        // 베큠, 실린더, 모터 설정 폴더 복사
                        dumpLogMessage = string.Format("Save SETTING_FOLDER Copy.");
                        sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                        files = Directory.GetFiles(m_objConfig.GetModelPath());
                        // Copy the files and overwrite destination files if they already exist.
                        foreach (string s in files)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string copyFileName = Path.GetFileName(s);
                            string destFile = Path.Combine(settingFilePath, copyFileName);
                            File.Copy(s, destFile, true);
                        }
                    }
                    string sourceIniFolderPath = m_objConfig.GetIniPath();
                    string destIniFolderPath = settingFilePath + "\\INI";
                    if (false == Directory.Exists(destIniFolderPath))
                    {
                        Directory.CreateDirectory(destIniFolderPath);
                    }
                    if (true == Directory.Exists(sourceIniFolderPath))
                    {
                        // INI 폴더 복사
                        dumpLogMessage = string.Format("Save INI_FOLDER Copy.");
                        sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                        string[] files = Directory.GetFiles(sourceIniFolderPath);
                        // Copy the files and overwrite destination files if they already exist.
                        foreach (string s in files)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string copyFileName = Path.GetFileName(s);
                            string destFile = Path.Combine(destIniFolderPath, copyFileName);
                            File.Copy(s, destFile, true);
                        }
                    }
                    string sourceConfigIniFile = string.Format("{0}\\{1}", m_objConfig.GetCurrentPath(), CDefine.DEF_CONFIG_INI);
                    string destConfigIniFile = string.Format("{0}\\{1}", settingFilePath, CDefine.DEF_CONFIG_INI);
                    File.Copy(sourceConfigIniFile, destConfigIniFile, true);
                    dumpLogMessage = string.Format("Save CONFIG_INI Copy.");
                    sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                    string sourceDeviceIniFile = string.Format("{0}\\{1}", m_objConfig.GetCurrentPath(), CDefine.DEF_DEVICE_INI);
                    string destDeviceIniFile = string.Format("{0}\\{1}", settingFilePath, CDefine.DEF_DEVICE_INI);
                    File.Copy(sourceDeviceIniFile, destDeviceIniFile, true);
                    dumpLogMessage = string.Format("Save DEVICE_INI Copy.");
                    sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                    string sourceDllVersionIniFile = string.Format("{0}\\{1}", m_objConfig.GetCurrentPath(), CDefine.DEF_DLL_VERSION_INI);
                    string destDllVersionIniFile = string.Format("{0}\\{1}", settingFilePath, CDefine.DEF_DLL_VERSION_INI);
                    File.Copy(sourceDllVersionIniFile, destDllVersionIniFile, true);
                    dumpLogMessage = string.Format("Save DLL_VERSION_INI Copy.");
                    sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                    string sourceInformationDatabaseFile = string.Format(@"{0}\{1}.db3", m_objConfig.GetCurrentPath(), m_objConfig.GetDatabaseParameter().strDatabaseInformation);
                    string destInformationDatabaseFile = string.Format(@"{0}\{1}.db3", settingFilePath, m_objConfig.GetDatabaseParameter().strDatabaseInformation);
                    File.Copy(sourceInformationDatabaseFile, destInformationDatabaseFile, true);
                    dumpLogMessage = string.Format("Save INFORMATION_DATABASE Copy.");
                    sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
                    fileName = string.Format("{0}\\Dump_CONFIG.zip", savePath);
                }

                dumpLogMessage = string.Format("Equipment Dump Process Finished. Process Time : {0}", sw.Elapsed.ToString());
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);

                bDumpFinished = true;
            }
            catch (Exception ex)
            {
                dumpLogMessage = string.Format("Equipment Dump Process Abnormal Finished. Process Time : {0}, Exception : {1}", sw.Elapsed.ToString(), ex.ToString());
                sbDumpLog.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]\t{1}" + Environment.NewLine, DateTime.Now, dumpLogMessage);
            }
            finally
            {
                string fileName = string.Format("{0}\\dump_log.txt", savePath);
                File.WriteAllText(fileName, sbDumpLog.ToString());

                if (bDumpFinished == true)
                {
                    Utils.CompressZipByIO(savePath, savePath + ".zip");
                    var deleteDirectoryInformation = new DirectoryInfo(savePath);
                    deleteDirectoryInformation.Delete(true);
                }
            }
        }

#if USE_HIGH_RESOLUTION_TIMER
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
        private static extern uint TimeBeginPeriod(uint uMilliseconds);

        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
        private static extern uint TimeEndPeriod(uint uMilliseconds);
#endif
    }

    public static class LogWrite
    {
        private static CDocument mDocument;

        public static void Initialize(CDocument document)
        {
            mDocument = document;
        }

        /// <summary>
        /// 예외 발생 로그를 ETC 로그에 남김
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="comment"></param>
        public static void Exception(Exception ex, string comment = "")
        {
            if (null != mDocument)
            {
                StringBuilder sb = new StringBuilder();
                StackTrace trace = new StackTrace(ex, true);
                sb.Append(comment).AppendLine();
                int count = 0;
                foreach (StackFrame sf in trace.GetFrames())
                {
                    count++;
                    sb.AppendFormat(
                        "[Depth:{0}, Line:{1}, Method:{2}, File:{3}]",
                        count,
                        sf.GetFileLineNumber(),
                        sf.GetMethod().Name,
                        Path.GetFileName(sf.GetFileName())
                        ).AppendLine();
                }
                sb.AppendFormat("[Message:{0}]", ex.Message);

                mDocument.SetUpdateLog(CDefine.ELogType.LOG_ETC, sb.ToString());
            }
        }

        public static void Dump(string dumpReason)
        {
            if (mDocument == null)
            {
                return;
            }
            if (mDocument.IsInitialized == false)
            {
                return;
            }
            mDocument.EquipmentDump(dumpReason);
        }
    }
}

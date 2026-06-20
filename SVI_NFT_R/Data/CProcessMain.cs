using Device;
using EqToEq;
using HLDevice;
using Mcc;
using SVI_NFT_R.EHS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using static HLDevice.Abstract.CDeviceFFUAbstract;

namespace SVI_NFT_R
{
    public partial class CProcessMain : CProcessAbstract
    {
        /// <summary>
        /// 설비 전원 상태 및 OPERATION PANEL 버튼 스레드
        /// </summary>
        private Thread m_ThreadEquipmentPower;

        /// <summary>
        /// 설비 상태 변경 스레드 (설비 상태를 체크해서 설비 상태를 변경)
        /// </summary>
        private Thread m_ThreadEquipmentStatus;

        /// <summary>
        /// EQ Interface 스레드 (Heart Beat 갱신 및 체크, 설비 상태에 따른 신호 갱신)
        /// </summary>
        private Thread m_ThreadEQInterface;

        /// <summary>
        /// 설비 환경 안전 상태 체크 스레드 (설비 환경안전 관련 상태를 실시간으로 감시하여 알람 발생)
        /// </summary>
        private Thread m_ThreadEqpSafetyCheck;

        /// <summary>
        /// 설비 상태에 따른 버튼 변경 딜리게이트 호출 스레드 (설비 런/스탑 상태 변경시 권한을 업데이트함)
        /// </summary>
        private Thread m_ThreadEquipmentStatusButtonStatus;

        /// <summary>
        /// 무언 정지 감시 스레드
        /// </summary>
        private Thread mThreadSilentStopWatchdog;

        /// <summary>
        /// 장비 조작 감시 스레드
        /// </summary>
        private Thread mThreadOperationWatchdog;

        /// <summary>
        /// 자재 관리 스레드
        /// </summary>
        private Thread mThreadMaterialsManagement;

        /// <summary>
        /// 저전력 모드 관리 스레드
        /// </summary>
        private Thread mThreadLowEnergyMode;

        /// <summary>
        /// 설비 외부 버튼 로그 스레드 (OPERATION PANEL 등 기타 외부 입력에 대한 로그)
        /// </summary>
        private readonly Utils.DigitalSignalObserver mThreadEquipmentButtonLog = new Utils.DigitalSignalObserver();

        /// <summary>
        /// 설비 세이프티 키 상태 체크
        /// </summary>
        private CSafetyIO m_objSafetyIO;

        /// <summary>
        /// 프로세스 모션
        /// </summary>
        public CProcessMotion m_objProcessMotion;

        /// <summary>
        /// 프로세스 타워램프
        /// </summary>
        public CProcessTowerLamp m_objProcessTowerLamp;

        /// <summary>
        /// 프로세스 세이프티 도어
        /// </summary>
        public CProcessSafetyDoor m_objProcessSafetyDoor;

        /// <summary>
        /// 프로세스 로그인
        /// </summary>
        public CProcessLogin m_objProcessLogin;

        /// <summary>
        /// 프로세스 FTP 전송
        /// </summary>
        public MccLogTransferService m_objProcessMccLogTransfer;

        private DateTime mFanOkDateTime;
        public bool IsFanFail { get; private set; }
        public bool IsJogMoving { get; set; }
        private int[] mDoorForcedReleasedAlarmDelay = new int[Enum.GetNames(typeof(Door.EDoor)).Length];
        // 온도 끊김 감지 알람 딜레이
        private int mSafetyThreadSleepTime = 10;
        public bool IsSamsungLogoClick = false;
        private CDeviceInterface mVirtualCcLinkVer2 = null;
        private long mSafetyControllerResetButtonLongPressEndTick = TimeSpan.Zero.Ticks;
        private readonly long mSafetyControllerResetButtonLongPressWaitTick = TimeSpan.FromSeconds(1.5d).Ticks;
        private MonitorIO.WordsSet mMonitorIoSet;
        private long mMonitorIoUpdateTick = 0;
        private readonly long mMonitorIoUpdateIntervalTick = TimeSpan.FromSeconds(1).Ticks;

        public CDeviceInterface GetVirtualCcLinkVer2()
        {
            if (mVirtualCcLinkVer2 == null)
            {
                if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON
                    || m_objDocument.m_objConfig.GetCCLinkVer2Parameter().bRunSimulationMode == true
                    )
                {
                    return m_objCCLinkVer2;
                }

                //Virtual CCLink Ver.2 초기화(로봇 인터페이스)
                mVirtualCcLinkVer2 = new CDeviceInterface();
                var config = m_objDocument.m_objConfig.GetCCLinkVer2Parameter().DeepClone();
                config.bRunSimulationMode = true;
                if (false == mVirtualCcLinkVer2.HLInitialize(m_objDocument, config))
                {
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, $"Virtual CC-Link Ver.2 Initialize Fail");
                    return null;
                }
            }
            return mVirtualCcLinkVer2;
        }

        /// <summary>
        /// Safety IO 객체를 받아온다.
        /// </summary>
        /// <returns>Safety IO 객체</returns>
        public CSafetyIO GetSafetyIO()
        {
            return m_objSafetyIO;
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="objDocument"></param>
        /// <param name="ePosition"></param>
        /// <returns></returns>
        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;
            string logMessage = string.Empty;

            do
            {
                // 알람 구조체
                m_objAlarmStructure.strAlarmObject = typeof(CProcessMain).Name;
                m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
                // Document 객체 연결
                m_objDocument = document;
                // ThreadPool 초기 갯수 설정.
                ThreadPool.SetMinThreads(100, 100);
                //ThreadPool.SetMaxThreads(300, 300);

                // IO 객체 초기화
                m_objIO = new CDeviceIO();
                if (false == m_objIO.HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetIOParameter()))
                {
                    logMessage = $"IO Initialize Fail";
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                    Debug.Assert(false, logMessage);
                }

                // 통합 IO 하드웨어 초기화
                IoHardware = new IoHardware();
                if (false == IoHardware.Initialize(m_objDocument))
                {
                    logMessage = $"IoHardware Initialize Fail";
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                    Debug.Assert(false, logMessage);
                }

                //CCLink Ver.2 초기화(로봇 인터페이스)
                m_objCCLinkVer2 = new CDeviceInterface();
                if (false == m_objCCLinkVer2.HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetCCLinkVer2Parameter()))
                {
                    logMessage = $"CC-Link Ver.2 Initialize Fail";
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                    Debug.Assert(false, logMessage);
                }

                // FFU 초기화
                m_objFFU = new Dictionary<CDefine.EMcu, CDeviceFFU>();
                foreach (CDefine.EMcu index in Enum.GetValues(typeof(CDefine.EMcu)))
                {
                    m_objFFU[index] = new CDeviceFFU();
                    if (false == m_objFFU[index].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetFfuParameter(index)))
                    {
                        logMessage = $"FFU[{index}] Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // 저항측정기 초기화 ( GMS )
                m_objElectrostatic = new Dictionary<CDefine.EGms, CDeviceElectrostatic>();
                foreach (CDefine.EGms index in Enum.GetValues(typeof(CDefine.EGms)))
                {
                    m_objElectrostatic[index] = new CDeviceElectrostatic();
                    if (false == m_objElectrostatic[index].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetElectrostaticParameter(index)))
                    {
                        logMessage = $"GMS[{index}] Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                //온도 컨트롤러 초기화
                int temperatureCount = Enum.GetNames(typeof(CDefine.ETemperatureController)).Length;
                m_objTemperature = new Dictionary<CDefine.ETemperatureController, CDeviceTemperature>();
                foreach (CDefine.ETemperatureController index in Enum.GetValues(typeof(CDefine.ETemperatureController)))
                {
                    m_objTemperature[index] = new CDeviceTemperature();
                    if (m_objTemperature[index].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetTemperatureParameter(index)) == false)
                    {
                        logMessage = $"Temperature[{index}] Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // 적산량계 초기화
                m_objPowerMeter = new Dictionary<CDefine.EPowerMeter, CDevicePowerMeter>();
                foreach (CDefine.EPowerMeter index in Enum.GetValues(typeof(CDefine.EPowerMeter)))
                {
                    m_objPowerMeter[index] = new CDevicePowerMeter();
                    if (false == m_objPowerMeter[index].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetPowerMeterParameter(index)))
                    {
                        logMessage = $"PowerMeter[{index}] Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // MCR 초기화
                m_objMcr = new Dictionary<CDefine.EMcr, CDeviceMCR>();
                foreach (CDefine.EMcr index in Enum.GetValues(typeof(CDefine.EMcr)))
                {
                    m_objMcr[index] = new CDeviceMCR();
                    if (false == m_objMcr[index].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetMcrParameter(index)))
                    {
                        logMessage = $"MCR[{index}] Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // Angle Sensor Interface 생성
                m_objAngleSensor = new Dictionary<CDefine.EAngularSensor, CDeviceAngleSensor>();
                foreach (CDefine.EAngularSensor index in Enum.GetValues(typeof(CDefine.EAngularSensor)))
                {
                    m_objAngleSensor[index] = new CDeviceAngleSensor();
                    if (false == m_objAngleSensor[index].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetAngleSensorParameter(index)))
                    {
                        logMessage = $"AngularSensor[{index}] Interface Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // Measurement Interface 생성
                m_objMeasurementInterfaces = new Dictionary<CDefine.EMeasurement, Device.Measurement.IDeviceMeasurement>();
                foreach (CDefine.EMeasurement index in Enum.GetValues(typeof(CDefine.EMeasurement)))
                {
                    Device.Measurement.EMeasurementDevice deviceType = 0;
                    if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
                    {
                        deviceType = Device.Measurement.EMeasurementDevice.Virtual;
                    }
                    else
                    {
                        switch (index)
                        {
                            default:
                                Debug.Assert(false);
                                break;
                        }
                    }
                    m_objMeasurementInterfaces[index] = Device.Measurement.Factory.Create(deviceType);
                    if (m_objMeasurementInterfaces[index].Initialize(m_objDocument.m_objConfig.GetMeasurementInitializeParameter(index)) == false)
                    {
                        logMessage = $"Measurement[{index}] Interface Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // Safety PLC Interface 생성
                m_objSafetyPlcs = new Dictionary<CDefine.ESafetyPlc, CDeviceSafetyPlc>();
                foreach (CDefine.ESafetyPlc index in Enum.GetValues(typeof(CDefine.ESafetyPlc)))
                {
                    m_objSafetyPlcs[index] = new CDeviceSafetyPlc();
                    if (false == m_objSafetyPlcs[index].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetSafetyPlcParameter(index)))
                    {
                        logMessage = $"SafetyPLC[{index}] Interface Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // FTP 초기화
                m_objFileInterface = new CDeviceFileInterface();
                if (false == m_objFileInterface.HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetMccInitializeParameter()))
                {
                    logMessage = $"FTP Interface Initialize Fail";
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                    Debug.Assert(false, logMessage);
                }

                // 검사기 Interface 생성
                m_objInspInterfaces = new Dictionary<CDefine.EInspInterface, CDeviceSviInterface>();
                foreach (CDefine.EInspInterface index in Enum.GetValues(typeof(CDefine.EInspInterface)))
                {
                    m_objInspInterfaces[index] = new CDeviceSviInterface();
                    CDefine.ELogType log = CDefine.ELogType.LOG_VISION_AMO_INSPECTION_PC1 + (int)index;
                    if (false == m_objInspInterfaces[index].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetInspInitializeParameter(index), log))
                    {
                        logMessage = $"Svi Inspect[{index}] Interface Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // Align Interface 생성
                m_objAlignInterfaces = new Dictionary<CDefine.EAlign, CDeviceAlignInterface>();
                foreach (CDefine.EAlign alignIndex in Enum.GetValues(typeof(CDefine.EAlign)))
                {
                    m_objAlignInterfaces[alignIndex] = new CDeviceAlignInterface();
                    CDefine.ELogType log = CDefine.ELogType.LOG_VISION_IN_PRE_ALIGN + (int)alignIndex;
                    if (false == m_objAlignInterfaces[alignIndex].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetAlignInitializeParameter(alignIndex), log))
                    {
                        logMessage = $"ALIGN[{alignIndex}] Interface Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                DoorManager.Initialize(m_objDocument);

                // Nachi Comm Interface 생성
                int nachiCommCount = Enum.GetNames(typeof(CDefine.ENachiComm)).Length;
                m_objNachiInterface = new CDeviceNachiInterface[nachiCommCount];
                for (int i = 0; i < nachiCommCount; i++)
                {
                    CDefine.ENachiComm commIndex = (CDefine.ENachiComm)i;
                    m_objNachiInterface[(int)commIndex] = new CDeviceNachiInterface();
                    if (false == m_objNachiInterface[(int)commIndex].HLInitialize(m_objDocument, m_objDocument.m_objConfig.GetNachiInitializeParameter(commIndex)))
                    {
                        logMessage = $"Nachi Interface {commIndex} Initialize Fail";
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                        Debug.Assert(false, logMessage);
                    }
                }

                // 모션 초기화
                m_objProcessMotion = new CProcessMotion();
                if (false == m_objProcessMotion.Initialize(m_objDocument))
                {
                    logMessage = $"ProcessMotion Initialize Fail";
                    m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_SYSTEM, logMessage);
                    Debug.Assert(false, logMessage);
                }

                // 세이프티 키 상태 객체
                m_objSafetyIO = new CSafetyIO(m_objDocument);
                m_objSafetyIO.Initialize();

                // 타워램프 초기화
                m_objProcessTowerLamp = new CProcessTowerLamp();
                m_objProcessTowerLamp.Initialize(m_objDocument);

                // 세이프티 도어 초기화
                m_objProcessSafetyDoor = new CProcessSafetyDoor();
                m_objProcessSafetyDoor.Initialize(m_objDocument);

                // 로그인 초기화
                m_objProcessLogin = new CProcessLogin();
                m_objProcessLogin.Initialize(m_objDocument);

                // FTP Transfer 초기화
                m_objProcessMccLogTransfer = new MccLogTransferService(
                    new MccConfigAdapter(m_objDocument),
                    new RemoteFileClientAdapter(m_objFileInterface),
                    new ConcurrentMccLogQueueAdapter(MccLogManager.LogDataQueue)
                    );
                m_objProcessMccLogTransfer.Start();

                // 전면부 전원 상태 쓰레드
                m_ThreadEquipmentPower = new Thread(ThreadEquipmentPowerProcess);
                m_ThreadEquipmentPower.Start();

                // 설비 상태 변경 쓰레드
                m_ThreadEquipmentStatus = new Thread(ThreadEquipmentStatusProcess);
                m_ThreadEquipmentStatus.Start();

                // 무언 정지 감시 스레드
                mThreadSilentStopWatchdog = new Thread(ThreadSilentStopWatchdog);
                mThreadSilentStopWatchdog.IsBackground = true;
                mThreadSilentStopWatchdog.Start();

                // 자재 관리 스레드
                mThreadMaterialsManagement = new Thread(ThreadMaterialsManagement);
                mThreadMaterialsManagement.Start();

                // 조작 감시 스레드
                mThreadOperationWatchdog = new Thread(ThreadOperationWatchdog);
                mThreadOperationWatchdog.IsBackground = true;
                mThreadOperationWatchdog.Start();

                // 인터페이스
                m_ThreadEQInterface = new Thread(ThreadEQInterfaceProcess);
                m_ThreadEQInterface.IsBackground = true;
                m_ThreadEQInterface.Start();

                // 설비 환경 안전 쓰레드
                m_ThreadEqpSafetyCheck = new Thread(ThreadEqpSafetyCheckProcess);
                m_ThreadEqpSafetyCheck.Start();

                // 설비 상태에 따른 버튼 변경 딜리게이트 호출 스레드
                m_ThreadEquipmentStatusButtonStatus = new Thread(ThreadEquipmentStatusButtonStatusProcess);
                m_ThreadEquipmentStatusButtonStatus.Start();

                // 저전력 모드 관리 스레드
                mThreadLowEnergyMode = new Thread(ThreadLowEnergyModeProcess);
                mThreadLowEnergyMode.IsBackground = true;
                mThreadLowEnergyMode.Start();

                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_BUTTON_OPERATION, "//////////////////////////////////////////////////////////////////////////");
                // 설비 외부 버튼 로그 스레드
                {
                    mThreadEquipmentButtonLog.SignalChanged += ThreadEquipmentButtonLog_SignalChanged;
                    var observerList = new List<Utils.DigitalSignalObserverOption>(128)
                    {
                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT FRONT_EMERGENCY_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_EMERGENCY_SWITCH_PUSH)),
                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT REAR_EMERGENCY_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_EMERGENCY_SWITCH_PUSH)),

                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT FRONT_MC_ON_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_OP_ON_BUTTON_PUSH)),
                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT REAR_MC_ON_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_OP_ON_BUTTON_PUSH)),
                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT FRONT_MC_OFF_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_OP_OFF_BUTTON_PUSH)),
                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT REAR_MC_OFF_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_OP_OFF_BUTTON_PUSH)),
                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT FRONT_RESET_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_OP_RESET_BUTTON_PUSH)),
                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT REAR_RESET_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_OP_RESET_BUTTON_PUSH)),
                        new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT ELECTRONIC_BOX_SMOKE_RESET_BUTTON", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_SMOKE_RESET_BUTTON_PUSH)),

                        new Utils.DigitalSignalObserverOption(new string[] { "ON", "OFF" }, string.Empty, $"EQUIPMENT MAGNET_CONTACT", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON)),

                        new Utils.DigitalSignalObserverOption(new string[] { "AUTO", "TEACH" }, string.Empty, $"EQUIPMENT SAFETY_STATUS_MODE", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH) == false),
                        new Utils.DigitalSignalObserverOption(new string[] { "EMS", "NORMAL" }, string.Empty, $"EQUIPMENT SAFETY_STATUS_EMS", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_EMS_NORMAL) == false),
                        new Utils.DigitalSignalObserverOption(new string[] { "DOOR-FORCE-OPEN", "NORMAL" }, string.Empty, $"EQUIPMENT SAFETY_STATUS_DOOR_FORCE_OPEN", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_DOOR_FORCE_OPEN_NORMAL) == false),

                        new Utils.DigitalSignalObserverOption(new string[] { "ON", "OFF" }, string.Empty, $"EQUIPMENT INSIDE_MAINT_LAMP", () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_EQUIPMENT_INSIDE_LAMP_ON)),
                    };
                    foreach (Door.EDoor index in Enum.GetValues(typeof(Door.EDoor)))
                    {
                        observerList.Add(new Utils.DigitalSignalObserverOption(new string[] { "OPEN", "CLOSE" }, string.Empty, $"EQUIPMENT DOOR[{index}]", () => DoorManager.Doors[index].IsOpened));
                    }
                    foreach (CProcessMotion.ERobot index in Enum.GetValues(typeof(CProcessMotion.ERobot)))
                    {
                        observerList.Add(new Utils.DigitalSignalObserverOption(new string[] { "AUTO", "TEACH" }, string.Empty, $"EQUIPMENT {index}_CONTROLLER_MODE", () => m_objProcessMotion.m_objRobot[index].Status.IsTeachModeEntry == false));
                        observerList.Add(new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT {index}_CONTROLLER_EMS", () => m_objProcessMotion.m_objRobot[index].Status.IsControllerEmsOn));
                        observerList.Add(new Utils.DigitalSignalObserverOption(new string[] { "PRESS", string.Empty }, string.Empty, $"EQUIPMENT {index}_PENDANT_EMS", () => m_objProcessMotion.m_objRobot[index].Status.IsPendantEmsOn));
                    }
                    mThreadEquipmentButtonLog.Initialize(observerList, pollingTime: 100);
                }

                // Alive On
                m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_LOWER_ALIVE, true);
                m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_UPPER_ALIVE, true);
                // Net-Ready On
                m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_LOWER_NET_READY, true);
                m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_UPPER_NET_READY, true);

                mFanOkDateTime = DateTime.Now;

                AddCommLogEvent();

                SerialReaders.Initialize(m_objDocument);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void DeInitialize()
        {
            SerialReaders.DeInitialize();

            RemoveCommLogEvent();

            // Alive Off
            m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_LOWER_ALIVE, false);
            m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_UPPER_ALIVE, false);
            // Net-Ready Off
            m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_LOWER_NET_READY, false);
            m_objIO.HLSetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_UPPER_NET_READY, false);

            mThreadEquipmentButtonLog.DeInitialize();
            mThreadEquipmentButtonLog.SignalChanged -= ThreadEquipmentButtonLog_SignalChanged;

            mbThreadExit = true;

            mThreadLowEnergyMode.Join();
            m_ThreadEquipmentPower.Join();
            m_ThreadEquipmentStatus.Join();
            m_ThreadEQInterface.Join();
            m_ThreadEqpSafetyCheck.Join();
            m_ThreadEquipmentStatusButtonStatus.Join();
            mThreadSilentStopWatchdog.Join();
            mThreadOperationWatchdog.Join();
            mThreadMaterialsManagement.Join();

            m_objProcessLogin.DeInitialize();
            m_objProcessSafetyDoor.DeInitialize();
            m_objProcessTowerLamp.DeInitialize();
            m_objProcessMotion.DeInitialize();
            DoorManager.DeInitialize();
            m_objIO.HLDeInitialize();
            m_objCCLinkVer2.HLDeInitialize();
            mVirtualCcLinkVer2?.HLDeInitialize();
            Array.ForEach(m_objMcr.Values.ToArray(), item => item.HLDeInitialize());
            Array.ForEach(m_objElectrostatic.Values.ToArray(), item => item.HLDeInitialize());
            Array.ForEach(m_objFFU.Values.ToArray(), item => item.HLDeInitialize());
            Array.ForEach(m_objTemperature.Values.ToArray(), item => item.HLDeInitialize());
            Array.ForEach(m_objPowerMeter.Values.ToArray(), item => item.HLDeInitialize());
            Array.ForEach(m_objNachiInterface, item => item.HLDeInitialize());
            Array.ForEach(m_objInspInterfaces.Values.ToArray(), item => item.HLDeInitialize());
            Array.ForEach(m_objSafetyPlcs.Values.ToArray(), item => item.HLDeInitialize());
            Array.ForEach(m_objMeasurementInterfaces.Values.ToArray(), item => item.DeInitialize());
            Array.ForEach(m_objAngleSensor.Values.ToArray(), item => item.HLDeInitialize());
            Array.ForEach(m_objAlignInterfaces.Values.ToArray(), item => item.HLDeInitialize());
            m_objProcessMccLogTransfer.Dispose();
            m_objFileInterface.HLDeInitialize();
            IoHardware.DeInitialize();
        }

        private void AddCommLogEvent()
        {
            // 온도 컨트롤러 통신 로그 이벤트 등록
            foreach (CDefine.ETemperatureController index in Enum.GetValues(typeof(CDefine.ETemperatureController)))
            {
                DeviceCommon.IHaveErrorLogEvent haveErrorLogEvent = m_objTemperature[index].GetErrorLogInstanceOrNull();
                if (haveErrorLogEvent == null)
                {
                    continue;
                }
                haveErrorLogEvent.Name = $"{CDefine.DEVICE_TEMPERATURE}+{index}";
                haveErrorLogEvent.OnErrorMessage += SerialComm_OnErrorMessage;
            }

            // FFU 통신 로그 이벤트 등록
            foreach (CDefine.EMcu index in Enum.GetValues(typeof(CDefine.EMcu)))
            {
                DeviceCommon.IHaveErrorLogEvent haveErrorLogEvent = m_objFFU[index].GetErrorLogInstanceOrNull();
                if (haveErrorLogEvent == null)
                {
                    continue;
                }
                haveErrorLogEvent.Name = $"{CDefine.DEVICE_MCU}+{index}";
                haveErrorLogEvent.OnErrorMessage += SerialComm_OnErrorMessage;
            }

            // GMS 통신 로그 이벤트 등록
            foreach (CDefine.EGms index in Enum.GetValues(typeof(CDefine.EGms)))
            {
                DeviceCommon.IHaveErrorLogEvent haveErrorLogEvent = m_objElectrostatic[index].GetErrorLogInstanceOrNull();
                if (haveErrorLogEvent == null)
                {
                    continue;
                }
                haveErrorLogEvent.Name = $"{CDefine.DEVICE_GMS}+{index}";
                haveErrorLogEvent.OnErrorMessage += SerialComm_OnErrorMessage;
            }

            // 세이프티PLC 통신 로그 이벤트 등록
            foreach (CDefine.ESafetyPlc index in Enum.GetValues(typeof(CDefine.ESafetyPlc)))
            {
                DeviceCommon.IHaveErrorLogEvent haveErrorLogEvent = m_objSafetyPlcs[index].GetErrorLogInstanceOrNull();
                if (haveErrorLogEvent == null)
                {
                    continue;
                }
                haveErrorLogEvent.Name = $"{CDefine.DEVICE_SAFETYPLC}+{index}";
                haveErrorLogEvent.OnErrorMessage += SerialComm_OnErrorMessage;
            }
        }

        private void RemoveCommLogEvent()
        {
            // 온도 컨트롤러 통신 로그 이벤트 등록 해제
            foreach (CDefine.ETemperatureController index in Enum.GetValues(typeof(CDefine.ETemperatureController)))
            {
                DeviceCommon.IHaveErrorLogEvent haveErrorLogEvent = m_objTemperature[index].GetErrorLogInstanceOrNull();
                if (haveErrorLogEvent == null)
                {
                    continue;
                }
                haveErrorLogEvent.OnErrorMessage -= SerialComm_OnErrorMessage;
            }

            // FFU 통신 로그 이벤트 등록 해제
            foreach (CDefine.EMcu index in Enum.GetValues(typeof(CDefine.EMcu)))
            {
                DeviceCommon.IHaveErrorLogEvent haveErrorLogEvent = m_objFFU[index].GetErrorLogInstanceOrNull();
                if (haveErrorLogEvent == null)
                {
                    continue;
                }
                haveErrorLogEvent.OnErrorMessage -= SerialComm_OnErrorMessage;
            }

            // GMS 통신 로그 이벤트 등록 해제
            foreach (CDefine.EGms index in Enum.GetValues(typeof(CDefine.EGms)))
            {
                DeviceCommon.IHaveErrorLogEvent haveErrorLogEvent = m_objElectrostatic[index].GetErrorLogInstanceOrNull();
                if (haveErrorLogEvent == null)
                {
                    continue;
                }
                haveErrorLogEvent.OnErrorMessage -= SerialComm_OnErrorMessage;
            }

            // 세이프티PLC 통신 로그 이벤트 등록 해제
            foreach (CDefine.ESafetyPlc index in Enum.GetValues(typeof(CDefine.ESafetyPlc)))
            {
                DeviceCommon.IHaveErrorLogEvent haveErrorLogEvent = m_objSafetyPlcs[index].GetErrorLogInstanceOrNull();
                if (haveErrorLogEvent == null)
                {
                    continue;
                }
                haveErrorLogEvent.OnErrorMessage -= SerialComm_OnErrorMessage;
            }
        }

        private void SerialComm_OnErrorMessage(object sender, string e)
        {
            var deviceInformation = sender as DeviceCommon.IDeviceInfomation;
            if (deviceInformation == null)
            {
                return;
            }

            CDefine.ELogType logTarget = 0;
            switch (deviceInformation.Category)
            {
                case DeviceCommon.EDevice.MCU:
                    logTarget = CDefine.ELogType.LOG_COMM_ERROR_MCU;
                    break;

                case DeviceCommon.EDevice.ResistanceMeter:
                    logTarget = CDefine.ELogType.LOG_COMM_ERROR_GMS;
                    break;

                case DeviceCommon.EDevice.TemperatureMeter:
                    logTarget = CDefine.ELogType.LOG_COMM_ERROR_TEMPERATURE;
                    break;

                case DeviceCommon.EDevice.Unknown:
                    if (deviceInformation.Model == "G9SP")
                    {
                        logTarget = CDefine.ELogType.LOG_COMM_ERROR_SAFETYPLC;
                    }
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
            m_objDocument.SetUpdateLog(logTarget, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}],{deviceInformation.Name},{e.Replace(Environment.NewLine, "@")}");
        }

        /// <summary>
        /// 전원 버튼 상태 표시
        /// </summary>
        private void DoProcessEquipmentPower()
        {
            const string KEY_X_FRONT_OP_OFF_BUTTON_PUSH = nameof(CDeviceIODefine.EDigitalInput.X_FRONT_OP_OFF_BUTTON_PUSH);
            const string KEY_X_REAR_OP_OFF_BUTTON_PUSH = nameof(CDeviceIODefine.EDigitalInput.X_REAR_OP_OFF_BUTTON_PUSH);
            const string KEY_X_MAGNET_CONTACT_ON = nameof(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON);

            bool bSafetyAutoMode = CheckSaftyKeyModeIsAuto();
            bool bResultFront = false;
            bool bResultRear = false;

            // 전면 또는 후면 Power ON 버튼 누름 감지 될 경우 Magnet 전원 ON
            m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_FRONT_OP_ON_BUTTON_PUSH, ref bResultFront);
            m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_REAR_OP_ON_BUTTON_PUSH, ref bResultRear);

            if (
                (
                true == bResultFront
                || true == bResultRear
                )
                &&
                false == NoiseFilterManager.Items[KEY_X_MAGNET_CONTACT_ON].Signal
                )
            {
                // 마그넷 전원 ON
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_MAGNET_CONTACT_ON, true);
                // 가상 모드 처리
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON, true);
                // 대기
                Stopwatch sw = Stopwatch.StartNew();
                TimeSpan timeout = TimeSpan.FromSeconds(1);
                while (timeout > sw.Elapsed)
                {
                    if (NoiseFilterManager.Items[KEY_X_MAGNET_CONTACT_ON].Signal == true)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }
            }

            // 전면 또는 후면 Power OFF 버튼 누름 감지 될 경우 Magnet 전원 OFF (A 접점)
            bResultFront = NoiseFilterManager.Items[KEY_X_FRONT_OP_OFF_BUTTON_PUSH].Signal;
            bResultRear = NoiseFilterManager.Items[KEY_X_REAR_OP_OFF_BUTTON_PUSH].Signal;

            if (
                (
                true == bResultFront
                || true == bResultRear
                )
                &&
                true == NoiseFilterManager.Items[KEY_X_MAGNET_CONTACT_ON].Signal
                )
            {
                // 마그넷 전원 OFF
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_MAGNET_CONTACT_ON, false);
                // 가상 모드 처리
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON, false);
                // 대기
                Stopwatch sw = Stopwatch.StartNew();
                TimeSpan timeout = TimeSpan.FromSeconds(1);
                while (timeout > sw.Elapsed)
                {
                    if (NoiseFilterManager.Items[KEY_X_MAGNET_CONTACT_ON].Signal == false)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }
            }

            if (
                m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_MAGNET_CONTACT_ON) == true
                && NoiseFilterManager.Items[KEY_X_MAGNET_CONTACT_ON].Signal == false
                )
            {
                // 마그넷 전원 OFF
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_MAGNET_CONTACT_ON, false);
                // 가상 모드 처리
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON, false);
                // 설비 정지
                m_objProcessMotion.EmergencyStopAll();
                // 런 모드 정지
                switch (m_objDocument.GetRunStatus())
                {
                    case CDefine.ERunStatus.Pause:
                    case CDefine.ERunStatus.Stopping:
                    case CDefine.ERunStatus.Stop:
                        // 정지 상태면 상태를 변경하지 않음
                        break;
                    case CDefine.ERunStatus.LoadingStop:
                    case CDefine.ERunStatus.Start:
                    case CDefine.ERunStatus.Initialize:
                    case CDefine.ERunStatus.Setup:
                        // 동작중이면 정지 진행중 상태로 변경
                        m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        break;
                }
                // 설비 초기화 상태 초기화
                //m_objProcessMotion.SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE);
            }

            // 램프 신호 ON/ OFF
            bool bMcOn = m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_MAGNET_CONTACT_ON);
            m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_FRONT_OP_ON_BUTTON_LAMP_ON, bMcOn == true);
            m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_REAR_OP_ON_BUTTON_LAMP_ON, bMcOn == true);
            m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_FRONT_OP_OFF_BUTTON_LAMP_ON, bMcOn == false);
            m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_REAR_OP_OFF_BUTTON_LAMP_ON, bMcOn == false);
        }

        private void DoProcessSafetyControllerResetProcess()
        {
            bool bAnyResetButtonPressed = m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_OP_RESET_BUTTON_PUSH) == true
                || m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_OP_RESET_BUTTON_PUSH) == true;

            if (bAnyResetButtonPressed == true)
            {
                if (mSafetyControllerResetButtonLongPressEndTick == TimeSpan.Zero.Ticks)
                {
                    mSafetyControllerResetButtonLongPressEndTick = Utils.GetTimestamp() + mSafetyControllerResetButtonLongPressWaitTick;
                }
                if (Utils.GetTimestamp() < mSafetyControllerResetButtonLongPressEndTick)
                {
                    return;
                }
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_SAFETY_CONTROLLER_POWER_OFF, CDefine.ON);
                return;
            }

            mSafetyControllerResetButtonLongPressEndTick = TimeSpan.Zero.Ticks;
            m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_SAFETY_CONTROLLER_POWER_OFF, CDefine.OFF);
        }

        /// <summary>
        /// 설비 아이들 상태 확인
        /// </summary>
        /// <returns>true = 설비 시퀀스 동작 완료 상태, false = 설비 시퀀스 동작 중 상태</returns>
        private bool DoProcessEquipmentStatus()
        {
            bool bReturn = false;
            do
            {
                if (false == m_objProcessMotion.CheckAllSequenceManagerStateIsIdle())
                {
                    break;
                }

                // 로딩 스탑일경우 원사이클을 마친 뒤 사이클 택타임 로그를 남기고 정지한다.
                if (m_objDocument.GetRunStatus() == CDefine.ERunStatus.LoadingStop)
                {
                    if (
                        m_objProcessMotion.InShuttle.IsCompleteWriteCycleLog == false
                        || m_objProcessMotion.InRobot.IsCompleteWriteCycleLog == false
                        || m_objProcessMotion.InspStage.IsCompleteWriteCycleLog == false
                        || m_objProcessMotion.OutRobot.IsCompleteWriteCycleLog == false
                        || m_objProcessMotion.OutFlip.IsCompleteWriteCycleLog == false
                        )
                    {
                        break;
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 설정된 시간 이상 프로세스 IDLE 상태 확인
        /// </summary>
        /// <param name="iTimeOut"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns>true = 유닛 IDLE, false = 유닛 RUN</returns>
        private bool CheckProcessIdleStatus(int iTimeOut = 1000, int iSleepPeriod = 10)
        {
            bool bReturn = false;
            do
            {
                while (0 < iTimeOut && (false != DoProcessEquipmentStatus()))
                {
                    Thread.Sleep(iSleepPeriod);
                    iTimeOut -= iSleepPeriod;
                }

                if (0 < iTimeOut)
                {
                    break;
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 설비 자재 정보 확인 ( 설비 내부의 자재 정보가 없을 경우 확인 )
        /// </summary>
        /// <returns></returns>
        private bool DoProcessDataEmpty()
        {
            bool bReturn = false;
            do
            {
                if (true == CellDataManager.Cells.Values.IsCellExistFromList())
                {
                    break;
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 도어 잠김 상태 확인
        /// </summary>
        /// <returns></returns>
        public bool CheckDoorLock()
        {
            // 도어 상태 확인
            if (m_objDocument.IsMasterLogin == false)
            {
                foreach (var door in DoorManager.Doors.Values)
                {
                    if (door.IsUnlockPermit == true)
                    {
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.DOOR_IS_NOT_LOCKED_PARAM1, door.DoorIndex);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 로봇 EMO 버튼 입력 상태 확인
        /// </summary>
        /// <returns></returns>
        public bool CheckRobotEMO()
        {
            foreach (var robot in m_objDocument.m_objProcessMain.m_objProcessMotion.m_objRobot.Values)
            {
                if (
                    robot.Status.IsPendantEmsOn == true
                    || robot.Status.IsControllerEmsOn == true
                    )
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 설비 동작 전 상태 확인 (동작 가능한 상태가 아니면 메시지를 띄우고 False를 반환한다)
        /// </summary>
        /// <returns></returns>
        public bool CheckEqpStatus()
        {
            bool bReturn = false;
            bool bResult = false;
            do
            {
                // 설비 MC 확인
                m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON, ref bResult);
                if (false == bResult)
                {
                    // Msg: 설비 메인 MC OFF 상태 입니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.MACHINE_MAIN_MC_OFF_STATE);
                    break;
                }

                if (false == m_objDocument.IsMasterLogin)
                {
                    if (true == IsFanFail)
                    {
                        int iLoopCount = 0;
                        foreach (var fanIO in mFanIO)
                        {
                            m_objIO.HLGetDigitalBit(fanIO, ref bResult);
                            if (false == bResult)
                            {
                                // Msg: Fan 이 미동작 중이다.
                                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.MACHINE_FAN_ALARM, fanIO.ToString());
                                break;
                            }
                            iLoopCount++;
                        }
                        break;
                    }
                }

                // Auto, Manual Key 상태 확인
                if (false == m_objDocument.IsMasterLogin)
                {
                    if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH) == true)
                    {
                        // Msg: 세이프티 키 모드 AUTO가 아닙니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                        break;
                    }
                }

                // 내부 램프 켜짐 체크
                if (false == m_objDocument.IsMasterLogin)
                {
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalOutput.Y_EQUIPMENT_INSIDE_LAMP_ON, ref bResult);
                    if (true == bResult)
                    {
                        // Msg: 실내등이 켜져 있습니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.INSIDE_LAMP_IS_ON);
                        break;
                    }
                }

                // 티치키 모두 AUTO 일때
                if (false == m_objDocument.IsMasterLogin)
                {
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH, ref bResult);
                    if (true == bResult)
                    {
                        // Msg: 세이프티 키 모드 AUTO가 아닙니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAFETY_KEY_IS_NOT_AUTO_MODE);
                        break;
                    }
                }

                // PLC 상태 확인
                if (false == m_objDocument.IsMasterLogin)
                {
                    if (GetSafetyIO().IsSafetyPlcNormal() == false)
                    {
                        // Msg: 설비 SAFETY_PLC_SIGNAL_NORMAL ON 상태가 아닙니다.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.EQP_SAFTTY_PLC_SIGNAL_NORMAL_STATUS_IS_NOT_ON);
                        break;
                    }
                }

                if (m_objDocument.GetRunMode() != CDefine.ERunMode.UnlinkDryrun)
                {
                    if (true == m_objDocument.m_objConfig.GetAlignOptionParameter(CDefine.EAlign.PreAlign).bUseVision)
                    {
                        if (m_objAlignInterfaces[CDefine.EAlign.PreAlign].HLIsConnected() == false)
                        {
                            m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.UNIT_CONNECTION_FAILED_PARAM1, "PRE-ALIGN");
                            break;
                        }
                    }

                    // 광학 검사 연결 상태 확인
                    var inspTypeMap = new Dictionary<CDefine.EInspInterface, CDefine.EInspectType>()
                    {
                        [CDefine.EInspInterface.PC1] = CDefine.EInspectType.Main,
                    };
                    foreach (CDefine.EInspInterface inspIndex in Enum.GetValues(typeof(CDefine.EInspInterface)))
                    {
                        if (m_objDocument.m_objProcessMain.m_objInspInterfaces[inspIndex].HLIsConnected() == false)
                        {
                            // 에러 스킵 모드에서는 연결 상태 유무 확인 하지 않음
                            if (m_objDocument.m_objConfig.GetInspOptionParameter(inspTypeMap[inspIndex]).eInspectionMode != CConfig.CInspOptionParameter.EInspectionMode.INSPECT_ERROR_SKIP
                                && m_objDocument.m_objConfig.GetInspOptionParameter(inspTypeMap[inspIndex]).eInspectionMode != CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE
                                )
                            {
                                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.UNIT_CONNECTION_FAILED_PARAM1, inspIndex);
                                return false;
                            }
                        }
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 키 스위치 모드가 오토인지 확인함
        /// </summary>
        /// <returns>True=Auto Mode, False=Teach Mode</returns>
        public bool CheckSaftyKeyModeIsAuto()
        {
            if (m_objDocument.IsMasterLogin == false)
            {
                if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH) == false)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 하트비트 상태 체크
        /// </summary>
        /// <param name="ePosition"></param>
        public void DoProcessCheckHeartBeatState(CDefine.EPosition ePosition)
        {
            // Heart Beat
            if (CDefine.EPosition.A == ePosition)
            {
                if (true == m_objProcessMotion.LoadInterface.IsInitialized)
                {
                    m_objProcessMotion.LoadInterface.UpdateTickHeartbeatSignals();
                }
            }
        }

        /// <summary>
        /// 하트비트 상태 체크
        /// </summary>
        /// <param name="ePosition"></param>
        public void DoProcessEQInterfaceCheckHeartBeatState(CDefine.EPosition ePosition)
        {
            // Heart Beat
            if (CDefine.EPosition.A == ePosition)
            {
                if (true == m_objProcessMotion.LoadInterface.IsInitialized)
                {
                    m_objDocument.m_eFrontState[0] = true == m_objProcessMotion.LoadInterface.IsUpperHeartbeat ? CCIMDefine.EFrontEquipmentState.FRONT_STATE_UP : CCIMDefine.EFrontEquipmentState.FRONT_STATE_DOWN;
                }
            }
            else
            {
                m_objDocument.m_eRearState[0] = CCIMDefine.ERearEquipmentState.REAR_STATE_UP;
            }
        }

        /// <summary>
        /// 설비 물류 인터페이스 전 확인 정보 상태
        /// </summary>
        public void DoProcessEQInterfaceEnviromentCheck(List<IProcessManagerInterface> interfaceManagers)
        {
            // 설비 중알람 상태 업데이트
            bool bHeavyAlarm = m_objDocument.GetIsHeavyAlarm();

            // 설비 경알람 상태 업데이트
            bool bLightAlarm = m_objDocument.GetisLightAlarm();

            // 설비 정지 상태 업데이트
            bool bPause =
                CDefine.ERunStatus.Start != m_objDocument.GetRunStatus()
                && CDefine.ERunStatus.LoadingStop != m_objDocument.GetRunStatus()
                && CDefine.ERunStatus.Stopping != m_objDocument.GetRunStatus()
                ;

            // 설비 알람 상태 업데이트
            bool bAbnormal =
                (
                    CDefine.ERunStatus.Start != m_objDocument.GetRunStatus()
                    && CDefine.ERunStatus.LoadingStop != m_objDocument.GetRunStatus()
                    && CDefine.ERunStatus.Stopping != m_objDocument.GetRunStatus()
                ) ? bHeavyAlarm : false;

            // DOOR OPEN 상태 업데이트
            bool bDoorOpen = DoorManager.Doors.Values.Any(item => item.IsOpened == true)
                        || DoorManager.Doors.Values.Any(item => item.IsUnlockPermit == true);

            // 비상 정지 상태 업데이트
            bool bEmergency = GetSafetyIO().IsSafetyPlcNormal() == false;

            // MC 상태 업데이트
            bool bMcStatus = m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON);

            // 설비 초기화 상태 업데이트
            bool bInitialStatus = m_objDocument.m_objProcessMain.m_objProcessMotion.CheckInitializeStatus();

            // 투입부 INTERLOCK 상태 업데이트
            bool bLoadInterlock = false;

            // 업데이트
            {
                m_objProcessMotion.LoadInterface.SetEmergency(bEmergency);
                m_objProcessMotion.LoadInterface.SetPause(bPause);
                m_objProcessMotion.LoadInterface.SetDoorOpen(bDoorOpen);
                m_objProcessMotion.LoadInterface.SetAbnormal(bAbnormal);
                m_objProcessMotion.LoadInterface.SetInterlock(0, bLoadInterlock);
                m_objProcessMotion.LoadInterface.SetMcStatus(bMcStatus);
                m_objProcessMotion.LoadInterface.SetHeavyAlarm(bHeavyAlarm);
                m_objProcessMotion.LoadInterface.SetLightAlarm(bLightAlarm);
                m_objProcessMotion.LoadInterface.SetInitialStatus(bInitialStatus);
            }
        }

        private HashSet<CDeviceIODefine.EDigitalInput> mFanIO = new HashSet<CDeviceIODefine.EDigitalInput>
        {
            CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_FAN_1_NORMAL,
            CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_FAN_2_NORMAL,
        };

        private List<CAlarmDefine.EAlarmList> mFanAlarmList = new List<CAlarmDefine.EAlarmList>
        {
            CAlarmDefine.EAlarmList.UT_MAIN_ELECTRONICBOX_FAN_1_STOP,
            CAlarmDefine.EAlarmList.UT_MAIN_ELECTRONICBOX_FAN_2_STOP,
        };

        /// <summary>
        /// EQP FanCheck 기능(현재 true -> 라인 내 false로 변경 필요)
        /// 현재 모달로 팝업 띄우는 것 수정 필요 / Fan 4개를 1번에 묶어 알람 1개만 띄워주도록 변경 필요.
        /// FIX
        /// </summary>
        public void DoProcessEqpFanCheck()
        {
            bool[] bFan = new bool[mFanAlarmList.Count];
            bFan.Select(i => i = false);

            do
            {
                if (true == m_objDocument.IsMasterLogin)
                {
                    mFanOkDateTime = DateTime.Now;
                    IsFanFail = false;
                    break;
                }

                int iLoopCount = 0;
                foreach (var fanIO in mFanIO)
                {
                    m_objIO.HLGetDigitalBit(fanIO, ref bFan[iLoopCount]);
                    iLoopCount++;
                }

                if (bFan.Any(i => i == false) == true)
                {
                    TimeSpan fanFailTime = DateTime.Now - mFanOkDateTime;

                    // Fan 알람이 발생한지 일정 시간후 체크
                    if (m_objDocument.m_objConfig.GetOptionParameter().iFanAlarmTime < fanFailTime.TotalMinutes)
                    {
                        IsFanFail = true;
                        iLoopCount = 0;
                        foreach (var fan in bFan)
                        {
                            if (false == fan)
                            {
                                // 중알람이면 중복 알람을 허용하지 않는다.
                                if (false == m_objDocument.GetIsOnAlarm(mFanAlarmList[iLoopCount]))
                                {
                                    m_objDocument.SetForcedAlarm(mFanAlarmList[iLoopCount]);
                                }
                            }
                            iLoopCount++;
                        }
                    }
                }
                else
                {
                    mFanOkDateTime = DateTime.Now;
                    IsFanFail = false;
                }
            } while (false);
        }

        public void DoProcessMaterialsManagement()
        {
            var optionParameter = m_objDocument.m_objConfig.GetOptionParameter();
            if (optionParameter.bUseMaterialsManagement == false)
            {
                return;
            }
            int firstAlarmIndex = (int)CAlarmDefine.EAlarmList.EF_PREFORM4_MAIN_EQUIPMENT_SILENT_STOP;
            foreach (var cell in CellDataManager.Cells.Values.GetExistCellList())
            {
                TimeSpan elapsedTime = DateTime.Now - cell.Data.Cell.InputTime;
                if (elapsedTime.TotalSeconds > optionParameter.iMaterialsManagementTime * 60)
                {
                    var alarmIndex = (CAlarmDefine.EAlarmList)(firstAlarmIndex + (int)cell.ProcessIndex);
                    // 최초 발생시 한 번만 처리함
                    if (m_objDocument.m_objAlarmList.GetIsOnAlarm(alarmIndex) == false)
                    {
                        // 알람 발생
                        m_objDocument.SetForcedAlarm(alarmIndex);
                    }
                }
            }
        }

        private void DoProcessNormalEnergyMode()
        {
            foreach (var item in m_objFFU.Values)
            {
                item.HLSetAllRequiredSpeed(0, m_objDocument.m_objConfig.GetOptionParameter().LowEnergyModeFfuNormalRpm);
            }
        }

        private void DoProcessLowEnergyMode()
        {
            foreach (var item in m_objFFU.Values)
            {
                item.HLSetAllRequiredSpeed(0, m_objDocument.m_objConfig.GetOptionParameter().LowEnergyModeFfuSlowRpm);
            }
        }

        private void DoProcessUpdateMonitorIO()
        {
            if (EqToEq.SdvGammaFoldableLine.Address.IsInitialized == false)
            {
                return;
            }
            if (mMonitorIoUpdateTick > Utils.GetTimestamp())
            {
                return;
            }
            if (mMonitorIoSet == null)
            {
                mMonitorIoSet = new MonitorIO.WordsSet();
            }

            // 설비 상태
            {
                mMonitorIoSet.EquipmentStatus.Availability = m_objDocument.m_eAvailabilityState[0] == CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_UP;
                mMonitorIoSet.EquipmentStatus.Interlock = m_objDocument.m_eInterlockState[0] == CCIMDefine.EInterlockState.INTERLOCK_STATE_OFF;
                mMonitorIoSet.EquipmentStatus.Move = m_objDocument.m_eMoveState[0] == CCIMDefine.EMoveState.MOVE_STATE_RUNNING;
                mMonitorIoSet.EquipmentStatus.Run = m_objDocument.m_eRunState[0] == CCIMDefine.ERunState.RUN_STATE_RUN;
                mMonitorIoSet.EquipmentStatus.InDelay1 = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.IsPending;
                mMonitorIoSet.EquipmentStatus.OutDelay1 = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip.IsUnloadPending;
                mMonitorIoSet.EquipmentStatus.CellCount = Convert.ToInt16(CellDataManager.Cells.Values.GetExistCellCount());
                EqToEq.SdvGammaFoldableLine.Address.Words.SelfToMonitorIO.EquipmentStatus.BatchWrite(mMonitorIoSet.EquipmentStatus);
            }
            // 셀 위치
            {
                var cellPositionMapping = new CellData.ECellData[]
                {
                    CellData.ECellData.InShuttleP2,
                    CellData.ECellData.InShuttleP1,
                    CellData.ECellData.InRobotP2,
                    CellData.ECellData.InRobotP1,
                    CellData.ECellData.InspStageP2,
                    CellData.ECellData.InspStageP1,
                    CellData.ECellData.OutRobotP2,
                    CellData.ECellData.OutRobotP1,
                    CellData.ECellData.OutFlipP2,
                    CellData.ECellData.OutFlipP1,
                };
                for (int i = 0; i < cellPositionMapping.Length; i++)
                {
                    mMonitorIoSet.CellPosition.PositionBits[i] = CellDataManager.Cells[cellPositionMapping[i]].IsCellExist();
                }
                EqToEq.SdvGammaFoldableLine.Address.Words.SelfToMonitorIO.CellPosition.BatchWrite(mMonitorIoSet.CellPosition);
            }

            mMonitorIoUpdateTick = Utils.GetTimestamp() + mMonitorIoUpdateIntervalTick;
        }

        private readonly HashSet<CAlarmDefine.EAlarmList> mEquipmentSafetyCheckList = new HashSet<CAlarmDefine.EAlarmList>
        {
            // 오토런 중 세이프티 모드 키 변경
            CAlarmDefine.EAlarmList.OP_MAIN_MODEKEY_N_TEACH,
            // 모든 유닛 정지
            CAlarmDefine.EAlarmList.OP_MAIN_UNIT_ALL_STOP,
            // 공압 공급 이상 상황
            CAlarmDefine.EAlarmList.VC_MAIN_VACUUM_MAIN_SUPPLY,
            CAlarmDefine.EAlarmList.VC_INSP_STAGE_VACUUM_MAIN_SUPPLY,
            CAlarmDefine.EAlarmList.VC_MAIN_AIR_MAIN_N_SUPPLY,
            // 과열 감지 상황
            CAlarmDefine.EAlarmList.UT_MAIN_ELECTRONICBOX_TEMPERATURE_1_N_OVER40,
            CAlarmDefine.EAlarmList.UT_MAIN_PCRACK_TEMPERATURE_1_N_OVER40,
            // 연기 감지 상황
            CAlarmDefine.EAlarmList.UT_MAIN_ELECTRONICBOX_SMOKE_SENSOR_1_N_DETECTED,
            // 비상 정지 상황
            CAlarmDefine.EAlarmList.UT_MAIN_SAFETYPLC_GPS_MC_POWER_OFF,
            CAlarmDefine.EAlarmList.EM_MAIN_CONTROLPANEL_FRONT_EMS_X000,
            CAlarmDefine.EAlarmList.EM_MAIN_CONTROLPANEL_REAR_EMS_X001,
            CAlarmDefine.EAlarmList.EM_TRANSFER_ROBOT_IN_PENDANT_EMS,
            CAlarmDefine.EAlarmList.EM_TRANSFER_ROBOT_IN_CONTROLLER_EMS,
            CAlarmDefine.EAlarmList.EM_TRANSFER_ROBOT_OUT_PENDANT_EMS,
            CAlarmDefine.EAlarmList.EM_TRANSFER_ROBOT_OUT_CONTROLLER_EMS,
            CAlarmDefine.EAlarmList.EM_UPPER_EQUIPMENT_N_EMERGENCY_STATE,
            CAlarmDefine.EAlarmList.EM_LOWER_EQUIPMENT_N_EMERGENCY_STATE,
            // 도어락 강제 해제
            CAlarmDefine.EAlarmList.DO_MAIN_ANY_DOOR_FORECE_RELEASED_X02A,
            // 시리얼 통신 끊김
            CAlarmDefine.EAlarmList.CO_MAIN_ELECTRONICBOX_TEMPERATURE_DISCONNECTED,
            CAlarmDefine.EAlarmList.CO_MAIN_PCRACK_TEMPERATURE_DISCONNECTED,
            CAlarmDefine.EAlarmList.CO_MAIN_DEVICE_MCU_DISCONNECTED,
            // 상,하류 도어 열림
            CAlarmDefine.EAlarmList.EM_UPPER_EQUIPMENT_DOOR_OPEN,
            CAlarmDefine.EAlarmList.EM_LOWER_EQUIPMENT_DOOR_OPEN,
            // FFU 정지
            CAlarmDefine.EAlarmList.UT_MAIN_MCU_FFU_1_STOP,
            // FFU 알람
            CAlarmDefine.EAlarmList.UT_MAIN_MCU_FFU_1_ALARM_OCCURRED,
            // 자재 체크
            CAlarmDefine.EAlarmList.EF_IN_SHUTTLE_CELL_DAMAGED_CHECK,
            CAlarmDefine.EAlarmList.EF_IN_ROBOT_CELL_DAMAGED_CHECK,
            CAlarmDefine.EAlarmList.EF_INSP_STAGE_CELL_DAMAGED_CHECK,
            CAlarmDefine.EAlarmList.EF_OUT_ROBOT_CELL_DAMAGED_CHECK,
            CAlarmDefine.EAlarmList.EF_OUT_FLIP_CELL_DAMAGED_CHECK,
        };
        private readonly HashSet<CAlarmDefine.EAlarmList> mEmergencyStopCheckList = new HashSet<CAlarmDefine.EAlarmList>
        {
            CAlarmDefine.EAlarmList.EM_UPPER_EQUIPMENT_N_EMERGENCY_STATE,
            CAlarmDefine.EAlarmList.EM_LOWER_EQUIPMENT_N_EMERGENCY_STATE,
        };
        private readonly HashSet<CAlarmDefine.EAlarmList> mSmokeOrOvertempCheckList = new HashSet<CAlarmDefine.EAlarmList>
        {
            // 연기 감지 상황
            CAlarmDefine.EAlarmList.UT_MAIN_ELECTRONICBOX_SMOKE_SENSOR_1_N_DETECTED,
            // 과열 감지 상황
            CAlarmDefine.EAlarmList.UT_MAIN_ELECTRONICBOX_TEMPERATURE_1_N_OVER40,
            CAlarmDefine.EAlarmList.UT_MAIN_PCRACK_TEMPERATURE_1_N_OVER40,
        };
        /// <summary>
        /// 환경 안전 체크 ( 도어열림 , 세이프티 키 상태, 비상정지 )
        /// </summary>
        public void DoProcessEqpSafetyCheck()
        {
            bool bSafetyAutoMode = CheckSaftyKeyModeIsAuto();
            bool bDoorOpened = GetAnyDoorOpened();
            bool bShouldEmergencyStop = false;
            foreach (var item in mEquipmentSafetyCheckList)
            {
                // 상시 체크 알람 발생 확인
                if (true == checkSafetyAlarmOccured(item, true, bSafetyAutoMode, bDoorOpened))
                {
                    if (
                        global::Utils.TimedAlarmActivator.IsTimedAlarmExpired(item) == false
                        && mEmergencyStopCheckList.Contains(item) == false
                        )
                    {
                        continue;
                    }
                    // 비상정지해야 할 알람인 경우
                    if (mEmergencyStopCheckList.Contains(item) == true)
                    {
                        bShouldEmergencyStop = true;
                    }
                    // 연기 혹은 과열감지 관련 알람인 경우
                    if (mSmokeOrOvertempCheckList.Contains(item) == true)
                    {
                        m_objDocument.IsSmokeOrOvertempAlarmHappened = true;
                    }
                    // 최초 발생시 한 번만 처리함
                    if (m_objDocument.m_objAlarmList.GetIsOnAlarm(item) == false)
                    {
                        // 알람 발생
                        m_objDocument.SetForcedAlarm(item);
                    }
                }
                else
                {
                    global::Utils.TimedAlarmActivator.StopTimedAlarm(item);
                }
            }
            global::Utils.TimedAlarmActivator.Backup();

            // 비상 정지 처리
            do
            {
                // 공통 조건
                {
                    // 마스터 로그인 상태에서는 비상 정지를 하지 않는다.
                    if (m_objDocument.IsMasterLogin == true)
                    {
                        break;
                    }
                    // 이동중이 아닐때는 비상 정지를 하지 않는다.
                    if (m_objProcessMotion.CheckMovingStatus() == false)
                    {
                        break;
                    }
                    // 조그 이동중에는 비상 정지를 하지 않는다.
                    if (IsJogMoving == true)
                    {
                        break;
                    }
                }

                // 비상 정지 실행
                {
                    // 비상 정지가 필요한 알람이 클리어되지 않은 상태에서 이동 중이면 강제로 정지시킨다.
                    // + 상류 설비 비상정지 알람
                    // + 하류 설비 비상정지 알람
                    if (bShouldEmergencyStop == true)
                    {
                        m_objProcessMotion.EmergencyStopAll();
                    }
                    // 티치 모드에서 문이 열렸을 때 이동 중이면 강제로 정지시킨다. (오토 모드에서 도어락을 해제하면 Safety PLC가 떨어지기 때문에 모드는 따로 체크하지 않음)
                    else if (bSafetyAutoMode == false)
                    {
                        m_objProcessMotion.EmergencyStopAll();
                    }
                    // 인체감지 센서가 감지됐을 때 이동 중이면 강제로 정지시킨다.
                    else if (GetSafetyIO().HumanDetectSensers.Any(i => m_objDocument.GetIOBit(i) == true) == true)
                    {
                        m_objProcessMotion.EmergencyStopAll();
                    }
                    // 다관절로봇 EMO 버튼이 눌렸을 때 이동 중이면 강제로 정지시킨다.
                    else if (CheckRobotEMO() == false)
                    {
                        m_objProcessMotion.EmergencyStopAll();
                    }
                    // 부분적으로 비상 정지해야 할 경우
                    else
                    {
                        m_objProcessMotion.EmergencyStopGroupByEhsPolicy();
                    }
                }
            } while (false);

            // 열연감지기 리셋 램프 및 버튼 처리
            do
            {
                if (m_objDocument.IsSmokeOrOvertempAlarmHappened == true
                    && m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_SMOKE_RESET_BUTTON_PUSH) == CDefine.ON
                    )
                {
                    m_objDocument.IsSmokeOrOvertempAlarmHappened = false;
                }
                if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_ELECTRONIC_BOX_SMOKE_RESET_BUTTON_LAMP_ON) == m_objDocument.IsSmokeOrOvertempAlarmHappened)
                {
                    break;
                }
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_ELECTRONIC_BOX_SMOKE_RESET_BUTTON_LAMP_ON, m_objDocument.IsSmokeOrOvertempAlarmHappened == true);
            } while (false);
        }

        public bool CheckSafetyAlarmOccured(CAlarmDefine.EAlarmList alarm)
        {
            return checkSafetyAlarmOccured(alarm, false, CheckSaftyKeyModeIsAuto(), GetAnyDoorOpened());
        }

        private bool checkSafetyAlarmOccured(CAlarmDefine.EAlarmList alarm, bool checkException, bool bSafetyAutoMode, bool bDoorOpened)
        {
            bool bResult = false;
            bool bGetSignal = false;
            bool bEquipmentRunning = (
                CDefine.ERunStatus.Start == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus()
                );
            bool bDryRunMode = CDefine.ERunMode.UnlinkDryrun == m_objDocument.GetRunMode();
            bool bSimulationMode = m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON;
            bool bMasterLoginMode = m_objDocument.IsMasterLogin;
            CFFUStatus ffuStatus;
            TimeSpan elapsedTime;
            var optionParameter = m_objDocument.m_objConfig.GetOptionParameter();

            switch (alarm)
            {
                case CAlarmDefine.EAlarmList.OP_MAIN_MODEKEY_N_TEACH:
                    if (false == bMasterLoginMode
                        && true == bEquipmentRunning
                        && m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH) == true
                        )
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.OP_MAIN_UNIT_ALL_STOP:
                    if (UnitManager.Units.Values.All(i => i.GetStatus() == UnitGroup.EStatus.Pause || i.GetStatus() == UnitGroup.EStatus.Interlock) == true)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.UT_MAIN_SAFETYPLC_GPS_MC_POWER_OFF:
                    if (GetSafetyIO().IsSafetyPlcNormal() == false)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_MAIN_CONTROLPANEL_FRONT_EMS_X000:
                    if (true == m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_EMERGENCY_SWITCH_PUSH))
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_MAIN_CONTROLPANEL_REAR_EMS_X001:
                    if (true == m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_EMERGENCY_SWITCH_PUSH))
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.DO_MAIN_ANY_DOOR_FORECE_RELEASED_X02A:
                    if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_DOOR_FORCE_OPEN_NORMAL) == false)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_TRANSFER_ROBOT_IN_PENDANT_EMS:
                    if (true == m_objProcessMotion.InRobot.Nachi.Robot.Status.IsPendantEmsOn)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_TRANSFER_ROBOT_IN_CONTROLLER_EMS:
                    if (true == m_objProcessMotion.InRobot.Nachi.Robot.Status.IsControllerEmsOn)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_TRANSFER_ROBOT_OUT_PENDANT_EMS:
                    if (true == m_objProcessMotion.OutRobot.Nachi.Robot.Status.IsPendantEmsOn)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_TRANSFER_ROBOT_OUT_CONTROLLER_EMS:
                    if (true == m_objProcessMotion.OutRobot.Nachi.Robot.Status.IsControllerEmsOn)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.VC_MAIN_VACUUM_MAIN_SUPPLY:
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_MAIN_VACUUM_PRESSURE_SENSOR, ref bGetSignal);
                    if (false == bGetSignal)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.VC_INSP_STAGE_VACUUM_MAIN_SUPPLY:
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_INSP_STAGE_VAC_PRESSURE_SENSOR, ref bGetSignal);
                    if (false == bGetSignal)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.VC_MAIN_AIR_MAIN_N_SUPPLY:
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_MAIN_CDA_PRESSURE_SENSOR, ref bGetSignal);
                    if (false == bGetSignal)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.UT_MAIN_ELECTRONICBOX_TEMPERATURE_1_N_OVER40:
                    // 온도 리셋 버튼을 누르지 않은 경우
                    if (m_objDocument.m_objAlarmList.GetIsOnAlarm(alarm) == true
                        && m_objDocument.IsSmokeOrOvertempAlarmHappened == true
                        )
                    {
                        bResult = true;
                        break;
                    }
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_OVER_TEMP_ALARM, ref bGetSignal);
                    if (true == bGetSignal)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.UT_MAIN_PCRACK_TEMPERATURE_1_N_OVER40:
                    // 온도 리셋 버튼을 누르지 않은 경우
                    if (m_objDocument.m_objAlarmList.GetIsOnAlarm(alarm) == true
                        && m_objDocument.IsSmokeOrOvertempAlarmHappened == true
                        )
                    {
                        bResult = true;
                        break;
                    }
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_PC_RACK_OVER_TEMP_ALARM, ref bGetSignal);
                    if (true == bGetSignal)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.UT_MAIN_ELECTRONICBOX_SMOKE_SENSOR_1_N_DETECTED:
                    // 온도 리셋 버튼을 누르지 않은 경우
                    if (m_objDocument.m_objAlarmList.GetIsOnAlarm(alarm) == true
                        && m_objDocument.IsSmokeOrOvertempAlarmHappened == true
                        )
                    {
                        bResult = true;
                        break;
                    }
                    m_objIO.HLGetDigitalBit(CDeviceIODefine.EDigitalInput.X_ELECTRONIC_BOX_SMOKE_ALARM, ref bGetSignal);
                    if (true == bGetSignal)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.CO_TRANSFER_CIM_N_CIM_DISCONNECTED:
                    if (
                        true == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM
                        && false == m_objDocument.m_bCIMConnected
                        )
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.CO_MAIN_ELECTRONICBOX_TEMPERATURE_DISCONNECTED:
                    if (SerialReaders.Temperature[CDefine.ETemperature.MainElectronicBox].IsReadingFail == true)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.CO_MAIN_PCRACK_TEMPERATURE_DISCONNECTED:
                    if (SerialReaders.Temperature[CDefine.ETemperature.PcRack].IsReadingFail == true)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.CO_MAIN_DEVICE_MCU_DISCONNECTED:
                    if (SerialReaders.Mcu[CDefine.EMcu.MainGruop].IsReadingFail == true)
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_UPPER_EQUIPMENT_DOOR_OPEN:
                    if (bDryRunMode == false
                        // 설비 상태와 상관없이 문이 열리면 알람 발생 (현장 요청 사항 반영)
                        //&& bEquipmentRunning == true
                        && m_objProcessMotion.LoadInterface.CheckUpperEqInterfaceDoorOpened() == true
                        )
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_LOWER_EQUIPMENT_DOOR_OPEN:
                    // !! Lower I/F 없음
                    //if (bDryRunMode == false
                    //    // 설비 상태와 상관없이 문이 열리면 알람 발생 (현장 요청 사항 반영)
                    //    //&& bEquipmentRunning == true
                    //    && m_objProcessMotion.UnloadInterface.CheckLowerEqInterfaceDoorOpened() == true
                    //    )
                    //{
                    //    bResult = true;
                    //}
                    break;

                case CAlarmDefine.EAlarmList.EM_UPPER_EQUIPMENT_N_EMERGENCY_STATE:
                    if (bDryRunMode == false
                        && m_objProcessMotion.LoadInterface.CheckUpperEqInterfaceOnEMS() == true
                        )
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EM_LOWER_EQUIPMENT_N_EMERGENCY_STATE:
                    // !! Lower I/F 없음
                    //if (bDryRunMode == false
                    //    && m_objProcessMotion.UnloadInterface.CheckLowerEqInterfaceOnEMS() == true
                    //    )
                    //{
                    //    bResult = true;
                    //}
                    break;

                case CAlarmDefine.EAlarmList.UT_MAIN_MCU_FFU_1_STOP:
                    m_objDocument.m_objProcessMain.m_objFFU[CDefine.EMcu.MainGruop].HLGetStatus(1, 0, out ffuStatus);
                    if (bMasterLoginMode == false
                        && SerialReaders.Mcu[CDefine.EMcu.MainGruop].IsReadingFail == false
                        && SerialReaders.Mcu[CDefine.EMcu.MainGruop].Value[0] == 0
                        )
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.UT_MAIN_MCU_FFU_1_ALARM_OCCURRED:
                    m_objDocument.m_objProcessMain.m_objFFU[CDefine.EMcu.MainGruop].HLGetStatus(1, 0, out ffuStatus);
                    if (bMasterLoginMode == false
                        && bSimulationMode == false
                        && SerialReaders.Mcu[CDefine.EMcu.MainGruop].IsReadingFail == false
                        && (ffuStatus[CFFUStatus.EStatus.NotConnect] == true)
                        )
                    {
                        bResult = true;
                    }
                    break;

                case CAlarmDefine.EAlarmList.EF_IN_SHUTTLE_CELL_DAMAGED_CHECK:
                    foreach (var cellDataHandler in CellDataManager.ProcessCells[CellData.EProcess.InShuttle].GetExistCellList())
                    {
                        elapsedTime = DateTime.Now - cellDataHandler.Data.Cell.InputTime;
                        if (elapsedTime.TotalSeconds > optionParameter.iMaterialsManagementTime * 60 && optionParameter.bUseMaterialsManagement == true)
                        {
                            bResult = true;
                            break;
                        }
                    }
                    break;

                case CAlarmDefine.EAlarmList.EF_IN_ROBOT_CELL_DAMAGED_CHECK:
                    foreach (var cellDataHandler in CellDataManager.ProcessCells[CellData.EProcess.InRobot].GetExistCellList())
                    {
                        elapsedTime = DateTime.Now - cellDataHandler.Data.Cell.InputTime;
                        if (elapsedTime.TotalSeconds > optionParameter.iMaterialsManagementTime * 60 && optionParameter.bUseMaterialsManagement == true)
                        {
                            bResult = true;
                            break;
                        }
                    }
                    break;

                case CAlarmDefine.EAlarmList.EF_INSP_STAGE_CELL_DAMAGED_CHECK:
                    foreach (var cellDataHandler in CellDataManager.ProcessCells[CellData.EProcess.InspStage].GetExistCellList())
                    {
                        elapsedTime = DateTime.Now - cellDataHandler.Data.Cell.InputTime;
                        if (elapsedTime.TotalSeconds > optionParameter.iMaterialsManagementTime * 60 && optionParameter.bUseMaterialsManagement == true)
                        {
                            bResult = true;
                            break;
                        }
                    }
                    break;

                case CAlarmDefine.EAlarmList.EF_OUT_ROBOT_CELL_DAMAGED_CHECK:
                    foreach (var cellDataHandler in CellDataManager.ProcessCells[CellData.EProcess.OutRobot].GetExistCellList())
                    {
                        elapsedTime = DateTime.Now - cellDataHandler.Data.Cell.InputTime;
                        if (elapsedTime.TotalSeconds > optionParameter.iMaterialsManagementTime * 60 && optionParameter.bUseMaterialsManagement == true)
                        {
                            bResult = true;
                            break;
                        }
                    }
                    break;

                case CAlarmDefine.EAlarmList.EF_OUT_FLIP_CELL_DAMAGED_CHECK:
                    foreach (var cellDataHandler in CellDataManager.ProcessCells[CellData.EProcess.OutFlip].GetExistCellList())
                    {
                        elapsedTime = DateTime.Now - cellDataHandler.Data.Cell.InputTime;
                        if (elapsedTime.TotalSeconds > optionParameter.iMaterialsManagementTime * 60 && optionParameter.bUseMaterialsManagement == true)
                        {
                            bResult = true;
                            break;
                        }
                    }
                    break;

                default:
                    if (true == checkException)
                    {
                        Debug.Assert(false, "알람 체크 루틴 등록 안됨.");
                    }
                    break;
            }
            return bResult;
        }

        private bool GetAnyDoorOpened() => DoorManager.Doors.Values.Any(item => item.IsOpened == true);

        private bool getDoorForcedReleased(Door.EDoor doorIndex, bool bSafetyAutoMode)
        {
            if (DoorManager.Doors[doorIndex].IsForcedUnlock == false)
            {
                mDoorForcedReleasedAlarmDelay[(int)doorIndex] = 0;
                return false;
            }
            mDoorForcedReleasedAlarmDelay[(int)doorIndex]++;
            if (mDoorForcedReleasedAlarmDelay[(int)doorIndex] < 10)
            {
                return false;
            }
            return true;
        }

        // !! Lazer 없음
        //public void DoProcessEqpLaserCheck()
        //{
        //    !!Lazer 없음
        //    if (CheckSafetyLaserInterlockOff() == false)
        //    {
        //        m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_CONTROL_INTERLOCK_ON, CDefine.ON);

        //        if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY) == CDefine.ON)
        //        {
        //            foreach (var inspInterface in m_objDocument.m_objProcessMain.m_objInspInterfaces.Values)
        //            {
        //                if (inspInterface.HLIsConnected() == false)
        //                {
        //                    continue;
        //                }
        //                inspInterface.HLSendLaserInterlockRequest();
        //                inspInterface.HLSendLaserCheckRequest();
        //            }
        //            m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_ON_MESSAGE_DISPLAY, CDefine.OFF);
        //        }
        //    }
        //    else
        //    {
        //        m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_LASER_CONTROL_INTERLOCK_ON, CDefine.OFF);
        //    }
        //}

        // !! Lazer 없음
        //public bool CheckSafetyLaserInterlockOff()
        //{
        //    if (m_objDocument.IsMasterLogin == true)
        //    {
        //        return true;
        //    }
        //    // 도어
        //    if (
        //        GetAnyDoorOpened() == false
        //        && GetSafetyIO().IsSafetyAllDoorClosed() == true
        //        )
        //    {
        //        return true;
        //    }

        //    // 로봇 동시작업
        //    if (m_objProcessMotion.m_objRobot.Values.All(i => i.Status.IsTeachModeEntry == false && i.Status.IsTeachModeEntry == false && i.Status.IsRunningU1 == true) == false)
        //    {
        //        return false;
        //    }
        //    if (m_objProcessMotion.m_objRobot.Values.All(i => i.Status.IsPendantEmsOn == false && i.Status.IsControllerEmsOn == false) == false)
        //    {
        //        return false;
        //    }
        //    // MC Power
        //    if (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON) == false)
        //    {
        //        return false;
        //    }
        //    // Enable Grip
        //    if (
        //        m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_MAINT_ENABLE_SWITCH_GRIP_ON) == false
        //        && m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_MAINT_ENABLE_SWITCH_GRIP_ON) == false
        //        )
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// 전면부 파워 동작 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadEquipmentPowerProcess()
        {
            NoiseFilterManager.Add(CDeviceIODefine.EDigitalInput.X_FRONT_OP_OFF_BUTTON_PUSH.ToString(), () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_FRONT_OP_OFF_BUTTON_PUSH), TimeSpan.FromMilliseconds(0d), FilterOptionFlags.RisingEdge);
            NoiseFilterManager.Add(CDeviceIODefine.EDigitalInput.X_REAR_OP_OFF_BUTTON_PUSH.ToString(), () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_REAR_OP_OFF_BUTTON_PUSH), TimeSpan.FromMilliseconds(0d), FilterOptionFlags.RisingEdge);
            NoiseFilterManager.Add(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON.ToString(), () => m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAGNET_CONTACT_ON), TimeSpan.FromMilliseconds(0d), FilterOptionFlags.FallingEdge);

            while (false == mbThreadExit)
            {
                DoProcessEquipmentPower();
                DoProcessSafetyControllerResetProcess();
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 설비 상태 변경 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadEquipmentStatusProcess()
        {
            while (false == mbThreadExit)
            {
                // 설비 정지중 상태 감지
                if (CDefine.ERunStatus.Stopping == m_objDocument.GetRunStatus())
                {
                    m_objDocument.GetMainFrame().ShowWaitMessage(true, "WAITING FOR EQUIPMENT STOP");
                    if (true == CheckProcessIdleStatus())
                    {
                        //m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_VISION_PARTICLE_BLOW, CDefine.OFF);
                        // 메뉴얼 속도 전환
                        m_objDocument.SetMoveModeType(CDefine.EMoveModeType.MOVE_MODE_TYPE_MANUAL);
                        m_objDocument.SetRunStatus(CDefine.ERunStatus.Stop);
                        m_objDocument.GetMainFrame().ShowWaitMessage(false);

                        // 설비 상태 업데이트
                        m_objDocument.m_objProcessCIM.BeginSyncEquipmentStateReport();
                        {
                            // 정지 버튼으로 정지시 Loss Code 창 디스플레이
                            if (
                                true == m_objDocument.m_bForceStopButton
                                && false == m_objDocument.GetIsHeavyAlarm()
                                && false == m_objDocument.m_bUnitInterLockHappened
                                && false == m_objDocument.m_bInterLockHappened
                                )
                            {
                                m_objDocument.GetMainFrame().ShowLossCode(true);

                                // 상태 중복 보고를 막는다
                                m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] = CCIMDefine.EMoveState.MOVE_STATE_PAUSE;
                            }
                            m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EMoveState.MOVE_STATE_PAUSE;
                            if (
                                true == m_objDocument.m_bInterLockHappened
                                || (
                                true == m_objDocument.m_bUnitInterLockHappened
                                && true == m_objDocument.m_bLoadingStopFinish
                                )
                                )
                            {
                                m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EInterlockState.INTERLOCK_STATE_ON;
                            }
                            // 중알람 발생시 설비 상태 덤프
                            if (true == m_objDocument.GetIsHeavyAlarm())
                            {
                                m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_DOWN;
                                m_objDocument.EquipmentDump("HEAVY_ALARM_STOP");
                                MccLogManager.GetAlarmStopActions().BatchWriteStart();
                            }
                        }
                        m_objDocument.m_objProcessCIM.EndSyncEquipmentStateReport();

                        // 설비 플래그 업데이트
                        if (true == m_objDocument.m_bLoadingStopFinish)
                        {
                            m_objDocument.m_bUnitInterLockHappened = false;
                        }
                        if (true == m_objDocument.m_bInterLockHappened)
                        {
                            m_objDocument.m_bInterLockHappened = false;
                        }
                        m_objDocument.m_bForceStopButton = false;
                    }
                }
                // 설비 사이클 스탑 상태 감지
                else if (CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus())
                {
                    m_objDocument.m_bLoadingStopFinish = false;
                    m_objDocument.GetMainFrame().ShowWaitMessage(true, "WAITING FOR EQUIPMENT LOT END");

                    // 사이클 스탑 완료 상태 확인
                    if (
                        true == DoProcessDataEmpty()
                        && true == DoProcessEquipmentStatus()
                        )
                    {
                        // 로딩 스탑 완료
                        m_objDocument.m_bLoadingStopFinish = true;
                        // 설비 정지중 상태로 전환
                        m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                    }
                }
                else if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                {
                    // 만약 유닛 인터락 로딩 스탑이 끝나지 않은 상태에서 강제로 정지하게 된다면 다음 시작시 로딩 스탑을 이어서 진행한다
                    if (
                        true == m_objDocument.m_bUnitInterLockHappened
                        && false == m_objDocument.m_bLoadingStopFinish
                        )
                    {
                        m_objDocument.SetRunStatus(CDefine.ERunStatus.LoadingStop);
                    }
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// EQ Interface 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadEQInterfaceProcess()
        {
            // 매니져 리스트 초기화
            var interfaceManagers = new List<IProcessManagerInterface>(2)
            {
                m_objProcessMotion.LoadInterface
            };

            // Interface 초기화 대기
            SpinWait.SpinUntil(() =>
            {
                Thread.Sleep(10);
                return (interfaceManagers.All(i => i.IsInitialized)) || mbThreadExit == true;
            });

            while (false == mbThreadExit)
            {
                // 설비 하트 비트 보내기.
                DoProcessCheckHeartBeatState(CDefine.EPosition.A);
                DoProcessCheckHeartBeatState(CDefine.EPosition.B);
                DoProcessEQInterfaceCheckHeartBeatState(CDefine.EPosition.A);
                DoProcessEQInterfaceCheckHeartBeatState(CDefine.EPosition.B);
                DoProcessEQInterfaceEnviromentCheck(interfaceManagers);
                DoProcessUpdateMonitorIO();
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// EQP 환경 안전 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadEqpSafetyCheckProcess()
        {
            Thread.Sleep(500);
            foreach (var item in mEquipmentSafetyCheckList)
            {
                global::Utils.TimedAlarmActivator.AddTimedAlarm(item);
            }

            while (false == mbThreadExit)
            {
                // EQP 환경 안전
                DoProcessEqpSafetyCheck();
                DoProcessEqpFanCheck();
                // !! Lazer 없음
                //DoProcessEqpLaserCheck();

                Thread.Sleep(mSafetyThreadSleepTime);
            }
        }

        /// <summary>
        /// 설비 상태에 따른 버튼 변경 딜리게이트 호출 스레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadEquipmentStatusButtonStatusProcess()
        {
            CCIMDefine.EMoveState ePrevMoveState = CCIMDefine.EMoveState.MOVE_STATE_PAUSE;
            bool bBeforeCimConnect = false;

            while (false == mbThreadExit)
            {
                // 현 상태 Move State가 다르면
                CCIMDefine.EMoveState eMoveState = m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE];
                if (ePrevMoveState != eMoveState)
                {
                    // 유저 권한에 따른 리소스 상태 변경 호출
                    m_objDocument.SetResourceControl();
                    // Move State 업데이트
                    ePrevMoveState = eMoveState;
                }
                // CIM 연결 상태가 변경되면
                if (bBeforeCimConnect != m_objDocument.m_bCIMConnected)
                {
                    // 유저 권한에 따른 리소스 상태 변경 호출
                    m_objDocument.SetResourceControl();
                    // 이전 상태 업데이트
                    bBeforeCimConnect = m_objDocument.m_bCIMConnected;
                }

                Thread.Sleep(10);
            }
        }

        private void ThreadSilentStopWatchdog()
        {
            long lastCheckTime = Utils.GetTimestamp();
            long checkInterval = TimeSpan.FromSeconds(1d).Ticks;
            long silentStopStartTime = TimeSpan.MaxValue.Ticks;
            string[] lastMotionStatus = new string[m_objProcessMotion.AllMotions.Count];
            string[] getMotionStatus = null;

            while (mbThreadExit == false)
            {
                Thread.Sleep(100);
                if (m_objDocument.GetRunStatus() == CDefine.ERunStatus.Start
                    || m_objDocument.GetRunStatus() == CDefine.ERunStatus.LoadingStop
                    || m_objDocument.GetRunStatus() == CDefine.ERunStatus.Stopping
                    )
                {
                    // 업데이트 시간이 경과하지 않았다
                    if (Utils.GetTimestamp() - lastCheckTime < checkInterval)
                    {
                        continue;
                    }
                    lastCheckTime = Utils.GetTimestamp();

                    // 설비에 셀 데이터가 없다 (투입 지연)
                    if (m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.ERunState.RUN_STATE_IDLE)
                    {
                        silentStopStartTime = TimeSpan.MaxValue.Ticks;
                        continue;
                    }

                    // 현재 설비 상태 업데이트
                    if (getMotionStatus == null)
                    {
                        getMotionStatus = new string[m_objProcessMotion.AllMotions.Count];
                    }
                    bool bEquipmentStatusChanged = false;
                    for (int i = 0; i < getMotionStatus.Length; i++)
                    {
                        getMotionStatus[i] = m_objProcessMotion.AllMotions[i].ToString();
                        if (bEquipmentStatusChanged == false
                            && getMotionStatus[i] != lastMotionStatus[i]
                            )
                        {
                            bEquipmentStatusChanged = true;
                        }
                    }

                    // 설비 상태 변경 됐다
                    if (bEquipmentStatusChanged == true)
                    {
                        Array.Copy(getMotionStatus, 0, lastMotionStatus, 0, getMotionStatus.Length);
                        silentStopStartTime = TimeSpan.MaxValue.Ticks;
                        continue;
                    }

                    // 배출 인터페이스가 지연 상태다 (배출 지연)
                    if (m_objProcessMotion.OutFlip.IsUnloadPending == true)
                    {
                        silentStopStartTime = TimeSpan.MaxValue.Ticks;
                        continue;
                    }

                    // 상류 설비 문열림으로 인한 투입 지연 상태다 (투입 지연)
                    if (m_objProcessMotion.LoadInterface.CheckUpperEqInterfaceDoorOpened() == true)
                    {
                        silentStopStartTime = TimeSpan.MaxValue.Ticks;
                        continue;
                    }

                    // 무언 정지 타이머가 정지 상태다
                    if (silentStopStartTime == TimeSpan.MaxValue.Ticks)
                    {
                        silentStopStartTime = Utils.GetTimestamp();
                    }

                    // 무언 정지 알람 발생 시간이 경과하지 않았다
                    if (Utils.GetTimestamp() - silentStopStartTime < Config.WaitTime.Etc.SilentStopTimeout.Value.Ticks)
                    {
                        continue;
                    }

                    // 알람 발생중이다
                    var alarmCode = CAlarmDefine.EAlarmList.EF_PREFORM4_MAIN_EQUIPMENT_SILENT_STOP;
                    if (m_objDocument.m_objAlarmList.GetIsOnAlarm(alarmCode) == true)
                    {
                        continue;
                    }
                    // 알람 발생
                    m_objDocument.SetForcedAlarm(alarmCode);
                }
                else
                {
                    if (getMotionStatus != null)
                    {
                        Array.Copy(getMotionStatus, 0, lastMotionStatus, 0, getMotionStatus.Length);
                        silentStopStartTime = TimeSpan.MaxValue.Ticks;
                        getMotionStatus = null;
                    }
                }
            }
        }

        private void ThreadOperationWatchdog()
        {
            // WatchDog 실행대기
            Thread.Sleep(5000);

            while (mbThreadExit == false)
            {
                Thread.Sleep(100);

                if (
                    m_objDocument.IsMasterLogin == true
                    || m_objDocument.GetMainFrame()?.m_objDialogNoOperation == null
                    )
                {
                    continue;
                }

                // Door Open상황
                bool bAllDoorLock = DoorManager.Doors.Values.All(item => item.IsOpened == false)
                                && DoorManager.Doors.Values.All(item => item.IsUnlockPermit == false);
                bool bPlcDoorClose = GetSafetyIO().IsSafetyAllDoorClosed();
                //bool bUpperEqDoorIsOpen = m_objProcessMotion.LoadInterface.CheckUpperEqInterfaceDoorOpened();
                //bool bLowerEqDoorIsOpen = false; // m_objProcessMotion.UnloadInterface.CheckLowerEqInterfaceDoorOpened();
                bool bEnableSwitchGrip = GetSafetyIO().IsAnyEnableSwitchGripped();
                bool bPause =
                    CDefine.ERunStatus.Start != m_objDocument.GetRunStatus()
                    && CDefine.ERunStatus.LoadingStop != m_objDocument.GetRunStatus()
                    && CDefine.ERunStatus.Stopping != m_objDocument.GetRunStatus()
                    ;
                if (
                    (
                    false == bAllDoorLock
                    || false == bPlcDoorClose
                    //|| true == bUpperEqDoorIsOpen
                    //|| true == bLowerEqDoorIsOpen
                    )
                    && true == bPause
                    )
                {
                    // Door Open + 정지 상황
                    // No Operation Dialog가 활성상태일경우 비활성화해준다
                    if (m_objDocument.GetMainFrame()?.m_objDialogNoOperation.Visible == true)
                    {
                        m_objDocument.GetMainFrame().SetDelegateNoOperationDialogShow(false);
                    }

                    // PM창이 꺼져있는 상황에서 Enable Switch가 Grip되지않은경우 PM창을 활성화 해준다
                    if (
                        m_objDocument.GetMainFrame()?.m_objDialogSafety?.Visible == false
                        && bEnableSwitchGrip == false
                        )
                    {
                        m_objDocument.GetMainFrame().SetDelegateSafetyDialogShow();
                    }
                }
                else
                {
                    if (
                        Utils.GetUiInputIdleTime() > Config.WaitTime.Etc.NoOperationDisplayDelay.ToTimeSpan()
                        || IsSamsungLogoClick == true
                        )
                    {
                        IsSamsungLogoClick = false;
                        // PM창이 꺼져있는 상황에서 No Operation 창이 켜져있지 않을경우 활성화 해준다
                        if (
                            m_objDocument.GetMainFrame()?.m_objDialogSafety?.Visible == false
                            && m_objDocument.GetMainFrame()?.m_objDialogNoOperation.Visible == false
                            )
                        {
                            m_objDocument.GetMainFrame().SetDelegateNoOperationDialogShow(true);
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        private void ThreadMaterialsManagement()
        {
            while (mbThreadExit == false)
            {
                DoProcessMaterialsManagement();
                Thread.Sleep(500);
            }
        }

        private void ThreadLowEnergyModeProcess()
        {
            long lowEnergyModeStartTime = 0;

            while (mbThreadExit == false)
            {
                Thread.Sleep(500);

                bool bIsUseLowEnergyMode = m_objDocument.m_objConfig.GetOptionParameter().UseLowEnergyMode;
                if (bIsUseLowEnergyMode == false)
                {
                    continue;
                }

                if (m_objFFU[CDefine.EMcu.MainGruop].HLGetRequiredSpeed(0, 0, out int ffuCurrentSettingSpeed) == false)
                {
                    continue;
                }

                bool bIsRunning = m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.EMoveState.MOVE_STATE_RUNNING;
                bool bIsExistCell = m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.ERunState.RUN_STATE_RUN;
                if (bIsRunning == true
                    && bIsExistCell == true
                    )
                {
                    if (ffuCurrentSettingSpeed != m_objDocument.m_objConfig.GetOptionParameter().LowEnergyModeFfuNormalRpm)
                    {
                        DoProcessNormalEnergyMode();
                        lowEnergyModeStartTime = 0;
                    }
                    continue;
                }

                if (lowEnergyModeStartTime == 0)
                {
                    lowEnergyModeStartTime = Utils.GetTimestamp();
                }
                long lowEnergyModeChangeDelay = Config.WaitTime.Etc.LowEnergyModeChangeDelay.Value.Ticks;
                if (Utils.GetTimestamp() - lowEnergyModeStartTime < lowEnergyModeChangeDelay)
                {
                    if (ffuCurrentSettingSpeed != m_objDocument.m_objConfig.GetOptionParameter().LowEnergyModeFfuNormalRpm)
                    {
                        DoProcessNormalEnergyMode();
                    }
                    continue;
                }

                if (ffuCurrentSettingSpeed != m_objDocument.m_objConfig.GetOptionParameter().LowEnergyModeFfuSlowRpm)
                {
                    DoProcessLowEnergyMode();
                }
            }
        }

        private void ThreadEquipmentButtonLog_SignalChanged(object sender, Utils.IDigitalSignalObserverItem e)
        {
            // 초기화시 입력한 Tag 문자열 배열 값을 기준으로 로그를 남길지 말지 판단함 (내용 있음=로그 기록, 공백=무시)
            var status = (string[])e.Tag;
            if (e.LastSignal == true
                && string.IsNullOrWhiteSpace(status[0]) == false
                )
            {
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_BUTTON_OPERATION, string.Join("_", e.Comment, status[0]));
            }
            else if (e.LastSignal == false
                && string.IsNullOrWhiteSpace(status[1]) == false
                )
            {
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_BUTTON_OPERATION, string.Join("_", e.Comment, status[1]));
            }
        }
    }
}

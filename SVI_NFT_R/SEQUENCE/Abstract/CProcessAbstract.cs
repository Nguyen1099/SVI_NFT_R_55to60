using HLDevice;
using HLDevice.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SVI_NFT_R
{
    public abstract class CProcessAbstract
    {
        /// <summary>
        /// 시퀀스 프로세스 매니저 초기화 정의
        /// </summary>
        public enum EProcessManagerInitialize
        {
            /// <summary>
            /// NONE
            /// </summary>
            PROCESS_MANAGER_INITIALIZE_NONE = 0,
            /// <summary>
            /// 초기화 시작
            /// </summary>
            PROCESS_MANAGER_INITIALIZE_START,
            /// <summary>
            /// 초기화 완료
            /// </summary>
            PROCESS_MANAGER_INITIALIZE_DONE,
            /// <summary>
            /// 초기화 에러
            /// </summary>
            PROCESS_MANAGER_INITIALIZE_ERROR,
            PROCESS_MANAGER_INITIALIZE_FINAL
        };
        /// <summary>
        /// 시퀀스 프로세스 매니저 재시작 정의
        /// </summary>
        public enum EProcessManagerRestart
        {
            /// <summary>
            /// NONE
            /// </summary>
            PROCESS_MANAGER_RESTART_NONE = 0,
            /// <summary>
            /// 재시작 프로세스 시작
            /// </summary>
            PROCESS_MANAGER_RESTART_START,
            /// <summary>
            /// 재시작 프로세스 완료
            /// </summary>
            PROCESS_MANAGER_RESTART_DONE,
            /// <summary>
            /// 재시작 프로세스 에러
            /// </summary>
            PROCESS_MANAGER_RESTART_ERROR
        }
        /// <summary>
        /// 사이클 택타임 로그를 썻는지 확인하는 플래그
        /// </summary>
        /// <![CDATA[                                                                                                                            
        /// 로딩이 패시브인 유닛에서는 액티브 측에서 유닛에서 로그를 찍는데 로그를 쓰기전에 액티브측
        /// 로딩 시퀀스에 들어가게되면 로그가 누락되는 현상이 있어서 추가함
        /// ]]>
        public bool IsCompleteWriteCycleLog { get; protected set; } = false;
        /// <summary>
        /// 시퀀스가 진행중인지 확인하는 플래그
        /// </summary>
        public bool IsRunningSequence { get; protected set; }
        /// <summary>
        /// Document 선언
        /// </summary>
        public CDocument m_objDocument;
        /// <summary>
        /// IO 선언
        /// </summary>
        public CDeviceIO m_objIO;
        /// <summary>
        /// 설정 선언
        /// </summary>
        public CConfig m_objConfig;
        /// <summary>
        /// 통합 IO 하드웨어
        /// </summary>
        public IoHardware IoHardware { get; protected set; }
        /// <summary>
        /// CCLink 인터페이스
        /// </summary>
        public CDeviceInterface m_objCCLinkVer2;

        /// <summary>
        /// FTP
        /// </summary>
        public CDeviceFileInterface m_objFileInterface;

        /// <summary>
        /// Vision Interface
        /// </summary>
        public Dictionary<CDefine.EInspInterface, CDeviceSviInterface> m_objInspInterfaces;

        /// <summary>
        /// 저항 측정기
        /// </summary>
        public Dictionary<CDefine.EGms, CDeviceElectrostatic> m_objElectrostatic;

        /// <summary>
        /// FFU
        /// </summary>
        public Dictionary<CDefine.EMcu, CDeviceFFU> m_objFFU;

        /// <summary>
        /// Temperature
        /// </summary>
        public Dictionary<CDefine.ETemperatureController, CDeviceTemperature> m_objTemperature;

        /// <summary>
        /// Power Meter
        /// </summary>
        public Dictionary<CDefine.EPowerMeter, CDevicePowerMeter> m_objPowerMeter;

        /// <summary>
        /// MCR
        /// </summary>
        public Dictionary<CDefine.EMcr, CDeviceMCR> m_objMcr;
        /// <summary>
        /// Align Interface
        /// </summary>
        public Dictionary<CDefine.EAlign, CDeviceAlignInterface> m_objAlignInterfaces;
        /// <summary>
        /// 각도 센서 통신 객체
        /// </summary>
        public Dictionary<CDefine.EAngularSensor, CDeviceAngleSensor> m_objAngleSensor;

        /// <summary>
        /// Measurement 통신 객체
        /// </summary>
        public Dictionary<CDefine.EMeasurement, Device.Measurement.IDeviceMeasurement> m_objMeasurementInterfaces;
        /// <summary>
        /// 세이프티PLC 통신 객체
        /// </summary>
        public Dictionary<CDefine.ESafetyPlc, CDeviceSafetyPlc> m_objSafetyPlcs;
        /// <summary>
        /// 인터락 객체
        /// </summary>
        public CProcessInterlock m_objInterlock;
        /// <summary>
        /// Nachi 통신 객체
        /// </summary>
        public CDeviceNachiInterface[] m_objNachiInterface;
        /// <summary>
        /// 배출 택타임 타이머
        /// </summary>
        private Stopwatch m_swOutTactTime = new Stopwatch();
        /// <summary>
        /// 유닛 포지션
        /// </summary>
        protected CDefine.EPosition m_ePosition;
        /// <summary>
        /// 유닛 포지션 이름
        /// </summary>
        protected string m_strPosition;
        /// <summary>
        /// 프로세스 종료
        /// </summary>
        protected bool mbThreadExit;
        /// <summary>
        /// 초기화 쓰레드
        /// </summary>
        protected Thread mThreadInitialize;
        /// <summary>
        /// 설비 프로세스 쓰레드
        /// </summary>
        protected Thread mThreadProcess;
        /// <summary>
        /// 재시작 동작 스레드
        /// </summary>
        protected Thread mThreadRestart;
        /// <summary>
        /// 알람 구조체
        /// </summary>
        protected CDefine.structureAlarmInformation m_objAlarmStructure;
        /// <summary>
        /// 시퀀스 프로레스 매니저 초기화 상태
        /// </summary>
        protected EProcessManagerInitialize m_eProcessManagerInitialize;
        /// <summary>
        /// 재시작 동작 상태
        /// </summary>
        protected EProcessManagerRestart m_eProcessManagerRestart;
        /// <summary>
        /// 구동 속도
        /// </summary>
        protected CDeviceMotorAbstract.EVelocityMode m_eMoveVelocityType;
        /// <summary>
        /// 프로세스 완료 대기 체크 간격
        /// </summary>
        protected const int WAIT_FOR_END_PROCESS_PERIOD_TIME = 1;
        public List<Mcc.IMccLogItem> MccLogItemsWhenJobsBegin { get; private set; } = new List<Mcc.IMccLogItem>();
        public List<Mcc.IMccLogItem> MccLogItemsWhenJobsFinish { get; private set; } = new List<Mcc.IMccLogItem>();
        private bool mbCanBootUpAction = true;

        /// <summary>
        /// 생성자
        /// </summary>
        public CProcessAbstract()
        {
            m_ePosition = CDefine.EPosition.A;
            m_strPosition = "A";

            mbThreadExit = false;
            mThreadInitialize = null;
            mThreadProcess = null;
            mThreadRestart = null;
            m_objInterlock = null;

            m_eProcessManagerInitialize = EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE;
            m_eProcessManagerRestart = EProcessManagerRestart.PROCESS_MANAGER_RESTART_NONE;

            m_objAlarmStructure = new CDefine.structureAlarmInformation();
            m_objAlarmStructure.strAlarmObject = typeof(CProcessAbstract).Name;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.strAlarmDescription = "";
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="document">CDocument의 객체</param>
        /// <param name="position">[옵션]유닛 위치</param>
        /// <param name="args"></param>
        /// <returns>초기화 성공 여부</returns>
        public abstract bool Initialize(CDocument document, int position = 0, params object[] args);

        /// <summary>
        /// 해제
        /// </summary>
        public abstract void DeInitialize();


        /// <summary>
        /// 매니저 초기화 후 1회 실행되는 동작 실행
        /// </summary>
        public void BootUpAction()
        {
            Debug.Assert(mbCanBootUpAction, "BootUpAction은 한번만 호출되어야 합니다.");
            mbCanBootUpAction = false;
            OnBootUpAction();
        }

        /// <summary>
        /// 호출하는 즉시 등록된 MCC 로그의 시작과 끝을 강제로 쓰고 MCC 로그 리스트에서 삭제한다
        /// </summary>
        /// <remarks>
        /// 우리 설비는 진공 택타임을 줄이기 위해 설정에 따라 Pick&Place 진행시 진공 압력이 모두 올라오지 않은 상태에서 업 동작이 실행 될 수 있어서
        /// MCC 차트가 비정상 적으로 보일 수 있기 때문에 단순히 차트를 깔끔하게 만들기 위해 사용 될 수 있음.
        /// </remarks>
        public void ForceWriteRegistMccLog()
        {
#if !RAW_MCC
            RaiseMccLogWhenJobsBegined();
            RaiseMccLogWhenJobsFinished();
#endif
        }

        /// <summary>
        /// 매니저 초기화 후 1회 실행되는 부팅 동작 정의
        /// </summary>
        /// <remarks>부팅시에 실행해야 할 동작이 있는데 그 동작이 다른 모션에 영향 없이 동시에 실행 할 수 있고, 처리 시간이 오래 걸릴 경우 이 함수를 재정의해서 사용한다.</remarks>
        protected virtual void OnBootUpAction()
        {
            // DoNothing
        }

        /// <summary>
        /// 홈 동작 진행
        /// </summary>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool SetHome()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;

            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 대기 위치 이동
        /// </summary>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool SetWaitPosition()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 로드 위치 이동
        /// </summary>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool SetLoadPosition()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 언로드 위치 이동
        /// </summary>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool SetUnloadPosition()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 제품 픽업
        /// </summary>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool SetPick()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 제품 플레이스
        /// </summary>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool SetPlace()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_objAlarmStructure.strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 매니저 클래스 초기화 커맨드 설정
        /// </summary>
        /// <param name="eProcessManagerInitialize">초기화 커맨드</param>
        public void SetInitializeCommand(EProcessManagerInitialize eProcessManagerInitialize)
        {
            m_eProcessManagerInitialize = eProcessManagerInitialize;
        }

        /// <summary>
        /// 매니저 클래스 초기화 상태 확인
        /// </summary>
        /// <returns>초기화 상태</returns>
        public EProcessManagerInitialize GetInitializeStatus()
        {
            return m_eProcessManagerInitialize;
        }

        /// <summary>
        /// 재시작 동작 커맨드 설정
        /// </summary>
        /// <param name="eProcessManagerRestart">재시작 커맨드</param>
        public void SetRestartCommand(EProcessManagerRestart eProcessManagerRestart)
        {
            m_eProcessManagerRestart = eProcessManagerRestart;
        }

        /// <summary>
        /// 재시작 동작 상태 확인
        /// </summary>
        /// <returns>재시작 상태</returns>
        public EProcessManagerRestart GetRestartStatus()
        {
            return m_eProcessManagerRestart;
        }

        /// <summary>
        /// 배출 택 타임 새로 시작
        /// </summary>
        public void SetOutTactTimeStartNew()
        {
            m_swOutTactTime = Stopwatch.StartNew();
        }

        /// <summary>
        /// 배출 택 타임 경과 시간
        /// </summary>
        /// <returns>배출 택 타임 경과 시간 (Unit: Seconds)</returns>
        public TimeSpan GetOutTactTimeElapsed()
        {
            return m_swOutTactTime.Elapsed;
        }

        /// <summary>
        /// 배출 택타임 타이머 정지
        /// </summary>
        protected void SetOutTactTimeStop()
        {
            m_swOutTactTime.Stop();
        }

        /// <summary>
        /// 배출 택타임 타이머 시작
        /// </summary>
        protected void SetOutTactTimeStart()
        {
            m_swOutTactTime.Start();
        }

        /// <summary>
        /// 홈 동작 완료 상태 대기 동작
        /// </summary>
        /// <param name="objMotor">CDeviceMotor의 객체</param>
        /// <param name="iTimeOut">[옵션] 타임 아웃 (Unit: MiliSeconds)</param>
        /// <param name="iSleepPeriod">[옵션] 체크 간격 (Unit: MiliSeconds)</param>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool WaitHomeComplete(HLDevice.CDeviceMotor objMotor, int iTimeOut = 60000, int iSleepPeriod = 10)
        {
            bool bReturn = false;
            bool bCurrentResult = false;
            int iHomeMainStepNo = 0;
            int iHomeStepNo = 0;
            do
            {
                // Invalid Pointer Check
                if (null == objMotor)
                {
                    break;
                }

                // 홈 진행률 확인
                objMotor.HLGetHomeProcessRate(ref iHomeMainStepNo, ref iHomeStepNo);
                // 홈 완료 상태 확인
                while (
                    0 < iTimeOut
                    && 100 != iHomeStepNo
                    )
                {
                    Thread.Sleep(iSleepPeriod);
                    iTimeOut -= iSleepPeriod;
                    // 홈 진행률 확인
                    objMotor.HLGetHomeProcessRate(ref iHomeMainStepNo, ref iHomeStepNo);
                }

                // Timeout Check
                if (0 >= iTimeOut)
                {
                    break;
                }

                // 초기화 실패
                bCurrentResult = objMotor.HLGetMotorStatus().bHome;
                if (bCurrentResult == false)
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 멜섹 비트 결과 확인 대기 동작
        /// </summary>
        /// <param name="objInterface">CDeviceInterface의 객체</param>
        /// <param name="strAddressName">대기할 Bit의 주소</param>
        /// <param name="bStatus">대기할 신호 상태</param>
        /// <param name="iTimeOut">[옵션] 타임 아웃 (Unit: MiliSeconds)</param>
        /// <param name="iSleepPeriod">[옵션] 체크 간격 (Unit: MiliSeconds)</param>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool WaitMelsecBit(HLDevice.CDeviceInterface objInterface, string strAddressName, bool bStatus, int iTimeOut = 20000, int iSleepPeriod = 5)
        {
            bool bReturn = false;
            bool bResult = false;
            do
            {
                if (null == objInterface)
                    break;

                // 멜섹 비트 리드
                HLDevice.CDeviceInterface.EError eError;
                eError = objInterface.HLGetInterfaceBit(strAddressName, out bResult);

                if (HLDevice.CDeviceInterface.EError.ERROR_NONE == eError)
                {
                    while (0 < iTimeOut && bResult != bStatus)
                    {
                        Thread.Sleep(iSleepPeriod);
                        iTimeOut -= iSleepPeriod;
                        // 멜섹 비트 리드
                        if (HLDevice.CDeviceInterface.EError.ERROR_NONE != objInterface.HLGetInterfaceBit(strAddressName, out bResult))
                            break;
                    }
                }
                else if (HLDevice.CDeviceInterface.EError.ERROR_LICENSE == eError)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LISENCE_KEY_RECOGNITION_FAILED);
                    break;
                }
                else
                {
                    break;
                }

                // Timeout Check
                if (0 >= iTimeOut)
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DIO 비트 결과 확인
        /// </summary>
        /// <param name="objIO">CDeviceIO의 객체</param>
        /// <param name="strAddressName">대기할 Bit의 주소</param>
        /// <param name="bStatus">대기할 신호 상태</param>
        /// <param name="iTimeOut">[옵션] 타임 아웃 (Unit: MiliSeconds)</param>
        /// <param name="iSleepPeriod">[옵션] 체크 간격 (Unit: MiliSeconds)</param>
        /// <returns>동작 성공 여부</returns>
        protected virtual bool WaitDigitalBit(HLDevice.CDeviceIO objIO, string strAddressName, bool bStatus, int iTimeOut = 20000, int iSleepPeriod = 5)
        {
            bool bReturn = false;
            bool bResult = false;
            do
            {
                if (null == objIO)
                    break;

                // IO 비트 리드
                if (true == objIO.HLGetDigitalBit(strAddressName, ref bResult))
                {
                    while (0 < iTimeOut && bResult != bStatus)
                    {
                        Thread.Sleep(iSleepPeriod);
                        iTimeOut -= iSleepPeriod;
                        // IO 비트 리드
                        objIO.HLGetDigitalBit(strAddressName, ref bResult);
                    }
                }
                else
                {
                    break;
                }

                // Timeout Check
                if (0 >= iTimeOut)
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 속도 모드 설정
        /// </summary>
        protected void SetVelocityMode()
        {
            if (CDefine.EMoveModeType.MOVE_MODE_TYPE_AUTO == m_objDocument.GetMoveModeType())
            {
                m_eMoveVelocityType = CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_AUTO_RUN;
            }
            else if (CDefine.EMoveModeType.MOVE_MODE_TYPE_MANUAL == m_objDocument.GetMoveModeType())
            {
                m_eMoveVelocityType = CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_MANUAL_RUN;
            }
        }

        /// <summary>
        /// 동작 완료 후 MCC 완료 메시지 작성 이벤트 발생
        /// </summary>
        protected void RaiseMccLogWhenJobsBegined()
        {
            lock (MccLogItemsWhenJobsBegin)
            {
                // 동작 시작전 MCC 로그 업데이트
                for (int i = 0; i < MccLogItemsWhenJobsBegin.Count; i++)
                {
                    MccLogItemsWhenJobsBegin[i].WriteStart();
                }
                MccLogItemsWhenJobsBegin.Clear();
            }
        }

        /// <summary>
        /// 동작 완료 후 MCC 완료 메시지 작성 이벤트 발생
        /// </summary>
        protected void RaiseMccLogWhenJobsFinished()
        {
            lock (MccLogItemsWhenJobsFinish)
            {
                // 동작 종료 후 MCC 로그 업데이트
                for (int i = 0; i < MccLogItemsWhenJobsFinish.Count; i++)
                {
                    MccLogItemsWhenJobsFinish[i].WriteEnd();
                }
                MccLogItemsWhenJobsFinish.Clear();
            }
        }

        /// <summary>
        /// 동작의 MCC 로그 아이템을 등록함
        /// </summary>
        /// <param name="mccLogItem">MCC 로그 아이템</param>
        public void SetMccLogItem(Mcc.IMccLogItem mccLogItem)
        {
            MccLogItemsWhenJobsBegin.Add(mccLogItem);
            MccLogItemsWhenJobsFinish.Add(mccLogItem);
        }

        public void SetMccLogItem(Mcc.IMccLogItem[] mccLogItems)
        {
            MccLogItemsWhenJobsBegin.AddRange(mccLogItems);
            MccLogItemsWhenJobsFinish.AddRange(mccLogItems);
        }

        public void SetMccLogItemExist(Mcc.IMccLogItem[] mccLogItems)
        {
            var existList = mccLogItems.Where(item => item.IsExist).ToArray();
            MccLogItemsWhenJobsBegin.AddRange(existList);
            MccLogItemsWhenJobsFinish.AddRange(existList);
        }

        /// <summary>
        /// 동작의 완료 시간과 별개로 동작중 동기를 맞춰야 하는경우 재정의해서 사용함
        /// (예 : 베큠 블로우, ...)
        /// </summary>
        /// <param name="syncTypeOrNull">동기를 맞춰야 할 구간이 다수 일 때 구분자로 사용함</param>
        /// <returns>항상 true를 반환하게 구현함. if 문에서 사용하기 위해서 bool 결과값을 반환함</returns>
        public virtual bool WaitForCommandSync(object syncTypeOrNull = null)
        {
            return true;
        }

        /// <summary>
        /// 동작 완료 체크
        /// </summary>
        /// <returns>true=동작 성공, false=동작 실패</returns>
        public virtual bool WaitForEndProcess()
        {
            return true;
        }
    }

    public static partial class ExtentionMethods
    {
        public static bool WaitForEndProcess(this CProcessAbstract[] waitProcess)
        {
            bool bResult = true;
            foreach (var process in waitProcess)
            {
                if (false == process.WaitForEndProcess())
                {
                    bResult = false;
                    break;
                }
            }
            return bResult;
        }

        public static void ForEach<T>(this T[] waitProcess, Action<T> action)
            where T : CProcessAbstract
        {
            foreach (var process in waitProcess)
            {
                action.Invoke(process);
            }
        }
    }
}
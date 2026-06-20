using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

/// <![CDATA[
/// 진공 제어 논리표
/// [  Status  ][          Signal          ]
/// |          ||[VacOn ]|[VacOff]|[Blow  ]|
/// ========================================
/// | NotUse   || OFF    | OFF    | OFF    |
/// | VacOn    || ON     | OFF    | OFF    |
/// | VacOff   || OFF    | ON     | OFF    |
/// | Blow     || OFF    | OFF    | ON     |
/// ]]>

namespace SVI_NFT_R
{
    /// <summary>
    /// 진공 추상화 클래스
    /// </summary>
    public abstract class CVacuumAbstract
    {
        /// <summary>
        /// 진공 동작 커맨드
        /// </summary>
        public enum EVacuumCommand
        {
            CMD_ON = 0,
            CMD_OFF,
            CMD_BLOW,
            CMD_UNKNOWN
        }
        /// <summary>
        /// 진공 감지 상태
        /// </summary>
        public enum EVacuumStatus
        {
            STS_ON = 0,
            STS_OFF,
            STS_TRANSITION,
            STS_UNKNOWN
        }
        /// <summary>
        /// 진공 구동 타입 ( 단동, 복동 )
        /// </summary>
        public enum EVacuumSolenoidType
        {
            UNKNOWN_SOLENOID = 0,
            SINGLE_SOLENOID,
            DOUBLE_SOLENOID,
            TRIPLE_SOLENOID,
            QUARD_SOLENOID
        }
        /// <summary>
        /// 진공 ON 출력 갯수
        /// </summary>
        public enum EVacuumOnOutputIO
        {
            ON_OUTPUT_IO_1 = 0,
            ON_OUTPUT_IO_2,
            ON_OUTPUT_IO_3,
            ON_OUTPUT_IO_4,
            ON_OUTPUT_IO_FINAL
        }
        /// <summary>
        /// 진공 OFF 출력 갯수
        /// </summary>
        public enum EVacuumOffOutputIO
        {
            OFF_OUTPUT_IO_1 = 0,
            OFF_OUTPUT_IO_2,
            OFF_OUTPUT_IO_3,
            OFF_OUTPUT_IO_4,
            OFF_OUTPUT_IO_FINAL
        }
        /// <summary>
        /// 진공 입력 갯수
        /// </summary>
        public enum EVacuumInputIO
        {
            INPUT_IO_1 = 0,
            INPUT_IO_2,
            INPUT_IO_3,
            INPUT_IO_4,
            INPUT_IO_FINAL
        }
        /// <summary>
        /// 진공 센서 체크 실행 여부
        /// </summary>
        public enum ESensorCheck
        {
            /// <summary>
            /// 센서 무시
            /// </summary>
            IGNORE,
            /// <summary>
            /// 센서 체크 실행
            /// </summary>
            CHECK
        }

        /// <summary>
        /// 진공 동작 초기화 파라미터
        /// </summary>
        [Serializable]
        public class CVacuumInitializeParameter
        {
            /// <summary>
            /// 진공 동작 파라미터 파일 경로
            /// </summary>
            public string strFilePath;
            /// <summary>
            /// 진공 동작 파라미터 파일 이름
            /// </summary>
            public string strFileName;
            /// <summary>
            /// 진공 이름
            /// </summary>
            public string strVacuumName;
            /// <summary>
            /// 진공 ON 출력 IO
            /// </summary>
            public string[] strVacuumOnOutputIO;
            /// <summary>
            /// 진공 OFF 출력 IO (옵션)
            /// </summary>
            public string[] strVacuumOffOutputIO;
            /// <summary>
            /// 파기 ON 출력 IO
            /// </summary>
            public string[] strVacuumBlowOutputIO;
            /// <summary>
            /// 입력 IO
            /// </summary>
            public string[] strVacuumInputIO;
            /// <summary>
            /// 아날로그 입력 IO
            /// </summary>
            public string[] strVacuumAnalogInputIO;
            /// <summary>
            /// 동작 타입
            /// </summary>
            public EVacuumSolenoidType eVacuumSolenoidType;
            /// <summary>
            /// 성공으로 인지 할 베큠의 개수를 설정함 (1 보다 작은 값 입력시 모든 베큠을 체크하겠다는 의미)
            /// </summary>
            public int iSuccessVacuumDetectCount;
            /// <summary>
            /// 베큠 사용 여부 옵션
            /// </summary>
            public List<HashSet<int>> VacuumUseConfigurations = new List<HashSet<int>>();
            public string[] ExtentionOutputIoNames = new string[0];
            public Action<CVacuumAbstract> BeforeVacuum;
            public Action<CVacuumAbstract> AfterVacuum;
            public Action<CVacuumAbstract> BeforeBlow;
            public Action<CVacuumAbstract> AfterBlow;
            public Action<CVacuumAbstract> BeforeOff;
            public Action<CVacuumAbstract> AfterOff;

            public CVacuumInitializeParameter()
            {
                strFilePath = "";
                strFileName = "";
                strVacuumName = "";
                strVacuumOnOutputIO = new string[(int)EVacuumOnOutputIO.ON_OUTPUT_IO_FINAL];
                strVacuumOffOutputIO = new string[(int)EVacuumOnOutputIO.ON_OUTPUT_IO_FINAL];
                strVacuumBlowOutputIO = new string[(int)EVacuumOffOutputIO.OFF_OUTPUT_IO_FINAL];
                strVacuumInputIO = new string[(int)EVacuumInputIO.INPUT_IO_FINAL];
                strVacuumAnalogInputIO = new string[(int)EVacuumInputIO.INPUT_IO_FINAL];
                eVacuumSolenoidType = EVacuumSolenoidType.UNKNOWN_SOLENOID;
                iSuccessVacuumDetectCount = -1;

                for (int iLoopCount = 0; iLoopCount < (int)EVacuumOnOutputIO.ON_OUTPUT_IO_FINAL; iLoopCount++)
                {
                    strVacuumOnOutputIO[iLoopCount] = "";
                    strVacuumOffOutputIO[iLoopCount] = "";
                    strVacuumBlowOutputIO[iLoopCount] = "";
                }
                for (int iLoopCount = 0; iLoopCount < (int)EVacuumInputIO.INPUT_IO_FINAL; iLoopCount++)
                {
                    strVacuumInputIO[iLoopCount] = "";
                    strVacuumAnalogInputIO[iLoopCount] = "";
                }
            }

            /// <summary>
            /// 옵션에 인덱스를 Bit Flag로 솔레이노이드 사용/미사용을 설정 할 수 있도록 셋팅함
            /// </summary>
            public void MakeConfigurationsToFlagType()
            {
                Debug.Assert(eVacuumSolenoidType != EVacuumSolenoidType.UNKNOWN_SOLENOID, "솔레노이드 개수를 설정한 뒤 실행해야합니다.");
                Debug.Assert(VacuumUseConfigurations.Count == 0, "이미 옵션이 등록되있습니다.");

                // 모든 경우에 수를 옵션 리스트에 등록함
                int solenoidCount = (int)eVacuumSolenoidType;
                int configurationCount = Convert.ToInt32(Math.Pow(2, solenoidCount));
                VacuumUseConfigurations.Clear();
                for (int i = 0; i < configurationCount; i++)
                {
                    HashSet<int> addItem = new HashSet<int>();
                    for (int k = 0; k < solenoidCount; k++)
                    {
                        if (((i >> k) & 0x01) != 0x01)
                        {
                            continue;
                        }
                        addItem.Add(k);
                    }
                    VacuumUseConfigurations.Add(addItem);
                }
            }
        }

        /// <summary>
        /// 진공 동작시 필요 파라미터
        /// </summary>
        [Serializable]
        public class CVacuumDataParameter
        {
            /// <summary>
            /// 진공 동작 타임아웃 시간
            /// </summary>
            public int iVacuumTimeOut;
            /// <summary>
            /// 진공 Blow 유지 시간
            /// </summary>
            public int iVacuumOffDelayTime;
            /// <summary>
            /// 솔레노이드가 꺼질때까지 딜레이 (Range: 0ms ~ 50ms)
            /// </summary>
            /// <![CDATA[
            /// SMC사에서 이론상 상태 전환에 38ms가 걸리고 기타 변수를 고려하여도 50ms는 '절대' 넘지 않는다고 함.
            /// ]]>
            public int SolenoidDelayTime;
            public const int SOLENOID_DELAY_MIN = 0;
            public const int SOLENOID_DELAY_MAX = 50;
        }
        public EVacuumStatus Status
        {
            get
            {
                EVacuumStatus status = default(EVacuumStatus);
                GetVacuumStatus(ref status);
                return status;
            }
        }
        public EVacuumCommand Command
        {
            get
            {
                return GetVacuumCommand();
            }
        }

        private string m_strAlarmFunction = "";
        /// <summary>
        /// 진공 신호 설정 시간
        /// </summary>
        public double VacuumSettingTime { get; private set; } = 0d;
        /// <summary>
        /// 블로우 신호 설정 시간
        /// </summary>
        public double BlowSettingTime { get; private set; } = 0d;
        /// <summary>
        /// 센서 ON 감지 시간
        /// </summary>
        public double VacuumSensorOnTime { get; private set; } = 0d;
        /// <summary>
        /// 센서 OFF 감지 시간
        /// </summary>
        public double VacuumSensorOffTime { get; private set; } = 0d;

        /// <summary>
        /// 옵션 선택 (필수 사항은 아님 / 모델에 따라 베큠 구성이 바뀌거나, 기타 이유로 베큠 구성을 바꿔가면서 사용해야 할 때 사용함)
        /// </summary>
        private int mOptionIndex = int.MinValue;
        public int OptionIndex
        {
            set
            {
                Utils.InRange(ref value, -1, m_objVacuumInitializeParameter.VacuumUseConfigurations.Count);
                if (mOptionIndex != value)
                {
                    mOptionIndex = value;
                    // + 옵션이 변경 되었을 때 미사용하는 솔레노이드가 켜져 있으면 꺼줌
                    NotUseSolenoidOff();

                    // 옵션 변경시 진공 솔레노이드 인덱스를 업데이트함
                    int solenoidCount = (int)m_objVacuumInitializeParameter.eVacuumSolenoidType;
                    if (m_objVacuumInitializeParameter.VacuumUseConfigurations.Count == 0
                        || mOptionIndex == -1
                        )
                    {
                        mSolenoidIndexes = Enumerable.Range(0, solenoidCount)
                            .ToArray();
                    }
                    else
                    {
                        mSolenoidIndexes = Enumerable.Range(0, solenoidCount)
                            .Where(i => m_objVacuumInitializeParameter.VacuumUseConfigurations[mOptionIndex].Contains(i))
                            .ToArray();
                    }

                    // 진공 끄기 신호를 하나라도 가지고 있는지 업데이트
                    mbHasVacuumOffSignal = m_objVacuumInitializeParameter.strVacuumOffOutputIO.Any(i => string.IsNullOrWhiteSpace(i) == false);
                }
            }
            get
            {
                return mOptionIndex;
            }
        }
        private int[] mSolenoidIndexes;
        private bool mbHasVacuumOffSignal;

        /// <summary>
        /// IO 객체 선언
        /// </summary>
        protected HLDevice.CDeviceIO m_objIO;
        /// <summary>
        /// 초기화 파라미터 선언
        /// </summary>
        protected CVacuumInitializeParameter m_objVacuumInitializeParameter;

        protected CVacuumDataParameter m_objVacuumDataParameter;

        protected CDocument m_objDocument;

        private DateTime mVacuumSettingStartTime = DateTime.Now;
        private DateTime mBlowSettingStartTime = DateTime.Now;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        public CVacuumAbstract(CDocument objDocument)
        {
            m_objDocument = objDocument;
        }

        /// <summary>
        /// 진공 초기화 함수
        /// </summary>
        /// <param name="objIO"></param>
        /// <param name="objVacuumInitialize"></param>
        /// <returns></returns>
        public abstract bool Initialize(HLDevice.CDeviceIO objIO, CVacuumInitializeParameter objVacuumInitialize);

        /// <summary>
        /// 해제
        /// </summary>
        public abstract void DeInitialize();

        /// <summary>
        /// 진공 커맨드 수행
        /// </summary>
        /// <param name="eCommand"></param>
        /// <param name="eSensorIgnore"></param>
        /// <returns></returns>
        public virtual bool SetVacuumCommand(EVacuumCommand eCommand, ESensorCheck eSensorIgnore = ESensorCheck.CHECK)
        {
            m_strAlarmFunction = "SetVacuumCommand";
            var stopwatch = new Stopwatch();

            switch (eCommand)
            {
                case EVacuumCommand.CMD_ON:
                    if (GetVacuumCommand() != eCommand)
                    {
                        // + 실제 동작전 사용자가 임의로 사용하지 않는 솔레노이드를 켯을 때 꺼줌
                        NotUseSolenoidOff();

                        m_objVacuumInitializeParameter.BeforeVacuum?.Invoke(this);
                        stopwatch.Restart();
                        if (SetVacuumOn() == false)
                        {
                            return false;
                        }
                        stopwatch.Stop();
                        VacuumSettingTime = stopwatch.Elapsed.TotalMilliseconds;
                        m_objVacuumInitializeParameter.AfterVacuum?.Invoke(this);
                    }
                    if (eSensorIgnore == ESensorCheck.CHECK)
                    {
                        stopwatch.Restart();
                        // 진공 감지 완료 대기
                        if (WaitForComplete(EVacuumStatus.STS_ON) == false)
                        {
                            return false;
                        }
                        stopwatch.Stop();
                        VacuumSensorOnTime = stopwatch.Elapsed.TotalMilliseconds;
                    }
                    break;

                case EVacuumCommand.CMD_BLOW:
                    if (GetVacuumCommand() != eCommand)
                    {
                        // + 실제 동작전 사용자가 임의로 사용하지 않는 솔레노이드를 켯을 때 꺼줌
                        NotUseSolenoidOff();

                        m_objVacuumInitializeParameter.BeforeBlow?.Invoke(this);
                        stopwatch.Restart();
                        if (SetVacuumBlow() == false)
                        {
                            return false;
                        }
                        stopwatch.Stop();
                        BlowSettingTime = stopwatch.Elapsed.TotalMilliseconds;
                        m_objVacuumInitializeParameter.AfterBlow?.Invoke(this);
                    }
                    if (eSensorIgnore == ESensorCheck.CHECK)
                    {
                        stopwatch.Restart();
                        // 블로우일 경우 모든 신호가 OFF 될 때까지 대기한다. 기존 루틴의 경우 하나라도 OFF되면 루틴을 빠져나왔기 때문에 잔압이 남을 경우 셀이 떨어지지 않는 현상이 나타나 개선함.
                        if (WaitForComplete(EVacuumStatus.STS_OFF) == false)
                        {
                            return false;
                        }
                        // 진공 Blow 완료 후 안정화 시간 대기
                        DateTime timeout = DateTime.Now.AddMilliseconds(m_objVacuumDataParameter.iVacuumOffDelayTime);
                        while (DateTime.Now <= timeout)
                        {
                            Thread.Sleep(1);
                        }
                        stopwatch.Stop();
                        VacuumSensorOffTime = stopwatch.Elapsed.TotalMilliseconds;
                    }
                    break;

                case EVacuumCommand.CMD_OFF:
                    if (GetVacuumCommand() != eCommand)
                    {
                        // + 실제 동작전 사용자가 임의로 사용하지 않는 솔레노이드를 켯을 때 꺼줌
                        NotUseSolenoidOff();

                        m_objVacuumInitializeParameter.BeforeOff?.Invoke(this);
                        if (SetVacuumOff() == false)
                        {
                            return false;
                        }
                        m_objVacuumInitializeParameter.AfterOff?.Invoke(this);
                    }
                    // OFF 일경우 센서 체크 하지 않음 Sol신호만 OFF 시켜준다.
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            return true;
        }

        /// <summary>
        /// 커맨드 반환
        /// </summary>
        /// <returns></returns>
        public virtual EVacuumCommand GetVacuumCommand()
        {
            m_strAlarmFunction = "GetVacuumCommand";
            int[] solenoidIndexes = mSolenoidIndexes;
            int vacuumOnOutputCount = 0;
            int vacuumOffOutputCount = 0;
            int blowOutputCount = 0;
            int checkSignalCount = 0;
            foreach (int index in solenoidIndexes)
            {
                bool bSignal = false;
                m_objIO.HLGetDigitalBit(m_objVacuumInitializeParameter.strVacuumOnOutputIO[index], ref bSignal);
                if (bSignal == true)
                {
                    vacuumOnOutputCount++;
                }
                if (string.IsNullOrWhiteSpace(m_objVacuumInitializeParameter.strVacuumOffOutputIO[index]) == false)
                {
                    m_objIO.HLGetDigitalBit(m_objVacuumInitializeParameter.strVacuumOffOutputIO[index], ref bSignal);
                    if (bSignal == true)
                    {
                        vacuumOffOutputCount++;
                    }
                }
                m_objIO.HLGetDigitalBit(m_objVacuumInitializeParameter.strVacuumBlowOutputIO[index], ref bSignal);
                if (bSignal == true)
                {
                    blowOutputCount++;
                }
                checkSignalCount++;
            }

            if (vacuumOnOutputCount == 0
                && (vacuumOffOutputCount == 0 || vacuumOffOutputCount == checkSignalCount)
                && blowOutputCount == 0
                )
            {
                return EVacuumCommand.CMD_OFF;
            }
            else if (vacuumOnOutputCount == checkSignalCount
                && vacuumOffOutputCount == 0
                && blowOutputCount == 0
                )
            {
                return EVacuumCommand.CMD_ON;
            }
            else if (vacuumOnOutputCount == 0
                && vacuumOffOutputCount == 0
                && blowOutputCount == checkSignalCount
                )
            {
                return EVacuumCommand.CMD_BLOW;
            }

            return EVacuumCommand.CMD_UNKNOWN;
        }

        /// <summary>
        /// 진공 상태 확인
        /// </summary>
        /// <param name="eStatusType"></param>
        /// <returns></returns>
        public virtual bool GetVacuumStatus(ref EVacuumStatus eStatusType)
        {
            if (mSolenoidIndexes == null)
            {
                return false;
            }
            m_strAlarmFunction = "GetVacuumStatus";
            int[] solenoidIndexes = mSolenoidIndexes;
            int vacuumInputOnCount = 0;
            int checkSignalCount = 0;
            foreach (int index in solenoidIndexes)
            {
                bool bSignal = false;
                m_objIO.HLGetDigitalBit(m_objVacuumInitializeParameter.strVacuumInputIO[index], ref bSignal);
                if (bSignal == true)
                {
                    vacuumInputOnCount++;
                }
                checkSignalCount++;
            }

            checkSignalCount = m_objVacuumInitializeParameter.iSuccessVacuumDetectCount < 1 ? checkSignalCount : m_objVacuumInitializeParameter.iSuccessVacuumDetectCount;
            if (vacuumInputOnCount == 0)
            {
                eStatusType = EVacuumStatus.STS_OFF;
            }
            else if (vacuumInputOnCount >= checkSignalCount)
            {
                eStatusType = EVacuumStatus.STS_ON;
            }
            else
            {
                eStatusType = EVacuumStatus.STS_TRANSITION;
            }
            return true;
        }

        /// <summary>
        /// 진공 ON 동작
        /// </summary>
        /// <returns></returns>
        private bool SetVacuumOn()
        {
            m_strAlarmFunction = "SetVacuumOn";
            int[] solenoidIndexes = mSolenoidIndexes;
            EVacuumCommand initialCommand = GetVacuumCommand();

            foreach (int index in solenoidIndexes)
            {
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumBlowOutputIO[index], CDefine.OFF);
            }
            WaitForSolenoidDelay(setCommand: EVacuumCommand.CMD_ON, initialCommand: initialCommand);
            foreach (int index in solenoidIndexes)
            {
                if (string.IsNullOrWhiteSpace(m_objVacuumInitializeParameter.strVacuumOffOutputIO[index]) == false)
                {
                    m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumOffOutputIO[index], CDefine.OFF);
                }
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumOnOutputIO[index], CDefine.ON);
                // 가상모드 처리
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumInputIO[index], CDefine.ON);
            }

            mVacuumSettingStartTime = DateTime.Now;
            return true;
        }

        /// <summary>
        /// 진공 OFF 동작
        /// </summary>
        /// <returns></returns>
        private bool SetVacuumOff()
        {
            m_strAlarmFunction = "SetVacuumOff";
            int[] solenoidIndexes = mSolenoidIndexes;
            EVacuumCommand initialCommand = GetVacuumCommand();

            foreach (int index in solenoidIndexes)
            {
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumBlowOutputIO[index], CDefine.OFF);
            }
            WaitForSolenoidDelay(setCommand: EVacuumCommand.CMD_OFF, initialCommand: initialCommand);
            foreach (int index in solenoidIndexes)
            {
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumOnOutputIO[index], CDefine.OFF);
                if (string.IsNullOrWhiteSpace(m_objVacuumInitializeParameter.strVacuumOffOutputIO[index]) == false)
                {
                    m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumOffOutputIO[index], CDefine.ON);
                }
                // 가상모드 처리
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumInputIO[index], CDefine.OFF);
            }

            return true;
        }

        /// <summary>
        /// 진공 BLOW 동작
        /// </summary>
        /// <returns></returns>
        private bool SetVacuumBlow()
        {
            m_strAlarmFunction = "SetVacuumBlow";
            int[] solenoidIndexes = mSolenoidIndexes;
            EVacuumCommand initialCommand = GetVacuumCommand();

            foreach (int index in solenoidIndexes)
            {
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumOnOutputIO[index], CDefine.OFF);
                if (string.IsNullOrWhiteSpace(m_objVacuumInitializeParameter.strVacuumOffOutputIO[index]) == false)
                {
                    m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumOffOutputIO[index], CDefine.OFF);
                }
            }
            WaitForSolenoidDelay(setCommand: EVacuumCommand.CMD_BLOW, initialCommand: initialCommand);
            foreach (int index in solenoidIndexes)
            {
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumBlowOutputIO[index], CDefine.ON);
                // 가상모드 처리
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumInputIO[index], CDefine.OFF);
            }

            mBlowSettingStartTime = DateTime.Now;
            return true;
        }

        private void NotUseSolenoidOff()
        {
            int solenoidCount = (int)m_objVacuumInitializeParameter.eVacuumSolenoidType;

            bool bAnyNotUseSolenoidOn = false;
            for (int i = 0; i < solenoidCount; i++)
            {
                // + 옵션 인덱스가 '-1'이면 모두 사용
                if (mOptionIndex == -1
                    || m_objVacuumInitializeParameter.VacuumUseConfigurations[mOptionIndex].Contains(i) == true
                    )
                {
                    continue;
                }

                bool getValue = false;
                m_objIO.HLGetDigitalBit(m_objVacuumInitializeParameter.strVacuumBlowOutputIO[i], ref getValue);
                if (getValue == true)
                {
                    bAnyNotUseSolenoidOn = true;
                    break;
                }
                if (string.IsNullOrWhiteSpace(m_objVacuumInitializeParameter.strVacuumOffOutputIO[i]) == false)
                {
                    m_objIO.HLGetDigitalBit(m_objVacuumInitializeParameter.strVacuumOffOutputIO[i], ref getValue);
                    if (getValue == true)
                    {
                        bAnyNotUseSolenoidOn = true;
                        break;
                    }
                }
                m_objIO.HLGetDigitalBit(m_objVacuumInitializeParameter.strVacuumOnOutputIO[i], ref getValue);
                if (getValue == true)
                {
                    bAnyNotUseSolenoidOn = true;
                    break;
                }
            }
            if (bAnyNotUseSolenoidOn == false)
            {
                // 켜진 사용하지 않는 솔레노이드가 없으면 스킵함
                return;
            }

            for (int i = 0; i < solenoidCount; i++)
            {
                // + 옵션 인덱스가 '-1'이면 모두 사용
                if (mOptionIndex == -1
                    || m_objVacuumInitializeParameter.VacuumUseConfigurations[mOptionIndex].Contains(i) == true
                    )
                {
                    continue;
                }

                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumBlowOutputIO[i], CDefine.OFF);
            }
            // !!! 기존 상태를 특정 할 수 없기 때문에 무조건 딜레이를 적용함
            Thread.Sleep(m_objVacuumDataParameter.SolenoidDelayTime);
            for (int i = 0; i < solenoidCount; i++)
            {
                // + 옵션 인덱스가 '-1'이면 모두 사용
                if (mOptionIndex == -1
                    || m_objVacuumInitializeParameter.VacuumUseConfigurations[mOptionIndex].Contains(i) == true
                    )
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(m_objVacuumInitializeParameter.strVacuumOffOutputIO[i]) == false)
                {
                    m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumOffOutputIO[i], CDefine.OFF);
                }
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumOnOutputIO[i], CDefine.OFF);
                // 가상모드 처리
                m_objIO.HLSetDigitalBit(m_objVacuumInitializeParameter.strVacuumInputIO[i], CDefine.OFF);
            }
        }

        /// <summary>
        /// 진공 동작 완료 확인
        /// </summary>
        /// <param name="eStatusType"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        private bool WaitForComplete(EVacuumStatus eStatusType, int iSleepPeriod = 5)
        {
            bool bReturn = false;
            EVacuumStatus eCurrentStatusType = EVacuumStatus.STS_UNKNOWN;
            int iTimeOut = m_objVacuumDataParameter.iVacuumTimeOut;
            do
            {
                if (
                    CDefine.ERunMode.UnlinkDryrun != m_objDocument.GetRunMode()
                    // 드라이런 상황이라도 블로우는 항상 체크해야함
                    || EVacuumStatus.STS_OFF == eStatusType
                    )
                {
                    // 입력된 상태 신호와 IO 읽어온 상태 신호를 비교 한다.
                    if (true == GetVacuumStatus(ref eCurrentStatusType))
                    {
                        while (0 < iTimeOut && (eCurrentStatusType != eStatusType))
                        {
                            Thread.Sleep(iSleepPeriod);
                            iTimeOut -= iSleepPeriod;
                            GetVacuumStatus(ref eCurrentStatusType);
                        }
                    }
                    else
                    {
                        break;
                    }

                    // 진공 동작 Timeout 발생.
                    if (0 >= iTimeOut)
                    {
                        break;
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private void WaitForSolenoidDelay(EVacuumCommand setCommand, EVacuumCommand initialCommand)
        {
            switch (setCommand)
            {
                case EVacuumCommand.CMD_ON:
                    {
                        switch (initialCommand)
                        {
                            case EVacuumCommand.CMD_UNKNOWN:
                            case EVacuumCommand.CMD_BLOW:
                                Thread.Sleep(m_objVacuumDataParameter.SolenoidDelayTime);
                                break;

                            case EVacuumCommand.CMD_ON:
                            case EVacuumCommand.CMD_OFF:
                            default:
                                break;
                        }
                    }
                    break;

                case EVacuumCommand.CMD_OFF:
                    {
                        switch (initialCommand)
                        {
                            case EVacuumCommand.CMD_UNKNOWN:
                            case EVacuumCommand.CMD_BLOW:
                                Thread.Sleep(m_objVacuumDataParameter.SolenoidDelayTime);
                                break;

                            case EVacuumCommand.CMD_ON:
                            case EVacuumCommand.CMD_OFF:
                            default:
                                break;
                        }
                    }
                    break;

                case EVacuumCommand.CMD_BLOW:
                    {
                        switch (initialCommand)
                        {
                            case EVacuumCommand.CMD_UNKNOWN:
                            case EVacuumCommand.CMD_ON:
                                Thread.Sleep(m_objVacuumDataParameter.SolenoidDelayTime);
                                break;

                            case EVacuumCommand.CMD_OFF:
                                if (mbHasVacuumOffSignal == false)
                                {
                                    break;
                                }
                                Thread.Sleep(m_objVacuumDataParameter.SolenoidDelayTime);
                                break;

                            case EVacuumCommand.CMD_BLOW:
                            default:
                                break;
                        }
                    }
                    break;

                case EVacuumCommand.CMD_UNKNOWN:
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public virtual bool LoadVacuumData(string strFilePath, string strFileName)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLLoadVacuumData";
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <param name="objCylinderData"></param>
        /// <returns></returns>
        public virtual bool SaveVacuumData(string strFilePath, string strFileName, CVacuumDataParameter objCylinderData)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLSaveVacuumData";
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        protected virtual bool SaveVacuumData(string strFilePath, string strFileName)
        {
            bool bReturn = false;
            m_strAlarmFunction = "SaveVacuumData";
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 진공 이름 얻음
        /// </summary>
        /// <returns></returns>
        public virtual string GetVacuumName()
        {
            return m_objVacuumInitializeParameter.strVacuumName;
        }

        /// <summary>
        /// 진공 데이터 파라미터 얻음
        /// </summary>
        /// <returns></returns>
        public virtual CVacuumDataParameter GetVacuumParameter()
        {
            return m_objVacuumDataParameter;
        }

        /// <summary>
        /// 초기화 파라미터 얻음
        /// </summary>
        /// <returns></returns>
        public CVacuumInitializeParameter GetInitializeParameter()
        {
            return m_objVacuumInitializeParameter;
        }
    }
}

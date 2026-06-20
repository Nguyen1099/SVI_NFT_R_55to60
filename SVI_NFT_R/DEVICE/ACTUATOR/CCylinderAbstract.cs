using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SVI_NFT_R
{
    /// <summary>
    /// 실린더 추상화 클래스
    /// </summary>
    public abstract class CCylinderAbstract
    {
        /// <summary>
        /// 실린더 동작 커맨드
        /// </summary>
        public enum ECylinderCommand
        {
            CMD_UP = 0,
            CMD_LEFT,
            CMD_FORWARD,
            CMD_CW,
            CMD_RETURN,
            CMD_DOWN,
            CMD_RIGHT,
            CMD_BACKWARD,
            CMD_CCW,
            CMD_TURN,
            CMD_MIDDLE,
            CMD_UNKNOWN
        }

        /// <summary>
        /// 실린더 상태
        /// </summary>
        public enum ECylinderStatus
        {
            STS_UP = 0,
            STS_LEFT,
            STS_FORWARD,
            STS_CW,
            STS_RETURN,
            STS_DOWN,
            STS_RIGHT,
            STS_BACKWARD,
            STS_CCW,
            STS_TURN,
            STS_MIDDLE,
            STS_UNKNOWN
        }
        /// <summary>
        /// 실린더 구동 타입 ( 단동, 리버스 단동, 복동, 리버스 복동 )
        /// </summary>
        public enum ECylinderSolenoidType
        {
            UNKNOWN_SOLENOID = 0,
            SINGLE_SOLENOID,
            REVERSE_SINGLE_SOLENOID,
            DOUBLE_SOLENOID,
            REVERSE_DOUBLE_SOLENOID
        }

        /// <summary>
        /// 실린더 출력 갯수
        /// </summary>
        public enum ECylinderOutputIO
        {
            OUTPUT_IO_1 = 0,
            OUTPUT_IO_2,
            OUTPUT_IO_FINAL
        }

        /// <summary>
        /// 실린더 입력 갯수
        /// </summary>
        public enum ECylinderInputIO
        {
            INPUT_IO_1 = 0,
            INPUT_IO_2,
            INPUT_IO_MIDDLE,
            INPUT_IO_FINAL
        }

        /// <summary>
        /// 실린더 센서 무시 유무
        /// </summary>
        public enum ESensorCheck
        {
            IGNORE,
            CHECK
        }

        public enum ECylinderType
        {
            UpDown,
            LeftRigth,
            ForwardBackward,
            CwCcw,
            ReturnTurn
        }

        /// <summary>
        /// 실린더 동작 초기화 파라미터
        /// </summary>
        public class CCylinderInitializeParameter
        {
            /// <summary>실린더 동작 파라미터 파일 경로</summary>
            public string strFilePath;
            /// <summary>실린더 동작 파라미터 파일 이름</summary>
            public string strFileName;
            /// <summary>실린더 이름</summary>
            public string strCylinderName;
            /// <summary>출력 IO</summary>
            public string[] strCylinderOutputIO;
            /// <summary>입력 IO 1동작 감지 신호 ( UP, LEFT, FRONT, CW ) , IO 2동작 감지 신호 ( DOWN, RIGHT, BACKWARD, CCW )</summary>
            public string[] strCylinderInputIO;
            /// <summary>동작 타입</summary>
            public ECylinderSolenoidType eCylinderSolenoidType;
            /// <summary>
            /// 실린더 동작 타입
            /// </summary>
            public ECylinderType eCylinderType;

            public CCylinderInitializeParameter()
            {
                strCylinderOutputIO = new string[(int)ECylinderOutputIO.OUTPUT_IO_FINAL];
                strCylinderInputIO = new string[(int)ECylinderInputIO.INPUT_IO_FINAL];
                eCylinderSolenoidType = ECylinderSolenoidType.UNKNOWN_SOLENOID;

                for (int iLoopCount = 0; iLoopCount < (int)ECylinderOutputIO.OUTPUT_IO_FINAL; iLoopCount++)
                {
                    strCylinderOutputIO[iLoopCount] = string.Empty;
                }
                for (int iLoopCount = 0; iLoopCount < (int)ECylinderOutputIO.OUTPUT_IO_FINAL; iLoopCount++)
                {
                    strCylinderInputIO[iLoopCount] = string.Empty;
                }
            }
        }

        /// <summary>
        /// 실린더 동작시 필요 파라미터
        /// </summary>
        public class CCylinderDataParameter
        {
            /// <summary>실린더 동작 타임아웃 시간</summary>
            public int iCylinderTimeOut;
            /// <summary>1동작 완료 후 지연 시간</summary>
            public int iFirstMoveAfterDelayTime;
            /// <summary>2동작 완료 후 지연 시간</summary>
            public int iSecondMoveAfterDelayTime;
            /// <summary>Middle 동작 완료 후 지연 시간</summary>
            public int iMiddleMoveAfterDelayTime;

            public object Clone()
            {
                CCylinderDataParameter objCylinderDataParameter = new CCylinderDataParameter();
                objCylinderDataParameter.iFirstMoveAfterDelayTime = iFirstMoveAfterDelayTime;
                objCylinderDataParameter.iSecondMoveAfterDelayTime = iSecondMoveAfterDelayTime;
                objCylinderDataParameter.iMiddleMoveAfterDelayTime = iMiddleMoveAfterDelayTime;
                objCylinderDataParameter.iCylinderTimeOut = iCylinderTimeOut;
                return objCylinderDataParameter;
            }
        }
        public ECylinderStatus Status
        {
            get
            {
                ECylinderStatus status = default(ECylinderStatus);
                GetCylinderStatus(ref status);
                return status;
            }
        }
        public ECylinderCommand Command
        {
            get
            {
                return GetCylinderCommand();
            }
        }
        protected ECylinderCommand m_eCommand;
        private string m_strAlarmFunction = string.Empty;
        /// <summary>
        /// 실린더 ON 동작 시간 측정 타이머
        /// </summary>
        private readonly Stopwatch m_swFirstMoveTime = new Stopwatch();
        /// <summary>
        /// 실린더 OFF 동작 시간 측정 타이머
        /// </summary>
        private readonly Stopwatch m_swSecondMoveTime = new Stopwatch();
        /// <summary>
        /// 실린더 MIDDLE 동작 시간 측정 타이머
        /// </summary>
        private readonly Stopwatch m_swMiddleMoveTime = new Stopwatch();
        /// <summary>실린더 ON 동작 시간</summary>
        private double _dFirstMoveTime;
        public double m_dFirstMoveTime
        {
            get { return _dFirstMoveTime; }
        }
        /// <summary>실린더 OFF 동작 시간</summary>
        private double _dSecondMoveTime;
        public double m_dSecondMoveTime
        {
            get { return _dSecondMoveTime; }
        }
        /// <summary>실린더 MIDDLE 동작 시간</summary>
        public double m_dMiddleMoveTime { get; private set; }

        /// <summary>IO 객체 선언</summary>
        protected HLDevice.CDeviceIO m_objIO;
        /// <summary>초기화 파라미터 선언</summary>
        protected CCylinderInitializeParameter m_objCylinderInitializeParameter;
        protected CCylinderDataParameter m_objCylinderDataParameter;
        protected CDocument m_objDocument;
        protected ECylinderType mCylinderType = ECylinderType.CwCcw;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        public CCylinderAbstract(CDocument objDocument)
        {
            m_objDocument = objDocument;
        }

        /// <summary>
        /// 실린더 초기화 함수
        /// </summary>
        /// <param name="objIO"></param>
        /// <param name="objCylinderInitialize"></param>
        /// <returns></returns>
        public abstract bool Initialize(HLDevice.CDeviceIO objIO, CCylinderInitializeParameter objCylinderInitialize);

        /// <summary>
        /// 해제
        /// </summary>
        public abstract void DeInitialize();

        /// <summary>
        /// 실린더 커맨드 수행
        /// </summary>
        /// <param name="eCommand"></param>
        /// <param name="eSensorIgnore"></param>
        /// <returns></returns>
        public virtual bool SetCylinderCommand(ECylinderCommand eCommand, ESensorCheck eSensorIgnore = ESensorCheck.CHECK)
        {
            bool bReturn = false;
            m_strAlarmFunction = "SetCylinderCommand";
            do
            {
                int command = 0;
                switch (eCommand)
                {
                    case ECylinderCommand.CMD_UP:
                        command = 0;
                        break;
                    case ECylinderCommand.CMD_LEFT:
                        command = 0;
                        break;
                    case ECylinderCommand.CMD_FORWARD:
                        command = 0;
                        break;
                    case ECylinderCommand.CMD_CW:
                        command = 0;
                        break;
                    case ECylinderCommand.CMD_RETURN:
                        command = 0;
                        break;
                    case ECylinderCommand.CMD_DOWN:
                        command = 1;
                        break;
                    case ECylinderCommand.CMD_RIGHT:
                        command = 1;
                        break;
                    case ECylinderCommand.CMD_BACKWARD:
                        command = 1;
                        break;
                    case ECylinderCommand.CMD_CCW:
                        command = 1;
                        break;
                    case ECylinderCommand.CMD_TURN:
                        command = 1;
                        break;
                    case ECylinderCommand.CMD_MIDDLE:
                        command = 2;
                        break;
                    default:
                        break;

                }
                // ( UP, LEFT, FRONT, CW, RETURN )  동작
                if (0 == command)
                {
                    m_swFirstMoveTime.Reset();
                    m_swFirstMoveTime.Start();
                    if (false == CylinderOn())
                    {
                        break;
                    }
                    // ( DOWN, RIGHT, BACKWARD, CCW, TURN ) 동작
                }
                else if (1 == command)
                {
                    m_swSecondMoveTime.Reset();
                    m_swSecondMoveTime.Start();
                    if (false == CylinderOff())
                    {
                        break;
                    }
                }
                else if (2 == command)
                {
                    m_swMiddleMoveTime.Reset();
                    m_swMiddleMoveTime.Start();
                    if (false == CylinderMiddle())
                    {
                        break;
                    }
                }

                if (ESensorCheck.CHECK == eSensorIgnore)
                {
                    if (false == WaitForComplete((ECylinderStatus)eCommand))
                    {
                        m_eCommand = ECylinderCommand.CMD_UNKNOWN;
                        break;
                    }
                }

                // 실린더 동작 후 지연 시간
                switch (command)
                {
                    case 0:
                        Thread.Sleep(m_objCylinderDataParameter.iFirstMoveAfterDelayTime);
                        break;
                    case 1:
                        Thread.Sleep(m_objCylinderDataParameter.iSecondMoveAfterDelayTime);
                        break;
                    case 2:
                        Thread.Sleep(m_objCylinderDataParameter.iMiddleMoveAfterDelayTime);
                        break;

                    default:
                        break;
                }

                m_swFirstMoveTime.Stop();
                _dFirstMoveTime = m_swFirstMoveTime.ElapsedMilliseconds;
                m_swSecondMoveTime.Stop();
                _dSecondMoveTime = m_swSecondMoveTime.ElapsedMilliseconds;
                m_swMiddleMoveTime.Stop();
                m_dMiddleMoveTime = m_swMiddleMoveTime.ElapsedMilliseconds;
                m_eCommand = eCommand;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 커맨드 반환
        /// </summary>
        /// <returns></returns>
        public virtual ECylinderCommand GetCylinderCommand()
        {
            var result = ECylinderCommand.CMD_UNKNOWN;
            bool cylinderOutput1 = false;
            bool cylinderOutput2 = false;

            m_objIO.HLGetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_1], ref cylinderOutput1);
            m_objIO.HLGetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_2], ref cylinderOutput2);

            switch (mCylinderType)
            {
                case ECylinderType.UpDown:
                    if (
                        true == cylinderOutput1
                        && false == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_UP;
                    }
                    else if (
                        false == cylinderOutput1
                        && true == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_DOWN;
                    }
                    break;
                case ECylinderType.LeftRigth:
                    if (
                        true == cylinderOutput1
                        && false == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_UP;
                    }
                    else if (
                        false == cylinderOutput1
                        && true == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_DOWN;
                    }
                    break;
                case ECylinderType.ForwardBackward:
                    if (
                        true == cylinderOutput1
                        && false == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_FORWARD;
                    }
                    else if (
                        false == cylinderOutput1
                        && true == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_BACKWARD;
                    }
                    break;
                case ECylinderType.CwCcw:
                    if (
                        true == cylinderOutput1
                        && false == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_CW;
                    }
                    else if (
                        false == cylinderOutput1
                        && true == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_CCW;
                    }
                    break;
                case ECylinderType.ReturnTurn:
                    if (
                        true == cylinderOutput1
                        && false == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_RETURN;
                    }
                    else if (
                        false == cylinderOutput1
                        && true == cylinderOutput2
                        )
                    {
                        result = ECylinderCommand.CMD_TURN;
                    }
                    break;
                default:
                    break;
            }

            if (
                result == ECylinderCommand.CMD_UNKNOWN
                && false == cylinderOutput1
                && false == cylinderOutput2
                )
            {
                result = ECylinderCommand.CMD_MIDDLE;
            }

            return result;
        }

        /// <summary>
        /// 실린더 상태 확인
        /// </summary>
        /// <param name="eStatusType"></param>
        /// <returns></returns>
        public virtual bool GetCylinderStatus(ref ECylinderStatus eStatusType)
        {
            bool bReturn = false;
            bool bFirstSensor = false;
            bool bSecondSensor = false;
            m_strAlarmFunction = "GetCylinderStatus";
            do
            {
                // ( UP, LEFT, FRONT, CW, RETURN )  동작 확인
                if (false == GetFirstSensorStatus(ref bFirstSensor))
                {
                    break;
                }
                // ( DOWN, RIGHT, BACKWARD, CCW, TURN )  동작 확인
                if (false == GetSecondSensorStatus(ref bSecondSensor))
                {
                    break;
                }

                eStatusType = ECylinderStatus.STS_UNKNOWN;
                switch (mCylinderType)
                {
                    case ECylinderType.UpDown:
                        if (true == bFirstSensor)
                        {
                            eStatusType = ECylinderStatus.STS_UP;
                        }
                        else if (true == bSecondSensor)
                        {
                            eStatusType = ECylinderStatus.STS_DOWN;
                        }
                        break;
                    case ECylinderType.LeftRigth:
                        if (true == bFirstSensor)
                        {
                            eStatusType = ECylinderStatus.STS_LEFT;
                        }
                        else if (true == bSecondSensor)
                        {
                            eStatusType = ECylinderStatus.STS_RIGHT;
                        }
                        break;
                    case ECylinderType.ForwardBackward:
                        if (true == bFirstSensor)
                        {
                            eStatusType = ECylinderStatus.STS_FORWARD;
                        }
                        else if (true == bSecondSensor)
                        {
                            eStatusType = ECylinderStatus.STS_BACKWARD;
                        }
                        break;
                    case ECylinderType.CwCcw:
                        if (true == bFirstSensor)
                        {
                            eStatusType = ECylinderStatus.STS_CW;
                        }
                        else if (true == bSecondSensor)
                        {
                            eStatusType = ECylinderStatus.STS_CCW;
                        }
                        break;
                    case ECylinderType.ReturnTurn:
                        if (true == bFirstSensor)
                        {
                            eStatusType = ECylinderStatus.STS_RETURN;
                        }
                        else if (true == bSecondSensor)
                        {
                            eStatusType = ECylinderStatus.STS_TURN;
                        }
                        break;
                    default:
                        break;
                }

                bool bSensor = false;
                if (
                    eStatusType == ECylinderStatus.STS_UNKNOWN
                    && false == string.IsNullOrWhiteSpace(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_MIDDLE])
                    && true == m_objIO.HLGetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_MIDDLE], ref bSensor)
                    && true == bSensor
                    )
                {
                    eStatusType = ECylinderStatus.STS_MIDDLE;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 실린더 출력 On 동작 ( UP, LEFT, FRONT, CW, RETURN )
        /// </summary>
        /// <returns></returns>
        private bool CylinderOn()
        {
            bool bReturn = false;
            bool bResult = false;
            m_strAlarmFunction = "CylinderOn";
            do
            {
                // 솔레노이드 타입에 맞게 실린더 동작
                switch (m_objCylinderInitializeParameter.eCylinderSolenoidType)
                {
                    case ECylinderSolenoidType.SINGLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_1], true);
                        break;
                    case ECylinderSolenoidType.REVERSE_SINGLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_2], true);
                        break;
                    case ECylinderSolenoidType.DOUBLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_1], true);
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_2], false);
                        break;
                    case ECylinderSolenoidType.REVERSE_DOUBLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_1], false);
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_2], true);
                        break;
                    default:
                        break;
                }

                if (false == bResult)
                {
                    break;
                }

                if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    // 솔레노이드 타입에 맞게 실린더 동작
                    switch (m_objCylinderInitializeParameter.eCylinderSolenoidType)
                    {
                        case ECylinderSolenoidType.SINGLE_SOLENOID:
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], true);
                            break;
                        case ECylinderSolenoidType.REVERSE_SINGLE_SOLENOID:
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], true);
                            break;
                        case ECylinderSolenoidType.DOUBLE_SOLENOID:
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], true);
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], false);
                            break;
                        case ECylinderSolenoidType.REVERSE_DOUBLE_SOLENOID:
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], false);
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], true);
                            break;
                        default:
                            break;
                    }
                    if (false == string.IsNullOrWhiteSpace(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_MIDDLE]))
                    {
                        m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_MIDDLE], false);
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 실린더 출력 Off 동작 ( DOWN, RIGHT, BACKWARD, CCW, TURN )
        /// </summary>
        /// <returns></returns>
        private bool CylinderOff()
        {
            bool bReturn = false;
            bool bResult = false;
            m_strAlarmFunction = "CylinderOff";
            do
            {
                // 솔레노이드 타입에 맞게 실린더 동작
                switch (m_objCylinderInitializeParameter.eCylinderSolenoidType)
                {
                    case ECylinderSolenoidType.SINGLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_1], false);
                        break;
                    case ECylinderSolenoidType.REVERSE_SINGLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_2], false);
                        break;
                    case ECylinderSolenoidType.DOUBLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_1], false);
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_2], true);
                        break;
                    case ECylinderSolenoidType.REVERSE_DOUBLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_1], true);
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_2], false);
                        break;
                    default:
                        break;
                }

                if (false == bResult)
                {
                    break;
                }

                if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    // 솔레노이드 타입에 맞게 실린더 동작
                    switch (m_objCylinderInitializeParameter.eCylinderSolenoidType)
                    {
                        case ECylinderSolenoidType.SINGLE_SOLENOID:
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], false);
                            break;
                        case ECylinderSolenoidType.REVERSE_SINGLE_SOLENOID:
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], false);
                            break;
                        case ECylinderSolenoidType.DOUBLE_SOLENOID:
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], false);
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], true);
                            break;
                        case ECylinderSolenoidType.REVERSE_DOUBLE_SOLENOID:
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], true);
                            m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], false);
                            break;
                        default:
                            break;
                    }
                    if (false == string.IsNullOrWhiteSpace(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_MIDDLE]))
                    {
                        m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_MIDDLE], false);
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 실린더 MIDDLE 동작
        /// </summary>
        /// <returns></returns>
        private bool CylinderMiddle()
        {
            bool bReturn = false;
            bool bResult = false;
            m_strAlarmFunction = "CylinderMiddle";
            do
            {
                // 솔레노이드 타입에 맞게 실린더 동작
                switch (m_objCylinderInitializeParameter.eCylinderSolenoidType)
                {
                    case ECylinderSolenoidType.REVERSE_DOUBLE_SOLENOID:
                    case ECylinderSolenoidType.DOUBLE_SOLENOID:
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_1], false);
                        bResult = m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderOutputIO[(int)ECylinderOutputIO.OUTPUT_IO_2], false);
                        break;
                    default:
                        break;
                }
                if (false == bResult)
                {
                    break;
                }

                if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    // 솔레노이드 타입에 맞게 실린더 동작
                    switch (m_objCylinderInitializeParameter.eCylinderSolenoidType)
                    {
                        case ECylinderSolenoidType.DOUBLE_SOLENOID:
                        case ECylinderSolenoidType.REVERSE_DOUBLE_SOLENOID:
                            if (false == string.IsNullOrWhiteSpace(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1]))
                            {
                                m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], false);
                            }
                            if (false == string.IsNullOrWhiteSpace(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2]))
                            {
                                m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], false);
                            }
                            if (false == string.IsNullOrWhiteSpace(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_MIDDLE]))
                            {
                                m_objIO.HLSetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_MIDDLE], true);
                            }
                            break;
                        default:
                            break;
                    }
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 실린더 입력 On 상태 확인 ( UP, LEFT, FRONT, CW, RETURN )
        /// </summary>
        /// <param name="bResult"></param>
        /// <returns></returns>
        private bool GetFirstSensorStatus(ref bool bResult)
        {
            bool bReturn = false;
            bool bFirstSensor = false;
            bool bSecondSensor = false;
            m_strAlarmFunction = "GetFirstSensorStatus";
            do
            {
                if (false == m_objIO.HLGetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], ref bFirstSensor))
                {
                    break;
                }

                if (false == m_objIO.HLGetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], ref bSecondSensor))
                {
                    break;
                }

                // 첫번째 동작 입력 센서 On 두번째 동작 입력 센서 Off 일 경우
                if (true == bFirstSensor && false == bSecondSensor)
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 실린더 입력 On 상태 확인 ( DOWN, RIGHT, BACKWARD, CCW, TURN )
        /// </summary>
        /// <param name="bResult"></param>
        /// <returns></returns>
        private bool GetSecondSensorStatus(ref bool bResult)
        {
            bool bReturn = false;
            bool bFirstSensor = false;
            bool bSecondSensor = false;
            m_strAlarmFunction = "GetSecondSensorStatus";
            do
            {
                if (false == m_objIO.HLGetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_1], ref bFirstSensor))
                {
                    break;
                }

                if (false == m_objIO.HLGetDigitalBit(m_objCylinderInitializeParameter.strCylinderInputIO[(int)ECylinderInputIO.INPUT_IO_2], ref bSecondSensor))
                {
                    break;
                }

                // 첫번째 동작 입력 센서 Off 두번째 동작 입력 센서 On 일 경우
                if (false == bFirstSensor && true == bSecondSensor)
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 실린더 동작 완료 확인
        /// </summary>
        /// <param name="eStatusType"></param>
        /// <param name="iSleepPeriod"></param>
        /// <returns></returns>
        private bool WaitForComplete(ECylinderStatus eStatusType, int iSleepPeriod = 5)
        {
            bool bReturn = false;
            ECylinderStatus eCurrentStatusType = ECylinderStatus.STS_UNKNOWN;
            int iTimeOut = m_objCylinderDataParameter.iCylinderTimeOut;
            do
            {
                // 입력된 상태 신호와 IO 읽어온 상태 신호를 비교 한다.
                if (true == GetCylinderStatus(ref eCurrentStatusType))
                {
                    while (0 < iTimeOut && (eCurrentStatusType != eStatusType))
                    {
                        Thread.Sleep(iSleepPeriod);
                        iTimeOut -= iSleepPeriod;
                        GetCylinderStatus(ref eCurrentStatusType);
                    }
                }
                else
                {
                    break;
                }

                // 실린더 동작 Timeout 발생.
                if (0 >= iTimeOut)
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public virtual bool LoadCylinderData(string strFilePath, string strFileName)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLLoadCylinderData";
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
        public virtual bool SaveCylinderData(string strFilePath, string strFileName, CCylinderDataParameter objCylinderData)
        {
            bool bReturn = false;
            m_strAlarmFunction = "HLSaveCylinderData";
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
        protected virtual bool SaveCylinderData(string strFilePath, string strFileName)
        {
            bool bReturn = false;
            m_strAlarmFunction = "SaveCylinderData";
            do
            {
                System.Windows.Forms.MessageBox.Show("[" + m_strAlarmFunction + "] 가상 함수가 구현되지 않았습니다.");
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 실린더 이름 얻음
        /// </summary>
        /// <returns></returns>
        public virtual string GetCylinderName()
        {
            return m_objCylinderInitializeParameter.strCylinderName;
        }

        /// <summary>
        /// 실린더 타입을 얻음
        /// </summary>
        /// <returns></returns>
        public ECylinderType GetCylinderType()
        {
            return m_objCylinderInitializeParameter.eCylinderType;
        }

        /// <summary>
        /// 실린더 데이터 파라미터 얻음
        /// </summary>
        /// <returns></returns>
        public virtual CCylinderDataParameter GetCylinderParameter()
        {
            return (CCylinderDataParameter)m_objCylinderDataParameter.Clone();
        }

        public bool HasSensor()
        {
            return m_objCylinderInitializeParameter.strCylinderInputIO.Any(i => string.IsNullOrWhiteSpace(i) == false);
        }
    }
}

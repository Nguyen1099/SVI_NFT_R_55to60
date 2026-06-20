using ENC.Device.Communication;
using SVI_NFT_R;
using SVI_NFT_R.DEVICE.Svi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HLDevice
{
    public class CDeviceSviInterface
    {
        /// <summary>
        /// 수신 데이터
        /// </summary>
        public class CReceiveData
        {
            public string ID { get; set; } = string.Empty;
            //////////////////////////////////////////////////////////////////////////
            // 수신 데이터
            public string strData;
            public byte[] byteReceiveData = new byte[4096];
            public int iByteLength;

            //////////////////////////////////////////////////////////////////////////
            // 파싱 데이터
            public EReceiveDataType eReceiveData;
            public string strReceiveData;
            public CDefine.ELogType LogType { get; set; }

            public void Clear()
            {
                strData = "";
                strReceiveData = "";
                eReceiveData = EReceiveDataType.UnkownType;
                Array.Clear(byteReceiveData, 0, byteReceiveData.Length);
                iByteLength = 0;
            }

            public object Clone()
            {
                CReceiveData objData = new CReceiveData();
                objData.strData = strData;
                byteReceiveData.CopyTo(objData.byteReceiveData, 0);
                objData.iByteLength = iByteLength;
                objData.eReceiveData = eReceiveData;
                objData.strReceiveData = strReceiveData;
                return objData;
            }
        }
        /// <summary>
        /// 수신 데이터 타입
        /// </summary>
        public enum EReceiveDataType
        {
            UnkownType = -1,

            //////////////////////////////////////////////////////////////////////////
            // 응답 수신
            /// <summary>"MTO.SEND.GRAB.START." => "OTM.RECV.GRAB.START."</summary>
            GrabStartAcknowledge = 0,
            /// <summary>"MTO.SEND.AMO_GRAB.START." => "OTM.RECV.AMO_GRAB.START."</summary>
            AmoGrabStartAcknowledge,
            /// <summary>"MTO.SEND.AMO_GRAB.END." => "OTM.RECV.AMO_GRAB.END."</summary>
            AmoGrabEndAcknowledge,
            /// <summary>"MTO.SEND.MCR." => "OTM.RECV.MCR."</summary>
            CellDataSyncAcknowledge,
            /// <summary>"MTO.SEND.RECIPE.VALUE" => "OTM.RECV.RECIPE.VALUE."</summary>
            CurrentRecipeInformationRequestAcknowledge,
            /// <summary>"MTO.SEND.ERROR." => No Response</summary>
            ErrorMessageAcknowledge,
            /// <summary>"MTO.SEND.INIT." => No Response</summary>
            InitializeMessageAcknowledge,
            /// <summary>"MTO.SEND.LIGHT.VALUE" => "OTM.RECV.LIGHT.VALUE."</summary>
            LightIntensityAcknowledge,
            /// <summary>"MTO.SEND.TEMP.VALUE" => "OTM.RECV.TEMP.VALUE."</summary>
            LightTemperatureAcknowledge,
            /// <summary>"MTO.SEND.TIMESYNC." => No Response</summary>
            TimeSyncAcknowledge,
            /// <summary>"MTO.SEND.VACUUM.[ON, OFF]" => No Response</summary>
            VacuumAcknowledge,
            /// <summary>"MTO.SEND.LASER.CHECK" => "OTM.RECV.LASER.CHECK.[ON, OFF]"</summary>
            LaserCheckAcknowledge,
            /// <summary>"MTO.SEND.LASER.OFF" => No Response</summary>
            LaserInterlockAcknowledge,
            /// <summary>"MTO.SEND.STATE.CHECK" => "OTM.RECV.STATE.[CHECK, WORK, INIT, IDLE, ERROR.Message]"</summary>
            StateCheckAcknowledge,

            //////////////////////////////////////////////////////////////////////////
            // 요청 수신
            /// <summary>{After Grab End} "OTM.SEND.GRAB.END." => "MTO.RECV.GRAB.END."</summary>
            GrabEndRequest,
            /// <summary>{After Grab End} "OTM.SEND.ALIGN." => "MTO.RECV.ALIGN."</summary>
            AlignDataRequest,
            /// <summary>{After Grab End} "OTM.SEND.AUTOREVIEW." => "MTO.RECV.AUTOREVIEW."</summary>
            AutoReviewDataRequest,
            /// <summary>"OTM.SEND.LASER.CHECK" => "MTO.RECV.LASER.CHECK.[ABLE, DISABLE]"</summary>
            CheckLaserAbleRequest,
            /// <summary>"OTM.SEND.LASER.[ON, OFF]" => "MTO.RECV.LASER.[ON, OFF]"</summary>
            LaserStatusDataRequest,
            /// <summary>"OTM.SEND.ERROR." => "MTO.RECV.ERROR."</summary>
            ErrorMessageRequest,
            /// <summary>{After Grab End} "OTM.SEND.INSPECT.RESULT." => "MTO.RECV.INSPECT.RESULT."</summary>
            ResultDataRequest,
            /// <summary>{After Grab End} "OTM.SEND.TOTALRESULT." => "MTO.RECV.TOTALRESULT."</summary>
            TotalResultDataRequest,
            /// <summary>{After Grab End} "OTM.SEND.LIGHT." => "MTO.RECV.LIGHT."</summary>
            LightForDvListRequest,
            /// <summary>"OTM.SWERROR.[HEAVY, LIGHT].MESSAGE.[CAM, LIGHT]"</summary>
            JavasSoftwareErrorRequest,
            /// <summary>{After Check Status} "OTM.RECV.VALIDATION."</summary>
            RecipeValidationDataRequest,
        };
        //////////////////////////////////////////////////////////////////////////
        // 동기를 맞춰야하는 항목들 정의
        private Dictionary<EReceiveDataType, bool> mReceiveFlag;
        /// <summary>
        /// 델리게이트 선언
        /// </summary>
        public delegate void CallBackFuntionReceiveData(CReceiveData receiveData);
        /// <summary>
        /// 접속 이벤트
        /// </summary>
        public event EventHandler OnConnected;
        /// <summary>
        /// 접속 해제 이벤트
        /// </summary>
        public event EventHandler OnDisconnected;
        /// <summary>
        /// 로그 타입
        /// </summary>
        public CDefine.ELogType LogType => mLogType;
        /// <summary>
        /// 데이터 수신 콜백 함수
        /// </summary>
        private readonly List<CallBackFuntionReceiveData> mReceiveDataCallbacks = new List<CallBackFuntionReceiveData>(4);
        /// <summary>
        /// 통신객체
        /// </summary>
        private CDeviceCommunication mCommunication;
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private CDocument mDocument;
        /// <summary>
        /// 수신 버퍼 (패킷이 끊겨 올 때 처리를 위한 버퍼)
        /// </summary>
        private readonly MemoryStream mReceiveBuffer = new MemoryStream(8192);

        //////////////////////////////////////////////////////////////////////////
        // 상수 정의
        public const string NOCELL = "NOCELL";
        public const string NONE = "NONE";
        public const string REASONCODE_SEPARATOR = ",";
        public const string SEPARATOR = ".";
        private const string SEND = "SEND";
        private const string RECEIVE = "RECV";
        private const string MTO = "MTO";
        private const string OTM = "OTM";
        private const string RESERVED0 = "00";
        private const string RESERVED1 = "00";
        private const char STX = '\u0005';
        private const char ETX = '\u000A';

        private const string PROTOCOL1_ALIGN = "ALIGN";
        private const string PROTOCOL1_AUTOREVIEW = "AUTOREVIEW";
        private const string PROTOCOL1_ERROR = "ERROR";
        private const string PROTOCOL1_GRAB = "GRAB";
        private const string PROTOCOL1_AMO_GRAB = "AMO_GRAB";
        private const string PROTOCOL1_MCR = "MCR";
        private const string PROTOCOL1_INIT = "INIT";
        private const string PROTOCOL1_INSPECT = "INSPECT";
        private const string PROTOCOL1_LASER = "LASER";
        private const string PROTOCOL1_LIGHT = "LIGHT";
        private const string PROTOCOL1_RECIPE = "RECIPE";
        private const string PROTOCOL1_STATE = "STATE";
        private const string PROTOCOL1_SWERROR = "SWERROR";
        private const string PROTOCOL1_TEMP = "TEMP";
        private const string PROTOCOL1_TIMESYNC = "TIMESYNC";
        private const string PROTOCOL1_TOTALRESULT = "TOTALRESULT";
        private const string PROTOCOL1_VACUUM = "VACUUM";
        private const string PROTOCOL1_VALIDATION = "VALIDATION";

        private const string PROTOCOL2_CHECK = "CHECK";
        private const string PROTOCOL2_END = "END";
        private const string PROTOCOL2_RESULT = "RESULT";
        private const string PROTOCOL2_START = "START";
        private const string PROTOCOL2_VALUE = "VALUE";
        private const string PROTOCOL2_OFF = "OFF";

        private const int HEADER_LENGTH = 8;
        private const int RETRY_COUNT = 1;

        private CDefine.ELogType mLogType;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objDocument">도큐먼트 객체</param>
        /// <param name="objInitializeParameter">초기화 파라메터</param>
        /// <returns>초기화 결과</returns>
        public bool HLInitialize(CDocument objDocument, CConfig.CInspInitializeParameter objInitializeParameter, CDefine.ELogType logType)
        {
            bool bReturn = false;

            do
            {
                mDocument = objDocument;
                mLogType = logType;
#if SCT_AMO
                mCommunication = new CDeviceCommunication(new CDeviceCommunicationSocketClient());
#else
                if (CDefine.ESimulationMode.SIMULATION_MODE_OFF == objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    mCommunication = new CDeviceCommunication(new CDeviceCommunicationSocketClient());
                }
                else
                {
                    mCommunication = new CDeviceCommunication(new CDeviceCommunicationVirtual());
                }
#endif

                // 이벤트 핸들러 연결
                mCommunication.OnConnect = Communication_OnConnected;
                mCommunication.OnDisconnect = Communication_OnDisconected;

                // 수신 완료 플래그 초기화
                mReceiveFlag = new Dictionary<EReceiveDataType, bool>();
                foreach (EReceiveDataType item in Enum.GetValues(typeof(EReceiveDataType)))
                {
                    mReceiveFlag[item] = false;
                }

                // 초기화
                var objParameter = new CDeviceCommunicationAbstract.CInitializeParameter()
                {
                    eType = (CDeviceCommunicationAbstract.CInitializeParameter.EType)objInitializeParameter.eType,
                    strSocketIPAddress = objInitializeParameter.strSocketIPAddress,
                    iSocketPortNumber = objInitializeParameter.iSocketPortNumber
                };
                if (mCommunication.HLInitialize(objParameter) == false)
                {
                    break;
                }

                // 데이터 수신 이벤트 핸덜러 연결
                mCommunication.SetCallbackFunction(Communication_OnReceiveData);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            mCommunication.HLDeInitialize();
        }

        /// <summary>
        /// 연결 상태
        /// </summary>
        /// <returns></returns>
        public bool HLIsConnected()
        {
            return mCommunication.HLIsConnected();
        }

        /// <summary>
        /// 콜백 연결
        /// </summary>
        /// <param name="objReceiveData"></param>
        public void AddCallbackFunction(CallBackFuntionReceiveData objReceiveData)
        {
            mReceiveDataCallbacks.Add(objReceiveData);
        }

        /// <summary>
        /// 콜백 연결 해제
        /// </summary>
        /// <param name="objReceiveData"></param>
        public void RemoveCallbackFunction(CallBackFuntionReceiveData objReceiveData)
        {
            mReceiveDataCallbacks.Remove(objReceiveData);
        }

        /// <summary>
        /// 그랩 시작 명령 전송 (에어리어 타입)
        /// </summary>
        /// <param name="data">그랩 시작 데이터</param>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendGrabStart(SendData.GrabStartData data, EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}.{4}",
                    MTO,
                    SEND,
                    PROTOCOL1_GRAB,
                    PROTOCOL2_START,
                    data.ToString()
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.GrabStartAcknowledge] = false;
                mReceiveFlag[EReceiveDataType.GrabEndRequest] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.GrabStartAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// AMO 그랩 시작 명령 전송 (라인스캔 타입)
        /// </summary>
        /// <param name="data">그랩 시작 데이터</param>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendAmoGrabStart(SendData.AmoGrabStartData data, EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}.{4}",
                    MTO,
                    SEND,
                    PROTOCOL1_AMO_GRAB,
                    PROTOCOL2_START,
                    data.ToString()
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.AmoGrabStartAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.AmoGrabStartAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// AMO 그랩 종료 명령 전송 (라인스캔 타입)
        /// </summary>
        /// <param name="data">그랩 종료 데이터</param>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendAmoGrabEnd(SendData.AmoGrabEndData data, EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}.{4}",
                    MTO,
                    SEND,
                    PROTOCOL1_AMO_GRAB,
                    PROTOCOL2_END,
                    data.ToString()
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.AmoGrabEndAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.AmoGrabEndAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 셀 데이터 동기화 명령 전송
        /// </summary>
        /// <param name="data">셀 데이터 동기화 데이터</param>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendCellDataSync(SendData.CellDataSyncData data, EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_MCR,
                    data.ToString()
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.CellDataSyncAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.CellDataSyncAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 조명값 요청
        /// </summary>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendLightIntensityRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_LIGHT,
                    PROTOCOL2_VALUE
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.LightIntensityAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.LightIntensityAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 조명 온도 요청
        /// </summary>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendLightTemperatureRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_TEMP,
                    PROTOCOL2_VALUE
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.LightTemperatureAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.LightTemperatureAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 시간 동기화 명령 전송
        /// </summary>
        /// <param name="dateTime">시간</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendTimeSync(DateTime dateTime)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3:D4}{4:D2}{5:D2}{6:D2}{7:D2}{8:D2}",
                    MTO,
                    SEND,
                    PROTOCOL1_TIMESYNC,
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.TimeSyncAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    // 응답 대기하지 않음
                    //if( waitReceiveFlag(EReceiveDataType.TimeSyncAcknowledge)== false)
                    //{
                    //    tryCount++;
                    //    continue;
                    //}

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// Javas 상태 요청
        /// </summary>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendStateCheckRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_STATE,
                    PROTOCOL2_CHECK
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.StateCheckAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.StateCheckAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// Javas 에 초기화 메시지를 전송
        /// </summary>
        /// <param name="initializeMessage">초기화 메시지</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendInitializeMessage(string initializeMessage)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_INIT,
                    initializeMessage
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.InitializeMessageAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    // 응답 없는 프로토콜
                    //if (EWait.ForReceive == waitResult
                    //    && WaitReceiveFlag(EReceiveDataType.InitializeMessageAcknowlege, waitTimeout, waitSleepPeriod) == false
                    //    )
                    //{
                    //    tryCount++;
                    //    continue;
                    //}

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// Javas 에 에러 메시지를 전송
        /// </summary>
        /// <param name="errorMessage">에러 메시지</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendErrorMessage(string errorMessage)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_INIT,
                    errorMessage
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.ErrorMessageAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    // 응답 없는 프로토콜
                    //if (EWait.ForReceive == waitResult
                    //    && WaitReceiveFlag(EReceiveDataType.ErrorMessageAcknowlege, waitTimeout, waitSleepPeriod) == false
                    //    )
                    //{
                    //    tryCount++;
                    //    continue;
                    //}

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// Javas 에 베큠 메시지를 전송
        /// </summary>
        /// <param name="onOff">true = VACUUM ON, false = VACUUM OFF</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendVacuum(EPower onOff)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_VACUUM,
                    (EPower.On == onOff) ? "ON" : "OFF"
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.VacuumAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    // 응답 없는 프로토콜
                    //if (EWait.ForReceive == waitResult
                    //    && WaitReceiveFlag(EReceiveDataType.VacuumAcknowlege, waitTimeout, waitSleepPeriod) == false
                    //    )
                    //{
                    //    tryCount++;
                    //    continue;
                    //}

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// Javas 에 현재 레시피 정보 요청 메시지를 전송
        /// </summary>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendCurrentRecipeInformationRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_RECIPE,
                    PROTOCOL2_VALUE
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.CurrentRecipeInformationRequestAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.CurrentRecipeInformationRequestAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// Javas 에 레이저 상태 요청 메시지를 전송
        /// </summary>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendLaserCheckRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_LASER,
                    PROTOCOL2_CHECK
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.LaserCheckAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.LaserCheckAcknowledge, waitTimeout, waitSleepPeriod) == false
                        )
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// Javas 에 레이저 상태 요청 메시지를 전송
        /// </summary>
        /// <param name="waitResult">결과 수신 대기</param>
        /// <param name="waitTimeout">결과 수신 타임아웃 시간</param>
        /// <param name="waitSleepPeriod">결과 수신 대기 슬립</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendLaserInterlockRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000, int waitSleepPeriod = 5)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_LASER,
                    PROTOCOL2_OFF
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.LaserInterlockAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    // 응답 없는 프로토콜
                    //if (EWait.ForReceive == waitResult
                    //    && WaitReceiveFlag(EReceiveDataType.LaserInterlockAcknowlege, waitTimeout, waitSleepPeriod) == false
                    //    )
                    //{
                    //    tryCount++;
                    //    continue;
                    //}

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// Javas 에 레이저 사용 가능 상태 요청에 대한 응답 전송
        /// </summary>
        /// <param name="able">레이저 사용 가능 여부</param>
        /// <returns>함수 실행 결과</returns>
        public bool HLSendLaserAbleAcknowlege(EPossible able)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}.{4}",
                    MTO,
                    RECEIVE,
                    PROTOCOL1_LASER,
                    PROTOCOL2_CHECK,
                    (able == EPossible.Able) ? "ABLE" : "DISABLE"
                    );

                int tryCount = 0;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    break;
                } while (RETRY_COUNT > tryCount);

                if (RETRY_COUNT <= tryCount)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        public bool GetReceiveFlag(EReceiveDataType waitType) => mReceiveFlag[waitType];

        /// <summary>
        /// 메세지 기다림 (동기를 맞춰야 할 새로운 프로토콜이 생긴다면 맴버 변수를 추가하여 사용해야함)
        /// </summary>
        /// <param name="waitType">대기 플래그 종류</param>
        /// <param name="iTimeOut">타임 아웃 시간 (단위 : ms)</param>
        /// <param name="iSleepPeriod">확인 간격 (단위 : ms)</param>
        /// <returns></returns>
        public bool WaitReceiveFlag(EReceiveDataType waitType, int iTimeOut = 5000, int iSleepPeriod = 5)
        {
            bool bReturn = false;
            do
            {
                // 수신 될 때까지 기다림
                while (0 < iTimeOut
                    && mReceiveFlag[waitType] == false
                    )
                {
                    System.Threading.Thread.Sleep(iSleepPeriod);
                    iTimeOut -= iSleepPeriod;
                }
                // Timeout Check
                if (0 >= iTimeOut)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] WaitReceiveFlag,Timeout,WaitType='{0}'", waitType.ToString()));
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 메시지 처리 완료 플래그를 업데이트한다.
        /// </summary>
        /// <param name="data">메시지 객체</param>
        public void SetMessageProcessDone(CReceiveData data)
        {
            mReceiveFlag[data.eReceiveData] = true;
        }

        /// <summary>
        /// 연결 이벤트 핸들러
        /// </summary>
        private void Communication_OnConnected()
        {
            mDocument.SetUpdateLog(mLogType, "[Connected]");

            // 접속 이벤트 핸들러 호출
            OnConnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 연결 해제 이벤트 핸들러
        /// </summary>
        private void Communication_OnDisconected()
        {
            mDocument.SetUpdateLog(mLogType, "[Disconnected]");

            // 접속 해제 이벤트 핸들러 호출
            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 수신된 데이터 송신
        /// </summary>
        /// <param name="objData"></param>
        private void Communication_OnReceiveData(CDeviceCommunicationAbstract.CReceiveData objData)
        {
            // JAVAS에서 Packet을 Send 할 때 1Byte('\0')를 추가로 붙여보내서 Rawdata를 받아 STX, ETX를 찾은 뒤 문자열로 인코딩하는 방식으로 변경함
            bool bFindStx = mReceiveBuffer.Length > 0;
            int recevicedByteCount = objData.iReceiveByteCount;
            int receiveCount = 0;
            for (int i = 0; i < recevicedByteCount; i++)
            {
                if (bFindStx == false)
                {
                    if (objData.byteReceiveData[i] == STX)
                    {
                        bFindStx = true;

                        // Buffer Clear
                        mReceiveBuffer.Seek(0, SeekOrigin.Begin);
                        mReceiveBuffer.SetLength(0);

                        mReceiveBuffer.WriteByte(objData.byteReceiveData[i]);
                    }
                }
                else
                {
                    if (objData.byteReceiveData[i] != ETX)
                    {
                        mReceiveBuffer.WriteByte(objData.byteReceiveData[i]);
                    }
                    else
                    {
                        bFindStx = false;
                        // STX와 ETX를 제외한 데이터로 객체를 다시 만듦
                        string receivePacket = mCommunication.Encoding.GetString(mReceiveBuffer.ToArray()).Replace(STX, ' ').TrimStart();
                        byte[] receiveBytes = mCommunication.Encoding.GetBytes(receivePacket);
                        var objReceiveData = new CReceiveData()
                        {
                            byteReceiveData = receiveBytes,
                            iByteLength = receiveBytes.Length,
                            strData = receivePacket,
                            LogType = mLogType
                        };
                        // Buffer Clear
                        mReceiveBuffer.Seek(0, SeekOrigin.Begin);
                        mReceiveBuffer.SetLength(0);

                        // Logging
                        mDocument.SetUpdateLog(mLogType, string.Format("[RCV{0}] : {1}", ++receiveCount, objReceiveData.strData.Trim()));

                        // Parsing
                        if (ReceivedDataParsing(ref objReceiveData) == false)
                        {
                            continue;
                        }

                        // Callback
                        foreach (var receiveDataCallback in mReceiveDataCallbacks)
                        {
                            if (objReceiveData.eReceiveData == EReceiveDataType.UnkownType)
                            {
                                break;
                            }
                            if (receiveDataCallback == null)
                            {
                                continue;
                            }
                            receiveDataCallback.Invoke(objReceiveData);
                        }
                    }
                }
            }

            if (mReceiveBuffer.Length > 0)
            {
                mDocument.SetUpdateLog(mLogType, $"[BUFFERING] {mReceiveBuffer.Length}");
            }
        }

        private bool ReceivedDataParsing(ref CReceiveData objReceiveData)
        {
            bool bReturn = false;
            objReceiveData.eReceiveData = EReceiveDataType.UnkownType;

            do
            {
                // HEADER_LENGTH(8byte) =  RESERVED0(2byte) + RESERVED1(2byte) + LENGTH(4byte)
                if (objReceiveData.iByteLength < HEADER_LENGTH)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] receivedDataParsing,데이터 길이가 해더보다 짧음 (Length:{0})", objReceiveData.iByteLength));
                    break;
                }

                int hearderlessDataLength;
                if (int.TryParse(objReceiveData.strData.Substring(RESERVED0.Length + RESERVED1.Length, 4), out hearderlessDataLength) == false)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] receivedDataParsing,데이터 길이를 파싱 하는데 실패 했습니다."));
                    break;
                }

                if (hearderlessDataLength > (objReceiveData.strData.Length - HEADER_LENGTH))
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] receivedDataParsing,수신한 데이터 길이가 파싱한 데이터 길이보다 작습니다. (ParseLength:{0}, ReceiveLength:{1})", hearderlessDataLength, objReceiveData.strData.Length - HEADER_LENGTH));
                    break;
                }

                string headerlessData = objReceiveData.strData
                    .Substring(HEADER_LENGTH, hearderlessDataLength)
                    .Replace(ETX, ' ')
                    .Replace('\u0000', ' ')
                    .Trim();

                string[] messageToken = headerlessData.Split(new string[] { SEPARATOR }, StringSplitOptions.None);
                if (messageToken.Length < 3)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] receivedDataParsing,메시지 토큰이 3보다 작음 (Length:{0})", messageToken.Length));
                    break;
                }

                string responseProtocolFrame = string.Empty;
                int dataStartIndex = 3;
                // 응답
                if (
                    OTM == messageToken[0]
                    && RECEIVE == messageToken[1]
                    )
                {
                    switch (messageToken[2])
                    {
                        case PROTOCOL1_GRAB:
                            dataStartIndex = 4;
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_START == messageToken[3]
                                )
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.GrabStartAcknowledge;
                            }
                            break;

                        case PROTOCOL1_AMO_GRAB:
                            dataStartIndex = 4;
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_START == messageToken[3]
                                )
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.AmoGrabStartAcknowledge;
                            }
                            else if (
                                messageToken.Length > 3
                                && PROTOCOL2_END == messageToken[3]
                                )
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.AmoGrabEndAcknowledge;
                            }
                            break;
                        //case PROTOCOL1_LIGHT:
                        //    dataStartIndex = 4;
                        //    if (
                        //        messageToken.Length > 3
                        //        && PROTOCOL2_VALUE == messageToken[3]
                        //        )
                        //    {
                        //        objReceiveData.eReceiveData = EReceiveDataType.LightIntensityAcknowledge;
                        //    }
                        //    break;
                        case PROTOCOL1_LIGHT:
                            dataStartIndex = 4;
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_VALUE == messageToken[3]
                                )
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.LightIntensityAcknowledge;
                            }
                            break;
                        case PROTOCOL1_LASER:
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_CHECK == messageToken[3]
                                )
                            {
                                dataStartIndex = 4;
                                objReceiveData.eReceiveData = EReceiveDataType.LaserCheckAcknowledge;
                            }
                            else if (
                                messageToken.Length > 3
                                && PROTOCOL2_OFF == messageToken[3]
                                )
                            {
                                dataStartIndex = 4;
                                objReceiveData.eReceiveData = EReceiveDataType.LaserInterlockAcknowledge;
                            }
                            break;
                        case PROTOCOL1_TEMP:
                            dataStartIndex = 4;
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_VALUE == messageToken[3]
                                )
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.LightTemperatureAcknowledge;
                            }
                            break;
                        case PROTOCOL1_TIMESYNC:
                            objReceiveData.eReceiveData = EReceiveDataType.TimeSyncAcknowledge;
                            break;
                        case PROTOCOL1_STATE:
                            objReceiveData.eReceiveData = EReceiveDataType.StateCheckAcknowledge;
                            break;
                        case PROTOCOL1_ERROR:
                            objReceiveData.eReceiveData = EReceiveDataType.ErrorMessageAcknowledge;
                            break;
                        case PROTOCOL1_INIT:
                            objReceiveData.eReceiveData = EReceiveDataType.InitializeMessageAcknowledge;
                            break;
                        case PROTOCOL1_VACUUM:
                            objReceiveData.eReceiveData = EReceiveDataType.VacuumAcknowledge;
                            break;
                        case PROTOCOL1_VALIDATION:
                            objReceiveData.eReceiveData = EReceiveDataType.RecipeValidationDataRequest;
                            responseProtocolFrame = string.Format("{0}", messageToken[2]);
                            break;
                        case PROTOCOL1_RECIPE:
                            dataStartIndex = 4;
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_VALUE == messageToken[3]
                                )
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.CurrentRecipeInformationRequestAcknowledge;
                            }
                            break;
                        case PROTOCOL1_MCR:
                            objReceiveData.eReceiveData = EReceiveDataType.CellDataSyncAcknowledge;
                            break;
                    }
                }
                // 요청
                else if (
                    OTM == messageToken[0]
                    && SEND == messageToken[1]
                    )
                {
                    switch (messageToken[2])
                    {
                        case PROTOCOL1_GRAB:
                            dataStartIndex = 4;
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_END == messageToken[3]
                                )
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.GrabEndRequest;
                                responseProtocolFrame = string.Format("{0}.{1}", messageToken[2], messageToken[3]);
                            }
                            break;
                        case PROTOCOL1_INSPECT:
                            dataStartIndex = 4;
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_RESULT == messageToken[3]
                                )
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.ResultDataRequest;
                                responseProtocolFrame = string.Format("{0}.{1}", messageToken[2], messageToken[3]);
                            }
                            break;
                        case PROTOCOL1_TOTALRESULT:
                            objReceiveData.eReceiveData = EReceiveDataType.TotalResultDataRequest;
                            responseProtocolFrame = string.Format("{0}", messageToken[2]);
                            break;
                        case PROTOCOL1_ERROR:
                            objReceiveData.eReceiveData = EReceiveDataType.ErrorMessageRequest;
                            responseProtocolFrame = string.Format("{0}", messageToken[2]);
                            break;
                        case PROTOCOL1_LIGHT:
                            objReceiveData.eReceiveData = EReceiveDataType.LightForDvListRequest;
                            responseProtocolFrame = string.Format("{0}", messageToken[2]);
                            break;
                        case PROTOCOL1_LASER:
                            if (
                                messageToken.Length > 3
                                && PROTOCOL2_CHECK == messageToken[3]
                                )
                            {
                                dataStartIndex = 4;
                                objReceiveData.eReceiveData = EReceiveDataType.CheckLaserAbleRequest;
                                responseProtocolFrame = string.Format("{0}.{1}", messageToken[2], messageToken[3]);
                            }
                            else
                            {
                                objReceiveData.eReceiveData = EReceiveDataType.LaserStatusDataRequest;
                                responseProtocolFrame = string.Format("{0}", messageToken[2]);
                            }
                            break;
                        case PROTOCOL1_ALIGN:
                            objReceiveData.eReceiveData = EReceiveDataType.AlignDataRequest;
                            responseProtocolFrame = string.Format("{0}", messageToken[2]);
                            break;
                        case PROTOCOL1_AUTOREVIEW:
                            objReceiveData.eReceiveData = EReceiveDataType.AutoReviewDataRequest;
                            responseProtocolFrame = string.Format("{0}", messageToken[2]);
                            break;
                        case PROTOCOL1_SWERROR:
                            objReceiveData.eReceiveData = EReceiveDataType.JavasSoftwareErrorRequest;
                            responseProtocolFrame = string.Format("{0}", messageToken[2]);
                            break;
                    }
                }

                if (objReceiveData.eReceiveData == EReceiveDataType.UnkownType)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] receivedDataParsing,지원하지 않는 타입 (Type:{0}.{1}.{2})", messageToken[0], messageToken[1], messageToken[2]));
                    break;
                }

                // 전송방향과 송/회신정보 프로토콜을 제외한 순수 데이터만 strReceiveData에 넣는다
                var sb = new StringBuilder();
                for (int i = dataStartIndex; i < messageToken.Length; i++)
                {
                    sb.AppendFormat("{0}.", messageToken[i]);
                }
                if (
                    sb.Length > 0
                    && '.' == sb[sb.Length - 1]
                    )
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                objReceiveData.strReceiveData = sb.ToString();

                // 요청에대한 응답 메시지 전송
                if (string.IsNullOrWhiteSpace(responseProtocolFrame) == false)
                {
                    SendMessage(string.Format(
                        "{0}.{1}.{2}.{3}",
                        MTO,
                        RECEIVE,
                        responseProtocolFrame,
                        sb.ToString()
                        )
                        );
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 메시지 프래임을 만들어 전송
        /// </summary>
        /// <param name="message">메시지</param>
        /// <returns>전송 결과</returns>
        private bool SendMessage(string message)
        {
            message = message
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();
            // STX(1byte) + RESERVED0(2byte) + RESERVED1(2byte) + LENGTH(4byte) + DATA(nbyte) + ETX(1byte)
            var messageFrame = string.Format("{0}{1}{2}{3:D4}{4}{5}", STX, RESERVED0, RESERVED1, message.Length, message, ETX);
            if (mCommunication.HLSend(messageFrame) == false)
            {
                mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] sendMessage,Message='{0}'", messageFrame).Replace(ETX, ' ').Trim());
                return false;
            }
            mDocument.SetUpdateLog(mLogType, string.Format("[SENT] : {0}", messageFrame).Replace(ETX, ' ').Trim());

            return true;
        }
    }
}

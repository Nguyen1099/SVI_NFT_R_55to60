using ENC.Device.Communication;
using SVI_NFT_R;
using SVI_NFT_R.DEVICE.Align;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace HLDevice
{
    public class CDeviceAlignInterface
    {
        /// <summary>
        /// 수신 데이터
        /// </summary>
        public class CReceiveData
        {
            public string ID { get; set; } = string.Empty;

            //////////////////////////////////////////////////////////////////////////
            // 수신 데이터
            public string strData = string.Empty;
            public byte[] byteReceiveData = new byte[8192];
            public int iByteLength = 0;

            //////////////////////////////////////////////////////////////////////////
            // 파싱 데이터
            public EReceiveDataType eReceiveData = EReceiveDataType.UnkownType;

            /// <summary>
            /// 전송방향과 송/회신정보, 프로토콜을 제외한 순수 데이터
            /// </summary>
            public string strReceiveData = string.Empty;

            public CDefine.ELogType LogType { get; set; }
        }

        /// <summary>
        /// 수신 데이터 타입
        /// </summary>
        public enum EReceiveDataType
        {
            UnkownType = -1,

            //////////////////////////////////////////////////////////////////////////
            // 응답 수신
            /// <summary>"MTO.SEND.ALIGN_START." => "OTM.RECV.ALIGN_START."</summary>
            AlignStartAcknowledge = 0,

            /// <summary>"MTO.SEND.ALIGN_CAL_MOVE_COMPLETE" => "OTM.RECV.ALIGN_CAL_MOVE_COMPLETE"</summary>
            CalibrationMoveCompleteAcknowledge,

            /// <summary>"MTO.SEND.OPER_ERROR" => "OTM.RECV.OPER_ERROR"</summary>
            EquipmentErrorAcknowledge,

            /// <summary>"MTO.SEND.TIMESYNC." => "OTM.RECV.TIMESYNC."</summary>
            TimeSyncAcknowledge,

            /// <summary>"MTO.SEND.MODEL_LIST" => "OTM.RECV.MODEL_LIST"</summary>
            ModelListAcknowledge,

            /// <summary>"MTO.SEND.LIGHT_TEMPERATURE" => "OTM.RECV.LIGHT_TEMPERATURE"</summary>
            LightTemperatureAcknowledge,

            /// <summary>"MTO.SEND.LIGHT_LEVEL" => "OTM.RECV.LIGHT_LEVEL"</summary>
            LightLevelAcknowledge,

            //////////////////////////////////////////////////////////////////////////
            // 요청 수신
            /// <summary>"OTM.SEND.ALIGN_COMPLETE." => "MTO.RECV.ALIGN_COMPLETE."</summary>
            AlignCompleteRequest,

            /// <summary>"OTM.SEND.ALIGN_RESULT_XYT." => "MTO.RECV.ALIGN_RESULT_XYT."</summary>
            AlignResultRequest,

            /// <summary>"OTM.SEND.ALIGN_CAL_START" => "MTO.RECV.ALIGN_CAL_START"</summary>
            CalibrationStartRequest,

            /// <summary>"OTM.SEND.ALIGN_CAL_MOVE_HOME" => "MTO.RECV.ALIGN_CAL_MOVE_HOME"</summary>
            CalibrationMoveHomeRequest,

            /// <summary>"OTM.SEND.ALIGN_CAL_MOVE_XYT." => "MTO.RECV.ALIGN_CAL_MOVE_XYT."</summary>
            CalibrationMoveOffsetRequest,

            /// <summary>"OTM.SEND.ALIGN_CAL_COMPLETE" => "MTO.RECV.ALIGN_CAL_COMPLETE"</summary>
            CalibrationCompleteRequest,

            /// <summary>"OTM.SEND.ALIGN_ERROR" => "MTO.RECV.ALIGN_ERROR"</summary>
            AlignErrorRequest,

            /// <summary>"OTM.SEND.MODEL_LIST_RESULT." => "MTO.RECV.MODEL_LIST_RESULT."</summary>
            ModelListResultRequest,

            /// <summary>"OTM.SEND.LIGHT_TEMPERATURE_RESULT." => "MTO.RECV.LIGHT_TEMPERATURE_RESULT."</summary>
            LightTemperatureResultRequest,

            /// <summary>"OTM.SEND.LIGHT_LEVEL_RESULT." => "MTO.RECV.LIGHT_LEVEL_RESULT."</summary>
            LightLevelResultRequest,
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
        private CallBackFuntionReceiveData mReceiveDataCallback = null;

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

        public const string SEPARATOR = ".";

        private const string SEND = "SEND";
        private const string RECEIVE = "RECV";
        private const string MTO = "MTO";
        private const string OTM = "OTM";
        private const string RESERVED0 = "00";
        private const string RESERVED1 = "00";
        private const char STX = '\u0005';
        private const char ETX = '\u000A';

        private const string PROTOCOL1_ALIGN_START = "ALIGN_START";
        private const string PROTOCOL1_ALIGN_COMPLETE = "ALIGN_COMPLETE";
        private const string PROTOCOL1_ALIGN_RESULT_XYT = "ALIGN_RESULT_XYT";
        private const string PROTOCOL1_ALIGN_CAL_START = "ALIGN_CAL_START";
        private const string PROTOCOL1_ALIGN_CAL_MOVE_HOME = "ALIGN_CAL_MOVE_HOME";
        private const string PROTOCOL1_ALIGN_CAL_MOVE_XYT = "ALIGN_CAL_MOVE_XYT";
        private const string PROTOCOL1_ALIGN_CAL_MOVE_COMPLETE = "ALIGN_CAL_MOVE_COMPLETE";
        private const string PROTOCOL1_ALIGN_CAL_COMPLETE = "ALIGN_CAL_COMPLETE";
        private const string PROTOCOL1_ALIGN_ERROR = "VISION_ERROR";
        private const string PROTOCOL1_OPER_ERROR = "OPER_ERROR";
        private const string PROTOCOL1_TIMESYNC = "TIMESYNC";
        private const string PROTOCOL1_MODEL_LIST = "MODEL_LIST";
        private const string PROTOCOL1_MODEL_LIST_RESULT = "MODEL_LIST_RESULT";
        private const string PROTOCOL1_LIGHT_TEMPERATURE = "LIGHT_TEMPERATURE";
        private const string PROTOCOL1_LIGHT_TEMPERATURE_RESULT = "LIGHT_TEMPERATURE_RESULT";
        private const string PROTOCOL1_LIGHT_LEVEL = "LIGHT_LEVEL";
        private const string PROTOCOL1_LIGHT_LEVEL_RESULT = "LIGHT_LEVEL_RESULT";

        private const int HEADER_LENGTH = 8;
        private const int RETRY_COUNT = 1;

        private CDefine.ELogType mLogType;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objDocument">도큐먼트 객체</param>
        /// <param name="objInitializeParameter">초기화 파라메터</param>
        /// <returns>초기화 결과</returns>
        public bool HLInitialize(CDocument objDocument, CConfig.CAlignInitializeParameter objInitializeParameter, CDefine.ELogType logType)
        {
            bool bReturn = false;

            do
            {
                mDocument = objDocument;
                mLogType = logType;

#if SCT_ALIGN
                mCommunication = new CDeviceCommunication(new CDeviceCommunicationSocketClient());
#else

                if (objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_OFF)
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
                    eDataEncoding = CDeviceCommunicationAbstract.CInitializeParameter.EDataEncoding.ENCODING_UTF8,
                    eType = (CDeviceCommunicationAbstract.CInitializeParameter.EType)objInitializeParameter.eType,
                    strSocketIPAddress = objInitializeParameter.strSocketIPAddress,
                    iSocketPortNumber = objInitializeParameter.iSocketPortNumber
                };
                if (mCommunication.HLInitialize(objParameter) == false)
                {
                    break;
                }

                // 데이터 수신 이벤트 핸들러 연결
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

        public void SetCallbackFunction(CallBackFuntionReceiveData callbackFunction)
        {
            mReceiveDataCallback = callbackFunction;
        }

        public bool HLSendAlignStartRequest(SendData.AlignStartData data, EWait waitResult = EWait.ForReceive, int waitTimeout = 5000)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}.{3}",
                    MTO,
                    SEND,
                    PROTOCOL1_ALIGN_START,
                    data.ToString()
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.AlignStartAcknowledge] = false;
                mReceiveFlag[EReceiveDataType.AlignCompleteRequest] = false;
                mReceiveFlag[EReceiveDataType.AlignResultRequest] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.AlignStartAcknowledge, waitTimeout) == false
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

        public bool HLSendCalibrationMoveCompleteRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}",
                    MTO,
                    SEND,
                    PROTOCOL1_ALIGN_CAL_MOVE_COMPLETE
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.CalibrationMoveCompleteAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.CalibrationMoveCompleteAcknowledge, waitTimeout) == false
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

        public bool HLSendErrorReportRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}",
                    MTO,
                    SEND,
                    PROTOCOL1_OPER_ERROR
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.EquipmentErrorAcknowledge] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.EquipmentErrorAcknowledge, waitTimeout) == false
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

        public bool HLSendTimeSyncRequest(DateTime dateTime)
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

                    if (WaitReceiveFlag(EReceiveDataType.TimeSyncAcknowledge) == false)
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

        public bool HLSendModelListRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}",
                    MTO,
                    SEND,
                    PROTOCOL1_MODEL_LIST
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.ModelListAcknowledge] = false;
                mReceiveFlag[EReceiveDataType.ModelListResultRequest] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.ModelListAcknowledge, waitTimeout) == false
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

        public bool HLSendLightTemperatureRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}",
                    MTO,
                    SEND,
                    PROTOCOL1_LIGHT_TEMPERATURE
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.LightTemperatureAcknowledge] = false;
                mReceiveFlag[EReceiveDataType.LightTemperatureResultRequest] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.LightTemperatureAcknowledge, waitTimeout) == false
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

        public bool HLSendLightLevelRequest(EWait waitResult = EWait.ForReceive, int waitTimeout = 5000)
        {
            bool bResult = false;

            do
            {
                string message = string.Format("{0}.{1}.{2}",
                    MTO,
                    SEND,
                    PROTOCOL1_LIGHT_LEVEL
                    );

                int tryCount = 0;
                mReceiveFlag[EReceiveDataType.LightLevelAcknowledge] = false;
                mReceiveFlag[EReceiveDataType.LightLevelResultRequest] = false;
                do
                {
                    if (SendMessage(message) == false)
                    {
                        tryCount++;
                        continue;
                    }

                    if (EWait.ForReceive == waitResult
                        && WaitReceiveFlag(EReceiveDataType.LightLevelAcknowledge, waitTimeout) == false
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

        public bool GetReceiveFlag(EReceiveDataType waitType) => mReceiveFlag[waitType];

        /// <summary>
        /// 메세지 기다림 (동기를 맞춰야 할 새로운 프로토콜이 생긴다면 맴버 변수를 추가하여 사용해야함)
        /// </summary>
        /// <param name="waitType">대기 플래그 종류</param>
        /// <param name="iTimeOut">타임 아웃 시간 (단위 : ms)</param>
        /// <returns></returns>
        public bool WaitReceiveFlag(EReceiveDataType waitType, int iTimeOut = 5000)
        {
            bool bReturn = false;
            do
            {
                if (SpinWait.SpinUntil(() => mReceiveFlag[waitType], iTimeOut) == false)
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
                        var receiveDataCallback = mReceiveDataCallback;
                        if (objReceiveData.eReceiveData != EReceiveDataType.UnkownType
                            && receiveDataCallback != null
                            )
                        {
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
                if (objReceiveData.strData.Length < HEADER_LENGTH)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] ReceivedDataParsing,데이터 길이가 해더보다 짧음 (Length:{0})", objReceiveData.strData.Length));
                    break;
                }

                int reservedDataLength = RESERVED0.Length + RESERVED1.Length;
                int headerlessDataLength;
                if (int.TryParse(objReceiveData.strData.Substring(reservedDataLength, 4), out headerlessDataLength) == false)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] ReceivedDataParsing,데이터 길이를 파싱 하는데 실패 했습니다."));
                    break;
                }

                if (headerlessDataLength > (objReceiveData.strData.Length - HEADER_LENGTH))
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] ReceivedDataParsing,수신한 데이터 길이가 파싱한 데이터 길이보다 작습니다. (ParseLength:{0}, ReceiveLength:{1})", headerlessDataLength, objReceiveData.strData.Length - HEADER_LENGTH));
                    break;
                }

                string[] messageToken = objReceiveData.strData
                    .Substring(HEADER_LENGTH, headerlessDataLength)
                    .Trim()
                    .Split(new string[] { SEPARATOR }, StringSplitOptions.None);
                if (messageToken.Length < 3)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] ReceivedDataParsing,메시지 토큰이 3보다 작음 (Length:{0})", messageToken.Length));
                    break;
                }

                string responseProtocolFrame = string.Empty;
                int dataStartIndex = 3;
                // 응답
                if (
                    messageToken[0] == OTM
                    && messageToken[1] == RECEIVE
                    )
                {
                    switch (messageToken[2])
                    {
                        case PROTOCOL1_ALIGN_START:
                            objReceiveData.eReceiveData = EReceiveDataType.AlignStartAcknowledge;
                            break;

                        case PROTOCOL1_ALIGN_CAL_MOVE_COMPLETE:
                            objReceiveData.eReceiveData = EReceiveDataType.CalibrationMoveCompleteAcknowledge;
                            break;

                        case PROTOCOL1_OPER_ERROR:
                            objReceiveData.eReceiveData = EReceiveDataType.EquipmentErrorAcknowledge;
                            break;

                        case PROTOCOL1_TIMESYNC:
                            objReceiveData.eReceiveData = EReceiveDataType.TimeSyncAcknowledge;
                            break;

                        case PROTOCOL1_MODEL_LIST:
                            objReceiveData.eReceiveData = EReceiveDataType.ModelListAcknowledge;
                            break;

                        case PROTOCOL1_LIGHT_TEMPERATURE:
                            objReceiveData.eReceiveData = EReceiveDataType.LightTemperatureAcknowledge;
                            break;

                        case PROTOCOL1_LIGHT_LEVEL:
                            objReceiveData.eReceiveData = EReceiveDataType.LightLevelAcknowledge;
                            break;
                    }
                }
                // 요청
                else if (
                    messageToken[0] == OTM
                    && messageToken[1] == SEND
                    )
                {
                    switch (messageToken[2])
                    {
                        case PROTOCOL1_ALIGN_COMPLETE:
                            objReceiveData.eReceiveData = EReceiveDataType.AlignCompleteRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_ALIGN_RESULT_XYT:
                            objReceiveData.eReceiveData = EReceiveDataType.AlignResultRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_ALIGN_CAL_START:
                            objReceiveData.eReceiveData = EReceiveDataType.CalibrationStartRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_ALIGN_CAL_MOVE_HOME:
                            objReceiveData.eReceiveData = EReceiveDataType.CalibrationMoveHomeRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_ALIGN_CAL_MOVE_XYT:
                            objReceiveData.eReceiveData = EReceiveDataType.CalibrationMoveOffsetRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_ALIGN_CAL_COMPLETE:
                            objReceiveData.eReceiveData = EReceiveDataType.CalibrationCompleteRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_ALIGN_ERROR:
                            objReceiveData.eReceiveData = EReceiveDataType.AlignErrorRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_MODEL_LIST_RESULT:
                            objReceiveData.eReceiveData = EReceiveDataType.ModelListResultRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_LIGHT_TEMPERATURE_RESULT:
                            objReceiveData.eReceiveData = EReceiveDataType.LightTemperatureResultRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;

                        case PROTOCOL1_LIGHT_LEVEL_RESULT:
                            objReceiveData.eReceiveData = EReceiveDataType.LightLevelResultRequest;
                            responseProtocolFrame = string.Join(SEPARATOR, messageToken.Skip(2).Take(1));
                            break;
                    }
                }

                if (objReceiveData.eReceiveData == EReceiveDataType.UnkownType)
                {
                    mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] ReceivedDataParsing,지원하지 않는 타입 (Type:{0})", string.Join(SEPARATOR, messageToken.Take(3))));
                    break;
                }

                // 전송방향과 송/회신정보 프로토콜을 제외한 순수 데이터만 strReceiveData에 넣는다
                string strReceiveData = string.Join(SEPARATOR, messageToken.Skip(dataStartIndex).Take(messageToken.Length - dataStartIndex));
                objReceiveData.strReceiveData = strReceiveData;
                if (string.IsNullOrWhiteSpace(strReceiveData) == false)
                {
                    strReceiveData = $".{strReceiveData}";
                }

                // 요청에대한 응답 메시지 전송
                if (string.IsNullOrWhiteSpace(responseProtocolFrame) == false)
                {
                    SendMessage(string.Format(
                        "{0}.{1}.{2}{3}",
                        MTO,
                        RECEIVE,
                        responseProtocolFrame,
                        strReceiveData
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
            string messageFrame = $"{STX}{RESERVED0}{RESERVED1}{message.Length:0000}{message}{ETX}";
            if (mCommunication.HLSend(messageFrame) == false)
            {
                mDocument.SetUpdateLog(mLogType, string.Format("[FAILED] SendMessage,Message='{0}'", messageFrame).Replace(ETX, ' ').Trim());
                return false;
            }
            mDocument.SetUpdateLog(mLogType, string.Format("[SENT] : {0}", messageFrame).Replace(ETX, ' ').Trim());
            return true;
        }
    }
}
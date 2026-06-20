using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMDateAndTimeSetRequest : CCIMAbstract
    {
        /// <summary>
        /// 로컬 시간 변경
        /// </summary>
        protected struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }
        // 데이터
        DateAndTimeSetReply m_objDateAndTimeSetReply;
        DateAndTimeSetRequest m_objDateAndTimeSetRequest;
        [DllImport("kernel32.dll")]
        private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32.dll")]
        private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32.dll")]
        protected extern static uint SetLocalTime(ref SYSTEMTIME lpSystemTime);

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMDateAndTimeSetRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
        {
            m_objDocument = (CDocument)objCIMIntiialize.objDocument as CDocument;
            m_objCommunicator = (urLinkDllCommunicator)objCIMIntiialize.objCommunicator as urLinkDllCommunicator;
            m_strEquipmentID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            Initialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        protected override bool Initialize()
        {
            bool bResult = false;
            do
            {
                // Receive Event 등록
                //m_objCommunicator.OnReceiveRequest += Communicator_OnReceiveRequest;

                // 쓰레드 종료 Flag
                m_bThreadExit = false;
                // 쓰레드 객체 생성.
                m_ThreadProcess = new System.Threading.Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start(this);
                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                // 데이터 생성.
                m_objDateAndTimeSetReply = new DateAndTimeSetReply();
                m_objDateAndTimeSetRequest = new DateAndTimeSetRequest();
                m_objDateAndTimeSetReply.EQPID = m_strEquipmentID;
                m_objDateAndTimeSetRequest.EQPID = m_strEquipmentID;
                m_bReceiveData = false;

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 리시브 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Communicator_OnReceiveRequest(object sender, ReqMsgInfo e)
        {
            try
            {
                XmlNodeList xmlMsgnode = e.ReceviedXml.SelectNodes("/MESSAGE");
                foreach (XmlNode xn in xmlMsgnode)
                {
                    string eqpid = xn["EQPID"].InnerText;
                    string name = xn["NAME"].InnerText;
                    string transactionno = xn["TRANSACTIONNO"].InnerText;
                    if ("DATEANDTIMESETREQUEST" == name)
                    {
                        XmlNode BodyNode = xn.SelectSingleNode("BODY");
                        if (BodyNode.HasChildNodes)
                        {
                            foreach (XmlNode item in BodyNode.ChildNodes)
                            {
                                string tagName = item.Name;
                                string value = item.InnerText;
                                if ("TIME" == tagName)
                                {
                                    m_objDateAndTimeSetRequest.EQPID = eqpid;
                                    m_objDateAndTimeSetRequest.NAME = name;
                                    m_objDateAndTimeSetRequest.TRANSACTIONNO = transactionno;
                                    m_objDateAndTimeSetRequest.BODY.TIME = value;
                                }
                                else
                                {
                                    m_objDateAndTimeSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EDateAndTimeSetReply.DATE_AND_TIME_SET_REPLY_FORMET_ERROR); // 시스템 시간 변경 실패
                                }
                            }
                        }
                        else
                        {
                            m_objDateAndTimeSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EDateAndTimeSetReply.DATE_AND_TIME_SET_REPLY_FORMET_ERROR); // 시스템 시간 변경 실패
                        }
                        m_bReceiveData = true;
                        m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[RECV]\n" + e.RecevedXml_2);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void DeInitialize()
        {
            m_bThreadExit = true;
            m_ThreadProcess.Join();
        }

        /// <summary>
        /// 쓰레드 상태
        /// </summary>
        /// <returns></returns>
        public override CCIMDefine.EProcessState GetStatus()
        {
            return m_eStatus;
        }

        /// <summary>
        /// 리시브 된 데이터
        /// </summary>
        /// <returns></returns>
        public DateAndTimeSetRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objDateAndTimeSetRequest;
        }

        /// <summary>
        /// 리스브 상태 확인
        /// </summary>
        /// <returns></returns>
        protected override bool IsStatus()
        {
            bool bResult = false;
            do
            {
                if (false == m_bReceiveData) break;

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 작업 수행
        /// </summary>
        /// <returns></returns>
        protected override bool DoProcess()
        {
            bool bResult = false;
            do
            {
                m_bReceiveData = false;

                if (m_objDateAndTimeSetRequest.BODY.TIME.Length == 14)
                {
                    SYSTEMTIME systemTime = new SYSTEMTIME();
                    systemTime.wYear = ushort.Parse(m_objDateAndTimeSetRequest.BODY.TIME.Substring(0, 4));     // 년
                    systemTime.wMonth = ushort.Parse(m_objDateAndTimeSetRequest.BODY.TIME.Substring(4, 2));    // 월
                    systemTime.wDay = ushort.Parse(m_objDateAndTimeSetRequest.BODY.TIME.Substring(6, 2));        // 일
                    systemTime.wHour = ushort.Parse(m_objDateAndTimeSetRequest.BODY.TIME.Substring(8, 2));       // 시
                    systemTime.wMinute = ushort.Parse(m_objDateAndTimeSetRequest.BODY.TIME.Substring(10, 2));    // 분
                    systemTime.wSecond = ushort.Parse(m_objDateAndTimeSetRequest.BODY.TIME.Substring(12, 2));    // 초
                    m_objDateAndTimeSetReply.TRANSACTIONNO = m_objDateAndTimeSetRequest.TRANSACTIONNO;
                    DateTime setTime = new DateTime(systemTime.wYear, systemTime.wMonth, systemTime.wDay, systemTime.wHour, systemTime.wMinute, systemTime.wSecond);
                    // 받은 메시지 시간으로 시스템 시간 변경
                    if (1 == SetLocalTime(ref systemTime))
                        m_objDateAndTimeSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EDateAndTimeSetReply.DATE_AND_TIME_SET_REPLY_ACCEPTED); // 시스템 시간 변경 성공
                    else
                        m_objDateAndTimeSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EDateAndTimeSetReply.DATE_AND_TIME_SET_REPLY_FORMET_ERROR); // 시스템 시간 변경 실패

                    // Vision 검사기 시간 동기화 요청
                    foreach (var item in m_objDocument.m_objProcessMain.m_objInspInterfaces)
                    {
                        if (item.Value.HLIsConnected() == true)
                        {
                            item.Value.HLSendTimeSync(setTime);
                        }
                    }
                    // Align 시간 동기화 요청
                    foreach (var item in m_objDocument.m_objProcessMain.m_objAlignInterfaces)
                    {
                        if (item.Value.HLIsConnected() == true)
                        {
                            item.Value.HLSendTimeSyncRequest(setTime);
                        }
                    }
                }
                else
                {
                    m_objDateAndTimeSetReply.BODY.TIACK = string.Format("{0}", (int)CCIMDefine.EDateAndTimeSetReply.DATE_AND_TIME_SET_REPLY_FORMET_ERROR); // 시스템 시간 변경 실패
                }

                // Reply 보고
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetDateAndTimeSetReply(m_objDateAndTimeSetReply);

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcess(object state)
        {
            CProcessCIMDateAndTimeSetRequest pThis = (CProcessCIMDateAndTimeSetRequest)state;

            while (false == pThis.m_bThreadExit)
            {
                if (pThis.IsStatus())
                {
                    if (pThis.DoProcess())
                    {
                        pThis.m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}

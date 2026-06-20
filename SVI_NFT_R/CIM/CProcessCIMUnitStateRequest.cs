using System;
using System.Threading;
using System.Xml;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMUnitStateRequest : CCIMAbstract
    {
        UnitStateRequest m_objUnitStateRequest;
        UnitStateReply m_objUnitStateReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMUnitStateRequest(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_objUnitStateRequest = new UnitStateRequest();
                m_objUnitStateReply = new UnitStateReply();
                m_objUnitStateRequest.EQPID = m_strEquipmentID;
                m_objUnitStateReply.EQPID = m_strEquipmentID;
                // 이장비는 유닛이 하나로 취급한다.
                m_objUnitStateReply.BODY.UNITS = new UnitStateReply._UnitStateReply.UNIT[Enum.GetNames(typeof(CCIMDefine.EUnit)).Length];

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
                    if ("UNITSTATEREQUEST" == name)
                    {
                        m_objUnitStateRequest.EQPID = eqpid;
                        m_objUnitStateRequest.NAME = name;
                        m_objUnitStateRequest.TRANSACTIONNO = transactionno;
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
        public UnitStateRequest GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return m_objUnitStateRequest;
        }

        /// <summary>
        /// 리시브 상태 확인
        /// </summary>
        /// <returns></returns>
        protected override bool IsStatus()
        {
            bool bResult = false;
            do
            {
                if (false == m_bReceiveData)
                    break;

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
                // 현재 설비 상태 보내기.
                m_objUnitStateReply.TRANSACTIONNO = m_objUnitStateRequest.TRANSACTIONNO;
                m_objUnitStateReply.BODY.AVAILABILITYSTATE = string.Format("{0}", (int)m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objUnitStateReply.BODY.INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objUnitStateReply.BODY.MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objUnitStateReply.BODY.RUNSTATE = string.Format("{0}", (int)m_objDocument.m_eRunState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objUnitStateReply.BODY.FRONTSTATE = string.Format("{0}", (int)m_objDocument.m_eFrontState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objUnitStateReply.BODY.REARSTATE = string.Format("{0}", (int)m_objDocument.m_eRearState[(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                m_objUnitStateReply.BODY.PP_SPLSTATE = string.Format("{0}", (int)m_objDocument.m_ePP_SPLState);
                m_objUnitStateReply.BODY.UNITS[0].UNITID = "0";
                m_objUnitStateReply.BODY.UNITS[0].AVAILABILITYSTATE = m_objUnitStateReply.BODY.AVAILABILITYSTATE;
                for (int i = 0; i < Enum.GetNames(typeof(CCIMDefine.EUnit)).Length; i++)
                {
                    m_objUnitStateReply.BODY.UNITS[i].UNITID = m_objDocument.m_objProcessCIM.GetUnitId((CCIMDefine.EUnit)i);
                    m_objUnitStateReply.BODY.UNITS[i].AVAILABILITYSTATE = string.Format("{0}", (int)CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_UP);
                    m_objUnitStateReply.BODY.UNITS[i].INTERLOCKSTATE = string.Format("{0}", (int)m_objDocument.m_eUnitInterlockState[i][(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    m_objUnitStateReply.BODY.UNITS[i].MOVESTATE = string.Format("{0}", (int)m_objDocument.m_eUnitMoveState[i][(int)CCIMDefine.EPresentState.CURRENT_STATE]);
                    m_objUnitStateReply.BODY.UNITS[i].RUNSTATE = string.Format("{0}", (int)CCIMDefine.ERunState.RUN_STATE_RUN);
                    m_objUnitStateReply.BODY.UNITS[i].FRONTSTATE = string.Format("{0}", (int)CCIMDefine.EFrontEquipmentState.FRONT_STATE_UP);
                    m_objUnitStateReply.BODY.UNITS[i].REARSTATE = string.Format("{0}", (int)CCIMDefine.ERearEquipmentState.REAR_STATE_UP);
                    m_objUnitStateReply.BODY.UNITS[i].PP_SPLSTATE = string.Format("{0}", (int)CCIMDefine.EPP_SPLState.PP_SPL_STATE_NORMAL);
                }

                // Reply 보고
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetUnitStateReply(m_objUnitStateReply);

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
            CProcessCIMUnitStateRequest pThis = (CProcessCIMUnitStateRequest)state;

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

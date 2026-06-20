using ENC.Data.Xml.Serialization;
using System;
using System.Threading;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMCellLotInformationReply : CCIMAbstract
    {
        private CellLotInformationReply mCellLotInformationReply;
        private XmlSerializer<CellLotInformationReply> mXmlCellLotInformationReply;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMCellLotInformationReply(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                m_ThreadProcess = new Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start();
                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                // 데이터 생성.
                mCellLotInformationReply = new CellLotInformationReply();
                mCellLotInformationReply.EQPID = m_strEquipmentID;
                mXmlCellLotInformationReply = new XmlSerializer<CellLotInformationReply>();
                // 리시브 되었는지 체크하는 Flag
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
                mCellLotInformationReply = mXmlCellLotInformationReply.Deserialize(e.RecevedXml_2);
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[RECV]\n" + e.RecevedXml_2);
                m_bReceiveData = true;
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
        /// 리시브된 데이터
        /// </summary>
        /// <returns></returns>
        public CellLotInformationReply GetReceivedData()
        {
            m_eStatus = CCIMDefine.EProcessState.STS_READY;
            return mCellLotInformationReply;
        }

        /// <summary>
        /// 리시브 상태 확인.
        /// </summary>
        /// <returns></returns>
        protected override bool IsStatus()
        {
            bool bResult = false;
            do
            {
                if (false == m_bReceiveData)
                {
                    break;
                }

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 작업 수행.
        /// </summary>
        /// <returns></returns>
        protected override bool DoProcess()
        {
            bool bResult = false;
            do
            {
                m_bReceiveData = false;

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadProcess()
        {
            while (false == m_bThreadExit)
            {
                if (IsStatus())
                {
                    if (DoProcess())
                    {
                        m_eStatus = CCIMDefine.EProcessState.STS_RECEIVED;
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}

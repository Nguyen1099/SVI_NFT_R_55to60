using ENC.Data.Xml.Serialization;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMCellInformationDownload : CCIMAbstract
    {
        [Serializable]
        public sealed class BackupCellInfomationDownload
        {
            public CellInfomationDownload Data { get; set; }
            public TimeSpan RegistElapsed { get { return DateTime.Now - mRegistDateTime; } }
            private DateTime mRegistDateTime = DateTime.Now;
        }
        public IBackupFileMapUserInterface<string, BackupCellInfomationDownload> MapCellInfomationDownload { get { return mMapCellInfomationDownload; } }
        private BackupFileMap<string, BackupCellInfomationDownload> mMapCellInfomationDownload = new BackupFileMap<string, BackupCellInfomationDownload>();
        private const string BACKUP_FILE_NAME_CELL_INFORMATION_DOWLOAD = @".\cache\CimCellInformationDowload.bak";

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMCellInformationDownload(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                mMapCellInfomationDownload.Initialize(BACKUP_FILE_NAME_CELL_INFORMATION_DOWLOAD);
                mMapCellInfomationDownload.AfterAdd += mMap_OnAfterAdd;

                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                m_bReceiveData = false;

                // 쓰레드 종료 Flag
                m_bThreadExit = false;
                // 쓰레드 객체 생성.
                m_ThreadProcess = new System.Threading.Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start(this);

                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 리시브 이벤트 (리시브 된 메시지가 오면 해당 메시지에 맞게 파싱한다.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Communicator_OnReceiveRequest(object sender, ReqMsgInfo e)
        {
            try
            {
                XmlSerializer<CellInfomationDownload> objXmlCellInformationDownload = new XmlSerializer<CellInfomationDownload>();
                CellInfomationDownload objCellInformationDownload = new CellInfomationDownload();

                objCellInformationDownload = objXmlCellInformationDownload.Deserialize(e.RecevedXml_2);
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[RECV]\n" + e.RecevedXml_2);

                BackupCellInfomationDownload addData = new BackupCellInfomationDownload();
                addData.Data = objCellInformationDownload;
                mMapCellInfomationDownload.Add(objCellInformationDownload.BODY.CELLID, addData);
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

            mMapCellInfomationDownload.AfterAdd -= mMap_OnAfterAdd;
            mMapCellInfomationDownload.DeInitialize();
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
        /// 리시브 상태 확인
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
                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 맵에 데이터가 추가 됐을 때 이벤트 (오래된 데이터 삭제)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMap_OnAfterAdd(object sender, ConcurrentDictionary<string, BackupCellInfomationDownload> e)
        {
            TimeSpan timeout = TimeSpan.FromHours(5);
            var dataKeys = e.Keys.ToArray();
            foreach (string key in dataKeys)
            {
                if (timeout < e[key].RegistElapsed)
                {
                    e.TryRemove(key, out BackupCellInfomationDownload data);
                }
            }
        }

        /// <summary>
        /// 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcess(object state)
        {
            CProcessCIMCellInformationDownload pThis = (CProcessCIMCellInformationDownload)state;

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
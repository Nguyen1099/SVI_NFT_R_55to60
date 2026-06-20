using ENC.Data.Xml.Serialization;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using urLinkDll;

namespace SVI_NFT_R
{
    public class CProcessCIMCellLotInformationDownload : CCIMAbstract
    {
        [Serializable]
        public sealed class BackupCellLotInformationDownload
        {
            public CellLotInformationDownload Data { get; set; }
            public TimeSpan RegistElapsed { get { return DateTime.Now - mRegistDateTime; } }
            private DateTime mRegistDateTime = DateTime.Now;
        }
        public IBackupFileMapUserInterface<string, BackupCellLotInformationDownload> MapCellLotInformationDownloadDefect { get { return mMapCellLotInformationDownloadDefect; } }
        public IBackupFileMapUserInterface<string, BackupCellLotInformationDownload> MapCellLotInformationDownloadLotInfo { get { return mMapCellLotInformationDownloadLotInfo; } }
        private BackupFileMap<string, BackupCellLotInformationDownload> mMapCellLotInformationDownloadDefect = new BackupFileMap<string, BackupCellLotInformationDownload>();
        private BackupFileMap<string, BackupCellLotInformationDownload> mMapCellLotInformationDownloadLotInfo = new BackupFileMap<string, BackupCellLotInformationDownload>();
        private const string BACKUP_FILE_NAME_CELL_LOT_INFORMATION_DEFECT_DOWLOAD = @".\cache\CimCellLotInformationDowloadDefect.bak";
        private const string BACKUP_FILE_NAME_CELL_LOT_INFORMATION_LOTINFO_DOWLOAD = @".\cache\CimCellLotInformationDowloadLotInfo.bak";

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objCIMIntiialize"></param>
        /// <returns></returns>
        public CProcessCIMCellLotInformationDownload(CCIMDefine.structureCIMInitialize objCIMIntiialize)
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
                mMapCellLotInformationDownloadLotInfo.Initialize(BACKUP_FILE_NAME_CELL_LOT_INFORMATION_LOTINFO_DOWLOAD);
                mMapCellLotInformationDownloadLotInfo.AfterAdd += mMap_OnAfterAdd;
                mMapCellLotInformationDownloadDefect.Initialize(BACKUP_FILE_NAME_CELL_LOT_INFORMATION_DEFECT_DOWLOAD);
                mMapCellLotInformationDownloadDefect.AfterAdd += mMap_OnAfterAdd;

                // 쓰레드 상태 초기화
                m_eStatus = CCIMDefine.EProcessState.STS_READY;
                // 리시브 되었는지 체크하는 Flag
                m_bReceiveData = false;

                // 쓰레드 종료 Flag
                m_bThreadExit = false;
                // 쓰레드 객체 생성.
                m_ThreadProcess = new Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start();

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
                XmlSerializer<CellLotInformationDownload> objXmlCellLotInformationDownload = new XmlSerializer<CellLotInformationDownload>();
                CellLotInformationDownload objCellLotInformationDownload = new CellLotInformationDownload();

                objCellLotInformationDownload = objXmlCellLotInformationDownload.Deserialize(e.RecevedXml_2);
                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_CIM, "[RECV]\n" + e.RecevedXml_2);

                if (0 < objCellLotInformationDownload.BODY.CELLLOTS.Count)
                {
                    BackupCellLotInformationDownload addData = new BackupCellLotInformationDownload();
                    if (
                        objCellLotInformationDownload.BODY.CELLLOTS[0].PRODUCT_TYPE == "PP"
                        || objCellLotInformationDownload.BODY.CELLLOTS[0].PRODUCT_TYPE == "RR"
                        )
                    {
                        addData.Data = objCellLotInformationDownload;
                        mMapCellLotInformationDownloadLotInfo.Add(objCellLotInformationDownload.BODY.CELLLOTS[0].CELLID, addData);
                    }
                    else if (objCellLotInformationDownload.BODY.CELLLOTS[0].PRODUCT_TYPE == "DFCT")
                    {
                        addData.Data = objCellLotInformationDownload;
                        mMapCellLotInformationDownloadDefect.Add(objCellLotInformationDownload.BODY.CELLLOTS[0].CELLID, addData);
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

            mMapCellLotInformationDownloadLotInfo.AfterAdd -= mMap_OnAfterAdd;
            mMapCellLotInformationDownloadLotInfo.DeInitialize();
            mMapCellLotInformationDownloadDefect.AfterAdd -= mMap_OnAfterAdd;
            mMapCellLotInformationDownloadDefect.DeInitialize();
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
        /// 맵에 데이터가 추가 됐을 때 이벤트 (오래된 데이터 삭제)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMap_OnAfterAdd(object sender, ConcurrentDictionary<string, BackupCellLotInformationDownload> e)
        {
            TimeSpan timeout = TimeSpan.FromHours(5);
            var dataKeys = e.Keys.ToArray();
            foreach (string key in dataKeys)
            {
                if (timeout < e[key].RegistElapsed)
                {
                    e.TryRemove(key, out BackupCellLotInformationDownload data);
                }
            }
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

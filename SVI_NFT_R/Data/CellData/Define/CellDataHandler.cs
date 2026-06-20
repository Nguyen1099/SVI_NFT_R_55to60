using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SVI_NFT_R.CellData
{
    [Serializable]
    public partial class CellDataHandler
    {
        public OneCell Data { get; private set; }
        public bool IsInitialized { get; private set; } = false;
        public bool IsChanged { get; set; }
        public ECellData CellDataIndex { get; private set; }
        public EProcess ProcessIndex { get; private set; }
        public int PositionIndex { get; private set; }
        public ECellPlacement CellPlacement { get; private set; }
        public DateTime LastBackupDateTime { get; private set; }
        public object DataLock { get { return mDataLock; } }
        public CProcessAbstract OwnerManager { get; set; } = null;
        private string mBackupFilename;
        private FileStream mBackupFileStream;
        private readonly object mDataLock = new object();

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="name">이름</param>
        /// <returns>함수 실행 결과</returns>
        public bool Initialize(ECellData cellDataIndex, EProcess processIndex, int positionIndex, ECellPlacement cellPlacement)
        {
            bool bResult = false;

            do
            {
                if (true == IsInitialized)
                {
                    break;
                }

                CellDataIndex = cellDataIndex;
                ProcessIndex = processIndex;
                PositionIndex = positionIndex;
                CellPlacement = cellPlacement;
                mBackupFilename = $@".\cache\CellData_{CellDataIndex}.bak";

                // 폴더 생성 확인
                var directoryName = Path.GetDirectoryName(mBackupFilename);
                if (
                    false == string.IsNullOrEmpty(directoryName)
                    && false == Directory.Exists(directoryName)
                    )
                {
                    Directory.CreateDirectory(directoryName);
                }

                // 파일 스트림 생성
                mBackupFileStream = new FileStream(mBackupFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

                // 복구
                Restore();

                IsInitialized = true;
                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 정리
        /// </summary>
        public void DeInitialize()
        {
            if (false == IsInitialized)
            {
                return;
            }

            Backup();

            lock (mDataLock)
            {
                if (null != mBackupFileStream)
                {
                    mBackupFileStream.Dispose();
                    mBackupFileStream = null;
                }
            }

            IsInitialized = false;
        }

        public void Backup()
        {
            lock (mDataLock)
            {
                BinaryFormatter serializer = new BinaryFormatter();
                try
                {
                    mBackupFileStream.SetLength(0);
                    mBackupFileStream.Seek(0, SeekOrigin.Begin);
                    serializer.Serialize(mBackupFileStream, Data);
                    LastBackupDateTime = DateTime.Now;
                    IsChanged = false;
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            }
        }

        public void Restore()
        {
            lock (mDataLock)
            {
                if (0 == mBackupFileStream.Length)
                {
                    Data = new OneCell();
                    Backup();
                }

                BinaryFormatter serializer = new BinaryFormatter();
                try
                {
                    mBackupFileStream.Seek(0, SeekOrigin.Begin);
                    Data = (OneCell)serializer.Deserialize(mBackupFileStream);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            }
        }

        public void ToCopy(OneCell src)
        {
            lock (mDataLock)
            {
                Data = src.DeepClone();
            }
        }
    }
}

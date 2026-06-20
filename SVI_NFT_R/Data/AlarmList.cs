using System;
using System.Collections.Generic;
using System.IO;

namespace SVI_NFT_R.Data
{
    public class AlarmList : IDisposable
    {
        /// <summary>
        /// 클래스 초기화 여부
        /// </summary>
        public bool IsInitialized { get { return mbInitialized; } }
        /// <summary>
        /// 중알람 발생 여부
        /// </summary>
        public bool IsHaveyAlarmSet { get { return mbHeavyAlarmSet; } }
        /// <summary>
        /// 중알람 발생 여부
        /// </summary>
        public bool IsLightAlarmSet { get { return mbHeavyAlarmSet; } }
        /// <summary>
        /// 알람 리스트
        /// </summary>
        public Dictionary<DateTime, CAlarmDefine.EAlarmList> SetAlarms { get { return mAlarmSetList; } }
        /// <summary>
        /// 알람 개수
        /// </summary>
        public int Count { get { return (null != mAlarmSetList) ? mAlarmSetList.Count : 0; } }
        private bool mbHeavyAlarmSet = false;
        private bool mbLightAlarmSet = false;
        private Dictionary<DateTime, CAlarmDefine.EAlarmList> mAlarmSetList = new Dictionary<DateTime, CAlarmDefine.EAlarmList>();
        private HashSet<CAlarmDefine.EAlarmList> mAlarmSetHashList = new HashSet<CAlarmDefine.EAlarmList>();
        private FileStream mBackupFile;
        private const string BACKUP_FILE_NAME = @".\cache\AlarmSetList.bak";
        private CDocument mDocument;
        private bool mbInitialized = false;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="document">도큐먼트</param>
        public void Initilaize(CDocument document)
        {
            mDocument = document;

            var directoryName = Path.GetDirectoryName(BACKUP_FILE_NAME);
            if (
                false == string.IsNullOrEmpty(directoryName)
                && false == Directory.Exists(directoryName)
                )
            {
                Directory.CreateDirectory(directoryName);
            }
            mBackupFile = File.Open(BACKUP_FILE_NAME, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            // 백업 파일에서 알람 리스트를 로드한다
            loadAlarmList();

            mbInitialized = true;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            lock (mAlarmSetHashList)
            {
                Dispose();
            }

            mbInitialized = false;
        }

        /// <summary>
        /// 리스트 초기화
        /// </summary>
        public void Clear()
        {
            if (0 < mAlarmSetHashList.Count)
            {
                lock (mAlarmSetHashList)
                {
                    mAlarmSetList.Clear();
                    mAlarmSetHashList.Clear();

                    mbHeavyAlarmSet = false;
                    mbLightAlarmSet = false;

                    backupAlarmList();
                }
            }
        }

        public DateTime CreateAlarmOccuredDateTime()
        {
            DateTime occuredDateTime = DateTime.Now;
            while (true)
            {
                if (mAlarmSetList.ContainsKey(occuredDateTime) == false)
                {
                    break;
                }
                occuredDateTime = occuredDateTime.AddMilliseconds(1);
            }
            return occuredDateTime;
        }

        /// <summary>
        /// 설정한 시간으로 알람 리스트 추가
        /// </summary>
        /// <param name="occuredDateTime">알람 발생 시간</param>
        /// <param name="alarmIndex">알람 인덱스</param>
        public void Add(DateTime occuredDateTime, CAlarmDefine.EAlarmList alarmIndex)
        {
            lock (mAlarmSetHashList)
            {
                mAlarmSetList.Add(occuredDateTime, alarmIndex);
                mAlarmSetHashList.Add(alarmIndex);

                if (false == mbHeavyAlarmSet)
                {
                    foreach (var alarm in mAlarmSetHashList)
                    {
                        if (true == GetIsHeavyAlarmCode(alarm))
                        {
                            mbHeavyAlarmSet = true;
                            break;
                        }
                    }
                }

                if (false == mbLightAlarmSet)
                {
                    foreach (var alarm in mAlarmSetHashList)
                    {
                        if (true == GetIsLightAlarmCode(alarm))
                        {
                            mbLightAlarmSet = true;
                            break;
                        }
                    }
                }
                backupAlarmList();
            }
        }

        /// <summary>
        /// 알람 리스트에서 제거
        /// </summary>
        /// <param name="removeData">알람 데이터</param>
        public void Remove(KeyValuePair<DateTime, CAlarmDefine.EAlarmList> removeData)
        {
            Remove(removeData.Key, removeData.Value);
        }

        /// <summary>
        /// 알람 리스트에서 제거
        /// </summary>
        /// <param name="occuredDateTime">발생 시간</param>
        /// <param name="alarmIndex">알람 인덱스</param>
        public void Remove(DateTime occuredDateTime, CAlarmDefine.EAlarmList alarmIndex)
        {
            if (0 < mAlarmSetHashList.Count)
            {
                lock (mAlarmSetHashList)
                {
                    if (true == mAlarmSetList.ContainsKey(occuredDateTime))
                    {
                        mAlarmSetList.Remove(occuredDateTime);
                        if (false == mAlarmSetList.ContainsValue(alarmIndex))
                        {
                            mAlarmSetHashList.Remove(alarmIndex);
                        }
                    }

                    bool heavyAlarmDetected = false;
                    foreach (var alarm in mAlarmSetHashList)
                    {
                        if (true == GetIsHeavyAlarmCode(alarm))
                        {
                            heavyAlarmDetected = true;
                            break;
                        }
                    }

                    bool lightalarmDetected = false;
                    foreach (var alarm in mAlarmSetHashList)
                    {
                        if (true == GetIsLightAlarmCode(alarm))
                        {
                            lightalarmDetected = true;
                            break;
                        }
                    }

                    mbHeavyAlarmSet = heavyAlarmDetected;
                    mbLightAlarmSet = lightalarmDetected;

                    backupAlarmList();
                }
            }
        }

        /// <summary>
        /// 알람이 발생중인지 확인함
        /// </summary>
        /// <param name="alarmIndex">알람 인덱스</param>
        /// <returns>true = 알람 발생중, false = 알람 미발생중</returns>
        public bool GetIsOnAlarm(CAlarmDefine.EAlarmList alarmIndex)
        {
            return mAlarmSetHashList.Contains(alarmIndex);
        }

        /// <summary>
        /// 알람이 중알람이지 확인함
        /// </summary>
        /// <param name="alarmIndex">알람 인덱스</param>
        /// <returns>true = 중알람, false = 경알람</returns>
        public bool GetIsHeavyAlarmCode(CAlarmDefine.EAlarmList alarmIndex)
        {
            if (
                null == mDocument
                || false == mDocument.m_bIsDatabaseInitialized
                )
            {
                return false;
            }

            // 헤비 알람 체크
            var selectRows = mDocument.AlarmDataTable.Select(string.Format("[{1}] = '{2}' AND [{0}] = 'HEAVY'",
                mDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.LEVEL],
                mDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.ID],
                (int)alarmIndex
                ));
            if (
                null != selectRows
                && 0 < selectRows.Length
                )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 알람이 경알람이지 확인함
        /// </summary>
        /// <param name="alarmIndex">알람 인덱스</param>
        /// <returns>true = 중알람, false = 경알람</returns>
        public bool GetIsLightAlarmCode(CAlarmDefine.EAlarmList alarmIndex)
        {
            if (
                null == mDocument
                || false == mDocument.m_bIsDatabaseInitialized
                )
            {
                return false;
            }

            // 헤비 알람 체크
            var selectRows = mDocument.AlarmDataTable.Select(string.Format("[{1}] = '{2}' AND [{0}] = 'LIGHT'",
                mDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.LEVEL],
                mDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.ID],
                (int)alarmIndex
                ));
            if (
                null != selectRows
                && 0 < selectRows.Length
                )
            {
                return true;
            }

            return false;
        }

        private void loadAlarmList()
        {
            int backupHeaderSize = sizeof(int);
            int backupBlockSize = sizeof(long) + sizeof(int);
            var headerRawdata = new byte[backupHeaderSize];
            mBackupFile.Seek(0, SeekOrigin.Begin);
            mBackupFile.Read(headerRawdata, 0, headerRawdata.Length);
            long backupAlarmCount = BitConverter.ToInt32(headerRawdata, 0);
            long backupAlarmSize = backupAlarmCount * backupBlockSize + backupHeaderSize;
            byte[] backupAlarmRawdata = new byte[backupAlarmSize];
            mBackupFile.Seek(0, SeekOrigin.Begin);
            mBackupFile.Read(backupAlarmRawdata, 0, backupAlarmRawdata.Length);
            for (int index = 0; index < backupAlarmCount; index++)
            {
                int startIndex = index * backupBlockSize + backupHeaderSize;
                DateTime addDateTime = DateTime.FromBinary(BitConverter.ToInt64(backupAlarmRawdata, startIndex));
                startIndex += sizeof(long);
                CAlarmDefine.EAlarmList addAlarmIndex = (CAlarmDefine.EAlarmList)BitConverter.ToInt32(backupAlarmRawdata, startIndex);

                mAlarmSetList.Add(addDateTime, addAlarmIndex);
                mAlarmSetHashList.Add(addAlarmIndex);
            }

            if (false == mbHeavyAlarmSet)
            {
                foreach (var alarm in mAlarmSetHashList)
                {
                    if (true == GetIsHeavyAlarmCode(alarm))
                    {
                        mbHeavyAlarmSet = true;
                        break;
                    }
                }
            }

            if (false == mbLightAlarmSet)
            {
                foreach (var alarm in mAlarmSetHashList)
                {
                    if (true == GetIsLightAlarmCode(alarm))
                    {
                        mbLightAlarmSet = true;
                        break;
                    }
                }
            }
        }

        private void backupAlarmList()
        {
            if (0 == mAlarmSetList.Count)
            {
                mBackupFile.SetLength(0);
            }
            else
            {
                int backupHeaderSize = sizeof(int);
                int backupBlockSize = sizeof(long) + sizeof(int);
                long backupAlarmCount = mAlarmSetList.Count;
                long backupAlarmSize = backupAlarmCount * backupBlockSize + backupHeaderSize;
                if (backupAlarmSize < mBackupFile.Length)
                {
                    backupAlarmSize = mBackupFile.Length;
                }
                byte[] backupAlarmRawdata = new byte[backupAlarmSize];
                Array.Copy(BitConverter.GetBytes(backupAlarmCount), 0, backupAlarmRawdata, 0, backupHeaderSize);
                int index = 0;
                foreach (var pair in mAlarmSetList)
                {
                    long dateTime = pair.Key.ToBinary();
                    int alarmIndex = (int)pair.Value;

                    var sourceData = BitConverter.GetBytes(dateTime);
                    int startIndex = index * backupBlockSize + backupHeaderSize;
                    Array.Copy(sourceData, 0, backupAlarmRawdata, startIndex, sourceData.Length);
                    startIndex += sizeof(long);
                    sourceData = BitConverter.GetBytes(alarmIndex);
                    Array.Copy(sourceData, 0, backupAlarmRawdata, startIndex, sourceData.Length);
                    index++;
                }

                mBackupFile.Seek(0, SeekOrigin.Begin);
                mBackupFile.Write(backupAlarmRawdata, 0, backupAlarmRawdata.Length);
                mBackupFile.Flush();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    if (null != mBackupFile)
                    {
                        mBackupFile.Dispose();
                        mBackupFile = null;
                    }
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~AlarmList() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

using SVI_NFT_R;
using SVI_NFT_R.CellData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

public static partial class TrackInManager
{
    public static bool IsInitialized { get; private set; } = false;
    public static bool IsBusy => mTaskItems.Any(i => i.Value.IsFinished == false);
    private static CDocument m_objDocument;
    private static Thread mThreadTrackIn;
    private static bool mbShouldStop = false;
    private static Stream mBackupDirectoryLock;
    private static readonly object mSyncRoot = new object();
    private static readonly Dictionary<string, TaskTrackInSet> mTaskItems = new Dictionary<string, TaskTrackInSet>();
    private static readonly string BACKUP_DIRECTORY_PATH = Path.Combine($"cache", $"TaskTrackInItems");
    private static readonly BinaryFormatter mBinaryFormatter = new BinaryFormatter();
    private static readonly EProcessing mProcessingType = EProcessing.Serial;

    public static bool Initialize(CDocument document)
    {
        if (IsInitialized == true)
        {
            return false;
        }

        m_objDocument = document;

        // 데이터형 유효성 검사 (개발자 실수 체크)
        {
            Type keyType = typeof(string);
            Type valueType = typeof(TaskTrackInSet);
            List<string> nonSerializableTypes = new List<string>();
            ReclusiveNonSerializableTypesOfParentType(keyType, nonSerializableTypes);
            ReclusiveNonSerializableTypesOfParentType(valueType, nonSerializableTypes);
            if (nonSerializableTypes.Count > 0)
            {
                Debug.Assert(false, $"[{string.Join(",", nonSerializableTypes)}]에 Serailizable Attribute가 없어서 {nameof(TrackInManager)}클래스를 초기화 할 수 없습니다. 해당 클래스에 선언부를 확인해 주세요.");
                return false;
            }
        }

        // 폴더 생성
        if (Directory.Exists(BACKUP_DIRECTORY_PATH) == false)
        {
            Directory.CreateDirectory(BACKUP_DIRECTORY_PATH);
        }

        // 폴더 삭제 방지
        string path = Path.Combine(BACKUP_DIRECTORY_PATH, $"lock");
        mBackupDirectoryLock = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        var fi = new FileInfo(path);
        fi.Attributes |= FileAttributes.Hidden;

        // 전체 복구
        RestoreInternal();
        DeleteEunnecessaryfiles();

        mThreadTrackIn = new Thread(ThreadProcessTrackIn);
        mThreadTrackIn.Start();

        IsInitialized = true;
        return true;
    }

    public static void DeInitialize()
    {
        if (IsInitialized == false)
        {
            return;
        }

        mbShouldStop = true;
        mThreadTrackIn.Join();

        // 전체 백업
        foreach (string innerID in mTaskItems.Keys.ToArray())
        {
            BackupInternal(innerID);
        }

        mBackupDirectoryLock.Dispose();

        IsInitialized = false;
    }

    public static void WaitForEndProcess(CellDataHandler cellData)
    {
        string innerID = cellData.GetInnerID();
        if (mTaskItems.ContainsKey(innerID) == false)
        {
            return;
        }

        TaskTrackInSet taskItem = mTaskItems[innerID];
        if (taskItem.ShouldRemove == true)
        {
            return;
        }

        taskItem.WaitForEndProcess(cellData);
    }

    public static bool TryAdd(CellDataHandler cellData)
    {
        if (mTaskItems.ContainsKey(cellData.GetInnerID()) == true)
        {
            return false;
        }

        TaskTrackInSet addItem = new TaskTrackInSet(cellData);
        addItem.DataChanged += TaskTrackInSet_DataChanged;
        BackupInternal(addItem.InnerID, addItem);
        lock (mSyncRoot)
        {
            mTaskItems[addItem.InnerID] = addItem;
        }

        return true;
    }

    public static bool IsTrackInFinished(CellDataHandler cellData)
    {
        // ! 시뮬레이션 모드일 경우 MCC 로그 확인을 위해 절반 확률로 완료 처리함
        if (m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON)
        {
            return new Random().NextDouble() < 0.5d;
        }
        string innerID = cellData.GetInnerID();
        if (mTaskItems.ContainsKey(innerID) == false)
        {
            return true;
        }
        TaskTrackInSet taskItem = mTaskItems[innerID];
        return taskItem.IsFinished;
    }

    private static void RemoveInternal(string innerID)
    {
        if (mTaskItems.ContainsKey(innerID) == false)
        {
            return;
        }

        string path = Path.Combine(BACKUP_DIRECTORY_PATH, $"{innerID}.dat");
        try
        {
            File.Delete(path);
        }
        catch
        {
            // 파일 삭제 실패시 재시도함
            return;
        }

        var item = mTaskItems[innerID];
        item.DataChanged -= TaskTrackInSet_DataChanged;
        mTaskItems.Remove(innerID);
    }

    private static void BackupInternal(string innerID)
    {
        if (mTaskItems.ContainsKey(innerID) == false)
        {
            return;
        }

        BackupInternal(innerID, mTaskItems[innerID]);
    }

    private static void BackupInternal(string innerID, TaskTrackInSet data)
    {
        string path = Path.Combine(BACKUP_DIRECTORY_PATH, $"{innerID}.dat");
        using (var stream = File.Open(path, FileMode.OpenOrCreate))
        {
            mBinaryFormatter.Serialize(stream, data);
        }
    }

    private static void RestoreInternal()
    {
        string[] fileNames = Directory.GetFiles(BACKUP_DIRECTORY_PATH, "*.dat", SearchOption.TopDirectoryOnly);
        foreach (string fileName in fileNames)
        {
            string innerID = Path.GetFileNameWithoutExtension(fileName);
            string path = Path.Combine(BACKUP_DIRECTORY_PATH, $"{innerID}.dat");
            TaskTrackInSet item = null;
            using (var stream = File.Open(path, FileMode.Open))
            {
                try
                {
                    item = (TaskTrackInSet)mBinaryFormatter.Deserialize(stream);
                }
                catch
                {
                    // 파일이 깨졌을 경우 삭제함
                }
            }

            if (item == null)
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    // 파일 삭제 실패시 스킵함
                }
                continue;
            }

            mTaskItems[innerID] = item;
        }
    }

    private static void TaskTrackInSet_DataChanged(object sender, EventArgs args)
    {
        TaskTrackInSet item = (TaskTrackInSet)sender;
        BackupInternal(item.InnerID, item);
    }

    private static void ReclusiveNonSerializableTypesOfParentType(Type type, List<string> nonSerializableTypes)
    {
        // base case
        if (type.IsValueType
            || type == typeof(string)
            || type == typeof(string[])
            || type == typeof(int[])
            || type == typeof(short[])
            || type == typeof(byte[])
            || type == typeof(float[])
            || type == typeof(double[])
            || type == typeof(long[])
            || type == typeof(decimal[])
            )
        {
            return;
        }

        if (!IsSerializable(type))
            nonSerializableTypes.Add(type.FullName);

        foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (propertyInfo.PropertyType.IsGenericType)
            {
                foreach (var genericArgument in propertyInfo.PropertyType.GetGenericArguments())
                {
                    if (genericArgument == type) continue; // base case for circularly referenced properties
                    ReclusiveNonSerializableTypesOfParentType(genericArgument, nonSerializableTypes);
                }
            }
            else if (propertyInfo.GetType() != type) // base case for circularly referenced properties
                ReclusiveNonSerializableTypesOfParentType(propertyInfo.PropertyType, nonSerializableTypes);
        }
    }

    private static void DeleteEunnecessaryfiles()
    {
        HashSet<string> allInnerIds = CellDataManager.Cells.Values.GetAllInnerID();
        string[] fileNames = Directory.GetFiles(BACKUP_DIRECTORY_PATH, "*.dat", SearchOption.TopDirectoryOnly);
        lock (mSyncRoot)
        {
            foreach (string fileName in fileNames)
            {
                string innerID = Path.GetFileNameWithoutExtension(fileName);
                string path = Path.Combine(BACKUP_DIRECTORY_PATH, $"{innerID}.dat");
                FileInfo fileInfo = new FileInfo(path);
                if (allInnerIds.Contains(innerID) == false)
                {
                    RemoveInternal(innerID);
                }
            }
        }
    }

    private static bool IsSerializable(Type type)
    {
        return (Attribute.IsDefined(type, typeof(SerializableAttribute)));
    }

    private static void ThreadProcessTrackIn()
    {
        // Document 초기화 완료 대기
        while (m_objDocument.IsInitialized == false && mbShouldStop == false)
        {
            Thread.Sleep(100);
        }

        List<string> removeItems = new List<string>();
        int lastDeleteHour = -1;
        TimeSpan dataKeepDays = TimeSpan.FromDays(7d);
        while (mbShouldStop == false)
        {
            Thread.Sleep(5);

            // 오래된 파일 삭제 스케쥴 진행
            if (lastDeleteHour != DateTime.Now.Hour)
            {
                lastDeleteHour = DateTime.Now.Hour;
                string[] fileNames = Directory.GetFiles(BACKUP_DIRECTORY_PATH, "*.dat", SearchOption.TopDirectoryOnly);
                lock (mSyncRoot)
                {
                    foreach (string fileName in fileNames)
                    {
                        string innerID = Path.GetFileNameWithoutExtension(fileName);
                        string path = Path.Combine(BACKUP_DIRECTORY_PATH, $"{innerID}.dat");
                        FileInfo fileInfo = new FileInfo(path);
                        if (DateTime.Now - fileInfo.CreationTime < dataKeepDays)
                        {
                            continue;
                        }
                        RemoveInternal(innerID);
                    }
                }
            }

            // 완료된 항목 삭제 진행
            if (removeItems.Count > 0)
            {
                lock (mSyncRoot)
                {
                    foreach (var removeItemKey in removeItems)
                    {
                        RemoveInternal(removeItemKey);
                    }
                }
                removeItems.Clear();
            }

            if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Start
                && m_objDocument.GetRunStatus() != CDefine.ERunStatus.Stopping
                && m_objDocument.GetRunStatus() != CDefine.ERunStatus.LoadingStop
                )
            {
                continue;
            }

            if (mTaskItems.Count > 0)
            {
                TaskTrackInSet[] captureItems;
                lock (mSyncRoot)
                {
                    captureItems = mTaskItems.Values.ToArray();
                }
                switch (mProcessingType)
                {
                    case EProcessing.Serial:
                        {
                            HashSet<string> allInnerIds = CellDataManager.Cells.Values.GetAllInnerID();
                            captureItems = captureItems.OrderBy(i => i.CreationTime).ToArray();
                            // 등록된 순서대로 한 셀 씩 진행
                            bool bIsProcessed = false;
                            for (int i = 0; i < captureItems.Length; i++)
                            {
                                TaskTrackInSet item = captureItems[i];

                                if (allInnerIds.Contains(item.InnerID) == false)
                                {
                                    removeItems.Add(item.InnerID);
                                    continue;
                                }
                                if (item.ShouldRemove == true)
                                {
                                    removeItems.Add(item.InnerID);
                                    continue;
                                }
                                if (item.IsFinished == true)
                                {
                                    continue;
                                }

                                if (bIsProcessed == false)
                                {
                                    bIsProcessed = true;
                                    item.DoProcess(m_objDocument);
                                }
                            }
                        }
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        }
    }
}
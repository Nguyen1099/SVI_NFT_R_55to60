using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace SVI_NFT_R
{
    /// <summary>
    /// 파일로 백업 및 복구하는 기능을 가진 맵 클래스
    /// </summary>
    /// <typeparam name="TKey">키의 타입</typeparam>
    /// <typeparam name="TValue">값의 타입</typeparam>
    public class BackupFileMap<TKey, TValue> : IBackupFileMapUserInterface<TKey, TValue>, IReadOnlyBackupFileMap<TKey, TValue>
    {
        /// <summary>
        /// 초기화 여부
        /// </summary>
        public bool IsInitialized { get; private set; }
        /// <summary>
        /// 백업 파일 이름
        /// </summary>
        public string BackupFileName { get; private set; }
        /// <summary>
        /// 맵 데이터
        /// </summary>
        public IReadOnlyDictionary<TKey, TValue> Map { get { return mMap; } }
        /// <summary>
        /// 파일을 신규 생성 할 때 기본값. null이면 딕셔너리만 새로 생성함.
        /// </summary>
        public ConcurrentDictionary<TKey, TValue> CreateFileDefaultOrNull { get; set; }
        /// <summary>
        /// 맵에 데이터 추가 후에 발생하는 이벤트 (백업 전에 호출됨)
        /// </summary>
        public event EventHandler<ConcurrentDictionary<TKey, TValue>> AfterAdd;
        /// <summary>
        /// 맵에서 데이터 삭제 후에 발생하는 이벤트 (백업 전에 호출됨)
        /// </summary>
        public event EventHandler<ConcurrentDictionary<TKey, TValue>> AfterRemove;
        /// <summary>
        /// 백업 전에 발생하는 이벤트
        /// </summary>
        public event EventHandler BeforeBackup;
        /// <summary>
        /// 백업 후에 발생하는 이벤트
        /// </summary>
        public event EventHandler AfterBackup;
        /// <summary>
        /// 복구 전에 발생하는 이벤트
        /// </summary>
        public event EventHandler BeforeRestore;
        /// <summary>
        /// 복구 후에 발생하는 이벤트
        /// </summary>
        public event EventHandler AfterRestore;
        /// <summary>
        /// 내부 맵 락 객체
        /// </summary>
        public object MapLock { get { return mMapLock; } }
        private readonly object mMapLock;
        private ConcurrentDictionary<TKey, TValue> mMap;
        private FileStream mBackupFileStream;
        private readonly BinaryFormatter mSerializer = new BinaryFormatter();

        /// <summary>
        /// 생성자
        /// </summary>
        public BackupFileMap()
        {
            mMapLock = new object();
            CreateFileDefaultOrNull = null;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="backupFileName">백업 파일 이름</param>
        /// <returns>함수 실행 결과</returns>
        public bool Initialize(string backupFileName)
        {
            bool bResult = false;

            do
            {
                if (true == IsInitialized)
                {
                    break;
                }

                // 데이터형 유효성 검사 (개발자 실수 체크)
                {
                    Type keyType = typeof(TKey);
                    Type valueType = typeof(TValue);
                    List<string> nonSerializableTypes = new List<string>();
                    ReclusiveNonSerializableTypesOfParentType(keyType, nonSerializableTypes);
                    ReclusiveNonSerializableTypesOfParentType(valueType, nonSerializableTypes);
                    if (nonSerializableTypes.Count > 0)
                    {
                        Debug.Assert(false, $"[{string.Join(",", nonSerializableTypes)}]에 Serailizable Attribute가 없어서 {nameof(BackupFileMap<TKey, TValue>)}클래스를 초기화 할 수 없습니다. 해당 클래스에 선언부를 확인해 주세요. ({backupFileName})");
                        break;
                    }
                }

                BackupFileName = backupFileName;

                // 폴더 생성 확인
                var directoryName = Path.GetDirectoryName(BackupFileName);
                if (
                    false == string.IsNullOrEmpty(directoryName)
                    && false == Directory.Exists(directoryName)
                    )
                {
                    Directory.CreateDirectory(directoryName);
                }

                // 파일 스트림 생성
                mBackupFileStream = new FileStream(BackupFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

                // 복구
                restore();

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

            lock (mMapLock)
            {
                backup();

                if (null != mBackupFileStream)
                {
                    mBackupFileStream.Flush(flushToDisk: true);
                    mBackupFileStream.Dispose();
                    mBackupFileStream = null;
                }
            }

            IsInitialized = false;
        }

        /// <summary>
        /// 해당 키에 대한 값을 불러옴
        /// </summary>
        /// <param name="key">키</param>
        /// <param name="value">값</param>
        /// <returns>함수 실행 결과</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            bool bResult = false;
            value = default(TValue);

            do
            {
                if (false == IsInitialized)
                {
                    break;
                }

                if (false == Map.ContainsKey(key))
                {
                    break;
                }

                value = Map[key];

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 해당 키에 값을 등록하고 백업함
        /// </summary>
        /// <param name="key">키</param>
        /// <param name="value">값</param>
        public void Add(TKey key, TValue value)
        {
            if (false == IsInitialized)
            {
                return;
            }

            lock (mMapLock)
            {
                mMap[key] = value;
                raiseAfterAddEvent(mMap);

                backup();
            }
        }

        /// <summary>
        /// 내부 데이터를 입력한 데이터로 변경함
        /// </summary>
        /// <param name="data"></param>
        public void SetDictonary(ConcurrentDictionary<TKey, TValue> data)
        {
            if (false == IsInitialized)
            {
                return;
            }

            lock (mMapLock)
            {
                mMap = data;

                backup();
            }
        }

        /// <summary>
        /// 해당 키에 값을 삭제하고 백업함
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            if (false == IsInitialized)
            {
                return;
            }

            lock (mMapLock)
            {
                if (true == mMap.ContainsKey(key))
                {
                    mMap.TryRemove(key, out TValue value);
                    raiseAfterRemoveEvent(mMap);

                    backup();
                }
            }
        }

        public void ForEach(Action<TKey, TValue> action, bool bShouldBackupAfterAction = true)
        {
            lock (mMapLock)
            {
                foreach (var item in mMap)
                {
                    action.Invoke(item.Key, item.Value);
                }

                if (bShouldBackupAfterAction == true)
                {
                    backup();
                }
            }
        }

        private void backup()
        {
            raiseBeforeBackupEvent();

            try
            {
                mBackupFileStream.Seek(0, SeekOrigin.Begin);
                mBackupFileStream.SetLength(0);
                mSerializer.Serialize(mBackupFileStream, mMap);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            raiseAfterBackupEvent();
        }

        private void restore()
        {
            raiseBeforeRestoreEvent();

            if (0 == mBackupFileStream.Length)
            {
                if (null != CreateFileDefaultOrNull)
                {
                    mMap = CreateFileDefaultOrNull.DeepClone();
                }
                else
                {
                    mMap = new ConcurrentDictionary<TKey, TValue>();
                }
                backup();
            }

            try
            {
                mBackupFileStream.Seek(0, SeekOrigin.Begin);
                mMap = (ConcurrentDictionary<TKey, TValue>)mSerializer.Deserialize(mBackupFileStream);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            if (mMap == null)
            {
                if (null != CreateFileDefaultOrNull)
                {
                    mMap = CreateFileDefaultOrNull.DeepClone();
                }
                else
                {
                    mMap = new ConcurrentDictionary<TKey, TValue>();
                }
                backup();
            }

            raiseAfterRestoreEvent();
        }

        private void raiseAfterAddEvent(ConcurrentDictionary<TKey, TValue> data)
        {
            var eventHandlerCapture = AfterAdd;
            if (null != eventHandlerCapture)
            {
                eventHandlerCapture.Invoke(this, data);
            }
        }

        private void raiseAfterRemoveEvent(ConcurrentDictionary<TKey, TValue> data)
        {
            var eventHandlerCapture = AfterRemove;
            if (null != eventHandlerCapture)
            {
                eventHandlerCapture.Invoke(this, data);
            }
        }

        private void raiseBeforeBackupEvent()
        {
            var eventHandlerCapture = BeforeBackup;
            if (null != eventHandlerCapture)
            {
                eventHandlerCapture.Invoke(this, EventArgs.Empty);
            }
        }

        private void raiseAfterBackupEvent()
        {
            var eventHandlerCapture = AfterBackup;
            if (null != eventHandlerCapture)
            {
                eventHandlerCapture.Invoke(this, EventArgs.Empty);
            }
        }

        private void raiseBeforeRestoreEvent()
        {
            var eventHandlerCapture = BeforeRestore;
            if (null != eventHandlerCapture)
            {
                eventHandlerCapture.Invoke(this, EventArgs.Empty);
            }
        }

        private void raiseAfterRestoreEvent()
        {
            var eventHandlerCapture = AfterRestore;
            if (null != eventHandlerCapture)
            {
                eventHandlerCapture.Invoke(this, EventArgs.Empty);
            }
        }

        private static void ReclusiveNonSerializableTypesOfParentType(Type type, List<string> nonSerializableTypes)
        {
            // base case
            if (
                type.IsValueType
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

            if (type.IsArray == true)
            {
                Type t = Type.GetType(type.FullName.Replace("[]", ""));
                if (!isSerializable(t))
                    nonSerializableTypes.Add(t.FullName);
            }
            else
            {
                if (!isSerializable(type))
                    nonSerializableTypes.Add(type.FullName);
            }

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

        private static bool isSerializable(Type type)
        {
            return (Attribute.IsDefined(type, typeof(SerializableAttribute)));
        }
    }
}

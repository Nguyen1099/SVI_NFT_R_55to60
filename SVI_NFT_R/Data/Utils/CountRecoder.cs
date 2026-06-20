using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        public class CountRecoder
        {
            public bool IsInitialized { get; private set; } = false;
            public bool IsDailyCount { get; private set; } = false;
            public string BackupFileName { get; private set; } = string.Empty;
            public int Count => mData.Count;
            public DateTime LastUpdateTime => mData.LastUpdateTime;
            public TimeSpan LastUpdateDuration => mData.LastUpdateDuration;
            public TimeSpan IdleSpinPeriod { get; set; } = TimeSpan.FromSeconds(1);
            public class CountBeforeUpdateEventArgs
            {
                public bool IsClearCountAfterAdd { get; set; } = false;
                public DateTime LastUpdateDate { get; set; }
                public TimeSpan LastUpdateDuration { get; set; }
            }
            public event EventHandler<CountBeforeUpdateEventArgs> CountBeforeUpdate;
            public event EventHandler<int> CountAfterUpdate;
            public event EventHandler BeforeBackup;
            public event EventHandler AfterBackup;
            public event EventHandler BeforeRestore;
            public event EventHandler AfterRestore;
            private InternalData mData = new InternalData();
            private FileStream mBackupFileStream;
            private Thread mThreadProcessCommand;
            private ConcurrentQueue<InternalCommand> mCommandQueue = new ConcurrentQueue<InternalCommand>();
            private readonly AutoResetEvent mCommandLock = new AutoResetEvent(false);
            private enum ECommand
            {
                None = 0,
                Add,
                SetValue,
                Clear,
                ExitThread
            }

            public bool Initialize(string backupFileName, bool bDailyResetCount)
            {
                if (IsInitialized == true)
                {
                    return false;
                }

                IsDailyCount = bDailyResetCount;

                // 폴더 생성 확인
                BackupFileName = backupFileName;
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

                if (IsDailyCount == true)
                {
                    if (DateTime.Now.DayOfYear != mData.LastUpdateTime.DayOfYear)
                    {
                        mData.Count = 0;
                        raiseCountAfterUpdateEvent(Count);
                        mData.LastUpdateTime = DateTime.Now;
                        backup();
                    }
                }

                mCommandQueue = new ConcurrentQueue<InternalCommand>();
                mThreadProcessCommand = new Thread(threadProcessCommand);
                mThreadProcessCommand.Start();

                IsInitialized = true;
                return true;
            }

            public void DeInitialize()
            {
                if (IsInitialized == false)
                {
                    return;
                }

                var command = new InternalCommand(ECommand.ExitThread);
                mCommandQueue.Enqueue(command);
                mCommandLock.Set();
                mThreadProcessCommand.Join();
                mThreadProcessCommand = null;
                mCommandQueue = new ConcurrentQueue<InternalCommand>();

                // 저장
                backup();

                mBackupFileStream.Dispose();
                mBackupFileStream = null;

                IsInitialized = false;
            }

            public int Add(int addCount)
            {
                using (var command = new InternalCommand(ECommand.Add, addCount))
                {
                    mCommandQueue.Enqueue(command);
                    mCommandLock.Set();
                    command.WaitProcessed.WaitOne();
                    return command.Result;
                }
            }

            public IAsyncResult AddAsync(int addCount)
            {
                var command = new InternalCommand(ECommand.Add, addCount, new InternalAsyncResult());
                mCommandQueue.Enqueue(command);
                mCommandLock.Set();
                return command.AsyncResult;
            }

            public void SetValue(int setValue)
            {
                using (var command = new InternalCommand(ECommand.SetValue, setValue))
                {
                    mCommandQueue.Enqueue(command);
                    mCommandLock.Set();
                    command.WaitProcessed.WaitOne();
                }
            }

            public IAsyncResult SetValueAsync(int setValue)
            {
                var command = new InternalCommand(ECommand.SetValue, setValue, new InternalAsyncResult());
                mCommandQueue.Enqueue(command);
                mCommandLock.Set();
                return command.AsyncResult;
            }

            public void Clear()
            {
                using (var command = new InternalCommand(ECommand.Clear))
                {
                    mCommandQueue.Enqueue(command);
                    mCommandLock.Set();
                    command.WaitProcessed.WaitOne();
                }
            }

            public IAsyncResult ClearAsync()
            {
                var command = new InternalCommand(ECommand.Clear, 0, new InternalAsyncResult());
                mCommandQueue.Enqueue(command);
                mCommandLock.Set();
                return command.AsyncResult;
            }

            private void threadProcessCommand()
            {
                SpinWait.SpinUntil(doProcessCommand);
            }

            private bool doProcessCommand()
            {
                if (IsInitialized == false)
                {
                    return false;
                }

                if (IsDailyCount == true)
                {
                    if (DateTime.Now.DayOfYear != mData.LastUpdateTime.DayOfYear)
                    {
                        mData.Count = 0;
                        raiseCountAfterUpdateEvent(Count);
                        mData.LastUpdateTime = DateTime.Now;
                        backup();
                    }
                }

                if (mCommandQueue.IsEmpty == true)
                {
                    if (mCommandLock.WaitOne(IdleSpinPeriod) == false)
                    {
                        return false;
                    }
                }

                InternalCommand getCommand;
                if (mCommandQueue.TryDequeue(out getCommand) == false)
                {
                    return false;
                }
                if (getCommand == null)
                {
                    return false;
                }
                try
                {
                    switch (getCommand.Code)
                    {
                        case ECommand.Add:
                            CountBeforeUpdateEventArgs args = new CountBeforeUpdateEventArgs()
                            {
                                LastUpdateDate = LastUpdateTime,
                                LastUpdateDuration = LastUpdateDuration
                            };
                            raiseCountBeforeUpdateEvent(args);
                            if (args.IsClearCountAfterAdd == true)
                            {
                                mData.Count = getCommand.Value;
                            }
                            else
                            {
                                mData.Count += getCommand.Value;
                            }
                            raiseCountAfterUpdateEvent(Count);
                            mData.LastUpdateTime = DateTime.Now;
                            backup();
                            return false;
                        case ECommand.SetValue:
                            mData.Count = getCommand.Value;
                            raiseCountAfterUpdateEvent(Count);
                            mData.LastUpdateTime = DateTime.Now;
                            backup();
                            return false;
                        case ECommand.Clear:
                            mData.Count = 0;
                            raiseCountAfterUpdateEvent(Count);
                            mData.LastUpdateTime = DateTime.Now;
                            backup();
                            return false;
                        case ECommand.ExitThread:
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                }
                finally
                {
                    getCommand.SetCompleted(mData.Count);
                }
                return true;
            }

            private void backup()
            {
                raiseBeforeBackupEvent();

                BinaryFormatter serializer = new BinaryFormatter();
                try
                {
                    mBackupFileStream.SetLength(0);
                    mBackupFileStream.Seek(0, SeekOrigin.Begin);
                    serializer.Serialize(mBackupFileStream, mData);
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
                    mData = new InternalData();
                    backup();
                }

                BinaryFormatter serializer = new BinaryFormatter();
                try
                {
                    mBackupFileStream.Seek(0, SeekOrigin.Begin);
                    mData = (InternalData)serializer.Deserialize(mBackupFileStream);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }

                raiseAfterRestoreEvent();
            }

            private void raiseCountBeforeUpdateEvent(CountBeforeUpdateEventArgs count)
            {
                var eventHandlerCapture = CountBeforeUpdate;
                if (null != eventHandlerCapture)
                {
                    eventHandlerCapture.Invoke(this, count);
                }
            }

            private void raiseCountAfterUpdateEvent(int count)
            {
                var eventHandlerCapture = CountAfterUpdate;
                if (null != eventHandlerCapture)
                {
                    eventHandlerCapture.Invoke(this, count);
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

            private class InternalCommand : IDisposable
            {
                public ECommand Code { get; private set; } = ECommand.None;
                public int Value { get; private set; } = 0;
                public WaitHandle WaitProcessed { get; private set; } = new ManualResetEvent(false);
                public int Result { get; private set; } = 0;
                public InternalAsyncResult AsyncResult { get; private set; }

                public InternalCommand(ECommand commandCode, int value = 0, InternalAsyncResult asyncResultOrNull = null)
                {
                    Code = commandCode;
                    Value = value;
                    AsyncResult = asyncResultOrNull;
                    if (AsyncResult != null)
                    {
                        AsyncResult.mAsyncWaitHandle = WaitProcessed;
                    }
                }

                public void SetCompleted(int result)
                {
                    Result = result;
                    var waitHandle = WaitProcessed as ManualResetEvent;
                    if (AsyncResult != null)
                    {
                        AsyncResult.mAsyncState = Result;
                        AsyncResult.mbCompletedSynchronously = true;
                        AsyncResult.mbCompleted = true;
                        AsyncResult.mAsyncWaitHandle = null;
                    }
                    waitHandle.Set();
                    Dispose();
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
                            WaitProcessed.Close();
                        }

                        // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                        // TODO: 큰 필드를 null로 설정합니다.

                        disposedValue = true;
                    }
                }

                // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
                // ~Command() {
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

            private class InternalAsyncResult : IAsyncResult
            {
                public object AsyncState => mAsyncState;
                public WaitHandle AsyncWaitHandle => mAsyncWaitHandle;
                public bool CompletedSynchronously => mbCompletedSynchronously;
                public bool IsCompleted => mbCompleted;
                public object mAsyncState = null;
                public WaitHandle mAsyncWaitHandle = null;
                public bool mbCompletedSynchronously = false;
                public bool mbCompleted = false;
            }

            [Serializable]
            private class InternalData
            {
                public int Count { get; set; } = 0;
                public DateTime LastUpdateTime { get; set; } = DateTime.Now;
                public TimeSpan LastUpdateDuration => DateTime.Now - LastUpdateTime;
            }
        }
    }

}

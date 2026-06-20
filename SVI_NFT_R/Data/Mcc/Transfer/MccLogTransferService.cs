using Mcc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R
{
    public sealed class MccLogTransferService : IDisposable
    {
        private readonly IMccConfig mConfig;
        private readonly IClock mClock;
        private readonly IFileSystem mFileSystem;
        private readonly IRemoteFileClient mRemoteClient;
        private readonly IPathBuilder mPaths;
        private readonly IRotationPolicy mRotate;
        private readonly IRetentionPolicy mRetention;
        private readonly IMccLogQueue mQueue;
        private readonly SemaphoreSlim mWriteLock = new SemaphoreSlim(1, 1);

        private CancellationTokenSource mCancel;
        private Task mLogWriterTask;
        private Timer mUploadTimer;
        private Timer mDeleteTimer;
        private ConnectInfo mLastConnectInfo;

        private string mLastActionPath = string.Empty;
        private string mLastIndexPath = string.Empty;
        private bool mbDayChangedFlag = false;
        private int mLastDayOfYear = 0;
        private DateTime mLastUploadTickAt = DateTime.MinValue;
        private DateTime mLastManualRequest = DateTime.MinValue;

        public bool IsLastUploadSuccessed { get; private set; }
        public bool IsUploadCompleted => mRemoteClient.IsUploadComplete;

        public int UploadProgress => mRemoteClient.UploadProgressPercentage;
        public int UploadFilesTotalCount => mRemoteClient.UploadListTotalCount;
        public int UploadFilesCount => mRemoteClient.UploadListCount;
        public string UploadFilesPath => mRemoteClient.UploadListPath;
        public string UploadFilesName => mRemoteClient.UploadListFileName;

        public TimeSpan UploadLessMinutes
        {
            get
            {
                var period = TimeSpan.FromMinutes(Math.Max(1, mConfig.LogUploadTimeMinutes));
                var last = mLastUploadTickAt == DateTime.MinValue ? mClock.Now : mLastUploadTickAt;
                var elapsed = mClock.Now - last;
                var remaining = period - elapsed;
                if (remaining < TimeSpan.Zero)
                {
                    return TimeSpan.Zero;
                }
                return remaining;
            }
        }

        public MccLogTransferService(
            IMccConfig cfg,
            IRemoteFileClient remote,
            IMccLogQueue queue,
            IClock clock = null,
            IFileSystem fs = null,
            IPathBuilder paths = null,
            IRotationPolicy rotate = null,
            IRetentionPolicy retention = null)
        {
            mConfig = cfg;
            mRemoteClient = remote;
            mQueue = queue;
            mClock = clock ?? new SystemClock();
            mFileSystem = fs ?? new SystemFileSystem();
            mPaths = paths ?? new DefaultPathBuilder(cfg);
            mRotate = rotate ?? new TimeWindowRotationPolicy();
            mRetention = retention ?? new SimpleRetentionPolicy();
        }

        public void Start()
        {
            mLastUploadTickAt = mClock.Now;
            mCancel = new CancellationTokenSource();
            mLogWriterTask = Task.Run(() => LogWriterLoopAsync(mCancel.Token));

            mUploadTimer = new Timer(async _ => await OnUploadTickAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            var dueTime = TimeSpan.FromMinutes(5);
            mDeleteTimer = new Timer(_ => OnDeleteTick(), null, dueTime, TimeSpan.FromDays(1));
        }

        public void Stop()
        {
            if (mUploadTimer != null)
            {
                mUploadTimer.Dispose();
            }
            if (mDeleteTimer != null)
            {
                mDeleteTimer.Dispose();
            }
            if (mCancel != null)
            {
                mCancel.Cancel();
            }
            try
            {
                mLogWriterTask?.Wait();
            }
            catch
            {
                // ignore
            }
        }

        public void Dispose()
        {
            Stop();
            mUploadTimer?.Dispose();
            mDeleteTimer?.Dispose();
            mCancel?.Dispose();
            mWriteLock?.Dispose();
        }

        private async Task LogWriterLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (mQueue.Count > 0 && mQueue.TryDequeue(out var data))
                {
                    var ok = await WriteMccLogAsync(data);
                    if (!ok)
                    {
                        await Task.Delay(5, ct);
                        continue;
                    }
                    if (mQueue.Count > 0)
                    {
                        await Task.Delay(5, ct);
                        continue;
                    }
                }
                else
                {
                    await WriteNullTickAsync();
                    await Task.Delay(1000, ct);
                }

                await Task.Delay(500, ct);
            }
        }

        private async Task<bool> WriteMccLogAsync(MccLogData logData)
        {
            try
            {
                var tDir = mPaths.BuildLocalTDir(logData.WriteDateTime);
                if (!mFileSystem.DirectoryExists(tDir))
                {
                    mFileSystem.CreateDirectory(tDir);
                }

                var baseDir = tDir;
                var filePath = mRotate.ResolveActionFilePath(mClock.Now, mLastActionPath, mConfig.LoggingTimeMinutes, baseDir, mConfig.EquipmentId);
                mLastActionPath = filePath;

                await EnsureIndexFileAsync(logData.WriteDateTime);

                await mWriteLock.WaitAsync();
                try
                {
                    mFileSystem.AppendAllText(filePath, logData.WriteLogMessage + Environment.NewLine);
                }
                finally
                {
                    mWriteLock.Release();
                }

                return true;
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
                return false;
            }
        }

        private async Task WriteNullTickAsync()
        {
            try
            {
                var tDir = mPaths.BuildLocalTDir(mClock.Now);
                if (!mFileSystem.DirectoryExists(tDir))
                {
                    mFileSystem.CreateDirectory(tDir);
                }

                var baseDir = tDir;
                var filePath = mRotate.ResolveActionFilePath(mClock.Now, mLastActionPath, mConfig.LoggingTimeMinutes, baseDir, mConfig.EquipmentId);
                mLastActionPath = filePath;

                await EnsureIndexFileAsync(mClock.Now);

                await mWriteLock.WaitAsync();
                try
                {
                    if (mFileSystem.FileExists(filePath) == false)
                    {
                        mFileSystem.AppendAllText(filePath, string.Empty);
                    }
                }
                finally
                {
                    mWriteLock.Release();
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private async Task EnsureIndexFileAsync(DateTime dt)
        {
            try
            {
                var indexDir = mPaths.BuildLocalIndexDir();
                if (mFileSystem.DirectoryExists(indexDir) == false)
                {
                    mFileSystem.CreateDirectory(indexDir);
                }

                var filePath = mRotate.ResolveIndexFilePath(dt, mLastIndexPath, mConfig.LoggingTimeMinutes, indexDir, mConfig.EquipmentId);
                mLastIndexPath = filePath;

                await mWriteLock.WaitAsync();
                try
                {
                    if (mFileSystem.FileExists(filePath) == false)
                    {
                        mFileSystem.AppendAllText(filePath, string.Empty);
                    }
                }
                finally
                {
                    mWriteLock.Release();
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private async Task OnUploadTickAsync()
        {
            if (mConfig.IsMccEnabled == false)
            {
                return;
            }
            if (mWriteLock.CurrentCount == 0)
            {
                return;
            }
            var periodMinutes = Math.Max(1, mConfig.LogUploadTimeMinutes);
            var dueTime = mLastUploadTickAt == DateTime.MinValue ? true : (mClock.Now - mLastUploadTickAt) >= TimeSpan.FromMinutes(periodMinutes);

            if (dueTime == false)
            {
                return;
            }

            await RunUploadCoreAsync();
        }

        public async Task<bool> RunUploadNowAsync()
        {
            var now = mClock.Now;
            if ((now - mLastManualRequest) < TimeSpan.FromSeconds(2))
            {
                return false;
            }
            mLastManualRequest = now;
            if (mConfig.IsMccEnabled == false)
            {
                return false;
            }
            if (mWriteLock.CurrentCount == 0)
            {
                return false;
            }
            return await RunUploadCoreAsync();
        }

        private async Task<bool> RunUploadCoreAsync()
        {
            await mWriteLock.WaitAsync();
            try
            {
                if (mbDayChangedFlag == true)
                {
                    var yesterday = mClock.Now.AddDays(-1);
                    if (mQueue.Count == 0)
                    {
                        IsLastUploadSuccessed = await UploadForDateAsync(yesterday);
                        if (IsLastUploadSuccessed)
                        {
                            mbDayChangedFlag = false;
                            IsLastUploadSuccessed = await UploadForDateAsync(mClock.Now);
                        }
                    }
                }
                else
                {
                    IsLastUploadSuccessed = await UploadForDateAsync(mClock.Now);
                }

                // 업로드가 성공/실패와 무관하게 기준시점 갱신
                mLastUploadTickAt = mClock.Now;
                return IsLastUploadSuccessed;
            }
            finally
            {
                mWriteLock.Release();
            }
        }

        private async Task<bool> UploadForDateAsync(DateTime dt)
        {
            try
            {
                if (Equals(mLastConnectInfo, mConfig.RemoteConnectInfo) == false)
                {
                    if (mRemoteClient.SetConnectInformation(mConfig.RemoteConnectInfo) == false)
                    {
                        return false;
                    }
                    mLastConnectInfo = mConfig.RemoteConnectInfo;
                }

                var tLocal = mPaths.BuildLocalTDir(dt);
                var indexLocal = mPaths.BuildLocalIndexDir();
                var tRemote = mPaths.BuildRemoteFromLocal(tLocal);
                var indexRemote = mPaths.BuildRemoteFromLocal(indexLocal);

                mRemoteClient.CreateDirectory(tRemote);
                mRemoteClient.CreateDirectory(indexRemote);

                var ftpTNames = mRemoteClient
                    .GetAllFiles(tRemote)
                    .Select(Path.GetFileName)
                    .Where(IsValidTLogFileName)
                    .ToList();
                var localTNames = mFileSystem.DirectoryExists(tLocal)
                    ? new DirectoryInfo(tLocal)
                        .EnumerateFiles("*.csv")
                        .Select(f => f.Name)
                        .Where(IsValidTLogFileName)
                        .ToList()
                    : new List<string>();

                // 미업로드 파일 업로드
                var missing = localTNames.Except(ftpTNames).ToList();
                if (missing.Count > 0)
                {
                    if (mRemoteClient.UploadFiles(tLocal, tRemote, missing) == false)
                    {
                        return false;
                    }
                    var timeout = missing.Count * 1000;
                    var ok = await SpinUntilAsync(() => mRemoteClient.IsUploadComplete, timeout);
                    if (!ok)
                    {
                        return false;
                    }
                }

                var indexNames = missing
                    .Select(ToIndexName)
                    .Where(n => n != null)
                    .ToList();
                if (indexNames.Count > 0)
                {
                    if (!mRemoteClient.UploadFiles(indexLocal, indexRemote, indexNames))
                    {
                        return false;
                    }
                    var timeout = Math.Max(1, indexNames.Count) * 200;
                    var ok = await SpinUntilAsync(() => mRemoteClient.IsUploadComplete, timeout);
                    if (!ok)
                    {
                        return false;
                    }
                }

                // 이미 있는 파일은 크기 비교 후 재업로드 (유효한 이름만)
                var intersect = localTNames.Intersect(ftpTNames);
                foreach (var name in intersect)
                {
                    if (!IsValidTLogFileName(name))
                    {
                        continue;
                    }

                    var localPath = Path.Combine(tLocal, name);
                    if (!mFileSystem.FileExists(localPath))
                    {
                        continue;
                    }

                    var localSize = mFileSystem.GetFileSize(localPath);
                    var remotePath = tRemote + name;
                    var remoteSize = mRemoteClient.GetFileSize(remotePath);
                    if (remoteSize != -1L && remoteSize != localSize)
                    {
                        if (!mRemoteClient.UploadFile(localPath, remotePath))
                        {
                            return false;
                        }
                        var ok = await SpinUntilAsync(() => mRemoteClient.IsUploadComplete, 5000);
                        if (!ok)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
                return false;
            }
        }

        private async Task<bool> SpinUntilAsync(Func<bool> condition, int timeoutMs)
        {
            var start = mClock.Now;
            while (mCancel != null && !mCancel.IsCancellationRequested && (int)(mClock.Now - start).TotalMilliseconds < timeoutMs)
            {
                if (condition() == true)
                {
                    return true;
                }
                await Task.Delay(50, mCancel.Token);
            }
            return condition();
        }

        private void OnDeleteTick()
        {
            try
            {
                var baseDir = Path.Combine(
                    mConfig.LocalPath,
                    mConfig.BasicPath,
                    mConfig.EquipmentCategoryName,
                    mConfig.EquipmentId);

                foreach (var dir in mFileSystem.GetDirectories(baseDir))
                {
                    if (dir.Name.Equals("index", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (mRetention.ShouldDeleteDirectory(dir.LastWriteTime, mConfig.LogDeletePeriodDays))
                    {
                        try
                        {
                            mFileSystem.DeleteDirectory(dir.FullName, true);
                        }
                        catch (Exception ex)
                        {
                            LogWrite.Exception(ex);
                        }
                    }
                }

                var indexDir = Path.Combine(baseDir, "index");
                foreach (var file in mFileSystem.GetFiles(indexDir, "*.csv"))
                {
                    if (mRetention.ShouldDeleteFile(file.LastWriteTime, mConfig.LogDeletePeriodDays))
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception ex)
                        {
                            LogWrite.Exception(ex);
                        }
                    }
                }

                if (DateTime.Today.DayOfYear != mLastDayOfYear)
                {
                    mbDayChangedFlag = true;
                    mLastDayOfYear = DateTime.Today.DayOfYear;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private bool IsValidTLogFileName(string name)
        {
            return IsValidStampedFileName("T", name, out _);
        }

        private bool IsValidIndexFileName(string name)
        {
            return IsValidStampedFileName("index", name, out _);
        }

        private string ToIndexName(string tLogName)
        {
            if (IsValidStampedFileName("T", tLogName, out var ts) == false)
            {
                return null;
            }
            var candidate = $"index-{mConfig.EquipmentId}-{ts:yyyyMMddHHmm00}.csv";
            return IsValidIndexFileName(candidate) ? candidate : null;
        }

        private bool IsValidStampedFileName(string expectedPrefix, string fileName, out DateTime timestamp)
        {
            timestamp = default;
            var escapedId = Regex.Escape(mConfig.EquipmentId);
            var pattern = $"^{expectedPrefix}-{escapedId}-(\\d{{14}})\\.csv$";
            var match = Regex.Match(fileName, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return false;
            }

            var tsText = match.Groups[1].Value;
            if (!DateTime.TryParseExact(tsText, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp))
            {
                return false;
            }

            if (timestamp.Second != 0)
            {
                return false;
            }

            var loggingMinutes = Math.Max(1, mConfig.LoggingTimeMinutes);
            if (loggingMinutes >= 60)
            {
                if (timestamp.Minute != 0)
                {
                    return false;
                }
            }
            else
            {
                if ((timestamp.Minute % loggingMinutes) != 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

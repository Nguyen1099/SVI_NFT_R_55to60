using System.Collections.Generic;

namespace SVI_NFT_R
{
    public interface IRemoteFileClient
    {
        bool SetConnectInformation(ConnectInfo info);
        void CreateDirectory(string remoteDir);
        List<string> GetAllFiles(string remoteDir);
        bool UploadFiles(string localDir, string remoteDir, List<string> fileNames);
        bool UploadFile(string localPath, string remotePath);
        long GetFileSize(string remotePath);


        // Properties
        bool IsUploadComplete { get; }
        int UploadProgressPercentage { get; }
        int UploadListTotalCount { get; }
        int UploadListCount { get; }
        string UploadListPath { get; }
        string UploadListFileName { get; }
    }
}

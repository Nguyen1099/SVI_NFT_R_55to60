using HLDevice.Abstract;
using System.Collections.Generic;

namespace SVI_NFT_R
{
    public sealed class RemoteFileClientAdapter : IRemoteFileClient
    {
        private readonly HLDevice.CDeviceFileInterface mInner;

        public RemoteFileClientAdapter(HLDevice.CDeviceFileInterface inner)
        {
            mInner = inner;
        }

        public bool SetConnectInformation(ConnectInfo info)
        {
            var s = new CDeviceFileInterfaceAbstract.CInitializeParameter.structureConnectInformation
            {
                strHostName = info.HostName,
                strHostPort = info.HostPort,
                strUserName = info.UserName,
                strUserPassword = info.UserPassword
            };
            return mInner.HLSetConnectInformation(s);
        }

        public void CreateDirectory(string remoteDir)
        {
            mInner.HLCreateDirectory(remoteDir);
        }

        public List<string> GetAllFiles(string remoteDir)
        {
            return mInner.HLGetAllFiles(remoteDir);
        }

        public bool UploadFiles(string localDir, string remoteDir, List<string> fileNames)
        {
            return mInner.HLUploadFiles(localDir, remoteDir, fileNames);
        }

        public bool UploadFile(string localPath, string remotePath)
        {
            return mInner.HLUploadFile(localPath, remotePath);
        }

        public long GetFileSize(string remotePath)
        {
            return mInner.HLGetFileSize(remotePath);
        }

        public bool IsUploadComplete => mInner.m_bUploadComplete;
        public int UploadProgressPercentage => mInner.m_iUpProgressPercentage;
        public int UploadListTotalCount => mInner.UploadListTotalCount;
        public int UploadListCount => mInner.UploadListCount;
        public string UploadListPath => mInner.UploadListPath;
        public string UploadListFileName => mInner.UploadListFileName;
    }
}

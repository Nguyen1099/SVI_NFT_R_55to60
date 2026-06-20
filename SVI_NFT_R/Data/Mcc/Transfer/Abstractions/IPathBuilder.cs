using System;

namespace SVI_NFT_R
{
    public interface IPathBuilder
    {
        string BuildLocalTDir(DateTime dt);
        string BuildLocalIndexDir();
        string BuildRemoteFromLocal(string localDir);
        string BuildActionFileName(DateTime dt, string equipmentId);
        string BuildIndexFileName(DateTime dt, string equipmentId);
    }
}

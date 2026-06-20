using System;

namespace SVI_NFT_R
{
    public interface IRotationPolicy
    {
        string ResolveActionFilePath(DateTime now, string lastActionPath, int loggingMinutes, string baseDir, string equipmentId);
        string ResolveIndexFilePath(DateTime now, string lastIndexPath, int loggingMinutes, string baseDir, string equipmentId);
    }
}

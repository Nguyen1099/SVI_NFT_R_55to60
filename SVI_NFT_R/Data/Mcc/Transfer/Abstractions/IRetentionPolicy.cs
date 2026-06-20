using System;

namespace SVI_NFT_R
{
    public interface IRetentionPolicy
    {
        bool ShouldDeleteDirectory(DateTime creationTime, int keepDays);
        bool ShouldDeleteFile(DateTime creationTime, int keepDays);
    }
}

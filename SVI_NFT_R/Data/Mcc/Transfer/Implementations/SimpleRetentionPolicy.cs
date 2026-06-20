using System;

namespace SVI_NFT_R
{
    public sealed class SimpleRetentionPolicy : IRetentionPolicy
    {
        public bool ShouldDeleteDirectory(DateTime creationTime, int keepDays)
        {
            return DateTime.Now - creationTime > TimeSpan.FromDays(keepDays);
        }

        public bool ShouldDeleteFile(DateTime creationTime, int keepDays)
        {
            return DateTime.Now - creationTime > TimeSpan.FromDays(keepDays);
        }
    }
}

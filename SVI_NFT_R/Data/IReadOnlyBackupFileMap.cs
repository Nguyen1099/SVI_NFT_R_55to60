using System.Collections.Generic;

namespace SVI_NFT_R
{
    public interface IReadOnlyBackupFileMap<TKey, TValue>
    {
        IReadOnlyDictionary<TKey, TValue> Map { get; }
        object MapLock { get; }

        bool TryGetValue(TKey key, out TValue value);
    }

}

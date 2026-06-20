using System;
using System.Collections.Generic;

namespace SVI_NFT_R
{
    public interface IBackupFileMapUserInterface<TKey, TValue>
    {
        IReadOnlyDictionary<TKey, TValue> Map { get; }
        object MapLock { get; }

        bool TryGetValue(TKey key, out TValue value);
        void Remove(TKey key);
        void ForEach(Action<TKey, TValue> action, bool bShouldBackupAfterAction = true);
    }
}

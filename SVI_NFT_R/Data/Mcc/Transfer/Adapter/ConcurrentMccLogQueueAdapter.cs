using Mcc;
using System;
using System.Collections.Concurrent;

namespace SVI_NFT_R
{
    public sealed class ConcurrentMccLogQueueAdapter : IMccLogQueue
    {
        private readonly ConcurrentQueue<MccLogData> mQueue;

        public ConcurrentMccLogQueueAdapter(ConcurrentQueue<MccLogData> queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }
            mQueue = queue;
        }

        public bool TryDequeue(out MccLogData data)
        {
            return mQueue.TryDequeue(out data);
        }

        public int Count
        {
            get
            {
                return mQueue.Count;
            }
        }

        public void Enqueue(MccLogData item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            mQueue.Enqueue(item);
        }
    }
}

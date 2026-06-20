using Mcc;

namespace SVI_NFT_R
{
    public interface IMccLogQueue
    {
        bool TryDequeue(out MccLogData data);
        int Count { get; }
    }
}

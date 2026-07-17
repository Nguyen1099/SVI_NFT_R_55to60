using SVI_NFT_R.CellData;

namespace Mcc
{
    public interface IMccLogItem
    {
        bool IsExist { get; }
        void WriteStart(OneCell cellDataOrNull = null);
        void WriteEnd();
        void WriteStartExist(OneCell cellDataOrNull = null);
        void WriteEndExist();
    }
}

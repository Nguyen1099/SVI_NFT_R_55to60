namespace SVI_NFT_R.Data
{
    public interface ISerialReaderInitializer
    {
        bool IsInitialized { get; }
        object Index { get; set; }
        string Name { get; set; }

        bool Initilaize();
        void DeInitilaize();
    }
}
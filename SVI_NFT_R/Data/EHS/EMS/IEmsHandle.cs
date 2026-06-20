namespace SVI_NFT_R
{
    public interface IEmsHandle
    {
        EEmsGroupFlags EmergencyStopGroup { get; }

        void EmergencyStop();
    }
}
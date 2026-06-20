namespace SVI_NFT_R
{
    public interface IMccConfig
    {
        string LocalPath { get; }
        string BasicPath { get; }
        string EquipmentCategoryName { get; }
        int LogUploadTimeMinutes { get; }
        int LogDeletePeriodDays { get; }
        int LoggingTimeMinutes { get; }
        bool IsMccEnabled { get; }
        string EquipmentId { get; }
        ConnectInfo RemoteConnectInfo { get; }
    }
}

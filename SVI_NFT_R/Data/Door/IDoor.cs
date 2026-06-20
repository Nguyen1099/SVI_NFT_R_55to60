namespace SVI_NFT_R.Door
{
    public interface IDoor
    {
        bool IsUnlockPermit { get; set; }
        bool IsLocked { get; }
        bool IsOpened { get; }
        bool IsForcedUnlock { get; }
        bool IsAutoMode { get; }
        bool HasOutsideKey { get; }
        bool HasInsideKey { get; }
        bool CanUnlock { get; }
        bool DoorOutsideKeyLockSol { get; set; }
        bool DoorLockSol { get; set; }
        EGroup GroupIndex { get; }
        EDoor DoorIndex { get; }
        EType DoorType { get; }
    }
}

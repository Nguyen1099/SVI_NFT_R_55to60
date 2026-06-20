using SVI_NFT_R.Door;
using System;

namespace SVI_NFT_R
{
    public static partial class DoorManager
    {
        private class OneDoor : IDoor
        {
            public bool IsUnlockPermit { get; set; } = false;
            public bool IsLocked => GetIsLocked.Invoke(this);
            public bool IsOpened => GetIsOpened.Invoke();
            public bool IsAutoMode => GetIsAutoMode.Invoke();
            public bool IsForcedUnlock
            {
                get
                {
                    // 언락 허가 상태다
                    if (IsUnlockPermit == true)
                    {
                        return false;
                    }
                    // 티치 모드다
                    if (IsAutoMode == false)
                    {
                        return false;
                    }
                    // 잠금 출력이 꺼져있다
                    if (DoorLockSol == false)
                    {
                        return false;
                    }
                    // 잠금 상태다
                    if (IsLocked == true)
                    {
                        return false;
                    }
                    return true;
                }
            }
            public bool HasOutsideKey => GetHasOutsideKey.Invoke();
            public bool HasInsideKey => GetHasInsideKey.Invoke();
            public bool CanUnlock
            {
                get
                {
                    // 언락 미허가 상태다
                    if (IsUnlockPermit == false)
                    {
                        return false;
                    }
                    // 자동 모드다
                    if (IsAutoMode == true)
                    {
                        return false;
                    }
                    // 외부키가 감지되어 있다
                    if (HasOutsideKey == true)
                    {
                        return false;
                    }
                    return true;
                }
            }
            public bool DoorOutsideKeyLockSol { get { return GetOutsideKeyLockSol.Invoke(); } set { SetOutsideKeyLockSol.Invoke(value); } }
            public bool DoorLockSol { get { return GetDoorLockSol.Invoke(); } set { SetDoorLockSol.Invoke(value); } }
            public EGroup GroupIndex { get; private set; }
            public EDoor DoorIndex { get; private set; }
            public EType DoorType { get; private set; }
            private readonly CDocument mDocument;
            public Func<OneDoor, bool> GetIsLocked;
            public Func<bool> GetIsOpened;
            public Func<bool> GetIsAutoMode;
            public Func<bool> GetHasOutsideKey;
            public Func<bool> GetHasInsideKey = () => false;
            public Func<bool> GetOutsideKeyLockSol;
            public Action<bool> SetOutsideKeyLockSol;
            public Func<bool> GetDoorLockSol;
            public Action<bool> SetDoorLockSol;

            public OneDoor(CDocument doc, EGroup group, EDoor door, EType type)
            {
                mDocument = doc;
                GroupIndex = group;
                DoorIndex = door;
                DoorType = type;
            }
        }
    }
}

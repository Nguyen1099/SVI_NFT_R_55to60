using SVI_NFT_R.Door;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SVI_NFT_R
{
    public static partial class DoorManager
    {
        public static bool IsInitialized { get; private set; } = false;
        public static IReadOnlyDictionary<EDoor, IDoor> Doors => mDoors;
        public static IReadOnlyDictionary<EGroup, IReadOnlyList<IDoor>> Groups => mGroups;
        private readonly static Dictionary<EDoor, IDoor> mDoors = new Dictionary<EDoor, IDoor>();
        private readonly static Dictionary<EGroup, IReadOnlyList<IDoor>> mGroups = new Dictionary<EGroup, IReadOnlyList<IDoor>>();
        private readonly static EDoor[] mAllDoor = Enum.GetValues(typeof(EDoor)).Cast<EDoor>().ToArray();
        private readonly static EGroup[] mAllGroup = Enum.GetValues(typeof(EGroup)).Cast<EGroup>().ToArray();
        private static CDocument mDocument;

        public static bool Initialize(CDocument doc)
        {
            if (IsInitialized == true)
            {
                return false;
            }
            mDocument = doc;
            foreach (EDoor index in mAllDoor)
            {
                mDoors.Add(index, GetDoorInstance(index));
            }
            foreach (EGroup index in mAllGroup)
            {
                mGroups.Add(index, mDoors.Values.Where(item => item.GroupIndex == index).ToList());
            }

            IsInitialized = true;
            return true;
        }

        public static void DeInitialize()
        {
            if (IsInitialized == false)
            {
                return;
            }
            mDoors.Clear();
            mGroups.Clear();

            IsInitialized = false;
        }

        private static IDoor GetDoorInstance(EDoor index)
        {
            OneDoor result = null;
            switch (index)
            {
                case EDoor.MainFront1:
                    result = new OneDoor(mDocument, EGroup.KeyFront, index, EType.Key);
                    result.GetIsAutoMode = GetIsAutoMode;
                    result.SetOutsideKeyLockSol = signal => DoNothing();
                    result.GetOutsideKeyLockSol = () => false;
                    result.SetDoorLockSol = signal => SetDoorLockSol(signal, CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_1_UNLOCK);
                    result.GetDoorLockSol = () => GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_1_UNLOCK);
                    result.GetIsLocked = door => door.IsUnlockPermit == false && GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_1_UNLOCK) == true;
                    result.GetIsOpened = () => GetIsOpened(CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_1_OPEN);
                    result.GetHasOutsideKey = () => false;
                    result.GetHasInsideKey = () => false;
                    break;

                case EDoor.MainFront2:
                    result = new OneDoor(mDocument, EGroup.KeyFront, index, EType.Key);
                    result.GetIsAutoMode = GetIsAutoMode;
                    result.SetOutsideKeyLockSol = signal => DoNothing();
                    result.GetOutsideKeyLockSol = () => false;
                    result.SetDoorLockSol = signal => SetDoorLockSol(signal, CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_2_UNLOCK);
                    result.GetDoorLockSol = () => GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_2_UNLOCK);
                    result.GetIsLocked = door => door.IsUnlockPermit == false && GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_2_UNLOCK) == true;
                    result.GetIsOpened = () => GetIsOpened(CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_2_OPEN);
                    result.GetHasOutsideKey = () => false;
                    result.GetHasInsideKey = () => false;
                    break;

                case EDoor.MainFront3:
                    result = new OneDoor(mDocument, EGroup.KeyFront, index, EType.Key);
                    result.GetIsAutoMode = GetIsAutoMode;
                    result.SetOutsideKeyLockSol = signal => DoNothing();
                    result.GetOutsideKeyLockSol = () => false;
                    result.SetDoorLockSol = signal => SetDoorLockSol(signal, CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_3_UNLOCK);
                    result.GetDoorLockSol = () => GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_3_UNLOCK);
                    result.GetIsLocked = door => door.IsUnlockPermit == false && GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_3_UNLOCK) == true;
                    result.GetIsOpened = () => GetIsOpened(CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_3_OPEN);
                    result.GetHasOutsideKey = () => false;
                    result.GetHasInsideKey = () => false;
                    break;

                case EDoor.MainFront4:
                    result = new OneDoor(mDocument, EGroup.KeyFront, index, EType.Key);
                    result.GetIsAutoMode = GetIsAutoMode;
                    result.SetOutsideKeyLockSol = signal => DoNothing();
                    result.GetOutsideKeyLockSol = () => false;
                    result.SetDoorLockSol = signal => SetDoorLockSol(signal, CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_4_UNLOCK);
                    result.GetDoorLockSol = () => GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_4_UNLOCK);
                    result.GetIsLocked = door => door.IsUnlockPermit == false && GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_FRONT_SAFETY_DOOR_4_UNLOCK) == true;
                    result.GetIsOpened = () => GetIsOpened(CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_4_OPEN);
                    result.GetHasOutsideKey = () => false;
                    result.GetHasInsideKey = () => false;
                    break;

                case EDoor.MainRear1:
                    result = new OneDoor(mDocument, EGroup.KeyRear, index, EType.Key);
                    result.GetIsAutoMode = GetIsAutoMode;
                    result.SetOutsideKeyLockSol = signal => DoNothing();
                    result.GetOutsideKeyLockSol = () => false;
                    result.SetDoorLockSol = signal => SetDoorLockSol(signal, CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_1_UNLOCK);
                    result.GetDoorLockSol = () => GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_1_UNLOCK);
                    result.GetIsLocked = door => door.IsUnlockPermit == false && GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_1_UNLOCK) == true;
                    result.GetIsOpened = () => GetIsOpened(CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_1_OPEN);
                    result.GetHasOutsideKey = () => false;
                    result.GetHasInsideKey = () => false;
                    break;

                case EDoor.MainRear2:
                    result = new OneDoor(mDocument, EGroup.KeyRear, index, EType.Key);
                    result.GetIsAutoMode = GetIsAutoMode;
                    result.SetOutsideKeyLockSol = signal => DoNothing();
                    result.GetOutsideKeyLockSol = () => false;
                    result.SetDoorLockSol = signal => SetDoorLockSol(signal, CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_2_UNLOCK);
                    result.GetDoorLockSol = () => GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_2_UNLOCK);
                    result.GetIsLocked = door => door.IsUnlockPermit == false && GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_2_UNLOCK) == true;
                    result.GetIsOpened = () => GetIsOpened(CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_2_OPEN);
                    result.GetHasOutsideKey = () => false;
                    result.GetHasInsideKey = () => false;
                    break;

                case EDoor.MainRear3:
                    result = new OneDoor(mDocument, EGroup.KeyRear, index, EType.Key);
                    result.GetIsAutoMode = GetIsAutoMode;
                    result.SetOutsideKeyLockSol = signal => DoNothing();
                    result.GetOutsideKeyLockSol = () => false;
                    result.SetDoorLockSol = signal => SetDoorLockSol(signal, CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_3_UNLOCK);
                    result.GetDoorLockSol = () => GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_3_UNLOCK);
                    result.GetIsLocked = door => door.IsUnlockPermit == false && GetDoorLockSol(CDeviceIODefine.EDigitalOutput.Y_REAR_SAFETY_DOOR_3_UNLOCK) == true;
                    result.GetIsOpened = () => GetIsOpened(CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_3_OPEN);
                    result.GetHasOutsideKey = () => false;
                    result.GetHasInsideKey = () => false;
                    break;

                default:
                    Debug.Assert(false, $"미등록 도어 초기화 시도함 ({index})");
                    break;
            }
            return result;
        }

        private static bool GetHasInsideKey(CDeviceIODefine.EDigitalInput index)
        {
            return mDocument.GetIOBit(index) == true;
        }

        private static bool GetHasOutsideKey(CDeviceIODefine.EDigitalInput index)
        {
            return mDocument.GetIOBit(index) == false;
        }

        private static bool GetIsOpened(CDeviceIODefine.EDigitalInput index)
        {
            return mDocument.GetIOBit(index) == true;
        }

        private static bool GetIsLocked(CDeviceIODefine.EDigitalInput index)
        {
            return mDocument.GetIOBit(index) == false;
        }

        private static void SetDoorLockSol(bool bLock, CDeviceIODefine.EDigitalOutput index)
        {
            mDocument.SetIOBit(index, !bLock);
        }

        private static bool GetDoorLockSol(CDeviceIODefine.EDigitalOutput index)
        {
            return mDocument.GetIOBit(index) == false;
        }

        private static void SetOutsideKeyLockSol(bool bLock, CDeviceIODefine.EDigitalOutput index)
        {
            mDocument.SetIOBit(index, !bLock);
        }

        private static bool GetOutsideKeyLockSol(CDeviceIODefine.EDigitalOutput index)
        {
            return mDocument.GetIOBit(index) == false;
        }

        private static void DoNothing()
        {
        }

        private static bool GetIsAutoMode()
        {
            return mDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH) == false;
        }
    }
}

using EqToEq;
using System.Diagnostics;

namespace SVI_NFT_R.EHS
{
    public static partial class ExtensionMethods
    {
        private static CDocument mDocument;

        public static bool CheckUpperEqInterfaceDoorOpened(this IProcessManagerLoadInterface loadInterface)
        {
            if (ShouldCheckEquipmentInterface() == false)
            {
                return false;
            }
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            return loadInterface.IsUpperDoorOpen;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        }

        public static bool CheckUpperEqInterfaceInterlocked(this IProcessManagerLoadInterface loadInterface)
        {
            if (ShouldCheckEquipmentInterface() == false)
            {
                return false;
            }
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            return loadInterface.IsUpperInterlock;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        }

        public static bool CheckUpperEqInterfaceOnEMS(this IProcessManagerLoadInterface loadInterface)
        {
            if (ShouldCheckEquipmentInterface() == false)
            {
                return false;
            }
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            return loadInterface.IsUpperEmergency;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        }

        public static bool CheckLowerEqInterfaceDoorOpened(this IProcessManagerUnloadInterface unloadInterface)
        {
            if (ShouldCheckEquipmentInterface() == false)
            {
                return false;
            }
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            return unloadInterface.IsLowerDoorOpen;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        }

        public static bool CheckLowerEqInterfaceInterlocked(this IProcessManagerUnloadInterface unloadInterface)
        {
            if (ShouldCheckEquipmentInterface() == false)
            {
                return false;
            }
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            return unloadInterface.IsLowerInterlock;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        }

        public static bool CheckLowerEqInterfaceOnEMS(this IProcessManagerUnloadInterface unloadInterface)
        {
            if (ShouldCheckEquipmentInterface() == false)
            {
                return false;
            }
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            return unloadInterface.IsLowerEmergency;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        }

        public static void EmergencyStopAll(this CProcessMotion processMotion)
        {
            foreach (var motion in processMotion.AllMotions)
            {
                if (motion is IEmsHandle emergencyStopHandle)
                {
                    emergencyStopHandle.EmergencyStop();
                }
            }
        }

        public static void EmergencyStopGroup(this CProcessMotion processMotion, EEmsGroupFlags group)
        {
            foreach (var motion in processMotion.AllMotions)
            {
                if (motion is IEmsHandle emergencyStopHandle
                    && emergencyStopHandle.EmergencyStopGroup.HasFlag(group)
                    )
                {
                    emergencyStopHandle.EmergencyStop();
                }
            }
        }

        public static void EmergencyStopGroupByEhsPolicy(this CProcessMotion processMotion)
        {
            foreach (var motion in processMotion.AllMotions)
            {
                if (motion is IEmsHandle emergencyStopHandle
                    && emergencyStopHandle.CanMovingGroupByEhsPolicy() == false
                    )
                {
                    emergencyStopHandle.EmergencyStop();
                }
            }
        }

        public static bool CanMovingGroupByEhsPolicy(this IEmsHandle emsHandle)
        {
            // 설비가 그룹별로 비상정지되는 정책을 이 함수에 구현한다.
            // 1. 상, 하류 설비에 문이 열리면 설비 내부에 모든 파트는 비상정지된다.
            var upper = mDocument.m_objProcessMain.m_objProcessMotion.LoadInterface;
            switch (emsHandle.EmergencyStopGroup)
            {
                case EEmsGroupFlags.None:
                case EEmsGroupFlags.Loader:
                case EEmsGroupFlags.Unloader:
                    return upper.CheckUpperEqInterfaceDoorOpened() == false;

                default:
                    Debug.Assert(false);
                    break;
            }
            return false;
        }

        internal static void SetDocument(CDocument document)
        {
            mDocument = document;
        }

        private static bool ShouldCheckEquipmentInterface()
        {
            // 마스터 모드 체크
            if (mDocument.IsMasterLogin)
            {
                return false;
            }
            // 수동 투입 모드 체크
            if (mDocument.IsManualInputMode)
            {
                return false;
            }
            return true;
        }
    }
}
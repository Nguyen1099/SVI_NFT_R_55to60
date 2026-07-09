using System;

namespace EqToEq
{
    /// <summary>
    /// 배출 Handshake 인터페이스
    /// </summary>
    public interface IProcessManagerUnloadInterface : IProcessManagerInterface
    {
        /// <summary>
        /// 하류 설비 정상 동작 상태
        /// </summary>
        bool IsLowerHeartBeat { get; }
        /// <summary>
        /// 하류 설비 비상정지 상태
        /// </summary>
        [Obsolete("대신에 'EHS.ExtensionMethod.CheckLowerEqInterfaceOnEMS()' 확장 메서드를 사용하세요.")]
        bool IsLowerEmergency { get; }
        /// <summary>
        /// 하류 설비 비정상 상태
        /// </summary>
        bool IsLowerAbnormal { get; }
        /// <summary>
        /// 하류 설비 일시정지 상태
        /// </summary>
        bool IsLowerPause { get; }
        /// <summary>
        /// 하류 설비 인터락 상태
        /// </summary>
        [Obsolete("대신에 'EHS.ExtensionMethod.CheckLowerEqInterfaceInterlocked()' 확장 메서드를 사용하세요.")]
        bool IsLowerInterlock { get; }
        /// <summary>
        /// 하류 설비 문 열림 상태
        /// </summary>
        [Obsolete("대신에 'EHS.ExtensionMethod.CheckLowerEqInterfaceDoorOpened()' 확장 메서드를 사용하세요.")]
        bool IsLowerDoorOpen { get; }
        /// <summary>
        /// MC 상태
        /// </summary>
        bool IsLowerMcStatus { get; }
        /// <summary>
        /// Heavy Alarm 상태
        /// </summary>
        bool IsLowerHeavyAlarm { get; }
        /// <summary>
        /// Light Alarm 상태
        /// </summary>
        bool IsLowerLightAlarm { get; }
        /// <summary>
        /// 장비 전체 초기화 상태
        /// </summary>
        bool IsLowerInitialStatus { get; }
        /// <summary>
        /// 언로드 동작 완료 플래그
        /// </summary>
        bool IsUnloadCompleted { get; set; }
        /// <summary>
        /// 하류 설비의 모든 신호 이름 배열 반환
        /// </summary>
        string[] LowerSignalNames { get; }
    }
}
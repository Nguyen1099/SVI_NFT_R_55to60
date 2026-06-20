using System;

namespace EqToEq
{
    /// <summary>
    /// 투입 Handshake 인터페이스
    /// </summary>
    public interface IProcessManagerLoadInterface : IProcessManagerInterface
    {
        /// <summary>
        /// 상위 설비 정상 동작 상태
        /// </summary>
        bool IsUpperHeartbeat { get; }
        /// <summary>
        /// 상위 설비 비상정지 상태
        /// </summary>
        [Obsolete("대신에 'EHS.ExtensionMethod.CheckUpperEqInterfaceOnEMS()' 확장 메서드를 사용하세요.")]
        bool IsUpperEmergency { get; }
        /// <summary>
        /// 상위 설비 비정상 상태
        /// </summary>
        bool IsUpperAbnormal { get; }
        /// <summary>
        /// 상위 설비 일시정지 상태
        /// </summary>
        bool IsUpperPause { get; }
        /// <summary>
        /// 상위 설비 인터락 상태
        /// </summary>
        [Obsolete("대신에 'EHS.ExtensionMethod.CheckUpperEqInterfaceInterlocked()' 확장 메서드를 사용하세요.")]
        bool IsUpperInterlock { get; }
        /// <summary>
        /// 상위 설비 문 열림 상태
        /// </summary>
        [Obsolete("대신에 'EHS.ExtensionMethod.CheckUpperEqInterfaceDoorOpened()' 확장 메서드를 사용하세요.")]
        bool IsUpperDoorOpen { get; }
        /// <summary>
        /// MC 상태
        /// </summary>
        bool IsUpperMcStatus { get; }
        /// <summary>
        /// Heavy Alarm 상태
        /// </summary>
        bool IsUpperHeavyAlarm { get; }
        /// <summary>
        /// Light Alarm 상태
        /// </summary>
        bool IsUpperLightAlarm { get; }
        /// <summary>
        /// 장비 전체 초기화 상태
        /// </summary>
        bool IsUpperInitialStatus { get; }
        /// <summary>
        /// 로드 동작 완료 플래그
        /// </summary>
        bool IsLoadCompleted { get; set; }
        /// <summary>
        /// 상위 설비의 모든 신호 이름 배열 반환
        /// </summary>
        string[] UpperSignalNames { get; }

        /// <summary>
        /// 상류 설비의 인터락 상태 조회
        /// </summary>
        bool GetUpperInterlock(int position);
    }
}

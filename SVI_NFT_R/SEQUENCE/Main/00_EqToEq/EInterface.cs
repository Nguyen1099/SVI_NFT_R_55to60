namespace EqToEq
{
    /// <summary>
    /// 인터페이스 공통 동작
    /// </summary>
    public enum EInterface
    {
        /// <summary>
        /// Able을 켜기 전에 수행 할 동작 (복수 정의 가능)
        /// </summary>
        PreAction,

        /// <summary>
        /// Handshake 진행이 가능한 상태에서 Able 신호를 켜는 동작
        /// </summary>
        Able,

        /// <summary>
        /// 인터페이스를 시작하고 Start 신호를 켜는 동작
        /// </summary>
        Start,

        /// <summary>
        /// 인터페이스 진행중에 수행 할 동작 (복수 정의 가능)
        /// </summary>
        MidAction,

        /// <summary>
        /// 인터페이스 완료 후 Complete 신호를 켜는 동작
        /// </summary>
        Complete,

        /// <summary>
        /// 인터페이스 완료 후 수행 할 동작 (복수 정의 가능)
        /// </summary>
        PostAction,

        /// <summary>
        /// 인터페이스 설비 간 AutoRun 기능 제어
        /// </summary>
        AutoRun,

        /// <summary>
        /// 인터페이스 완료후 신호 초기화 동작
        /// </summary>
        ClearAllInterfaceSignals
    }

    public enum EInterfaceCP
    {
        /// <summary>
        /// MC 의 현재 상태 (0:Stop, 1:Run)
        /// </summary>
        McStatus,
        /// <summary>
        /// 중 알람 발생 상태 (0:Occur, 1:Normal)
        /// </summary>
        HeavyAlarm,
        /// <summary>
        /// 경 알람 발생 상태 (0:Occur, 1:Normal)
        /// </summary>
        LightAlarm,
        /// <summary>
        /// 초기화 상태 (0:Not, 1: Complete)
        /// </summary>
        InitialStatus
    }

    public enum EResultInterface
    {
        /// <summary>
        /// 결과 전송 Able 동작
        /// </summary>
        Able,
        /// <summary>
        /// 결과 전송 완료후 신호 초기화 동작
        /// </summary>
        ClearAllInterfaceSignals,
        /// <summary>
        /// 결과 전송 Alarm 처리
        /// </summary>
        Alarm,
    }
}
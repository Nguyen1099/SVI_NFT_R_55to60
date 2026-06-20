namespace EqToEq
{
    /// <summary>
    /// Handshake 에서 공통으로 사용되는 인터페이스
    /// </summary>
    public interface IProcessManagerInterface
    {
        /// <summary>
        /// 설비는 준비가 되었으나 타 설비에 의해 인터페이스가 지연되고 있는 상황에 True
        /// </summary>
        bool IsPending { get; }
        /// <summary>
        /// 상, 하류 설비의 Able과 Start 신호가 모두 켜진 시점부터 모든 신호가 클리어 될 때까지 True
        /// </summary>
        bool IsHandShaking { get; }
        /// <summary>
        /// 매니져의 초기화 진행 여부
        /// </summary>
        bool IsInitialized { get; }
        /// <summary>
        /// 설비 정상 동작 상태
        /// </summary>
        bool IsSelfHeartBeat { get; }
        /// <summary>
        /// 설비 비상정지 상태
        /// </summary>
        bool IsSelfEmergency { get; }
        /// <summary>
        /// 설비 비정상 상태
        /// </summary>
        bool IsSelfAbnormal { get; }
        /// <summary>
        /// 설비 일시정지 상태
        /// </summary>
        bool IsSelfPause { get; }
        /// <summary>
        /// 설비 Ch1 인터락 상태
        /// </summary>
        bool IsSelfInterlock { get; }
        /// <summary>
        /// 설비 문 열림 상태
        /// </summary>
        bool IsSelfDoorOpen { get; }
        /// <summary>
        /// MC 상태
        /// </summary>
        bool IsSelfMcStatus { get; }
        /// <summary>
        /// Heavy Alarm 상태
        /// </summary>
        bool IsSelfHeavyAlarm { get; }
        /// <summary>
        /// Light Alarm 상태
        /// </summary>
        bool IsSelfLightAlarm { get; }
        /// <summary>
        /// 장비 전체 초기화 상태
        /// </summary>
        bool IsSelfInitialStatus { get; }
        /// <summary>
        /// 설비의 모든 신호 이름 배열 반환
        /// </summary>
        string[] SelfSignalNames { get; }
        /// <summary>
        /// 신호를 켜고 끄는 컨트롤러 객체
        /// </summary>
        object[] SignalControllers { get; }

        /// <summary>
        /// 매니져 초기화
        /// </summary>
        /// <param name="document">도큐먼트</param>
        /// <param name="positionIndex">포지션 인덱스</param>
        /// <param name="args">파라메터</param>
        /// <returns>초기화 성공 여부</returns>
        bool Initialize(SVI_NFT_R.CDocument document, int positionIndex);
        /// <summary>
        /// 정리
        /// </summary>
        void DeInitialize();
        /// <summary>
        /// 주기적으로 호출하여 업데이트해야하는 부분을 구현함
        /// </summary>
        void UpdateTickHeartbeatSignals();
        /// <summary>
        /// 설비 비상정지 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetEmergency(bool bSignal);
        /// <summary>
        /// 설비 비정상 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetAbnormal(bool bSignal);
        /// <summary>
        /// 설비 일시정지 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetPause(bool bSignal);
        /// <summary>
        /// 인터락 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetInterlock(int position, bool bSignal);
        /// <summary>
        /// 인터락 상태 조회
        /// </summary>
        /// <param name="bSignal"></param>
        bool GetSelfInterlock(int position);
        /// <summary>
        /// 설비 도어 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetDoorOpen(bool bSignal);
        /// <summary>
        /// 설비 도어 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetMcStatus(bool bSignal);
        /// <summary>
        /// 설비 도어 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetHeavyAlarm(bool bSignal);
        /// <summary>
        /// 설비 도어 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetLightAlarm(bool bSignal);
        /// <summary>
        /// 설비 도어 상태 업데이트
        /// </summary>
        /// <param name="bSignal"></param>
        void SetInitialStatus(bool bSignal);
        /// <summary>
        /// 드라이런 진행전 모든 신호 초기화 기능
        /// </summary>
        void DryRunSignalClear();
        /// <summary>
        /// 실제 신호 장치와 연결
        /// </summary>
        void AttachRealSignalDevice();
        /// <summary>
        /// 가상 신호 장치와 연결 (언링크 드라이런 용)
        /// </summary>
        void AttachVirtualSignalDevice();
        /// <summary>
        /// 타설비에서 선택한 위치 정보를 가져옴
        /// </summary>
        /// <returns>타설비에서 선택한 위치 정보</returns>
        EPosition GetOtherEquipmentSelectPosition();
    }
}
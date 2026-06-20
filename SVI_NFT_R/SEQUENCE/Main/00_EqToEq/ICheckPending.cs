namespace EqToEq
{
    public interface ICheckPending
    {
        /// <summary>
        /// 타설비에 신호 클리어 타임아웃 여부
        /// </summary>
        /// <returns>true=타임아웃 발생, false=정상</returns>
        bool IsOtherEquipmentSingalClearTimeout();
        /// <summary>
        /// 타설비로 인한 인터페이스 지연 타임아웃 여부
        /// </summary>
        /// <returns>true=타임아웃 발생, false=정상</returns>
        bool IsOtherEquipmentPendingTimeout();
    }
}

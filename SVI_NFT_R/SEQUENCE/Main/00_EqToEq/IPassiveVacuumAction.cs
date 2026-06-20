using System;

namespace EqToEq
{
    public interface IPassiveVacuumAction
    {
        /// <summary>
        /// Active설비에서 Vacuum 요청을 받았을 때 발생하는 이벤트 (이벤트 핸들러에 실제 Vacuum과 연결하는 로직이 구현돼야함)
        /// </summary>
        event EventHandler<ReceiveVacuumEventArgs> ReceiveActiveVacuumRequest;
    }
}

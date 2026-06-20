using System;

namespace Utils
{
    /// <summary>
    /// 셀 배출 사이클에 사용할 유닛을 나타내는 인터페이스
    /// </summary>
    /// <![CDATA[
    /// - ICellOutputUnit은 설비에 하나만 존재해야함
    /// - 설비 내에서 가장 느린 유닛을 선정함
    /// ]]>
    public interface ICellOutputUnit
    {
        /// <summary>
        /// 셀 배출 이벤트
        /// </summary>
        event EventHandler<CellOutputEventArgs> CellOutput;
    }
}

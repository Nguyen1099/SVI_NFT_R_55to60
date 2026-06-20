namespace SVI_NFT_R
{
    public interface IUnitNode
    {
        /// <summary>
        /// 해당 유닛에 동작이 모두 끝났는지 여부
        /// </summary>
        bool IsIdle { get; }
        /// <summary>
        /// 해당 유닛에 셀 데이터가 없는지 여부
        /// </summary>
        bool IsEmpty { get; }
    }
}
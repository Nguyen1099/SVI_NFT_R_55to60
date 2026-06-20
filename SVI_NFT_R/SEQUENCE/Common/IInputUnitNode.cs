namespace SVI_NFT_R
{
    public interface IInputUnitNode : IUnitNode
    {
        /// <summary>
        /// 해당 유닛에 물류 투입 가능 여부
        /// </summary>
        bool CanInput { get; }

        /// <summary>
        /// 물류 투입 중지
        /// </summary>
        void StopInput();

        /// <summary>
        /// 물류 투입 재개
        /// </summary>
        void ResumeInput();
    }
}
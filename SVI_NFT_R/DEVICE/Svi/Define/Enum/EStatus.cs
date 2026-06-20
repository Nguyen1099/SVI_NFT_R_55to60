namespace SVI_NFT_R.DEVICE.Svi
{
    /// <summary>
    /// 검사기 상태
    /// </summary>
    public enum EStatus
    {
        /// <summary>
        /// 정상 상태. 검사 진행 가능
        /// </summary>
        Check,
        /// <summary>
        /// 작업중 상태. 검사 진행 불가능
        /// </summary>
        Work,
        /// <summary>
        /// 초기화중 상태. 검사 진행 불가능
        /// </summary>
        Initialize,
        /// <summary>
        /// 대기 상태. 검사 진행 가능
        /// </summary>
        Idle,
        /// <summary>
        /// 에러 상태. 검사 진행 불가능
        /// </summary>
        Error
    }

}

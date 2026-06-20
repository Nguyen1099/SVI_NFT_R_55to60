namespace SVI_NFT_R.CIM
{
    /// <summary>
    /// CCODE
    /// </summary>
    internal sealed class CommandCode
    {
        /// <summary>
        /// Non-Action
        /// </summary>
        public const int DEFAULT = 0;
        /// <summary>
        /// Recipe / Parameter 생성
        /// </summary>
        public const int CREATE = 1;
        /// <summary>
        /// Recipe / Parameter 삭제
        /// </summary>
        public const int DELETE = 2;
        /// <summary>
        /// Recipe / Parameter 수정
        /// </summary>
        public const int MODIFY = 3;
    }
}

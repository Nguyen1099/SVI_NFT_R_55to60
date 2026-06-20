namespace SVI_NFT_R.CIM
{
    /// <summary>
    /// HCACK
    /// </summary>
    internal sealed class HostCommandAcknowledge
    {
        /// <summary>
        /// 0 = ACCEPTED (승인)
        /// </summary>
        public const int ACCEPTED = 0;
        /// <summary>
        /// 1 = CELLID 부정확하여 수행 할 수 없다.
        /// </summary>
        public const int CELLISINVALID = 1;
        /// <summary>
        /// 2 = 설비에 정의 된 명령이 존재하지 않아 수행할 수 없다.
        /// </summary>
        public const int COMMANDDOESNOTEXIST = 2;
        /// <summary>
        /// 3 = 동작 조건이 맞지 않아 수행 할 수 없다.
        /// </summary>
        public const int REJECT_ALREADYINDESIRECONDITION = 3;
        /// <summary>
        /// 4 = 설비의 특정 ERROR로 수행 할 수 없다.
        /// </summary>
        public const int OTHERERRORS = 4;

        // (n > 4) = Reserved
    }
}

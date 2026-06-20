namespace SVI_NFT_R.CIM
{
    /// <summary>
    /// ACK7
    /// </summary>
    internal sealed class Acknowledge7
    {
        /// <summary>
        /// 0 = ACCEPTED (승인)
        /// </summary>
        public const int ACCEPTED = 0;
        /// <summary>
        /// 1 = Permission not granted (권한이 부여되지 않았습니다.)
        /// </summary>
        public const int PERMISSION_NOT_GRANTED = 1;
        /// <summary>
        /// 2 = If there is a parameter abnormality (Parameter 이상 {Name 누락, 중복 및 Value 누락, Spec Over 등} 있을 경우에는 생성, 수정 할 수 없다.)
        /// </summary>
        public const int IF_THERE_IS_A_PARAMETER_ABNORMALITY = 2;
        /// <summary>
        /// 3 = Current PPID cannot be deleted (Current PPID는 삭제할 수 없다.)
        /// </summary>
        public const int CURRENT_PPID_CANNOT_BE_DELETED = 3;
        /// <summary>
        /// 4 = Only parameters of the current PPID can be modified (Current PPID가 아닌 PPID에 대한 Parameter 값은 수정할 수 없다.)
        /// </summary>
        public const int ONLY_PARAMETERS_OF_THE_CURRENT_PPID_CAN_BE_MODIFIED = 4;
        /// <summary>
        /// 5 = The equipment can only be performed in a Pause state (설비가 가동 중인{AutoRun} 상태이기 때문에 수행할 수 없다.)
        /// </summary>
        public const int THE_EQUIPMENT_CAN_ONLY_BE_PERFORMED_IN_A_PAUSE_STATE = 5;
        /// <summary>
        /// 6 = If it does not exist or there is no PPID value (존재하지 않거나 PPID 값이 없는 경우는 생성, 삭제, 수정 할 수 없다.)
        /// </summary>
        public const int IF_IT_DOES_NOT_EXIST_OR_THERE_IS_NO_PPID_VALUE = 6;
        /// <summary>
        /// 7 = EQPID is not exist (비에 해당 EQPID가 존재하지 않아 수행할 수 없다.)
        /// </summary>
        public const int EQPID_IS_NOT_EXIST = 7;
        /// <summary>
        /// 8 = PPID already exists (ModuleID가 존재하지 않습니다.생성하고자 하는 PPID가 설비에 이미 존재하면 생성할 수 없다.)
        /// </summary>
        public const int PPID_ALREADY_EXISTS = 8;
        /// <summary>
        /// 9 = PPID_TYPE or CCODE is not match (PPID_TYPE이 맞지 않거나, CCODE 누락 및 이상값(1, 2, 3이 아닌 경우)은 수행할 수 없다.)
        /// </summary>
        public const int PPID_TYPE_OR_CCODE_IS_NOT_MATCH = 9;
    }
}

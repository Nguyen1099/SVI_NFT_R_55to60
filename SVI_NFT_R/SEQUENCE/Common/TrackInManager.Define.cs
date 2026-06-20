using SVI_NFT_R;

public static partial class TrackInManager
{
    private static class AlarmDefine
    {
        public static readonly CAlarmDefine.EAlarmList[] CellInfoDownloadTimeoutAlarms = new CAlarmDefine.EAlarmList[] { CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P1_CELL_INFORMATION_DOWNLOAD_TIMEOUT, CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P2_CELL_INFORMATION_DOWNLOAD_TIMEOUT };
        public static readonly CAlarmDefine.EAlarmList[] CellInfoDownloadFailedAlarms = new CAlarmDefine.EAlarmList[] { CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P1_CELL_INFORMATION_DOWNLOAD_FAILED, CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P2_CELL_INFORMATION_DOWNLOAD_FAILED };
        public static readonly CAlarmDefine.EAlarmList[] JobProcessRequestTimeoutAlarms = new CAlarmDefine.EAlarmList[] { CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P1_TRACKING_TIMEOUT, CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P2_TRACKING_TIMEOUT };
        public static readonly CAlarmDefine.EAlarmList[] JobProcessRequestFailedAlarms = new CAlarmDefine.EAlarmList[] { CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P1_TRACKING_FAILED, CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P2_TRACKING_FAILED };
        public static readonly CAlarmDefine.EAlarmList[] JobProcessRequestMissmatchAlarms = new CAlarmDefine.EAlarmList[] { CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P1_TRACKING_MISSMATCH, CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P2_TRACKING_MISSMATCH };
        public static readonly CAlarmDefine.EAlarmList[] CellLotInformationTimeoutAlarms = new CAlarmDefine.EAlarmList[] { CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P1_TRACKING_TIMEOUT, CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P2_TRACKING_TIMEOUT };
        public static readonly CAlarmDefine.EAlarmList[] CellLotInformationFailedAlarms = new CAlarmDefine.EAlarmList[] { CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P1_TRACKING_FAILED, CAlarmDefine.EAlarmList.VA_TRANSFER_CIM_P2_TRACKING_FAILED };

    }

    private enum EProcessing
    {
        Serial = 0,
    }
}

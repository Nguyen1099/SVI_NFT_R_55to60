namespace SVI_NFT_R.DEVICE.Nachi
{
    public enum EJcmCommand : int
    {
        None = 0,

        // 정위치
        T1,
        T2,
        T3,
        T4,
        T12,
        T34,

        // 교차
        Crossing_T1_S2,
        Crossing_T2_S1,
        Crossing_T3_S4,
        Crossing_T4_S3,
        Crossing_T12_S34,
        Crossing_T34_S12,
    }
}

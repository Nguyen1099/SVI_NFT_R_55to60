namespace SVI_NFT_R.UI
{
    /// <summary>
    /// UI 모터 응답 데이터
    /// </summary>
    public class OffsetData
    {
        /// <summary>
        /// X 옵셋
        /// </summary>
        public OffsetItem X { get; set; }
        /// <summary>
        /// Y 옵셋
        /// </summary>
        public OffsetItem Y { get; set; }

        public OffsetData()
        {
            X = OffsetItem.Skip();
            Y = OffsetItem.Skip();
        }
    }

    /// <summary>
    /// 옵셋 데이터
    /// </summary>
    public class OffsetItem
    {
        /// <summary>
        /// 옵셋 데이터의 종류
        /// </summary>
        public EOffsetItem Type { get; private set; }
        /// <summary>
        /// 옵셋값
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// 스테틱 함수로 생성 할 수 있도록 유도하기 위해 선언만 해놓음
        /// </summary>
        private OffsetItem()
        {
        }

        /// <summary>
        /// 스킵 데이터 인스턴스를 생성합니다.
        /// </summary>
        /// <returns></returns>
        public static OffsetItem Skip()
        {
            return new OffsetItem()
            {
                Type = EOffsetItem.Skip
            };
        }

        /// <summary>
        /// 비율 옵셋 데이터 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="value">옵셋값 (0~1)</param>
        /// <returns>옵셋 데이터</returns>
        public static OffsetItem Ratio(float value)
        {
            if (0f > value) value = 0f;
            else if (1f < value) value = 1f;
            return new OffsetItem()
            {
                Type = EOffsetItem.Ratio,
                Value = value
            };
        }

        /// <summary>
        /// 픽셀 옵셋 데이터 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="value">픽셀 옵셋값</param>
        /// <returns>옵셋 데이터</returns>
        public static OffsetItem Offset(float value)
        {
            if (0f > value) value = 0f;
            return new OffsetItem()
            {
                Type = EOffsetItem.Offset,
                Value = value
            };
        }

        /// <summary>
        /// 반대편을 기준으로 한 픽셀 옵셋 데이터 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="value">픽셀 옵셋값</param>
        /// <returns>옵셋 데이터</returns>
        public static OffsetItem ReverseOffset(float value)
        {
            if (0f > value) value = 0f;
            return new OffsetItem()
            {
                Type = EOffsetItem.ReverseOffset,
                Value = value
            };
        }
    }

    /// <summary>
    /// 옵셋 데이터 종류
    /// </summary>
    public enum EOffsetItem
    {
        /// <summary>
        /// 비율 ( 0 ~ 1 )
        /// </summary>
        Ratio,
        /// <summary>
        /// 시작 위치 기준 픽셀 옵셋
        /// </summary>
        Offset,
        /// <summary>
        /// 끝 위치 기준 픽셀 옵셋
        /// </summary>
        ReverseOffset,
        /// <summary>
        /// 스킵
        /// </summary>
        Skip
    };

}

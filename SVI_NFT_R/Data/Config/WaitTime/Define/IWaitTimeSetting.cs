using System;

namespace SVI_NFT_R
{
    /// <summary>
    /// 대기시간을 설정 할 때 인터페이스
    /// </summary>
    public interface IWaitTimeSetting
    {
        /// <summary>
        /// 대기시간 인덱스
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 대기시간 카테고리
        /// </summary>
        string Category { get; }
        /// <summary>
        /// 대기시간 단위
        /// </summary>
        ETimeUnit Unit { get; }
        /// <summary>
        /// 대기시간 기본값 (Ini에 값이 없을 때 사용됨)
        /// </summary>
        TimeSpan DefaultValue { get; }
        /// <summary>
        /// 셋팅용으로 유닛 기준으로 변경된 값
        /// </summary>
        double SettingFromUnit { get; set; }

        /// <summary>
        /// INI에 저장
        /// </summary>
        void Save();

        /// <summary>
        /// INI에서 불러오기
        /// </summary>
        void Load();

        /// <summary>
        /// 내부 설정 버퍼를 현재 값으로 초기화함
        /// </summary>
        void ResetSettingValue();

        /// <summary>
        /// 설정 버퍼에 값과 단위를 조합하여 반환함
        /// </summary>
        /// <returns>"{unitValue:0.000} ( [ms|sec|min|hour|day] )"</returns>
        string ToString();
    }
}
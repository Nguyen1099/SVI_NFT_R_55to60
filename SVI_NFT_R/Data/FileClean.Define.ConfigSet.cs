using System;

namespace FileClean.Define
{
    /// <summary>
    /// 설정
    /// </summary>
    [Serializable]
    public sealed class ConfigSet
    {
        /// <summary>
        /// 사용 여부
        /// </summary>
        public bool IsUse { get; set; } = true;
        /// <summary>
        /// 감시 폴더 경로
        /// </summary>
        public string ScanPath { get; set; } = @"D:\Default\";
        /// <summary>
        /// 감시 실행 간격
        /// </summary>
        public int ScanHours { get; set; } = 1;
        /// <summary>
        /// 파일 및 폴더 보관 기간
        /// </summary>
        public int KeepDays { get; set; } = 1;
    }
}

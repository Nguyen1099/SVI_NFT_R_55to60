using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SVI_NFT_R.Config
{
    /// <![CDATA[
    /// [WaitTime 수정 방법]
    /// 1. 'CategoryAttribute'가 정의된 클래스에 Property를 [추가|제거|수정] 한다.
    ///     : ./Data/Config/WaitTime/Define/WaitTime.CommTimeout.cs
    ///     : ./Data/Config/WaitTime/Define/WaitTIme.Equipment.cs
    ///     : ./Data/Config/WaitTime/Define/WaitTime.Etc.cs
    /// 2. 다국어 DB 내에 'CFormConfigWaitTime'에 수정된 내용을 반영한다.
    ///     ; ./Database/RECORD/RECORD_INFORMATION_UI_TEXT.txt
    /// ]]>>

    public static partial class WaitTime
    {
        /// <summary>
        /// 초기화 여부
        /// </summary>
        public static bool IsInitialized { get; private set; } = false;
        /// <summary>
        /// 셋팅용 UI에 사용되는 모든 대기시간에 설정 객체 맵
        /// </summary>
        public static IReadOnlyDictionary<string, IWaitTimeSetting> Settings => mSettingMapping;
        private static Dictionary<string, IWaitTimeSetting> mSettingMapping;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns>true=성공, false=실패</returns>
        public static bool Initialize()
        {
            if (IsInitialized == true)
            {
                return false;
            }

            // Reflection을 활용해서 'CategoryAttribute'가 정의된 클래스를 기준으로 셋팅 UI용 맵 데이터 생성함
            mSettingMapping = AppDomain.CurrentDomain.GetAssemblies()
                // 현재 어플리케이션에 모든 타입을 나열함
                .SelectMany(i => i.GetTypes())
                // 'CategoryAttribute'를 정의한 클래스만 골라냄
                .Where(i => i.GetCustomAttribute<CategoryAttribute>() != null)
                // 모든 Property를 나열함
                .SelectMany(i => i.GetProperties())
                // 'IWaitTime' 타입의 Property만 골라냄
                .Where(i => i.PropertyType == typeof(IReadOnlyWaitTime) && i.GetCustomAttribute<IgnoreSettingAttribute>() == null)
                // Key: Property 이름, Value: IWaitTimeSetting인 Dictionary 생성
                .ToDictionary(i => i.Name, i => (IWaitTimeSetting)i.GetValue(null));

            IsInitialized = true;
            return true;
        }
    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SVI_NFT_R.Config
{
    public static partial class WaitTime
    {
        /// <summary>
        /// 인터페이스 구현부
        /// </summary>
        private sealed class ImplWaitTime : IReadOnlyWaitTime, IWaitTimeSetting
        {
            public string Name { get; private set; } = string.Empty;
            public string Category { get; private set; } = string.Empty;
            public ETimeUnit Unit { get; private set; } = 0;
            public TimeSpan DefaultValue => mDefaultValue;
            public TimeSpan Value => mValue;

            public double SettingFromUnit
            {
                get
                {
                    // Convert: TimeSpan -> Unit
                    return convertTimeSpanToUnit(mSettingValue);
                }
                set
                {
                    // Convert: Unit -> TimeSpan
                    mSettingValue = convertUnitToTimeSpan(value);
                }
            }

            private readonly TimeSpan mDefaultValue = TimeSpan.Zero;
            private TimeSpan mValue = TimeSpan.Zero;
            private TimeSpan mSettingValue = TimeSpan.Zero;

            /// <summary>
            /// 생성자
            /// </summary>
            /// <param name="unit">WaitTime 설정 UI에 표시할 시간 단위</param>
            /// <param name="defaultValue">값이 없거나 형식이 안맞을 때 사용할 기본값</param>
            /// <param name="callerName"></param>
            public ImplWaitTime(ETimeUnit unit, TimeSpan defaultValue, [CallerMemberName] string callerName = "")
            {
                // 호출한 클래스에 'CategoryAttribute'에 설정된 값을 기준으로 Category를 설정함
                // Exception: 'CategoryAttribute'가 정의되지 않은 클래스에서 생성하면 NullReferenceException이 발생함
                Category = new StackTrace()
                    .GetFrame(1)
                    .GetMethod().ReflectedType
                    .GetCustomAttribute<CategoryAttribute>().Name;
                // 호출한 Property 이름으로 Name을 설정함
                Name = callerName;
                Unit = unit;
                mDefaultValue = defaultValue;

                // 초기값 INI에서 불러오기
                Load();

                // 초기값 INI에 쓰기
                Save();
            }

            public void Save()
            {
                mValue = mSettingValue;

                ClassINI objINI = new ClassINI(getIniPath());
                objINI.WriteValue($"{nameof(WaitTime)}", $"{Category}>{Name}", mValue.ToString());
            }

            public void Load()
            {
                ClassINI objINI = new ClassINI(getIniPath());
                if (TimeSpan.TryParse(objINI.GetString($"{nameof(WaitTime)}", $"{Category}>{Name}", mDefaultValue.ToString()), out mValue) == false)
                {
                    mValue = mDefaultValue;
                }

                mSettingValue = mValue;
            }

            public void ResetSettingValue()
            {
                mSettingValue = mValue;
            }

            public override string ToString()
            {
                double unitValue = convertTimeSpanToUnit(mSettingValue);

                switch (Unit)
                {
                    case ETimeUnit.Millisecond:
                        return $"{unitValue:0.000} ( ms )";

                    case ETimeUnit.Second:
                        return $"{unitValue:0.000} ( sec )";

                    case ETimeUnit.Minute:
                        return $"{unitValue:0.000} ( min )";

                    case ETimeUnit.Hour:
                        return $"{unitValue:0.000} ( hour )";

                    case ETimeUnit.Day:
                        return $"{unitValue:0.000} ( day )";

                    default:
                        return "-";
                }
            }

            private string getIniPath()
            {
                return Path.Combine(Directory.GetCurrentDirectory(), CDefine.DEF_DEVICE_INI);
            }

            private double convertTimeSpanToUnit(TimeSpan input)
            {
                switch (Unit)
                {
                    case ETimeUnit.Millisecond:
                        return input.TotalMilliseconds;

                    case ETimeUnit.Second:
                        return input.TotalSeconds;

                    case ETimeUnit.Minute:
                        return input.TotalMinutes;

                    case ETimeUnit.Hour:
                        return input.TotalHours;

                    case ETimeUnit.Day:
                        return input.TotalDays;

                    default:
                        return 0d;
                }
            }

            private TimeSpan convertUnitToTimeSpan(double input)
            {
                switch (Unit)
                {
                    case ETimeUnit.Millisecond:
                        return TimeSpan.FromMilliseconds(input);

                    case ETimeUnit.Second:
                        return TimeSpan.FromSeconds(input);

                    case ETimeUnit.Minute:
                        return TimeSpan.FromMinutes(input);

                    case ETimeUnit.Hour:
                        return TimeSpan.FromHours(input);

                    case ETimeUnit.Day:
                        return TimeSpan.FromDays(input);

                    default:
                        return TimeSpan.Zero;
                }
            }
        }
    }
}
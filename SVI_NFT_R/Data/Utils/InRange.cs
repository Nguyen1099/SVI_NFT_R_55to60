using System;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        /// <summary>
        /// 입력값이 입력한 범위 내에 있는지 확인하고, 범위를 초과했으면 최대값이나 최소값을 넣어주는 함수
        /// </summary>
        /// <typeparam name="T">비교할 숫자 타입</typeparam>
        /// <param name="input">입력값 (범위 초과시 최대값이나 최소값으로 변경되어 반환함)</param>
        /// <param name="min">최소값</param>
        /// <param name="max">최대값</param>
        /// <returns>입력값이 범위 내에 있으면 true, 아니면 false</returns>
        public static bool InRange<T>(ref T input, T min, T max)
            where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            bool bInRange = true;
            if (input.CompareTo(min) < 0)
            {
                input = min;
                bInRange = false;
            }
            else if (input.CompareTo(max) > 0)
            {
                input = max;
                bInRange = false;
            }
            return bInRange;
        }
    }

}

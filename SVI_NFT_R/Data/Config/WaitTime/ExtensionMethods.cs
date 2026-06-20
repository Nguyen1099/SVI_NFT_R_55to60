using System;
using System.Threading;

namespace SVI_NFT_R
{
    public static partial class ExtensionMethods
    {
        public static void Sleep(this IReadOnlyWaitTime waitTime)
        {
            Thread.Sleep(waitTime.ToMilliseconds());
        }

        public static bool SpinUntil(this IReadOnlyWaitTime waitTime, Func<bool> condition, int sleep = 1)
        {
            DateTime endTime = DateTime.Now + waitTime.ToTimeSpan();
            while (true)
            {
                if (condition.Invoke() == true)
                {
                    break;
                }
                if (endTime < DateTime.Now)
                {
                    return false;
                }
                Thread.Sleep(sleep);
            }
            return true;
        }

        public static int ToMilliseconds(this IReadOnlyWaitTime waitTime)
        {
            return Convert.ToInt32(waitTime.Value.TotalMilliseconds);
        }

        public static TimeSpan ToTimeSpan(this IReadOnlyWaitTime waitTime)
        {
            return waitTime.Value;
        }
    }
}

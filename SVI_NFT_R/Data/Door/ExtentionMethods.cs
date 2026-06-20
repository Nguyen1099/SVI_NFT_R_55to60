using SVI_NFT_R.Door;
using System.Collections.Generic;
using System.Linq;

namespace SVI_NFT_R
{
    public static partial class ExtentionMethods
    {
        public static bool IsUnlockPermit(this IReadOnlyList<IDoor> items)
        {
            return items.All(item => item.IsUnlockPermit);
        }

        public static void SetUnlockPermit(this IReadOnlyList<IDoor> items, bool bPermit)
        {
            foreach (var item in items)
            {
                item.IsUnlockPermit = bPermit;
            }
        }
    }
}

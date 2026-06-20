using System;

namespace SVI_NFT_R.Config
{
    public static partial class WaitTime
    {
        [AttributeUsage(AttributeTargets.Class)]
        private sealed class CategoryAttribute : Attribute
        {
            public string Name { get; set; }
        }
    }
}

using System.Collections.Generic;

namespace SVI_NFT_R.UI
{
    public class SwapComponentPosition
    {
        public readonly List<SwapPair> SwapPairCollection = new List<SwapPair>();

        public void SetSwap(ESwapOrigin target)
        {
            foreach (var pair in SwapPairCollection)
            {
                pair.SetSwap(target);
            }
        }

        public void SetOrigin(ESwapOrigin target)
        {
            foreach (var pair in SwapPairCollection)
            {
                pair.SetOrigin(target);
            }
        }
    }
}

using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    [Serializable]
    public class AlignOffset
    {
        public int X { get; set; }
        public int T { get; set; }

        public AlignOffset()
        {
            X = 0;
            T = 0;
        }
    }
}

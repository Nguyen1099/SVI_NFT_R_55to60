using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    [Serializable]
    public class DefectPoint
    {
        public string DefectName { get; set; }
        public uint LocationX { get; set; }
        public uint LocationY { get; set; }

        public DefectPoint()
        {
            DefectName = string.Empty;
            LocationX = 0;
            LocationY = 0;
        }
    }
}

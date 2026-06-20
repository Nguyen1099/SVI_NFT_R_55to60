using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    [Serializable]
    public partial class LinkedInspection
    {
        public string CellID { get; set; }
        public int Count { get { return (null == Name) ? 0 : Name.Length; } }
        public string[] Name { get; set; }
        public string[] XY { get; set; }
        public string[] ImageXY { get; set; }
        public string[] CircleDraw { get; set; }
        public string[] DefectGroup { get; set; }
        public string[] PatternName { get; set; }

        public LinkedInspection()
        {
            CellID = HLDevice.CDeviceSviInterface.NONE;
            Name = new string[0];
            XY = new string[0];
            ImageXY = new string[0];
            CircleDraw = new string[0];
            DefectGroup = new string[0];
            PatternName = new string[0];
        }
    }
}

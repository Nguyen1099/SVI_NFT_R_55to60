using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public partial class ReceiveData
    {
        [Serializable]
        public class AlignCompleteData : KeepData
        {
            public string P1ModelID { get; set; } = string.Empty;
            public string P1InnerID { get; set; } = DeviceIF.NOCELL;
            public string P1CellID { get; set; } = DeviceIF.NOCELL;
            public string P2ModelID { get; set; } = string.Empty;
            public string P2InnerID { get; set; } = DeviceIF.NOCELL;
            public string P2CellID { get; set; } = DeviceIF.NOCELL;
        }
    }
}
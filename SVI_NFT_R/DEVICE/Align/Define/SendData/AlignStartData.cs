using System;
using System.Text;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public partial class SendData
    {
        [Serializable]
        public class AlignStartData : KeepData
        {
            public string P1ModelID { get; set; } = string.Empty;
            public string P1InnerID { get; set; } = DeviceIF.NOCELL;
            public string P1CellID { get; set; } = DeviceIF.NOCELL;
            public string P2ModelID { get; set; } = string.Empty;
            public string P2InnerID { get; set; } = DeviceIF.NOCELL;
            public string P2CellID { get; set; } = DeviceIF.NOCELL;

            /// <summary>
            /// {ModelID1: (string)}.{InnerID1: (string)}.{CellID1: (string)}.{ModelID2: (string)}.{InnerID2: (string)}.{CellID2: (string)}
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder(1024);
                return sb
                    .Append(P1ModelID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(P1InnerID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(P1CellID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(P2ModelID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(P2InnerID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(P2CellID)
                    .ToString();
            }
        }
    }
}
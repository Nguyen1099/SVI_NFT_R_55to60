using System.Text;
using DeviceIF = HLDevice.CDeviceSviInterface;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class SendData
    {
        public class AmoGrabEndData
        {
            public CellInformation PositionA { get; set; }
            public CellInformation PositionB { get; set; }

            public AmoGrabEndData()
            {
                PositionA = new CellInformation()
                {
                    CellID = DeviceIF.NOCELL,
                    InnerID = DeviceIF.NOCELL
                };
                PositionB = new CellInformation()
                {
                    CellID = DeviceIF.NOCELL,
                    InnerID = DeviceIF.NOCELL
                };
            }

            /// <summary>
            /// CellID1.InnerID1.CellID2.InnerID2
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(PositionA.CellID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(PositionA.InnerID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(PositionB.CellID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(PositionB.InnerID)
                    ;
                return sb.ToString();
            }
        }
    }
}

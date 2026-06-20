using System.Text;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class SendData
    {
        public class LinkedInspectionData
        {
            public LinkedInspection PositionA { get; set; }
            public LinkedInspection PositionB { get; set; }

            public LinkedInspectionData()
            {
                PositionA = new LinkedInspection();
                PositionB = new LinkedInspection();
            }

            /// <summary>
            /// [CellIDA].[DataCountA].NAME1.XY1.IMG_XY1...NAMEn.XYn.IMG_XYn.[CellIDB].[DataCountB].NAME1.XY1.IMG_XY1...NAMEn.XYn.IMG_XYn
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(PositionA.CellID)
                    .Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                    .Append(PositionA.Count);
                for (int i = 0; i < PositionA.Count; i++)
                {
                    sb.Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                        .Append(PositionA.Name[i])
                        .Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                        .Append(PositionA.XY[i])
                        .Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                        .Append(PositionA.ImageXY[i]);
                }

                sb.Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                    .Append(PositionB.CellID)
                    .Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                    .Append(PositionB.Count);
                for (int i = 0; i < PositionB.Count; i++)
                {
                    sb.Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                        .Append(PositionB.Name[i])
                        .Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                        .Append(PositionB.XY[i])
                        .Append(HLDevice.CDeviceSviInterface.SEPARATOR)
                        .Append(PositionB.ImageXY[i]);
                }

                return sb.ToString();
            }
        }
    }
}

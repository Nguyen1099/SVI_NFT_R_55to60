using System.Text;
using DeviceIF = HLDevice.CDeviceSviInterface;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class SendData
    {
        public class CellDataSyncData
        {
            public string CellID { get; set; } = DeviceIF.NOCELL;
            public string InnerID { get; set; } = DeviceIF.NOCELL;
            public string JobID { get; set; } = DeviceIF.NOCELL;

            /// <summary>
            /// CellID.InnerID.JobID
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(CellID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(InnerID)
                    .Append(DeviceIF.SEPARATOR)
                    .Append(JobID)
                    ;
                return sb.ToString();
            }
        }
    }
}

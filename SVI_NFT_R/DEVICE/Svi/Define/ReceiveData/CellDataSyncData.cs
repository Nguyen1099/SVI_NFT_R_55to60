using System;
using DeviceIF = HLDevice.CDeviceSviInterface;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class CellDataSyncData : KeepData
        {
            public string CellID { get; set; } = DeviceIF.NOCELL;
            public string InnerID { get; set; } = DeviceIF.NOCELL;
            public string JobID { get; set; } = DeviceIF.NOCELL;

            public CellDataSyncData()
            {
                ResetRegistTime();
            }
        }
    }
}

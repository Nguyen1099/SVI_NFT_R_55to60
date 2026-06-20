namespace SVI_NFT_R.DEVICE.Svi
{
    public class CellInformation
    {
        public string InnerID { get; set; }
        public string CellID { get; set; }

        public CellInformation()
        {
            InnerID = HLDevice.CDeviceSviInterface.NOCELL;
            CellID = HLDevice.CDeviceSviInterface.NOCELL;
        }
    }
}

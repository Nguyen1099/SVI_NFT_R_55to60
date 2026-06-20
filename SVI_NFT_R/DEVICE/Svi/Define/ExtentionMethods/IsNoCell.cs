using DeviceIF = HLDevice.CDeviceSviInterface;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 해당 셀 정보가 NOCELL인지 확인 합니다.
        /// </summary>
        /// <param name="cellInformation"></param>
        /// <returns></returns>
        public static bool IsNoCell(this CellInformation cellInformation)
        {
            bool bResult = true;

            do
            {
                if (cellInformation.InnerID == DeviceIF.NOCELL
                    || cellInformation.CellID == DeviceIF.NOCELL
                    )
                {
                    break;
                }

                bResult = false;
            } while (false);

            return bResult;
        }
    }
}

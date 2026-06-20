using System.Collections.Generic;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static List<string> GetExistInnerIds(this ReceiveData.AlignCompleteData data)
        {
            List<string> result = new List<string>();
            if (data.P1InnerID != DeviceIF.NOCELL)
            {
                result.Add(data.P1InnerID);
            }
            if (data.P2InnerID != DeviceIF.NOCELL)
            {
                result.Add(data.P2InnerID);
            }
            return result;
        }
    }
}
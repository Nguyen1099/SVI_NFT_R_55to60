using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseAlignErrorData(this DeviceIF.CReceiveData packet, out string errorIdOrNull)
        {
            // OTM.RECV.ALIGN_ERROR.{ErrorID:string}
            bool bResult = false;
            errorIdOrNull = null;
            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.AlignErrorRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.AlignErrorRequest)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                errorIdOrNull = splitText[0];

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
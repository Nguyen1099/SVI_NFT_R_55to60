using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseCalibrationCompleteData(this DeviceIF.CReceiveData packet, out int index)
        {
            // OTM.SEND.ALIGN_CAL_COMPLETE.{Index: (int)}
            bool bResult = false;
            index = 0;
            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.CalibrationCompleteRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.CalibrationCompleteRequest)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
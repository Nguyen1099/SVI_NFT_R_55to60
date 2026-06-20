using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseCalibrationStartData(this DeviceIF.CReceiveData packet)
        {
            // OTM.SEND.ALIGN_CAL_START
            bool bResult = false;
            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.CalibrationStartRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.CalibrationStartRequest)"));
                    break;
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
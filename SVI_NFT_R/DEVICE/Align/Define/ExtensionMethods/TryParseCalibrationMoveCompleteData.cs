using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseCalibrationMoveCompleteData(this DeviceIF.CReceiveData packet, out int index)
        {
            // OTM.RECV.ALIGN_CAL_MOVE_COMPLETE.{Index:int}
            bool bResult = false;
            index = 0;
            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.CalibrationMoveCompleteAcknowledge)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.CalibrationMoveCompleteAcknowledge)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                if (int.TryParse(splitText[0], out index) == false)
                {
                    RaiseErrorEvent(packet, string.Format("if (int.TryParse(splitText[0], out index) == false)"));
                    break;
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
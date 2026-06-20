using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseCalibrationMoveHomeData(this DeviceIF.CReceiveData packet, out ReceiveData.CalibrationMoveHomeData resultDataOrNull)
        {
            // OTM.SEND.ALIGN_CAL_MOVE_HOME.{Index: (int)}
            bool bResult = false;
            resultDataOrNull = null;
            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.CalibrationMoveHomeRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.CalibrationMoveHomeRequest)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                if (int.TryParse(splitText[0], out int index) == false)
                {
                    RaiseErrorEvent(packet, string.Format("if (int.TryParse(splitText[0], out int index) == false)"));
                    break;
                }

                resultDataOrNull = new ReceiveData.CalibrationMoveHomeData()
                {
                    Index = index
                };
                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
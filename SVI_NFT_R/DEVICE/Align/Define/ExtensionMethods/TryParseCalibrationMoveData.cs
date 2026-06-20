using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseCalibrationMoveData(this DeviceIF.CReceiveData packet, out ReceiveData.CalibrationMoveData resultDataOrNull)
        {
            // OTM.SEND.ALIGN_CAL_MOVE_XYT.{Index: (int)}.{X: (int, μm)}.{Y: (int, μm)}.{T: (int, μº)}
            bool bResult = false;
            resultDataOrNull = null;
            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.CalibrationMoveOffsetRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.CalibrationMoveOffsetRequest)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 4)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 4)"));
                    break;
                }

                if (int.TryParse(splitText[0], out int index) == false
                    || int.TryParse(splitText[1], out int x) == false
                    || int.TryParse(splitText[2], out int y) == false
                    || int.TryParse(splitText[3], out int t) == false
                    )
                {
                    RaiseErrorEvent(packet, string.Format("if (int.TryParse() == false)"));
                    break;
                }

                resultDataOrNull = new ReceiveData.CalibrationMoveData()
                {
                    Index = index,
                    X = x,
                    Y = y,
                    T = t
                };

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
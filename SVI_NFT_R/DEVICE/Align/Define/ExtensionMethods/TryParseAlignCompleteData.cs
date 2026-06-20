using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseAlignCompleteData(this DeviceIF.CReceiveData packet, out ReceiveData.AlignCompleteData resultDataOrNull)
        {
            // OTM.SEND.ALIGN_COMPLETE.{ModelID1: (string)}.{InnerID1: (string)}.{CellID1: (string)}.{ModelID2: (string)}.{InnerID2: (string)}.{CellID2: (string)}
            bool bResult = false;
            resultDataOrNull = null;
            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.AlignCompleteRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.AlignCompleteRequest)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 6)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 6)"));
                    break;
                }

                resultDataOrNull = new ReceiveData.AlignCompleteData()
                {
                    P1ModelID = splitText[0],
                    P1InnerID = splitText[1],
                    P1CellID = splitText[2],
                    P2ModelID = splitText[3],
                    P2InnerID = splitText[4],
                    P2CellID = splitText[5],
                };

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
using System;
using System.Linq;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseModelListResultData(this DeviceIF.CReceiveData packet, out string[] modelNames)
        {
            // OTM.SEND.MODEL_LIST_RESULT.{ModelCount: (int)}.{ModelNames[0]: (string)}. ... {ModelNames[n]: (string)}
            bool bResult = false;
            modelNames = new string[0];

            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.ModelListResultRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.ModelListResultRequest)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                int modelCount;
                if (int.TryParse(splitText[0], out modelCount) == false)
                {
                    RaiseErrorEvent(packet, string.Format("if (int.TryParse(splitText[0], out modelCount) == false)"));
                    break;
                }

                if ((splitText.Length - 1) != modelCount)
                {
                    RaiseErrorEvent(packet, string.Format("if ((splitText.Length - 1) != modelCount)"));
                    break;
                }

                modelNames = splitText
                    .Skip(1)
                    .Take(modelCount)
                    .ToArray();

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
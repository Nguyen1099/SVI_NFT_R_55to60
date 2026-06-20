using System;
using System.Linq;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        public static bool TryParseLightTemperatureResultData(this DeviceIF.CReceiveData packet, out double[] values)
        {
            // OTM.SEND.LIGHT_TEMPERATURE_RESULT.{LightTempCount: (int)}.{LightTempValues[0]: (int)}. … .{LightTempValues[N]: (int)}
            bool bResult = false;
            values = new double[0];

            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.LightTemperatureResultRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.LightTemperatureResultRequest)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                int itemCount;
                if (int.TryParse(splitText[0], out itemCount) == false)
                {
                    RaiseErrorEvent(packet, string.Format("if (int.TryParse(splitText[0], out itemCount) == false)"));
                    break;
                }

                if ((splitText.Length - 1) != itemCount)
                {
                    RaiseErrorEvent(packet, string.Format("if ((splitText.Length - 1) != itemCount)"));
                    break;
                }

                values = new double[itemCount];
                int index = 0;
                foreach (var valueText in splitText.Skip(1).Take(itemCount))
                {
                    double.TryParse(valueText, out values[index]);
                    // ! 단위 변환
                    values[index] /= 10d;
                    ++index;
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
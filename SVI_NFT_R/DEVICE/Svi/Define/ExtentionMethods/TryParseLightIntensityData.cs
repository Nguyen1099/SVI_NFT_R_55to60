using System;
using System.Linq;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// CIM SVID용 외관 검사기 조명 값 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="lightIntensityOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseLightIntensityData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out int[] lightIntensityOrNull)
        {
            // OTM.RECV.LIGHT.VALUE.499.498.499.499.499.499.498.601.600.600.600.601.601.601.0.3.550.551.550.550.551.551.550.469.390.390.390.390.390.391.0.0.900.900.0.0.0.0.0.0.0.0
            bool bResult = false;
            lightIntensityOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.LightIntensityAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.LightIntensityAcknowledge != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                lightIntensityOrNull = splitText.Select(item =>
                {
                    int parseValue;
                    int.TryParse(item, out parseValue);
                    return parseValue;
                }).ToArray();

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

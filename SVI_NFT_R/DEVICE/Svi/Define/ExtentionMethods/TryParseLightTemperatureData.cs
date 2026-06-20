using System;
using System.Linq;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// CIM SVID용 외관 검사기 조명 온도 값 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="lightTemperatureOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseLightTemperatureData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out int[] lightTemperatureOrNull)
        {
            // OTM.RECV.TEMP.VALUE.28.28.0.27.26.0.0.0
            bool bResult = false;
            lightTemperatureOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.LightTemperatureAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.LightTemperatureAcknowledge != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                lightTemperatureOrNull = splitText.Select(item =>
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

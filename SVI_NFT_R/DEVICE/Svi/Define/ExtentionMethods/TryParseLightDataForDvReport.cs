using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// CIM DV 보고용 조명 이름과 밝기 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="lightDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseLightDataForDvReport(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.LightDataForDvReport lightDataOrNull)
        {
            // OTM.SEND.LIGHT.A3VX1S06JL7AC018.A3VX1S06JL7AC018.PARALLEL.1. BACKLIGHT.5.  DIFFTOP.143. DIFFDOWN.53.     SIDE.1.            BRIGHT.109.      INTENSIVE.35.     INTENSIVE2.51.     INTENSIVE3.52.      INTENSIVE4.56
            // 요기요
            bool bResult = false;
            lightDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.LightForDvListRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.LightForDvListRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 2)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 2)"));
                    break;
                }

                lightDataOrNull = new ReceiveData.LightDataForDvReport();
                lightDataOrNull.CellID = splitText[0];
                lightDataOrNull.InnerID = splitText[1];
                int dataCount = (splitText.Length - 2) / 2;
                for (int i = 0; i < dataCount; i++)
                {
                    int startIndex = i * 2 + 2;
                    int value;
                    if (true == int.TryParse(splitText[startIndex + 1], out value))
                    {
                        lightDataOrNull.Data.Add(splitText[startIndex], value);
                    }
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

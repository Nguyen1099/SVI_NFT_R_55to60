using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 얼라인 옵셋 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="alignDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseAlignData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.AlignData alignDataOrNull)
        {
            // OTM.SEND.ALIGN.-999000.-999000.-999000.-999000
            bool bResult = false;
            alignDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.AlignDataRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.AlignDataRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 4)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 4)"));
                    break;
                }
                int x1, t1, x2, t2;
                if (
                    false == int.TryParse(splitText[0], out x1)
                    || false == int.TryParse(splitText[1], out t1)
                    || false == int.TryParse(splitText[2], out x2)
                    || false == int.TryParse(splitText[3], out t2)
                    )
                {
                    RaiseErrorEvent(packet, string.Format("data parse failed"));
                    break;
                }

                alignDataOrNull = new ReceiveData.AlignData()
                {
                    PositionA = new AlignOffset() { X = x1, T = t1 },
                    PositionB = new AlignOffset() { X = x2, T = t2 }
                };
                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

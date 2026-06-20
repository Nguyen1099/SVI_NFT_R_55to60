using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// 외관 검사기에서 수신한 에러 메시지 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="errorMessageOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseAlignResultData(this DeviceIF.CReceiveData packet, out ReceiveData.AlignResultData resultDataOrNull)
        {
            // OTM.SEND.ALIGN_RESULT_XYT.{ModelID: (string)}.{InnerID: (string)}.{CellID: (string)}.{X: (int, μm)}.{Y: (int, μm)}.{T: (int, μº)}.{Score: (int, 1/1000 %)}
            bool bResult = false;
            resultDataOrNull = null;
            do
            {
                if (packet.eReceiveData != DeviceIF.EReceiveDataType.AlignResultRequest)
                {
                    RaiseErrorEvent(packet, string.Format("if (packet.eReceiveData != EReceiveDataType.AlignResultRequest)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { DeviceIF.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 7)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 7)"));
                    break;
                }

                if (int.TryParse(splitText[3], out int x) == false
                    || int.TryParse(splitText[4], out int y) == false
                    || int.TryParse(splitText[5], out int t) == false
                    || int.TryParse(splitText[6], out int score) == false
                    )
                {
                    RaiseErrorEvent(packet, string.Format("if (int.TryParse() == false)"));
                    break;
                }

                resultDataOrNull = new ReceiveData.AlignResultData()
                {
                    ModelID = splitText[0],
                    InnerID = splitText[1],
                    CellID = splitText[2],
                    X = x,
                    Y = y,
                    T = t,
                    Score = score
                };

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}
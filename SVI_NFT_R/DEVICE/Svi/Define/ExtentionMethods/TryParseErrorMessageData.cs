using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 외관 검사기에서 수신한 에러 메시지 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="errorMessageOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseErrorMessageData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out string errorMessageOrNull)
        {
            // OTM.SEND.ERROR.ErrorMessage
            bool bResult = false;
            errorMessageOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.ErrorMessageRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.ErrorMessageRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                errorMessageOrNull = splitText[0];

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

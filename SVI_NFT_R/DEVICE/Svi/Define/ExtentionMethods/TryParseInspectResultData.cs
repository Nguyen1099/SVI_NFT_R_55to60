using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 검사 결과 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="inspectResultDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseInspectResultData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.InspectResultData inspectResultDataOrNull)
        {
            // OTM.SEND.INSPECT.RESULT.A3UH1D9711MAP019.A3UH1D9711MAP019.BIN1
            bool bResult = false;
            inspectResultDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.ResultDataRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.ResultDataRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 3)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 3)"));
                    break;
                }

                inspectResultDataOrNull = new ReceiveData.InspectResultData();
                inspectResultDataOrNull.CellID = splitText[0];
                inspectResultDataOrNull.InnerID = splitText[1];
                inspectResultDataOrNull.Result = splitText[2];

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

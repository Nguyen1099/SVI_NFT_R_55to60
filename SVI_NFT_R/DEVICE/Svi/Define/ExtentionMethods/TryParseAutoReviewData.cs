using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 오토 리뷰 판정 및 좌표 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="autoReviewDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseAutoReviewData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.AutoReviewData autoReviewDataOrNull)
        {
            // OTM.SEND.AUTOREVIEW.A3UH1D9712IAM057.BIN2.4.Scratch-ActiveAll.23631.77290.ActiveAll.27680.2230.Scratch-ActiveAll.23631.77290.ActiveAll.27680.2230
            bool bResult = false;
            autoReviewDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.AutoReviewDataRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.AutoReviewDataRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 3)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 3)"));
                    break;
                }
                int dataCount;
                if (false == int.TryParse(splitText[2], out dataCount))
                {
                    RaiseErrorEvent(packet, string.Format("if (false == int.TryParse(splitText[2], out dataCount))"));
                    break;
                }
                int realDataCount = (splitText.Length - 3) / 3;
                if (dataCount != realDataCount)
                {
                    RaiseErrorEvent(packet, string.Format("if (dataCount != realDataCount)"));
                    break;
                }
                autoReviewDataOrNull = new ReceiveData.AutoReviewData();
                autoReviewDataOrNull.CellID = splitText[0];
                autoReviewDataOrNull.Result = splitText[1];
                autoReviewDataOrNull.DefectPoints = new DefectPoint[dataCount];
                for (int i = 0; i < dataCount; i++)
                {
                    int startIndex = i * 3 + 3;
                    autoReviewDataOrNull.DefectPoints[i] = new DefectPoint();
                    autoReviewDataOrNull.DefectPoints[i].DefectName = splitText[startIndex + 0];
                    uint locationX, locationY;
                    if (
                        true == uint.TryParse(splitText[startIndex + 1], out locationX)
                        && true == uint.TryParse(splitText[startIndex + 2], out locationY)
                        )
                    {
                        autoReviewDataOrNull.DefectPoints[i].LocationX = locationX;
                        autoReviewDataOrNull.DefectPoints[i].LocationY = locationY;
                    }
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

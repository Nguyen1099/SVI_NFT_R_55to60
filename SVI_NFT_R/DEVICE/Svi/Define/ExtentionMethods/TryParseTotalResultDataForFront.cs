using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 종합 검사 결과 (SVI-F용)
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="totalResultDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseTotalResultDataForFront(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.TotalResultDataForFront totalResultDataOrNull)
        {
            // OTM.SEND.TOTALRESULT.A3UH1D9712IAM057.BIN2.Scratch-ActiveAll.ActiveAll.Scratch-ActiveAll.ActiveAll.F2NoData.None
            bool bResult = false;
            totalResultDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.TotalResultDataRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.TotalResultDataRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 2)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 2)"));
                    break;
                }

                totalResultDataOrNull = new ReceiveData.TotalResultDataForFront();
                totalResultDataOrNull.CellID = splitText[0];
                totalResultDataOrNull.Result.Result = splitText[1];
                totalResultDataOrNull.Result.DefectNames = new string[6];
                for (int i = 0; i < totalResultDataOrNull.Result.DefectNames.Length; i++)
                {
                    int startIndex = i + 2;
                    if (splitText.Length > startIndex)
                    {
                        totalResultDataOrNull.Result.DefectNames[i] = splitText[startIndex];
                    }
                    // "OTM.SEND.TOTALRESULT.CellID.BIN2.NoData" Case에대한 예외처리
                    else
                    {
                        totalResultDataOrNull.Result.DefectNames[i] = string.Empty;
                    }
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

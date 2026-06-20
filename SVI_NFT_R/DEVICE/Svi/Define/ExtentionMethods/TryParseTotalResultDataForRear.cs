using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 종합 검사 결과 (SVI-R용)
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="totalResultDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseTotalResultDataForRear(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.TotalResultDataForRear totalResultDataOrNull)
        {
            // OTM.SEND.TOTALRESULT.A3UH1D9712IAM057.A3UH1D9712IAM057.BIN2.Scratch-ActiveAll.ActiveAll.Scratch-ActiveAll.ActiveAll.F2NoData.None.BIN2.Scratch-ActiveAll.ActiveAll.Scratch-ActiveAll.ActiveAll.F2NoData.None
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

                totalResultDataOrNull = new ReceiveData.TotalResultDataForRear();
                totalResultDataOrNull.CellID = splitText[0];
                totalResultDataOrNull.InnerID = splitText[1];
                if (16 == splitText.Length)
                {
                    totalResultDataOrNull.FrontResult.Result = splitText[2];
                    totalResultDataOrNull.FrontResult.DefectNames = new string[6];
                    totalResultDataOrNull.FrontResult.DefectNames[0] = splitText[3];
                    totalResultDataOrNull.FrontResult.DefectNames[1] = splitText[4];
                    totalResultDataOrNull.FrontResult.DefectNames[2] = splitText[5];
                    totalResultDataOrNull.FrontResult.DefectNames[3] = splitText[6];
                    totalResultDataOrNull.FrontResult.DefectNames[4] = splitText[7];
                    totalResultDataOrNull.FrontResult.DefectNames[5] = splitText[8];
                    totalResultDataOrNull.RearResult.Result = splitText[9];
                    totalResultDataOrNull.RearResult.DefectNames = new string[6];
                    totalResultDataOrNull.RearResult.DefectNames[0] = splitText[10];
                    totalResultDataOrNull.RearResult.DefectNames[1] = splitText[11];
                    totalResultDataOrNull.RearResult.DefectNames[2] = splitText[12];
                    totalResultDataOrNull.RearResult.DefectNames[3] = splitText[13];
                    totalResultDataOrNull.RearResult.DefectNames[4] = splitText[14];
                    totalResultDataOrNull.RearResult.DefectNames[5] = splitText[15];
                }
                else
                {
                    totalResultDataOrNull.FrontResult.Result = "BIN2";
                    totalResultDataOrNull.FrontResult.DefectNames = new string[6];
                    totalResultDataOrNull.FrontResult.DefectNames[0] = "NODATA";
                    totalResultDataOrNull.FrontResult.DefectNames[1] = string.Empty;
                    totalResultDataOrNull.FrontResult.DefectNames[2] = string.Empty;
                    totalResultDataOrNull.FrontResult.DefectNames[3] = string.Empty;
                    totalResultDataOrNull.FrontResult.DefectNames[4] = string.Empty;
                    totalResultDataOrNull.FrontResult.DefectNames[5] = string.Empty;
                    totalResultDataOrNull.RearResult.Result = "BIN2";
                    totalResultDataOrNull.RearResult.DefectNames = new string[6];
                    totalResultDataOrNull.RearResult.DefectNames[0] = "NODATA";
                    totalResultDataOrNull.RearResult.DefectNames[1] = string.Empty;
                    totalResultDataOrNull.RearResult.DefectNames[2] = string.Empty;
                    totalResultDataOrNull.RearResult.DefectNames[3] = string.Empty;
                    totalResultDataOrNull.RearResult.DefectNames[4] = string.Empty;
                    totalResultDataOrNull.RearResult.DefectNames[5] = string.Empty;
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

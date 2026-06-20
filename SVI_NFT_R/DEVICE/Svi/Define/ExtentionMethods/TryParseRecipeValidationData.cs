using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// CIM 보고용 외관 검사기의 레시피 정보 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="recipeValidationDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseRecipeValidationData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.RecipeValidationData recipeValidationDataOrNull)
        {
            // OTM.RECV.VALIDATION.4.DECISIONLOGIC.3D8FB71F6F4BE559A7C56E4E22D7F9AF.DL.C14488EE403489CEF9D18549E37B4C5E.RCP.556875825C023871731DF3254EF843A7.RCP_NAME.MF-Foldable-191009
            bool bResult = false;
            recipeValidationDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.RecipeValidationDataRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.RecipeValidationDataRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                int dataCount;
                if (false == int.TryParse(splitText[0], out dataCount))
                {
                    RaiseErrorEvent(packet, string.Format("if (false == int.TryParse(splitText[0], out dataCount))"));
                    break;
                }
                int realDataCount = (splitText.Length - 1) / 2;
                if (realDataCount != dataCount)
                {
                    RaiseErrorEvent(packet, string.Format("if (realDataCount != dataCount)"));
                    break;
                }
                recipeValidationDataOrNull = new ReceiveData.RecipeValidationData();
                for (int i = 0; i < dataCount; i++)
                {
                    int startIndex = i * 2 + 1;
                    recipeValidationDataOrNull.Data.TryAdd(splitText[startIndex], splitText[startIndex + 1]);
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

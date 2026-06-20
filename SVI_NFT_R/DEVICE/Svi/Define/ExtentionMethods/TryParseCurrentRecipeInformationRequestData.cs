using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// CIM PPID용 외관 검사기 레시피 이름 및 조명 설정 값 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="recipeNameOrNull"></param>
        /// <param name="lightIntensityOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseCurrentRecipeInformationRequestData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out string recipeNameOrNull, out int[] lightIntensityOrNull)
        {
            // OTM.RECV.RECIPE.VALUE.RecipeName.LightIntensity1. ... .LightIntensityN
            bool bResult = false;
            recipeNameOrNull = null;
            lightIntensityOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.CurrentRecipeInformationRequestAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.CurrentRecipeInformationRequestAcknowlege != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                recipeNameOrNull = splitText[0];
                int dataCount = splitText.Length - 1;
                lightIntensityOrNull = new int[dataCount];
                for (int i = 0; i < dataCount; i++)
                {
                    int startIndex = i + 1;
                    int parseValue;
                    int.TryParse(splitText[startIndex], out parseValue);
                    lightIntensityOrNull[i] = parseValue;
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

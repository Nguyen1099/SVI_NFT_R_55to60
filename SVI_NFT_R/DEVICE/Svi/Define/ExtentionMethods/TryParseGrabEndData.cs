using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 촬상 종료 응답 데이터 (에어리어 타입)
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="grabEndDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseGrabEndData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.GrabEndData grabEndDataOrNull)
        {
            // OTM.RECV.GRAB.END.A3UH1D9712IAM057.A3UH1D9712IAM057.A3UH1D9711MAP019.A3UH1D9711MAP019.1
            bool bResult = false;
            grabEndDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.GrabEndRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.GrabEndRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 5)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 5)"));
                    break;
                }
                if (int.TryParse(splitText[4], out int selectIndex) == false)
                {
                    RaiseErrorEvent(packet, string.Format("if (int.TryParse(splitText[4], out int selectIndex) == false)"));
                    break;
                }
                grabEndDataOrNull = new ReceiveData.GrabEndData();
                grabEndDataOrNull.PositionA.CellID = splitText[0];
                grabEndDataOrNull.PositionA.InnerID = splitText[1];
                grabEndDataOrNull.PositionB.CellID = splitText[2];
                grabEndDataOrNull.PositionB.InnerID = splitText[3];
                grabEndDataOrNull.SelectIndex = selectIndex;

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 촬상 시작 응답 데이터 (에어리어 타입)
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="grabStartDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseGrabStartData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.GrabStartData grabStartDataOrNull)
        {
            // OTM.RECV.GRAB.START.A3UH1D9712IAM057.A3UH1D9712IAM057.A3UH1D9711MAP019.A3UH1D9711MAP019.0
            bool bResult = false;
            grabStartDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.GrabStartAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.GrabStartAcknowledge != packet.eReceiveData)"));
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
                grabStartDataOrNull = new ReceiveData.GrabStartData();
                grabStartDataOrNull.PositionA.CellID = splitText[0];
                grabStartDataOrNull.PositionA.InnerID = splitText[1];
                grabStartDataOrNull.PositionB.CellID = splitText[2];
                grabStartDataOrNull.PositionB.InnerID = splitText[3];
                grabStartDataOrNull.SelectIndex = selectIndex;

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

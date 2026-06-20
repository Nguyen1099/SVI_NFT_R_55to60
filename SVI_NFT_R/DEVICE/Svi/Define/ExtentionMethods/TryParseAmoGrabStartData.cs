using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// AMO 촬상 시작 응답 데이터 (라인스캔 타입)
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="grabStartDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseAmoGrabStartData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.AmoGrabStartData grabStartDataOrNull)
        {
            // OTM.RECV.AMO_GRAB.START.A3UH1D9712IAM057.A3UH1D9712IAM057.A3UH1D9711MAP019.A3UH1D9711MAP019
            bool bResult = false;
            grabStartDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.AmoGrabStartAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.AmoGrabStartAcknowledge != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 4)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 4)"));
                    break;
                }

                grabStartDataOrNull = new ReceiveData.AmoGrabStartData();
                grabStartDataOrNull.PositionA.CellID = splitText[0];
                grabStartDataOrNull.PositionA.InnerID = splitText[1];
                grabStartDataOrNull.PositionB.CellID = splitText[2];
                grabStartDataOrNull.PositionB.InnerID = splitText[3];

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

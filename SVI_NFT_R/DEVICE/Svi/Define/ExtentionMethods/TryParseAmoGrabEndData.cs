using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 촬상 종료 응답 데이터 (라인스캔 타입)
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="grabEndDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseAmoGrabEndData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.AmoGrabEndData grabEndDataOrNull)
        {
            // OTM.RECV.AMO_GRAB.END.A3UH1D9712IAM057.A3UH1D9712IAM057.A3UH1D9711MAP019.A3UH1D9711MAP019
            bool bResult = false;
            grabEndDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.AmoGrabEndAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.AmoGrabEndAcknowledge != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 4)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 4)"));
                    break;
                }

                grabEndDataOrNull = new ReceiveData.AmoGrabEndData();
                grabEndDataOrNull.PositionA.CellID = splitText[0];
                grabEndDataOrNull.PositionA.InnerID = splitText[1];
                grabEndDataOrNull.PositionB.CellID = splitText[2];
                grabEndDataOrNull.PositionB.InnerID = splitText[3];

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

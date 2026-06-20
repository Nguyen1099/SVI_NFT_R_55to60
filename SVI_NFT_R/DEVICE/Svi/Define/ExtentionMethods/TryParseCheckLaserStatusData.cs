using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 환경 안전용 레이저(평행광) 점등 상태 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="laserStatus"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseCheckLaserStatusData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out EPower laserStatus)
        {
            // OTM.RECV.LASER.CHECK.[ON, OFF]
            bool bResult = false;
            laserStatus = 0;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.LaserCheckAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.LaserCheckAcknowlege != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 1)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 1)"));
                    break;
                }

                switch (splitText[0])
                {
                    case "ON":
                        laserStatus = EPower.On;
                        break;
                    case "OFF":
                        laserStatus = EPower.Off;
                        break;
                    default:
                        RaiseErrorEvent(packet, string.Format("unkown_status={0}", splitText[0]));
                        break;
                }

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

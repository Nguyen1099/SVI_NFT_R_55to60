using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 자바스 에러 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="laserStatus"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseJavasSoftwareErrorData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out EAlarmType alarmType, out string errorMessage)
        {
            // OTM.SWERROR.[HEAVY, LIGHT].MESSAGE.[CAM, LIGHT]
            bool bResult = false;
            alarmType = 0;
            errorMessage = string.Empty;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.JavasSoftwareErrorRequest != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.LaserStatusDataRequest != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 3)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 3)"));
                    break;
                }

                switch (splitText[0])
                {
                    case "HEAVY":
                        alarmType = EAlarmType.Heavy;
                        break;
                    case "LIGHT":
                        alarmType = EAlarmType.Light;
                        break;
                    default:
                        RaiseErrorEvent(packet, string.Format("unkown_status={0}", splitText[0]));
                        break;
                }
                errorMessage = splitText[2];

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

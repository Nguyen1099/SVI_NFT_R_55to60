using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 외관 검사기의 상태 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="status"></param>
        /// <param name="errorMessageOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseCheckStatusData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out EStatus status, out string errorMessageOrNull)
        {
            // OTM.RECV.STATE.[CHECK, WORK, INIT, IDLE, ERROR.Message]
            bool bResult = false;
            status = 0;
            errorMessageOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.StateCheckAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.StateCheckAcknowlege != packet.eReceiveData)"));
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
                    case "CHECK":
                        status = EStatus.Check;
                        break;
                    case "IDLE":
                        status = EStatus.Idle;
                        break;
                    case "INIT":
                        status = EStatus.Initialize;
                        break;
                    case "WORK":
                        status = EStatus.Work;
                        break;
                    case "ERROR":
                        status = EStatus.Error;
                        if (splitText.Length > 1)
                        {
                            errorMessageOrNull = splitText[1];
                        }
                        else
                        {
                            errorMessageOrNull = string.Empty;
                        }
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

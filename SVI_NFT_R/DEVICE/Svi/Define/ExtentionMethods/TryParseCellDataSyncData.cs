using System;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// 셀 데이터 동기화 응답 데이터
        /// </summary>
        /// <param name="packet">수신 받은 패킷</param>
        /// <param name="cellDataSyncDataOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseCellDataSyncData(this HLDevice.CDeviceSviInterface.CReceiveData packet, out ReceiveData.CellDataSyncData cellDataSyncDataOrNull)
        {
            // OTM.RECV.MCR.A3UH1D9712IAM057.A3UH1D9712IAM057.JOBID123456789
            bool bResult = false;
            cellDataSyncDataOrNull = null;
            do
            {
                if (HLDevice.CDeviceSviInterface.EReceiveDataType.CellDataSyncAcknowledge != packet.eReceiveData)
                {
                    RaiseErrorEvent(packet, string.Format("if (EReceiveDataType.CellDataSyncAcknowledge != packet.eReceiveData)"));
                    break;
                }
                string[] splitText = packet.strReceiveData.Split(new string[] { HLDevice.CDeviceSviInterface.SEPARATOR }, StringSplitOptions.None);
                if (splitText.Length < 3)
                {
                    RaiseErrorEvent(packet, string.Format("if (splitText.Length < 3)"));
                    break;
                }
                cellDataSyncDataOrNull = new ReceiveData.CellDataSyncData()
                {
                    CellID = splitText[0],
                    InnerID = splitText[1],
                    JobID = splitText[2]
                };

                bResult = true;
            } while (false);
            return bResult;
        }
    }
}

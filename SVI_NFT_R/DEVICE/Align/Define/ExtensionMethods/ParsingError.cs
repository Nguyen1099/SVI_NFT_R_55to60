using System;
using System.Runtime.CompilerServices;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// 데이터 파싱 중에 에러가 나면 발생하는 이벤트
        /// </summary>
        public static event EventHandler<string> ParsingError;

        private static void RaiseErrorEvent(DeviceIF.CReceiveData packet, string ErrorMessage, [CallerMemberName] string callerMemberName = "")
        {
            ParsingError?.Invoke(packet, string.Format("[FAILED] [{0}] {1}", callerMemberName, ErrorMessage));
        }
    }
}
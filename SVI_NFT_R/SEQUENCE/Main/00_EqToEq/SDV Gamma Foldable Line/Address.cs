using ENC.IO.Common;
using ENC.IO.DebugKit;
using System.Collections.Generic;

namespace SVI_NFT_R.EqToEq.SdvGammaFoldableLine
{
    public static class Address
    {
        public static bool IsInitialized { get; private set; } = false;
        public static global::EqToEq.BitDeviceMap Bits { get; } = new global::EqToEq.BitDeviceMap();
        public static global::EqToEq.WordDeviceMap Words { get; } = new global::EqToEq.WordDeviceMap();
        private static int mReferenceCount = 0;

        public static bool Initialize(AddressOptionSet option)
        {
            mReferenceCount++;
            if (IsInitialized == true)
            {
                return true;
            }

            var errorsAndWarnings = new List<ErrorSet>();

            // 디바이스 맵 연결
            {
                Bits.AssignMap(option.CcLinkIeControlNetworkDevB, errorsAndWarnings);
                Words.AssignMap(option.CcLinkIeControlNetworkDevW, errorsAndWarnings);
            }

            // 에러 디스플레이
            if (errorsAndWarnings.Count > 0)
            {
                errorsAndWarnings.ShowErrorDisplayUi();
            }

            IsInitialized = true;
            return true;
        }

        public static void DeInitialize()
        {
            if (IsInitialized == false)
            {
                return;
            }
            if (--mReferenceCount > 0)
            {
                return;
            }

            IsInitialized = false;
        }
    }
}

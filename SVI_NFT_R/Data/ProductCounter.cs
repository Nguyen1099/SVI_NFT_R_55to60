using System;

namespace SVI_NFT_R
{
    public static class ProductCounter
    {
        public static Utils.CountRecoder TodayTrackIn => mTodayTrackIn.Value;
        public static Utils.CountRecoder TodayTrackOut => mTodayTrackOut.Value;
        public static Utils.CountRecoder TotalTrackOut => mTotalTrackOut.Value;
        private static readonly Lazy<Utils.CountRecoder> mTodayTrackIn = new Lazy<Utils.CountRecoder>(() =>
        {
            var newInstance = new Utils.CountRecoder();
            newInstance.Initialize(@".\cache\ProductCounter_TodayTkIn.bak", true);
            return newInstance;
        }, true);
        private static readonly Lazy<Utils.CountRecoder> mTodayTrackOut = new Lazy<Utils.CountRecoder>(() =>
        {
            var newInstance = new Utils.CountRecoder();
            newInstance.Initialize(@".\cache\ProductCounter_TodayTkOut.bak", true);
            return newInstance;
        }, true);
        private static readonly Lazy<Utils.CountRecoder> mTotalTrackOut = new Lazy<Utils.CountRecoder>(() =>
        {
            var newInstance = new Utils.CountRecoder();
            newInstance.Initialize(@".\cache\ProductCounter_TotalTkOut.bak", false);
            return newInstance;
        }, true);

        public static void DeInitialize()
        {
            TodayTrackIn.DeInitialize();
            TodayTrackOut.DeInitialize();
            TotalTrackOut.DeInitialize();
        }
    }

}

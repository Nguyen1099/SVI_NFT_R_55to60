using System;
using DeviceIF = HLDevice.CDeviceAlignInterface;

namespace SVI_NFT_R.DEVICE.Align
{
    public partial class ReceiveData
    {
        [Serializable]
        public class AlignResultData : KeepData
        {
            public string ModelID { get; set; } = string.Empty;
            public string InnerID { get; set; } = DeviceIF.NOCELL;
            public string CellID { get; set; } = DeviceIF.NOCELL;
            public int X { get; set; } = 0;
            public int Y { get; set; } = 0;
            public int T { get; set; } = 0;
            public int Score { get; set; } = 0;
        }
    }

    public class AlignResult
    {
        public bool CanUse => (!IsInterlockScore && !IsInterlockX && !IsInterlockY && !IsInterlockT);
        public bool IsInterlockX => Math.Abs(X) > mConfig.GetAlignOptionParameter(mAlignIndex).dAlignInterlockX;
        public bool IsInterlockY => Math.Abs(Y) > mConfig.GetAlignOptionParameter(mAlignIndex).dAlignInterlockY;
        public bool IsInterlockT => Math.Abs(T) > mConfig.GetAlignOptionParameter(mAlignIndex).dAlignInterlockT;
        public bool IsInterlockScore => Score < mConfig.GetAlignOptionParameter(mAlignIndex).iVisionScore;
        public double X { get; private set; } = 0d;
        public double Y { get; private set; } = 0d;
        public double T { get; private set; } = 0d;
        public double Score { get; private set; } = 0d;
        private readonly CConfig mConfig;
        private readonly CDefine.EAlign mAlignIndex;

        public AlignResult(CDefine.EAlign alignIndex, CConfig config)
        {
            mAlignIndex = alignIndex;
            mConfig = config;
        }

        public void SetValue(double score, double x, double y, double t)
        {
            X = x;
            Y = y;
            T = t;
            Score = score;
        }

        public void Reset()
        {
            X = 0;
            Y = 0;
            T = 0;
            Score = 0;
        }
    }
}
using SVI_NFT_R.CellData;
using System;
using System.Linq;

namespace SVI_NFT_R.DEVICE.Nachi
{
    [Serializable]
    public sealed class NachiRobotAlignData
    {
        public OffsetData AlignTool1 { get; set; } = new OffsetData();
        public OffsetData AlignTool2 { get; set; } = new OffsetData();

        public int[] ToIntArray()
        {
            int[] result = AlignTool1.ToIntArray()
                .Concat(AlignTool2.ToIntArray())
                .ToArray();
            return result;
        }

        [Serializable]
        public class OffsetData
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double T { get; set; }
            public ECellPlacement CameraIndex { get; set; }
            public double Score { get; set; }

            public OffsetData()
            {
                X = 0d;
                Y = 0d;
                T = 0d;
                CameraIndex = ECellPlacement.P1;
                Score = 0d;
            }

            public OffsetData(double x, double y, double t, ECellPlacement cameraIndex, double score)
            {
                X = x;
                Y = y;
                T = t;
                CameraIndex = cameraIndex;
                Score = score;
            }

            public int[] ToIntArray()
            {
                return new int[] { (int)(X * 1000d), (int)(Y * 1000d), (int)(T * 1000d) };
            }
        }
    }

}
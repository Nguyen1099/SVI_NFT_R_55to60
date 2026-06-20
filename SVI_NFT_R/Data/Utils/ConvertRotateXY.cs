using System;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        public static void ConvertRotateXY(ref double x, ref double y, double angle)
        {
            double radian = angle * Math.PI / 180d;
            double inputX = x;
            double inputY = y;
            x = inputX * Math.Cos(radian) - inputY * Math.Sin(radian);
            y = inputX * Math.Sin(radian) + inputY * Math.Cos(radian);
        }
    }
}

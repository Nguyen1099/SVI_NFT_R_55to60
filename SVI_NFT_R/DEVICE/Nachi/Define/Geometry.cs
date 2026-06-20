using System;

namespace SVI_NFT_R.DEVICE.Nachi
{
    [Serializable]
    public class Geometry
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }
        public double Z { get; protected set; }
        public double Rx { get; protected set; }
        public double Ry { get; protected set; }
        public double Rz { get; protected set; }

        public Geometry(double x, double y, double z, double rx, double ry, double rz)
        {
            X = x;
            Y = y;
            Z = z;
            Rx = rx;
            Ry = ry;
            Rz = rz;
        }

        public override string ToString() => $"{X:0.000},{Y:0.000},{Z:0.000},{Rx:0.000},{Ry:0.000},{Rz:0.000}";
    }
}

using System;
using System.Collections.Generic;

namespace SVI_NFT_R.DEVICE.Nachi
{
    [Serializable]
    public class RmsData
    {
        public IReadOnlyList<Geometry> ToolDownData => mToolDownData;
        private List<SettableGeometry> mToolDownData = new List<SettableGeometry>();
        private int[] mRawdata;

        public bool SetRawdata(int[] rawdata)
        {
            if (rawdata.Length % 6 != 0)
            {
                return false;
            }
            mRawdata = rawdata;
            int dataCount = rawdata.Length / 6;
            if (mToolDownData.Count != dataCount)
            {
                List<SettableGeometry> newList = new List<SettableGeometry>();
                for (int i = 0; i < dataCount; i++)
                {
                    newList.Add(new SettableGeometry(0d, 0d, 0d, 0d, 0d, 0d));
                }
                mToolDownData = newList;
            }
            int index = 0;
            bool bChanged = false;
            foreach (var data in mToolDownData)
            {
                int startIndex = index * 6;
                bool bChangeValue = data.SetValue(
                    rawdata[startIndex + 0] / 1000d,
                    rawdata[startIndex + 1] / 1000d,
                    rawdata[startIndex + 2] / 1000d,
                    rawdata[startIndex + 3] / 1000d,
                    rawdata[startIndex + 4] / 1000d,
                    rawdata[startIndex + 5] / 1000d
                    );
                if (bChangeValue == true)
                {
                    bChanged = true;
                }
                index++;
            }
            return bChanged;
        }

        [Serializable]
        private class SettableGeometry : Geometry
        {
            public SettableGeometry(double x, double y, double z, double rx, double ry, double rz) : base(x, y, z, rx, ry, rz)
            {
            }

            public bool SetValue(double x, double y, double z, double rx, double ry, double rz)
            {
                bool bChanged = false;
                if (
                    X != x
                    || Y != y
                    || Z != z
                    || Rx != rx
                    || Ry != ry
                    || Rz != rz
                    )
                {
                    bChanged = true;
                }
                X = x;
                Y = y;
                Z = z;
                Rx = rx;
                Ry = ry;
                Rz = rz;
                return bChanged;
            }
        }
    }
}

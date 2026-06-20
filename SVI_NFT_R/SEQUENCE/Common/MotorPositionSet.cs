using System;

namespace SVI_NFT_R
{
    public sealed class MotorPositionSet<TCommand, TPosition>
    {
        public TCommand Command { get; private set; }
        public TPosition Position { get; private set; }
        public double Offset => mOffset.Invoke();
        private Func<double> mOffset;

        public MotorPositionSet(TCommand cmd, TPosition pos, Func<double> getOffsetOrNull = null)
        {
            Command = cmd;
            Position = pos;
            mOffset = getOffsetOrNull != null ? getOffsetOrNull : getZeroOffset;
        }

        internal double getZeroOffset() => 0d;
    }
}

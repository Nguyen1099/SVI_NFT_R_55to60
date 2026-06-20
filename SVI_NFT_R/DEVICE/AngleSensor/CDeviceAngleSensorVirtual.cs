using HLDevice.Abstract;

namespace HLDevice.Sensor.Angle
{
    public class CDeviceAngleSensorVirtual : CDeviceAngleSensorAbstract
    {
        private bool[] mIsReadingChannel = new bool[16];

        public override bool Initialize(CInitializeParameter objInitializeParameter)
        {
            return true;
        }

        public override void DeInitialize()
        {
        }

        public override string GetVersion()
        {
            return "1.0.0.0";
        }

        public override bool IsConnected()
        {
            return true;
        }

        public override bool IsOpened()
        {
            return true;
        }

        public override double GetAngle(int channelIndex)
        {
            return mIsReadingChannel[channelIndex - 1] ? channelIndex / 10d : -997d;
        }

        public override bool GetReadingChannel(int channelIndex)
        {
            return mIsReadingChannel[channelIndex - 1];
        }

        public override void SetReadingChannel(int channelIndex, bool bIsUpdate)
        {
            mIsReadingChannel[channelIndex - 1] = bIsUpdate;
        }
    }
}
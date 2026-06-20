using DeviceDLL.Sensor.Angle.Dgac;
using HLDevice.Abstract;

namespace HLDevice.Sensor.Angle
{
    public class CDeviceAngleSensorDgac : CDeviceAngleSensorAbstract
    {
        private readonly DeviceAngleSensorDgac mDevice = new DeviceAngleSensorDgac();

        public override bool Initialize(CInitializeParameter objInitializeParameter)
        {
            CDefine.CInitializeParameter initializeParameter = new CDefine.CInitializeParameter();
            initializeParameter.eType = (CDefine.CInitializeParameter.EType)objInitializeParameter.eType;
            initializeParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
            initializeParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;
            initializeParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
            initializeParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
            initializeParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
            initializeParameter.eParity = (CDefine.CInitializeParameter.ESerialPortParity)objInitializeParameter.eParity;
            initializeParameter.eStopBits = (CDefine.CInitializeParameter.ESerialPortStopBits)objInitializeParameter.eStopBits;
            return mDevice.Initialize(initializeParameter);
        }

        public override void DeInitialize()
        {
            mDevice.DeInitialize();
        }

        public override string GetVersion()
        {
            return mDevice.GetVersion();
        }

        public override bool IsConnected()
        {
            return mDevice.IsConnected();
        }

        public override bool IsOpened()
        {
            return mDevice.IsOpened();
        }

        public override double GetAngle(int channelIndex)
        {
            return mDevice.GetAngle(channelIndex);
        }

        public override bool GetReadingChannel(int channelIndex)
        {
            return mDevice.GetReadingChannel(channelIndex);
        }

        public override void SetReadingChannel(int channelIndex, bool bIsUpdate)
        {
            mDevice.SetReadingChannel(channelIndex, bIsUpdate);
        }
    }
}
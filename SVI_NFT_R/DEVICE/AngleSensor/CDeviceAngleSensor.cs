using HLDevice.Abstract;
using SVI_NFT_R;

namespace HLDevice
{
    public class CDeviceAngleSensor
    {
        private CDeviceAngleSensorAbstract mAngleSensor;
        private CDocument mDocument;

        public bool HLInitialize(CDocument document, CConfig.CAngleSensorParameter setParameter)
        {
            mDocument = document;

            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == mDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                mAngleSensor = new Sensor.Angle.CDeviceAngleSensorVirtual();
            }
            else
            {
                mAngleSensor = new Sensor.Angle.CDeviceAngleSensorDgac();
            }

            CDeviceAngleSensorAbstract.CInitializeParameter objInitializeParameter = new CDeviceAngleSensorAbstract.CInitializeParameter();
            objInitializeParameter.eType = (CDeviceAngleSensorAbstract.CInitializeParameter.EType)setParameter.eType;
            objInitializeParameter.strSocketIPAddress = setParameter.strSocketIPAddress;
            objInitializeParameter.iSocketPortNumber = setParameter.iSocketPortNumber;
            objInitializeParameter.strSerialPortName = setParameter.strSerialPortName;
            objInitializeParameter.iSerialPortBaudrate = setParameter.iSerialPortBaudrate;
            objInitializeParameter.iSerialPortDataBits = setParameter.iSerialPortDataBits;
            objInitializeParameter.eParity = (CDeviceAngleSensorAbstract.CInitializeParameter.ESerialPortParity)setParameter.eParity;
            objInitializeParameter.eStopBits = (CDeviceAngleSensorAbstract.CInitializeParameter.ESerialPortStopBits)setParameter.eStopBits;

            return mAngleSensor.Initialize(objInitializeParameter);
        }

        public void HLDeInitialize() => mAngleSensor.DeInitialize();

        public string HLGetVersion() => mAngleSensor.GetVersion();

        public bool HLIsOpened() => mAngleSensor.IsOpened();

        public bool HLIsConnected() => mAngleSensor.IsConnected();

        public double GetAngle(int channelIndex) => mAngleSensor.GetAngle(channelIndex);

        public bool GetIsReadingChannel(int channelIndex) => mAngleSensor.GetReadingChannel(channelIndex);

        public void SetIsReadingChannel(int channelIndex, bool bIsUpdate) => mAngleSensor.SetReadingChannel(channelIndex, bIsUpdate);
    }
}

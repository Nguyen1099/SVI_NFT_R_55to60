namespace HLDevice.Abstract
{
    public abstract class CDeviceAngleSensorAbstract
    {
        public class CInitializeParameter
        {
            public enum EType
            {
                TYPE_SOCKET_CLIENT = 0,
                TYPE_SOCKET_SERVER,
                TYPE_SERIAL_PORT,
                TYPE_FINAL
            };

            public enum ESerialPortParity
            {
                PARITY_NONE = 0,
                PARITY_ODD,
                PARITY_EVEN,
                PARITY_MARK,
                PARITY_SPACE
            };

            public enum ESerialPortStopBits
            {
                STOP_BITS_NONE = 0,
                STOP_BITS_ONE,
                STOP_BITS_TWO,
                STOP_BITS_ONE_POINT_FIVE
            };

            public EType eType;
            public string strSocketIPAddress;
            public int iSocketPortNumber;
            public string strSerialPortName;
            public int iSerialPortBaudrate;
            public int iSerialPortDataBits;
            public ESerialPortParity eParity;
            public ESerialPortStopBits eStopBits;
        }

        public abstract bool Initialize(CInitializeParameter objInitializeParameter);

        public abstract void DeInitialize();

        public abstract string GetVersion();

        public abstract bool IsOpened();

        public abstract bool IsConnected();

        public abstract double GetAngle(int channelIndex);

        public abstract bool GetReadingChannel(int channelIndex);

        public abstract void SetReadingChannel(int channelIndex, bool bIsUpdate);
    }
}
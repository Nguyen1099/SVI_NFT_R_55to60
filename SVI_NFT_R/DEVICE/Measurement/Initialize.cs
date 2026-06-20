using SVI_NFT_R;
using System.Diagnostics;

namespace Device
{
    public static partial class ExtensionMethods
    {
        public static bool Initialize(this Measurement.IDeviceMeasurement device, CConfig.CMeasurementInitializeParameter parameter)
        {
            // + Measurement 디바이스 공용 Parameter를 전용 Parameter로 변환한 뒤 초기화 함수를 실행함
            switch (device.DeviceType)
            {
                case Measurement.EMeasurementDevice.Virtual:
                    {
                        object internalParameter = null;
                        {
                            // 셋팅값 없음
                        }
                        var deviceInitializer = (Measurement.IDeviceMeasurementInitialize<object>)device;
                        return deviceInitializer.Initialize(internalParameter);
                    }

                case Measurement.EMeasurementDevice.KyengQcManagerMulti:
                    {
                        var internalParameter = new DeviceDLL.Measurement.KYENG.QcManagerMulti.InitializeParameter();
                        {
                            switch (parameter.eType)
                            {
                                case CConfig.CommunicationParameter.EType.TYPE_SOCKET_SERVER:
                                case CConfig.CommunicationParameter.EType.TYPE_SOCKET_CLIENT:
                                    {
                                        internalParameter.Type = DeviceDLL.Measurement.KYENG.QcManagerMulti.ECommType.Socket;
                                        internalParameter.SocketIPAddress = parameter.strSocketIPAddress;
                                        internalParameter.SocketPortNumber = parameter.iSocketPortNumber;
                                    }
                                    break;

                                case CConfig.CommunicationParameter.EType.TYPE_SERIAL_PORT:
                                    {
                                        internalParameter.Type = DeviceDLL.Measurement.KYENG.QcManagerMulti.ECommType.SerialPort;
                                        internalParameter.SerialPortName = parameter.strSerialPortName;
                                        internalParameter.SerialPortBaudrate = parameter.iSerialPortBaudrate;
                                        internalParameter.SerialPortDataBits = parameter.iSerialPortDataBits;
                                        switch (parameter.eStopBits)
                                        {
                                            case CConfig.CommunicationParameter.ESerialPortStopBits.STOP_BITS_NONE:
                                                internalParameter.SerialPortStopBits = DeviceDLL.Protocol.Modbus.EStopBits.None;
                                                break;

                                            case CConfig.CommunicationParameter.ESerialPortStopBits.STOP_BITS_ONE:
                                                internalParameter.SerialPortStopBits = DeviceDLL.Protocol.Modbus.EStopBits.One;
                                                break;

                                            case CConfig.CommunicationParameter.ESerialPortStopBits.STOP_BITS_TWO:
                                                internalParameter.SerialPortStopBits = DeviceDLL.Protocol.Modbus.EStopBits.Two;
                                                break;

                                            case CConfig.CommunicationParameter.ESerialPortStopBits.STOP_BITS_ONE_POINT_FIVE:
                                                internalParameter.SerialPortStopBits = DeviceDLL.Protocol.Modbus.EStopBits.OnePointFive;
                                                break;
                                        }
                                        switch (parameter.eParity)
                                        {
                                            case CConfig.CommunicationParameter.ESerialPortParity.PARITY_NONE:
                                                internalParameter.SerialPortParity = DeviceDLL.Protocol.Modbus.EParity.None;
                                                break;

                                            case CConfig.CommunicationParameter.ESerialPortParity.PARITY_ODD:
                                                internalParameter.SerialPortParity = DeviceDLL.Protocol.Modbus.EParity.Odd;
                                                break;

                                            case CConfig.CommunicationParameter.ESerialPortParity.PARITY_EVEN:
                                                internalParameter.SerialPortParity = DeviceDLL.Protocol.Modbus.EParity.Even;
                                                break;

                                            case CConfig.CommunicationParameter.ESerialPortParity.PARITY_MARK:
                                                internalParameter.SerialPortParity = DeviceDLL.Protocol.Modbus.EParity.Mark;
                                                break;

                                            case CConfig.CommunicationParameter.ESerialPortParity.PARITY_SPACE:
                                                internalParameter.SerialPortParity = DeviceDLL.Protocol.Modbus.EParity.Space;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        var deviceInitializer = (Measurement.IDeviceMeasurementInitialize<DeviceDLL.Measurement.KYENG.QcManagerMulti.InitializeParameter>)device;
                        return deviceInitializer.Initialize(internalParameter);
                    }

                default:
                    Debug.Assert(false);
                    return false;
            }
        }
    }
}

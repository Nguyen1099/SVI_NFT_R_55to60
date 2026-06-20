using System.Diagnostics;

namespace Device.Measurement
{
    public static partial class Factory
    {
        public static IDeviceMeasurement Create(EMeasurementDevice createDevice)
        {
            IDeviceMeasurement device = null;
            switch (createDevice)
            {
                case EMeasurementDevice.Virtual:
                    device = new DeviceDLL.Measurement.ENC.Virtual.DeviceMeasurementVirtual();
                    break;

                case EMeasurementDevice.KyengQcManagerMulti:
                    device = new DeviceDLL.Measurement.KYENG.QcManagerMulti.DeviceMeasurementKyengQcManagerMulti();
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
            return device;
        }
    }
}
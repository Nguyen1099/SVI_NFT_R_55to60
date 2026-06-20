using HLDevice.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class CProcessMain
    {
        public static class SerialReaders
        {
            private class ImplSafetyPlcSignatureData : CDeviceSafetyPlcAbstract.ISafetyPlcSignatureData
            {
                public bool IsValid => false;
                public string ErrorReason => string.Empty;
                public string SignatureCode => string.Empty;
                public DateTime LastModifyDateTime => DateTime.MinValue;
            }
            public static bool IsInitialized { get; private set; } = false;
            public static IReadOnlyList<Data.ISerialReaderStatistics> Items => mItems;
            public static Dictionary<CDefine.EGms, Data.SerialReader<double[]>> Gms { get; private set; } = new Dictionary<CDefine.EGms, Data.SerialReader<double[]>>();
            public static Dictionary<CDefine.EMcu, Data.SerialReader<int[]>> Mcu { get; private set; } = new Dictionary<CDefine.EMcu, Data.SerialReader<int[]>>();
            public static Dictionary<CDefine.ETemperature, Data.SerialReader<double>> Temperature { get; private set; } = new Dictionary<CDefine.ETemperature, Data.SerialReader<double>>();
            public static Dictionary<CDefine.ESafetyPlc, Data.SerialReader<CDeviceSafetyPlcAbstract.ISafetyPlcSignatureData>> SafetyPlc { get; private set; } = new Dictionary<CDefine.ESafetyPlc, Data.SerialReader<CDeviceSafetyPlcAbstract.ISafetyPlcSignatureData>>();
            private static readonly List<Data.ISerialReaderStatistics> mItems = new List<Data.ISerialReaderStatistics>(32);
            private static string mGmsNickName;
            private static CDocument mDocument;
            private static readonly object mTemperatureDeviceReadingLock = new object();

            public static bool Initialize(CDocument document)
            {
                if (IsInitialized == true)
                {
                    return false;
                }
                mDocument = document;

                foreach (CDefine.EGms index in Enum.GetValues(typeof(CDefine.EGms)))
                {
                    Gms[index] = new Data.SerialReader<double[]>();
                    Gms[index].Index = index;
                    Gms[index].Name = $"{CDefine.DEVICE_GMS}+{index}";
                    Gms[index].ReadingFailDefualtValue = Enumerable.Repeat(0d, 5).ToArray();
                    Gms[index].OnReading += SerialReaderGms_OnReading;
                    Gms[index].OnReport += SerialReaderGms_OnReport;
                    Gms[index].Initilaize();
                }
                foreach (CDefine.EMcu index in Enum.GetValues(typeof(CDefine.EMcu)))
                {
                    Mcu[index] = new Data.SerialReader<int[]>();
                    Mcu[index].Index = index;
                    Mcu[index].Name = $"{CDefine.DEVICE_MCU}+{index}";
                    Mcu[index].ReadingFailDefualtValue = Enumerable.Repeat(0, 32).ToArray();
                    Mcu[index].OnReading += SerialReaderMcu_OnReading;
                    Mcu[index].OnReport += SerialReaderMcu_OnReport;
                    Mcu[index].Initilaize();
                }
                foreach (CDefine.ETemperature index in Enum.GetValues(typeof(CDefine.ETemperature)))
                {
                    Temperature[index] = new Data.SerialReader<double>();
                    Temperature[index].Index = index;
                    Temperature[index].Name = $"{CDefine.DEVICE_TEMPERATURE}+{index}";
                    Temperature[index].ReadingFailDefualtValue = 0d;
                    Temperature[index].OnReading += SerialReaderTemperature_OnReading;
                    Temperature[index].OnReport += SerialReaderTemperature_OnReport;
                    Temperature[index].Initilaize();
                }
                foreach (CDefine.ESafetyPlc index in Enum.GetValues(typeof(CDefine.ESafetyPlc)))
                {
                    SafetyPlc[index] = new Data.SerialReader<CDeviceSafetyPlcAbstract.ISafetyPlcSignatureData>();
                    SafetyPlc[index].Index = index;
                    SafetyPlc[index].Name = $"{CDefine.DEVICE_SAFETYPLC}+{index}";
                    SafetyPlc[index].ReadingFailDefualtValue = new ImplSafetyPlcSignatureData();
                    SafetyPlc[index].OnReading += SerialReaderSafetyPlc_OnReading;
                    SafetyPlc[index].OnReport += SerialReaderSafetyPlc_OnReport;
                    SafetyPlc[index].Initilaize();
                }

                mItems.Clear();
                mItems.AddRange(Gms.Values);
                mItems.AddRange(Mcu.Values);
                mItems.AddRange(Temperature.Values);
                mItems.AddRange(SafetyPlc.Values);

                IsInitialized = true;
                return true;
            }

            public static void DeInitialize()
            {
                if (IsInitialized == false)
                {
                    return;
                }

                foreach (CDefine.EGms index in Enum.GetValues(typeof(CDefine.EGms)))
                {
                    Gms[index].DeInitilaize();
                    Gms[index].OnReading -= SerialReaderGms_OnReading;
                    Gms[index].OnReport -= SerialReaderGms_OnReport;
                }
                foreach (CDefine.EMcu index in Enum.GetValues(typeof(CDefine.EMcu)))
                {
                    Mcu[index].DeInitilaize();
                    Mcu[index].OnReading -= SerialReaderMcu_OnReading;
                    Mcu[index].OnReport -= SerialReaderMcu_OnReport;
                }
                foreach (CDefine.ETemperature index in Enum.GetValues(typeof(CDefine.ETemperature)))
                {
                    Temperature[index].DeInitilaize();
                    Temperature[index].OnReading -= SerialReaderTemperature_OnReading;
                    Temperature[index].OnReport -= SerialReaderTemperature_OnReport;
                }
                foreach (CDefine.ESafetyPlc index in Enum.GetValues(typeof(CDefine.ESafetyPlc)))
                {
                    SafetyPlc[index].DeInitilaize();
                    SafetyPlc[index].OnReading -= SerialReaderSafetyPlc_OnReading;
                    SafetyPlc[index].OnReport -= SerialReaderSafetyPlc_OnReport;
                }

                IsInitialized = false;
            }

            private static void SerialReaderGms_OnReading(object sender, Data.ReadingEventArgs<double[]> e)
            {
                CDefine.EGms index = (CDefine.EGms)e.Index;
                if (string.IsNullOrEmpty(mGmsNickName) == true
                    && mDocument.m_objProcessMain.m_objElectrostatic[index].HLLoadNickName(1, out mGmsNickName) == false
                    )
                {
                    mGmsNickName = null;
                    e.Result = false;
                    return;
                }
                string[] readValues;
                e.Result = mDocument.m_objProcessMain.m_objElectrostatic[index].HLReadChannel(mGmsNickName, out readValues);
                e.Value = new double[readValues.Length];
                for (int i = 0; i < readValues.Length; i++)
                {
                    if (string.IsNullOrEmpty(readValues[i]) == true
                        || readValues[i].Equals("OPEN") == true
                        || readValues[i].Equals("RANGE OVER") == true
                        )
                    {
                        readValues[i] = "9999";
                    }

                    double value;
                    double.TryParse(readValues[i], out value);
                    e.Value[i] = value / 100d;
                }
            }

            private static void SerialReaderMcu_OnReading(object sender, Data.ReadingEventArgs<int[]> e)
            {
                CDefine.EMcu index = (CDefine.EMcu)e.Index;
                int[] readValues = new int[1] { 0 };
                //e.Result = mDocument.m_objProcessMain.m_objFFU[index].HLGetFanSpeedAll(out readValues);
                for (int i = 0; i < readValues.Length; i++)
                {
                    e.Result = mDocument.m_objProcessMain.m_objFFU[index].HLGetCurrentSpeed(1, i, out readValues[i]);
                    if (e.Result == false)
                    {
                        break;
                    }
                }
                e.Value = readValues;
            }

            private static void SerialReaderTemperature_OnReading(object sender, Data.ReadingEventArgs<double> e)
            {
                byte index = (byte)(CDefine.ETemperature)e.Index;
                double readValue;
                lock (mTemperatureDeviceReadingLock)
                {
                    e.Result = mDocument.m_objProcessMain.m_objTemperature[CDefine.ETemperatureController.MainGruop].HLReadTemp(index, out readValue);
                }
                e.Value = readValue;
            }

            private static void SerialReaderSafetyPlc_OnReading(object sender, Data.ReadingEventArgs<CDeviceSafetyPlcAbstract.ISafetyPlcSignatureData> e)
            {
                CDefine.ESafetyPlc index = (CDefine.ESafetyPlc)e.Index;
                CDeviceSafetyPlcAbstract.ISafetyPlcSignatureData readValue;
                mDocument.m_objProcessMain.m_objSafetyPlcs[index].TryReadSignatureCode(out readValue);
                e.Result = readValue.IsValid;
                e.Value = readValue;
            }

            private static void SerialReaderGms_OnReport(object sender, EventArgs e)
            {
                var serialReaderReportCsv = sender as Data.ISerialReaderReportCsv;
                mDocument.SetUpdateLog(CDefine.ELogType.LOG_COMM_STATISTICS_GMS, serialReaderReportCsv.GetOneLineForCsvReport());
            }

            private static void SerialReaderMcu_OnReport(object sender, EventArgs e)
            {
                var serialReaderReportCsv = sender as Data.ISerialReaderReportCsv;
                mDocument.SetUpdateLog(CDefine.ELogType.LOG_COMM_STATISTICS_MCU, serialReaderReportCsv.GetOneLineForCsvReport());
            }

            private static void SerialReaderTemperature_OnReport(object sender, EventArgs e)
            {
                var serialReaderReportCsv = sender as Data.ISerialReaderReportCsv;
                mDocument.SetUpdateLog(CDefine.ELogType.LOG_COMM_STATISTICS_TEMPERATURE, serialReaderReportCsv.GetOneLineForCsvReport());
            }

            private static void SerialReaderSafetyPlc_OnReport(object sender, EventArgs e)
            {
                var serialReaderReportCsv = sender as Data.ISerialReaderReportCsv;
                mDocument.SetUpdateLog(CDefine.ELogType.LOG_COMM_STATISTICS_SAFETYPLC, serialReaderReportCsv.GetOneLineForCsvReport());
            }
        }
    }
}

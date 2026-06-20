using ENC.IO;
using ENC.IO.Common;
using System;

namespace EqToEq
{
    public sealed class EqToEqUiOneDataSet
    {
        public bool IsReadOnly { get; }
        public string Name => $"{mPrefix}{HandlingData.Information.ID}";
        public string Label => $"W{HandlingData.StartByteIndex / 2:X5}";
        public string Comment => HandlingData.Information.Comment;
        public string Description => HandlingData.Information.Description;
        public string Value { get => GetValue(); set => SetValue(value); }
        public double DataRangeMin { get; private set; } = double.MinValue;
        public double DataRangeMax { get; private set; } = double.MaxValue;
        public IHaveHandlingDataInformation HandlingData { get; }
        private readonly string mPrefix;

        public EqToEqUiOneDataSet(IHaveHandlingDataInformation handlingData, bool bIsReadOnly, string prefix)
        {
            IsReadOnly = bIsReadOnly;
            HandlingData = handlingData;
            mPrefix = prefix;
            InitDataRange();
        }

        public void SetDoubleValue(double value)
        {
            unchecked
            {
                switch (HandlingData.Information.HandlingDataTypeString)
                {
                    case "bool":
                    case "byte":
                        SetValue(((byte)value).ToString());
                        break;
                    case "sbyte":
                        SetValue(((sbyte)value).ToString());
                        break;
                    case "ushort":
                        SetValue(((ushort)value).ToString());
                        break;
                    case "short":
                        SetValue(((short)value).ToString());
                        break;
                    case "uint":
                        SetValue(((uint)value).ToString());
                        break;
                    case "int":
                        SetValue(((int)value).ToString());
                        break;
                    case "ulong":
                        SetValue(((ulong)value).ToString());
                        break;
                    case "long":
                        SetValue(((long)value).ToString());
                        break;
                    case "float":
                    case "double":
                        SetValue(value.ToString());
                        break;
                    case "char":
                    default:
                        return;
                }
            }
        }

        public static bool IsSupportType(Type type)
        {
            if (type == typeof(IHandlingData<bool>)
                || type == typeof(IHandlingData<byte>)
                || type == typeof(IHandlingData<sbyte>)
                || type == typeof(IHandlingData<ushort>)
                || type == typeof(IHandlingData<short>)
                || type == typeof(IHandlingData<uint>)
                || type == typeof(IHandlingData<int>)
                || type == typeof(IHandlingData<ulong>)
                || type == typeof(IHandlingData<long>)
                || type == typeof(IHandlingData<float>)
                || type == typeof(IHandlingData<double>)
                // ! 배열 타입은 문자열만 지원함
                || type == typeof(IHandlingArrayData<char>)
                || IsDataBlockType(type) == true
                || IsDataBlockArrayType(type) == true
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsDataBlockType(Type type)
        {
            return Attribute.GetCustomAttribute(type, typeof(HandlingDataBlockAttribute)) != null;
        }

        public static bool IsDataBlockArrayType(Type type)
        {
            if (type.IsGenericType == true)
            {
                var typeParameters = type.GetGenericArguments();
                foreach (var item in typeParameters)
                {
                    if (Attribute.GetCustomAttribute(item, typeof(HandlingDataBlockAttribute)) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsIntegerType(IHaveHandlingDataInformation type)
        {
            if (type is IHandlingData<bool>
                || type is IHandlingData<byte>
                || type is IHandlingData<sbyte>
                || type is IHandlingData<ushort>
                || type is IHandlingData<short>
                || type is IHandlingData<uint>
                || type is IHandlingData<int>
                || type is IHandlingData<ulong>
                || type is IHandlingData<long>
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsRealType(IHaveHandlingDataInformation type)
        {
            if (type is IHandlingData<float>
                || type is IHandlingArrayData<double>
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsStringType(IHaveHandlingDataInformation type)
        {
            if (type is IHandlingArrayData<char>)
            {
                return true;
            }
            return false;
        }

        private void InitDataRange()
        {
            switch (HandlingData.Information.HandlingDataTypeString)
            {
                case "bool":
                    DataRangeMin = 0d;
                    DataRangeMax = 1d;
                    break;
                case "byte":
                    DataRangeMin = byte.MinValue;
                    DataRangeMax = byte.MaxValue;
                    break;
                case "sbyte":
                    DataRangeMin = sbyte.MinValue;
                    DataRangeMax = sbyte.MaxValue;
                    break;
                case "ushort":
                    DataRangeMin = ushort.MinValue;
                    DataRangeMax = ushort.MaxValue;
                    break;
                case "short":
                    DataRangeMin = short.MinValue;
                    DataRangeMax = short.MaxValue;
                    break;
                case "uint":
                    DataRangeMin = uint.MinValue;
                    DataRangeMax = uint.MaxValue;
                    break;
                case "int":
                    DataRangeMin = int.MinValue;
                    DataRangeMax = int.MaxValue;
                    break;
                case "ulong":
                    DataRangeMin = ulong.MinValue;
                    DataRangeMax = ulong.MaxValue;
                    break;
                case "long":
                    DataRangeMin = long.MinValue;
                    DataRangeMax = long.MaxValue;
                    break;
                case "float":
                    DataRangeMin = float.MinValue;
                    DataRangeMax = float.MaxValue;
                    break;
                case "double":
                    DataRangeMin = double.MinValue;
                    DataRangeMax = double.MaxValue;
                    break;
                case "char":
                default:
                    break;
            }
        }

        private void SetValue(string dataValue)
        {
            if (string.IsNullOrEmpty(dataValue) == true)
            {
                return;
            }

            switch (HandlingData.Information.HandlingDataTypeString)
            {
                case "bool":
                    {
                        byte.TryParse(dataValue, out byte value);
                        ((IHandlingData<bool>)HandlingData).SetValue(Convert.ToBoolean(value));
                    }
                    break;
                case "byte":
                    {
                        byte.TryParse(dataValue, out byte value);
                        ((IHandlingData<byte>)HandlingData).SetValue(value);
                    }
                    break;
                case "sbyte":
                    {
                        sbyte.TryParse(dataValue, out sbyte value);
                        ((IHandlingData<sbyte>)HandlingData).SetValue(value);
                    }
                    break;
                case "ushort":
                    {
                        ushort.TryParse(dataValue, out ushort value);
                        ((IHandlingData<ushort>)HandlingData).SetValue(value);
                    }
                    break;
                case "short":
                    {
                        short.TryParse(dataValue, out short value);
                        ((IHandlingData<short>)HandlingData).SetValue(value);
                    }
                    break;
                case "uint":
                    {
                        uint.TryParse(dataValue, out uint value);
                        ((IHandlingData<uint>)HandlingData).SetValue(value);
                    }
                    break;
                case "int":
                    {
                        int.TryParse(dataValue, out int value);
                        ((IHandlingData<int>)HandlingData).SetValue(value);
                    }
                    break;
                case "ulong":
                    {
                        ulong.TryParse(dataValue, out ulong value);
                        ((IHandlingData<ulong>)HandlingData).SetValue(value);
                    }
                    break;
                case "long":
                    {
                        long.TryParse(dataValue, out long value);
                        ((IHandlingData<long>)HandlingData).SetValue(value);
                    }
                    break;
                case "float":
                    {
                        float.TryParse(dataValue, out float value);
                        ((IHandlingData<float>)HandlingData).SetValue(value);
                    }
                    break;
                case "double":
                    {
                        double.TryParse(dataValue, out double value);
                        ((IHandlingData<double>)HandlingData).SetValue(value);
                    }
                    break;
                case "char":
                    {
                        ((IHandlingArrayData<char>)HandlingData).SetString(dataValue);
                    }
                    break;
                default:
                    break;
            }
        }

        private string GetValue()
        {
            switch (HandlingData.Information.HandlingDataTypeString)
            {
                case "bool":
                    return ((IHandlingData<bool>)HandlingData).GetValue().ToString();
                case "byte":
                    return ((IHandlingData<byte>)HandlingData).GetValue().ToString();
                case "sbyte":
                    return ((IHandlingData<sbyte>)HandlingData).GetValue().ToString();
                case "ushort":
                    return ((IHandlingData<ushort>)HandlingData).GetValue().ToString();
                case "short":
                    return ((IHandlingData<short>)HandlingData).GetValue().ToString();
                case "uint":
                    return ((IHandlingData<uint>)HandlingData).GetValue().ToString();
                case "int":
                    return ((IHandlingData<int>)HandlingData).GetValue().ToString();
                case "ulong":
                    return ((IHandlingData<ulong>)HandlingData).GetValue().ToString();
                case "long":
                    return ((IHandlingData<long>)HandlingData).GetValue().ToString();
                case "float":
                    return ((IHandlingData<float>)HandlingData).GetValue().ToString();
                case "double":
                    return ((IHandlingData<double>)HandlingData).GetValue().ToString();
                case "char":
                    return ((IHandlingArrayData<char>)HandlingData).GetString();
                default:
                    return "";
            }
        }
    }
}
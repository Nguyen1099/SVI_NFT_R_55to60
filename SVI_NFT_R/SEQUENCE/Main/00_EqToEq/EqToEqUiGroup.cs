using ENC.IO.Common;
using System;
using System.Collections.Generic;

namespace EqToEq
{
    public sealed class EqToEqUiGroup
    {
        public string GroupName { get; set; }
        public string LeftName { get; set; }
        public string RightName { get; set; }
        public IReadOnlyList<EqToEqUiOneBitSet> LeftBits => mLeftBits;
        public IReadOnlyList<EqToEqUiOneBitSet> RightBits => mRightBits;
        public IReadOnlyList<EqToEqUiOneDataSet> LeftDatas => mLeftDatas;
        public IReadOnlyList<EqToEqUiOneDataSet> RightDatas => mRightDatas;
        private readonly List<EqToEqUiOneBitSet> mLeftBits = new List<EqToEqUiOneBitSet>();
        private readonly List<EqToEqUiOneBitSet> mRightBits = new List<EqToEqUiOneBitSet>();
        private readonly List<EqToEqUiOneDataSet> mLeftDatas = new List<EqToEqUiOneDataSet>();
        private readonly List<EqToEqUiOneDataSet> mRightDatas = new List<EqToEqUiOneDataSet>();

        public void AddLeftBits<TData>(TData data, bool bIsReadOnly)
            where TData : new()
        {
            AddBitsFormDataType(mLeftBits, data, typeof(TData), bIsReadOnly);
        }

        public void AddRightBits<TData>(TData data, bool bIsReadOnly)
            where TData : new()
        {
            AddBitsFormDataType(mRightBits, data, typeof(TData), bIsReadOnly);
        }

        public void AddLeftDatas<TData>(TData data, bool bIsReadOnly)
            where TData : new()
        {
            AddDatassFormDataTypeReclucive(mLeftDatas, data, typeof(TData), bIsReadOnly);
        }

        public void AddRightDatas<TData>(TData data, bool bIsReadOnly)
            where TData : new()
        {
            AddDatassFormDataTypeReclucive(mRightDatas, data, typeof(TData), bIsReadOnly);
        }

        private void AddBitsFormDataType(IList<EqToEqUiOneBitSet> list, object data, Type dataType, bool bIsReadOnly)
        {
            foreach (var property in dataType.GetProperties())
            {
                if (property.GetValue(data) is IHandlingData<bool> bitHandler)
                {
                    list.Add(new EqToEqUiOneBitSet(bitHandler, bIsReadOnly));
                }
            }
        }

        private void AddDatassFormDataTypeReclucive(IList<EqToEqUiOneDataSet> list, object data, Type dataType, bool bIsReadOnly, string prefix = "")
        {
            foreach (var property in dataType.GetProperties())
            {
                if (EqToEqUiOneDataSet.IsSupportType(property.PropertyType) == false)
                {
                    continue;
                }

                // 데이터 블록 배열 타입
                if (EqToEqUiOneDataSet.IsDataBlockArrayType(property.PropertyType) == true)
                {
                    var typeParameters = property.PropertyType.GetGenericArguments();
                    foreach (var item in typeParameters)
                    {
                        if (Attribute.GetCustomAttribute(item, typeof(HandlingDataBlockAttribute)) != null)
                        {
                            var arrayData = property.GetValue(data);
                            int arrayCount = (int)arrayData.GetType().GetMethod("GetLength").Invoke(arrayData, new object[] { 0 });
                            for (int i = 0; i < arrayCount; ++i)
                            {
                                var value = arrayData.GetType().GetMethod("Get").Invoke(arrayData, new object[] { i });
                                AddDatassFormDataTypeReclucive(list, value, item, bIsReadOnly, $"{prefix}{property.Name}[{i}].");
                            }
                            break;
                        }
                    }
                }
                // 데이터 블록 단일 타입
                else if (EqToEqUiOneDataSet.IsDataBlockType(property.PropertyType) == true)
                {
                    AddDatassFormDataTypeReclucive(list, property.GetValue(data), property.PropertyType, bIsReadOnly, $"{prefix}{property.Name}.");
                }
                else if (property.GetValue(data) is IHaveHandlingDataInformation information)
                {
                    list.Add(new EqToEqUiOneDataSet(information, bIsReadOnly, prefix));
                }
            }
        }
    }
}

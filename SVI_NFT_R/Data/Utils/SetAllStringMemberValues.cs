using System;
using System.Reflection;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        public static void SetAllStringMemberValues(this object o, string value = "")
        {
            Type type = o.GetType();
            foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (field.FieldType == typeof(string))
                {
                    field.SetValue(o, value);
                }
            }
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.CanWrite && property.PropertyType == typeof(string))
                {
                    property.SetValue(o, value);
                }
            }
        }
    }
}

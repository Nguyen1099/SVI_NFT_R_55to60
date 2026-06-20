using System;
using System.Linq;
using System.Reflection;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        /// <summary>
        /// 어셈블리에서 파일 버전을 읽어옴
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>파일 버전</returns>
		public static int[] GetFileVersion(this Assembly assembly)
        {
            AssemblyFileVersionAttribute fileVersionAttr = GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly);
            if (fileVersionAttr == null)
            {
                return new int[0];
            }

            return fileVersionAttr.Version.Split('.')
                .Select(i => Convert.ToInt32(i))
                .ToArray();
        }

        private static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            // Get attributes of this type.
            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

            // If we didn't get anything, return null.
            if (attributes == null
                || attributes.Length == 0
                )
            {
                return null;
            }

            // Convert the first attribute value into
            // the desired type and return it.
            return (T)attributes.First();
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        /// <summary>
        /// 깊은 복사
        /// </summary>
        /// <typeparam name="T">깊은 복사할 객체의 타입</typeparam>
        /// <param name="src">깊은 복사 할 객체</param>
        /// <returns>깊은 복사된 객체</returns>
        public static T DeepClone<T>(this T src)
        {
            Type type = typeof(T);
            if (false == type.IsSerializable)
            {
                Debug.Assert(false, "Serializable Attribute가 선언되지 않은 클래스는 복사 할 수 없습니다. Type:" + type.Name);
                return default(T);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, src);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

}

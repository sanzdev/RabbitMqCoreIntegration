using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Integration.Service.Common
{
    public static class Converter
    {
        /// <summary>
        /// Convert object to byte array
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;

            using (var stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Convert byte array to object
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static object ToObject(this byte[] buffer)
        {
            if (buffer.Length == 0)
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }
    }
}

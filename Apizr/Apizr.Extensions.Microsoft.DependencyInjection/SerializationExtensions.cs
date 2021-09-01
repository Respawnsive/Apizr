using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Apizr
{
    public static class SerializationExtensions
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static T FromByteArray<T>(this byte[] byteArray)
        {
            if (byteArray == null)
            {
                return default;
            }
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(byteArray))
            {
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

    }
}

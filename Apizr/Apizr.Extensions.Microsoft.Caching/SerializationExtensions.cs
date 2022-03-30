using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Apizr
{
    internal static class SerializationExtensions
    {
        internal static byte[] ToByteArray(this object obj)
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

        internal static T FromByteArray<T>(this byte[] byteArray)
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

        internal static async Task<string> ToJsonStringAsync(this object obj, IHttpContentSerializer contentSerializer)
        {
            var httpContent = contentSerializer.ToHttpContent(obj);
            return await httpContent.ReadAsStringAsync();
        }

        internal static async Task<T> FromJsonStringAsync<T>(this string str, IHttpContentSerializer contentSerializer, CancellationToken cancellationToken = default)
        {
            var content = new StringContent(str, Encoding.UTF8, "application/json");
            return await contentSerializer.FromHttpContentAsync<T>(content, cancellationToken);
        }
    }
}

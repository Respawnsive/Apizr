using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Apizr
{
    internal static class SerializationExtensions
    {
        internal static async Task<byte[]> ToByteArrayAsync(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, obj);
            return memoryStream.ToArray();
        }

        internal static async Task<T> FromByteArrayAsync<T>(this byte[] byteArray)
        {
            if (byteArray == null)
            {
                return default;
            }

            using var memoryStream = new MemoryStream(byteArray);
            return await JsonSerializer.DeserializeAsync<T>(memoryStream);
        }

        internal static async Task<string> ToJsonStringAsync(this object obj, IHttpContentSerializer contentSerializer)
        {
            var httpContent = contentSerializer.ToHttpContent(obj);
            return await httpContent.ReadAsStringAsync();
        }

        internal static async Task<T> FromJsonStringAsync<T>(this string str, IHttpContentSerializer contentSerializer, CancellationToken cancellationToken = default)
        {
            if (str == null)
            {
                return default;
            }
            var content = new StringContent(str, Encoding.UTF8, "application/json");
            return await contentSerializer.FromHttpContentAsync<T>(content, cancellationToken);
        }
    }
}

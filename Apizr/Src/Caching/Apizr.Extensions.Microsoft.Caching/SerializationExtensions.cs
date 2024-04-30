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
        internal static async Task<byte[]> ToSerializedByteArrayAsync(this object obj, IHttpContentSerializer contentSerializer)
        {
            if (obj == null)
                return null;

            try
            {
                var httpContent = contentSerializer.ToHttpContent(obj);
                return await httpContent.ReadAsByteArrayAsync();
            }
            catch (System.Exception)
            {
                // only for retro-compatibility
                using var memoryStream = new MemoryStream();
                await JsonSerializer.SerializeAsync(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        internal static async Task<T> FromSerializedByteArrayAsync<T>(this byte[] byteArray, IHttpContentSerializer contentSerializer, CancellationToken cancellationToken = default)
        {
            if (byteArray == null)
                return default;

            try
            {
                var content = new ByteArrayContent(byteArray);
                return await contentSerializer.FromHttpContentAsync<T>(content, cancellationToken);
            }
            catch (System.Exception)
            {
                // only for retro-compatibility
                using var memoryStream = new MemoryStream(byteArray);
                return await JsonSerializer.DeserializeAsync<T>(memoryStream, cancellationToken: cancellationToken);
            }
        }

        internal static async Task<string> ToSerializedStringAsync(this object obj, IHttpContentSerializer contentSerializer)
        {
            if (obj == null)
                return null;

            var httpContent = contentSerializer.ToHttpContent(obj);
            return await httpContent.ReadAsStringAsync();
        }

        internal static async Task<T> FromSerializedStringAsync<T>(this string str, IHttpContentSerializer contentSerializer, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(str))
                return default;

            var content = new StringContent(str);
            return await contentSerializer.FromHttpContentAsync<T>(content, cancellationToken);
        }
    }
}

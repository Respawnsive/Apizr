using Apizr.Configuring.Request;
using Apizr.Logging.Attributes;
using Apizr.Logging;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Apizr.Sample
{
    [BaseAddress("http://speedtest.ftp.otenet.gr/files"), Log(HttpMessageParts.None, LogLevel.None)]
    public interface ITransferService
    {
        [Get("/{fileName}")]
        Task<HttpResponseMessage> DownloadAsync(string fileName, [RequestOptions] IApizrRequestOptions options);

        [Get("/{fileName}")]
        Task<HttpResponseMessage> DownloadAsync(string fileName);
    }
}

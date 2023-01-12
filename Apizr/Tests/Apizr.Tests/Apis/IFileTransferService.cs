using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Tests.Apis
{
    [WebApi("http://speedtest.ftp.otenet.gr/files"), Log(HttpMessageParts.None, LogLevel.None)]
    public interface IFileTransferService
    {
        [Get("/{fileName}")]
        Task<HttpResponseMessage> DownloadAsync(string fileName, [RequestOptions] IApizrRequestOptions options);

        [Get("/{fileName}")]
        Task<HttpResponseMessage> DownloadAsync(string fileName);
    }
}

using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Refit;

namespace Apizr.Tests.Apis
{
    [WebApi("http://speedtest.ftp.otenet.gr/files")]
    public interface IFileTransferService
    {
        [Get("/{fileName}")]
        Task<HttpResponseMessage> DownloadAsync(string fileName, [RequestOptions] IApizrRequestOptions options);
    }
}

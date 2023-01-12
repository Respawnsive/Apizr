using Apizr.Configuring.Request;
using Apizr.Logging.Attributes;
using Apizr.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Apizr.Sample
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

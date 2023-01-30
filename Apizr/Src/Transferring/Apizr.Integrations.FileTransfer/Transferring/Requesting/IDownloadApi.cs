using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Refit;

namespace Apizr.Transferring.Requesting;

public interface IDownloadApi<in TDownloadParams> : ITransferApiBase
{
    [Get("/{filePathOrName}")]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName);

    [Get("/{filePathOrName}")]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, [RequestOptions] IApizrRequestOptions options);

    [Get("/{filePathOrName}")]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, TDownloadParams downloadParams);

    [Get("/{filePathOrName}")]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, TDownloadParams downloadParams, [RequestOptions] IApizrRequestOptions options);
}

public interface IDownloadApi : IDownloadApi<IDictionary<string, object>>
{
}
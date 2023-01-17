using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Refit;

namespace Apizr.Transferring.Requesting;

public interface IDownloadApi<in TDownloadParams> : IDataTransferApi
{
    [Get("/{fileName}")]
    Task<HttpResponseMessage> DownloadAsync(string fileName);

    [Get("/{fileName}")]
    Task<HttpResponseMessage> DownloadAsync(string fileName, [RequestOptions] IApizrRequestOptions options);

    [Get("/{fileName}")]
    Task<HttpResponseMessage> DownloadAsync(string fileName, TDownloadParams downloadParams);

    [Get("/{fileName}")]
    Task<HttpResponseMessage> DownloadAsync(string fileName, TDownloadParams downloadParams, [RequestOptions] IApizrRequestOptions options);
}

public interface IDownloadApi : IDownloadApi<IDictionary<string, object>>
{
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Request;
using Refit;

namespace Apizr.Transferring.Requesting;

public interface IDownloadApi<in TDownloadParams> : ITransferApiBase
{
    [Get("/{filePathOrName}"), QueryUriFormat(UriFormat.Unescaped), Cache(CacheMode.None)]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName);

    [Get("/{filePathOrName}"), QueryUriFormat(UriFormat.Unescaped), Cache(CacheMode.None)]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, [RequestOptions] IApizrRequestOptions options);

    [Get("/{filePathOrName}"), QueryUriFormat(UriFormat.Unescaped), Cache(CacheMode.None)]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, TDownloadParams downloadParams);

    [Get("/{filePathOrName}"), QueryUriFormat(UriFormat.Unescaped), Cache(CacheMode.None)]
    Task<HttpResponseMessage> DownloadAsync(string filePathOrName, TDownloadParams downloadParams, [RequestOptions] IApizrRequestOptions options);
}

public interface IDownloadApi : IDownloadApi<IDictionary<string, object>>
{
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public interface IApizrDownloadManager<TDownloadApi, in TDownloadParams> : IApizrDataTransferManager<TDownloadApi> where TDownloadApi : IDownloadApi<TDownloadParams>
{
    Task<FileInfo> DownloadAsync(FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

    Task<FileInfo> DownloadAsync(FileInfo fileInfo, TDownloadParams downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
}

public interface IApizrDownloadManager<TDownloadApi> : IApizrDownloadManager<TDownloadApi, IDictionary<string, object>> where TDownloadApi : IDownloadApi<IDictionary<string, object>>
{
}
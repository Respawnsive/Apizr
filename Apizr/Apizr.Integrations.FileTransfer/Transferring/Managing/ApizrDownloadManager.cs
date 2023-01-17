using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public class ApizrDownloadManager<TDownloadApi, TDownloadParams> : ApizrDataTransferManager<TDownloadApi>, IApizrDownloadManager<TDownloadApi, TDownloadParams> where TDownloadApi : IDownloadApi<TDownloadParams>
{
    public ApizrDownloadManager(IApizrManager<TDownloadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }

    /// <inheritdoc />
    public async Task<FileInfo> DownloadAsync(FileInfo fileInfo,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
    {
        using var response = await TransferApiManager.ExecuteAsync((opt, api) => api.DownloadAsync(fileInfo.Name, opt), optionsBuilder).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        using var ms = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        using var fs = File.Create(fileInfo.FullName);
        if (ms.CanSeek) ms.Seek(0, SeekOrigin.Begin);
        await ms.CopyToAsync(fs);

        return fileInfo;
    }

    /// <inheritdoc />
    public async Task<FileInfo> DownloadAsync(FileInfo fileInfo, TDownloadParams downloadParams,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
    {
        using var response = await TransferApiManager.ExecuteAsync((opt, api) => api.DownloadAsync(fileInfo.Name, downloadParams, opt), optionsBuilder).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        using var ms = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        using var fs = File.Create(fileInfo.FullName);
        if (ms.CanSeek) ms.Seek(0, SeekOrigin.Begin);
        await ms.CopyToAsync(fs);

        return fileInfo;
    }
}


public class ApizrDownloadManager<TDownloadApi> : ApizrDownloadManager<TDownloadApi, IDictionary<string, object>>, IApizrDownloadManager<TDownloadApi> where TDownloadApi : IDownloadApi<IDictionary<string, object>>
{
    public ApizrDownloadManager(IApizrManager<TDownloadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public class ApizrTransferManager<TTransferApi, TDownloadParams> : IApizrTransferManager<TTransferApi, TDownloadParams> where TTransferApi : ITransferApi<TDownloadParams>
{
    private readonly IApizrDownloadManager<TTransferApi, TDownloadParams> _downloadManager;
    private readonly IApizrUploadManager<TTransferApi> _uploadManager;

    public ApizrTransferManager(IApizrDownloadManager<TTransferApi, TDownloadParams> downloadManager, IApizrUploadManager<TTransferApi> uploadManager)
    {
        _downloadManager = downloadManager;
        _uploadManager = uploadManager;
    }

    /// <inheritdoc />
    public Task<FileInfo> DownloadAsync(FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _downloadManager.DownloadAsync(fileInfo, optionsBuilder);

    /// <inheritdoc />
    public Task<FileInfo> DownloadAsync(FileInfo fileInfo, TDownloadParams downloadParams,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _downloadManager.DownloadAsync(fileInfo, downloadParams, optionsBuilder);

    /// <inheritdoc />
    public Task<FileInfo> UploadAsync(FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _uploadManager.UploadAsync(fileInfo, optionsBuilder);
}

public class ApizrTransferManager<TTransferApi> : ApizrTransferManager<TTransferApi, IDictionary<string, object>>, IApizrTransferManager<TTransferApi> where TTransferApi : ITransferApi
{
    /// <inheritdoc />
    public ApizrTransferManager(IApizrDownloadManager<TTransferApi> downloadManager, IApizrUploadManager<TTransferApi> uploadManager) : base(downloadManager, uploadManager)
    {
    }
}
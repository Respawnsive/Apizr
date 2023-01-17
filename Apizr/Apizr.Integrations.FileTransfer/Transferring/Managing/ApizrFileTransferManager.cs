using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public class ApizrFileTransferManager<TFileTransferApi, TDownloadParams> : IApizrFileTransferManager<TFileTransferApi, TDownloadParams> where TFileTransferApi : IFileTransferApi<TDownloadParams>
{
    private readonly IApizrDownloadManager<TFileTransferApi, TDownloadParams> _downloadManager;
    private readonly IApizrUploadManager<TFileTransferApi> _uploadManager;

    public ApizrFileTransferManager(IApizrDownloadManager<TFileTransferApi, TDownloadParams> downloadManager,IApizrUploadManager<TFileTransferApi> uploadManager)
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

public class ApizrFileTransferManager<TFileTransferApi> : ApizrFileTransferManager<TFileTransferApi, IDictionary<string, object>>, IApizrFileTransferManager<TFileTransferApi> where TFileTransferApi : IFileTransferApi
{
    /// <inheritdoc />
    public ApizrFileTransferManager(IApizrDownloadManager<TFileTransferApi> downloadManager, IApizrUploadManager<TFileTransferApi> uploadManager) : base(downloadManager, uploadManager)
    {
    }
}
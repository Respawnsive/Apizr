using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Transferring.Managing;

public class
    ApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData> : IApizrTransferManager<TTransferApi,
        TDownloadParams, TUploadApiResultData> where TTransferApi : ITransferApi<TDownloadParams, TUploadApiResultData>
{
    private readonly IApizrDownloadManager<TTransferApi, TDownloadParams> _downloadManager;
    private readonly IApizrUploadManager<TTransferApi, TUploadApiResultData> _uploadManager;

    public ApizrTransferManager(IApizrDownloadManager<TTransferApi, TDownloadParams> downloadManager,
        IApizrUploadManager<TTransferApi, TUploadApiResultData> uploadManager)
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
    public Task<TUploadApiResultData> UploadAsync(ByteArrayPart byteArrayPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _uploadManager.UploadAsync(byteArrayPart, optionsBuilder);

    /// <inheritdoc />
    public Task<TUploadApiResultData> UploadAsync(StreamPart streamPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _uploadManager.UploadAsync(streamPart, optionsBuilder);

    /// <inheritdoc />
    public Task<TUploadApiResultData> UploadAsync(FileInfoPart fileInfoPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _uploadManager.UploadAsync(fileInfoPart, optionsBuilder);
}

public class ApizrTransferManager<TTransferApi, TDownloadParams> : IApizrTransferManager<TTransferApi, TDownloadParams> where TTransferApi : ITransferApi<TDownloadParams, HttpResponseMessage>, IUploadApi
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
    public Task<HttpResponseMessage> UploadAsync(ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _uploadManager.UploadAsync(byteArrayPart, optionsBuilder);

    /// <inheritdoc />
    public Task<HttpResponseMessage> UploadAsync(StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _uploadManager.UploadAsync(streamPart, optionsBuilder);

    /// <inheritdoc />
    public Task<HttpResponseMessage> UploadAsync(FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => _uploadManager.UploadAsync(fileInfoPart, optionsBuilder);
}

public class ApizrTransferManager<TTransferApi> : ApizrTransferManager<TTransferApi, IDictionary<string, object>>, IApizrTransferManager<TTransferApi> where TTransferApi : ITransferApi
{
    /// <inheritdoc />
    public ApizrTransferManager(IApizrDownloadManager<TTransferApi> downloadManager, IApizrUploadManager<TTransferApi> uploadManager) : base(downloadManager, uploadManager)
    {
    }
}

public class ApizrTransferManager : ApizrTransferManager<ITransferApi>, IApizrTransferManager
{
    /// <inheritdoc />
    public ApizrTransferManager(IApizrDownloadManager<ITransferApi> downloadManager, IApizrUploadManager<ITransferApi> uploadManager) : base(downloadManager, uploadManager)
    {
    }
}

public class ApizrTransferManagerWith<TDownloadParams, TUploadApiResultData> :
    ApizrTransferManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams, TUploadApiResultData>,
    IApizrTransferManagerWith<TDownloadParams, TUploadApiResultData>
{
    /// <inheritdoc />
    public ApizrTransferManagerWith(
        IApizrDownloadManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams> downloadManager,
        IApizrUploadManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TUploadApiResultData> uploadManager) :
        base(downloadManager, uploadManager)
    {
    }
}
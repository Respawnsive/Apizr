using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Transferring.Requesting;
using Microsoft.Extensions.Options;
using Refit;

namespace Apizr.Transferring.Managing;

public class ApizrUploadManager<TUploadApi, TUploadApiResultData> : ApizrTransferManagerBase<TUploadApi>,
    IApizrUploadManager<TUploadApi, TUploadApiResultData> where TUploadApi : IUploadApi<TUploadApiResultData>
{
    public ApizrUploadManager(IApizrManager<TUploadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }

    /// <inheritdoc />
    public Task<TUploadApiResultData> UploadAsync(ByteArrayPart byteArrayPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => ExecuteUploadAsync(byteArrayPart, optionsBuilder);

    /// <inheritdoc />
    public Task<TUploadApiResultData> UploadAsync(StreamPart streamPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => ExecuteUploadAsync(streamPart, optionsBuilder);

    /// <inheritdoc />
    public Task<TUploadApiResultData> UploadAsync(FileInfoPart fileInfoPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => ExecuteUploadAsync(fileInfoPart, optionsBuilder);

    private Task<TUploadApiResultData> ExecuteUploadAsync<TDataType>(TDataType data, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
    {
        var requestOnlyOptionsBuilder = ApizrManager.CreateRequestOptionsBuilder(TransferApiManager.Options, optionsBuilder);
        var path = requestOnlyOptionsBuilder.ApizrOptions.GetDynamicPathOrDefault();
        return string.IsNullOrWhiteSpace(path)
            ? data switch
            {
                ByteArrayPart byteArrayPart => TransferApiManager.ExecuteAsync((opt, api) => api.UploadAsync(byteArrayPart, opt), optionsBuilder),
                StreamPart streamPart => TransferApiManager.ExecuteAsync((opt, api) => api.UploadAsync(streamPart, opt), optionsBuilder),
                FileInfoPart fileInfoPart => TransferApiManager.ExecuteAsync((opt, api) => api.UploadAsync(fileInfoPart, opt), optionsBuilder),
                _ => throw new NotSupportedException($"Data type {data.GetType().Name} is not supported")
            }
            : data switch
            {
                ByteArrayPart byteArrayPart => TransferApiManager.ExecuteAsync((opt, api) => api.UploadAsync(byteArrayPart, path, opt), optionsBuilder),
                StreamPart streamPart => TransferApiManager.ExecuteAsync((opt, api) => api.UploadAsync(streamPart, path, opt), optionsBuilder),
                FileInfoPart fileInfoPart => TransferApiManager.ExecuteAsync((opt, api) => api.UploadAsync(fileInfoPart, path, opt), optionsBuilder),
                _ => throw new NotSupportedException($"Data type {data.GetType().Name} is not supported")
            };
    }
}

public class ApizrUploadManager<TUploadApi> : ApizrUploadManager<TUploadApi, HttpResponseMessage>, IApizrUploadManager<TUploadApi> where TUploadApi : IUploadApi
{
    /// <inheritdoc />
    public ApizrUploadManager(IApizrManager<TUploadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }
}

public class ApizrUploadManager : ApizrUploadManager<IUploadApi>, IApizrUploadManager
{
    /// <inheritdoc />
    public ApizrUploadManager(IApizrManager<IUploadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }
}

public class ApizrUploadManagerWith<TUploadApiResultData> : ApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData>, IApizrUploadManagerWith<TUploadApiResultData>
{
    /// <inheritdoc />
    public ApizrUploadManagerWith(IApizrManager<IUploadApi<TUploadApiResultData>> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }
}
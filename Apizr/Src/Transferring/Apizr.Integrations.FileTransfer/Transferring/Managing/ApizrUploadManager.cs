using System;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Transferring.Managing;

public class ApizrUploadManager<TUploadApi, TApiResultData> : ApizrTransferManagerBase<TUploadApi>,
    IApizrUploadManager<TUploadApi, TApiResultData> where TUploadApi : IUploadApi<TApiResultData>
{
    public ApizrUploadManager(IApizrManager<TUploadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }

    /// <inheritdoc />
    public Task<TApiResultData> UploadAsync(ByteArrayPart byteArrayPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, byteArrayPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    /// <inheritdoc />
    public Task<TApiResultData> UploadAsync(StreamPart streamPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, streamPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    /// <inheritdoc />
    public Task<TApiResultData> UploadAsync(FileInfoPart fileInfoPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, fileInfoPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    private static Task<TApiResultData> UploadAsync<TDataType>(TUploadApi api, TDataType data, string path,
        IApizrRequestOptions options)
    {
        return string.IsNullOrWhiteSpace(path)
            ? data switch
            {
                ByteArrayPart byteArrayPart => api.UploadAsync(byteArrayPart, options),
                StreamPart streamPart => api.UploadAsync(streamPart, options),
                FileInfoPart fileInfoPart => api.UploadAsync(fileInfoPart, options),
                _ => throw new NotSupportedException($"Data type {data.GetType().Name} is not supported")
            }
            : data switch
            {
                ByteArrayPart byteArrayPart => api.UploadAsync(byteArrayPart, options.GetDynamicPathOrDefault(), options),
                StreamPart streamPart => api.UploadAsync(streamPart, options.GetDynamicPathOrDefault(), options),
                FileInfoPart fileInfoPart => api.UploadAsync(fileInfoPart, options.GetDynamicPathOrDefault(), options),
                _ => throw new NotSupportedException($"Data type {data.GetType().Name} is not supported")
            };
    }
}

public class ApizrUploadManager<TUploadApi> : ApizrTransferManagerBase<TUploadApi>, IApizrUploadManager<TUploadApi> where TUploadApi : IUploadApi
{
    public ApizrUploadManager(IApizrManager<TUploadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }

    /// <inheritdoc />
    public Task UploadAsync(ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, byteArrayPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    /// <inheritdoc />
    public Task UploadAsync(StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, streamPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    /// <inheritdoc />
    public Task UploadAsync(FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, fileInfoPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    private static Task UploadAsync<TDataType>(TUploadApi api, TDataType data, string path, IApizrRequestOptions options)
    {
        return string.IsNullOrWhiteSpace(path)
            ? data switch
            {
                ByteArrayPart byteArrayPart => api.UploadAsync(byteArrayPart, options),
                StreamPart streamPart => api.UploadAsync(streamPart, options),
                FileInfoPart fileInfoPart => api.UploadAsync(fileInfoPart, options),
                _ => throw new NotSupportedException($"Data type {data.GetType().Name} is not supported")
            }
            : data switch
            {
                ByteArrayPart byteArrayPart => api.UploadAsync(byteArrayPart, options.GetDynamicPathOrDefault(), options),
                StreamPart streamPart => api.UploadAsync(streamPart, options.GetDynamicPathOrDefault(), options),
                FileInfoPart fileInfoPart => api.UploadAsync(fileInfoPart, options.GetDynamicPathOrDefault(), options),
                _ => throw new NotSupportedException($"Data type {data.GetType().Name} is not supported")
            };
    }
}

public class ApizrUploadManager : ApizrUploadManager<IUploadApi>, IApizrUploadManager
{
    /// <inheritdoc />
    public ApizrUploadManager(IApizrManager<IUploadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }
}
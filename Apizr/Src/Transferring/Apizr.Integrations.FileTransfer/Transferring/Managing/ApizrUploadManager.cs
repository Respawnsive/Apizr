using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Transferring.Requesting;
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
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, byteArrayPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    /// <inheritdoc />
    public Task<TUploadApiResultData> UploadAsync(StreamPart streamPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, streamPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    /// <inheritdoc />
    public Task<TUploadApiResultData> UploadAsync(FileInfoPart fileInfoPart,
        Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => UploadAsync(api, fileInfoPart, opt.GetDynamicPathOrDefault(), opt), optionsBuilder);

    private static Task<TUploadApiResultData> UploadAsync<TDataType>(TUploadApi api, TDataType data, string path,
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
                ByteArrayPart byteArrayPart => api.UploadAsync(byteArrayPart, options.GetDynamicPathOrDefault(),
                    options),
                StreamPart streamPart => api.UploadAsync(streamPart, options.GetDynamicPathOrDefault(), options),
                FileInfoPart fileInfoPart => api.UploadAsync(fileInfoPart, options.GetDynamicPathOrDefault(), options),
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
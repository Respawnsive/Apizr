using System;
using System.IO;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Transferring.Managing;

public class ApizrUploadManager<TUploadApi> : ApizrTransferManagerBase<TUploadApi>, IApizrUploadManager<TUploadApi> where TUploadApi : IUploadApi
{
    public ApizrUploadManager(IApizrManager<TUploadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }

    /// <inheritdoc />
    public Task UploadAsync(ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => api.UploadAsync(byteArrayPart, opt.GetEndingPathOrDefault(),
                opt), optionsBuilder);

    /// <inheritdoc />
    public Task UploadAsync(StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => api.UploadAsync(streamPart, opt.GetEndingPathOrDefault(),
                opt), optionsBuilder);

    /// <inheritdoc />
    public Task UploadAsync(FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        => TransferApiManager.ExecuteAsync(
            (opt, api) => api.UploadAsync(fileInfoPart, opt.GetEndingPathOrDefault(),
                opt), optionsBuilder);
}
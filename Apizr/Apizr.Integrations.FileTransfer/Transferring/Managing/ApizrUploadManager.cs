using System;
using System.IO;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public class ApizrUploadManager<TUploadApi> : ApizrDataTransferManager<TUploadApi>, IApizrUploadManager<TUploadApi> where TUploadApi : IUploadApi
{
    public ApizrUploadManager(IApizrManager<TUploadApi> fileTransferApiManager) : base(fileTransferApiManager)
    {
    }

    /// <inheritdoc />
    public Task<FileInfo> UploadAsync(FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
    {
        throw new NotImplementedException();
    }
}
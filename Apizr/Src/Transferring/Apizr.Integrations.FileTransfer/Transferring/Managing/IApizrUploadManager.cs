using System;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Transferring.Managing;

public interface IApizrUploadManager<TUploadApi> : IApizrTransferManagerBase<TUploadApi> where TUploadApi : IUploadApi
{
    Task UploadAsync(ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
    Task UploadAsync(StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
    Task UploadAsync(FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
}
using System;
using System.IO;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public interface IApizrUploadManager<TUploadApi> : IApizrDataTransferManager<TUploadApi> where TUploadApi : IUploadApi
{
    Task<FileInfo> UploadAsync(FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
}
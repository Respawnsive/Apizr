using System.Collections.Generic;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public interface IApizrFileTransferManager<TFileTransferApi, in TDownloadParams> : IApizrDownloadManager<TFileTransferApi, TDownloadParams>, IApizrUploadManager<TFileTransferApi> where TFileTransferApi : IFileTransferApi<TDownloadParams>
{

}

public interface IApizrFileTransferManager<TFileTransferApi> : IApizrFileTransferManager<TFileTransferApi, IDictionary<string, object>> where TFileTransferApi : IFileTransferApi<IDictionary<string, object>>
{

}
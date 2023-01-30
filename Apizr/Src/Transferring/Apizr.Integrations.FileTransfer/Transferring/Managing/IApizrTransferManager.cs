using System.Collections.Generic;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public interface IApizrTransferManager<TTransferApi, in TDownloadParams> : IApizrDownloadManager<TTransferApi, TDownloadParams>, IApizrUploadManager<TTransferApi> where TTransferApi : ITransferApi<TDownloadParams>
{

}

public interface IApizrTransferManager<TTransferApi> : IApizrTransferManager<TTransferApi, IDictionary<string, object>>, IApizrDownloadManager<TTransferApi> where TTransferApi : ITransferApi<IDictionary<string, object>>
{

}
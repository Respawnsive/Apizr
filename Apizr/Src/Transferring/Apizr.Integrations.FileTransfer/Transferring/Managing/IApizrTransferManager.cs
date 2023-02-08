using System.Collections.Generic;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

/// <summary>
/// The transfer manager to work with both downloads and uploads and with a custom query parameters type
/// </summary>
/// <typeparam name="TTransferApi">The transfer api type to manage</typeparam>
/// <typeparam name="TDownloadParams">The custom query parameters type</typeparam>
public interface IApizrTransferManager<TTransferApi, in TDownloadParams> : IApizrDownloadManager<TTransferApi, TDownloadParams>, IApizrUploadManager<TTransferApi> where TTransferApi : ITransferApi<TDownloadParams>
{

}

/// <summary>
/// The transfer manager to work with both downloads and uploads and with a dictionary query parameters type
/// </summary>
/// <typeparam name="TTransferApi">The transfer api type to manage</typeparam>
public interface IApizrTransferManager<TTransferApi> : IApizrTransferManager<TTransferApi, IDictionary<string, object>>, IApizrDownloadManager<TTransferApi> where TTransferApi : ITransferApi<IDictionary<string, object>>
{

}

public interface IApizrTransferManager : IApizrTransferManager<ITransferApi>
{}
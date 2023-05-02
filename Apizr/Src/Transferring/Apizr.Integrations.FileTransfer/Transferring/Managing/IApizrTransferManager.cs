using System.Collections.Generic;
using System.Net.Http;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

/// <summary>
/// The transfer manager to work with both downloads and uploads and with both a custom download query parameters type and upload result type
/// </summary>
/// <typeparam name="TTransferApi">The transfer api type to manage</typeparam>
/// <typeparam name="TDownloadParams">The custom query parameters type</typeparam>
/// <typeparam name="TUploadApiResultData">The transfer api upload result type</typeparam>
public interface IApizrTransferManager<TTransferApi, in TDownloadParams, TUploadApiResultData> : IApizrDownloadManager<TTransferApi, TDownloadParams>, IApizrUploadManager<TTransferApi, TUploadApiResultData> where TTransferApi : ITransferApi<TDownloadParams, TUploadApiResultData>
{

}

/// <summary>
/// The transfer manager to work with both downloads and uploads and with a custom download query parameters type and no result
/// </summary>
/// <typeparam name="TTransferApi">The transfer api type to manage</typeparam>
/// <typeparam name="TDownloadParams">The custom query parameters type</typeparam>
public interface IApizrTransferManager<TTransferApi, in TDownloadParams> : IApizrDownloadManager<TTransferApi, TDownloadParams>, IApizrUploadManager<TTransferApi> where TTransferApi : ITransferApi<TDownloadParams>
{

}

/// <summary>
/// The transfer manager to work with both downloads and uploads and with a dictionary query parameters type and no result
/// </summary>
/// <typeparam name="TTransferApi">The transfer api type to manage</typeparam>
public interface IApizrTransferManager<TTransferApi> : IApizrDownloadManager<TTransferApi>, IApizrUploadManager<TTransferApi> where TTransferApi : ITransferApi
{

}

/// <summary>
/// The transfer manager to work with both downloads and uploads and with a dictionary query parameters type and no result
/// </summary>
public interface IApizrTransferManager : IApizrDownloadManager, IApizrUploadManager
{ }
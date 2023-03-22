using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

/// <summary>
/// The download manager with a custom query parameters type
/// </summary>
/// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
/// <typeparam name="TDownloadParams">The query parameters type</typeparam>
public interface IApizrDownloadManager<TDownloadApi, in TDownloadParams> : IApizrTransferManagerBase<TDownloadApi> where TDownloadApi : IDownloadApi<TDownloadParams>
{
    /// <summary>
    /// Download a file
    /// </summary>
    /// <param name="fileInfo">Some information about the file to download</param>
    /// <param name="optionsBuilder">Some request options</param>
    /// <returns></returns>
    Task<FileInfo> DownloadAsync(FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

    /// <summary>
    /// Download a file with custom query parameters
    /// </summary>
    /// <param name="fileInfo">Some information about the file to download</param>
    /// <param name="downloadParams">Some custom query parameters</param>
    /// <param name="optionsBuilder">Some request options</param>
    /// <returns></returns>
    Task<FileInfo> DownloadAsync(FileInfo fileInfo, TDownloadParams downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
}

/// <summary>
/// The download manager with a dictionary query parameters type
/// </summary>
/// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
public interface IApizrDownloadManager<TDownloadApi> : IApizrDownloadManager<TDownloadApi, IDictionary<string, object>> where TDownloadApi : IDownloadApi<IDictionary<string, object>>
{
}

/// <summary>
/// The download manager with a dictionary query parameters type
/// </summary>
public interface IApizrDownloadManager : IApizrDownloadManager<IDownloadApi>
{
}
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Transferring.Managing;

/// <summary>
/// The upload manager
/// </summary>
/// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
/// <typeparam name="TUploadApiResultData">The upload api result type</typeparam>
public interface IApizrUploadManager<TUploadApi, TUploadApiResultData> : IApizrTransferManagerBase<TUploadApi> where TUploadApi : IUploadApi<TUploadApiResultData>
{
    /// <summary>
    /// Upload a file from its bytes data
    /// </summary>
    /// <param name="byteArrayPart">The file bytes data</param>
    /// <param name="optionsBuilder">Some request options</param>
    /// <returns></returns>
    Task<TUploadApiResultData> UploadAsync(ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

    /// <summary>
    /// Upload a file from its stream data
    /// </summary>
    /// <param name="streamPart">The file stream data</param>
    /// <param name="optionsBuilder">Some request options</param>
    /// <returns></returns>
    Task<TUploadApiResultData> UploadAsync(StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

    /// <summary>
    /// Upload a file from its file info data
    /// </summary>
    /// <param name="fileInfoPart">The file info data</param>
    /// <param name="optionsBuilder">Some request options</param>
    /// <returns></returns>
    Task<TUploadApiResultData> UploadAsync(FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
}

/// <summary>
/// The upload manager
/// </summary>
/// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
public interface IApizrUploadManager<TUploadApi> : IApizrUploadManager<TUploadApi, HttpResponseMessage> where TUploadApi : IUploadApi
{}

/// <summary>
/// The upload manager
/// </summary>
public interface IApizrUploadManager : IApizrUploadManager<IUploadApi>
{}
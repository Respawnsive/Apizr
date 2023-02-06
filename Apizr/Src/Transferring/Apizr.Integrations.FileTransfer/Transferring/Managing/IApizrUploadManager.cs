using System;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Transferring.Managing;

/// <summary>
/// The upload manager
/// </summary>
/// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
public interface IApizrUploadManager<TUploadApi> : IApizrTransferManagerBase<TUploadApi> where TUploadApi : IUploadApi
{
    /// <summary>
    /// Upload a file from its bytes data
    /// </summary>
    /// <param name="byteArrayPart">The file bytes data</param>
    /// <param name="optionsBuilder">Some request options</param>
    /// <returns></returns>
    Task UploadAsync(ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

    /// <summary>
    /// Upload a file from its stream data
    /// </summary>
    /// <param name="streamPart">The file stream data</param>
    /// <param name="optionsBuilder">Some request options</param>
    /// <returns></returns>
    Task UploadAsync(StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);

    /// <summary>
    /// Upload a file from its file info data
    /// </summary>
    /// <param name="fileInfoPart">The file info data</param>
    /// <param name="optionsBuilder">Some request options</param>
    /// <returns></returns>
    Task UploadAsync(FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null);
}
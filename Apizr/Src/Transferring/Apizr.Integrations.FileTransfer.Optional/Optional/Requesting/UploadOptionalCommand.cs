using System;
using Apizr.Configuring.Request;
using Apizr.Mediation.Commanding;
using Apizr.Transferring.Requesting;
using MediatR;
using Optional;
using Refit;

namespace Apizr.Optional.Requesting
{
    /// <summary>
    /// The mediation upload command returning an optional result
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
    public class UploadOptionalCommand<TUploadApi> : MediationCommandBase<Unit, Option<Unit, ApizrException>, IApizrRequestOptions, IApizrRequestOptionsBuilder> where TUploadApi : IUploadApi
    {
        /// <summary>
        /// Upload a file from its bytes data
        /// </summary>
        /// <param name="byteArrayPart">The file bytes data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public UploadOptionalCommand(ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            ByteArrayPart = byteArrayPart;
        }

        /// <summary>
        /// Upload a file from its stream data
        /// </summary>
        /// <param name="streamPart">The file stream data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public UploadOptionalCommand(StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            StreamPart = streamPart;
        }

        /// <summary>
        /// Upload a file from its file info data
        /// </summary>
        /// <param name="fileInfoPart">The file info data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public UploadOptionalCommand(FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            FileInfoPart = fileInfoPart;
        }

        /// <summary>
        /// The file info data
        /// </summary>
        public FileInfoPart FileInfoPart { get; }

        /// <summary>
        /// The file stream data
        /// </summary>
        public StreamPart StreamPart { get; }

        /// <summary>
        /// The file bytes data
        /// </summary>
        public ByteArrayPart ByteArrayPart { get; }
    }

    /// <summary>
    /// The mediation upload command
    /// </summary>
    public class UploadOptionalCommand : UploadOptionalCommand<IUploadApi>
    {
        /// <inheritdoc />
        public UploadOptionalCommand(ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(byteArrayPart, optionsBuilder)
        {
        }

        /// <inheritdoc />
        public UploadOptionalCommand(StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(streamPart, optionsBuilder)
        {
        }

        /// <inheritdoc />
        public UploadOptionalCommand(FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(fileInfoPart, optionsBuilder)
        {
        }
    }
}

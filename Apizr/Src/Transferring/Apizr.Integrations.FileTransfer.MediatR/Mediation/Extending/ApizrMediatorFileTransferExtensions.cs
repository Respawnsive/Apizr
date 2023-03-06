using Apizr.Mediation.Requesting.Sending;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting;
using Apizr.Transferring.Requesting;
using Apizr.Configuring.Request;
using System.IO;
using Refit;

namespace Apizr.Mediation.Extending
{
    public static class ApizrMediatorFileTransferExtensions
    {
        #region Download

        /// <summary>
        /// Send a download query to Apizr using MediatR for <typeparamref name="TDownloadApi"/> with a custom query parameters type
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api type</typeparam>
        /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some custom query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> SendDownloadQueryFor<TDownloadApi, TDownloadParams>(this IApizrMediator apizrMediator,
            FileInfo fileInfo,
            TDownloadParams downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi<TDownloadParams> =>
            ((IApizrInternalMediator) apizrMediator).Send(
                new DownloadQuery<TDownloadApi, TDownloadParams>(fileInfo, downloadParams, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a download query to Apizr using MediatR for <typeparamref name="TDownloadApi"/>
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api type</typeparam>
        /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> SendDownloadQueryFor<TDownloadApi, TDownloadParams>(this IApizrMediator apizrMediator,
            FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi<TDownloadParams> =>
            ((IApizrInternalMediator) apizrMediator).Send(
                new DownloadQuery<TDownloadApi, TDownloadParams>(fileInfo, default, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a download query to Apizr using MediatR for <typeparamref name="TDownloadApi"/> with a dictionary query parameters type
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api type</typeparam>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some custom query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> SendDownloadQueryFor<TDownloadApi>(this IApizrMediator apizrMediator, FileInfo fileInfo,
            IDictionary<string, object> downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi =>
            ((IApizrInternalMediator) apizrMediator).Send(
                new DownloadQuery<TDownloadApi>(fileInfo, downloadParams, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a download query to Apizr using MediatR for <typeparamref name="TDownloadApi"/>
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api type</typeparam>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> SendDownloadQueryFor<TDownloadApi>(this IApizrMediator apizrMediator, FileInfo fileInfo,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi =>
            ((IApizrInternalMediator) apizrMediator).Send(
                new DownloadQuery<TDownloadApi>(fileInfo, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a download query to Apizr using MediatR for <see cref="IDownloadApi"/> with a dictionary query parameters type
        /// </summary>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some custom query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> SendDownloadQuery(this IApizrMediator apizrMediator, FileInfo fileInfo,
            IDictionary<string, object> downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            ((IApizrInternalMediator) apizrMediator).Send(
                new DownloadQuery(fileInfo, downloadParams, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a download query to Apizr using MediatR for <see cref="IDownloadApi"/>
        /// </summary>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> SendDownloadQuery(this IApizrMediator apizrMediator, FileInfo fileInfo,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            ((IApizrInternalMediator) apizrMediator).Send(
                new DownloadQuery(fileInfo, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion

        #region Upload

        /// <summary>
        /// Send a upload command to Apizr using MediatR for <typeparamref name="TUploadApi"/> from file bytes data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type</typeparam>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="byteArrayPart">The file bytes data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task SendUploadCommandFor<TUploadApi>(this IApizrMediator apizrMediator,
            ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TUploadApi : IUploadApi =>
            ((IApizrInternalMediator)apizrMediator).Send(
                new UploadCommand<TUploadApi>(byteArrayPart, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a upload command to Apizr using MediatR for <typeparamref name="TUploadApi"/> from file stream data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type</typeparam>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="streamPart">The file stream data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task SendUploadCommandFor<TUploadApi>(this IApizrMediator apizrMediator,
            StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TUploadApi : IUploadApi =>
            ((IApizrInternalMediator)apizrMediator).Send(
                new UploadCommand<TUploadApi>(streamPart, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a upload command to Apizr using MediatR for <typeparamref name="TUploadApi"/> from file info data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type</typeparam>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="fileInfoPart">The file info data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task SendUploadCommandFor<TUploadApi>(this IApizrMediator apizrMediator,
            FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TUploadApi : IUploadApi =>
            ((IApizrInternalMediator)apizrMediator).Send(
                new UploadCommand<TUploadApi>(fileInfoPart, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a upload command to Apizr using MediatR for <see cref="IUploadApi"/> from file bytes data
        /// </summary>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="byteArrayPart">The file bytes data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task SendUploadCommand(this IApizrMediator apizrMediator,
            ByteArrayPart byteArrayPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            ((IApizrInternalMediator)apizrMediator).Send(
                new UploadCommand(byteArrayPart, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a upload command to Apizr using MediatR for <see cref="IUploadApi"/> from file stream data
        /// </summary>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="streamPart">The file stream data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task SendUploadCommand(this IApizrMediator apizrMediator,
            StreamPart streamPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            ((IApizrInternalMediator)apizrMediator).Send(
                new UploadCommand(streamPart, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        /// <summary>
        /// Send a upload command to Apizr using MediatR for <see cref="IUploadApi"/> from file info data
        /// </summary>
        /// <param name="apizrMediator">The extended mediator</param>
        /// <param name="fileInfoPart">The file info data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task SendUploadCommand(this IApizrMediator apizrMediator,
            FileInfoPart fileInfoPart, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            ((IApizrInternalMediator)apizrMediator).Send(
                new UploadCommand(fileInfoPart, optionsBuilder),
                ApizrManager.CreateRequestOptionsBuilder(optionsBuilder).ApizrOptions.CancellationToken);

        #endregion
    }
}

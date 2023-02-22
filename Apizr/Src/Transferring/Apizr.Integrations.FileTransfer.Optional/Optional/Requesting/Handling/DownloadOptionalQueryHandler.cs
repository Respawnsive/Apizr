using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
using MediatR;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Requesting.Handling
{
    /// <summary>
    /// The mediation handler for <see cref="DownloadOptionalQuery{TDownloadApi, TDownloadParams}"/>
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
    /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
    public class DownloadOptionalQueryHandler<TDownloadApi, TDownloadParams> :
        RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<DownloadOptionalQuery<TDownloadApi, TDownloadParams>, Option<FileInfo, ApizrException>>
        where TDownloadApi : IDownloadApi<TDownloadParams>
    {
        private readonly IApizrDownloadManager<TDownloadApi, TDownloadParams> _downloadManager;

        public DownloadOptionalQueryHandler(IApizrDownloadManager<TDownloadApi, TDownloadParams> downloadManager)
        {
            _downloadManager = downloadManager;
        }

        /// <inheritdoc />
        public async Task<Option<FileInfo, ApizrException>> Handle(
            DownloadOptionalQuery<TDownloadApi, TDownloadParams> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        _downloadManager.DownloadAsync(request.FileInfo, request.DownloadParams, request.OptionsBuilder))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<FileInfo, ApizrException>(e);
            }
        }
    }

    /// <summary>
    /// The mediation handler for <see cref="DownloadOptionalQuery{TDownloadApi}"/>
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
    public class DownloadOptionalQueryHandler<TDownloadApi> :
        RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<DownloadOptionalQuery<TDownloadApi>, Option<FileInfo, ApizrException>>,
        IRequestHandler<DownloadOptionalQuery, Option<FileInfo, ApizrException>> 
        where TDownloadApi : IDownloadApi
    {
        private readonly IApizrDownloadManager<TDownloadApi> _downloadManager;

        public DownloadOptionalQueryHandler(IApizrDownloadManager<TDownloadApi> downloadManager)
        {
            _downloadManager = downloadManager;
        }
        
        /// <inheritdoc />
        public async Task<Option<FileInfo, ApizrException>> Handle(DownloadOptionalQuery<TDownloadApi> request, CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        _downloadManager.DownloadAsync(request.FileInfo, request.DownloadParams, request.OptionsBuilder))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<FileInfo, ApizrException>(e);
            }
        }

        /// <inheritdoc />
        public async Task<Option<FileInfo, ApizrException>> Handle(DownloadOptionalQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        _downloadManager.DownloadAsync(request.FileInfo, request.DownloadParams, request.OptionsBuilder))
                    .ConfigureAwait(false);
            }
            catch (ApizrException e)
            {
                return Option.None<FileInfo, ApizrException>(e);
            }
        }
    }
}

using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Handling.Base;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Mediation.Requesting.Handling
{
    /// <summary>
    /// The mediation handler for <see cref="DownloadQuery{TDownloadApi, TDownloadParams}"/>
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
    /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
    public class DownloadQueryHandler<TDownloadApi, TDownloadParams> :
        RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<DownloadQuery<TDownloadApi, TDownloadParams>, FileInfo>
        where TDownloadApi : IDownloadApi<TDownloadParams>
    {
        private readonly IApizrDownloadManager<TDownloadApi, TDownloadParams> _downloadManager;

        public DownloadQueryHandler(IApizrDownloadManager<TDownloadApi, TDownloadParams> downloadManager)
        {
            _downloadManager = downloadManager;
        }

        /// <summary>
        /// Handling the download request
        /// </summary>
        /// <param name="request">The download request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public Task<FileInfo> Handle(DownloadQuery<TDownloadApi, TDownloadParams> request,
            CancellationToken cancellationToken) =>
            _downloadManager.DownloadAsync(request.FileInfo, request.DownloadParams, request.OptionsBuilder);
    }

    /// <summary>
    /// The mediation handler for <see cref="DownloadQuery{TDownloadApi}"/>
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
    public class DownloadQueryHandler<TDownloadApi> :
        RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<DownloadQuery<TDownloadApi>, FileInfo>,
        IRequestHandler<DownloadQuery, FileInfo> 
        where TDownloadApi : IDownloadApi
    {
        private readonly IApizrDownloadManager<TDownloadApi> _downloadManager;

        public DownloadQueryHandler(IApizrDownloadManager<TDownloadApi> downloadManager)
        {
            _downloadManager = downloadManager;
        }

        /// <summary>
        /// Handling the download request
        /// </summary>
        /// <param name="request">The download request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public Task<FileInfo> Handle(DownloadQuery<TDownloadApi> request,
            CancellationToken cancellationToken) =>
            _downloadManager.DownloadAsync(request.FileInfo, request.DownloadParams, request.OptionsBuilder);

        /// <summary>
        /// Handling the download request
        /// </summary>
        /// <param name="request">The download request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public Task<FileInfo> Handle(DownloadQuery request, CancellationToken cancellationToken) =>
            _downloadManager.DownloadAsync(request.FileInfo, request.DownloadParams, request.OptionsBuilder);
    }
}

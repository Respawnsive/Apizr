using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling
{
    public class UploadCommandHandler<TUploadApi, TUploadApiResultData> : RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<UploadCommand<TUploadApi, TUploadApiResultData>, TUploadApiResultData>
        where TUploadApi : IUploadApi<TUploadApiResultData>
    {
        private readonly IApizrUploadManager<TUploadApi, TUploadApiResultData> _uploadManager;

        public UploadCommandHandler(IApizrUploadManager<TUploadApi, TUploadApiResultData> uploadManager)
        {
            _uploadManager = uploadManager;
        }

        /// <summary>
        /// Handling the upload request
        /// </summary>
        /// <param name="request">The upload request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public Task<TUploadApiResultData> Handle(UploadCommand<TUploadApi, TUploadApiResultData> request, CancellationToken cancellationToken)
        {
            if (request.ByteArrayPart != null)
                return _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder);
            if (request.StreamPart != null)
                return _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder);
            if (request.FileInfoPart != null)
                return _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder);

            throw new NullReferenceException("You must provide some data to upload");
        }
    }

    public class UploadCommandHandler<TUploadApi> : RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<UploadCommand<TUploadApi>, HttpResponseMessage>,
        IRequestHandler<UploadCommand, HttpResponseMessage> 
        where TUploadApi : IUploadApi
    {
        private readonly IApizrUploadManager<TUploadApi> _uploadManager;

        public UploadCommandHandler(IApizrUploadManager<TUploadApi> uploadManager)
        {
            _uploadManager = uploadManager;
        }

        /// <summary>
        /// Handling the upload request
        /// </summary>
        /// <param name="request">The upload request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Handle(UploadCommand<TUploadApi> request, CancellationToken cancellationToken)
        {
            if(request.ByteArrayPart != null)
                return _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder);
            if (request.StreamPart != null)
                return _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder);
            if (request.FileInfoPart != null)
                return _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder);

            throw new NullReferenceException("You must provide some data to upload");
        }

        /// <summary>
        /// Handling the upload request
        /// </summary>
        /// <param name="request">The upload request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Handle(UploadCommand request, CancellationToken cancellationToken)
        {
            if (request.ByteArrayPart != null)
                return _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder);
            if (request.StreamPart != null)
                return _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder);
            if (request.FileInfoPart != null)
                return _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder);

            throw new NullReferenceException("You must provide some data to upload");
        }
    }

    public class UploadWithCommandHandler<TUploadApiResultData> : RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<UploadWithCommand<TUploadApiResultData>, TUploadApiResultData>
    {
        private readonly IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData> _uploadManager;

        public UploadWithCommandHandler(IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData> uploadManager)
        {
            _uploadManager = uploadManager;
        }

        /// <summary>
        /// Handling the upload request
        /// </summary>
        /// <param name="request">The upload request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public Task<TUploadApiResultData> Handle(UploadWithCommand<TUploadApiResultData> request, CancellationToken cancellationToken)
        {
            if (request.ByteArrayPart != null)
                return _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder);
            if (request.StreamPart != null)
                return _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder);
            if (request.FileInfoPart != null)
                return _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder);

            throw new NullReferenceException("You must provide some data to upload");
        }
    }
}

using System;
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
    public class UploadOptionalCommandHandler<TUploadApi> : RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<UploadOptionalCommand<TUploadApi>, Option<Unit, ApizrException>>,
        IRequestHandler<UploadOptionalCommand, Option<Unit, ApizrException>> 
        where TUploadApi : IUploadApi
    {
        private readonly IApizrUploadManager<TUploadApi> _uploadManager;

        public UploadOptionalCommandHandler(IApizrUploadManager<TUploadApi> uploadManager)
        {
            _uploadManager = uploadManager;
        }

        /// <summary>
        /// Handling the upload optional request
        /// </summary>
        /// <param name="request">The upload optional request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public async Task<Option<Unit, ApizrException>> Handle(UploadOptionalCommand<TUploadApi> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ByteArrayPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder)
                                .ContinueWith(_ => Unit.Value, cancellationToken)).ConfigureAwait(false);
                if (request.StreamPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder)
                                .ContinueWith(_ => Unit.Value, cancellationToken)).ConfigureAwait(false);
                if (request.FileInfoPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder)
                                .ContinueWith(_ => Unit.Value, cancellationToken)).ConfigureAwait(false);

                throw new ApizrException(new NotImplementedException());
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }

        /// <summary>
        /// Handling the upload optional request
        /// </summary>
        /// <param name="request">The upload optional request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public async Task<Option<Unit, ApizrException>> Handle(UploadOptionalCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.ByteArrayPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder)
                                .ContinueWith(_ => Unit.Value, cancellationToken)).ConfigureAwait(false);
                if (request.StreamPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder)
                                .ContinueWith(_ => Unit.Value, cancellationToken)).ConfigureAwait(false);
                if (request.FileInfoPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder)
                                .ContinueWith(_ => Unit.Value, cancellationToken)).ConfigureAwait(false);

                throw new ApizrException(new NotImplementedException());
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }
}

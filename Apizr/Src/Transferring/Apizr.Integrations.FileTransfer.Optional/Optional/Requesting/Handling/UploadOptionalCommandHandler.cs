using System;
using System.Net.Http;
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
    /// The mediation handler for <see cref="UploadOptionalCommand{TUploadApi, TUploadApiResultData}"/>
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
    /// <typeparam name="TUploadApiResultData">The upload result type</typeparam>
    public class UploadOptionalCommandHandler<TUploadApi, TUploadApiResultData> : RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<UploadOptionalCommand<TUploadApi, TUploadApiResultData>, Option<TUploadApiResultData, ApizrException>>
        where TUploadApi : IUploadApi<TUploadApiResultData>
    {
        private readonly IApizrUploadManager<TUploadApi, TUploadApiResultData> _uploadManager;

        /// <summary>
        /// The mediation handler for <see cref="UploadOptionalCommand{TUploadApi, TUploadApiResultData}"/>
        /// </summary>
        /// <param name="uploadManager">The upload manager</param>
        public UploadOptionalCommandHandler(IApizrUploadManager<TUploadApi, TUploadApiResultData> uploadManager)
        {
            _uploadManager = uploadManager;
        }

        /// <summary>
        /// Handling the upload optional request
        /// </summary>
        /// <param name="request">The upload optional request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public async Task<Option<TUploadApiResultData, ApizrException>> Handle(UploadOptionalCommand<TUploadApi, TUploadApiResultData> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ByteArrayPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder))
                        .ConfigureAwait(false);
                if (request.StreamPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder))
                        .ConfigureAwait(false);
                if (request.FileInfoPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder))
                        .ConfigureAwait(false);

                throw new ApizrException(new NotImplementedException());
            }
            catch (ApizrException e)
            {
                return Option.None<TUploadApiResultData, ApizrException>(e);
            }
        }
    }

    /// <summary>
    /// The mediation handler for <see cref="UploadOptionalCommand{TUploadApi}"/>
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
    public class UploadOptionalCommandHandler<TUploadApi> : RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<UploadOptionalCommand<TUploadApi>, Option<HttpResponseMessage, ApizrException>>,
        IRequestHandler<UploadOptionalCommand, Option<HttpResponseMessage, ApizrException>> 
        where TUploadApi : IUploadApi
    {
        private readonly IApizrUploadManager<TUploadApi> _uploadManager;

        /// <summary>
        /// The mediation handler for <see cref="UploadOptionalCommand{TUploadApi}"/>
        /// </summary>
        /// <param name="uploadManager">The upload manager</param>
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
        public async Task<Option<HttpResponseMessage, ApizrException>> Handle(UploadOptionalCommand<TUploadApi> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ByteArrayPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder))
                        .ConfigureAwait(false);
                if (request.StreamPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder))
                        .ConfigureAwait(false);
                if (request.FileInfoPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder))
                        .ConfigureAwait(false);

                throw new ApizrException(new NotImplementedException());
            }
            catch (ApizrException e)
            {
                return Option.None<HttpResponseMessage, ApizrException>(e);
            }
        }

        /// <summary>
        /// Handling the upload optional request
        /// </summary>
        /// <param name="request">The upload optional request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public async Task<Option<HttpResponseMessage, ApizrException>> Handle(UploadOptionalCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.ByteArrayPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder))
                        .ConfigureAwait(false);
                if (request.StreamPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder))
                        .ConfigureAwait(false);
                if (request.FileInfoPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder))
                        .ConfigureAwait(false);

                throw new ApizrException(new NotImplementedException());
            }
            catch (ApizrException e)
            {
                return Option.None<HttpResponseMessage, ApizrException>(e);
            }
        }
    }

    /// <summary>
    /// The mediation handler for <see cref="UploadWithOptionalCommand{TUploadApiResultData}"/>
    /// </summary>
    /// <typeparam name="TUploadApiResultData">The upload result type</typeparam>
    public class UploadWithOptionalCommandHandler<TUploadApiResultData> : RequestHandlerBase<IApizrRequestOptions, IApizrRequestOptionsBuilder>,
        IRequestHandler<UploadWithOptionalCommand<TUploadApiResultData>, Option<TUploadApiResultData, ApizrException>>
    {
        private readonly IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData> _uploadManager;

        /// <summary>
        /// The mediation handler for <see cref="UploadWithOptionalCommand{TUploadApiResultData}"/>
        /// </summary>
        /// <param name="uploadManager">The upload manager</param>
        public UploadWithOptionalCommandHandler(IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData> uploadManager)
        {
            _uploadManager = uploadManager;
        }

        /// <summary>
        /// Handling the upload optional request
        /// </summary>
        /// <param name="request">The upload optional request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public async Task<Option<TUploadApiResultData, ApizrException>> Handle(UploadWithOptionalCommand<TUploadApiResultData> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ByteArrayPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.ByteArrayPart, request.OptionsBuilder))
                        .ConfigureAwait(false);
                if (request.StreamPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.StreamPart, request.OptionsBuilder))
                        .ConfigureAwait(false);
                if (request.FileInfoPart != null)
                    return await request
                        .SomeNotNull(new ApizrException(
                            new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                        .MapAsync(_ =>
                            _uploadManager.UploadAsync(request.FileInfoPart, request.OptionsBuilder))
                        .ConfigureAwait(false);

                throw new ApizrException(new NotImplementedException());
            }
            catch (ApizrException e)
            {
                return Option.None<TUploadApiResultData, ApizrException>(e);
            }
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Mapping;
using Apizr.Optional.Requesting.Handling.Base;
using MediatR;
using Optional;
using Optional.Async.Extensions;
using Polly;

namespace Apizr.Optional.Requesting.Handling
{
    public class ExecuteOptionalRequestHandler<TWebApi, TModelResponse, TApiResponse> :
        ExecuteOptionalRequestHandlerBase<TWebApi, TModelResponse, TApiResponse,
            ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse>>
    {
        public ExecuteOptionalRequestHandler(IMappingHandler mappingHandler, IApizrManager<TWebApi> webApiManager) :
            base(mappingHandler, webApiManager)
        {
        }

        public override async Task<Option<TModelResponse, ApizrException<TModelResponse>>> Handle(
            ExecuteOptionalResultRequest<TWebApi, TModelResponse, TApiResponse> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync(executeApiMethod))
                            .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync(executeApiMethod, request.Context))
                            .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)))
                            .ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken))
                            .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)))
                            .ConfigureAwait(false);

                    case Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync(executeApiMethod))
                            .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync(executeApiMethod, request.Context, cancellationToken))
                            .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync(executeApiMethod, request.Context))
                            .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)))
                            .ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken))
                            .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync(executeApiMethod, request.Context, cancellationToken))
                            .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)))
                            .ConfigureAwait(false);

                    default:
                        throw new ApizrException<TApiResponse>(new NotImplementedException());
                }
            }
            catch (ApizrException<TApiResponse> e)
            {
                return Option.None<TModelResponse, ApizrException<TModelResponse>>(
                    new ApizrException<TModelResponse>(e.InnerException,
                        Map<TApiResponse, TModelResponse>(e.CachedResult)));
            }
        }
    }

    public class ExecuteOptionalRequestHandler<TWebApi, TApiResponse> : ExecuteOptionalRequestHandlerBase<TWebApi,
        TApiResponse, ExecuteOptionalRequest<TWebApi, TApiResponse>>
    {
        public ExecuteOptionalRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<Option<TApiResponse, ApizrException<TApiResponse>>> Handle(
            ExecuteOptionalRequest<TWebApi, TApiResponse> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod, request.Context))
                            .ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiResponse>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod, request.Context, cancellationToken))
                            .ConfigureAwait(false);

                    default:
                        throw new ApizrException<TApiResponse>(new NotImplementedException());
                }
            }
            catch (ApizrException<TApiResponse> e)
            {
                return Option.None<TApiResponse, ApizrException<TApiResponse>>(e);
            }
        }
    }

    public class ExecuteOptionalRequestHandler<TWebApi> : ExecuteOptionalRequestHandlerBase<TWebApi, ExecuteOptionalUnitRequest<TWebApi>>
    {
        public ExecuteOptionalRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(ExecuteOptionalUnitRequest<TWebApi> request,
            CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(async _ =>
                            {
                                await WebApiManager.ExecuteAsync(executeApiMethod);

                                return Unit.Value;
                            }).ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, Task>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(async _ =>
                            {
                                await WebApiManager.ExecuteAsync(executeApiMethod, request.Context);

                                return Unit.Value;
                            }).ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(async _ =>
                            {
                                await WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken);

                                return Unit.Value;
                            }).ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(async _ =>
                            {
                                await WebApiManager.ExecuteAsync(executeApiMethod, request.Context, cancellationToken);

                                return Unit.Value;
                            }).ConfigureAwait(false);

                    default:
                        throw new ApizrException(new NotImplementedException());
                }
            }
            catch (ApizrException e)
            {
                return Option.None<Unit, ApizrException>(e);
            }
        }
    }
}

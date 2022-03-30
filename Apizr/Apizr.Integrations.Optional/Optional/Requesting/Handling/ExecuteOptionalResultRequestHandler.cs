using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Optional.Requesting.Handling.Base;
using Optional;
using Optional.Async.Extensions;
using Polly;

namespace Apizr.Optional.Requesting.Handling
{
    public class ExecuteOptionalResultRequestHandler<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> :
        ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData,
            ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>>
    {
        public ExecuteOptionalResultRequestHandler(IApizrManager<TWebApi> webApiManager) :
            base(webApiManager)
        {
        }

        public override async Task<Option<TModelResultData, ApizrException<TModelResultData>>> Handle(ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData>(executeApiMethod, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData>(executeApiMethod, request.Context, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData>(executeApiMethod, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData>(executeApiMethod, request.Context, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(executeApiMethod, request.ModelRequestData, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(executeApiMethod, request.ModelRequestData, request.Context, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(executeApiMethod, request.ModelRequestData, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(executeApiMethod, request.ModelRequestData, request.Context, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    default:
                        throw new ApizrException<TApiResultData>(new NotImplementedException());
                }
            }
            catch (ApizrException<TModelResultData> e)
            {
                return Option.None<TModelResultData, ApizrException<TModelResultData>>(e);
            }
        }
    }

    public class ExecuteOptionalResultRequestHandler<TWebApi, TModelData, TApiData> :
        ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelData, TApiData,
            ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>>
    {
        public ExecuteOptionalResultRequestHandler(IApizrManager<TWebApi> webApiManager) :
            base(webApiManager)
        {
        }

        public override async Task<Option<TModelData, ApizrException<TModelData>>> Handle(
            ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.Context, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.Context, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.Context, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.Context, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    default:
                        throw new ApizrException<TApiData>(new NotImplementedException());
                }
            }
            catch (ApizrException<TModelData> e)
            {
                return Option.None<TModelData, ApizrException<TModelData>>(e);
            }
        }
    }

    public class ExecuteOptionalResultRequestHandler<TWebApi, TApiData> : ExecuteOptionalResultRequestHandlerBase<TWebApi,
        TApiData, ExecuteOptionalResultRequest<TWebApi, TApiData>>
    {
        public ExecuteOptionalResultRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<Option<TApiData, ApizrException<TApiData>>> Handle(
            ExecuteOptionalResultRequest<TWebApi, TApiData> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod, request.Context, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod, request.Context, cancellationToken, request.ClearCache))
                            .ConfigureAwait(false);

                    default:
                        throw new ApizrException<TApiData>(new NotImplementedException());
                }
            }
            catch (ApizrException<TApiData> e)
            {
                return Option.None<TApiData, ApizrException<TApiData>>(e);
            }
        }
    }
}

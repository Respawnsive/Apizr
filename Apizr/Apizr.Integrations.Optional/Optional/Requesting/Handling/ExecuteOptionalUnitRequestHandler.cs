using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Extending;
using Apizr.Optional.Requesting.Handling.Base;
using MediatR;
using Optional;
using Optional.Async.Extensions;
using Polly;

namespace Apizr.Optional.Requesting.Handling
{
    public class ExecuteOptionalUnitRequestHandler<TWebApi, TModelData, TApiData> : ExecuteOptionalUnitRequestHandlerBase<TWebApi, TModelData, TApiData, ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData>>
    {
        public ExecuteOptionalUnitRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<Option<Unit, ApizrException>> Handle(ExecuteOptionalUnitRequest<TWebApi, TModelData, TApiData> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, TApiData, Task>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(async _ =>
                            {
                                await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData);

                                return Unit.Value;
                            }).ConfigureAwait(false);

                    case Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(async _ =>
                            {
                                await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.Context);

                                return Unit.Value;
                            }).ConfigureAwait(false);

                    case Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(async _ =>
                            {
                                await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, cancellationToken);

                                return Unit.Value;
                            }).ConfigureAwait(false);

                    case Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(async _ =>
                            {
                                await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.Context, cancellationToken);

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

    public class ExecuteOptionalUnitRequestHandler<TWebApi> : ExecuteOptionalUnitRequestHandlerBase<TWebApi, ExecuteOptionalUnitRequest<TWebApi>>
    {
        public ExecuteOptionalUnitRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
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

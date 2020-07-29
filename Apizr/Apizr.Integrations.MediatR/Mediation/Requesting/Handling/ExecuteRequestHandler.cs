using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Handling.Base;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling
{
    public class ExecuteRequestHandler<TWebApi, TModelResponse, TApiResponse> : ExecuteRequestHandlerBase<TWebApi,
        TModelResponse, TApiResponse, ExecuteRequest<TWebApi, TModelResponse, TApiResponse>, TModelResponse>
    {
        public ExecuteRequestHandler(IMappingHandler mappingHandler, IApizrManager<TWebApi> webApiManager) : base(
            mappingHandler, webApiManager)
        {
        }

        public override async Task<TModelResponse> Handle(ExecuteRequest<TWebApi, TModelResponse, TApiResponse> request,
            CancellationToken cancellationToken)
        {
            TApiResponse result;
            switch (request.ExecuteApiMethod)
            {
                case Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod:
                    result = await WebApiManager.ExecuteAsync(executeApiMethod, request.Priority)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod:
                    result = await WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken, request.Priority)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod:
                    result = await WebApiManager.ExecuteAsync(executeApiMethod, request.Priority)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TApiResponse>>> executeApiMethod:
                    result = await WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken, request.Priority)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new ApizrException<TApiResponse>(new NotImplementedException());
            }

            return Map<TApiResponse, TModelResponse>(result);
        }
    }

    public class ExecuteRequestHandler<TWebApi, TApiResponse> : ExecuteRequestHandlerBase<TWebApi, TApiResponse,
        ExecuteRequest<TWebApi, TApiResponse>, TApiResponse>
    {
        public ExecuteRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<TApiResponse> Handle(ExecuteRequest<TWebApi, TApiResponse> request,
            CancellationToken cancellationToken)
        {
            switch (request.ExecuteApiMethod)
            {
                case Expression<Func<CancellationToken, TWebApi, Task<TApiResponse>>> executeApiMethod:
                    return await WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken, request.Priority)
                        .ConfigureAwait(false);

                case Expression<Func<TWebApi, Task<TApiResponse>>> executeApiMethod:
                    return await WebApiManager.ExecuteAsync(executeApiMethod, request.Priority)
                        .ConfigureAwait(false);

                default:
                    throw new ApizrException<TApiResponse>(new NotImplementedException());
            }
        }
    }

    public class ExecuteRequestHandler<TWebApi> : ExecuteRequestHandlerBase<TWebApi, ExecuteRequest<TWebApi>, Unit>
    {
        public ExecuteRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<Unit> Handle(ExecuteRequest<TWebApi> request, CancellationToken cancellationToken)
        {
            switch (request.ExecuteApiMethod)
            {
                case Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken, request.Priority)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, request.Priority)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new ApizrException(new NotImplementedException());
            }

            return Unit.Value;
        }
    }
}

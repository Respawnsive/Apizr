using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Handling.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Requesting.Handling
{
    public class ExecuteUnitRequestHandler<TWebApi, TModelData, TApiData> : ExecuteUnitRequestHandlerBase<TWebApi, TModelData, TApiData, ExecuteUnitRequest<TWebApi, TModelData, TApiData>>
    {
        public ExecuteUnitRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<Unit> Handle(ExecuteUnitRequest<TWebApi, TModelData, TApiData> request, CancellationToken cancellationToken)
        {
            switch (request.ExecuteApiMethod)
            {
                case Expression<Func<TWebApi, TApiData, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.Context)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.Context, cancellationToken)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new ApizrException<TModelData>(new NotImplementedException());
            }

            return Unit.Value;
        }
    }

    public class ExecuteUnitRequestHandler<TWebApi> : ExecuteUnitRequestHandlerBase<TWebApi, ExecuteUnitRequest<TWebApi>>
    {
        public ExecuteUnitRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<Unit> Handle(ExecuteUnitRequest<TWebApi> request, CancellationToken cancellationToken)
        {
            switch (request.ExecuteApiMethod)
            {
                case Expression<Func<TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<Context, TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, request.Context)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, request.Context, cancellationToken)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new ApizrException(new NotImplementedException());
            }

            return Unit.Value;
        }
    }
}

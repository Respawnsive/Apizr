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
    /// <summary>
    /// The mediation execute unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteUnitRequestHandler<TWebApi, TModelData, TApiData> : ExecuteUnitRequestHandlerBase<TWebApi, TModelData, TApiData, ExecuteUnitRequest<TWebApi, TModelData, TApiData>>
    {
        public ExecuteUnitRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override async Task<Unit> Handle(ExecuteUnitRequest<TWebApi, TModelData, TApiData> request, CancellationToken cancellationToken)
        {
            switch (request.ExecuteApiMethod)
            {
                case Expression<Func<TWebApi, TApiData, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.OnException)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, cancellationToken, request.OnException)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.Context, request.OnException)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.Context, cancellationToken, request.OnException)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new ApizrException<TModelData>(new NotImplementedException());
            }

            return Unit.Value;
        }
    }

    /// <summary>
    /// The mediation execute unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    public class ExecuteUnitRequestHandler<TWebApi> : ExecuteUnitRequestHandlerBase<TWebApi, ExecuteUnitRequest<TWebApi>>
    {
        public ExecuteUnitRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override async Task<Unit> Handle(ExecuteUnitRequest<TWebApi> request, CancellationToken cancellationToken)
        {
            switch (request.ExecuteApiMethod)
            {
                case Expression<Func<TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, request.OnException)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<Context, TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, request.Context, request.OnException)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, cancellationToken, request.OnException)
                        .ConfigureAwait(false);
                    break;

                case Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod:
                    await WebApiManager.ExecuteAsync(executeApiMethod, request.Context, cancellationToken, request.OnException)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new ApizrException(new NotImplementedException());
            }

            return Unit.Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
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
        public override Task<Unit> Handle(ExecuteUnitRequest<TWebApi, TModelData, TApiData> request, CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, TApiData, Task>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData,
                        request.OptionsBuilder)
                    .ContinueWith(_ => Unit.Value, cancellationToken),

                Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData,
                        request.OptionsBuilder)
                    .ContinueWith(_ => Unit.Value, cancellationToken),

                _ => throw new ApizrException<TModelData>(new NotImplementedException())
            };
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
        public override Task<Unit> Handle(ExecuteUnitRequest<TWebApi> request, CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, Task>> executeApiMethod => 
                    WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder)
                        .ContinueWith(_ => Unit.Value, cancellationToken),

                Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod => 
                    WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder)
                        .ContinueWith(_ => Unit.Value, cancellationToken),

                _ => throw new ApizrException(new NotImplementedException())
            };
        }
    }
}

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
using Refit;

namespace Apizr.Mediation.Requesting.Handling
{
    /// <summary>
    /// The mediation execute unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteSafeUnitRequestHandler<TWebApi, TModelData, TApiData> : ExecuteResultRequestHandlerBase<TWebApi, IApizrResponse, IApiResponse, TApiData, TModelData, ExecuteSafeUnitRequest<TWebApi, TModelData, TApiData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        public ExecuteSafeUnitRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse> Handle(ExecuteSafeUnitRequest<TWebApi, TModelData, TApiData> request, CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData,request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData,
                        request.OptionsBuilder),

                _ => throw new ApizrException<TModelData>(new NotImplementedException())
            };
        }
    }

    /// <summary>
    /// The mediation execute unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    public class ExecuteSafeUnitRequestHandler<TWebApi> : ExecuteResultRequestHandlerBase<TWebApi, IApizrResponse, IApiResponse, ExecuteSafeUnitRequest<TWebApi>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        public ExecuteSafeUnitRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse> Handle(ExecuteSafeUnitRequest<TWebApi> request, CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, Task<IApiResponse>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder),

                _ => throw new ApizrException(new NotImplementedException())
            };
        }
    }
}

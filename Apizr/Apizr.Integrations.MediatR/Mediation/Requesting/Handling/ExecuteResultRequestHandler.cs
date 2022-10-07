using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Handling.Base;
using Polly;

namespace Apizr.Mediation.Requesting.Handling
{
    /// <summary>
    /// The mediation execute result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    public class ExecuteResultRequestHandler<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> : ExecuteResultRequestHandlerBase<TWebApi,
        TModelResultData, TApiResultData, TApiRequestData, TModelRequestData, ExecuteResultRequest<TWebApi,
            TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>>
    {
        public ExecuteResultRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<TModelResultData> Handle(
            ExecuteResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> request,
            CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod =>
                    WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                        executeApiMethod, request.ModelRequestData, request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                        executeApiMethod, request.ModelRequestData, request.OptionsBuilder),

                _ => throw new ApizrException<TModelResultData>(new NotImplementedException())
            };
        }
    }

    /// <summary>
    /// The mediation execute result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteResultRequestHandler<TWebApi, TModelData, TApiData> : ExecuteResultRequestHandlerBase<TWebApi,
        TModelData, TApiData, ExecuteResultRequest<TWebApi,
            TModelData, TApiData>>
    {
        public ExecuteResultRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<TModelData> Handle(
            ExecuteResultRequest<TWebApi, TModelData, TApiData> request,
            CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.OptionsBuilder),

                Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData,
                        request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod =>
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData,
                        request.OptionsBuilder),

                _ => throw new ApizrException<TModelData>(new NotImplementedException())
            };
        }
    }

    /// <summary>
    /// The mediation execute result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteResultRequestHandler<TWebApi, TApiData> : ExecuteResultRequestHandlerBase<TWebApi,
        TApiData, ExecuteResultRequest<TWebApi, TApiData>>
    {
        public ExecuteResultRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<TApiData> Handle(
            ExecuteResultRequest<TWebApi, TApiData> request,
            CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod =>
                    WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder),

                Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync(executeApiMethod, request.ModelRequestData, request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod =>
                    WebApiManager.ExecuteAsync(executeApiMethod, request.ModelRequestData, request.OptionsBuilder),

                _ => throw new ApizrException<TApiData>(new NotImplementedException())
            };
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Handling.Base;
using Polly;
using Refit;

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
    public class ExecuteSafeResultRequestHandler<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData> : ExecuteResultRequestHandlerBase<TWebApi,
        IApizrResponse<TModelResultData>, IApiResponse<TApiResultData>, TApiRequestData, TModelRequestData, ExecuteSafeResultRequest<TWebApi,
            TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        public ExecuteSafeResultRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse<TModelResultData>> Handle(
            ExecuteSafeResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> request,
            CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>> executeApiMethod =>
                    WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                        executeApiMethod, request.ModelRequestData, request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>> executeApiMethod => 
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
    public class ExecuteSafeResultRequestHandler<TWebApi, TModelData, TApiData> : ExecuteResultRequestHandlerBase<TWebApi,
        IApizrResponse<TModelData>, IApiResponse<TApiData>, TApiData, TModelData, ExecuteSafeResultRequest<TWebApi,
            TModelData, TApiData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        public ExecuteSafeResultRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse<TModelData>> Handle(
            ExecuteSafeResultRequest<TWebApi, TModelData, TApiData> request,
            CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.OptionsBuilder),

                Expression<Func<TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, 
                        request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod =>
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
    public class ExecuteSafeResultRequestHandler<TWebApi, TApiData> : ExecuteResultRequestHandlerBase<TWebApi,
        IApizrResponse<TApiData>, IApiResponse<TApiData>, ExecuteSafeResultRequest<TWebApi, TApiData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        public ExecuteSafeResultRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override Task<IApizrResponse<TApiData>> Handle(
            ExecuteSafeResultRequest<TWebApi, TApiData> request,
            CancellationToken cancellationToken)
        {
            return request.ExecuteApiMethod switch
            {
                Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod => 
                    WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder),

                Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod =>
                    WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder),

                _ => throw new ApizrException<TApiData>(new NotImplementedException())
            };
        }
    }
}

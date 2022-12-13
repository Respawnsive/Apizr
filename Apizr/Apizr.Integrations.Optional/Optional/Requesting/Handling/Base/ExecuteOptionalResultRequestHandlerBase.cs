using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Optional.Requesting.Base;
using Optional;

namespace Apizr.Optional.Requesting.Handling.Base
{
    /// <summary>
    /// The top level base mediation execute optional result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    /// <typeparam name="TRequest">The request to handle</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        ExecuteResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData, Option<TModelResultData, ApizrException<TModelResultData>>, TApiRequestData, TModelRequestData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TRequest : ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected ExecuteOptionalResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute optional result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TRequest">The request to handle</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelData, TApiData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> :
        ExecuteResultRequestHandlerBase<TWebApi, TModelData, TApiData, Option<TModelData, ApizrException<TModelData>>, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TRequest : ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected ExecuteOptionalResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute optional result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TRequest">The request to handle</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteOptionalResultRequestHandlerBase<TWebApi, TApiData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : 
        ExecuteResultRequestHandlerBase<TWebApi, TApiData, TApiData, Option<TApiData, ApizrException<TApiData>>, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TRequest : ExecuteOptionalResultRequestBase<TWebApi, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected ExecuteOptionalResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }
}

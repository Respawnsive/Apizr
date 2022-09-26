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
    public abstract class ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData, TRequest> :
        ExecuteResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData,
            Option<TModelResultData, ApizrException<TModelResultData>>, TApiRequestData, TModelRequestData,
            TRequest>
        where TRequest : ExecuteOptionalResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>
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
    public abstract class ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelData, TApiData, TRequest> :
        ExecuteResultRequestHandlerBase<TWebApi, TModelData, TApiData, Option<TModelData, ApizrException<TModelData>>,
            TRequest>
        where TRequest : ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData>
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
    public abstract class ExecuteOptionalResultRequestHandlerBase<TWebApi, TApiData, TRequest> : ExecuteResultRequestHandlerBase
    <TWebApi, TApiData, TApiData, Option<TApiData, ApizrException<TApiData>>,
        TRequest>
        where TRequest : ExecuteOptionalResultRequestBase<TWebApi, TApiData>
    {
        protected ExecuteOptionalResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }
}

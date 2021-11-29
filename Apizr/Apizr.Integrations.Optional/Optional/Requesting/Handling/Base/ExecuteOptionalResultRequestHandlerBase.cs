using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Optional.Requesting.Base;
using Optional;

namespace Apizr.Optional.Requesting.Handling.Base
{
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

    public abstract class ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelData, TApiData, TRequest> :
        ExecuteResultRequestHandlerBase<TWebApi, TModelData, TApiData, Option<TModelData, ApizrException<TModelData>>,
            TRequest>
        where TRequest : ExecuteOptionalResultRequestBase<TWebApi, TModelData, TApiData>
    {
        protected ExecuteOptionalResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }

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

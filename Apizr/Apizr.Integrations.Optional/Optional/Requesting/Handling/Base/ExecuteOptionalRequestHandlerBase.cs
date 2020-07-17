using Apizr.Mapping;
using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Optional.Requesting.Base;
using MediatR;
using Optional;

namespace Apizr.Optional.Requesting.Handling.Base
{
    public abstract class ExecuteOptionalRequestHandlerBase<TWebApi, TModelResponse, TApiResponse, TRequest> :
        ExecuteRequestHandlerBase<TWebApi, TModelResponse, TApiResponse,
            TRequest, Option<TModelResponse, ApizrException<TModelResponse>>>
        where TRequest : ExecuteOptionalRequestBase<TWebApi, TModelResponse, TApiResponse>
    {
        protected ExecuteOptionalRequestHandlerBase(IMappingHandler mappingHandler,
            IApizrManager<TWebApi> webApiManager) : base(mappingHandler, webApiManager)
        {
        }
    }

    public abstract class ExecuteOptionalRequestHandlerBase<TWebApi, TApiResponse, TRequest> : ExecuteRequestHandlerBase
    <TWebApi, TApiResponse,
        TRequest, Option<TApiResponse, ApizrException<TApiResponse>>>
        where TRequest : ExecuteOptionalRequestBase<TWebApi, TApiResponse>
    {
        protected ExecuteOptionalRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }

    public abstract class ExecuteOptionalRequestHandlerBase<TWebApi, TRequest> : ExecuteRequestHandlerBase<TWebApi, TRequest,
            Option<Unit, ApizrException>>
        where TRequest : ExecuteOptionalRequestBase<TWebApi>
    {
        protected ExecuteOptionalRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }
}

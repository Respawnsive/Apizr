using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Optional.Requesting.Base;
using MediatR;
using Optional;

namespace Apizr.Optional.Requesting.Handling.Base
{
    public abstract class ExecuteOptionalUnitRequestHandlerBase<TWebApi, TModelData, TApiData, TRequest> : ExecuteUnitRequestHandlerBase<TWebApi, TModelData, TApiData, Option<Unit, ApizrException>, TRequest>
        where TRequest : ExecuteOptionalUnitRequestBase<TWebApi, TModelData, TApiData>
    {
        protected ExecuteOptionalUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }

    public abstract class ExecuteOptionalUnitRequestHandlerBase<TWebApi, TRequest> : ExecuteUnitRequestHandlerBase<TWebApi, Option<Unit, ApizrException>, TRequest>
        where TRequest : ExecuteOptionalUnitRequestBase<TWebApi>
    {
        protected ExecuteOptionalUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }
}

using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Handling.Base;
using Apizr.Optional.Requesting.Base;
using MediatR;
using Optional;

namespace Apizr.Optional.Requesting.Handling.Base
{
    /// <summary>
    /// The top level base mediation execute optional unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TRequest">The execute unit request to handle</typeparam>
    public abstract class ExecuteOptionalUnitRequestHandlerBase<TWebApi, TModelData, TApiData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : 
        ExecuteUnitRequestHandlerBase<TWebApi, TModelData, TApiData, Option<Unit, ApizrException>, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TRequest : ExecuteOptionalUnitRequestBase<TWebApi, TModelData, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrUnitRequestOptions
        where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected ExecuteOptionalUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute optional unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TRequest">The execute unit request to handle</typeparam>
    public abstract class ExecuteOptionalUnitRequestHandlerBase<TWebApi, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : 
        ExecuteUnitRequestHandlerBase<TWebApi, Option<Unit, ApizrException>, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TRequest : ExecuteOptionalUnitRequestBase<TWebApi, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrUnitRequestOptions
        where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected ExecuteOptionalUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }
    }
}

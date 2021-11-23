using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mediation.Requesting.Base;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling.Base
{
    public abstract class
        ExecuteUnitRequestHandlerBase<TWebApi, TModelData, TApiData,
            TRequest> : IRequestHandler<TRequest, Unit>
        where TRequest : ExecuteUnitRequestBase<TWebApi, TModelData, TApiData>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        public abstract Task<Unit> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class
        ExecuteUnitRequestHandlerBase<TWebApi, TRequest> : IRequestHandler<TRequest, Unit>
        where TRequest : ExecuteUnitRequestBase<TWebApi>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        public abstract Task<Unit> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Base;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling.Base
{
    public abstract class ExecuteRequestHandlerBase<TWebApi, TModelResponse, TApiResponse, TRequest, TRequestResponse> : RequestHandlerBase, IRequestHandler<TRequest, TRequestResponse>
        where TRequest : ExecuteRequestBase<TWebApi, TModelResponse, TApiResponse, TRequestResponse>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteRequestHandlerBase(IMappingHandler mappingHandler, IApizrManager<TWebApi> webApiManager) : base(mappingHandler)
        {
            WebApiManager = webApiManager;
        }


        public abstract Task<TRequestResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class ExecuteRequestHandlerBase<TWebApi, TApiResponse, TRequest, TRequestResponse> : IRequestHandler<TRequest, TRequestResponse>
    where TRequest : ExecuteRequestBase<TWebApi, TApiResponse, TRequestResponse>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        public abstract Task<TRequestResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class ExecuteRequestHandlerBase<TWebApi, TRequest, TRequestResponse> : IRequestHandler<TRequest, TRequestResponse>
    where TRequest : ExecuteRequestBase<TWebApi, TRequestResponse>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        public abstract Task<TRequestResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

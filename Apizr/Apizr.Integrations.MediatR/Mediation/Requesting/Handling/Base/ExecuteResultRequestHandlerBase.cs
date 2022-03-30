using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Base;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling.Base
{
    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData,
        TFormattedModelResultData, TApiRequestData, TModelRequestData, TRequest> : RequestHandlerBase,
        IRequestHandler<TRequest, TFormattedModelResultData>
        where TRequest : ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TFormattedModelResultData,
            TApiRequestData, TModelRequestData>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }
        
        public abstract Task<TFormattedModelResultData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData, TRequest> : RequestHandlerBase,
        IRequestHandler<TRequest, TModelResultData>
        where TRequest : ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }
        
        public abstract Task<TModelResultData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TModelData, TApiData, TFormattedModelResultData,
        TRequest> : RequestHandlerBase, IRequestHandler<TRequest, TFormattedModelResultData>
        where TRequest : ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }
        
        public abstract Task<TFormattedModelResultData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TModelData, TApiData,
        TRequest> : RequestHandlerBase, IRequestHandler<TRequest, TModelData>
        where TRequest : ExecuteResultRequestBase<TWebApi, TModelData, TApiData>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }
        
        public abstract Task<TModelData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TApiData, TRequest> : IRequestHandler<TRequest, TApiData>
    where TRequest : ExecuteResultRequestBase<TWebApi, TApiData>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        public abstract Task<TApiData> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

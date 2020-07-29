using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Mediation.Requesting.Handling.Base;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling
{
    public class ExecuteRequestHandler<TWebApi, TModelResponse, TApiResponse> : ExecuteRequestHandlerBase<TWebApi,
        TModelResponse, TApiResponse, ExecuteRequest<TWebApi, TModelResponse, TApiResponse>, TModelResponse>
    {
        public ExecuteRequestHandler(IMappingHandler mappingHandler, IApizrManager<TWebApi> webApiManager) : base(
            mappingHandler, webApiManager)
        {
        }

        public override async Task<TModelResponse> Handle(ExecuteRequest<TWebApi, TModelResponse, TApiResponse> request,
            CancellationToken cancellationToken)
        {
            var result = await WebApiManager.ExecuteAsync(
                    (ct, api) => request.ExecuteApiMethod.Compile()(ct, api, MappingHandler),
                    cancellationToken, request.Priority)
                .ConfigureAwait(false);

            return Map<TApiResponse, TModelResponse>(result);
        }
    }

    public class ExecuteRequestHandler<TWebApi, TApiResponse> : ExecuteRequestHandlerBase<TWebApi, TApiResponse,
        ExecuteRequest<TWebApi, TApiResponse>, TApiResponse>
    {
        public ExecuteRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<TApiResponse> Handle(ExecuteRequest<TWebApi, TApiResponse> request,
            CancellationToken cancellationToken)
        {
            return await WebApiManager.ExecuteAsync(request.ExecuteApiMethod, cancellationToken, request.Priority)
                .ConfigureAwait(false);
        }
    }

    public class ExecuteRequestHandler<TWebApi> : ExecuteRequestHandlerBase<TWebApi, ExecuteRequest<TWebApi>, Unit>
    {
        public ExecuteRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override async Task<Unit> Handle(ExecuteRequest<TWebApi> request, CancellationToken cancellationToken)
        {
            await WebApiManager.ExecuteAsync(request.ExecuteApiMethod, cancellationToken, request.Priority)
                .ConfigureAwait(false);

            return Unit.Value;
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling
{
    public class ExecuteRequestHandler<TWebApi, TResult> : IRequestHandler<ExecuteRequest<TWebApi, TResult>, TResult>
    {
        private readonly IApizrManager<TWebApi> _webApiManager;

        public ExecuteRequestHandler(IApizrManager<TWebApi> webApiManager)
        {
            _webApiManager = webApiManager;
        }

        public Task<TResult> Handle(ExecuteRequest<TWebApi, TResult> request, CancellationToken cancellationToken)
        {
            return _webApiManager.ExecuteAsync(request.ExecuteApiMethod, cancellationToken, request.Priority);
        }
    }

    public class ExecuteRequestHandler<TWebApi> : IRequestHandler<ExecuteRequest<TWebApi>, Unit>
    {
        private readonly IApizrManager<TWebApi> _webApiManager;

        public ExecuteRequestHandler(IApizrManager<TWebApi> webApiManager)
        {
            _webApiManager = webApiManager;
        }

        public Task<Unit> Handle(ExecuteRequest<TWebApi> request, CancellationToken cancellationToken)
        {
            return _webApiManager.ExecuteAsync(request.ExecuteApiMethod, cancellationToken, request.Priority)
                .ContinueWith(t => Unit.Value, cancellationToken);
        }
    }
}

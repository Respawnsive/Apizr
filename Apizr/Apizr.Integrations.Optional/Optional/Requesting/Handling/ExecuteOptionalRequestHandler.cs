using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Requesting.Handling
{
    public class ExecuteOptionalRequestHandler<TWebApi> : IRequestHandler<ExecuteOptionalRequest<TWebApi>, Option<Unit, ApizrException>>
    {
        private readonly IApizrManager<TWebApi> _webApiManager;

        public ExecuteOptionalRequestHandler(IApizrManager<TWebApi> webApiManager)
        {
            _webApiManager = webApiManager;
        }

        public Task<Option<Unit, ApizrException>> Handle(ExecuteOptionalRequest<TWebApi> request, CancellationToken cancellationToken)
        {
            try
            {
                return request
                    .SomeNotNull(new ApizrException(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        _webApiManager
                            .ExecuteAsync(request.ExecuteApiMethod, cancellationToken, request.Priority)
                            .ContinueWith(task => Unit.Value, cancellationToken));
            }
            catch (ApizrException e)
            {
                return Task.FromResult(Option.None<Unit, ApizrException>(e));
            }
        }
    }

    public class ExecuteOptionalRequestHandler<TWebApi, TResult> : IRequestHandler<ExecuteOptionalRequest<TWebApi, TResult>, Option<TResult, ApizrException<TResult>>>
    {
        private readonly IApizrManager<TWebApi> _webApiManager;

        public ExecuteOptionalRequestHandler(IApizrManager<TWebApi> webApiManager)
        {
            _webApiManager = webApiManager;
        }

        public Task<Option<TResult, ApizrException<TResult>>> Handle(ExecuteOptionalRequest<TWebApi, TResult> request, CancellationToken cancellationToken)
        {
            try
            {
                return request
                    .SomeNotNull(new ApizrException<TResult>(new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        _webApiManager
                            .ExecuteAsync(request.ExecuteApiMethod, cancellationToken, request.Priority));
            }
            catch (ApizrException<TResult> e)
            {
                return Task.FromResult(Option.None<TResult, ApizrException<TResult>>(e));
            }
        }
    }
}

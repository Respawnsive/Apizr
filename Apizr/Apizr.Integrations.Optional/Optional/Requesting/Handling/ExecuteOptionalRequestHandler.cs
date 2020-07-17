using System;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Mapping;
using Apizr.Optional.Requesting.Handling.Base;
using MediatR;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Requesting.Handling
{
    public class ExecuteOptionalRequestHandler<TWebApi, TModelResponse, TApiResponse> :
        ExecuteOptionalRequestHandlerBase<TWebApi, TModelResponse, TApiResponse,
            ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse>>
    {
        public ExecuteOptionalRequestHandler(IMappingHandler mappingHandler, IApizrManager<TWebApi> webApiManager) :
            base(mappingHandler, webApiManager)
        {
        }

        public override Task<Option<TModelResponse, ApizrException<TModelResponse>>> Handle(
            ExecuteOptionalRequest<TWebApi, TModelResponse, TApiResponse> request, CancellationToken cancellationToken)
        {
            try
            {
                return request
                    .SomeNotNull(new ApizrException<TModelResponse>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ => WebApiManager.ExecuteAsync(
                        (ct, api) => request.ExecuteApiMethod.Compile()(ct, api, MappingHandler), cancellationToken,
                        request.Priority))
                    .MapAsync(result => Task.FromResult(Map<TApiResponse, TModelResponse>(result)));
                ;
            }
            catch (ApizrException<TApiResponse> e)
            {
                return Task.FromResult(Option.None<TModelResponse, ApizrException<TModelResponse>>(
                    new ApizrException<TModelResponse>(e.InnerException,
                        Map<TApiResponse, TModelResponse>(e.CachedResult))));
            }
        }
    }

    public class ExecuteOptionalRequestHandler<TWebApi, TApiResponse> : ExecuteOptionalRequestHandlerBase<TWebApi,
        TApiResponse, ExecuteOptionalRequest<TWebApi, TApiResponse>>
    {
        public ExecuteOptionalRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override Task<Option<TApiResponse, ApizrException<TApiResponse>>> Handle(
            ExecuteOptionalRequest<TWebApi, TApiResponse> request, CancellationToken cancellationToken)
        {
            try
            {
                return request
                    .SomeNotNull(new ApizrException<TApiResponse>(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        WebApiManager.ExecuteAsync(request.ExecuteApiMethod, cancellationToken, request.Priority));
            }
            catch (ApizrException<TApiResponse> e)
            {
                return Task.FromResult(Option.None<TApiResponse, ApizrException<TApiResponse>>(e));
            }
        }
    }

    public class ExecuteOptionalRequestHandler<TWebApi> : ExecuteOptionalRequestHandlerBase<TWebApi, ExecuteOptionalRequest<TWebApi>>
    {
        public ExecuteOptionalRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        public override Task<Option<Unit, ApizrException>> Handle(ExecuteOptionalRequest<TWebApi> request,
            CancellationToken cancellationToken)
        {
            try
            {
                return request
                    .SomeNotNull(new ApizrException(
                        new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                    .MapAsync(_ =>
                        WebApiManager
                            .ExecuteAsync(request.ExecuteApiMethod, cancellationToken, request.Priority)
                            .ContinueWith(task => Unit.Value, cancellationToken));
            }
            catch (ApizrException e)
            {
                return Task.FromResult(Option.None<Unit, ApizrException>(e));
            }
        }
    }
}

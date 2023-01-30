using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Optional.Requesting.Handling.Base;
using Optional;
using Optional.Async.Extensions;
using Polly;

namespace Apizr.Optional.Requesting.Handling
{
    /// <summary>
    /// The mediation execute optional result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    public class ExecuteOptionalResultRequestHandler<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> :
        ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData,
            ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        public ExecuteOptionalResultRequestHandler(IApizrManager<TWebApi> webApiManager) :
            base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override async Task<Option<TModelResultData, ApizrException<TModelResultData>>> Handle(ExecuteOptionalResultRequest<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData>(executeApiMethod, request.OptionsBuilder))
                            .ConfigureAwait(false);

                    case Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData>((options, api) => executeApiMethod.Compile()(options, api), request.OptionsBuilder))
                            .ConfigureAwait(false);

                    case Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(executeApiMethod, request.ModelRequestData, request.OptionsBuilder))
                            .ConfigureAwait(false);

                    case Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelResultData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>((options, api, apiData) => executeApiMethod.Compile()(options, api, apiData), request.ModelRequestData, request.OptionsBuilder))
                            .ConfigureAwait(false);

                    default:
                        throw new ApizrException<TApiResultData>(new NotImplementedException());
                }
            }
            catch (ApizrException<TModelResultData> e)
            {
                return Option.None<TModelResultData, ApizrException<TModelResultData>>(e);
            }
        }
    }

    /// <summary>
    /// The mediation execute optional result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteOptionalResultRequestHandler<TWebApi, TModelData, TApiData> :
        ExecuteOptionalResultRequestHandlerBase<TWebApi, TModelData, TApiData,
            ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        public ExecuteOptionalResultRequestHandler(IApizrManager<TWebApi> webApiManager) :
            base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override async Task<Option<TModelData, ApizrException<TModelData>>> Handle(
            ExecuteOptionalResultRequest<TWebApi, TModelData, TApiData> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.OptionsBuilder))
                            .ConfigureAwait(false);

                    case Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>((options, api) => executeApiMethod.Compile()(options, api), request.OptionsBuilder))
                            .ConfigureAwait(false);

                    case Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>(executeApiMethod, request.ModelRequestData, request.OptionsBuilder))
                            .ConfigureAwait(false);

                    case Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TModelData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ => WebApiManager.ExecuteAsync<TModelData, TApiData>((options, api, apiData) => executeApiMethod.Compile()(options, api, apiData), request.ModelRequestData, request.OptionsBuilder))
                            .ConfigureAwait(false);

                    default:
                        throw new ApizrException<TApiData>(new NotImplementedException());
                }
            }
            catch (ApizrException<TModelData> e)
            {
                return Option.None<TModelData, ApizrException<TModelData>>(e);
            }
        }
    }

    /// <summary>
    /// The mediation execute optional result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    public class ExecuteOptionalResultRequestHandler<TWebApi, TApiData> : ExecuteOptionalResultRequestHandlerBase<TWebApi,
        TApiData, ExecuteOptionalResultRequest<TWebApi, TApiData>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        public ExecuteOptionalResultRequestHandler(IApizrManager<TWebApi> webApiManager) : base(webApiManager)
        {
        }

        /// <inheritdoc />
        public override async Task<Option<TApiData, ApizrException<TApiData>>> Handle(
            ExecuteOptionalResultRequest<TWebApi, TApiData> request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.ExecuteApiMethod)
                {
                    case Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync(executeApiMethod, request.OptionsBuilder))
                            .ConfigureAwait(false);

                    case Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod:
                        return await request
                            .SomeNotNull(new ApizrException<TApiData>(
                                new NullReferenceException($"Request {request.GetType().GetFriendlyName()} can not be null")))
                            .MapAsync(_ =>
                                WebApiManager.ExecuteAsync((options, api) => executeApiMethod.Compile()(options, api), request.OptionsBuilder))
                            .ConfigureAwait(false);

                    default:
                        throw new ApizrException<TApiData>(new NotImplementedException());
                }
            }
            catch (ApizrException<TApiData> e)
            {
                return Option.None<TApiData, ApizrException<TApiData>>(e);
            }
        }
    }
}

﻿using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Base;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling.Base
{
    /// <summary>
    /// The top level base mediation execute result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TFormattedModelResultData">The formatted model result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    /// <typeparam name="TRequest">The request to handle</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData,
        TFormattedModelResultData, TApiRequestData, TModelRequestData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IRequestHandler<TRequest, TFormattedModelResultData>
        where TRequest : ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TFormattedModelResultData,
            TApiRequestData, TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute result request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TFormattedModelResultData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The top level base mediation execute result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelResultData">The model result type</typeparam>
    /// <typeparam name="TApiResultData">The api result type</typeparam>
    /// <typeparam name="TApiRequestData">The api request type</typeparam>
    /// <typeparam name="TModelRequestData">The model request type</typeparam>
    /// <typeparam name="TRequest">The request to handle</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
        TModelRequestData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IRequestHandler<TRequest, TModelResultData>
        where TRequest : ExecuteResultRequestBase<TWebApi, TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute result request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TModelResultData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The top level base mediation execute result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TFormattedModelResultData">The formatted model result type</typeparam>
    /// <typeparam name="TRequest">The request to handle</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TModelData, TApiData, TFormattedModelResultData,
        TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>, IRequestHandler<TRequest, TFormattedModelResultData>
        where TRequest : ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute result request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TFormattedModelResultData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The top level base mediation execute result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TRequest">The request to handle</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TModelData, TApiData,
        TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>, IRequestHandler<TRequest, TModelData>
        where TRequest : ExecuteResultRequestBase<TWebApi, TModelData, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute result request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TModelData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The top level base mediation execute result request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TRequest">The request to handle</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteResultRequestHandlerBase<TWebApi, TApiData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : IRequestHandler<TRequest, TApiData>
    where TRequest : ExecuteResultRequestBase<TWebApi, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
    where TApizrRequestOptions : IApizrRequestOptions
    where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteResultRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute result request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TApiData> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

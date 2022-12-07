using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Base;
using MediatR;

namespace Apizr.Mediation.Requesting.Handling.Base
{
    /// <summary>
    /// The top level base mediation execute unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TFormattedModelResultData">The formatted model result data type</typeparam>
    /// <typeparam name="TRequest">The execute unit request to handle</typeparam>
    public abstract class
        ExecuteUnitRequestHandlerBase<TWebApi, TModelData, TApiData, TFormattedModelResultData,
            TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>, 
            IRequestHandler<TRequest, TFormattedModelResultData>
        where TRequest : ExecuteUnitRequestBase<TWebApi, TModelData, TApiData, TFormattedModelResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute unit request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TFormattedModelResultData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The top level base mediation execute unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TRequest">The execute unit request to handle</typeparam>
    public abstract class
        ExecuteUnitRequestHandlerBase<TWebApi, TModelData, TApiData,
            TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>, 
            IRequestHandler<TRequest, Unit>
        where TRequest : ExecuteUnitRequestBase<TWebApi, TModelData, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute unit request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<Unit> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The top level base mediation execute unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TFormattedModelResultData">The formatted model result data type</typeparam>
    /// <typeparam name="TRequest">The execute unit request to handle</typeparam>
    public abstract class
        ExecuteUnitRequestHandlerBase<TWebApi, TFormattedModelResultData, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>, 
            IRequestHandler<TRequest, TFormattedModelResultData>
        where TRequest : ExecuteUnitRequestBase<TWebApi, TFormattedModelResultData, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute unit request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<TFormattedModelResultData> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The top level base mediation execute unit request handler
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TRequest">The execute unit request to handle</typeparam>
    public abstract class
        ExecuteUnitRequestHandlerBase<TWebApi, TRequest, TApizrRequestOptions, TApizrRequestOptionsBuilder> : RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>, 
            IRequestHandler<TRequest, Unit>
        where TRequest : ExecuteUnitRequestBase<TWebApi, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        protected readonly IApizrManager<TWebApi> WebApiManager;

        protected ExecuteUnitRequestHandlerBase(IApizrManager<TWebApi> webApiManager)
        {
            WebApiManager = webApiManager;
        }

        /// <summary>
        /// Handling the execute unit request
        /// </summary>
        /// <param name="request">The execute result request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public abstract Task<Unit> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

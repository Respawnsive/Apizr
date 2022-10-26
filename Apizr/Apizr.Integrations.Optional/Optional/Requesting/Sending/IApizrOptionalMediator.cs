using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using MediatR;
using Microsoft.Extensions.Options;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Sending
{
    /// <summary>
    /// Apizr mediator to send request using MediatR by calling expression and returning optional result
    /// </summary>
    public interface IApizrOptionalMediator : IApizrOptionalMediatorBase
    {
        #region Unit

        #region SendFor<TWebApi>

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<TWebApi, Task>> executeApiMethod, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TWebApi>(Expression<Func<IApizrUnitRequestOptions, TWebApi, Task>> executeApiMethod, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TWebApi, TModelData, TApiData>(Expression<Func<IApizrUnitRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TWebApi, TApiData>(Expression<Func<IApizrResultRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrResultRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TWebApi, TModelData, TApiData>(
            Expression<Func<IApizrResultRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);
        
        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TWebApi">The web api type</typeparam>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrResultRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #endregion
    }

    /// <summary>
    /// <see cref="IApizrOptionalMediator"/> but dedicated to <typeparamref name="TWebApi"/> with optional result, getting all shorter
    /// </summary>
    public interface IApizrOptionalMediator<TWebApi> : IApizrOptionalMediatorBase
    {
        #region Unit

        #region SendFor<TWebApi>

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<TWebApi, Task>> executeApiMethod, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor(Expression<Func<IApizrUnitRequestOptions, TWebApi, Task>> executeApiMethod, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and returning an optional result
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with mapped request and returning an optional result
        /// </summary>
        /// <typeparam name="TModelData">The model request type to map from</typeparam>
        /// <typeparam name="TApiData">The api request type to map to</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        Task<Option<Unit, ApizrException>> SendFor<TModelData, TApiData>(Expression<Func<IApizrUnitRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #endregion

        #region Result

        #region SendFor<TWebApi, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR and returning an optional result
        /// </summary>
        /// <typeparam name="TApiData">The api result type</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TApiData, ApizrException<TApiData>>> SendFor<TApiData>(Expression<Func<IApizrResultRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #region SendFor<TWebApi, TModelData, TApiData>

        /// <summary>
        /// Send an api call to Apizr using MediatR returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map to</typeparam>
        /// <typeparam name="TApiData">The api result type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrResultRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelData">The mapped model type to map request from and result to</typeparam>
        /// <typeparam name="TApiData">The api result type to map request to and result from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelData">The model data to map</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelData, ApizrException<TModelData>>> SendFor<TModelData, TApiData>(
            Expression<Func<IApizrResultRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #region SendFor<TWebApi, TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Send an api call to Apizr using MediatR with a mapped request and returning an optional mapped result
        /// </summary>
        /// <typeparam name="TModelResultData">The model result type to map to</typeparam>
        /// <typeparam name="TApiRequestData">The api result type to map from</typeparam>
        /// <typeparam name="TApiResultData">The api request type to map to</typeparam>
        /// <typeparam name="TModelRequestData">The model request type to map from</typeparam>
        /// <param name="executeApiMethod">The <typeparamref name="TWebApi"/> call to execute</param>
        /// <param name="modelRequestData">The model request data</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        /// <returns></returns>
        Task<Option<TModelResultData, ApizrException<TModelResultData>>> SendFor<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrResultRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrResultRequestOptionsBuilder> optionsBuilder = null);

        #endregion

        #endregion
    }
}
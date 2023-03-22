using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Mediation.Requesting.Base;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Requesting.Base
{
    /// <summary>
    /// The top level base mediation execute optional unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TModelData">The model data type</typeparam>
    /// <typeparam name="TApiData">The api data type</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteOptionalUnitRequestBase<TWebApi, TModelData, TApiData, TApizrRequestOptions, TApizrRequestOptionsBuilder> : 
        ExecuteUnitRequestBase<TWebApi, TModelData, TApiData, Option<Unit, ApizrException>, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalUnitRequestBase(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalUnitRequestBase(Expression<Func<TApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute optional unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ExecuteOptionalUnitRequestBase<TWebApi, TApizrRequestOptions, TApizrRequestOptionsBuilder> : ExecuteUnitRequestBase<TWebApi, Option<Unit, ApizrException>, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalUnitRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalUnitRequestBase(Expression<Func<TApizrRequestOptions, TWebApi, Task>> executeApiMethod, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
    }
}

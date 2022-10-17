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
    public abstract class ExecuteOptionalUnitRequestBase<TWebApi, TModelData, TApiData> : ExecuteUnitRequestBase<TWebApi, TModelData, TApiData, Option<Unit, ApizrException>>
    {
        /// <summary>
        /// The top level base mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalUnitRequestBase(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="modelData">The data provided to the request</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalUnitRequestBase(Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, modelData, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The top level base mediation execute optional unit request (returning no result)
    /// </summary>
    /// <typeparam name="TWebApi">The web api type</typeparam>
    public abstract class ExecuteOptionalUnitRequestBase<TWebApi> : ExecuteUnitRequestBase<TWebApi, Option<Unit, ApizrException>>
    {
        /// <summary>
        /// The top level base mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalUnitRequestBase(Expression<Func<TWebApi, Task>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }

        /// <summary>
        /// The top level base mediation execute optional unit request constructor
        /// </summary>
        /// <param name="executeApiMethod">The request to execute</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ExecuteOptionalUnitRequestBase(Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(executeApiMethod, optionsBuilder)
        {
        }
    }
}

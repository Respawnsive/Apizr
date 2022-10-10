using System;
using System.Collections.Generic;
using Apizr.Configuring.Request;
using Apizr.Mediation.Querying;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    /// <summary>
    /// The top level base mediation ReadAll query
    /// </summary>
    /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public abstract class ReadAllQueryBase<TReadAllParams, TReadAllResult> : MediationQueryBase<TReadAllResult>
    {
        /// <summary>
        /// The top level base mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ReadAllQueryBase(TReadAllParams parameters, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// The top level base mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ReadAllQueryBase(TReadAllParams parameters, int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(priority, optionsBuilder)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// The query parameters to send
        /// </summary>
        public TReadAllParams Parameters { get; }
    }

    /// <summary>
    /// The top level base mediation ReadAll query
    /// </summary>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public abstract class ReadAllQueryBase<TReadAllResult> : ReadAllQueryBase<IDictionary<string, object>, TReadAllResult>
    {
        /// <inheritdoc />
        protected ReadAllQueryBase(IDictionary<string, object> parameters, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, priority, optionsBuilder)
        {
        }
    }
}

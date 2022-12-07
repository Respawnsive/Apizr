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
    public abstract class ReadAllQueryBase<TReadAllParams, TReadAllResult, TApizrRequestOptions, TApizrRequestOptionsBuilder> : MediationQueryBase<TReadAllResult, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ReadAllQueryBase(TReadAllParams parameters, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
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
    public abstract class ReadAllQueryBase<TReadAllResult, TApizrRequestOptions, TApizrRequestOptionsBuilder> : ReadAllQueryBase<IDictionary<string, object>, TReadAllResult, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected ReadAllQueryBase(IDictionary<string, object> parameters, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {
        }
    }
}

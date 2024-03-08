using System;
using System.Collections.Generic;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    /// <summary>
    /// The mediation ReadAll query
    /// </summary>
    /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public class SafeReadAllQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, IApizrResponse<TReadAllResult>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public SafeReadAllQuery(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(default, optionsBuilder)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public SafeReadAllQuery(TReadAllParams parameters, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The mediation ReadAll query
    /// </summary>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public class SafeReadAllQuery<TReadAllResult> : ReadAllQueryBase<IApizrResponse<TReadAllResult>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public SafeReadAllQuery(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(default, optionsBuilder)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public SafeReadAllQuery(IDictionary<string, object> parameters, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {
        }
    }
}

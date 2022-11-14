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
    public class ReadAllQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, TReadAllResult, IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) : base(default, optionsBuilder)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(TReadAllParams parameters, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The mediation ReadAll query
    /// </summary>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public class ReadAllQuery<TReadAllResult> : ReadAllQueryBase<TReadAllResult, IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) : base(default, optionsBuilder)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(IDictionary<string, object> parameters, Action<IApizrCatchResultRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {
        }
    }
}

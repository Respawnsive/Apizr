using System;
using System.Collections.Generic;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    /// <summary>
    /// The mediation ReadAll optional query
    /// </summary>
    /// <typeparam name="TReadAllParams">The ReadAll parameters type</typeparam>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public class ReadAllOptionalQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, Option<TReadAllResult, ApizrException<TReadAllResult>>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllOptionalQuery(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(default, optionsBuilder)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllOptionalQuery(TReadAllParams parameters, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The mediation ReadAll optional query
    /// </summary>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public class ReadAllOptionalQuery<TReadAllResult> : ReadAllQueryBase<Option<TReadAllResult, ApizrException<TReadAllResult>>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllOptionalQuery(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(default, optionsBuilder)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllOptionalQuery(IDictionary<string, object> parameters, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {

        }
    }
}

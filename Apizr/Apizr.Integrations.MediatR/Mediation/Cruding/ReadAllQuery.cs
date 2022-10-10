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
    public class ReadAllQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, TReadAllResult>
    {
        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(default, optionsBuilder)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(TReadAllParams parameters, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {

        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(default, priority, optionsBuilder)
        {

        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(TReadAllParams parameters, int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, priority, optionsBuilder)
        {

        }
    }

    /// <summary>
    /// The mediation ReadAll query
    /// </summary>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public class ReadAllQuery<TReadAllResult> : ReadAllQueryBase<TReadAllResult>
    {
        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(default, optionsBuilder)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(IDictionary<string, object> parameters, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(default, priority, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadAllQuery(IDictionary<string, object> parameters, int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(parameters, priority, optionsBuilder)
        {
        }
    }
}

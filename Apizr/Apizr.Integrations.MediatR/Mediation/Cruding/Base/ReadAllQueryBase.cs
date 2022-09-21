using System;
using System.Collections.Generic;
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
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ReadAllQueryBase(TReadAllParams parameters, bool clearCache, Action<Exception> onException = null) : base(clearCache, onException)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// The top level base mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ReadAllQueryBase(TReadAllParams parameters, int priority, bool clearCache, Action<Exception> onException = null) : base(priority, clearCache, onException)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// The top level base mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ReadAllQueryBase(TReadAllParams parameters, Context context, bool clearCache, Action<Exception> onException = null) : base(context, clearCache, onException)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// The top level base mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ReadAllQueryBase(TReadAllParams parameters, int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(priority, context, clearCache, onException)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// The query parameters to send
        /// </summary>
        public TReadAllParams Parameters { get; }
    }

    /// <inheritdoc />
    public abstract class ReadAllQueryBase<TReadAllResult> : ReadAllQueryBase<IDictionary<string, object>, TReadAllResult>
    {
        /// <inheritdoc />
        protected ReadAllQueryBase(IDictionary<string, object> parameters, bool clearCache, Action<Exception> onException = null) : base(parameters, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority, bool clearCache, Action<Exception> onException = null) : base(parameters, priority, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ReadAllQueryBase(IDictionary<string, object> parameters, Context context, bool clearCache, Action<Exception> onException = null) : base(parameters, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(parameters, priority, context, clearCache, onException)
        {
        }
    }
}

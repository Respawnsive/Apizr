using System;
using System.Collections.Generic;
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
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(bool clearCache = false, Action<Exception> onException = null) : base(default, clearCache, onException)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(TReadAllParams parameters, bool clearCache = false, Action<Exception> onException = null) : base(parameters, clearCache, onException)
        {

        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(int priority, bool clearCache = false, Action<Exception> onException = null) : base(default, priority, clearCache, onException)
        {

        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(Context context, bool clearCache = false, Action<Exception> onException = null) : base(default, context, clearCache, onException)
        {

        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(TReadAllParams parameters, int priority, bool clearCache = false, Action<Exception> onException = null) : base(parameters, priority, clearCache, onException)
        {

        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(TReadAllParams parameters, Context context, bool clearCache = false, Action<Exception> onException = null) : base(parameters, context, clearCache, onException)
        {

        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(default, priority, context, clearCache, onException)
        {

        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(TReadAllParams parameters, int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(parameters, priority, context, clearCache, onException)
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
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(bool clearCache = false, Action<Exception> onException = null) : base(default, clearCache, onException)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(IDictionary<string, object> parameters, bool clearCache = false, Action<Exception> onException = null) : base(parameters, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(int priority, bool clearCache = false, Action<Exception> onException = null) : base(default, priority, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(Context context, bool clearCache = false, Action<Exception> onException = null) : base(default, context, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(IDictionary<string, object> parameters, int priority, bool clearCache = false, Action<Exception> onException = null) : base(parameters, priority, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(IDictionary<string, object> parameters, Context context, bool clearCache = false, Action<Exception> onException = null) : base(parameters, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(default, priority, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation ReadAll query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadAllQuery(IDictionary<string, object> parameters, int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(parameters, priority, context, clearCache)
        {
        }
    }
}

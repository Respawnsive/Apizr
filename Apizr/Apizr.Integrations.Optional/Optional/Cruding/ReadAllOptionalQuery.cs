using System.Collections.Generic;
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
    public class ReadAllOptionalQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(bool clearCache = false) : base(default, clearCache)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(TReadAllParams parameters, bool clearCache = false) : base(parameters, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(int priority, bool clearCache = false) : base(default, priority, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(Context context, bool clearCache = false) : base(default, context, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(TReadAllParams parameters, int priority, bool clearCache = false) : base(parameters, priority, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(TReadAllParams parameters, Context context, bool clearCache = false) : base(parameters, context, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(int priority, Context context, bool clearCache = false) : base(default, priority, context, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(TReadAllParams parameters, int priority, Context context, bool clearCache = false) : base(parameters, priority, context, clearCache)
        {

        }
    }

    /// <summary>
    /// The mediation ReadAll optional query
    /// </summary>
    /// <typeparam name="TReadAllResult">The api result type</typeparam>
    public class ReadAllOptionalQuery<TReadAllResult> : ReadAllQueryBase<Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(bool clearCache = false) : base(default, clearCache)
        {
            
        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(IDictionary<string, object> parameters, bool clearCache = false) : base(parameters, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(int priority, bool clearCache = false) : base(default, priority, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(Context context, bool clearCache = false) : base(default, context, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(IDictionary<string, object> parameters, int priority, bool clearCache = false) : base(parameters, priority, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(IDictionary<string, object> parameters, Context context, bool clearCache = false) : base(parameters, context, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(int priority, Context context, bool clearCache = false) : base(default, priority, context, clearCache)
        {

        }

        /// <summary>
        /// The mediation ReadAll optional query constructor
        /// </summary>
        /// <param name="parameters">The query parameters to send</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadAllOptionalQuery(IDictionary<string, object> parameters, int priority, Context context, bool clearCache = false) : base(parameters, priority, context, clearCache)
        {

        }
    }
}

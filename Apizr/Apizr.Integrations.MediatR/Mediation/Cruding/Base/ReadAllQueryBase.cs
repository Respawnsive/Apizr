using System;
using System.Collections.Generic;
using Apizr.Mediation.Querying;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadAllQueryBase<TReadAllParams, TReadAllResult> : MediationQueryBase<TReadAllResult>
    {
        protected ReadAllQueryBase(TReadAllParams parameters, bool clearCache, Action<Exception> onException = null) : base(clearCache, onException)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, int priority, bool clearCache, Action<Exception> onException = null) : base(priority, clearCache, onException)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, Context context, bool clearCache, Action<Exception> onException = null) : base(context, clearCache, onException)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(priority, context, clearCache, onException)
        {
            Parameters = parameters;
        }

        public TReadAllParams Parameters { get; }
    }

    public abstract class ReadAllQueryBase<TReadAllResult> : ReadAllQueryBase<IDictionary<string, object>, TReadAllResult>
    {
        protected ReadAllQueryBase(IDictionary<string, object> parameters, bool clearCache, Action<Exception> onException = null) : base(parameters, clearCache, onException)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority, bool clearCache, Action<Exception> onException = null) : base(parameters, priority, clearCache, onException)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, Context context, bool clearCache, Action<Exception> onException = null) : base(parameters, context, clearCache, onException)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(parameters, priority, context, clearCache, onException)
        {
        }
    }
}

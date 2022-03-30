using System;
using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class ReadAllQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, TReadAllResult>
    {
        public ReadAllQuery(bool clearCache = false, Action<Exception> onException = null) : base(default, clearCache, onException)
        {
            
        }

        public ReadAllQuery(TReadAllParams parameters, bool clearCache = false, Action<Exception> onException = null) : base(parameters, clearCache, onException)
        {

        }

        public ReadAllQuery(int priority, bool clearCache = false, Action<Exception> onException = null) : base(default, priority, clearCache, onException)
        {

        }

        public ReadAllQuery(Context context, bool clearCache = false, Action<Exception> onException = null) : base(default, context, clearCache, onException)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, int priority, bool clearCache = false, Action<Exception> onException = null) : base(parameters, priority, clearCache, onException)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, Context context, bool clearCache = false, Action<Exception> onException = null) : base(parameters, context, clearCache, onException)
        {

        }

        public ReadAllQuery(int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(default, priority, context, clearCache, onException)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(parameters, priority, context, clearCache, onException)
        {

        }
    }

    public class ReadAllQuery<TReadAllResult> : ReadAllQueryBase<TReadAllResult>
    {
        public ReadAllQuery(bool clearCache = false, Action<Exception> onException = null) : base(default, clearCache, onException)
        {
            
        }

        public ReadAllQuery(IDictionary<string, object> parameters, bool clearCache = false, Action<Exception> onException = null) : base(parameters, clearCache, onException)
        {
        }

        public ReadAllQuery(int priority, bool clearCache = false, Action<Exception> onException = null) : base(default, priority, clearCache, onException)
        {
        }

        public ReadAllQuery(Context context, bool clearCache = false, Action<Exception> onException = null) : base(default, context, clearCache, onException)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, int priority, bool clearCache = false, Action<Exception> onException = null) : base(parameters, priority, clearCache, onException)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, Context context, bool clearCache = false, Action<Exception> onException = null) : base(parameters, context, clearCache)
        {
        }

        public ReadAllQuery(int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(default, priority, context, clearCache)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(parameters, priority, context, clearCache)
        {
        }
    }
}

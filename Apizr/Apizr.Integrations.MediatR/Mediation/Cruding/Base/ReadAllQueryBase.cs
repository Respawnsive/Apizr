using System.Collections.Generic;
using Apizr.Mediation.Querying;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadAllQueryBase<TReadAllParams, TReadAllResult> : MediationQueryBase<TReadAllResult>
    {
        protected ReadAllQueryBase(TReadAllParams parameters, bool clearCache) : base(clearCache)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, int priority, bool clearCache) : base(priority, clearCache)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, Context context, bool clearCache) : base(context, clearCache)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, int priority, Context context, bool clearCache) : base(priority, context, clearCache)
        {
            Parameters = parameters;
        }

        public TReadAllParams Parameters { get; }
    }

    public abstract class ReadAllQueryBase<TReadAllResult> : ReadAllQueryBase<IDictionary<string, object>, TReadAllResult>
    {
        protected ReadAllQueryBase(IDictionary<string, object> parameters, bool clearCache) : base(parameters, clearCache)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority, bool clearCache) : base(parameters, priority, clearCache)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, Context context, bool clearCache) : base(parameters, context, clearCache)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority, Context context, bool clearCache) : base(parameters, priority, context, clearCache)
        {
        }
    }
}

using System.Collections.Generic;
using Apizr.Mediation.Querying;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadAllQueryBase<TReadAllParams, TReadAllResult> : MediationQueryBase<TReadAllResult>
    {
        protected ReadAllQueryBase(TReadAllParams parameters)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, int priority) : base(priority)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, Context context) : base(context)
        {
            Parameters = parameters;
        }

        protected ReadAllQueryBase(TReadAllParams parameters, int priority, Context context) : base(priority, context)
        {
            Parameters = parameters;
        }

        public TReadAllParams Parameters { get; }
    }

    public abstract class ReadAllQueryBase<TReadAllResult> : ReadAllQueryBase<IDictionary<string, object>, TReadAllResult>
    {
        protected ReadAllQueryBase(IDictionary<string, object> parameters) : base(parameters)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority) : base(parameters, priority)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, Context context) : base(parameters, context)
        {
        }

        protected ReadAllQueryBase(IDictionary<string, object> parameters, int priority, Context context) : base(parameters, priority, context)
        {
        }
    }
}

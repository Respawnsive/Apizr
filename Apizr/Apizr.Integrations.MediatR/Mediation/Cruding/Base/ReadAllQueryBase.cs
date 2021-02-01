using System.Collections.Generic;
using Apizr.Mediation.Querying;

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
    }
}

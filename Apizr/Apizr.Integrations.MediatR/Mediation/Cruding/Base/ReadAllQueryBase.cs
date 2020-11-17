using System.Collections.Generic;
using Apizr.Mediation.Querying;
using Fusillade;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadAllQueryBase<TReadAllParams, TReadAllResult> : MediationQueryBase<TReadAllResult>
    {
        protected ReadAllQueryBase(TReadAllParams parameters, Priority priority = Priority.UserInitiated) : base(priority)
        {
            Parameters = parameters;
        }

        public TReadAllParams Parameters { get; }
    }

    public abstract class ReadAllQueryBase<TReadAllResult> : ReadAllQueryBase<IDictionary<string, object>, TReadAllResult>
    {
        protected ReadAllQueryBase(IDictionary<string, object> parameters, Priority priority = Priority.UserInitiated) : base(parameters, priority)
        {
        }
    }
}

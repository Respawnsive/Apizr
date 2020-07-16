using System.Collections.Generic;
using Apizr.Mediation.Querying;
using Fusillade;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadAllQueryBase<TReadAllParams, TReadAllResult> : IQuery<TReadAllResult>
    {
        protected ReadAllQueryBase(TReadAllParams parameters, Priority priority = Priority.UserInitiated)
        {
            Parameters = parameters;
            Priority = priority;
        }

        public TReadAllParams Parameters { get; }
        public Priority Priority { get; }
    }

    public abstract class ReadAllQueryBase<TReadAllResult> : ReadAllQueryBase<IDictionary<string, object>, TReadAllResult>
    {
        protected ReadAllQueryBase(IDictionary<string, object> parameters, Priority priority = Priority.UserInitiated) : base(parameters, priority)
        {
        }
    }
}

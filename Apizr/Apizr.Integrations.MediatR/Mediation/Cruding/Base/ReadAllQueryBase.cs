using System.Collections.Generic;
using System.Linq;
using Apizr.Mediation.Querying;

namespace Apizr.Mediation.Cruding.Base
{
    public abstract class ReadAllQueryBase<TReadAllParams, TReadAllResult> : IQuery<TReadAllResult>
    {
        protected ReadAllQueryBase(TReadAllParams parameters)
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
    }
}

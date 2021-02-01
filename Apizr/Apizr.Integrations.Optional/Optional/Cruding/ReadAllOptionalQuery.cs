using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class ReadAllOptionalQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        public ReadAllOptionalQuery() : base(default)
        {
            
        }

        public ReadAllOptionalQuery(TReadAllParams parameters) : base(parameters)
        {

        }

        public ReadAllOptionalQuery(int priority) : base(default, priority)
        {

        }

        public ReadAllOptionalQuery(TReadAllParams parameters, int priority) : base(parameters, priority)
        {

        }
    }

    public class ReadAllOptionalQuery<TReadAllResult> : ReadAllQueryBase<Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        public ReadAllOptionalQuery() : base(default)
        {
            
        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters) : base(parameters)
        {

        }

        public ReadAllOptionalQuery(int priority) : base(default, priority)
        {

        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters, int priority) : base(parameters, priority)
        {

        }
    }
}

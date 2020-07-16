using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Fusillade;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class ReadAllOptionalQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        public ReadAllOptionalQuery(TReadAllParams parameters = default, Priority priority = Priority.UserInitiated) : base(parameters, priority)
        {

        }
    }

    public class ReadAllOptionalQuery<TReadAllResult> : ReadAllQueryBase<Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        public ReadAllOptionalQuery(IDictionary<string, object> parameters = default, Priority priority = Priority.UserInitiated) : base(parameters, priority)
        {

        }
    }
}

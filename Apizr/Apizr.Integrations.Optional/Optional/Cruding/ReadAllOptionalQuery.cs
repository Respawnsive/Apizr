using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Optional;

namespace Apizr.Optional.Cruding
{
    public class ReadAllOptionalQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        public ReadAllOptionalQuery(TReadAllParams parameters = default) : base(parameters)
        {

        }
    }

    public class ReadAllOptionalQuery<TReadAllResult> : ReadAllQueryBase<Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        public ReadAllOptionalQuery(IDictionary<string, object> parameters = default) : base(parameters)
        {

        }
    }
}

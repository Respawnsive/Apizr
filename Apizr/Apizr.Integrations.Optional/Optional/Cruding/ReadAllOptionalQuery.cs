using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

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

        public ReadAllOptionalQuery(Context context) : base(default, context)
        {

        }

        public ReadAllOptionalQuery(TReadAllParams parameters, int priority) : base(parameters, priority)
        {

        }

        public ReadAllOptionalQuery(TReadAllParams parameters, Context context) : base(parameters, context)
        {

        }

        public ReadAllOptionalQuery(int priority, Context context) : base(default, priority, context)
        {

        }

        public ReadAllOptionalQuery(TReadAllParams parameters, int priority, Context context) : base(parameters, priority, context)
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

        public ReadAllOptionalQuery(Context context) : base(default, context)
        {

        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters, int priority) : base(parameters, priority)
        {

        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters, Context context) : base(parameters, context)
        {

        }

        public ReadAllOptionalQuery(int priority, Context context) : base(default, priority, context)
        {

        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters, int priority, Context context) : base(parameters, priority, context)
        {

        }
    }
}

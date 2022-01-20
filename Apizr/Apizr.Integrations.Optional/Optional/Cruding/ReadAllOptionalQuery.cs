using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    public class ReadAllOptionalQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        public ReadAllOptionalQuery(bool clearCache = false) : base(default, clearCache)
        {
            
        }

        public ReadAllOptionalQuery(TReadAllParams parameters, bool clearCache = false) : base(parameters, clearCache)
        {

        }

        public ReadAllOptionalQuery(int priority, bool clearCache = false) : base(default, priority, clearCache)
        {

        }

        public ReadAllOptionalQuery(Context context, bool clearCache = false) : base(default, context, clearCache)
        {

        }

        public ReadAllOptionalQuery(TReadAllParams parameters, int priority, bool clearCache = false) : base(parameters, priority, clearCache)
        {

        }

        public ReadAllOptionalQuery(TReadAllParams parameters, Context context, bool clearCache = false) : base(parameters, context, clearCache)
        {

        }

        public ReadAllOptionalQuery(int priority, Context context, bool clearCache = false) : base(default, priority, context, clearCache)
        {

        }

        public ReadAllOptionalQuery(TReadAllParams parameters, int priority, Context context, bool clearCache = false) : base(parameters, priority, context, clearCache)
        {

        }
    }

    public class ReadAllOptionalQuery<TReadAllResult> : ReadAllQueryBase<Option<TReadAllResult, ApizrException<TReadAllResult>>>
    {
        public ReadAllOptionalQuery(bool clearCache = false) : base(default, clearCache)
        {
            
        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters, bool clearCache = false) : base(parameters, clearCache)
        {

        }

        public ReadAllOptionalQuery(int priority, bool clearCache = false) : base(default, priority, clearCache)
        {

        }

        public ReadAllOptionalQuery(Context context, bool clearCache = false) : base(default, context, clearCache)
        {

        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters, int priority, bool clearCache = false) : base(parameters, priority, clearCache)
        {

        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters, Context context, bool clearCache = false) : base(parameters, context, clearCache)
        {

        }

        public ReadAllOptionalQuery(int priority, Context context, bool clearCache = false) : base(default, priority, context, clearCache)
        {

        }

        public ReadAllOptionalQuery(IDictionary<string, object> parameters, int priority, Context context, bool clearCache = false) : base(parameters, priority, context, clearCache)
        {

        }
    }
}

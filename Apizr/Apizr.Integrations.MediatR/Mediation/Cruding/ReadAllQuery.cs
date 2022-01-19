using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class ReadAllQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, TReadAllResult>
    {
        public ReadAllQuery(bool clearCache = false) : base(default, clearCache)
        {
            
        }

        public ReadAllQuery(TReadAllParams parameters, bool clearCache = false) : base(parameters, clearCache)
        {

        }

        public ReadAllQuery(int priority, bool clearCache = false) : base(default, priority, clearCache)
        {

        }

        public ReadAllQuery(Context context, bool clearCache = false) : base(default, context, clearCache)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, int priority, bool clearCache = false) : base(parameters, priority, clearCache)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, Context context, bool clearCache = false) : base(parameters, context, clearCache)
        {

        }

        public ReadAllQuery(int priority, Context context, bool clearCache = false) : base(default, priority, context, clearCache)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, int priority, Context context, bool clearCache = false) : base(parameters, priority, context, clearCache)
        {

        }
    }

    public class ReadAllQuery<TReadAllResult> : ReadAllQueryBase<TReadAllResult>
    {
        public ReadAllQuery(bool clearCache = false) : base(default, clearCache)
        {
            
        }

        public ReadAllQuery(IDictionary<string, object> parameters, bool clearCache = false) : base(parameters, clearCache)
        {
        }

        public ReadAllQuery(int priority, bool clearCache = false) : base(default, priority, clearCache)
        {
        }

        public ReadAllQuery(Context context, bool clearCache = false) : base(default, context, clearCache)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, int priority, bool clearCache = false) : base(parameters, priority, clearCache)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, Context context, bool clearCache = false) : base(parameters, context, clearCache)
        {
        }

        public ReadAllQuery(int priority, Context context, bool clearCache = false) : base(default, priority, context, clearCache)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, int priority, Context context, bool clearCache = false) : base(parameters, priority, context, clearCache)
        {
        }
    }
}

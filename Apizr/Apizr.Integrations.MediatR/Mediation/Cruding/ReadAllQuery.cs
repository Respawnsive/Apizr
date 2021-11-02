using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    public class ReadAllQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, TReadAllResult>
    {
        public ReadAllQuery() : base(default)
        {
            
        }

        public ReadAllQuery(TReadAllParams parameters) : base(parameters)
        {

        }

        public ReadAllQuery(int priority) : base(default, priority)
        {

        }

        public ReadAllQuery(Context context) : base(default, context)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, int priority) : base(parameters, priority)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, Context context) : base(parameters, context)
        {

        }

        public ReadAllQuery(int priority, Context context) : base(default, priority, context)
        {

        }

        public ReadAllQuery(TReadAllParams parameters, int priority, Context context) : base(parameters, priority, context)
        {

        }
    }

    public class ReadAllQuery<TReadAllResult> : ReadAllQueryBase<TReadAllResult>
    {
        public ReadAllQuery() : base(default)
        {
            
        }

        public ReadAllQuery(IDictionary<string, object> parameters) : base(parameters)
        {
        }

        public ReadAllQuery(int priority) : base(default, priority)
        {
        }

        public ReadAllQuery(Context context) : base(default, context)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, int priority) : base(parameters, priority)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, Context context) : base(parameters, context)
        {
        }

        public ReadAllQuery(int priority, Context context) : base(default, priority, context)
        {
        }

        public ReadAllQuery(IDictionary<string, object> parameters, int priority, Context context) : base(parameters, priority, context)
        {
        }
    }
}

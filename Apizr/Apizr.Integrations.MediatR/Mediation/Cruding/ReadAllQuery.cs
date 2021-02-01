using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;

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

        public ReadAllQuery(TReadAllParams parameters, int priority) : base(parameters, priority)
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

        public ReadAllQuery(IDictionary<string, object> parameters, int priority) : base(parameters, priority)
        {
        }
    }
}

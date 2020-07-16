using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;
using Fusillade;

namespace Apizr.Mediation.Cruding
{
    public class ReadAllQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, TReadAllResult>
    {
        public ReadAllQuery(TReadAllParams parameters = default, Priority priority = Priority.UserInitiated) : base(parameters, priority)
        {
            
        }
    }

    public class ReadAllQuery<TReadAllResult> : ReadAllQueryBase<TReadAllResult>
    {
        public ReadAllQuery(IDictionary<string, object> parameters = default, Priority priority = Priority.UserInitiated) : base(parameters, priority)
        {
        }
    }
}

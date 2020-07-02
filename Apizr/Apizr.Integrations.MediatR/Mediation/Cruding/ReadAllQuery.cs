using System.Collections.Generic;
using Apizr.Mediation.Cruding.Base;

namespace Apizr.Mediation.Cruding
{
    public class ReadAllQuery<TReadAllParams, TReadAllResult> : ReadAllQueryBase<TReadAllParams, TReadAllResult>
    {
        public ReadAllQuery(TReadAllParams parameters = default) : base(parameters)
        {
            
        }
    }

    public class ReadAllQuery<TReadAllResult> : ReadAllQueryBase<TReadAllResult>
    {
        public ReadAllQuery(IDictionary<string, object> parameters = default) : base(parameters)
        {
        }
    }
}

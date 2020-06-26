using System.Collections.Generic;
using System.Linq;
using Apizr.Mediation.Querying;

namespace Apizr.Mediation.Cruding
{
    public class ReadAllQuery<TResponse> : IQuery<TResponse>
    {
        public ReadAllQuery(params KeyValuePair<string, object>[] parameters)
        {
            Parameters = parameters?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? new Dictionary<string, object>();
        }

        public IDictionary<string, object> Parameters { get; }
    }
}

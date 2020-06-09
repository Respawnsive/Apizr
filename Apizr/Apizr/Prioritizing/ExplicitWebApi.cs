using System;
using Fusillade;

namespace Apizr.Prioritizing
{
    public class ExplicitWebApi<T> : LazyPrioritizedWebApi<T>
    {
        public ExplicitWebApi(Func<T> valueFactory) : base(Priority.Explicit, valueFactory)
        {
        }

        public ExplicitWebApi(Func<object> valueFactory) : base(Priority.Explicit, valueFactory)
        {
        }
    }
}
using System;
using Fusillade;

namespace Apizr.Prioritizing
{
    public class SpeculativeWebApi<T> : LazyPrioritizedWebApi<T>
    {
        public SpeculativeWebApi(Func<T> valueFactory) : base(Priority.Speculative, valueFactory)
        {
        }

        public SpeculativeWebApi(Func<object> valueFactory) : base(Priority.Speculative, valueFactory)
        {
        }
    }
}
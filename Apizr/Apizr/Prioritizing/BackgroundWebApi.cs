using System;
using Fusillade;

namespace Apizr.Prioritizing
{
    public class BackgroundWebApi<T> : LazyPrioritizedWebApi<T>
    {
        public BackgroundWebApi(Func<T> valueFactory) : base(Priority.Background, valueFactory)
        {
        }

        public BackgroundWebApi(Func<object> valueFactory) : base(Priority.Background, valueFactory)
        {
        }
    }
}
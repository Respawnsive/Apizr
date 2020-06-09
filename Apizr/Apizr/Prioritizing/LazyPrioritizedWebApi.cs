using System;
using Fusillade;

namespace Apizr.Prioritizing
{
    public abstract class LazyPrioritizedWebApi<T> : Lazy<T>, ILazyPrioritizedWebApi<T>
    {
        protected LazyPrioritizedWebApi(Priority priority, Func<T> valueFactory) : base(valueFactory)
        {
            Priority = priority;
        }

        protected LazyPrioritizedWebApi(Priority priority, Func<object> valueFactory) : base(() => (T)valueFactory.Invoke())
        {
            Priority = priority;
        }

        public Priority Priority { get; }
    }
}
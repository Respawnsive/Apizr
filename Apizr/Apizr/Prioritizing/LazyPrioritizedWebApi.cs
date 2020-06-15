using System;
using Fusillade;

namespace Apizr.Prioritizing
{
    public abstract class LazyPrioritizedWebApi<TWebApi> : Lazy<TWebApi>, ILazyPrioritizedWebApi<TWebApi>
    {
        protected LazyPrioritizedWebApi(Priority priority, Func<TWebApi> valueFactory) : base(valueFactory)
        {
            Priority = priority;
        }

        protected LazyPrioritizedWebApi(Priority priority, Func<object> valueFactory) : base(() => (TWebApi)valueFactory.Invoke())
        {
            Priority = priority;
        }

        public Priority Priority { get; }
    }
}
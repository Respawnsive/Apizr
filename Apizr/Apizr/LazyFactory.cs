using System;

namespace Apizr
{
    public class LazyFactory<TInstance> : Lazy<TInstance>, ILazyFactory<TInstance>
    {
        public LazyFactory(Func<TInstance> instanceFactory) : base(instanceFactory)
        {
        }

        public LazyFactory(Func<object> instanceFactory) : base(() => (TInstance)instanceFactory.Invoke())
        {
        }
    }
}
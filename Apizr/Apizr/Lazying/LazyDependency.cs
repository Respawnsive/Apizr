using System;

namespace Apizr.Lazying
{
    public class LazyDependency<T> : Lazy<T>, ILazyDependency<T>
    {
        public LazyDependency(Func<T> valueFactory) : base(valueFactory)
        {
            
        }

        public LazyDependency(Func<object> valueFactory) : base(() => (T)valueFactory.Invoke())
        {
        }
    }
}
using System;

namespace Apizr
{
    /// <inheritdoc cref="ILazyFactory{TInstance}"/>
    public class LazyFactory<TInstance> : Lazy<TInstance>, ILazyFactory<TInstance>
    {
        /// <summary>
        /// Lazy factory constructor
        /// </summary>
        /// <param name="instanceFactory">The factory to be lazy</param>
        public LazyFactory(Func<TInstance> instanceFactory) : base(instanceFactory)
        {
        }

        /// <summary>
        /// Lazy factory constructor
        /// </summary>
        /// <param name="instanceFactory">The factory to be lazy</param>
        public LazyFactory(Func<object> instanceFactory) : base(() => (TInstance)instanceFactory.Invoke())
        {
        }
    }
}
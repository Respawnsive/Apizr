using System;

namespace Apizr.Caching
{
    /// <summary>
    /// Tells Apizr to cache all methods returning result when decorating an interface or a specific one when decorating a method
    /// You have to provide an <see cref="ICacheHandler"/> mapping implementation to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class CacheItAttribute : CacheAttributeBase
    {
        public CacheItAttribute()
        {
        }

        public CacheItAttribute(CacheMode mode) : base(mode)
        {
        }

        public CacheItAttribute(string lifeSpanRepresentation) : base(lifeSpanRepresentation)
        {
        }

        public CacheItAttribute(bool shouldInvalidateOnError) : base(shouldInvalidateOnError)
        {
        }

        public CacheItAttribute(CacheMode mode, string lifeSpanRepresentation) : base(mode, lifeSpanRepresentation)
        {
        }

        public CacheItAttribute(CacheMode mode, bool shouldInvalidateOnError) : base(mode, shouldInvalidateOnError)
        {
        }

        public CacheItAttribute(TimeSpan lifeSpan, bool shouldInvalidateOnError) : base(lifeSpan, shouldInvalidateOnError)
        {
        }

        public CacheItAttribute(CacheMode mode, string lifeSpanRepresentation, bool shouldInvalidateOnError) : base(mode, lifeSpanRepresentation, shouldInvalidateOnError)
        {
        }
    }
}

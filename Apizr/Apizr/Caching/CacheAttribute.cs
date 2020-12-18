using System;

namespace Apizr.Caching
{
    /// <summary>
    /// Tells Apizr to cache all methods returning result when decorating an interface or a specific one when decorating a method
    /// You have to provide an <see cref="ICacheHandler"/> mapping implementation to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class CacheAttribute : CacheAttributeBase
    {
        public CacheAttribute()
        {
        }

        public CacheAttribute(CacheMode mode) : base(mode)
        {
        }

        public CacheAttribute(string lifeSpanRepresentation) : base(lifeSpanRepresentation)
        {
        }

        public CacheAttribute(bool shouldInvalidateOnError) : base(shouldInvalidateOnError)
        {
        }

        public CacheAttribute(CacheMode mode, string lifeSpanRepresentation) : base(mode, lifeSpanRepresentation)
        {
        }

        public CacheAttribute(CacheMode mode, bool shouldInvalidateOnError) : base(mode, shouldInvalidateOnError)
        {
        }

        public CacheAttribute(TimeSpan lifeSpan, bool shouldInvalidateOnError) : base(lifeSpan, shouldInvalidateOnError)
        {
        }

        public CacheAttribute(CacheMode mode, string lifeSpanRepresentation, bool shouldInvalidateOnError) : base(mode, lifeSpanRepresentation, shouldInvalidateOnError)
        {
        }
    }
}

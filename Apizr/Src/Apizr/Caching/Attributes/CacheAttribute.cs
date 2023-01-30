using System;

namespace Apizr.Caching.Attributes
{
    /// <summary>
    /// Tells Apizr to cache all methods returning result when decorating an interface or a specific one when decorating a method
    /// You have to provide an <see cref="ICacheHandler"/> mapping implementation to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class CacheAttribute : CacheAttributeBase
    {
        /// <inheritdoc />
        public CacheAttribute()
        {
        }

        /// <inheritdoc />
        public CacheAttribute(CacheMode mode) : base(mode)
        {
        }

        /// <inheritdoc />
        public CacheAttribute(string lifeSpanRepresentation) : base(lifeSpanRepresentation)
        {
        }

        /// <inheritdoc />
        public CacheAttribute(bool shouldInvalidateOnError) : base(shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheAttribute(CacheMode mode, string lifeSpanRepresentation) : base(mode, lifeSpanRepresentation)
        {
        }

        /// <inheritdoc />
        public CacheAttribute(CacheMode mode, bool shouldInvalidateOnError) : base(mode, shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheAttribute(TimeSpan lifeSpan, bool shouldInvalidateOnError) : base(lifeSpan, shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheAttribute(CacheMode mode, string lifeSpanRepresentation, bool shouldInvalidateOnError) : base(mode, lifeSpanRepresentation, shouldInvalidateOnError)
        {
        }
    }
}

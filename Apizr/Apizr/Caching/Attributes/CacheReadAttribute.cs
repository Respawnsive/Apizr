using System;

namespace Apizr.Caching.Attributes
{
    /// <summary>
    /// Tells Apizr to cache Read method
    /// You have to provide an <see cref="ICacheHandler"/> mapping implementation to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheReadAttribute : CacheAttributeBase
    {
        /// <inheritdoc />
        public CacheReadAttribute()
        {
        }

        /// <inheritdoc />
        public CacheReadAttribute(CacheMode mode) : base(mode)
        {
        }

        /// <inheritdoc />
        public CacheReadAttribute(string lifeSpanRepresentation) : base(lifeSpanRepresentation)
        {
        }

        /// <inheritdoc />
        public CacheReadAttribute(bool shouldInvalidateOnError) : base(shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheReadAttribute(CacheMode mode, string lifeSpanRepresentation) : base(mode, lifeSpanRepresentation)
        {
        }

        /// <inheritdoc />
        public CacheReadAttribute(CacheMode mode, bool shouldInvalidateOnError) : base(mode, shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheReadAttribute(TimeSpan lifeSpan, bool shouldInvalidateOnError) : base(lifeSpan, shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheReadAttribute(CacheMode mode, string lifeSpanRepresentation, bool shouldInvalidateOnError) : base(mode, lifeSpanRepresentation, shouldInvalidateOnError)
        {
        }
    }
}

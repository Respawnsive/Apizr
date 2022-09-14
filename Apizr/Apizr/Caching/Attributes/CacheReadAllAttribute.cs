using System;

namespace Apizr.Caching.Attributes
{
    /// <summary>
    /// Tells Apizr to cache ReadAll method
    /// You have to provide an <see cref="ICacheHandler"/> mapping implementation to Apizr to use this feature
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheReadAllAttribute : CacheAttributeBase
    {
        /// <inheritdoc />
        public CacheReadAllAttribute()
        {
        }

        /// <inheritdoc />
        public CacheReadAllAttribute(CacheMode mode) : base(mode)
        {
        }

        /// <inheritdoc />
        public CacheReadAllAttribute(string lifeSpanRepresentation) : base(lifeSpanRepresentation)
        {
        }

        /// <inheritdoc />
        public CacheReadAllAttribute(bool shouldInvalidateOnError) : base(shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheReadAllAttribute(CacheMode mode, string lifeSpanRepresentation) : base(mode, lifeSpanRepresentation)
        {
        }

        /// <inheritdoc />
        public CacheReadAllAttribute(CacheMode mode, bool shouldInvalidateOnError) : base(mode, shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheReadAllAttribute(TimeSpan lifeSpan, bool shouldInvalidateOnError) : base(lifeSpan, shouldInvalidateOnError)
        {
        }

        /// <inheritdoc />
        public CacheReadAllAttribute(CacheMode mode, string lifeSpanRepresentation, bool shouldInvalidateOnError) : base(mode, lifeSpanRepresentation, shouldInvalidateOnError)
        {
        }
    }
}

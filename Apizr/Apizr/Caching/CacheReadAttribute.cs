using System;

namespace Apizr.Caching
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheReadAttribute : CacheAttributeBase
    {
        public CacheReadAttribute()
        {
        }

        public CacheReadAttribute(CacheMode mode) : base(mode)
        {
        }

        public CacheReadAttribute(string lifeSpanRepresentation) : base(lifeSpanRepresentation)
        {
        }

        public CacheReadAttribute(bool shouldInvalidateOnError) : base(shouldInvalidateOnError)
        {
        }

        public CacheReadAttribute(CacheMode mode, string lifeSpanRepresentation) : base(mode, lifeSpanRepresentation)
        {
        }

        public CacheReadAttribute(CacheMode mode, bool shouldInvalidateOnError) : base(mode, shouldInvalidateOnError)
        {
        }

        public CacheReadAttribute(TimeSpan lifeSpan, bool shouldInvalidateOnError) : base(lifeSpan, shouldInvalidateOnError)
        {
        }

        public CacheReadAttribute(CacheMode mode, string lifeSpanRepresentation, bool shouldInvalidateOnError) : base(mode, lifeSpanRepresentation, shouldInvalidateOnError)
        {
        }
    }
}

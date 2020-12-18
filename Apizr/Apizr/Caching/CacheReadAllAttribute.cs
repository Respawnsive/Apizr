using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Caching
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheReadAllAttribute : CacheAttributeBase
    {
        public CacheReadAllAttribute()
        {
        }

        public CacheReadAllAttribute(CacheMode mode) : base(mode)
        {
        }

        public CacheReadAllAttribute(string lifeSpanRepresentation) : base(lifeSpanRepresentation)
        {
        }

        public CacheReadAllAttribute(bool shouldInvalidateOnError) : base(shouldInvalidateOnError)
        {
        }

        public CacheReadAllAttribute(CacheMode mode, string lifeSpanRepresentation) : base(mode, lifeSpanRepresentation)
        {
        }

        public CacheReadAllAttribute(CacheMode mode, bool shouldInvalidateOnError) : base(mode, shouldInvalidateOnError)
        {
        }

        public CacheReadAllAttribute(TimeSpan lifeSpan, bool shouldInvalidateOnError) : base(lifeSpan, shouldInvalidateOnError)
        {
        }

        public CacheReadAllAttribute(CacheMode mode, string lifeSpanRepresentation, bool shouldInvalidateOnError) : base(mode, lifeSpanRepresentation, shouldInvalidateOnError)
        {
        }
    }
}

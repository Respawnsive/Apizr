using System;

namespace Apizr.Caching
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method)]
    public class CacheAttribute : Attribute
    {
        public TimeSpan? LifeSpan { get; }
        public CacheMode Mode { get; } = CacheMode.GetAndFetch;
        public bool ShouldInvalidateOnError { get; }

        public CacheAttribute()
        {
        }

        public CacheAttribute(CacheMode mode)
        {
            Mode = mode;
        }

        public CacheAttribute(TimeSpan lifeSpan)
        {
            LifeSpan = lifeSpan;
        }

        public CacheAttribute(bool shouldInvalidateOnError)
        {
            ShouldInvalidateOnError = shouldInvalidateOnError;
        }

        public CacheAttribute(CacheMode mode, TimeSpan lifeSpan)
        {
            Mode = mode;
            LifeSpan = lifeSpan;
        }

        public CacheAttribute(CacheMode mode, bool shouldInvalidateOnError)
        {
            Mode = mode;
            ShouldInvalidateOnError = shouldInvalidateOnError;
        }

        public CacheAttribute(TimeSpan lifeSpan, bool shouldInvalidateOnError)
        {
            LifeSpan = lifeSpan;
            ShouldInvalidateOnError = shouldInvalidateOnError;
        }

        public CacheAttribute(CacheMode mode, TimeSpan lifeSpan, bool shouldInvalidateOnError)
        {
            Mode = mode;
            LifeSpan = lifeSpan;
            ShouldInvalidateOnError = shouldInvalidateOnError;
        }
    }
}

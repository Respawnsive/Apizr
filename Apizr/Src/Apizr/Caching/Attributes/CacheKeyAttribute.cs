using System;

namespace Apizr.Caching.Attributes
{
    /// <summary>
    /// The decorated parameter will be used as cache key
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CacheKeyAttribute : Attribute
    {
        /// <summary>
        /// The decorated parameter will be used as cache key
        /// </summary>
        public CacheKeyAttribute()
        {

        }

        /// <summary>
        /// If you decorate a complex type as cache key, you may want to provide 
        /// its properties to include by name or to override its ToString() method.
        /// Otherwise, Apizr will take all its non-null properties.
        /// </summary>
        /// <param name="propertyNames">Properties to include as cache key (default: all non-null properties).</param>
        public CacheKeyAttribute(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }

        /// <summary>
        /// Properties of to include as cache key (default: all non-null properties).
        /// </summary>
        public string[] PropertyNames { get; }
    }
}

using System;

namespace Apizr.Caching
{
    /// <summary>
    /// Tells Apizr the key to cache value at
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CacheKeyAttribute : Attribute
    {
        /// <summary>
        /// The decorated parameter will be used as cache key (should be primitive otherwise ToString() method will be used, unless providing a property name)
        /// </summary>
        public CacheKeyAttribute()
        {

        }

        /// <summary>
        /// If you use non primitive type (like your ModelClass object) as Cache Primary key you should provide 
        /// property name of primitive primary Id, otherwise ToString() method will be used.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        public CacheKeyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}

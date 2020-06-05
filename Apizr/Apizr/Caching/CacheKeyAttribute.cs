using System;

namespace Apizr.Caching
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CacheKeyAttribute : Attribute
    {
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

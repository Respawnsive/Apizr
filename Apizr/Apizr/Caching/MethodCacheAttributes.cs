using System;

namespace Apizr.Caching
{
    internal class MethodCacheAttributes
    {
        public MethodCacheAttributes(CacheAttributeBase cacheAttribute, CacheKeyAttribute primaryKeyAttribute, string paramName,
            Type paramType, int paramOrder)
        {
            CacheAttribute = cacheAttribute;
            CachePrimaryKeyAttribute = primaryKeyAttribute;
            ParameterName = paramName;
            ParameterType = paramType;
            ParameterOrder = paramOrder;
        }

        public int ParameterOrder { get; }

        public CacheAttributeBase CacheAttribute { get; }

        public CacheKeyAttribute CachePrimaryKeyAttribute { get; }

        public string ParameterName { get; }

        public Type ParameterType { get; }
    }
}

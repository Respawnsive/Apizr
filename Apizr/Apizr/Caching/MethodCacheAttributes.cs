using System;

namespace Apizr.Caching
{
    internal class MethodCacheAttributes
    {
        public MethodCacheAttributes(CacheAttribute cacheAttribute, CacheKeyAttribute primaryKeyAttribute, string paramName,
            Type paramType, int paramOrder)
        {
            CacheAttribute = cacheAttribute;
            CachePrimaryKeyAttribute = primaryKeyAttribute;
            ParameterName = paramName;
            ParameterType = paramType;
            ParameterOrder = paramOrder;
        }

        public int ParameterOrder { get; }

        public CacheAttribute CacheAttribute { get; }

        public CacheKeyAttribute CachePrimaryKeyAttribute { get; }

        public string ParameterName { get; }

        public Type ParameterType { get; }
    }
}

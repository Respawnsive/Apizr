using System;
using System.Reflection;

namespace Apizr.Caching
{
    internal class MethodCacheDetails
    {
        public MethodCacheDetails(Type apiInterfaceType, MethodInfo methodInfo)
        {
            ApiInterfaceType = apiInterfaceType;
            MethodInfo = methodInfo;
        }

        public Type ApiInterfaceType { get; }

        public MethodInfo MethodInfo { get; }

        public CacheItAttribute CacheAttribute { get; internal set; }

        public override int GetHashCode() => ApiInterfaceType.GetHashCode() * 23 * MethodInfo.GetHashCode() * 23 * 29;

        public override bool Equals(object obj)
        {
            return obj is MethodCacheDetails methodCacheDetails &&
                   methodCacheDetails.ApiInterfaceType == ApiInterfaceType &&
                   methodCacheDetails.MethodInfo.Equals(MethodInfo);
        }
    }
}

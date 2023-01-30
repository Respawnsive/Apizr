using System;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Mapping;

namespace Apizr
{
    internal static class InternalExtensions
    {
        #region CacheHandler

        private static Func<ICacheHandler> _cacheHandlerFactory;

        internal static void SetCacheHandlerFactory(this IApizrGlobalCommonOptionsBuilderBase builder,
            Func<ICacheHandler> cacheHandlerFactory)
        {
            _cacheHandlerFactory = cacheHandlerFactory;
        }

        internal static Func<ICacheHandler> GetCacheHanderFactory(this IApizrCommonOptionsBase builder) =>
            _cacheHandlerFactory;

        #endregion

        #region MappingHandler

        #region Static

        private static Func<IMappingHandler> _mappingHandlerFactory;

        internal static void SetMappingHandlerFactory(this IApizrCommonOptionsBuilderBase builder,
            Func<IMappingHandler> mappingHandlerFactory)
        {
            _mappingHandlerFactory = mappingHandlerFactory;
        }

        internal static Func<IMappingHandler> GetMappingHanderFactory(this IApizrCommonOptionsBase builder) =>
            _mappingHandlerFactory;

        #endregion
        
        #region Extended

        private static Type _mappingHandlerType;

        internal static void SetMappingHandlerType<TMappingHandler>(this IApizrExtendedCommonOptionsBuilderBase builder) where TMappingHandler : IMappingHandler
        {
            _mappingHandlerType = typeof(TMappingHandler);
        }

        internal static Type GetMappingHanderType(this IApizrCommonOptionsBase builder) =>
            _mappingHandlerType;

        #endregion

        #endregion
    }
}

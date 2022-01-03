using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Apizr.Extensions.Microsoft.DependencyInjection"), 
           InternalsVisibleTo("Apizr.Integrations.Fusillade"),
           InternalsVisibleTo("Apizr.Integrations.Akavache"),
           InternalsVisibleTo("Apizr.Integrations.MonkeyCache"),
           InternalsVisibleTo("Apizr.Integrations.AutoMapper")]
namespace Apizr
{
    internal static class InternalExtensions
    {
        #region PrimaryHttpMessageHandler

        private static Func<DelegatingHandler, ILogger, IApizrOptionsBase, HttpMessageHandler> _primaryHandlerFactory;

        internal static void SetPrimaryHttpMessageHandler<T>(this T builder,
            Func<DelegatingHandler, ILogger, IApizrOptionsBase, HttpMessageHandler> primaryHandlerFactory)
            where T : IApizrSharedOptionsBuilderBase
        {
            _primaryHandlerFactory = primaryHandlerFactory;
        }

        internal static HttpMessageHandler GetPrimaryHttpMessageHandler(this ExtendedHttpHandlerBuilder httpHandlerBuilder,
            ILogger logger, IApizrOptionsBase options)
        {
            var innerHandler = httpHandlerBuilder.Build();
            return _primaryHandlerFactory?.Invoke(innerHandler, logger, options) ?? innerHandler;
        }

        #endregion

        #region CacheHandler

        private static Func<ICacheHandler> _cacheHandlerFactory;

        internal static void SetCacheHandlerFactory(this IApizrCommonOptionsBuilderBase builder,
            Func<ICacheHandler> cacheHandlerFactory)
        {
            _cacheHandlerFactory = cacheHandlerFactory;
        }

        internal static Func<ICacheHandler> GetCacheHanderFactory(this IApizrCommonOptionsBase builder) =>
            _cacheHandlerFactory;

        #endregion

        #region MappingHandler

        #region Factory

        private static Func<IMappingHandler> _mappingHandlerFactory;

        internal static void SetMappingHandlerFactory(this IApizrCommonOptionsBuilderBase builder,
            Func<IMappingHandler> mappingHandlerFactory)
        {
            _mappingHandlerFactory = mappingHandlerFactory;
        }

        internal static Func<IMappingHandler> GetMappingHanderFactory(this IApizrCommonOptionsBase builder) =>
            _mappingHandlerFactory;

        #endregion
        
        #region Type

        private static Type _mappingHandlerType;

        internal static void SetMappingHandlerType<TMappingHandler>(this IApizrCommonOptionsBuilderBase builder) where TMappingHandler : IMappingHandler
        {
            _mappingHandlerType = typeof(TMappingHandler);
        }

        internal static Type GetMappingHanderType(this IApizrCommonOptionsBase builder) =>
            _mappingHandlerType; 

        #endregion

        #endregion
    }
}

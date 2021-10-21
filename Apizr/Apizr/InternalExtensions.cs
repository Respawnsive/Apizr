using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Apizr.Extensions.Microsoft.DependencyInjection"), 
           InternalsVisibleTo("Apizr.Integrations.Fusillade"),
           InternalsVisibleTo("Apizr.Integrations.Akavache"),
           InternalsVisibleTo("Apizr.Integrations.MonkeyCache")]
namespace Apizr
{
    internal static class InternalExtensions
    {
        private static Func<DelegatingHandler, ILogger, HttpMessageHandler> _primaryHandlerFactory;
        internal static void SetPrimaryHttpMessageHandler(this IApizrSharedOptionsBuilderBase builder, Func<DelegatingHandler, ILogger, HttpMessageHandler> primaryHandlerFactory)
        {
            _primaryHandlerFactory = primaryHandlerFactory;
        }

        internal static HttpMessageHandler GetPrimaryHttpMessageHandler(this ExtendedHttpHandlerBuilder httpHandlerBuilder,
            ILogger logger)
        {
            var innerHandler = httpHandlerBuilder.Build();
            return _primaryHandlerFactory?.Invoke(innerHandler, logger) ?? innerHandler;
        }

        private static Func<ICacheHandler> _cacheHandlerFactory;

        internal static void SetCacheHandlerFactory(this IApizrCommonOptionsBuilderBase builder,
            Func<ICacheHandler> cacheHandlerFactory)
        {
            _cacheHandlerFactory = cacheHandlerFactory;
        }

        internal static Func<ICacheHandler> GetCacheHanderFactory(this IApizrCommonOptionsBase builder) =>
            _cacheHandlerFactory;
    }
}

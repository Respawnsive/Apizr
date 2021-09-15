using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Apizr.Extensions.Microsoft.DependencyInjection"), 
           InternalsVisibleTo("Apizr.Integrations.Fusillade")]
namespace Apizr
{
    internal static class InternalExtensions
    {
        private static Func<DelegatingHandler, ILogger, HttpMessageHandler> _primaryHandlerFactory;
        internal static void SetPrimaryHttpMessageHandler(this IApizrOptionsBuilderBase builder, Func<DelegatingHandler, ILogger, HttpMessageHandler> primaryHandlerFactory)
        {
            _primaryHandlerFactory = primaryHandlerFactory;
        }

        internal static HttpMessageHandler GetPrimaryHttpMessageHandler(this ExtendedHttpHandlerBuilder httpHandlerBuilder,
            ILogger logger)
        {
            var innerHandler = httpHandlerBuilder.Build();
            return _primaryHandlerFactory?.Invoke(innerHandler, logger) ?? innerHandler;
        }
    }
}

using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Apizr.Logging;
using HttpTracer;

[assembly: InternalsVisibleTo("Apizr.Extensions.Microsoft.DependencyInjection"), 
           InternalsVisibleTo("Apizr.Integrations.Fusillade")]
namespace Apizr
{
    internal static class InternalExtensions
    {
        private static Func<DelegatingHandler, ILogHandler, HttpMessageHandler> _primaryHandlerFactory;
        internal static void SetPrimaryHttpMessageHandler(this IApizrOptionsBuilderBase builder, Func<DelegatingHandler, ILogHandler, HttpMessageHandler> primaryHandlerFactory)
        {
            _primaryHandlerFactory = primaryHandlerFactory;
        }

        internal static HttpMessageHandler GetPrimaryHttpMessageHandler(this HttpHandlerBuilder httpHandlerBuilder,
            ILogHandler logHandler)
        {
            var innerHandler = httpHandlerBuilder.Build();
            return _primaryHandlerFactory?.Invoke(innerHandler, logHandler) ?? innerHandler;
        }
    }
}

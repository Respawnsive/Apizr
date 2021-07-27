using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpTracer;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr
{
    public interface IApizrOptionsBuilderBase
    {}

    public interface IApizrOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> : IApizrOptionsBuilderBase 
        where TApizrOptions : IApizrOptionsBase 
        where TApizrOptionsBuilder : IApizrOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
        /// <summary>
        /// Apizr options
        /// </summary>
        TApizrOptions ApizrOptions { get; }

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(string baseAddress);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(Uri baseAddress);

        /// <summary>
        /// Provide a method to refresh the authorization token when needed
        /// </summary>
        /// <param name="refreshTokenFactory">Refresh token method called when expired or empty</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandler">A delegating handler</param>
        /// <returns></returns>
        TApizrOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler);

        /// <summary>
        /// Configure your logging layer
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: All)</param>
        /// <param name="trafficLogLevel">Log level to apply while writing http traces (default: Information)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLogging(HttpMessageParts trafficVerbosity = HttpMessageParts.All, LogLevel trafficLogLevel = LogLevel.Information);

        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettings">A <see cref="RefitSettings"/> instance</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithRefitSettings(RefitSettings refitSettings);

        /// <summary>
        /// Provide a function to invoke while checking connectivity
        /// </summary>
        /// <param name="connectivityCheckingFunction">A function to invoke while checking connectivity</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithConnectivityHandler(Func<bool> connectivityCheckingFunction);

    }
}

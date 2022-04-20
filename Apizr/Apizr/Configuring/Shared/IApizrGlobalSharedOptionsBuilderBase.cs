using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    public interface IApizrGlobalSharedOptionsBuilderBase
    {
    }

    public interface IApizrGlobalSharedOptionsBuilderBase<out TApizrSharedOptions, out TApizrSharedOptionsBuilder> : IApizrGlobalSharedOptionsBuilderBase
        where TApizrSharedOptions : IApizrSharedOptionsBase
        where TApizrSharedOptionsBuilder : IApizrGlobalSharedOptionsBuilderBase<TApizrSharedOptions, TApizrSharedOptionsBuilder>
    {
        /// <summary>
        /// Apizr common options
        /// </summary>
        TApizrSharedOptions ApizrOptions { get; }

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithBaseAddress(string baseAddress);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithBaseAddress(Uri baseAddress);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandler">An <see cref="HttpClientHandler"/> instance</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler);

        /// <summary>
        /// Provide a method to refresh the authorization token when needed
        /// </summary>
        /// <param name="refreshTokenFactory">Refresh token method called when expired or empty</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Add a custom delegating handler
        /// </summary>
        /// <param name="delegatingHandler">A delegating handler</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler);

        /// <summary>
        /// Configure logging level for the api
        /// </summary>
        /// <param name="httpTracerMode"></param>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: All)</param>
        /// <param name="logLevels">Log levels to apply while writing (default: Information)</param>
        /// <returns></returns>
        TApizrSharedOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels);
    }
}

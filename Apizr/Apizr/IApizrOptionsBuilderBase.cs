using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Logging;
using HttpTracer;
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
        /// Define http traces and Apizr logs verbosity (could be defined with LogItAttribute)
        /// </summary>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity</param>
        /// <param name="apizrVerbosity">Apizr execution steps verbosity</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLoggingVerbosity(HttpMessageParts trafficVerbosity, ApizrLogLevel apizrVerbosity);

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
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettings">A <see cref="RefitSettings"/> instance</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithRefitSettings(RefitSettings refitSettings);

    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HttpTracer;
using Refit;

namespace Apizr
{
    public interface IApizrOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> 
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
        /// Define http tracer verbosity (could be defined with TraceAttribute)
        /// </summary>
        /// <param name="httpTracerVerbosity">Http tracer verbosity</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHttpTracing(HttpMessageParts httpTracerVerbosity);

        /// <summary>
        /// Enable Fusillade priority management (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="isPriorityManagementEnabled">Enable Fusillade priority management</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithPriorityManagement(bool isPriorityManagementEnabled);

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

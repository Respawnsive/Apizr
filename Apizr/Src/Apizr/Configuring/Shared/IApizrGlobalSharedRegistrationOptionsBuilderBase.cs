using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Builder options available at both common and proper level for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalSharedRegistrationOptionsBuilderBase : IApizrGlobalSharedOptionsBuilderBase
    {
    }

    /// <inheritdoc cref="IApizrGlobalSharedRegistrationOptionsBuilderBase" />
    public interface IApizrGlobalSharedRegistrationOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> : 
        IApizrGlobalSharedOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrGlobalSharedRegistrationOptionsBuilderBase
        where TApizrOptions : IApizrGlobalSharedRegistrationOptionsBase
        where TApizrOptionsBuilder : IApizrGlobalSharedRegistrationOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
        /// <summary>
        /// Set options from configuration
        /// </summary>
        /// <param name="configurationSection">The configuration to set options from</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithConfiguration(IConfigurationSection configurationSection);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(string baseAddress);

        internal TApizrOptionsBuilder WithBaseAddress(string baseAddress, ApizrDuplicateStrategy strategy);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(Uri baseAddress);

        /// <summary>
        /// Define your web api base path (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="basePath">Your web api base path</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBasePath(string basePath);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandler">An <see cref="HttpClientHandler"/> instance</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler);

        /// <summary>
        /// Provide a method to refresh the authorization token when needed
        /// </summary>
        /// <param name="refreshTokenFactory">Refresh token method called when expired or empty</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Add a custom delegating handler inheriting from <see cref="DelegatingHandler"/> (serial call)
        /// </summary>
        /// <param name="delegatingHandler">A delegating handler</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithDelegatingHandler<THandler>(THandler delegatingHandler,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler;

        /// <summary>
        /// Add a custom http message handler inheriting from <see cref="HttpMessageHandler"/> (last call)
        /// </summary>
        /// <param name="httpMessageHandler">A http message handler</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHttpMessageHandler<THandler>(THandler httpMessageHandler) where THandler : HttpMessageHandler;

        /// <summary>
        /// Add some headers to the request
        /// </summary>
        /// <param name="headers">Headers to add to the request</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <param name="mode">Set headers right the way or store it for further attribute key match use (default: Set)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHeaders(IList<string> headers,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set);
    }
}

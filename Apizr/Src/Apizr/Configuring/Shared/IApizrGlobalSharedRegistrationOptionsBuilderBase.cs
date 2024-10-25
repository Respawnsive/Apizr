using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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
        /// <param name="configuration">The configuration to set options from</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithConfiguration(IConfiguration configuration);

        /// <summary>
        /// Set options from a specific configuration section
        /// </summary>
        /// <param name="configurationSection">The configuration section to set options from</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithConfiguration(IConfigurationSection configurationSection);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(string baseAddress, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBaseAddress(Uri baseAddress, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Define your web api base path (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="basePath">Your web api base path</param>
        /// <param name="strategy">The duplicate strategy if there's any other already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithBasePath(string basePath, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Provide a custom HttpClientHandler
        /// </summary>
        /// <param name="httpClientHandler">An <see cref="HttpClientHandler"/> instance</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler);

        /// <summary>
        /// Provide methods to only get the authorization constant token when needed
        /// </summary>
        /// <param name="getTokenFactory">The method called to get local constant token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory);

        /// <summary>
        /// Provide methods to get and set the authorization token when needed
        /// </summary>
        /// <param name="getTokenFactory">The method called to get local token</param>
        /// <param name="setTokenFactory">The method called to set local token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task> setTokenFactory);

        /// <summary>
        /// Provide a method to refresh the authorization token when needed
        /// </summary>
        /// <param name="refreshTokenFactory">Refresh token method called when expired or empty</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, string, CancellationToken, Task<string>> refreshTokenFactory);

        /// <summary>
        /// Provide methods to get, set and refresh the authorization token when needed
        /// </summary>
        /// <param name="getTokenFactory">The method called to get local token</param>
        /// <param name="setTokenFactory">The method called to get local token</param>
        /// <param name="refreshTokenFactory">Refresh token method called when expired or empty</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task> setTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task<string>> refreshTokenFactory);

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

        /// <summary>
        /// Apply some resilience strategies by getting pipelines from registry with key matching.
        /// </summary>
        /// <param name="resiliencePipelineKeys">Resilience pipeline keys from the registry.</param>
        /// <param name="methodScope">Http or Crud methods to apply pipelines on (default: null = All)</param>
        /// <param name="duplicateStrategy">The duplicate strategy if there's any other names already (default: Add)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithResiliencePipelineKeys(string[] resiliencePipelineKeys, IEnumerable<ApizrRequestMethod> methodScope = null, ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add);

    }
}

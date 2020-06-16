using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public interface IApizrExtendedOptions : IApizrOptionsBase
    {
        /// <summary>
        /// Type of the manager
        /// </summary>
        Type ApizrManagerType { get; }

        /// <summary>
        /// Type of the connectivity handler
        /// </summary>
        Type ConnectivityHandlerType { get; }

        /// <summary>
        /// Type of the cache handler
        /// </summary>
        Type CacheHandlerType { get; }

        /// <summary>
        /// Type of the logging handler
        /// </summary>
        Type LogHandlerType { get; }

        /// <summary>
        /// Refit settings factory
        /// </summary>
        Func<IServiceProvider, RefitSettings> RefitSettingsFactory { get; }

        /// <summary>
        /// HttpClient builder
        /// </summary>
        Action<IHttpClientBuilder> HttpClientBuilder { get; }

        /// <summary>
        /// Delegating handlers factories
        /// </summary>
        IList<Func<IServiceProvider, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
    }
}

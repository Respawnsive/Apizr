using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Apizr.Requesting;
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
        /// Type of the mapping handler
        /// </summary>
        Type MappingHandlerType { get; }

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

        /// <summary>
        /// Entities auto registered with <see cref="IApizrManager{ICrudApi}"/>
        /// </summary>
        IDictionary<Type, CrudEntityAttribute> CrudEntities { get; }

        /// <summary>
        /// Web apis auto registered with <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        IDictionary<Type, WebApiAttribute> WebApis { get; }

        /// <summary>
        /// Post registration actions
        /// </summary>
        IList<Action<IServiceCollection>> PostRegistrationActions { get; }
    }
}

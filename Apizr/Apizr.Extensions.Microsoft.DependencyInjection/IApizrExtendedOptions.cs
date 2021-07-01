using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Connecting;
using Apizr.Mapping;
using Apizr.Requesting;
using HttpTracer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        /// Type of the mapping handler
        /// </summary>
        Type MappingHandlerType { get; }

        /// <summary>
        /// Base address factory
        /// </summary>
        Func<IServiceProvider, Uri> BaseAddressFactory { get; }

        /// <summary>
        /// Http traffic tracing verbosity factory
        /// </summary>
        Func<IServiceProvider, HttpMessageParts> TrafficVerbosityFactory { get; }

        /// <summary>
        /// Http traffic tracing log level factory
        /// </summary>
        Func<IServiceProvider, LogLevel> TrafficLogLevelFactory { get; }

        /// <summary>
        /// HttpClientHandler factory
        /// </summary>
        Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory { get; }

        /// <summary>
        /// Refit settings factory
        /// </summary>
        Func<IServiceProvider, RefitSettings> RefitSettingsFactory { get; }

        /// <summary>
        /// Connectivity handler factory
        /// </summary>
        Func<IServiceProvider, IConnectivityHandler> ConnectivityHandlerFactory { get; }

        /// <summary>
        /// HttpClient builder
        /// </summary>
        Action<IHttpClientBuilder> HttpClientBuilder { get; }

        /// <summary>
        /// Delegating handlers factories
        /// </summary>
        IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }

        /// <summary>
        /// Entities auto registered with <see cref="IApizrManager{ICrudApi}"/>
        /// </summary>
        IDictionary<Type, CrudEntityAttribute> CrudEntities { get; }

        /// <summary>
        /// Web apis auto registered with <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        IDictionary<Type, WebApiAttribute> WebApis { get; }

        /// <summary>
        /// Mappings between api request object and model object used for classic auto registration
        /// </summary>
        IDictionary<Type, MappedWithAttribute> ObjectMappings { get; }

        /// <summary>
        /// Post registration actions
        /// </summary>
        IList<Action<IServiceCollection>> PostRegistrationActions { get; }
    }
}

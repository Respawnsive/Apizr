using System;
using System.Collections.Generic;
using System.Reflection;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Manager;
using Apizr.Extending.Configuring.Registry;
using Apizr.Extending.Configuring.Shared;
using Apizr.Mapping;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Apizr.Extending.Configuring.Common
{
    /// <summary>
    /// Options available at common level for extended registrations
    /// </summary>
    public interface IApizrExtendedCommonOptions : IApizrExtendedCommonOptionsBase, IApizrExtendedSharedOptions
    {
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
        /// Refit settings factory
        /// </summary>
        Func<IServiceProvider, RefitSettings> RefitSettingsFactory { get; }

        /// <summary>
        /// Connectivity handler factory
        /// </summary>
        Func<IServiceProvider, IConnectivityHandler> ConnectivityHandlerFactory { get; }

        /// <summary>
        /// Cache handler factory
        /// </summary>
        Func<IServiceProvider, ICacheHandler> CacheHandlerFactory { get; }

        /// <summary>
        /// Mapping handler factory
        /// </summary>
        Func<IServiceProvider, IMappingHandler> MappingHandlerFactory { get; }

        /// <summary>
        /// Web apis auto registered with <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        IDictionary<Type, BaseAddressAttribute> WebApis { get; }

        /// <summary>
        /// Mappings between api request object and model object used for classic auto registration
        /// </summary>
        IDictionary<Assembly, MappedWithAttribute[]> ObjectMappings { get; }

        /// <summary>
        /// Other registries plugged during post registration actions
        /// </summary>
        IDictionary<Type, IApizrExtendedConcurrentRegistryBase> PostRegistries { get; }

        /// <summary>
        /// Post registration actions
        /// </summary>
        IList<Action<IApizrExtendedManagerOptions, IServiceCollection>> PostRegistrationActions { get; }
    }
}

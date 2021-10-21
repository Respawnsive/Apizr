using System;
using System.Collections.Generic;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Shared;
using Apizr.Mapping;
using Apizr.Requesting;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Apizr.Extending.Configuring.Common
{
    public interface IApizrExtendedCommonOptions : IApizrCommonOptionsBase, IApizrExtendedSharedOptions
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
        IList<Action<Type, IServiceCollection>> PostRegistrationActions { get; }
    }
}

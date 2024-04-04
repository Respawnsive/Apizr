using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Shared;

namespace Apizr.Extending.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level for extended registrations
    /// </summary>
    public interface IApizrExtendedSharedOptionsBase : IApizrGlobalSharedRegistrationOptionsBase
    {
        /// <summary>
        /// Http message handlers factories
        /// </summary>
        IDictionary<Type, Func<IServiceProvider, IApizrManagerOptionsBase, HttpMessageHandler>> HttpMessageHandlersExtendedFactories { get; }
    }
}

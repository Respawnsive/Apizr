using System;
using Apizr.Configuring.Manager;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level for both static and extended registrations
    /// </summary>
    public abstract class ApizrGlobalSharedRegistrationOptionsBase : ApizrGlobalSharedOptionsBase,
        IApizrGlobalSharedRegistrationOptionsBase
    {
        protected ApizrGlobalSharedRegistrationOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions = null) :
            base(sharedOptions)
        {
            BaseUri = sharedOptions?.BaseUri;
            BaseAddress = sharedOptions?.BaseAddress;
            BasePath = sharedOptions?.BasePath;
            PrimaryHandlerFactory = sharedOptions?.PrimaryHandlerFactory;
            HeadersFactories = sharedOptions?.HeadersFactories?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
            Headers = sharedOptions?.Headers?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
        }

        /// <inheritdoc />
        public Uri BaseUri { get; protected set; }

        /// <inheritdoc />
        public string BaseAddress { get; protected set; }

        /// <inheritdoc />
        public string BasePath { get; protected set; }

        /// <inheritdoc />
        public Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> PrimaryHandlerFactory { get; internal set; }

        /// <inheritdoc />
        public IDictionary<(ApizrRegistrationMode, ApizrLifetimeScope), Func<IList<string>>> HeadersFactories { get; internal set; }

        /// <inheritdoc />
        public IDictionary<ApizrRegistrationMode, IList<string>> Headers { get; internal set; }
    }
}
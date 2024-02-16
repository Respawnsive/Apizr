using System;
using Apizr.Configuring.Manager;
using Microsoft.Extensions.Logging;
using System.Net.Http;

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
        }

        /// <inheritdoc />
        public Uri BaseUri { get; protected set; }

        /// <inheritdoc />
        public string BaseAddress { get; protected set; }

        /// <inheritdoc />
        public string BasePath { get; protected set; }

        /// <inheritdoc />
        public Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> PrimaryHandlerFactory { get; internal set; }
    }
}
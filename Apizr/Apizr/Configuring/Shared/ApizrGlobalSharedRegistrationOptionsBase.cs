using System;
using Polly;

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
            ContextFactory = sharedOptions?.ContextFactory;
        }

        /// <inheritdoc />
        public Uri BaseUri { get; protected set; }

        /// <inheritdoc />
        public string BaseAddress { get; protected set; }

        /// <inheritdoc />
        public string BasePath { get; protected set; }

        /// <inheritdoc />
        public Func<Context> ContextFactory { get; internal set; }
    }
}
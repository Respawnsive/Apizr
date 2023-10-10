using System;
using System.Collections.Generic;
using System.Linq;
using Apizr.Configuring.Manager;
using Microsoft.Extensions.Logging;
using System.Net.Http;
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
            ContextFactories = new List<Func<Context>>();
            if(sharedOptions?.ContextFactory != null)
                ContextFactories.Add(sharedOptions.ContextFactory);
            PrimaryHandlerFactory = sharedOptions?.PrimaryHandlerFactory;
        }

        /// <inheritdoc />
        public Uri BaseUri { get; protected set; }

        /// <inheritdoc />
        public string BaseAddress { get; protected set; }

        /// <inheritdoc />
        public string BasePath { get; protected set; }

        internal IList<Func<Context>> ContextFactories { get; }
        private Func<Context> _contextFactory;

        /// <inheritdoc />
        public Func<Context> ContextFactory => _contextFactory ??= ContextFactories.Count > 0
            ? () => new Context(null,
                ContextFactories.Reverse()
                    .SelectMany(factory => factory.Invoke().ToList())
                    .GroupBy(kpv => kpv.Key)
                    .ToDictionary(x => x.Key, x => x.First().Value))
            : null;

        /// <inheritdoc />
        public Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> PrimaryHandlerFactory { get; internal set; }
    }
}
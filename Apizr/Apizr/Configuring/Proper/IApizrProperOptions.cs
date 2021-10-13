using System;
using System.Net.Http;
using Apizr.Caching;
using Apizr.Configuring.Shared;
using Polly.Registry;

namespace Apizr.Configuring.Proper
{
    public interface IApizrProperOptions : IApizrProperOptionsBase, IApizrSharedOptions
    {
        /// <summary>
        /// Base address factory
        /// </summary>
        Func<Uri> BaseAddressFactory { get; }

        /// <summary>
        /// Policy registry factory
        /// </summary>
        Func<IReadOnlyPolicyRegistry<string>> PolicyRegistryFactory { get; }
    }
}

using System;
using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    public interface IApizrProperOptions : IApizrProperOptionsBase, IApizrSharedOptions
    {
        /// <summary>
        /// Base address factory
        /// </summary>
        Func<Uri> BaseAddressFactory { get; }
    }
}

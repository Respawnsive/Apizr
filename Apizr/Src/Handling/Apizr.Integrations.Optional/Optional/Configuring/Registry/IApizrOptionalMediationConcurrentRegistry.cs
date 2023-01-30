using System;
using Apizr.Extending.Configuring.Registry;

namespace Apizr.Optional.Configuring.Registry
{
    /// <inheritdoc cref="IApizrOptionalMediationRegistry" />
    public interface IApizrOptionalMediationConcurrentRegistry : IApizrOptionalMediationRegistry, IApizrExtendedConcurrentRegistryBase
    {
    }
}

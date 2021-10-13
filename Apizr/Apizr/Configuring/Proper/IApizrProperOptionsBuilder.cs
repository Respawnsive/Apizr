using System;
using Apizr.Configuring.Shared;
using Polly.Registry;

namespace Apizr.Configuring.Proper
{
    public interface IApizrProperOptionsBuilder<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>,
        IApizrSharedOptionsBuilder<TApizrProperOptions, TApizrProperOptionsBuilder>
        where TApizrProperOptions : IApizrProperOptionsBase
        where TApizrProperOptionsBuilder : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory);

        /// <summary>
        /// Provide a policy registry
        /// </summary>
        /// <param name="policyRegistryFactory">A policy registry instance factory</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithPolicyRegistry(Func<IReadOnlyPolicyRegistry<string>> policyRegistryFactory);
    }

    public interface IApizrProperOptionsBuilder : IApizrProperOptionsBuilder<IApizrProperOptions, IApizrProperOptionsBuilder>
    { }
}

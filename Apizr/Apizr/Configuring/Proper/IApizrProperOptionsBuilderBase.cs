using System;
using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    public interface IApizrProperOptionsBuilderBase : IApizrSharedOptionsBuilderBase
    {
    }

    public interface IApizrProperOptionsBuilderBase<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrProperOptionsBuilderBase,
        IApizrSharedOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    where TApizrProperOptions : IApizrProperOptionsBase
    where TApizrProperOptionsBuilder : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithBaseAddress(string baseAddress);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddress">Your web api base address</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithBaseAddress(Uri baseAddress);
    }
}

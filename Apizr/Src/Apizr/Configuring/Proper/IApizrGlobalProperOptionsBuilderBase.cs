using System;
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalProperOptionsBuilderBase : IApizrGlobalSharedRegistrationOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalProperOptionsBuilderBase<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrGlobalProperOptionsBuilderBase,
        IApizrGlobalSharedRegistrationOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    where TApizrProperOptions : IApizrProperOptionsBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
        /// <summary>
        /// Configure options for specific requests
        /// </summary>
        /// <param name="requestName">The name of the request to configure</param>
        /// <param name="optionsBuilder">The configuration builder</param>
        /// <param name="duplicateStrategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithRequestOptions(string requestName,
            Action<IApizrRequestOptionsBuilder> optionsBuilder,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add);

        /// <summary>
        /// Configure options for specific requests
        /// </summary>
        /// <param name="requestNames">The name of the requests to configure</param>
        /// <param name="optionsBuilder">The configuration builder</param>
        /// <param name="duplicateStrategy">The duplicate strategy if there's any other already (default: Add)</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithRequestOptions(string[] requestNames,
            Action<IApizrRequestOptionsBuilder> optionsBuilder,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add);
    }
}

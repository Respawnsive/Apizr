using Apizr.Configuring.Proper;
using Apizr.Configuring.Registry;
using System;

namespace Apizr.Extending.Configuring.Registry
{
    internal interface IApizrInternalExtendedRegistryBuilder<out TApizrProperOptionsBuilder> : IApizrInternalGlobalRegistryBuilder
        where TApizrProperOptionsBuilder : IApizrExtendedProperOptionsBuilderBase
    {
        void AddWrappingManagerFor<TWebApi, TWrappingManagerService, TWrappingManagerImplementation>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where TWrappingManagerService : IApizrManager;
    }
}

using Apizr.Configuring.Proper;
using System;

namespace Apizr.Configuring.Registry
{
    internal interface IApizrInternalRegistryBuilder<out TApizrProperOptionsBuilder> : IApizrInternalGlobalRegistryBuilder
        where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    {
        void AddWrappingManagerFor<TWebApi, TWrappingManager>(
            Func<IApizrManager<TWebApi>, TWrappingManager> wrappingManagerFactory,
            Action<TApizrProperOptionsBuilder> optionsBuilder = null) where TWrappingManager : IApizrManager;
    }
}

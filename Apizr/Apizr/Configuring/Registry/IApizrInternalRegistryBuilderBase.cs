using Apizr.Configuring.Proper;
using System;

namespace Apizr.Configuring.Registry
{
    internal interface IApizrInternalRegistryBuilderBase<out TApizrProperOptionsBuilder>
        where TApizrProperOptionsBuilder : IApizrProperOptionsBuilderBase
    {
        void AddWrappedManagerFor<TWebApi, TWrappedManager>(
            Func<IApizrManager<TWebApi>, TWrappedManager> wrappedManagerFactory,
            Action<TApizrProperOptionsBuilder> optionsBuilder = null) where TWrappedManager : IApizrManager;
    }
}

using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Requesting;
using System;

namespace Apizr.Configuring.Registry
{
    public interface IApizrDownloadRegistryBuilder<out TApizrRegistry, out TApizrRegistryBuilder, out TApizrProperOptionsBuilder, out TApizrCommonOptionsBuilder>
        where TApizrRegistry : IApizrEnumerableRegistry
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
        where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
        where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    {
        IApizrDownloadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
            TApizrCommonOptionsBuilder> AddFor<TDownloadApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi;

        IApizrDownloadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
            TApizrCommonOptionsBuilder> AddFor<TDownloadApi, TDownloadParams>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi<TDownloadParams>;
    }
}

using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Requesting;
using System;

namespace Apizr.Configuring.Registry
{
    public interface IApizrTransferRegistryBuilder<out TApizrRegistry, out TApizrRegistryBuilder, out TApizrProperOptionsBuilder, out TApizrCommonOptionsBuilder>
        where TApizrRegistry : IApizrEnumerableRegistry
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
        where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
        where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    {
        IApizrTransferRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
            TApizrCommonOptionsBuilder> AddFor<TTransferApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where TTransferApi : ITransferApi;

        IApizrTransferRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
            TApizrCommonOptionsBuilder> AddFor<TTransferApi, TDownloadParams>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where TTransferApi : ITransferApi<TDownloadParams>;
    }
}

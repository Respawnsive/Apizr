using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Requesting;
using System;

namespace Apizr.Configuring.Registry
{
    public interface IApizrTransferRegistryBuilderBase<out TApizrTransferRegistryBuilder, out TApizrRegistryBuilder, out TApizrProperOptionsBuilder, out TApizrCommonOptionsBuilder>
        where TApizrTransferRegistryBuilder : IApizrTransferRegistryBuilderBase<TApizrTransferRegistryBuilder, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase
        where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
        where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    {
        TApizrTransferRegistryBuilder AddFor<TTransferApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where TTransferApi : ITransferApi;

        TApizrTransferRegistryBuilder AddFor<TTransferApi, TDownloadParams>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where TTransferApi : ITransferApi<TDownloadParams>;
    }
}

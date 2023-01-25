using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public class ApizrTransferRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrTransferRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistry : IApizrEnumerableRegistry
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder,
        TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
{
    private readonly IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder> _internalBuilder;

    public ApizrTransferRegistryBuilder(TApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder>;
    }

    /// <inheritdoc />
    public IApizrTransferRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
        TApizrCommonOptionsBuilder> AddFor<TTransferApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi
    {
        _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi>>(
            apizrManager => new ApizrTransferManager<TTransferApi>(new ApizrDownloadManager<TTransferApi>(apizrManager),
                new ApizrUploadManager<TTransferApi>(apizrManager)), optionsBuilder);

        return this;
    }

    /// <inheritdoc />
    public IApizrTransferRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
            TApizrCommonOptionsBuilder>
        AddFor<TTransferApi, TDownloadParams>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi<TDownloadParams>
    {
        _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi, TDownloadParams>>(
            apizrManager => new ApizrTransferManager<TTransferApi, TDownloadParams>(
                new ApizrDownloadManager<TTransferApi, TDownloadParams>(apizrManager),
                new ApizrUploadManager<TTransferApi>(apizrManager)), optionsBuilder);

        return this;
    }
}